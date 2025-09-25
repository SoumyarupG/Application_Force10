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
    public partial class MemSearch : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ddlMem.SelectedIndex = -1;
                PopRO();
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Member Search";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuMemSrchRpt);
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


        private void PopRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                //oGb = new CGblIdGenerator();
                //dt = oGb.PopComboMIS("D", "N", "AA", "EoId", "EoName", "EoMst", vBrCode, "BranchCode", "Tra_DropDate", vLogDt, vBrCode);
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


        protected void ddlRO_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vRoId = "";
            string vCentId = "";
            vRoId = Convert.ToString(ddlRO.SelectedValue);
            vCentId = Convert.ToString(ddlRO.SelectedValue);
            //PopCenter(vRoId);
            PopGroup(vCentId);
        }


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


        //protected void ddlCent_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string vCentId = "";
        //    vCentId = ddlCent.SelectedValue.ToString();
        //    PopGroup(vCentId);
        //}


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


        private void GetData(string pFormat)
        {
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\MemSearch.rpt";
            DataTable dt = null;
            string vMemId;
            string vBranch = Session[gblValue.BrName].ToString();
            //ReportDocument rptDoc = null;
            CReports oRpt = null;
            try
            {
                if (ddlMem.SelectedIndex > 0)
                    vMemId = ddlMem.SelectedValue;
                else
                {
                    gblFuction.MsgPopup("No Records Found.");
                    return;
                }
                //rptDoc = new ReportDocument();
                oRpt = new CReports();
                dt = oRpt.rptMemSearch(vMemId);
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", gblValue.Address1);
                    rptDoc.SetParameterValue("pTitle", "Member Detail Status Report");
                    rptDoc.SetParameterValue("pBranch", vBranch);
                    if (pFormat == "PDF")
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Member Search");
                    else
                        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Member Search");

                    rptDoc.Dispose();
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
            finally
            {
                dt = null;
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
