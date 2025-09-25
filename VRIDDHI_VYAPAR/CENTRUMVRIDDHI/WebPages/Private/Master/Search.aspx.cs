using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.Web.Security;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace CENTRUM_VRIDDHIVYAPAR.WebPages.Private.Master
{
    public partial class Search : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode] == null)
                {
                    Session.Abandon();
                    FormsAuthentication.SignOut();
                    Session.RemoveAll();
                    Response.Redirect("~/Login.aspx?e=random");
                }

                this.Menu = true;
                this.PageHeading = "Search";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
            }
            finally
            {
                //nothing to do here
            }
        }

        private void loadGrid(Int32 pPgIndx)
        {
            string vBranchCode = Session[gblValue.BrnchCode].ToString();
            DateTime vEffectiveDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                dt = oMem.GetMemberPG(txtSrch.Text.Trim(), vBranchCode, vEffectiveDt);
                gvMember.DataSource = dt.DefaultView;
                gvMember.DataBind();
                mvPos.ActiveViewIndex = 0;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        protected void gvMem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string customerid = "";           
            if (e.CommandName == "cmdShow")
            {
                customerid = Convert.ToString(e.CommandArgument);
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                foreach (GridViewRow gr in gvMember.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;
                getMemInfo(customerid);
            }
        }

        private void getMemInfo(string customerid)
        {
            ViewState["MemId"] = "";
            ViewState["LoanAppId"] = "";
            DataSet ds = null;
            DataTable dt = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            DataTable dt4 = null;
            DataTable dt5 = null;
            DataTable dt6 = null;
            DataTable dt7 = null;
            DataTable dt8 = null;
            DataTable dt9 = null;
            DataTable dt10 = null;

            CMember oMem = null;
            try
            {
                ViewState["MemId"] = customerid;
                oMem = new CMember();
                DateTime logindate = gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate]));
                string branchcode = Convert.ToString(Session[gblValue.BrnchCode]);
                ds = oMem.GetMemberDetailsSearch(customerid, logindate);
                dt = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[2];
                dt4 = ds.Tables[3];
                dt5 = ds.Tables[4];
                dt6 = ds.Tables[5];
                dt7 = ds.Tables[6];
                dt8 = ds.Tables[7];
                dt9 = ds.Tables[8];
                dt10 = ds.Tables[9];
                
                if (Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
                {
                    foreach (DataRow dr in dt.Rows) // search whole table
                    {
                        if (Convert.ToInt32(dr["M_AddProfId"].ToString()) == 1)
                        {
                            dr["M_AddProfNo"] = String.Format("{0}{1}", "********", Convert.ToString(dr["M_AddProfNo"]).Substring(Convert.ToString(dr["M_AddProfNo"]).Length - 4, 4));
                        }
                        if (Convert.ToInt32(dr["M_IdentyPRofId"].ToString()) == 1)
                        {
                            dr["M_IdentyProfNo"] = String.Format("{0}{1}", "********", Convert.ToString(dr["M_IdentyProfNo"]).Substring(Convert.ToString(dr["M_IdentyProfNo"]).Length - 4, 4));
                        }
                    }
                }

                gvMemInfo.DataSource = dt;
                gvMemInfo.DataBind();
                tabBr.ActiveTabIndex = 1;
               
                gvMemCBApproval.DataSource = dt2;
                gvMemCBApproval.DataBind();
                tabBr.ActiveTabIndex = 1; 

                gvHouseVisit.DataSource = dt3;
                gvHouseVisit.DataBind();
                tabBr.ActiveTabIndex = 1;
               
                gvGRT.DataSource = dt4;
                gvGRT.DataBind();
                tabBr.ActiveTabIndex = 1;
               
                gvLoanDisb.DataSource = dt5;
                gvLoanDisb.DataBind();
                tabBr.ActiveTabIndex = 1;
               
                gvCollection.DataSource = dt6;
                gvCollection.DataBind();
                tabBr.ActiveTabIndex = 1;
                
                gvSanc.DataSource = dt7;
                gvSanc.DataBind();
                tabBr.ActiveTabIndex = 1;
               
                gvPreDisb.DataSource = dt8;
                gvPreDisb.DataBind();
                tabBr.ActiveTabIndex = 1;                

                gvRed.DataSource = dt9;
                gvRed.DataBind();
                tabBr.ActiveTabIndex = 1;

                if (Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
                {
                    foreach (DataRow dr in dt10.Rows) // search whole table
                    {
                        if (Convert.ToString(dr["Identity Proof 1"].ToString()) == "AADHAAR")
                        {
                            dr["Identity Proof 1 No"] = String.Format("{0}{1}", "********", Convert.ToString(dr["Identity Proof 1 No"]).Substring(Convert.ToString(dr["Identity Proof 1 No"]).Length - 4, 4));
                        }
                        if (Convert.ToString(dr["Identity Proof 2"].ToString()) == "AADHAAR")
                        {
                            dr["Identity Proof 2 No"] = String.Format("{0}{1}", "********", Convert.ToString(dr["Identity Proof 2 No"]).Substring(Convert.ToString(dr["Identity Proof 2 No"]).Length - 4, 4));
                        }
                    }
                }
                gvKYC.DataSource = dt10;
                gvKYC.DataBind();
                tabBr.ActiveTabIndex = 1;
            }
            finally
            {
                dt = null;
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            gvMember.DataSource = null;
            gvMember.DataBind();
            loadGrid(0);
        }

        protected void gvMemInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string customerid = "";
            customerid = Convert.ToString(e.CommandArgument);
            CreateDynamicGrid(customerid);
        }

        protected void gvMemCBApproval_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vEnqId = "", CbId = "";
            int vCbId = 0;
            vEnqId = Convert.ToString(e.CommandArgument);
            string[] commandArgs = vEnqId.ToString().Split(new char[] { '#' });
            vEnqId = commandArgs[0];
            CbId = commandArgs[1];
            vCbId = Convert.ToInt32(CbId);
            SetParameterForRptData(vEnqId, vCbId, "PDF");

        }

        private void SetParameterForRptData(string pEnquiryId, int pCbAppId, string pType)
        {
            DataSet ds = null;
            DataTable dt1 = null, dt2 = null;
            CReports oRpt = null;
            string vRptPath = "";
            string vBranch = Session[gblValue.BrName].ToString();
            try
            {
                //cvar = 1;
                oRpt = new CReports();
                string enqstatusmsg = "";
               // ds = oRpt.Equifax_Report(pEnquiryId, pCbAppId, ref  enqstatusmsg);//09122022
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
                dt1.TableName = "CBPortDtl";
                dt2.TableName = "CBPortMst";
                if (pType == "PDF")
                {
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptMemberCredit.rpt";
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(ds);
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "CreditSummaryReport");

                        Response.ClearHeaders();
                        Response.ClearContent();
                    }
                }
            }

            finally
            {
                dt1 = null;
                dt2 = null;
                oRpt = null;
            }
        }

        protected void gvHouseVisit_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void gvGRT_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void gvLoanDisb_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vLoanId = "";
            vLoanId = Convert.ToString(e.CommandArgument);
            GetData(vLoanId, "PDF");
        }


        private void GetData(string vLoanId, string pFormat)
        {
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptRePayment.rpt";
            string vBranch = "", vBrCode = "", vGroup = "", vMemName = "", vMemNo = "", vLnProduct = "";
            string vLnNo = "", vDisbDt = "", vROName = "", vMarketNm = "";
            double vLoanAmt = 0, vIntvIntRate = 0.0;
            Int32 vCycle = 0, vTotalInstNo = 0;
            DataTable dt = null, dt1 = null;
            DataSet ds = null;           
            CReports oRpt = null;
            try
            {
                vBranch = Session[gblValue.BrName].ToString();
                vBrCode = Session[gblValue.BrnchCode].ToString();               
                oRpt = new CReports();
                // ds = oRpt.rptRepaySchedule(vLoanId, vBrCode); //09122022
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                vROName = dt.Rows[0]["EoName"].ToString();              
                vMemName = dt.Rows[0]["MemberName"].ToString();
                vMemNo = dt.Rows[0]["MemberNo"].ToString();
                vGroup = dt.Rows[0]["GroupName"].ToString();
                vLnProduct = dt.Rows[0]["LoanType"].ToString();
                vLnNo = dt.Rows[0]["LoanNo"].ToString();
                vDisbDt = dt.Rows[0]["LoanDt"].ToString();
                vLoanAmt = Convert.ToDouble(dt.Rows[0]["LoanAmt"]);
                vIntvIntRate = Convert.ToDouble(dt.Rows[0]["IntRate"]);
                vCycle = Convert.ToInt32(dt.Rows[0]["dose"]);
                vTotalInstNo = Convert.ToInt32(dt.Rows[0]["TotalInstNo"]);
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt1);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", gblValue.Address1);
                    rptDoc.SetParameterValue("pAddress2", gblValue.Address2);
                    rptDoc.SetParameterValue("pBranch", vBranch);
                    rptDoc.SetParameterValue("pTitle", "Repayment Schedule");
                    rptDoc.SetParameterValue("pRO", vROName);
                    rptDoc.SetParameterValue("pMarket", vMarketNm);
                    rptDoc.SetParameterValue("pGroup", vGroup);
                    rptDoc.SetParameterValue("pMemName", vMemName);
                    rptDoc.SetParameterValue("pMemNo", vMemNo);
                    rptDoc.SetParameterValue("pLnProduct", vLnProduct);
                    rptDoc.SetParameterValue("pLoanNo", vLnNo);
                    rptDoc.SetParameterValue("pLoanAmt", vLoanAmt);
                    rptDoc.SetParameterValue("pDisbDt", vDisbDt);
                    rptDoc.SetParameterValue("pMode", "R");
                    rptDoc.SetParameterValue("pIntvIntRate", vIntvIntRate);
                    rptDoc.SetParameterValue("pCycle", vCycle);
                    rptDoc.SetParameterValue("pTotalInstNo", vTotalInstNo);
                    if (pFormat == "PDF")
                    {
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Repayment Schedule");
                    }
                    else
                    {
                        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Repayment Schedule");
                    }
                    rptDoc.Dispose();
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
            finally
            {
                dt = null;
                dt1 = null;
                ds = null;              
                oRpt = null;
            }
        }


        protected void gvCollection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vLoanId = "", vBranchCode = "";
            vLoanId = Convert.ToString(e.CommandArgument);
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            vBranchCode = gvr.Cells[5].Text;
            GetDataSOA(vLoanId, "PDF", vBranchCode);
        }

        private void GetDataSOA(string vLoanId, string pFormat, string pBranchCode)
        {
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            string vBrCode = pBranchCode, vMemName = "", vMemNo = "", vLnProduct = "";
            string vLnNo = "", vDisbDt = "", vSpouseNm = "";
            string vFundSource = "", vPurpose = "", vFileNm = "", vGroupName = "", vMarket = "", vEO = "";
            double vLoanAmt = 0, vIntAmt = 0, vOSAmt = 0, vTopupAmt = 0, vIntRate = 0;
            string vRptPath = "";
            DataTable dt = null, dt1 = null;
            DataSet ds = null;
            CReports oRpt = null;
            try
            {
                oRpt = new CReports();
                ds = oRpt.rptPartyLedger(vLoanId, vBrCode);
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];

                if (dt.Rows.Count > 0 && dt1.Rows.Count > 0)
                {

                    vOSAmt = Convert.ToDouble(dt.Rows[0]["LoanAmt"]);
                    foreach (DataRow dr in dt1.Rows)
                    {                       
                        if (Convert.IsDBNull(dr["Principal_"]) == false)
                        {
                            vOSAmt = vOSAmt - Convert.ToDouble(dr["Principal_"].ToString());
                            dr["Out_Standing"] = Math.Round(vOSAmt, 2);
                        }
                    }
                    dt1.AcceptChanges();
                    vMemNo = dt.Rows[0]["MemberNo"].ToString();
                    vMemName = dt.Rows[0]["MemberName"].ToString();
                    vSpouseNm = dt.Rows[0]["Spouce"].ToString();
                    vFundSource = dt.Rows[0]["FundSource"].ToString();
                    vPurpose = dt.Rows[0]["Purpose"].ToString();
                    vLnNo = dt.Rows[0]["LoanNo"].ToString();
                    vLoanAmt = Convert.ToDouble(dt.Rows[0]["LoanAmt"]);// Convert.ToDouble(dt.Rows[0]["LOSAmt"]);
                    vIntAmt = Convert.ToDouble(dt.Rows[0]["IntAmt"]);//Convert.ToDouble(dt.Rows[0]["IOSAmt"]);
                    vDisbDt = dt.Rows[0]["LoanDt"].ToString();
                    vLnProduct = dt.Rows[0]["LoanType"].ToString();
                    vGroupName = dt.Rows[0]["Groupname"].ToString();
                    vMarket = dt.Rows[0]["Market"].ToString();
                    vEO = dt.Rows[0]["EoName"].ToString();
                    vTopupAmt = Convert.ToDouble(dt.Rows[0]["TopupAmt"]);
                    vIntRate = Convert.ToDouble(dt.Rows[0]["IntRate"]);
                    
                    if (pFormat == "Excel")
                    {
                        DataGrid1.DataSource = dt1;
                        DataGrid1.DataBind();

                        vFileNm = "attachment;filename=PartyLedger.xls";
                        StringWriter sw = new StringWriter();
                        HtmlTextWriter htw = new HtmlTextWriter(sw);
                        htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
                        htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
                        htw.WriteLine("<tr><td align=center' colspan='13'><b><u><font size='5'>Unity Small Finance Bank Ltd.</font></u></b></td></tr>");
                        htw.WriteLine("<tr><td align=center' colspan='13'><b><u><font size='3'>Party Ledger</font></u></b></td></tr>");                        
                        htw.WriteLine("<tr><td colspan='2'><b>Member No:</b></td><td align='left' colspan='3'><b>" + vMemNo + "</b></td><td colspan='2'><b>Member Name:</b></td><td align='left' colspan='4'><b>" + vMemName + "</b></td><td colspan='1'><b>RO Name:</b></td><td align='left' colspan='4'><b>" + vEO + "</b></td></tr>");
                        htw.WriteLine("<tr><td colspan='2'><b>Spouce Name:</b></td><td align='left' colspan='3'><b>" + vSpouseNm + "</b></td><td colspan='2'><b>Fund Source:</b></td><td align='left' colspan='4'><b>" + vFundSource + "</b></td><td colspan='1'><b>Group Name:</b></td><td align='left' colspan='4'><b>" + vGroupName + "</b></td></tr>"); //<td colspan='1'><b>Center Name:</b></td><td align='left' colspan='4'><b>" + vMarket + "</b></td>
                        htw.WriteLine("<tr><td colspan='2'><b>Loan No:</b></td><td align='left' colspan='3'><b>" + vLnNo + "</b></td><td colspan='2'><b>Purpose:</b></td><td align='left' colspan='4'><b>" + vPurpose + "</b></td><td colspan='2'><b>Topup Deduction Amount:</b></td><td align='left' colspan='4'><b>" + vTopupAmt + "</b></td></tr>");
                        htw.WriteLine("<tr><td colspan='2'><b>Loan Amount:</b></td><td align='left' colspan='3'><b>" + vLoanAmt + "</b></td><td colspan='2'><b>Interest Amount:</b></td><td align='left' colspan='4'><b>" + vIntAmt + "</b></td></tr>");
                        htw.WriteLine("<tr><td colspan='2'><b>Disburse Date:</b></td><td align='left' colspan='3'><b>" + vDisbDt + "</b></td><td colspan='2'><b>Loan Scheme:</b></td><td align='left' colspan='4'><b>" + vLnProduct + "</b></td></tr>");
                        htw.WriteLine("<tr><td align=center' colspan='6'><b><u><font size='3'>Repayment Schedule</font></u></b></td><td align=center' colspan='7'><b><u><font size='3'>Collection Details</font></u></b></td></tr>");
                        
                        DataGrid1.RenderControl(htw);
                        htw.WriteLine("</td></tr>");
                        htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
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
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptPartyLedger.rpt";
                        using (ReportDocument rptDoc = new ReportDocument())
                        {
                            rptDoc.Load(vRptPath);
                            rptDoc.SetDataSource(dt1);
                            rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                            rptDoc.SetParameterValue("pMemNo", vMemNo);
                            rptDoc.SetParameterValue("pMemName", vMemName);
                            rptDoc.SetParameterValue("pRoName", vEO);
                            rptDoc.SetParameterValue("pSpouceName", vSpouseNm);
                            rptDoc.SetParameterValue("pFndSource", vFundSource);
                            rptDoc.SetParameterValue("pGroupName", vGroupName);
                            rptDoc.SetParameterValue("pLoanNo", vLnNo);
                            rptDoc.SetParameterValue("pPurpose", vPurpose);
                            rptDoc.SetParameterValue("pLoanAmt", vLoanAmt);
                            rptDoc.SetParameterValue("pIntAmt", vIntAmt);
                            rptDoc.SetParameterValue("pDisbDate", vDisbDt);
                            rptDoc.SetParameterValue("pLoanSchm", vLnProduct);
                            rptDoc.SetParameterValue("pTopupAmt", vTopupAmt);
                            rptDoc.SetParameterValue("pIntRate", vIntRate);
                            rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Party_Ledger");
                            rptDoc.Dispose();
                            Response.ClearHeaders();
                            Response.ClearContent();
                        }
                    }


                }
                else
                {
                    gblFuction.AjxMsgPopup("No Records Found.....");
                    return;
                }                
            }
            finally
            {
                dt = null;
                dt1 = null;
                ds = null;
                oRpt = null;
            }
        }

      
        private void CreateDynamicGrid(string customerid)
        {
            //execute the select statement
            CMember oMem = null;
            oMem = new CMember();
            DateTime logindate = gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate]));
            string branchcode = Convert.ToString(Session[gblValue.BrnchCode]);
            DataTable dt = null;
            //  dt = oMem.GetMemberHistory(customerid, logindate); //09122022

            BoundField boundField = null;
            //iterate through the columns of the datatable and add them to the gridview
            foreach (DataColumn col in dt.Columns)
            {
                //initialize the bound field
                boundField = new BoundField();

                //set the DataField.
                boundField.DataField = col.ColumnName;

                //set the HeaderText
                boundField.HeaderText = col.ColumnName;

                //Add the field to the GridView columns.
                gvDetails.Columns.Add(boundField);

            }
            //bind the gridview the DataSource
            gvDetails.DataSource = dt;
            gvDetails.DataBind();
            tabBr.ActiveTabIndex = 2;
        }

        protected void gvPreDisb_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvSanc_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}