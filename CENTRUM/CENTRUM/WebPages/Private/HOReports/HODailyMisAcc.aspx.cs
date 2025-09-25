using System;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;


namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HODailyMisAcc : CENTRUMBase
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
                this.PageHeading = "Daily MIS Accounts";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHODlyMisAccRpt);
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

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("PDF");
        }

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
            string vRptPath = "";
            string vBranch = Session[gblValue.BrName].ToString();
            string vActMstTbl = Session[gblValue.ACVouMst].ToString();
            string vActDtlTbl = Session[gblValue.ACVouDtl].ToString();
            DataTable dt = null;
            //ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();


            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptDailyMisAcc.rpt";

            dt = oRpt.rptDailyMISACC(vFromDt, vToDt, vBrCode, vActMstTbl, vActDtlTbl, gblFuction.setDate(Session[gblValue.FinFromDt].ToString()), Convert.ToInt32(Session[gblValue.FinYrNo].ToString()));
            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1("0000"));
                rptDoc.SetParameterValue("dtFrom", txtFDt.Text);
                rptDoc.SetParameterValue("dtTo", txtTDt.Text);

                if (pMode == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Daily MIS Accounts");
                else if (pMode == "Excel")
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Daily MIS Accounts");

                rptDoc.Dispose();
                Response.ClearHeaders();
                Response.ClearContent();
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
    }
}
