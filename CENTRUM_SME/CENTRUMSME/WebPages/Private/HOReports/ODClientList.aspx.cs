using System;
using System.Data;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CENTRUMSME.WebPages.Private.HOReports
{
    public partial class ODClientList : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                // PopBranch(Session[gblValue.UserName].ToString());
                // PopFund();
                txtAsDt.Text = Session[gblValue.LoginDate].ToString();
                //txtToDt.Text = Session[gblValue.LoginDate].ToString();
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
                this.PageHeading = "Over Due Client List";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                // this.GetModuleByRole(mnuID.mnuRecPayRpt);
                this.GetModuleByRole(mnuID.mnuHOODClientList);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Consolidate Portfolio Ageing", false);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            // GetData("PDF");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            string vFileNm = "";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBranch = Session[gblValue.BrName].ToString();
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            dt = oRpt.rptODClientList(vBrCode, gblFuction.setDate(txtAsDt.Text));
           // dt.Columns.Remove("Branch");
            vFileNm = "attachment;filename=" + txtAsDt.Text.Replace("/", "") + "_ODClientList_Report.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";

            HttpContext.Current.Response.Write("<style> .txt" + "\r\n" + "{mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
            Response.Write("<table border='1' cellpadding='0' width='100%'>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + " </font></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress2(vBrCode) + " </font></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> Branch - " + vBrCode + " - " + vBranch + " </font></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>  Over Due Client List </font></u></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> As On Date - " + txtAsDt.Text + " </font></b></td></tr>");
            Response.Write("<tr>");

            foreach (DataColumn dtCol in dt.Columns)
            {
                Response.Write("<td><b>" + dtCol.ColumnName + "<b></td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in dt.Rows)
            {
                Response.Write("<tr style='height:20px;'>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName == "CustId")
                    {
                        Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                    }
                    else if (dt.Columns[j].ColumnName == "LoanNo")
                    {
                        Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                    }
                    else if (dt.Columns[j].ColumnName == "PAR Category")
                    {
                        Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                    }
                    else if (dt.Columns[j].ColumnName == "Customer Residence Address")
                    {
                        Response.Write("<td nowrap class='txt' style='text-align: left; width:300px;' >" + Convert.ToString(dr[j]) + "</td>");
                    }
                    else if (dt.Columns[j].ColumnName == "Guarantor Name")
                    {
                        Response.Write("<td nowrap class='txt'  style='text-align: left; width:200px;' >" + Convert.ToString(dr[j]) + "</td>");
                    }
                    else if (dt.Columns[j].ColumnName == "Guarantor Mobile No")
                    {
                        Response.Write("<td nowrap class='txt'  style='text-align: left; width:200px;' >" + Convert.ToString(dr[j]) + "</td>");
                    }
                    else if (dt.Columns[j].ColumnName == "Guarantor Residence Address")
                    {
                        Response.Write("<td nowrap class='txt'  style='text-align: left; width:300px;' >" + Convert.ToString(dr[j]) + "</td>");
                    }
                    else if (dt.Columns[j].ColumnName == "Co Applicant Name")
                    {
                        Response.Write("<td nowrap  class='txt' style='text-align: left; width:200px;' >" + Convert.ToString(dr[j]) + "</td>");
                    }
                    else if (dt.Columns[j].ColumnName == "Co Applicant Mobile No")
                    {
                        Response.Write("<td nowrap class='txt' style='text-align: left; width:200px;' >" + Convert.ToString(dr[j]) + "</td>");
                    }
                    else if (dt.Columns[j].ColumnName == "Co Applicant Residence Address")
                    {
                        Response.Write("<td nowrap style='text-align: left; width:300px;' >" + Convert.ToString(dr[j]) + "</td>");
                    }
                    else
                    {
                        Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                    }
                }
            }
            Response.Write("</tr>");
            Response.Write("</table>");
            Response.Flush();
            Response.End();

        }
    }
}
