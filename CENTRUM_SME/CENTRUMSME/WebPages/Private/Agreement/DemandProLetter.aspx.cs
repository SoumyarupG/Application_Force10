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
    public partial class DemandProLetter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadRecord();
        }
        private void LoadRecord()
        {
            try
            {
                DataTable dt = (DataTable)Session["SLDt"];
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
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:22px; line-height:30px; font-weight:bold; text-decoration: underline;'>DEMAND PROMISSORY NOTE</td>");
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
                    sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>Rs <b>{0}</b> /-</td>", dt.Rows[0]["SanctionAmt"]);
                    sb.AppendFormat(@"<td width='40%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px; '></td>");
                    sb.AppendFormat(@"<td width='10%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; '></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>&nbsp;</td>");
                    sb.AppendFormat(@"<td width='40%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px; '>Date :</td>");
                    sb.AppendFormat(@"<td width='10%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>{0}</b></td>", dt.Rows[0]["AgrmntDate"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>&nbsp;</td>");
                    sb.AppendFormat(@"<td width='40%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px; '>Place :</td>");
                    sb.AppendFormat(@"<td width='10%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>{0}</b></td>","Pune");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>On demand I/We <b>{0}</b> havingour registered office at <b>{1}</b> jointly and severally promise to <b> pay Centrum Micro Credit,</b> Pune a sum of Rs. <b>{2}</b>/- ( <b>{3}</b>) together with interest thereon @  <b>{4}</b> %  per annum (with <b>{5}</b>) or at such other rate as Centrum Micro Credit may fix from time to time.Presentment for payment and protest of this Note are hereby unconditionally and irrevocably waived.</td>", dt.Rows[0]["CompanyName"], dt.Rows[0]["ComAddress"], dt.Rows[0]["SanctionAmt"], word, dt.Rows[0]["FIntRate"], dt.Rows[0]["RepayType"]);
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>For ________________</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Name </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Designation </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Date : <b>{0}</b></td>", dt.Rows[0]["AgrmntDate"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");

                    sb.Append(@"<td width='40%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='30%' height='80px' cellspacing='0' cellpadding='5' border='1'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; '> Rs.1/- Revenue Stamp.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");

                    sb.Append(@"<td width='20%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='100%' height='80px' cellspacing='0' cellpadding='5'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '> &nbsp</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");

                    sb.Append(@"<td width='40%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='30%' height='80px' cellspacing='0' cellpadding='5' border='1'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>Rs.1/- Revenue Stamp.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");

                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='40%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='30%'  cellspacing='0' cellpadding='5' >");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; '> (Borrower)</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"<td width='20%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='100%' height='80px' cellspacing='0' cellpadding='5'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '> &nbsp</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"<td width='40%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='30%'  cellspacing='0' cellpadding='5' >");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>(Co- Borrower)</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");
                   
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");

                    sb.Append(@"<td width='40%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='30%' height='80px' cellspacing='0' cellpadding='5' border='1'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; '> Rs.1/- Revenue Stamp.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"<td width='20%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='100%' height='80px' cellspacing='0' cellpadding='5'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '> &nbsp</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"<td width='40%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; '>");
                    sb.Append(@"<table width='30%' height='80px' cellspacing='0' cellpadding='5' border='1'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>Rs.1/- Revenue Stamp.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");

                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>For Centrum Micro Credit	</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>Authorised Signatory</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
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
        //public override void VerifyRenderingInServerForm(Control control)
        //{

        //}
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "mywindow", "printform();", true);
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
    }
}