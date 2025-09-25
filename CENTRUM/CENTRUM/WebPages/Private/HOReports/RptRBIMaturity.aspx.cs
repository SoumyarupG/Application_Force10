using System;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using FORCECA;
using FORCEBA;
using System.Web.UI;
using System.Web;
using System.Configuration;
using Newtonsoft.Json;
using System.Runtime.Serialization;


namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class RptRBIMaturity : CENTRUMBase
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
                ViewState["BrCode"] = null;
                txtFromDt.Text = Session[gblValue.LoginDate].ToString();
                PopBranch();
            }

        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "RBI Maturity Analysis Report";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuRBIMaturityAnl);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "RBI Maturity Analysis Report", false);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBrAll();
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void PopBranch()
        {
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            DataTable dt = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CUser oUsr = null;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFormat"></param>
        private void GetData(string pMode)
        {
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = ViewState["BrCode"].ToString();
            DateTime vAsOn = gblFuction.setDate(txtFromDt.Text);
            DateTime vFinyear = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            string vRptPath = "";
            DataTable dt = null;
            CReports oRpt = new CReports();
            string vFileNm = "";
            try
            {
                if (pMode == "PDF")
                {

                }
                else
                {
                    
                    if (rdbSumDel.SelectedValue == "49")
                    {
                        dt = oRpt.rptRBIMaturity(vAsOn, vBrCode, rdbSumDel.SelectedValue);
                        dgMaturity.DataSource = dt;
                        dgMaturity.DataBind();
                    }
                    else if (rdbSumDel.SelectedValue == "50")
                    {
                        dt = oRpt.rptRBIMaturity(vAsOn, vBrCode, rdbSumDel.SelectedValue);
                        DgSum.DataSource = dt;
                        DgSum.DataBind();
                    }
                    else 
                    {
                        dt = oRpt.rptRBIALMExcel(vAsOn, vBrCode, rdbSumDel.SelectedValue);
                        dgALM.DataSource = dt;
                        dgALM.DataBind();
                    }
                    if (rdbSumDel.SelectedValue == "ALM")
                    {
                        vFileNm = "attachment;filename=RBI_ALM_Report.xls";
                    }
                    else
                    {
                        vFileNm = "attachment;filename=RBI_Maturity_Analysis_Report.xls";
                    }
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
                    htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='"+dt.Columns.Count+"'><b><u><font size='4'>" + gblValue.CompName + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + CGblIdGenerator.GetBranchAddress1("000") + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + CGblIdGenerator.GetBranchAddress2("000") + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + vBranch + "</font></u></b></td></tr>");
                    if (rdbSumDel.SelectedValue == "ALM")
                        htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>ALM Report</font></u></b></td></tr>");
                    else
                        htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>Maturity Analysis Report</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td></td></tr>");
                    htw.WriteLine("<tr><td align=Center' colspan='" + dt.Columns.Count + "'><b>As on Date : " + txtFromDt.Text + "</b></td></tr>");
                    htw.WriteLine("<tr><td></td></tr>");
                    if (rdbSumDel.SelectedValue == "49")
                    {
                        dgMaturity.RenderControl(htw);
                    }
                    else if (rdbSumDel.SelectedValue == "50")
                    {
                        DgSum.RenderControl(htw);
                    }
                    else 
                    {
                        dgALM.RenderControl(htw);
                    }

                    htw.WriteLine("</table>");
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Write(sw.ToString());
                    Response.End();
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            GetData("PDF");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
           // GetData("Excel");
            //GenerateRptData("Excel");
            Int32 vRptRBIMaturity = Convert.ToInt32(Session[gblValue.RptRBIMaturity].ToString());
            if (vRptRBIMaturity != 0)
            {
                Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                if (unixTicks - vRptRBIMaturity > 300)
                {
                    Session[gblValue.RptRBIMaturity] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    GenerateRptData("Excel");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Already Report Generate Request Is Executing ...Please Wait For 5 Mins..And Re Generate..");
                }
            }
            else
            {
                Session[gblValue.RptRBIMaturity] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                GenerateRptData("Excel");

            }
        }

        private void GenerateRptData(string pMode)
        {
            string vBrCode = ViewState["BrCode"].ToString();           
            string vSumDel = Convert.ToString(rdbSumDel.SelectedValue);
            var req = new RptRBIMaturityCentrum()
            {
                pAsOnDt = txtFromDt.Text,
                pBrCode = vBrCode,
                vMode = vSumDel,
                pFormat = pMode,
                pUserId = Convert.ToString(Session[gblValue.UserId]),
                pDBName = vDBName,
                pPassword = vPw,
                pServerIP = vSrvName,
                pCompanyName = gblValue.CompName,
                pCompanyAddress = CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString())
            };
            string Requestdata = JsonConvert.SerializeObject(req);
            CApiCalling API = new CApiCalling();
            string vMsg = API.GenerateReport("GenerateRBIMaturityCentrum", Requestdata, vReportUrl);
            gblFuction.AjxMsgPopup(vMsg);
            btnExcl.Enabled = false;
        }
    }
    [DataContract]
    public class RptRBIMaturityCentrum
    {
        [DataMember]
        public string pAsOnDt { get; set; }
        [DataMember]
        public string pBrCode { get; set; }
        [DataMember]
        public string vMode { get; set; }
        [DataMember]
        public string pFormat { get; set; }
        [DataMember]
        public string pUserId { get; set; }
        [DataMember]
        public string pDBName { get; set; }
        [DataMember]
        public string pServerIP { get; set; }
        [DataMember]
        public string pPassword { get; set; }
        [DataMember]
        public string pCompanyName { get; set; }
        [DataMember]
        public string pCompanyAddress { get; set; }
    }
}