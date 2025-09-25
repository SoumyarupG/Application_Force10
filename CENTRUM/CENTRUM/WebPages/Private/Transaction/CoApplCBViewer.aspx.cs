using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class CoApplCBViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String encURL = "";
            string pEnquiryId = "", pCBEnqDate = "", pMemberType = "";
            Int32 pCbAppId = 0;
            encURL = Request.RawUrl;
            encURL = encURL.Substring(encURL.IndexOf('?') + 1);
            if (!encURL.Equals(""))
            {
                if (encURL.Contains("&"))
                {
                    String[] queryStrParam1 = encURL.Split('&');
                    String[] queryStr1 = queryStrParam1[0].Split('=');
                    String[] queryStr2 = queryStrParam1[1].Split('=');
                    String[] queryStr3 = queryStrParam1[2].Split('=');
                    String[] queryStr4 = queryStrParam1[3].Split('=');

                    if (queryStr1.Length > 1)
                        pEnquiryId = queryStr1[1].ToString();
                    if (queryStr2.Length > 1)
                        pCbAppId = Convert.ToInt32(queryStr2[1]);
                    if (queryStr3.Length > 1)
                        pCBEnqDate = Convert.ToString(queryStr3[1]);
                    if (queryStr4.Length > 1)
                        pMemberType = Convert.ToString(queryStr4[1]).Trim() == "Applicant" ? "Member" : "CoAppMem";


                    DataSet ds = null;
                    DataTable dt1 = null, dt2 = null, dt3 = null;
                    CReports oRpt = null;
                    string vRptPath = "";
                    string vBranch = Session[gblValue.BrName].ToString();
                    try
                    {
                        //cvar = 1;
                        oRpt = new CReports();
                        string enqstatusmsg = "";
                        // ds = oRpt.Equifax_Report(pEnquiryId, pCbAppId, ref  enqstatusmsg);-CoAppMem
                        if (gblFuction.setDate(pCBEnqDate.Replace("_", "/")) <= gblFuction.setDate("01/07/2022"))
                        {
                            ds = oRpt.Equifax_Report(pEnquiryId, pCbAppId, ref  enqstatusmsg,Convert.ToInt32(Session[gblValue.UserId].ToString()));
                        }
                        else
                        {
                            ds = oRpt.Equifax_Report_CCR(pEnquiryId, pCbAppId, ref enqstatusmsg, pMemberType, 0, Convert.ToInt32(Session[gblValue.UserId].ToString()));
                        }
                        if (!String.IsNullOrEmpty(enqstatusmsg))
                        {
                            gblFuction.MsgPopup("Equifax Error : " + enqstatusmsg);
                            return;
                        }
                        else
                        {
                            if (ds.Tables.Count == 2 && ds != null)
                            {
                                if (ds.Tables[0].Rows.Count == 0 && ds.Tables[1].Rows.Count == 0)
                                {
                                    gblFuction.AjxMsgPopup("New User");
                                    return;
                                }
                            }
                        }
                        dt1 = ds.Tables[0];
                        dt2 = ds.Tables[1];
                        dt3 = ds.Tables[2];
                        //-----------------------------------------------------------------------
                        if (Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
                        {
                            foreach (DataRow dr in dt2.Rows) // search whole table
                            {
                                if (Convert.ToInt32(dr["IdentyPRofId"].ToString()) == 1)
                                {
                                    dr["VoterId"] = String.Format("{0}{1}", "********", Convert.ToString(dt2.Rows[0]["VoterId"]).Substring(Convert.ToString(dt2.Rows[0]["VoterId"]).Length - 4, 4)); ; //change the name                           
                                }
                                if (Convert.ToInt32(dr["AddProfId"].ToString()) == 1)
                                {
                                    dr["UID"] = String.Format("{0}{1}", "********", Convert.ToString(dt2.Rows[0]["UID"]).Substring(Convert.ToString(dt2.Rows[0]["UID"]).Length - 4, 4)); ; //change the name                           
                                }
                            }
                        }
                        //-------------------------------------------------------------------------
                        dt1.TableName = "CBPortDtl";
                        dt2.TableName = "CBPortMst";
                        dt3.TableName = "CBHistoryMonth";
                        if (gblFuction.setDate(pCBEnqDate) <= gblFuction.setDate("01/07/2022"))
                        {
                            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptMemberCredit_New.rpt";
                        }
                        else
                        {
                            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptMemberCredit_CCR.rpt";
                        }
                        ReportDocument rptDoc = new ReportDocument();
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(ds);

                        CrystalReportViewer1.ReportSource = rptDoc;
                    }

                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        dt1 = null;
                        dt2 = null;
                        oRpt = null;
                    }

                }
            }

        }
    }
}