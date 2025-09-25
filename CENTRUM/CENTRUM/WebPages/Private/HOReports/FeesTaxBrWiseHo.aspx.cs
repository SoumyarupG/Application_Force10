using System;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Web.UI;
using System.Web;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class FeesTaxBrWiseHo : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtFDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtTDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch();
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Branch Wise Fees And Service Tax Details";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHOBrWsFeesSrvcTaxDtls);
                if (this.UserID == 1) return;
                //if (this.CanReport == "Y")
                //{
                //}
                //else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                //{
                //    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Repayment Schedule", false);
                //}
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
        }

        //protected void btnPdf_Click(object sender, EventArgs e)
        //{
        //    SetParameterForRptData("PDF");
        //}

        private void PopBranch()
        {
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            DataTable dt = null;
            CUser oUsr = null;
            //string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));

                chkBrDtl.DataSource = dt;
                chkBrDtl.DataTextField = "BranchName";
                chkBrDtl.DataValueField = "BranchCode";
                chkBrDtl.DataBind();

                if (rblAlSel.SelectedValue == "rbAll")
                {
                    chkBrDtl.Enabled = false;
                    for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    {
                        chkBrDtl.Items[vRow].Selected = true;
                        if (strin == "")
                        {
                            strin = chkBrDtl.Items[vRow].Value;
                        }
                        else
                        {
                            strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                        }
                    }
                }
                else if (rblAlSel.SelectedValue == "rbSel")
                {
                    for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    {
                        chkBrDtl.Items[vRow].Selected = false;
                    }
                }
                ViewState["BrCode"] = strin;
            }
        


        private void CheckBrAll()
        {
            Int32 vRow;
            string strin = "";
            if (rblAlSel.SelectedValue == "rbAll")
            {
                chkBrDtl.Enabled = false;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
                ViewState["BrCode"] = strin;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                ViewState["BrCode"] = null;
                chkBrDtl.Enabled = true;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    chkBrDtl.Items[vRow].Selected = false;

            }
        }

        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBrAll();
        }


        protected void chkBrDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
            {
                if (chkBrDtl.Items[vRow].Selected == true)
                {
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
            }
            ViewState["BrCode"] = strin;
        }


        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtFDt.Text);
            DateTime vToDt = gblFuction.setDate(txtTDt.Text);
            string vBrCode = ViewState["BrCode"].ToString();
            string vFileNm = "";
            string vBranch = Session[gblValue.BrName].ToString();
            DataTable dt = null;
            CReports oRpt = new CReports();
            if (rbDetailSum.SelectedValue == "rbSummary")
            {
                SetParameterForsummary("Excel");
            }
            else
            {

                System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                dt = oRpt.rptFeesAndTaxBrWise(vFromDt, vToDt, vBrCode);

                //dt = ChangeDataTable(dt);// to add grand total

                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();


                tdx.Controls.Add(DataGrid1);
                tdx.Visible = false;
                vFileNm = "attachment;filename=Branch_Wise_Processing_Fees_and_Service_Tax_Details.xls";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                htw.WriteLine("<table border='1' cellpadding='10' widht='100%'>");
                htw.WriteLine("<tr><td align=center' colspan='10'><b><font size='6'>" + gblValue.CompName + "</font></b></td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='10'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress1("0000") + "</font></b></td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='10'><b><font size='4'>Branch Wise Processing Fees and Service Tax Details from " + txtFDt.Text + " to " + txtTDt.Text + "</font></b></td></tr>");
                DataGrid1.RenderControl(htw);
                htw.WriteLine("</td></tr>");
                htw.WriteLine("<tr><td colspan='9'><b><u><font size='16'></font></u></b></td></tr>");
                htw.WriteLine("</table>");

                Response.ClearContent();
                Response.AddHeader("content-disposition", vFileNm);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                //Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
                Response.Write(sw.ToString());
                Response.End();

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable ChangeDataTable(DataTable dt)
        {
            Int32 dr, vNoofLoan = 0;
            Double vDAP = 0.0, vDAI = 0.0, vPF = 0.0, vIF = 0.0, vCP = 0.0, vCI = 0.0, vCT = 0.0, vDP = 0.0, vDI = 0.0, vDT = 0.0, vAI = 0.0, vDRR = 0.0, vAdv = 0.0, vOD = 0.0, vCL = 0.0, vFP = 0.0;
            for (dr = 0; dr < dt.Rows.Count; dr++)
            {
                vNoofLoan = vNoofLoan + Convert.ToInt32(dt.Rows[dr]["No_Of_Loan_Disburse"].ToString());
                vDAP = vDAP + Convert.ToDouble(dt.Rows[dr]["Amount_Of_Disburse_Loan"].ToString());
                vDAI = vDAI + Convert.ToDouble(dt.Rows[dr]["Inerest_On_Disburse_Loan"].ToString());
                vPF = vPF + Convert.ToDouble(dt.Rows[dr]["Processing_Fees"].ToString());
                vIF = vIF + Convert.ToDouble(dt.Rows[dr]["Insurence_Fees"].ToString());
                vCP = vCP + Convert.ToDouble(dt.Rows[dr]["Collection_Realised_Principal"].ToString());
                vCI = vCI + Convert.ToDouble(dt.Rows[dr]["Collection_Realised_Interest"].ToString());
                vCT = vCT + Convert.ToDouble(dt.Rows[dr]["Collection_Realised_Total"].ToString());
                vDP = vDP + Convert.ToDouble(dt.Rows[dr]["Demand_Realisable_Principal"].ToString());
                vDI = vDI + Convert.ToDouble(dt.Rows[dr]["Demand_Realisable_Interest"].ToString());
                vDT = vDT + Convert.ToDouble(dt.Rows[dr]["Demand_Realisable_Total"].ToString());
                vAI = vAI + Convert.ToDouble(dt.Rows[dr]["Accrued_Interest"].ToString());
                vDRR = vDRR + Convert.ToDouble(dt.Rows[dr]["Difference_Realised_Realisable"].ToString());
                vAdv = vAdv + Convert.ToDouble(dt.Rows[dr]["Advance"].ToString());
                vOD = vOD + Convert.ToDouble(dt.Rows[dr]["OverDue"].ToString());
                vCL = vCL + Convert.ToInt32(dt.Rows[dr]["Total_Closed_Loan"].ToString());
                vFP = vFP + Convert.ToDouble(dt.Rows[dr]["Total_Full_Paid_Collection"].ToString());
            }
            dt.Rows.InsertAt(dt.NewRow(), 0);

            dt.Rows[0]["No_Of_Loan_Disburse"] = Convert.ToString(vNoofLoan);
            dt.Rows[0]["Amount_Of_Disburse_Loan"] = Convert.ToString(vDAP);
            dt.Rows[0]["Inerest_On_Disburse_Loan"] = Convert.ToString(vDAI);
            dt.Rows[0]["Processing_Fees"] = Convert.ToString(vPF);
            dt.Rows[0]["Insurence_Fees"] = Convert.ToString(vIF);
            dt.Rows[0]["Collection_Realised_Principal"] = Convert.ToString(vCP);
            dt.Rows[0]["Collection_Realised_Interest"] = Convert.ToString(vCI);
            dt.Rows[0]["Collection_Realised_Total"] = Convert.ToString(vCT);
            dt.Rows[0]["Demand_Realisable_Principal"] = Convert.ToString(vDP);
            dt.Rows[0]["Demand_Realisable_Interest"] = Convert.ToString(vDI);
            dt.Rows[0]["Demand_Realisable_Total"] = Convert.ToString(vDT);
            dt.Rows[0]["Accrued_Interest"] = Convert.ToString(vAI);
            dt.Rows[0]["Difference_Realised_Realisable"] = Convert.ToString(vDRR);
            dt.Rows[0]["Advance"] = Convert.ToString(vAdv);
            dt.Rows[0]["OverDue"] = Convert.ToString(vOD);
            dt.Rows[0]["Total_Closed_Loan"] = Convert.ToString(vCL);
            dt.Rows[0]["Total_Full_Paid_Collection"] = Convert.ToString(vFP);

            dt.AcceptChanges();
            return dt;
        }
        private void SetParameterForsummary(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtFDt.Text);
            DateTime vToDt = gblFuction.setDate(txtTDt.Text);
            string vBrCode = ViewState["BrCode"].ToString();
            string vTitle = "Branch Wise Processing Fees and Service Tax Summary", vRptPath = "";
            string vBranch = Session[gblValue.BrName].ToString();
            DataTable dt = null;
            CReports oRpt = new CReports();
            string vFileNm = "";

            //ReportDocument rptDoc = new ReportDocument();
            //dt = new DataTable();
            //Int32 vLTID = 0, vFSID = 0, vLCID = 0;



            //vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\BranchWiseFeesAndTaxSummary.rpt";
            //dt = oRpt.rptFeesAndTaxBrWiseSummary(vFromDt, vToDt, vBrCode);
            //rptDoc.Load(vRptPath);
            //rptDoc.SetDataSource(dt);
            //rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
            //rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
            //rptDoc.SetParameterValue("pTitle", vTitle);
            ////rptDoc.SetParameterValue("pBranch", vBranch);
            //rptDoc.SetParameterValue("dtFrom", txtFDt.Text);
            //rptDoc.SetParameterValue("dtTo", txtTDt.Text);
            ////rptDoc.SetParameterValue("pMode", pMode);
            //if (pMode == "PDF")
            //    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Demand_Collection_Balance" + txtTDt.Text.Replace("/", "_"));
            //else if (pMode == "Excel")
            //    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Branch_Wise_Processing_Fees_and_Service_Tax_Summary" + txtTDt.Text.Replace("/", "_") + ".xls");
            //rptDoc.Dispose();

            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            dt = new DataTable();
            dt = oRpt.rptFeesAndTaxBrWiseSummary(vFromDt, vToDt, vBrCode);
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();
            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            vFileNm = "attachment;filename=Branch_Wise_Processing_Fees_and_Service_Tax_Summary.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='1' cellpadding='9' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan='9'><b><font size='6'>" + gblValue.CompName + "</font></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='9'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress1("0000") + "</font></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='9'><b><font size='4'>Branch_Wise_Processing_Fees_and_Service_Tax_Summary from " + txtFDt.Text + " to " + txtTDt.Text + "</font></b></td></tr>");
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='9'><b><u><font size='16'></font></u></b></td></tr>");
            htw.WriteLine("</table>");

            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            //Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}
