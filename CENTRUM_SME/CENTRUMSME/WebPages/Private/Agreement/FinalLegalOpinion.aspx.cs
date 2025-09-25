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
    public partial class FinalLegalOpinion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadRecord();
        }
        private void LoadRecord()
        {
            try
            {
                DataSet ds = (DataSet)Session["FLO"];
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    dt3 = ds.Tables[2];
                }
                StringBuilder sb = new StringBuilder();
                if (dt1.Rows.Count > 0)
                {
                    sb.Append(@"<table width='1002px' cellspacing='10' cellpadding='0' border='0' style='margin:0 auto;'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:64px; line-height:64px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;font-weight:bold;text-decoration: underline;'>Legal Opinion {0}</td>", dt1.Rows[0]["CustomerName"]);
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'>Appl ID : {0}</td>", dt1.Rows[0]["LoanAppId"]);
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;font-weight:bold;text-decoration: underline;'>Schedule of the Property</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>{0}</b></td>", dt1.Rows[0]["Remarks"]);
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='center' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>North by: {0}</td>", dt1.Rows[0]["NBoundary"]);
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>South by: {0}</td>", dt1.Rows[0]["SBoundary"]);
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>East by: {0}</td>", dt1.Rows[0]["EBoundary"]);
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>West by: {0}</td>", dt1.Rows[0]["WBoundary"]);
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>Situated at within the Sub-Registration Office of............................. and Registration District of.........................</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>&nbsp;</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Perused photocopies of the following documents:</b></td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='0' border='1'>");
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='10%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>SL.NO</b></td>");
                    sb.Append(@"<td width='20%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>DATE</b></td>");
                    sb.Append(@"<td width='50%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>DOCUMENTS</b></td>");
                    sb.Append(@"<td width='20%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>DOC. NO.</b></td>");
                    sb.Append(@"</tr>");
                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='10%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>{0}</b></td>", dt2.Rows[i]["SLNo"].ToString().Trim());
                            sb.AppendFormat(@"<td width='20%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>{0}</td>", dt2.Rows[i]["DocDate"].ToString().Trim());
                            sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>{0}</td>", dt2.Rows[i]["DocName"].ToString().Trim());
                            sb.AppendFormat(@"<td width='20%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>{0}</td>", dt2.Rows[i]["DocNo"].ToString().Trim());
                            sb.Append(@"</tr>");
                        }
                    }
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;font-weight:bold;text-decoration: underline;'>TITTLE PASSING HISTORY FOR 13 YEARS </td>");
                    sb.Append(@"</tr>");

                    int x = 0;
                    if (string.IsNullOrEmpty(dt1.Rows[0]["PassHis1"].ToString()) == false)
                    {
                        x = x + 1;
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '><b>{0}. {1}</b></td>", x.ToString(), dt1.Rows[0]["PassHis1"].ToString());
                        sb.Append(@"</tr>");
                    }
                    else
                    {
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '><b>&nbsp;</b></td>");
                        sb.Append(@"</tr>");
                    }
                    if (string.IsNullOrEmpty(dt1.Rows[0]["PassHis2"].ToString()) == false)
                    {
                        x = x + 1;
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '><b>{0}. {1}</b></td>", x.ToString(), dt1.Rows[0]["PassHis2"].ToString());
                        sb.Append(@"</tr>");
                    }
                    else
                    {
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '><b>&nbsp;</b></td>");
                        sb.Append(@"</tr>");
                    }
                    if (string.IsNullOrEmpty(dt1.Rows[0]["PassHis3"].ToString()) == false)
                    {
                        x = x + 1;
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '><b>{0}. {1}</b></td>", x.ToString(), dt1.Rows[0]["PassHis3"].ToString());
                        sb.Append(@"</tr>");
                    }
                    else
                    {
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '><b>&nbsp;</b></td>");
                        sb.Append(@"</tr>");
                    }
                    if (string.IsNullOrEmpty(dt1.Rows[0]["PassHis4"].ToString()) == false)
                    {
                        x = x + 1;
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '><b>{0}. {1}</b></td>", x.ToString(), dt1.Rows[0]["PassHis4"].ToString());
                        sb.Append(@"</tr>");
                    }
                    else
                    {
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '><b>&nbsp;</b></td>");
                        sb.Append(@"</tr>");
                    }
                    if (string.IsNullOrEmpty(dt1.Rows[0]["PassHis5"].ToString()) == false)
                    {
                        x = x + 1;
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '>{0}. {1}</b></td>", x.ToString(), dt1.Rows[0]["PassHis5"].ToString());
                        sb.Append(@"</tr>");
                    }
                    else
                    {
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '><b>&nbsp;</b></td>");
                        sb.Append(@"</tr>");
                    }


                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>{0}</td>", dt1.Rows[0]["LegalReport"]);
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Encumbrance Certificate / Search Report:</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>The above said Encumbrance does not disclose any subsisting encumbrance over the Schedule Property.</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>The Nil Encumbrance Certificate/ Affidavit is required to be furnished.</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>The veracity of the original documents and photocopies of the documents also verified for the Scheduled mentioned property.</b></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>CONCLUSION:</b></td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>From the records and documents produced before us and information furnished to us, we are of the opinion that {0} the present owner of the Schedule Property has got a good, validand marketable title over it and he is competent to deal with the Schedule Property thereof the documentsmentioned above same may be considered and taken as a security for the purpose of grant of loan andproduction and deposit of the documents to this Opinion.</td>", dt1.Rows[0]["PropertyOwnerNm"].ToString());
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'><b>Note:</b></td>");
                    sb.Append(@"</tr>");

                    int y = 0;
                    if (string.IsNullOrEmpty(dt1.Rows[0]["LegalNote1"].ToString()) == false)
                    {
                        y = y + 1;
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '><b>{0}. {1}</b></td>", y.ToString(), dt1.Rows[0]["LegalNote1"].ToString());
                        sb.Append(@"</tr>");
                    }
                    else
                    {
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '><b>&nbsp;</b></td>");
                        sb.Append(@"</tr>");
                    }
                    if (string.IsNullOrEmpty(dt1.Rows[0]["LegalNote2"].ToString()) == false)
                    {
                        y = y + 1;
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '><b>{0}. {1}</b></td>", y.ToString(), dt1.Rows[0]["LegalNote2"].ToString());
                        sb.Append(@"</tr>");
                    }
                    else
                    {
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '><b>&nbsp;</b></td>");
                        sb.Append(@"</tr>");
                    }
                    if (string.IsNullOrEmpty(dt1.Rows[0]["LegalNote3"].ToString()) == false)
                    {
                        y = y + 1;
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '><b>{0}. {1}</b></td>", y.ToString(), dt1.Rows[0]["LegalNote3"].ToString());
                        sb.Append(@"</tr>");
                    }
                    else
                    {
                        sb.Append(@"<tr>");
                        sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '><b>&nbsp;</b></td>");
                        sb.Append(@"</tr>");
                    }

                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;font-weight:bold;text-decoration: underline;'>THE FOLLOWING DOCUMENT SHOULD COLLECTED FROM THE BORROWER AT THE TIME OF DISBURESMENT</td>");
                    sb.Append(@"</tr>");

                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='0' border='1'>");
                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='10%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>SL.NO</b></td>");
                    sb.Append(@"<td width='10%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>DATE</b></td>");
                    sb.Append(@"<td width='40%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>DOCUMENTS</b></td>");
                    sb.Append(@"<td width='20%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>DOC. NO.</b></td>");
                    sb.Append(@"<td width='20%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px; '><b>DOCUMENT TYPE</b></td>");
                    sb.Append(@"</tr>");
                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            sb.Append(@"<tr>");
                            sb.AppendFormat(@"<td width='10%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>{0}</b></td>", dt2.Rows[i]["SLNo"].ToString().Trim());
                            sb.AppendFormat(@"<td width='10%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>{0}</td>", dt2.Rows[i]["DocDate"].ToString().Trim());
                            sb.AppendFormat(@"<td width='40%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>{0}</td>", dt2.Rows[i]["DocName"].ToString().Trim());
                            sb.AppendFormat(@"<td width='20%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>{0}</td>", dt2.Rows[i]["DocNo"].ToString().Trim());
                            sb.AppendFormat(@"<td width='20%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px; '>{0}</td>", dt2.Rows[i]["DocTypeName"].ToString().Trim());
                            sb.Append(@"</tr>");
                        }
                    }
                    sb.Append(@"</table>");
                    sb.Append(@"</td>");
                    sb.Append(@"</tr>");


                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>Legal Comments:</td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='100%' align='justify' style='font-family:Garamond; font-size:16px; line-height:16px;'>{0}</td>", dt1.Rows[0]["FinalLegComments"].ToString());
                    sb.Append(@"</tr>");


                    sb.Append(@"<tr>");
                    sb.Append(@"<td width='100%' align='center' style='font-family:Garamond; font-size:14px; line-height:16px;'>");
                    sb.Append(@"<table width='100%' cellspacing='0' cellpadding='5' border='0'>");
                    sb.Append(@"<tr>");
                    sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '>Date : {0} </td>", dt1.Rows[0]["FinalLegApproveDt"].ToString());
                    if (dt3.Rows.Count > 0)
                        sb.AppendFormat(@"<td width='50%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold;'>{0}</td>", dt3.Rows[0]["UserName"].ToString());
                    else
                        sb.AppendFormat(@"<td width='50%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold;'></td>");
                    sb.Append(@"</tr>");
                    sb.Append(@"<tr>");
                    if (dt3.Rows.Count > 0)
                    {
                        sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '></td>");
                        sb.AppendFormat(@"<td width='50%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold;'>({0})</td>", dt3.Rows[0]["Designation"].ToString());
                    }
                    else
                    {
                        sb.AppendFormat(@"<td width='50%' align='justify' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold; '></td>");
                        sb.AppendFormat(@"<td width='50%' align='right' style='font-family:Garamond; font-size:14px; line-height:16px;font-weight:bold;'></td>");
                    }
                        
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
        //public override void VerifyRenderingInServerForm(Control control)
        //{

        //}
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "mywindow", "printform();", true);
        }
    }
}