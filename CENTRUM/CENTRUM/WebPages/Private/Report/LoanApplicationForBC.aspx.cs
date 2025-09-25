using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using FORCECA;
using FORCEBA;
using ClosedXML.Excel;


namespace CENTRUM.WebPages.Private.Report
{
    public partial class LoanApplicationForBC : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                FrmDt.Text = Session[gblValue.LoginDate].ToString();
                ToDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Loan Application For BC";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanAppBCRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Application For BC", false);
                }
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
                ViewState["List"] = null;
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            CReports oRpt = null;
            try
            {
                DateTime vStDate = gblFuction.setDate(FrmDt.Text);
                DateTime vEndDate = gblFuction.setDate(ToDt.Text);
                //oRpt = new CReports();
                //ds = oRpt.BC_rptSavingApplication(vStDate, vEndDate, Session[gblValue.BrnchCode].ToString());
                //DataGrid1.DataSource = ds;
                //DataGrid1.DataBind();
                //ViewState["List"] = ds;
            }
            finally
            {
                ds = null;
                oRpt = null;
            }
        }

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            CReports oRpt = null;
            try
            {
                DateTime vStDate = gblFuction.setDate(FrmDt.Text);
                DateTime vEndDate = gblFuction.setDate(ToDt.Text);
                oRpt = new CReports();
                ds = oRpt.BC_rptLoanApplication(vStDate, vEndDate, Session[gblValue.BrnchCode].ToString());

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(ds.Tables[0], "Customers");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string vFileNm = "";
                    vFileNm = "attachment;filename=Loan_Application_For_BC.xlsx";
                    //Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            finally
            {
                ds = null;
                oRpt = null;
            }

        }
    }
}