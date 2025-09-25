using System;
using System.Data;
using FORCECA;
using FORCEBA;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Configuration;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class rptExcessLedger : CENTRUMBase
    {
        private static string vSrvName = ConfigurationManager.AppSettings["SrvName"];
        private static string vDBName = ConfigurationManager.AppSettings["DBName"];
        private static string vPw = ConfigurationManager.AppSettings["PassPW"];
        private static string vReportUrl = ConfigurationManager.AppSettings["ReportUrl"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch(Session[gblValue.UserName].ToString());
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                PopState();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Excess Ledger Report";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuExcessLedger);
                if (this.RoleId == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Excess Ledger Report", false);
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
        private void GetBranch()
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
            {
                if (chkBr.Items[vRow].Selected == true)
                {
                    if (strin == "")
                        strin = chkBr.Items[vRow].Value;
                    else
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                }
            }
            ViewState["ID"] = strin;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //private void PopBranch()
        //{
        //    //Int32 vBrId = 0;
        //    //string vBrCode = "";
        //    ViewState["ID"] = null;
        //    DataTable dt = null;
        //    CGblIdGenerator oCG = null;
        //    try
        //    {
        //        DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //        //vBrCode = (string)Session[gblValue.BrnchCode];
        //        //vBrId = Convert.ToInt32(vBrCode);
        //        oCG = new CGblIdGenerator();
        //        dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, "0000");
        //        chkBr.DataSource = dt;
        //        chkBr.DataTextField = "Name";
        //        chkBr.DataValueField = "BranchCode";
        //        chkBr.DataBind();
        //        CheckAll();
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oCG = null;
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUser"></param>
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
            ViewState["ID"] = null;
            try
            {

                if (dt.Rows.Count > 0)
                {
                    chkBr.DataSource = dt;
                    chkBr.DataTextField = "BranchName";
                    chkBr.DataValueField = "BranchCode";
                    chkBr.DataBind();
                    CheckAll();
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void CheckAll()
        {
            Int32 vRow;
            string strin = "";
            if (ddlSel.SelectedValue == "C")
            {
                chkBr.Enabled = false;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                {
                    chkBr.Items[vRow].Selected = true;
                    if (strin == "")
                        strin = chkBr.Items[vRow].Value;
                    else
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                }
                ViewState["ID"] = strin;
            }
            else if (ddlSel.SelectedValue == "B")
            {
                ViewState["ID"] = null;
                chkBr.Enabled = true;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                    chkBr.Items[vRow].Selected = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFormat"></param>
        private void GetData(string pFormat)
        {
            GetBranch();

            string vBrCode = Convert.ToString(ViewState["ID"]);

            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\Trial.rpt";
            string vBranch = Session[gblValue.BrName].ToString();
            string vAcMst = Session[gblValue.ACVouMst].ToString();
            string vAcDtl = Session[gblValue.ACVouDtl].ToString();
            string vRptType = "T";
            if (rbDtlsSumm.SelectedValue == "1")
                vRptType = "S";

            
            DataTable dt = null;
            //ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();
            //rptDoc.Load(vRptPath);
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());

            if (vFinFrom > gblFuction.setDate(txtFrmDt.Text) || gblFuction.setDate(txtFrmDt.Text) > vFinTo)
            {
                gblFuction.MsgPopup("From Date should be within this financial year.");
                return;
            }
            if (vFinFrom > gblFuction.setDate(txtToDt.Text) || gblFuction.setDate(txtToDt.Text) > vFinTo)
            {
                gblFuction.MsgPopup("To date should be within this financial year.");
                return;
            }

            //TimeSpan t = gblFuction.setDate(txtToDt.Text) - gblFuction.setDate(txtFrmDt.Text);
            //if (t.TotalDays > 2)
            //{
            //    gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
            //    return;
            //}

            dt = oRpt.RptExcessAmount(vAcMst, vAcDtl, gblFuction.setStrDate(txtFrmDt.Text), gblFuction.setStrDate(txtToDt.Text), vBrCode);
            if (pFormat == "PDF")
            {
                
            }
            else if (pFormat == "Excel")
            {
                string vFileNm = "attachment;filename=" + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_Consolidate_Trial_Balance.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", vFileNm);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                Response.Write("<table border='1' cellpadding='5' widht='120%'>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> Excess Ledger Report </font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>Date From:  - " + txtFrmDt.Text + " To:" + txtToDt.Text + "</font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'></font></b></td></tr>");
                string tab = string.Empty;
                Response.Write("<tr>");
                foreach (DataColumn dtcol in dt.Columns)
                {
                    Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
                }
                Response.Write("</tr>");
                foreach (DataRow dtrow in dt.Rows)
                {
                    Response.Write("<tr style='height:20px;'>");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName == "BranchCode" || dt.Columns[j].ColumnName == "MemberNo" || dt.Columns[j].ColumnName == "Mobile No" || dt.Columns[j].ColumnName == "LoanNo" || dt.Columns[j].ColumnName == "AccNo")
                        {
                            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                        }
                        else
                        {
                            Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
                        }
                        
                    }
                    Response.Write("</tr>");
                }
                
                Response.Write("</table>");
                Response.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidateDate()
        {
            bool vRst = true;
            return vRst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            if (ValidateDate() == false)
            {
                gblFuction.MsgPopup("Please Set The Valid Date Range.");
                return;
            }
            else
            {
                GetData("PDF");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            if (ValidateDate() == false)
            {
                gblFuction.MsgPopup("Please Set The Valid Date Range.");
                return;
            }
            else
            {
                GetData("Excel");
                
            }
        }

        protected void btnOnDateExcl_Click(object sender, EventArgs e)
        {
            if (ValidateDate() == false)
            {
                gblFuction.MsgPopup("Please Set The Valid Date Range.");
                return;
            }
            else
            {
                GetData("Excel");
                
            }
        }

        private void PopState()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            oCG = new CGblIdGenerator();
            dt = oCG.PopComboMIS("N", "N", "AA", "StateId", "StateName", "StateMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
            ddlState.DataSource = dt;
            ddlState.DataTextField = "StateName";
            ddlState.DataValueField = "StateId";
            ddlState.DataBind();
        }

        protected void btnStateWise_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CUser oUsr = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            string pUser = Session[gblValue.UserName].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            chkBr.Items.Clear();
            ViewState["Id"] = null;
            try
            {
                oCG = new CGblIdGenerator();
                oUsr = new CUser();
                ViewState["Id"] = null;
                if (vBrCode == "0000")
                {
                    dt = oUsr.GetBranchByState(pUser, Convert.ToInt32(Session[gblValue.RoleId]), ddlState.SelectedValues.Replace("|", ","), ddlBrType.SelectedValue);
                    if (dt.Rows.Count > 0)
                    {
                        chkBr.DataSource = dt;
                        chkBr.DataTextField = "BranchName";
                        chkBr.DataValueField = "BranchCode";
                        chkBr.DataBind();
                        if (ddlSel.SelectedValue == "C")
                            CheckBrAll();
                    }
                }
                else
                {
                    dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, vBrCode);
                    chkBr.DataSource = dt;
                    chkBr.DataTextField = "Name";
                    chkBr.DataValueField = "BranchCode";
                    chkBr.DataBind();
                    if (ddlSel.SelectedValue == "C")
                        CheckBrAll();
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
                oCG = null;
            }

        }

        private void CheckBrAll()
        {
            Int32 vRow;
            string strin = "";
            if (ddlSel.SelectedValue == "C")
            {
                chkBr.Enabled = false;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                {
                    chkBr.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkBr.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                    }
                }
            }
            else if (ddlSel.SelectedValue == "B")
            {
                ViewState["BrCode"] = null;
                chkBr.Enabled = true;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                    chkBr.Items[vRow].Selected = false;
            }
            ViewState["BrCode"] = strin;
        }

        protected void chkBrDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
            {
                if (chkBr.Items[vRow].Selected == true)
                {
                    if (strin == "")
                    {
                        strin = chkBr.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                    }
                }
            }
            ViewState["BrCode"] = strin;
        }

        private void GenerateRptData(string pMode, string ptype)
        {
            GetBranch();

            string vBrCode = Convert.ToString(ViewState["ID"]);

            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\Trial.rpt";
            string vBranch = Session[gblValue.BrName].ToString();
            string vAcMst = Session[gblValue.ACVouMst].ToString();
            string vAcDtl = Session[gblValue.ACVouDtl].ToString();
            string vRptType = "T";
            if (rbDtlsSumm.SelectedValue == "1")
                vRptType = "S";

            Int32 vFinYr = Convert.ToInt32(Session[gblValue.FinYrNo]);
            DataTable dt = null;
            //ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();
            //rptDoc.Load(vRptPath);
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());

            if (vFinFrom > gblFuction.setDate(txtFrmDt.Text) || gblFuction.setDate(txtFrmDt.Text) > vFinTo)
            {
                gblFuction.MsgPopup("From Date should be within this financial year.");
                return;
            }
            if (vFinFrom > gblFuction.setDate(txtToDt.Text) || gblFuction.setDate(txtToDt.Text) > vFinTo)
            {
                gblFuction.MsgPopup("To date should be within this financial year.");
                return;
            }
            var req = new HoTrialCentrum
            {
                TableMst = vAcMst,
                TableDtl = vAcDtl,
                FinFrom = Session[gblValue.FinFromDt].ToString(),
                DateFrom = txtFrmDt.Text,
                DateTo = txtToDt.Text,
                Branch = vBrCode,
                YearNo = Convert.ToString(vFinYr),
                pFormat = pMode,
                pUserId = Convert.ToString(Session[gblValue.UserId]),
                pDBName = vDBName,
                pPassword = vPw,
                pServerIP = vSrvName,
                pCompanyName = gblValue.CompName,
                pCompanyAddress = CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()),
                pType = ptype
            };
            string Requestdata = JsonConvert.SerializeObject(req);
            CApiCalling API = new CApiCalling();
            string vMsg = API.GenerateReport("GenerateHoTrialCentrum", Requestdata, vReportUrl);
            gblFuction.AjxMsgPopup(vMsg);

            btnExcl.Enabled = false;

        }


    }



    
}
