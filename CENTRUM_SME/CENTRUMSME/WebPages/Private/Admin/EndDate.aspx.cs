using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.Private.Webpages.Admin
{
    public partial class EndDate : CENTRUMBAse
    {
        DateTime LstEndDt = System.DateTime.Now;
        public static DataTable dtDayEnd = new DataTable();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //InitBasePage();
            DataTable dt = null;
            CDayEnd oDay = null;
            if (!IsPostBack)
            {
                try
                {
                    if (Session[gblValue.BrnchCode].ToString() != "0000")
                    {
                        dvDayEnd.Visible = false;
                        gvCB.Visible = false;
                        txtEndDt.Visible = true;
                        btnProc.Visible = true;
                        oDay = new CDayEnd();
                        dt = oDay.GetLastEndDayDate(Session[gblValue.BrnchCode].ToString());
                        if (dt.Rows.Count > 0)
                        {
                            txtEndDt.Text = Convert.ToString(dt.Rows[0]["EndDate"]);
                            LstEndDt = gblFuction.setDate(Convert.ToString(dt.Rows[0]["EndDate"]));
                        }
                        else
                        {
                            txtEndDt.Text = Session[gblValue.LoginDate].ToString();
                            LstEndDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                        }
                    }
                    else
                    {
                        dvDayEnd.Visible = true;
                        gvCB.Visible = true;
                        txtEndDt.Visible = false;
                        btnProc.Visible = false;
                        LoadGrid();
                    }
                }
                finally
                {
                    dt = null;
                    oDay = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        public override void VerifyRenderingInServerForm(Control control)
        {
            base.VerifyRenderingInServerForm(control);
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadGrid()
        {
            DataTable dt = null;
            Int32 vYrNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
            DateTime vFinFrmDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vAsDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CDayEnd oDE = null;
            try
            {
                oDE = new CDayEnd();
                dt = oDE.GetDayEndDatainHO(vFinFrmDt, vAsDt, vYrNo);
                dtDayEnd = dt;
                ViewState["CGT"] = dt;
                gvCB.DataSource = dt;
                gvCB.DataBind();

            }
            finally
            {
                oDE = null;
                dt = null;
            }
        }




        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);
                
                this.Menu = false;
                this.PageHeading = "Day End Process";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDayendProc);

                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    //btnDelete.Visible = false;
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnEdit.Visible = false;
                    //btnDelete.Visible = false;
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnDelete.Visible = false;
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Day End Process", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CheckDayEnd(DateTime pEndDate)
        {
            bool vRest = true;
            CDayEnd oDE = null;
            Int32 vLstDayEnd = 0;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oDE = new CDayEnd();
                vLstDayEnd = oDE.GetLastDayEnd(pEndDate, vBrCode);
                if (vLstDayEnd == 1)
                {
                    gblFuction.MsgPopup(gblPRATAM.DayEndMsg);
                    vRest = false;
                }
                else
                {
                    vRest = true;
                }
                return vRest;
            }
            finally
            {
                oDE = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            string vFileNm = "";
            vFileNm = "attachment;filename=" + (Session[gblValue.LoginDate]).ToString() + "_Day_End.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            dgRight2.DataSource = dtDayEnd;
            dgRight2.DataBind();
            htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan='6'><b><u><font size='5'>Day End Report</font></u></b></td></tr>");
            dgRight2.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
            htw.WriteLine("</table>");
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(sw.ToString());
            Response.End();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            string vBranch = Session[gblValue.BrnchCode].ToString();
            if (txtEndDt.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Process Date Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtEndDt");
                vResult = false;
            }
            if (txtEndDt.Text.Trim() != "")
            {
                if (gblFuction.IsDate(txtEndDt.Text.Trim()) == false)
                {
                    gblFuction.MsgPopup("Please Enter Valid Date...");
                    gblFuction.focus("ctl00_cph_Main_txtEndDt");
                    vResult = false;
                }
            }
            return vResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProc_Click(object sender, EventArgs e)
        {
            Int32 vRst = 0;
            CDayEnd oDE = null;
            DateTime vEndDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vBranch = Session[gblValue.BrnchCode].ToString();
            DateTime vDayEnddt = gblFuction.setDate(txtEndDt.Text);
            try
            {
                if (ValidateFields() == false) return;
                oDE = new CDayEnd();
                vRst = oDE.DayEndProcess(this.UserID, vBranch, vDayEnddt);
                if (vRst == 1)
                {
                    txtEndDt.Text = gblFuction.putStrDate(vDayEnddt.AddDays(1));
                    gblFuction.AjxMsgPopup("Day End Process Completed Successfully.");
                    if (CheckDayEnd(vEndDate) == true)
                    {
                        if (Session["MnuId"] == null)
                        {
                            Session["MnuId"] = "Deft";
                            this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                            this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                            Session[gblValue.EndDate] = gblFuction.putStrDate(vDayEnddt);
                            Response.Redirect("~/WebPages/Public/Main.aspx", false);
                        }
                        else
                        {
                            Session[gblValue.EndDate] = gblFuction.putStrDate(vDayEnddt);
                            Response.Redirect("~/WebPages/Public/Main.aspx", false);
                        }
                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("Error For Processing Day End.");
                }
            }
            finally
            {
                oDE = null;
            }
        }

    }
}