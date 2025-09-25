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
    public partial class GuaranteeBond : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadRecord();
        }
        private void LoadRecord()
        {
            try
            {
                DataSet ds = (DataSet)Session["GBDt"];
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                StringBuilder sb = new StringBuilder();
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                }
                // For eaqch guarantor separate report will be generated........
                if (dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            sb.Append(@"<table width='1002px' cellspacing='10' cellpadding='0' border='0' style='margin:0 auto;'>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                            sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:22px; line-height:30px; font-weight:bold; text-decoration: underline;'>GUARANTEE BOND</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"</table>");
                            sb.Append(@"</td>");
                            sb.Append(@"</tr>");

                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                            sb.Append(@"</tr>");

                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px;line-height:16px;'>In consideration of Centrum Micro Credit, having its registered office at <b>{0}{1} (hereinafter referred to as'Centrum Micro Credit')</b></td>", CGblIdGenerator.GetBranchAddress1("0000"), CGblIdGenerator.GetBranchAddress2("0000"));
                            sb.Append(@"</tr>");

                            sb.Append(@"<tr>");
                            sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                            sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='20%' align='justify' style='font-family:Garamond; font-size:16px; line-height:14px; '>Granting a Loan of Rs.</td>");
                            sb.AppendFormat(@"<td width='70%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; font-weight:bold; '>{0} ,</td>", dt.Rows[0]["SanctionAmt"].ToString());
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='2%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>To </td>");
                            sb.AppendFormat(@"<td width='98%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; font-weight:bold; text-decoration: underline;'></td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='2%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>M/s. </td>");
                            sb.AppendFormat(@"<td width='98%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; font-weight:bold; text-decoration: underline;'>{0}</td>", dt.Rows[0]["CompanyName"].ToString());
                            sb.Append(@"</tr>");
                            sb.Append(@"</table>");
                            sb.Append(@"</td>");
                            sb.Append(@"</tr>");

                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>vide Business loan agreement no <b>{0}</b> dated <b>{1}</b> hereinafter referred to as theSaid ('LOAN AGREEMENT') on the terms and conditions therein mentionedand on the terms and conditions as shall be mutually agreed between CENTRUMSME and the Borrower without</td>", dt.Rows[0]["SanctionID"].ToString(), dt.Rows[0]["AgrmntDate"].ToString());
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Reference to me / us, me /We Mr <b>{0}</b></td>", dt1.Rows[i]["CoAppName"].ToString());
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Mr <b>{0}</b></td>", dt1.Rows[i]["CoAppName"].ToString());
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>Age: {0},  Occupation: {1},  Residing at {2}</b></td>", dt1.Rows[i]["Age"].ToString(), dt1.Rows[i]["Occupation"].ToString(), dt1.Rows[i]["CoAppAddress"].ToString());
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Hereinafter referred to as the <b>'GUARANTOR/S'</b> (which expression shall include each one of us and ourrespective heirs, executor/s. administrator/s) jointly and severally hereby agree with and guarantee to youthe due and punctual payment by the BORROWER of all and every sum payable by the Borrower underthe said Loan Agreement</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>I/we agree that this Guarantee shill not be avoided, released or affected by CENTRUMSME giving time to the</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Borrower for the repayment of any sums or granting the Borrower any indulgence or CENTRUMSME and the Borrowermaking any variations in the terms of the Loan Agreement. I/We further agree that this guaranteeshall not be avoided, released or affected by CENTRUMSME giving time to the Borrower either before or after anytermination of the said Agreement and whether before or subsequent to CENTRUMSME exercising its right to repossessthe Hypothecated assets.</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Though as between the Borrower and ourself/ yes, I/We am/are surety only, I/We agree that in between CENTRUMSME and myself/ourselves I/We am/are the principal debtor jointly with the Borrower and accordingly I/Weunderstand that I/We am/are not entitled to any rights conferred as sureties under the relevant provisionsof the Contract Act, 1872.</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>All the rights and powers of CENTRUMSME shall remain in full force notwithstanding any neglect or forbearance ofdelays in the enforcement of any ofthe terms of the agreement with the Borrower. I/We further agree that CENTRUMSME shall be at liberty to sue the Borrower in the first instance if CENTRUMSME so desires to bind myself /ourselves to pay at PUNE to CENTRUMSME any amount due on any judgment, that CENTRUMSME may obtain against theBorrower or against me/us with the costs thereon.</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>This guarantee shall be construed as a continuing Guarantee notwithstanding the deaths of any one or more of us and shall be binding on the representative and estates of the deceased Guarantor or Guarantors.It is agreed that duly in the event of the Borrower complying with all the terms of the Loan Agreementand repaying all the amounts due, together with interest, to the Company, i/We shall be entitled toobtain on demand from CENTRUMSME a complete discharge of all my/our obligations under the terms and conditionsof this Guarantee.</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Should the Borrower be a limited Company, corporate or un-incorporated body, committee, firm, partnership,trustees or debtors on a joint account the provisions hereinbefore contained shall be construed andtake effect where necessary as if words importing the singular number included also the plural number.This guarantee shall remain effective notwithstanding any death, retirement, change, accession or additionas fully as if the person/s constituting or trading or acting as such body, committee, firm, partnership,trustee/s or debtor's on a joint account at the date of the Borrower's default at any time previously was orwere the same as the date hereof,</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>In the event of there being more than one guarantor the liability of remaining guarantors shall not beaffected or released or given up by time or other indulgence to one or more of the guarantors nor by thedeath of any one or more of the guarantors.</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>Name of the Guarantor(s) Signature</b></td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'><b>1)	Mr. {0}</b></td>", dt1.Rows[i]["CoAppName"]);
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>&nbsp;</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>In the presence of:</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>1)Name  <b>{0}</b></td>", "");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Address: </td>");
                            sb.Append(@"</tr>");

                            sb.Append(@"<tr>");
                            sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                            sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; '>&nbsp;</td>");
                            sb.AppendFormat(@"<td width='50%' align='left' style='font-family:Garamond; font-size:16px; line-height:14px; '>&nbsp;</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>&nbsp;</td>");
                            sb.AppendFormat(@"<td width='50%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px;'>Signature</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"</table>");
                            sb.Append(@"</td>");
                            sb.Append(@"</tr>");

                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>2)Name  <b>{0}</b></td>", "");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>Address: </td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;text-decoration: underline;'><b>{0}</b></td>", "");
                            sb.Append(@"</tr>");

                            sb.Append(@"<tr>");
                            sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                            sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; '>&nbsp;</td>");
                            sb.AppendFormat(@"<td width='50%' align='left' style='font-family:Garamond; font-size:16px; line-height:14px; '>&nbsp;</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>&nbsp;</td>");
                            sb.AppendFormat(@"<td width='50%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px;'>Signature</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"</table>");
                            sb.Append(@"</td>");
                            sb.Append(@"</tr>");

                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>DATE : <b>{0}</b></td>", dt.Rows[0]["AgrmntDate"].ToString());
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>PLACE : <b>{0}</b></td>", "Pune");
                            sb.Append(@"</tr>");

                            sb.Append(@"<tr>");
                            sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                            sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; '>&nbsp;</td>");
                            sb.AppendFormat(@"<td width='50%' align='left' style='font-family:Garamond; font-size:16px; line-height:14px; '>&nbsp;</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>Centrum Micro Credit</td>");
                            sb.AppendFormat(@"<td width='50%' align='left' style='font-family:Garamond; font-size:14px; line-height:16px; font-weight:bold; text-decoration: underline;'></td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; '>&nbsp;</td>");
                            sb.AppendFormat(@"<td width='50%' align='left' style='font-family:Garamond; font-size:16px; line-height:14px; '>&nbsp;</td>");
                            sb.Append(@"</tr>");
                            //sb.Append(@"<tr>");
                            //sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; '>&nbsp;</td>");
                            //sb.AppendFormat(@"<td width='50%' align='left' style='font-family:Garamond; font-size:16px; line-height:14px; '>&nbsp;</td>");
                            //sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>Authorised Signatory</td>");
                            sb.AppendFormat(@"<td width='50%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px;'>Guarantor</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; '>&nbsp;</td>");
                            sb.AppendFormat(@"<td width='50%' align='left' style='font-family:Garamond; font-size:16px; line-height:14px; '>&nbsp;</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; '>&nbsp;</td>");
                            sb.AppendFormat(@"<td width='50%' align='left' style='font-family:Garamond; font-size:16px; line-height:14px; '>&nbsp;</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; '>&nbsp;</td>");
                            sb.AppendFormat(@"<td width='50%' align='left' style='font-family:Garamond; font-size:16px; line-height:14px; '>&nbsp;</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px; '>&nbsp;</td>");
                            sb.AppendFormat(@"<td width='50%' align='left' style='font-family:Garamond; font-size:16px; line-height:14px; '>&nbsp;</td>");
                            sb.Append(@"</tr>");
                            sb.Append(@"</table>");
                            sb.Append(@"</td>");
                            sb.Append(@"</tr>");

                            sb.Append(@"</table>");
                        }
                    }
                    Literal1.Text = sb.ToString();
                }

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