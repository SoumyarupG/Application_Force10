using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;
using System.Web.UI;

namespace CENTRUM_SARALVYAPAR.Public
{
    public partial class FinYear : CENTRUMBAse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Control menu = Page.Master.FindControl("divmenubar");
            Control header = Page.Master.FindControl("spancompname");
            //Control marquee = Page.Master.FindControl("marqueemsg");
            Control submsg = Page.Master.FindControl("lt1");
            Control divMenu = Page.Master.FindControl("divMenu");
            menu.Visible = false;
            header.Visible = false;
            divMenu.Visible = false;
            //marquee.Visible = false;
            submsg.Visible = false;
            Control img3 = Page.Master.FindControl("Img3");
            Control imgCngBr = Page.Master.FindControl("imgCngBr");
            Control img2 = Page.Master.FindControl("Img2");
            Control imgSearch = Page.Master.FindControl("imgSearch");
            img3.Visible = false;
            imgCngBr.Visible = false;
            if (img2 != null)
            {
                img2.Visible = false;
            }
            if (imgSearch != null)
            {
                imgSearch.Visible = false;
            }
            //((LinkButton)Master.FindControl("lblChBranch")).Text = "";
            btnLog.Focus();
            this.GetModuleByRole(mnuID.mnuUser);
            if (!IsPostBack)
            {
                try
                { 
                    PopBranch(Session[gblValue.UserName].ToString());
                    CheckPassExpiry(Session[gblValue.UserName].ToString());
                    string vFinYr = gblFuction.getFinYrNo(ddlFinYr.SelectedValue.ToString());
                    txtDate.Text = Session[gblValue.LoginDate].ToString();
                    Session[gblValue.ACVouMst] = gblValue.ACVouMst + vFinYr;
                    Session[gblValue.ACVouDtl] = gblValue.ACVouDtl + vFinYr;
                    txtDate.Attributes.Add("onKeyPress", "doClick('" + btnLog.ClientID + "',event)");
                    Session[gblValue.NeftAPI] = "50";
                    Session[gblValue.RptInitialStatus] = "50";
                    Session[gblValue.mnuCBDataSubmissionRpt] = "50";
                    Session[gblValue.RptInitialStatusOnDemand] = "50";
                    Session[gblValue.RptInitialReject] = "50";
                    Session["LoginYN"] = "Y";
                }
                catch 
                {
                    Response.Redirect("~/Login.aspx", false);
                   // throw ex;
                }
            }
        }
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();
            dt = oUsr.GetBranchByUserLogin(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
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

            }
        }
        private void CheckPassExpiry(string pUser)
        {
            //lblPassExpMsg.Text = "";
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();
            dt = oUsr.ChkPassExp(pUser);
            if (dt.Rows.Count > 0)
            {
                int ExpDays = Convert.ToInt32(dt.Rows[0]["ExpDays"]);
                if (ExpDays <= 7)
                {
                    //lblPassExpMsg.Text = "Your Password Will Expire With In " + ExpDays.ToString() + " Days. Kindly Change Password.";
                   // lblPassExpMsg.Visible = true;
                }
            }
            else
            {
                //lblPassExpMsg.Visible = false;
            }
        }
        private void PopFinYear()
        {
            DataTable dt = null;
            CFinYear oFinYr =null;
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
                    Session[gblValue.FinYear] = ddlFinYr.SelectedItem.Text;
                    Session[gblValue.FinYrNo] = ddlFinYr.SelectedValue;
                    string vFinYr = gblFuction.getFinYrNo(ddlFinYr.SelectedValue.ToString());
                    Session[gblValue.ACVouMst] = gblValue.ACVouMst + vFinYr;
                    Session[gblValue.ACVouDtl] = gblValue.ACVouDtl + vFinYr;
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
            DataTable dt = null, dtBrCntrl = null;
            Int32 vMonth = 0, vDay = 0, vRest = 0, vLstDayEnd = 0, vLstDayBegin=0;
            DateTime vLogDate = gblFuction.setDate(txtDate.Text);
            CDayEnd oDE = null;

           if (ddlBranch.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Selected a Branch.");
                gblFuction.focus("ctl00_cph_Main_ddlBranch");
                return;
            }            
            string vBrCode = Session[gblValue.BrnchCode].ToString();

            try
            {
                if (ValidateYear() == false) return;
                GetFinYear();
                Session[gblValue.LoginDate] = txtDate.Text;
                oDE = new CDayEnd();
                dt = oDE.GetLastEndDayDate(Session[gblValue.BrnchCode].ToString());
                if (dt.Rows.Count > 0)
                {
                    DateTime LstEndDt = gblFuction.setDate(Convert.ToString(dt.Rows[0]["EndDate"]));
                    //for 01/04 Ist Day End
                    //vMonth = gblFuction.setDate(txtDate.Text).Month;
                    //vDay = gblFuction.setDate(txtDate.Text).Day;
                    //if (vMonth == 4 && vDay == 1)
                    //{
                    //    vRest = oDE.DayEndProcess(this.UserID, vBrCode, LstEndDt);
                    //}
                    vLstDayEnd = oDE.GetLastDayEnd(vLogDate, vBrCode);
                    if (vLstDayEnd == 1)
                    {
                        //Response.Redirect("~/WebPages/Private/Admin/EndDate.aspx");
                        gblFuction.AjxMsgPopup("Please Run Day End for the date " + Convert.ToString(dt.Rows[0]["EndDate"]) + " from System --> DayEnd.");
                        return;
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
                else
                {
                    Session[gblValue.EndDate] = null;
                }

                dtBrCntrl = oDE.GetBranchCtrlByBranchCode(Session[gblValue.BrnchCode].ToString(), vLogDate);
                Session["BrCntrl"] = dtBrCntrl;
                //if (vBrCode == "0000")
                //    Session[gblValue.EndDate] = Session[gblValue.LoginDate];

                ////-----------------------
                Session["MnuId"] = "Deft";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                //Session[gblValue.ClientIP] =  GetUserIP();
                oDE.UpdateUserBranch(vBrCode, this.UserID, "I");
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDE = null;
            }
    
        }
        private Boolean ValidateYear()
        {
            Boolean vRest = true;
            CFinYear oFy = new CFinYear();
            Int32 vYrNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
            
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (txtDate.Text == null || txtDate.Text == "")
                vRest = false;
            if (txtDate.Text.Trim() != "")
            {
                vRest = gblFuction.IsDate(txtDate.Text);
                if (vRest == true)
                {
                    DateTime vLgDt = gblFuction.setDate(txtDate.Text);  
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
                }
            }
        }
        protected void ddlFinYr_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetFinYear();
                Session[gblValue.ACVouMst] = gblValue.ACVouMst + gblFuction.getFinYrNo(ddlFinYr.SelectedValue.ToString());
                Session[gblValue.ACVouDtl] = gblValue.ACVouDtl + gblFuction.getFinYrNo(ddlFinYr.SelectedValue.ToString());
                Session[gblValue.FinYear] = ddlFinYr.SelectedItem.Text;
                Session[gblValue.FinYrNo] = ddlFinYr.SelectedValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
    }
}