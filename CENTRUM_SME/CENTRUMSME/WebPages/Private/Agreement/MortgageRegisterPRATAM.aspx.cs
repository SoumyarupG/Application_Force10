using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using CENTRUMCA;


namespace CENTRUMSME.WebPages.Private.Agreement
{
    public partial class MortgageRegisterPRATAM : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void LoadRecord()
        {
            try
            {
                DataSet ds = (DataSet)Session["MortgRegis"];
                DataTable dt = new DataTable ();
                DataTable dt1 = new DataTable();
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                StringBuilder sb = new StringBuilder();
                if (dt.Rows.Count > 0)
                {
                    string word = ConvertNumbertoWords(Convert.ToInt32(dt.Rows[0]["SanctionAmt"]));
                    
                    sb.Append(@"<table width='1002px' cellspacing='10' cellpadding='0' border='0' style='margin:0 auto;'>");
                    //sb.Append(@"<tr>");
                    //sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:12px; line-height:14px;'>");
                    //sb.Append(@"<table width='100%' cellspacing='0' cellpadding='0' border='0'>");
                    //sb.Append(@"<tr>");
                    //sb.Append(@"<td width='10%' align='left' rowspan='3' style='font-family:Garamond; font-size:16px; line-height:16px;'><img src='../../../images/Centrum_Logo.png' height='50px' width='200px' alt='Company Logo' /></td>");
                    //sb.AppendFormat(@"<td width='80%' align='center' style='font-family:Garamond; font-size:30px; line-height:30px;'><b>{0}</b></td>",gblValue.CompName.ToString());
                    //sb.Append(@"<td width='10%' align='left' rowspan='3' style='font-family:Garamond; font-size:12px; line-height:16px;'>&nbsp;</td>");
                    //sb.Append(@"</tr>");
                    //sb.Append(@"<tr>");
                    //sb.AppendFormat(@"<td width='80%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", gblValue.Address1);
                    //sb.Append(@"</tr>");
                    //sb.Append(@"<tr>");
                    //sb.AppendFormat(@"<td width='80%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", gblValue.Address2 + " CIN No- " + gblValue.CIN.ToString());
                    //sb.Append(@"</tr>");
                    //sb.Append(@"</table>");
                    //sb.Append(@"</td>");
                    //sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:128px; line-height:128px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:22px; line-height:30px; font-weight:bold;'>DEED OF MORTGAGE</td>");
                    sb.Append(@"</tr>");
                    //sb.Append(@"<tr>");
                    //sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:22px; line-height:30px; font-weight:bold;'>(Without possession)</td>");
                    //sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");
                    string AmtInWord = ConvertNumbertoWords(Convert.ToInt32(dt.Rows[0]["SanctionAmt"]));
                   
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>THIS INDENTURE OF MORTGAGE  TO SECURE LOAN AMOUNT is made and executed at _________________________ on this ____________________________________________ by and between </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>BETWEEN</b></td>");
                    sb.Append(@"</tr>");
                    int x = 0;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        x = x + 1;
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'> <b> {0})  {1},  PAN No - {2} ,Age - {3} Years, Occupation - {4}  </td>", x.ToString(), dt1.Rows[i]["CustName"].ToString(), dt1.Rows[i]["PANNO"].ToString(), dt1.Rows[i]["Age"].ToString(), dt1.Rows[i]["Occupation"].ToString());
                        sb.Append(@"</tr>");
                    }
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>All Residence at {0} , District  - {1} - {2},State - {3} </td>",  dt.Rows[0]["CustAddress1"].ToString(), dt.Rows[0]["CustDist"].ToString(), dt.Rows[0]["CustPIN"].ToString(), dt.Rows[0]["CustState"].ToString());
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Hereafter in called as “THE BORROWER / MORTGAGOR” </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>AND</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Centrum SME Loan, a company registered under the Companies Act, 1956, having its registered office at Thane (West) Maharashtra, and Head office at 4th Floor Shastri Heritage, Near Tata Motors Main Gate, Chinchwad Pune-411033. Through its Branch Office  <b>{0}</b>  (hereinafter referred to as the 'Lender', which expression shall, unless repugnant to the subject or context thereof, be deemed to include its successors and assigns) of the SECOND PART;</td>", dt.Rows[0]["CustBranch"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Mortgagors and Lender are hereinafter individually referred to as 'Party' and collectively as 'Parties'. <b>WHEREAS:</b></td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>{0}</td>", dt.Rows[0]["Remarks"].ToString());
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>(More particularly described in the Schedule hereunder written and hereinafter referred to as THE MORTGAGED PROPERTY)</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>1.	WHEREAS, the Mortgagors have applied  for Mortgage Loan to the tune of Rs.<b>{0}</b>/- for expansion of business to the Party of the Second Part and the Party of the Second Part has agreed and sanctioned Rs.<b> {1}/- (Rupees {2} Only)</b> on the terms and conditions mentioned  in the Sanction Letter bearing No. <b>{3}</b> Date <b>{4}</b>  given by the Party of the Second Part  to the Party of the First Part  and the said Loan amount is to be repaid by the Party of the First  Part/ Mortgagor in Equated Monthly Installment of Rs. <b>{5}</b>/-  the said Loan is to be repaid in <b>{6}</b> monthly installment. The said Loan is to carry interest at the rate of 27% per anum with yearly rest. The said entire property alongwith the said structure is to be mortgaged infavor of Company as and by way of security. The Mortgagor agreed to the said terms and conditions stipulated in the sanction Letter Dt. <b>{7}</b> and accepted the said Terms and Conditions and has signed the same expressing the acceptance of the same.</td>", dt.Rows[0]["SanctionAmt"].ToString(), dt.Rows[0]["SanctionAmt"].ToString(), AmtInWord, dt.Rows[0]["SanctionID"].ToString(), dt.Rows[0]["FinalApprovedDt"].ToString(), dt.Rows[0]["EMI"].ToString(), dt.Rows[0]["NoOfInstallment"].ToString(), dt.Rows[0]["FinalApprovedDt"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>2.	AND WHEREAS in pursuance of the agreement and acceptances of Mortgagor the Mortgagee has called upon the Mortgagor to execute these presents which the Mortgagors have now agreed to do as Principle Borrower to secure the due performance of its agreement with the Mortgagee to repay the said Loan amount together with interest cost expenses charges etc. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;text-decoration: underline;'><b>NOW THIS DEED WITNESSETH AS UNDER</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>3.	That pursuant to the said agreement and on consideration of the sum of Rs. <b>{0}</b>/- to be lent and advance by the mortgagee to the mortgagor on execution of these presents (receipt whereof the mortgagor both hereby admit ). The Mortgagors hereby covenant with the mortgagee that the Mortgagor will pay to the mortgagee the said sum or Rs. <b>{1}</b>/-within <b>{2}</b> Month as agreed under loan agreement ( hereinafter referred to as the 'due date') with interest thereon at the rate of 27 % p.a., in the meanwhile and until repayment  of the said sum in full, the mortgagor shall pay the <b>{3}</b>/- per month. The first installment on E.M.I. to be paid on 10th day of each month, first of such EMI will start from the next month of last installment disbursed and each subsequent installment on the 10th day of each succeeding month until the said principle amount along with interest is repaid in full. And the Mortgagor further covenants with the mortgagee that, if in the event of the mortgagor failing to pay any monthly installment of principal and interest, he will be liable to interest on the said installment as and by way of compound interest, without prejudice to the right of the mortgagee to take any action on default as hereinafter provide. And it is agreed and declared that in the event of the mortgagor committing default in payment of any two installments of principal and interest on the due date or committing breach of any other terns of deed, the whole amount of principal then due with interest thereon will at the option of the mortgagor became payable forthwith as if the said due date has expired. </td>", dt.Rows[0]["SanctionAmt"].ToString(), dt.Rows[0]["SanctionAmt"].ToString(), dt.Rows[0]["NoOfInstallment"].ToString(), dt.Rows[0]["EMI"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>4.	And  this further witnessed that in consideration  aforesaid, the mortgagors do and each of them doth hereby grant, transfer, assign and assure unto the Mortgagee by way of mortgage all that said immovable property consisting of property land with building House property being constructed and renovated having undivided share in the admeasuring about Schedule of Property ( <b>{0}</b>) described in schedule hereunder written and all estate right title interest of the Mortgagors therein as a security for repayment of the said sum with interest and all other moneys due and payable hereunder with conditions that on the mortgagor repaying the said principal sum of Rs.<b>{1}</b>/- with all interest and other moneys due to the mortgagee  (hereinafter referred to as the mortgage money) the mortgagee will redeem the said land and premises from the mortgage security and shall if so required by the mortgagor execute a deed of release but at the costs of mortgagor.</td>", dt.Rows[0]["Remarks"].ToString(), dt.Rows[0]["SanctionAmt"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>5.	And it is further agreed and declared by the mortgagor that in the event of the mortgagor failing to pay the said principal sum with all the interest and others moneys when the same shall become due and payable under these presents, the mortgagee will become entitle to have the said property with easamentary right sold through any competent Court and to realize and receive and the said mortgage amount out of the net sale proceed of the said land and premises.And it is further agreed and declared by the mortgagor, that he shall also be liable to pay and shall pay all costs, charges and expenses that the Mortgagee will incur for the protection of the Mortgagee security and or for the realization of the mortgage amount and the same shall be deemed to form part of the mortgage amount and the security thereof as aforesaid.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>6.	That, the payments made by the borrower /Mortgagor shall first be credited towards defaulted outstanding interest. Regular interest and other outstanding charges such as outstanding insurance charges etc, and thereafter, the balance shall be applied towards the satisfaction of the principal amount.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>7.	The Mortgagor further agrees that in case the amount of loan with interest thereon together with incidental charges etc., not paid within the time and manner aforesaid, and as per the agreement executed separately as stated above, it shall be lawful for the Mortgagee to enforce it's right against the Mortgaged property and cause it to be sold and to appropriate the proceeds thereof towards the satisfaction of Mortgaged Debt.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>8.	And it is further agreed that during the pendency of the security hereby created and until repayment of the mortgage amount the mortgagor  will get insured and keep insured the said property, against loss and damages due to fire or any other accident in the sum of at least  Rs.<b>{0}</b>/- only with some insurance Company of repute and pay all premium on the insurance Policy as and when it becomes due and payable in respect thereof to such Company and shall handover the Policy  to the Mortgagee during endorse in his name as assignee and in the event of the Mortgagor failing to do so or pay premium , the Mortgagee will be entitled to insure the said flat ad structures and /or to pay premium thereon and the amount paid by the Mortgagee in respect  thereof will be deemed  to form part of the Mortgage amount.</td>", dt.Rows[0]["SanctionAmt"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>9.	And it is further agreed that in the event of the said land and premises being destroyed or damaged by fire or any accident as aforesaid, the mortgagee will be entitled to receive insurance claim under such policy to the exclusion of the Mortgagors and to appropriate the same first towards all arrears of interest and then the principle amount or any part thereof as may be sufficient to pay the mortgage amount due and if any surplus remains the same will become payable to mortgagor.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>10.	That, the Mortgagor agrees that the sum of money awarded as compensation for any compulsory acquisition of any portion of the mortgaged property by any Government, Municipal Corporation, Railway or any local authority shall be receivable by the Mortgagee company directly on behalf of the Mortgagor.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>11.	The Mortgagors will permit the Mortgagee with its officers, representatives, servants agents from time to time and at all reasonable time to enter upon the Mortgage property or any part of thereof and to inspect the same and if upon such inspections it appears to the Mortgagee that the Mortgaged premises or any part thereof require repairs or replacement the Mortgagee may give notice to the Mortgagors calling upon them to repair the same and in the event of the failure on the part of the Mortgagors to do so within a reasonable time , it shall be lawful for, but not obligatory upon, the Mortgagee to do the same and all expenses that may be incurred by the Mortgagee shall be forthwith repaid by the Mortgagors and until such repayment, will be a charge upon the Mortgaged premises jointly with the said principal sum and interest hereby secured, as if they had formed part thereof.</td>");
                    sb.Append(@"</tr>");
                    
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:224px; line-height:224px;'><b>&nbsp;</b> </td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>12.	The Mortgagor shall pay the whole of the stamp duty and registration charges payable in respect of these presents and shall also pay the costs charged and expenses between Advocate client anywise incurred or paid by the Mortgagee of the incidental to or in connection with these presents or this security and incurred as well for the assertion or Defense or the rights of the Mortgagor as for the protection and security of Mortgaged property and for the demand realization and recovery of the said principal sum, interest and other moneys payable by the Mortgagors to the Mortgagee and until such repayment the same shall a charged upon the Mortgage premises. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>13.	The Mortgagors shall not create, alienate or transfer right of the said property by way of any kind of deed, gift, Mortgage or sale and not create any third party interest in the said property till the realization of loan taken from Mortgagee. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>14.	This document has been stamped with a stamp duty. In the event of any additional stamp duty being required to be paid or levied, then the Mortgagors shall bear and pay the same as also any penalty that may be levied. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>15.	DISPUTE RESOLUTION – ARBITRATION</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>In the event of any dispute or differences arising directly or indirectly out of this Agreement or in relation to any other documents executed in connection with the Facility or otherwise, the Parties undertake to use all reasonable endeavors to resolve such disputes amicably. If disputes and differences cannot be settled amicably, then all disputes and differences arising between the Parties hereto in connection with this Agreement or the interpretation hereof or anything done or omitted to be done pursuant hereto or the performance or non-performance of this Agreement shall be referred to the Arbitration.' Such disputes, differences and/or claims arising out of these presents or as to the construction, meaning or effect hereof or as to the rights and liabilities of the parties hereunder shall be settled by Arbitration to be held at Pune in accordance with the provisions of the Arbitration and Conciliation Act, 1996 or any statutory amendments thereof or any statute enacted for replacement there for and shall be referred to a single arbitrator to be appointed by the Lender (hereinafter referred to as 'Arbitrator') and the & Arbitrator's award shall be final and binding on both the Parties hereto. The arbitration shall be held in Mumbai and the expenses of the arbitration shall be borne in such manner as the Arbitrator may determine. In the event of death, refusal, neglect, inability or incapability of the person so appointed to act as an Arbitrator, the Lender shall appoint a new Arbitrator. The award including interim award/s of the arbitration shall be final, conclusive and binding on all parties concerned. The Arbitrator shall not give any reason for his award including interim award/so The Arbitrator may lay down from time to time the procedure to be followed by him in conducting arbitration proceedings and shall conduct the arbitration proceedings in such manner as he considers appropriate. The Arbitration shall be governed by the Arbitration & Conciliation Act, 1996 or such other law relating to Arbitration as may be in force in India at the relevant time.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>THE SCHEDULE OF PROPERTY ABOVE REFERRED TO</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;text-decoration: underline;'><b>SCHEDULE – I</b></td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>All that piece and parcel of land <b>{0}</b> whichis bounded as under:</td>", dt.Rows[0]["Remarks"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>On or towards East		: {0} </b></td>", dt.Rows[0]["EastBoundary"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>On or towards West		: {0} </b></td>", dt.Rows[0]["WestBoundary"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>On or towards South		: {0} </b></td>", dt.Rows[0]["SouthBoundary"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>On or towards North		: {0} </b></td>", dt.Rows[0]["NorthBoundary"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>IN WITNESS WHEREOF</b>, I/We have set our respective hand and seal and signed of this Deed of Mortgage at <b>{0}</b> on the day and date first mentioned herein above. </td>", dt.Rows[0]["CustBranch"].ToString());
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");

                    x = 0;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        x = x + 1;
                        sb.Append(@"<tr>");
                        sb.Append(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                        sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                        sb.Append(@"<tr>");
                        sb.Append(@"<td width='25%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                        sb.Append(@"<table width='50%' height='100px' cellspacing='0' cellpadding='5' border='0'>");
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>&nbsp;</b></td>");
                        sb.Append(@"</tr>");
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '> photograph</td>");
                        sb.Append(@"</tr>");
                        sb.Append(@"</table>");
                        sb.Append(@"</td>");
                        sb.Append(@"<td width='25%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                        sb.Append(@"<table width='100%' height='100px' cellspacing='0' cellpadding='5'>");
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>{0}</b></td>", dt1.Rows[i]["CustName"].ToString());
                        sb.Append(@"</tr>");
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '> <b>Aadhar No. {0}</b></td>", dt1.Rows[i]["AadharNo"].ToString());
                        sb.Append(@"</tr>");
                        sb.Append(@"</table>");
                        sb.Append(@"</td>");
                        sb.Append(@"<td width='25%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                        sb.Append(@"<table width='50%' height='100px' cellspacing='0' cellpadding='5' border='0'>");
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>&nbsp;</b></td>");
                        sb.Append(@"</tr>");
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>L. H.Thumb.</td>");
                        sb.Append(@"</tr>");
                        sb.Append(@"</table>");
                        sb.Append(@"</td>");
                        sb.Append(@"<td width='25%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                        sb.Append(@"<table width='50%' height='100px' cellspacing='0' cellpadding='5' border='0'>");
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>&nbsp;</b></td>");
                        sb.Append(@"</tr>");
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>Signature</td>");
                        sb.Append(@"</tr>");
                        sb.Append(@"</table>");
                        sb.Append(@"</td>");
                        sb.Append(@"</tr>");
                        sb.Append(@"</table>");
                        sb.Append(@"</td>");
                        sb.Append(@"</tr>");
                    }

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");



                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='25%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='50%' height='100px' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '> photograph</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"<td width='25%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='100%' height='100px' cellspacing='0' cellpadding='5'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '> <b>WITNESS No. 1 {0}</b></td>", "");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '> <b>Name</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '> <b>Address </b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"<td width='25%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='50%' height='100px' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>L. H.Thumb.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"<td width='25%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='50%' height='100px' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>Signature</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='25%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='50%' height='100px' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '> photograph</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"<td width='25%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='100%' height='100px' cellspacing='0' cellpadding='5'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '> <b>WITNESS No. 2 {0}</b></td>", "");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '> <b>Name</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '> <b>Address </b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"<td width='25%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='50%' height='100px' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>L. H.Thumb.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"<td width='25%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='50%' height='100px' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>Signature</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"</table>");
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

        //public override void VerifyRenderingInServerForm(Control control)
        //{

        //}
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "mywindow", "printform();", true);
        }
    }
}