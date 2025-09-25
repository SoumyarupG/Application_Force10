using System;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class rptGPBlockVill : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopList();
                CheckAll();
                popDetail();
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Village/Panchayat/Block";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.nmuTehBlocVilMohRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "GP/Block/Village Master", false);
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
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            if (rblSel.SelectedValue == "rbDis")
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "DistrictId", "DistrictName", "DistrictMst", 0, "AA", "AA", vLogDt, "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "DistrictName";
                chkDtl.DataValueField = "DistrictId";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbGP")
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "GPId", "GPName", "GPMst", 0, "AA", "AA", vLogDt, "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "GPName";
                chkDtl.DataValueField = "GPId";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbBlk")
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "BlockId", "BlockName", "BlockMst", 0, "AA", "AA", vLogDt, "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "BlockName";
                chkDtl.DataValueField = "BlockId";
                chkDtl.DataBind();
            }


            if (rblSel.SelectedValue == "rbVill")
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oVill = new CVillage();
                dt = oVill.PopVillage(vBrCode);
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "VillageName";
                chkDtl.DataValueField = "VillageId";
                chkDtl.DataBind();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vTypeId = "", vTitle = "", vRptPath = "";
            DataTable dt = null;
            string vBranch = Session[gblValue.BrName].ToString();
            vTypeId = ViewState["Dtl"].ToString();
            //ReportDocument rptDoc = new ReportDocument();
            CReports oGrpt = new CReports();
            if (rblSel.SelectedValue == "")
            {
                gblFuction.MsgPopup("Please select atleast one option...");
                return;
            }
            if (rblSel.SelectedValue == "rbDis")
            {
                dt = oGrpt.rptGPBlockVill("D", vTypeId);
                vTitle = "District List";
                //vGroupType = "District Name";
            }

            if (rblSel.SelectedValue == "rbGP")
            {
                dt = oGrpt.rptGPBlockVill("G", vTypeId);
                vTitle = "G.P List";
                //vGroupType = "G.P. Name";
            }
            if (rblSel.SelectedValue == "rbBlk")
            {
                dt = oGrpt.rptGPBlockVill("B", vTypeId);
                vTitle = "Block List";
                //vGroupType = "Block Name";
            }
            if (rblSel.SelectedValue == "rbVill")
            {
                dt = oGrpt.rptGPBlockVill("V", vTypeId);
                vTitle = "Village List";
                //vGroupType = "Village Name";
            }
            if (dt.Rows.Count > 0)
            {
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptGPBlockVill.rpt";
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pTitle", vTitle);
                    //rptDoc.SetParameterValue("pGoupType", vGroupType);
                    if (pMode == "PDF")
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Master_Listing_Report.pdf");
                    else if (pMode == "Excel")
                        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Master_Listing_Report.xls");
                    rptDoc.Dispose();
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
            else
                gblFuction.MsgPopup("No Records Found");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopList();
            CheckAll();
            popDetail();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
            popDetail();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            popDetail();
        }
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
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
                Response.Redirect("~/WebPages/Public/Main.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
