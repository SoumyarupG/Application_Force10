using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class RptNEFTPrint : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtAsOnDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                //popBatchNo();
                //PopBank();
            }
        }
        private void PopBank()
        {
            DataTable dt = null;
            CNEFTTransfer oNeft = null;
            //string vBrCode = "";
            //Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oNeft = new CNEFTTransfer();
            dt = oNeft.PopBank();

            ddlBank.DataSource = dt;
            ddlBank.DataTextField = "Desc";
            ddlBank.DataValueField = "DescId";
            ddlBank.DataBind();
            ListItem olist = new ListItem("<--select-->", "-1");
            ddlBank.Items.Insert(0, olist);
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "NEFT Print";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHOPrintNEFT);
                if (this.UserID == 1) return;
                if (this.CanView == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Consolidated Reports", false);
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
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
        }

        //private void popBatchNo()
        //{
        //    DataTable dt = null;
        //    CGblIdGenerator oGb = null;
        //    try
        //    {
        //        oGb = new CGblIdGenerator();
        //        dt = oGb.GetBatchNoByTransDate(gblFuction.setDate(txtAsOnDt.Text));
        //        ddlBatchNo.DataSource = dt;
        //        ddlBatchNo.DataTextField = "BATCHID";
        //        ddlBatchNo.DataValueField = "BATCHID";
        //        ddlBatchNo.DataBind();
        //        ListItem oli = new ListItem("<--Select-->", "-1");
        //        ddlBatchNo.Items.Insert(0, oli);
        //    }
        //    finally
        //    {
        //        oGb = null;
        //        dt = null;
        //    }
        //}        

        private void SetParameterForRptData(string pMode)
        {
            //string vBatchNo = ddlBatchNo.SelectedValue;
            DataTable dt = null;
            CReports oRpt = new CReports();
            //string vFileNm = "attachment;filename=NEFT Print Letter_" + ddlBatchNo.SelectedValue.ToString() + ".xls";
            string vFileNm = "attachment;filename=NEFT Print Letter_" + DateTime.Now + ".xls";
            try
            {
                oRpt = new CReports();
                dt = oRpt.rptNEFTPrint(gblFuction.setDate(txtAsOnDt.Text), ddlType.SelectedValue);
                if (dt.Rows.Count > 0)
                {
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                    Response.Write("<table border='1' cellpadding='5' widht='120%'>");                 
                    string tab = string.Empty;
                    Response.Write("<tr>");
                    foreach (DataColumn dtcol in dt.Columns)
                    {
                        Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
                    }
                    Response.Write("</tr>");
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        Response.Write("<tr style='height:20px;'>");
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Columns[j].ColumnName == "Debit Ac No" || dt.Columns[j].ColumnName == "Beneficiary Ac No"
                                || dt.Columns[j].ColumnName == "Add Details 1" || dt.Columns[j].ColumnName == "Date")
                            {
                                Response.Write("<td &nbsp nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                            }
                            else
                            {
                                Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
                            }
                        }
                        Response.Write("</tr>");
                    }

                    Response.Write("</table>");
                    Response.End();
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Data Found.");
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }


        //protected void txtAsOnDt_TextChanged(object sender, EventArgs e)
        //{
        //    if (txtAsOnDt.Text != "")
        //    {
        //        popBatchNo();
        //    }
        //}

    }
}