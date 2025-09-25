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
    public partial class RepaySchedule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadRecord();
        }
        private void LoadRecord()
        {
            try
            {
                DataSet ds = (DataSet)Session["RSchedule"];
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                string BankName = "";
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    dt2 = ds.Tables[2];
                }
                if (dt.Rows.Count > 0)
                {
                    BankName = dt.Rows[0]["BankName"].ToString();
                }
                StringBuilder sb = new StringBuilder();
                if (dt1.Rows.Count > 0)
                {
                    sb.Append(@"<table width='1002px' cellspacing='10' cellpadding='0' border='0' style='margin:0 auto;'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:18px; line-height:18px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                  
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:12px; line-height:16px;'>");
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
                    sb.AppendFormat(@"<td width='70%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", (CGblIdGenerator.GetBranchAddress2("0000") + " , CIN No-"));
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:18px; line-height:18px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:22px; line-height:30px; font-weight:bold; text-decoration: underline;'>REPAYMENT SCHEDULE</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='15%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Applicant : </td>");
                    sb.AppendFormat(@"<td width='35%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>{0} </td>", dt.Rows[0]["CompanyName"].ToString());
                    sb.AppendFormat(@"<td width='15%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Co-Borrower : </td>");
                    sb.AppendFormat(@"<td width='35%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>{0} </td>", dt.Rows[0]["CoAppName"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='15%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Tenure : </td>");
                    sb.AppendFormat(@"<td width='35%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>{0} {1} </td>", dt.Rows[0]["Tenure"].ToString(), dt.Rows[0]["Repay Mode"].ToString());
                    sb.AppendFormat(@"<td width='15%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Application No : </td>");
                    sb.AppendFormat(@"<td width='35%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>{0} </td>", dt.Rows[0]["ApplicationNo"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='15%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>EMI Amount : </td>");
                    sb.AppendFormat(@"<td width='35%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Rs. {0}</td>", dt.Rows[0]["EMIAmt"].ToString());
                    sb.AppendFormat(@"<td width='15%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Loan Amount : </td>");
                    sb.AppendFormat(@"<td width='35%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Rs. {0}</td>", dt.Rows[0]["LoanAmt"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='15%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>First EMI Date : </td>");
                    sb.AppendFormat(@"<td width='35%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'> {0}</td>", dt.Rows[0]["FirstEMIDate"].ToString());
                    sb.AppendFormat(@"<td width='15%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>Last EMI Date : </td>");
                    sb.AppendFormat(@"<td width='35%' align='left' style='font-family:Garamond; font-size:16px; line-height:16px; font-weight:bold;'>{0} </td>", dt.Rows[0]["LastEMIDate"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='1'>");
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='10%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Installment No.</b></td>");
                    sb.Append(@"<td width='20%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Due Date</b></td>");
                    sb.Append(@"<td width='16%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Installment Amt</b></td>");
                    sb.Append(@"<td width='16%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Interest  Repaid</b></td>");
                    sb.Append(@"<td width='16%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Principal Repaid</b></td>");
                    sb.Append(@"<td width='16%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Principal Balance</b></td>");
                    sb.Append(@"</tr>");
                    if (dt1.Rows.Count > 0)
                        if (dt2.Rows[0]["AdvEMIRcvYN"].ToString() == "Y")
                        {
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                sb.Append(@"<tr>");
                                sb.AppendFormat(@"<td width='15%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt1.Rows[i]["InstNo"].ToString());
                                sb.AppendFormat(@"<td width='15%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", Convert.ToDateTime(dt1.Rows[i]["DueDt"]).ToString("dd/MM/yyyy"));
                                sb.AppendFormat(@"<td width='16%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt1.Rows[i]["ResAmt"].ToString());
                                sb.AppendFormat(@"<td width='16%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt1.Rows[i]["InstAmt"].ToString());
                                sb.AppendFormat(@"<td width='16%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt1.Rows[i]["PrinceAmt"].ToString());
                                sb.AppendFormat(@"<td width='16%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt1.Rows[i]["Outstanding"].ToString());
                                sb.Append(@"</tr>");
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                sb.Append(@"<tr>");
                                sb.AppendFormat(@"<td width='15%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt1.Rows[i]["InstNo"].ToString());
                                sb.AppendFormat(@"<td width='15%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", Convert.ToDateTime(dt1.Rows[i]["DefaultDate"]).ToString("dd/MM/yyyy"));
                                sb.AppendFormat(@"<td width='16%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt1.Rows[i]["ResAmt"].ToString());
                                sb.AppendFormat(@"<td width='16%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt1.Rows[i]["InstAmt"].ToString());
                                sb.AppendFormat(@"<td width='16%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt1.Rows[i]["PrinceAmt"].ToString());
                                sb.AppendFormat(@"<td width='16%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt1.Rows[i]["Outstanding"].ToString());
                                sb.Append(@"</tr>");
                            }
                        }
                       
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px;line-height:16px;'>&nbsp; </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px;line-height:16px;'>&nbsp; </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px;line-height:16px;'>Accepted </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px;line-height:16px;'>&nbsp; </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px;line-height:16px;'>&nbsp; </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px;line-height:16px;'><b>{0} </b></td>", dt.Rows[0]["CompanyName"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px;line-height:16px;'><b>{0}</b></td>", dt.Rows[0]["ComAddress"].ToString());
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
    }
}