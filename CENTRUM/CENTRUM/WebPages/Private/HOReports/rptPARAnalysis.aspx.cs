using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using FORCEBA;
using FORCECA;
using System.IO;
using System.Web.UI;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class rptPARAnalysis : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtAsOnDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                ceFDt.EndDate = gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate])).AddDays(-1);
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Portfolio Analysis Report";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuParAnalysis);
                if (this.UserID == 1) return;
                if (this.CanView == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Portfolio Analysis Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
        }

        private void SetParameterForRptData(string pMode)
        {
            DataSet Ds = null;
            DataTable dt = null, dt1 = null, dt2 = null, dt3 = null, dt4 = null, dt5 = null;
            CReports oRpt = new CReports();
            string vFileNm = "attachment;filename=Portfolio_Analysis_Report_" + DateTime.Now + ".xls";
            try
            {
                oRpt = new CReports();
                Ds = oRpt.rptPARAnalysis(gblFuction.setDate(txtAsOnDt.Text));
                dt = Ds.Tables[0];
                dt1 = Ds.Tables[1];
                dt2 = Ds.Tables[2];
                dt3 = Ds.Tables[3];
                dt4 = Ds.Tables[4];
                dt5 = Ds.Tables[5];
                if (dt.Rows.Count > 0)
                {
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);

                    htw.WriteLine("<table border='1' cellpadding='5' widht='100%'>");
                    htw.WriteLine("<tr><td align='center' colspan='7'><b><font size='5'>" + dt.Rows[0]["Particulars"].ToString() + "</font></b></td></tr>");
                    htw.WriteLine("<tr><td rowspan='2' align='center'>Product</td><td colspan='2' align='center'>" + dt.Rows[0]["Header1"].ToString() + "</td><td colspan='2' align='center'>" + dt.Rows[0]["Header2"].ToString() + "</td><td colspan='2' align='center'>+/-</td></tr>");
                    htw.WriteLine("<tr><td align='center'>PAR POS Cr</td><td align='center'>PAR Count</td><td align='center'>PAR POS Cr</td><td align='center'>PAR Count</td><td align='center'>PAR POS Cr</td><td align='center'>PAR Count</td></tr>");

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            //if (dr["SlNo"].ToString() != "0")
                            htw.WriteLine(string.Format("<tr><td align='center'>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>", dr["Product"].ToString(), dr["PARPrOS_Prev"].ToString(), dr["PARLoanCnt_Prev"].ToString(), dr["PARPrOS_Present"].ToString(), dr["PARLoanCnt_Present"].ToString(), dr["DiffPos"].ToString() , dr["DiffCnt"].ToString()));
                            
                        }
                    }
                    htw.WriteLine("<tr><td colspan='7'>&nbsp;</td></tr>");
                    htw.WriteLine("<tr><td colspan='7'>&nbsp;</td></tr>");
                    htw.WriteLine("<tr><td colspan='7'>&nbsp;</td></tr>");

                    if (dt1.Rows.Count > 0)
                    {
                        htw.WriteLine("<table border='1' cellpadding='5' widht='100%'>");
                        htw.WriteLine("<tr><td align='center' colspan='7'><b><font size='5'>" + dt1.Rows[0]["Particulars"].ToString() + "</font></b></td></tr>");
                        htw.WriteLine("<tr><td rowspan='3' align='center'>State</td><td colspan='2' align='center'>" + dt1.Rows[0]["Header1"].ToString() + "</td><td colspan='2' align='center'>" + dt1.Rows[0]["Header2"].ToString() + "</td><td colspan='2' align='center'>+/-</td></tr>");
                        htw.WriteLine("<tr><td align='center'>PAR POS Cr</td><td align='center'>PAR Count</td><td align='center'>PAR POS Cr</td><td align='center'>PAR Count</td><td rowspan='2' align='center'>PAR POS Cr</td><td rowspan='2' align='center'>PAR Count</td></tr>");
                        htw.WriteLine("<tr><td align='center'>PAR POS</td><td align='center'>AC</td><td align='center'>PAR POS</td><td align='center'>AC</td></tr>");

                        if (dt1 != null && dt1.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt1.Rows)
                            {
                                //if (dr["SlNo"].ToString() != "0")
                                htw.WriteLine(string.Format("<tr><td align='center'>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>", dr["StateName"].ToString(), dr["PARPrOS_Prev"].ToString(), dr["PARLoanCnt_Prev"].ToString(), dr["PARPrOS_Present"].ToString(), dr["PARLoanCnt_Present"].ToString(), dr["DiffPos"].ToString() , dr["DiffCnt"].ToString()));

                            }
                        }
                        htw.WriteLine("<tr><td colspan='7'>&nbsp;</td></tr>");
                        htw.WriteLine("<tr><td colspan='7'>&nbsp;</td></tr>");
                        htw.WriteLine("<tr><td colspan='7'>&nbsp;</td></tr>");
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        htw.WriteLine("<table border='1' cellpadding='5' widht='100%'>");
                        htw.WriteLine("<tr><td align='center' colspan='7'><b><font size='5'>" + dt2.Rows[0]["Particulars"].ToString() + "</font></b></td></tr>");
                        htw.WriteLine("<tr><td rowspan='3' align='center'>State</td><td colspan='2' align='center'>" + dt2.Rows[0]["Header1"].ToString() + "</td><td colspan='2' align='center'>" + dt2.Rows[0]["Header2"].ToString() + "</td><td colspan='2' align='center'>+/-</td></tr>");
                        htw.WriteLine("<tr><td align='center'>PAR POS Cr</td><td align='center'>PAR Count</td><td align='center'>PAR POS Cr</td><td align='center'>PAR Count</td><td rowspan='2' align='center'>PAR POS Cr</td><td rowspan='2' align='center'>PAR Count</td></tr>");
                        htw.WriteLine("<tr><td align='center'>PAR POS</td><td align='center'>AC</td><td align='center'>PAR POS</td><td align='center'>AC</td></tr>");

                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt2.Rows)
                            {
                                //if (dr["SlNo"].ToString() != "0")
                                htw.WriteLine(string.Format("<tr><td align='center'>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>", dr["StateName"].ToString(), dr["PARPrOS_Prev"].ToString(), dr["PARLoanCnt_Prev"].ToString(), dr["PARPrOS_Present"].ToString(), dr["PARLoanCnt_Present"].ToString(), dr["DiffPos"].ToString(), dr["DiffCnt"].ToString()));

                            }
                        }
                        htw.WriteLine("<tr><td colspan='7'>&nbsp;</td></tr>");
                        htw.WriteLine("<tr><td colspan='7'>&nbsp;</td></tr>");
                        htw.WriteLine("<tr><td colspan='7'>&nbsp;</td></tr>");
                    }

                    if (dt3.Rows.Count > 0)
                    {
                        htw.WriteLine("<table border='1' cellpadding='5' widht='100%'>");
                        htw.WriteLine("<tr><td align='center' colspan='7'><b><font size='5'>" + dt3.Rows[0]["Particulars"].ToString() + "</font></b></td></tr>");
                        htw.WriteLine("<tr><td rowspan='2' align='center'>Product</td><td colspan='2' align='center'>" + dt3.Rows[0]["Header"].ToString() + "</td><td colspan='2' align='center'>AUM</td><td rowspan='2' align='center'>POS%</td><td rowspan='2' align='center'>AC%</td></tr>");
                        htw.WriteLine("<tr><td align='center'>PAR POS Cr</td><td align='center'>PAR Count</td><td align='center'>POS Cr</td><td align='center'>Active Cust Count</td></tr>");
                        

                        if (dt3 != null && dt3.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt3.Rows)
                            {
                                //if (dr["SlNo"].ToString() != "0")
                                htw.WriteLine(string.Format("<tr><td align='center'>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>", dr["Product"].ToString(), dr["PARPrOS"].ToString(), dr["PARLoanCnt"].ToString(), dr["PrOS"].ToString(), dr["LoanCnt"].ToString(), dr["POSPer"].ToString(), dr["CntPer"].ToString()));

                            }
                        }
                        htw.WriteLine("<tr><td colspan='7'>&nbsp;</td></tr>");
                        htw.WriteLine("<tr><td colspan='7'>&nbsp;</td></tr>");
                        htw.WriteLine("<tr><td colspan='7'>&nbsp;</td></tr>");
                    }

                    if (dt4.Rows.Count > 0)
                    {
                        htw.WriteLine("<table border='1' cellpadding='5' widht='100%'>");
                        htw.WriteLine("<tr><td align='center' colspan='7'><b><font size='5'>" + dt4.Rows[0]["Particulars"].ToString() + "</font></b></td></tr>");
                        htw.WriteLine("<tr><td rowspan='3' align='center'>State</td><td colspan='2' align='center'>" + dt4.Rows[0]["Header1"].ToString() + "</td><td colspan='2' align='center'>" + dt4.Rows[0]["Header2"].ToString() + "</td><td colspan='2' align='center'>+/-</td></tr>");
                        htw.WriteLine("<tr><td align='center'>PAR POS Cr</td><td align='center'>PAR Count</td><td align='center'>PAR POS Cr</td><td align='center'>PAR Count</td><td rowspan='2' align='center'>PAR POS Cr</td><td rowspan='2' align='center'>PAR Count</td></tr>");
                        htw.WriteLine("<tr><td align='center'>PAR POS</td><td align='center'>AC</td><td align='center'>PAR POS</td><td align='center'>AC</td></tr>");

                        if (dt4 != null && dt4.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt4.Rows)
                            {
                                //if (dr["SlNo"].ToString() != "0")
                                htw.WriteLine(string.Format("<tr><td align='center'>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>", dr["StateName"].ToString(), dr["PARPrOS_Prev"].ToString(), dr["PARLoanCnt_Prev"].ToString(), dr["PARPrOS_Present"].ToString(), dr["PARLoanCnt_Present"].ToString(), dr["DiffPos"].ToString(), dr["DiffCnt"].ToString()));

                            }
                        }
                        htw.WriteLine("<tr><td colspan='7'>&nbsp;</td></tr>");
                        htw.WriteLine("<tr><td colspan='7'>&nbsp;</td></tr>");
                        htw.WriteLine("<tr><td colspan='7'>&nbsp;</td></tr>");
                    }

                    if (dt5.Rows.Count > 0)
                    {
                        htw.WriteLine("<table border='1' cellpadding='5' widht='100%'>");
                        htw.WriteLine("<tr><td align='center' colspan='7'><b><font size='5'>" + dt5.Rows[0]["Particulars"].ToString() + "</font></b></td></tr>");
                        htw.WriteLine("<tr><td rowspan='3' align='center'>State</td><td colspan='2' align='center'>" + dt5.Rows[0]["Header1"].ToString() + "</td><td colspan='2' align='center'>" + dt5.Rows[0]["Header2"].ToString() + "</td><td colspan='2' align='center'>+/-</td></tr>");
                        htw.WriteLine("<tr><td align='center'>PAR POS Cr</td><td align='center'>PAR Count</td><td align='center'>PAR POS Cr</td><td align='center'>PAR Count</td><td rowspan='2' align='center'>PAR POS Cr</td><td rowspan='2' align='center'>PAR Count</td></tr>");
                        htw.WriteLine("<tr><td align='center'>PAR POS</td><td align='center'>AC</td><td align='center'>PAR POS</td><td align='center'>AC</td></tr>");

                        if (dt5 != null && dt5.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt5.Rows)
                            {
                                //if (dr["SlNo"].ToString() != "0")
                                htw.WriteLine(string.Format("<tr><td align='center'>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>", dr["StateName"].ToString(), dr["PARPrOS_Prev"].ToString(), dr["PARLoanCnt_Prev"].ToString(), dr["PARPrOS_Present"].ToString(), dr["PARLoanCnt_Present"].ToString(), dr["DiffPos"].ToString(), dr["DiffCnt"].ToString()));

                            }
                        }
                       
                    }


                    htw.WriteLine("</table>");


                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Write(sw.ToString());
                    Response.End();
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Data Found.");
                }
            }
            finally
            {
                dt = dt1 = dt2 = dt3 = dt4 = dt5 = null;
                oRpt = null;
                Ds = null;
            }
        }
    }
}