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
using System.IO;
using System.Text;
using FORCECA;
using FORCEBA;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class PartyLedger : CENTRUMBase
    {
        public string vSOA;
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
                this.PageHeading = "Party Ledger";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.nmuPartLedgRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Party Ledger", false);
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
        //private void PopRO()
        //{
        //    DataTable dt = null;
        //    CGblIdGenerator oGb = null;
        //    string vBrCode = "";
        //    Int32 vBrId = 0;
        //    DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //    try
        //    {
        //        //vBrCode = (string)Session[gblValue.BrnchCode];
        //        if (Session[gblValue.BrnchCode].ToString() != "0000")
        //            vBrCode = Session[gblValue.BrnchCode].ToString();
        //        else
        //            vBrCode = ddlBranch.SelectedValue.ToString();  
        //        vBrId = Convert.ToInt32(vBrCode);
        //        oGb = new CGblIdGenerator();
        //        dt = oGb.PopComboMIS("N", "N", "AA", "LoanId", "LoanNo", "LoanMst",0, "", "", vLogDt, vBrCode);
        //        ddlLoan.DataSource = dt;
        //        ddlLoan.DataTextField = "LoanNo";
        //        ddlLoan.DataValueField = "LoanId";
        //        ddlLoan.DataBind();
        //        ListItem oli = new ListItem("<--Select-->", "-1");
        //        ddlLoan.Items.Insert(0, oli);
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
        /// <param name="pFormat"></param>
        private void GetData(string pFormat)
        {
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            string vBranch = "", vBrCode = "", vMemName = "", vMemNo = "", vLnProduct = "";
            string vLnNo = "", vDisbDt = "", vLoanId = "", vSpouseNm = "";
            string vFundSource = "", vPurpose = "", vFileNm = "", vGroupName = "", vMarket = "", vEO = "";
            double vLoanAmt = 0, vIntAmt = 0, vOSAmt = 0, vTopupAmt = 0,vIntRate=0;
            string vRptPath = "";
            DataTable dt = null, dt1 = null;
            DataSet ds = null;
            CReports oRpt = null;
            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            try
            {
                //if (ddlLoan.SelectedIndex > 0)
                //    vLoanId = ddlLoan.SelectedValue;
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
                oRpt = new CReports();

                if (chk1stRepaySchedule.Checked == true)
                {
                    ds = oRpt.rptPartyLedger1st(vLoanId, vBrCode, vSOA);
                }
                else
                {
                    ds = oRpt.rptPartyLedger(vLoanId, vBrCode, vSOA);
                }
                
                
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];

                if (dt.Rows.Count > 0 && dt1.Rows.Count > 0)
                {

                    vOSAmt = Convert.ToDouble(dt.Rows[0]["LoanAmt"]);
                    foreach (DataRow dr in dt1.Rows)
                    {
                        //if (Convert.IsDBNull(dr["PrinCollAmt"]) == false)
                        //{
                        //    vOSAmt = vOSAmt - Convert.ToDouble(dr["PrinCollAmt"].ToString());
                        //    dr["OSAmt"] = vOSAmt;
                        //}
                        if (Convert.IsDBNull(dr["Principal_"]) == false)
                        {
                            vOSAmt = vOSAmt - Convert.ToDouble(dr["Principal_"].ToString());
                            dr["Out_Standing"] = Math.Round(vOSAmt, 2);
                        }
                    }
                    dt1.AcceptChanges();
                    vMemNo = dt.Rows[0]["MemberNo"].ToString();
                    vMemName = dt.Rows[0]["MemberName"].ToString();
                    vSpouseNm = dt.Rows[0]["Spouce"].ToString();
                    vFundSource = dt.Rows[0]["FundSource"].ToString();
                    vPurpose = dt.Rows[0]["Purpose"].ToString();
                    vLnNo = dt.Rows[0]["LoanNo"].ToString();
                    vLoanAmt = Convert.ToDouble(dt.Rows[0]["LoanAmt"]);// Convert.ToDouble(dt.Rows[0]["LOSAmt"]);
                    vIntAmt = Convert.ToDouble(dt.Rows[0]["IntAmt"]);//Convert.ToDouble(dt.Rows[0]["IOSAmt"]);
                    vDisbDt = dt.Rows[0]["LoanDt"].ToString();
                    vLnProduct = dt.Rows[0]["LoanType"].ToString();
                    vGroupName = dt.Rows[0]["Groupname"].ToString();
                    vMarket = dt.Rows[0]["Market"].ToString();
                    vEO = dt.Rows[0]["EoName"].ToString();
                    vTopupAmt = Convert.ToDouble(dt.Rows[0]["TopupAmt"]);
                    vIntRate = Convert.ToDouble(dt.Rows[0]["IntRate"]);
                    //dgRight2.DataSource = dt1;
                    //dgRight2.DataBind();
                    if (pFormat == "Excel")
                    {
                        DataGrid1.DataSource = dt1;
                        DataGrid1.DataBind();

                        vFileNm = "attachment;filename=PartyLedger.xls";
                        StringWriter sw = new StringWriter();
                        HtmlTextWriter htw = new HtmlTextWriter(sw);
                        htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
                        htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
                        htw.WriteLine("<tr><td align=center' colspan='13'><b><u><font size='5'>Unity Small Finance Bank Ltd.</font></u></b></td></tr>");
                        if (chk1stRepaySchedule.Checked == true)
                            htw.WriteLine("<tr><td align=center' colspan='13'><b><u><font size='3'>Party Ledger - Rescheduled Loan</font></u></b></td></tr>");
                        else
                            htw.WriteLine("<tr><td align=center' colspan='13'><b><u><font size='3'>Party Ledger</font></u></b></td></tr>");
                        //htw.WriteLine("<tr><td colspan='2'><b>Member No:</b></td><td align='left' colspan='3'><b>" + vMemNo + "</b></td><td colspan='2'><b>Member Name:</b></td><td align='left' colspan='4'><b>" + vMemName + "</b></td></tr>");
                        htw.WriteLine("<tr><td colspan='2'><b>Member No:</b></td><td align='left' colspan='3'><b>" + vMemNo + "</b></td><td colspan='2'><b>Member Name:</b></td><td align='left' colspan='4'><b>" + vMemName + "</b></td><td colspan='1'><b>RO Name:</b></td><td align='left' colspan='4'><b>" + vEO + "</b></td></tr>");
                        htw.WriteLine("<tr><td colspan='2'><b>Spouce Name:</b></td><td align='left' colspan='3'><b>" + vSpouseNm + "</b></td><td colspan='2'><b>Fund Source:</b></td><td align='left' colspan='4'><b>" + vFundSource + "</b></td><td colspan='1'><b>Group Name:</b></td><td align='left' colspan='4'><b>" + vGroupName + "</b></td></tr>"); //<td colspan='1'><b>Center Name:</b></td><td align='left' colspan='4'><b>" + vMarket + "</b></td>
                        htw.WriteLine("<tr><td colspan='2'><b>Loan No:</b></td><td align='left' colspan='3'><b>" + vLnNo + "</b></td><td colspan='2'><b>Purpose:</b></td><td align='left' colspan='4'><b>" + vPurpose + "</b></td><td colspan='2'><b>Topup Deduction Amount:</b></td><td align='left' colspan='4'><b>" + vTopupAmt + "</b></td></tr>");
                        htw.WriteLine("<tr><td colspan='2'><b>Loan Amount:</b></td><td align='left' colspan='3'><b>" + vLoanAmt + "</b></td><td colspan='2'><b>Interest Amount:</b></td><td align='left' colspan='4'><b>" + vIntAmt + "</b></td><td><b>Interest Rate:</b></td><td align='left' colspan='4'><b>" + vIntRate + "%</b></td></tr>");
                        htw.WriteLine("<tr><td colspan='2'><b>Disburse Date:</b></td><td align='left' colspan='3'><b>" + vDisbDt + "</b></td><td colspan='2'><b>Loan Scheme:</b></td><td align='left' colspan='4'><b>" + vLnProduct + "</b></td></tr>");
                        htw.WriteLine("<tr><td align=center' colspan='6'><b><u><font size='3'>Repayment Schedule</font></u></b></td><td align=center' colspan='7'><b><u><font size='3'>Collection Details</font></u></b></td></tr>");
                        //dgRight2.RenderControl(htw);
                        DataGrid1.RenderControl(htw);
                        htw.WriteLine("</td></tr>");
                        htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
                        htw.WriteLine("</table>");
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", vFileNm);
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.ms-excel";
                        //Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
                        Response.Write(sw.ToString());
                        Response.End();

                    }
                    else
                    {
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptPartyLedger.rpt";
                        using (ReportDocument rptDoc = new ReportDocument())
                        {
                            rptDoc.Load(vRptPath);
                            rptDoc.SetDataSource(dt1);
                            rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                            rptDoc.SetParameterValue("pMemNo", vMemNo);
                            rptDoc.SetParameterValue("pMemName", vMemName);
                            rptDoc.SetParameterValue("pRoName", vEO);
                            rptDoc.SetParameterValue("pSpouceName", vSpouseNm);
                            rptDoc.SetParameterValue("pFndSource", vFundSource);
                            rptDoc.SetParameterValue("pGroupName", vGroupName);
                            rptDoc.SetParameterValue("pLoanNo", vLnNo);
                            rptDoc.SetParameterValue("pPurpose", vPurpose);
                            rptDoc.SetParameterValue("pLoanAmt", vLoanAmt);
                            rptDoc.SetParameterValue("pIntAmt", vIntAmt);
                            rptDoc.SetParameterValue("pDisbDate", vDisbDt);
                            rptDoc.SetParameterValue("pLoanSchm", vLnProduct);
                            rptDoc.SetParameterValue("pTopupAmt", vTopupAmt);
                            rptDoc.SetParameterValue("pIntRate", vIntRate);
                            rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Party_Ledger");
                            rptDoc.Dispose();
                            Response.ClearHeaders();
                            Response.ClearContent();
                        }
                    }


                }
                else
                {
                    gblFuction.AjxMsgPopup("No Records Found.....");
                    return;
                }
                //else if (pFormat == "PDF")
                //{
                //    vFileNm = "attachment;filename=PartyLedger.pdf";  
                //    Response.ClearContent();
                //    Response.AddHeader("content-disposition", vFileNm);                    
                //    Response.ContentType = "application/pdf";
                //    StringReader sr = new StringReader(sw.ToString());
                //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                //    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                //    PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                //    pdfDoc.Open();
                //    htmlparser.Parse(sr);
                //    pdfDoc.Close();
                //    Response.Write(pdfDoc);
                //    Response.End();                                    
                //}
            }
            finally
            {
                dt = null;
                dt1 = null;
                ds = null;
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
            vSOA = "P";
            GetData("PDF");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            vSOA = "P";
            GetData("Excel");
        }

        protected void btnSOA_Click(object sender, EventArgs e)
        {
            vSOA = "S";
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            //PopRO();
        }
    }
}