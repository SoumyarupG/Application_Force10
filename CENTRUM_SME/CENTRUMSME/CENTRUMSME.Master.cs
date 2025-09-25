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

namespace CENTRUMSME
{
    public partial class CENTRUMSME : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["USFBSME"] != null)
            {
                if (Convert.ToString(Session["LoginCookies"]) != Request.Cookies["USFBSME"].Value)
                {
                    Response.Redirect("SsnExpr.aspx");
                }
            }
            Response.AppendHeader("Refresh", Convert.ToString(18000) + "; url=SsnExpr.aspx");// 5 hrs
            lblMsg.Text = "";
            CMarquee objMar = new CMarquee();
            DataTable dt = objMar.GetLastMarquee();
            if (dt.Rows.Count > 0)
            {
                lt1.Text = dt.Rows[0]["Marquee"].ToString();
            }
            else
            {
                lt1.Text = "CENTRUMSME";
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
                btnGST.Visible = true;
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
                btnGST.Visible = false;

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
                System.IO.File.GetLastWriteTime(Request.PhysicalApplicationPath.ToString() + "\\bin\\KUDOSBA.dll").ToString("dd/MMM/yyyy HH:mm:ss"));

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
                case "Acct":
                    acAct.SelectedIndex = vIndex;
                    LinkButton lBtn = (LinkButton)this.acAct.FindControl(lkId);
                    lBtn.Style.Add("background", "#95B5D6");
                    btnAct.Style.Add("background", "#3da1e0");
                    break;
                case "Mst":
                    acMst.SelectedIndex = vIndex;
                    LinkButton lBtn1 = (LinkButton)this.acMst.FindControl(lkId);
                    lBtn1.Style.Add("background", "#95B5D6");
                    btnMst.Style.Add("background", "#3da1e0");
                    break;
                case "Tran":
                    acTrn.SelectedIndex = vIndex;
                    LinkButton lBtn2 = (LinkButton)this.acTrn.FindControl(lkId);
                    lBtn2.Style.Add("background", "#95B5D6");
                    btnTrn.Style.Add("background", "#3da1e0");
                    break;
                case "Rept":
                    acRpt.SelectedIndex = vIndex;
                    LinkButton lBtn3 = (LinkButton)this.acRpt.FindControl(lkId);
                    lBtn3.Style.Add("background", "#95B5D6");
                    btnRpt.Style.Add("background", "#3da1e0");
                    break;
                case "Syst":
                    acSys.SelectedIndex = vIndex;
                    LinkButton lBtn4 = (LinkButton)this.acSys.FindControl(lkId);
                    lBtn4.Style.Add("background", "#95B5D6");
                    btnSys.Style.Add("background", "#3da1e0");
                    break;
                case "Utly":
                    Accordion1.SelectedIndex = vIndex;
                    LinkButton lBtn5 = (LinkButton)this.Accordion1.FindControl(lkId);
                    lBtn5.Style.Add("background", "#95B5D6");
                    btnUty.Style.Add("background", "#3da1e0");
                    break;
                case "Opr":
                    acOpr.SelectedIndex = vIndex;
                    LinkButton lBtn6 = (LinkButton)this.acOpr.FindControl(lkId);
                    lBtn6.Style.Add("background", "#95B5D6");
                    btnOprtn.Style.Add("background", "#3da1e0");
                    break;
                case "Leg":
                    acLegal.SelectedIndex = vIndex;
                    LinkButton lBtnLeg = (LinkButton)this.acLegal.FindControl(lkId);
                    lBtnLeg.Style.Add("background", "#95B5D6");
                    btnLegal.Style.Add("background", "#3da1e0");
                    break;
                case "LegAgreement":
                    acLegal.SelectedIndex = vIndex;
                    LinkButton lBtnLegAgr = (LinkButton)this.acLegalAgreemnt.FindControl(lkId);
                    lBtnLegAgr.Style.Add("background", "#95B5D6");
                    btnLegal.Style.Add("background", "#3da1e0");
                    break;
                case "ConRpt":
                    acConRpt.SelectedIndex = vIndex;
                    LinkButton lBtn8 = (LinkButton)this.acConRpt.FindControl(lkId);
                    lBtn8.Style.Add("background", "#95B5D6");
                    btnConRpt.Style.Add("background", "#3da1e0");
                    break;
                case "Bon":
                    AccBonFleet.SelectedIndex = vIndex;
                    LinkButton lBtn9 = (LinkButton)this.AccBonFleet.FindControl(lkId);
                    lBtn9.Style.Add("background", "#95B5D6");
                    btnBon.Style.Add("background", "#3da1e0");
                    break;
                case "GST":
                    acGST.SelectedIndex = vIndex;
                    LinkButton lBtn10 = (LinkButton)this.acGST.FindControl(lkId);
                    lBtn10.Style.Add("background", "#95B5D6");
                    btnGST.Style.Add("background", "#3da1e0");
                    break;
            }
        }
        private void MenuStat(string pMenu)
        {
            switch (pMenu)
            {
                case "Acct":
                    mnuAct.Visible = true;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuLegal.Visible = false;
                    mnuLegalAgreemnt.Visible = false;
                    mnuBonFleet.Visible = false;
                    mnuGST.Visible = false;
                    break;
                case "Mst":
                    mnuAct.Visible = false;
                    mnuMst.Visible = true;
                    mnuTrn.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuLegal.Visible = false;
                    mnuLegalAgreemnt.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuBonFleet.Visible = false;
                    mnuGST.Visible = false;
                    break;
                case "Tran":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = true;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuLegal.Visible = false;
                    mnuLegalAgreemnt.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuBonFleet.Visible = false;
                    mnuGST.Visible = false;
                    break;
                case "Rept":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuRpt.Visible = true;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuLegal.Visible = false;
                    mnuLegalAgreemnt.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuBonFleet.Visible = false;
                    mnuGST.Visible = false;
                    break;
                case "Syst":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = true;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuLegal.Visible = false;
                    mnuLegalAgreemnt.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuBonFleet.Visible = false;
                    mnuGST.Visible = false;
                    break;
                case "Utly":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = true;
                    mnuOpr.Visible = false;
                    mnuLegal.Visible = false;
                    mnuLegalAgreemnt.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuBonFleet.Visible = false;
                    mnuGST.Visible = false;
                    break;
                case "Opr":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = true;
                    mnuLegal.Visible = false;
                    mnuLegalAgreemnt.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuBonFleet.Visible = false;
                    mnuGST.Visible = false;
                    break;
                case "Leg":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuLegal.Visible = true;
                    mnuLegalAgreemnt.Visible = true;
                    mnuConRpt.Visible = false;
                    mnuBonFleet.Visible = false;
                    mnuGST.Visible = false;
                    break;
                case "LegAgreement":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuLegal.Visible = true;
                    mnuLegalAgreemnt.Visible = true;
                    mnuConRpt.Visible = false;
                    mnuBonFleet.Visible = false;
                    mnuGST.Visible = false;
                    break;
                case "Audit":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuLegal.Visible = false;
                    mnuLegalAgreemnt.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuBonFleet.Visible = false;
                    mnuGST.Visible = false;
                    break;
                case "ConRpt":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuLegal.Visible = false;
                    mnuLegalAgreemnt.Visible = false;
                    mnuConRpt.Visible = true;
                    mnuBonFleet.Visible = false;
                    mnuGST.Visible = false;
                    break;
                case "Mobile":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuLegal.Visible = false;
                    mnuLegalAgreemnt.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuBonFleet.Visible = false;
                    mnuGST.Visible = false;
                    break;
                case "Deft":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuLegal.Visible = false;
                    mnuLegalAgreemnt.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuBonFleet.Visible = false;
                    mnuGST.Visible = false;
                    break;
                case "Bon":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuLegal.Visible = false;
                    mnuLegalAgreemnt.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuBonFleet.Visible = true;
                    mnuGST.Visible = false;
                    break;
                case "GST":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuLegal.Visible = false;
                    mnuLegalAgreemnt.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuBonFleet.Visible = false;
                    mnuGST.Visible = true;
                    break;
            }
        }
        protected void btnAct_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Acct");
            btnAct.Style.Add("background", "#3da1e0");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnLegal.Style.Add("background", "none");
            //btnAudit.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnBon.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
            //btnMobile.Style.Add("background", "none");
        }
        protected void btnMst_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Mst");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "#3da1e0");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            //btnAudit.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnBon.Style.Add("background", "none");
            btnLegal.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
            //btnMobile.Style.Add("background", "none");
        }
        protected void btnTrn_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Tran");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "#3da1e0");
            btnSys.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnLegal.Style.Add("background", "none");
            //btnAudit.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnBon.Style.Add("background", "none");
            //btnMobile.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }
        protected void btnRpt_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Rept");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "#3da1e0");
            btnTrn.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnLegal.Style.Add("background", "none");
            //btnAudit.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnBon.Style.Add("background", "none");
            //btnMobile.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }
        protected void btnSys_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Syst");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnSys.Style.Add("background", "#3da1e0");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnLegal.Style.Add("background", "none");
            //btnAudit.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnBon.Style.Add("background", "none");
            //btnMobile.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }
        protected void btnUty_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Utly");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnUty.Style.Add("background", "#3da1e0");
            btnOprtn.Style.Add("background", "none");
            btnLegal.Style.Add("background", "none");
            //btnAudit.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnBon.Style.Add("background", "none");
            //btnMobile.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }
        protected void btnOprtn_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Opr");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "#3da1e0");
            btnUty.Style.Add("background", "none");
            btnLegal.Style.Add("background", "none");
            //btnAudit.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnBon.Style.Add("background", "none");
            //btnMobile.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }
        protected void btnLegal_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Leg");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnLegal.Style.Add("background", "#3da1e0");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            //btnAudit.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnBon.Style.Add("background", "none");
            //btnMobile.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }
        protected void btnAudit_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Audit");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnLegal.Style.Add("background", "none");
            //btnAudit.Style.Add("background", "#3da1e0");
            btnConRpt.Style.Add("background", "none");
            btnBon.Style.Add("background", "none");
            //btnMobile.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }
        protected void btnConRpt_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("ConRpt");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnLegal.Style.Add("background", "none");
            //btnAudit.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "#3da1e0");
            btnBon.Style.Add("background", "none");
            //btnMobile.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }
        protected void btnBon_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Bon");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnLegal.Style.Add("background", "none");
            //btnAudit.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnBon.Style.Add("background", "#3da1e0");
            btnLegal.Style.Add("background", "none");
            //btnMobile.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }

        protected void btnGST_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("GST");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnLegal.Style.Add("background", "none");
            //btnAudit.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnBon.Style.Add("background", "none");
            //btnMobile.Style.Add("background", "none");
            btnGST.Style.Add("background", "#3da1e0");
        }
        protected void btnMobile_Click(object sender, EventArgs e)
        {

            if (Session[gblValue.RoleId].ToString() == "1")
            {
                if (Session["MnuId"] == null) return;
                MenuStat("Mobile");
                btnAct.Style.Add("background", "none");
                btnMst.Style.Add("background", "none");
                btnRpt.Style.Add("background", "none");
                btnTrn.Style.Add("background", "none");
                btnSys.Style.Add("background", "none");
                btnOprtn.Style.Add("background", "none");
                btnUty.Style.Add("background", "none");
                btnLegal.Style.Add("background", "none");
                //btnAudit.Style.Add("background", "none");
                btnConRpt.Style.Add("background", "none");
                btnBon.Style.Add("background", "none");
                //btnMobile.Style.Add("background", "#3da1e0");
                btnGST.Style.Add("background", "none");
            }
            else
            {
                gblFuction.MsgPopup("Other than Super User Login Cannot use It.........");
                return;
            }
        }

        #region AccountMenu
        protected void lb1_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lb1";
            Response.Redirect("~/WebPages/Private/Master/AcGroup.aspx");
        }
        protected void lb2_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lb2";
            Response.Redirect("~/WebPages/Private/Master/AcSubGrp.aspx");
        }
        protected void lb3_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lb3";
            Response.Redirect("~/WebPages/Private/Master/GenLedDtl.aspx");
        }
        protected void lb4_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lb4";
            Response.Redirect("~/WebPages/Private/Master/AcctOpBal.aspx");
        }
        protected void lb5_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lb5";
            Response.Redirect("~/WebPages/Private/Transaction/VoucherRP.aspx");
        }
        protected void lb6_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lb6";
            Response.Redirect("~/WebPages/Private/Transaction/VoucherJ.aspx");
        }
        protected void lb7_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lb7";
            Response.Redirect("~/WebPages/Private/Transaction/VoucherC.aspx");
        }
        protected void lbSub_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lbSub";
            Response.Redirect("~/WebPages/Private/Master/SubsidiaryLdgr.aspx");
        }
        protected void lbBR_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lbBR";
            Response.Redirect("~/WebPages/Private/Transaction/BankRecon.aspx");
        }
        protected void lbCB_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lbCB";
            Response.Redirect("~/WebPages/Private/Transaction/ChqBounce.aspx");
        }
        protected void lbBankers_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Acct";
                Session["PaneId"] = acAct.SelectedIndex;
                Session["LinkId"] = "lbBankers";
                Response.Redirect("~/WebPages/Private/Master/BankerMst.aspx");
            }
            else
            {
                gblFuction.MsgPopup("Branch Login can not Access this Option.");
                return;
            }
        }
        protected void lbBankersLn_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Acct";
                Session["PaneId"] = acAct.SelectedIndex;
                Session["LinkId"] = "lbBankersLn";
                Response.Redirect("~/WebPages/Private/Transaction/BankersLoan.aspx");
            }
            else
            {
                gblFuction.MsgPopup("Branch Login can not Access this Option.");
                return;
            }
        }
        protected void lbBankRepaySchedule_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Acct";
                Session["PaneId"] = acAct.SelectedIndex;
                Session["LinkId"] = "lbBankRepaySchedule";
                Response.Redirect("~/WebPages/Private/Report/BankRepaySchedule.aspx");
            }
            else
            {
                gblFuction.MsgPopup("Branch Login can not Access this Option.");
                return;
            }
        }
        protected void lbMonthlyRepaySchedule_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Acct";
                Session["PaneId"] = acAct.SelectedIndex;
                Session["LinkId"] = "lbMonthlyRepaySchedule";
                Response.Redirect("~/WebPages/Private/Report/MonthlyRepaySchedule.aspx");
            }
            else
            {
                gblFuction.MsgPopup("Branch Login can not Access this Option.");
                return;
            }
        }
        protected void lbBankerLoanStatus_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Acct";
                Session["PaneId"] = acAct.SelectedIndex;
                Session["LinkId"] = "lbBankerLoanStatus";
                Response.Redirect("~/WebPages/Private/Report/BankerLoanStatus.aspx");
            }
            else
            {
                gblFuction.MsgPopup("Branch Login can not Access this Option.");
                return;
            }
        }
        protected void lbBnkrLnRecovery_Click(object sender, EventArgs e)
        {

            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Acct";
                Session["PaneId"] = acAct.SelectedIndex;
                Session["LinkId"] = "lbBnkrLnRecovery";
                Response.Redirect("~/WebPages/Private/Transaction/BnkrLoanRecovry.aspx");
            }
            else
            {
                gblFuction.MsgPopup("Branch Login can not Access this Option.");
                return;
            }

        }
        #endregion

        #region MasterMenu
        protected void lb11_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lb11";
            Response.Redirect("~/WebPages/Private/Master/BranchMst.aspx");
        }
        protected void lb12_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lb12";
            Response.Redirect("~/WebPages/Private/Master/DistrictMaster.aspx");
        }
        protected void lblDivision_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lblDivision";
            Response.Redirect("~/WebPages/Private/Master/DivisionMst.aspx");
        }
        protected void lblLocalArea_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lblLocalArea";
            Response.Redirect("~/WebPages/Private/Master/LoaclAreaMst.aspx");
        }
        protected void lb13_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lb13";
            Response.Redirect("~/WebPages/Private/Master/BlockMaster.aspx");
        }
        protected void lb14_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lb14";
            Response.Redirect("~/WebPages/Private/Master/TehsilMaster.aspx");
        }
        protected void lb15_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lb15";
            Response.Redirect("~/WebPages/Private/Master/VillageMaster.aspx");
        }
        protected void lbClustermst_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbClustermst";
            Response.Redirect("~/WebPages/Private/Master/ClusterMaster.aspx");
        }
        protected void lbLnPur_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbLnPur";
            Response.Redirect("~/WebPages/Private/Master/PurposeMaster.aspx");
        }
        protected void lbGenParameter_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbGenParameter";
            Response.Redirect("~/WebPages/Private/Master/GenralParameter.aspx");
        }
        protected void lbAppSource_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbAppSource";
            Response.Redirect("~/WebPages/Private/Master/LoanApplicationSource.aspx");
        }
        protected void lbFnd_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbFnd";
            Response.Redirect("~/WebPages/Private/Master/FundSourceMaster.aspx");
        }
        protected void lbLnProd_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbLnProd";
            Response.Redirect("~/WebPages/Private/Master/LoanProductMaster.aspx");
        }
        protected void lbLnParam_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbLnParam";
            Response.Redirect("~/WebPages/Private/Master/LoanParameter.aspx");
        }
        protected void lbLevelRange_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbLevelRange";
            Response.Redirect("~/WebPages/Private/Master/LevelRangeMst.aspx");
        }
        protected void lbZone_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbZone";
            Response.Redirect("~/WebPages/Private/Master/ZoneMaster.aspx");
        }
        protected void lbState_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbState";
            Response.Redirect("~/WebPages/Private/Master/StateMaster.aspx");
        }
        protected void lbReg_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbReg";
            Response.Redirect("~/WebPages/Private/Master/RegionMaster.aspx");
        }
        protected void lbMo_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbMo";
            Response.Redirect("~/WebPages/Private/Master/MohallaMaster.aspx");
        }
        protected void lbInsCmp_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbInsCmp";
            Response.Redirect("~/WebPages/Private/Master/ICMst.aspx");
        }
        protected void lbCrBu_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbCrBu";
            Response.Redirect("~/WebPages/Private/Master/CreditBureauMaster.aspx");
        }
        protected void lbCnclRes_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbCnclRes";
            Response.Redirect("~/WebPages/Private/Master/CancellationReasonMaster.aspx");
        }
        protected void lbLnLoss_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbLnLoss";
            Response.Redirect("~/WebPages/Private/Master/LoanLossProvision.aspx");
        }
        protected void lbLnScm_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbLnScm";
            Response.Redirect("~/WebPages/Private/Master/LoanScheme.aspx");
        }
        protected void lbTrgt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbTrgt";
            Response.Redirect("~/WebPages/Private/Master/TargetSetup.aspx");
        }
        protected void lbMarQ_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbMarQ";
            Response.Redirect("~/WebPages/Private/Master/MarqueeMaster.aspx");
        }
        protected void lbIncomeItemMst_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbIncomeItemMst";
            Response.Redirect("~/WebPages/Private/Master/IncomeItemMaster.aspx");
        }
        protected void lbOccSubTypeMst_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbOccSubTypeMst";
            Response.Redirect("~/WebPages/Private/Master/OccupationSubTypeMst.aspx");
        }
        protected void lblExpenseItemMst_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lblExpenseItemMst";
            Response.Redirect("~/WebPages/Private/Master/ExpenseItemMaster.aspx");
        }
        protected void lblnServTx_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lblnServTx";
            Response.Redirect("~/WebPages/Private/Master/ServiceTaxMst.aspx");
        }
        protected void lbInsSchme_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbInsSchme";
            Response.Redirect("~/WebPages/Private/Master/InsuranceScheme.aspx");
        }
        protected void lbasmblr_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbasmblr";
            Response.Redirect("~/WebPages/Private/Master/Assembler.aspx");
        }
        protected void lbOccup_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbOccup";
            Response.Redirect("~/WebPages/Private/Master/OccupationMaster.aspx");
        }
        protected void lbDesignation_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbDesignation";
            Response.Redirect("~/WebPages/Private/Master/DesignationMaster.aspx");
        }
        #endregion

        #region Operation
        protected void lbEmp_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Branch Office can not do this operation...");
                return;
            }            
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbEmp";
            Response.Redirect("~/WebPages/Private/Master/ROMst.aspx");
        }
        protected void lbEmpAloc_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Branch Office can not do this operation...");
                return;
            } 
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbEmpAloc";
            Response.Redirect("~/WebPages/Private/Master/RoTransfer.aspx");
        }
        protected void lbMsurv_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbMsurv";
            Response.Redirect("~/WebPages/Private/Master/MohallaSurvey.aspx");
        }
        protected void lbCenter_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbCenter";
            Response.Redirect("~/WebPages/Private/Master/CentMst.aspx");
        }
        protected void lbCgt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbCgt";
            Response.Redirect("~/WebPages/Private/Master/CGTMaster.aspx");
        }
        protected void lbCrBAp_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbCrBAp";
            Response.Redirect("~/WebPages/Private/Master/HighmarkApprovalMaster.aspx");
        }
        protected void lbGrFrm_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbGrFrm";
            Response.Redirect("~/WebPages/Private/Master/GrMst.aspx");
        }
        protected void lbCustAloc_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbCustAloc";
            Response.Redirect("~/WebPages/Private/Master/CustomerAllocation.aspx");
        }
        protected void lbCAMBucket_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbCAMBucket";
            Response.Redirect("~/WebPages/Private/Master/CAMBucket.aspx");
        }
        protected void lbPDBucket_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbPDBucket";
            Response.Redirect("~/WebPages/Private/Master/PDBucket.aspx");
        }

        protected void lbLnAppBucket_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbLnAppBucket";
            Response.Redirect("~/WebPages/Private/Master/LoanAppBucket.aspx");
        }

        protected void lbDocUpload_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbDocUpload";
            Response.Redirect("~/WebPages/Private/Master/DocumentUpload.aspx");
        }

        protected void lbDigiCon_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbDigiCon";
            Response.Redirect("~/WebPages/Private/Report/DigitalConsentForm.aspx");
        }

        protected void lbBrLegProc_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.MsgPopup("Login to Branch For Branch Legal Process");
                return;
            }  
            Session["MnuId"] = "Leg";
            Session["PaneId"] = acLegal.SelectedIndex;
            Session["LinkId"] = "lbBrLegProc";
            Response.Redirect("~/WebPages/Private/Legal/BranchLegal.aspx");
        }
        protected void lbBrResAgQuery_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() == "0000")
            //{
            //    gblFuction.MsgPopup("Login to Branch For Branch Legal Process");
            //    return;
            //}
            Session["MnuId"] = "Leg";
            Session["PaneId"] = acLegal.SelectedIndex;
            Session["LinkId"] = "lbBrResAgQuery";
            Response.Redirect("~/WebPages/Private/Legal/BranchQueryResponse.aspx");
        }
        protected void lbResolveAgQuery_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() == "0000")
            //{
            //    gblFuction.MsgPopup("Login to Branch For Branch Legal Process");
            //    return;
            //}
            Session["MnuId"] = "Leg";
            Session["PaneId"] = acLegal.SelectedIndex;
            Session["LinkId"] = "lbResolveAgQuery";
            Response.Redirect("~/WebPages/Private/Legal/LegalResolveQuery.aspx");
        }
        protected void lbHOLegApp_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Login to Head Office For Approval");
            //    return;
            //}
            Session["MnuId"] = "Leg";
            Session["PaneId"] = acLegal.SelectedIndex;
            Session["LinkId"] = "lbHOLegApp";
            Response.Redirect("~/WebPages/Private/Legal/HOLegal.aspx");
        }
        protected void lbPreLegalOpinon_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Login to Head Office For Approval");
            //    return;
            //}
            Session["MnuId"] = "Leg";
            Session["PaneId"] = acLegal.SelectedIndex;
            Session["LinkId"] = "lbPreLegalOpinon";
            Response.Redirect("~/WebPages/Private/Legal/PreOpinion.aspx");
        }
        protected void lbFinalLegalOpinion_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Login to Head Office For Approval");
            //    return;
            //}
            Session["MnuId"] = "Leg";
            Session["PaneId"] = acLegal.SelectedIndex;
            Session["LinkId"] = "lbFinalLegalOpinion";
            Response.Redirect("~/WebPages/Private/Legal/FinalLegalOpinion.aspx");
        }
        protected void lbLegMODTD_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Login to Head Office For Approval");
            //    return;
            //}
            Session["MnuId"] = "Leg";
            Session["PaneId"] = acLegal.SelectedIndex;
            Session["LinkId"] = "lbLegMODTD";
            Response.Redirect("~/WebPages/Private/Legal/LegalMODTD.aspx");
        }
        protected void lbLegDocReceive_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Login to Head Office For Approval");
            //    return;
            //}
            Session["MnuId"] = "Leg";
            Session["PaneId"] = acLegal.SelectedIndex;
            Session["LinkId"] = "lbLegDocReceive";
            Response.Redirect("~/WebPages/Private/Legal/LegalDocReceived.aspx");
        }
        protected void lbLegOrgDocSend_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Login to Head Office For Approval");
            //    return;
            //}
            Session["MnuId"] = "Leg";
            Session["PaneId"] = acLegal.SelectedIndex;
            Session["LinkId"] = "lbLegOrgDocSend";
            Response.Redirect("~/WebPages/Private/Legal/LegalDocSendFromBranch.aspx");
        }
        protected void lbLegOrgDocRecByHO_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Please Login to Head Office to Receive Original Documents");
                return;
            }
            Session["MnuId"] = "Leg";
            Session["PaneId"] = acLegal.SelectedIndex;
            Session["LinkId"] = "lbLegOrgDocRecByHO";
            Response.Redirect("~/WebPages/Private/Legal/LegalDocRecByHO.aspx");
        }
        protected void lbLOD_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Please Login to Head Office For LOD Verification");
            //    return;
            //}
            Session["MnuId"] = "Leg";
            Session["PaneId"] = acLegal.SelectedIndex;
            Session["LinkId"] = "lbLOD";
            Response.Redirect("~/WebPages/Private/Legal/LegalLOD.aspx");
        }
        protected void lbDocSendToCustody_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Please Login to Head Office For LOD Verification");
            //    return;
            //}
            Session["MnuId"] = "Leg";
            Session["PaneId"] = acLegal.SelectedIndex;
            Session["LinkId"] = "lbDocSendToCustody";
            Response.Redirect("~/WebPages/Private/Legal/DocSendToCustody.aspx");
        }
        protected void lbPreCloseDocRecByHO_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Please Login to Head Office For LOD Verification");
            //    return;
            //}
            Session["MnuId"] = "Leg";
            Session["PaneId"] = acLegal.SelectedIndex;
            Session["LinkId"] = "lbPreCloseDocRecByHO";
            Response.Redirect("~/WebPages/Private/Legal/PreCLoseDocRecByHO.aspx");
        }
        protected void lbPreCloseDocHandOver_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Please Login to Head Office For LOD Verification");
            //    return;
            //}
            Session["MnuId"] = "Leg";
            Session["PaneId"] = acLegal.SelectedIndex;
            Session["LinkId"] = "lbPreCloseDocHandOver";
            Response.Redirect("~/WebPages/Private/Legal/PreCLoseDocHandOver.aspx");
        }
        protected void lbLOA_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Login to Head Office For Approval");
            //    return;
            //}
            Session["MnuId"] = "Leg";
            Session["PaneId"] = acLegal.SelectedIndex;
            Session["LinkId"] = "lbLOA";
            Response.Redirect("~/WebPages/Private/Legal/LegalLOA.aspx");
        }
        protected void lbLegalPrint_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() == "0000")
            //{
            //    gblFuction.MsgPopup("Login to Branch For Branch Legal Process");
            //    return;
            //}
            Session["MnuId"] = "LegAgreement";
            Session["PaneId"] = acLegalAgreemnt.SelectedIndex;
            Session["LinkId"] = "lbLegalPrint";
            Response.Redirect("~/WebPages/Private/Legal/LegalDocPrint.aspx");
        }
        protected void lbLeadGen_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbLeadGen";
            Response.Redirect("~/WebPages/Private/Master/LeadGeneration.aspx");
        }
        protected void lbPreCAMBucket_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbPreCAMBucket";
            Response.Redirect("~/WebPages/Private/Master/PreCAMBucket.aspx");
        }

        protected void lbOverride_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Login to Head Office For Override");
                return;
            }
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbOverride";
            Response.Redirect("~/WebPages/Private/Transaction/Override.aspx");
        }

        protected void lbPDC_Click(object sender, EventArgs e)
        {            
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbPDC";
            Response.Redirect("~/WebPages/Private/Transaction/PDC.aspx");
        }

        protected void lbSanction_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Login to Head Office For Loan Sanction");
            //    return;
            //}  
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbSanction";
            Response.Redirect("~/WebPages/Private/Transaction/LoanSanction.aspx");
        }
        protected void lbFSanction_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Login to Head Office For Final Loan Sanction");
                return;
            }  
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbFSanction";
            Response.Redirect("~/WebPages/Private/Transaction/LoanFinalSanction.aspx");
        }
        protected void lbFSancNotDisb_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Login to Head Office For Final Loan Sanction List");
                return;
            }
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbFSancNotDisb";
            Response.Redirect("~/WebPages/Private/Transaction/LoanFinalSancNotDisb.aspx");
        }
        protected void lblAgreement_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Login to Head Office For Agreement ");
            //    return;
            //} 
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lblAgreement";
            Response.Redirect("~/WebPages/Private/Agreement/AgreementMaster.aspx");
           // Response.Redirect("~/Agreements/MasterFacility.aspx");
        }        
        protected void lblDocsDownload_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Login to Head Office For Document Download");
            //    return;
            //}
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lblDocsDownload";
            Response.Redirect("~/WebPages/Private/Transaction/DocsDownload.aspx");
        }
        protected void lbOffVer_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbOffVer";
            Response.Redirect("~/WebPages/Private/Verification/OffVerification.aspx");
        }
        protected void lbResVer_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbResVer";
            Response.Redirect("~/WebPages/Private/Verification/ResidenceVerification.aspx");
        }
        protected void lblTrnfrCBChck_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.MsgPopup("Head Office can not do this operation...");
                return;
            }
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lblTrnfrCBChck";
            Response.Redirect("~/WebPages/Private/Master/TransferToCBChecking.aspx");
        }
        protected void lbKYC_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.MsgPopup("Head Office can not do this operation...");
                return;
            }
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbKYC";
            Response.Redirect("~/WebPages/Private/Master/HighMarkDataNew.aspx");
        }
        protected void lbBulkHighMEntry_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbBulkHighMEntry";
            Response.Redirect("~/WebPages/Private/Master/BulkHighMarkEntry.aspx");
        }
        protected void lbHighMAppr_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Branch Office can not do this operation...");
                return;
            }
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbHighMAppr";
            Response.Redirect("~/WebPages/Private/Master/ApproveHighMark.aspx");
        }
        protected void lbHighMApprBulk_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Branch Office can not do this operation...");
                return;
            }
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbHighMApprBulk";
            Response.Redirect("~/WebPages/Private/Master/HighmarkApprovalMaster.aspx");
        }
        protected void lbLnBulkSancXml_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbLnBulkSancXml";
            Response.Redirect("~/WebPages/Private/Master/BulkLoanSanctionXml.aspx");
        }
        protected void lbCBCgt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbCBCgt";
            Response.Redirect("~/WebPages/Private/Master/CGT.aspx");
        }
        protected void lbColRu_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbColRu";
            Response.Redirect("~/WebPages/Private/Master/ColRoutine.aspx");
        }
        protected void lbMDS_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbMDS";
            Response.Redirect("~/WebPages/Private/Master/YBMasterData.aspx");
        }
        #endregion

        #region TransactionMenu
        protected void lb24_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lb24";
            Response.Redirect("~/WebPages/Private/Transaction/LoanAppl.aspx");
        }
        protected void lbLABlk_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbLABlk";
            Response.Redirect("~/WebPages/Private/Transaction/LoanAppBulk.aspx");
        }
        protected void lbGrt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbGrt";
            Response.Redirect("~/WebPages/Private/Transaction/HouseVisit.aspx");
        }
        protected void lbTelPreDisb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbTelPreDisb";
            Response.Redirect("~/WebPages/Private/Transaction/TelePreDisb.aspx");
        }
        protected void lblBLC_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lblBLC";
            Response.Redirect("~/WebPages/Private/Transaction/LoanApprisal.aspx");
        }
        protected void lbChqPrint_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbChqPrint";
            Response.Redirect("~/WebPages/Private/Transaction/ChequePrint.aspx");
        }
        protected void lb25_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lb25";
            Response.Redirect("~/WebPages/Private/Transaction/LoanSancn.aspx");
        }
        protected void lb26_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Login to Head Office For Loan Disbursement ");
                return;
            } 
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lb26";
            Response.Redirect("~/WebPages/Private/Transaction/LoanDisbGrp.aspx");
        }

        protected void lbLoanDisbCancel_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Login to Head Office For Loan Disbursement ");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbLoanDisbCancel";
            Response.Redirect("~/WebPages/Private/Transaction/CancelDisbursement.aspx");
        }

        protected void lbJocataStatus_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Login to Head Office For Loan Disbursement ");
            //    return;
            //}
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbJocataStatus";
            Response.Redirect("~/WebPages/Private/Transaction/Jocata.aspx");
        }

        protected void lbTransDisb_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Login to Head Office For Tranche Disbursement ");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbTransDisb";
            Response.Redirect("~/WebPages/Private/Transaction/TransDisburse.aspx");
        }
        protected void lb27_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lb26";
            Response.Redirect("~/WebPages/Private/Transaction/LoanRecovry.aspx");
        }
        protected void lbLDB_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbLDB";
            Response.Redirect("~/WebPages/Private/Transaction/LoanDisBulk.aspx");
        }
        protected void lbDisbIn_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbDisbIn";
            Response.Redirect("~/WebPages/Private/Transaction/LoanDisbIndiv.aspx");
        }
        protected void lbRecovery_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbRecovery";
            Response.Redirect("~/WebPages/Private/Transaction/CollectionUpload.aspx");
        }
        protected void lbRecoveryInd_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbRecoveryInd";
            Response.Redirect("~/WebPages/Private/Transaction/LoanRecovry.aspx");
        }
        protected void lbOtherCollection_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbOtherCollection";
            Response.Redirect("~/WebPages/Private/Transaction/OtherCollection.aspx");
        }

        protected void lbOtherCollectionBulk_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbOtherCollectionBulk";
            Response.Redirect("~/WebPages/Private/Transaction/OtherCollectionBulk.aspx");
        }

        protected void lbPreMatchColl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbPreMatchColl";
            Response.Redirect("~/WebPages/Private/Transaction/PreMatColl.aspx");
        }

        protected void lbLnRecoveryBulk_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbLnRecoveryBulk";
            Response.Redirect("~/WebPages/Private/Transaction/LoanRecoveryBulk.aspx");
        }
        protected void lbEditFundSource_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbEditFundSource";
            Response.Redirect("~/WebPages/Private/Transaction/EditFundSource.aspx");
        }
        protected void lbSfBulkUpload_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
                return;
            }
            else
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbEditFundSource";
                Response.Redirect("~/WebPages/Private/Transaction/FundSourceUpload.aspx");
            }
        }

        protected void lbSfBulkUploadAppr_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
                return;
            }
            else
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbSfBulkUploadAppr";
                Response.Redirect("~/WebPages/Private/Transaction/FundSourceUploadApproval.aspx");
            }
        }
        protected void lbChequeBounce_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbChequeBounce";
            Response.Redirect("~/WebPages/Private/Transaction/ChequeBounce.aspx");
        }
        protected void lbChequeBounceBulk_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbChequeBounceBulk";
            Response.Redirect("~/WebPages/Private/Transaction/CollectionBounceBulk.aspx");
        }
        protected void lbLnRecDelBulk_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbLnRecDelBulk";
            Response.Redirect("~/WebPages/Private/Transaction/DeleteRecoveryBulk.aspx");
        }
        protected void lbLnReschedule_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbLnReschedule";
            Response.Redirect("~/WebPages/Private/Transaction/LoanReschedule.aspx");
        }
        protected void lbLnReStr_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbLnReStr";
            Response.Redirect("~/WebPages/Private/Transaction/LoanReStructure.aspx");
        }
        protected void lbUpLendDetail_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbUpLendDetail";
            Response.Redirect("~/WebPages/Private/Transaction/BulkLendersEntry.aspx");
            // Response.Redirect("~/Agreements/MasterFacility.aspx");
        }
        protected void lbAccruedInt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbAccruedInt";
            Response.Redirect("~/WebPages/Private/Transaction/Accrued.aspx");
        }
        protected void lbLendAccruedInt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbLendAccruedInt";
            Response.Redirect("~/WebPages/Private/Transaction/AccruedLender.aspx");
        }
        protected void lbNOCCertificate_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Login to Head Office For NOC Certificate Generation ");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbNOCCertificate";
            Response.Redirect("~/WebPages/Private/Transaction/NocCertificate.aspx");
        }

        protected void lbPDD_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbPDD";
            Response.Redirect("~/WebPages/Private/Transaction/DeathFlaging.aspx");
        }
        protected void lbClaimStatementDischLet_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can only be operated from Branch");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbClaimStatementDischLet";
            Response.Redirect("~/WebPages/Private/Transaction/ClaimeStatementAndDeathRecipt.aspx");
        }
        protected void lbDownCertificate_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can only be operated from HO");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbDownCertificate";
            Response.Redirect("~/WebPages/Private/Transaction/HODwnAllCertificate.aspx");
        }
        protected void lbDthDocCancl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbDthDocCancl";
            Response.Redirect("~/WebPages/Private/Transaction/DeathCancelAfterVerification.aspx", false);
        }
        protected void lbDD_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbDD";
            Response.Redirect("~/WebPages/Private/Transaction/DeathDeclare.aspx");
        }

        protected void lbBulkDeathDeclare_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can only be operated from HO");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbBulkDeathDeclare";
            Response.Redirect("~/WebPages/Private/Transaction/DemiseCloseBulk.aspx");
        }

        protected void LbLoanRecoveryAdj_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "LbLoanRecoveryAdj";
            Response.Redirect("~/WebPages/Private/Transaction/LoanRecovaryAdjastment.aspx");
        }

        protected void lbTransfer_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbTransfer";
            Response.Redirect("~/WebPages/Private/Admin/TransferMember.aspx");
        }

        protected void lbRiskCatChange_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbRiskCatChange";
            Response.Redirect("~/WebPages/Private/Transaction/RiskCatChng.aspx");
        }
        protected void lbFtchAadhDet_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("This Module Can only be operated from Head Office");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbFtchAadhDet";
            Response.Redirect("~/WebPages/Private/Transaction/FetchAadhaarDetails.aspx", false);
        }

        protected void lbCollDelRev_Click(object sender, EventArgs e)
        {
            
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbCollDelRev";
            Response.Redirect("~/WebPages/Private/Transaction/CollectionReverseDelete.aspx", false);
        }

        protected void lbLoanUtilization_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Login to Head Office For NOC Certificate Generation ");
            //    return;
            //}
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbLoanUtilization";
            Response.Redirect("~/WebPages/Private/Transaction/LoanUtilCheck.aspx");
        }

        protected void lbWriteOffDeclare_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbWriteOffDeclare";
            Response.Redirect("~/WebPages/Private/Transaction/WriteOffDeclare.aspx");
        }
        protected void lbWOffRecovery_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbWOffRecovery";
            Response.Redirect("~/WebPages/Private/Transaction/WriteOffRecovery.aspx");
        }
        protected void lbLoanUtil_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbLoanUtil";
            Response.Redirect("~/WebPages/Private/Transaction/LoanUtilisation.aspx");
        }
        protected void lb28_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lb28";
            Response.Redirect("~/WebPages/Private/Transaction/HHSurvey.aspx");
        }
        protected void lb29_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lb29";
            Response.Redirect("~/WebPages/Private/Transaction/ShgGrade.aspx");
        }
        protected void lb30_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Branch can not do this operation...");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lb30";
            Response.Redirect("~/WebPages/Private/Transaction/Woffdecl.aspx");
        }
        protected void lbWofCol_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Branch can not do this operation...");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbWofCol";
            Response.Redirect("~/WebPages/Private/Transaction/WOffColl.aspx");
        }
        protected void lbAccIntPost_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Branch can not do this operation...");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbAccIntPost";
            Response.Redirect("~/WebPages/Private/Transaction/AccruedIntCal.aspx");
        }
        protected void lbTransferLed_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Branch can not do this operation...");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbTransferLed";
            Response.Redirect("~/WebPages/Private/Transaction/TransferLed.aspx");
        }
        protected void lbIntWithheld_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Branch can not do this operation...");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbIntWithheld";
            Response.Redirect("~/WebPages/Private/Transaction/IntWithHeld.aspx");
        }
        protected void lbSavDeposit_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.MsgPopup("Head Office can not do this operation...");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbSavDeposit";
            Response.Redirect("~/WebPages/Private/Transaction/SBTrans.aspx");
        }
        protected void lbBulkSavDeposit_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.MsgPopup("Head Office can not do this operation...");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbBulkSavDeposit";
            Response.Redirect("~/WebPages/Private/Transaction/BulkSBTrans.aspx");
        }
        //protected void lbEoCg_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "Tran";
        //    Session["PaneId"] = acTrn.SelectedIndex;
        //    Session["LinkId"] = "lbEoCg";
        //    Response.Redirect("~/WebPages/Private/Transaction/EoCgtGrt.aspx");
        //}
        protected void lbRedFlag_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbRedFlag";
            Response.Redirect("~/WebPages/Private/Transaction/RedFlagging.aspx");
        }
        protected void lbBnkLoanRcv_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbBnkLoanRcv";
            Response.Redirect("~/WebPages/Private/Transaction/LoanDisbursement.aspx");
        }
        protected void lbBnkLoanPmt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbBnkLoanPmt";
            Response.Redirect("~/WebPages/Private/Transaction/YbLoanCollection.aspx");
        }
        protected void lbBnkLoanBulkPmt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbBnkLoanBulkPmt";
            Response.Redirect("~/WebPages/Private/Transaction/BcLoanBulkPayment.aspx");
        }
        protected void lbSecRef_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbSecRef";
            Response.Redirect("~/WebPages/Private/Transaction/SecurityRefund.aspx");
        }
        protected void lbSrcFnd_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbSrcFnd";
            Response.Redirect("~/WebPages/Private/Transaction/EditFundSource.aspx");
        }
        protected void lbInsPol_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbInsPol";
            Response.Redirect("~/WebPages/Private/Transaction/EditLoanInsure.aspx");
        }
        protected void lbInvLnRes_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbInvLnRes";
            Response.Redirect("~/WebPages/Private/Transaction/LnReschedule.aspx");
        }
        //protected void lbGrpLnRes_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "Tran";
        //    Session["PaneId"] = acTrn.SelectedIndex;
        //    Session["LinkId"] = "lbGrpLnRes";
        //    Response.Redirect("~/WebPages/Private/Transaction/Reschedule.aspx");
        //}
        protected void lbGrMt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbGrMt";
            Response.Redirect("~/WebPages/Private/Transaction/GroupMeetDay.aspx");
        }
        protected void lbDmndCol_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbDmndCol";
            Response.Redirect("~/WebPages/Private/Report/DmndColl.aspx");
        }
        protected void lbCoPrfm_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbCoPrfm";
            Response.Redirect("~/WebPages/Private/Report/COReportWeek.aspx");
        }
        protected void lbBrPrfm_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBrPrfm";
            Response.Redirect("~/WebPages/Private/Report/BMWeekly.aspx");
        }
        #endregion

        #region ReportMenu
        protected void lbExpDisb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbExpDisb";
            Response.Redirect("~/WebPages/Private/Report/ExpectedDisbursement.aspx");
        }
        protected void lbGrExp_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbGrExp";
            Response.Redirect("~/WebPages/Private/Report/GroupExpectedInstallments.aspx");
        }
        protected void lbCo_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbCo";
            Response.Redirect("~/WebPages/Private/Report/COReportDay.aspx");
        }
        protected void lbBm_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBm";
            Response.Redirect("~/WebPages/Private/Report/BMDaily.aspx");
        }
        protected void lbFpay_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbFpay";
            Response.Redirect("~/WebPages/Private/Report/FinalPaid.aspx");
        }
        protected void lbLoanUtilRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLoanUtilRpt";
            Response.Redirect("~/WebPages/Private/Report/LnUtilisationList.aspx");
        }
        protected void lblDPN_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] =acRpt.SelectedIndex;
            Session["linkId"] = "lblDPN";
            Response.Redirect("~/WebPages/Private/Report/DemandPromissoryNote.aspx");
        }
        protected void lbSvLed_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbSvLed";
            Response.Redirect("~/WebPages/Private/Report/SavingsLedger.aspx");
        }
        protected void lblYBPMTSCH_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lblYBPMTSCH";
            Response.Redirect("~/WebPages/Private/Report/YesBankPaymentSchedulerpt.aspx");
        }
        protected void lblYBRepay_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lblYBRepay";
            Response.Redirect("~/WebPages/Private/Report/YesBankCollectionrpt.aspx");
        }
        protected void lbLedgr_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLedgr";
            //Response.Redirect("~/WebPages/Private/Report/CashBook.aspx");
            DataTable dt = null;
            CReports oRpt = null;
            try
            {
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\Ledger.rpt";
                    oRpt = new CReports();
                    dt = oRpt.rptLedger();
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Ledger_List");
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }
        protected void lbEmpRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbEmpRpt";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DataTable dt = null;
            CReports oRpt = null;
            try
            {
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\Emp.rpt";
                    oRpt = new CReports();
                    dt = oRpt.rptEmp(vLogDt);
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pBrCode", vBrCode);
                    rptDoc.SetParameterValue("pTitle", "Employee Report");
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Employee_Report");
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
            finally
            {

            }
        }
        protected void lbTehRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbTehRpt";
            Response.Redirect("~/WebPages/Private/Report/TehBlokVillMaha.aspx");
        }
        protected void lbGrp_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbGrp";
            Response.Redirect("~/WebPages/Private/Report/GroupMst.aspx");
        }
        protected void lbGrSch_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbGrSch";
            Response.Redirect("~/WebPages/Private/Report/GroupMeeting.aspx");
        }
        protected void lbGrSchDtWise_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbGrSchDtWise";
            Response.Redirect("~/WebPages/Private/Report/GroupMeetingDt.aspx");
        }
        protected void lbLnpRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLnpRpt";
            Response.Redirect("~/WebPages/Private/Report/LoanPurpose.aspx");
        }
        protected void lbLnSncRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLnSncRpt";
            Response.Redirect("~/WebPages/Private/Report/LoanSanctionRpt.aspx");
        }
        protected void lbBrHighmarkRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBrHighmarkRpt";
            Response.Redirect("~/WebPages/Private/HOReports/HOHighMarkAnalysis.aspx");
        }
        protected void lbCGTRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbCGTRpt";
            Response.Redirect("~/WebPages/Private/Report/CGTRpt.aspx");
        }
        protected void lbGrtRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbGrtRpt";
            Response.Redirect("~/WebPages/Private/Report/GRTRpt.aspx");
        }
        protected void lbRptTelPreDisb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLnSncRpt";
            Response.Redirect("~/WebPages/Private/Report/rptTelPreDisb.aspx");
        }
        protected void lbLnDisb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLnDisb";
            Response.Redirect("~/WebPages/Private/Report/LoanDisbursement.aspx");
        }
        protected void lblDmdSht_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lblDmdSht";
            Response.Redirect("~/WebPages/Private/Report/DemandSheet.aspx");
        }
        protected void lbParty_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbParty";
            Response.Redirect("~/WebPages/Private/Report/PartyLedgerNew.aspx");
        }
        protected void lbDash_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbDash";
            Response.Redirect("~/WebPages/Private/Master/DashBoard.aspx");
        }
        protected void lbGrpParty_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbGrpParty";
            Response.Redirect("~/WebPages/Private/Report/rptGrpPartyLedger.aspx");
        }
        protected void lbRepay_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRepay";
            Response.Redirect("~/WebPages/Private/Report/RepaymentSche.aspx");
        }
        protected void lbGrpRepay_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbGrpRepay";
            Response.Redirect("~/WebPages/Private/Report/rptGroupRepaySche.aspx");
        }
        protected void lbColRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbColRpt";
            Response.Redirect("~/WebPages/Private/Report/CollectionRpt.aspx");
        }
        protected void lb32_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb32";
            Response.Redirect("~/WebPages/Private/Report/CashBook.aspx");
        }
        protected void lb33_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb33";
            Response.Redirect("~/WebPages/Private/Report/BankBook.aspx");
        }
        protected void lb34_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb34";
            Response.Redirect("~/WebPages/Private/Report/JournalBook.aspx");
        }
        protected void lbTranBook_Click(object snender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbTranBook";
            Response.Redirect("~/WebPages/Private/Report/TransactionBook.aspx");
        }
        protected void lbLedgerBook_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLedgerBook";
            Response.Redirect("~/WebPages/Private/Report/LedgerBook.aspx");
        }
        protected void lb44_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb44";
            Response.Redirect("~/WebPages/Private/Report/AccLedgerDtls.aspx");
        }
        protected void lb45_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb45";
            Response.Redirect("~/WebPages/Private/Report/AccLedgerSummary.aspx");
        }
        //protected void lblSub_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "Rept";
        //    Session["PaneId"] = acRpt.SelectedIndex;
        //    Session["LinkId"] = "lblSub";
        //    Response.Redirect("~/WebPages/Private/Report/AccSubLedgerDtls.aspx");
        //}
        protected void lb46_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb46";
            Response.Redirect("~/WebPages/Private/Report/RecPay.aspx");
        }
        protected void lb47_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb47";
            Response.Redirect("~/WebPages/Private/Report/ProfitLoss.aspx");
        }
        protected void lb48_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb48";
            Response.Redirect("~/WebPages/Private/Report/Trial.aspx");
        }
        protected void lb49_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb49";
            Response.Redirect("~/WebPages/Private/Report/BalSheet.aspx");
        }
        protected void lbBRecon_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBRecon";
            Response.Redirect("~/WebPages/Private/Report/BankReconRpt.aspx");
        }
        protected void lb35_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb35";
            Response.Redirect("~/WebPages/Private/Report/CollSheet.aspx");
        }
        protected void lb36_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb36";
            Response.Redirect("~/WebPages/Private/Report/DisbRpt.aspx");
        }
        protected void lb37_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb37";
            Response.Redirect("~/WebPages/Private/Report/CollectionRpt.aspx");
        }
        protected void lb38_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb38";
            Response.Redirect("~/WebPages/Private/Report/AtaGlance.aspx");
        }
        protected void lbPfAct_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbPfAct";
            Response.Redirect("~/WebPages/Private/Report/PortfolioActivity.aspx");
        }
        protected void lbAgeW_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbAgeW";
            Response.Redirect("~/WebPages/Private/Report/AgeWise.aspx");
        }
        protected void lbFeCol_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbFeCol";
            Response.Redirect("~/WebPages/Private/Report/FeesCollection.aspx");
        }
        protected void lbAIF_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbAIF";
            Response.Redirect("~/WebPages/Private/Report/AIFData.aspx");
        }
        protected void lbYBMDSR_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbYBMDSR";
            Response.Redirect("~/WebPages/Private/Report/YBMasterDataReport.aspx");
        }
        protected void lbBD_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBD";
            Response.Redirect("~/WebPages/Private/Report/BookDebtReport.aspx");
        }
        protected void lb39_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb39";
            Response.Redirect("~/WebPages/Private/Report/HHSurvey.aspx");
        }
        protected void lb40_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb40";
            Response.Redirect("~/WebPages/Private/Report/ShgGrading.aspx");
        }
        protected void lb41_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb41";
            Response.Redirect("~/WebPages/Private/Report/RatioAnalysis.aspx");
        }
        protected void lb42_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb42";
            Response.Redirect("~/WebPages/Private/Report/PortfolioAgeing.aspx");
        }
        protected void lbLnStatus_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLnStatus";
            Response.Redirect("~/WebPages/Private/Report/LoanStat.aspx");
        }
        protected void lbLnStatComb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLnStatComb";
            Response.Redirect("~/WebPages/Private/Report/LoanStatCombine.aspx");
        }
        protected void lbMIS_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbMIS";
            Response.Redirect("~/WebPages/Private/Report/MISReport.aspx");
        }
        protected void lbLeadInfo_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLeadInfo";
            Response.Redirect("~/WebPages/Private/Report/LeadInformation.aspx");
        }
        protected void lbCIBILSubmission_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbCIBILSubmission";
            Response.Redirect("~/WebPages/Private/Report/CIBILSubmissionRpt.aspx");
        }
        protected void lbHOInsuranceData_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Rept";
                Session["PaneId"] = acRpt.SelectedIndex;
                Session["LinkId"] = "lbHOInsuranceData";
                Response.Redirect("~/WebPages/Private/HOReports/RptHOInsurance.aspx");
            }
            else
            {
                gblFuction.MsgPopup("Branch Login can not Access this Option.");
                return;
            }
        }
        protected void lbMISSummary_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbMISSummary";
            Response.Redirect("~/WebPages/Private/Report/MISReportSummary.aspx");
        }
        protected void lbCAM_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbCAM";
            Response.Redirect("~/WebPages/Private/Report/CAMReport.aspx");
        }
        protected void lbPortBreakUp_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbPortBreakUp";
            Response.Redirect("~/WebPages/Private/Report/PortfolioBreakUpReport.aspx");
        }
        protected void lbMOMDisb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbMOMDisb";
            Response.Redirect("~/WebPages/Private/Report/MOMDisbReport.aspx");
        }
        protected void lbFutureDem_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbFutureDem";
            Response.Redirect("~/WebPages/Private/Report/FutureDemandRpt.aspx");
        }
        protected void lbHOCBCheckRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbHOCBCheckRpt";
            Response.Redirect("~/WebPages/Private/Report/CBCheckRpt.aspx");
        }
        protected void lbHOLogInMisRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbHOLogInMisRpt";
            Response.Redirect("~/WebPages/Private/Report/LogInMisRpt.aspx");
        }
        protected void lblCBOverrideRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lblCBOverrideRpt";
            Response.Redirect("~/WebPages/Private/Report/CbOverrideRpt.aspx");
        }

        protected void lbOthColl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbOthColl";
            Response.Redirect("~/WebPages/Private/Report/OtherCollectionRpt.aspx");
        }

        protected void lbCbEnqRpt_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Rept";
                Session["PaneId"] = acRpt.SelectedIndex;
                Session["LinkId"] = "lbCbEnqRpt";
                Response.Redirect("~/WebPages/Private/Report/CBEnqRpt.aspx");
            }
            else
            {
                gblFuction.MsgPopup("Branch Login can not Access this Option.");
                return;
            }
        }

        protected void lbOthChrgDmdShtRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbOthChrgDmdShtRpt";
            Response.Redirect("~/WebPages/Private/HOReports/OtherChargesDemandSheet.aspx");
        }

        protected void lbChargesRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbChargesRpt";
            Response.Redirect("~/WebPages/Private/HOReports/RptPenalCharge.aspx");
        }

        protected void lbFuturePay_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbFuturePay";
            Response.Redirect("~/WebPages/Private/Report/FuturePaymentLendRpt.aspx");
        }
        protected void lbPortfolioCut_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbPortfolioCut";
            Response.Redirect("~/WebPages/Private/Report/PortfolioCutRpt.aspx");
        }
        protected void lbDisbCut_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbDisbCut";
            Response.Redirect("~/WebPages/Private/Report/DisbCutRpt.aspx");
        }
        protected void lbBankStatmnt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBankStatmnt";
            Response.Redirect("~/WebPages/Private/Report/BankStatmntReport.aspx");
        }
        protected void lbFinStatmnt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbFinStatmnt";
            Response.Redirect("~/WebPages/Private/Report/FinStatmntReport.aspx");
        }
        protected void lb50_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb50";
            Response.Redirect("~/WebPages/Private/Report/RePayment.aspx");
        }
        protected void lb51_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbl51";
            Response.Redirect("~/WebPages/Private/Report/PartyLedger.aspx");
        }
        protected void lbLnInf_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLnInf";
            Response.Redirect("~/WebPages/Private/Report/LoanMasterInfo.aspx");
        }
        protected void lbAttReg_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbAttReg";
            Response.Redirect("~/WebPages/Private/Report/AttendanceReg.aspx");
        }
        protected void lbLnUtil_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLnUtil";
            Response.Redirect("~/WebPages/Private/Report/LoanUtlRpt.aspx");
        }
        protected void lbAudTail_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbAudTail";
            Response.Redirect("~/WebPages/Private/Report/RptHighMark.aspx");
        }
        protected void lblInsData_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lblInsData";
            Response.Redirect("~/WebPages/Private/Report/RptInsurance.aspx");
        }
        protected void lbRptEoCG_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptEoCG";
            Response.Redirect("~/WebPages/Private/Report/EoWiseCgtGrtRpt.aspx");
        }
        protected void lblDeclaration_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lblDeclaration";
            Response.Redirect("~/WebPages/Private/Report/rptWriteOffDec.aspx");
        }
        protected void lblWriteOffDemad_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lblWriteOffDemad";
            Response.Redirect("~/WebPages/Private/Report/rptWriteOffDemand.aspx");
        }
        protected void lblWriteOffCol_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lblWriteOffCol";
            Response.Redirect("~/WebPages/Private/Report/rptWriteOffCollection.aspx");
        }
        protected void lblWriteOffStat_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lblWriteOffStat";
            Response.Redirect("~/WebPages/Private/Report/rptWriteOffStatus.aspx");
        }
        protected void lbSavingsDetailList_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbSavingsDetailList";
            Response.Redirect("~/WebPages/Private/Report/SavingsDetailList.aspx");
        }
        protected void lbSMSStatus_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbSMSStatus";
            Response.Redirect("~/WebPages/Private/Report/SMSStatusRpt.aspx");
        }
        #endregion

        #region SystemMenu
        protected void lbS7_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                //Response.Redirect("~/WebPages/Public/Main.aspx", false);
                //gblFuction.MsgPopup("Head Office can not start day End only See the Report...");
                //return;
            }
            else
            {
                if (Convert.IsDBNull(Session[gblValue.EndDate]))
                {
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(Session[gblValue.LoginDate].ToString()))
                    {
                        gblFuction.MsgPopup("Day End already completed for--" + gblFuction.putStrDate(gblFuction.setDate(Session[gblValue.LoginDate].ToString())));
                        return;
                    }
                }
            }
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbS7";
            Response.Redirect("~/WebPages/Private/Admin/EndDate.aspx");
            //gblFuction.MsgPopup("Day End process is now De-Activated...");
        }
        protected void lbS1_Click(object sender, EventArgs e)
        {
            //gblFuction.MsgPopup("Carry Forward process is now De-Activated...");
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbS1";
            Response.Redirect("~/WebPages/Private/Admin/YearEnd.aspx");
        }
        protected void lbS3_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbS3";
            Response.Redirect("~/WebPages/Private/Admin/Role.aspx");
        }
        protected void lbS4_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbS4";
            Response.Redirect("~/WebPages/Private/Admin/RoleAssigne.aspx");
        }
        protected void lbS6_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbS6";
            Response.Redirect("~/WebPages/Private/Admin/Users.aspx");
        }
        protected void lbS8A_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbS8A";
            Response.Redirect("~/WebPages/Private/Admin/MobUserMaster.aspx");
        }
        protected void lbS5_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbS5";
            Response.Redirect("~/WebPages/Private/Admin/ChgPass.aspx");
        }
        protected void lbS2_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                Session["MnuId"] = "Syst";
                Session["PaneId"] = acSys.SelectedIndex;
                Session["LinkId"] = "lbS2";
                Response.Redirect("~/WebPages/Private/Admin/MobileFileGenerate.aspx");
            }
            else
            {
                gblFuction.MsgPopup("Branch Login Cannot use It.........");
                return;
            }
        }
        protected void lbS8_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                Session["MnuId"] = "Syst";
                Session["PaneId"] = acSys.SelectedIndex;
                Session["LinkId"] = "lbS8";
                Response.Redirect("~/WebPages/Private/Admin/AuditTrail.aspx");
            }
            else
            {
                gblFuction.MsgPopup("Branch Login Cannot use It.........");
                return;
            }
        }
        protected void lbS9_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                string strURL = HttpContext.Current.Request.PhysicalApplicationPath + "App_Data\\CENTRUMSME.apk";
                System.Net.WebClient req = new System.Net.WebClient();
                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                response.AddHeader("Content-Disposition", "attachment;filename=\"" + strURL);
                byte[] data = req.DownloadData(Server.MapPath(strURL));
                response.BinaryWrite(data);
                response.End();
            }
            else
            {
                gblFuction.MsgPopup("Branch Login Cannot use It.........");
                return;
            }
        }
        #endregion

        #region DashBoard
        protected void lbdb1_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbdb1";
            Response.Redirect("~/WebPages/Private/DashBoard/DBAtaglace.aspx");
        }
        protected void lbdb2_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbdb2";
            Response.Redirect("~/WebPages/Private/DashBoard/DBDisb.aspx");
        }
        #endregion

        

        #region UtilityMenu
        protected void lbFulBak_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Fedaration can not take the backup...");
            //    return;
            //}
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbFulBak";
            Response.Redirect("~/WebPages/Private/Admin/DbBackup.aspx");
        }
        protected void lbU1_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.MsgPopup("Head Office can not do this operation...");
                return;
            }
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbU1";
            Response.Redirect("~/WebPages/Private/Admin/Transfer.aspx");
        }
        protected void lbU111_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.MsgPopup("Head Office can not do this operation...");
                return;
            }
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbU111";
            Response.Redirect("~/WebPages/Private/Admin/InterBrTransfer.aspx");
        }
        //protected void lbU4_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "Utly";
        //    Session["PaneId"] = Accordion1.SelectedIndex;
        //    Session["LinkId"] = "lbU4";
        //    Response.Redirect("~/WebPages/Private/Admin/DocuUpDownLoad.aspx");
        //}
        protected void lbGrpLnRes_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Utly";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbGrpLnRes";
            Response.Redirect("~/WebPages/Private/Transaction/Reschedule.aspx");
        }
        protected void lbU2_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Fedaration can not take the backup...");
            //    return;
            //}
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbU2";
            Response.Redirect("~/WebPages/Private/Admin/TblSql.aspx");
        }
        protected void lbU3_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Fedaration can not take the backup...");
            //    return;
            //}
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbU3";
            Response.Redirect("~/WebPages/Private/Admin/ExctSql.aspx");
        }
        //protected void lbLnCal_Click(object sender, EventArgs e)
        //{
        //    //if (Session[gblValue.BrnchCode].ToString() != "0000")
        //    //{
        //    //    gblFuction.MsgPopup("Fedaration can not take the backup...");
        //    //    return;
        //    //}
        //    Session["MnuId"] = "Utly";
        //    Session["PaneId"] = Accordion1.SelectedIndex;
        //    Session["LinkId"] = "lbLnCal";
        //    Response.Redirect("~/WebPages/Private/Admin/LoanCalculator.aspx");
        //}
        protected void lbSearch_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Fedaration can not take the backup...");
            //    return;
            //}
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbSearch";
            Response.Redirect("~/WebPages/Private/Admin/Search.aspx");
        }

        protected void lbBlkLnReSchduling_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbBlkLnReSchduling";
            Response.Redirect("~/WebPages/Private/Transaction/ImportExcelData.aspx");
        }
        #endregion

        #region GSTMenu
        protected void lbGSTInvoice_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "GST";
            Session["PaneId"] = acGST.SelectedIndex;
            Session["LinkId"] = "lbGSTInvoice";
            Response.Redirect("~/WebPages/Private/Report/GSTInvoice.aspx", false);
        }
        #endregion

        #region Properties
        public bool Menu
        {
            set
            {
                btnAct.Enabled = value;
                btnMst.Enabled = value;
                btnTrn.Enabled = value;
                btnRpt.Enabled = value;
                btnSys.Enabled = value;
                btnUty.Enabled = value;
                btnOprtn.Enabled = value;
                btnGST.Enabled = value;
                //btnAudit.Enabled = value;
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

        # region Consolidate Reports
        protected void lblHighMarkAnalysis_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHighMarkAnalysis";
            Response.Redirect("~/WebPages/Private/HOReports/HOHighMarkAnalysis.aspx");
        }
        protected void lblHOHighMarkData_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHOHighMarkData";
            Response.Redirect("~/WebPages/Private/HOReports/HOHighMarkEnquiry.aspx");
        }
        protected void lbSMSRemind_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                // Response.Redirect("~/WebPages/Public/Main.aspx", false);
                gblFuction.MsgPopup("Branch Office Can Not Do This Operation...");
                return;
            }
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbSMSRemind";
            Response.Redirect("~/WebPages/Private/Admin/SMSReminder.aspx");
        }
        protected void lbDsbPos_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDsbPos";
            Response.Redirect("~/DashBoard/DsbPos.aspx");
        }
        protected void lbDsbActvClnt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDsbActvClnt";
            Response.Redirect("~/DashBoard/DsbActiveClient.aspx");
        }
        protected void lbDsbODClnt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDsbODClnt";
            Response.Redirect("~/DashBoard/DsbODClient.aspx");
        }
        protected void lbDsbODAmt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDsbODAmt";
            Response.Redirect("~/DashBoard/DsbODAmount.aspx");
        }
        protected void lbDsbODOS_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDsbODOS";
            Response.Redirect("~/DashBoard/DsbODOS.aspx");
        }
        protected void lbDsbDisbAmt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDsbDisbAmt";
            Response.Redirect("~/DashBoard/DsbDisbAmt.aspx");
        }
        protected void lbRRAmt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRRAmt";
            Response.Redirect("~/DashBoard/DsbRR.aspx");
        }
        protected void lbDsbDisbNo_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDsbDisbNo";
            Response.Redirect("~/DashBoard/DsbDisbNo.aspx");
        }
        protected void lblHOHMark_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHOHMark";
            Response.Redirect("~/WebPages/Private/HOReports/HiMarkDataSubmission.aspx");
        }
        protected void lblHOHMarkEnq_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHOHMarkEnq";
            Response.Redirect("~/WebPages/Private/HOReports/HoHighmark.aspx");
        }
        protected void lblInsHO_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblInsHO";
            Response.Redirect("~/WebPages/Private/HOReports/rptInsuranceHO.aspx");
        }
        protected void lbHOPer_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOPer";
            Response.Redirect("~/WebPages/Private/HOReports/HOBranchPerformance.aspx");
        }
        protected void lbHOComA_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOComA";
            Response.Redirect("~/WebPages/Private/Report/CompAnalysisPos.aspx");
        }
        protected void lbHOPortAct_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOPortAct";
            Response.Redirect("~/WebPages/Private/HOReports/HOPortfolioActivity.aspx");
        }
        protected void lbHOAtAGlance_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOAtAGlance";
            Response.Redirect("~/WebPages/Private/HOReports/HOAtAGlance.aspx");
        }
        protected void lbHODisb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHODisb";
            Response.Redirect("~/WebPages/Private/HOReports/HOLoanDisburse.aspx");
        }
        protected void lbBucket_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbBucket";
            Response.Redirect("~/WebPages/Private/Report/LoanStatusBucket.aspx");
        }
        protected void lblHORecPay_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHORecPay";
            Response.Redirect("~/WebPages/Private/HOReports/HORecPay.aspx");
        }
        protected void lblHOProfitLoss_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHOProfitLoss";
            Response.Redirect("~/WebPages/Private/HOReports/HOProfitLoss.aspx");
        }
        protected void lblHOTrial_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHOTrial";
            Response.Redirect("~/WebPages/Private/HOReports/HOTrial.aspx");
        }
        protected void lblHOBalSheet_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHOBalSheet";
            Response.Redirect("~/WebPages/Private/HOReports/HOBalSheet.aspx");
        }
        protected void lbHOBankRecoRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOBankRecoRpt";
            Response.Redirect("~/WebPages/Private/HOReports/BankRecoRpt.aspx");
        }
        protected void lbHOAcLed_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOAcLed";
            Response.Redirect("~/WebPages/Private/HOReports/HOAccLedgerDtl.aspx");
        }
        protected void lbHOPortAge_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOPortAge";
            Response.Redirect("~/WebPages/Private/HOReports/HOPortfolioAgeing.aspx");
        }
        protected void lblHOODClient_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHOODClient";
            Response.Redirect("~/WebPages/Private/HOReports/HOODClientList.aspx");
        }
        protected void lbHoEoCgRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHoEoCgRpt";
            Response.Redirect("~/WebPages/Private/Report/EoWiseCgtGrtRpt.aspx");
        }
        protected void lblHOApprovNotDisb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHOApprovNotDisb";
            Response.Redirect("~/WebPages/Private/HOReports/HOApprovNotDisb.aspx");
        }
        protected void lblHoFinalPaid_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHoFinalPaid";
            Response.Redirect("~/WebPages/Private/HOReports/HOFinalPaid.aspx");
        }
        protected void lblHODemndAndColl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHODemndAndColl";
            Response.Redirect("~/WebPages/Private/HOReports/HODemandColl.aspx");
        }
        protected void lblClRet_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblClRet";
            Response.Redirect("~/WebPages/Private/HOReports/HOClientRetain.aspx");
        }
        protected void lblFoWiseLnStat_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblFoWiseLnStat";
            Response.Redirect("~/WebPages/Private/HOReports/FOwiseLnStat.aspx");
        }
        protected void lblHOAging_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHoAging";
            Response.Redirect("~/WebPages/Private/HOReports/HOAging.aspx");
        }
        protected void lblHORatioAnalysis_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHORatioAnalysis";
            Response.Redirect("~/WebPages/Private/HOReports/HORatioAnalysis.aspx");
        }
        protected void lblHOCashCompair_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHOCashCompair";
            Response.Redirect("~/WebPages/Private/HOReports/HOCashCompair.aspx");
        }
        protected void lblHOFundRequirement_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHOFundRequirement";
            Response.Redirect("~/WebPages/Private/HOReports/HOFundReq.aspx");
        }
        protected void lbHOClientProfile_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOClientProfile";
            Response.Redirect("~/WebPages/Private/HOReports/HOClientProfile.aspx");
        }
        protected void lbHOLoanSanction_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOLoanSanction";
            Response.Redirect("~/WebPages/Private/HOReports/HOLoanSanction.aspx");
        }
        protected void lbHODemandSheet_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHODemandSheet";
            Response.Redirect("~/WebPages/Private/HOReports/HODemandSheet.aspx");
        }
        protected void lbHOPortfolioAgeing_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOPortfolioAgeing";
            Response.Redirect("~/WebPages/Private/HOReports/HOPortfolioAgeing.aspx");
        }
        protected void lblODClientList_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblODClientList";
            Response.Redirect("~/WebPages/Private/HOReports/ODClientList.aspx");
        }
        protected void lbDashboard_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDashboard";
            Response.Redirect("~/WebPages/Private/HOReports/HOAtaGlance.aspx");
        }
        protected void lbBounceList_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbBounceList";
            Response.Redirect("~/WebPages/Private/Report/BounceList.aspx");
        }
        protected void lbNonBounceRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbNonBounceRpt";
            Response.Redirect("~/WebPages/Private/Report/NonBounceList.aspx");
        }
        protected void lbNDCRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbNDCRpt";
            Response.Redirect("~/WebPages/Private/Report/rptNDC.aspx");
        }

        protected void lbLnParamRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLnParamRpt";
            Response.Redirect("~/WebPages/Private/HOReports/RptLoanParameter.aspx");
        }

        protected void lbWriteOffDeclRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbWriteOffDeclRpt";
            Response.Redirect("~/WebPages/Private/Report/WriteOffDeclareList.aspx");
        }
        protected void lbWriteOffCollec_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbWriteOffDeclRpt";
            Response.Redirect("~/WebPages/Private/Report/WriteOffCollection.aspx");
        }
        protected void lbWriteOffStatus_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbWriteOffStatus";
            Response.Redirect("~/WebPages/Private/Report/WriteOffStatus.aspx");
        }
        protected void lbSettleCustomer_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbSettleCustomer";
            Response.Redirect("~/WebPages/Private/Report/SettleCustomerList.aspx");
        }
        protected void lbLenderRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbLenderRpt";
            Response.Redirect("~/WebPages/Private/Report/LendersReport.aspx");
        }
        protected void lbLenRepaySch_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbLenRepaySch";
            Response.Redirect("~/WebPages/Private/Report/LenderRepaymentSche.aspx");
        }
        protected void lbHOCollection_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOCollection";
            Response.Redirect("~/WebPages/Private/HOReports/HOLoanCollection.aspx");
        }
        protected void lblHOLoanUtilRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHOLoanUtilRpt";
            Response.Redirect("~/WebPages/Private/Report/LnUtilisationList.aspx");
        }
        protected void lbHOFeesCollectionRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOFeesCollectionRpt";
            Response.Redirect("~/WebPages/Private/HOReports/HOFeesCollection.aspx");
        }
        protected void lbHOSettleClnt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOSettleClnt";
            Response.Redirect("~/WebPages/Private/HOReports/HOSettledClientList.aspx");
        }
        protected void lbrptHOPAR_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbrptHOPAR";
            Response.Redirect("~/WebPages/Private/HOReports/rptHOPAR.aspx", false);
        }
        protected void lbHoAccIntRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHoAccIntRpt";
            Response.Redirect("~/WebPages/Private/HOReports/HOAccIntRpt.aspx", false);
        }
        protected void lbLoanPreCloserView_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbLoanPreCloserView";
            Response.Redirect("~/WebPages/Private/Report/PreCloser.aspx");
        }
        protected void lblHOPartyLedger_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHOPartyLedger";
            Response.Redirect("~/WebPages/Private/Report/PartyLedgerNew.aspx");
        }
        protected void lbHOSavingsDetailList_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOSavingsDetailList";
            Response.Redirect("~/WebPages/Private/HOReports/HOSavingsDetailList.aspx");
        }
        protected void lbHOWriteOffDeathList_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOWriteOffDeathList";
            Response.Redirect("~/WebPages/Private/HOReports/HOWriteOffDeathRpt.aspx");
        }
        #endregion

        #region Bonfleet
        protected void lbuBonCustRegistration_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Bon";
            Session["PaneId"] = AccBonFleet.SelectedIndex;
            Session["LinkId"] = "lbuBonCustRegistration";
            Response.Redirect("~/WebPages/Private/Report/BonCustReport.aspx");
        }
        protected void lbuBonCustReg_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Bon";
            Session["PaneId"] = AccBonFleet.SelectedIndex;
            Session["LinkId"] = "lbuBonCustReg";
            Response.Redirect("~/WebPages/Private/Report/BonCustReg.aspx");
        }
        protected void lbuBonLoanDisb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Bon";
            Session["PaneId"] = AccBonFleet.SelectedIndex;
            Session["LinkId"] = "lbuBonLoanDisb";
            Response.Redirect("~/WebPages/Private/Report/BonLoanDisb.aspx");
        }
        protected void lbuBonLoanColl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Bon";
            Session["PaneId"] = AccBonFleet.SelectedIndex;
            Session["LinkId"] = "lbuBonLoanColl";
            Response.Redirect("~/WebPages/Private/Report/BonLoanColl.aspx");
        }
        protected void lbuBonPartyLed_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Bon";
            Session["PaneId"] = AccBonFleet.SelectedIndex;
            Session["LinkId"] = "lbuBonPartyLed";
            Response.Redirect("~/WebPages/Private/Report/BonPartyLedger.aspx");
        }
        protected void lbuBonLoanStatus_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Bon";
            Session["PaneId"] = AccBonFleet.SelectedIndex;
            Session["LinkId"] = "lbuBonLoanStatus";
            Response.Redirect("~/WebPages/Private/Report/BonLoanStatus.aspx");
        }
        protected void lbBonClosedLoan_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Bon";
            Session["PaneId"] = AccBonFleet.SelectedIndex;
            Session["LinkId"] = "lbBonClosedLoan";
            Response.Redirect("~/WebPages/Private/Report/BonSettleCustomerList.aspx");
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