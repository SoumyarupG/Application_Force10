using System;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class GroupMst : CENTRUMBase
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

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Group";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.nmuGrMstRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Group Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //private void PopCO()
        //{
        //    DataTable dt = null;
        //    string vBrCode = Session[gblValue.BrnchCode].ToString();
        //    CCM oCM = new CCM();
        //    dt = oCM.GetCOPop(vBrCode, "SCO,CO,TCO,JTCO,GO");
        //    chkDtl.DataSource = dt;
        //    chkDtl.DataTextField = "EOName";
        //    chkDtl.DataValueField = "EOID";
        //    chkDtl.DataBind();
        //}


        /// <summary>
        /// 
        /// </summary>
        private void PopList()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            CVillage oVill = null;
            Int32 vBrId = 0;
            CEO oRO = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vBrCode = (string)Session[gblValue.BrnchCode];
            if (rblSel.SelectedValue == "rbCO")
            {
                //oCG = new CGblIdGenerator();
                ////dt = oCM.GetCOPop(vBrCode, "SCO,CO,TCO,JTCO,GO");
                //dt = new DataTable();
                //dt = oCG.PopComboMIS("N", "N", "AA", "EoId", "EoName", "EoMst", vBrCode, "BranchCode", "Tra_DropDate", vLogDt, vBrCode);
                //chkDtl.DataSource = dt;
                //chkDtl.DataTextField = "EoName";
                //chkDtl.DataValueField = "EoId";
                //chkDtl.DataBind();
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "EoName";
                chkDtl.DataValueField = "Eoid";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbViAra")
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
            if (rblSel.SelectedValue == "rbBlk")
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "BlockId", "BlockName", "BlockMst", vBrId, "AA", "AA", vLogDt, "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "BlockName";
                chkDtl.DataValueField = "BlockId";
                chkDtl.DataBind();
            }

            //if (rblSel.SelectedValue == "rbGP")
            //{
            //    vBrCode = (string)Session[gblValue.BrnchCode];
            //    vBrId = Convert.ToInt32(vBrCode);
            //    oCG = new CGblIdGenerator();
            //    dt = oCG.PopComboMIS("N", "N", "AA", "GPId", "GPName", "GPMst", vBrId, "AA", "AA", vLogDt, "0000");
            //    chkDtl.DataSource = dt;
            //    chkDtl.DataTextField = "GPName";
            //    chkDtl.DataValueField = "GPId";
            //    chkDtl.DataBind();
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vTypeId = "", vTitle = "", vRptPath = "", vGroupType = "";
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
            if (rblSel.SelectedValue == "rbCO")
            {
                dt = oGrpt.rptGetGrp(vBrCode, "C", vTypeId);
                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptGrpMst.rpt";
                vTitle = "CO Wise Group List";
                vGroupType = "CO Name :";
            }

            if (rblSel.SelectedValue == "rbViAra")
            {
                dt = oGrpt.rptGetGrp(vBrCode, "V", vTypeId);
                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptGrpMst.rpt";
                vTitle = "Village Wise Group List";
                vGroupType = "Group Name :";
            }
            if (rblSel.SelectedValue == "rbBlk")
            {
                dt = oGrpt.rptGetGrp(vBrCode, "B", vTypeId);
                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptGrpMst.rpt";
                vTitle = "Block Wise Group List";
                vGroupType = "Block Name :";
            }
            //if (rblSel.SelectedValue == "rbGP")
            //{
            //    dt = oGrpt.rptGetGrp(vBrCode, "M", vTypeId);
            //    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptGrpMst.rpt";
            //    vTitle = "Mahalla Wise Group List";
            //    vGroupType = "Mahalla Name :";
            //}
            if (dt.Rows.Count > 0)
            {
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("vBrCode", vBrCode);
                    rptDoc.SetParameterValue("pTitle", vTitle);
                    rptDoc.SetParameterValue("pGoupType", vGroupType);
                    if (pMode == "PDF")
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Group Master Report");
                    else if (pMode == "Excel")
                        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Group Master Report");
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
