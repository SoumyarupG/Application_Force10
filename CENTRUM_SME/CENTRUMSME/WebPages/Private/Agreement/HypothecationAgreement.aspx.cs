using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using CENTRUMCA;
using CENTRUMBA;
using CENTRUMDA;

namespace CENTRUMSME.WebPages.Private.Agreement
{
    public partial class HypothecationAgreement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadRecord();
        }
        private void LoadRecord()
        {
            try
            {
                DataSet ds = (DataSet)Session["HADt"];
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                if (ds.Tables.Count == 1)
                    dt = ds.Tables[0];
                else if (ds.Tables.Count == 2)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                }
                StringBuilder sb = new StringBuilder();
                if (dt.Rows.Count > 0)
                {
                    string word = ConvertNumbertoWords(Convert.ToInt32(dt.Rows[0]["SanctionAmt"]));

                    sb.Append(@"<table width='1002px' cellspacing='10' cellpadding='0' border='0' style='margin:0 auto;'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:18px; line-height:18px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:22px; line-height:30px; font-weight:bold; text-decoration: underline;'>AGREEMENT OF HYPOTHECATION</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:18px;'>This Agreement of Hypothecation is executed on <b>{0}</b> at <b>{1}</b> </td>", dt.Rows[0]["AgrmntDate"].ToString(), "Pune");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:18px;'>(“Agreement”)</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:18px;'><b>BETWEEN</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>M/s {0} having its registered office at {1} represented by {2} and {3} (hereinafter referred to as “Borrower” which expression shall unless repugnant to the context thereof, include its successors and assigns) on the FIRST PART</td>", dt.Rows[0]["CompanyName"].ToString(), dt.Rows[0]["ComAddress"].ToString(), dt.Rows[0]["ApplicantName"].ToString(), dt.Rows[0]["CoApplicantName"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>AND </b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>{0}</b>, a Company incorporated under the Companies Act, 1956 having its registered office at <b>{1}</b>. Hereinafter referred to as “<b>KFI</b>” (which expression, unless repugnant to the subject or context or meaning thereof, shall include its successors and assigns) on the <b>SECOND PART</b>. Borrower and KFI shall be individually referred to as “Party” and collectively as “Parties”.</td>", gblValue.CompName, CGblIdGenerator.GetBranchAddress1("0000"));
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>WHEREAS,  </b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px;line-height:16px;'><b>I.</b>	By and Master Facility Agreement dated <b>{0}</b> entered into between the Borrower and KFI (hereinafter referred to as <b>“Loan Agreement”</b>), KFI has agreed to lend and advance to the Borrower and the Borrower has agreed to borrow from KFI on the terms and conditions contained in the Loan Agreement sum not exceeding Rs. <b>{1}</b> (Rupees <b>{2}</b> Only) hereinafter referred to as <b>“the Loan”</b>;</td>", dt.Rows[0]["AgrmntDate"].ToString(), dt.Rows[0]["SanctionAmt"].ToString(), word);
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px;line-height:16px;'><b>II.</b>	One of the conditions of the Loan Agreement is that the Loan along with all interest at the agreed rate, applicable taxes, future interest, costs, charges, expenses and all other monies whatsoever payable by the Borrower shall be secured inter alia by an exclusive first charge by way of hypothecation of assets including book debts of the Borrower in the manner as provided herein;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; line-height:16px;'><b>III.</b>	KFI hereby calls upon the Borrower to execute these presents which the Borrower has agreed to do in the manner hereinafter expressed; </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; line-height:16px;'><b>NOW THEREFORE THESE PRESENTS WITNESSETH THAT:</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'><b>1.	</b>The Borrower hereby hypothecates by way of First Charge to KFI the following : </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>1.1 </b>The assets (“Hypothecated Assets”)as mentioned in Schedule I herein; and </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>1.2 </b>All the book debts, outstanding monies, receivables, claims and bills which are now due and owing  or which may at any time hereafter during the continuance of this security becomes due and owing to the Borrower in the course of its  business by any person, firm, company or body corporate or by the Government Department or office or any Municipal or Local or Public or Semi-Government Body or authority or undertaking or project whatever in the public sector, (all of which hereinafter collectively referred to <b>“the said debts”);[The highlighted portion to be kept if required]</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>2. </b>The Borrower herein undertakes to secure as a continuing security the due repayment by the Borrower to KFI at any time on demand of : </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>2.1 </b>All the monies now or at any time and from time to time hereafter due/may become due and owing by Borrower to KFI in respect of and under the above the Loan and interest thereon and all legal and other costs, charges, payments, re-imbursements and expenses relating thereto and payable hereunder and incidental to this security or for enforcement thereof (hereinafter collectively referred to as <b>“the said dues”</b>). </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>3. </b>The Borrower further agrees and undertakes the following: </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>3.1 </b>The said dues shall be payable by us to KFI at the registered office as mentioned herein above. KFI shall be entitled to demand payment of all or any of the dues at any time from the Borrower. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>3.2 </b>So long as any monies are outstanding under the Loan Agreement, the Borrower shall pay to KFI interest at the rate as may be communicated by KFI from time to time and this Agreement shall be construed as if such revised rate of interest was mentioned herein and thereby secured; </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>3.3 </b>KFI shall have an absolute discretion for granting or continuance of the credit facilities under the Loan Agreement and determining the amounts to be advanced and/or allowed to be outstanding from time to time within the limit and in the account(s) to be opened by KFI in respect of the said credit facilities and be at liberty to close the account(s) and refuse to allow further drawing or advances thereon at any time any previous notice to the Borrower. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>4. </b>In further pursuance of the Loan Agreement  and for the consideration aforesaid the Borrower hereby agrees, declares and covenants as follows: </td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>4.1 </b>The Borrower shall at its expense keep the Hypothecated Assets in marketable and good condition and obtain insurance of the same as against any loss or damage by theft, fire, lightning, earthquake, explosion, riot, strike, civil commotion, storm, flood and such other risks as KFI shall from time to time require, with an insurance company(s). The Borrower shall deliver to KFI the relevant policies of insurance and maintain such insurance throughout the continuance of the security of these presents and deliver to KFI the renewal receipts thereof. The Borrower shall duly pay all premiums in respect of such insurance. In default of the same, KFI may (but shall not be bound to) keep in good condition and render marketable the Hypothecated Assets and take out/ renew such insurance as and when required at the cost to be borne by the Borrower . Any such premium(s) paid by KFI towards the insurance and any costs, charges and expenses incurred by KFI shall forthwith on receipt of notice of demand by KFI together the interest thereon at the applicable rates from the date of payment and until such reimbursement by the Borrower, the same shall be debited to the Borrower’s loan account and be a charge on Hypothecated Assets.  </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>4.2 </b>The Authorized officers/agent of KFI shall, without any notice and at the risk and expense of the Borrower, be entitled at all times to enter any place where the Hypothecated Assets may be kept/stored and inspect value insurance, superintend the disposal of and take particulars of all parts of the said Hypothecated Assets and check any statement/accounts/reports and information in respect of Hypothecated Assets.  </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>4.3 </b>In the event of any breach or default  by the Borrower in performance of its obligations, hereunder or any of the terms, covenants, obligations and conditions as stipulated under the Loan Agreement or the related security documents or deeds executed of that may hereinafter be executed by the Borrower in favor of KFI or in the event of the Borrower failing to pay either the Loan or interest or any installment or in the event of the charge or any security created in favor of KFI having become enforceable for any reason whatsoever, KFI or its authorized officers/agents shall, in case such breach or default is not remedied by the Borrower to the satisfaction of KFI within a period of 10 days from the date of such intimation by KFI regarding breach or default of obligations by the Borrower in writing and at the risk of the Borrower and if necessary as Attorney for and in the name of Borrower be entitled to take charge and/or possession/ seize/recover/receive and remove the Hypothecated Assets and/or sell by public auction or by private contract such Hypothecated Assets without prejudice to other remedies as available to KFI herein. </td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>4.4 </b>Notwithstanding with any pending suit or other legal proceeding, the Borrower undertakes to give immediate possession to the authorized officers/agent of KFI on demand of the said Hypothecated Assets and to transfer, and to deliver to KFI all related bills, contracts, securities and documents.  </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>4.5 </b>The Borrower shall, whenever required by KFI, give full particulars to KFI of all assets of the Borrower and of the Hypothecated Assets and shall furnish and verify all statements, reports, returns, certificates and information from time to time and as required by KFI and execute all necessary documents to give effect to.   </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp; </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp; </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>4.6 </b>KFI at any time after the security hereby constituted has become enforceable and whether or not KFI shall then have entered into or taken possession of and in addition to the powers herein before conferred upon KFI after such entry into or taking possession of may have Receiver(s) appointed in respect of the Hypothecated Assets or any part thereof. The following provisions shall also apply to the Receiver.   </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>5. </b>All the Hypothecated Assets and all the sale realizations and insurance proceeds thereof and all documents under the security shall always be kept distinguished and held as the exclusive property of KFI specifically appropriated to the security and be dealt with only under the directions of KFI and the Borrower shall not create any charge, mortgage, lien or any other encumbrances upon or over the same, or any part thereof. The Borrower agrees not to sale any of the said Hypothecated Assets except to the extent as permitted by KFI. The Borrower shall on any and every such sale pay to KFI, if so required, the net proceeds of the sale or disposal in satisfaction, so far as the same shall extend, of the monies, due and payable by the Borrower.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>6. </b>The security shall be a continuing security for repayment of the Loan together with all the interest, further interest, liquidated damages and repayment or payment of all other monies due to KFI under the Loan Agreement and these presents, and shall not affect, impair or discharge the liability of the Borrower by factors such as but not limited to winding up (voluntary or otherwise) or by any merger or amalgamation, reconstruction or otherwise of the management. </td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>7. </b>All the disputes or differences arising between the parties hereto as to the interpretation of this Agreement or any covenants or conditions thereof or as to the rights, duties or liabilities of any Party hereunder or as to any act, performance or non-performance of any act, deed or thing as agreed under this agreement or matter or thing arising out of or relating to or under this Agreement (even though the Agreement may have been terminated), the same shall be referred to the arbitration to be governed by the Arbitration and Conciliation Act, 1996. The Arbitral Tribunal shall consist of a sole arbitrator to be appointed by KFI. The language of arbitration shall be English and the seat of arbitration shall be Pune. The decision of the said Arbitrator shall be final, conclusive and binding upon the Parties hereto. All costs of Arbitration, including the Arbitrator’s fees, advocate’s fees, traveling costs and other miscellaneous expenses shall be borne equally by the Parties hereto. Any dispute relating to this Agreement shall be resolved, decided and/or settled on the   basis of the laws of India and the Courts at Pune to the exclusion of the other Courts shall have exclusive jurisdiction to entertain any dispute.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>8. </b>The provisions herein contained shall be read in conjunction with the provisions of the Loan Agreement as amended from time to time and to the extent of any inconsistency or repugnancy the latter shall prevail to all intents and purposes. </td>");
                    sb.Append(@"</tr>");


                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp; </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>IN WITNESS WHEREOF, the Borrower has caused its common seal to be affixed hereto on <b>{0}</b>  at Pune. </b> </td>", dt.Rows[0]["AgrmntDate"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>Ms. {0} </b> </td>", dt.Rows[0]["CompanyName"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp; </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Authorized Signatory </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp; </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp; </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>For {0}</b> </td>", gblValue.CompName);
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>___________________________ </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp; </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Authorized Signatory </td>");
                    sb.Append(@"</tr>");


                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:20px; line-height:30px; font-weight:bold;'>SCHEDULE I</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:20px; line-height:30px; font-weight:bold;'>(Particulars of Hypothecated Assets)</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:20px; line-height:30px; font-weight:bold;'>( Details such as name, type, quantity, place where located/stored of machinery or other assets to be mentioned in the schedule )</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");

                    if (dt1.Rows.Count > 0)
                    {
                        sb.Append(@"<tr>");
                        sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'>");
                        sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='1'>");
                        sb.Append(@"<tr>");
                        sb.Append(@"<td width='10%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Sr. No</b></td>");
                        sb.Append(@"<td width='20%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Description of Machinery</b></td>");
                        sb.Append(@"<td width='16%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Name of Supplier</b></td>");
                        sb.Append(@"<td width='16%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Make</b></td>");
                        sb.Append(@"<td width='16%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Model</b></td>");
                        sb.Append(@"<td width='16%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Amount (Rs)</b></td>");
                        sb.Append(@"</tr>");

                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='15%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt1.Rows[i]["SlNo"].ToString());
                            sb.AppendFormat(@"<td width='15%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt1.Rows[i]["MachinDesc"].ToString());
                            sb.AppendFormat(@"<td width='16%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt1.Rows[i]["SupplierName"].ToString());
                            sb.AppendFormat(@"<td width='16%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt1.Rows[i]["Make"].ToString());
                            sb.AppendFormat(@"<td width='16%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt1.Rows[i]["Model"].ToString());
                            sb.AppendFormat(@"<td width='16%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt1.Rows[i]["Amount"].ToString());
                            sb.Append(@"</tr>");
                        }
                        sb.Append(@"</table>");
                        sb.Append(@"</td>");
                        sb.Append(@"</tr>");

                    }
                    else
                    {
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:50px;'>&nbsp; </td>");
                        sb.Append(@"</tr>");
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:50px;'>&nbsp; </td>");
                        sb.Append(@"</tr>");
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:50px;'>&nbsp; </td>");
                        sb.Append(@"</tr>");
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:50px;'>&nbsp; </td>");
                        sb.Append(@"</tr>");
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:50px;'>&nbsp; </td>");
                        sb.Append(@"</tr>");
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:50px;'>&nbsp; </td>");
                        sb.Append(@"</tr>");
                    }

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:50px;'>&nbsp; </td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"</table>");
                }
                Literal1.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ConvertNumbertoWords(Int32 number)
        {
            if (number == 0) return "ZERO";
            if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 10000000) > 0)
            {
                words += ConvertNumbertoWords(number / 10000000) + " CRORE ";
                number %= 10000000;
            }
            if ((number / 100000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " LACS ";
                number %= 100000;
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            //if ((number / 10) > 0)  
            //{  
            // words += ConvertNumbertoWords(number / 10) + " RUPEES ";  
            // number %= 10;  
            //}  
            if (number > 0)
            {
                if (words != "") words += "AND ";
                var unitsMap = new[]   
                {  
                    "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"  
                };
                var tensMap = new[]   
                {  
                    "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"  
                };
                if (number < 20) words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "mywindow", "printform();", true);
        }
    }
}