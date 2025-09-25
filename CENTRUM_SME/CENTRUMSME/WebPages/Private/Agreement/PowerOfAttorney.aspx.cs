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
    public partial class PowerOfAttorney : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void LoadRecord()
        {
            try
            {
                DataTable dt = (DataTable)Session["POADt"];
                StringBuilder sb = new StringBuilder();
                if (dt.Rows.Count > 0)
                {
                    string word = ConvertNumbertoWords(Convert.ToInt32(dt.Rows[0]["SanctionAmt"]));
                    
                    sb.Append(@"<table width='1002px' cellspacing='10' cellpadding='0' border='0' style='margin:0 auto;'>");
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:12px; line-height:14px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='0' border='0'>");
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='15%' align='left' rowspan='3' style='font-family:Garamond; font-size:16px; line-height:16px;'><img src='../../../images/KUDOS-logo.gif' height='80px' alt='Company Logo' /></td>");
                    sb.AppendFormat(@"<td width='70%' align='center' style='font-family:Garamond; font-size:30px; line-height:30px;'><b>{0}</b></td>", "Centrum Micro Credit");
                    sb.Append(@"<td width='15%' align='left' rowspan='3' style='font-family:Garamond; font-size:12px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='70%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", CGblIdGenerator.GetBranchAddress1("0000"));
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='70%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", CGblIdGenerator.GetBranchAddress2("0000") + " CIN No-");
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
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:22px; line-height:30px; font-weight:bold; text-decoration: underline;'>POWER OF ATTORNEY</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>TO ALL TO WHOM THESE PRESENTS THAT I/WE <b>{0}</b> a company incorporated under the provisions of the Indian Companies Act, 1956/2013/ Partnership Firm registered with the Registrar of Firms/ Sole Proprietorship Firm/ Individuals having its Registered Office / Place of Business/ Residence at <b>{1}</b> and acting through Mr./ Mrs. <b>{2}</b> who is duly authorized in their behalf vide Board Resolution/ Letter of Authority dated <b>{3}</b>. </td>", dt.Rows[0]["CompanyName"].ToString(), dt.Rows[0]["ComAddress"].ToString(), dt.Rows[0]["ApplicantName"].ToString(), dt.Rows[0]["AgrmntDate"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>SEND GREETING:</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>WHEREAS I/we have been doing business at <b>{0}</b> and/ elsewhere in India;</td>", dt.Rows[0]["ComAddress"].ToString());
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>AND WHEREAS <b> {0}</b>, a company incorporated under the Companies Act, 1956, having its Registered Office at <b>{1}{2} Centrum Micro Credit</b>, (hereinafter referred to as “Centrum Micro Credit”) has granted or agreed to grant us for the purpose of our said business credit facility/ies (<b>{3}</b>), presently, to the extent of <b> Rs. {4}(Rupees {5} only )</b></td>", gblValue.CompName, CGblIdGenerator.GetBranchAddress1("0000"), CGblIdGenerator.GetBranchAddress2("0000"), dt.Rows[0]["LoanType"].ToString(), dt.Rows[0]["SanctionAmt"].ToString(), word);
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>AND WHEREAS in the course of our business, there are certain book debts now due and owing to us and there will be also some future debts which may hereafter become due and owing to us from time to time by our various constituents, customers, agents and dealers and various persons;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>AND WHEREAS we are desirous of authorizing Centrum Micro Credit to recover and realize all our present and future book debts, receivables now owing or which may hereafter become owing to us in the course of our said business from or by the various constituents customers, agents and dealers and all other persons as aforesaid, and out of the net recovery and realization so received to pay itself the moneys in repayment of the our dues to Centrum Micro Credit under or in respect of the said Facility from time to time. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'><b>NOTE:</b> Reference to the words I, me and my shall include both singular and plural as the context may require. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>NOW KNOW <b>{0}</b>AND THESE PRESENTS WITNESS that I/we, M/s <b>{1}</b> do hereby irrevocably nominate constitute and appoint Centrum Micro Credit, to act through any of its Officers, each of them severally to be our true and lawful Attorneys for us in our name and on our behalf and for our use and benefit and at our costs to do all the following acts deeds matters and things or any of them that is to say:</td>", "Centrum Micro Credit", dt.Rows[0]["CompanyName"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'><b>1.</b>	To ask, demand, sue for recover and receive of and from all our constituents, customers, agents and dealers and all other persons liable to pay, transfer and deliver the same respectively all and every debt or debts sum or sums of money goods, chattels and effects due and owing to us by virtue of any security or upon any balance of account or otherwise howsoever as we shall at any time hereafter in writing under our hand express to be due from them or any of them and upon receipt thereof or any part hereof for us and in our name to give, sign and execute good and sufficient receipts, release, conveyances and other discharges for the same respectively. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'><b>2.</b>	Upon non-payment, non-transfer or non-delivery thereof or any part thereof respectively to commence, carry on and prosecute any action, suit or other proceedings whatsoever for recovering and compelling the payment, transfer or delivery thereof respectively and for the purpose to engage solicitor and advocates and to settle and pay their fees. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'><b>3.</b>	To settle, compound and submit to arbitration all actions, suits, accounts, claims and demands whatsoever which now are or hereafter shall or may be pending between us and any such constituent, dealers, agents, customers and all other persons as aforesaid in such manner and in all respects as our said Attorneys shall think fit. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'><b>4.</b>	To pay Centrum Micro Credit itself out of and to appropriate and credit the monies so realized after deducting there out all costs, charges and expenses incurred by Centrum Micro Credit for the recovery and realization thereof, between Advocate and client and all the Centrum Micro Credit usual charges against the said Facility granted by Centrum Micro Credit to us and/or against monies whatsoever due and payable by us to Centrum Micro Credit howsoever. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'><b>5.</b>	To sign, make, affirm and declare all such applications, affidavits, petitions, pleadings, written statements, counterclaims, memos of review or revision, and memos of appeal as the Attorneys may deem proper.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'><b>6.</b>	To apply for, review of or to appeal from any order or decree passed against us. </td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'><b>7.</b>	To execute any order or decree passed in our favor by attachment and sale of the property of the judgment debtor and/or by detention of the person of the judgment debtor in civil prison.  </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'><b>8.</b>	To use and sign our name for the purpose aforesaid. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'><b>IN GENERAL</b> to do and execute all such other acts deeds matters and things as may be necessary for the purposes aforesaid as fully and effectually to all intents and purposes as we could to in our own proper person if these presents had not been made.   </td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>And we the undersigned M/s <b>{0}</b> do hereby undertake to ratify and confirm everything which our said Attorneys shall do or purport to do in virtue of these presents.  </td>", dt.Rows[0]["CompanyName"].ToString());
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>IN WITNESS WHEREOF we have executed this writing at  <b>{0}</b> on <b>{1}</b>.</td>", "Pune", dt.Rows[0]["AgrmntDate"].ToString());
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>SIGNED AND DELIVERED By   </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;font-weight:bold; text-decoration: underline;'><b> (i) {0}</b></td>", "");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;font-weight:bold; text-decoration: underline;'><b> (ii) {0}</b></td>", "");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>In the presence of <b>{0}</b></td>", "");
                    sb.Append(@"</tr>");


                    //sb.Append(@"<tr>");
                    //sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>In the presence of <b>{0}</b></td>", "");
                    //sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;'>(Witness)</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;font-weight:bold; text-decoration: underline;'>In the presence of <b>1.	____________________</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:14px; line-height:16px;font-weight:bold; text-decoration: underline;'>In the presence of <b>2.	____________________</b></td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.AppendFormat(@"<td width='50%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px;'>BEFORE ME (NOTARY)</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
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

        //public override void VerifyRenderingInServerForm(Control control)
        //{

        //}
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "mywindow", "printform();", true);
        }
    }
}