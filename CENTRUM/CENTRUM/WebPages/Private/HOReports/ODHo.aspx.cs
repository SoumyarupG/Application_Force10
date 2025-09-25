using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using FORCECA;
using FORCEBA;
using System.Configuration;
using Newtonsoft.Json;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class ODHo : CENTRUMBase
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
                txtDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                popRO();
                PopBranch();
                PopLoanType();
                PopState();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Overdue";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHOOvrDueRpt);
                if (this.UserID == 1) return;
                //if (this.CanReport == "Y")
                //{
                //}
                //else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                //{
                //    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Repayment Schedule", false);
                //}
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oRO = new CEO();
                //dt = oCM.GetCOPop(vBrCode, "SCO,CO,TCO,JTCO,GO");
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlCo.DataSource = dt;
                ddlCo.DataTextField = "EoName";
                ddlCo.DataValueField = "EoId";
                ddlCo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCo.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

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

        private void PopLoanType()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            oCG = new CGblIdGenerator();
            dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanType", "LoanTypeMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
            ddlLoanType.DataSource = dt;
            ddlLoanType.DataTextField = "LoanType";
            ddlLoanType.DataValueField = "LoanTypeId";
            ddlLoanType.DataBind();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            string vFileNm = "", vRoID = "";
            DataSet ds = null;
            string vBrCode = ViewState["BrCode"].ToString();
            CReports oRpt = new CReports();
            DateTime vToDt = gblFuction.setDate(txtDt.Text);
            if (ddlCo.SelectedIndex > 0) vRoID = ddlCo.SelectedValue;

            #region OLD
            ////***************************************
            //System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            //ds = oRpt.rptOverDue(vToDt, vRoID, vBrCode, ddlLoanType.SelectedValues.Replace("|", ","));
            //DataGrid1.DataSource = ds;
            //DataGrid1.DataBind();
            //DataTable dt = null;
            //dt = ds.Tables[0];

            //tdx.Controls.Add(DataGrid1);
            //tdx.Visible = false;
            //vFileNm = "attachment;filename=Overdue_Report.xls";
            //Response.ClearContent();
            //Response.AddHeader("content-disposition", vFileNm);
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "application/vnd.ms-excel";
            //HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
            //Response.Write("<table border='1' cellpadding='0' widht='100%'>");
            ////htw.WriteLine("<tr><td align=center' colspan='13'><b><u><font size='3'>Party Ledger</font></u></b></td></tr>");
            ////htw.WriteLine("<tr><td colspan='2'><b>Member No:</b></td><td align='left' colspan='3'><b>" + vMemNo + "</b></td><td colspan='2'><b>Member Name:</b></td><td align='left' colspan='4'><b>" + vMemName + "</b></td></tr>");
            ////htw.WriteLine("<tr><td colspan='2'><b>Spouce Name:</b></td><td align='left' colspan='3'><b>" + vSpouseNm + "</b></td><td colspan='2'><b>Fund Source:</b></td><td align='left' colspan='4'><b>" + vFundSource + "</b></td></tr>");
            ////htw.WriteLine("<tr><td colspan='2'><b>Loan No:</b></td><td align='left' colspan='3'><b>" + vLnNo + "</b></td><td colspan='2'><b>Purpose:</b></td><td align='left' colspan='4'><b>" + vPurpose + "</b></td></tr>");
            ////htw.WriteLine("<tr><td colspan='2'><b>Loan Amount:</b></td><td align='left' colspan='3'><b>" + vLoanAmt + "</b></td><td colspan='2'><b>Interest Amount:</b></td><td align='left' colspan='4'><b>" + vIntAmt + "</b></td></tr>");
            ////htw.WriteLine("<tr><td colspan='2'><b>Disburse Date:</b></td><td align='left' colspan='3'><b>" + vDisbDt + "</b></td><td colspan='2'><b>Loan Scheme:</b></td><td align='left' colspan='4'><b>" + vLnProduct + "</b></td></tr>");
            ////htw.WriteLine("<tr><td align=center' colspan='5'><b><u><font size='3'>Repayment Schedule</font></u></b></td><td align=center' colspan='7'><b><u><font size='3'>Collection Details</font></u></b></td></tr>");
            //string tab = string.Empty;
            //Response.Write("<tr>");
            //foreach (DataColumn dtcol in dt.Columns)
            //{
            //    Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
            //}
            //Response.Write("</tr>");
            //foreach (DataRow dtrow in dt.Rows)
            //{
            //    Response.Write("<tr style='height:20px;'>");
            //    for (int j = 0; j < dt.Columns.Count; j++)
            //    {

            //        if (dt.Columns[j].ColumnName == "Loan No")
            //        {
            //            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
            //        }
                    
            //        else
            //        {
            //            Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
            //        }
            //    }
            //    Response.Write("</tr>");
            //}
            //Response.Write("</table>");
            //Response.End();
            #endregion
            //------------------Generate Report from Bijli API------------------------
            var req = new ODRequest()
            {
                pToDt = txtDt.Text,
                pEoID = vRoID,
                pBranch = vBrCode,
                pLoanType = ddlLoanType.SelectedValues.Replace("|", ","),
                pFormat = "Excel",
                pUserId = Convert.ToString(Session[gblValue.UserId]),
                pDBName = vDBName,
                pPassword = vPw,
                pServerIP = vSrvName,
                pCompanyName = gblValue.CompName,
                pCompanyAddress = CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString())
            };
            string Requestdata = JsonConvert.SerializeObject(req);
            Int32 RptOD = Convert.ToInt32(Session[gblValue.RptOD].ToString());
            if (RptOD != 0)
            {
                Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                if (unixTicks - RptOD > 300)
                {
                    Session[gblValue.RptOD] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    GenerateReport("GenerateOverDue", Requestdata);
                }
                else
                {
                    gblFuction.AjxMsgPopup("Already Report Generate Request Is Executing ...Please Wait For 5 Mins..And Re Generate..");
                }
            }
            else
            {
                Session[gblValue.RptOD] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                GenerateReport("GenerateOverDue", Requestdata);
            }
            //------------------------------------------------------------------------------
        }

        protected void btnCSV_Click(object sender, EventArgs e)
        {
            string vFileNm = "", vRoID = "";
            DataSet ds = null;
            string vBrCode = ViewState["BrCode"].ToString();
            CReports oRpt = new CReports();
            DateTime vToDt = gblFuction.setDate(txtDt.Text);
            if (ddlCo.SelectedIndex > 0) vRoID = ddlCo.SelectedValue;

            #region OLD
            ////***************************************
            //System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            //ds = oRpt.rptOverDue(vToDt, vRoID, vBrCode, ddlLoanType.SelectedValues.Replace("|", ","));
            //DataGrid1.DataSource = ds;
            //DataGrid1.DataBind();
            //DataTable dt = null;
            //dt = ds.Tables[0];

            //tdx.Controls.Add(DataGrid1);
            //tdx.Visible = false;
            //vFileNm = "attachment;filename=Overdue_Report.xls";
            //Response.ClearContent();
            //Response.AddHeader("content-disposition", vFileNm);
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "application/vnd.ms-excel";
            //HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
            //Response.Write("<table border='1' cellpadding='0' widht='100%'>");
            ////htw.WriteLine("<tr><td align=center' colspan='13'><b><u><font size='3'>Party Ledger</font></u></b></td></tr>");
            ////htw.WriteLine("<tr><td colspan='2'><b>Member No:</b></td><td align='left' colspan='3'><b>" + vMemNo + "</b></td><td colspan='2'><b>Member Name:</b></td><td align='left' colspan='4'><b>" + vMemName + "</b></td></tr>");
            ////htw.WriteLine("<tr><td colspan='2'><b>Spouce Name:</b></td><td align='left' colspan='3'><b>" + vSpouseNm + "</b></td><td colspan='2'><b>Fund Source:</b></td><td align='left' colspan='4'><b>" + vFundSource + "</b></td></tr>");
            ////htw.WriteLine("<tr><td colspan='2'><b>Loan No:</b></td><td align='left' colspan='3'><b>" + vLnNo + "</b></td><td colspan='2'><b>Purpose:</b></td><td align='left' colspan='4'><b>" + vPurpose + "</b></td></tr>");
            ////htw.WriteLine("<tr><td colspan='2'><b>Loan Amount:</b></td><td align='left' colspan='3'><b>" + vLoanAmt + "</b></td><td colspan='2'><b>Interest Amount:</b></td><td align='left' colspan='4'><b>" + vIntAmt + "</b></td></tr>");
            ////htw.WriteLine("<tr><td colspan='2'><b>Disburse Date:</b></td><td align='left' colspan='3'><b>" + vDisbDt + "</b></td><td colspan='2'><b>Loan Scheme:</b></td><td align='left' colspan='4'><b>" + vLnProduct + "</b></td></tr>");
            ////htw.WriteLine("<tr><td align=center' colspan='5'><b><u><font size='3'>Repayment Schedule</font></u></b></td><td align=center' colspan='7'><b><u><font size='3'>Collection Details</font></u></b></td></tr>");
            //string tab = string.Empty;
            //Response.Write("<tr>");
            //foreach (DataColumn dtcol in dt.Columns)
            //{
            //    Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
            //}
            //Response.Write("</tr>");
            //foreach (DataRow dtrow in dt.Rows)
            //{
            //    Response.Write("<tr style='height:20px;'>");
            //    for (int j = 0; j < dt.Columns.Count; j++)
            //    {

            //        if (dt.Columns[j].ColumnName == "Loan No")
            //        {
            //            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
            //        }

            //        else
            //        {
            //            Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
            //        }
            //    }
            //    Response.Write("</tr>");
            //}
            //Response.Write("</table>");
            //Response.End();
            #endregion
            //------------------Generate Report from Bijli API------------------------
            var req = new ODRequest()
            {
                pToDt = txtDt.Text,
                pEoID = vRoID,
                pBranch = vBrCode,
                pLoanType = ddlLoanType.SelectedValues.Replace("|", ","),
                pFormat = "CSV",
                pUserId = Convert.ToString(Session[gblValue.UserId]),
                pDBName = vDBName,
                pPassword = vPw,
                pServerIP = vSrvName,
                pCompanyName = gblValue.CompName,
                pCompanyAddress = CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString())
            };
            string Requestdata = JsonConvert.SerializeObject(req);
            Int32 RptOD = Convert.ToInt32(Session[gblValue.RptOD].ToString());
            if (RptOD != 0)
            {
                Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                if (unixTicks - RptOD > 300)
                {
                    Session[gblValue.RptOD] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    GenerateReport("GenerateOverDue", Requestdata);
                }
                else
                {
                    gblFuction.AjxMsgPopup("Already Report Generate Request Is Executing ...Please Wait For 5 Mins..And Re Generate..");
                }
            }
            else
            {
                Session[gblValue.RptOD] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                GenerateReport("GenerateOverDue", Requestdata);
            }
            //------------------------------------------------------------------------------
        }


        private void GenerateReport(string pApiName, string pRequestdata)
        {
            string vMsg = "";
            CApiCalling oAPI = new CApiCalling();
            try
            {
                vMsg = oAPI.GenerateReport(pApiName, pRequestdata, vReportUrl);
            }
            finally
            {
                gblFuction.AjxMsgPopup(vMsg);
                btnExcl.Enabled = false;
            }
        }

        private void PopBranch()
        {
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            DataTable dt = null;
            CUser oUsr = null;
            //string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));

            chkBrDtl.DataSource = dt;
            chkBrDtl.DataTextField = "BranchName";
            chkBrDtl.DataValueField = "BranchCode";
            chkBrDtl.DataBind();

            if (rblAlSel.SelectedValue == "rbAll")
            {
                chkBrDtl.Enabled = false;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = false;
                }
            }
            ViewState["BrCode"] = strin;
        }

        private void CheckBrAll()
        {
            Int32 vRow;
            string strin = "";
            if (rblAlSel.SelectedValue == "rbAll")
            {
                chkBrDtl.Enabled = false;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
                ViewState["BrCode"] = strin;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                ViewState["BrCode"] = null;
                chkBrDtl.Enabled = true;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    chkBrDtl.Items[vRow].Selected = false;

            }
        }

        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBrAll();
        }

        protected void chkBrDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
            {
                if (chkBrDtl.Items[vRow].Selected == true)
                {
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
            }
            ViewState["BrCode"] = strin;
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
            DateTime vLogDt = gblFuction.setDate(txtDt.Text.ToString());
            chkBrDtl.Items.Clear();
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
                        chkBrDtl.DataSource = dt;
                        chkBrDtl.DataTextField = "BranchName";
                        chkBrDtl.DataValueField = "BranchCode";
                        chkBrDtl.DataBind();
                        if (rblAlSel.SelectedValue == "rbAll")
                            CheckBrAll();
                    }
                }
                else
                {
                    dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, vBrCode);
                    chkBrDtl.DataSource = dt;
                    chkBrDtl.DataTextField = "Name";
                    chkBrDtl.DataValueField = "BranchCode";
                    chkBrDtl.DataBind();
                    if (rblAlSel.SelectedValue == "rbAll")
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
    }

    public class ODRequest
    {
        public string pToDt { get; set; }
        public string pEoID { get; set; }
        public string pBranch { get; set; }
        public string pLoanType { get; set; }
        public string pFormat { get; set; }
        public string pUserId { get; set; }
        public string pDBName { get; set; }
        public string pServerIP { get; set; }
        public string pPassword { get; set; }
        public string pCompanyName { get; set; }
        public string pCompanyAddress { get; set; }
    }
}
