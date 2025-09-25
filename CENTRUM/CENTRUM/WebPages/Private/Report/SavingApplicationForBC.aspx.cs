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
    public partial class SavingApplicationForBC : CENTRUMBase
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
                FrmDt.Text = Session[gblValue.LoginDate].ToString();
                ToDt.Text = Session[gblValue.LoginDate].ToString();
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
                this.PageHeading = "Saving Application For BC";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuSavingApplBCRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Saving Application For BC", false);
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
                ViewState["List"] = null;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            CReports oRpt = null;
            try
            {
                DateTime vStDate = gblFuction.setDate(FrmDt.Text);
                DateTime vEndDate = gblFuction.setDate(ToDt.Text);
                oRpt = new CReports();
                //System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                ds = oRpt.BC_rptSavingApplication(vStDate, vEndDate, Session[gblValue.BrnchCode].ToString());
                //DataSet ds = (DataSet)ViewState["List"];
                //ViewState["List"] = ds;
                //DataGrid1.DataSource = ds.Tables[0];
                //DataGrid1.DataBind();
                //tdx.Controls.Add(DataGrid1);
                //tdx.Visible = false;
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(ds.Tables[0], "Customers");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string vFileNm = "";
                    vFileNm = "attachment;filename=Saving_Application_For_BC.xlsx";
                    //Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //Response.ContentType = "application/vnd.ms-excel";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                    //Response.Write("<table border='1' cellpadding='0' widht='100%'>");
                    //Response.Write("<tr><td align=center' colspan='" + ds.Tables[0].Columns.Count + "'><b><u><font size='5'>" + gblValue.CompName + "</font></u></b></td></tr>");
                    //Response.Write("<tr><td align=center' colspan='" + ds.Tables[0].Columns.Count + "'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrName].ToString()) + "</td></tr>");
                    //Response.Write("<tr><td align=center' colspan='" + ds.Tables[0].Columns.Count + "'><b><u><font size='3'>Saving Application For BC</font></u></b></td></tr>");
                    //Response.Write("<tr><td align=center' colspan='" + ds.Tables[0].Columns.Count + "'><b><u><font size='3'>Branch: " + Session[gblValue.BrName].ToString() + "</font></u></b></td></tr>");
                    //Response.Write("<tr><td align=center' colspan='" + ds.Tables[0].Columns.Count + "'><b><u><font size='3'>From Date: " + FrmDt.Text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; To Date: " + ToDt.Text + " </font></u></b></td></tr>");
                    //string tab = string.Empty;
                    //Response.Write("<tr>");
                    //foreach (DataColumn dtcol in ds.Tables[0].Columns)
                    //{
                    //    Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
                    //}
                    //Response.Write("</tr>");
                    //foreach (DataRow dtrow in ds.Tables[0].Rows)
                    //{
                    //    Response.Write("<tr style='height:20px;'>");
                    //    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    //    {
                    //        if (ds.Tables[0].Columns[j].ColumnName == "SOL_ID")
                    //        {
                    //            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                    //        }
                    //        else if (ds.Tables[0].Columns[j].ColumnName == "PIN")
                    //        {
                    //            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                    //        }
                    //        else if (ds.Tables[0].Columns[j].ColumnName == "MOBILE_NO")
                    //        {
                    //            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                    //        }
                    //        else if (ds.Tables[0].Columns[j].ColumnName == "KYC_NO")
                    //        {
                    //            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                    //        }
                    //        else if (ds.Tables[0].Columns[j].ColumnName == "MEMBER_ID")
                    //        {
                    //            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                    //        }
                    //        else if (ds.Tables[0].Columns[j].ColumnName == "URNID")
                    //        {
                    //            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                    //        }

                    //        else
                    //        {
                    //            Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
                    //        }
                    //    }
                    //    Response.Write("</tr>");
                    //}
                    //Response.Write("</table>");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                    //Response.End();


                }

                //DateTime vStDate = gblFuction.setDate(FrmDt.Text);
                //DateTime vEndDate = gblFuction.setDate(ToDt.Text);
                //oRpt = new CReports();
                //ds = oRpt.BC_rptSavingApplication(vStDate, vEndDate, Session[gblValue.BrnchCode].ToString());
                ////DataSet ds = (DataSet)ViewState["List"];
                //ViewState["List"] = ds;
                //DataGrid1.DataSource = ds;
                //DataGrid1.DataBind();
                //string vFileNm = "";
                //vFileNm = "attachment;filename=Saving Application For BC.xls";
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter htw = new HtmlTextWriter(sw);

                //htw.WriteLine("<table border='0' widht='100%'>");
                //htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='5'>" + gblValue.CompName + "</font></u></b></td></tr>");
                //htw.WriteLine("<tr><td align=center' colspan='7'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrName].ToString()) + "</td></tr>");
                //htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='3'>Saving Application For BC</font></u></b></td></tr>");
                //htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='3'>Branch: " + Session[gblValue.BrName].ToString() + "</font></u></b></td></tr>");
                //htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='3'>From Date: " + FrmDt.Text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; To Date: " + ToDt.Text + " </font></u></b></td></tr>");
                //DataGrid1.RenderControl(htw);
                //htw.WriteLine("</td></tr>");
                //htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
                //htw.WriteLine("</table>");
                //Response.ClearContent();
                //Response.AddHeader("content-disposition", vFileNm);
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                ////Response.ContentType = "application/vnd.ms-excel";
                //Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
                //Response.Write(sw.ToString());
                //Response.End();
            }
            finally
            {
                ds = null;
                oRpt = null;
            }
            
        }
    }
}