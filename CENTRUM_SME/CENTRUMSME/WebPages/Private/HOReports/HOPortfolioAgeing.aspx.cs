using System;
using System.Data;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CENTRUMSME.WebPages.Private.HOReports
{
    public partial class HOPortfolioAgeing : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch(Session[gblValue.UserName].ToString());
                // PopFund();
                txtAsDt.Text = Session[gblValue.LoginDate].ToString();
                //txtToDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Consolidate Portfolio Ageing";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                // this.GetModuleByRole(mnuID.mnuRecPayRpt);
                this.GetModuleByRole(mnuID.mnuHOPortAgeing);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Consolidate Portfolio Ageing", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        //private void PopBranch()
        //{
        //    ViewState["ID"] = null;
        //    DataTable dt = null;
        //    CGblIdGenerator oCG = null;
        //    try
        //    {
        //        //DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //        DateTime vLogDt = gblFuction.setDate(txtAsDt.Text.ToString());

        //        oCG = new CGblIdGenerator();
        //        dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, "0000");
        //        cblBr.DataSource = dt;
        //        cblBr.DataTextField = "Name";
        //        cblBr.DataValueField = "BranchCode";
        //        cblBr.DataBind();
        //        CheckAll("B");
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oCG = null;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUser"></param>
        //private void PopBranch(string pUser)
        //{
        //    DataTable dt = null;
        //    CUser oUsr = null;
        //    oUsr = new CUser();
        //    dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
        //    ViewState["ID"] = null;
        //    try
        //    {

        //        if (dt.Rows.Count > 0)
        //        {
        //            cblBr.DataSource = dt;
        //            cblBr.DataTextField = "BranchName";
        //            cblBr.DataValueField = "BranchCode";
        //            cblBr.DataBind();
        //            CheckAll("B");
        //        }
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oUsr = null;
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        private void PopFund()
        {
            ViewState["FundID"] = null;
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            try
            {
                cblFund.Items.Clear();
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "FunderID", "FunderName", "FunderMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                cblFund.DataSource = dt;
                cblFund.DataTextField = "FunderName";
                cblFund.DataValueField = "FunderID";
                cblFund.DataBind();
                CheckAll("F");
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopLoanType()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            ViewState["FundID"] = null;
            try
            {
                cblFund.Items.Clear();
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanTypeName", "LoanTypeMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                cblFund.DataSource = dt;
                cblFund.DataTextField = "LoanTypeName";
                cblFund.DataValueField = "LoanTypeId";
                cblFund.DataBind();
                CheckAll("F");
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }
        private void PopLoanPurpose()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            ViewState["FundID"] = null;
            try
            {
                cblFund.Items.Clear();
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "PurposeId", "PurposeName", "PurposeMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                cblFund.DataSource = dt;
                cblFund.DataTextField = "PurposeName";
                cblFund.DataValueField = "PurposeId";
                cblFund.DataBind();
                CheckAll("F");
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["FundID"] = null;
            ViewState["LoanID"] = null;
            if (rdbMode.SelectedValue == "A")
                cblFund.Items.Clear();
            else if (rdbMode.SelectedValue == "F")
                PopFund();
            else if (rdbMode.SelectedValue == "L")
                PopLoanType();
            else if (rdbMode.SelectedValue == "P")
                PopLoanPurpose();
            else if (rdbMode.SelectedValue == "B")
                PopBranch(Session[gblValue.UserName].ToString());
        }
        //private void PopBranch()
        //{
        //    if (rdbMode.SelectedValue == "B")
        //    {
        //        ViewState["ID"] = null;
        //        DataTable dt = null;
        //        CBranch oCB = null;
        //        try
        //        {
        //            oCB = new CBranch();
        //            DateTime vAsOnDate = gblFuction.setDate(txtAsDt.Text);
        //            dt = oCB.GetBranchForRpt(vAsOnDate);
        //            cblFund.DataSource = dt;
        //            cblFund.DataTextField = "Name";
        //            cblFund.DataValueField = "BranchCode";
        //            cblFund.DataBind();
        //            CheckAll("B");
        //        }
        //        finally
        //        {
        //            dt = null;
        //            oCB = null;
        //        }
        //    }
        //    else
        //    {
        //        ViewState["ID"] = null;
        //    }
        //}
        protected void cblFund_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll("F");
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckAll(string vMode)//vMode=B(Branch),F(Fund Source)
        {
            Int32 vRow;
            string strin = "";
            ViewState["FundID"] = null;
            ViewState["LoanID"] = null;
            ViewState["PurposeID"] = null;
            ViewState["ID"] = null;
            if (vMode == "F")
            {
                if (ddlFund.SelectedValue == "A")
                {
                    cblFund.Enabled = false;
                    for (vRow = 0; vRow < cblFund.Items.Count; vRow++)
                    {
                        cblFund.Items[vRow].Selected = true;
                        if (strin == "")
                            strin = cblFund.Items[vRow].Value;
                        else
                            strin = strin + "," + cblFund.Items[vRow].Value + "";
                    }
                }
                else if (ddlFund.SelectedValue == "S")
                {
                    //ViewState["FundID"] = null;
                    cblFund.Enabled = true;
                    for (vRow = 0; vRow < cblFund.Items.Count; vRow++)
                    {
                        if (cblFund.Items[vRow].Selected == true)
                        {
                            if (strin == "")
                            {
                                strin = cblFund.Items[vRow].Value;
                            }
                            else
                            {
                                strin = strin + "," + cblFund.Items[vRow].Value + "";
                            }
                        }
                    }
                    //cblFund.Items[vRow].Selected = false;
                }
                if (rdbMode.SelectedValue == "F")
                    ViewState["FundID"] = strin;
                else if (rdbMode.SelectedValue == "L")
                    ViewState["LoanID"] = strin;
                else if (rdbMode.SelectedValue == "P")
                    ViewState["PurposeID"] = strin;
                else if (rdbMode.SelectedValue == "B")
                    ViewState["ID"] = strin;
            }
            //if (vMode == "B")
            //{
            //    if (ddlFund.SelectedValue == "A")
            //    {
            //        cblFund.Enabled = false;
            //        for (vRow = 0; vRow < cblFund.Items.Count; vRow++)
            //        {
            //            cblFund.Items[vRow].Selected = true;
            //            if (strb == "")
            //                strb = cblFund.Items[vRow].Value;
            //            else
            //                strb = strb + "," + cblFund.Items[vRow].Value + "";
            //        }
            //    }
            //    else if (ddlFund.SelectedValue == "S")
            //    {
            //        ViewState["ID"] = null;
            //        cblFund.Enabled = true;
            //        for (vRow = 0; vRow < cblFund.Items.Count; vRow++)
            //            cblFund.Items[vRow].Selected = false;
            //    }
            //    ViewState["ID"] = strb;
            //}
        }

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
            ViewState["ID"] = null;
            try
            {

                if (dt.Rows.Count > 0)
                {
                    cblFund.DataSource = dt;
                    cblFund.DataTextField = "BranchName";
                    cblFund.DataValueField = "BranchCode";
                    cblFund.DataBind();
                    CheckAll("F");
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }/// <summary>
        /// 
        /// </summary>
        private void GetFund()
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < cblFund.Items.Count; vRow++)
            {
                if (cblFund.Items[vRow].Selected == true)
                {
                    if (strin == "")
                        strin = cblFund.Items[vRow].Value;
                    else
                        strin = strin + "," + cblFund.Items[vRow].Value + "";
                }
            }
            ViewState["FundID"] = strin;
        }
        private void GetBranch()
        {
            if (rdbMode.SelectedValue == "B")
            {
                Int32 vRow;
                string strin = "";
                for (vRow = 0; vRow < cblFund.Items.Count; vRow++)
                {
                    if (cblFund.Items[vRow].Selected == true)
                    {
                        if (strin == "")
                            strin = cblFund.Items[vRow].Value;
                        else
                            strin = strin + "," + cblFund.Items[vRow].Value + "";
                    }
                }
                ViewState["ID"] = strin;
            }
        }
        /// <summary>
        /// 
        /// </summary>



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFund_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFund.SelectedValue == "S")
            {
                Int32 vRow;
                ViewState["FundID"] = null;
                cblFund.Enabled = true;
                for (vRow = 0; vRow < cblFund.Items.Count; vRow++)
                    cblFund.Items[vRow].Selected = false;
            }
            else
            {
                CheckAll("F");
            }
            // CheckAll("F");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBr_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll("B");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            // GetData("PDF");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            string vBrCode = Convert.ToString(ViewState["ID"]);
            string vFunder = Convert.ToString(ViewState["FundID"]);
            string vLoanType = Convert.ToString(ViewState["LoanID"]);
            string vPurpose = Convert.ToString(ViewState["PurposeID"]);
            string vFileNm = "";
            string vBranchCode = Session[gblValue.BrnchCode].ToString();
            string vPrm = Convert.ToString(ViewState["FundID"]);
            string vBranch = Session[gblValue.BrName].ToString();
            string vMode = rdbMode.SelectedValue;
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            dt = oRpt.rptHOPortfolioAgeing(vMode, vBrCode, vPrm, gblFuction.setDate(txtAsDt.Text), vFunder, vLoanType, vPurpose);
            vFileNm = "attachment;filename=" + txtAsDt.Text.Replace("/", "") + "_PortfolioAgeing_Report.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";

            HttpContext.Current.Response.Write("<style> .txt" + "\r\n" + "{mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
            Response.Write("<table border='1' cellpadding='0' width='100%'>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress1(vBranchCode) + " </font></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress2(vBranchCode) + " </font></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> Branch - " + vBranchCode + " - " + vBranch + " </font></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>  Portfolio Ageing</font></u></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> As On Date - " + txtAsDt.Text + " </font></b></td></tr>");
            Response.Write("<tr>");

            foreach (DataColumn dtCol in dt.Columns)
            {
                Response.Write("<td><b>" + dtCol.ColumnName + "<b></td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in dt.Rows)
            {
                Response.Write("<tr style='height:20px;'>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    //if (dt.Columns[j].ColumnName == "Customer No")
                    //{
                    //    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                    //}
                    //else if (dt.Columns[j].ColumnName == "Customer Contact No")
                    //{
                    //    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                    //}
                    //else if (dt.Columns[j].ColumnName == "LoanNo")
                    //{
                    //    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                    //}
                    //else
                    //{
                    Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                    // }
                }
            }
            Response.Write("</tr>");
            Response.Write("</table>");
            Response.Flush();
            Response.End();

        }


        //private void GetData(string pFormat)
        //{
        //    //GetBranch();
        //    GetFund();
        //    string vBrCode = Convert.ToString(ViewState["ID"]);
        //    string vPrm = Convert.ToString(ViewState["FundID"]);
        //    string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\HOPortfolioAgeing.rpt";
        //    string vBranch = Session[gblValue.BrName].ToString();
        //    string vMode = rdbMode.SelectedValue;
        //    DataTable dt = null;
        //    CReports oRpt = null;
        //    try
        //    {
        //        using (ReportDocument rptDoc = new ReportDocument())
        //        {
        //            oRpt = new CReports();
        //            rptDoc.Load(vRptPath);
        //            dt = oRpt.rptHOPortfolioAgeing(vMode, vBrCode, vPrm, gblFuction.setDate(txtAsDt.Text));
        //            rptDoc.SetDataSource(dt);
        //            rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
        //            rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1("0000"));
        //            rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2("0000"));
        //            rptDoc.SetParameterValue("pBranch", "Consolidate");
        //            rptDoc.SetParameterValue("pTitle", "Consolidate Portfolio Aging");
        //            rptDoc.SetParameterValue("pFrmDt", txtAsDt.Text);
        //            rptDoc.SetParameterValue("pGrpName", Convert.ToString(rdbMode.SelectedValue));
        //            //if (pFormat == "PDF")
        //            //    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, gblFuction.setDate(txtAsDt.Text).ToString("yyyyMMdd") + "_Consolidate_Portfolio_Aging");
        //            //else
        //                rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, gblFuction.setDate(txtAsDt.Text).ToString("yyyyMMdd") + "_Consolidate_Portfolio_Aging");
        //        }
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oRpt = null;
        //    }
        //}
    }
}
