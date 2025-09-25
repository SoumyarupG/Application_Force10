using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CENTRUMCA;
using CENTRUMBA;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web;

namespace CENTRUM_VRIDDHIVYAPAR.WebPages.Private.Report
{
    public partial class PartyLedgerNew : CENTRUMBAse
    {
        protected int vPgNo = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                // popCustomer();
                //ddlLoan.Items.Clear();
                //ListItem oLi = new ListItem("<--Select-->", "-1");
                //ddlLoan.Items.Insert(0, oLi);
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
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Party Ledger";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
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

        private bool ValidateDate()
        {
            bool vRst = true;
            return vRst;
        }
        private void GetData(string pFormat)
        {
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\PartyLedger.rpt";
            string vBrCode = ddlBranch.SelectedValue;
            string vLoanNo = txtLoanNo.Text;
            DataSet ds = new DataSet();
            DataTable dt, dt1 = null;
            CReports oRpt = null;

            try
            {
                oRpt = new CReports();
                if (vLoanNo == "")
                {
                    gblFuction.AjxMsgPopup("Please Select a Loan No.");
                    return;
                }

                using (ReportDocument rptDoc = new ReportDocument())
                {
                    rptDoc.Load(vRptPath);
                    ds = oRpt.rptPartyLedgerPDF(vLoanNo, vBrCode, Convert.ToInt32(Session[gblValue.BCProductId]));
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    rptDoc.SetDataSource(dt);
                    rptDoc.Subreports["OtherCollection.rpt"].SetDataSource(dt1);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                    rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                    rptDoc.SetParameterValue("pBranch", vBrCode);
                    rptDoc.SetParameterValue("pTitle", "Party Ledger");

                    if (pFormat == "PDF")
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Party_Ledger");
                    //else
                    //    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Party_Ledger");
                    Response.ClearContent();
                    Response.ClearHeaders();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            if (ValidateDate() == false)
            {
                gblFuction.MsgPopup("Please Set The Valid Date Range.");
                return;
            }
            else
            {
                GetData("PDF");
            }
        }

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            string vMemName = "", vProduct = "", vLnNo = "", vDisbDt = "", vFileNm = "", vInstNo = "", vPoolTagging = "";
            double vLoanAmt = 0, vIntAmt = 0, vIntRate = 0;
            vFileNm = "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "_Party_Ledger.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            CReports oRpt = new CReports();
            DataTable dt, dt1 = new DataTable();
            DataSet ds = null;
            string vBrCode = ddlBranch.SelectedValue;
            string vLoanNo = txtLoanNo.Text;
            string vCmpName = Convert.ToString(gblValue.CompName);
            string vBranch = Session[gblValue.BrName].ToString();

            ////if (chk1stRepaySchedule.Checked == true)
            ////    ds = oRpt.rptPartyLedger1st(vLoanNo, vBrCode);
            ////else
            ds = oRpt.rptPartyLedger(vLoanNo, vBrCode);
            dt = ds.Tables[0];
            dt1 = ds.Tables[1];

            if (dt != null && dt.Rows.Count > 0)
            {
                vBranch = dt.Rows[0]["Branch"].ToString();
                vLnNo = dt.Rows[0]["LoanNo"].ToString();
                vDisbDt = dt.Rows[0]["LoanDt"].ToString();
                vMemName = dt.Rows[0]["MemName"].ToString();
                vProduct = dt.Rows[0]["LoanTypeName"].ToString();
                vLoanAmt = Convert.ToDouble(dt.Rows[0]["LoanAmt"]);
                vIntRate = Convert.ToDouble(dt.Rows[0]["IntRate"]);
                vIntAmt = Convert.ToDouble(dt.Rows[0]["InstallmentAmt"]);
                vPoolTagging = Convert.ToString(dt.Rows[0]["FundSource"]);
            }

            htw.WriteLine("<table border='1' cellpadding='5' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan='32'><b><font size='5'>" + vCmpName + "</font></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='32'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + "</font></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='32'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress2(vBrCode) + "</font></b></td></tr>");
            //if (chk1stRepaySchedule.Checked == true)
            //    htw.WriteLine("<tr><td align=center' colspan='32'><b><font size='3'>Party Ledger - Rescheduled Loan</font></b></td></tr>");
            //else
            htw.WriteLine("<tr><td align=center' colspan='32'><b><font size='3'>Party Ledger</font></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='32'><b><font size='3'>" + vBranch + "</font></b></td></tr>");
            htw.WriteLine("<tr><td align='right' colspan='4'><b>Disbursement No :</b></td><td align='left' colspan='5' style='width:300px;' ><b>" + "&nbsp;" + vLnNo + "</b></td><td align='right' colspan='2'><b>Loan Amount :</b></td><td align='left' colspan='18'><b>" + vLoanAmt + "</b></td></tr>");
            htw.WriteLine("<tr><td align='right' colspan='4'><b>Disbursement Date :</b></td><td align='left' colspan='5' style='width:300px;' ><b>" + vDisbDt + "</b></td><td align='right' colspan='2'><b>Interest Rate (%) :</b></td><td align='left' colspan='18'><b>" + vIntRate + "</b></td></tr>");
            htw.WriteLine("<tr><td align='right' colspan='4'><b>Member Name :</b></td><td align='left' colspan='5' style='width:300px;'><b>" + vMemName + "</b></td><td align='right' colspan='2'><b>Installment :</b></td><td align='left' colspan='18'><b>" + vIntAmt + "</b></td></tr>");
            htw.WriteLine("<tr><td align='right' colspan='4'><b>Loan Product :</b></td><td align='left' colspan='5' style='width:300px;'><b>" + vProduct + "</b></td><td align='right' colspan='2'><b>Pool Tagging :</b></td><td align='left' colspan='18'><b>" + vPoolTagging + "</b></td></tr>");
            htw.WriteLine("<tr><td colspan='32'>&nbsp;</td></tr>");
            htw.WriteLine("<tr><td rowspan='2' align='center' style='width:60px;'>Serial No</td><td rowspan='2' align='center'>Due Date</td><td rowspan='2' align='center'>Principal Due</td><td rowspan='2' align='center'>Interest Due</td><td rowspan='2' align='center'>Installment</td><td rowspan='2' align='center'>Interest Payment Status</td><td rowspan='2' align='center'>Principal Payment Status</td><td colspan='20' align='center'>Received Amount</td><td colspan='1' align='center'>Balance Outstanding</td><td colspan='3' align='center'>Due Amount(After Collection)</td><td rowspan='2' align='center'>Advance Adjusted Aginst Principal</td></tr>");
            htw.WriteLine("<tr><td align='center'>Received Date</td><td align='center'>Principal</td><td align='center'>Interest</td><td align='center'>Advance</td><td align='center' style='width:100px;'>Penal Charge Amount</td><td align='center' style='width:100px;'>Penal Charge Waived</td><td align='center' style='width:100px;'>Penal Charge CGST</td><td align='center' style='width:100px;'>Penal Charge SGST</td><td align='center' style='width:100px;'>Visit Charge</td><td align='center' style='width:100px;'>Visit Charge Waived</td><td align='center' style='width:100px;'>Visit Charge CGST</td><td align='center' style='width:100px;'>Visit Charge SGST</td><td align='center' style='width:100px;'>Bounce Received</td><td align='center' style='width:100px;'>Bounce Waived</td><td align='center' style='width:100px;'>Bounce Charge CGST</td><td align='center' style='width:100px;'>Bounce Charge SGST</td><td align='center' style='width:100px;'>PreClose Charge</td><td align='center'>Total</td><td align='center'>Excess Amount</td><td align='center'>Interest on Advance</td><td align='center'>Principal</td><td align='center'>Bounce Due</td><td align='center'>Delay Payment Due</td><td align='center'>Visit Charge Due</td></tr>");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["SlNo"].ToString() != "0")
                        htw.WriteLine(string.Format("<tr><td align='center'>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td><td>{11}</td><td>{12}</td><td>{13}</td><td>{14}</td><td>{15}</td><td>{16}</td><td>{17}</td><td>{18}</td><td>{19}</td><td>{20}</td><td>{21}</td><td>{22}</td><td>{23}</td><td>{24}</td><td>{25}</td><td>{26}</td><td>{27}</td><td>{28}</td><td>{29}</td><td>{30}</td><td>{31}</td></tr>",
                                                            dr["SlNo"].ToString(), dr["DueDt"].ToString(), dr["PrinceAmt"].ToString(), dr["InstAmt"].ToString(), dr["ResAmt"].ToString(), dr["Payment Status"].ToString(), dr["Principal Payment Status"].ToString(), Convert.IsDBNull(dr["AccDate"].ToString()) ? "" : dr["AccDate"].ToString(), dr["PrinCollAmt"].ToString() == "0" ? "" : dr["PrinCollAmt"].ToString(), dr["IntCollAmt"].ToString() == "0" ? "" : dr["IntCollAmt"].ToString(), dr["AdvanceAmt"].ToString() == "0" ? "" : dr["AdvanceAmt"].ToString(), dr["PenCollAmt"].ToString() == "0" ? "" : dr["PenCollAmt"].ToString(), dr["PenWaive"].ToString() == "0" ? "" : dr["PenWaive"].ToString(), dr["PenCGSTAmt"].ToString() == "0" ? "" : dr["PenCGSTAmt"].ToString(), dr["PenSGSTAmt"].ToString() == "0" ? "" : dr["PenSGSTAmt"].ToString(), dr["VisitChargeRec"].ToString() == "0" ? "" : dr["VisitChargeRec"].ToString(), dr["VisitChargeWaive"].ToString() == "0" ? "" : dr["VisitChargeWaive"].ToString(), dr["VisitChargeCGST"].ToString() == "0" ? "" : dr["VisitChargeCGST"].ToString(), dr["VisitChargeSGST"].ToString() == "0" ? "" : dr["VisitChargeSGST"].ToString(), dr["BounceReceived"].ToString() == "0" ? "" : dr["BounceReceived"].ToString(), dr["BounceWave"].ToString() == "0" ? "" : dr["BounceWave"].ToString(), dr["BounceCGSTAmt"].ToString() == "0" ? "" : dr["BounceCGSTAmt"].ToString(), dr["BounceSGSTAmt"].ToString() == "0" ? "" : dr["BounceSGSTAmt"].ToString(), dr["PreCloseChrge"].ToString() == "0" ? "" : dr["PreCloseChrge"].ToString(), dr["CollAmt"].ToString() == "0" ? "" : dr["CollAmt"].ToString(), dr["ExcessAmt"].ToString() == "0" ? "" : dr["ExcessAmt"].ToString(), dr["InterestOnAdvance"].ToString() == "0" ? "" : dr["InterestOnAdvance"].ToString(), dr["TotPrnOS"].ToString() == "0" ? "" : dr["TotPrnOS"].ToString(), dr["BounceDue"].ToString() == "0" ? "" : dr["BounceDue"].ToString(), dr["PenDue"].ToString() == "0" ? "" : dr["PenDue"].ToString(), dr["VisitChargeDue"].ToString() == "0" ? "" : dr["VisitChargeDue"].ToString(), dr["AdvanceAdjustedAgainstPrincipal"].ToString() == "0" ? "" : dr["AdvanceAdjustedAgainstPrincipal"].ToString()));
                }
            }
            htw.WriteLine("<tr><td colspan='24'>&nbsp;</td></tr>");
            htw.WriteLine("<tr><td colspan='24'>Other Collection</td></tr>");
            htw.WriteLine("<tr><td align='center'>EntryDate</td><td align='center'>Charges</td><td align='center'>Amount</td></tr>");
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    htw.WriteLine(string.Format("<tr><td align='center'>{0}</td><td>{1}</td><td>{2}</td></tr>",
                                                            dr["EntryDate"].ToString(), dr["Charges"].ToString(), dr["Amount"].ToString()));
                }
            }
            htw.WriteLine("</table>");

            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(sw.ToString());
            Response.End();
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


        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
            string vBrCode = Convert.ToString(Session[gblValue.BrnchCode]);
            ViewState["ID"] = null;
            try
            {
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                    if (vBrCode != "0000")
                    {
                        dt.DefaultView.RowFilter = "BranchCode ='" + vBrCode + "'";
                    }
                    ddlBranch.Items.Insert(0, new ListItem("<--Select-->", "-1"));
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }

    }
}