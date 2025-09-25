using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Web.UI;
using System.Web;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class DemandSheet : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
               // txtDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtDtTo.Text = Convert.ToString(Session[gblValue.LoginDate]);
                popRO();
                PopLoanType();
                //txtDtFrm.Enabled = false;
                //txtDtTo.Enabled = false;
                DateTime vToDt = gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate]));
                ceFrmDt.StartDate = vToDt;
                ceFrmDt.EndDate = vToDt.AddDays(1);
                CalendarExtender1.StartDate = vToDt; 
                CalendarExtender1.EndDate = vToDt.AddDays(1);
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
                this.PageHeading = "Demand Sheet";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuDmndSht);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Demand Sheet", false);
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
        private void popRO()
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
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlCo.DataSource = dt;
                ddlCo.DataTextField = "EoName";
                ddlCo.DataValueField = "EoId";
                ddlCo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCo.Items.Insert(0, oli);
            }
            finally
            {
                dt = null;
                oRO = null;
            }
        }

        private void PopCenter(string vCOID)
        {
            DataTable dtGr = null;
            CGblIdGenerator oGbl = null;
            try
            {
                ddlCenter.Items.Clear();
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oGbl = new CGblIdGenerator();
                dtGr = oGbl.PopComboMIS("S", "N", "", "MarketID", "Market", "MarketMst", vCOID, "EOID", "DropoutDt", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);
                dtGr.AcceptChanges();
                ddlCenter.DataSource = dtGr;
                ddlCenter.DataTextField = "Market";
                ddlCenter.DataValueField = "MarketID";
                ddlCenter.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlCenter.Items.Insert(0, oLi);
            }
            finally
            {
                dtGr = null;
                oGbl = null;
            }
        }


        private void PopGroup(string vCenterID)
        {
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
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
        private void PopLoanType()
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            CGblIdGenerator oCG = new CGblIdGenerator();
            Int32 vBrId = Convert.ToInt32(vBrCode);
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanType", "LoanTypeMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                ddlLT.DataSource = dt;
                ddlLT.DataTextField = "LoanType";
                ddlLT.DataValueField = "LoanTypeId";
                ddlLT.DataBind();
                ListItem oL1 = new ListItem("<-- Select -->", "-1");
                ddlLT.Items.Insert(0, oL1);
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
        protected void ddlCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCo.SelectedIndex > 0) PopCenter(ddlCo.SelectedValue);
            ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(ddlCo.SelectedValue));
            if (ddlCenter.SelectedIndex > 0) PopGroup(ddlCenter.SelectedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCenter.SelectedIndex > 0) PopGroup(ddlCenter.SelectedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            DateTime vAsonDt = gblFuction.setDate("");
            DateTime vFromDate=gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDate=gblFuction.setDate(txtDtTo.Text);
            DataGrid DataGrid1 = new DataGrid();
            double Day = (vToDate - vFromDate).TotalDays;
            if (Day > 30)
            {
                gblFuction.AjxMsgPopup("Date Difference Should not be more than 30 days.");
                return;
            }
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBranch = Session[gblValue.BrName].ToString();
            string vAddress1 = CGblIdGenerator.GetBranchAddress1(vBrCode);
            string vAddress2 = "";// CGblIdGenerator.GetBranchAddress2(vBrCode);
            string vCompName = gblValue.CompName;
            string vCmId = "", vMktId = "", vGroupId = "", vRptPath = "", vTitle = "Deemand Sheet";
            Int32 vLoanTypeId = 0;
            DataTable dt = null;
            //ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();
            //try
            //{

            TimeSpan t = vToDate - vFromDate;
            if (t.TotalDays > 2)
            {
                gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                return;
            }

            if (ddlCo.SelectedIndex > 0)
                vCmId = ddlCo.SelectedValue;
            if (ddlCenter.SelectedIndex > 0)
                vMktId = ddlCenter.SelectedValue;
            if (ddlGroup.SelectedIndex > 0)
                vGroupId = ddlGroup.SelectedValue;
            if (ddlLT.SelectedIndex > 0)
                vLoanTypeId = Convert.ToInt32(ddlLT.SelectedValue);            
            if (chkRestruct.Checked == true)
                dt = oRpt.rptDemandSheetRS(vFromDate, vToDate, vLoanTypeId, vCmId, vGroupId, vBrCode,vMktId);
            else
                dt = oRpt.rptDemandSheet(vFromDate, vToDate, vLoanTypeId, vCmId, vGroupId, vBrCode, vMktId);
            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptDemandSheetNew.rpt";
            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", vCompName);
                rptDoc.SetParameterValue("pAddress1", vAddress1);
                rptDoc.SetParameterValue("pAddress2", vAddress2);
                rptDoc.SetParameterValue("pBranch", vBranch);
                rptDoc.SetParameterValue("dtAsOn", "01/01/1900");
                rptDoc.SetParameterValue("dtFrom", vFromDate);
                rptDoc.SetParameterValue("dtTo", vToDate);
                rptDoc.SetParameterValue("pTitle", vTitle);

                if (pMode == "PDF")
                {
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Collection Disbursement Report");
                    rptDoc.Dispose();
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
                else
                {
                    //rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Collection Disbursement Report");                    
                    //DataGrid1.DataSource = dt;
                    //DataGrid1.DataBind();
                    //tdx.Controls.Add(DataGrid1);
                    //string vFileNm = "attachment;filename=Collection Disbursement Report";
                    //StringWriter sw = new StringWriter();
                    //HtmlTextWriter htw = new HtmlTextWriter(sw);
                    //htw.WriteLine("<table border='1' cellpadding='19' widht='100%'>");
                    //htw.WriteLine("<tr><td align=center' colspan='19'><b><u><font size='3'>" + gblValue.CompName + "</font></u></b></td></tr>");
                    //htw.WriteLine("<tr><td align=center' colspan='19'><b><u><font size='2'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + "</font></u></b></td></tr>");
                    //htw.WriteLine("<tr><td align=center' colspan='19'><b><u><font size='2'>Demand Sheet For the Period from " + txtDtFrm.Text + " to " + txtDtTo.Text + "</font></u></b></td></tr>");
                    //DataGrid1.RenderControl(htw);
                    //htw.WriteLine("</td></tr>");
                    //htw.WriteLine("<tr><td colspan='7'><b><u><font size='19'></font></u></b></td></tr>");
                    //htw.WriteLine("</table>");

                    //Response.ClearContent();
                    //Response.AddHeader("content-disposition", vFileNm+".xls");
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
                    //Response.Write(sw.ToString());
                    //Response.End();

                   string vFileNm = "attachment;filename=Collection Disbursement Report.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                    Response.Write("<table border='1' cellpadding='5' widht='120%'>");
                    Response.Write("<tr><td align=center' colspan='"+ dt.Columns.Count +"'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='"+dt.Columns.Count+"'><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>Demand Sheet For the Period from " + txtDtFrm.Text + " to " + txtDtTo.Text + "</font></b></td></tr>");              
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
                            if (dt.Columns[j].ColumnName == "BranchID"||dt.Columns[j].ColumnName == "ClientID")
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
            SetParameterForRptData("Excel");

        }
    }
}
