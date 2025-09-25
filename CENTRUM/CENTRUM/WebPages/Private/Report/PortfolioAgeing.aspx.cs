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
    public partial class PortfolioAgeing : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtAsDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["ID"] = null;
                PopList();
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
                this.PageHeading = "Portfolio Ageing";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuPortflioAginRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Portfolio Ageing", false);
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
            CGblIdGenerator oCG = null;
            CVillage oVill = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            Int32 vBrId = Convert.ToInt32(vBrCode);
            CEO oRO = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            //if (rdbOpt.SelectedValue == "rdbAll")
            //    chkDtl.Items.Clear();
            if (rdbOpt.SelectedValue == "rdbCo")
            {
                //oCG = new CGblIdGenerator();
                //dt = oCG.PopComboMIS("D", "N", "AA", "EoId", "EoName", "EoMst", vBrCode, "BranchCode", "Tra_DropDate", vLogDt, vBrCode);
                //chkDtl.DataSource = dt;
                //chkDtl.DataTextField = "EoName";
                //chkDtl.DataValueField = "EoID";
                //chkDtl.DataBind();
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "EoName";
                chkDtl.DataValueField = "Eoid";
                chkDtl.DataBind();
            }

            if (rdbOpt.SelectedValue == "rdbShg")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "GroupNo", "GroupID", "GroupName", "GroupMst", 0, "EOID", "DropoutDt", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "GroupName";
                chkDtl.DataValueField = "GroupID";
                chkDtl.DataBind();
            }

            if (rdbOpt.SelectedValue == "rdbVill")
            {
                oVill = new CVillage();
                dt = oVill.PopVillage(vBrCode);
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "VillageName";
                chkDtl.DataValueField = "VillageID";
                chkDtl.DataBind();
            }

            if (rdbOpt.SelectedValue == "rdbPurps")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "PurposeID", "Purpose", "LoanPurposeMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "Purpose";
                chkDtl.DataValueField = "PurposeID";
                chkDtl.DataBind();
            }

            if (rdbOpt.SelectedValue == "rdbFndSrc")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "FundSourceID", "FundSource", "FundSourceMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "FundSource";
                chkDtl.DataValueField = "FundSourceID";
                chkDtl.DataBind();
            }
            if (rdbOpt.SelectedValue == "rdbLoanType")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanType", "LoanTypeMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "LoanType";
                chkDtl.DataValueField = "LoanTypeId";
                chkDtl.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckAll()
        {
            Int32 vRow;
            string strin = "";
            if (rdbSel.SelectedValue == "rdbAll")
            {
                chkDtl.Enabled = false;
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                {
                    chkDtl.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkDtl.Items[vRow].Value + "";
                    }
                }
                ViewState["ID"] = strin;
            }
            else if (rdbSel.SelectedValue == "rdbSelct")
            {
                ViewState["ID"] = null;
                chkDtl.Enabled = true;
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = false;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopList();
            CheckAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
            {
                if (chkDtl.Items[vRow].Selected == true)
                {
                    if (strin == "")
                    {
                        strin = chkDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkDtl.Items[vRow].Value + "";
                    }
                }
            }
            ViewState["ID"] = strin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromdt = gblFuction.setDate(txtAsDt.Text);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vStrId = "", vRptPath = "", vMode = "A";
            DataTable dt = null;
            string vBranch = Session[gblValue.BrName].ToString();
            //ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();
            if (rdbOpt.SelectedValue == "")
            {
                gblFuction.AjxMsgPopup("Please select atleast one option...");
                return;
            }
            if (rdbOpt.SelectedValue != "rdbAll")
            {
                vStrId = ViewState["ID"].ToString();
                if (vStrId == null || vStrId == "")
                {
                    gblFuction.MsgPopup("Please Select the Records to print.");
                    return;
                }
            }

            if (rdbOpt.SelectedValue == "rdbAll")
                vMode = "A";
            if (rdbOpt.SelectedValue == "rdbCo")
                vMode = "C";
            if (rdbOpt.SelectedValue == "rdbShg")
                vMode = "G";
            if (rdbOpt.SelectedValue == "rdbVill")
                vMode = "V";
            if (rdbOpt.SelectedValue == "rdbPurps")
                vMode = "P";
            if (rdbOpt.SelectedValue == "rdbFndSrc")
                vMode = "F";
            if (rdbOpt.SelectedValue == "rdbLoanType")
                vMode = "T";

            dt = oRpt.rptPortfolioAgeing(vMode, vBrCode, vFromdt, vStrId);
            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\PortfolioAgeing.rpt";
            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                rptDoc.SetParameterValue("pAddress2", "");
                rptDoc.SetParameterValue("pBranch", vBranch);
                rptDoc.SetParameterValue("pFrmDt", txtAsDt.Text);
                rptDoc.SetParameterValue("pTitle", "Portfolio Ageing");
                rptDoc.SetParameterValue("pGrpName", vMode);
                if (pMode == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Loan Collection Report");
                else if (pMode == "Excel")
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Loan Collection Report");

                rptDoc.Dispose();
                Response.ClearHeaders();
                Response.ClearContent();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("PDF");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
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
    }
}
