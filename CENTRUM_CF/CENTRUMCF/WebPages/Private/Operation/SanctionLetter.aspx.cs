using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using CENTRUMCA;


namespace CENTRUMCF.WebPages.Private.Operation
{
    public partial class SanctionLetter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void LoadRecord()
        {
            Int32 a = 1;
            DataSet ds = (DataSet)Session["SLDt"];
            DataTable dt = ds.Tables[0];
            StringBuilder sb = new StringBuilder();           

            if (dt.Rows.Count > 0)
            {

                string RefNo = dt.Rows[0]["AppNo"].ToString();
                string Date = DateTime.Today.ToString("dd/MM/yyyy");
                string BorrowerName = dt.Rows[0]["MemberName"].ToString();
                string BorrowerAddress = dt.Rows[0]["Address"].ToString();
                string BranchOffice = dt.Rows[0]["BranchName"].ToString();
                string LoanType = dt.Rows[0]["LoanTypeName"].ToString();
                string LoanAmount = dt.Rows[0]["LoanAmt"].ToString();
                string Tenor = dt.Rows[0]["Tenure"].ToString();
                string Purpose = dt.Rows[0]["PurposeFclty"].ToString();
                string RateOfInterest = dt.Rows[0]["ROI"].ToString();
                string Margin = "";
                string ProcessingFees = dt.Rows[0]["ProcessingFee"].ToString();
                string OtherFees = dt.Rows[0]["OtherFees"].ToString();
                string AuthorizedSignatoryName = "";
                string AuthorizedSignatoryDesignation = "";


                sb.Append("<table style='width:100%; font-family:Arial, sans-serif; font-size:14px; border-collapse:collapse;'>");

                // Ref No and Date
                sb.Append("<tr>");
                sb.AppendFormat("<td style='width:50%; padding:5px;'><strong>Ref. No.:</strong> {0}</td>", RefNo); 
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.AppendFormat("<td style='width:50%; padding:5px;'><strong>Date:</strong> {0}</td>", Date);
                sb.Append("</tr>");

                // Title
                sb.Append("<tr><td colspan='2' style='text-align:center; font-weight:bold; font-size:16px; text-decoration: underline;'>UNITY SMALL FINANCE BANK LIMITED</td></tr>");
                sb.Append("<tr><td colspan='2' style='text-align:center; font-weight:bold; font-size:16px; padding-bottom:20px;'>SANCTION LETTER</td></tr>");

                // Borrower Name and Address
                sb.Append("<tr><td colspan='2'><i>Name and Address of the Borrower</i></td></tr>");
                sb.AppendFormat("<tr><td colspan='2'>{0}<br />{1}<br /></td></tr>", BorrowerName, BorrowerAddress.Replace("\n", "<br />"));

                // Kind Attention and Greeting
                sb.Append("<tr><td colspan='2' style='padding-top:20px;font-weight:bold;'>Kind attention:</td></tr>");
                sb.Append("<tr><td colspan='2'>Dear Sir/ Madam,</td></tr>");

                // Subject
                sb.Append("<tr><td colspan='2' style='padding-top:20px;'><strong>SUB: SANCTION OF LOAN.</strong></td></tr>");

                // Introductory paragraph (long text)
                sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                sb.AppendFormat("We, <strong>Unity Small Finance Bank Limited</strong> a public limited company incorporated under the Companies Act, 2013 and licensed under section 22 of the Banking Regulation Act, 1949, to carry on its business of banking by the Reserve Bank of India, having corporate identification number U65990DL202l PLC385568 and registered having its office at Unit No. 1201, 1202 & 1203, 12th floor, Ansal Bhawan, 16, KG Marg, New Delhi – 110001 and a branch office at {0} <strong>(“Bank”)</strong>, are pleased to offer {1} <strong>(“Borrower”)</strong> the credit facility upto a maximum amount as mentioned under this sanction letter <strong>(“Loan”) subject to the terms and conditions (“Terms and Conditions”)</strong> as set out in this sanction letter <strong>(“Sanction Letter”)</strong>.", BranchOffice, BorrowerName);
                sb.Append("</td></tr>");

                // Next paragraphs
                sb.Append("<tr><td colspan='2' style='padding-top:10px;'>This Sanction Letter is intended for your guidance and information only and does not constitute an offer, commitment or agreement to enter into any transaction. Rather, it is intended to outline certain basic terms upon which the Loan would be granted to you. The terms and conditions set out below are indicative and not exhaustive. Detailed terms and conditions of the Loan would be mentioned under the relevant Loan documents. The Loan will be made available on acceptance and due execution of requisite Loan documents, submission of required undertaking, completion of legal due diligence and completion of Know Your Customer (KYC) documentation and creation and perfection of requisite securities, as applicable, in the form and substance acceptable to the Bank.</td></tr>");

                sb.Append("<tr><td colspan='2' style='padding-top:20px;'>I/ We, hereby accept terms and conditions stipulated in this letter and annexure (s) hereto:</td></tr>");
               
                // Details Table inside main table (2 columns)
                sb.Append("<tr><td colspan='2'>");
                sb.Append("<table style='width:100%; border-collapse:collapse;' border='1'>");

                // Borrower & Lender Details row
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='padding:5px; width:30%; background-color: #cccccc;'>TERMS AND CONDITIONS FOR THE LOAN</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='padding:5px; width:30%;'>Details of the Borrower</td>");
                sb.AppendFormat("<td style='padding:5px; width:70%;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='padding:5px; width:30%;'>Details of the Lender</td>");
                sb.AppendFormat("<td style='padding:5px; width:70%;'>UNITY SMALL FINANCE BANK LIMITED a public limited company incorporated under the Companies Act, 2013 and licensed under section 22 of the Banking Regulation Act, 1949, to carry on its business of banking by the Reserve Bank of India, having corporate identification number U65990DL202l PLC385568 and registered having its office at Unit No. 1201, 1202 & 1203, 12th floor, Ansal Bhawan, 16, KG Marg, New Delhi – 110001 and a branch office at {0}.</td>", BranchOffice);
                sb.Append("</tr>");

                // Loan details rows
                sb.AppendFormat("<tr><td style='padding:5px;'>Loan Type</td><td style='padding:5px;'>{0}</td></tr>", LoanType);
                sb.AppendFormat("<tr><td style='padding:5px;'>Loan Amount</td><td style='padding:5px;'>{0}</td></tr>", LoanAmount);
                sb.AppendFormat("<tr><td style='padding:5px;'>Tenor</td><td style='padding:5px;'>{0}</td></tr>", Tenor);
                sb.AppendFormat("<tr><td style='padding:5px;'>Purpose</td><td style='padding:5px;'>{0}</td></tr>", Purpose);
                sb.AppendFormat("<tr><td style='padding:5px;'>Availability Period</td><td style='padding:5px;'>{0}</td></tr>","");
                sb.AppendFormat("<tr><td style='padding:5px;'>Rate of Interest</td><td style='padding:5px;'>{0}</td></tr>", RateOfInterest);
                sb.AppendFormat("<tr><td style='padding:5px;'>Margin/ Spread</td><td style='padding:5px;'>{0}</td></tr>", Margin);

                // Security and Timelines with nested bullets
                sb.Append("<tr>");
                sb.Append("<td style='padding:5px; vertical-align:top;'>Security and Timelines</td>");
                sb.Append("<td style='padding:5px;'>");
                sb.Append("The Loan shall be secured by :<br />");
                sb.Append("<ol style='padding-left:20px; margin-top:0;'>");
                sb.Append("<li><strong>Primary</strong><br />");
                sb.Append("• Solar rooftop panels<br />");
                sb.Append("• Roof Mounts<br />");
                sb.Append("• Ground Mounts<br />");
                sb.Append("• Shed/Tin Mounts</li>");
                sb.Append("<li><strong>Collateral</strong> Mortgage if applicable</li>");
                sb.Append("<li><strong>Guarantee</strong> Guarantee if applicable</li>");
                sb.Append("</ol>");
                sb.Append("Timeline:");
                sb.Append("</td>");
                sb.Append("</tr>");

                // Other terms rows
                sb.Append("<tr><td style='padding:5px;'>Repayment Schedule</td><td style='padding:5px;'></td></tr>");

                sb.Append("<tr>");
                sb.Append("<td style='padding:5px; vertical-align:top;'>Fees</td>");
                sb.Append("<td style='padding:5px;'>");
                sb.AppendFormat("(i) Upfront Fees / Processing Fees (exclusive of taxes, if any): Rs.{0}<br />", ProcessingFees);
                sb.AppendFormat("(ii) Any other Fees or Charges, as may be applicable (exclusive of taxes, if any): Rs.{0}", OtherFees);
                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("<tr><td style='padding:5px;'>Cost</td>");
                sb.Append("<td style='padding:5px;'>All costs / legal expenses/ out of pocket expenses including for preparation, perfection and execution of the Loan Documents including the security documentation, applicable duties, taxes including stamp duties, are to be paid by the Borrower</td></tr>");

                sb.Append("<tr><td style='padding:5px;'>Release of property</td>");
                sb.Append("<td style='padding:5px;'>The Bank shall release all original movable / immovable Property Documents ( hereinafter referred to as “Property Documents\") and remove any charges registered with relevant authorities/any registry within a period of 30 days after closure of loan. The Borrower shall have the option to collect the Property Documents from the banking outlet / branch where the loan account was serviced or any other office of the Bank where the property documents are available. The timeline for such release of property documents shall be within 30 working days from the date of loan closure. The Loan shall be subject to the applicable laws/regulations and the policies of the Bank.</td></tr>");

                sb.AppendFormat("<tr><td style='padding:5px;'>Prepayment Charges</td><td style='padding:5px;'>{0}</td></tr>", Margin);
                sb.AppendFormat("<tr><td style='padding:5px;'>Interest Reset Days</td><td style='padding:5px;'>{0}</td></tr>", Margin);
                sb.AppendFormat("<tr><td style='padding:5px;'>Disbursement Schedule</td><td style='padding:5px;'>{0}</td></tr>", Margin);
                sb.AppendFormat("<tr><td style='padding:5px;'>Drawdown Notice</td><td style='padding:5px;'>{0}</td></tr>", Margin);
                sb.AppendFormat("<tr><td style='padding:5px;'>Inspection</td><td style='padding:5px;'>{0}</td></tr>", Margin);
                sb.AppendFormat("<tr><td style='padding:5px;'>Insurance</td><td style='padding:5px;'>{0}</td></tr>", Margin);
                sb.AppendFormat("<tr><td style='padding:5px;'>Conditions Precedent</td><td style='padding:5px;'>{0}</td></tr>", Margin);
                sb.AppendFormat("<tr><td style='padding:5px;'>Financial Covenants</td><td style='padding:5px;'>{0}</td></tr>", Margin);
                sb.AppendFormat("<tr><td style='padding:5px;'>Conditions Subsequent</td><td style='padding:5px;'>{0}</td></tr>", Margin);
                sb.AppendFormat("<tr><td style='padding:5px;'>Cancellation of Loan</td><td style='padding:5px;'>{0}</td></tr>", Margin);
                sb.AppendFormat("<tr><td style='padding:5px;'>Other Charges (If Applicable)</td><td style='padding:5px;'>{0}</td></tr>", Margin);
                sb.AppendFormat("<tr><td style='padding:5px;'>Governing Law</td><td style='padding:5px;'>{0}</td></tr>", Margin);
                

                sb.Append("</table>");
                sb.Append("</td></tr>");

                // Closing paragraphs
                sb.Append("<tr><td colspan='2' style='padding-top:20px;'>");
                sb.Append("You are requested to accept the conditions stipulated under this Sanction Letter and return the duplicate copy of this Sanction Letter to us within 30 days from the date of issuance of this Sanction Letter, Post which this Sanction Letter shall lapse. This letter is valid for a period of 30 days from the date hereof and shall expire immediately on the lapse of the described period, provided however, that the Bank may, at its sole discretion and without any obligation, consider your acceptance of this letter even after the expiry of such period.");
                sb.Append("</td></tr>");

                sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                sb.Append("Please note that this communication should not be constructed as giving rise to any binding obligation on the part of the Bank with regards to the Loan.<br />");
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan='2' style='padding-top:5px;'>");
                sb.Append("The terms and conditions contained in this document are confidential and are not to be disclosed to any other person (save the Borrower's legal & tax advisors for the purposes of the proposed transaction).<br />");
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan='2' style='padding-top:5px;'>");
                sb.Append("In case you (the Borrower) have any queries, please contact _______________ at ______________. We assure you of our best services at all times.");
                sb.Append("</td></tr>");

                // Signature section for bank
                sb.Append("<tr><td colspan='2' style='padding-top:40px;'>");
                sb.Append("Yours Faithfully,<br /><br />");
                sb.Append("For and behalf of <strong>UNITY SMALL FINANCE BANK LIMITED</strong><br /><br />");
                sb.Append("<strong>__________________________</strong><br />");
                //sb.AppendFormat("Authorized Signatory<br />Name: {0}<br />Designation: {1}<br /><br />", AuthorizedSignatoryName, AuthorizedSignatoryDesignation);
                sb.AppendFormat("Authorized Signatory<br /><br /><br />Name: ____________________________________________________<br /><br /><br />Designation: _________________________________________________<br /><br />");
                sb.Append("</td></tr>");

                // Acceptance paragraph
                sb.Append("<tr><td colspan='2' style='padding-top:20px;'>");
                sb.Append("We hereby accept the offer to make available to us the Loan and agree to be bound by the terms, conditions and provisions of this Sanction Letter and the following conditions:<br />");
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan='2' style='padding-top:5px;'>");
                sb.Append("1. To execute all required loan documentation for the said Loan including but not limited to loan agreement, application form, key facts statement, security documents and all other documents executed with respect to the Loan granted herein (“Loan Documents”).<br />");
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan='2' style='padding-top:5px;'>");
                sb.Append("2. To utilise the loan amount solely for the purpose it is sanctioned by the Bank and not for any kind of speculative, anti-social and illegal activities, any agricultural activity and any other activity prohibited by RBI from time to time.");
                sb.Append("</td></tr>");

                // Declaration from Borrower(s)
                sb.Append("<tr><td colspan='2' style='padding-top:20px;'>");
                sb.Append("Declaration from Borrower(s)<br /><br />");
                sb.Append("I/We understand and accept the terms & conditions mentioned above and adhere to all the terms and conditions laid down in the Loan Documents. We look forward to a mutually beneficial and long-term relationship.<br /><br />");
                sb.Append("For and behalf of<br /><br />");
                sb.Append("<strong>__________________________</strong><br /><br /><br />");
                sb.AppendFormat("Authorized Signatory<br /><br /><br />Name: {0}<br /><br /><br />Designation: {1}", AuthorizedSignatoryName, AuthorizedSignatoryDesignation);
                sb.Append("</td></tr>");

                // Close main table
                sb.Append("</table>");

                
            }
            Literal1.Text = sb.ToString();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "mywindow", "printform();", true);
        }


        //StringBuilder sb = new StringBuilder();

        //sb.Append("<h2>CLIMATE FINANCE ROOFTOP SOLAR SYSTEM – APPLICATION FORM</h2>");
        //sb.Append("<h3>Customer Declaration</h3>");

        //// Declarations list
        //sb.Append("<ol>");
        //sb.Append("<li>I/we am/are a citizen and tax resident of India.</li>");
        //sb.Append("<li>I/we are a company established in accordance with the laws of India.</li>");
        //sb.Append("<li>I/We have not submitted any application for insolvency, been declared insolvent nor has any insolvency/bankruptcy proceeding been initiated against me/us.</li>");
        //sb.Append("<li>I/We have not defaulted on any repayment of any other loan repayment or any other amount to any lender or creditor and my/our name does not appear on the Reserve Bank of India's (\"RBI\") list of defaulters...</li>");
        //// (Add all declarations similarly)
        //sb.Append("<li>I/We are neither related to any directors of Bank nor I/We are his/her relatives as defined under Companies Act, 2013.</li>");
        //sb.Append("</ol>");

        //sb.Append("<br/><hr/>");

        //sb.Append("<h3>FILL ALL FIELDS IN CAPITAL LETTERS ONLY</h3>");

        //// SOURCING DETAILS Table
        //sb.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse:collapse;width:100%'>");
        //sb.Append("<tr><th colspan='2'>SOURCING DETAILS</th></tr>");
        //sb.AppendFormat("<tr><td>Channel</td><td><input type='text' name='channel' placeholder='Direct Walkin / Branch Sourcing / ...' style='width:100%'/></td></tr>");
        //sb.AppendFormat("<tr><td>Sub Source</td><td><input type='text' name='subSource' style='width:100%'/></td></tr>");
        //sb.AppendFormat("<tr><td>EPC Name & Code</td><td><input type='text' name='epcNameCode' style='width:100%'/></td></tr>");
        //sb.Append("</table>");

        //sb.Append("<br/>");

        //// LOAN DETAILS Table
        //sb.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse:collapse;width:100%'>");
        //sb.Append("<tr><th colspan='2'>LOAN DETAILS (“the Loan”)</th></tr>");
        //sb.AppendFormat("<tr><td>Application Date</td><td><input type='date' name='applicationDate' /></td></tr>");
        //sb.AppendFormat("<tr><td>Branch Name</td><td><input type='text' name='branchName' /></td></tr>");
        //sb.AppendFormat("<tr><td>Applicant Type</td><td><select name='applicantType'><option>New</option><option>Existing</option></select></td></tr>");
        //sb.AppendFormat("<tr><td>Loan Type</td><td><select name='loanType'><option>Term Loan</option></select></td></tr>");
        //sb.AppendFormat("<tr><td>Loan Amount Required</td><td><input type='number' name='loanAmount' /></td></tr>");
        //sb.AppendFormat("<tr><td>Repayment Option</td><td><select name='repaymentOption'><option>EMI</option></select></td></tr>");
        //sb.AppendFormat("<tr><td>Loan Tenor</td><td><input type='text' name='loanTenor' /></td></tr>");
        //sb.AppendFormat("<tr><td>Bank Account Details for Disbursal</td><td>Bank Name: <input type='text' name='bankName' style='width:80%' /><br/>Branch: <input type='text' name='branch' style='width:80%' /><br/>Account Holder Name: <input type='text' name='accountHolder' style='width:80%' /><br/>Account Number: <input type='text' name='accountNumber' style='width:80%' /><br/>IFSC Code: <input type='text' name='ifscCode' style='width:80%' /><br/>Account Type: <input type='text' name='accountType' style='width:80%' /></td></tr>");
        //sb.Append("</table>");

        //sb.Append("<br/>");

        //// PERSONAL DETAILS Table (Primary Applicant)
        //sb.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse:collapse;width:100%'>");
        //sb.Append("<tr><th colspan='2'>PERSONAL DETAILS (Primary Applicant)</th></tr>");
        //sb.AppendFormat("<tr><td>Applicant Name</td><td><input type='text' name='applicantName' style='width:100%' /></td></tr>");
        //sb.AppendFormat("<tr><td>Residence Address</td><td><input type='text' name='residenceAddress' style='width:100%' /><br/>Landmark:<input type='text' name='residenceLandmark' /></td></tr>");
        //sb.AppendFormat("<tr><td>City</td><td><input type='text' name='residenceCity' /></td></tr>");
        //sb.AppendFormat("<tr><td>Pin</td><td><input type='text' name='residencePin' /></td></tr>");
        //sb.AppendFormat("<tr><td>Residence Type</td><td><select name='residenceType'><option>Owned</option><option>Rented</option><option>Leased</option><option>Company Provided</option></select></td></tr>");
        //sb.AppendFormat("<tr><td>Years at Current Residence</td><td><input type='number' name='yearsAtResidence' /></td></tr>");
        //// Similarly add office address, permanent address, contacts, DOB, Gender, Marital Status, etc.
        //sb.Append("</table>");

        //sb.Append("<br/>");

        //// You can add other sections similarly by repeating the pattern of sb.Append with tables and inputs

        //// Finally, add a submit button
        //sb.Append("<input type='submit' value='Submit Application' />");

        //return sb.ToString();
    }
}