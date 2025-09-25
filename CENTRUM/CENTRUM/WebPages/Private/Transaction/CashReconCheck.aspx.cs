using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FORCECA;
using FORCEBA;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class CashReconCheck : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!Page.IsPostBack)
            {
                ViewState["ReconId"] = null;
                ViewState["BranchCode"] = null;
                txtDate.Text = Session[gblValue.LoginDate].ToString();
                // lblBranch.Text = Session[gblValue.BrName].ToString();
                StatusButton("View");
                LoadGrid();
            }
        }

        private void LoadGrid()
        {
            CCashRecon oCRecon = new CCashRecon();
            DataTable dt = new DataTable();
            dt = oCRecon.GetCashReconList(Session[gblValue.BrnchCode].ToString());
            gvCashReconList.DataSource = dt;
            gvCashReconList.DataBind();
        }

        private void BindAdjustment()
        {
            DataTable dt = new DataTable("Tr");
            dt.Columns.Add("Perticulars");
            dt.Columns.Add("Amount");
            DataRow dF;
            dF = dt.NewRow();
            dt.Rows.Add(dF);
            ViewState["Adjustment"] = dt;
            gvAdjustment.DataSource = dt;
            gvAdjustment.DataBind();
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Cash Reconciliation Approval";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCashReconCheck);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnAdd.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    btnApprove.Visible = false;
                    btnSendBack.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnAdd.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    btnApprove.Visible = false;
                    btnSendBack.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    btnApprove.Visible = false;
                    btnSendBack.Visible = false;
                }
                else if (this.CanProcess == "Y")
                {
                    btnAdd.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;                   
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanProcess != "Y")
                {
                    btnApprove.Visible = false;
                    btnSendBack.Visible = false;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Cash Reconciliation", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["StateEdit"] = "Add";
                tabCashRecon.ActiveTabIndex = 1;
                StatusButton("Add");
                // FillDtl();
            }
            finally
            {
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabCashRecon.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            Int32 vR = 0;
            double vAmt = 0;
            DataRow dr;
            dt = (DataTable)ViewState["Adjustment"];
            if (dt.Rows.Count > 0)
            {
                vR = dt.Rows.Count - 1;
                TextBox txtClosingCash = (TextBox)gvAdjustment.Rows[vR].FindControl("txtClosingCash");
                TextBox txtAmt = (TextBox)gvAdjustment.Rows[vR].FindControl("txtAmt");
                if (txtClosingCash.Text == "")
                {
                    gblFuction.AjxMsgPopup("Perticulars cannot be Blank...");
                    return;
                }
                else if (txtAmt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Amount cannot be Blank...");
                    return;
                }
                dt.Rows[vR]["Perticulars"] = txtClosingCash.Text;
                dt.Rows[vR]["Amount"] = txtAmt.Text;
                vAmt = Convert.ToDouble(txtAmt.Text);
            }
            dt.AcceptChanges();

            if (dt.Rows[vR]["Perticulars"].ToString() != "")
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            else
            {
                gblFuction.MsgPopup("Perticulars cannot be Blank...");
                return;
            }
            //------------------------------------------------------------------
            txtTotalAdj.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtTotalAdj.Text) + vAmt, 2));
            txtAdjClosingCash.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtAdjClosingCash.Text) + vAmt, 2));
            txtTotalAdjClosingCash.Text = Convert.ToString(Math.Ceiling(Convert.ToDouble(txtAdjClosingCash.Text)));
            //-------------------------------------------------------------------
            ViewState["Adjustment"] = dt;
            gvAdjustment.DataSource = dt;
            gvAdjustment.DataBind();
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnAdd.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnApprove.Enabled = true;
                    btnSendBack.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnApprove.Enabled = false;
                    btnSendBack.Enabled = false;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        private void ClearControls()
        {
            txtOpnCash.Text = "0.00";
            txtCollCash.Text = "0.00";
            txtTotalDepo.Text = "0.00";
            txtClosingCash.Text = "0.00";
            txtTotalAdj.Text = "0.00";
            txtAdjClosingCash.Text = "0.00";
            txtTotalAdjClosingCash.Text = "0.00";
            txtTotalDeno.Text = "0.00";
            txtExcessColl.Text = "0.00";
            txtTransAmt.Text = "0.00";
            txt2000.Text = "";
            txt500.Text = "";
            txt200.Text = "";
            txt100.Text = "";
            txt50.Text = "";
            txt20.Text = "";
            txt10.Text = "";
            txt10Coin.Text = "";
            txt5Coin.Text = "";
            txt2Coin.Text = "";
            txt1Coin.Text = "";
            ViewState["Adjustment"] = null;
            ViewState["Collection"] = null;

            chkTransfer.Checked = false;
            txtTransferAmt.Text = "0";
            txtTotalDenoTrans.Text = "0";
            txt2000Trans.Text = "";
            txt500Trans.Text = "";
            txt200Trans.Text = "";
            txt100Trans.Text = "";
            txt50Trans.Text = "";
            txt20Trans.Text = "";
            txt10Trans.Text = "";
            txt10CoinTrans.Text = "";
            txt5CoinTrans.Text = "";
            txt2CoinTrans.Text = "";
            txt1CoinTrans.Text = "";
        }

        private void EnableControl(Boolean Status)
        {
            txtDate.Enabled = false;
            txtOpnCash.Enabled = Status;
            txtCollCash.Enabled = Status;
            txtTotalDepo.Enabled = Status;
            txtClosingCash.Enabled = Status;
            gvAdjustment.Enabled = Status;
            txtTotalAdj.Enabled = Status;
            txtAdjClosingCash.Enabled = Status;
            txtTotalAdjClosingCash.Enabled = Status;
            txtTotalDeno.Enabled = Status;

            txt2000.Enabled = Status;
            txt500.Enabled = Status;
            txt200.Enabled = Status;
            txt100.Enabled = Status;
            txt50.Enabled = Status;
            txt20.Enabled = Status;
            txt10.Enabled = Status;
            txt10Coin.Enabled = Status;
            txt5Coin.Enabled = Status;
            txt2Coin.Enabled = Status;
            txt1Coin.Enabled = Status;

            chkTransfer.Enabled = Status;
            txt2000Trans.Enabled = Status;
            txt500Trans.Enabled = Status;
            txt200Trans.Enabled = Status;
            txt100Trans.Enabled = Status;
            txt50Trans.Enabled = Status;
            txt20Trans.Enabled = Status;
            txt10Trans.Enabled = Status;
            txt10CoinTrans.Enabled = Status;
            txt5CoinTrans.Enabled = Status;
            txt2CoinTrans.Enabled = Status;
            txt1CoinTrans.Enabled = Status;
            ddlCashBalRemarks.Enabled = Status;
            
            txtOthRem.Enabled = Status;
            if (ddlCashBalRemarks.SelectedValue == "3")
            {
                lblOthRem.Visible = true;
                txtOthRem.Visible = true;
            }
            else
            {
                lblOthRem.Visible = false;
                txtOthRem.Visible = false;

            }
        }

        protected void gvAdjustment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dt = null;
            try
            {
                if (e.CommandName == "cmdDel")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtAmt = (TextBox)row.FindControl("txtAmt");
                    TextBox txtClosingCash = (TextBox)row.FindControl("txtClosingCash");
                    if (txtAmt.Text == "" || txtClosingCash.Text == "")
                    {
                        gblFuction.AjxMsgPopup("You can not delete this row.");
                        return;
                    }

                    dt = (DataTable)ViewState["Adjustment"];
                    int index = row.RowIndex;
                    if (dt.Rows.Count > 1)
                    {
                        if (txtAmt.Text != "")
                        {
                            txtTotalAdj.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtTotalAdj.Text) - Convert.ToDouble(txtAmt.Text), 2));
                            txtAdjClosingCash.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtAdjClosingCash.Text) - Convert.ToDouble(txtAmt.Text), 2));
                            txtTotalAdjClosingCash.Text = Convert.ToString(Math.Ceiling(Convert.ToDouble(txtAdjClosingCash.Text)));
                        }
                        dt.Rows[index].Delete();
                        dt.AcceptChanges();
                        gvAdjustment.DataSource = dt;
                        gvAdjustment.DataBind();
                    }
                    else
                    {
                        gblFuction.MsgPopup("First Row can not be deleted.");
                        return;
                    }
                    ViewState["Adjustment"] = dt;
                }
            }
            finally
            {

            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                if (vStateEdit == "" || vStateEdit == null)
                    vStateEdit = "A";
                if (SaveRecords(vStateEdit) == true)
                {
                    gblFuction.MsgPopup(gblMarg.SaveMsg);
                    StatusButton("View");
                    ViewState["StateEdit"] = null;
                    EnableControl(false);
                    LoadGrid();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSendBack_Click(object sender, EventArgs e)
        {
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                if (vStateEdit == "" || vStateEdit == null)
                    vStateEdit = "S";
                if (SaveRecords(vStateEdit) == true)
                {
                    gblFuction.MsgPopup(gblMarg.SaveMsg);
                    StatusButton("View");
                    ViewState["StateEdit"] = null;
                    EnableControl(false);
                    LoadGrid();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private Boolean SaveRecords(string Mode)
        {
            DateTime vDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            Boolean vResult = false;
            int vErr = 0, pReconId = 0;
            CCashRecon oCaRe = null;
            //---------------------------------------------------------            
            try
            {
                oCaRe = new CCashRecon();
                pReconId = Convert.ToInt32(ViewState["ReconId"].ToString());
                if (Mode == "A")
                {
                    vErr = oCaRe.SaveCashReconAppRej(pReconId, gblFuction.setDate(txtDate.Text), Convert.ToString(ViewState["BranchCode"]),
                        "Y", Convert.ToInt32(Session[gblValue.UserId]), "A");
                }
                if (Mode == "S")
                {
                    vErr = oCaRe.SaveCashReconAppRej(pReconId, gblFuction.setDate(txtDate.Text), Convert.ToString(ViewState["BranchCode"]),
                        "N", Convert.ToInt32(Session[gblValue.UserId]), "S");
                }
                if (vErr == 0)
                {
                    vResult = true;
                }
                else if (vErr == 2)
                {
                    gblFuction.AjxMsgPopup("Day End Already done for the day.");
                    vResult = false;
                }
                else if (vErr == 3)
                {
                    gblFuction.AjxMsgPopup("Same User Cannot Request and Approve Cash Recon.");
                    vResult = false;
                }
                else
                {
                    gblFuction.MsgPopup(gblMarg.DBError);
                    vResult = false;
                }

                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gvCashReconList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 pReconId = 0;
            int RowIndex = 0;
            if (e.CommandName == "cmdShow")
            {
                pReconId = Convert.ToInt32(e.CommandArgument);

                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                RowIndex = gvRow.RowIndex;
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                foreach (GridViewRow gr in gvCashReconList.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;


                DataSet ds = null;
                DataTable dt, dt2, dt3, dt4 = null;
                CCashRecon oCashRe = null;
                oCashRe = new CCashRecon();
                ds = oCashRe.GetCashReconDtlById(pReconId);
                dt = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[2];
                dt4 = ds.Tables[3];

                gvDeposit.DataSource = dt3;
                gvDeposit.DataBind();

                gvAdjustment.DataSource = dt2;
                gvAdjustment.DataBind();

                gvEoWiseColl.DataSource = dt4;
                gvEoWiseColl.DataBind();

                ViewState["ReconId"] = pReconId;
                ViewState["BranchCode"] = Convert.ToString(dt.Rows[0]["BranchCode"]);
                lblBranch.Text = Convert.ToString(dt.Rows[0]["BranchName"]);
                txtDate.Text = Convert.ToString(dt.Rows[0]["ReconDate"]);
                txtOpnCash.Text = Convert.ToString(dt.Rows[0]["OpenCash"]);
                txtExcessColl.Text = Convert.ToString(dt.Rows[0]["ExcessColl"]);
                txtCollCash.Text = Convert.ToString(dt.Rows[0]["CollCash"]);
                txtTotalDepo.Text = Convert.ToString(dt.Rows[0]["TotalDeposit"]);
                txtTransAmt.Text = Convert.ToString(dt.Rows[0]["Tramt"]);
                txtClosingCash.Text = Convert.ToString(dt.Rows[0]["ClosingCash"]);
                txtAdjClosingCash.Text = Convert.ToString(dt.Rows[0]["ClosingCashAsPerSystem"]);
                txtTotalAdjClosingCash.Text = Convert.ToString(Math.Ceiling(Convert.ToDouble(txtAdjClosingCash.Text)));
                txtTotalAdj.Text = Convert.ToString(dt.Rows[0]["TotalAdjustment"]);
                txtTotalDeno.Text = Convert.ToString(dt.Rows[0]["TotalDeno"]);

                txt2000.Text = Convert.ToString(dt.Rows[0]["Count2000"]);
                txt500.Text = Convert.ToString(dt.Rows[0]["Count500"]);
                txt200.Text = Convert.ToString(dt.Rows[0]["Count200"]);
                txt100.Text = Convert.ToString(dt.Rows[0]["Count100"]);
                txt50.Text = Convert.ToString(dt.Rows[0]["Count50"]);
                txt20.Text = Convert.ToString(dt.Rows[0]["Count20"]);
                txt10.Text = Convert.ToString(dt.Rows[0]["Count10"]);
                txt10Coin.Text = Convert.ToString(dt.Rows[0]["Count10Coin"]);
                txt5Coin.Text = Convert.ToString(dt.Rows[0]["Count5Coin"]);
                txt2Coin.Text = Convert.ToString(dt.Rows[0]["Count2Coin"]);
                txt1Coin.Text = Convert.ToString(dt.Rows[0]["Count1Coin"]);

                if (Convert.ToString(dt.Rows[0]["CashTransfer"]) == "1")
                {
                    chkTransfer.Checked = true;
                }
                else
                {
                    chkTransfer.Checked = false;
                }

                txtTransferAmt.Text = Convert.ToString(dt.Rows[0]["CashTransferAmount"]);
                txtTotalDenoTrans.Text = Convert.ToString(dt.Rows[0]["TotalDenoTrans"]);
                txt2000Trans.Text = Convert.ToString(dt.Rows[0]["Count2000Trans"]);
                txt500Trans.Text = Convert.ToString(dt.Rows[0]["Count500Trans"]);
                txt200Trans.Text = Convert.ToString(dt.Rows[0]["Count200Trans"]);
                txt100Trans.Text = Convert.ToString(dt.Rows[0]["Count100Trans"]);
                txt50Trans.Text = Convert.ToString(dt.Rows[0]["Count50Trans"]);
                txt20Trans.Text = Convert.ToString(dt.Rows[0]["Count20Trans"]);
                txt10Trans.Text = Convert.ToString(dt.Rows[0]["Count10Trans"]);
                txt10CoinTrans.Text = Convert.ToString(dt.Rows[0]["Count10CoinTrans"]);
                txt5CoinTrans.Text = Convert.ToString(dt.Rows[0]["Count5CoinTrans"]);
                txt2CoinTrans.Text = Convert.ToString(dt.Rows[0]["Count2CoinTrans"]);
                txt1CoinTrans.Text = Convert.ToString(dt.Rows[0]["Count1CoinTrans"]);
                ddlCashBalRemarks.SelectedIndex = ddlCashBalRemarks.Items.IndexOf(ddlCashBalRemarks.Items.FindByValue(dt.Rows[0]["CashBalRemarks"].ToString()));
                txtOthRem.Text = Convert.ToString(dt.Rows[0]["OthRem"]);

                if (dt.Rows[0]["CashBalRemarks"].ToString() == "3")
                {
                    txtOthRem.Visible = true;
                    txtOthRem.Visible = true;
                }
                else
                {
                    txtOthRem.Visible = false;
                    txtOthRem.Visible = false;
                }
                tabCashRecon.ActiveTabIndex = 1;
                StatusButton("Show");
            }
            if (e.CommandName == "cmdCertificate")
            {
                DataTable dt = null;
                CReports oRpt = null;
                string vRptPath = "", vRptName = "";
                GridViewRow gvRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                pReconId = Convert.ToInt32(gvRow.Cells[5].Text);
                dt = new DataTable();
                oRpt = new CReports();
                dt = oRpt.rptCashCerfificate(pReconId);
                if (dt.Rows.Count > 0)
                {
                    using (ReportDocument rptDoc = new ReportDocument())
                    {

                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptCashCertificate.rpt";

                        vRptName = "Cash_Certificate";
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt);
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, vRptName);
                        Response.ClearContent();
                        Response.ClearHeaders();
                    }
                }
            }
            //EnableControl(false);
            if (gvCashReconList.Rows[RowIndex].Cells[4].Text == "Y")
            {
                btnApprove.Enabled = false;
                btnSendBack.Enabled = false;
            }
        }

        

    }
}