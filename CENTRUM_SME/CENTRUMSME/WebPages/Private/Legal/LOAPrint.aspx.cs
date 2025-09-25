using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using CENTRUMCA;


namespace CENTRUMSME.WebPages.Private.Legal
{
    public partial class LOAPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void LoadRecord()
        {
            try
            {
                DataSet ds = (DataSet)Session["LOAPrint"];
                DataTable dt = new DataTable ();
                DataTable dt1 = new DataTable();
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                StringBuilder sb = new StringBuilder();
                if (dt.Rows.Count > 0)
                {
                    sb.Append(@"<table width='1002px' cellspacing='10' cellpadding='0' border='0' style='margin:0 auto;'>");
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:12px; line-height:14px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='0' border='0'>");
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='10%' align='left' rowspan='3' style='font-family:Garamond; font-size:16px; line-height:16px;'><img src='../../../images/Centrum_Logo.png' height='50px' width='200px' alt='Company Logo' /></td>");
                    sb.AppendFormat(@"<td width='80%' align='center' style='font-family:Garamond; font-size:30px; line-height:30px;'><b>{0}</b></td>", gblValue.CompName.ToString());
                    sb.Append(@"<td width='10%' align='left' rowspan='3' style='font-family:Garamond; font-size:12px; line-height:16px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='80%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", gblValue.Address1);
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='80%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", gblValue.Address2);
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");


                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:128px; line-height:128px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:22px; line-height:30px; font-weight:bold;text-decoration: underline;'>DOCUMENTS ACKNOWLEDGEMENT LETTER</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");


                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='20%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold;'>Date : </td>");
                    sb.AppendFormat(@"<td width='80%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;'>{0}</td>", dt.Rows[0]["LOAGenDate"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='20%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold;'>Applicant Name : </td>");
                    sb.AppendFormat(@"<td width='80%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;'>{0}</td>", dt.Rows[0]["CustomerName"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='20%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold;'>Co-Applicant : </td>");
                    sb.AppendFormat(@"<td width='80%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;'>{0}</td>", dt.Rows[0]["CoAppName"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='20%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold;'>Address : </td>");
                    sb.AppendFormat(@"<td width='80%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;'>{0}</td>", dt.Rows[0]["CustomerAddress"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='20%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold;'>Branch : </td>");
                    sb.AppendFormat(@"<td width='80%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;'>{0}</td>", dt.Rows[0]["CustBranch"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='20%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold;'>Loan Application ID : </td>");
                    sb.AppendFormat(@"<td width='80%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;'>{0}</td>", dt.Rows[0]["LoanAppId"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:32px; line-height:32px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='1'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='10%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>SL. NO.</b></td>");
                    sb.AppendFormat(@"<td width='10%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>DATE</b></td>");
                    sb.AppendFormat(@"<td width='60%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>DOCUMENTS</b></td>");
                    sb.AppendFormat(@"<td width='20%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>DOCUMENT TYPE</b></td>");
                    sb.Append(@"</tr>");

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='10%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>{0}</b></td>", dt1.Rows[i]["SLNo"].ToString());
                        sb.AppendFormat(@"<td width='10%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>{0}</b></td>", dt1.Rows[i]["Date"].ToString());
                        sb.AppendFormat(@"<td width='60%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>{0}</b></td>", dt1.Rows[i]["DocName"].ToString());
                        sb.AppendFormat(@"<td width='20%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>{0}</b></td>", dt1.Rows[i]["DocTypeName"].ToString());
                        sb.Append(@"</tr>");
                    }
                    sb.Append(@"</table>"); 
                    sb.Append(@"</td>"); 
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'>I have Handed over the above said documents and Acknowledge the Receipt of the same.Also I have received the welcome letter, copy of Loan Agreement.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:80px; line-height:80px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='50%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>&nbsp;</b> </td>");
                    sb.AppendFormat(@"<td width='50%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>{0}</td>", dt.Rows[0]["LealOfficerName"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='50%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>Customer Signature </td>");
                    sb.AppendFormat(@"<td width='50%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>(Legal Officer)</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='50%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                    sb.AppendFormat(@"<td width='50%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>Employee ID </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");


                    sb.Append(@"</table>");
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
       
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "mywindow", "printform();", true);
        }
    }
}