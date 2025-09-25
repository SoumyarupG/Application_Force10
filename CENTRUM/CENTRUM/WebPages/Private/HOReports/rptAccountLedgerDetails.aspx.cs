using System;
using FORCECA;
using System.Data;
using FORCEBA;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;
using System.Runtime.Serialization;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class rptAccountLedgerDetails : CENTRUMBase
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
                if (rblDtFrmt.SelectedValue == "rbDtRng")
                {
                    txtFrmDt.Enabled = true;
                    txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                    ceFrmDt.Enabled = true;
                    txtToDt.Text = Session[gblValue.LoginDate].ToString();
                }
                else
                {
                    txtFrmDt.Enabled = false;
                    txtFrmDt.Text = Session[gblValue.FinFromDt].ToString();
                    ceFrmDt.Enabled = false;
                    txtToDt.Text = Session[gblValue.LoginDate].ToString();
                }
                PopBranch();
                PopState();
                PopParentGL();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Account Ledger Details";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuAccountLedgerDtlReport);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Account Ledger Details", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void rblDtFrmt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblDtFrmt.SelectedValue == "rbDtRng")
            {
                txtFrmDt.Enabled = true;
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                ceFrmDt.Enabled = true;
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
            }
            else
            {
                txtFrmDt.Enabled = false;
                txtFrmDt.Text = Session[gblValue.FinFromDt].ToString();
                ceFrmDt.Enabled = false;
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
            }
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
        protected void btnExecute_Click(object sender, EventArgs e)
        {
            Int32 vRptGLReport = Convert.ToInt32(Session[gblValue.RptGLReport].ToString());
            TimeSpan t = gblFuction.setDate(txtToDt.Text) - gblFuction.setDate(txtFrmDt.Text);
            if (t.TotalDays > 2)
            {
                gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                return;
            }
            if (vRptGLReport != 0)
            {
                Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                if (unixTicks - vRptGLReport > 300)
                {
                    Session[gblValue.RptGLReport] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    GenerateRptData(ddlOutputType.SelectedValue);
                }
                else
                {
                    gblFuction.AjxMsgPopup("Already Report Generate Request Is Executing ...Please Wait For 5 Mins..And Re Generate..");
                }
            }
            else
            {
                Session[gblValue.RptGLReport] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                GenerateRptData(ddlOutputType.SelectedValue);

            }
            
        }

        private void GenerateRptData(string pMode)
        {
            string vBrCode = Convert.ToString(ViewState["BrCode"]);
            string vDescID = Convert.ToString(ViewState["ChildGL"]);
            string vFinYrNo = Convert.ToString(Session[gblValue.FinYrNo]);
            string vFinFrom = Session[gblValue.FinFromDt].ToString();

            var req = new GuaranteeGLReportRequest()
            {
                pFromDt = txtFrmDt.Text,
                pToDt = txtToDt.Text,
                pMst = Session[gblValue.ACVouMst].ToString(),
                pDtl = Session[gblValue.ACVouDtl].ToString(),
                pDescID = vDescID,
                pBranch = vBrCode,
                pYrNo = vFinYrNo,
                pFinFromDt = vFinFrom,
                pProject = rblProj.SelectedValue,
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
            string vMsg = API.GenerateReport("GenerateGLReport", Requestdata, vReportUrl);
            gblFuction.AjxMsgPopup(vMsg);
            btnExecute.Enabled = false;

        }

        private void PopChildGL()
        {
            Int32 vRow;
            string strin = "";
            DataTable dt = null;
            CAcGenled oUsr = null;
            oUsr = new CAcGenled();
            dt = oUsr.GetParentChildGLDesc("C", Convert.ToString(ViewState["ParGL"]));
            chkChildGL.DataSource = dt;
            chkChildGL.DataTextField = "ChildGL";
            chkChildGL.DataValueField = "DescId";
            chkChildGL.DataBind();

            if (rblSelChildGL.SelectedValue == "rbAll")
            {
                chkChildGL.Enabled = false;
                for (vRow = 0; vRow < chkChildGL.Items.Count; vRow++)
                {
                    chkChildGL.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkChildGL.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkChildGL.Items[vRow].Value + "";
                    }
                }
            }
            else if (rblSelChildGL.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkChildGL.Items.Count; vRow++)
                {
                    chkChildGL.Items[vRow].Selected = false;
                }
            }
            ViewState["ChildGL"] = strin;
        }

        private void CheckChildGLAll()
        {
            Int32 vRow;
            string strin = "";
            if (rblSelChildGL.SelectedValue == "rbAll")
            {
                chkChildGL.Enabled = false;
                for (vRow = 0; vRow < chkChildGL.Items.Count; vRow++)
                {
                    chkChildGL.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkChildGL.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkChildGL.Items[vRow].Value + "";
                    }
                }
            }
            else if (rblSelChildGL.SelectedValue == "rbSel")
            {
                ViewState["ChildGL"] = null;
                chkChildGL.Enabled = true;
                for (vRow = 0; vRow < chkChildGL.Items.Count; vRow++)
                    chkChildGL.Items[vRow].Selected = false;
            }
            ViewState["ChildGL"] = strin;
        }

        protected void rblSelChildGL_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckChildGLAll();
        }

        protected void chkChildGL_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkChildGL.Items.Count; vRow++)
            {
                if (chkChildGL.Items[vRow].Selected == true)
                {
                    if (strin == "")
                    {
                        strin = chkChildGL.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkChildGL.Items[vRow].Value + "";
                    }
                }
            }
            ViewState["ChildGL"] = strin;
        }
        
        private void PopParentGL()
        {
            Int32 vRow;
            string strin = "";
            ViewState["ParGL"] = null;
            DataTable dt = null;
            CAcGenled oUsr = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            oUsr = new CAcGenled();
            dt = oUsr.GetParentChildGLDesc("P", "");
            chkParentGL.DataSource = dt;
            chkParentGL.DataTextField = "ParentGL";
            chkParentGL.DataValueField = "ParentGL";
            chkParentGL.DataBind();

            if (rblSelParGL.SelectedValue == "rbAll")
            {
                chkParentGL.Enabled = false;
                for (vRow = 0; vRow < chkParentGL.Items.Count; vRow++)
                {
                    chkParentGL.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkParentGL.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkParentGL.Items[vRow].Value + "";
                    }
                }
            }
            else if (rblSelParGL.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkParentGL.Items.Count; vRow++)
                {
                    chkParentGL.Items[vRow].Selected = false;
                }
            }
            ViewState["ParGL"] = strin;
        }

        private void CheckParentGLAll()
        {
            Int32 vRow;
            string strin = "";
            if (rblSelParGL.SelectedValue == "rbAll")
            {
                chkParentGL.Enabled = false;
                for (vRow = 0; vRow < chkParentGL.Items.Count; vRow++)
                {
                    chkParentGL.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkParentGL.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkParentGL.Items[vRow].Value + "";
                    }
                }
            }
            else if (rblSelParGL.SelectedValue == "rbSel")
            {
                ViewState["ParGL"] = null;
                chkParentGL.Enabled = true;
                for (vRow = 0; vRow < chkParentGL.Items.Count; vRow++)
                    chkParentGL.Items[vRow].Selected = false;
            }
            ViewState["ParGL"] = strin;
        }

        protected void rblSelParGL_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckParentGLAll();
            PopChildGL();
        }

        protected void chkParentGL_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkParentGL.Items.Count; vRow++)
            {
                if (chkParentGL.Items[vRow].Selected == true)
                {
                    if (strin == "")
                    {
                        strin = chkParentGL.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkParentGL.Items[vRow].Value + "";
                    }
                }
            }
            ViewState["ParGL"] = strin;
            PopChildGL();
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
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                ViewState["BrCode"] = null;
                chkBrDtl.Enabled = true;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    chkBrDtl.Items[vRow].Selected = false;
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
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));
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

    }

    [DataContract]
    public class GuaranteeGLReportRequest
    {
        [DataMember]
        public string pFromDt { get; set; }
        [DataMember]
        public string pToDt { get; set; }
        [DataMember]
        public string pMst { get; set; }
        [DataMember]
        public string pDtl { get; set; }
        [DataMember]
        public string pDescID { get; set; }
        [DataMember]
        public string pBranch { get; set; }
        [DataMember]
        public string pYrNo { get; set; }
        [DataMember]
        public string pFinFromDt { get; set; }
        [DataMember]
        public string pProject { get; set; }
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