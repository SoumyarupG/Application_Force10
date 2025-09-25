using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;
using System.Web;
using System.Configuration;

namespace CENTRUM
{
    public partial class CentrumMFI : System.Web.UI.MasterPage
    {
        string vAccessTime = ConfigurationManager.AppSettings["AccessTime"];
        
        /// <summary>
        /// 900
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
           
            //string Status = "";
            ////if (Session["Switch"] != null)
            ////{
            ////    Status = Session["Switch"].ToString();
            ////}
            ////else
            ////{
            //CLogin cl = new CLogin();
            //Status = cl.CHKProcActYN();
            //Session["Switch"] = Status;
            ////}

            //if (Status == "Y")
            //{
            //    string authenticate_status = (Session["authenticate_status"] == null) ? "failed" : Session["authenticate_status"].ToString();
            //    if (authenticate_status == "failed")
            //    {
            //        Response.RedirectPermanent("Unauthorized.aspx");
            //        Session["authenticate_status"] = null;
            //    }
            //}

            if (Request.Cookies["UNITY"] != null)
            {
                if (Convert.ToString(Session["LoginCookies"]) != Request.Cookies["UNITY"].Value)
                {
                    Response.Redirect("SsnExpr.aspx");
                }
            }


            Cmarquee objMar = null;
            DataTable dt = null;
            Response.AppendHeader("Refresh", Convert.ToString(1800) + "; url=SsnExpr.aspx");
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                btnConRpt.Visible = true;
                btnGST.Visible = true;
                //apMstList.Visible = false;
                apLnReltd.Visible = false;
                apMIS.Visible = false;
                //lbNEFTDisb.Visible = false;
                //lbNEFTTransfer.Visible = true;

                lbVend.Visible = true;
                lbItem.Visible = true;

                lbItmRec.Visible = true;
                lbHoArea.Visible = true;
                lbHoBr.Visible = true;
                lbArBr.Visible = false;
                lbBrBr.Visible = false;
                lbBrSt.Visible = true;
                lbRVHoArea.Visible = false;
                lbRVHoToBr.Visible = false;
                lbRVArToBr.Visible = false;
                lbRVBrToBr.Visible = false;
                lbInOutLtr.Visible = true;
                lbBRPettyCash.Visible = false;
                lbBRPettyCashBalance.Visible = false;

                //lbNEFTDisbApprovHO.Visible = true;

            }
            else
            {
                btnConRpt.Visible = false;
                btnGST.Visible = false;
                //apMstList.Visible = true;
                apLnReltd.Visible = true;
                apMIS.Visible = true;
                //lbNEFTDisb.Visible = true;
                lbNEFTTransfer.Visible = false;
                lbCanDisb.Visible = false;

                lbVend.Visible = false;
                lbItem.Visible = false;

                lbItmRec.Visible = false;
                lbHoArea.Visible = false;
                lbHoBr.Visible = false;
                lbArBr.Visible = true;
                lbBrBr.Visible = true;
                lbBrSt.Visible = true;
                lbRVHoArea.Visible = true;
                lbRVHoToBr.Visible = true;
                lbRVArToBr.Visible = true;
                lbRVBrToBr.Visible = true;
                lbInOutLtr.Visible = false;
                lbNEFTDisbApprovHO.Visible = false;


            }

            if (!Page.IsPostBack)
            {
                objMar = new Cmarquee();
                dt = objMar.GetActiveMarquee(0);
                if (dt.Rows.Count > 0)
                    ltMarquee.Text = dt.Rows[0]["MarqueeName"].ToString();
                else
                    ltMarquee.Text = "";

                if (Session["MnuId"] == null)
                    MenuStat("Deft");
                else
                {
                    MenuStat(Session["MnuId"].ToString());
                    if (Session["LinkId"] != null)
                        MnuFocus(Session["MnuId"].ToString(), Convert.ToInt32(Session["PaneId"]), Session["LinkId"].ToString());

                    if (Convert.ToString(Session[gblValue.AgencyType]) == "Y")
                    {
                        btnAct.Visible = false;
                        btnMst.Visible = false;
                        btnOprtn.Visible = false;
                        btnTrn.Visible = false;
                        btnTraining.Visible = false;
                        btnRpt.Visible = false;
                        btnConRpt.Visible = false;
                        btnSys.Visible = false;
                        btnUty.Visible = false;
                        btnUty.Visible = false;
                    }

                }
                lblVersion.Text = String.Format("Version: {0} &nbsp; Dated: {1}",
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                System.IO.File.GetLastWriteTime(Request.PhysicalApplicationPath.ToString() + "\\bin\\CENTRUM.dll").ToString("dd/MMM/yyyy HH:mm:ss"));
            }
        }
        public string getCurrentPageName()
        {
            string pagename = this.cph_Main.Page.GetType().FullName;
            return pagename;
        }

        public string getWindowName()
        {
            //if (this.cph_Main.Page.GetType().FullName != "ASP.vfs_aspx")
            //{
            if (Session["WindowName"] != null)
            {
                string WindowName = Session["WindowName"].ToString();
                return WindowName;
            }
            else
            {
                return "Invalid";
            }
            //}
            //else
            //{
            //    return "NA";
            //}
        }

        public string geErrorPageName()
        {
            string errorpage = Page.ResolveUrl("~/SsnExpr.aspx");
            return errorpage;
        }
        /// <summary>
        /// D:\Web Software\SBT_Web\CENTRUM\WebPages\Public\ErrDisp.aspx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Error(object sender, EventArgs e)
        {
            Response.RedirectPermanent("~/WebPages/Public/ErrDisp.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMenu"></param>
        /// <param name="vIndex"></param>
        /// <param name="lkId"></param>
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
                case "Training":
                    acTraining.SelectedIndex = vIndex;
                    LinkButton lbTraining = (LinkButton)this.acTraining.FindControl(lkId);
                    lbTraining.Style.Add("background", "#95B5D6");
                    btnTraining.Style.Add("background", "#3da1e0");
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
                case "ConRpt":
                    acConRpt.SelectedIndex = vIndex;
                    LinkButton lBtn8 = (LinkButton)this.acConRpt.FindControl(lkId);
                    lBtn8.Style.Add("background", "#95B5D6");
                    btnConRpt.Style.Add("background", "#3da1e0");
                    break;
                case "NPS":
                    acNPS.SelectedIndex = vIndex;
                    LinkButton lBtn11 = (LinkButton)this.acNPS.FindControl(lkId);
                    lBtn11.Style.Add("background", "#95B5D6");
                    btnNPS.Style.Add("background", "#3da1e0");
                    break;
                case "BCOpr":
                    acBCOpr.SelectedIndex = vIndex;
                    LinkButton lBtn12 = (LinkButton)this.acBCOpr.FindControl(lkId);
                    lBtn12.Style.Add("background", "#95B5D6");
                    btnBCOpr.Style.Add("background", "#3da1e0");
                    break;
                case "GST":
                    acGST.SelectedIndex = vIndex;
                    LinkButton lBtn13 = (LinkButton)this.acGST.FindControl(lkId);
                    lBtn13.Style.Add("background", "#95B5D6");
                    btnGST.Style.Add("background", "#3da1e0");
                    break;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMenu"></param>
        private void MenuStat(string pMenu)
        {
            switch (pMenu)
            {
                case "Acct":
                    mnuAct.Visible = true;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuTraining.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuNPS.Visible = false;
                    mnuBCOpr.Visible = false;
                    mnuSME.Visible = false;
                    mnuGST.Visible = false;
                    break;
                case "Mst":
                    mnuAct.Visible = false;
                    mnuMst.Visible = true;
                    mnuTrn.Visible = false;
                    mnuTraining.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuNPS.Visible = false;
                    mnuBCOpr.Visible = false;
                    mnuSME.Visible = false;
                    mnuGST.Visible = false;

                    break;
                case "Tran":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = true;
                    mnuTraining.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuNPS.Visible = false;
                    mnuBCOpr.Visible = false;
                    mnuSME.Visible = false;
                    mnuGST.Visible = false;
                    break;
                case "Training":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuTraining.Visible = true;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuNPS.Visible = false;
                    mnuBCOpr.Visible = false;
                    mnuSME.Visible = false;
                    mnuGST.Visible = false;

                    break;
                case "Rept":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuTraining.Visible = false;
                    mnuRpt.Visible = true;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuNPS.Visible = false;
                    mnuBCOpr.Visible = false;
                    mnuSME.Visible = false;
                    mnuGST.Visible = false;

                    break;
                case "Syst":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuTraining.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = true;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuNPS.Visible = false;
                    mnuBCOpr.Visible = false;
                    mnuSME.Visible = false;
                    mnuGST.Visible = false;

                    break;
                case "BCOpr":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuTraining.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuBCOpr.Visible = true;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuNPS.Visible = false;
                    mnuSME.Visible = false;
                    mnuGST.Visible = false;

                    break;
                case "Utly":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuTraining.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = true;
                    mnuOpr.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuNPS.Visible = false;
                    mnuBCOpr.Visible = false;
                    mnuSME.Visible = false;
                    mnuGST.Visible = false;

                    break;
                case "Opr":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuTraining.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = true;
                    mnuConRpt.Visible = false;
                    mnuNPS.Visible = false;
                    mnuBCOpr.Visible = false;
                    mnuSME.Visible = false;
                    mnuGST.Visible = false;

                    break;
                case "Audit":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuTraining.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuNPS.Visible = false;
                    mnuBCOpr.Visible = false;
                    mnuSME.Visible = false;
                    mnuGST.Visible = false;

                    break;
                case "Deft":

                    btnAct.Enabled = true;
                    btnMst.Enabled = true;
                    btnOprtn.Enabled = true;
                    btnTrn.Enabled = true;
                    btnTraining.Enabled = true;
                    btnRpt.Enabled = true;
                    btnConRpt.Enabled = true;
                    btnBCOpr.Enabled = true;
                    btnSys.Enabled = true;
                    btnUty.Enabled = true;
                    btnNPS.Enabled = true;
                    btnGST.Enabled = true;

                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuTraining.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuNPS.Visible = false;
                    mnuBCOpr.Visible = false;
                    mnuSME.Visible = false;
                    mnuGST.Visible = false;

                    break;

                case "Init":
                    btnAct.Enabled = false;
                    btnMst.Enabled = false;
                    btnOprtn.Enabled = false;
                    btnTrn.Enabled = false;
                    btnTraining.Enabled = false;
                    btnRpt.Enabled = false;
                    btnConRpt.Enabled = false;
                    btnBCOpr.Enabled = false;
                    btnSys.Enabled = false;
                    btnUty.Enabled = false;
                    btnNPS.Enabled = false;
                    btnGST.Enabled = false;
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuTraining.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuNPS.Visible = false;
                    mnuBCOpr.Visible = false;
                    mnuSME.Visible = false;
                    mnuGST.Visible = false;

                    break;
                case "ConRpt":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuTraining.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuConRpt.Visible = true;
                    mnuNPS.Visible = false;
                    mnuBCOpr.Visible = false;
                    mnuSME.Visible = false;
                    mnuGST.Visible = false;

                    break;
                case "NPS":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuTraining.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuNPS.Visible = true;
                    mnuBCOpr.Visible = false;
                    mnuSME.Visible = false;
                    mnuGST.Visible = false;

                    break;
                case "SME":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuTraining.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuNPS.Visible = false;
                    mnuBCOpr.Visible = false;
                    mnuSME.Visible = true;
                    mnuGST.Visible = false;

                    break;
                case "GST":
                    mnuAct.Visible = false;
                    mnuMst.Visible = false;
                    mnuTrn.Visible = false;
                    mnuTraining.Visible = false;
                    mnuRpt.Visible = false;
                    mnuSys.Visible = false;
                    mnuUty.Visible = false;
                    mnuOpr.Visible = false;
                    mnuConRpt.Visible = false;
                    mnuNPS.Visible = false;
                    mnuBCOpr.Visible = false;
                    mnuSME.Visible = false;
                    mnuGST.Visible = true;

                    break;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAct_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Acct");
            btnAct.Style.Add("background", "#3da1e0");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnTraining.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnNPS.Style.Add("background", "none");
            btnBCOpr.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMst_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Mst");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "#3da1e0");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnTraining.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnNPS.Style.Add("background", "none");
            btnBCOpr.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTrn_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Tran");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "#3da1e0");
            btnTraining.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnNPS.Style.Add("background", "none");
            btnBCOpr.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTraining_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Training");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnTraining.Style.Add("background", "#3da1e0");
            btnSys.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnNPS.Style.Add("background", "none");
            btnBCOpr.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRpt_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Rept");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "#3da1e0");
            btnTrn.Style.Add("background", "none");
            btnTraining.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnNPS.Style.Add("background", "none");
            btnBCOpr.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConRpt_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("ConRpt");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnTraining.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "#3da1e0");
            btnNPS.Style.Add("background", "none");
            btnBCOpr.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBCOpr_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("BCOpr");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnTraining.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnBCOpr.Style.Add("background", "#3da1e0");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnNPS.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSys_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Syst");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnTraining.Style.Add("background", "none");
            btnSys.Style.Add("background", "#3da1e0");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnNPS.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUty_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Utly");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnTraining.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnUty.Style.Add("background", "#3da1e0");
            btnOprtn.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnNPS.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNPS_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("NPS");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnTraining.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnNPS.Style.Add("background", "#3da1e0");
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
            btnTraining.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnNPS.Style.Add("background", "none");
            btnGST.Style.Add("background", "#3da1e0");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOprtn_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Opr");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnTraining.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "#3da1e0");
            btnUty.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnNPS.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAudit_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Audit");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnTraining.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnNPS.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }

        protected void btnSME_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("SME");
            btnSME.Style.Add("background", "#3da1e0");
            btnAct.Style.Add("background", "none");
            btnMst.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnTrn.Style.Add("background", "none");
            btnTraining.Style.Add("background", "none");
            btnSys.Style.Add("background", "none");
            btnUty.Style.Add("background", "none");
            btnOprtn.Style.Add("background", "none");
            btnConRpt.Style.Add("background", "none");
            btnNPS.Style.Add("background", "none");
            btnBCOpr.Style.Add("background", "none");
            btnGST.Style.Add("background", "none");
        }


        #region AccountMenu
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb1_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lb1";
            Response.Redirect("~/WebPages/Private/Master/AcGroup.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb2_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lb2";
            Response.Redirect("~/WebPages/Private/Master/AcSubGrp.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb3_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lb3";
            Response.Redirect("~/WebPages/Private/Master/GenLedDtl.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb4_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lb4";
            Response.Redirect("~/WebPages/Private/Master/AcctOpBal.aspx", false);
        }


        protected void lblCollPointMst_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                Session["MnuId"] = "Acct";
                Session["PaneId"] = acAct.SelectedIndex;
                Session["LinkId"] = "lblCollPointMst";
                Response.Redirect("~/WebPages/Private/Master/CollectionPointMaster.aspx", false);
            }
            gblFuction.AjxMsgPopup("This Module Can only be operated from Head Office");
            return;
        }


        /// <summary>
        /// Receipt and Payment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb5_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lb5";
            Response.RedirectPermanent("~/WebPages/Private/Transaction/VoucherRP.aspx", false);
        }

        /// <summary>
        /// Journal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb6_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lb6";
            Response.RedirectPermanent("~/WebPages/Private/Transaction/VoucherJ.aspx", false);
        }

        /// <summary>
        /// Contra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb7_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lb7";
            Response.Redirect("~/WebPages/Private/Transaction/VoucherC.aspx", false);
        }

        protected void lbBulkJournalPosting_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Acct";
                Session["PaneId"] = acAct.SelectedIndex;
                Session["LinkId"] = "lbBulkJournalPosting";
                Response.Redirect("~/WebPages/Private/Transaction/BulkJournalPosting.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login can not Access this Option.");
                return;
            }

        }

        protected void lbBulkUpload_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Acct";
                Session["PaneId"] = acAct.SelectedIndex;
                Session["LinkId"] = "lbBulkUpload";
                //Response.Redirect("~/WebPages/Private/Transaction/BranchFundTransfer.aspx", false);
                Response.Redirect("~/WebPages/Private/Transaction/BulkUpload.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login can not Access this Option.");
                return;
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSub_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lbSub";
            Response.Redirect("~/WebPages/Private/Master/SubsidiaryLdgr.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbBR_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lbBR";
            Response.Redirect("~/WebPages/Private/Transaction/BankRecon.aspx", false);
        }

        protected void lbCollDeposit_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
            {
                Session["MnuId"] = "Acct";
                Session["PaneId"] = acAct.SelectedIndex;
                Session["LinkId"] = "lbCollDeposit";
                Response.Redirect("~/WebPages/Private/Transaction/CollectionDeposit.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Head Office Login can not Access this Option.");
                return;
            }
        }

        protected void lbCashRecon_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lbCashRecon";
            Response.Redirect("~/WebPages/Private/Transaction/CashRecon.aspx", false);
        }

        protected void lbCashReconApproval_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lbCashReconApproval";
            Response.Redirect("~/WebPages/Private/Transaction/CashReconCheck.aspx", false);
        }

        protected void lblMisUpload_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lblMisUpload";
            Response.Redirect("~/WebPages/Private/Transaction/MisUpload.aspx", false);
        }

        protected void lblReconRptLoWise_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lblReconRptLoWise";
            Response.Redirect("~/WebPages/Private/Transaction/ReconRptLoWise.aspx", false);
        }

        protected void lblReconRptCollPointWise_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lblReconRptCollPointWise";
            Response.Redirect("~/WebPages/Private/Transaction/ReconRptCollPointWise.aspx", false);
        }
        protected void lblReconMIS_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lblReconMIS";
            Response.Redirect("~/WebPages/Private/Report/RptReconMIS.aspx", false);
        }
        protected void lblCashReconRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lblCashReconRpt";
            Response.Redirect("~/WebPages/Private/Report/RptCashRecon.aspx", false);
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
        protected void lbFDDtls_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Acct";
                Session["PaneId"] = acAct.SelectedIndex;
                Session["LinkId"] = "lbFDDtls";
                Response.Redirect("~/WebPages/Private/Report/RptFDDtls.aspx");
            }
            else
            {
                gblFuction.MsgPopup("Branch Login can not Access this Option.");
                return;
            }
        }

        protected void lbBankLnDtls_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Acct";
                Session["PaneId"] = acAct.SelectedIndex;
                Session["LinkId"] = "lbBankLnDtls";
                Response.Redirect("~/WebPages/Private/Report/RptBankLnDtls.aspx");
            }
            else
            {
                gblFuction.MsgPopup("Branch Login can not Access this Option.");
                return;
            }
        }

        protected void lbPettyCash_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lbPettyCash";
            Response.Redirect("~/WebPages/Private/Transaction/PettyCash.aspx", false);
        }

        protected void lbPettyCashReplenish_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Acct";
                Session["PaneId"] = acAct.SelectedIndex;
                Session["LinkId"] = "lbPettyCashReplenish";
                Response.Redirect("~/WebPages/Private/Transaction/PettyCash_Replenish.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login can not Access this Option.");
                return;
            }
        }

        protected void lbPettyCashMst_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Acct";
                Session["PaneId"] = acAct.SelectedIndex;
                Session["LinkId"] = "lbPettyCashMst";
                Response.Redirect("~/WebPages/Private/Transaction/PettyCashMst.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login can not Access this Option.");
                return;
            }
        }

        protected void lbBankerRepay_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Acct";
                Session["PaneId"] = acAct.SelectedIndex;
                Session["LinkId"] = "lbBankerRepay";
                Response.Redirect("~/WebPages/Private/Report/RptBankerLnRepayDtl.aspx");
            }
            else
            {
                gblFuction.MsgPopup("Branch Login can not Access this Option.");
                return;
            }
        }

        protected void lbBankerRepayFuture_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Acct";
                Session["PaneId"] = acAct.SelectedIndex;
                Session["LinkId"] = "lbBankerRepayFuture";
                Response.Redirect("~/WebPages/Private/Report/RptBankerLnFutureRepayDtls.aspx");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbCB_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Acct";
            Session["PaneId"] = acAct.SelectedIndex;
            Session["LinkId"] = "lbCB";
            Response.Redirect("~/WebPages/Private/Transaction/ChqBounce.aspx", false);
        }

        #endregion

        #region MasterMenu
        /// <summary>
        /// Branch Master
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb11_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lb11";
            Response.Redirect("~/WebPages/Private/Master/BranchMst.aspx", false);
        }

        /// <summary>
        /// District Master
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb12_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lb12";
            Response.Redirect("~/WebPages/Private/Master/DistrictMaster.aspx", false);
        }

        /// <summary>
        /// Block Master
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb13_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                Session["MnuId"] = "Mst";
                Session["PaneId"] = acMst.SelectedIndex;
                Session["LinkId"] = "lb13";
                Response.Redirect("~/WebPages/Private/Master/BlockMaster.aspx", false);

            }


            gblFuction.AjxMsgPopup("This Module Can only be operated from Branch.");
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lb14_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "Mst";
        //    Session["PaneId"] = acMst.SelectedIndex;
        //    Session["LinkId"] = "lb14";
        //    Response.Redirect("~/WebPages/Private/Master/TehsilMaster.aspx", false);
        //}

        /// <summary>
        /// Village Master
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb15_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                Session["MnuId"] = "Mst";
                Session["PaneId"] = acMst.SelectedIndex;
                Session["LinkId"] = "lb15";
                Response.Redirect("~/WebPages/Private/Master/VillageMaster.aspx", false);
            }
            gblFuction.AjxMsgPopup("This Module Can only be operated from Branch.");
            return;
        }
        /// <summary>
        /// Village Master
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbActivity_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                Session["MnuId"] = "Mst";
                Session["PaneId"] = acMst.SelectedIndex;
                Session["LinkId"] = "lbActivity";
                Response.Redirect("~/WebPages/Private/Master/ActivityMaster.aspx", false);
            }
            gblFuction.AjxMsgPopup("This Module Can only be operated from Head Office.");
            return;
        }
        /// <summary>
        ///  Work Area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbWArea_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbWArea";
            Response.Redirect("~/WebPages/Private/Master/WorkArea.aspx", false);
        }

        /// <summary>
        /// Loan Application Parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSnParam_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbSnParam";
            Response.Redirect("~/WebPages/Private/Master/LnAppParam.aspx", false);
        }

        /// <summary>
        /// Loan Product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbLnProd_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbLnProd";
            Response.Redirect("~/WebPages/Private/Master/ProductMst.aspx", false);
        }

        /// <summary>
        /// Loan Parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbLnParam_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbLnParam";
            Response.Redirect("~/WebPages/Private/Master/LoanParameter.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbArea_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbArea";
            Response.Redirect("~/WebPages/Private/Master/AreaMaster.aspx", false);
        }

        /// <summary>
        /// GP / Ward  Master
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbGP_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                Session["MnuId"] = "Mst";
                Session["PaneId"] = acMst.SelectedIndex;
                Session["LinkId"] = "lbGP";
                Response.Redirect("~/WebPages/Private/Master/GP.aspx", false);

            }
            gblFuction.AjxMsgPopup("This Module Can only be operated from Branch.");
            return;
        }

        /// <summary>
        /// RegionMaster
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReg_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbReg";
            Response.Redirect("~/WebPages/Private/Master/RegionMaster.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lbMo_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "Mst";
        //    Session["PaneId"] = acMst.SelectedIndex;
        //    Session["LinkId"] = "lbMo";
        //    Response.Redirect("~/WebPages/Private/Master/MohallaMaster.aspx", false);
        //}

        /// <summary>
        /// Holiday Master
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbHldy_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbHldy";
            Response.Redirect("~/WebPages/Private/Master/Holiday.aspx", false);
        }

        protected void lbICMST_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbICMST";
            Response.Redirect("~/WebPages/Private/Transaction/ICMst.aspx", false);
        }

        protected void lbHospiCashMst_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbHospiCashMst";
            Response.Redirect("~/WebPages/Private/Transaction/HospiMst.aspx", false);
        }

        protected void lbBankMst_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbBankMst";
            Response.Redirect("~/WebPages/Private/Master/BankMst.aspx", false);
        }
        protected void lbIfscMst_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                Session["MnuId"] = "Mst";
                Session["PaneId"] = acMst.SelectedIndex;
                Session["LinkId"] = "lbIfscMst";
                Response.Redirect("~/WebPages/Private/Master/IfscMaster.aspx", false);
            }
            gblFuction.AjxMsgPopup("This Module Can only be operated from Head Office");
            return;
        }

        protected void lbCanReasonMst_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                Session["MnuId"] = "Mst";
                Session["PaneId"] = acMst.SelectedIndex;
                Session["LinkId"] = "lbCanReasonMst";
                Response.Redirect("~/WebPages/Private/Master/CancellationReasonMaster.aspx", false);
            }
            gblFuction.AjxMsgPopup("This Module Can only be operated from Head Office");
            return;
        }

        protected void lbNatCapMst_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbNatCapMst";
            Response.Redirect("~/WebPages/Private/Master/NatCatMst.aspx", false);

        }

        protected void lbBusTypeMst_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbBusTypeMst";
            Response.Redirect("~/WebPages/Private/Master/BusinessTypeMaster.aspx", false);
        }

        protected void lbBusSubTypeMst_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbBusSubTypeMst";
            Response.Redirect("~/WebPages/Private/Master/BusinessSubTypeMaster.aspx", false);
        }

        protected void lbBusActivity_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbBusActivity";
            Response.Redirect("~/WebPages/Private/Master/BusinessActivity.aspx", false);
        }

        protected void lbPinMst_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                Session["MnuId"] = "Mst";
                Session["PaneId"] = acMst.SelectedIndex;
                Session["LinkId"] = "lbPinMst";
                Response.Redirect("~/WebPages/Private/Master/PinCodeMaster.aspx", false);
            }
            gblFuction.AjxMsgPopup("This Module Can only be operated from Head Office");
            return;
        }

        protected void lbGenParam_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                Session["MnuId"] = "Mst";
                Session["PaneId"] = acMst.SelectedIndex;
                Session["LinkId"] = "lbGenParam";
                Response.Redirect("~/WebPages/Private/Master/GenralParameter.aspx", false);
            }
            gblFuction.AjxMsgPopup("This Module Can only be operated from Head Office");
            return;
        }

        protected void lbBrCntrlMst_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                Session["MnuId"] = "Mst";
                Session["PaneId"] = acMst.SelectedIndex;
                Session["LinkId"] = "lbBrCntrlMst";
                Response.Redirect("~/WebPages/Private/Transaction/BranchControl.aspx", false);
            }
            else
            {
                gblFuction.AjxMsgPopup("This Module Can only be operated from Head Office");
                return;
            }
        }

        protected void lbRiskPremiumChart_Click(object sender, EventArgs e)
        {
            
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbRiskPremiumChart";
            Response.Redirect("~/WebPages/Private/Master/RiskPremiumChart.aspx", false);
            
        }

        protected void lblPricingSchemeParam_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lblPricingSchemeParam";
            Response.Redirect("~/WebPages/Private/Master/PricingSchemeParam.aspx", false);

        }

        protected void lblPricingSchemeParamApp_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lblPricingSchemeParamApp";
            Response.Redirect("~/WebPages/Private/Master/PricingSchemeParamApp.aspx", false);

        }


        protected void lbDeviceNo_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbDeviceNo";
            Response.Redirect("~/WebPages/Private/Master/DeviceMaster.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lbCrBu_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "Mst";
        //    Session["PaneId"] = acMst.SelectedIndex;
        //    Session["LinkId"] = "lbCrBu";
        //    Response.Redirect("~/WebPages/Private/Master/CreditBureauMaster.aspx", false);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lbCnclRes_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "Mst";
        //    Session["PaneId"] = acMst.SelectedIndex;
        //    Session["LinkId"] = "lbCnclRes";
        //    Response.Redirect("~/WebPages/Private/Master/CancellationReasonMaster.aspx", false);
        //}

        /// <summary>
        /// Loan Wave Off
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbLnWof_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbLnWof";
            Response.Redirect("~/WebPages/Private/Master/WaveOff.aspx", false);
        }
        protected void lbInPolicy_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Mst";
                Session["PaneId"] = acMst.SelectedIndex;
                Session["LinkId"] = "lbInPolicy";
                Response.Redirect("~/WebPages/Private/Master/InsPolicyMaster.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }

        /// <summary>
        /// Loan Scheme
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbLnScm_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbLnScm";
            Response.Redirect("~/WebPages/Private/Master/LoanScheme.aspx", false);
        }

        /// <summary>
        /// Loan Sub Purpose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbLnSup_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbLnSup";
            Response.Redirect("~/WebPages/Private/Master/SubPurpose.aspx", false);
        }

        /// <summary>
        /// Occupation Master
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbOcp_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbOcp";
            Response.Redirect("~/WebPages/Private/Master/OcupMst.aspx", false);
        }
        protected void lbExRn_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbExRn";
            Response.Redirect("~/WebPages/Private/Master/ExitReasonMst.aspx", false);
        }
        protected void lbSubExRn_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbSubExRn";
            Response.Redirect("~/WebPages/Private/Master/SubExitRnMst.aspx", false);
        }

        /// <summary>
        /// Human Relation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbHR_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbHR";
            Response.Redirect("~/WebPages/Private/Master/HRMst.aspx", false);
        }

        /// <summary>
        /// Designation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDsg_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbDsg";
            Response.Redirect("~/WebPages/Private/Master/DesigMst.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDept_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbDept";
            Response.Redirect("~/WebPages/Private/Master/DepartmentMaster.aspx", false);
        }

        /// <summary>
        /// Qualification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbQly_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbQly";
            Response.Redirect("~/WebPages/Private/Master/Qualify.aspx", false);
        }

        /// <summary>
        /// Loan Purpose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbLnPur_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbLnPur";
            Response.Redirect("~/WebPages/Private/Master/PurposeMst.aspx", false);
        }

        /// <summary>
        /// Fund Source
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbFnd_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
                return;
            }
            else
            {
                Session["MnuId"] = "Mst";
                Session["PaneId"] = acMst.SelectedIndex;
                Session["LinkId"] = "lbFnd";
                Response.Redirect("~/WebPages/Private/Master/FundSource.aspx", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPool_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can only be operated from Head Office");
                return;
            }
            Session["MnuId"] = "Mst";
            Session["PaneId"] = acMst.SelectedIndex;
            Session["LinkId"] = "lbPool";
            Response.Redirect("~/WebPages/Private/Master/LnPool.aspx", false);
        }


        #endregion

        #region Operation

        /// <summary>
        /// RO Master
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbRO_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbRO";
            Response.Redirect("~/WebPages/Private/Master/ROMst.aspx", false);
        }


        protected void lblROTrns_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lblROTrns";
            Response.Redirect("~/WebPages/Private/Master/RoTransfer.aspx", false);
        }
        protected void lbEmpKPI_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbEmpKPI";
            Response.Redirect("~/WebPages/Private/Master/EmpKPINew.aspx", false);
        }
        protected void lbQuestMst_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbQuestMst";
            Response.Redirect("~/WebPages/Private/Master/KPIQuestMst.aspx", false);
        }
        protected void lbKPISuper1App_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbKPISuper1App";
            Response.Redirect("~/WebPages/Private/Master/KPISupervisor1App.aspx", false);
        }
        protected void lbKPISuper2App_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbKPISuper2App";
            Response.Redirect("~/WebPages/Private/Master/KPISupervisor2App.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbCent_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbCent";
            Response.Redirect("~/WebPages/Private/Master/CentMst.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbGrpM_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbGrpM";
            Response.Redirect("~/WebPages/Private/Master/Group.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbInitAppEdit_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbInitAppEdit";
            Response.Redirect("~/WebPages/Private/Master/InitialApproachEdit.aspx", false);
        }
        protected void lbInitAppApprove_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbInitAppApprove";
            Response.Redirect("~/WebPages/Private/Master/InitialApproachApprove.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbMem_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbMem";
            Response.Redirect("~/WebPages/Private/Master/Member.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbNewMem_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
            {
                Session["MnuId"] = "Opr";
                Session["PaneId"] = acOpr.SelectedIndex;
                Session["LinkId"] = "lbNewMem";
                Response.Redirect("~/WebPages/Private/Master/NewMember.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Head office Login cannot Open This ");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbExistMem_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
            {
                Session["MnuId"] = "Opr";
                Session["PaneId"] = acOpr.SelectedIndex;
                Session["LinkId"] = "lbExistMem";
                Response.Redirect("~/WebPages/Private/Master/ExistingMember.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Head office Login cannot Open This ");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbColRu_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbColRu";
            Response.Redirect("~/WebPages/Private/Master/ColRoutineNew.aspx", false);
        }

        protected void lbMemRedFlag_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbMemRedFlag";
            Response.Redirect("~/WebPages/Private/Transaction/MemberRedFlag.aspx", false);
        }

        protected void lbReLoanCbCheck_Click(object sender, EventArgs e)
        {
            //if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            //{

            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbReLoanCbCheck";
            Response.Redirect("~/WebPages/Private/Transaction/ReLoanCbCheck.aspx", false);
            //}
            //else
            //{
            //    gblFuction.MsgPopup("Branch Login cannot Open This ");
            //}
        }

        protected void lbFestivLoan_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbFestivLoan";
            Response.Redirect("~/WebPages/Private/Transaction/FestivalLoan.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAtten_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbAtten";
            Response.Redirect("~/WebPages/Private/Master/Attendance.aspx", false);

        }


        #endregion

        #region TransactionMenu


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbCGT_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbCGT";
            Response.Redirect("~/WebPages/Private/Transaction/CGTTrans.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbGRT_Click(object sender, EventArgs e)
        {
            string time = vAccessTime.Substring(11, 5);
            if (TimeChk() == true)
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbGRT";
                Response.Redirect("~/WebPages/Private/Transaction/MemberGRT.aspx", false);
            }
            else
            {
                gblFuction.AjxMsgPopup("After" + " " + time + " " + "cannot open this page...");
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbHVisit_Click(object sender, EventArgs e)
        {
            string time = vAccessTime.Substring(11, 5);
            if (TimeChk() == true)
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbHVisit";
                Response.Redirect("~/WebPages/Private/Transaction/HouseVisitNew.aspx", false);
            }
            else
            {
                gblFuction.AjxMsgPopup("After" + " " + time + " " + "cannot open this page...");
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbJocataApproval_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbJocataApproval";
            Response.Redirect("~/WebPages/Private/Transaction/JocataApproval.aspx", false);
        }

        protected void lbKarzaRetrigger_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbKarzaRetrigger";
            Response.Redirect("~/WebPages/Private/Transaction/KarzaReTrigger.aspx", false);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb24_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lb24";
            Response.Redirect("~/WebPages/Private/Transaction/LoanAppl.aspx", false);
            //Response.Redirect("~/WebPages/Private/Transaction/LoanApp.aspx", false);
        }
        protected void LBVeri_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opr";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "LBVeri";
            Response.Redirect("~/WebPages/Private/Master/DocVerification.aspx", false);
            //Response.Redirect("~/WebPages/Private/Transaction/LoanApp.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb25_Click(object sender, EventArgs e)
        {
            string time = vAccessTime.Substring(11, 5);
            if (TimeChk() == true)
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lb25";
                Response.Redirect("~/WebPages/Private/Transaction/LoanSancn.aspx", false);
            }
            else
            {
                gblFuction.AjxMsgPopup("After" + " " + time + " " + "cannot open this page...");
                return;
            }
        }

        protected void lbEquifaxMember_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbEquifaxMember";
            Response.Redirect("~/WebPages/Private/Transaction/EquifaxChking.aspx", false);
        }

        protected void lbBrNEFT_Click(object sender, EventArgs e)
        {
            string time = vAccessTime.Substring(11, 5);
            if (TimeChk() == true)
            {
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    gblFuction.AjxMsgPopup("HO cannot open this page...");
                    return;
                }
                else
                {
                    Session["MnuId"] = "Tran";
                    Session["PaneId"] = acTrn.SelectedIndex;
                    Session["LinkId"] = "lbBrNEFT";
                    Response.Redirect("~/WebPages/Private/Transaction/BrNEFTApprove.aspx", false);
                }
            }
            else
            {
                gblFuction.AjxMsgPopup("After" + " " + time + " " + "cannot open this page...");
                return;
            }
        }
        protected void lbDocPrint_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.AjxMsgPopup("HO cannot open this page...");
                return;
            }
            else
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbDocPrint";
                Response.Redirect("~/WebPages/Private/Transaction/DocPrinting.aspx", false);
            }
        }
        protected void lbDigitalDocPrint_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.AjxMsgPopup("HO cannot open this page...");
                return;
            }
            else
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbDigitalDocPrint";
                Response.Redirect("~/WebPages/Private/Transaction/DigitalDocPrinting.aspx", false);
            }
        }
        protected void lbDigiDocManualUpload_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.AjxMsgPopup("HO cannot open this page...");
                return;
            }
            else
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbDigiDocManualUpload";
                Response.Redirect("~/WebPages/Private/Transaction/DigiDocManualUpload.aspx", false);
            }
        }


        protected void lbDigitalConsent_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbDigitalConsent";
            Response.Redirect("~/WebPages/Private/Report/DigitalConsentForm.aspx", false);

        }

        protected void lbMemVerfy_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("Branch cannot open this page...");
                return;
            }
            else
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbMemVerfy";
                Response.Redirect("~/WebPages/Private/Transaction/MemberVerification.aspx", false);
            }
        }
        protected void lbHoNEFT_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("Branch cannot open this page...");
                return;
            }
            else
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbHoNEFT";
                Response.Redirect("~/WebPages/Private/Transaction/HoNEFTAppr.aspx", false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbHoNeftDisbAPI_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbHoNeftDisbAPI";
                Response.Redirect("~/WebPages/Private/Transaction/HONEFTAPITransfer.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }
        protected void lbHoNEFTCancel_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("Branch cannot open this page...");
                return;
            }
            else
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbHoNEFTCancel";
                Response.Redirect("~/WebPages/Private/Transaction/HoNEFTApprCancel.aspx", false);
            }
        }
        //protected void lbSncCan_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "Tran";
        //    Session["PaneId"] = acTrn.SelectedIndex;
        //    Session["LinkId"] = "lbSncCan";
        //    Response.Redirect("~/WebPages/Private/Transaction/LoanSancCancel.aspx", false);
        //}
        protected void lbHOSncCan_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbHOSncCan";
                Response.Redirect("~/WebPages/Private/Transaction/HOLoanSancCancel.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }

        protected void lbHoNeftDisb_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbHoNeftDisb";
                Response.Redirect("~/WebPages/Private/Transaction/HONEFTDisbursement.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }
        protected void lbHoNeftCanclDisb_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbHoNeftCanclDisb";
                Response.Redirect("~/WebPages/Private/Transaction/CancelNEFTDisbursement.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb26_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lb26";
            Response.Redirect("~/WebPages/Private/Transaction/LoanDisbGrp.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDisbIn_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbDisbIn";
            Response.Redirect("~/WebPages/Private/Transaction/LoanDisbIndiv.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb27_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lb27";
            Response.Redirect("~/WebPages/Private/Transaction/LoanRecovry.aspx", false);
        }

        protected void lbInstallCollBulk_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbInstallCollBulk";
                Response.Redirect("~/WebPages/Private/Transaction/LoanRecovryBulk.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This.");
            }
        }


        protected void lbPAI_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbPAI";
            Response.Redirect("~/WebPages/Private/Transaction/PAI.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb28_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lb28";
            Response.Redirect("~/WebPages/Private/Transaction/HHSurvey.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb29_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lb29";
            Response.Redirect("~/WebPages/Private/Transaction/ShgGrade.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb30_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can only be operated from Head Office");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lb30";
            Response.Redirect("~/WebPages/Private/Transaction/Woffdecl.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbWOffRpt_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can only be operated from Head Office");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbWOffRpt";
            Response.Redirect("~/WebPages/Private/Report/LnWriteOff.aspx", false);
        }

        protected void lbMonitoring_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can only be operated from Branch");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbMonitoring";
            Response.Redirect("~/WebPages/Private/Transaction/Monitoring.aspx", false);
        }

        protected void lbMonitoringOth_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can only be operated from Branch");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbMonitoringOth";
            Response.Redirect("~/WebPages/Private/Transaction/MonitoringOther.aspx", false);
        }

        protected void lbllbMonitoringOD_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can only be operated from Branch");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbllbMonitoringOD";
            Response.Redirect("~/WebPages/Private/Transaction/MonitoringOD.aspx", false);
        }

        protected void lbMonitoringCompliance_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can only be operated from Branch");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbMonitoringCompliance";
            Response.Redirect("~/WebPages/Private/Transaction/MonitoringCompliance.aspx", false);
        }

        protected void lbMonitoringOthCompliance_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can only be operated from Branch");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbMonitoringOthCompliance";
            Response.Redirect("~/WebPages/Private/Transaction/MonitoringOtherCompliance.aspx", false);
        }


        protected void lbPCash_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbPCash";
            Response.Redirect("~/WebPages/Private/Transaction/PettyCashReq.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbEquiImport_Click(object sender, EventArgs e)
        {
            //if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            //{
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbEquiImport";
            Response.Redirect("~/WebPages/Private/Transaction/ImportEquifaxData.aspx", false);
            //}
            //else
            //{
            //    gblFuction.MsgPopup("Branch Login cannot Open This ");
            //}

        }

        protected void lbIDBIImport_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbIDBIImport";
                Response.Redirect("~/WebPages/Private/Transaction/ImportIDBIData.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }

        }

        protected void lbCBApproveReject_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbCBApproveReject";
                Response.Redirect("~/WebPages/Private/Transaction/CreditbureauApproveReject.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        protected void lbImportPolicyNo_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbImportPolicyNo";
                Response.Redirect("~/WebPages/Private/Transaction/ImportPolicyNo.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        protected void lbIntBrFndTr_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbIntBrFndTr";
                Response.Redirect("~/WebPages/Private/Transaction/FundTranserBranchtoBranch.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }


        protected void lbIntBrFndTrBr_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbIntBrFndTrBr";
                Response.Redirect("~/WebPages/Private/Transaction/InterBranchFundTransfer.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("HO Login cannot Open This ");
            }
        }

        protected void lbBrFndTr_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbBrFndTr";
                Response.Redirect("~/WebPages/Private/Transaction/BranchFundTr.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }

        protected void lbHoBrFndTrChecker_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbHoBrFndTrChecker";
                Response.Redirect("~/WebPages/Private/Transaction/BranchFundTrChecker.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbCFtrf_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbCFtrf";
                Response.Redirect("~/WebPages/Private/Transaction/ClientFundTransfer.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void lbInsPay_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbInsPay";
            Response.Redirect("~/WebPages/Private/Transaction/InsAmtPayMem.aspx", false);
        }

        protected void lbInsRec_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbInsRec";
            Response.Redirect("~/WebPages/Private/Transaction/InsuranceRecMem.aspx", false);
        }

        protected void lbNEFTDisb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbNEFTDisb";
            Response.Redirect("~/WebPages/Private/Transaction/NEFTDisbApprvl.aspx", false);
        }
        protected void lbNEFTTransfer_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbNEFTTransfer";
            Response.Redirect("~/WebPages/Private/Transaction/NEFTTransfer.aspx", false);
        }
        protected void lbNEFTDisbApprovHO_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbNEFTDisbApprovHO";
            Response.Redirect("~/WebPages/Private/Transaction/NEFTDisburseApprovalHO.aspx", false);
        }
        protected void lbCanDisb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbCanDisb";
            Response.Redirect("~/WebPages/Private/Transaction/CancelNEFTDisbursement.aspx", false);
        }
        protected void lbREtExIDBI_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbREtExIDBI";
            Response.Redirect("~/WebPages/Private/Transaction/RetExIDBI.aspx", false);
        }
        protected void lbREtExIDBIBr_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbREtExIDBIBr";
            Response.Redirect("~/WebPages/Private/Transaction/REtExIDBIBr.aspx", false);
        }
        protected void lbMedCL_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbMedCL";
            Response.Redirect("~/WebPages/Private/Transaction/MedClaim.aspx", false);
        }
        protected void lbRescheduleByLoanNo_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbRescheduleByLoanNo";
            Response.Redirect("~/WebPages/Private/Transaction/RescheduleByLoanNo.aspx", false);
        }
        protected void lbLoanRecovaryAdj_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbLoanRecovaryAdj";
            Response.Redirect("~/WebPages/Private/Transaction/LoanRecovaryAdjastment.aspx", false);
        }
        protected void lbLoanLUC_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbLoanLUC";
            Response.Redirect("~/WebPages/Private/Transaction/LoanUtilCheck.aspx", false);
        }
        protected void lbPreMatApp_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can not be operated from Head Office");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbPreMatApp";
            Response.Redirect("~/WebPages/Private/Transaction/PreMatCloseApproval.aspx", false);
        }

        protected void lbEliAmtCng_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can not be operated from Branch");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbEliAmtCng";
            Response.Redirect("~/WebPages/Private/Master/EligibleAmountUpdate.aspx", false);
        }

        protected void lbActivityApproval_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbActivityApproval";
            Response.Redirect("~/WebPages/Private/Transaction/ActivityApproval.aspx", false);
        }

        protected void lbRiskCatChng_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbRiskCatChng";
            Response.Redirect("~/WebPages/Private/Transaction/RiskCatChngNew.aspx", false);
        }

        protected void lbAdvIntPayToMem_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can not be operated from Branch");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbAdvIntPayToMem";
            Response.Redirect("~/WebPages/Private/Transaction/AdvIntPayToMem.aspx", false);
        }

        protected void lbUdyamUploadDownload_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can not be operated from Branch");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbUdyamUploadDownload";
            Response.Redirect("~/WebPages/Private/Transaction/UdyamAadharUploadDownload.aspx", false);
        }

        protected void lbIRACDupDataRec_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can not be operated from Branch");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbIRACDupDataRec";
            Response.Redirect("~/WebPages/Private/Transaction/IRACDupDataRectify.aspx", false);
        }

        protected void lbMultiUpdateUcic_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can not be operated from Branch");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbMultiUpdateUcic";
            Response.Redirect("~/WebPages/Private/Transaction/UpdateUCIC.aspx", false);
        }

        protected void lbRecPoolUplBulk_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can not be operated from Branch");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbRecPoolUplBulk";
            Response.Redirect("~/WebPages/Private/Transaction/RecoveryPoolUpload.aspx", false);
        }

        protected void lbInsuNeftApi_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can not be operated from Branch");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbInsuNeftApi";
            Response.Redirect("~/WebPages/Private/Transaction/InsuranceNeftApiTransfer.aspx", false);
        }


        protected void lbBankDetailsDeathMem_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbBankDetailsDeathMem";
            Response.Redirect("~/WebPages/Private/Transaction/BankDetailsOfDeathMember.aspx", false);
        }
        protected void lbPDD_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbPDD";
            Response.Redirect("~/WebPages/Private/Transaction/DeathFlaging.aspx", false);
        }
        protected void lbDDecCLoan_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbDDecCLoan";
            Response.Redirect("~/WebPages/Private/Transaction/DeathDeclare.aspx", false);
        }

        protected void lbHospiClaim_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbHospiClaim";
            Response.Redirect("~/WebPages/Private/Transaction/HospiCashClaim.aspx", false);
        }
        protected void lbHospDownCertificate_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can not be operated from Branch");
                return;
            }
            else
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbHospDownCertificate";
                Response.Redirect("~/WebPages/Private/Transaction/HospiCashClaimHoApproval.aspx", false);
            }
        }

        protected void lbDthDocSnt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbDthDocSnt";
            Response.Redirect("~/WebPages/Private/Transaction/DeathDecDocSent.aspx", false);
        }
        protected void lbDthDocHoRcv_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {

                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbDthDocHoRcv";
                Response.Redirect("~/WebPages/Private/Transaction/DthDocRcvByHO.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }
        protected void lbDDRcvCnclIns_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {

                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbDDRcvCnclIns";
                Response.Redirect("~/WebPages/Private/Transaction/DDRcvCnclByIns.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }
        protected void lbDthDocCancl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbDthDocCancl";
            Response.Redirect("~/WebPages/Private/Transaction/DeathCancelAfterVerification.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbLvApp_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbLvApp";
            Response.Redirect("~/WebPages/Private/Transaction/LeaveApplication.aspx", false);
        }


        protected void lbLvSnc_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbLvSnc";
            Response.Redirect("~/WebPages/Private/Transaction/LeaveSanction.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lbAuditPlan_Click(object sender, EventArgs e)
        //{
        //    if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
        //    {
        //        Session["MnuId"] = "Tran";
        //        Session["PaneId"] = acTrn.SelectedIndex;
        //        Session["LinkId"] = "lbAuditPlan";
        //        Response.Redirect("~/WebPages/Private/Transaction/AuditTrail.aspx");
        //    }
        //    else
        //    {
        //        gblFuction.MsgPopup("Branch Login cannot Open This ");
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbInsPlan_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbInsPlan";
                Response.Redirect("~/WebPages/Private/Transaction/IntInsPlan.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbOnlSub_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbOnlSub";
            Response.Redirect("~/WebPages/Private/Transaction/AuditSubmission.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbIntPM_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbIntPM";
                Response.Redirect("~/WebPages/Private/Transaction/IntInspPM.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbIntRR_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbIntRR";
                Response.Redirect("~/WebPages/Private/Transaction/IntInspRR.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbIntLUC_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbIntLUC";
                Response.Redirect("~/WebPages/Private/Transaction/IntInspLUC.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbIntPDC_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbIntPDC";
                Response.Redirect("~/WebPages/Private/Transaction/IntInspPDC.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }
        protected void lbFundTrBank_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbFundTrBank";
                Response.Redirect("~/WebPages/Private/Transaction/InterBrFundtrBank.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("HO Login cannot Open This ");
            }
        }

        //protected void lbICMST_Click(object sender, EventArgs e)
        //{
        //    if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
        //    {
        //        Session["MnuId"] = "Tran";
        //        Session["PaneId"] = acTrn.SelectedIndex;
        //        Session["LinkId"] = "lbICMST";
        //        Response.Redirect("~/WebPages/Private/Transaction/ICMst.aspx", false);
        //    }
        //    else
        //    {
        //        gblFuction.MsgPopup("Branch Login cannot Open This ");
        //    }
        //}




        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbBrCmpl_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbBrCmpl";
                Response.Redirect("~/WebPages/Private/Transaction/BrCmplPM.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login can Open This.");
            }
        }

        /// <summary>
        /// Inventory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbVend_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbVend";
                Response.Redirect("~/WebPages/Private/Inventory/VendMst.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbItem_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbItem";
                Response.Redirect("~/WebPages/Private/Inventory/Items.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbItmOp_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbItmOp";
                Response.Redirect("~/WebPages/Private/Inventory/ItmOpen.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbItmRec_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbItmRec";
                Response.Redirect("~/WebPages/Private/Inventory/ItmRecv.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbHoArea_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbHoArea";
                Response.Redirect("~/WebPages/Private/Inventory/HoToArea.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbHoBr_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbHoBr";
                Response.Redirect("~/WebPages/Private/Inventory/HoToBrnch.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbArBr_Click(object sender, EventArgs e)
        {
            //if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            //{
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbArBr";
            Response.Redirect("~/WebPages/Private/Inventory/AreaToBranch.aspx", false);
            //}
            //else
            //{
            //    gblFuction.MsgPopup("Branch Login cannot Open This ");
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbBrBr_Click(object sender, EventArgs e)
        {
            //if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            //{
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbBrBr";
            Response.Redirect("~/WebPages/Private/Inventory/BranchToBranch.aspx", false);
            //}
            //else
            //{
            //    gblFuction.MsgPopup("Branch Login cannot Open This ");
            //}
        }


        protected void lbBrSt_Click(object sender, EventArgs e)
        {
            //if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            //{
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbBrSt";
            Response.Redirect("~/WebPages/Private/Inventory/BranchToStaff.aspx", false);
            //}
            //else
            //{
            //    gblFuction.MsgPopup("Branch Login cannot Open This ");
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lbBrBr_Click(object sender, EventArgs e)
        //{
        //    if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
        //    {
        //        Session["MnuId"] = "Tran";
        //        Session["PaneId"] = acTrn.SelectedIndex;
        //        Session["LinkId"] = "lbBrBr";
        //        Response.Redirect("~/WebPages/Private/Inventory/HoToBrnch.aspx");
        //    }
        //    else
        //    {
        //        gblFuction.MsgPopup("Branch Login cannot Open This ");
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbRVHoArea_Click(object sender, EventArgs e)
        {
            //if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            //{
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbRVHoArea";
            Response.Redirect("~/WebPages/Private/Inventory/RVHoToArea.aspx", false);
            //}
            //else
            //{
            //    gblFuction.MsgPopup("Branch Login cannot Open This ");
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbRVHoToBr_Click(object sender, EventArgs e)
        {
            //if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            //{
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbRVHoToBr";
            Response.Redirect("~/WebPages/Private/Inventory/RVHoToBr.aspx", false);
            //}
            //else
            //{
            //    gblFuction.MsgPopup("Branch Login cannot Open This ");
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbRVArToBr_Click(object sender, EventArgs e)
        {
            //if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            //{
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbRVArToBr";
            Response.Redirect("~/WebPages/Private/Inventory/RVArToBr.aspx", false);
            //}
            //else
            //{
            //    gblFuction.MsgPopup("Branch Login cannot Open This ");
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbRVBrToBr_Click(object sender, EventArgs e)
        {
            //if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            //{
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbRVBrToBr";
            Response.Redirect("~/WebPages/Private/Inventory/RVBrToBr.aspx", false);
            //}
            //else
            //{
            //    gblFuction.MsgPopup("Branch Login cannot Open This ");
            //}
        }

        protected void lbInOutLtr_Click(object sender, EventArgs e)
        {
            //if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            //{
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbInOutLtr";
            Response.Redirect("~/WebPages/Private/Inventory/InOutLtr.aspx", false);
            //}
            //else
            //{
            //    gblFuction.MsgPopup("Branch Login cannot Open This ");
            //}
        }
        protected void lbStockSummVeri_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbStockSummVeri";
            Response.Redirect("~/WebPages/Private/Inventory/StockSummVeri.aspx", false);
        }
        protected void lbStckDam_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbStckDam";
            Response.Redirect("~/WebPages/Private/Inventory/StckDamage.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSecRef_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbSecRef";
            Response.Redirect("~/WebPages/Private/Transaction/SecurityRefund.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSrcFnd_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                gblFuction.MsgPopup("Head Office Login cannot Open This ");
                return;
            }
            else
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lbSrcFnd";
                Response.Redirect("~/WebPages/Private/Transaction/EditFundSource.aspx", false);
            }
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
                Session["LinkId"] = "lbSfBulkUpload";
                Response.Redirect("~/WebPages/Private/Transaction/FundSourceUpload.aspx", false);
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
                Response.Redirect("~/WebPages/Private/Transaction/FundSourceUploadApproval.aspx", false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSrcSubPurpose_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbSrcSubPurpose";
            Response.Redirect("~/WebPages/Private/Transaction/EditSubPurpose.aspx", false);
        }

        protected void lbDeviationMat_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbDeviationMat";
            Response.Redirect("~/WebPages/Private/Transaction/DeviationMatrix.aspx", false);
        }

        protected void lbDeviationMatApp_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbDeviationMatApp";
            Response.Redirect("~/WebPages/Private/Transaction/DeviationMatrixApproval.aspx", false);
        }

        protected void lbOpenBucket_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbOpenBucket";
            Response.Redirect("~/WebPages/Private/Transaction/OpenBucket.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblFA_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lblFA";
                Response.Redirect("~/WebPages/Private/Transaction/FundAllocation.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblRtoD_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                Session["MnuId"] = "Tran";
                Session["PaneId"] = acTrn.SelectedIndex;
                Session["LinkId"] = "lblRtoD";
                Response.Redirect("~/WebPages/Private/Transaction/GenerateReadytoDisb.aspx", false);
            }
            else
            {
                gblFuction.MsgPopup("Branch Login cannot Open This ");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbInsPol_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbInsPol";
            Response.Redirect("~/WebPages/Private/Transaction/EditLoanInsure.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPoolTag_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can only be operated from Head Office");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbPoolTag";
            Response.Redirect("~/WebPages/Private/Transaction/PoolTag.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lbInvLnRes_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "Tran";
        //    Session["PaneId"] = acTrn.SelectedIndex;
        //    Session["LinkId"] = "lbInvLnRes";
        //    Response.Redirect("~/WebPages/Private/Transaction/LnReschedule.aspx", false);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbGrMt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbGrMt";
            Response.Redirect("~/WebPages/Private/Transaction/MonthlyGroupMeetDayChng.aspx", false);
        }

        protected void lnkCollTimeCng_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lnkCollTimeCng";
            Response.Redirect("~/WebPages/Private/Transaction/CollTimeChange.aspx", false);
        }

        protected void lbWeeklyGrMt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbWeeklyGrMt";
            Response.Redirect("~/WebPages/Private/Transaction/GroupMeetDay.aspx", false);
        }

        #endregion

        # region Training

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbModuleMst_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Training";
            Session["PaneId"] = acTraining.SelectedIndex;
            Session["LinkId"] = "lbModuleMst";
            Response.Redirect("~/WebPages/Private/Master/TrainingMaster.aspx", false);
            //gblFuction.MsgPopup("The page is Under Construction");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbTrainingSchedule_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Training";
            Session["PaneId"] = acTraining.SelectedIndex;
            Session["LinkId"] = "lbTrainingSchedule";
            Response.Redirect("~/WebPages/Private/Master/TrainingSchedule.aspx", false);
            //gblFuction.MsgPopup("The page is Under Construction");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbTrainerPlan_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Training";
            Session["PaneId"] = acTraining.SelectedIndex;
            Session["LinkId"] = "lbTrainerPlan";
            Response.Redirect("~/WebPages/Private/Transaction/TrainerModulePlan.aspx", false);
            //gblFuction.MsgPopup("The page is Under Construction");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbTrainerModuleActual_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Training";
            Session["PaneId"] = acTraining.SelectedIndex;
            Session["LinkId"] = "lbTrainerModuleActual";
            Response.Redirect("~/WebPages/Private/Transaction/TrainerModuleActual.aspx", false);
            //gblFuction.MsgPopup("The page is Under Construction");
        }


        #endregion

        #region ReportMenu

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbLedgr_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLedgr";
            //Response.Redirect("~/WebPages/Private/Report/CashBook.aspx");
            DataTable dt = null;
            //ReportDocument rptDoc = new ReportDocument();
            //string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\Ledger.rpt";
            CReports oRpt = new CReports();
            dt = oRpt.rptLedger();
            //rptDoc.Load(vRptPath);
            //rptDoc.SetDataSource(dt);
            //rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Ledger List");

            string vFileNm = "attachment;filename=Ledger List.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
            Response.Write("<table border='1' cellpadding='5' widht='120%'>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>LEDGER LIST</font></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'></font></b></td></tr>");
            Response.Write("<tr><td><b>Accounts Head</b></td><td><b>Accounts Group</b></td><td><b>Accounts Sub Group</b></td><td><b>Description</b></td><td><b>Ledger Code</b></td><td><b>Short Name</b></td><td><b>Oracle GL Code</b></td><td><b>Oracle GL Name</b></td><td><b>GL Status</b></td></tr>");
            string tab = string.Empty;
            foreach (DataRow dtrow in dt.Rows)
            {
                Response.Write("<tr style='height:20px;'>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</table>");
            Response.End();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lbEmpRpt_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "Rept";
        //    Session["PaneId"] = acRpt.SelectedIndex;
        //    Session["LinkId"] = "lbEmpRpt";
        //    DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //    string vBrCode = Session[gblValue.BrnchCode].ToString();
        //    DataTable dt = null;
        //    ReportDocument rptDoc = new ReportDocument();
        //    string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\Emp.rpt";
        //    CReports oRpt = new CReports();
        //    dt = oRpt.rptEmp(vLogDt);
        //    rptDoc.Load(vRptPath);
        //    rptDoc.SetDataSource(dt);
        //    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
        //    rptDoc.SetParameterValue("pBrCode", vBrCode);
        //    rptDoc.SetParameterValue("pTitle", "Employee Report");
        //    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Employee Report");
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lbGrpRpt_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "Rept";
        //    Session["PaneId"] = acRpt.SelectedIndex;
        //    Session["LinkId"] = "lbGrpRpt";
        //    Response.Redirect("~/WebPages/Private/Report/rptGroupMst.aspx", false);
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lbGrSchRpt_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "Rept";
        //    Session["PaneId"] = acRpt.SelectedIndex;
        //    Session["LinkId"] = "lbGrSchRpt";
        //    Response.Redirect("~/WebPages/Private/Report/rptGroupMeeting.aspx", false);
        //}
        protected void lbMembershipRegd_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbMemRegd";
            Response.Redirect("~/WebPages/Private/Report/rptMembershipRegd.aspx", false);
        }

        protected void lbNEFTCancel_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbNEFTCancel";
            Response.Redirect("~/WebPages/Private/Report/rptNEFTCancel.aspx", false);
        }
        protected void lbRptAtten_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptAtten";
            Response.Redirect("~/WebPages/Private/Report/rptAttendanceDtl.aspx", false);
        }
        protected void lbNEFTCancelDisb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbNEFTCancelDisb";
            Response.Redirect("~/WebPages/Private/Report/rptNEFTCancel.aspx", false);
        }
        protected void lbInterestAccrual_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbInterestAccrual";
            Response.Redirect("~/WebPages/Private/Report/AccruedInterest.aspx", false);
        }

        protected void lbCustomerDtl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbCustomerDtl";
            Response.Redirect("~/WebPages/Private/Report/CustomerDetails.aspx", false);
        }

        protected void lbLoanAppl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLoanAppl";
            Response.Redirect("~/WebPages/Private/Report/rptLoanApplication.aspx", false);
        }

        protected void lbRptPrtfolio_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptPrtfolio";
            Response.Redirect("~/WebPages/Private/Report/rptPortFolio.aspx", false);
        }

        protected void lbRptPar_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptPar";
            Response.Redirect("~/WebPages/Private/Report/rptPAR.aspx", false);
        }

        protected void lbRptFunder_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptFunder";
            Response.Redirect("~/WebPages/Private/Report/rptFunder.aspx", false);
        }

        protected void lbRptRunDown_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptRunDown";
            Response.Redirect("~/WebPages/Private/Report/rptRunDown.aspx", false);
        }


        protected void lbCollAttBr_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbCollAttBr";
            Response.Redirect("~/WebPages/Private/Report/rptCollAttendance.aspx", false);
        }

        protected void lbPerformanceRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbPerformanceRpt";
            Response.Redirect("~/WebPages/Private/Report/rptPerformance.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lbGPBlockVillRpt_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "Rept";
        //    Session["PaneId"] = acRpt.SelectedIndex;
        //    Session["LinkId"] = "lbGPBlockVillRpt";
        //    Response.Redirect("~/WebPages/Private/Report/rptGPBlockVill.aspx", false);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lbLnPurpRpt_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "Rept";
        //    Session["PaneId"] = acRpt.SelectedIndex;
        //    Session["LinkId"] = "lbLnPurpRpt";
        //    Response.Redirect("~/WebPages/Private/Report/rptLoanPurposeList.aspx", false);
        //}
        protected void lbIniApprStatus_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbIniApprStatus";
            Response.Redirect("~/WebPages/Private/Report/InitialApproachStatusRpt.aspx", false);
        }

        protected void lbLnSncRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLnSncRpt";
            Response.Redirect("~/WebPages/Private/Report/LoanSanctionRpt.aspx", false);
        }

        protected void lbMemVerifyRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbMemVerifyRpt";
            Response.Redirect("~/WebPages/Private/HOReports/MemberVerifyRpt.aspx", false);
        }
        protected void lbLnDisbList_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLnDisbList";
            Response.Redirect("~/WebPages/Private/Report/LoanDisbursement.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbColDisb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbColDisb";
            Response.Redirect("~/WebPages/Private/Report/DemandSheet.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbRepay_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRepay";
            Response.Redirect("~/WebPages/Private/Report/RepaymentSche.aspx", false);
        }

        protected void lbStatOfAc_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbStatOfAc";
            Response.Redirect("~/WebPages/Private/Report/rptStatementOfAccount.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAgeW_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbAgeW";
            Response.Redirect("~/WebPages/Private/Report/AgeWise.aspx", false);
        }

        protected void lbDmdCol_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbDmdCol";
            Response.Redirect("~/WebPages/Private/Report/DmndColl.aspx", false);
        }

        protected void lbRptDmdColHo_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptDmdColHo";
            Response.Redirect("~/WebPages/Private/Report/DmndColl.aspx", false);
        }
        protected void lbRptAreaMapping_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptAreaMapping";
            Response.Redirect("~/WebPages/Private/Report/RptAreaMapping.aspx", false);
        }

        protected void lbBCCollInput_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbBCCollInput";
            Response.Redirect("~/WebPages/Private/HOReports/BCCollectionInput.aspx", false);
        }

        protected void lbBcLoanDisb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbBcLoanDisb";
            Response.Redirect("~/WebPages/Private/HOReports/BCLoanDisburse.aspx", false);
        }

        protected void lbBCNewMember_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbBCNewMember";
            Response.Redirect("~/WebPages/Private/HOReports/BCNewMember.aspx", false);
        }

        protected void lbBCSanctionMIS_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbBCSanctionMIS";
            Response.Redirect("~/WebPages/Private/HOReports/BCSanctionMIS.aspx", false);
        }
        protected void lbInsuClaim_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbInsuClaim";
            Response.Redirect("~/WebPages/Private/Report/rptInsuClaim.aspx", false);
        }

        protected void lbHospiCashClaim_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHospiCashClaim";
            Response.Redirect("~/WebPages/Private/Report/RptHospiCashClaim.aspx", false);
        }

        protected void lbCollEffiRpt_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbCollEffiRpt";
            Response.Redirect("~/WebPages/Private/HOReports/RptCollectionEfficiency.aspx", false);
        }
        protected void lbReloanCbChkRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbReloanCbChkRpt";
            Response.Redirect("~/WebPages/Private/Report/RptReLoanCBChk.aspx", false);
        }

        protected void lbDemandCollStat_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbDemandCollStat";
            Response.Redirect("~/WebPages/Private/Report/DmdCollStatus.aspx", false);
        }

        protected void lbFeCol_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbFeCol";
            Response.Redirect("~/WebPages/Private/Report/FeesCollection.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbColRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbColRpt";
            Response.Redirect("~/WebPages/Private/Report/CollectionRpt.aspx", false);
        }

        protected void lblLoanColl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lblLoanColl";
            Response.Redirect("~/WebPages/Private/Report/RptLoanCollection.aspx", false);
        }

        protected void lbNDC_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbNDC";
            Response.Redirect("~/WebPages/Private/Report/rptNDC.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbWOffColRpt_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can only be operated from Head Office");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbWOffColRpt";
            Response.Redirect("~/WebPages/Private/Report/WOffCollectionRpt.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbParty_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbParty";
            Response.Redirect("~/WebPages/Private/Report/PartyLedger.aspx", false);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbHOUpLnCls_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOUpLnCls";
            Response.Redirect("~/WebPages/Private/HOReports/HOUpcomingClsLoan.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbHOParty_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOParty";
            Response.Redirect("~/WebPages/Private/Report/PartyLedger.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblPoolRpr_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.UserId]) != 13 && Convert.ToInt32(Session[gblValue.UserId]) != 1 && Convert.ToInt32(Session[gblValue.UserId]) != 4883)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblPoolRpr";
            Response.Redirect("~/WebPages/Private/HOReports/HOPool.aspx", false);
        }
        protected void lblBookDebt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblBookDebt";
            Response.Redirect("~/WebPages/Private/HOReports/BookDebt.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbHOFundGiven_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOFundGiven";
            Response.Redirect("~/WebPages/Private/HOReports/HOFundGiven.aspx", false);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAccInt_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbAccInt";
            Response.Redirect("~/WebPages/Private/HOReports/HOAccruedInt.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbMaturity_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbMaturity";
            Response.Redirect("~/WebPages/Private/HOReports/RptMaturity.aspx", false);
        }

        protected void lbExgratiaData_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbExgratiaData";
            Response.Redirect("~/WebPages/Private/HOReports/rptExgratiaData.aspx", false);
        }


        protected void lbRptLoanParameter_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptLoanParameter";
            Response.Redirect("~/WebPages/Private/HOReports/RptLoanParameter.aspx", false);
        }
        protected void lbHOPettyCash_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOPettyCash";
            Response.Redirect("~/WebPages/Private/Report/RptPettyCash.aspx", false);
        }

        protected void lbHOPettyCashBalance_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOPettyCashBalance";
            Response.Redirect("~/WebPages/Private/HOReports/HORptPettyCashBalance.aspx", false);
        }

        protected void lbExcessLed_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbExcessLed";
            Response.Redirect("~/WebPages/Private/HOReports/rptExcessLedger.aspx", false);
        }

        protected void lbActivityTrackRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbActivityTrackRpt";
            Response.Redirect("~/WebPages/Private/Report/RptActivityTracker.aspx", false);
        }

        protected void lbOCRLogRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbOCRLogRpt";
            Response.Redirect("~/WebPages/Private/HOReports/RptOCRLog.aspx", false);
        }
        protected void lbDeviationStatus_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDeviationStatus";
            Response.Redirect("~/WebPages/Private/Report/RptDeviationStatus.aspx", false);
        }

        protected void lbPenOpenBucketRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbPenOpenBucketRpt";
            Response.Redirect("~/WebPages/Private/Report/RptPendingOpenBucket.aspx", false);
        }
        protected void lbDayWiseDPD_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbDayWiseDPD";
            Response.Redirect("~/WebPages/Private/HOReports/rptDayWiseDPD.aspx", false);
        }
        protected void lbHOInsuMstRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbHOInsuMstRpt";
            Response.Redirect("~/WebPages/Private/HOReports/HOAllInsuranceReport.aspx", false);
        }


        protected void lbRptHOtoBRFundTr_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptHOtoBRFundTr";
            Response.Redirect("~/WebPages/Private/HOReports/RptHOtoBRFundTr.aspx", false);
        }

        protected void lbHoVouseVisit_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbHoVouseVisit";
            Response.Redirect("~/WebPages/Private/HOReports/rptHoHouseVisitData.aspx", false);
        }
        protected void lbDailyAccInt_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbDailyAccInt";
            Response.Redirect("~/WebPages/Private/HOReports/DailyAccuredInterest.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lbrptHOPAR_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "ConRpt";
        //    Session["PaneId"] = acConRpt.SelectedIndex;
        //    Session["LinkId"] = "lbrptHOPAR";
        //    Response.Redirect("~/WebPages/Private/HOReports/rptHOPAR.aspx", false);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDailyMisAcc_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDailyMisAcc";
            Response.Redirect("~/WebPages/Private/HOReports/HODailyMisAcc.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbHOLoanDisbProspone_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOLoanDisbProspone";
            Response.Redirect("~/WebPages/Private/HOReports/HOLoanDisbProsPoneRpt.aspx", false);
        }

        /// <summary>
        /// LUC Analysis Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbInspLUC_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbInspLUC";
            Response.Redirect("~/WebPages/Private/HOReports/InspLUC.aspx", false);
        }

        /// <summary>
        /// LUC Analysis (Branchwise) Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LbLucBr_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "LbLucBr";
            Response.Redirect("~/WebPages/Private/HOReports/LucBranch.aspx", false);
        }

        /// <summary>
        /// Process Management Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPMRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbPMRpt";
            Response.Redirect("~/WebPages/Private/HOReports/PMBrWise.aspx", false);
        }

        /// <summary>
        /// Inspection Schedule Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbISchd_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbISchd";
            Response.Redirect("~/WebPages/Private/HOReports/InspPlan.aspx", false);
        }

        /// <summary>
        /// PDC Analysis Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbInspPDC_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbInspPDC";
            Response.Redirect("~/WebPages/Private/HOReports/InspPDC.aspx", false);
        }

        /// <summary>
        /// PDC Analysis (Branchwise) Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPdcBr_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbPdcBr";
            Response.Redirect("~/WebPages/Private/HOReports/InspPdcBranch.aspx", false);
        }

        /// <summary>
        /// Risk Rating Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbRR_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRR";
            Response.Redirect("~/WebPages/Private/HOReports/RR.aspx", false);
        }

        /// <summary>
        /// Risk Rating (Branchwise) Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbRRB_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRRB";
            Response.Redirect("~/WebPages/Private/HOReports/RRBranchWise.aspx", false);
        }

        /// <summary>
        /// Risk Rating (Consolidated) Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbRRC_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRRC";
            Response.Redirect("~/WebPages/Private/HOReports/RRConsolidated.aspx", false);
        }

        /// <summary>
        /// Branch Grading through Process Management Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPMC_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbPMC";
            Response.Redirect("~/WebPages/Private/HOReports/ConsldtPM.aspx", false);
        }

        /// <summary>
        /// Inspection At A Glance Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbInspGlnc_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbInspGlnc";
            Response.Redirect("~/WebPages/Private/HOReports/InspGlance.aspx", false);
        }

        /// <summary>
        /// LUC At A Glance Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbLucGlnc_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbLucGlnc";
            Response.Redirect("~/WebPages/Private/HOReports/LucGlance.aspx", false);
        }

        /// <summary>
        /// PDC At A Glance Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPdcGlnc_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbPdcGlnc";
            Response.Redirect("~/WebPages/Private/HOReports/PdcGlance.aspx", false);
        }

        /// <summary>
        /// Branch Compliance Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbBrCmplR_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbBrCmplR";
            Response.Redirect("~/WebPages/Private/HOReports/BrnchCmplPM.aspx", false);
        }

        /// <summary>
        /// Irregularities Vs. Rectification Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbIreRecty_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbIreRecty";
            Response.Redirect("~/WebPages/Private/HOReports/IrreVsRecty.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbInvBr_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbInvBr";
            Response.Redirect("~/WebPages/Private/HOReports/InvArBrWise.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbInvItemSum_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                //Response.Redirect("~/WebPages/Public/Main.aspx", false);
                gblFuction.MsgPopup("Only Head Office can generate this Report...");
                return;
            }
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbInvItemSum";
            Response.Redirect("~/WebPages/Private/HOReports/InvItemWiseSum.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbInvItemDtl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbInvItemDtl";
            Response.Redirect("~/WebPages/Private/HOReports/InvItemWiseDtl.aspx", false);
        }

        protected void lbInvRent_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                //Response.Redirect("~/WebPages/Public/Main.aspx", false);
                gblFuction.MsgPopup("Only Head Office can generate this Report...");
                return;
            }
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbInvRent";
            Response.Redirect("~/WebPages/Private/HOReports/InvRentDtl.aspx", false);
        }

        protected void lbInvTradeLi_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                //Response.Redirect("~/WebPages/Public/Main.aspx", false);
                gblFuction.MsgPopup("Only Head Office can generate this Report...");
                return;
            }
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbInvTradeLi";
            Response.Redirect("~/WebPages/Private/HOReports/InvTrLicenseDtl.aspx", false);
        }
        protected void lbStckVeriRpt_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbStckVeriRpt";
            Response.Redirect("~/WebPages/Private/Inventory/RptStckSummVeri.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        protected void lb32_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb32";
            Response.Redirect("~/WebPages/Private/Report/CashBook.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb33_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb33";
            Response.Redirect("~/WebPages/Private/Report/BankBook.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb34_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb34";
            Response.Redirect("~/WebPages/Private/Report/JournalBook.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb44_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb44";
            Response.Redirect("~/WebPages/Private/Report/AccLedgerDtls.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb45_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb45";
            Response.Redirect("~/WebPages/Private/Report/AccLedgerSummary.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb46_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb46";
            Response.Redirect("~/WebPages/Private/Report/RecPay.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb47_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb47";
            Response.Redirect("~/WebPages/Private/Report/ProfitLoss.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb48_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb48";
            Response.Redirect("~/WebPages/Private/Report/Trial.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb49_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb49";
            Response.Redirect("~/WebPages/Private/Report/BalSheet.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbBRecon_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBRecon";
            Response.Redirect("~/WebPages/Private/Report/ReconciliationStatement.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbBrTransferRegister_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBrTransferRegister";
            Response.Redirect("~/WebPages/Private/Report/RptTransferRegister.aspx", false);
        }

        protected void lbLoanWiseTransReg_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLoanWiseTransReg";
            Response.Redirect("~/WebPages/Private/Report/LoanWiseTransferRegister.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb35_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb35";
            Response.Redirect("~/WebPages/Private/Report/CollSheet.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb36_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb36";
            Response.Redirect("~/WebPages/Private/Report/DisbRpt.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb37_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb37";
            Response.Redirect("~/WebPages/Private/Report/CollectionRpt.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb38_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb38";
            Response.Redirect("~/WebPages/Private/Report/AtaGlance.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPfAct_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbPfAct";
            Response.Redirect("~/WebPages/Private/Report/PortfolioActivity.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb39_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb39";
            Response.Redirect("~/WebPages/Private/Report/HHSurvey.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb40_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb40";
            Response.Redirect("~/WebPages/Private/Report/ShgGrading.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb41_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb41";
            Response.Redirect("~/WebPages/Private/Report/RatioAnalysis.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb42_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb42";
            Response.Redirect("~/WebPages/Private/Report/PortfolioAgeing.aspx", false);
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb43_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb43";
            Response.Redirect("~/WebPages/Private/Report/LoanStat.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAudSumBr_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbAudSumBr";
            Response.Redirect("~/WebPages/Private/Report/AuditSubBranch.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbInsSum_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbInsSum";
            Response.Redirect("~/WebPages/Private/Report/AudInsSum.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAudTeam_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbAudTeam";
            Response.Redirect("~/WebPages/Private/Report/AuditTeam.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbInsDtlRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbInsDtlRpt";
            Response.Redirect("~/WebPages/Private/Report/AudDetailRpt.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblBorrDtl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lblBorrDtl";
            Response.Redirect("~/WebPages/Private/Report/BorrowerDtl.aspx", false);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblLoanPurposeRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lblLoanPurposeRpt";
            Response.Redirect("~/WebPages/Private/Report/rptLoanPurpose.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblInsurenceRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lblInsurenceRpt";
            Response.Redirect("~/WebPages/Private/Report/rptInsurence.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblInsClDtls_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lblInsClDtls";
            Response.Redirect("~/WebPages/Private/Report/FundGiven.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblDailyMisRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lblInsurenceRpt";
            Response.Redirect("~/WebPages/Private/Report/rptDailyMis.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbMemSearch_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbMemSearch";
            Response.Redirect("~/WebPages/Private/Report/MemSearch.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUpComClsLoan_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbUpComClsLoan";
            Response.Redirect("~/WebPages/Private/Report/UpcomingClsLoan.aspx", false);
        }
        protected void lbSavingApplForBC_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbSavingApplForBC";
            Response.Redirect("~/WebPages/Private/Report/SavingApplicationForBC.aspx", false);
        }
        protected void lbSBulkUpldForBC_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbSBulkUpldForBC";
            Response.Redirect("~/WebPages/Private/Report/BulkUploadForBC.aspx", false);
        }
        protected void lbLoanApplForBC_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLoanApplForBC";
            Response.Redirect("~/WebPages/Private/Report/LoanApplicationForBC.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReadyToDis_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbReadyToDis";
            Response.Redirect("~/WebPages/Private/Report/ReadytoDisBranch.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbLoanDisbProspone_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbLoanDisbProspone";
            Response.Redirect("~/WebPages/Private/Report/LoanDisbProsPoneRpt.aspx", false);
        }

        protected void lbBCColl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbBCColl";
            Response.Redirect("~/WebPages/Private/Report/rtpBCCollection.aspx", false);
        }
        protected void lbrptPAI_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbrptPAI";
            Response.Redirect("~/WebPages/Private/Report/PAIRpt.aspx", false);
        }
        protected void lbSancCanA_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbSancCanA";
            Response.Redirect("~/WebPages/Private/HOReports/RptSancCanAppList.aspx", false);
        }
        protected void lbKYCDoc_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbKYCDoc";
            Response.Redirect("~/WebPages/Private/Report/KYCDocVeri.aspx", false);
        }
        protected void lbNeftBranch_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbKYCDoc";
            Response.Redirect("~/WebPages/Private/HOReports/RptNEFTStatHO.aspx", false);
        }
        protected void lbRptDDStatus_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptDDStatus";
            Response.Redirect("~/WebPages/Private/Report/RptDeathDocStatus.aspx", false);
        }

        protected void lbRptIntAccrual_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptIntAccrual";
            Response.Redirect("~/WebPages/Private/Report/AccruedInterest.aspx", false);
        }
        protected void lbCustDtl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbCustDtl";
            Response.Redirect("~/WebPages/Private/Report/CustomerDetails.aspx", false);
        }
        protected void lbLoanAppln_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbLoanAppln";
            Response.Redirect("~/WebPages/Private/Report/rptLoanApplication.aspx", false);
        }

        protected void lbRptPortFolio_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptPortFolio";
            Response.Redirect("~/WebPages/Private/Report/rptPortFolio.aspx", false);
        }

        protected void lbPARReport_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbPARReport";
            Response.Redirect("~/WebPages/Private/Report/rptPAR.aspx", false);
        }

        protected void lbFunderReport_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbFunderReport";
            Response.Redirect("~/WebPages/Private/Report/rptFunder.aspx", false);
        }

        protected void lbOverride_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbOverride";
            Response.Redirect("~/WebPages/Private/Report/rptOverride.aspx", false);
        }

        protected void lbovrride_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbovrride";
            Response.Redirect("~/WebPages/Private/Report/rptOverride.aspx", false);
        }

        protected void lbLoanUtilization_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLoanUtilization";
            Response.Redirect("~/WebPages/Private/Report/rptLoanUtilization.aspx", false);
        }
        protected void lbReLoanCbChkRptBranch_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbReLoanCbChkRptBranch";
            Response.Redirect("~/WebPages/Private/Report/RptReLoanCBChk.aspx", false);
        }

        protected void lbCenterWiseCustDtlRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbCenterWiseCustDtlRpt";
            Response.Redirect("~/WebPages/Private/Report/RptCenterWiseCustomerDtl.aspx", false);
        }

        protected void lbBRPettyCash_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBRPettyCash";
            Response.Redirect("~/WebPages/Private/Report/RptPettyCash.aspx", false);
        }

        protected void lbBRPettyCashBalance_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBRPettyCashBalance";
            Response.Redirect("~/WebPages/Private/HOReports/HORptPettyCashBalance.aspx", false);
        }

        protected void lbBrPettyCashCert_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBrPettyCashCert";
            Response.Redirect("~/WebPages/Private/Report/RptPettyCashCert.aspx", false);
        }

        protected void lbBrActivityTrackerRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBrActivityTrackerRpt";
            Response.Redirect("~/WebPages/Private/Report/RptActivityTracker.aspx", false);
        }
        protected void lbDeviationStatusRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbDeviationStatusRpt";
            Response.Redirect("~/WebPages/Private/Report/RptDeviationStatus.aspx", false);
        }

        protected void lbMonCmplRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbMonCmplRpt";
            Response.Redirect("~/WebPages/Private/HOReports/MonitoringComplianceRpt.aspx", false);
        }

        protected void lbIntOnAdvRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbIntOnAdvRpt";
            Response.Redirect("~/WebPages/Private/HOReports/rptAdvIntRpt.aspx", false);
        }
        protected void lbDiscReportBr_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbDiscReportBr";
            Response.Redirect("~/WebPages/Private/HOReports/HoDiscrepancyReport.aspx", false);
        }

        protected void lbHolidayrpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";//"Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHolidayrpt";
            Response.Redirect("~/WebPages/Private/Report/rptHoliDayList.aspx", false);
        }

        protected void lbMonitorRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbMonitorRpt";
            Response.Redirect("~/WebPages/Private/HOReports/RptMonitoring.aspx", false);
        }

        protected void lbMonitorRptHO_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbMonitorRptHO";
            Response.Redirect("~/WebPages/Private/HOReports/RptMonitoring.aspx", false);
        }

        protected void lbProsidexStat_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbProsidexStat";
            Response.Redirect("~/WebPages/Private/Report/ProsidexStatus.aspx", false);
        }

        protected void lbMonitCompl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbMonitCompl";
            Response.Redirect("~/WebPages/Private/HOReports/rptMonitoringCompliance.aspx", false);
        }

        protected void lbIncentiveRpt_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbIncentiveRpt";
            Response.Redirect("~/WebPages/Private/HOReports/rptIncentive.aspx", false);
        }

        protected void lbIntOnAdvRptHO_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbIntOnAdvRptHO";
            Response.Redirect("~/WebPages/Private/HOReports/rptAdvIntRpt.aspx", false);
        }

        protected void lbAgeasBulkSubmit_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbAgeasBulkSubmit";
            Response.Redirect("~/WebPages/Private/HOReports/AgeasDataSubmissionReport.aspx", false);
        }

        protected void lbAiquHospiData_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbAiquHospiData";
            Response.Redirect("~/WebPages/Private/HOReports/AiquDataSubmission.aspx", false);
        }

        protected void lbAttendance_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbAttendance";
            Response.Redirect("~/WebPages/Private/HOReports/Attendance.aspx", false);
        }

        protected void lbRptMonthlyAtten_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptMonthlyAtten";
            Response.Redirect("~/WebPages/Private/HOReports/MonthlyAttendance.aspx", false);
        }

        protected void lbDeletedDataRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDeletedDataRpt";
            Response.Redirect("~/WebPages/Private/Report/RptDeletedData.aspx", false);
        }

        protected void lbOnTimeColl_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbOnTimeColl";
            Response.Redirect("~/WebPages/Private/HOReports/RptOnTimeCollection.aspx", false);
        }

        protected void lbMonthEndBorrowerDetails_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbMonthEndBorrowerDetails";
            Response.Redirect("~/WebPages/Private/HOReports/RptMonthEndBorrowerDetails.aspx", false);
        }

        protected void lbCBEnqRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbCBEnqRpt";
            Response.Redirect("~/WebPages/Private/Report/CBEnqRpt.aspx", false);
        }

        protected void lbAwaaz_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbAwaaz";
            Response.Redirect("~/WebPages/Private/HOReports/HOAwaazDe.aspx", false);
        }

        protected void lbLoanUtilizationHead_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbLoanUtilizationHead";
            Response.Redirect("~/WebPages/Private/Report/rptLoanUtilization.aspx", false);
        }

        protected void lbRnDwnReport_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRnDwnReport";
            Response.Redirect("~/WebPages/Private/Report/rptRunDown.aspx", false);
        }

        protected void lbStatOfAcc_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbStatOfAcc";
            Response.Redirect("~/WebPages/Private/Report/rptStatementOfAccount.aspx", false);
        }

        protected void lbHoDemandSheet_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHoDemandSheet";
            Response.Redirect("~/WebPages/Private/HOReports/HODemandSheet.aspx", false);
        }

        protected void lbCollAttHo_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbCollAttHo";
            Response.Redirect("~/WebPages/Private/Report/rptCollAttendance.aspx", false);
        }

        protected void lbRptInsuAtaGlance_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptInsuAtaGlance";
            Response.Redirect("~/WebPages/Private/HOReports/RptInsuranceAtaGlance.aspx", false);
        }
        protected void lbRptInsuAgeingHO_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptInsuAgeingHO";
            Response.Redirect("~/WebPages/Private/HOReports/RptInsuranceAgeing.aspx", false);
        }
        protected void lbRptAttendance_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptAttendance";
            Response.Redirect("~/WebPages/Private/Report/rptAttendanceDtl.aspx", false);
        }

        protected void lbUserDtl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbUserDtl";
            Response.Redirect("~/WebPages/Private/HOReports/HORptUser.aspx", false);
        }
        protected void lbKPIDtl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbKPIDtl";
            Response.Redirect("~/WebPages/Private/HOReports/HORptKPIDtl.aspx", false);
        }
        protected void lbRptDDStatus1_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptDDStatus1";
            Response.Redirect("~/WebPages/Private/Report/RptDeathDocStatus.aspx", false);
        }
        protected void lbRptInsAtaGlance_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptInsAtaGlance";
            Response.Redirect("~/WebPages/Private/HOReports/RptInsuranceAtaGlance.aspx", false);
        }
        protected void lbRptInsAgeingBr_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptInsAgeingBr";
            Response.Redirect("~/WebPages/Private/HOReports/RptInsuranceAgeing.aspx", false);
        }
        protected void lbRptNEFT_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptNEFT";
            Response.Redirect("~/WebPages/Private/HOReports/RptNEFTStatHO.aspx", false);
        }
        protected void lbNEFTPrint_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbNEFTPrint";
            Response.Redirect("~/WebPages/Private/HOReports/RptNEFTPrint.aspx", false);
        }

        protected void lbInsPrem_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbInsPrem";
            Response.Redirect("~/WebPages/Private/HOReports/RptHoInsurance.aspx", false);
        }
        protected void lbInsClaimUpdate_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbInsClaimUpdate";
            Response.Redirect("~/WebPages/Private/HOReports/RptInsuranceClaim.aspx", false);
        }

        protected void lbRptMudra_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptMudra";
            Response.Redirect("~/WebPages/Private/HOReports/RptMudra.aspx", false);
        }
        protected void lbKYCDocHO_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbKYCDocHO";
            Response.Redirect("~/WebPages/Private/Report/KYCDocVeri.aspx", false);
        }


        protected void lbCanPostR_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbCanPostR";
            Response.Redirect("~/WebPages/Private/HOReports/RptCancelOrPostpone.aspx", false);
        }

        protected void lbMemberSbmt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbMemberSbmt";
            Response.Redirect("~/WebPages/Private/HOReports/MemberSubmit.aspx", false);
        }
        protected void lbPPIScore_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbPPIScore";
            Response.Redirect("~/WebPages/Private/HOReports/HOCGTScore.aspx", false);
        }
        protected void lbDCBBal_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDCBBal";
            Response.Redirect("~/WebPages/Private/HOReports/HODmndColl.aspx", false);
        }


        protected void lbBCCollHO_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbBCCollHO";
            Response.Redirect("~/WebPages/Private/HOReports/rptHOBCCollection.aspx", false);
        }
        protected void lbMailMerge_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbMailMerge";
            Response.Redirect("~/WebPages/Private/HOReports/MailMerge.aspx", false);
        }
        protected void lbBC_Data_Upload_Status_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbBC_Data_Upload_Status";
            Response.Redirect("~/WebPages/Private/HOReports/BC_Data_Upload_Status.aspx", false);
        }
        protected void lbHoPAIRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHoPAIRpt";
            Response.Redirect("~/WebPages/Private/Report/PAIRpt.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbCenterWisePOS_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbCenterWisePOS";
            Response.Redirect("~/WebPages/Private/HOReports/rptCenterWisePOS.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblHOFinalPaid_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHOFinalPaid";
            Response.Redirect("~/WebPages/Private/Report/rptLoanClosure.aspx", false);
            //Response.Redirect("~/WebPages/Private/HOReports/rptLoanClosure.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblrptPDDHO_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblrptPDDHO";
            Response.Redirect("~/WebPages/Private/HOReports/rptPDDHO.aspx", false);
        }

        protected void lblrptCreditFlow_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblrptCreditFlow";
            Response.Redirect("~/WebPages/Private/HOReports/HORptCreditFlowIDBI.aspx", false);
        }
        /// <summary>
        /// Process Management Report (BM)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbBPM_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBPM";
            Response.Redirect("~/WebPages/Private/Report/ProcMgmt.aspx", false);
        }

        /// <summary>
        /// Risk Rating (BM)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbBRR_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBRR";
            Response.Redirect("~/WebPages/Private/Report/RiskRate.aspx", false);
        }

        /// <summary>
        /// LUC Analysis (BM)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbBLuc1_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBLuc1";
            Response.Redirect("~/WebPages/Private/Report/LucInsp.aspx", false);
        }

        /// <summary>
        /// LUC Analysis (Branchwise - BM)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbBLuc2_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBLuc2";
            Response.Redirect("~/WebPages/Private/Report/LucBranch.aspx", false);
        }

        /// <summary>
        /// PDC Analysis (BM)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbBPdc1_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBPdc1";
            Response.Redirect("~/WebPages/Private/Report/PdcAnaly.aspx", false);
        }

        /// <summary>
        /// PDC Analysis (Branchwise - BM)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbBPdc2_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBPdc2";
            Response.Redirect("~/WebPages/Private/Report/PdcRept.aspx", false);
        }

        /// <summary>
        /// Branch Compliance (BM)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbBCmp_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbBCmp";
            Response.Redirect("~/WebPages/Private/Report/BrnchCmpl.aspx", false);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb50_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lb50";
            Response.Redirect("~/WebPages/Private/Report/RePayment.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb51_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbl51";
            Response.Redirect("~/WebPages/Private/Report/PartyLedger.aspx", false);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbFpay_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbFpay";
            Response.Redirect("~/WebPages/Private/Report/rptLoanClosure.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblOD_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lblOD";
            Response.Redirect("~/WebPages/Private/Report/Overdue.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblrptPDD_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lblrptPDD";
            Response.Redirect("~/WebPages/Private/Report/rptPDD.aspx", false);
        }

        protected void lbAdvNotPostedHO_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbAdvNotPostedHO";
            Response.Redirect("~/WebPages/Private/HOReports/HoAdvCollNotPosted.aspx", false);
        }

        protected void lbHONDC_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHONDC";
            Response.Redirect("~/WebPages/Private/Report/rptNDC.aspx", false);
        }

        protected void lbRptGst_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptGst";
            Response.Redirect("~/WebPages/Private/HOReports/rptGst.aspx", false);
        }

        protected void lbBcDisburse_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbBcDisburse";
            Response.Redirect("~/WebPages/Private/HOReports/BcDisbursementFile.aspx", false);
        }

        protected void lbScoreCardInternalRiskRating_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbScoreCardInternalRiskRating";
            Response.Redirect("~/WebPages/Private/HOReports/rptHoHouseVisitNew.aspx", false);
        }

        protected void lbPsl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbPsl";
            Response.Redirect("~/WebPages/Private/HOReports/rptPSL.aspx", false);
        }

        protected void lbIBPSL_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbIBPSL";
            Response.Redirect("~/WebPages/Private/HOReports/InternalIBPSLReport.aspx", false);
        }
        protected void lbrptHospiCashClaimNew_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbrptHospiCashClaimNew";
            Response.Redirect("~/WebPages/Private/HOReports/rptHospiCashClaimNew.aspx", false);
        }

        protected void lbrptDiscrepancy_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbrptDiscrepancy";
            Response.Redirect("~/WebPages/Private/HOReports/HoDiscrepancyReport.aspx", false);
        }

        protected void lbRptTelecalling_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptTelecalling";
            Response.Redirect("~/WebPages/Private/HOReports/RptTeleCalling.aspx", false);
        }

        protected void lbRptVillageMst_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptVillageMst";
            Response.Redirect("~/WebPages/Private/Report/RptVillageMst.aspx", false);
        }

        protected void lbRptRoleMatrix_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptRoleMatrix";
            Response.Redirect("~/WebPages/Private/HOReports/rptRoleMatrix.aspx", false);
        }

        protected void lbRptGuaranteeScheme_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptGuaranteeScheme";
            Response.Redirect("~/WebPages/Private/HOReports/GuaranteeSchemeReport.aspx", false);
        }

        protected void lbAdvReschRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbAdvReschRpt";
            Response.Redirect("~/WebPages/Private/HOReports/RptAdvanceReschedule.aspx", false);
        }

        protected void lbAdvAdjRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbAdvAdjRpt";
            Response.Redirect("~/WebPages/Private/HOReports/RptAdvanceAdjustment.aspx", false);
        }

        protected void lbGLRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbGLRpt";
            Response.Redirect("~/WebPages/Private/HOReports/rptAccountLedgerDetails.aspx", false);
        }

        protected void lbInsuApiRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbInsuApiRpt";
            Response.Redirect("~/WebPages/Private/HOReports/rptInsuranceAPI.aspx", false);
        }

        protected void lbOtsMasterRpt_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 1)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbOtsMasterRpt";
            Response.Redirect("~/WebPages/Private/HOReports/rptOTSMasterCheck.aspx", false);
        }

        protected void lbHOCenterWiseCustRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOCenterWiseCustRpt";
            Response.Redirect("~/WebPages/Private/HOReports/rptHOCenterWiseCustomerDtl.aspx", false);
        }

        protected void lbRptUserWiseBranch_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptUserWiseBranch";
            Response.Redirect("~/WebPages/Private/HOReports/rptUserWiseBranch.aspx", false);
        }

        protected void lbRptCreditSanction_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptCreditSanction";
            Response.Redirect("~/WebPages/Private/HOReports/rptCreditSanction.aspx", false);
        }

        protected void lbRptMultiUcic_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptMultiUcic";
            Response.Redirect("~/WebPages/Private/HOReports/RptMultipleUCIC.aspx", false);
        }

        protected void lbrptRBIDataIndent_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbrptRBIDataIndent";
            Response.Redirect("~/WebPages/Private/HOReports/RptRBIDataIndent.aspx", false);
        }

        #endregion

        # region Consolidate Reports

        protected void lbDsbPos_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.UserId]) != 13 && Convert.ToInt32(Session[gblValue.UserId]) != 1 && Convert.ToInt32(Session[gblValue.UserId]) != 4883)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDsbPos";
            Response.Redirect("~/WebPages/Private/DashBoard/DsbPos.aspx", false);
        }

        protected void lbDsbActvClnt_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.UserId]) != 13 && Convert.ToInt32(Session[gblValue.UserId]) != 1 && Convert.ToInt32(Session[gblValue.UserId]) != 4883)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDsbActvClnt";
            Response.Redirect("~/WebPages/Private/DashBoard/DsbActiveClient.aspx", false);
        }

        protected void lbDsbODClnt_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.UserId]) != 13 && Convert.ToInt32(Session[gblValue.UserId]) != 1 && Convert.ToInt32(Session[gblValue.UserId]) != 4883)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDsbODClnt";
            Response.Redirect("~/WebPages/Private/DashBoard/DsbODClient.aspx", false);
        }

        protected void lbDsbODOS_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.UserId]) != 13 && Convert.ToInt32(Session[gblValue.UserId]) != 1 && Convert.ToInt32(Session[gblValue.UserId]) != 4883)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDsbODOS";
            Response.Redirect("~/WebPages/Private/DashBoard/DsbODOS.aspx", false);
        }

        protected void lbDsbDisbNo_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.UserId]) != 13 && Convert.ToInt32(Session[gblValue.UserId]) != 1 && Convert.ToInt32(Session[gblValue.UserId]) != 4883)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDsbDisbNo";
            Response.Redirect("~/WebPages/Private/DashBoard/DsbDisbNo.aspx", false);
        }

        protected void lbDsbODAmt_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.UserId]) != 13 && Convert.ToInt32(Session[gblValue.UserId]) != 1 && Convert.ToInt32(Session[gblValue.UserId]) != 4883)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDsbODAmt";
            Response.Redirect("~/WebPages/Private/DashBoard/DsbODAmount.aspx", false);
        }

        protected void lblHOHMark_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHOHMark";
            Response.Redirect("~/WebPages/Private/HOReports/HiMarkDataSubmission.aspx", false);
        }
        protected void lblEqFax_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblEqFax";
            Response.Redirect("~/WebPages/Private/HOReports/EquiFaxDataSubmission.aspx", false);
        }
        protected void lblEqFaxBC_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblEqFaxBC";
            Response.Redirect("~/WebPages/Private/HOReports/EquiFaxDataSubmissionBC.aspx", false);
        }
        protected void lblCibilFormat_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblCibilFormat";
            Response.Redirect("~/WebPages/Private/HOReports/CIBILDataSubmission.aspx", false);
        }
        protected void lblExperianUnnatiTrade_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblExperianUnnatiTrade";
            Response.Redirect("~/WebPages/Private/HOReports/ExperianUNNATI_TRADEDataSubmission.aspx", false);
        }
        protected void lblDayEndReport_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblDayEndReport";
            Response.Redirect("~/WebPages/Private/HOReports/HODayEndReport.aspx", false);
        }

        //protected void lbAppNotDisb_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "ConRpt";
        //    Session["PaneId"] = acConRpt.SelectedIndex;
        //    Session["LinkId"] = "lbAppNotDisb";
        //    Response.Redirect("~/WebPages/Private/HOReports/AppNotDisbHo.aspx", false);
        //}
        protected void lbBucket_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbBucket";
            Response.Redirect("~/WebPages/Private/HOReports/LoanStatusHo.aspx", false);
        }
        protected void lbHOAtaGlance_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOAtaGlance";
            Response.Redirect("~/WebPages/Private/HOReports/HOAtaGlance.aspx", false);
        }

        protected void lbCollRptHo_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbCollRptHo";
            Response.Redirect("~/WebPages/Private/HOReports/CollectionHo.aspx", false);
        }

        protected void lblLoanCollHo_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblLoanCollHo";
            Response.Redirect("~/WebPages/Private/Report/RptLoanCollection.aspx", false);
        }

        protected void lbFeeDtlHo_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbFeeDtlHo";
            Response.Redirect("~/WebPages/Private/HOReports/FeesDtlHo.aspx", false);
        }

        protected void lbTATHo_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbTATHo";
            Response.Redirect("~/WebPages/Private/HOReports/TATHo.aspx", false);
        }

        protected void lbLnDisbHo_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbLnDisbHo";
            Response.Redirect("~/WebPages/Private/HOReports/LoanDisbHo.aspx", false);
        }
        protected void lbSPlLnDisbHo_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbSPlLnDisbHo";
            Response.Redirect("~/WebPages/Private/HOReports/RptSpecialDisbursement.aspx", false);
        }

        protected void lbLnSncHo_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbLnSncHo";
            Response.Redirect("~/WebPages/Private/HOReports/HOLoanSanctionRpt.aspx", false);
        }

        protected void lbIniApprStatusHO_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbIniApprStatusHO";
            Response.Redirect("~/WebPages/Private/Report/InitialApproachStatusRpt.aspx", false);
        }

        protected void lbMemVerifyRptHo_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbMemVerifyRptHo";
            Response.Redirect("~/WebPages/Private/HOReports/MemberVerifyRpt.aspx", false);
        }

        protected void lbPortAgeHo_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.UserId]) != 13 && Convert.ToInt32(Session[gblValue.UserId]) != 1 && Convert.ToInt32(Session[gblValue.UserId]) != 4883)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbPortAgeHo";
            Response.Redirect("~/WebPages/Private/HOReports/PortfolioAgeingHo.aspx", false);
        }

        protected void lbPortActHo_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbPortActHo";
            Response.Redirect("~/WebPages/Private/HOReports/PortActHo.aspx", false);
        }
        protected void lbOD_Click(object sender, EventArgs e)
        {
            //if (Convert.ToInt32(Session[gblValue.UserId]) != 13 && Convert.ToInt32(Session[gblValue.UserId]) != 1
            //    && Convert.ToInt32(Session[gblValue.UserId]) != 4883 && Convert.ToInt32(Session[gblValue.RoleId]) != 51)
            //{
            //    gblFuction.MsgPopup("Permission Denied...");
            //    return;
            //}
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbOD";
            Response.Redirect("~/WebPages/Private/HOReports/ODHo.aspx", false);
        }
        protected void lblMemDiffStage_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblMemDiffStage";
            Response.Redirect("~/WebPages/Private/HOReports/MemDiffStage.aspx", false);
        }
        protected void lbdayEnd_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbdayEnd";
            Response.Redirect("~/WebPages/Private/HOReports/RptHODayEnd.aspx", false);
        }
        protected void lbcanPos_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbcanPos";
            Response.Redirect("~/WebPages/Private/HOReports/RptCancelOrPostpone.aspx", false);
        }
        protected void lbSancCan_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbSancCan";
            Response.Redirect("~/WebPages/Private/HOReports/RptSancCanAppList.aspx", false);
        }
        protected void lbBrDtlHo_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbBrDtlHo";
            Response.Redirect("~/WebPages/Private/HOReports/BorrowerDtlHo.aspx", false);
        }
        protected void lbDmdColHo_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDmdColHo";
            Response.Redirect("~/WebPages/Private/HOReports/DmdColHo.aspx", false);
        }
        protected void lbHOComA_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOComA";
            Response.Redirect("~/WebPages/Private/HOReports/CompAnalysisPos.aspx", false);
        }

        protected void lblEquiFaxSendRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblEquiFaxSendRpt";
            Response.Redirect("~/WebPages/Private/Report/rptEquifaxSend.aspx", false);
        }

        protected void lbRToD_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRToD";
            Response.Redirect("~/WebPages/Private/Report/RtoD.aspx", false);
        }

        protected void lbDmColStat_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDmColStat";
            Response.Redirect("~/WebPages/Private/HOReports/DmdCollStatusHo.aspx", false);
        }

        protected void lblDMisHO_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblDMisHO";
            Response.Redirect("~/WebPages/Private/HOReports/HODailyMIS.aspx", false);
        }

        protected void lbCollDtlHO_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbCollDtlHO";
            Response.Redirect("~/WebPages/Private/HOReports/CollDtlBrWiseHO.aspx", false);
        }

        protected void lbDisbDtlHO_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDisbDtlHO";
            Response.Redirect("~/WebPages/Private/HOReports/DisbDtlBrWiseHo.aspx", false);
        }

        protected void lbFeesTaxHO_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbFeesTaxHO";
            Response.Redirect("~/WebPages/Private/HOReports/FeesTaxBrWiseHo.aspx", false);
        }

        protected void lbInsuDtlHO_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbInsuDtlHO";
            Response.Redirect("~/WebPages/Private/HOReports/InsuDtlBrWiseHo.aspx", false);
        }
        protected void lbHOAttendance_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOAttendance";
            Response.Redirect("~/WebPages/Private/Master/AttendanceReg.aspx", false);
        }
        protected void lbBrWiseLnDtl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbBrWiseLnDtl";
            Response.Redirect("~/WebPages/Private/HOReports/rptBrWiseLnDtl.aspx", false);
        }
        protected void lbBrWiseLnDisb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbBrWiseLnDisb";
            Response.Redirect("~/WebPages/Private/HOReports/rptBrWiseLnDisb.aspx", false);
        }
        protected void lbMnwiseColl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbMnwiseColl";
            Response.Redirect("~/WebPages/Private/HOReports/RptMonthWiseLnDtl.aspx", false);
        }
        protected void lbRptPortPerf_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRptPortPerf";
            Response.Redirect("~/WebPages/Private/HOReports/HOPortPerform.aspx", false);
        }

        protected void lblExpRepHO_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblExpRepHO";
            Response.Redirect("~/WebPages/Private/HOReports/HOBranchwiseExp.aspx", false);
        }

        protected void lbInsuDtl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbInsuDtl";
            Response.Redirect("~/WebPages/Private/HOReports/rptInsuranceDtl.aspx", false);
        }


        protected void lbHOAcLed_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOAcLed";
            Response.Redirect("~/WebPages/Private/HOReports/HOAccLedgerDtl.aspx", false);
        }

        protected void lblHORecPay_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHORecPay";
            Response.Redirect("~/WebPages/Private/HOReports/HORecPay.aspx", false);
        }

        protected void lblCBCls_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.UserId]) != 13 && Convert.ToInt32(Session[gblValue.UserId]) != 1 && Convert.ToInt32(Session[gblValue.UserId]) != 4883)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblCBCls";
            Response.Redirect("~/WebPages/Private/HOReports/HOCashCompair.aspx", false);
        }

        protected void lblCBClsBr_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.UserId]) != 13 && Convert.ToInt32(Session[gblValue.UserId]) != 1 && Convert.ToInt32(Session[gblValue.UserId]) != 4883)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblCBClsBr";
            Response.Redirect("~/WebPages/Private/HOReports/HOCashBnkClBrWise.aspx", false);
        }
        protected void lblFTR_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblFTR";
            Response.Redirect("~/WebPages/Private/HOReports/HOFundReconcile.aspx", false);
        }
        protected void lblHOProfitLoss_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHOProfitLoss";
            Response.Redirect("~/WebPages/Private/HOReports/HOProfitLoss.aspx", false);
        }


        protected void lblHOTrial_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHOTrial";
            Response.Redirect("~/WebPages/Private/HOReports/HOTrial.aspx", false);
        }


        protected void lblHOBalSheet_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.UserId]) != 13 && Convert.ToInt32(Session[gblValue.UserId]) != 1 && Convert.ToInt32(Session[gblValue.UserId]) != 4883)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblHOBalSheet";
            Response.Redirect("~/WebPages/Private/HOReports/HOBalSheet.aspx", false);
        }


        protected void lblSpAcRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lblSpAcRpt";
            Response.Redirect("~/WebPages/Private/HOReports/HOSpAcRpt.aspx", false);
        }
        protected void lbHOAccLedgerdtl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOAccLedgerdtl";
            Response.Redirect("~/WebPages/Private/HOReports/HOAccLedgerDtl.aspx", false);
        }

        protected void lbHOLoanWiseLedgerDtl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOLoanWiseLedgerDtl";
            Response.Redirect("~/WebPages/Private/HOReports/LoanWiseLedgerDetails.aspx", false);
        }

        protected void lbHoTransferRegister_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHoTransferRegister";
            Response.Redirect("~/WebPages/Private/Report/RptTransferRegister.aspx", false);
        }

        protected void lbHOLoanWiseTransReg_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOLoanWiseTransReg";
            Response.Redirect("~/WebPages/Private/Report/LoanWiseTransferRegister.aspx", false);
        }

        protected void lbHOAccRqFundSentToBr_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOAccRqFundSentToBr";
            Response.Redirect("~/WebPages/Private/Report/RqFundtoBr.aspx", false);
        }

        protected void lbHOCollRecon_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbHOCollRecon";
            Response.Redirect("~/WebPages/Private/HOReports/HOCollectionRecon.aspx", false);
        }

        protected void lbBranchListRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbBranchListRpt";
            Response.Redirect("~/WebPages/Private/HOReports/rptBranchMaster.aspx", false);
        }

        protected void lbEmpListRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbEmpListRpt";
            Response.Redirect("~/WebPages/Private/HOReports/rptEmployeeList.aspx", false);
        }

        protected void lbDisbRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDisbRpt";
            Response.Redirect("~/WebPages/Private/HOReports/RptDisbursement.aspx", false);
        }

        protected void lbPARMgtRpt_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 51 && Convert.ToInt32(Session[gblValue.RoleId]) != 1 && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 11 && Convert.ToInt32(Session[gblValue.RoleId]) != 27)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbPARMgtRpt";
            Response.Redirect("~/WebPages/Private/HOReports/rptPARMgt.aspx", false);
        }

        protected void lbPARRankingRpt_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.UserId]) != 13 && Convert.ToInt32(Session[gblValue.UserId]) != 1 && Convert.ToInt32(Session[gblValue.UserId]) != 4883)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbPARRankingRpt";
            Response.Redirect("~/WebPages/Private/HOReports/RptPARRanking.aspx", false);
        }

        protected void lbDisbRankRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDisbRankRpt";
            Response.Redirect("~/WebPages/Private/HOReports/RptDisbRanking.aspx", false);
        }

        protected void lbDataEntry_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbDataEntry";
            Response.Redirect("~/WebPages/Private/HOReports/RptDataEntry.aspx", false);
        }

        protected void lbAttendanceMIS_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbAttendanceMIS";
            Response.Redirect("~/WebPages/Private/HOReports/RptAttendance.aspx", false);
        }
        protected void lbOTPBasedDisbRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbOTPBasedDisbRpt";
            Response.Redirect("~/WebPages/Private/HOReports/RptLoanDisbHoOTP.aspx", false);
        }

        protected void lbPARAnalysis_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbPARAnalysis";
            Response.Redirect("~/WebPages/Private/HOReports/rptPARAnalysis.aspx", false);
        }

        protected void lbRBIMaturityReport_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session[gblValue.UserId]) != 13 && Convert.ToInt32(Session[gblValue.UserId]) != 1
                && Convert.ToInt32(Session[gblValue.UserId]) != 4883 && Convert.ToInt32(Session[gblValue.RoleId]) != 54)
            {
                gblFuction.MsgPopup("Permission Denied...");
                return;
            }
            Session["MnuId"] = "ConRpt";
            Session["PaneId"] = acConRpt.SelectedIndex;
            Session["LinkId"] = "lbRBIMaturityReport";
            Response.Redirect("~/WebPages/Private/HOReports/RptRBIMaturity.aspx", false);
        }

        //protected void lbHOVouPrint_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "Rept";
        //    Session["PaneId"] = acConRpt.SelectedIndex;
        //    Session["LinkId"] = "lbHOVouPrint";
        //    Response.Redirect("~/WebPages/Private/HOReports/HOClaimVouPrint.aspx", false);
        //}
        #endregion


        #region BCOperationMenu
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbBC_CustFile_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "BCOpr";
            Session["PaneId"] = acBCOpr.SelectedIndex;
            Session["LinkId"] = "lbBC_CustFile";
            Response.Redirect("~/WebPages/Private/BCOperation/BC_SFTP_UPLOAD.aspx", false);
        }
        protected void lbBC_JLGFile_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "BCOpr";
            Session["PaneId"] = acBCOpr.SelectedIndex;
            Session["LinkId"] = "lbBC_JLGFile";
            Response.Redirect("~/WebPages/Private/BCOperation/BC_JLGTAG_UPLOAD.aspx", false);
        }
        protected void lbBC_JCONFFile_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "BCOpr";
            Session["PaneId"] = acBCOpr.SelectedIndex;
            Session["LinkId"] = "lbBC_JCONFFile";
            Response.Redirect("~/WebPages/Private/BCOperation/BC_JCONF_Upload.aspx", false);
        }
        protected void lbBC_CustRej_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "BCOpr";
            Session["PaneId"] = acBCOpr.SelectedIndex;
            Session["LinkId"] = "lbBC_CustRej";
            Response.Redirect("~/WebPages/Private/BCOperation/BC_Cust_Rej_Recheck.aspx", false);
        }
        protected void lbBC_JLG_Rej_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "BCOpr";
            Session["PaneId"] = acBCOpr.SelectedIndex;
            Session["LinkId"] = "lbBC_JLG_Rej";
            Response.Redirect("~/WebPages/Private/BCOperation/BC_JLG_Reject_Upload.aspx", false);
        }
        protected void lbSFTP_Download_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "BCOpr";
            Session["PaneId"] = acBCOpr.SelectedIndex;
            Session["LinkId"] = "lbSFTP_Download";
            Response.Redirect("~/WebPages/Private/BCOperation/BC_File_Download.aspx", false);
        }
        protected void lbSFTP_Receive_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "BCOpr";
            Session["PaneId"] = acBCOpr.SelectedIndex;
            Session["LinkId"] = "lbSFTP_Receive";
            Response.Redirect("~/WebPages/Private/BCOperation/BC_File_Receive.aspx", false);
        }
        protected void lbBCAppTrack_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "BCOpr";
            Session["PaneId"] = acBCOpr.SelectedIndex;
            Session["LinkId"] = "lbBCAppTrack";
            Response.Redirect("~/WebPages/Private/BCOperation/BC_URNID_Track.aspx", false);
        }
        protected void lbBCOverPnding_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "BCOpr";
            Session["PaneId"] = acBCOpr.SelectedIndex;
            Session["LinkId"] = "lbBCOverPnding";
            Response.Redirect("~/WebPages/Private/BCOperation/BC_File_Download.aspx", false);
        }
        protected void lbrptBsAct_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "BCOpr";
            Session["PaneId"] = acBCOpr.SelectedIndex;
            Session["LinkId"] = "lbrptBsAct";
            Response.Redirect("~/WebPages/Private/BCOperation/BC_Business_Activity.aspx", false);
        }
        protected void lbPortOut_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "BCOpr";
            Session["PaneId"] = acBCOpr.SelectedIndex;
            Session["LinkId"] = "lbPortOut";
            Response.Redirect("~/WebPages/Private/BCOperation/BC_Portfolio_Outstanding.aspx", false);
        }
        protected void lbNewBsns_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "BCOpr";
            Session["PaneId"] = acBCOpr.SelectedIndex;
            Session["LinkId"] = "lbNewBsns";
            Response.Redirect("~/WebPages/Private/BCOperation/BC_New_Business_Summary.aspx", false);
        }
        protected void lbRepaySum_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "BCOpr";
            Session["PaneId"] = acBCOpr.SelectedIndex;
            Session["LinkId"] = "lbRepaySum";
            Response.Redirect("~/WebPages/Private/BCOperation/BC_Repayment_Summary.aspx", false);
        }
        protected void lbRepayFileTrck_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "BCOpr";
            Session["PaneId"] = acBCOpr.SelectedIndex;
            Session["LinkId"] = "lbRepayFileTrck";
            Response.Redirect("~/WebPages/Private/BCOperation/BC_RepayFileTracking.aspx", false);
        }
        protected void lbRepayTrck_Click(object sender, EventArgs e)
        {

            Session["MnuId"] = "BCOpr";
            Session["PaneId"] = acBCOpr.SelectedIndex;
            Session["LinkId"] = "lbRepayTrck";
            Response.Redirect("~/WebPages/Private/BCOperation/BC_RepayTracking.aspx", false);
        }
        #endregion

        #region SystemMenu
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDayBegin_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                //Response.Redirect("~/WebPages/Public/Main.aspx", false);
                gblFuction.MsgPopup("Branch Office can not start day end...");
                return;
            }
            //if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(Session[gblValue.LoginDate].ToString()))
            //{
            //    gblFuction.MsgPopup("Day End already completed for--" + gblFuction.putStrDate(gblFuction.setDate(Session[gblValue.LoginDate].ToString())));
            //    return;
            //}
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbDayBegin";
            Response.Redirect("~/WebPages/Private/Admin/HODayBegin.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbS7_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                //Response.Redirect("~/WebPages/Public/Main.aspx", false);
                gblFuction.MsgPopup("Branch Office can not start day end...");
                return;
            }
            //if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(Session[gblValue.LoginDate].ToString()))
            //{
            //    gblFuction.MsgPopup("Day End already completed for--" + gblFuction.putStrDate(gblFuction.setDate(Session[gblValue.LoginDate].ToString())));
            //    return;
            //}
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbS7";
            Response.Redirect("~/WebPages/Private/Admin/HODayEnd.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbS1_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbS1";
            Response.Redirect("~/WebPages/Private/Admin/YearEnd.aspx", false);
        }

        protected void lblWorkAlloc_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lblWorkAlloc";
            Response.Redirect("~/WebPages/Private/Admin/MobWorkAllocation.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbS3_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbS3";
            Response.Redirect("~/WebPages/Private/Admin/Role.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbS4_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbS4";
            Response.Redirect("~/WebPages/Private/Admin/RoleAssigne.aspx", false);
        }
        protected void lbS9_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbS9";
            Response.Redirect("~/WebPages/Private/Admin/Mob_Role.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbS6_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbS6";
            Response.Redirect("~/WebPages/Private/Admin/Users.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbS5_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbS5";
            Response.Redirect("~/WebPages/Private/Admin/ChgPass.aspx", false);
        }

        protected void lbs8_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbS8";
            Response.Redirect("~/WebPages/Private/Admin/MarqueeMaster.aspx", false);
        }
        protected void lbLocation_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbLocation";
            Response.Redirect("~/WebPages/Private/Master/LocationTrack.aspx", false);
        }
        protected void lbMac_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                //Response.Redirect("~/WebPages/Public/Main.aspx", false);
                gblFuction.MsgPopup("Only Head Office Can Access...");
                return;
            }
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbMac";
            Response.Redirect("~/WebPages/Private/Admin/MacEntry.aspx", false);
        }

        protected void lbAuditTrail_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Only Head Office Can Access...");
                return;
            }
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lbAuditTrail";
            Response.Redirect("~/WebPages/Private/Admin/AuditTrail.aspx", false);
        }


        protected void lblFinacleAccData_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("Only Head Office Can Access...");
                return;
            }
            Session["MnuId"] = "Syst";
            Session["PaneId"] = acSys.SelectedIndex;
            Session["LinkId"] = "lblFinacleAccData";
            Response.Redirect("~/WebPages/Private/HOReports/RptFinacleAccData.aspx", false);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lbTrnsfr_Click(object sender, EventArgs e)
        //{
        //    Session["MnuId"] = "Syst";
        //    Session["PaneId"] = acSys.SelectedIndex;
        //    Session["LinkId"] = "lbTrnsfr";
        //    Response.Redirect("~/WebPages/Private/Admin/Transfer.aspx");
        //}

        #endregion

        #region DashBoard

        protected void lbdb1_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbdb1";
            Response.Redirect("~/WebPages/Private/DashBoard/DBAtaglace.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbdb2_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rept";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbdb2";
            Response.Redirect("~/WebPages/Private/DashBoard/DBDisb.aspx");
        }

        #endregion

        #region UtilityMenu
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbFulBak_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.MsgPopup("This Module Can only be operated from Branch");
                return;
            }
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbU1";
            Response.Redirect("~/WebPages/Private/Admin/DbBackup.aspx", false);
        }

        protected void lbActivityTrak_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("This Module Can only be operated from Head Office");
                return;
            }
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbActivityTrak";
            Response.Redirect("~/WebPages/Private/Admin/ActivityTracker.aspx", false);
        }
        protected void lbFtchAadhDet_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.MsgPopup("This Module Can only be operated from Head Office");
                return;
            }
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbFtchAadhDet";
            Response.Redirect("~/WebPages/Private/Transaction/FetchAadhaarDetails.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbU1_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Fedaration can not take the backup...");
            //    return;
            //}
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbU1";
            Response.Redirect("~/WebPages/Private/Admin/BackupRestore.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            Response.Redirect("~/WebPages/Private/Admin/TblSql.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            Response.Redirect("~/WebPages/Private/Admin/ExctSql.aspx", false);
        }


        protected void lbSrch_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Fedaration can not take the backup...");
            //    return;
            //}
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbSrch";
            Response.Redirect("~/WebPages/Private/Admin/Search.aspx", false);
        }


        protected void lbLoanAppUpdate_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbLoanAppUpdate";
            Response.Redirect("~/WebPages/Private/Admin/LoanApplUpdate.aspx", false);
        }


        protected void lbLoanCalculator_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbLoanCalculator";
            Response.Redirect("~/WebPages/Private/Admin/LoanCalculator.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbU4_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Fedaration can not take the backup...");
            //    return;
            //}
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbU4";
            Response.Redirect("~/WebPages/Private/Admin/Restore.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbLnCal_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            //    gblFuction.MsgPopup("Fedaration can not take the backup...");
            //    return;
            //}
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbLnCal";
            Response.Redirect("~/WebPages/Private/Admin/LoanCalculator.aspx", false);
        }

        protected void lbTrBr_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.MsgPopup("This Module Can only be operated from Branch");
                return;
            }
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbTrBr";
            Response.Redirect("~/WebPages/Private/Admin/TransferCenter.aspx", false);
        }

        protected void lbInBrTr_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.MsgPopup("This Module Can only be operated from Branch");
                return;
            }
            Session["MnuId"] = "Utly";
            Session["PaneId"] = Accordion1.SelectedIndex;
            Session["LinkId"] = "lbInBrTr";
            Response.Redirect("~/WebPages/Private/Admin/InterBrTransferCenter.aspx", false);
        }

        #endregion

        #region NPS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAgnMst_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "NPS";
            Session["PaneId"] = acNPS.SelectedIndex;
            Session["LinkId"] = "lbAgnMst";
            Response.Redirect("~/WebPages/Private/Master/NpsAgent.aspx", false);
        }
        protected void lbTaxInvoice_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "GST";
            Session["PaneId"] = acGST.SelectedIndex;
            Session["LinkId"] = "lbTaxInvoice";
            Response.Redirect("~/WebPages/Private/Transaction/TaxInvoice.aspx", false);
        }
        protected void lbGSTInvoice_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "GST";
            Session["PaneId"] = acGST.SelectedIndex;
            Session["LinkId"] = "lbGSTInvoice";
            Response.Redirect("~/WebPages/Private/Report/GSTInvoice.aspx", false);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbMemMst_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "NPS";
            Session["PaneId"] = acNPS.SelectedIndex;
            Session["LinkId"] = "lbMemMst";
            Response.Redirect("~/WebPages/Private/Master/NpsMember.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPrmMst_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "NPS";
            Session["PaneId"] = acNPS.SelectedIndex;
            Session["LinkId"] = "lbPrmMst";
            Response.Redirect("~/WebPages/Private/Master/NpsParameter.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb32No_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "NPS";
            Session["PaneId"] = acNPS.SelectedIndex;
            Session["LinkId"] = "lb32No";
            Response.Redirect("~/WebPages/Private/Transaction/NpsMemSelPran.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPran_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "NPS";
            Session["PaneId"] = acNPS.SelectedIndex;
            Session["LinkId"] = "lbPran";
            Response.Redirect("~/WebPages/Private/Transaction/NpsPranData.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbColl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "NPS";
            Session["PaneId"] = acNPS.SelectedIndex;
            Session["LinkId"] = "lbColl";
            Response.Redirect("~/WebPages/Private/Transaction/NpsColl.aspx", false);
        }

        protected void lbCollRemitt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "NPS";
            Session["PaneId"] = acNPS.SelectedIndex;
            Session["LinkId"] = "lbCollRemitt";
            Response.Redirect("~/WebPages/Private/Transaction/NpsCollRemitt.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUpCont_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "NPS";
            Session["PaneId"] = acNPS.SelectedIndex;
            Session["LinkId"] = "lbUpCont";
            Response.Redirect("~/WebPages/Private/Transaction/NpsExpCollCont.aspx", false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblSendPFRDA_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Option Can only be operated from Head Office");
                return;
            }
            Session["MnuId"] = "NPS";
            Session["PaneId"] = acNPS.SelectedIndex;
            Session["LinkId"] = "lblSendPFRDA";
            Response.Redirect("~/WebPages/Private/Transaction/NpsPFRDReturn.aspx", false);
        }


        protected void lbNpsMemRpt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "NPS";
            Session["PaneId"] = acNPS.SelectedIndex;
            Session["LinkId"] = "lbNpsMemRpt";
            Response.Redirect("~/WebPages/Private/Report/rptNPSMember.aspx");
        }


        protected void lbNpsBnkStat_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can only be operated from Head Office");
                return;
            }
            Session["MnuId"] = "NPS";
            Session["PaneId"] = acNPS.SelectedIndex;
            Session["LinkId"] = "lbNpsBnkStat";
            Response.Redirect("~/WebPages/Private/Report/rptNPSBankStmnt.aspx", false);
        }


        protected void lbNpsLiteColl_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can only be operated from Head Office");
                return;
            }
            Session["MnuId"] = "NPS";
            Session["PaneId"] = acNPS.SelectedIndex;
            Session["LinkId"] = "lbNpsLiteColl";
            Response.Redirect("~/WebPages/Private/Report/rptNPSLiteColl.aspx", false);
        }

        protected void lbNpsUploadAlankrit_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "NPS";
            Session["PaneId"] = acNPS.SelectedIndex;
            Session["LinkId"] = "lbNpsUploadAlankrit";
            Response.Redirect("~/WebPages/Private/Report/rpt32LotNpsAlankrit.aspx", false);
        }

        protected void lblNpsRemittStatus_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "NPS";
            Session["PaneId"] = acNPS.SelectedIndex;
            Session["LinkId"] = "lblNpsRemittStatus";
            Response.Redirect("~/WebPages/Private/Report/NPSrptRemittList.aspx", false);
        }

        protected void lblNpsAging_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "NPS";
            Session["PaneId"] = acNPS.SelectedIndex;
            Session["LinkId"] = "lblNpsAging";
            Response.Redirect("~/WebPages/Private/Report/NpsAging.aspx", false);
        }



        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
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
                //btnAudit.Enabled = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Welcome
        {
            set { trHeading.Visible = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PageHeading
        {
            set
            {
                this.lblPgNm.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ShowLoginInfo
        {
            set
            {
                this.lblWlCome.Visible = true;
                this.lblWlCome.Text = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string ShowBranchName
        {
            set
            {
                this.lblFedrtn.Visible = true;
                this.lblFedrtn.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ShowFinYear
        {
            set
            {
                this.lblFinYr.Visible = true;
                this.lblFinYr.Text = value;
            }
        }

        #endregion

        #region SME
        protected void lbLeadGen_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "SME";
            Session["PaneId"] = acOpr.SelectedIndex;
            Session["LinkId"] = "lbLeadGen";
            Response.Redirect("~/WebPages/Private/SME/LeadGeneration.aspx", false);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbLogOut_Click(object sender, EventArgs e)
        {
            CUser oUsr = new CUser();
            int vErr = oUsr.UpdateLogOutDt(Convert.ToInt32(Session[gblValue.LoginId]));
            Session.Abandon();
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            foreach (string cookieName in Request.Cookies.AllKeys)
            {
                HttpCookie cookie = new HttpCookie(cookieName);
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            lblWlCome.Text = " Guest";
            // Response.Cookies["LoginYN"].Expires = DateTime.Now.AddDays(-1); 
            Response.Redirect("~/Login.aspx", false);
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

        protected void lbHoInsuCoStatus_Click(object sender, EventArgs e)
        {
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("This Module Can only be operated from HO");
                return;
            }
            Session["MnuId"] = "Tran";
            Session["PaneId"] = acTrn.SelectedIndex;
            Session["LinkId"] = "lbHoInsuCoStatus";
            Response.Redirect("~/WebPages/Private/Transaction/InsuCoStatus.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblChBranch_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/FinYear.aspx", false);
        }
        protected void lblHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx", false);
        }
        protected void lblSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Private/Master/Search.aspx", false);
        }
        protected void lblReport_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Private/Report/DownloadReports.aspx", false);
        }
        protected void lblDashBoard_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/DashBoard.aspx", false);
        }

        public Boolean TimeChk()
        {
            string vTime = DateTime.Now.ToString("HH:mm:ss");
            DateTime dt = DateTime.Parse("01/01/1900 " + vTime);
            DateTime dt1 = DateTime.Parse(vAccessTime);
            if (dt >= dt1)
            {
                return false;
            }
            return true;
        }

    }
}
