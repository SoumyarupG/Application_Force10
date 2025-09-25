using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class AccLedgerDtlsTest : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch(Session[gblValue.UserName].ToString());
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                    ddlBranch.Enabled = false;
                }
                else
                {
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                    ddlBranch.Enabled = true;
                }
                PopList();
                CheckAll();
                popDetail();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUser"></param>
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
                }
                else
                {
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
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
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Account Ledger Details";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.nmuLedgDtlsRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Bank Book", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }


        private void PopList()
        {
            DataTable dt = null;
            CVoucher oGen = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vBrCode;
            try
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }
                else
                {
                    vBrCode = ddlBranch.SelectedValue.ToString();
                }
                oGen = new CVoucher();
                //dt = oGen.GetLedgerList();
                dt = oGen.GetAcGenLedCB(vBrCode, "G", "");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "Desc";
                chkDtl.DataValueField = "DescID";
                chkDtl.DataBind();
            }
            finally
            {
                dt = null;
                oGen = null;
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


        private bool ValidateDate()
        {
            bool vRst = true;
            if (gblFuction.CheckDtRange(txtDtFrm.Text, txtToDt.Text) == false)
            {
                vRst = false;
            }
            return vRst;
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

        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
            popDetail();
        }

        protected void chkDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            popDetail();
        }


        private void GetData(string pFormat)
        {
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\AcLedDtl.rpt";
            string vBranch = Session[gblValue.BrName].ToString();
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBrCode;
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
            }
            else
            {
                vBrCode = ddlBranch.SelectedValue.ToString();
            }
            string vAcMst = Session[gblValue.ACVouMst].ToString();
            string vAcDtl = Session[gblValue.ACVouDtl].ToString();
            Int32 vFinYrNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            string vDescID = ViewState["Dtl"].ToString();
            DataTable dt = null;
            ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();
            rptDoc.Load(vRptPath);
            dt = oRpt.rptAcLedDetail(gblFuction.setDate(txtDtFrm.Text), gblFuction.setDate(txtToDt.Text),
                                    Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(),
                                    vDescID, vBrCode, vFinYrNo, vFinFrom);
            rptDoc.SetDataSource(dt);
            rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
            rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
            rptDoc.SetParameterValue("pAddress2", "");
            rptDoc.SetParameterValue("pBranch", vBranch);
            rptDoc.SetParameterValue("pTitle", "A/C Ledger Details");
            rptDoc.SetParameterValue("DtFrom", txtDtFrm.Text);
            rptDoc.SetParameterValue("DtTo", txtToDt.Text);
            if (pFormat == "PDF")
                rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "A/C Ledger Details");
            else
                rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "A/C Ledger Details");
            rptDoc.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopList();
            CheckAll();
            popDetail();
        }
    }
}
