using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FORCECA;
using FORCEBA;
using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;


namespace CENTRUM.WebPages.Private.Report
{
    public partial class RptPettyCashCert : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch(Session[gblValue.UserName].ToString());
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Petty Cash Certificate";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuPettyCashCertRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Petty Cash Certificate", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void GetData(string pFormat)
        {
            GetBranch();
            string vBrCode = Convert.ToString(ViewState["ID"]);
            string vRptPath = "";
            string vBranch = Session[gblValue.BrName].ToString();
            //string vAcMst = Session[gblValue.ACVouMst].ToString();
            //string vAcDtl = Session[gblValue.ACVouDtl].ToString();
            Int32 vFinYr = Convert.ToInt32(Session[gblValue.FinYrNo]);
            //string vFileNm = "attachment;filename=Petty_Cash_Balance_" + DateTime.Now + ".xls";
            DataTable dt = null;
            CReports oRpt = new CReports();
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());

            if (vFinFrom > gblFuction.setDate(txtFrmDt.Text) || gblFuction.setDate(txtFrmDt.Text) > vFinTo)
            {
                gblFuction.MsgPopup("As On Date should be within this financial year.");
                return;
            }
            //if (vFinFrom > gblFuction.setDate(txtToDt.Text) || gblFuction.setDate(txtToDt.Text) > vFinTo)
            //{
            //    gblFuction.MsgPopup("To date should be within this financial year.");
            //    return;
            //}

            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\PettyCashCert.rpt";

            dt = oRpt.RptPettyCashCert(vFinFrom, gblFuction.setDate(txtFrmDt.Text), Session[gblValue.BrnchCode].ToString(), vFinYr, "P0001");           
            if (dt.Rows.Count > 0)
            {
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pTitle", "Petty Cash Certificate");
                    rptDoc.SetParameterValue("pBranch", vBranch);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()));
                    rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()));
                    rptDoc.SetParameterValue("pFrom", txtFrmDt.Text);
                    rptDoc.SetParameterValue("pTo", txtToDt.Text);

                    if (pFormat == "PDF")
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Petty Cash Certificate");
                    else if (pFormat == "Excel")
                        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Petty Cash Certificate");

                    rptDoc.Dispose();
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
            else
            {
                gblFuction.AjxMsgPopup("No Data Found.");
            }            
        }

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

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
            if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToString(row["BranchCode"]) != Convert.ToString(Session[gblValue.BrnchCode]))
                    {
                        row.Delete();
                    }
                }
                dt.AcceptChanges();
            }
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

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            GetData("PDF");
        }

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            GetData("Excel");
        }

        protected void ddlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
        }

        protected void btnExit_Click1(object sender, EventArgs e)
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
    }
}