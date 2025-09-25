using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Text;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class Search : CENTRUMBAse
    {
        protected Int32 cPgNo = 1;
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
            //gvList.DataSource = null;
            //gvList.DataBind();

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

                //if (dt.Rows.Count > 0)
                //{
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
                //}

                //if (dt2.Rows.Count > 0)
                //{
                gvMemCBApproval.DataSource = dt2;
                gvMemCBApproval.DataBind();
                tabBr.ActiveTabIndex = 1; //gvHouseVisit,gvGRT,gvLoanDisb
                //}
                //if (dt3.Rows.Count > 0)
                //{
                gvHouseVisit.DataSource = dt3;
                gvHouseVisit.DataBind();
                tabBr.ActiveTabIndex = 1;
                //}
                //if (dt4.Rows.Count > 0)
                //{
                gvGRT.DataSource = dt4;
                gvGRT.DataBind();
                tabBr.ActiveTabIndex = 1;
                //}
                //if (dt5.Rows.Count > 0)
                //{
                gvLoanDisb.DataSource = dt5;
                gvLoanDisb.DataBind();
                tabBr.ActiveTabIndex = 1;
                //}
                //if (dt6.Rows.Count > 0)
                //{
                gvCollection.DataSource = dt6;
                gvCollection.DataBind();
                tabBr.ActiveTabIndex = 1;
                //}
                //if (dt7.Rows.Count > 0)
                //{
                gvSanc.DataSource = dt7;
                gvSanc.DataBind();
                tabBr.ActiveTabIndex = 1;
                //}
                //if (dt8.Rows.Count > 0)
                //{
                gvPreDisb.DataSource = dt8;
                gvPreDisb.DataBind();
                tabBr.ActiveTabIndex = 1;
                //}

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
            //if (ddlInterviewState.SelectedValue != "AI")
            //{
            vEnqId = commandArgs[0];
            CbId = commandArgs[1];
            vCbId = Convert.ToInt32(CbId);
            SetParameterForRptData(vEnqId, vCbId, "PDF");
        }
        private void SetParameterForRptData(string pEnquiryId, int pCbAppId, string pType)
        {
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
                ds = oRpt.Equifax_Report(pCbAppId, ref  enqstatusmsg);
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
                dt1.TableName = "CBPortDtl";
                dt2.TableName = "CBPortMst";
                dt3.TableName = "CBHistoryMonth";
                if (pType == "PDF")
                {
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptMemberCredit_New.rpt";
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(ds);
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "CreditSummaryReport");
                        rptDoc.Dispose();
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
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RepaySche.rpt";
            string vBranch = "", vBrCode = "";
            DataTable dt = null;
            CReports oRpt = null;
            try
            {
                vBranch = Session[gblValue.BrName].ToString();
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oRpt = new CReports();
                dt = oRpt.rptRepaySchedule(vLoanId, vBrCode,"Y");
                if (dt.Rows.Count > 0)
                {
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt);
                        rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                        rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                        rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                        rptDoc.SetParameterValue("pBranch", vBranch);
                        rptDoc.SetParameterValue("pTitle", "Repayment Schedule");
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Repayment_Schedule");
                        rptDoc.Dispose();
                        Response.ClearHeaders();
                        Response.ClearContent();
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
            string vBrCode = pBranchCode;
            string vRptPath = "";
            DataSet ds = new DataSet();
            DataTable dt,dt1 = null;
            CReports oRpt = null;

            try
            {
                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\PartyLedger.rpt";
                oRpt = new CReports();
                ds = oRpt.rptPartyLedgerPDF(vLoanId, vBrCode);
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                if (dt.Rows.Count > 0)
                {
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        rptDoc.Load(vRptPath);
                       // dt = oRpt.rptPartyLedgerPDF(vLoanId, vBrCode);
                        rptDoc.SetDataSource(dt);
                        rptDoc.Subreports["OtherCollection.rpt"].SetDataSource(dt1);
                        rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                        rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                        rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                        rptDoc.SetParameterValue("pBranch", vBrCode);
                        rptDoc.SetParameterValue("pTitle", "Party Ledger");                        
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Party_Ledger");
                        rptDoc.Dispose();
                        Response.ClearContent();
                        Response.ClearHeaders();
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
                oRpt = null;
            }
        }
        private void CreateDynamicGrid(string customerid)
        {
            /*
            //execute the select statement
            CMember oMem = null;
            oMem = new CMember();
            DateTime logindate = gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate]));
            string branchcode = Convert.ToString(Session[gblValue.BrnchCode]);
            DataTable dt = null;
            //dt = oMem.GetMemberHistory(customerid, logindate);
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
            */
        }
        protected void gvPreDisb_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
        protected void gvSanc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
    }
}
