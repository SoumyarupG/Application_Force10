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
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;

namespace CENTRUM_VRIDDHIVYAPAR.WebPages.Private.Report
{
    public partial class RepaymentSche : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                popCustomer();
            }
        }
        protected void ddlCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CLoanRecovery oMem = null;
            try
            {
                string pCustId = ddlCust.SelectedValue.ToString();

                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oMem = new CLoanRecovery();
                dt = oMem.GetLoanNoByCustId(pCustId, Convert.ToInt32(Session[gblValue.BCProductId]),"R");
                ddlLoanNo.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlLoanNo.DataTextField = "LoanNo";
                    ddlLoanNo.DataValueField = "LoanId";
                    ddlLoanNo.DataSource = dt;
                    ddlLoanNo.DataBind();
                    ListItem oLi = new ListItem("<--Select-->", "-1");
                    ddlLoanNo.Items.Insert(0, oLi);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }


        }
        private void popCustomer()
        {
            DataTable dt = null;
            CLoanRecovery oCD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oCD = new CLoanRecovery();
                dt = oCD.GetRePayMember(Convert.ToInt32(Session[gblValue.BCProductId]), vBrCode);
                ddlCust.DataSource = dt;
                ddlCust.DataTextField = "CustName";
                ddlCust.DataValueField = "CustId";
                ddlCust.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCust.Items.Insert(0, oli);
            }
            finally
            {
                oCD = null;
                dt = null;
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Repayment Schedule";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.nmuRepayScheRpt);
                if (this.RoleId == 1) return;
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

        private void GetData(string pFormat)
        {
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RepaySche.rpt";
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vLoanId = "";
            DataTable dt = null;
            CReports oRpt = null;
            try
            {
                string vLoanNo = (Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string;
                oRpt = new CReports();
                dt = oRpt.rptRepaySchedule(vLoanNo, vBrCode, "N", Convert.ToInt32(Session[gblValue.BCProductId]));
                if (pFormat == "PDF")
                {
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt);
                        rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                        rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                        rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                        rptDoc.SetParameterValue("pBranch", vBranch);
                        rptDoc.SetParameterValue("pTitle", "Repayment Schedule");
                        //if (pFormat == "PDF")
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Repayment_Schedule");
                        //else
                        //    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Repayment_Schedule");
                        Response.ClearHeaders();
                        Response.ClearContent();
                    }
                }
                if (pFormat == "Excel")
                {
                    string vMemName = "", vProduct = "", vLnNo = "", vDisbDt = "",  vLInstDt = "", vFileNm = "", vLoanCycle = "",  vNoofInst = "", vRepFreq = "";
                    double vLoanAmt = 0, vIntRate = 0, vTotInstAmt = 0, vInsuAmt = 0, vProcFees = 0,vPropInsu=0,vCERSAIChargee=0,vBrokenPrdInt=0;
                    vFileNm = "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "_Repayment_Schedule.xls";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        vBranch = dt.Rows[0]["Branch"].ToString();
                        vMemName = dt.Rows[0]["MemberName"].ToString();
                        vLoanCycle = dt.Rows[0]["LoanCycle"].ToString();

                        vDisbDt = dt.Rows[0]["LoanDt"].ToString();
                        vLnNo = dt.Rows[0]["LoanNo"].ToString();
                        vLoanAmt = Convert.ToDouble(dt.Rows[0]["LoanAmt"]);
                        vNoofInst = dt.Rows[0]["TotalInstNo"].ToString();
                        vTotInstAmt = Convert.ToDouble(dt.Rows[0]["IntAmt"]);
                        vLInstDt = dt.Rows[0]["LInstDt"].ToString();
                        vRepFreq = dt.Rows[0]["RepFreq"].ToString();
                        vProcFees = Convert.ToDouble(dt.Rows[0]["ProcFees"]);
                        vProduct = dt.Rows[0]["LoanTypeName"].ToString();
                        vIntRate = Convert.ToDouble(dt.Rows[0]["IntRate"]);
                        vInsuAmt = Convert.ToDouble(dt.Rows[0]["InsuAmt"]);
                        vPropInsu = Convert.ToDouble(dt.Rows[0]["PropertyInsurance"]);
                        vCERSAIChargee = Convert.ToDouble(dt.Rows[0]["CERSAICharge"]);
                        vBrokenPrdInt = Convert.ToDouble(dt.Rows[0]["BrkPrdIntAct"]);
                    }

                    htw.WriteLine("<table border='1' cellpadding='5' widht='100%'>");
                    htw.WriteLine("<tr><td align='center' colspan='10'><b><font size='5'>" + gblValue.CompName + "</font></b></td></tr>");
                    htw.WriteLine("<tr><td align='center' colspan='10'><b><font size='2'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + "</font></b></td></tr>");
                    htw.WriteLine("<tr><td align='center' colspan='10'><b><font size='2'>" + CGblIdGenerator.GetBranchAddress2(vBrCode) + "</font></b></td></tr>");
                    htw.WriteLine("<tr><td align='center' colspan='10'><b><font size='2'>Repayment Schedule</font></b></td></tr>");
                    htw.WriteLine("<tr><td><b><font size='3'>Branch :</font></b></td><td colspan='9'><b><font size='3'>" + vBranch + "</font></b></td></tr>");
                    htw.WriteLine("<tr><td colspan='2'><b>Customer Name :</b></td><td align='left' colspan='3'><b>" + vMemName + "</b></td><td colspan='2'><b>Loan Cycle :</b></td><td align='left' colspan='3'><b>" + vLoanCycle + "</b></td></tr>");
                    htw.WriteLine("<tr><td colspan='2'><b>Disbursed Date :</b></td><td align='left' colspan='3'><b>" + vDisbDt + "</b></td><td colspan='2'><b>Disbursed Loan No :</b></td><td align='left' colspan='3'><b>" + vLnNo + "</b></td></tr>");
                    htw.WriteLine("<tr><td colspan='2'><b>Loan Amt (Rs.) :</b></td><td align='left' colspan='3'><b>" + vLoanAmt + "</b></td><td colspan='2'><b>No of Installment :</b></td><td align='left' colspan='3'><b>" + vNoofInst + "</b></td></tr>");
                    htw.WriteLine("<tr><td colspan='2'><b>Total Interest (Rs.) :</b></td><td align='left' colspan='3'><b>" + vTotInstAmt + "</b></td><td colspan='2'><b>Last Installment Date :</b></td><td align='left' colspan='3'><b>" + vLInstDt + "</b></td></tr>");
                    htw.WriteLine("<tr><td colspan='2'><b>Broken Period Interest (Rs.)  :</b></td><td align='left' colspan='3'><b>" + vBrokenPrdInt + "</b></td><td colspan='2'><b>Repayment Frequency :</b></td><td align='left' colspan='3'><b>" + vRepFreq + "</b></td></tr>");
                    htw.WriteLine("<tr><td colspan='2'><b>Processing Fees + GST (Rs.) :</b></td><td align='left' colspan='3'><b>" + vProcFees + "</b></td><td colspan='2'><b>Loan Scheme :</b></td><td align='left' colspan='3'><b>" + vProduct + "</b></td></tr>");
                    //htw.WriteLine("<tr><td colspan='2'><b>Credit Shield + GST (Rs.) :</b></td><td align='left' colspan='3'><b>" + vInsuAmt + "</b></td><td colspan='2'><b>CERSAI Charge + GST (Rs.) </b></td><td align='left' colspan='3'><b>" + vCERSAIChargee + "</b></td></tr>");
                    htw.WriteLine("<tr><td colspan='2'><b>Insurance + GST (Rs.)  :</b></td><td align='left' colspan='3'><b>" + vPropInsu + "</b></td><td colspan='2'><b>Interest Rate (%)</b></td><td align='left' colspan='3'><b>" + vIntRate + "</b></td></tr>");
                    htw.WriteLine("<tr><td colspan='10'>&nbsp;</td></tr>");
                    htw.WriteLine("<tr><td rowspan='2' align='center'>Inst No</td><td rowspan='2' align='center'>Due Date</td><td colspan='3' align='center'>Due Amount</td><td colspan='3' align='center'>Outstanding</td><td colspan='2' align='center'>Initial</td></tr>");
                    htw.WriteLine("<tr><td align='center'>Principal</td><td align='center'>Interest</td><td align='center'>Total</td><td align='center'>Principal</td><td align='center'>Interest</td><td align='center'>Total</td><td align='center'>Credit Officer</td><td align='center'>Branch Manager</td></tr>");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            htw.WriteLine(string.Format("<tr><td align='center'>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>&nbsp;</td><td>&nbsp;</td></tr>", dr["InstNo"].ToString(), dr["DueDt"].ToString(), dr["PrinceAmt"].ToString(), dr["InstAmt"].ToString(), dr["TotDue"].ToString(), dr["Outstanding"].ToString(), dr["IntOut"].ToString(), ((Convert.ToDouble(dr["Outstanding"].ToString()) + Convert.ToDouble(dr["IntOut"].ToString())) == 0 ? "" : (Convert.ToDouble(dr["Outstanding"].ToString()) + Convert.ToDouble(dr["IntOut"].ToString())).ToString())));
                        }
                    }
                    htw.WriteLine("<tr><td colspan='10'>&nbsp;</td></tr>");
                    htw.WriteLine("</table>");

                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Write(sw.ToString());
                    Response.End();
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            GetData("PDF");
        }

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            GetData("Excel");
        }

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
