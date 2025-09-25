using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;


namespace CENTRUM.WebPages.Private.Report
{
    public partial class FinalPaid : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopCO();
                PopLoanScheme();
                txtDtFrm.Text = System.DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
                txtToDt.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
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
                this.PageHeading = "Final Paid List";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.nmuFinlPaidRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Settled Client", false);
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
                Response.Redirect("~/WebPages/Public/Main.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopCO()
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CGblIdGenerator oCG = new CGblIdGenerator();
            CEO oRO = null;
            try
            {
                //dt = oCM.GetCOPop(vBrCode, "SCO,CO,TCO,JTCO,GO");
                //oCG = new CGblIdGenerator();
                //dt = oCG.PopComboMIS("D", "N", "AA", "EoId", "EoName", "EoMst", vBrCode, "BranchCode", "Tra_DropDate", vLogDt, vBrCode);
                //ddlCo.DataSource = dt;
                //ddlCo.DataTextField = "EOName";
                //ddlCo.DataValueField = "EoId";
                //ddlCo.DataBind();
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlCo.DataSource = dt;
                ddlCo.DataTextField = "EoName";
                ddlCo.DataValueField = "Eoid";
                ddlCo.DataBind();
                ListItem oL1 = new ListItem("<-- Select -->", "-1");
                ddlCo.Items.Insert(0, oL1);
            }
            catch (Exception ex)
            {
                throw ex;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ////protected void ddlCo_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        ////    //lblMemName.Text = "";
        ////    DataTable dtGr = null;
        ////    int vEOID = Convert.ToInt32(ddlCo.SelectedValue);
        ////    string vBrCode = Session[gblValue.BrnchCode].ToString();
        ////    dtGr = new DataTable();
        ////    CGblIdGenerator oGbl = new CGblIdGenerator();
        ////    dtGr = oGbl.PopComboMIS("S", "N", "AA", "GroupId", "GroupName", "GroupMst", vEOID, "EOID", "ClosingDt", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);
        ////    ddlGroup.DataSource = dtGr;
        ////    ddlGroup.DataTextField = "GroupName";
        ////    ddlGroup.DataValueField = "GroupID";
        ////    ddlGroup.DataBind();
        ////    ListItem oLi = new ListItem("<--Select-->", "-1");
        ////    ddlGroup.Items.Insert(0, oLi);
        ////}


        protected void ddlCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataTable dtMk = null;
            //CGblIdGenerator oGbl;
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            //try
            //{
            //    if (Convert.ToInt32(ddlCo.SelectedValue) > 0)
            //    {
            //        dtMk = new DataTable();
            //        oGbl = new CGblIdGenerator();
            //        dtMk = oGbl.PopComboMIS("S", "N", "AA", "MarketID", "Market", "MarketMst", Convert.ToInt32(ddlCo.SelectedValue), "EOID", "Tra_DropDate", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);
            //        ddlMarket.DataSource = dtMk;
            //        ddlMarket.DataTextField = "Market";
            //        ddlMarket.DataValueField = "MarketID";
            //        ddlMarket.DataBind();
            //        ListItem oLi = new ListItem("<--Select-->", "-1");
            //        ddlMarket.Items.Insert(0, oLi);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    dtMk = null;
            //    oGbl = null;
            //}
            DataTable dtGr = null;
            CGblIdGenerator oGbl;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                if (Convert.ToInt32(ddlCo.SelectedValue) > 0)
                {
                    dtGr = new DataTable();
                    oGbl = new CGblIdGenerator();
                    dtGr = oGbl.PopComboMIS("S", "N", "AA", "Groupid", "GroupName", "GroupMSt", Convert.ToInt32(ddlCo.SelectedValue), "MarketID", "Tra_DropDate", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);

                    ddlGroup.DataSource = dtGr;
                    ddlGroup.DataTextField = "GroupName";
                    ddlGroup.DataValueField = "Groupid";
                    ddlGroup.DataBind();
                    ListItem oLi = new ListItem("<--Select-->", "-1");
                    ddlGroup.Items.Insert(0, oLi);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtGr = null;
                oGbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ddlMarket_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DataTable dtGr = null;
        //    CGblIdGenerator oGbl;
        //    string vBrCode = Session[gblValue.BrnchCode].ToString();
        //    try
        //    {
        //        if (Convert.ToInt32(ddlMarket.SelectedValue) > 0)
        //        {
        //            dtGr = new DataTable();
        //            oGbl = new CGblIdGenerator();
        //            dtGr = oGbl.PopComboMIS("S", "N", "AA", "Groupid", "GroupName", "GroupMSt", Convert.ToInt32(ddlMarket.SelectedValue), "MarketID", "Tra_DropDate", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);

        //            ddlGroup.DataSource = dtGr;
        //            ddlGroup.DataTextField = "GroupName";
        //            ddlGroup.DataValueField = "Groupid";
        //            ddlGroup.DataBind();
        //            ListItem oLi = new ListItem("<--Select-->", "-1");
        //            ddlGroup.Items.Insert(0, oLi);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dtGr = null;
        //        oGbl = null;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblMemName.Text = "";
            //ddlLoanNo.Items.Clear();
            DataTable dtGr = null;
            string vGrpId = ddlGroup.SelectedValue;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            dtGr = new DataTable();
            CMember oMem = new CMember();
            try
            {
                //dtGr = oGbl.PopComboMIS("D", "Y", "MemberName", "MemberID", "MemberNo", "MemberMst", vGrpId, "GroupID", "ClosingDt", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);
                dtGr = oMem.GetMemListByGroupId(vGrpId, vBrCode);
                ddlMem.DataSource = dtGr;
                ddlMem.DataTextField = "MemberCode";
                ddlMem.DataValueField = "MemberId";
                ddlMem.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlMem.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtGr = null;
                oMem = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopLoanScheme()
        {
            DataTable dtGr = null;
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            dtGr = new DataTable();
            CGblIdGenerator oGbl = new CGblIdGenerator();
            dtGr = oGbl.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanType", "LoanTypeMst", 0, "EOID", "AA", gblFuction.setDate("01/01/1900"), "0000");
            ddlLoanSche.DataSource = dtGr;
            ddlLoanSche.DataTextField = "LoanType";
            ddlLoanSche.DataValueField = "LoanTypeId";
            ddlLoanSche.DataBind();
            ListItem oLi = new ListItem("<--Select-->", "-1");
            ddlLoanSche.Items.Insert(0, oLi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vCType = "", vCmId = "", vGrpId = "", vMemId = "", vRptPath = "";
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vFsId = "0", vLsId = "0";

            DataTable dt = null;
            CReports oRpt = new CReports();
            //ReportDocument rptDoc = new ReportDocument();

            if (rdbGrp.SelectedValue == "rdbNormal")
                vCType = "N";
            else if (rdbGrp.SelectedValue == "rdbAll")
                vCType = "A";
            else if (rdbGrp.SelectedValue == "rdbPrem")
                vCType = "P";
            else if (rdbGrp.SelectedValue == "rdbDeath")
                vCType = "D";

            //if (ddlF.SelectedIndex > 0)
            //    vFsId = Convert.ToInt32(ddlFS.SelectedValue);
            if (ddlLoanSche.SelectedIndex > 0)
                vLsId = Convert.ToString(ddlLoanSche.SelectedValue);
            if (ddlCo.SelectedIndex > 0)
                vCmId = ddlCo.SelectedValue;

            if (ddlGroup.SelectedIndex > 0)
                vGrpId = ddlGroup.SelectedValue;
            if (ddlMem.SelectedIndex > 0)
                vMemId = ddlMem.SelectedValue;

            dt = oRpt.rptFinalPaidList(vFromDt, vToDt, vCType, vFsId, vLsId, vCmId, vGrpId, vMemId, vBrCode);
            //if (vFsId > 0)
            //    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\FinalPaidFS.rpt";
            //else
            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptFinalPaid.rpt";

            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                rptDoc.SetParameterValue("pBranch", vBranch);
                rptDoc.SetParameterValue("FromDt", txtDtFrm.Text);
                rptDoc.SetParameterValue("ToDt", txtToDt.Text);
                rptDoc.SetParameterValue("pTitle", "Settled Client List");
                if (pMode == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Settled Client List Report");
                else if (pMode == "Excel")
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Settled Client List Report");

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
    }
}
