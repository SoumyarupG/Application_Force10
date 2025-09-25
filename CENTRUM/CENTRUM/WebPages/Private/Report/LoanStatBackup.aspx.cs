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
    public partial class LoanStatBackup : CENTRUMBase 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtAsDt.Text = Session[gblValue.LoginDate].ToString();
                txtinstno.Text = "1";
                txtinstno.Enabled = false;
                PopRO();
                popLoanProduct();
                popFundSource();
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
                this.PageHeading = "Loan Status";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuLoanStatusRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Status Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }


        private void PopRO()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("D", "N", "AA", "EoId", "EoName", "EoMst", vBrCode, "BranchCode", "Tra_DropDate", vLogDt, vBrCode);
                ddlRO.DataSource = dt;
                ddlRO.DataTextField = "EoName";
                ddlRO.DataValueField = "EoId";
                ddlRO.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlRO.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vAsOn = gblFuction.setDate(txtAsDt.Text);
            string vRptPath = "", vTitle = "", vType = "D", vROID = "", vMKID = "", vGRID = "";
            Int32 vLPID = 0, vLSID = 0, vFSID = 0, vLCID = 0, vInstno = 1;
            DataTable dt = null;
            CReports oRpt = new CReports();
            ReportDocument rptDoc = new ReportDocument();

            if (ddlRO.SelectedIndex > 0) vROID = ddlRO.SelectedValue;
            if (ddlCent.SelectedIndex > 0) vMKID = ddlCent.SelectedValue;
            if (ddlGroup.SelectedIndex > 0) vGRID = ddlGroup.SelectedValue;
            if (ddlLP.SelectedIndex > 0) vLPID = Convert.ToInt32(ddlLP.SelectedValue);
            if (ddlLS.SelectedIndex > 0) vLSID = Convert.ToInt32(ddlLS.SelectedValue);
            if (ddlLC.SelectedIndex > 0) vLCID = Convert.ToInt32(ddlLC.SelectedValue);

            //if (chkInstall.Checked == true) vChkInstall = 1;
            if (txtinstno.Text != "") vInstno = Convert.ToInt32(txtinstno.Text);
            //if (chkonly.Checked == true) vChkonly = 1;
            //if (chkDList.Checked == true) vChkDList = 1;
            vType = rdbSel.SelectedValue;

            if (ddlFS.SelectedIndex > 0)
            {
                vFSID = Convert.ToInt32(ddlFS.SelectedValue);
                if (vType == "D")
                {
                    vTitle = "LOAN STATUS REPORT - FUND SOURCE-WISE";
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanStatusSF.rpt";
                }
                else
                {
                    vTitle = "LOAN STATUS SUMMARY REPORT - FUND SOURCE-WISE";
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanStatusSumSF.rpt";
                }
            }
            else
            {
                if (vType == "D")
                {
                    vTitle = "LOAN STATUS REPORT";
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanStatus.rpt";
                }
                else
                {
                    vTitle = "LOAN STATUS SUMMARY REPORT";
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanStatusSum.rpt";
                }
            }

            //dt = oRpt.rptLoanStatus(vAsOn, vChkInstall, vFSID, vLPID, vLSID, vGRID, vMKID, vROID, vInstno, vLCID, vChkDList, vChkonly, vBrCode);
            //vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanStatus.rpt";
            rptDoc.Load(vRptPath);
            rptDoc.SetDataSource(dt);
            rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
            rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
            rptDoc.SetParameterValue("pBranch", vBranch);
            rptDoc.SetParameterValue("AsOnDt", txtAsDt.Text);
            rptDoc.SetParameterValue("pTitle", vTitle);

            if (pMode == "PDF")
                rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Loan Status Report");
            else if (pMode == "Excel")
                rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Loan Status Report");
            rptDoc.Dispose();
        }

        protected void ddlRO_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vRoId = "";
            vRoId = Convert.ToString(ddlRO.SelectedValue);
            PopCenter(vRoId);
        }


        private void PopCenter(string vEOID)
        {
            ddlGroup.Items.Clear();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("D", "N", "AA", "MarketID", "Market", "MarketMSt", vEOID, "EOID", "Tra_DropDate", vLogDt, vBrCode);
                ddlCent.DataSource = dt;
                ddlCent.DataTextField = "Market";
                ddlCent.DataValueField = "MarketID";
                ddlCent.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCent.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        protected void ddlCent_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vCentId = "";
            vCentId = ddlCent.SelectedValue.ToString();
            PopGroup(vCentId);
        }

        private void PopGroup(string vCenterID)
        {
            ddlGroup.Items.Clear();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("D", "N", "AA", "GroupID", "GroupName", "GroupMst", vCenterID, "MarketID", "Tra_DropDate", vLogDt, vBrCode);
                ddlGroup.DataSource = dt;
                ddlGroup.DataTextField = "GroupName";
                ddlGroup.DataValueField = "GroupID";
                ddlGroup.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlGroup.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popLoanProduct()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "ProductId", "Product", "LoanProductMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlLP.DataSource = dt;
                ddlLP.DataTextField = "Product";
                ddlLP.DataValueField = "ProductId";
                ddlLP.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlLP.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        protected void ddlLP_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vLProdID = 0;
            vLProdID = Convert.ToInt32(ddlLP.SelectedValue);
            PopLoanScheme(vLProdID);
        }

        private void PopLoanScheme(Int32 vLProdID)
        {
            ddlLS.Items.Clear();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("S", "N", "AA", "LoanTypeID", "LoanType", "LoanTypeMst", vLProdID, "ProductID", "Tra_DropDate", vLogDt, vBrCode);
                ddlLS.DataSource = dt;
                ddlLS.DataTextField = "LoanType";
                ddlLS.DataValueField = "LoanTypeID";
                ddlLS.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlLS.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popFundSource()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "FundSourceId", "FundSource", "FundSourceMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlFS.DataSource = dt;
                ddlFS.DataTextField = "FundSource";
                ddlFS.DataValueField = "FundSourceId";
                ddlFS.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlFS.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        protected void chkDList_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDList.Checked == true)
            {
                rdbSel.Visible = false;
                rdbSel.SelectedValue = "D";
            }
            else
                rdbSel.Visible = true;
        }

        protected void chkInstall_CheckedChanged(object sender, EventArgs e)
        {
            if (chkInstall.Checked == true)
                txtinstno.Enabled = true;
            else
                txtinstno.Enabled = false;
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("PDF");
        }

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
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

    }
}
