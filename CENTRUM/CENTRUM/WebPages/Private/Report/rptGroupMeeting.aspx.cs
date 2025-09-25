using System;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class rptGroupMeeting : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopEO();
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
                this.PageHeading = "Group Meeting Schedule";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.nmuGrMstSchedulRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Group Meeting", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void PopEO()
        {
            DataTable dt = null;
            //CCM oCM = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            CEO oRO = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CGblIdGenerator oCG = null;

            try
            {
                ////////vBrCode = (string)Session[gblValue.BrnchCode];
                ////////vBrId = Convert.ToInt32(vBrCode);
                ////////oCM = new CCM();
                ////////dt = oCM.GetCOPop(vBrCode, "SCO,CO,TCO,JTCO,GO");
                ////////ddlCO.DataSource = dt;
                ////////ddlCO.DataTextField = "EoName";
                ////////ddlCO.DataValueField = "EoID";
                ////////ddlCO.DataBind();
                ////////ListItem oLi = new ListItem("<--Select-->", "-1");
                ////////ddlCO.Items.Insert(0, oLi);

                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oCG = new CGblIdGenerator();
                //dt = oCM.GetCOPop(vBrCode, "SCO,CO,TCO,JTCO,GO");
                dt = new DataTable();
                //dt = oCG.PopComboMIS("D", "N", "AA", "EoId", "EoName", "EoMst", vBrCode, "BranchCode", "Tra_DropDate", vLogDt, vBrCode);
                //ddlCO.DataSource = dt;
                //ddlCO.DataTextField = "EoName";
                //ddlCO.DataValueField = "EoId";
                //ddlCO.DataBind();
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlCO.DataSource = dt;
                ddlCO.DataTextField = "EoName";
                ddlCO.DataValueField = "Eoid";
                ddlCO.DataBind();

            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vRptPath = "";
            string vBranch = Session[gblValue.BrName].ToString();
            //ReportDocument rptDoc = new ReportDocument();
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            Int32 vEoID = 0;
            if (ddlCO.SelectedIndex > 0)
                vEoID = Convert.ToInt32(ddlCO.SelectedValue);
            if (ddlType.SelectedValue == "D")
                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\GroupMeetSch.rpt";
            if (ddlType.SelectedValue == "Y")
                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\GroupMeetSchDay.rpt";

            dt = oRpt.rptGrpMeetSch(vFromDt, vToDt, vEoID, vBrCode, ddlType.SelectedValue);
            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                rptDoc.SetParameterValue("pBranch", vBranch);
                if (ddlType.SelectedValue == "Y")
                    rptDoc.SetParameterValue("pTitle", "Group Meeting Schedule (Day Wise)");
                if (ddlType.SelectedValue == "D")
                    rptDoc.SetParameterValue("pTitle", "Group Meeting Schedule (Date Wise)");
                rptDoc.SetParameterValue("dtFrom", txtDtFrm.Text);
                rptDoc.SetParameterValue("dtTo", txtToDt.Text);
                if (pMode == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Group_Meeting_Schedule" + txtToDt.Text.Replace("/", "_"));
                else if (pMode == "Excel")
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Group_Meeting_Schedule" + txtToDt.Text.Replace("/", "_") + ".xls");
                rptDoc.Dispose();
                Response.ClearHeaders();
                Response.ClearContent();
            }
        }


        protected void btnPdf_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("PDF");
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
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
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlType.SelectedValue == "Y")
            {
                txtDtFrm.Enabled = false;
                txtToDt.Enabled = false;
            }
            else
            {
                txtDtFrm.Enabled = true;
                txtToDt.Enabled = true;
            }

        }
    }
}
