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
    public partial class MortgageRegisterMAS : System.Web.UI.Page
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
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:12px; line-height:14px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='0' border='0'>");
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='15%' align='left' rowspan='3' style='font-family:Garamond; font-size:16px; line-height:16px;'><img src='../../../images/Centrum_Logo.png' height='50px' width='200px' alt='Company Logo' /></td>");
                    sb.AppendFormat(@"<td width='70%' align='center' style='font-family:Garamond; font-size:30px; line-height:30px;'><b>{0}</b></td>", "Centrum Micro Credit");
                    sb.Append(@"<td width='15%' align='left' rowspan='3' style='font-family:Garamond; font-size:12px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='70%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", gblValue.Address1);
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='70%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", gblValue.Address2 + " CIN No-" + gblValue.CIN.ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:18px; line-height:18px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:22px; line-height:30px; font-weight:bold;'>REGISTERED  OF MORTGAGE</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:22px; line-height:30px; font-weight:bold;'>(Without possession)</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");
                    string AmtInWord = ConvertNumbertoWords(Convert.ToInt32(dt.Rows[0]["SanctionAmt"]));
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Indenture of Mortgage for Rupee Term Loan Facility Aggregating to Rs.{0}/- ( {1} Rupees Only ). </td>", dt.Rows[0]["SanctionAmt"].ToString(), AmtInWord);
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>THIS INDENTURE OF MORTGAGE ('Indenture') is made and executed at _________________________ on this ____________________________________________ by and between </td>");
                    sb.Append(@"</tr>");
                    int x = 0;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        x = x + 1;
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'> <b> {0})  {1},  Aadhar No. {2}   </td>", x.ToString(), dt1.Rows[i]["CustName"].ToString(), dt1.Rows[i]["AadharNo"].ToString());
                        sb.Append(@"</tr>");
                    }
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Both having residential address at {0} , District  - {1} - {2},State - {3} , Maharashtra.(hereinafter referred to as the 'Mortgagors', which expression shall unless it be repugnant to the context or meaning thereof, mean and include its successors and permitted assigns) of the FIRST PART;</td>",  dt.Rows[0]["CustAddress1"].ToString(), dt.Rows[0]["CustDist"].ToString(), dt.Rows[0]["CustPIN"].ToString(), dt.Rows[0]["CustState"].ToString());
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>AND</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>MAS FINANCIAL SERVICES LIMITED</b>, a company registered under the Companies Act, 1956, having its registered & corporate office at <b>6th Ground, B/h. Patang Hotel, Ashram Road, Ahmedabad-9. Gujarat </b>(hereinafter referred to as the 'Lende', which expression shall, unless repugnant to the subject or context thereof, be deemed to include its successors and assigns) of the SECOND PART;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Mortgagors and Lender are hereinafter individually referred to as 'Party' and collectively as 'Parties'. <b>WHEREAS:</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>A.</td>");
                    sb.Append(@"</tr>");

                    x = 0;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        x = x + 1;
                        if (dt1.Rows[i]["CustType"].ToString() == "A")
                        {
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>{0}) {1}('Borrower')</td>", x.ToString(), dt1.Rows[i]["CustName"].ToString());
                            sb.Append(@"</tr>");
                        }
                        else
                        {
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>{0}) {1}</td>", x.ToString(), dt1.Rows[i]["CustName"].ToString());
                            sb.Append(@"</tr>");
                        }
                    }
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>has approached Lender and requested Lender to provide a facility in the nature of a term loan facility (hereinafter referred to as the 'Loan') for the purpose as mentioned in the Loan Agreement dated <b>{0}</b> ('Loan Agreement')</td>", dt.Rows[0]["AgrmntDate"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>B. Upon request of the Borrowers, the Lender have agreed to grant to the Borrowers, term loan facility for an aggregate amount of <b>{0}</b>/- (<b>{1}</b>) Upon the terms and conditions as more particularly set out in the Loan Agreement entered into between the Borrowers and the Lender.</td>", dt.Rows[0]["SanctionAmt"].ToString(), AmtInWord);
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>C. Mortgagors are the legal and absolute owner of land located in together with all the plant and machinery erected, installed and embedded on to it and all other fixtures and buildings, superstructures thereon (hereinafter referred to as 'Mortgaged Property') more particularly described in this Schedule I hereunder.Description of mortgaged property (Schedule I ) –</td>");
                    sb.Append(@"</tr>");


                    // Enter Details




                   
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>D. In terms of the Loan Agreement, the Mortgagors has agreed that Loan together with interest, Additional Interest, liquidated damages, commitment charges, premia / prepayment charge on prepayment or on redemption, costs, expenses and other monies whatsoever stipulated in the Loan Agreement shall be inter-alia, secured by a first exclusive charge by way of mortgage in a form and manner satisfactory to the Lenders of the Mortgaged Property along with the Leasehold Rights, if any, of the Mortgagors in the Mortgaged Property;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>E. On the above premise, the Mortgagors have agreed to mortgage the Mortgaged Property, by way of exclusive charge, together with its absolute interest! title in the Mortgaged Property to secure the Loan advanced by the Lenders. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'><b>NOWTHEREFORE, THIS INDENTURE WITNESSES AS UNDER:</b> </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>1. That to facilitate grant of Loan to the Mortgagors by the Lenders, and to secure all the sums payable by the Mortgagors in relation thereto, the Mortgagors have agreed to create by a first exclusive charge by way of mortgage in a form and manner satisfactory to the Lenders of the Mortgaged Property in order to fully secure the Loan together with interest, Additional Interest, liquidated damages, commitment charges, premium/prepayment charge on prepayment or on redemption, costs, expenses and other monies whatsoever stipulated in the Loan Agreement.</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>2. That the creation of security on the Mortgaged Property under this Indenture is without prejudice to any other security, if any, which may have been furnished by the Mortgagors or any other Person to the Lender in connection with the Loan advanced by the Lenders pursuant to the Transaction Documents o as defined in the Loan Agreement. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>3. That the Mortgagors hereby to whomsoever it is applicable, represent and warrant to the Lender as follows: </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>(a) The Mortgagors is a company incorporated under the Companies Act, 1956, and validly existing under the Indian laws and has all the requisite legal power and authority and has been duly authorized by requisite corporate actions to execute this Indenture and carry out the terms, conditions and obligations hereof. The person(s) executing this Indenture on behalf of the Obligors have been duly authorized to do so, and when executed, constitutes valid and binding obligations, enforceable against the Obligors in accordance with the terms hereof. There is no prohibition, whether under law or otherwise, any order, or any suits pending before any court, or tribunal, which would adversely affect the ability of the Obligors to meet and carry out their obligations under these presents. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>(b) The Mortgagors have good right, full power and absolute authority to create mortgage on the Mortgaged Property, Mortgagors has clear and marketable title to the Mortgaged Property and that the Mortgaged Property is contiguous and is free from all sorts of Encumbrances including prior sale, agreement, lispendens, mortgage, attachment etc., and if it is proved otherwise, the Mortgagors, to whomsoever it is applicable shall be fully responsible and liable to indemnify and keep indemnified the Lender under all circumstances against any monetary loss, harm, injury suffered or caused to be suffered by the Lender on account of legal defects in the title documents of the Mortgagors in respect of their respective interests/rights in the Mortgaged Property. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>(c) No notice has been served on either the Mortgagors by any governmental authority or any other person or entity, which might impair, prevent or otherwise interfere with the Mortgagors's respective rights and interest in the Said Properties.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>(d) The creation of the present security on the Mortgaged Property is and shall at all times be in compliance with all the provisions of the applicable laws and the Mortgagors have taken and shall take all necessary measures to ensure such compliance and there is no circumstance in existence or which is likely to come into existence, pursuant to which the creation of the present security on the Mortgaged Property would amount to violation or non-compliance of the applicable law. </td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>(e) The Mortgaged Property comprising the security is free from all encumbrances or charges (statutory or otherwise), claims and demands and that the same or any of them or any part thereof are/is not subject to any lien / lispendens, attachment or any other process issued by any court or authority and that the Mortgaged Property is in the exclusive, uninterrupted and undisturbed possession and enjoyment of the Mortgagors and no adverse claim has been made against any of the Mortgagors in respect of the Mortgaged Property or any of it or any part thereof and the same is not affected by any notice of acquisition or requisition, and that no proceedings are pending or initiated against Mortgagors under the Income Tax Act, 1961, or under any other law in force in India for the time being and that no notice has been received or served on any of the Mortgagors under the Income Tax Act, 1961 and/or under any other law or issued or initiated against the Mortgaged Property. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>(f) That this Indenture shall be duly registered with the concerned Registrar / Sub-registrar of Assurances and all necessary and appropriate recordings and filings shall be made with the relevant authorities, including but not limited to filing of Form- CHG 1 in respect of creation of mortgage on the Mortgaged Property with the concerned Registrar of Companies within seven (7) Business Days of execution of this Indenture.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>(g) the Mortgagors shall furnish the no objection certificate with respect to creation of security/mortgage on the Mortgaged Property as required under the provisions of section 281 of the Income Tax, Act, 1961 in o such manner as stipulated under the Loan Agreement. </td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>(g1) the Mortgagors have read and understood all the provisions of the Transaction Documents and agrees to comply with or abide by the provisions of the Transaction Document to the extent applicable to the Mortgagors.</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>(h) In any proceedings taken against the Mortgagors in relation to any of the matters stipulated in this Indenture, the Mortgagors shall not be entitled to claim for the Mortgaged Property, immunity from suit, execution, attachment or other legal process, and the Mortgagors shall not make any such claim for immunity and hereby irrevocably waives its right, if any, to make any such claim for immunity;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>(i)There are no attachments, executions, proceedings in relation to a compromise or arrangement with the creditors/debtors, appointment of any official or provisional liquidator, receiverships, or voluntary or involuntary liquidation or winding-up proceedings contemplated or filed by the Mortgagors or pending against the Mortgagors in relation to the Mortgaged Property advanced as security pursuant to this Indenture.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>4. That the Mortgagors shall at all times during the continuance of the security hereby created, pay all rents, rates, cesses, taxes, revenues and assessments, present as well as future and all dues, duties and outgoings whatsoever payable in respect of the Mortgaged Property.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>5. The Mortgagors shall not raise any challenge, claim, question or dispute, in any manner whatsoever, with respect to the creation and enforcement of the present Security in terms of this Indenture.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>6. The Mortgagors shall at all times during the continuance of the present security on the Mortgaged Property, at their own costs, whenever called upon by the Lender, satisfy the Lender that the Mortgagor's rights, title and interest, as applicable, to the Mortgaged Property is/are clear and marketable and without reasonable doubts.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>7. The Mortgagors shall permit the Lenders' representatives, agents and officers to enter into and upon the Mortgaged Property from time to time and to inspect the same. Any such representatives, agents or officers of the Lender shall have free access, to any part of the Mortgaged Property, with prior notice to the Mortgagors, and to its records, registers and accounts and to all schedules, costs, estimates, plans and specifications relating to the Mortgaged Property and shall receive full co-operation and assistance from the officers and employees of the Mortgagors. The cost of inspection, including traveling and all other expenses shall be payable by the Mortgagors to the Lender in this behalf.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>8. The Mortgagors shall promptly inform the Lender of any occurrence or likely occurrence of any event of which it becomes aware which might adversely affect the present security interest created on the Mortgaged Property in favor of the Lender including but not limited to any material litigation, arbitration or other proceedings which affects the rights of the Mortgagors in relation to the Mortgaged Property offered as security by the Mortgagors and any matters related thereto and in respect of any damage to the Mortgaged Property.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>9. That on the occurrence of any 'Event of Default' as stated and howsoever defined in the Transaction Documents which is not remedied within the applicable cure period, if any, provided under the Transaction Documents, the Lender shall have the unconditional and unrestricted right to enforce the security created on the Mortgaged Property herein, take possession of, sell and dispose of or otherwise deal with the Mortgaged Property with or without the intervention of the courts to recover all outstanding amounts payable to the Lender under the Transaction Documents.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>10. The stamp duty, registration charges adjudication charges and other costs in respect of this Indenture, & any other document required for the purpose of perfecting the present Security in favour of the Lenders shall be borne and paid by the Mortgagors only.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>11. For all or any of the aforesaid. Purposes mentioned in this Indenture, the Mortgagors hereby irrevocably appoint the Lender to be their attorney and in the name and on behalf of the Mortgagors to execute and do all acts, deeds and things which the Mortgagors ought to execute and do under the covenants and provisions herein contained and generally to use the name of the Mortgagors in the exercise of all or any' of the power( s) by these presents conferred on the Lender.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>12. Any notices to be provided by either Party shall be in writing, signed by an authorized officer, if sent by post, delivered to the addresses set out below or to such other address as may be notified for the purpose by the Parties from time to time, and if sent by facsimile, sent to the numbers notified below or to such other number as may be notified for the purpose by the Parties from time to time. Notices shall be deemed to have been delivered two (2) working days following dispatch.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>13. The Mortgagors hereby confirm that no delay of the Lender in exercising or not exercising any right, power or remedy accruing/available to the Lender under this Indenture shall impair or prejudice such right, power or remedy or shall be construed as its waiver or acquiescence. The Mortgagors further confirm that any single or partial exercise of any right, power or remedy by the Lender shall not preclude further exercise thereof. Every right and remedy of the Lender shall continue in full force until the Lender specifically waives it by a written instrument.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>14. The Mortgagors shall indemnify and keep the Lender, its respective directors, employees and associates, at all times, indemnified against all actions, suits, proceedings and all costs, charges, expenses, losses or damages etc. which may be incurred or suffered by the Lender by reason of any false or misleading information given by the Mortgagors to the Lender herein or by any breach / default / contravention/non-observance/ non-performance by the Mortgagors of any terms, covenants, conditions, agreements and provisions of this Indenture.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>15.That theMortgagors hereby agreed and confirmed that he/ they will spend all the loan money only on the specific purpose for which the lender company sanctioned the loan. Hereby Mortgagors agreed and confirmed that he/ they will spendmoney only on property sanction by govt. authority (NIT, NMC, Etc ) and also confirmed that Mortgagors is strictly not allowed to spend money on extra /  illegal construction and any other purpose / property, other than property mentioned in Schedule Iof this document.</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'><b>16. DISCLOSURES</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>16.1 	The Mortgagors hereby accept and confirm that as a pre-condition to the grant of the credit facility/ies by the Lenders to the Mortgagors, the Lender require consent of the Mortgagors to make certain disclosures including information and data relating to the Obligors and any credit facility availed of or to be availed of by the Mortgagors, obligations assumed or to be assumed by the Mortgagors in relation thereto under this Indenture and default, if any, committed by the Mortgagors in discharge thereof.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>16.2 	Accordingly, the Mortgagors hereby agree and give consent to the disclosure by the Lender of all or any such:</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>(a) Information and data relating to the Mortgagors.</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>(b) The information or data relating to any credit facilities including, the Loan availed by the Mortgagors from the Lender and obligations cast on the Mortgagors pursuant to this Indenture; and</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>(c) Default, if any, committed by the Obligor's in discharge of their respective obligations in terms of this Indenture; as the Lender may deem appropriate and necessary, to disclose and furnish to Credit Information Bureau(India) Ltd. and any other agency authorized in this behalf by Reserve Bank of India or any other regulatory authority.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>16.3 	The Mortgagors further declare that the information and data furnished by the Mortgagors to the Lenders is/shall be true and correct and further undertake and declare that:</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>(a) The Credit Information Bureau (India) Ltd. and any other agency so authorized may use, process the said information and data disclosed by the Lenders in the manner as deemed fit by them; and</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>(b) The Credit Information Bureau (India) Ltd. and any other agency so authorized may furnish for consideration, the processed information and data or products thereof prepared by them to banks/financial institutions and other credit grantors or registered users, as may be specified by the Reserve Bank of India in this behalf.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>17. The rights and remedies available to the Lender under this Indenture shall be in addition to any rights and remedies available to the Lender under any other provision of Applicable law or Transaction Documents executed in respect of the Loan granted by the Lender. The rights and remedies provided in this Indenture are cumulative and not exclusive of any rights or remedies provided by law.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>18. No amendment or modification to this Indenture will be effective or binding unless it is in writing and executed by the Parties hereto.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>19. If, at any time, any provision of this Indenture is or becomes illegal, invalid or unenforceable in any respect, neither the legality, validity or enforceability of the remaining provisions will in any way be affected or impaired. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>20. That the Security hereby created shall remain in full force by virtue of this Indenture till the time the Loan and the outstanding amounts thereof under the Transaction Documents are fully repaid by the Mortgagors.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'><b>21.DISPUTE RESOLUTION -ARBITRATION</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>In the event of any dispute or differences arising directly or indirectly out of this Agreement or in relation to any other documents executed in connection with the Facility or otherwise, the Parties undertake to use all reasonable endeavors to resolve such disputes amicably. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>If disputes and differences cannot be settled amicably, then all disputes and differences arising between the Parties hereto in connection with this Agreement or the interpretation hereof or anything done or omitted to be done pursuant hereto or the performance or non-performance of this Agreement shall be referred to the Arbitration.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>Such disputes, differences and/or claims arising out of these presents or as to the construction, meaning or effect hereof or as to the rights and liabilities of the parties hereunder shall be settled by Arbitration to be held at Mumbai in accordance with the provisions of the Arbitration and Conciliation Act, 1996 or any statutory amendments thereof or an statute enacted for replacement there for and shall be referred to a single arbitrator to be appointed by the Lender (hereinafter referred to as 'Arbitrator') and the &Arbitrator's award shall be final and binding on both the Parties hereto.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>The arbitration shall be held in Mumbai and the expenses of the arbitration shall be borne in such manner as the Arbitrator may determine. In the event of death, refusal, neglect, inability or incapability of the person so appointed to act as an Arbitrator, the Lender shall appoint a new Arbitrator. The award including interim award/s of the arbitration shall be final, conclusive and binding on all parties concerned. The Arbitrator shall not give any reason for his award including interim award/so The Arbitrator may lay down from time to time the procedure to be followed by him in conducting arbitration proceedings and shall conduct the arbitration proceedings in such manner as he considers appropriate. The Arbitration shall be governed by the Arbitration & Conciliation Act, 1996 or such other law relating to Arbitration as may be in force in India at the relevant time.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>IN WITNESS WHEREOF the respective common seals of the Mortgagors have been affixed hereto, and the authorized signatory of the Lender have executed this Indenture, on the date mentioned above.</td>");
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
                        sb.Append(@"<td width='30%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                        sb.Append(@"<table width='50%' height='100px' cellspacing='0' cellpadding='5' border='1'>");
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '> photograph</td>");
                        sb.Append(@"</tr>");
                        sb.Append(@"</table>");
                        sb.Append(@"</td>");
                        sb.Append(@"<td width='40%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                        sb.Append(@"<table width='100%' height='100px' cellspacing='0' cellpadding='5'>");
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>{0}</b></td>", dt1.Rows[i]["CustName"].ToString());
                        sb.Append(@"</tr>");
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '> <b>Aadhar No. {0}</b></td>", dt1.Rows[i]["AadharNo"].ToString());
                        sb.Append(@"</tr>");
                        sb.Append(@"</table>");
                        sb.Append(@"</td>");
                        sb.Append(@"<td width='30%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                        sb.Append(@"<table width='50%' height='100px' cellspacing='0' cellpadding='5' border='1'>");
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>L. H.Thumb.</td>");
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
                    sb.Append(@"<td width='30%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='50%' height='100px' cellspacing='0' cellpadding='5' border='1'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '> photograph</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"<td width='40%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
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
                    sb.Append(@"<td width='30%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='50%' height='100px' cellspacing='0' cellpadding='5' border='1'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>L. H.Thumb.</td>");
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
                    sb.Append(@"<td width='30%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='50%' height='100px' cellspacing='0' cellpadding='5' border='1'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '> photograph</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"<td width='40%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
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
                    sb.Append(@"<td width='30%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='50%' height='100px' cellspacing='0' cellpadding='5' border='1'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>L. H.Thumb.</td>");
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