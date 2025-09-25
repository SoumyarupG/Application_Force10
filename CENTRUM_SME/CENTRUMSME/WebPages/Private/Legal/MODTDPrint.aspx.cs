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
    public partial class MODTDPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void LoadRecord()
        {
            try
            {
                DataSet ds = (DataSet)Session["MODTDPrint"];
                DataTable dt = new DataTable ();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                dt2 = ds.Tables[2];
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
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:22px; line-height:30px; font-weight:bold;text-decoration: underline;'>Notice of Intimation regarding Mortgage by way of Deposit of Title Deed</td>");
                    sb.Append(@"</tr>");
                    //sb.Append(@"<tr>");
                    //sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:22px; line-height:30px; font-weight:bold;'>(Without possession)</td>");
                    //sb.Append(@"</tr>");
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");
                    string AmtInWord = ConvertNumbertoWords(Convert.ToInt32(dt.Rows[0]["SanctionAmt"]));
                   
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>I/We, the undersigned parties, is/are by this notice of intimation, giving notice to the public atlarge that, the mortgagor herein had deposited the title deeds of the property for the security of the loan given/agreed to be given by the mortgagee herein.</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>1) Party Details:</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>a)	Mortgagee -	</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>CENTRUMSME FIN SERV PRIVATE LIMITED</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Address: Shastri Heritage, 4th Floor,</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Near TATA Motors Main Gate,</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Chinchwad, Pune-411033</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>b)	Mortgagor(s)</td>");
                    sb.Append(@"</tr>");
                    int x = 0;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        x = x + 1;
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'> <b> {0})  {1},  PAN No - {2} </td>", x.ToString(), dt1.Rows[i]["CustName"].ToString(), dt1.Rows[i]["PANNO"].ToString());
                        sb.Append(@"</tr>");
                    }
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>2) Property Location(s) and Property Details (with Attribute No. Area, Unit):-</b></td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>{0}</td>", dt.Rows[0]["Remarks"].ToString());
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>3) List of Documents deposited with CENTRUMSME Fin ServPvt.Ltd.: -</b></td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='1'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='10%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>SL. NO.</b></td>");
                    sb.AppendFormat(@"<td width='40%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>DATE</b></td>");
                    sb.AppendFormat(@"<td width='25%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>DOCUMENTS</b></td>");
                    sb.AppendFormat(@"<td width='25%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>DOCUMENT TYPE</b></td>");
                    sb.Append(@"</tr>");
                   
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='10%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>{0}</b></td>", dt2.Rows[i]["SLNo"].ToString());
                        sb.AppendFormat(@"<td width='40%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>{0}</b></td>", dt2.Rows[i]["Date"].ToString());
                        sb.AppendFormat(@"<td width='25%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>{0}</b></td>", dt2.Rows[i]["DocName"].ToString());
                        sb.AppendFormat(@"<td width='25%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>{0}</b></td>", dt2.Rows[i]["DocTypeName"].ToString());
                        sb.Append(@"</tr>");
                    }
                    sb.Append(@"</table>"); 
                    sb.Append(@"</td>"); 
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'><b>1.Loan Amount	:{0}</b></td>", dt.Rows[0]["SanctionAmt"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'><b>2.Rate of Interest	:{0} %</b></td>", dt.Rows[0]["RIntRate"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'><b>3.Date of Mortgage	: </b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'><b>4.Date of Notice	: </b></td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");


                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' >");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='25%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;border-right: solid 1px black; border-left: solid 1px black; border-top: solid 1px black;'><b>Name of Mortgagor& Borrower</b></td>");
                    sb.AppendFormat(@"<td width='25%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;border-right: solid 1px black; border-left: solid 1px black; border-top: solid 1px black;'><b>Party Photo (to be attested by mortgagee)</b></td>");
                    sb.AppendFormat(@"<td width='25%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;border-right: solid 1px black; border-left: solid 1px black; border-top: solid 1px black;'><b>Party Thumb Impression</b></td>");
                    sb.AppendFormat(@"<td width='25%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;border-right: solid 1px black; border-left: solid 1px black; border-top: solid 1px black;'><b>Signature (In case of Institution, sign and seal of Institution )</b></td>");
                    sb.Append(@"</tr>");

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        sb.Append(@"<tr>");
                      
                        if (dt1.Rows[i]["CustName"].ToString().Trim().Length <= 23)
                        {
                            sb.AppendFormat(@"<td width='25%' align='left' style='font-family:Garamond;  border-right: solid 1px black; border-left: solid 1px black; border-top: solid 1px black; font-size:14px;border-bottom: solid 1px black; line-height:128px; '><b>{0}</b></td>", dt1.Rows[i]["CustName"].ToString().Trim()); //
                        }
                        else
                            sb.AppendFormat(@"<td width='25%' align='left' style='font-family:Garamond;  border-right: solid 1px black; border-left: solid 1px black; border-top: solid 1px black;border-bottom: solid 1px black; font-size:14px; line-height:64px; '><b>{0}</b></td>", dt1.Rows[i]["CustName"].ToString().Trim()); //
                        sb.AppendFormat(@"<td width='25%' align='left' style='font-family:Garamond; border-right: solid 1px black; border-left: solid 1px black; border-top: solid 1px black;border-bottom: solid 1px black; font-size:14px; line-height:64px; '><b>{0}</b></td>", "");
                        sb.AppendFormat(@"<td width='25%' align='left' style='font-family:Garamond; border-right: solid 1px black; border-left: solid 1px black; border-top: solid 1px black;border-bottom: solid 1px black; font-size:14px; line-height:64px; '><b>{0}</b></td>", "");
                        sb.AppendFormat(@"<td width='25%' align='left' style='font-family:Garamond;border-right: solid 1px black; border-left: solid 1px black; border-top: solid 1px black;border-bottom: solid 1px black; font-size:14px; line-height:64px; '><b>{0}</b></td>", "");
                        sb.Append(@"</tr>");
                    }
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'>The information is verified and found correct:</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:64px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'>(Sign & Seal of the authorized the person of mortgagee)</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5'  >");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;border-top: solid 1px black;border-left: solid 1px black;border-right: solid 1px black;'><b>Payment Details :</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:48px;border-left: solid 1px black;border-right: solid 1px black;'>Stamp Duty of Rs. _______/- has been paid vide ................................................................... ......................................................</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:48px;border-bottom: solid 1px black;border-left: solid 1px black;border-right: solid 1px black;'>Dated : ............................</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:48px;border-left: solid 1px black;border-right: solid 1px black;'>Filling charges of Rs. 1000/- has been paid vide challan dated ............................. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:48px;border-bottom: solid 1px black;border-left: solid 1px black;border-right: solid 1px black;'>Document handling charges of Rs. 300/- has been paid vide..................dated........................ </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:32px; line-height:32px;'>&nbsp;</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'>(For office use only)</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='1' >");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='33%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>Name of Sub Registrar Office</b></td>");
                    sb.AppendFormat(@"<td width='33%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>Submission No.</b></td>");
                    sb.AppendFormat(@"<td width='34%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>Date of Submission</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='33%' align='left' style='font-family:Garamond; font-size:14px; line-height:64px;'><b>&nbsp;</b></td>");
                    sb.AppendFormat(@"<td width='33%' align='left' style='font-family:Garamond; font-size:14px; line-height:64px;'><b>&nbsp;</b></td>");
                    sb.AppendFormat(@"<td width='34%' align='left' style='font-family:Garamond; font-size:14px; line-height:64px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' colspan='3' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:48px;'>Filed On Serial No. ...................... On ...................Day of ..........................20.............. </td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:192px; line-height:192px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond,Geneva, sans-serif; font-size:16px; line-height:16px;'>Signature and Seal of Sub-Registrar</td>");
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