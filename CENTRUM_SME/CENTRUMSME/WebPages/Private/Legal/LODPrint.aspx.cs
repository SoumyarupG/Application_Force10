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
    public partial class LODPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void LoadRecord()
        {
            try
            {
                DataSet ds = (DataSet)Session["LODPrint"];
                DataTable dt = new DataTable ();
                DataTable dt1 = new DataTable();
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                StringBuilder sb = new StringBuilder();
                if (dt.Rows.Count > 0)
                {
                    sb.Append(@"<table width='1002px' cellspacing='10' cellpadding='0' border='0' style='margin:0 auto;'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:48px; line-height:48px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
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
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='1'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='30%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold;'><b>Name of Customer/Applicant</b></td>");
                    sb.AppendFormat(@"<td width='70%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["CustomerName"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='30%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold;'><b>Loan Agreement No</b></td>");
                    sb.AppendFormat(@"<td width='70%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["SanctionID"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='30%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold;'><b>Date</b></td>");
                    sb.AppendFormat(@"<td width='70%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["LODGenDate"].ToString());
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
                    sb.AppendFormat(@"<td width='40%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>DOCUMENTS</b></td>");
                    sb.AppendFormat(@"<td width='20%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>DOCUMENT NO</b></td>");
                    sb.AppendFormat(@"<td width='20%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>DOCUMENT TYPE</b></td>");
                    sb.Append(@"</tr>");

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='10%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>{0}</b></td>", dt1.Rows[i]["SLNo"].ToString());
                        sb.AppendFormat(@"<td width='10%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>{0}</b></td>", dt1.Rows[i]["Date"].ToString());
                        sb.AppendFormat(@"<td width='40%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>{0}</b></td>", dt1.Rows[i]["DocName"].ToString());
                        sb.AppendFormat(@"<td width='20%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>{0}</b></td>", dt1.Rows[i]["DocNo"].ToString());
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
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'>All the documents mentioned hereinabove are present in this document folder while verifying and sealing the folder. Hence we are signing hereunder to seal the same. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:140px; line-height:140px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='50%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>Mr. Tejas Bhende</b> </td>");
                    sb.AppendFormat(@"<td width='50%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>{0}</b> </td>", dt.Rows[0]["LealOfficerName"].ToString());
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