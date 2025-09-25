using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.Data;
using System.IO;
using System.Configuration;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace CENTRUM_SARALVYAPAR.WebPages.Private.Report
{
    public partial class RptInitialApproachStatusOnDemand : CENTRUMBAse
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
                txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch(Session[gblValue.UserName].ToString());
                CheckAll();
                popDetail();
                PopState();
            }

            if (Page.IsPostBack == false)
            {
                rblAlSel_SelectedIndexChanged(sender, e);
                rblRptType_SelectedIndexChanged(sender, e);
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Initial Approach Status On Demand";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuInitialApproachStatusOnDemandRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Initial Approach Status On Demand", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
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

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            ViewState["Id"] = null;
            try
            {
                DateTime vLogDt = gblFuction.setDate(txtToDt.Text.ToString());
                oCG = new CGblIdGenerator();
                oUsr = new CUser();
                ViewState["Id"] = null;

                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (Convert.ToString(row["BranchCode"]) != Convert.ToString(Session[gblValue.BrnchCode]))
                        {
                            row.Delete();
                        }
                    }
                    dt.AcceptChanges();
                }
                if (dt.Rows.Count > 0)
                {
                    chkDtl.DataSource = dt;
                    chkDtl.DataTextField = "BranchName";
                    chkDtl.DataValueField = "BranchCode";
                    chkDtl.DataBind();
                    CheckAll();
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
                oCG = null;
            }
        }

        private void CheckAll()
        {
            Int32 vRow;
            if (rblAlSel.SelectedValue == "rbAll")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = true;
                chkDtl.Enabled = false;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = false;
                chkDtl.Enabled = true;
            }
        }

        private void popDetail()
        {
            ViewState["Dtl"] = null;
            string str = "";
            for (int vRow = 0; vRow < chkDtl.Items.Count; vRow++)
            {
                if (chkDtl.Items[vRow].Selected == true)
                {
                    if (str == "")
                        str = chkDtl.Items[vRow].Value;
                    else if (str != "")
                        str = str + "," + chkDtl.Items[vRow].Value;
                }
            }
            if (str == "")
                ViewState["Dtl"] = 0;
            else
                ViewState["Dtl"] = str;
        }

        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
            popDetail();
        }

        protected void rblRptType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblRptType.SelectedValue == "A" || rblRptType.SelectedValue == "B" || rblRptType.SelectedValue == "C" || rblRptType.SelectedValue == "D")
            {
                lblDtFrm.Visible = true;
                txtDtFrm.Visible = true;
                ceDtFrm.Enabled = true;
                lblDtTo.Text = "Date To";
            }
            else
            {
                lblDtFrm.Visible = false;
                txtDtFrm.Visible = false;
                ceDtFrm.Enabled = false;
                lblDtTo.Text = "As On Date";
            }
        }

        protected void btnStateWise_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CUser oUsr = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            string pUser = Session[gblValue.UserName].ToString();
            chkDtl.Items.Clear();
            ViewState["Id"] = null;
            try
            {
                DateTime vLogDt = gblFuction.setDate(txtToDt.Text.ToString());
                oCG = new CGblIdGenerator();
                oUsr = new CUser();
                ViewState["Id"] = null;
                if (vBrCode == "0000")
                {
                    dt = oUsr.GetBranchByState(pUser, Convert.ToInt32(Session[gblValue.RoleId]), ddlState.SelectedValues.Replace("|", ","));
                    if (dt.Rows.Count > 0)
                    {
                        chkDtl.DataSource = dt;
                        chkDtl.DataTextField = "BranchName";
                        chkDtl.DataValueField = "BranchCode";
                        chkDtl.DataBind();
                        if (rblAlSel.SelectedValue == "rbAll")
                            CheckAll();
                    }
                }
                else
                {
                    dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, vBrCode);
                    chkDtl.DataSource = dt;
                    chkDtl.DataTextField = "Name";
                    chkDtl.DataValueField = "BranchCode";
                    chkDtl.DataBind();
                    if (rblAlSel.SelectedValue == "rbAll")
                        CheckAll();
                }

            }
            finally
            {
                dt = null;
                oUsr = null;
                oCG = null;
            }

        }

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            //SetParameterForRptData("Excel");
            SetRptData("Excel");
        }

        protected void btnCSV_Click(object sender, EventArgs e)
        {
            //SetParameterForRptData("CSV");
            SetRptData("CSV");
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                btnCSV.Enabled = false;
                btnExcl.Enabled = false;
            }
        }

        private void SetRptData(string pMode)
        {                    
            CReports oRpt = null;

            DateTime vDtFrm = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            popDetail();
            string vBrCode = ViewState["Dtl"].ToString();
            try
            {
                oRpt = new CReports();
                
                var req = new RptInitialApproachOnDemandRequest()
                {
                    pFromDt = txtDtFrm.Text,
                    pToDt = txtToDt.Text,
                    pBrCode = vBrCode,
                    pRptType = Convert.ToString(rblRptType.SelectedValue),
                    pBCProductId = Convert.ToString(Session[gblValue.BCProductId]),
                    pFormat = pMode,
                    pUserId = Convert.ToString(Session[gblValue.UserId]),
                    pDBName = vDBName,
                    pPassword = vPw,
                    pServerIP = vSrvName,
                    pCompanyName = gblValue.CompName,
                    pCompanyAddress = CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString())
                };

                string Requestdata = JsonConvert.SerializeObject(req);
               
                Int32 vRptInitialStatusOnDemand = Convert.ToInt32(Session[gblValue.RptInitialStatusOnDemand].ToString());
                if (vRptInitialStatusOnDemand != 0)
                {
                    Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    if (unixTicks - vRptInitialStatusOnDemand > 300)
                    {
                        Session[gblValue.RptInitialStatusOnDemand] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                        GenerateReport("GenerateInitialApproachStatusOnDemandSaral", Requestdata);
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Already Report Generate Request Is Executing ...Please Wait For 5 Mins..And Re Generate..");
                    }
                }
                else
                {
                    Session[gblValue.RptInitialStatusOnDemand] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    GenerateReport("GenerateInitialApproachStatusOnDemandSaral", Requestdata);
                }
            }
            finally
            {               
                oRpt = null;
            }
        }
    }

    [DataContract]
    public class RptInitialApproachOnDemandRequest
    {
        [DataMember]
        public string pFromDt { get; set; }
        [DataMember]
        public string pToDt { get; set; }
        [DataMember]
        public string pBrCode { get; set; }
        [DataMember]
        public string pRptType { get; set; }
        [DataMember]
        public string pBCProductId { get; set; }
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