using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class BnkrLoanRecovry : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                //txtBankChrgPay.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                txtTotPaid.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                ClearControls();
                PopBank();
                PopBanker();
                PopLedgerList();
                PopInterestHead();
                ViewState["StateEdit"] = null;
                txtRecovryDt.Text = (string)Session[gblValue.LoginDate];
                btnDone.Enabled = true;
                btnDel.Enabled = false;
                btnShow.Enabled = true;
                gvRecvry.Enabled = true;
                ddlBank.Enabled = true;
                ddlLedger.Enabled = false;
            }
        }

        private void InitBasePage()
        {

            try
            {
                this.Menu = false;
                this.PageHeading = "Banker Loan Repayment";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBankerLoanRecovery);
                if (this.UserID == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                    btnDone.Visible = false;
                    btnDel.Visible = false;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                    btnDone.Visible = true;
                    btnDel.Visible = false;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                    btnDone.Visible = true;
                    btnDel.Visible = false;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnSave.Visible = true;
                    btnDone.Visible = true;
                    btnDel.Visible = true;
                    return;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Recovery", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }

        }



        private void ClearControls()
        {
            rdbLoan.SelectedValue = "O";
            //rdbColl.SelectedValue = "G";
            rdbRecvry.SelectedValue = "N";
            txtLnAmt.Text = "0.00";
            txtLnDt.Text = "";
            txtIntRate.Text = "0.00";
            txtPrinOS.Text = "0.00";
            txtIntOs.Text = "0.00";
            txtPrinPay.Text = "0.00";
            txtIntPay.Text = "0.00";
            //txtBankChrgPay.Text = "0.00";
            txtTotAmtPay.Text = "0.00";
            txtPrinPaid.Text = "0.00";
            txtIntPaid.Text = "0.00";
            //txtBankPaid.Text = "0.00";
            txtTotPaid.Text = "0.00";
            txtTotPrin.Text = "0.00";
            txtTotInt.Text = "0.00";
            txtTotal.Text = "0.00";
            txtPrinPaidL.Text = "0.00";
            txtInstPaidL.Text = "0.00";
            txtTotalPaidL.Text = "0.00";
            ddlBank.SelectedIndex = -1;
            ddlLedger.SelectedIndex = -1;
            ddlBanker.SelectedIndex = -1;
            ddlInterest.SelectedIndex = -1;
            gvRecvry.DataSource = null;
            gvRecvry.DataBind();
            gvLed.DataSource = null;
            gvLed.DataBind();
        }




        protected void rdbRecvry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbRecvry.SelectedValue == "N")
            {
                btnDel.Enabled = false;
                btnSave.Enabled = true;
                btnShow.Enabled = true;
                btnDone.Enabled = true;
                gvRecvry.Enabled = true;
                txtTotPaid.Enabled = true;
                ClearControls();
            }
            else if (rdbRecvry.SelectedValue == "M")
            {
                btnDel.Enabled = true;
                btnSave.Enabled = false;
                btnShow.Enabled = false;
                btnDone.Enabled = false;
                gvRecvry.Enabled = true;
                txtTotPaid.Enabled = false;
            }
        }

        ///------------------------------



        //protected void rdbColl_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (rdbColl.SelectedValue == "I")
        //    {
        //        IblGroupName.Visible = false;
        //        ddlGrp.Visible = false;
        //        ClearControls();
        //    }
        //    else if (rdbColl.SelectedValue == "G")
        //    {
        //        IblGroupName.Visible = true;
        //        ddlGrp.Visible = true;
        //        ClearControls();
        //    }
        //}
        //---------------------------------




        private void PopBank()
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            CVoucher oVou = new CVoucher();
            dt = oVou.GetBank(vBrCode);
            ddlBank.DataTextField = "Desc";
            ddlBank.DataValueField = "DescID";
            ddlBank.DataSource = dt;
            ddlBank.DataBind();
            ListItem oItem = new ListItem();
            oItem.Text = "<--- Select --->";
            oItem.Value = "-1";
            ddlBank.Items.Insert(0, oItem);
        }

        private void PopLedgerList()
        {
            DataTable dt = null;
            CAcGenled oGen = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oGen = new CAcGenled();
            dt = oGen.GetLedgerList();
            ddlLedger.DataTextField = "Desc";
            ddlLedger.DataValueField = "DescID";
            ddlLedger.DataSource = dt;
            ddlLedger.DataBind();
            ListItem oItem = new ListItem();
            oItem.Text = "<--- Select --->";
            oItem.Value = "-1";
            ddlLedger.Items.Insert(0, oItem);
        }


        private void PopInterestHead()
        {
            DataTable dt = null;
            CBankers oBNK = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oBNK = new CBankers();
            dt = oBNK.GetInterestHead();
            ddlInterest.DataTextField = "Desc";
            ddlInterest.DataValueField = "DescID";
            ddlInterest.DataSource = dt;
            ddlInterest.DataBind();
            ListItem oItem = new ListItem();
            oItem.Text = "<--- Select --->";
            oItem.Value = "-1";
            ddlInterest.Items.Insert(0, oItem);
        }

        private void PopBanker()
        {
            DataTable dt = null;
            CGblIdGenerator oGbl = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            try
            {
                oGbl = new CGblIdGenerator();
                dt = oGbl.PopComboMIS("N", "N", "AA", "BankerId", "BankerName", "BankerMst", 0, "AA", "AA", System.DateTime.Now, "0000");

                ddlBanker.DataSource = dt;
                ddlBanker.DataTextField = "BankerName";
                ddlBanker.DataValueField = "BankerId";
                ddlBanker.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBanker.Items.Insert(0, oli);
            }
            finally
            {
                oGbl = null;
                dt = null;
            }
        }



        protected void btnShow_Click(object sender, EventArgs e)
        {
            String vLnStatus = "", vLoanType = "";
            string vLoanId = "";
            Int32 vRoutine = 0, vEoId = 0, vBankerId = 0;
            DataTable dt = null, dtPA = null;
            CBankers oCB = null;
            ViewState["dtRst"] = null;
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());


            if (ViewState["DescId"] == null)
            {
                gblFuction.AjxMsgPopup("Account Head Not Specified for Selected Bank, Please Check....");
                return;
            };

            if (gblFuction.setDate(txtRecovryDt.Text) < vFinFromDt || gblFuction.setDate(txtRecovryDt.Text) > vFinToDt)
            {
                gblFuction.AjxMsgPopup("Recovery Date should login financial year.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtRecovryDt");
                return;
            }

            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                DateTime vRecvDt = gblFuction.setDate(txtRecovryDt.Text);


                if (rdbLoan.SelectedValue == "O")
                    vLnStatus = "O";
                else if (rdbLoan.SelectedValue == "C")
                    vLnStatus = "C";

                if (ddlBanker.SelectedIndex > 0)
                    vBankerId = Convert.ToInt32(ddlBanker.SelectedValue.ToString());
                else
                    vBankerId = 0;

                oCB = new CBankers();
                dt = oCB.GetBankLoanPayment(vBankerId, vLnStatus, vRoutine, vRecvDt, vLoanId, vBrCode);
                dt.PrimaryKey = new DataColumn[] { dt.Columns["LoanID"] };
                ViewState["dtRst"] = dt;
                gvRecvry.DataSource = dt;
                gvRecvry.DataBind();
                TotCalculation(dt, "gvRecvry");
                chkPreClose.Enabled = true;
                chkPreClose.Checked = false;
                chkJurnl.Checked = false;
                btnSave.Enabled = true;
                if (dt.Rows.Count > 0)
                {
                    vLoanId = Convert.ToString(dt.Rows[0]["LoanID"]);
                    GetDetails(vLoanId, vRecvDt, vBrCode);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                dtPA = null;
                oCB = null;
            }
        }


        protected void GetDetails(string pLoanId, DateTime pRecvDt, string pBrCode)
        {
            DataTable dt = null;
            DataTable dtDtl = null;
            CBankers oLR = null;
            decimal vTotDue = 0, vTotPaid = 0;
            oLR = new CBankers();
            ViewState["LoanId"] = pLoanId;

            dt = oLR.GetRepaymentByLoanId(pLoanId, pRecvDt, pBrCode);
            if (dt.Rows.Count > 0)
            {
                txtLnDt.Text = Convert.ToString(dt.Rows[0]["LoanDt"]);
                txtIntRate.Text = Convert.ToString(dt.Rows[0]["IntRate"]);
                txtLnAmt.Text = Convert.ToString(dt.Rows[0]["LoanAmt"]);
                txtPrinOS.Text = Convert.ToString(dt.Rows[0]["PrncOutStd"]);
                txtIntOs.Text = Convert.ToString(dt.Rows[0]["IntOutStd"]);
                txtPrinPay.Text = string.Format("{0:N}", dt.Rows[0]["PrincpalDue"]);
                txtIntPay.Text = string.Format("{0:N}", dt.Rows[0]["InterestDue"]);
                vTotDue = Math.Round(Convert.ToDecimal(dt.Rows[0]["PrincpalDue"]) + Convert.ToDecimal(dt.Rows[0]["InterestDue"]), 2);
                txtTotAmtPay.Text = string.Format("{0:N}", vTotDue);
                txtPrinPaid.Text = "0.00";
                txtIntPaid.Text = "0.00";
                vTotPaid = Convert.ToDecimal(dt.Rows[0]["PaidPric"]) + Convert.ToDecimal(dt.Rows[0]["PaidInt"]);
                txtTotPaid.Text = "0.00";
                dtDtl = oLR.GetBankPaymentDtlByLoanId(pLoanId, "B", pBrCode);
                if (dtDtl.Rows.Count > 0)
                {
                    gvLed.DataSource = dtDtl;
                    gvLed.DataBind();
                    TotCalculation(dtDtl, "gvLed");
                    dtDtl.PrimaryKey = new DataColumn[] { dtDtl.Columns["SlNo"] };
                    ViewState["dtDtlRst"] = dtDtl;
                }
                else
                {
                    gvLed.DataSource = null;
                    gvLed.DataBind();
                }
            }
            else
            {
                txtLnDt.Text = "";
                txtIntRate.Text = "0.00";
                txtLnAmt.Text = "0.00";
                txtPrinOS.Text = "0.00";
                txtIntOs.Text = "0.00";
                txtPrinPay.Text = "0.00";
                txtIntPay.Text = "0.00";
                txtTotAmtPay.Text = "0.00";
                txtPrinPaid.Text = "0.00";
                txtIntPaid.Text = "0.00";
                txtTotPaid.Text = "0.00";
            }
        }




        protected void gvRecvry_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vLoanID = "";
            DateTime vCollDt;
            try
            {
                chkPreClose.Checked = false;
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
                    vCollDt = gblFuction.setDate(txtRecovryDt.Text);
                    GetDetails(vLoanID, vCollDt, vBrCode);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        protected void txtRecDt_TextChanged(object sender, EventArgs e)
        {
            ClearControls();
        }

        protected void txtBankChrgPay_TextChanged(object sender, EventArgs e)
        {
            txtTotAmtPay.Text = Convert.ToString(Convert.ToDecimal(txtPrinPay.Text) + Convert.ToDecimal(txtIntPay.Text)); // + Convert.ToDecimal(txtBankChrgPay.Text));
        }

        protected void txtTotPaid_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CBankers oLR = null;
            decimal vBalAmt = 0, vPrinAmt = 0, vIntAmt = 0;
            decimal vPTot = 0, vITot = 0;
            try
            {
                if (ViewState["LoanId"] == null) return;
                if (txtTotPaid.Text == "")
                {
                    txtTotPaid.Text = "0.00";
                }


                string vLoanId = Convert.ToString(ViewState["LoanId"]);
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                decimal vTotPOS = Convert.ToDecimal(txtPrinOS.Text);
                decimal vTotIOS = Convert.ToDecimal(txtIntOs.Text);
                decimal vTotOS = vTotPOS + vTotIOS;

                /**************************************************    
                if (chkPreClose.Checked == false)
                        {
                            oLR = new CBankers();
                    dt = oLR.GetBnkInstallAllocation(vLoanId, vBrCode);
                            if (dt.Rows.Count > 0)
                            {
                                vPTot = Convert.ToDecimal(dt.Compute("Sum(PrinceAmt)", ""));
                                vITot = Convert.ToDecimal(dt.Compute("Sum(InstAmt)", ""));
                        
                                if (vPTot > vTotPOS)
                                    dt.Rows[0]["PrinceAmt"] = Convert.ToDecimal(dt.Rows[0]["PrinceAmt"]) - (vPTot - vTotPOS);
                                if (vITot > vTotIOS)
                                    dt.Rows[0]["InstAmt"] = Convert.ToDecimal(dt.Rows[0]["InstAmt"]) - (vITot - vTotIOS);
                        
                                dt.Rows[0]["TotAmt"] = Convert.ToDecimal(dt.Rows[0]["PrinceAmt"]) + Convert.ToDecimal(dt.Rows[0]["InstAmt"]);
                        
                                txtBankPaid.Text =  string.Format("{0:N}",txtBankChrgPay.Text);
                                vBalAmt = Convert.ToDecimal(txtTotPaid.Text) - Convert.ToDecimal(txtBankChrgPay.Text);
                        
                                if (vBalAmt > vTotOS)       // For Invalid Paid Total Value as Paid Total can not be > then Total OS
                                {
                                    gblFuction.AjxMsgPopup("Total paid Amount should less than equal to total Principal and Interest OS..");
                                    return;
                                }
                                if (vTotOS == vBalAmt)      // For  Paid Total =  Total OS
                                {
                                    txtPrinPaid.Text = txtPrinPay.Text;
                                    txtIntPaid.Text = txtIntPay.Text;
                                }
                                else if (vBalAmt < vTotOS) //Total Amt <=Total OS
                                {
                                        // For Part Payment and Full Payment And Advance payment
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            if (vBalAmt >= Convert.ToDecimal(dr["TotAmt"].ToString()))
                                            {
                                                vIntAmt += Convert.ToDecimal(dr["InstAmt"].ToString());
                                                vPrinAmt += Convert.ToDecimal(dr["PrinceAmt"].ToString());
                                                vBalAmt = vBalAmt - (Convert.ToDecimal(dr["PrinceAmt"].ToString()) + Convert.ToDecimal(dr["InstAmt"].ToString()));
                                            }
                                            else if (vBalAmt > 0)
                                            {
                                                if (vBalAmt > Convert.ToDecimal(dr["InstAmt"].ToString()))
                                                {
                                                    vIntAmt += Convert.ToDecimal(dr["InstAmt"].ToString());
                                                    vBalAmt = vBalAmt - Convert.ToDecimal(dr["InstAmt"].ToString());
                                                    vPrinAmt += vBalAmt;
                                                    vBalAmt = 0;
                                                }
                                                else
                                                {
                                                    vIntAmt += vBalAmt;
                                                    vBalAmt = 0;
                                                }
                                            }
                                    }
                                    txtPrinPaid.Text = string.Format("{0:N}", vPrinAmt);
                                    txtIntPaid.Text = string.Format("{0:N}", vIntAmt);
                                }
                            }
                            else
                            {
                                if (txtTotAmtPay.Text == txtTotPaid.Text)
                                {
                                    txtPrinPaid.Text = string.Format("{0:N}", txtPrinPay.Text);
                                    txtIntPaid.Text = string.Format("{0:N}", txtIntPay.Text);
                                }
                                else
                                {
                                    gblFuction.AjxMsgPopup("Invalied Collection Amount.");
                                    gblFuction.focus("ctl00_cph_Main_txtPDTotAmt");
                                    return;
                                }
                            }
                            txtTotPaid.Focus();
                 * 
                 * 
                 *********************************/
                if (txtTotPaid.Text.Trim() == "") return;
                if (txtTotAmtPay.Text.Trim() == "") return;

                double vPrinPay = 0, vIntPay = 0, vBankChrgPay = 0, vTotAmtPay = 0, vPrinPaid = 0, vIntPaid = 0, vBankPaid = 0, vTotPaid = 0;
                if (txtPrinPay.Text.Trim() != "") vPrinPay = Convert.ToDouble(txtPrinPay.Text);
                if (txtIntPay.Text.Trim() != "") vIntPay = Convert.ToDouble(txtIntPay.Text);
                //if (txtBankChrgPay.Text.Trim() != "") vBankChrgPay = Convert.ToDouble(txtBankChrgPay.Text);
                //if (txtPrinPaid.Text.Trim() != "") vPrinPaid = Convert.ToDouble(txtPrinPaid.Text);
                //if (txtIntPaid.Text.Trim() != "") vIntPaid = Convert.ToDouble(txtIntPaid.Text);
                //if (txtBankPaid.Text.Trim() != "") vBankPaid = Convert.ToDouble(txtBankPaid.Text);
                if (txtTotAmtPay.Text.Trim() != "") vTotAmtPay = Convert.ToDouble(txtTotAmtPay.Text);
                if (txtTotPaid.Text.Trim() != "") vTotPaid = Convert.ToDouble(txtTotPaid.Text);

                if (vTotAmtPay <= 0) return;
                if (vTotPaid <= 0) return;

                if (vTotPaid > vTotAmtPay)
                {
                    gblFuction.AjxMsgPopup("Paid Amount Should not be more than Payable Amount...");
                    return;
                }

                //if(vTotPaid > vBankChrgPay)
                //{
                //        vTotPaid = vTotPaid - vBankChrgPay;
                //        txtBankPaid.Text = string.Format("{0:N}",vBankChrgPay);
                //}
                //else
                //{
                //        txtBankPaid.Text = string.Format("{0:N}", vTotPaid);
                //        return;
                //}
                if (vTotPaid > vIntPay)
                {
                    vTotPaid = vTotPaid - vIntPay;
                    txtIntPaid.Text = string.Format("{0:N}", vIntPay);
                }
                else
                {
                    txtIntPaid.Text = string.Format("{0:N}", vTotPaid);
                    return;
                }
                if (vTotPaid > 0)
                {
                    txtPrinPaid.Text = string.Format("{0:N}", vTotPaid);
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
        protected void chkJourEnt_CheckedChanged(object sender, EventArgs e)
        {
            if (chkJurnl.Checked == true)
            {
                ddlLedger.Enabled = true;
                ddlLedger.SelectedIndex = -1;
            }
            else
            {
                ddlLedger.Enabled = false;
                ddlLedger.SelectedIndex = -1;
            }
        }



        protected void chkPreClose_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            DataTable dtM = null;
            Int32 vRow = 0;
            Int32 vTotAmt = 0;
            string vLoanID = "";
            //Decimal vIntAmt = 0.0M;
            dt = (DataTable)ViewState["dtRst"];
            if (ViewState["LoanId"] == null) return;
            vLoanID = (string)ViewState["LoanId"];
            vRow = dt.Rows.IndexOf(dt.Rows.Find(vLoanID));
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            CCollectionRoutine oCC = new CCollectionRoutine();
            dtM = oCC.GetMaxCollDate(vLoanID, vBrCode, "M");
            if (dtM.Rows.Count > 0)
            {
                if (Convert.ToString(dtM.Rows[0]["MaxCollDt"]) != "01/01/1900" && gblFuction.setDate(Convert.ToString(dtM.Rows[0]["MaxCollDt"]).ToString()) >= gblFuction.setDate(txtRecovryDt.Text))
                {
                    gblFuction.AjxMsgPopup(" Last collection date is" + Convert.ToString(dtM.Rows[0]["MaxCollDt"]));
                    chkPreClose.Checked = false;
                    return;
                }
            }
            try
            {
                //vIntAmt = Convert.ToDecimal(txtIntPay.Text);
                if (chkPreClose.Checked == true)
                {
                    txtPrinPay.Text = string.Format("{0:N}", txtPrinOS.Text);
                    txtTotAmtPay.Text = Convert.ToString(Convert.ToDecimal(txtPrinPay.Text) + Convert.ToDecimal(txtIntPay.Text));

                    vTotAmt = Convert.ToInt32(Convert.ToDecimal(txtTotAmtPay.Text));
                    if ((vTotAmt % 5) != 0)
                    {
                        vTotAmt = ((vTotAmt / 5) + 1) * 5;
                    }
                    else
                    {
                        vTotAmt = vTotAmt * 1;
                    }
                    txtTotAmtPay.Text = Convert.ToString(vTotAmt);
                    txtIntPay.Text = Convert.ToString(Convert.ToDecimal(txtTotAmtPay.Text) - Convert.ToDecimal(txtPrinPay.Text));

                    txtPrinPaid.Text = txtPrinPay.Text;
                    txtIntPaid.Text = txtIntPay.Text;
                    txtTotPaid.Text = txtTotAmtPay.Text;
                }
                else
                {
                    txtIntPay.Text = Convert.ToString(dt.Rows[vRow]["InterestDue"]);
                    txtPrinPay.Text = Convert.ToString(dt.Rows[vRow]["PrincpalDue"]);
                    txtTotAmtPay.Text = Convert.ToString(Convert.ToDecimal(txtPrinPay.Text) + Convert.ToDecimal(txtIntPay.Text));
                    txtPrinPaid.Text = txtPrinPay.Text;
                    txtIntPaid.Text = txtIntPay.Text;
                    txtTotPaid.Text = txtTotAmtPay.Text;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }


        protected void ddlPyn_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            Int32 vRow = 0;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                DropDownList ddlPA = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlPA.NamingContainer;
                vRow = gvRow.RowIndex;
                DropDownList ddlPAList = (DropDownList)gvRow.FindControl("ddlPyn");
                dt = (DataTable)ViewState["dtRst"];
                if (ddlPAList.SelectedValue == "N")
                    dt.Rows[vRow]["PA"] = "N";
                ViewState["dtRst"] = dt;
            }
            catch { }
            finally
            {
                dt = null;
            }
        }




        protected void btnDone_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            txtTotPaid.Text = "0";
            txtTotAmtPay.Text = "0";
            Int32 vRow = 0;
            try
            {
                string vBranch = Session[gblValue.BrnchCode].ToString();
                decimal vTotalPaidAmt = Convert.ToDecimal(txtTotPaid.Text);
                decimal vPayTotalAmt = Convert.ToDecimal(txtTotAmtPay.Text);
                DateTime vCollDt = gblFuction.setDate(txtRecovryDt.Text);
                if (ViewState["LoanId"] == null)
                {
                    gblFuction.AjxMsgPopup("Please Select Record before process.");
                    return;
                }

                string vLoanID = Convert.ToString(ViewState["LoanId"]);
                dt = (DataTable)ViewState["dtRst"];
                vRow = dt.Rows.IndexOf(dt.Rows.Find(vLoanID));

                //-------------dt.Rows[vRow]["BankChDue"] = txtBankChrgPay.Text;
                dt.Rows[vRow]["PrincpalPaid"] = txtPrinPaid.Text;
                dt.Rows[vRow]["InterestPaid"] = txtIntPaid.Text;
                //----------- dt.Rows[vRow]["BankChPaid"] = txtBankPaid.Text;

                dt.Rows[vRow]["Total"] = Convert.ToDecimal(txtPrinPaid.Text) + Convert.ToDecimal(txtIntPaid.Text); //--------- + Convert.ToDecimal(txtBankPaid.Text);

                if (chkJurnl.Checked == true && ddlLedger.SelectedIndex == 0)
                {
                    gblFuction.AjxMsgPopup("Please Select Ledger before process.");
                    return;
                }

                if (chkJurnl.Checked == true && ddlLedger.SelectedIndex != 0)
                {
                    dt.Rows[vRow]["DescId"] = ddlLedger.SelectedValue;
                    dt.Rows[vRow]["RorJ"] = "J";
                }

                if (chkPreClose.Checked == true)
                {
                    dt.Rows[vRow]["ClosingType"] = "P";
                    dt.Rows[vRow]["PrincpalDue"] = txtIntPaid.Text;
                }
                else
                {
                    dt.Rows[vRow]["ClosingType"] = "N";
                }
                dt.AcceptChanges();
                gvRecvry.DataSource = dt;
                gvRecvry.DataBind();
                TotCalculation(dt, "gvRecvry");
                chkPreClose.Checked = false;
                txtPrinPay.Text = "0";
                txtPrinPaid.Text = "0";
                txtIntPay.Text = "0";
                txtIntPaid.Text = "0";
                txtTotAmtPay.Text = "0";
                txtTotPaid.Text = "0";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }




        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CBankers oLR = null;
            string sXml = "", vNaration = "", vCollMode = "";
            Int32 vErr = 0, vEoID = 0;
            Boolean vResult = false;
            string vModeAC = "", vLoanAC = "", vInstAC = "";
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                DateTime pAccDate = gblFuction.setDate(txtRecovryDt.Text);
                string pBranch = Session[gblValue.BrnchCode].ToString();
                string vActMstTbl = Session[gblValue.ACVouMst].ToString();
                string vActDtlTbl = Session[gblValue.ACVouDtl].ToString();
                string vFinYear = Session[gblValue.ShortYear].ToString();
                vNaration = "Being the Amt of Loan Repay On ";
                vEoID = 0;

                //if (rdbCashBank.SelectedValue == "C")
                //{
                //    vModeAC = "C0001";
                //    vCollMode = "C";
                //}
                //else if (rdbCashBank.SelectedValue == "B")
                //{

                vModeAC = Convert.ToString(ddlBank.SelectedValue);
                vLoanAC = Convert.ToString(ViewState["DescId"].ToString());
                vInstAC = Convert.ToString(ddlInterest.SelectedValue);
                vCollMode = "B";
                if (vInstAC == "-1")
                {
                    gblFuction.AjxMsgPopup("A/c is not Set for Interest");
                    return;
                }
                //}

                dt = (DataTable)ViewState["dtRst"];
                if (vModeAC == "-1")
                {
                    gblFuction.AjxMsgPopup("A/c is not Set for Bank");
                    return;
                }
                if (dt == null)
                {
                    gblFuction.AjxMsgPopup("Nothing to Save");
                    return;
                }

                //if (dt.Rows[0]["LoanAc"].ToString().Trim() == "")
                //{
                //    gblFuction.AjxMsgPopup("Loan A/c is not Set for this Loan Type");
                //    return;
                //}
                //if (dt.Rows[0]["InstAc"].ToString().Trim() == "")
                //{
                //    gblFuction.AjxMsgPopup("Loan Interest A/c is not Set for this Loan Type");
                //    return;
                //}
                //if (dt.Rows[0]["BankChAc"].ToString().Trim() == "")
                //{
                //    gblFuction.AjxMsgPopup("Bank Charges A/c is not Set for this Loan Type");
                //    return;
                //}

                if (dt == null) return;
                if (pAccDate > vLoginDt)
                {
                    gblFuction.AjxMsgPopup("Collection date should not grater than login date");
                    return;
                }

                //if (ValidateFields() == false)
                //{
                //    btnSaveColl.Enabled = true;
                //    return;
                //}

                if (Convert.ToDecimal(txtTotal.Text) == 0)
                {
                    gblFuction.AjxMsgPopup("No amount is collected");
                    return;
                }

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    if (Convert.ToInt32(dt.Rows[i]["Total"]) == 0)
                //        dt.Rows.RemoveAt(i);
                //}

                //dt.Rows[0]["DescId"] = vModeAC;
                //dt.Rows[0]["LoanAc"] = vModeAC;
                //dt.Rows[0]["InstAC"] = vInstAC;   

                foreach (DataRow row in dt.Rows)
                {
                    row["DescId"] = vModeAC;
                    row["LoanAc"] = vLoanAC;
                    row["InstAC"] = vInstAC;

                    if (Convert.ToDecimal(row["Total"]) == 0)
                        row.Delete();
                }

                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    gblFuction.AjxMsgPopup("No amount is collected");
                    return;
                }
                if (this.RoleId != 1)
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
                oLR = new CBankers();
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    sXml = oSW.ToString();
                }

                vErr = oLR.InsertRepayment(pAccDate, sXml, pBranch, vActMstTbl, vActDtlTbl, vModeAC, vFinYear, vNaration, this.UserID, "I", 0, vCollMode);
                if (vErr == 0)
                {
                    gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
                    vResult = true;
                }
                else
                {
                    gblFuction.AjxMsgPopup(gblMarg.DBError);
                    vResult = false;
                }
                ViewState["dtDtlRst"] = null;
                ViewState["dtRst"] = null;
                //ClearControls();
                chkPreClose.Checked = false;
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




        private void TotCalculation(DataTable dt, string pgvName)
        {
            decimal TotPrin = 0, TotInst = 0, TotAmt = 0;
            if (pgvName == "gvRecvry")
            {
                foreach (DataRow dr in dt.Rows)
                {
                    TotPrin = TotPrin + Convert.ToDecimal(dr["PrincpalPaid"]);
                    TotInst = TotInst + Convert.ToDecimal(dr["InterestPaid"]);
                    TotAmt = TotAmt + Convert.ToDecimal(dr["Total"]);
                }
                txtTotPrin.Text = string.Format("{0:N}", TotPrin);
                txtTotInt.Text = string.Format("{0:N}", TotInst);
                txtTotal.Text = string.Format("{0:N}", TotAmt);
            }
            if (pgvName == "gvLed")
            {
                foreach (DataRow dr in dt.Rows)
                {
                    TotPrin = TotPrin + Convert.ToDecimal(dr["PrinCollAmt"]);
                    TotInst = TotInst + Convert.ToDecimal(dr["IntCollAmt"]);
                }
                txtPrinPaidL.Text = string.Format("{0:N}", TotPrin);
                txtInstPaidL.Text = string.Format("{0:N}", TotInst);
                txtTotalPaidL.Text = string.Format("{0:N}", TotPrin + TotInst);
            }
        }




        protected void btnLdgr_Click(object sender, EventArgs e)
        {
            DataTable dtDtl = null;
            CLoanRecovery oLR = null;
            string vLedTyp = "A";
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                string pLoanID = Convert.ToString(ViewState["LoanId"]);
                oLR = new CLoanRecovery();
                dtDtl = oLR.GetCollectionDtlByLoanId(pLoanID, vLedTyp, vBrCode);
                if (dtDtl.Rows.Count > 0)
                {
                    gvLed.DataSource = dtDtl;
                    gvLed.DataBind();
                    dtDtl.PrimaryKey = new DataColumn[] { dtDtl.Columns["SlNo"] };
                    ViewState["dtDtlRst"] = dtDtl;
                    TotCalculation(dtDtl, "gvLed");
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




        protected void gvLed_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dtDtl = null;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                if (rdbRecvry.SelectedValue != "N")
                {
                    if (e.CommandName == "cmdDel")
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("lbtnShow");
                        foreach (GridViewRow gr in gvLed.Rows)
                        {
                            LinkButton lb = (LinkButton)gr.FindControl("lbtnShow");
                            lb.ForeColor = System.Drawing.Color.Black;
                        }
                        btnShow.ForeColor = System.Drawing.Color.Red;
                        dtDtl = (DataTable)ViewState["dtDtlRst"];
                        //string vLoanID = ViewState["LoanId"].ToString();
                        Int32 vSLNo = Convert.ToInt32(e.CommandArgument);
                        Int32 vRow = dtDtl.Rows.IndexOf(dtDtl.Rows.Find(vSLNo));
                        txtPrinPay.Text = Convert.ToString(dtDtl.Rows[vRow]["PrincDue"]);
                        txtIntPay.Text = Convert.ToString(dtDtl.Rows[vRow]["InstDue"]);
                        //txtBankChrgPay.Text = Convert.ToString(dtDtl.Rows[vRow]["BankChargeDue"]);
                        txtTotAmtPay.Text = Convert.ToString(dtDtl.Rows[vRow]["TotalDue"]);

                        txtPrinPaid.Text = Convert.ToString(dtDtl.Rows[vRow]["PrinCollAmt"]);
                        txtIntPaid.Text = Convert.ToString(dtDtl.Rows[vRow]["IntCollAmt"]);
                        //txtBankPaid.Text = Convert.ToString(dtDtl.Rows[vRow]["BankChargeColl"]);
                        txtTotPaid.Text = Convert.ToString(dtDtl.Rows[vRow]["TotalAmt"]);
                        ViewState["SlNo"] = vSLNo;
                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("Please select the modify recovery...");
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




        protected void btnDel_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            DataTable dtDtl = null;
            DataTable dtW = null;
            CBankers oLR = null;
            DateTime vlastCollDt;
            Int32 vSlNo = 0, vErr = 0, vMaxSLNo = 0;
            string vReffID = "", vLedTyp = "M", vLnStatus = "";
            if (Convert.ToDecimal(txtTotPaid.Text) <= 0) return;
            try
            {
                string vAcMst = Session[gblValue.ACVouMst].ToString();
                string vAcDtl = Session[gblValue.ACVouDtl].ToString();
                string vBranch = Session[gblValue.BrnchCode].ToString();
                string vLoanID = Convert.ToString(ViewState["LoanId"]);
                CCollectionRoutine oCC = new CCollectionRoutine();
                dtW = oCC.GetMaxCollDate(vLoanID, vBranch, "B");
                if (dtW.Rows.Count > 0)
                {
                    if (Convert.ToString(dtW.Rows[0]["IsWriteoff"]) == "Y")
                    {
                        gblFuction.AjxMsgPopup("After Write off you can not delete the collection.");
                        return;
                    }
                }
                vSlNo = (Int32)ViewState["SlNo"];
                vReffID = vLoanID + "-" + vSlNo;
                dt = (DataTable)ViewState["dtDtlRst"];
                vMaxSLNo = Convert.ToInt32(dt.Compute("MAX(SLNo)", ""));
                vlastCollDt = gblFuction.setDate(Convert.ToString(dt.Compute("max(RecDate)", " ")));
                if (vSlNo != vMaxSLNo)
                {
                    gblFuction.AjxMsgPopup("You Can delete only Last Record.");
                    return;
                }
                if (this.RoleId != 1)
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= vlastCollDt)
                        {
                            gblFuction.AjxMsgPopup("You can not Delete, Day end already done..");
                            return;
                        }
                    }
                }
                else
                {
                    oLR = new CBankers();
                    if (rdbLoan.SelectedValue == "O")
                        vLnStatus = "O";
                    else if (rdbLoan.SelectedValue == "C")
                        vLnStatus = "C";
                    vErr = oLR.DeleteRepayment(vLoanID, vSlNo, vLnStatus, vReffID, vAcMst, vAcDtl, vBranch, this.UserID);
                    if (vErr == 0)
                    {
                        gblFuction.AjxMsgPopup(gblMarg.DeleteMsg);
                        dtDtl = oLR.GetBankPaymentDtlByLoanId(vLoanID, vLedTyp, vBranch);
                        gvLed.DataSource = dtDtl;
                        gvLed.DataBind();
                        dtDtl.PrimaryKey = new DataColumn[] { dtDtl.Columns["SlNo"] };
                        ViewState["dtDtlRst"] = dtDtl;
                        TotCalculation(dtDtl, "gvLnDisb");
                        txtPrinPay.Text = "0.00";
                        txtPrinPaid.Text = "0.00";
                        txtIntPay.Text = "0.00";
                        txtIntPaid.Text = "0.00";
                        txtTotAmtPay.Text = "0.00";
                        txtTotPaid.Text = "0.00";
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup(gblMarg.DBError);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                dtDtl = null;
                oLR = null;
            }
        }




        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }



        protected void gvRecvry_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtChque = (TextBox)e.Row.FindControl("txtChque");
                //if (rdbCashBank.SelectedValue == "C")
                //    txtChque.Enabled = false;
                //else if (rdbCashBank.SelectedValue == "B")
                txtChque.Enabled = true;
            }
        }



        protected void txtChque_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CLoanRecovery oLR = null;
            Int32 vRow = 0;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                TextBox txtBox = (TextBox)sender;
                GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
                vRow = gvRow.RowIndex;
                TextBox txtChequeText = (TextBox)gvRow.FindControl("txtChque");
                if (txtChequeText.Text == "")
                    return;
                if (Convert.ToInt32(txtChequeText.Text.Length) != 6)
                {
                    //EnableControl(true);
                    gblFuction.AjxMsgPopup("Cheque No. should be 6 Digit");
                    //gblFuction.focus("ctl00_cph_Main_tabLnDis_PnlDtl_txtChque");
                    txtChequeText.Text = "";
                    return;
                }
                else
                {
                    oLR = new CLoanRecovery();
                    //vRet = oLR.ChkduplicateChechno(txtChequeText.Text, vBrCode);
                    //if (vRet == 1)
                    //{
                    //    gblFuction.AjxMsgPopup("Cheque No is duplicate");
                    //    return;
                    //}
                    dt = (DataTable)ViewState["dtRst"];
                    dt.Rows[vRow]["ChequeNo"] = txtChequeText.Text;
                    dt.AcceptChanges();
                    gvRecvry.DataSource = dt;
                    gvRecvry.DataBind();
                    ViewState["dtRst"] = dt;
                }
            }
            catch { }
            finally
            {
                dt = null;
                oLR = null;
            }
        }


        protected void ddlBanker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["DescId"] = null;
            DataTable dt = null;
            CBankers oBNK = null;
            if (ddlBanker.SelectedIndex > 0)
            {

                oBNK = new CBankers();
                dt = oBNK.GetBankerAcHead(Convert.ToInt32(ddlBanker.SelectedValue));
                if (dt.Rows.Count > 0)
                {
                    ViewState["DescId"] = Convert.ToString(dt.Rows[0]["Descid"].ToString());
                }
                else
                {
                    ViewState["DescId"] = null;
                }

            }
        }
    }
}