using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using FORCEBA;
using CrystalDecisions.Shared;
using FORCECA;
using System.IO;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class rptMembershipRegd : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtFDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtTDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch();
                CheckAll();
                popDetail();
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Membership Register Report";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuMemRegd);
                if (this.UserID == 1) return;
                //if (this.CanReport == "Y")
                //{
                //}
                //else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                //{
                //    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Repayment Schedule", false);
                //}
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            //SetParameterForRptData("Excel");
            Export();
        }

        //protected void btnPdf_Click(object sender, EventArgs e)
        //{
        //    SetParameterForRptData("PDF");
        //}


        private void PopBranch()
        {
            DataTable dt = null;
            CUser oUsr = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            vBrCode = (string)Session[gblValue.BrnchCode];
            vBrId = Convert.ToInt32(vBrCode);
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["BranchCode"].ToString() != Session[gblValue.BrnchCode].ToString())
                    {
                        dr.Delete();
                    }
                }
                dt.AcceptChanges();
            }
            chkDtl.DataSource = dt;
            chkDtl.DataTextField = "BranchName";
            chkDtl.DataValueField = "BranchCode";
            chkDtl.DataBind();
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

        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
            popDetail();
        }


        protected void chkDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            popDetail();
        }
        private void Export()
        {
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = ViewState["Dtl"].ToString();
            string vWithDef = "";
            DateTime vFrmDt = gblFuction.setDate(txtFDt.Text);
            DateTime vToDt = gblFuction.setDate(txtTDt.Text);

            DataTable dt = null;
            CReports oRpt = new CReports();
            ReportDocument rptDoc = new ReportDocument();

            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            dt = oRpt.rptMemRegd(vFrmDt, vToDt, vBrCode);

            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();
            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            string vFileNm = "attachment;filename=Membership_Register.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' cellpadding='5' width='100%'>");
            htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
            //htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='5'>" + Session[gblValue.CompName].ToString() + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.CompName + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.Address1 + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.Address2 + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + vBranch + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>Membership Register Report</font></u></b></td></tr>");
            htw.WriteLine("<tr><td></td></tr>");
            htw.WriteLine("<tr><td align='center' colspan='" + dt.Columns.Count + "'><b>From : " + gblFuction.setDate(txtFDt.Text) + " To : " + gblFuction.setDate(txtTDt.Text) + "</b></td></tr>");
            htw.WriteLine("<tr><td></td></tr>");
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</table>");

            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.xls";
            this.EnableViewState = false;
            Response.Write(sw.ToString());
            Response.End();
        }


        //private void SetParameterForRptData(string pMode)
        //{
        //    DateTime vFromDt = gblFuction.setDate(txtFDt.Text);
        //    DateTime vToDt = gblFuction.setDate(txtTDt.Text);
        //    string vBrCode = ViewState["Dtl"].ToString();
        //    string vRptPath = "";
        //    string vBranch = Session[gblValue.BrName].ToString();
        //    //ReportDocument rptDoc = new ReportDocument();
        //    DataTable dt = new DataTable();
        //    CReports oRpt = new CReports();

        //    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\MemberSubmit.rpt";

        //    dt = oRpt.rptMemberSubmit(vFromDt, vToDt, vBrCode);
        //    using (ReportDocument rptDoc = new ReportDocument())
        //    {
        //        //rptDoc.Load(vRptPath);
        //        rptDoc.SetDataSource(dt);
        //        rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
        //        rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1("0000"));
        //        rptDoc.SetParameterValue("dtFrom", txtFDt.Text);
        //        rptDoc.SetParameterValue("dtTo", txtTDt.Text);

        //        if (pMode == "PDF")
        //            rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Member Submit Status");
        //        else if (pMode == "Excel")
        //            rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Member Submit Status");

        //        rptDoc.Dispose();
        //        Response.ClearHeaders();
        //        Response.ClearContent();
        //    }
        //}
    }
}