using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.Web.Security;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace CENTRUM_SARALVYAPAR.WebPages.Private.Report
{
    public partial class GSTInvoice : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "GST Invoice";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuGSTInvoiceSARAL);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "GST Invoice", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void loadGrid(Int32 pPgIndx)
        {
            
            DataTable dt = null;
            CReports oRpt = null;
            try
            {
                oRpt = new CReports();
                dt = oRpt.GetGSTInvoiceByLoanNo(txtSrch.Text.Trim());
                gvMember.DataSource = dt.DefaultView;
                gvMember.DataBind();
                mvPos.ActiveViewIndex = 0;
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            gvMember.DataSource = null;
            gvMember.DataBind();
            loadGrid(0);
        }

        protected void gvMem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string InvoiceNo = "", InvType = "";
            if (e.CommandName == "cmdShow")
            {
                InvoiceNo = Convert.ToString(e.CommandArgument);
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                InvType = gvRow.Cells[10].Text;
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                foreach (GridViewRow gr in gvMember.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;
                GenGSTInvoice(InvoiceNo,InvType);
            }
        }

        private void GenGSTInvoice(string pInvoiceNo,string pInvType)
        {
            CReports oRpt = null;
            DataTable dt = null;
            string vRptPath = "";
            try
            {
                oRpt = new CReports();
                if (pInvType == "GST Invoice")
                {
                    dt = oRpt.rptGSTInvoice(pInvoiceNo,"GST");
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\GSTInvoice.rpt";
                }
                else if (pInvType == "Credit Note")
                {
                    dt = oRpt.rptGSTInvoice(pInvoiceNo, "CN");
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\CreditNote.rpt";
                }

                
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    if (pInvType == "GST Invoice")
                    {
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_GSTInvoice");
                    }
                    else if (pInvType == "Credit Note")
                    {
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_CREDITNOTE");
                    }
                    
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }
    }
}