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
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class RepaymentSche : CENTRUMBase
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
                lblMemName.Text = "";
                ddlLoanNo.SelectedIndex = -1;
                ddlMem.SelectedIndex = -1;
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

                //PopRO();
                //PopLoanType();
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
                this.PageHeading = "Repayment Schedule";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.nmuRepayScheRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Repayment Schedule", false);
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
        /// <summary>
        /// 
        /// </summary>
        private void PopRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode;
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            vBrCode = Session[gblValue.BrnchCode].ToString();
            //}
            //else
            //{
            //    vBrCode = ddlBranch.SelectedValue.ToString();
            //}

            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlRO.DataSource = dt;
                ddlRO.DataTextField = "EoName";
                ddlRO.DataValueField = "EoId";
                ddlRO.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlRO.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRO_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vRoId = "";
            string vCentId = "";
            vCentId = Convert.ToString(ddlRO.SelectedValue);
            vRoId = Convert.ToString(ddlRO.SelectedValue);
            //PopCenter(vRoId);
            PopGroup(vCentId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vEOID"></param>
        //private void PopCenter(string vEOID)
        //{
        //    ddlMem.Items.Clear();
        //    ddlGroup.Items.Clear();
        //    DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //    DataTable dt = null;
        //    CGblIdGenerator oGb = null;
        //    string vBrCode = "";
        //    try
        //    {
        //        vBrCode = (string)Session[gblValue.BrnchCode];
        //        oGb = new CGblIdGenerator();
        //        dt = oGb.PopComboMIS("D", "N", "AA", "MarketID", "Market", "MarketMSt", vEOID, "EOID", "Tra_DropDate", vLogDt, vBrCode);
        //        ddlCent.DataSource = dt;
        //        ddlCent.DataTextField = "Market";
        //        ddlCent.DataValueField = "MarketID";
        //        ddlCent.DataBind();
        //        ListItem oli = new ListItem("<--Select-->", "-1");
        //        ddlCent.Items.Insert(0, oli);
        //    }
        //    finally
        //    {
        //        oGb = null;
        //        dt = null;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ddlCent_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string vCentId = "";
        //    vCentId = ddlCent.SelectedValue.ToString();
        //    PopGroup(vCentId);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vCenterID"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vGropId = "";
            vGropId = ddlGroup.SelectedValue.ToString();
            PopMember(vGropId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vGroupID"></param>
        private void PopMember(string vGroupID)
        {
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CMember oMem = null;
            string vBrCode = "";

            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oMem = new CMember();
                dt = oMem.GetMemListByGroupId(vGroupID, vBrCode);
                ddlMem.DataSource = dt;
                ddlMem.DataTextField = "MemberNo";
                ddlMem.DataValueField = "MemberID";
                ddlMem.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlMem.Items.Insert(0, oli);
            }
            finally
            {
                oMem = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMem_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            string vMemId = ddlMem.SelectedValue;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            CMember oMem = null;
            lblMemName.Text = "";
            ddlLoanNo.Items.Clear();
            try
            {
                oMem = new CMember();
                dt = oMem.GetMemberDtlByMemberNo(vMemId, vBrCode);
                if (dt.Rows.Count > 0)
                {
                    lblMemName.Text = dt.Rows[0]["MemberName"].ToString();
                    ddlLoanType_SelectedIndexChanged(sender, e);
                }
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopLoanType()
        {
            DataTable dt = null;
            CApplication oApp = null;
            try
            {
                oApp = new CApplication();
                dt = oApp.GetLoanTypeForApp("N", Session[gblValue.BrnchCode].ToString());
                ddlLoanType.DataSource = dt;
                ddlLoanType.DataTextField = "LoanType";
                ddlLoanType.DataValueField = "LoanTypeId";
                ddlLoanType.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlLoanType.Items.Insert(0, oLi);
            }
            finally
            {
                dt = null;
                oApp = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlLoanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vMemberID = "";
            Int32 vLoanTypId = 0;
            vMemberID = ddlMem.SelectedValue.ToString();
            vLoanTypId = Convert.ToInt32(ddlLoanType.SelectedValue);
            PopLoan(vMemberID, vLoanTypId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMemberID"></param>
        /// <param name="pLoanTypId"></param>
        private void PopLoan(string pMemberID, Int32 pLoanTypId)
        {
            ddlLoanNo.Items.Clear();
            DataTable dt = null;
            CLoan oLn = null;
            string vBrCode = "";

            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oLn = new CLoan();
                dt = oLn.GetLoanByTypIdMemId(pMemberID, pLoanTypId, vBrCode);
                ddlLoanNo.DataSource = dt;
                ddlLoanNo.DataTextField = "LoanNo";
                ddlLoanNo.DataValueField = "LoanId";
                ddlLoanNo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlLoanNo.Items.Insert(0, oli);
            }
            finally
            {
                oLn = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFormat"></param>
        private void GetData(string pFormat)
        {
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptRePayment.rpt";
            string vBranch = "", vBrCode = "", vGroup = "", vMemName = "", vMemNo = "", vLnProduct = "";
            string vLnNo = "", vDisbDt = "", vROName = "", vMarketNm = "", vLoanId = "";
            double vLoanAmt = 0, vIntvIntRate = 0.0;
            Int32 vCycle = 0, vTotalInstNo=0;
            DataTable dt = null, dt1 = null;
            DataSet ds = null;
            //ReportDocument rptDoc = null;
            CReports oRpt = null;
            try
            {
                //if (ddlLoanNo.SelectedIndex > 0)
                //    vLoanId = ddlLoanNo.SelectedValue;
                //else
                //{
                //    gblFuction.MsgPopup("No Records Found.");
                //    return;
                //}

                if (txtLnNo.Text != "")
                    vLoanId = txtLnNo.Text.Trim();
                else
                {
                    gblFuction.MsgPopup("Enter Loan No.");
                    return;
                }

                vBranch = Session[gblValue.BrName].ToString();
                //vBrCode = Session[gblValue.BrnchCode].ToString();

                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                else
                    vBrCode = ddlBranch.SelectedValue.ToString();

                //rptDoc = new ReportDocument();
                oRpt = new CReports();
                if (chk1stRepaySchedule.Checked == true)
                {
                    ds = oRpt.rptRepaySchedule1st(vLoanId, vBrCode);
                }
                else
                {
                    ds = oRpt.rptRepaySchedule(vLoanId, vBrCode);
                }
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                if (dt1.Rows.Count == 0)
                {
                    gblFuction.MsgPopup("No Data Found ....");
                    return;
                }
                vROName = dt.Rows[0]["EoName"].ToString();
                //vMarketNm = dt.Rows[0]["Market"].ToString();
                vMemName = dt.Rows[0]["MemberName"].ToString();
                vMemNo = dt.Rows[0]["MemberNo"].ToString();
                vGroup = dt.Rows[0]["GroupName"].ToString();
                vLnProduct = dt.Rows[0]["LoanType"].ToString();
                vLnNo = dt.Rows[0]["LoanNo"].ToString();
                vDisbDt = dt.Rows[0]["LoanDt"].ToString();
                vLoanAmt = Convert.ToDouble(dt.Rows[0]["LoanAmt"]);
                vIntvIntRate = Convert.ToDouble(dt.Rows[0]["IntRate"]);
                vCycle = Convert.ToInt32(dt.Rows[0]["dose"]);
                vTotalInstNo = Convert.ToInt32(dt.Rows[0]["TotalInstNo"]);
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt1);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", gblValue.Address1);
                    rptDoc.SetParameterValue("pAddress2", gblValue.Address2);
                    rptDoc.SetParameterValue("pBranch", vBranch);
                    if (chk1stRepaySchedule.Checked == true)
                        rptDoc.SetParameterValue("pTitle", "Repayment Schedule - Rescheduled Loan");
                    else
                        rptDoc.SetParameterValue("pTitle", "Repayment Schedule");
                    rptDoc.SetParameterValue("pRO", vROName);
                    rptDoc.SetParameterValue("pMarket", vMarketNm);
                    rptDoc.SetParameterValue("pGroup", vGroup);
                    rptDoc.SetParameterValue("pMemName", vMemName);
                    rptDoc.SetParameterValue("pMemNo", vMemNo);
                    rptDoc.SetParameterValue("pLnProduct", vLnProduct);
                    rptDoc.SetParameterValue("pLoanNo", vLnNo);
                    rptDoc.SetParameterValue("pLoanAmt", vLoanAmt);
                    rptDoc.SetParameterValue("pDisbDt", vDisbDt);
                    rptDoc.SetParameterValue("pMode", "R");
                    rptDoc.SetParameterValue("pIntvIntRate", vIntvIntRate);
                    rptDoc.SetParameterValue("pCycle", vCycle);
                    rptDoc.SetParameterValue("pTotalInstNo", vTotalInstNo);
                    if (pFormat == "PDF")
                    {
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Repayment Schedule");
                    }
                    else
                    {
                        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Repayment Schedule");
                    }
                    rptDoc.Dispose();
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
            finally
            {
                dt = null;
                dt1 = null;
                ds = null;
                //rptDoc = null;
                oRpt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            GetData("PDF");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            GetData("Excel");
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