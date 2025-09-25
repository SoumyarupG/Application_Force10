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

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class CashRecon : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!Page.IsPostBack)
            {
                ViewState["Adjustment"] = null;
                ViewState["Deposit"] = null;
                ViewState["Collection"] = null;
                txtDate.Text = Session[gblValue.LoginDate].ToString();
                lblBranch.Text = Session[gblValue.BrName].ToString();
                StatusButton("View");
                LoadGrid();
                DataTable dt = null;
                CBranch oBr = null;

                oBr = new CBranch();
                dt = oBr.GetBranchDetails(Session[gblValue.BrnchCode].ToString());
                if (dt.Rows.Count > 0)
                {
                    ViewState["CBSYN"] = Convert.ToString(dt.Rows[0]["CBSApplicableYN"]);
                }
                else
                {
                    ViewState["CBSYN"] = "N";
                }
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
                this.PageHeading = "Cash Reconciliation Request";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCashRecon);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = true;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
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
                
                
                FillDtl();
            }
            finally
            {
               
            }
        }

        private void FillDtl()
        {
            BindAdjustment();
            DataSet ds = null;
            DataTable dt = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            DataTable dt4 = null;
            DataTable dt5,dt6 = null;
            CCashRecon oCashRe = null;
            DateTime vDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vBranch = Session[gblValue.BrnchCode].ToString();
            DateTime vFinStDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            int vYrNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
            try
            {
                oCashRe = new CCashRecon();
                ds = oCashRe.GetCashReconDtl(vDate, vDate, Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), vBranch, vFinStDt, vYrNo);
                dt = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[2];
                dt4 = ds.Tables[3];
                dt5 = ds.Tables[4];
                dt6 = ds.Tables[5];

                txtDate.Text = Session[gblValue.LoginDate].ToString();
                txtOpnCash.Text = Convert.ToString(dt.Rows[0]["OpeningBal"]);
                txtExcessColl.Text = Convert.ToString(dt5.Rows[0]["ExcessColl"]);
                txtCollCash.Text = Convert.ToString(Convert.ToDouble(dt3.Rows[0]["CollectionAmt"]) + Convert.ToDouble(dt5.Rows[0]["ExcessColl"]));
                gvDeposit.DataSource = dt2;
                gvDeposit.DataBind();
                ViewState["Deposit"] = dt2;
                object sumAmt;
                if (dt2.Rows.Count > 0)
                {
                    sumAmt = dt2.Compute("Sum(DrAmt)", string.Empty);
                }
                else
                {
                    sumAmt = "0";
                }

                //---------------------------------------
                gvEoWiseColl.DataSource = dt4;
                gvEoWiseColl.DataBind();
                ViewState["Collection"] = dt4;
                //---------------------------------------
                txtTotalDepo.Text = sumAmt.ToString();
                txtTransAmt.Text=Convert.ToString(dt6.Rows[0]["Tramt"]);
                txtClosingCash.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtOpnCash.Text) + Convert.ToDouble(txtCollCash.Text) + Convert.ToDouble(txtTransAmt.Text)) - Convert.ToDouble(sumAmt), 2));
                txtAdjClosingCash.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtClosingCash.Text) + Convert.ToDouble(txtTotalAdj.Text), 2));
                txtTotalAdjClosingCash.Text = Convert.ToString(Math.Ceiling(Convert.ToDouble(txtAdjClosingCash.Text)));
            }
            finally
            {
                ds = null;
                dt = null;
                dt2 = null;
                dt3 = null;
                dt4 = null;
                dt5 = null;
                dt6 = null;
                oCashRe = null;
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
                    btnAdd.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
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
            ddlCashBalRemarks.SelectedIndex = -1;
            txtOthRem.Text = "";
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
            chkTransfer.Enabled = Status;

            if (Convert.ToString(ViewState["CBSYN"]) == "N")
            {
                chkTransfer.Enabled = false;
                txt2000Trans.Enabled = false;
                txt500Trans.Enabled = false;
                txt200Trans.Enabled = false;
                txt100Trans.Enabled = false;
                txt50Trans.Enabled = false;
                txt20Trans.Enabled = false;
                txt10Trans.Enabled = false;
                txt10CoinTrans.Enabled = false;
                txt5CoinTrans.Enabled = false;
                txt2CoinTrans.Enabled = false;
                txt1CoinTrans.Enabled = false;
            }
            ddlCashBalRemarks.Enabled = Status;
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
            txtOthRem.Enabled = Status;
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                if (vStateEdit == "" || vStateEdit == null)
                    vStateEdit = "Save";
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
            string vXmlDeposit, vXmlAdjustment, vXmlCollection = string.Empty;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            Boolean vResult = false;
            int vErr = 0, vTransfer = 0;
            CCashRecon oCaRe = null;
            //---------------------------------------------------------------
            DataTable dt = (DataTable)ViewState["Deposit"];
            dt.TableName = "Table1";
            dt.AcceptChanges();
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vXmlDeposit = oSW.ToString();
            }
            //---------------------------------------------------------------
            DataTable dt1 = (DataTable)ViewState["Adjustment"];
            dt1.TableName = "Table1";
            dt1.AcceptChanges();
            using (StringWriter oSW = new StringWriter())
            {
                dt1.WriteXml(oSW);
                vXmlAdjustment = oSW.ToString();
            }
            //---------------------------------------------------------------
            DataTable dtColl = (DataTable)ViewState["Collection"];
            dtColl.TableName = "Table1";
            dtColl.AcceptChanges();
            using (StringWriter oSW = new StringWriter())
            {
                dtColl.WriteXml(oSW);
                vXmlCollection = oSW.ToString();
            }

            if (chkTransfer.Checked == true)
            {
                vTransfer = 1;
            }

            

            //---------------------------------------------------------------
            try
            {
                if (Convert.ToString(ViewState["CBSYN"]) == "Y")
                {
                    if (chkTransfer.Checked == false && Convert.ToDouble(txtTotalAdjClosingCash.Text) > 0 && ddlCashBalRemarks.SelectedValue == "-1")
                    {
                        gblFuction.AjxMsgPopup("CBS Branch can not keep closing balance without valid reason.");
                        return false;
                    }
                    else if (chkTransfer.Checked == true && Convert.ToDouble(txtTransferAmt.Text) > 0 && ddlCashBalRemarks.SelectedValue != "-1")
                    {
                        gblFuction.AjxMsgPopup("CBS Branch Closing balance transferred so reason should be blank.");
                        return false;
                    }
                    if (chkTransfer.Checked == false && Convert.ToDouble(txtTotalAdjClosingCash.Text) == 0 && ddlCashBalRemarks.SelectedValue != "-1")
                    {
                        gblFuction.AjxMsgPopup("Closing balance is zero so reason should be blank.");
                        return false;
                    }

                    if (ddlCashBalRemarks.SelectedValue == "3")
                    {
                        if (txtOthRem.Text.Trim() == "")
                        {
                            gblFuction.AjxMsgPopup("Other Remarks should not be blank.");
                            return false;
                        }
                    }
                }
                else
                {
                    if (ddlCashBalRemarks.SelectedValue != "-1")
                    {
                        gblFuction.AjxMsgPopup("Reason should be blank.");
                        return false;
                    }
                }

                oCaRe = new CCashRecon();

                vErr = oCaRe.SaveCashReconciliation(vBrCode, vDate, Convert.ToDouble(txtOpnCash.Text == "" ? "0" : txtOpnCash.Text), Convert.ToDouble(txtCollCash.Text == "" ? "0" : txtCollCash.Text),
                    Convert.ToDouble(txtClosingCash.Text == "" ? "0" : txtClosingCash.Text), Convert.ToDouble(txtAdjClosingCash.Text == "" ? "0" : txtAdjClosingCash.Text), vXmlDeposit, vXmlAdjustment,
                    Convert.ToDouble(txtTotalDepo.Text == "" ? "0" : txtTotalDepo.Text), Convert.ToDouble(txtTotalAdj.Text == "" ? "0" : txtTotalAdj.Text), Convert.ToDouble(txtTotalDeno.Text),
                    Convert.ToDouble(txt2000.Text == "" ? "0" : txt2000.Text), Convert.ToDouble(txt500.Text == "" ? "0" : txt500.Text),
                    Convert.ToDouble(txt200.Text == "" ? "0" : txt200.Text), Convert.ToDouble(txt100.Text == "" ? "0" : txt100.Text), Convert.ToDouble(txt50.Text == "" ? "0" : txt50.Text),
                    Convert.ToDouble(txt20.Text == "" ? "0" : txt20.Text), Convert.ToDouble(txt10.Text == "" ? "0" : txt10.Text), Convert.ToDouble(txt10Coin.Text == "" ? "0" : txt10Coin.Text),
                    Convert.ToDouble(txt5Coin.Text == "" ? "0" : txt5Coin.Text), Convert.ToDouble(txt2Coin.Text == "" ? "0" : txt2Coin.Text), Convert.ToDouble(txt1Coin.Text == "" ? "0" : txt1Coin.Text),
                    Convert.ToInt32(Session[gblValue.UserId]), vXmlCollection, Convert.ToDouble(txtExcessColl.Text == "" ? "0" : txtExcessColl.Text), Convert.ToDouble(txtTransAmt.Text == "" ? "0" : txtTransAmt.Text),
                    vTransfer, Convert.ToDouble(txtTransferAmt.Text == "" ? "0" : txtTransferAmt.Text), Convert.ToDouble(txtTotalDenoTrans.Text),
                    Convert.ToDouble(txt2000Trans.Text == "" ? "0" : txt2000Trans.Text), Convert.ToDouble(txt500Trans.Text == "" ? "0" : txt500Trans.Text),
                    Convert.ToDouble(txt200Trans.Text == "" ? "0" : txt200Trans.Text), Convert.ToDouble(txt100Trans.Text == "" ? "0" : txt100Trans.Text), Convert.ToDouble(txt50Trans.Text == "" ? "0" : txt50Trans.Text),
                    Convert.ToDouble(txt20Trans.Text == "" ? "0" : txt20Trans.Text), Convert.ToDouble(txt10Trans.Text == "" ? "0" : txt10Trans.Text), Convert.ToDouble(txt10CoinTrans.Text == "" ? "0" : txt10CoinTrans.Text),
                    Convert.ToDouble(txt5CoinTrans.Text == "" ? "0" : txt5CoinTrans.Text), Convert.ToDouble(txt2CoinTrans.Text == "" ? "0" : txt2CoinTrans.Text), Convert.ToDouble(txt1CoinTrans.Text == "" ? "0" : txt1CoinTrans.Text),
                    ddlCashBalRemarks.SelectedValue, txtOthRem.Text
                    );

                if (vErr == 0)
                {
                    vResult = true;
                }
                else if (vErr == 2)
                {
                    gblFuction.AjxMsgPopup("Already Cash Reconciliation Done For the Day.");
                    vResult = false;
                }
                else if (vErr == 3)
                {
                    gblFuction.AjxMsgPopup("Pre Closure approval pending For the Day.");
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
            pReconId = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                foreach (GridViewRow gr in gvCashReconList.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;
            }
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
                lblOthRem.Visible = true;
            }
            else
            {
                txtOthRem.Visible = false;
                lblOthRem.Visible = false;
            }

            upOthRem.Update();

            tabCashRecon.ActiveTabIndex = 1;
            StatusButton("Show");
            EnableControl(false);
        }

        protected void chkTransfer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTransfer.Checked == true)
            {
                if (Convert.ToDouble(txtTotalAdjClosingCash.Text) > 0)
                {
                    txtTransferAmt.Text = txtTotalAdjClosingCash.Text;
                    ViewState["AdjClosingCash"] = txtAdjClosingCash.Text;
                    txtTotalAdjClosingCash.Text = "0";
                    txtAdjClosingCash.Text = "0";
                    //Denomination blank
                    txtTotalDeno.Text = "0";
                    txt2000.Text = "0";
                    txt500.Text = "0";
                    txt200.Text = "0";
                    txt100.Text = "0";
                    txt50.Text = "0";
                    txt20.Text = "0";
                    txt10.Text = "0";
                    txt10Coin.Text = "0";
                    txt5Coin.Text = "0";
                    txt2Coin.Text = "0";
                    txt1Coin.Text = "0";
                }
                else
                {
                    gblFuction.AjxMsgPopup("Closing balance is zero so Transfer not possible.");
                    chkTransfer.Checked = false;
                }
            }
            else
            {
                txtTotalAdjClosingCash.Text = txtTransferAmt.Text;
                txtTransferAmt.Text = "0";
                txtAdjClosingCash.Text = Convert.ToString(ViewState["AdjClosingCash"]);
                //Transfer Denomination blank
                txtTotalDenoTrans.Text = "0";
                txt2000Trans.Text = "0";
                txt500Trans.Text = "0";
                txt200Trans.Text = "0";
                txt100Trans.Text = "0";
                txt50Trans.Text = "0";
                txt20Trans.Text = "0";
                txt10Trans.Text = "0";
                txt10CoinTrans.Text = "0";
                txt5CoinTrans.Text = "0";
                txt2CoinTrans.Text = "0";
                txt1CoinTrans.Text = "0";
            }
        }
        protected void ddlCashBalRemarks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCashBalRemarks.SelectedValue == "3")
            {
                lblOthRem.Visible = true;
                txtOthRem.Visible = true;
            }
            else
            {
                lblOthRem.Visible = false;
                txtOthRem.Visible = false;
                txtOthRem.Text = "";
            }
        }
    }
}