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


namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HODayEndReport : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                //FrmDt.Text = Session[gblValue.LoginDate].ToString();
                //ToDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Day End Report For HO";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDayEndRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Day End Report For HO", false);
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

        //protected void btnShow_Click(object sender, EventArgs e)
        //{
        //    DataSet ds = null;
        //    CReports oRpt = null;
        //    try
        //    {
        //        DateTime vStDate = gblFuction.setDate(FrmDt.Text);
        //        DateTime vEndDate = gblFuction.setDate(ToDt.Text);
        //        //oRpt = new CReports();
        //        //ds = oRpt.BC_rptSavingApplication(vStDate, vEndDate, Session[gblValue.BrnchCode].ToString());
        //        //DataGrid1.DataSource = ds;
        //        //DataGrid1.DataBind();
        //        //ViewState["List"] = ds;
        //    }
        //    finally
        //    {
        //        ds = null;
        //        oRpt = null;
        //    }
        //}

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            CReports oRpt = null;
            try
            {
                //DateTime vStDate = gblFuction.setDate(FrmDt.Text);
                //DateTime vEndDate = gblFuction.setDate(ToDt.Text);
                string vBranchCode = Session[gblValue.BrnchCode].ToString();
                oRpt = new CReports();
                System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                ds = oRpt.GetDayEnd();
                //DataSet ds = (DataSet)ViewState["List"];
                ViewState["List"] = ds;
                DataGrid1.DataSource = ds.Tables[0];
                DataGrid1.DataBind();
                tdx.Controls.Add(DataGrid1);
                tdx.Visible = false;
                string vFileNm = "";
                vFileNm = "attachment;filename=Day End Report For HO.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", vFileNm);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                Response.Write("<table border='1' cellpadding='0' widht='100%'>");
                Response.Write("<tr><td align=center' colspan='" + ds.Tables[0].Columns.Count + "'><b><u><font size='5'>" + gblValue.CompName + "</font></u></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + ds.Tables[0].Columns.Count + "'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrName].ToString()) + "</td></tr>");
                Response.Write("<tr><td align=center' colspan='" + ds.Tables[0].Columns.Count + "'><b><u><font size='3'>Day End Report For HO</font></u></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + ds.Tables[0].Columns.Count + "'><b><u><font size='3'>Branch: " + Session[gblValue.BrName].ToString() + "</font></u></b></td></tr>");
                //Response.Write("<tr><td align=center' colspan='" + ds.Tables[0].Columns.Count + "'><b><u><font size='3'>From Date: " + FrmDt.Text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; To Date: " + ToDt.Text + " </font></u></b></td></tr>");
                string tab = string.Empty;
                Response.Write("<tr>");
                foreach (DataColumn dtcol in ds.Tables[0].Columns)
                {
                    Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
                }
                Response.Write("</tr>");
                foreach (DataRow dtrow in ds.Tables[0].Rows)
                {
                    Response.Write("<tr style='height:20px;'>");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (ds.Tables[0].Columns[j].ColumnName == "BranchCode")
                        {
                            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                        }
                        else if (ds.Tables[0].Columns[j].ColumnName == "BranchName")
                        {
                            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                        }
                        else if (ds.Tables[0].Columns[j].ColumnName == "LastDayEnd")
                        {
                            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                        }
                        else
                        {
                            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                        }
                    }
                    Response.Write("</tr>");
                }
                Response.Write("</table>");
                Response.End();
            }
            finally
            {
                ds = null;
                oRpt = null;
            }

        }
    }
}