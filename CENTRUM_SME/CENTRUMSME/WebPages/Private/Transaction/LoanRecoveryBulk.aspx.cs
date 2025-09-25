using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.Data;
using System.IO;
using System.Web.Security;
using System.Data.OleDb;
using ClosedXML.Excel;
using System.Configuration;
using SendSms;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class LoanRecoveryBulk : CENTRUMBAse
    {
        private static string vExcelPath = ConfigurationManager.AppSettings["DBPath"];
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ClearControls();
                PopBranch();
                PopBank();
                ViewState["StateEdit"] = null;
                txtRecovryDt.Text = (string)Session[gblValue.LoginDate];
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Loan Collection Bulk";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanRecoveryBulk);
                if (this.UserID == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                    //btnDone.Visible = false;
                    //btnDel.Visible = false;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                    // btnDone.Visible = true;
                    //btnDel.Visible = false;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                    // btnDone.Visible = true;
                    //btnDel.Visible = false;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnSave.Visible = true;
                    // btnDone.Visible = true;
                    //btnDel.Visible = true;
                    return;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Collection Bulk", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }

        }
        private void ClearControls()
        {
            //txtLnAmt.Text = "0.00";
            //txtLnDt.Text = "";
            //txtIntRate.Text = "0.00";
            //txtPrinOS.Text = "0.00";
            //txtIntOs.Text = "0.00";
            //txtPrinPay.Text = "0.00";
            //txtIntPay.Text = "0.00";
            ////txtTotAmtPay.Text = "0.00";
            //txtPrinPaid.Text = "0.00";
            //txtIntPaid.Text = "0.00";
            //txtFLDGBal.Text = "0.00";
            ddlBankLedgr.SelectedIndex = -1;
            gvLed.DataSource = null;
            gvLed.DataBind();
        }
        private void PopBranch()
        {
            ddlBranch.Items.Clear();
            CMember oCM = new CMember();
            DataTable dt = new DataTable(); ;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();

                dt = oCM.GetBranchByBrCode(vBrCode);
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataBind();
                    if (vBrCode == "0000")
                    {
                        ListItem oItm = new ListItem("All", "A");
                        ddlBranch.Items.Insert(0, oItm);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCM = null;
            }
        }
        private void PopBank()
        {
            gvLed.DataSource = null;
            gvLed.DataBind();
            gvRecvry.DataSource = null;
            gvRecvry.DataBind();
            Session["dtRst"] = null;
            ddlBankLedgr.Items.Clear();
            DataTable dt = new DataTable();
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBrCode = ddlBranch.SelectedValue.ToString();

            CVoucher oVou = new CVoucher();
            dt = oVou.GetBank(vBrCode);
            if (dt.Rows.Count > 0)
            {
                ddlBankLedgr.DataTextField = "Desc";
                ddlBankLedgr.DataValueField = "DescID";
                ddlBankLedgr.DataSource = dt;
                ddlBankLedgr.DataBind();
            }
            ListItem oItem = new ListItem("<--Select-->", "-1");
            ddlBankLedgr.Items.Insert(0, oItem);
        }
        public static string GetCurrentFinancialYear(DateTime dt)
        {
            int CurrentYear = dt.Year;
            int PreviousYear = dt.Year - 1;
            int NextYear = dt.Year + 1;
            string PreYear = PreviousYear.ToString();
            string NexYear = NextYear.ToString();
            string CurYear = CurrentYear.ToString();
            string FinYear = null;

            if (dt.Month > 3)
                FinYear = CurYear + "-" + NexYear;
            else
                FinYear = PreYear + "-" + CurYear;
            return FinYear.Trim();
        }
        private DataTable GetData()
        {
            DataRow dr = null;
            DataTable dtXml = new DataTable();
            dtXml.Columns.Add("LoanId", typeof(string));
            dtXml.Columns.Add("CustId", typeof(string));
            dtXml.Columns.Add("CustName", typeof(string));
            dtXml.Columns.Add("PrincOS", typeof(decimal));
            dtXml.Columns.Add("PrinPaid", typeof(decimal));
            dtXml.Columns.Add("IntPaid", typeof(decimal));
            dtXml.Columns.Add("WaveIntColl", typeof(decimal));

            dtXml.Columns.Add("PenaltyAmt", typeof(decimal));
            dtXml.Columns.Add("PenCollAmt", typeof(decimal));
            dtXml.Columns.Add("PenWaive", typeof(decimal));
            dtXml.Columns.Add("PenCGSTAmt", typeof(decimal));
            dtXml.Columns.Add("PenSGSTAmt", typeof(decimal));

            dtXml.Columns.Add("VisitCharge", typeof(decimal));
            dtXml.Columns.Add("VisitChargeRec", typeof(decimal));
            dtXml.Columns.Add("VisitChargeWaive", typeof(decimal));
            dtXml.Columns.Add("VisitChargeCGST", typeof(decimal));
            dtXml.Columns.Add("VisitChargeSGST", typeof(decimal));




            dtXml.Columns.Add("BounceAmt", typeof(decimal));
            dtXml.Columns.Add("BounceRecv", typeof(decimal));
            dtXml.Columns.Add("BounceWave", typeof(decimal));
            dtXml.Columns.Add("BounceDue", typeof(decimal));
            dtXml.Columns.Add("TotalPaid", typeof(decimal));
            dtXml.Columns.Add("IntDue", typeof(decimal));

            dtXml.Columns.Add("ODDays", typeof(decimal));
            dtXml.Columns.Add("ODAmt", typeof(decimal));

            dtXml.Columns.Add("BrCode", typeof(string));
            dtXml.Columns.Add("BankNm", typeof(string));
            dtXml.Columns.Add("ChqRefNo", typeof(string));


            Double TotCollAmt = 0;
            foreach (GridViewRow gr in gvRecvry.Rows)
            {
                CheckBox chkRow = (CheckBox)gr.FindControl("chkRow");
                LinkButton lbLoanNo = (LinkButton)gr.FindControl("lbtnShow");
                Label lblCustId = (Label)gr.FindControl("lblCustId");
                Label lblCustName = (Label)gr.FindControl("lblCustName");
                TextBox txtPrincOS = (TextBox)gr.FindControl("txtPrincOS");
                TextBox txtPrinAmt = (TextBox)gr.FindControl("txtPrinAmt");
                TextBox txtIntAmt = (TextBox)gr.FindControl("txtIntAmt");
                TextBox txtIntWaiveAmt = (TextBox)gr.FindControl("txtIntWaiveAmt");
                // For Delay Payment
                TextBox txtDelayPayAmt = (TextBox)gr.FindControl("txtDelayPayAmt");
                TextBox txtDelayRec = (TextBox)gr.FindControl("txtDelayRec");
                TextBox txtDelayWaive = (TextBox)gr.FindControl("txtDelayWaive");
                TextBox txtDelayCGST = (TextBox)gr.FindControl("txtDelayCGST");
                TextBox txtDelaySGST = (TextBox)gr.FindControl("txtDelaySGST");
                // For Visiting Charge
                TextBox txtVisitCharge = (TextBox)gr.FindControl("txtVisitCharge");
                TextBox txtVisitChargeRec = (TextBox)gr.FindControl("txtVisitChargeRec");
                TextBox txtVisitChargeWaive = (TextBox)gr.FindControl("txtVisitChargeWaive");
                TextBox txtVisitChrgeCGST = (TextBox)gr.FindControl("txtVisitChrgeCGST");
                TextBox txtVisitChrgeSGST = (TextBox)gr.FindControl("txtVisitChrgeSGST");
                //For Bounce
                TextBox txtBounceAmt = (TextBox)gr.FindControl("txtBounceAmt");
                TextBox txtBounceRecAmt = (TextBox)gr.FindControl("txtBounceRecAmt");
                TextBox txtBounceWaiveAmt = (TextBox)gr.FindControl("txtBounceWaiveAmt");
                TextBox txtBounceDue = (TextBox)gr.FindControl("txtBounceDue");
                TextBox txtGrandTotAmt = (TextBox)gr.FindControl("txtGrandTotAmt");
                Label lbPreIntDue = (Label)gr.FindControl("lbPreIntDue");

                TextBox txtODDays = (TextBox)gr.FindControl("txtODDays");
                TextBox txtODAmt = (TextBox)gr.FindControl("txtODAmt");

                Label lbBrCode = (Label)gr.FindControl("lbBrCode");
                TextBox txtBankName = (TextBox)gr.FindControl("txtBankName");
                TextBox txtChqRefNo = (TextBox)gr.FindControl("txtChqRefNo");

                if (txtGrandTotAmt.Text != "")
                    TotCollAmt = Convert.ToDouble(txtGrandTotAmt.Text);
                if (chkRow.Checked && TotCollAmt > 0 && lbLoanNo.Text != "")
                {
                    dr = dtXml.NewRow();
                    dr["LoanId"] = lbLoanNo.Text;
                    dr["CustId"] = lblCustId.Text;
                    dr["CustName"] = lblCustName.Text;
                    if (txtPrincOS.Text != "")
                        dr["PrincOS"] = Convert.ToDecimal(txtPrincOS.Text);
                    else
                    {
                        dr["PrincOS"] = "0";
                    }
                    if (txtPrinAmt.Text != "")
                        dr["PrinPaid"] = Convert.ToDecimal(txtPrinAmt.Text);
                    else
                    {
                        dr["PrinPaid"] = "0";
                    }
                    if (txtIntAmt.Text != "")
                        dr["IntPaid"] = Convert.ToDecimal(txtIntAmt.Text);
                    else
                    {
                        dr["IntPaid"] = "0";
                    }
                    if (txtIntWaiveAmt.Text != "")
                        dr["WaveIntColl"] = Convert.ToDecimal(txtIntWaiveAmt.Text);
                    else
                    {
                        dr["WaveIntColl"] = "0";
                    }
                    

                    // For Delay Payment Charge
                    if (txtDelayPayAmt.Text != "")
                        dr["PenaltyAmt"] = Convert.ToDecimal(txtDelayPayAmt.Text);
                    else
                    {
                        dr["PenaltyAmt"] = "0";
                    }
                    if (txtDelayRec.Text != "")
                        dr["PenCollAmt"] = Convert.ToDecimal(txtDelayRec.Text);
                    else
                    {
                        dr["PenCollAmt"] = "0";
                    }
                    if (txtDelayRec.Text != "")
                        dr["PenWaive"] = Convert.ToDecimal(txtDelayWaive.Text);
                    else
                    {
                        dr["PenWaive"] = "0";
                    }
                    if (txtDelayCGST.Text != "")
                        dr["PenCGSTAmt"] = Convert.ToDecimal(txtDelayCGST.Text);
                    else
                    {
                        dr["PenCGSTAmt"] = "0";
                    }
                    if (txtDelaySGST.Text != "")
                        dr["PenSGSTAmt"] = Convert.ToDecimal(txtDelaySGST.Text);
                    else
                    {
                        dr["PenSGSTAmt"] = "0";
                    }

                    // For Visiting Charge
                    if (txtVisitCharge.Text != "")
                        dr["VisitCharge"] = Convert.ToDecimal(txtVisitCharge.Text);
                    else
                    {
                        dr["VisitCharge"] = "0";
                    }
                    if (txtVisitChargeRec.Text != "")
                        dr["VisitChargeRec"] = Convert.ToDecimal(txtVisitChargeRec.Text);
                    else
                    {
                        dr["VisitChargeRec"] = "0";
                    }
                    if (txtVisitChargeWaive.Text != "")
                        dr["VisitChargeWaive"] = Convert.ToDecimal(txtVisitChargeWaive.Text);
                    else
                    {
                        dr["VisitChargeWaive"] = "0";
                    }
                    if (txtVisitChrgeCGST.Text != "")
                        dr["VisitChargeCGST"] = Convert.ToDecimal(txtVisitChrgeCGST.Text);
                    else
                    {
                        dr["VisitChargeCGST"] = "0";
                    }
                    if (txtVisitChrgeSGST.Text != "")
                        dr["VisitChargeSGST"] = Convert.ToDecimal(txtVisitChrgeSGST.Text);
                    else
                    {
                        dr["VisitChargeSGST"] = "0";
                    }
                    // For Bounce Amount
                    if (txtBounceAmt.Text != "")
                        dr["BounceAmt"] = Convert.ToDecimal(txtBounceAmt.Text);
                    else
                    {
                        dr["BounceAmt"] = "0";
                    }
                    if (txtBounceRecAmt.Text != "")
                        dr["BounceRecv"] = Convert.ToDecimal(txtBounceRecAmt.Text);
                    else
                    {
                        dr["BounceRecv"] = "0";
                    }
                    if (txtBounceWaiveAmt.Text != "")
                        dr["BounceWave"] = Convert.ToDecimal(txtBounceWaiveAmt.Text);
                    else
                    {
                        dr["BounceWave"] = "0";
                    }
                    if (txtBounceDue.Text != "")
                        dr["BounceDue"] = Convert.ToDecimal(txtBounceDue.Text);
                    else
                    {
                        dr["BounceDue"] = "0";
                    }
                    if (txtGrandTotAmt.Text != "")
                        dr["TotalPaid"] = Convert.ToDecimal(txtGrandTotAmt.Text);
                    else
                    {
                        dr["TotalPaid"] = "0";
                    }
                    if (lbPreIntDue.Text != "")
                        dr["IntDue"] = Convert.ToDecimal(lbPreIntDue.Text);
                    else
                    {
                        dr["IntDue"] = "0";
                    }

                    if (txtODDays.Text != "")
                        dr["ODDays"] = Convert.ToDecimal(txtODDays.Text);
                    else
                    {
                        dr["ODDays"] = "0";
                    }
                    if (txtODAmt.Text != "")
                        dr["ODAmt"] = Convert.ToDecimal(txtODAmt.Text);
                    else
                    {
                        dr["ODAmt"] = "0";
                    }
                   

                    dr["BrCode"] = Convert.ToString(lbBrCode.Text);
                    dr["BankNm"] = Convert.ToString(txtBankName.Text);
                    dr["ChqRefNo"] = Convert.ToString(txtChqRefNo.Text);
                    dtXml.Rows.Add(dr);
                }
                dtXml.AcceptChanges();
            }
            return dtXml;
        }
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopBank();
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            Session["dtRst"] = null;

            Int32 vRoutine = 1;
            string vLnStatus = "O";
            DataTable dt = new DataTable();
            CLoanRecovery oLR = new CLoanRecovery();
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());

            if (gblFuction.setDate(txtRecovryDt.Text) < vFinFromDt || gblFuction.setDate(txtRecovryDt.Text) > vFinToDt)
            {
                gblFuction.AjxMsgPopup("Recovery Date should be login financial year.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtRecovryDt");
                return;
            }
            try
            {
                string vBrCode = ddlBranch.SelectedValue;
                DateTime vRecvDt = gblFuction.setDate(txtRecovryDt.Text);
                vRoutine = chkRoutin.Checked == true ? 1 : 0;
                //N- Normal Download and Show In GridView
                dt = oLR.GetAllLoanCollection(vLnStatus, vRoutine, vRecvDt, vBrCode, "N");
                if (dt.Rows.Count > 0)
                {
                    gvRecvry.DataSource = dt;
                    gvRecvry.DataBind();
                    Session["dtRst"] = dt;
                }
                else
                {
                    gvRecvry.DataSource = null;
                    gvRecvry.DataBind();
                    Session["dtRst"] = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oLR = null;
            }
        }
        protected void btnDownLoad_Click(object sender, EventArgs e)
        {
            Session["dtRst"] = null;

            Int32 vRoutine = 1;
            string vLnStatus = "O";
            DataTable dt = new DataTable();
            CLoanRecovery oLR = new CLoanRecovery();
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());

            if (gblFuction.setDate(txtRecovryDt.Text) < vFinFromDt || gblFuction.setDate(txtRecovryDt.Text) > vFinToDt)
            {
                gblFuction.AjxMsgPopup("Recovery Date should be login financial year.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtRecovryDt");
                return;
            }
            try
            {
                string vBrCode = ddlBranch.SelectedValue;
                DateTime vRecvDt = gblFuction.setDate(txtRecovryDt.Text);
                vRoutine = chkRoutin.Checked == true ? 1 : 0;
                //E- Download In Excel
                dt = oLR.GetAllLoanCollection(vLnStatus, vRoutine, vRecvDt, vBrCode, "E");
                if (dt.Rows.Count > 0)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("Sheet1");
                        ws.Cell(1, 1).Style.Font.Bold = true;
                        ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(1, 1).InsertTable(dt); //insert datatable
                        ws.Table("Table1").ShowAutoFilter = false; //remove default filter
                        ws.SheetView.FreezeRows(1); //freeze rows
                        ws.Columns().AdjustToContents(); //adjust column according to data
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "_RecoveryData.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found");
                    gvRecvry.DataSource = null;
                    gvRecvry.DataBind();
                    Session["dtRst"] = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oLR = null;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.RoleId != 1)  //  && (this.RoleId != 2)
            {
                if (Session[gblValue.EndDate] != null)
                {
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtRecovryDt.Text))
                    {
                        gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                        return;
                    }
                }
            }
            DateTime vAccDate = gblFuction.setDate(txtRecovryDt.Text);
            string vBranch = Session[gblValue.BrnchCode].ToString(), vCollXml = "", vErrDesc = "";
            string vActMstTbl = Session[gblValue.ACVouMst].ToString(),
                vActDtlTbl = Session[gblValue.ACVouDtl].ToString();
            DateTime FinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString()),
                FinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            string vFinYear = Session[gblValue.ShortYear].ToString();
            //string PayMode = "B", BankLedgr = ddlBankLedgr.SelectedValue, BankNm = "", ChqRefNo = "";
            Int32 vErr = 0;
            string vCollMode = "", vBankLedgrAC = "", vBankName = "", vChqRefNo = "", vTransMode = "";
            // Collection Mode---  Cash
            if (rdbPayMode.SelectedValue == "C")
            {
                vCollMode = "C";
                vBankLedgrAC = "C0001";
                vBankName = "";
                vChqRefNo = "";
                vTransMode = "C";
            }
            // Collection Mode---  Bank
            else if (rdbPayMode.SelectedValue == "B")
            {
                if (ddlBankLedgr.SelectedValue == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Ledger Account...");
                    return;
                }
                vCollMode = "B";
                vBankLedgrAC = ddlBankLedgr.SelectedValue;
                vBankName = txtBankNm.Text.ToString().Trim();
                if (txtChqRefNo.Text.ToString() == "")
                {
                    gblFuction.AjxMsgPopup("Please Give Cheque No...");
                    return;
                }
                vChqRefNo = txtChqRefNo.Text.ToString().Trim();
                vTransMode = "B";
            }
            // Collection Mode---  NEFT/RTGS
            else if (rdbPayMode.SelectedValue == "N")
            {
                if (ddlBankLedgr.SelectedValue == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Ledger Account");
                    return;
                }
                vCollMode = "N";
                vBankLedgrAC = ddlBankLedgr.SelectedValue;
                vBankName = txtBankNm.Text.ToString().Trim();
                vChqRefNo = txtChqRefNo.Text.ToString().Trim();
                vTransMode = "B";
            }
            // Collection Mode---  ECS/NACH
            else if (rdbPayMode.SelectedValue == "H")
            {
                if (ddlBankLedgr.SelectedValue == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Ledger Account");
                    return;
                }
                vCollMode = "H";
                vBankLedgrAC = ddlBankLedgr.SelectedValue;
                vBankName = txtBankNm.Text.ToString().Trim();
                vChqRefNo = txtChqRefNo.Text.ToString().Trim();
                vTransMode = "B";
            }
            // Collection Mode--- PDC
            else if (rdbPayMode.SelectedValue == "P")
            {
                if (ddlBankLedgr.SelectedValue == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Ledger Account");
                    return;
                }
                vCollMode = "P";
                vBankLedgrAC = ddlBankLedgr.SelectedValue;
                vBankName = txtBankNm.Text.ToString().Trim();
                vChqRefNo = txtChqRefNo.Text.ToString().Trim();
                vTransMode = "B";
            }
            // Collection Mode--- UPI/EFT
            else if (rdbPayMode.SelectedValue == "U")
            {
                if (ddlBankLedgr.SelectedValue == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Ledger Account");
                    return;
                }
                vCollMode = "U";
                vBankLedgrAC = ddlBankLedgr.SelectedValue;
                vBankName = txtBankNm.Text.ToString().Trim();
                vChqRefNo = txtChqRefNo.Text.ToString().Trim();
                vTransMode = "B";
            }
            if ((vAccDate < FinFrom) || (vAccDate > FinTo))
            {
                gblFuction.AjxMsgPopup("Loan Recovery Date must be with in Login Financial Year");
                return;
            }

            CLoanRecovery oLR = new CLoanRecovery();
            try
            {
                DataTable dt = new DataTable();
                dt = GetData();
                if (dt.Rows.Count == 0)
                {
                    gblFuction.MsgPopup("Please Select any Loan for Recovery");
                    return;
                }
                dt.TableName = "Table1";
                using (StringWriter sW = new StringWriter())
                {
                    dt.WriteXml(sW);
                    vCollXml = sW.ToString();
                }

                vErr = oLR.InsertCollectionBulk(vAccDate, vActMstTbl, vActDtlTbl, vFinYear, vCollMode, vTransMode, vBankLedgrAC, vBankName, vChqRefNo, vCollXml, vBranch,
                                                Convert.ToInt32(Session[gblValue.UserId].ToString()), ref vErrDesc);
                if (vErr == 0)
                {
                    // Trigger SMS For Recovery
                    DataTable dt_Sms = new DataTable();
                    CSMS oSms = null;
                    AuthSms oAuth = null;
                    string vRtnGuid = string.Empty, vStatusCode = string.Empty, vStatusDesc = string.Empty;
                    oSms = new CSMS();
                    oAuth = new AuthSms();

                    // For Applicant  (LC----> Loan Collection)
                    dt_Sms = oSms.Get_ToSend_SMSBulk(vAccDate, "LC");
                    if (dt_Sms.Rows.Count > 0)
                    {
                        foreach (DataRow drSMS in dt_Sms.Rows)
                        {
                            if (drSMS["MobNo"].ToString().Length >= 10)
                            {
                                vRtnGuid = oAuth.SendSms(drSMS["MobNo"].ToString(), drSMS["Msg"].ToString());
                                System.Threading.Thread.Sleep(500);
                                if (!string.IsNullOrEmpty(vRtnGuid))
                                {
                                    vRtnGuid = vRtnGuid.Remove(0, 7);
                                    if (vRtnGuid != "")
                                    {
                                        vStatusDesc = "Message Delivered";
                                        vStatusCode = "Message Delivered Successfully";
                                        oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                                    }
                                    else
                                    {
                                        vStatusDesc = "Unknown Error";
                                        vStatusCode = "Unknown Error";
                                        oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                                    }
                                }
                                else
                                {
                                    vStatusDesc = "Unknown Error";
                                    vStatusCode = "Unknown Error";
                                    oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                                }
                            }
                        }
                    }
                    //btnSave.Enabled = false;
                    gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                    ClearControls();
                }
                else
                {
                    gblFuction.AjxMsgPopup(vErrDesc);
                    return;
                }
                gvRecvry.DataSource = null;
                gvRecvry.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                oLR = null;
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void gvRecvry_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vLoanID = "";
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                if (e.CommandName == "cmdCollect")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("lbtnShow");
                    foreach (GridViewRow gr in gvRecvry.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("lbtnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    vLoanID = Convert.ToString(e.CommandArgument);
                    ViewState["LoanId"] = vLoanID;

                    DataSet ds = new DataSet();
                    DataTable dtDtl = new DataTable();
                    DataTable dtLnDtl = new DataTable();
                    CLoanRecovery oLR = null;
                    string vLedTyp = "A";
                    try
                    {
                        oLR = new CLoanRecovery();
                        ds = oLR.GetCollectionDtlByLoanId(vLoanID);

                        // Populate Ledger Details
                        if (ds.Tables.Count > 0)
                        {
                            dtDtl = ds.Tables[0];
                            dtLnDtl = ds.Tables[1];
                        }

                        if (dtDtl.Rows.Count > 0)
                        {
                            gvLed.DataSource = dtDtl;
                            gvLed.DataBind();
                        }
                        else
                        {
                            gvLed.DataSource = null;
                            gvLed.DataBind();
                        }
                        dtDtl.PrimaryKey = new DataColumn[] { dtDtl.Columns["SlNo"] };
                        Session["dtDtlRst"] = dtDtl;

                        // Populate Loan Related Details
                        if (dtLnDtl.Rows.Count > 0)
                        {
                            txtLnDt.Text = dtLnDtl.Rows[0]["DisbDate"].ToString();
                            txtIntRate.Text = dtLnDtl.Rows[0]["IntRate"].ToString();
                            txtLnAmt.Text = dtLnDtl.Rows[0]["DisbAmt"].ToString();
                            txtPrinOS.Text = dtLnDtl.Rows[0]["POSAmt"].ToString();
                            txtIntOs.Text = dtLnDtl.Rows[0]["IntOutStand"].ToString();
                            txtLastTransDate.Text = dtLnDtl.Rows[0]["LastRecvDate"].ToString();
                            lblLnScme.Text = dtLnDtl.Rows[0]["LoanTypeName"].ToString();
                            lblEMIAmt.Text = dtLnDtl.Rows[0]["EMIAmt"].ToString();
                        }
                        else
                        {
                            txtLnDt.Text = "";
                            txtIntRate.Text = "";
                            txtLnAmt.Text = "";
                            txtPrinOS.Text = "";
                            txtIntOs.Text = "";
                            txtLastTransDate.Text = "";
                            lblLnScme.Text = "";
                            lblEMIAmt.Text = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        dtDtl = null;
                        oLR = null;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void gvLed_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dtDtl = null;
            CLoanRecovery oLR = new CLoanRecovery();
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                string vAcMst = Session[gblValue.ACVouMst].ToString();
                string vAcDtl = Session[gblValue.ACVouDtl].ToString();
                string vFinYear = Session[gblValue.FinYear].ToString();
                string vReffID = "", vCollMode = "";
                Int32 vErr = 0;

                if (e.CommandName == "cmdDelete")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnSlct");
                    string RowIndex = e.CommandArgument.ToString();
                    int totalrow = gvLed.Rows.Count;
                    if (Convert.ToInt32(RowIndex) != 0)
                    {
                        gblFuction.AjxMsgPopup("Only Last Collection Details can be deleted...");
                        return;
                    }
                    else
                    {
                        string vLoanId = gvLed.Rows[0].Cells[1].Text;
                        Int32 vSlNo = Convert.ToInt32(gvLed.Rows[0].Cells[17].Text);
                        vReffID = vLoanId + "-" + vSlNo;
                        DateTime vAccDate = gblFuction.setDate(gvLed.Rows[0].Cells[2].Text);
                        DateTime vLnDate = gblFuction.setDate(gvLed.Rows[0].Cells[19].Text);
                        vCollMode = gvLed.Rows[0].Cells[15].Text;
                        String FYear = GetCurrentFinancialYear(vAccDate);
                        if (vFinYear != FYear)
                        {
                            gblFuction.AjxMsgPopup("Collection Details can not be deleted as Collection Date is not in same Login  Financial Year...");
                            return;
                        }
                        if (vAccDate == vLnDate)
                        {
                            gblFuction.AjxMsgPopup("Collection Details can not be deleted as Collection Done during Loan Disbursement as Advance EMI...");
                            return;
                        }
                        vErr = oLR.DeleteCollection(vLoanId, vSlNo, vReffID, vAcMst, vAcDtl, vBrCode, Convert.ToInt32(Session[gblValue.UserId]), vAccDate, vCollMode);
                        if (vErr == 0)
                        {
                            gblFuction.AjxMsgPopup(gblPRATAM.DeleteMsg);
                            DataSet ds = new DataSet();
                            ds = oLR.GetCollectionDtlByLoanId(vLoanId);
                            dtDtl = ds.Tables[0];
                            if (dtDtl.Rows.Count > 0)
                            {
                                gvLed.DataSource = dtDtl;
                                gvLed.DataBind();
                            }
                            else
                            {
                                gvLed.DataSource = null;
                                gvLed.DataBind();
                            }
                            Session["dtDtlRst"] = dtDtl;
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtDtl = null;
            }
        }
        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (ddlBranch.SelectedValue == "-1")
            {
                gblFuction.MsgPopup("Please Select Any Branch");
                ddlBranch.Focus();
                return;
            }

            int p = 0, b = 0;
            String vFileName = "", vFileType = "", strExcelConn = "", SheetName = "", vErrReason = "", vCollXml = "", vBank = "-1",
                vBrnch = "", vCheque = "", vErrDesc = "";
            DateTime vChqDt = gblFuction.setDate("01/01/1900");
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            OleDbDataAdapter da = null;
            CLoanRecovery oLR = new CLoanRecovery();
            bool isExcelConOpen = false;
            OleDbConnection connExcel = null;

            vBrnch = ddlBranch.SelectedValue;

            string vActMstTbl = Session[gblValue.ACVouMst].ToString(),
               vActDtlTbl = Session[gblValue.ACVouDtl].ToString();
            DateTime FinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString()),
                FinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            string vFinYear = Session[gblValue.ShortYear].ToString();
            DateTime vAccDate = gblFuction.setDate(txtRecovryDt.Text);
            if ((vAccDate < FinFrom) || (vAccDate > FinTo))
            {
                gblFuction.AjxMsgPopup("Loan Recovery Date must be with in Login Financial Year");
                return;
            }
            if (fuColImport.HasFile)
            {
                if (!Directory.Exists(vExcelPath))
                {
                    Directory.CreateDirectory(vExcelPath);
                }
                vFileName = fuColImport.FileName;
                fuColImport.SaveAs(vExcelPath + vFileName);
                p = vFileName.LastIndexOf(".");
                vFileType = vFileName.Substring(p + 1);

                if (vFileType.Equals("xls") || vFileType.Equals("xlsx"))
                {
                    try
                    {
                        if (vFileType.Equals("xlsx"))
                            strExcelConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + vExcelPath + vFileName + ";" + "Extended Properties='Excel 12.0;ReadOnly=false; HDR=Yes'";
                        if (vFileType.Equals("xls"))
                            strExcelConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + vExcelPath + vFileName + ";" + "Extended Properties='Excel 8.0;ReadOnly=false;HDR=Yes'";

                        connExcel = new OleDbConnection(strExcelConn);
                        OleDbCommand cmdExcel = new OleDbCommand();
                        cmdExcel.Connection = connExcel;

                        connExcel.Open();
                        isExcelConOpen = true;
                        DataTable dtExcelSchema = new DataTable();
                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        da = new OleDbDataAdapter();
                        ds = new DataSet();

                        var abc = dtExcelSchema.AsEnumerable().Where(r => ((string)r["TABLE_NAME"]) == "Sheet1$").Count();

                        if (Convert.ToInt32(abc) == 1)
                        {
                            SheetName = "Sheet1$";
                        }
                        else if (Convert.ToInt32(abc) == 0)
                        {
                            gblFuction.MsgPopup("Please provide Sheet Name(Sheet1) Properly");
                            return;
                        }
                        else
                        {
                            gblFuction.MsgPopup("No Data Found");
                            return;
                        }

                        cmdExcel.CommandText = "SELECT LoanId, CustId, CustName,PrincOS,PrinPaid,IntPaid,DalayPaymentCharge as PenaltyAmt,DelayPaymentWaive as PenWaive,DalayPaymentReceived as PenCollAmt ,DalayPaymentCGST as PenCGSTAmt,DalayPaymentSGST as PenSGSTAmt,"
                         + "VisitCharge,VisitChargeRec,VisitChargeWaive,VisitChargeCGST,VisitChargeSGST,IntWaiveAmt,BounceAmt,BounceReceived,BounceWaive,GrandTotal as TotalPaid,ODDays,ODAmt,ExcessCharge,PreIntDue as IntDue,BrCode "
                         +  "From [" + SheetName + "]" + " WHERE LoanId IS NOT NULL AND CustId IS NOT NULL "
                         + " AND PrinPaid  IS NOT NULL and IntPaid  IS NOT NULL and GrandTotal IS NOT NULL " 
                         +  " AND BounceReceived  IS NOT NULL and BounceWaive  IS NOT NULL and IntWaiveAmt IS NOT NULL ";

                        da.SelectCommand = cmdExcel;
                        da.Fill(ds);
                        dt = new DataTable();
                        dt = ds.Tables[0];
                        dt.TableName = "Table1";
                        Decimal pTotPaid = 0, pPrincPaid = 0, pIntPaid = 0,  pIntWaive = 0, pBounceRec = 0, pSum = 0, pBounceAmt = 0, pBounceWaive = 0,
                            pPenaltyAmt = 0, pPenWaive = 0, pPenCollAmt = 0, pPenCGSTAmt = 0, pPenSGSTAmt=0,
                            pVisitChrge = 0, pVisitChargeWaive = 0, pVisitChrgeRec = 0, pVisitChrgeCGST = 0, pVisitChrgeSGST = 0, pExcessCharge=0;
                        if (dt.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                if (string.IsNullOrEmpty(dt.Rows[j]["TotalPaid"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record GrandTotal Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString());
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["PrinPaid"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record PrinPaid Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString()+" Please Input zero");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["IntPaid"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record IntPaid Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString() + " Please Input zero");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["IntWaiveAmt"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record IntWaive Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString() + " Please Input zero");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["PenaltyAmt"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record DalayPaymentCharge Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString() + " Please Input zero");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["PenWaive"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record DelayPaymentWaive Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString() + " Please Input zero");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["PenCollAmt"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record DalayPaymentReceived Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString() + " Please Input zero");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["PenCGSTAmt"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record DalayPaymentCGST Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString() + " Please Input zero");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["PenSGSTAmt"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record DalayPaymentSGST Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString() + " Please Input zero");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["VisitCharge"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record VisitChrge Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString() + " Please Input zero");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["VisitChargeWaive"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record VisitChargeWaive Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString() + " Please Input zero");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["VisitChargeRec"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record VisitChrgeRec Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString() + " Please Input zero");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["VisitChargeCGST"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record VisitChrgeCGST Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString() + " Please Input zero");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["VisitChargeSGST"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record VisitChrgeSGST Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString() + " Please Input zero");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["BounceAmt"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record BounceAmt Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString() + " Please Input zero");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["BounceReceived"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record BounceReceived Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString() + " Please Input zero");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["BounceWaive"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record BounceWaive Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString() + " Please Input zero");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["IntDue"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record Previous Interest Due Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString() + " Please Input zero");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dt.Rows[j]["ExcessCharge"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record Excess Charge Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString() + " Please Input zero");
                                    return;
                                }
                                pTotPaid = Convert.ToDecimal(dt.Rows[j]["TotalPaid"]);
                                pPrincPaid = Convert.ToDecimal(dt.Rows[j]["PrinPaid"]);
                                pIntPaid = Convert.ToDecimal(dt.Rows[j]["IntPaid"]);

                                pPenaltyAmt = Convert.ToDecimal(dt.Rows[j]["PenaltyAmt"]);
                                pPenCollAmt  = Convert.ToDecimal(dt.Rows[j]["PenCollAmt"]);
                                pPenWaive = Convert.ToDecimal(dt.Rows[j]["PenWaive"]);
                                pPenCGSTAmt = Convert.ToDecimal(dt.Rows[j]["PenCGSTAmt"]);
                                pPenSGSTAmt = Convert.ToDecimal(dt.Rows[j]["PenSGSTAmt"]);



                                pVisitChrge = Convert.ToDecimal(dt.Rows[j]["VisitCharge"]);
                                pVisitChargeWaive = Convert.ToDecimal(dt.Rows[j]["VisitChargeWaive"]);
                                pVisitChrgeRec = Convert.ToDecimal(dt.Rows[j]["VisitChargeRec"]);
                                pVisitChrgeCGST = Convert.ToDecimal(dt.Rows[j]["VisitChargeCGST"]);
                                pVisitChrgeSGST = Convert.ToDecimal(dt.Rows[j]["VisitChargeSGST"]);


                                pIntWaive = Convert.ToDecimal(dt.Rows[j]["IntWaiveAmt"]);
                                pBounceRec = Convert.ToDecimal(dt.Rows[j]["BounceReceived"]);
                                if (string.IsNullOrEmpty(dt.Rows[j]["BounceAmt"].ToString()) == false)
                                    pBounceAmt = Convert.ToDecimal(dt.Rows[j]["BounceAmt"]);
                                if (string.IsNullOrEmpty(dt.Rows[j]["BounceWaive"].ToString()) == false)
                                    pBounceWaive = Convert.ToDecimal(dt.Rows[j]["BounceWaive"]);
                                if (string.IsNullOrEmpty(dt.Rows[j]["ExcessCharge"].ToString()) == false)
                                    pExcessCharge = Convert.ToDecimal(dt.Rows[j]["ExcessCharge"]);

                                pSum = (pPrincPaid + pIntPaid - pIntWaive + pPenCollAmt + pPenCGSTAmt + pPenSGSTAmt + pVisitChrgeRec + pVisitChrgeCGST + pVisitChrgeSGST + pBounceRec + pExcessCharge);
                                if (pTotPaid != pSum)
                                {
                                    gblFuction.MsgPopup("Please Check Record Total Paid must be equal to (PrinPaid + IntPaid - IntWaive + DelayPaymentRec + DelayPaymentCGST + DelayPaymentSGST + VisitChrgeRec + VisitChrgeCGST +VisitChrgeSGST + BounceReceivedAmt + ExcessCharge) For Loan Id " + dt.Rows[j]["LoanId"].ToString());
                                    return;
                                }
                                // For Delay payment
                                if (pPenCollAmt > pPenaltyAmt)
                                {
                                    gblFuction.MsgPopup("Please Check Record Delay Payment Received Amount Can Not Be Greater Than Calculated Delay Payment Amount For Loan Id " + dt.Rows[j]["LoanId"].ToString());
                                    return;
                                }
                                if (pPenWaive > pPenaltyAmt)
                                {
                                    gblFuction.MsgPopup("Please Check Record Delay Payment Waive Amount Can Not Be Greater Than Calculated Delay Payment Amount For Loan Id " + dt.Rows[j]["LoanId"].ToString());
                                    return;
                                }
                                if ((pPenCollAmt + pPenWaive) > pPenaltyAmt)
                                {
                                    gblFuction.MsgPopup("Please Check Record Sum Of (Delay Payment Waive Amount + Delay Payment Received Amount) Can Not Be Greater Than Calculated Delay Payment Amount For Loan Id " + dt.Rows[j]["LoanId"].ToString());
                                    return;
                                }
                                // For Visit Charge
                                if (pVisitChrgeRec > pVisitChrge)
                                {
                                    gblFuction.MsgPopup("Please Check Record Visit Chrge Received Amount Can Not Be Greater Than Calculated Visit Chrge Amount For Loan Id " + dt.Rows[j]["LoanId"].ToString());
                                    return;
                                }
                                if (pVisitChargeWaive > pVisitChrge)
                                {
                                    gblFuction.MsgPopup("Please Check Record Visit Chrge Waive Amount Can Not Be Greater Than Calculated Visit Chrge Amount For Loan Id " + dt.Rows[j]["LoanId"].ToString());
                                    return;
                                }
                                if ((pVisitChrgeRec + pVisitChargeWaive) > pVisitChrge)
                                {
                                    gblFuction.MsgPopup("Please Check Record Sum Of (Visit Charge Waive Amount + Visit Charge Received Amount) Can Not Be Greater Than Calculated Visit Charge Amount For Loan Id " + dt.Rows[j]["LoanId"].ToString());
                                    return;
                                }
                                // For Bounce Charge

                                if (pBounceRec > pBounceAmt)
                                {
                                    gblFuction.MsgPopup("Please Check Record Bounce Received Amount Can Not Be Greater Than Bounce Amount For Loan Id " + dt.Rows[j]["LoanId"].ToString());
                                    return;
                                }
                                if (pBounceWaive > pBounceAmt)
                                {
                                    gblFuction.MsgPopup("Please Check Record Bounce Waive Amount Can Not Be Greater Than Bounce Amount For Loan Id " + dt.Rows[j]["LoanId"].ToString());
                                    return;
                                }
                                if ((pBounceRec + pBounceWaive) > pBounceAmt)
                                {
                                    gblFuction.MsgPopup("Please Check Record Sum Of (Bounce Waive Amount + Bounce Received Amount) Can Not Be Greater Than Bounce Amount For Loan Id " + dt.Rows[j]["LoanId"].ToString());
                                    return;
                                }
                            }

                            foreach (DataRow dataRow in dt.Rows)
                            {
                                for (int j = 0; j < dataRow.ItemArray.Length; j++)
                                {
                                    if (dataRow.ItemArray[j] == DBNull.Value)
                                        dataRow.SetField(j, string.Empty);
                                }
                            }
                            using (StringWriter oSW = new StringWriter())
                            {
                                dt.WriteXml(oSW);
                                vCollXml = oSW.ToString().Replace("T00:00:00+05:30", "");
                            }
                        }
                        else
                        {
                            gblFuction.MsgPopup("Check Excel Sheet Properly.");
                            return;
                        }

                        connExcel.Close();
                        isExcelConOpen = false;

                        #region Save Collection
                        string vCollMode = "", vBankLedgrAC = "", vBankName = "", vChqRefNo = "", vTransMode = "";
                        // Collection Mode---  Cash
                        if (rdbPayMode.SelectedValue == "C")
                        {
                            vCollMode = "C";
                            vBankLedgrAC = "C0001";
                            vBankName = "";
                            vChqRefNo = "";
                            vTransMode = "C";
                        }
                        // Collection Mode---  Bank
                        else if (rdbPayMode.SelectedValue == "B")
                        {
                            if (ddlBankLedgr.SelectedValue == "-1")
                            {
                                gblFuction.AjxMsgPopup("Please Select Ledger Account...");
                                return;
                            }
                            vCollMode = "B";
                            vBankLedgrAC = ddlBankLedgr.SelectedValue;
                            vBankName = txtBankNm.Text.ToString().Trim();
                            if (txtChqRefNo.Text.ToString() == "")
                            {
                                gblFuction.AjxMsgPopup("Please Give Cheque No...");
                                return;
                            }
                            vChqRefNo = txtChqRefNo.Text.ToString().Trim();
                            vTransMode = "B";
                        }
                        // Collection Mode---  NEFT/RTGS
                        else if (rdbPayMode.SelectedValue == "N")
                        {
                            if (ddlBankLedgr.SelectedValue == "-1")
                            {
                                gblFuction.AjxMsgPopup("Please Select Ledger Account");
                                return;
                            }
                            vCollMode = "N";
                            vBankLedgrAC = ddlBankLedgr.SelectedValue;
                            vBankName = txtBankNm.Text.ToString().Trim();
                            vChqRefNo = txtChqRefNo.Text.ToString().Trim();
                            vTransMode = "B";
                        }
                        // Collection Mode---  ECS/NACH
                        else if (rdbPayMode.SelectedValue == "H")
                        {
                            if (ddlBankLedgr.SelectedValue == "-1")
                            {
                                gblFuction.AjxMsgPopup("Please Select Ledger Account");
                                return;
                            }
                            vCollMode = "H";
                            vBankLedgrAC = ddlBankLedgr.SelectedValue;
                            vBankName = txtBankNm.Text.ToString().Trim();
                            vChqRefNo = txtChqRefNo.Text.ToString().Trim();
                            vTransMode = "B";
                        }
                        // Collection Mode--- PDC
                        else if (rdbPayMode.SelectedValue == "P")
                        {
                            if (ddlBankLedgr.SelectedValue == "-1")
                            {
                                gblFuction.AjxMsgPopup("Please Select Ledger Account");
                                return;
                            }
                            vCollMode = "P";
                            vBankLedgrAC = ddlBankLedgr.SelectedValue;
                            vBankName = txtBankNm.Text.ToString().Trim();
                            vChqRefNo = txtChqRefNo.Text.ToString().Trim();
                            vTransMode = "B";
                        }
                        // Collection Mode--- PDC
                        else if (rdbPayMode.SelectedValue == "U")
                        {
                            if (ddlBankLedgr.SelectedValue == "-1")
                            {
                                gblFuction.AjxMsgPopup("Please Select Ledger Account");
                                return;
                            }
                            vCollMode = "U";
                            vBankLedgrAC = ddlBankLedgr.SelectedValue;
                            vBankName = txtBankNm.Text.ToString().Trim();
                            vChqRefNo = txtChqRefNo.Text.ToString().Trim();
                            vTransMode = "B";
                        }
                        //b = oLR.SaveBulkCollUpload(sXml, vBrnch, gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBank, vCheque,
                        //    vChqDt, vTblMst, vTblDtl, vFinYear, this.UserID, ref  vErrReason);
                        b = oLR.InsertCollectionBulkExecl(vAccDate, vActMstTbl, vActDtlTbl, vFinYear, vCollMode, vTransMode, vBankLedgrAC, vBankName, vChqRefNo, vCollXml,
                            ddlBranch.SelectedValue.ToString(), Convert.ToInt32(Session[gblValue.UserId].ToString()), ref vErrDesc);

                        if (b == 0)
                        {
                            // Trigger SMS For Recovery
                            DataTable dt_Sms = new DataTable();
                            CSMS oSms = null;
                            AuthSms oAuth = null;
                            string vRtnGuid = string.Empty, vStatusCode = string.Empty, vStatusDesc = string.Empty;
                            oSms = new CSMS();
                            oAuth = new AuthSms();

                            // For Applicant  (LC----> Loan Collection)
                            dt_Sms = oSms.Get_ToSend_SMSBulk(vAccDate, "LC");
                            if (dt_Sms.Rows.Count > 0)
                            {
                                foreach (DataRow drSMS in dt_Sms.Rows)
                                {
                                    if (drSMS["MobNo"].ToString().Length >= 10)
                                    {
                                        vRtnGuid = oAuth.SendSms(drSMS["MobNo"].ToString(), drSMS["Msg"].ToString());
                                        System.Threading.Thread.Sleep(500);
                                        if (!string.IsNullOrEmpty(vRtnGuid))
                                        {
                                            vRtnGuid = vRtnGuid.Remove(0, 7);
                                            if (vRtnGuid != "")
                                            {
                                                vStatusDesc = "Message Delivered";
                                                vStatusCode = "Message Delivered Successfully";
                                                oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                                            }
                                            else
                                            {
                                                vStatusDesc = "Unknown Error";
                                                vStatusCode = "Unknown Error";
                                                oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                                            }
                                        }
                                        else
                                        {
                                            vStatusDesc = "Unknown Error";
                                            vStatusCode = "Unknown Error";
                                            oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                                        }
                                    }
                                }
                            }
                            gblFuction.AjxMsgPopup("Record Save Successfully");
                        }
                        else
                        {
                            if (vErrDesc != "")
                            {
                                gblFuction.MsgPopup(vErrDesc);
                                return;
                            }
                            else
                            {
                                gblFuction.MsgPopup(gblPRATAM.DBError);
                                return;
                            }
                        }
                        #endregion
                    }
                    finally
                    {
                        if (isExcelConOpen == true)
                        {
                            connExcel.Close();
                            isExcelConOpen = false;
                        }
                        if (dt.Rows.Count != 0)
                        {
                            if (File.Exists(vExcelPath + vFileName))
                            {
                                File.Delete(vExcelPath + vFileName);

                            }
                        }
                        dt = null;
                        ds = null;
                        da = null;
                        oLR = null;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("Please Select EXCEL File..");
                }
            }
            else
                gblFuction.MsgPopup("Please Select File To Upload ..");
        }
    }
}