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
    public partial class SanctionLetter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void LoadRecord()
        {
            string BankName = "";
            string AcNo = "";
            DataTable dt = (DataTable)Session["SLDt"];
            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count > 0)
            {

                sb.Append(@"<table width='1002px' cellspacing='10' cellpadding='0' border='0' style='margin:0 auto;'>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:48px; line-height:48px;'>&nbsp;</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:12px; line-height:16px;'>");
                sb.Append(@"<table width='100%' cellspacing='0' cellpadding='0' border='0'>");
                //sb.Append(@"<tr>");
                //sb.Append(@"<td width='10%' align='left' rowspan='3' style='font-family:Garamond; font-size:16px; line-height:16px;'><img src='../../../images/Centrum_Logo.png' height='60px' width='180px' alt='Company Logo' /></td>");
                //sb.AppendFormat(@"<td width='80%' align='center' style='font-family:Garamond; font-size:30px; line-height:30px;'><b>{0}</b></td>", "Centrum SME Loan");
                //sb.Append(@"<td width='10%' align='left' rowspan='3' style='font-family:Garamond; font-size:12px; line-height:16px;'>&nbsp;</td>");
                //sb.Append(@"</tr>");
                //sb.Append(@"<tr>");
                //sb.AppendFormat(@"<td width='80%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", CGblIdGenerator.GetBranchAddress1("0000"));
                //sb.Append(@"</tr>");
                //sb.Append(@"<tr>");
                //sb.AppendFormat(@"<td width='80%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", (CGblIdGenerator.GetBranchAddress2("0000")));
                //sb.Append(@"</tr>");
                //sb.Append(@"</table>");
                //sb.Append(@"</td>");
                //sb.Append(@"</tr>");


                //sb.Append(@"<tr>");
                //sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:18px; line-height:18px;'>&nbsp;</td>");
                //sb.Append(@"</tr>");
                //sb.Append(@"<tr>");
                //sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:32px; line-height:32px;'>&nbsp;</td>");
                //sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'>");
                sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Calibri (Body); font-size:11px; line-height:30px; font-weight:bold; text-decoration: underline;'>Sanction Letter</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='left' style='font-family:Calibri (Body); font-size:16px; line-height:16px; >Date : {0}</td>", dt.Rows[0]["FinalSanctionDate"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"</table>");
                sb.Append(@"</td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:18px; line-height:16px;'>&nbsp;</td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'>");
                sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                sb.Append(@"<tr>");
                //sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Applicant Name :</td>");
                sb.AppendFormat(@"<td width='80%' align='left' style='font-family:Calibri (Body); font-size:16px; line-height:16px; font-weight:bold; '><b>{0}</b></td>", dt.Rows[0]["CompanyName"].ToString());
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Co-Applicant Name :</td>");
                sb.AppendFormat(@"<td width='80%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold; '><b>{0}</b></td>", dt.Rows[0]["CoAppName"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Guarantor Name :</td>");
                sb.AppendFormat(@"<td width='80%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold; '><b>{0}</b></td>", dt.Rows[0]["GuarantorName"].ToString());
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Residential Address :</td>");
                sb.AppendFormat(@"<td width='80%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold; '><b>{0}</b></td>", dt.Rows[0]["ComAddress"].ToString());
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Contact Number :</td>");
                sb.AppendFormat(@"<td width='80%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold; '><b>{0}</b></td>", dt.Rows[0]["ContactNo"].ToString());
                sb.Append(@"</tr>");

                sb.Append(@"</table>");
                sb.Append(@"</td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'>&nbsp;</td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:18px; line-height:16px;'>Dear Sir/Madam,</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:18px; line-height:16px;font-weight:bold;'>Sub.: In-principle sanction Of Loan Against Property Application Number {0}</td>", dt.Rows[0]["SanctionId"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:18px; line-height:16px;'>Thank you for selecting <b>Centrum SME Loan.</b> for your loan requirement. The <b>Centrum SME Loan.</b> is pleased to communicate sanction of the loan, subject to the general and the special terms and conditions (changes as per RBI directives / the NBFC’s policies from time to time) mentioned below and overleaf. This loan is also subject to the conditions that are contained in the documents, which you shall execute between and in favour of the <b>Centrum SME Loan.</b></td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'>&nbsp;</td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'>");
                sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='1'>");
                sb.Append(@"<tr>");
                sb.Append(@"<td width='30%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Sanctioned Loan Amount(Rs)</b></td>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["SanctionAmt"].ToString());
                sb.Append(@"<td width='30%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Rate of Interest</b></td>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0} %</b></td>", dt.Rows[0]["RIntRate"].ToString());
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.Append(@"<td width='30%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Indicative EMI (Rs)</b></td>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["EMI"].ToString());
                sb.Append(@"<td width='30%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Term in Months</b></td>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["Tenure"].ToString());
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.Append(@"<td width='30%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Processing fee (A)</b></td>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["ProcFees"].ToString());
                sb.Append(@"<td width='30%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Credit Shield Premium (Rs)*(B)</b></td>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["CreditShieldAmt"].ToString());
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.Append(@"<td width='30%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Property Insurance Premium (C)</b></td>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["PropertyInsurance"].ToString());
                sb.Append(@"<td width='30%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Admin Fee (Rs) (D)</b></td>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["AdminFees"].ToString());
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.Append(@"<td width='30%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Stamp Duty Charge (Rs) (E)</b></td>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["StampCharge"].ToString());
                sb.Append(@"<td width='30%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Tech Fee (Rs) (F)</b></td>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["TechFees"].ToString());
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.Append(@"<td width='30%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>CERSAI Charge (Rs) (G)</b></td>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["CERSAIAmt"].ToString());
                sb.Append(@"<td width='30%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Total(A+B+C+D+E+F+G) (Rs)</b></td>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["TotalCharge"].ToString());
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.Append(@"<td width='30%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Purpose Of Loan</b></td>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["PurposeName"].ToString());
                sb.Append(@"<td width='30%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b></b></td>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b></b></td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.Append(@"<td width='30%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>MODTD (stamp duty and Registration)Charges</b></td>");
                sb.Append(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>As per prevailing state govt. charges</b></td>");
                sb.Append(@"<td width='30%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Loan Pre-Closure Charges</b></td>");
                sb.Append(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>5% on the outstanding amount</b></td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.Append(@"<td width='30%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Credit Shield Holder Name </b></td>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["CreditHolderName"].ToString());
                sb.Append(@"<td width='30%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Credit Shield Insurer</b></td>");
                sb.AppendFormat(@"<td width='20%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["CreditShieldInsurer"].ToString());
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.Append(@"<td width='30%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Collateral Address </b></td>");
                sb.AppendFormat(@"<td width='20%' colspan='3' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["CollateralAddress"].ToString());
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.Append(@"<td width='30%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Sanction Conditions</b></td>");
                sb.AppendFormat(@"<td width='20%' colspan='3' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["SanctionCondition"].ToString());
                sb.Append(@"</tr>");


                sb.Append(@"</table>");
                sb.Append(@"</td>");
                sb.Append(@"</tr>");


                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>&nbsp;</b> </td>");
                sb.Append(@"</tr>");

                

                //sb.Append(@"<tr>");
                //sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:18px;'>&nbsp;</td>");
                //sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; '>Kindly acknowledge the receipt of this sanction letter and confirm that the terms and conditions as communicated herein are acceptable to you. The sanction letter is therefore sent to you in duplicate. You are requested to return one copy duly signed at the relevant space in token of having accepted these terms and conditions. On receipt of the copy of sanction letter duly signed by you and your executing the required document as per the terms of this sanction letter, we will arrange for the disbursement of the final loan amount.</td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>&nbsp; </td>");
                sb.Append(@"</tr>");



                sb.Append(@"<tr>");
                sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'>");
                sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='40%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'><b>For Centrum SME Loan .</b></td>");
                sb.AppendFormat(@"<td width='60%' align='center'  colspan='2'  style='font-family:Garamond; font-size:16px; line-height:16px;'>I/We accept the offer</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' colspan='3' style='font-family:Garamond; font-size:96px; line-height:96px; font-weight:bold;'><b>&nbsp;</b></td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='40%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Authorized Signatory</td>");
                sb.AppendFormat(@"<td width='30%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>{0}</td>", dt.Rows[0]["CompanyName"].ToString());
                sb.AppendFormat(@"<td width='30%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>{0}</td>",dt.Rows[0]["CoAppName"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='40%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'></td>");
                sb.AppendFormat(@"<td width='30%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>(Applicant)</td>");
                sb.AppendFormat(@"<td width='30%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>(Co-Applicant)</td>");
                sb.Append(@"</tr>");
                sb.Append(@"</table>");
                sb.Append(@"</td>");
                sb.Append(@"</tr>");


                if (dt.Rows[0]["CoAppName"].ToString().Length <= 28 && dt.Rows[0]["ComAddress"].ToString().Length <= 78 && dt.Rows[0]["SanctionCondition"].ToString().Length <= 70)
                {
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:170px; line-height:170px; '><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                }
                else if (dt.Rows[0]["CoAppName"].ToString().Length <= 28 && dt.Rows[0]["ComAddress"].ToString().Length <= 78 && dt.Rows[0]["SanctionCondition"].ToString().Length > 70)
                {
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:120px; line-height:120px; '><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                }
                else if(dt.Rows[0]["CoAppName"].ToString().Length <= 28 && dt.Rows[0]["ComAddress"].ToString().Length > 78)
                {
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:144px; line-height:144px; '><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                }
                else if (dt.Rows[0]["CoAppName"].ToString().Length > 28 && dt.Rows[0]["ComAddress"].ToString().Length > 78)
                {
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:84px; line-height:84px; '><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                }
                else if (dt.Rows[0]["CoAppName"].ToString().Length > 28 && dt.Rows[0]["ComAddress"].ToString().Length <= 78)
                {
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:116px; line-height:116px; '><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                }
                sb.Append(@"<tr>");
                sb.Append(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; '><b>General Terms and Conditions</b></td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; '>1. This offer is valid for a period of 30 (Thirty) days from the date hereof.</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>2. All formalities applicable to the Loan Against Property facility shall be complied with.</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>3. The title of the security / property should be clear and marketable. The property needs to be approved by <b>Centrum SME Loan</b> empanelled lawyer/Inhouse Legal Department. No disbursement will be made without legal clearance. You shall pay the cost of services of empanelled lawyer, etc. in connection with this loan. All taxes, duties and levies as applicable to a specific transaction are to be borne by you.</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>4. The Loan Against Property is being sanctioned to you with a clear understanding that the property in question is located in India and within the approved city limits as specified by  <b>Centrum SME Loan.</b> Even within the specified limits  <b>Centrum SME Loan.</b> may refuse to disburse the loan if the property does not meet applicable lending norms and guidelines.</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>5. Prior to disbursement you are required to submit duly signed and executed Electronic Clearing System (ECS) Form and also set of Security Post Dated Cheques (PDCs) as per  <b>Centrum SME Loan.</b>  policy, and execute all necessary documentation as prescribed by  <b>Centrum SME Loan.</b></td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>6. In case of taken over loans disbursement will only be made to the bank / financial institution from which the loan is being taken over. Or outstanding payment in favour of customer and customer need to provide the NOC or pre closed receipt of the same and after the seen on NOC or pre closed receipts balance payment can be realised as per the sanctioned conditions.</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>7. Processing Fee / Charges will be collected as per <b>Centrum SME Loan.</b> rules. <b>Centrum SME Loan.</b> will deduct the balance fees/charges payable, if any by the Borrower from the amount due for disbursement. The Processing Fee/Charges are non-refundable under any circumstances whatsoever. </td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>8. The rate of interest as indicated in the sanction letter is the current rate of interest applicable on the loan. The actual interest rate chargeable on the loan and the EMI would be that prevailing on the date of disbursement of the loan.</td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>9. EMI is payable on the 10th of every month. Pre-EMI (interest only) will be charged on the borrowed amount for the period  between the disbursement date and EMI start date.</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>10. If the monthly installments or any other payments are due and are not paid on or before the due date, additional interest of 3% per month shall be charged for the period of default with compounding at monthly rests on the overdue amount. Failure of <b>CENTRUMSME Fin Serv Pvt</b> Ltd to send notice for payment or deposit of post-dated cheques shall not serve as a reason for non-payment of monthly installments.</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>11. All payments to be made favouring <b>Centrum SME Loan.</b></td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>12. Appropriate Security, acceptable to <b>Centrum SME Loan.</b>, shall be created for disbursement.</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>13. You shall inform <b>Centrum SME Loan.</b> in writing about the change in address, job, business, profession as the case may be immediately after such change. You are prohibited from using the loan amount or any part thereof for any purpose other than for which it has been sanctioned.</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>14. <b>Centrum SME Loan.</b> does not accept any responsibility nor liability for any loss or damage arising from any service failures or disruptions (including but not limited to, loss of data) attributable to a systems or equipment failure or due to reliance by  <b>Centrum SME Loan.</b> third party products or interdependencies including but not limited to electricity or telecommunications and for any consequences arising out of interruption of its business by Acts of God, riots, civil commotion, insurrections, wars or  any other causes beyond its control or by any strikes or lockouts.</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>15. You acknowledge and agree that in the absence of manifest error, Centrum SME Loan’s records (including, without limitation, account balances, transaction details/facility limits) shall be conclusive proof of the matters to which they relate and you shall honor all your obligations to the satisfaction of <b>Centrum SME Loan.</b></td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>16. In the event of default, <b>Centrum SME Loan.</b> shall in its absolute discretion have the right to sell and dispose off the security/property to be mortgaged / charged or already mortgaged / charged with <b>Centrum SME Loan.</b> in such manner as prescribed by <b>Centrum SME Loan.</b> and agreed to by you to secure the loan/credit facilities on “As is where is” basis without seeking the intervention of court of law and/or applying for foreclosure of the mortgage and / or initiating any other legal action. <b>Centrum SME Loan.</b> shall be deemed to have been authorized by you, to sell or dispose off the security/property as your agent and attorney in the event of your default to repay the outstanding dues payable to CENTRUMSME <b>Centrum SME Loan.</b> as per terms and conditions of loan/credit facilities.</td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>17. <b>Centrum SME Loan.</b> may revoke in part or in full or withdraw / stop financial assistance at any stage without any notice or giving any reasons for any purpose whatsoever. Without prejudice to the aforesaid, this sanction shall stand revoked in the event of any material change in the proposal / application / facts on the basis of which loan has been sanctioned.</td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>18. A loan agreement as per terms prescribed by <b>Centrum SME Loan.</b> and other documents in relation thereof shall be executed by the Borrower. The terms in the loan Agreement shall supersede all pervious related communications in respect of the said finance Facility/loan.</td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>19. Part prepayment and Prepayment charges for - Loan Against Property:</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Foreclose terms & Conditions :</b></td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>i. No foreclose allowed in the first 6 (six) months fromthe date of full disbursement.</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>ii. Tenor served 0 to 48 month 5% foreclosing charges + GST.</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Cheque Dishonor Charges :</b></td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>1. Rs. 500/- + GST for per return/bounce of PDC / NACH or ECS.</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>2. 3% of total Installment delay payment charges per month.</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>3. Rs. 250/- + GST field collection charges for each visit of company officer for the collection purpose. </td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'>");
                sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='40%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'><b>For Centrum SME Loan .</b></td>");
                sb.AppendFormat(@"<td width='60%' align='center'  colspan='2'  style='font-family:Garamond; font-size:16px; line-height:16px;'>I/We accept the offer</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' colspan='3' style='font-family:Garamond; font-size:112px; line-height:112px; font-weight:bold;'><b>&nbsp;</b></td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='40%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Authorized Signatory</td>");
                sb.AppendFormat(@"<td width='30%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>{0}</td>", dt.Rows[0]["CompanyName"].ToString());
                sb.AppendFormat(@"<td width='30%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>{0}</td>", dt.Rows[0]["CoAppName"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='40%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'></td>");
                sb.AppendFormat(@"<td width='30%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>(Applicant)</td>");
                sb.AppendFormat(@"<td width='30%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>(Co-Applicant)</td>");
                sb.Append(@"</tr>");
                sb.Append(@"</table>");
                sb.Append(@"</td>");
                sb.Append(@"</tr>");

                sb.Append(@"</table>");
            }
            Literal1.Text = sb.ToString();
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