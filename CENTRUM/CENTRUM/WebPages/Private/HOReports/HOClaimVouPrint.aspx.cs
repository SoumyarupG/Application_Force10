using System;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HOClaimVouPrint : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                //rblSel.SelectedValue = "rbWhole";
                PopBranch();
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Claim/FT/Insurance Voucher";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHOClaimTrnIns);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Portfolio Ageing", false);
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
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {

            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = Convert.ToString(ViewState["BrCode"]);
            if (vBrCode == null || vBrCode == "")
            {
                gblFuction.MsgPopup("Please Select a Branch ..");
                return;
            }
            string vRptPath = "", vType = "";
            string vBranch = Session[gblValue.BrName].ToString();
            //ReportDocument rptDoc = new ReportDocument();
            DataTable dt = new DataTable();
            CReports oVoucher = new CReports();
            //vTypeId = ViewState["Dtl"].ToString();
            string vBranchCode = Session[gblValue.BrnchCode].ToString();
            double vAllToTal = 0.0;

            if (rblSel.SelectedValue == "rbClaim") vType = "CR";
            if (rblSel.SelectedValue == "rbTrns") vType = "FT";
            if (rblSel.SelectedValue == "rbIns") vType = "FG";
            if (rblSel.SelectedValue == "rbBrTrns") vType = "HB";
            if (rblSel.SelectedValue == "rbHoTrns") vType = "BH";



            if (pMode == "PDF")
            {

                dt = oVoucher.GetVoucherDtlForClaim(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), vFromDt, vToDt, vType, vBrCode);
                foreach (DataRow dr in dt.Rows)
                {
                    vAllToTal = vAllToTal + Convert.ToDouble(dr["Debit"].ToString());
                }
                //dt.DefaultView.Sort = "DC ASC";
                DataView dv = dt.DefaultView;
                //dv.Sort = "DC ASC";
                dv.Sort = "DC DESC";
                DataTable sortedDT = dv.ToTable();
                //if (dt.Rows[0]["VoucherType"].ToString() == "P")
                //{
                // vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptPrintVoucherPayment.rpt";
                //}
                //else
                //{
                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptPrintVoucherJournalClaim.rpt";
                //}

                using (ReportDocument rptDoc = new ReportDocument())
                {
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(sortedDT);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBranchCode));
                    rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBranchCode));
                    rptDoc.SetParameterValue("pBranch", Session[gblValue.BrName].ToString());

                    if (dt.Rows.Count > 0)
                    {
                        //if (dt.Rows[0]["VoucherType"].ToString() == "R")
                        //    rptDoc.SetParameterValue("pTitle", "Receipt Voucher");
                        //else if (dt.Rows[0]["VoucherType"].ToString() == "P")
                        //    rptDoc.SetParameterValue("pTitle", "Payment Voucher");
                        //else if (dt.Rows[0]["VoucherType"].ToString() == "J")
                        //    rptDoc.SetParameterValue("pTitle", "Journal Voucher");
                        //else
                        //    rptDoc.SetParameterValue("pTitle", "Contra Voucher");
                        if (vType == "CR")
                        {
                            rptDoc.SetParameterValue("pTitle", "Receipt Voucher");
                        }
                        else
                        {
                            rptDoc.SetParameterValue("pTitle", "Payment Voucher");
                        }
                        //if (vType == "FT")
                        //{
                        //    rptDoc.SetParameterValue("pTitle", "Payment Voucher");
                        //}
                        //if (vType == "FG")
                        //{
                        //    rptDoc.SetParameterValue("pTitle", "Payment Voucher");
                        //}
                        //if (vType == "HB")
                        //{
                        //    rptDoc.SetParameterValue("pTitle", "Payment Voucher");
                        //}
                        //if (vType == "BH")
                        //{
                        //    rptDoc.SetParameterValue("pTitle", "Payment Voucher");
                        //}

                    }
                    else
                    {
                        rptDoc.SetParameterValue("pTitle", "");
                    }
                    rptDoc.SetParameterValue("pAllTotal", vAllToTal);
                    //rptDoc.SetParameterValue("pFrmDt", txtDtFrm.Text);
                    //rptDoc.SetParameterValue("pToDt", txtToDt.Text);

                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Journal_Voucher");
                    rptDoc.Close();
                    rptDoc.Dispose();
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
            else if (pMode == "Excel")
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("PDF");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PopBranch()
        {
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            //string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oCG = new CGblIdGenerator();
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                dt = oCG.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
            }
            else
            {
                dt = oCG.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", vBrId, "BranchCode", "AA", vLogDt, Session[gblValue.BrnchCode].ToString());
            }

            chkBrDtl.DataSource = dt;
            chkBrDtl.DataTextField = "BranchName";
            chkBrDtl.DataValueField = "BranchCode";
            chkBrDtl.DataBind();

            if (rblBr.SelectedValue == "rbAll")
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
            else if (rblBr.SelectedValue == "rbSel")
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
            if (rblBr.SelectedValue == "rbAll")
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
            else if (rblBr.SelectedValue == "rbSel")
            {
                ViewState["BrCode"] = null;
                chkBrDtl.Enabled = true;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    chkBrDtl.Items[vRow].Selected = false;

            }
        }

        protected void rblBr_SelectedIndexChanged(object sender, EventArgs e)
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
    }
}
