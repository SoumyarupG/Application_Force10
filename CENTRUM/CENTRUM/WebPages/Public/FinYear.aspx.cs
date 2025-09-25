using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FORCECA;
using FORCEBA;

namespace CENTRUM.Public
{
    public partial class FinYear : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Control menu = Page.Master.FindControl("divmenubar");
            Control header = Page.Master.FindControl("spancompname");
            Control marquee = Page.Master.FindControl("marqueemsg");
            Control submsg = Page.Master.FindControl("lt1");
            menu.Visible = false;
            header.Visible = false;
            marquee.Visible = false;
            submsg.Visible = false;
            Control img3 = Page.Master.FindControl("Img3");
            Control Img2 = Page.Master.FindControl("Img2");
            Control imgReport = Page.Master.FindControl("imgReport");
            Control imgDashBoard = Page.Master.FindControl("imgDashBoard");

            img3.Visible = false;
            Img2.Visible = false;
            imgReport.Visible = false;
            imgDashBoard.Visible = false;
            ((LinkButton)Master.FindControl("lblChBranch")).Text = "";
            btnLog.Focus();
            this.GetModuleByRole(mnuID.mnuUser);
            if (!IsPostBack)
            {
                Control lblSearch = Page.Master.FindControl("lblSearch");
                lblSearch.Visible = false;

                try
                {
                    PopBranch(Session[gblValue.UserName].ToString());                   
                    txtDate.Text = Session[gblValue.LoginDate].ToString();
                    hdnUser.Value = Session[gblValue.UserId].ToString();
                    txtDate.Attributes.Add("onKeyPress", "doClick('" + btnLog.ClientID + "',event)");
                   // txtDate.Attributes.Add("onClick", "doClick('" + btnLog.ClientID + "',event)");
                    Session["MnuId"] = "Init";

                    Session["LoginYN"] = "Y"; //set value after successful login
                    //Response.Cookies["LoginYN"].Value = "Y";

                    divLogin.Visible = false;
                    divRegister.Visible = false;
                    ddlBranch.Focus();
                    //Reporting Purpose
                    Session[gblValue.RptCollectionEfficiency] = "50";
                    Session[gblValue.LoanDisbHO] = "50";
                    Session[gblValue.LoanStatusHO] = "50";
                    Session[gblValue.RptDayWiseDPD] = "50";
                    Session[gblValue.RptOCRLog] = "50";
                    Session[gblValue.RptOnTimeCollection] = "50";
                    Session[gblValue.RptRBIMaturity] = "50";
                    Session[gblValue.RptFunder] = "50";
                    Session[gblValue.RptLoanCollection] = "50";
                    Session[gblValue.RptPAR] = "50";
                    Session[gblValue.RptPortfolio] = "50";
                    Session[gblValue.RptRunDown] = "50";
                    Session[gblValue.NeftAPI] = "50";
                    Session[gblValue.HOTrial] = "50";
                    Session[gblValue.RptIncentive] = "50";
                    Session[gblValue.RptLoanSanctionLog] = "50";
                    Session[gblValue.RptProsidexLog] = "50";
                    Session[gblValue.RptGST] = "50";
                    Session[gblValue.RptHouseVisit] = "50";
                    Session[gblValue.RptPSL] = "50";
                    Session[gblValue.RptUdyamDownload] = "50";
                    Session[gblValue.RptIBPSL] = "50";
                    Session[gblValue.rptHospiCashClaimNew] = "50";
                    Session[gblValue.rptDiscrepancy] = "50";
                    Session[gblValue.RptOD] = "50";
                    Session[gblValue.RptGuarScheme] = "50";
                    Session[gblValue.RptAdvAdj] = "50";
                    Session[gblValue.RptGLReport] = "50";
                    Session[gblValue.RptOTSMasterCheckReport] = "50";
                    Session[gblValue.RptHOCenterWiseCustomerDtlReport] = "50";
                    Session[gblValue.RptHOAccLedgerDtlReport] = "50";
                    Session[gblValue.RptCreditSanction] = "50";
                    Session[gblValue.RptRBIDataIndent] = "50";
                    Session[gblValue.RptWOffColl] = "50";
                    //
                }
                catch 
                {
                    Response.Redirect("~/Login.aspx", false);
                }
            }
        }

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUserLogin(pUser, Convert.ToInt32(Session[gblValue.RoleId]), "R");
                Session[gblValue.AreaID] = dt.Rows[0]["AreaID"].ToString();
                //Session[gblValue.StateID] = dt.Rows[0]["StateID"].ToString();
                ViewState["Branch"] = dt;
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                    ListItem liSel = new ListItem("<--- Select Branch --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
                }
                else
                {
                    ListItem liSel = new ListItem("<--- Select Branch --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session[gblValue.BrnchCode] = ddlBranch.SelectedValue;
            Session[gblValue.BrName] = ddlBranch.SelectedItem.Text;
            PopFinYear();
            DataTable dt = (DataTable)ViewState["Branch"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["BranchCode"].ToString() == ddlBranch.SelectedValue)
                {
                    Session[gblValue.StateID] = dt.Rows[i]["StateID"].ToString();
                    Session[gblValue.CbType] = dt.Rows[i]["CbType"].ToString();
                    Session[gblValue.BCBranchYN] = dt.Rows[i]["BCBranchYN"].ToString();
                    Session[gblValue.ParentBranchCode] = dt.Rows[i]["ParentBranchCode"].ToString();
                    Session[gblValue.ParentBranchName] = dt.Rows[i]["ParentBranchName"].ToString();
                }
            }
            string vFinYr = gblFuction.getFinYrNo(ddlFinYr.SelectedValue.ToString());
            Session[gblValue.ACVouMst] = gblValue.ACVouMst + vFinYr;
            Session[gblValue.ACVouDtl] = gblValue.ACVouDtl + vFinYr;
            Session[gblValue.ACVouMst] = gblValue.ACVouMst + gblFuction.getFinYrNo(ddlFinYr.SelectedValue.ToString());
            Session[gblValue.ACVouDtl] = gblValue.ACVouDtl + gblFuction.getFinYrNo(ddlFinYr.SelectedValue.ToString());
            Session[gblValue.FinYear] = ddlFinYr.SelectedItem.Text;
            Session[gblValue.FinYrNo] = ddlFinYr.SelectedValue;
                 
        }

        //private void PopDistrict(string vBrCode)
        //{
        //    DataTable dt = null;
        //    CTehsil oThl = null;
        //    //string vBrCode = Session[gblValue.BrnchCode].ToString();
        //    try
        //    {
        //        oThl = new CTehsil();
        //        dt = oThl.GetDistbyBranch(vBrCode);
        //        if (dt.Rows.Count > 0)
        //        {
        //            Session[gblValue.DistrictId] = dt.Rows[0]["DistrictId"];
        //        }
        //        else
        //        {
        //            Response.Redirect("~/Private/WebPages/Admin/FinancialYear.aspx", false);
        //        }
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oThl = null;
        //    }
        //}

        private void PopFinYear()
        {
            DataTable dt = null;
            CFinYear oFinYr = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oFinYr = new CFinYear();
                dt = oFinYr.GetFinYearList(vBrCode);
                if (dt.Rows.Count > 0)
                {
                    ddlFinYr.DataTextField = "FYear";
                    ddlFinYr.DataValueField = "YrNo";
                    ddlFinYr.DataSource = dt;
                    ddlFinYr.DataBind();
                    //ListItem liSel = new ListItem("<- Select ->", "-1");
                    //ddlFinYr.Items.Insert(0, liSel);
                    Session[gblValue.DistrictId]=dt.Rows[0]["AreaID"];
                    Session[gblValue.AgencyType] = dt.Rows[0]["AgencyType"];
                }
                else
                {
                    Response.Redirect("~/WebPages/Public/FinYear.aspx", false);
                }
            }
            finally
            {
                dt = null;
                oFinYr = null;
            }
        }

        protected void btnLog_Click(object sender, EventArgs e)
        {
            GetFinYear();
            DataTable dt = null, dtBrCntrl = null;
            CDayEnd oDE = null;
            Int32 vMonth = 0, vDay = 0, vRest = 0, vLstDayEnd = 0, vLstDayBegin = 0;
            Session[gblValue.LoginDate] = txtDate.Text;
            DateTime vLogDate = gblFuction.setDate(txtDate.Text);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            int pRec = 0;
            CUser oUsr = null;
            try
            {
                if (ValidateYear() == false) return;
                oDE = new CDayEnd();
                dt = oDE.GetLastEndDayDate(Session[gblValue.BrnchCode].ToString());
                if (dt.Rows.Count > 0)
                {
                    DateTime LstEndDt = gblFuction.setDate(Convert.ToString(dt.Rows[0]["EndDate"]));
                    vMonth = gblFuction.setDate(txtDate.Text).Month;
                    vDay = gblFuction.setDate(txtDate.Text).Day;
                    //if (vMonth == 4 && vDay == 1)
                    //{
                    //    vRest = oDE.DayEndProcess(this.UserID, vBrCode, LstEndDt);
                    //}
                    vLstDayEnd = oDE.GetLastDayEnd(vLogDate, vBrCode);
                    if (vLstDayEnd == 1)
                    {
                        //Response.Redirect("~/WebPages/Private/Admin/DayEnd.aspx");
                        gblFuction.AjxMsgPopup("Please Run Day End for the date " + Convert.ToString(dt.Rows[0]["EndDate"]) + " from System --> DayEnd.");
                        return;
                        //Response.Redirect("~/Jagaran.aspx");

                    }
                    vLstDayBegin = oDE.GetLastDayBegin(vLogDate, vBrCode);
                    if (vLstDayBegin == 1)
                    {
                        //Response.Redirect("~/WebPages/Private/Admin/DayEnd.aspx");
                        gblFuction.AjxMsgPopup("Please Run Day Begin for the date from System --> Day Begin.");
                        return;
                        //Response.Redirect("~/Jagaran.aspx");

                    }
                    Session[gblValue.EndDate] = gblFuction.putStrDate(LstEndDt.AddDays(-1));
                }

                dtBrCntrl = oDE.GetBranchCtrlByBranchCode(Session[gblValue.BrnchCode].ToString(), vLogDate);
                Session["BrCntrl"] = dtBrCntrl;
                //if (vBrCode == "0000")
                //    Session[gblValue.EndDate] = gblFuction.setDate(Session[gblValue.LoginDate].ToString()).AddDays(-1).ToString("dd/MM/yyyy");
                Session["MnuId"] = "Deft";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                oUsr = new CUser();
                pRec = oUsr.LoginCheck(Convert.ToInt32(Session[gblValue.UserId].ToString()), vBrCode);
                if (pRec == 0)
                {
                    divLogin.Visible = true;
                    divRegister.Visible = true;
                    mp2.Hide();
                    mp1.Show();
                }
                else
                {
                    if (pRec == 1)
                    {
                        divLogin.Visible = true;
                        divRegister.Visible = true;
                        mp1.Hide();
                        mp2.Show();
                    }
                    else
                    {
                        Response.Redirect("~/WebPages/Public/Main.aspx", false);
                    }
                }
               
            }
            finally
            {
                dt = null;
                oDE = null;
            }
        }

        private Boolean ValidateYear()
        {
            Boolean vRest = true;
            CFinYear oFy = new CFinYear();
            DateTime vLgDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            Int32 vYrNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (txtDate.Text == null || txtDate.Text == "")
                vRest = false;
            if (txtDate.Text.Trim() != "")
            {
                vRest = gblFuction.IsDate(txtDate.Text);
                if (vRest == true)
                {
                    if (oFy.ChkFinancialYear(vLgDt, vYrNo, vBrCode) == 0)
                    {
                        gblFuction.MsgPopup("Selected Date Is Not Login Financial Year.");
                        gblFuction.focus("ctl00_cph_Main_ddlFinYr");
                        vRest = false;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("Selected Date Is Not Valid Date.");
                    gblFuction.focus("ctl00_cph_Main_txtDate");
                    vRest = false;
                }
            }
            return vRest;
        }

        protected void ddlFinYr_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetFinYear();
            //           
            string vFinYr = gblFuction.getFinYrNo(ddlFinYr.SelectedValue.ToString());
            Session[gblValue.ACVouMst] = gblValue.ACVouMst + vFinYr;
            Session[gblValue.ACVouDtl] = gblValue.ACVouDtl + vFinYr;
            Session[gblValue.ACVouMst] = gblValue.ACVouMst + gblFuction.getFinYrNo(ddlFinYr.SelectedValue.ToString());
            Session[gblValue.ACVouDtl] = gblValue.ACVouDtl + gblFuction.getFinYrNo(ddlFinYr.SelectedValue.ToString());
            Session[gblValue.FinYear] = ddlFinYr.SelectedItem.Text;
            Session[gblValue.FinYrNo] = ddlFinYr.SelectedValue;
        }

        private void GetFinYear()
        {
            DataTable dt = null;
            CFinYear oFinYr = null;
            try
            {
                string vYear = ddlFinYr.SelectedItem.Text;
                oFinYr = new CFinYear();
                dt = oFinYr.GetFinYearByYear(vYear);
                if (dt.Rows.Count > 0)
                {
                    Session[gblValue.FinFromDt] = Convert.ToString(dt.Rows[0]["SDT"]);
                    Session[gblValue.FinToDt] = Convert.ToString(dt.Rows[0]["EDT"]);
                    Session[gblValue.ShortYear] = Convert.ToString(dt.Rows[0]["ShortYear"]);
                }
                else
                {
                    Session[gblValue.FinFromDt] = "";
                    Session[gblValue.FinToDt] = "";
                    Session[gblValue.ShortYear] = "";
                }
            }
            finally
            {
                dt = null;
                oFinYr = null;
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Int32 vErr = 0;
            CUser oUser = null;
            string vPassL = "", vPassR = "";
            Int32 vUserId = Convert.ToInt32(Session[gblValue.UserId].ToString());

            try
            {
                vPassL = hdnL.Value;
                vPassR = hdnR.Value;
                oUser = new CUser();
                vErr = oUser.Update_BioImg(vUserId, vPassL, vPassR);
                if (vErr == 0)
                {
                    gblFuction.MsgPopup("Finger Image Capture");
                    Response.Redirect("~/WebPages/Public/Main.aspx", false);
                }
                else if (vErr == 1)
                {
                    gblFuction.MsgPopup(gblMarg.DBError);

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnImgClose_Click(object sender, ImageClickEventArgs e)
        {
            mp1.Hide();
        }

        protected void btnLogImg_Click(object sender, ImageClickEventArgs e)
        {
            mp2.Hide();
        }

        protected void btnLogDone_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Match();", true);
            if ((Request[hdnStatus.UniqueID] as string == null ? hdnStatus.Value : Request[hdnStatus.UniqueID] as string) == "0")
            {
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }

        }
    }
}