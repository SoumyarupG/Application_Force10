using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Collections;
using Newtonsoft.Json;
using System.Configuration;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HOAllInsuranceReport : CENTRUMBase
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
                txtDtFrm.Text = Session[gblValue.FinFromDt].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                PopBranch();
                PopState();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Insurance Master Report";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHOInsuMstRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Insurance Master Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = ViewState["BrCode"].ToString();
            string vBranch = Session[gblValue.BrName].ToString();
            DataTable dt = null;
            CReports oRpt = new CReports();
            string vFileNm = "";
            try
            {
                TimeSpan t = vToDt - vFromDt;
                if (t.TotalDays > 2)
                {
                    gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                    return;
                }

                oRpt = new CReports();
                dt = oRpt.rptInsuranceAutomation(vBrCode, vFromDt, vToDt, rdbOpt.SelectedValue);
                if (dt.Rows.Count > 0)
                {
                    if (pMode == "Excel")
                    {
                        if (rdbOpt.SelectedValue=="CH")
                            vFileNm = "attachment;filename=CHOLA_HOSPI_DATA_" + gblFuction.setDate(txtDtFrm.Text).ToString("yyyyMMdd") + "_ " + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + ".xls";
                        else if (rdbOpt.SelectedValue == "HL")
                            vFileNm = "attachment;filename=HDFC_LIFE_DATA_" + gblFuction.setDate(txtDtFrm.Text).ToString("yyyyMMdd") + "_ " + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + ".xls";
                        else if (rdbOpt.SelectedValue == "HM")
                            vFileNm = "attachment;filename=HDFC_MEL_DATA_" + gblFuction.setDate(txtDtFrm.Text).ToString("yyyyMMdd") + "_ " + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + ".xls";
                        else if (rdbOpt.SelectedValue == "NC")
                            vFileNm = "attachment;filename=NAT_CAT_DATA_" + gblFuction.setDate(txtDtFrm.Text).ToString("yyyyMMdd") + "_ " + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + ".xls";

                        else if (rdbOpt.SelectedValue == "GDNC")
                            vFileNm = "attachment;filename=GoDigit_NAT_CAT_DATA_" + gblFuction.setDate(txtDtFrm.Text).ToString("yyyyMMdd") + "_ " + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + ".xls";

                        else if (rdbOpt.SelectedValue == "TE")
                            vFileNm = "attachment;filename=TATA_AIG_EMI_DATA_" + gblFuction.setDate(txtDtFrm.Text).ToString("yyyyMMdd") + "_ " + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + ".xls";
                        else if (rdbOpt.SelectedValue == "TH")
                            vFileNm = "attachment;filename=TATA_AIG_HOSPI_DATA_" + gblFuction.setDate(txtDtFrm.Text).ToString("yyyyMMdd") + "_ " + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + ".xls";
                        else if (rdbOpt.SelectedValue == "AG")
                            vFileNm = "attachment;filename=Ageas_Data_Submission_Report_" + gblFuction.setDate(txtDtFrm.Text).ToString("yyyyMMdd") + "_ " + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + ".xls";
                        else if (rdbOpt.SelectedValue == "AQ")
                            vFileNm = "attachment;filename=Aiqu_Hospi_Data_Submission_Report_" + gblFuction.setDate(txtDtFrm.Text).ToString("yyyyMMdd") + "_ " + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + ".xls";

                        Response.ClearContent();
                        Response.AddHeader("content-disposition", vFileNm);
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.ms-excel";
                        HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                        Response.Write("<table border='1' cellpadding='5' widht='120%'>");
                        Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                        Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
                        Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress2(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
                        Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>From- " + txtDtFrm.Text + " To- " + txtToDt.Text + "</font></td></tr>");
                        Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'></td></tr>");
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
                                if (dt.Columns[j].ColumnName == "Partner Application Number" || dt.Columns[j].ColumnName == "Certificate Inception Date" 
                                    || dt.Columns[j].ColumnName == "Date of Birth" || dt.Columns[j].ColumnName == "Mobile Number" 
                                    || dt.Columns[j].ColumnName == "Nominee Date of Birth" || dt.Columns[j].ColumnName == "Disbursement Date" 
                                    || dt.Columns[j].ColumnName == "TelephoneHome" || dt.Columns[j].ColumnName == "DOB" || dt.Columns[j].ColumnName == "Risk Start Date" 
                                    || dt.Columns[j].ColumnName == "Date of Proposal" || dt.Columns[j].ColumnName == "Loan Account No" 
                                    || dt.Columns[j].ColumnName == "Billing Frequency" || dt.Columns[j].ColumnName == "Loan Nature"
                                    || dt.Columns[j].ColumnName == "Loan Disbursement Date" || dt.Columns[j].ColumnName == "DOB or Age"
                                    || dt.Columns[j].ColumnName == "Share Percentage" || dt.Columns[j].ColumnName == "MobileNumber"
                                    || dt.Columns[j].ColumnName == "Loan disbursed date (RSD)" || dt.Columns[j].ColumnName == "Loan account number"
                                    || dt.Columns[j].ColumnName == "First EMI/EWI date" || dt.Columns[j].ColumnName == "Last EMI/EWI date"
                                    || dt.Columns[j].ColumnName == "Branch Code" || dt.Columns[j].ColumnName == "Branch_Code"
                                    || dt.Columns[j].ColumnName == "Group ID" || dt.Columns[j].ColumnName == "Center ID"
                                    || dt.Columns[j].ColumnName == "Latitude" || dt.Columns[j].ColumnName == "Longitude"
                                    || dt.Columns[j].ColumnName == "LOAN_ACCNO" || dt.Columns[j].ColumnName == "Insured DOB"
                                    || dt.Columns[j].ColumnName == "MOBILENO" || dt.Columns[j].ColumnName == "Loan Date" || dt.Columns[j].ColumnName == "CMS NO")
                                {
                                    Response.Write("<td &nbsp nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
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
                    else
                    {
                        gblFuction.MsgPopup("No data found...");
                    }
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }

        private void GenerateRptData(string pMode)
        {
            string vBrCode = ViewState["BrCode"].ToString();
            string vType = rdbOpt.SelectedValue;

            var req = new InsuranceAutomation()
            {
                pFromDt = txtDtFrm.Text,
                pToDt = txtToDt.Text,
                pBranch = vBrCode,
                pFormat = pMode,
                pType = vType,
                pUserId = Convert.ToString(Session[gblValue.UserId]),
                pDBName = vDBName,
                pPassword = vPw,
                pServerIP = vSrvName,
                pCompanyName = gblValue.CompName,
                pCompanyAddress = CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString())
            };
            string Requestdata = JsonConvert.SerializeObject(req);
            CApiCalling API = new CApiCalling();
            string vMsg = API.GenerateReport("GenerateInsuranceAutomation", Requestdata, vReportUrl);
            gblFuction.AjxMsgPopup(vMsg);
            btnExcl.Enabled = false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            //SetParameterForRptData("Excel");
            GenerateRptData("Excel");
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
            DateTime vLogDt = gblFuction.setDate(txtToDt.Text.ToString());
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

    public class InsuranceAutomation
    {
        public string pFromDt { get; set; }
        public string pToDt { get; set; }
        public string pBranch { get; set; }
        public string pType { get; set; }
        public string pFormat { get; set; }
        public string pUserId { get; set; }
        public string pDBName { get; set; }
        public string pServerIP { get; set; }
        public string pPassword { get; set; }
        public string pCompanyName { get; set; }
        public string pCompanyAddress { get; set; }
    }
}