using System;
using System.Data;
using FORCECA;
using FORCEBA;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HORecPay : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch(Session[gblValue.UserName].ToString());
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
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
                this.PageHeading = "Receipt Payment";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHORecptPay);
                if (this.RoleId == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Consolidate Receipt Payment", false);
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
        private void GetBranch()
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
            {
                if (chkBr.Items[vRow].Selected == true)
                {
                    if (strin == "")
                        strin = chkBr.Items[vRow].Value;
                    else
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                }
            }
            ViewState["ID"] = strin;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //private void PopBranch()
        //{
        //    //Int32 vBrId = 0;
        //    //string vBrCode = "";
        //    ViewState["ID"] = null;
        //    DataTable dt = null;
        //    CGblIdGenerator oCG = null;
        //    try
        //    {
        //        DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //        //vBrCode = (string)Session[gblValue.BrnchCode];
        //        //vBrId = Convert.ToInt32(vBrCode);
        //        oCG = new CGblIdGenerator();
        //        dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, "0000");
        //        chkBr.DataSource = dt;
        //        chkBr.DataTextField = "Name";
        //        chkBr.DataValueField = "BranchCode";
        //        chkBr.DataBind();
        //        CheckAll();
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
                    chkBr.DataSource = dt;
                    chkBr.DataTextField = "BranchName";
                    chkBr.DataValueField = "BranchCode";
                    chkBr.DataBind();
                    CheckAll();
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void CheckAll()
        {
            Int32 vRow;
            string strin = "";
            if (ddlSel.SelectedValue == "C")
            {
                chkBr.Enabled = false;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                {
                    chkBr.Items[vRow].Selected = true;
                    if (strin == "")
                        strin = chkBr.Items[vRow].Value;
                    else
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                }
                ViewState["ID"] = strin;
            }
            else if (ddlSel.SelectedValue == "B")
            {
                ViewState["ID"] = null;
                chkBr.Enabled = true;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                    chkBr.Items[vRow].Selected = false;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
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
        /// <param name="pFormat"></param>
        private void GetData(string pFormat)
        {
            DateTime vFromDt = gblFuction.setDate(txtFrmDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vAcMst = Session[gblValue.ACVouMst].ToString();
            string vAcDtl = Session[gblValue.ACVouDtl].ToString();
            Int32 vFinYrNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            string vRptPath = "";
            string vBranch = Session[gblValue.BrName].ToString();
            GetBranch();

            string vBrCode = Convert.ToString(ViewState["ID"]);
            //Int32 vEoID = 0;
            DataTable dt = null;
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());

            if (vFinFrom > vFromDt || vFromDt > vFinTo)
            {
                gblFuction.MsgPopup("From Date should be within this financial year.");
                return;
            }
            if (vFinFrom > vToDt || vToDt > vFinTo)
            {
                gblFuction.MsgPopup("To date should be within this financial year.");
                return;
            }

            TimeSpan t = vToDt - vFromDt;
            if (t.TotalDays > 2)
            {
                gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                return;
            }

            //ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();
            dt = oRpt.rptRecPay(vFromDt, vToDt, vAcMst, vAcDtl, vBrCode, vFinYrNo, vFinFrom);
            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RecPay.rpt";
            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1("0000"));
                rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2("0000"));
                rptDoc.SetParameterValue("Branch", vBranch);
                rptDoc.SetParameterValue("pTitle", "Consolidate Receipt & Payment Report");
                rptDoc.SetParameterValue("dtFrom", txtFrmDt.Text);
                rptDoc.SetParameterValue("dtTo", txtToDt.Text);

                if (pFormat == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_Consolidate_Receipt_Payement");
                else if (pFormat == "Excel")
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_Consolidate_Receipt_Payement");
                rptDoc.Dispose();
                Response.ClearHeaders();
                Response.ClearContent();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidateDate()
        {
            bool vRst = true;
            return vRst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            if (ValidateDate() == false)
            {
                gblFuction.MsgPopup("Please Set The Valid Date Range.");
                return;
            }
            else
            {
                GetData("PDF");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            if (ValidateDate() == false)
            {
                gblFuction.MsgPopup("Please Set The Valid Date Range.");
                return;
            }
            else
            {
                GetData("Excel");
            }
        }
    }
}
