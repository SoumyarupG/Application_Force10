using System;
using System.Web;
using System.Data;
using FORCECA;
using FORCEBA;
using System.Configuration;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HOCashBnkClBrWise : CENTRUMBase
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
                txtDtAsOn.Text = Session[gblValue.LoginDate].ToString();
                PopBranch();
            }

        }
        
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Cash Bank Closing (Branch Wise)";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHOCashBnkClsng);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Cash/Bank Closing (Branch Wise) Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
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
        
        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBrAll();
        }
        
        private void CheckBrAll()
        {
            Int32 vRow;
            string strin = "";
            chkBrDtl.Enabled = true;
            if (rblAlSel.SelectedValue == "rbAll")
            {
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
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    chkBrDtl.Items[vRow].Selected = false;

            }
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
                chkBrDtl.Enabled = true;
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

        private void GetData(string pFormat)
        {
            string vFileNm = "", vMsg = "";
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = ViewState["BrCode"].ToString();
            DateTime vFinYrFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            Int32 vFinYrNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            
            DataTable dt = null;
            CReports oRpt = new CReports();

            if (vFinYrFrom > gblFuction.setDate(txtDtAsOn.Text) || gblFuction.setDate(txtDtAsOn.Text) > vFinTo)
            {
                gblFuction.MsgPopup("Date should be within this financial year.");
                return;
            }

            CApiCalling oAPI = new CApiCalling();
            var req = new HOCashBnkClBrWiseReportRequest()
            {

                pFinYrFrom = Session[gblValue.FinFromDt].ToString(),
                pDtAsOn = txtDtAsOn.Text.ToString(),
                pBrCode = vBrCode,
                pFinYrNo = vFinYrNo,               
                pUserId = Convert.ToString(Session[gblValue.UserId]),
                pDBName = vDBName,
                pPassword = vPw,
                pServerIP = vSrvName,
                pCompanyName = gblValue.CompName,
                pCompanyAddress = CGblIdGenerator.GetBranchAddress1(vBrCode)
            };

            string Requestdata = JsonConvert.SerializeObject(req);
            vMsg = oAPI.GenerateReport("GenerateHOCashBnkClBrWiseReport_Centrum", Requestdata, vReportUrl);
            gblFuction.AjxMsgPopup(vMsg);

            //dt = oRpt.rptCashBankBalance(vFinYrFrom, gblFuction.setDate(txtDtAsOn.Text), vBrCode, vFinYrNo);
            //System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            //DataGrid1.DataSource = dt;
            //DataGrid1.DataBind();

            //tdx.Controls.Add(DataGrid1);
            //tdx.Visible = false;
            //vFileNm = "attachment;filename=" + gblFuction.setDate(txtDtAsOn.Text).ToString("yyyyMMdd") + "_Cash_Comparison_Branch_Report.xls";
            
            //System.IO.StringWriter sw = new System.IO.StringWriter();
            //System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            //htw.WriteLine("<table border='0' cellpadding='3' widht='100%'>");


            //htw.WriteLine("<tr><td align=center' colspan='4'><b><u><font size='4'>" + gblValue.CompName + "</font></u></b></td></tr>");
            
            //htw.WriteLine("<tr><td align=center' colspan='4'><b><u><font size='3'>Cash Comparison (Branch-Wise) Report As On " + txtDtAsOn.Text.ToString() + "</font></u></b></td></tr>");
            //DataGrid1.RenderControl(htw);
            //htw.WriteLine("</td></tr>");
            //htw.WriteLine("<tr><td colspan='4'><b><u><font size='2'></font></u></b></td></tr>");
            //htw.WriteLine("</table>");

            //Response.ClearContent();
            //Response.AddHeader("content-disposition", vFileNm);
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.Write(sw.ToString());
            //Response.End();

        }

        private bool ValidateDate()
        {
            bool vRst = true;
            return vRst;
        }

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

    }

    [DataContract]
    public class HOCashBnkClBrWiseReportRequest
    {

        [DataMember]
        public string pFinYrFrom { get; set; }
        [DataMember]
        public string pDtAsOn { get; set; }
        [DataMember]
        public string pBrCode { get; set; }
        [DataMember]
        public int pFinYrNo { get; set; }        
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