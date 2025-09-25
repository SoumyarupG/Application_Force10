using System;
using FORCECA;
using System.Web.UI.WebControls;
using System.Data;
using FORCEBA;
using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class BranchFundTrChecker : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDt.Text = Session[gblValue.LoginDate].ToString();
                PopDtl(txtDt.Text.Trim());
                btnSave.Enabled = true;
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "HO To Branch Fund Transfer Checker";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBrFundTrChecker);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "HO To Branch Fund Transfer Checker", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void PopDtl(string pDate)
        {
            CBranchFundTr oBT = null;
            DataTable dt = null;
            DateTime vDate = gblFuction.setDate(pDate);
            try
            {
                oBT = new CBranchFundTr();
                dt = oBT.GetBrFundTrUnApprvDetails(Session[gblValue.BrnchCode].ToString(), vDate);
                dt.Columns.Add("BrDt");
                foreach (DataRow dr in dt.Rows)
                {
                    dr["BrDt"] = txtDt.Text.Trim();
                }
                gvDtl.DataSource = dt;
                gvDtl.DataBind();
            }
            catch
            {
                gvDtl.DataSource = null;
                gvDtl.DataBind();
            }
            finally
            {
                oBT = null;
                dt = null;
            }
        }
        protected void gvDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //CVoucher oVoucher = null;
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{               
            //}
        }
        private String XmlList()
        {
            double vTotAmt = 0;
            String vSpecXML = "";
            DataTable dt = new DataTable("List");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("HOBankID");
            dt.Columns.Add("BrBankID");
            dt.Columns.Add("BrDt");
            dt.Columns.Add("FundTrDt");
            dt.Columns.Add("Amount");
            dt.Columns.Add("IsChk");
            dt.Columns.Add("TrID");
            dt.Columns.Add("ReffID");

            foreach (GridViewRow gR in gvDtl.Rows)
            {
                CheckBox chkTr = (CheckBox)gR.FindControl("chkTr");
                HiddenField hdHOBankId = (HiddenField)gR.FindControl("hdHOBankId");
                HiddenField hdBrBankId = (HiddenField)gR.FindControl("hdBrBankId");
                HiddenField hdBranchCode = (HiddenField)gR.FindControl("hdBranchCode");
                HiddenField hdTrID = (HiddenField)gR.FindControl("hdTrID");
                HiddenField hdReffID = (HiddenField)gR.FindControl("hdReffID");
                Label lblAmt = (Label)gR.FindControl("lblAmt");
                Label lblFundTrDt = (Label)gR.FindControl("lblFundTrDt");

                if (chkTr.Checked == true)
                {
                    DataRow dr = dt.NewRow();
                    dr["BranchCode"] = Convert.ToString(hdBranchCode.Value).Trim();
                    dr["HOBankID"] = Convert.ToString(hdHOBankId.Value).Trim();
                    dr["BrBankID"] = Convert.ToString(hdBrBankId.Value).Trim();
                    dr["BrDt"] = gblFuction.setDate(txtDt.Text.Trim()).ToString();
                    dr["FundTrDt"] = gblFuction.setDate(lblFundTrDt.Text.Trim()).ToString();
                    dr["Amount"] = lblAmt.Text.Trim();
                    vTotAmt = vTotAmt + Convert.ToDouble(lblAmt.Text.Trim());
                    dr["IsChk"] = chkTr.Checked == true ? "Y" : "N";
                    dr["TrID"] = Convert.ToString(hdTrID.Value).Trim();
                    dr["ReffID"] = Convert.ToString(hdReffID.Value).Trim();
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
            }
            using (StringWriter oSW = new StringWriter())
            {
                ViewState["TotAmt"] = vTotAmt;
                dt.WriteXml(oSW);
                vSpecXML = oSW.ToString();
            }
            return vSpecXML;
        }
        protected void txtDt_TextChanged(object sender, EventArgs e)
        {
            PopDtl(txtDt.Text.Trim());
        }
        protected void chkTr_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTr = (CheckBox)sender;
            GridViewRow gR = (GridViewRow)chkTr.NamingContainer;
            Label lblFundTrDt = (Label)gR.FindControl("lblFundTrDt");
            Label lblAmt = (Label)gR.FindControl("lblAmt");
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());

            if (gblFuction.setDate(lblFundTrDt.Text) < vFinFromDt || gblFuction.setDate(lblFundTrDt.Text) > vFinToDt)
            {
                chkTr.Checked = false;
                gblFuction.AjxMsgPopup("Transfer Date should login financial year.");
                return;
            }
            else if (lblAmt.Text.ToString() == "" || lblAmt.Text.ToString() == "0")
            {
                chkTr.Checked = false;
                gblFuction.AjxMsgPopup("Amount missing");
                return;
            }
            else if (this.RoleId != 1 && this.RoleId != 12)
            {
                if (Session[gblValue.EndDate] != null)
                {
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(lblFundTrDt.Text.Trim()))
                    {
                        chkTr.Checked = false;
                        gblFuction.AjxMsgPopup("Day end already done..");
                        return;
                    }
                }
            }
            Total(gR);
        }
        protected int SaveRecords()
        {
            int vRes = 0; string vXml = "";
            CBranchFundTr oBF = null;
            DateTime vDate = gblFuction.setDate(txtDt.Text.Trim());
            int vUserID = this.UserID;
            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vTblDtl = Session[gblValue.ACVouDtl].ToString();
            String vFinYear = Session[gblValue.ShortYear].ToString();
            int vYearNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
            String vBranchCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                vXml = XmlList();
                if (vXml == "" || vXml == "<DocumentElement />")
                {
                    gblFuction.MsgPopup("No Records to Save...");
                    return 2;
                }
                if (Convert.ToDouble(txtGrdTot.Text) != Convert.ToDouble(ViewState["TotAmt"]))
                {
                    gblFuction.MsgPopup("Total Amount Mis Match.");
                    return 2;
                }
                else
                {
                    oBF = new CBranchFundTr();
                    vRes = oBF.SaveBrFundTrCheckerDtls(vXml, vDate, vTblMst, vTblDtl, vFinYear, vYearNo, vBranchCode, vUserID);
                    return vRes;
                }
            }
            catch
            {
                return 1;
            }
        }
        protected void btnPrn_Click(object sender, EventArgs e)
        {
            DateTime vFromDt = gblFuction.setDate(txtDt.Text.Trim());
            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vRptPath = "";
            string vTitle = "Payment Voucher";
            ReportDocument rptDoc = new ReportDocument();
            DataTable dt = new DataTable();
            CReports oVoucher = new CReports();
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = Session[gblValue.BrnchCode].ToString();

            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\FundTrVoucher.rpt";
            dt = oVoucher.rptFundTransVoucher(vTblMst);
            rptDoc.Load(vRptPath);
            rptDoc.SetDataSource(dt);
            rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
            rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
            rptDoc.SetParameterValue("pAddress2", "");

            rptDoc.SetParameterValue("pBranch", vBranch);
            rptDoc.SetParameterValue("dtFrom", txtDt.Text);
            rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Fund_Transfer_Voucher");

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int vRes = 0;
            vRes = SaveRecords();
            gvDtl.DataSource = null;
            gvDtl.DataBind();
            btnSave.Enabled = false;
            if (vRes == 0)
                gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
            else if (vRes == 1)
                gblFuction.AjxMsgPopup(gblMarg.DBError);
            else { }
                //gblFuction.AjxMsgPopup("Validation Error !");
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
        private void Total(GridViewRow gvr)
        {
            int rowindex = 0, vMaxRow = 0;
            double vTotAmt = 0;
            rowindex = gvr.RowIndex;
            vMaxRow = gvDtl.Rows.Count;
            foreach (GridViewRow gr in gvDtl.Rows)
            {
                Label lblAmt = (Label)gr.FindControl("lblAmt");
                CheckBox chkTr = (CheckBox)gr.FindControl("chkTr");
                if (chkTr.Checked == true)
                {
                    if (lblAmt.Text != "" && lblAmt.Text != null && chkTr.Checked == true) vTotAmt += Convert.ToDouble(lblAmt.Text);
                }
            }
            txtGrdTot.Text = vTotAmt.ToString();
            UpTot.Update();
            upDtl.Update();
        }

        //protected void chkSelectAll_CheckedChange(object sender, EventArgs e)
        //{
        //    double vTotAmt = 0;
        //    GridViewRow row = gvDtl.HeaderRow;
            
        //    CheckBox chkSelectAll = (CheckBox)row.FindControl("chkSelectAll");
        //    if (chkSelectAll.Checked == true)
        //    {
        //        foreach (GridViewRow gR in gvDtl.Rows)
        //        {
        //            CheckBox chkTr = (CheckBox)gR.FindControl("chkTr");
        //            chkTr.Checked = true;
        //        }
        //    }

        //    else
        //    {
        //        foreach (GridViewRow gR in gvDtl.Rows)
        //        {
        //            CheckBox chkTr = (CheckBox)gR.FindControl("chkTr");
        //            chkTr.Checked = false;
        //        }
        //    }
        //    foreach (GridViewRow gr in gvDtl.Rows)
        //    {
        //        Label lblAmt = (Label)gr.FindControl("lblAmt");
        //        CheckBox chkTr = (CheckBox)gr.FindControl("chkTr");
        //        if (chkTr.Checked == true)
        //        {
        //            if (lblAmt.Text != "" && lblAmt.Text != null && chkTr.Checked == true) vTotAmt += Convert.ToDouble(lblAmt.Text);
        //        }
        //    }
        //    txtGrdTot.Text = vTotAmt.ToString();
        //}
    }
}