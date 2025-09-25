using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Agreement
{
    public partial class WelComeLetter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void LoadRecord()
        {
            DataTable dt = (DataTable)Session["WlcmLtr"];
            StringBuilder sb = new StringBuilder();

            if (dt.Rows.Count > 0)
            {
                sb.Append(@"<table width='1002px' cellspacing='10' cellpadding='0' border='0' style='margin:0 auto;'>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:18px; line-height:130px;'>&nbsp;</td>");
                sb.Append(@"</tr>");
                

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:18px; line-height:18px;'>&nbsp;</td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'>");
                sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:22px; line-height:30px; font-weight:bold; text-decoration: underline;'>WELCOME LETTER</td>");
                sb.Append(@"</tr>");
                sb.Append(@"</table>");
                sb.Append(@"</td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:18px; line-height:16px;'>&nbsp;</td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='right' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>DATE: </b>{0}</td>", dt.Rows[0]["DisbDate"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'>To,</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'>{0}</td>", dt.Rows[0]["CustomerName"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'>{0}</td>", dt.Rows[0]["ComAddress"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'>{0}</td>", dt.Rows[0]["ComAddress2"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'>{0}</td>", dt.Rows[0]["ComAddress3"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'>Mobile No:- {0}</td>", dt.Rows[0]["ContactNo"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:18px; line-height:16px;'>&nbsp;</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Loan Account No: </b>{0}</td>", dt.Rows[0]["LoanId"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Customer ID: </b>{0}</td>", dt.Rows[0]["CustId"].ToString());
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:18px; line-height:16px;'>&nbsp;</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='left' style='font-family:Garamond; font-size:18px; line-height:16px;'>Dear Sir/Madam,</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:18px; font-weight:bold;'>WELCOME TO Centrum Micro Credit PRIVATE LIMITED</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:18px; line-height:16px;'>&nbsp;</td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='left' style='font-family:Garamond; font-size:18px; line-height:16px;'>We thank you for choosing us to serve your financial needs.Centrum Micro Credit PRIVATE LIMITED is committed to providing products and services of the highest standards.We would like to assure you that we will strive our best to meet your financial needs.</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:18px; line-height:16px;'>&nbsp;</td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.Append(@"<td width='100%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'>");
                sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='1'>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='40%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Loan Amount (Rs.):</td>");
                sb.AppendFormat(@"<td width='60%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'>{0}</td>", dt.Rows[0]["DisbAmt"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='40%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Tenure (Months):</td>");
                sb.AppendFormat(@"<td width='60%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'>{0}</td>", dt.Rows[0]["Tenure"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='40%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>EMI (Rs.):</td>");
                sb.AppendFormat(@"<td width='60%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'>{0}</td>", dt.Rows[0]["EMIAmt"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='40%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>EMI Start Date:</td>");
                sb.AppendFormat(@"<td width='60%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'>{0}</td>", dt.Rows[0]["RepayStartDt"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='40%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>ROI (% Per Annum):</td>");
                sb.AppendFormat(@"<td width='60%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'>{0}</td>", dt.Rows[0]["IntRate"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='40%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Disbursement Date:</td>");
                sb.AppendFormat(@"<td width='60%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px;'>{0}</td>", dt.Rows[0]["DisbDate"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"</table>");
                sb.Append(@"</td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:18px; line-height:18px;'>We request you to quote the loan account number in all future correspondence with us.</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:18px; line-height:18px;'>As you are aware, we will present your monthly installment (EMI) Cheque/SI/ECS on the {0}<sup>th</sup> of every month. Please ensure that your bank account is adequately funded. This will avoid levy of additioal charges for non-receipt of payment. If you have missed any EMI and making payments to any of our representative or making payments towards closure and other charges of your loan either to our representative or at the branch offices, please ensure that you collect a receipt for your payments with Centrum Micro Credit PRIVATE LIMITED hologram on it. Any receipt without the hologram is invalid.</td>", dt.Rows[0]["SchdlDay"].ToString());
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:18px; line-height:18px;'>We value your relationship and assure your of best services always.</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:18px; line-height:18px;'>In case you have any queries, please feel free to contact us through any of the below channels. We shall be glad to be of assistance to you.</td>");
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>&nbsp; </td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='right' style='font-family:Garamond; font-size:16px; line-height:16px;'>Your Sincerely,</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:18px; line-height:16px;'>&nbsp;</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:18px; line-height:16px;'>&nbsp;</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='right' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Centrum Micro Credit PRIVATE LIMITED</b></td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='right' style='font-family:Garamond; font-size:12px; line-height:10px;'><b>Registered Office: </b>Centrum Micro Credit PRIVATE LIMITED</td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='right' style='font-family:Garamond; font-size:12px; line-height:10px;'>{0}</td>", CGblIdGenerator.GetBranchAddress1("0000"));
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='right' style='font-family:Garamond; font-size:12px; line-height:10px;'>{0}</td>", CGblIdGenerator.GetBranchAddress2("0000"));
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>&nbsp; </td>");
                sb.Append(@"</tr>");
                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='right' style='font-family:Garamond; font-size:12px; line-height:16px;'>Print Date : {0}</td>", dt.Rows[0]["PrintDate"].ToString());
                sb.Append(@"</tr>");

                sb.Append(@"<tr>");
                sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>&nbsp; </td>");
                sb.Append(@"</tr>");

                sb.Append(@"</table>");
            }
            Literal1.Text = sb.ToString();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "mywindow", "printform();", true);
        }
    }
}