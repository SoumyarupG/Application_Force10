using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Data.OleDb;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class BankersLoan : CENTRUMBase
    {
        protected int cPgNo = 0;
        protected int vFlag = 0;
        private static string vFile;
        private static string vExcelPath = ConfigurationManager.AppSettings["DBPath"];
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {

                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
                    StatusButton("View");

                popBanker();
                LoadBankLedger();
                txtFrmDt.Text = (string)Session[gblValue.LoginDate];
                txtToDt.Text = (string)Session[gblValue.LoginDate];
                txtLnDt.Text = (string)Session[gblValue.LoginDate];
                LoadGrid(0);
                ViewState["StateEdit"] = null;
                StatusButton("View");
                tabLoanDisb.ActiveTabIndex = 0;
                popMature(1);
                popManual(1);
                PopPool();
                //tabLoanDisb.Tabs[0].Enabled = true;
                //tabLoanDisb.Tabs[1].Enabled = false;
                //tabLoanDisb.Tabs[2].Enabled = false;

            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(0);
        }
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CBankers oLD = null;
            Int32 vRows = 0;
            string vBrCode = string.Empty;
            vBrCode = (string)Session[gblValue.BrnchCode];
            DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            oLD = new CBankers();
            dt = oLD.GetBankerLoanPG(vFrmDt, vToDt, vBrCode, pPgIndx, ref vRows);
            gvLoanAppl.DataSource = dt.DefaultView;
            gvLoanAppl.DataBind();
            lblTotalPages.Text = CalTotPgs(vRows).ToString();
            lblCurrentPage.Text = cPgNo.ToString();
            if (cPgNo == 0)
            {
                Btn_Previous.Enabled = false;
                if (Int32.Parse(lblTotalPages.Text) > 1)
                    Btn_Next.Enabled = true;
                else
                    Btn_Next.Enabled = false;
            }
            else
            {
                Btn_Previous.Enabled = true;
                if (cPgNo == Int32.Parse(lblTotalPages.Text))
                    Btn_Next.Enabled = false;
                else
                    Btn_Next.Enabled = true;
            }
            CalculateTotal();
        }
        /// <summary>
        /// 
        /// </summary>
        private void CalculateTotal()
        {
            double DrAmt = 0.00;
            for (Int32 i = 0; i < gvLoanAppl.Rows.Count; i++)
            {
                DrAmt = Math.Round(DrAmt, 2) + Math.Round(Convert.ToDouble(gvLoanAppl.Rows[i].Cells[3].Text), 2);
            }
            txtDisAmt.Text = Convert.ToString(DrAmt);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRows"></param>
        /// <returns></returns>
        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }
            LoadGrid(cPgNo);
            tabLoanDisb.ActiveTabIndex = 0;
            //tabLoanDisb.Tabs[0].Enabled = true;
            //tabLoanDisb.Tabs[1].Enabled = false;
            //tabLoanDisb.Tabs[2].Enabled = false;
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadBankLedger()
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            string vBranch = Session[gblValue.BrnchCode].ToString();
            oVoucher = new CVoucher();
            dt = oVoucher.GetAcGenLedCB(vBranch, "B", "");
            ddlBank.DataSource = dt;
            ddlBank.DataTextField = "Desc";
            ddlBank.DataValueField = "DescId";
            ddlBank.DataBind();
            ListItem liSel = new ListItem("<--- Select --->", "-1");
            ddlBank.Items.Insert(0, liSel);
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Disbursement(Banker's Loan)";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBankerLoan);
                if (this.UserID == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Disbursement(Group-Leading)", false);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSchedul_Click(object sender, EventArgs e)
        {
            //if (ValidaeField() == false)
            //    return;
            //CBankers oBK = null;
            //string vPMora = "N";
            //string vInstType = ddlInstType.SelectedValue.ToString();
            //decimal vLoanAmt = Convert.ToDecimal(txtLnAmt.Text);
            //decimal vInstRate = Convert.ToDecimal(txtIntRate.Text);
            //Int32 vInstallNo = Convert.ToInt32(txtInstNo.Text);
            //Int32 vInstPeriod = Convert.ToInt32(txtIntPeriod.Text);
            //DateTime vStartDt = gblFuction.setDate(txtStDt.Text);
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            //string vPaySchedule = Convert.ToString(ddlRpSchdle.SelectedValue);
            //Int32 vPMoraInst = Convert.ToInt32(txtPInstlmnt.Text);
            //Int32 vFrDueday = Convert.ToInt32((gblFuction.setDate(txtStDt.Text) - gblFuction.setDate(txtLnDt.Text)).TotalDays);
            //oBK = new CBankers();

            //int vCalDays = Convert.ToInt32(rdbCalDays.SelectedValue.ToString());
            //decimal vTDSPerc = Convert.ToDecimal(txtTDSPerc.Text.ToString() == "" ? "0" : txtTDSPerc.Text.ToString());


            // L is for Only generate Not for Saving
            //if (!chkIsEqualEMI.Checked)
            //    GetBankSchedule(vInstType, vLoanAmt, vInstRate, vInstallNo, vInstPeriod, vStartDt, "L", "", "N", vBrCode, vPaySchedule, "", "", vPMoraInst, gblFuction.setDate(txtLnDt.Text), ddlInstallAmt.SelectedValue,vCalDays,vTDSPerc);
            //else
            //    GetBankScheduleEqualEMI(vInstType, vLoanAmt, vInstRate, vInstallNo, vInstPeriod, vStartDt, "L", "", "N", vBrCode, vPaySchedule, "", "", vPMoraInst, gblFuction.setDate(txtLnDt.Text), ddlInstallAmt.SelectedValue,vCalDays,vTDSPerc);

        }


        //protected void btnImpSchedul_Click(object sender, EventArgs e)
        //{
        //    DataSet ds = null;
        //    DataTable dt = null;
        //    decimal vLoanInt = 0;
        //    if (fuExcl.HasFile == true)
        //    {
        //        if (!Directory.Exists(vExcelPath))
        //        {
        //            Directory.CreateDirectory(vExcelPath);
        //        }
        //        vFile = fuExcl.FileName;
        //        FileInfo vfile = new FileInfo(vExcelPath + vFile);
        //        if (vfile.Exists)
        //        {
        //            vfile.Delete();
        //        }
        //        fuExcl.SaveAs(vExcelPath + vFile);
        //        if (vfile.Exists)
        //        {
        //            gblFuction.AjxMsgPopup("Upload Successful !!");
        //            ds = ExcelToDS(vExcelPath + vFile);
        //            dt = ds.Tables[0];
        //            //dt.Columns.Add("DueDt1");
        //            //dt.AcceptChanges();

        //            if (dt.Rows.Count > 0)
        //            {
        //                gvSchdl.DataSource = dt;
        //                gvSchdl.DataBind();
        //                vLoanInt = Convert.ToDecimal(dt.Compute("Sum(InstAmt)", ""));
        //                txtIntAmt.Text = vLoanInt.ToString();
        //            }
        //            ViewState["Sdl"] = dt;

        //            //tabLoanDisb.Tabs[0].Enabled = false;
        //            tabLoanDisb.Tabs[1].Enabled = true;
        //            tabLoanDisb.Tabs[2].Enabled = true;
        //            tabLoanDisb.ActiveTabIndex = 2;
        //        }
        //    }
        //    else
        //        gblFuction.AjxMsgPopup("Please Select a File");
        //}


        public DataSet ExcelToDS(string Path)
        {
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            conn.Open();
            strExcel = "select * from [sheet1$]";
            try
            {
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "ImportExcel");
                conn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                gblFuction.AjxMsgPopup("Error:Please Check Excel");
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        /*        
                /// <summary>
                /// 
                /// </summary>
                /// <param name="pLoanTypeID"></param>
                /// <param name="pLoanAmt"></param>
                /// <param name="pInterest"></param>
                /// <param name="pInstallNot"></param>
                /// <param name="pInstPeriod"></param>
                /// <param name="pStatrDt"></param>
                /// <param name="pType"></param>
                /// <param name="pLoanID"></param>
                /// <param name="pIsDisburse"></param>
                /// <param name="pBranch"></param>
                /// <param name="pPaySchedule"></param>
                /// <param name="pBank"></param>
                /// <param name="pChequeNo"></param>
                /// <param name="pCollDay"></param>
                /// <param name="pCollDayNo"></param>
                /// <param name="pLoanType"></param>
                /// 

                private void GetBankSchedule(string pInstType, decimal pLoanAmt, decimal pInterest, int pInstallNo, int pInstPeriod, DateTime pStatrDt,
                    string pType, string pLoanID, string pIsDisburse, string pBranch, string pPaySchedule, string pChequeNo, string pBank,
                    Int32 pPMoraAmt, DateTime pLoanDisbDt, string vInsAmtPeriod, int vCalDays, decimal vTDSPerc)
                {
                    DataTable dt = null;
                    CBankers oLD = null;
                    double VLoanInt = 0.0;
                    oLD = new CBankers();
                    dt = oLD.GetBankSchedule(pInstType, pLoanAmt, pInterest, pInstallNo, pInstPeriod, pStatrDt, "L", pLoanID, "N", pBranch, pPaySchedule, pChequeNo, pBank, pPMoraAmt, pLoanDisbDt, vInsAmtPeriod, vCalDays, vTDSPerc);
                    if (dt.Rows.Count > 0)
                    {
                        gvSchdl.DataSource = dt;
                        gvSchdl.DataBind();
                        foreach (DataRow dr in dt.Rows)
                        {
                            VLoanInt = VLoanInt + 0;
                        }
                        VLoanInt = Convert.ToDouble(dt.Compute("Sum(InstAmt)", ""));
                        txtIntAmt.Text = VLoanInt.ToString();
                    }

                    //tabLoanDisb.Tabs[0].Enabled = false;
                    tabLoanDisb.Tabs[1].Enabled = true;
                    tabLoanDisb.Tabs[2].Enabled = true;
                    tabLoanDisb.ActiveTabIndex = 2;
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
                private void GetBankScheduleEqualEMI(string pInstType, decimal pLoanAmt, decimal pInterest, int pInstallNo, int pInstPeriod, DateTime pStatrDt,
                   string pType, string pLoanID, string pIsDisburse, string pBranch, string pPaySchedule, string pChequeNo, string pBank, decimal pPMoraAmt, 
                    DateTime pLoanDisbDt, string vInsAmtPeriod,int vCalDays, decimal vTDSPerc)
                {
                    DataTable dt = null;
                    CBankers oLD = null;
                    double VLoanInt = 0.0;
                    oLD = new CBankers();
                    dt = oLD.GetBankScheduleEqualEMI(pInstType, pLoanAmt, pInterest, pInstallNo, pInstPeriod, pStatrDt, "L", pLoanID, "N", pBranch, pPaySchedule, pChequeNo, pBank, pPMoraAmt, pLoanDisbDt, vInsAmtPeriod,vCalDays,vTDSPerc);
                    if (dt.Rows.Count > 0)
                    {
                        gvSchdl.DataSource = dt;
                        gvSchdl.DataBind();
                        foreach (DataRow dr in dt.Rows)
                        {
                            VLoanInt = VLoanInt + 0;
                        }
                        VLoanInt = Convert.ToDouble(dt.Compute("Sum(InstAmt)", ""));
                        txtIntAmt.Text = VLoanInt.ToString();
                    }

                    //tabLoanDisb.Tabs[0].Enabled = false;
                    tabLoanDisb.Tabs[1].Enabled = true;
                    tabLoanDisb.Tabs[2].Enabled = true;
                    tabLoanDisb.ActiveTabIndex = 2;
                }
        */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanAdd == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                ViewState["StateEdit"] = "Add";
                tabLoanDisb.ActiveTabIndex = 1;
                ////tabLoanDisb.Tabs[0].Enabled = false;
                ////tabLoanDisb.Tabs[1].Enabled = true;
                ////tabLoanDisb.Tabs[2].Enabled = false;
                StatusButton("Add");
                ClearControls();
                txtLnDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                ViewState["Sdl"] = null;
                txtSpread.Enabled = false;
                txtBaseRt.Enabled = false;
                popManual(1);
                popMature(1);
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabLoanDisb.ActiveTabIndex = 0;
            //tabLoanDisb.Tabs[0].Enabled = true;
            //tabLoanDisb.Tabs[1].Enabled = false;
            //tabLoanDisb.Tabs[2].Enabled = false;
            EnableControl(false);
            StatusButton("View");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadGrid(1);
                    ClearControls();
                    tabLoanDisb.ActiveTabIndex = 0;
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidaeField()
        {
            bool vRes = true;


            ////if (ddlAppNo.SelectedIndex <= 0)
            ////{
            ////    gblFuction.MsgPopup("Application No Cannot be Left Blank ..");
            ////    gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlAppNo");
            ////    return vRes = false;
            ////}

            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            if (gblFuction.IsDate(txtLnDt.Text.Trim()) == false)
            {
                gblFuction.MsgPopup("Loan Date Should be in DD/MM/YYYY Format ..");
                gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_txtLnDt");
                return vRes = false;
            }
            //*******************************************************
            if (txtLnDt.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Loan Disburse Date cannot be empty.");
                gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtAppDt");
                return vRes = false;
            }
            else
            {
                DateTime vLnDate = gblFuction.setDate(txtLnDt.Text);
                //if (vLnDate < vFinFrom || vLnDate > vFinTo)
                //{
                //    EnableControl(true);
                //    gblFuction.MsgPopup("Loan Disburse Date should be Financial Year.");
                //    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtAppDt");
                //    return vRes = false;
                //}
                if (vLnDate > vLoginDt)
                {
                    EnableControl(true);
                    gblFuction.MsgPopup("Loan Disburse Date should not be greater than login date.");
                    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtAppDt");
                    vRes = false;
                }
            }
            return vRes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)  //Check Account
        {
            DateTime vPrevDueDt=Convert.ToDateTime("01/01/1900");
            if (ValidaeField() == false)
                return false;

            //if (txtLnAmt.Text.Trim() == "")
            //{
            //    gblFuction.AjxMsgPopup("Loan Amount is Blank...");
            //    return false;
            //}


            //if (txtLnDt.Text.Trim() == "")
            //{
            //    gblFuction.AjxMsgPopup("Loan Date is Blank...");
            //    return false;
            //}

            //if (txtIntRate.Text.Trim() == "")
            //{
            //    gblFuction.AjxMsgPopup("Loan Interest Rate is Blank...");
            //    return false;
            //}

            //if (txtLnCycle.Text.Trim() == "")
            //{
            //    gblFuction.AjxMsgPopup("Loan Cycle is Blank...");
            //    return false;
            //}

            //if (txtIntPeriod.Text.Trim() == "")
            //{
            //    gblFuction.AjxMsgPopup("Loan Period is Blank...");
            //    return false;
            //}

            //if (txtInstNo.Text.Trim() == "")
            //{
            //    gblFuction.AjxMsgPopup("Number of Installment is Blank...");
            //    return false;
            //}

            //if (ddlBank.SelectedIndex == -1)
            //{
            //    gblFuction.AjxMsgPopup("Deposited Bank A/c not Selected...");
            //    return false;
            //}

            if (ddlBanker.SelectedIndex == -1)
            {
                gblFuction.AjxMsgPopup("Loan Amount is Blank...");
                return false;
            }

            //if (txtStDt.Text.Trim() == "")
            //{
            //    gblFuction.AjxMsgPopup("Loan Collection Start Date is Blank...");
            //    return false;
            //}


            //if (txtChqNo.Text.Trim() != "")
            //{
            //    if (txtChqDt.Text.Trim() == "")
            //    {
            //        gblFuction.AjxMsgPopup("Check Date is Blank...");
            //        return false;
            //    }
            //}


            Boolean vResult = false;
            DataTable dt = null, dtSDL = null, dtMtr=null;
            string vBranch = Session[gblValue.BrnchCode].ToString();
            string vNarationL = string.Empty;
            string vNarationF = string.Empty;
            string vXmlSdl = "", vXmlMature="";
            //string vEmail = "", vBody = "", vSubject = "", vConEmailId="";
            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vTblDtl = Session[gblValue.ACVouDtl].ToString();
            string vFinYear = Session[gblValue.ShortYear].ToString();

            string vLoanId = Convert.ToString(ViewState["LoanId"]);
            string vLoanNo = Convert.ToString(txtLnNo.Text.Trim() == "" ? "" : txtLnNo.Text.Trim());
            decimal vFLDGAmt = 0;
            //vFLDGAmt = Convert.ToDecimal(txtFdlg.Text.Trim() == "" ? "0" : txtFdlg.Text.ToString());
            Int32 vBanker = Convert.ToInt32(ddlBanker.SelectedValue.ToString() == "-1" ? "0" : ddlBanker.SelectedValue.ToString());
            //string vFdlgBy = Convert.ToString(ddlFdlg.SelectedValue.ToString() == "-1" ? "" : ddlFdlg.SelectedValue.ToString());
            string vLoanAcNo = Convert.ToString(txtAccountNo.Text.ToString() == "" ? "" : txtAccountNo.Text.ToString());
            string vRpSchdle = Convert.ToString(ddlRpSchdle.SelectedValue.ToString() == "-1" ? "" : ddlRpSchdle.SelectedValue.ToString());
            decimal vFdlgPrcOwn = 0;
            vFdlgPrcOwn = Convert.ToDecimal(txtFdlgPerc.Text == "" ? "0" : txtFdlgPerc.Text);
            decimal vFldgPercPrt = 0;
           // vFldgPercPrt = Convert.ToDecimal(txtFldgPercPrt.Text == "" ? "0" : txtFldgPercPrt.Text);
            string vInstType = Convert.ToString(ddlInstType.SelectedValue.ToString() == "-1" ? "" : ddlInstType.SelectedValue.ToString());
            DateTime vMaturityDt = gblFuction.setDate(txtMd.Text.Trim() == "" ? "01/01/1900" : txtMd.Text.Trim());
            decimal vMerginPer = Convert.ToDecimal(txtMerginPer.Text.Trim() == "" ? "0" : txtMerginPer.Text.Trim());
            decimal vMargin = Convert.ToDecimal(txtMargin.Text.Trim() == "" ? "0" : txtMargin.Text.Trim());
            decimal vMarginmoney = Convert.ToDecimal(txtMarginmoney.Text.Trim() == "" ? "0" : txtMarginmoney.Text);
            decimal vPropercent = Convert.ToDecimal(txtPropercent.Text.Trim() == "" ? "0" : txtPropercent.Text.Trim());
            decimal vProamount = Convert.ToDecimal(txtProamount.Text.Trim() == "" ? "0" : txtProamount.Text);
            decimal vOfpercent = Convert.ToDecimal(txtOfpercent.Text.Trim() == "" ? "0" : txtOfpercent.Text.Trim());
            decimal vOfamount = Convert.ToDecimal(txtOfamount.Text.Trim() == "" ? "0" : txtOfamount.Text);
            DateTime vSancDt = gblFuction.setDate(txtSancDt.Text.Trim() == "" ? "01/01/1900" : txtSancDt.Text.ToString());
            decimal vSancAmt = Convert.ToDecimal(txtSancAmt.Text.ToString() == "" ? "0" : txtSancAmt.Text.ToString());
            DateTime vFLDGDate = gblFuction.setDate(txtFLDGDate.Text.Trim() == "" ? "01/01/1900" : txtFLDGDate.Text.ToString());
            decimal vFLDGPeriod = 0;
            //vFLDGPeriod = Convert.ToDecimal(txtFLDGPeriod.Text.Trim() == "" ? "0" : txtFLDGPeriod.Text);
            decimal vPenalPer = 0;
            //vPenalPer=Convert.ToDecimal(txtPenalPer.Text.Trim() == "" ? "0" : txtPenalPer.Text.Trim());
            decimal vPenalInt = 0;
            //vPenalInt=Convert.ToDecimal(txtPenalInt.Text.Trim() == "" ? "0" : txtPenalInt.Text.Trim());
            decimal vServTaxPer = Convert.ToDecimal(txtServTaxPer.Text.Trim() == "" ? "0" : txtServTaxPer.Text.Trim());
            decimal vServTaxAmt = Convert.ToDecimal(txtServTaxAmt.Text.Trim() == "" ? "0" : txtServTaxAmt.Text.Trim());
            decimal vPInstlmnt = Convert.ToInt32(txtPInstlmnt.Text.Trim() == "" ? "0" : txtPInstlmnt.Text.Trim());
            decimal vTDSPerc = Convert.ToDecimal(txtTDSPerc.Text.Trim() == "" ? "0" : txtTDSPerc.Text.Trim());
            string vIsEqualEMI = "N";
            //vIsEqualEMI=chkIsEqualEMI.Checked == true ? "Y" : "N";
            decimal vCalDays = 0;
            //vCalDays = Convert.ToDecimal(rdbCalDays.SelectedValue.ToString() == "-1" ? "0" : rdbCalDays.SelectedValue.ToString());
            decimal vIInstlmnt = Convert.ToDecimal(txtIInstlmnt.Text.ToString() == "" ? "0" : txtIInstlmnt.Text.Trim());
            decimal vLoanType = Convert.ToDecimal(rdbLoanType.SelectedValue.ToString() == "-1" ? "0" : rdbLoanType.SelectedValue.ToString());
            decimal vLnAmt = Convert.ToDecimal(txtLnAmt.Text.Trim() == "" ? "0" : txtLnAmt.Text.Trim());
            decimal vIntRate = 0;
            if (txtBaseRt.Text == "0" && txtSpread.Text=="0")
            vIntRate = Convert.ToDecimal(txtIntRate.Text.Trim() == "" ? "0" : txtIntRate.Text.Trim());
            if (txtBaseRt.Text != "0" || txtSpread.Text != "0")
            vIntRate = Convert.ToDecimal(Request[txtIntRate.UniqueID] as string == "0" ? txtIntRate.Text : Request[txtIntRate.UniqueID] as string);
            //decimal vLnCycle = Convert.ToDecimal(txtLnCycle.Text.Trim() == "" ? "0" : txtLnCycle.Text.Trim());
            decimal vIntAmt = Convert.ToDecimal(txtIntAmt.Text.Trim() == "" ? "0" : txtIntAmt.Text.Trim());
            decimal vIntPeriod = Convert.ToDecimal(txtIntPeriod.Text.Trim() == "" ? "0" : txtIntPeriod.Text.Trim());
            decimal vInstNo = Convert.ToDecimal(txtInstNo.Text.Trim() == "" ? "0" : txtInstNo.Text.Trim());
            DateTime vLnDt = gblFuction.setDate(txtLnDt.Text.Trim() == "" ? "01/01/1900" : txtLnDt.Text.Trim());
            DateTime vStDt = gblFuction.setDate(txtStDt.Text.Trim() == "" ? "01/01/1900" : txtStDt.Text.Trim());
            string vInstPayMode = "";
            //vInstPayMode=Convert.ToString(ddlInstallAmt.SelectedValue.ToString() == "-1" ? "" : ddlInstallAmt.SelectedValue.Trim());
            string vChqNo = Convert.ToString(txtChqNo.Text.Trim() == "" ? "" : txtChqNo.Text.Trim());
            DateTime vChqDt = gblFuction.setDate(txtChqDt.Text.Trim() == "" ? "01/01/1900" : txtChqDt.Text.Trim());
            string vDepBank = ddlBank.SelectedValue.ToString() == "" ? "-1" : ddlBank.SelectedValue.ToString();
            string vRemarks = Convert.ToString(txtRemarks.Text.Trim() == "" ? "" : txtRemarks.Text.Trim());
            decimal vBaseRt = Convert.ToDecimal(txtBaseRt.Text.Trim() == "" ? "0" : txtBaseRt.Text.Trim());
            decimal vSpread = Convert.ToDecimal(txtSpread.Text.Trim() == "" ? "0" : txtSpread.Text.Trim());
            decimal vFLDGPerc = Convert.ToDecimal(txtFdlgPerc.Text.Trim() == "" ? "0" : txtFdlgPerc.Text.Trim());
            string vGuarCollateral = "", vGuarCorporate = "", vGuarPersonal="";
            string vProcFees = Convert.ToString(ddlProcFees.SelectedValue.ToString() == "-1" ? "" : ddlProcFees.SelectedValue.ToString());
            string vSchedule = Convert.ToString(ddlSchedule.SelectedValue.ToString() == "-1" ? "" : ddlSchedule.SelectedValue.ToString());
            string vLIEN = Convert.ToString(txtMarkedTo.Text.Trim() == "" ? "" : txtMarkedTo.Text.Trim());
            string vFLDGBank = Convert.ToString(txtFLDGBankNm.Text.Trim() == "" ? "" : txtFLDGBankNm.Text.Trim());
            string vFLDGAcNo = Convert.ToString(txtFLDGAcNo.Text.Trim() == "" ? "" : txtFLDGAcNo.Text.Trim());
            
            dtSDL = (DataTable)ViewState["Manual"];
            if (dtSDL.Rows.Count == 0)
            {
                gblFuction.AjxMsgPopup("Repayment Schedule Not Set....");
                return false;
            }

            if (dtSDL != null)
            {
                if (dtSDL.Rows[0]["PrinceAmt"].ToString() == "" && ddlSchedule.SelectedValue == "C")
                {
                    gblFuction.AjxMsgPopup("Your Schedule should be Complete...");
                    return false;
                }

                if (dtSDL.Rows[0]["PrinceAmt"].ToString() != "")
                {
                    TextBox txtBankNm = (TextBox)gvManSchdl.Rows[gvManSchdl.Rows.Count - 1].FindControl("txtBankNm");
                    TextBox txtGChqNo = (TextBox)gvManSchdl.Rows[gvManSchdl.Rows.Count - 1].FindControl("txtChqNo");
                    TextBox txtGChqDt = (TextBox)gvManSchdl.Rows[gvManSchdl.Rows.Count - 1].FindControl("txtChqDt");
                    dtSDL.Rows[dtSDL.Rows.Count - 1]["BankName"] = txtBankNm.Text;
                    dtSDL.Rows[dtSDL.Rows.Count - 1]["ChequeNo"] = txtGChqNo.Text;
                    dtSDL.Rows[dtSDL.Rows.Count - 1]["ChequeDt"] = txtGChqDt.Text;
                    foreach (DataRow dr in dtSDL.Rows)
                    {
                        dr["DueDt"] = gblFuction.setDate(dr["DueDt"].ToString());
                        dr["ChequeDt"] = gblFuction.setDate(dr["ChequeDt"].ToString());
                        if (vPrevDueDt != Convert.ToDateTime("01/01/1900"))
                        {
                            if (Convert.ToDateTime(dr["DueDt"].ToString()) != Convert.ToDateTime("01/01/1900"))
                            {
                                if (vPrevDueDt > Convert.ToDateTime(dr["DueDt"].ToString()))
                                {
                                    gblFuction.AjxMsgPopup("Next schedule date should be grater than previous schedule date");
                                    return false;
                                }
                            }
                        }
                        vPrevDueDt = Convert.ToDateTime(dr["DueDt"].ToString());

                    }
                    if (ddlSchedule.SelectedValue == "C")
                    {
                        if (Convert.ToDouble(dtSDL.Rows[dtSDL.Rows.Count - 1]["Outstanding"]) != 0)
                            return false;
                    }
                    dtSDL.TableName = "Table1";
                    using (StringWriter oSW = new StringWriter())
                    {
                        dtSDL.WriteXml(oSW);
                        vXmlSdl = oSW.ToString().Replace("T00:00:00+05:30", "");
                    }
                }
            }
            dtMtr = (DataTable)ViewState["Mature"];
            if (dtMtr != null)
            {
                if (dtMtr.Rows[dtMtr.Rows.Count - 1]["FLDGAmt"].ToString() == "")
                    dtMtr.Rows[dtMtr.Rows.Count - 1].Delete();
                if (dtMtr.Rows.Count > 0)
                {
                    if (dtMtr.Rows[0]["FLDGAmt"].ToString() == "")
                    {
                        gblFuction.AjxMsgPopup("FLDG details should not be blank...");
                        return false;
                    }
                    //if (dtMtr.Rows[dtMtr.Rows.Count - 1]["FLDGAmt"].ToString() == "")
                    //    dtMtr.Rows[dtMtr.Rows.Count - 1].Delete();
                    foreach (DataRow dr in dtMtr.Rows)
                    {
                        dr["MatureDt"] = gblFuction.setDate(dr["MatureDt"].ToString());

                    }
                    dtMtr.TableName = "TabMature";
                    using (StringWriter oSW = new StringWriter())
                    {
                        dtMtr.WriteXml(oSW);
                        vXmlMature = oSW.ToString().Replace("T00:00:00+05:30", "");
                    }
                }
            }
            CBankers oBK = null;


            /*
            vBanker = ddlBanker.SelectedValue.ToString();
            vRschedule = ddlRpSchdle.SelectedValue.ToString();
            vInstType = ddlInstType.SelectedValue.ToString();
            vLoanAmt = Convert.ToDecimal(txtLnAmt.Text);
            vIntRate = Convert.ToDecimal(txtIntRate.Text);
            vLoanCycle = Convert.ToInt32(txtLnCycle.Text);
            vIntPeriod = Convert.ToInt32(txtIntPeriod.Text);
            vInstNo = Convert.ToInt32(txtInstNo.Text);
            dtLoan = gblFuction.setDate(txtLnDt.Text);
            dtCollSt = gblFuction.setDate(txtStDt.Text);
            vChqDt = gblFuction.setDate(txtChqDt.Text);
            FLDGMatDT = gblFuction.setDate(txtMd.Text);
            vChqNo = txtChqNo.Text;
            vBank = ddlBank.SelectedValue.ToString();
            vNarationL = "Being the Amt of Loan taken from " + ddlBanker.SelectedItem;
            vNarationF = "";//Being the Amt of Fees For Loan Disbursed for " + ddlBank.SelectedItem;
            vIntAmt = Convert.ToDecimal(txtIntAmt.Text);
            vFLDGAmt = Convert.ToDecimal(txtFdlg.Text);
            vFLDGPer = Convert.ToDecimal(txtFdlgPerc.Text);
            vFLDGPerPrt = Convert.ToDecimal(txtFldgPercPrt.Text == "" ? "0" : txtFldgPercPrt.Text);

            decimal vMarginPer = Convert.ToDecimal(txtMargin.Text == "" ? "0" : txtMargin.Text);
            decimal vMarginAmt = Convert.ToDecimal(txtMarginmoney.Text == "" ? "0" : txtMarginmoney.Text);
            decimal vProcePer = Convert.ToDecimal(txtPropercent.Text == "" ? "0" : txtPropercent.Text);
            decimal vProceAmt = Convert.ToDecimal(txtProamount.Text == "" ? "0" : txtProamount.Text);
            decimal vOtherPer = Convert.ToDecimal(txtOfpercent.Text == "" ? "0" : txtOfpercent.Text);
            decimal vOtherAmt = Convert.ToDecimal(txtOfamount.Text == "" ? "0" : txtOfamount.Text);
            decimal vPenalPer = Convert.ToDecimal(txtPenalPer.Text == "" ? "0" : txtPenalPer.Text);
            decimal vPenalAmt = Convert.ToDecimal(txtPenalInt.Text == "" ? "0" : txtPenalInt.Text);
            decimal vSrvTaxPer = Convert.ToDecimal(txtServTaxPer.Text == "" ? "0" : txtServTaxPer.Text);
            decimal vSrvTaxAmt = Convert.ToDecimal(txtServTaxAmt.Text == "" ? "0" : txtServTaxAmt.Text);
            decimal vTDSPer = Convert.ToDecimal(txtTDSPerc.Text == "" ? "0" : txtTDSPerc.Text);
            
            vFLDGBy = ddlFdlg.SelectedValue.ToString();
            vIsRickshaw = rdbLoanType.SelectedValue.ToString(); // chkIsRickshawSangh.Checked == true ? "Y" : "N";
            vIntCalMethod = Convert.ToInt32(rdbCalDays.SelectedValue.ToString());

            vSanctionDate = txtSancDt.Text != "" ? gblFuction.setDate(txtSancDt.Text) : gblFuction.setDate("01/01/1900");
            vSanctionAmt = txtSancAmt.Text != "" ? Convert.ToInt32(txtSancAmt.Text) : 0;
            vFLDGDate = txtFLDGDate.Text != "" ? gblFuction.setDate(txtFLDGDate.Text) : gblFuction.setDate("01/01/1900");
            vFLDGPeriod = txtFLDGPeriod.Text != "" ? Convert.ToInt32(txtFLDGPeriod.Text) : 0;
            vPenalIntRate = txtPenalInt.Text != "" ? Convert.ToDecimal(txtPenalInt.Text) : 0;
            
            oBK = new CBankers();
            if (ChkPm.Checked == true)
            {
                vPrintMora = Convert.ToInt32(txtPInstlmnt.Text);
            }
            else
            {
                vPrintMora = Convert.ToInt32("0");
                vXmlData = "";
            }
            if (ChkIm.Checked == true)
            {
                vIntMora = Convert.ToInt32(txtIInstlmnt.Text);
            }
            else
            {
                vIntMora = Convert.ToInt32("0");
                vXmlData = "";
            }
            * 
            */
            int vErr = 0;
            oBK = new CBankers();
            if (chkGuarCollateral.Checked == true)
                vGuarCollateral = "Y";
            else
                vGuarCollateral = "N";
            if (chkGuarCorporate.Checked == true)
                vGuarCorporate = "Y";
            else
                vGuarCorporate = "N";
            if (chkGuarPersonal.Checked == true)
                vGuarPersonal = "Y";
            else
                vGuarPersonal = "N";
            if (Mode == "Save")
            {
                //if (this.RoleId != 1 && this.RoleId != 2)
                //{
                //    if (Session[gblValue.EndDate] != null)
                //    {
                //        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtLnDt.Text))
                //        {
                //            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                //            return false;
                //        }
                //    }
                //

                vErr = oBK.InsertBankLoanMst(ref vLoanNo, vFLDGAmt, vBanker, "", vLoanAcNo, vRpSchdle, vFdlgPrcOwn, vFldgPercPrt, vInstType, vMaturityDt, vMerginPer
                            , vMargin, vMarginmoney, vPropercent, vProamount, vOfpercent, vOfamount, vSancDt, vSancAmt, vFLDGDate, vFLDGPeriod, vPenalPer, vPenalInt
                            , vServTaxPer, vServTaxAmt, vPInstlmnt, vTDSPerc, vIsEqualEMI, vCalDays, vIInstlmnt, vLoanType, vLnAmt, vIntRate, txtLnCycle.Text, vIntAmt
                            , vIntPeriod, vInstNo, vLnDt, vStDt, vChqNo, vChqDt, vInstPayMode, vDepBank, vRemarks, vBranch, vTblMst, vTblDtl, vFinYear, vXmlSdl, vXmlMature,
                              vBaseRt, vSpread, vFLDGPerc, vGuarCollateral, vGuarCorporate, vGuarPersonal, vProcFees, vSchedule, vLIEN, this.UserID, vFLDGBank, vFLDGAcNo
                              ,Convert.ToInt32(txtMoraPrd.Text),Convert.ToInt32(txtTenure.Text),Convert.ToInt32(ddlPool.SelectedValue));

                if (vErr == 0)
                {
                    ViewState["LoanId"] = vLoanNo;
                    txtLnNo.Text = vLoanNo;
                    gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
                    vResult = true;
                }
                else
                {
                    gblFuction.AjxMsgPopup(gblMarg.DBError);
                    vResult = false;
                }

            }
            else if (Mode == "Edit")
            {
                //if (this.RoleId != 1 && this.RoleId != 2)
                //{
                //    if (Session[gblValue.EndDate] != null)
                //    {
                //        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtLnDt.Text))
                //        {
                //            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                //            return false;
                //        }
                //    }
                //}
                vErr = oBK.UpdateBankLoanMst(vLoanId, vLoanNo, vFLDGAmt, vBanker, "", vLoanAcNo, vRpSchdle, vFdlgPrcOwn, vFldgPercPrt, vInstType, vMaturityDt, vMerginPer
                            , vMargin, vMarginmoney, vPropercent, vProamount, vOfpercent, vOfamount, vSancDt, vSancAmt, vFLDGDate, vFLDGPeriod, vPenalPer, vPenalInt
                            , vServTaxPer, vServTaxAmt, vPInstlmnt, vTDSPerc, vIsEqualEMI, vCalDays, vIInstlmnt, vLoanType, vLnAmt, vIntRate, txtLnCycle.Text, vIntAmt
                            , vIntPeriod, vInstNo, vLnDt, vStDt, vChqNo, vChqDt, vInstPayMode, vDepBank, vRemarks, vBranch, vTblMst, vTblDtl, vFinYear, vXmlSdl, vXmlMature,
                              vBaseRt, vSpread, vFLDGPerc, vGuarCollateral, vGuarCorporate, vGuarPersonal, vProcFees, vSchedule, vLIEN, this.UserID, vFLDGBank, vFLDGAcNo
                              , Convert.ToInt32(txtMoraPrd.Text), Convert.ToInt32(txtTenure.Text),Convert.ToInt32(ddlPool.SelectedValue));


                if (vErr == 0)
                {
                    gblFuction.AjxMsgPopup(gblMarg.EditMsg);
                    vResult = true;
                }
                else
                {
                    gblFuction.AjxMsgPopup(gblMarg.DBError);
                    vResult = false;
                }
            }
            else if (Mode == "Delete")
            {
                CCollectionRoutine oColl = new CCollectionRoutine();
                dt = oColl.GetMaxCollDate(vLoanId, vBranch, "M");
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["MaxCollDt"]) != "01/01/1900" && gblFuction.setDate(Convert.ToString(dt.Rows[0]["MaxCollDt"]).ToString()) >= gblFuction.setDate(txtLnDt.Text))
                    {
                        gblFuction.AjxMsgPopup("After collection you can not delete the loan..");
                        return false;
                    }
                }
                //if (this.RoleId != 1 && this.RoleId != 2)
                //{
                //    if (Session[gblValue.EndDate] != null)
                //    {
                //        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtLnDt.Text))
                //        {
                //            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                //            return false;
                //        }
                //    }
                //}
                vErr = oBK.DeleteBankLoanMst(txtLnNo.Text, gblFuction.setDate(txtLnDt.Text), this.UserID, vBranch, vTblMst, vTblDtl);
                if (vErr == 0)
                {
                    vResult = true;
                    gblFuction.AjxMsgPopup(gblMarg.DeleteMsg);
                }
                else
                {
                    gblFuction.AjxMsgPopup(gblMarg.DBError);
                    vResult = false;
                }
            }


            /*
            vSubject = "Banker's Loan Disbursement";
            vBody = "Loan amount of Rs." + txtLnAmt.Text + " has been taken from " + ddlBanker.SelectedItem.Text + " on " + txtLnDt.Text;
            vEmail = ConfigurationManager.AppSettings["RcvEmail"];

            SendToMail(vEmail, vBody, vSubject);
            */
            LoadGrid(0);
            return vResult;
        }

        public static void SendToMail(string pMail, string pBody, string pSubject)
        {
            string vMTo = "", vBody = "";
            string vCompEmail = ConfigurationManager.AppSettings["CompEmail"];
            string vCompPwd = ConfigurationManager.AppSettings["CompPwd"];
            try
            {
                vMTo = pMail;
                if (vMTo != "")
                {
                    vBody = pBody;
                    MailMessage oM = new MailMessage();
                    oM.To.Add(vMTo);
                    oM.From = new MailAddress(vCompEmail);
                    oM.Subject = pSubject;
                    oM.Body = vBody;
                    oM.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Host = "smtp.gmail.com";
                    smtp.Credentials = new System.Net.NetworkCredential(vCompEmail, vCompPwd);
                    smtp.EnableSsl = true;
                    //smtp.Timeout = 360000;
                    smtp.Send(oM);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        //////private string DataTableTOXml(DataTable dt)
        //////{
        //////    string sXml = "";
        //////    using (StringWriter oSW = new StringWriter())
        //////    {
        //////        dt.WriteXml(oSW);
        //////        sXml = oSW.ToString();
        //////    }
        //////    return sXml;
        //////}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(ViewState["IsMig"]) == "Y")
                {
                    gblFuction.MsgPopup("Migrated Loan Cannot be Modified");
                    return;
                }
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                ViewState["StateEdit"] = "Edit";
                StatusButton("Edit");
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
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                //LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    //gblFuction.focus("ctl00_cph_Main_tabLnDisb_pnlDtl_ddlCo");
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    gblFuction.focus("ctl00_cph_Main_tabLnDisb_pnlDtl_ddlCo");
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        /// 
        private void EnableControl(Boolean Status)
        {
            txtAccountNo.Enabled = Status;
            txtLnNo.Enabled = Status;
            ddlBanker.Enabled = Status;
            ddlRpSchdle.Enabled = Status;
            ddlInstType.Enabled = Status;
            txtLnAmt.Enabled = Status;
            txtIntRate.Enabled = Status;
            txtLnCycle.Enabled = Status;
            txtIntAmt.Enabled = Status;
            //ddlInstallAmt.Enabled = Status;
            txtIntPeriod.Enabled = Status;
            txtInstNo.Enabled = Status;
            txtIntsNo.Enabled = Status;
            txtLnDt.Enabled = Status;
            txtStDt.Enabled = Status;
            txtToDt.Enabled = Status;
            txtChqNo.Enabled = Status;
            txtChqDt.Enabled = Status;
            ddlBank.Enabled = Status;
            //btnImpSchdl.Enabled = Status;
            txtFdlgPerc.Enabled = Status;
            txtMargin.Enabled = Status;
            txtMarginmoney.Enabled = Status;
            txtPropercent.Enabled = Status;
            //txtFdlg.Enabled = Status;
            ChkPm.Enabled = Status;
            ChkIm.Enabled = Status;
            txtIInstlmnt.Enabled = Status;
            txtPInstlmnt.Enabled = Status;
           // ddlFdlg.Enabled = Status;
            txtMd.Enabled = Status;
            txtOfpercent.Enabled = Status;
            //chkIsRickshawSangh.Enabled = Status;
            chkGuarCollateral.Enabled = Status;
            chkGuarCorporate.Enabled = Status;
            chkGuarPersonal.Enabled = Status;
            ddlProcFees.Enabled = Status;
            ddlSchedule.Enabled = Status;
            gvFLDG.Enabled = Status;
            gvManSchdl.Enabled = Status;
            txtServTaxPer.Enabled = Status;
            txtServTaxAmt.Enabled = Status;
            txtTDSPerc.Enabled = Status;
            txtMarkedTo.Enabled = Status;
            txtSancAmt.Enabled = Status;
            txtFLDGDate.Enabled = Status;
            txtSancDt.Enabled = Status;
            txtBaseRt.Enabled = Status;
            txtSpread.Enabled = Status;
            txtOfamount.Enabled = Status;
            txtFLDGAcNo.Enabled = Status;
            txtFLDGBankNm.Enabled = Status;
            txtMoraPrd.Enabled = Status;
            txtTenure.Enabled = Status;
            ddlPool.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtLnNo.Text = "";
            ddlRpSchdle.SelectedIndex = 3;
            ddlInstType.SelectedIndex = 2;
            txtLnAmt.Text = "0";
            txtIntRate.Text = "0";
            txtLnCycle.Text = "";
            txtIntAmt.Text = "0";
            //ddlInstallAmt.SelectedIndex = 0;
            txtIntPeriod.Text = "0";
            txtInstNo.Text = "0";
            txtIntsNo.Text = "0";
            txtLnDt.Text = Session[gblValue.LoginDate].ToString();
           // txtMd.Text = Session[gblValue.LoginDate].ToString();
            txtStDt.Text = "";
            txtChqNo.Text = "";
            txtChqDt.Text = "";
            ddlBanker.SelectedIndex = -1;
            ddlBank.SelectedIndex = -1;
            //txtFdlg.Text = "0";
            txtFdlgPerc.Text = "0";
            txtMargin.Text = "0";
            txtMarginmoney.Text = "0";
            txtPropercent.Text = "0";
            //txtProamount.Text = "0";
            txtPInstlmnt.Text = "0";
            txtIInstlmnt.Text = "0";
            txtOfpercent.Text = "0";
            //txtOfamount.Text = "0";
            txtMd.Text = "";
            //chkIsRickshawSangh.Checked = false;
            chkGuarPersonal.Checked = false;
            chkGuarCorporate.Checked = false;
            chkGuarCollateral.Checked = false;
            txtBaseRt.Text = "0";
            txtSpread.Text = "0";
            ddlProcFees.SelectedIndex = -1;
            ddlSchedule.SelectedIndex = -1;
            txtTDSPerc.Text = "0";
            txtFLDGDate.Text = "";
            txtSancDt.Text = "";
            txtAccountNo.Text = "";
            txtLnDt.Text = "";
            txtFLDGBankNm.Text = "";
            txtFLDGAcNo.Text = "";
            txtMoraPrd.Text = "0";
            txtTenure.Text = "0";
            ddlPool.SelectedIndex = -1;
        }
        /// <summary>
        /// 
        /// </summary>
        private void popBanker()
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLoanAppl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dtDtl = null, dt=null, dtMtr=null;
            CDisburse oLD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vLoanId = "", vPrintMora = "", vIntMora = "";
            try
            {
                vLoanId = Convert.ToString(e.CommandArgument);
                ViewState["LoanId"] = vLoanId;
                //ViewState["LoanId"] = Convert.ToString(dt.Rows[0]["LoanId"]);
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvLoanAppl.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oLD = new CDisburse();
                    ds = oLD.GetBankLoanByLoanId(vLoanId);
                    dt = ds.Tables[0];
                    dtDtl = ds.Tables[1];
                    dtMtr = ds.Tables[2];
                    if (dt.Rows.Count > 0)
                    {
                        txtAccountNo.Text = Convert.ToString(dt.Rows[0]["LoanAcNo"].ToString());
                        txtLnNo.Text = Convert.ToString(dt.Rows[0]["LoanId"]).ToString();
                      //  txtFdlg.Text = Convert.ToString(dt.Rows[0]["LoanNo"]).ToString();
                        ddlBanker.SelectedIndex = ddlBanker.Items.IndexOf(ddlBanker.Items.FindByValue(Convert.ToString(dt.Rows[0]["BankerID"]).ToString()));
                        txtLnDt.Text = Convert.ToString(dt.Rows[0]["LoanDt"]).ToString();
                        txtLnCycle.Text = Convert.ToString(dt.Rows[0]["dose"]).ToString();
                        ddlRpSchdle.SelectedIndex = ddlRpSchdle.Items.IndexOf(ddlRpSchdle.Items.FindByValue(Convert.ToString(dt.Rows[0]["RSchedule"]).ToString()));
                        ddlInstType.SelectedIndex = ddlInstType.Items.IndexOf(ddlInstType.Items.FindByValue(Convert.ToString(dt.Rows[0]["InstType"]).ToString()));
                        txtLnAmt.Text = Convert.ToString(dt.Rows[0]["LoanAmt"]).ToString();
                        txtIntRate.Text = Convert.ToString(dt.Rows[0]["IntRate"]).ToString();
                        txtIntAmt.Text = Convert.ToString(dt.Rows[0]["IntAmt"]).ToString();
                        txtIntPeriod.Text = Convert.ToString(dt.Rows[0]["Period"]).ToString();
                        txtInstNo.Text = Convert.ToString(dt.Rows[0]["TotalInstNo"]).ToString();
                        txtStDt.Text = Convert.ToString(dt.Rows[0]["CollStartDt"]).ToString();
                        txtPInstlmnt.Text = Convert.ToString(dt.Rows[0]["PrinMora"]).ToString();
                        txtIInstlmnt.Text = Convert.ToString(dt.Rows[0]["IntMora"]).ToString();
                        //chkIsEqualEMI.Checked = Convert.ToString(dt.Rows[0]["IsEqualEMI"]).ToString() == "Y" ? true : false;
                        //rdbCalDays.SelectedIndex = rdbCalDays.Items.IndexOf(rdbCalDays.Items.FindByValue(Convert.ToString(dt.Rows[0]["IntCalcMethod"])));
                        rdbLoanType.SelectedIndex = rdbLoanType.Items.IndexOf(rdbLoanType.Items.FindByValue(Convert.ToString(dt.Rows[0]["BankerLoanType"]).ToString()));
                        txtRemarks.Text = Convert.ToString(dt.Rows[0]["ClosingRemarks"]).ToString();
                       // ddlFdlg.SelectedIndex = ddlFdlg.Items.IndexOf(ddlFdlg.Items.FindByValue(Convert.ToString(dt.Rows[0]["FLDGBy"]).ToString()));
                        txtFLDGDate.Text = Convert.ToString(dt.Rows[0]["FLDGDate"]).ToString();
                        //txtFLDGPeriod.Text = Convert.ToString(dt.Rows[0]["FLDGPeriod"]).ToString();
                        //txtFdlg.Text = Convert.ToString(dt.Rows[0]["FLDGAmt"]).ToString();
                        txtFdlgPerc.Text = Convert.ToString(dt.Rows[0]["FLDGPerOwn"]).ToString();
                        //txtFldgPercPrt.Text = Convert.ToString(dt.Rows[0]["FLDGPerPartner"]).ToString();
                        //txtFLDGDate.Text = Convert.ToString(dt.Rows[0]["FLDGMatDT"]).ToString();
                        //chkIsEqualEMI.Checked == true ? "Y" : "N";
                        //txtMarginmoney.Text = Convert.ToString(dt.Rows[0]["FLDGInt"]).ToString();
                        txtMarginmoney.Text = Convert.ToString(dt.Rows[0]["FLDGMarMoney"]).ToString();
                        txtSancDt.Text = Convert.ToString(dt.Rows[0]["SanctionDate"]).ToString();
                        txtSancAmt.Text = Convert.ToString(dt.Rows[0]["SanctionAmt"]).ToString();
                        txtMerginPer.Text = Convert.ToString(dt.Rows[0]["MarginPer"]).ToString();
                        txtMargin.Text = Convert.ToString(dt.Rows[0]["MarginAmt"]).ToString();
                        txtPropercent.Text = Convert.ToString(dt.Rows[0]["ProcessPerc"]).ToString();
                        txtProamount.Text = Convert.ToString(dt.Rows[0]["ProcessAmt"]).ToString();
                        txtOfpercent.Text = Convert.ToString(dt.Rows[0]["OtherPerc"]).ToString();
                        txtOfamount.Text = Convert.ToString(dt.Rows[0]["OtherAmt"]).ToString();
                        txtServTaxPer.Text = Convert.ToString(dt.Rows[0]["SrvTaxPer"]).ToString();
                        txtServTaxAmt.Text = Convert.ToString(dt.Rows[0]["SrvTaxAmt"]).ToString();
                        txtTDSPerc.Text = Convert.ToString(dt.Rows[0]["TDSPerc"]).ToString();
                        txtBaseRt.Text = Convert.ToString(dt.Rows[0]["BaseRate"]).ToString();
                        txtSpread.Text=Convert.ToString(dt.Rows[0]["Spread"]).ToString();
                        txtMarkedTo.Text = Convert.ToString(dt.Rows[0]["LIENMarkedTo"]).ToString();
                        txtFdlgPerc.Text = Convert.ToString(dt.Rows[0]["FLDGPerc"]).ToString();
                        if (Convert.ToString(dt.Rows[0]["GuarPersonal"]).ToString() == "Y")
                            chkGuarPersonal.Checked=true;
                        else
                            chkGuarPersonal.Checked=false;
                        if (Convert.ToString(dt.Rows[0]["GuarCorporate"]).ToString() == "Y")
                            chkGuarCorporate.Checked = true;
                        else
                            chkGuarCorporate.Checked = false;
                        if (Convert.ToString(dt.Rows[0]["GuarCollateral"]).ToString() == "Y")
                            chkGuarCollateral.Checked = true;
                        else
                            chkGuarCollateral.Checked = false;
                        
                        ddlSchedule.SelectedIndex = ddlSchedule.Items.IndexOf(ddlSchedule.Items.FindByValue(Convert.ToString(dt.Rows[0]["Schedule"]).ToString()));
                        ddlProcFees.SelectedIndex = ddlProcFees.Items.IndexOf(ddlProcFees.Items.FindByValue(Convert.ToString(dt.Rows[0]["ProcFeesYN"]).ToString()));
                        txtFLDGBankNm.Text = Convert.ToString(dt.Rows[0]["FLDGBank"]).ToString();
                        txtMoraPrd.Text = Convert.ToString(dt.Rows[0]["MoraPrd"]).ToString();
                        txtTenure.Text = Convert.ToString(dt.Rows[0]["Tenure"]).ToString();
                        ddlPool.SelectedIndex = ddlPool.Items.IndexOf(ddlPool.Items.FindByValue(Convert.ToString(dt.Rows[0]["PoolId"]).ToString()));
                        //txtFLDGAcNo.Text = Convert.ToString(dt.Rows[0]["FLDGAcNo"]).ToString();


                        //txtPenalPer.Text = Convert.ToString(dt.Rows[0]["PenalIntRate"]).ToString();
                        //txtPenalInt.Text = Convert.ToString(dt.Rows[0]["PenalIntAmt"]).ToString();
                        //ddlInstallAmt.SelectedIndex = ddlInstallAmt.Items.IndexOf(ddlInstallAmt.Items.FindByValue(Convert.ToString(dt.Rows[0]["InstPayMode"]).ToString()));
                        ddlBank.SelectedIndex = ddlBank.Items.IndexOf(ddlBank.Items.FindByValue(Convert.ToString(dt.Rows[0]["DescID"]).ToString()));

                        LblUser.Text = "Last Modified By : " + Convert.ToString(dt.Rows[0]["UserName"]);
                        LblDate.Text = "Last Modified Date : " + Convert.ToString(dt.Rows[0]["CreationDateTime"]);

                        //dtDtl = oLD.GetBnkLnSchedule(vLoanId);

                        //ViewState["Sdl"] = dtDtl;
                        if (dtMtr.Rows.Count > 0)
                        {
                            ViewState["Mature"] = dtMtr;
                            gvFLDG.DataSource = dtMtr;
                            gvFLDG.DataBind();
                        }
                        if (dtDtl.Rows.Count > 0)
                        {
                            ViewState["Manual"] = dtDtl;
                            gvManSchdl.DataSource = dtDtl;
                            gvManSchdl.DataBind();
                        }
                        else
                        {
                            popManual(1);
                        }
                        //if (dtDtl.Rows.Count > 0)
                        //{
                        //    gvMSchdl.DataSource = dtDtl;
                        //    gvMSchdl.DataBind();
                        //}
                        //tabLoanDisb.Tabs[0].Enabled = true;
                        //tabLoanDisb.Tabs[1].Enabled = true;
                        //tabLoanDisb.Tabs[2].Enabled = true;
                        tabLoanDisb.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                    //EnableControl(false);
                }
            }
            finally
            {
                dt = null;
                dtDtl = null;
                oLD = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtLnDt_TextChanged(object sender, EventArgs e)
        {
            ////Int32 vCollDay = 0, vCollDayNo = 0;
            ////DataTable dtDay = null;
            ////CCollectionRoutine oCR = null;
            ////if (ddlAppNo.SelectedIndex <= 0)
            ////{
            ////    gblFuction.AjxMsgPopup("Please Select the application No...");
            ////    return;
            ////}
            ////if (txtLnDt.Text != "" && gblFuction.IsDate(txtLnDt.Text) == true)
            ////{
            ////    oCR = new CCollectionRoutine();
            ////    dtDay = oCR.GetCollDay(Convert.ToString(ddlGrp.SelectedValue));
            ////    if (dtDay.Rows.Count > 0)
            ////    {
            ////        vCollDay = Convert.ToInt32(dtDay.Rows[0]["CollDay"]);
            ////        vCollDayNo = Convert.ToInt32(dtDay.Rows[0]["CollDayNo"]);
            ////        txtStDt.Text = gblFuction.GetStartDate(gblFuction.setDate(txtLnDt.Text), vCollDay, vCollDayNo, Convert.ToString(ddlRpSchdle.SelectedValue), Convert.ToString(ddlGrp.SelectedValue));
            ////        txtDay.Text = Convert.ToString(gblFuction.setDate(txtStDt.Text).DayOfWeek);
            ////    }
            ////}
            ////else
            ////{
            ////    gblFuction.AjxMsgPopup("Invalid Loan Date...");
            ////    return;
            ////}
        }

        protected void txtIntsNo_TextChanged(object sender, EventArgs e)
        {
            tabLoanDisb.ActiveTabIndex = 2;
            int vRow = Convert.ToInt32(txtIntsNo.Text);
            
        }

        private void popManual(int vRow)
        {
            DataTable dt = null;
            dt = GetManualSchedule(vRow);
            ViewState["Manual"] = dt;
            gvManSchdl.DataSource = dt;
            gvManSchdl.DataBind();
        }
        private void popMature(int vRow)
        {
            DataTable dt = null;
            dt = GetMaturityData(vRow);
            ViewState["Mature"] = dt;
            gvFLDG.DataSource = dt;
            gvFLDG.DataBind();
        }

        private DataTable GetManualSchedule(int vRow)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("InstNo", typeof(int));
            dt.Columns.Add("DueDt");
            dt.Columns.Add("PrinceAmt", typeof(double));
            dt.Columns.Add("InstAmt");
            dt.Columns.Add("ResAmt");
            dt.Columns.Add("TDSAmt");
            dt.Columns.Add("NetInstallAmt");
            dt.Columns.Add("Outstanding");
            dt.Columns.Add("BankName");
            dt.Columns.Add("ChequeNo");
            dt.Columns.Add("ChequeDt");
            dt.Columns.Add("IsPaid");
            for (int i = 0; i < vRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dt.Rows[i]["InstNo"] = i + 1;
                dt.Rows[i]["IsPaid"] = "N";
            }
            return dt;
        }
        private DataTable GetMaturityData(int vRow)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("FLDGAcNo");
            dt.Columns.Add("FLDGAmt");
            dt.Columns.Add("MatureDt");
            dt.Columns.Add("MatureAmt");
            for (int i = 0; i < vRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            return dt;

        }
        protected void txtPrincAmt_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Manual"];
            int curRow = 0;
            double vPrin = 0, vInt = 0, vTDS=0, vTDTAmt=0;
            //DateTime vDue = System.DateTime.Now;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;

            TextBox txtDueDt = (TextBox)gvManSchdl.Rows[curRow].FindControl("txtDueDate");
            TextBox txtPrin = (TextBox)gvManSchdl.Rows[curRow].FindControl("txtPrincAmt");
            TextBox txtInt = (TextBox)gvManSchdl.Rows[curRow].FindControl("txtIntAmt");
            TextBox txtTDSAmt = (TextBox)gvManSchdl.Rows[curRow].FindControl("txtTDSAmt");
            if (txtPrin.Text != "")
                vPrin = Convert.ToDouble(txtPrin.Text);
            else
                vPrin = 0;

            if (txtInt.Text != "")
                vInt = Convert.ToDouble(txtInt.Text);
            else
                vInt = 0;
            if (txtTDSPerc.Text != "")
                vTDS = Convert.ToDouble(txtTDSPerc.Text);
            else
                vTDS = 0;
            vTDTAmt = (vTDS * vInt) / 100;
            txtTDSAmt.Text = vTDTAmt.ToString();
            //if (txtDueDt.Text != "")
            //    vDue = Convert.ToDateTime(txtDueDt.Text);

            //vDue = gblFuction.getDate(txtDueDt.Text);
            dt.Rows[curRow]["InstNo"] = gvManSchdl.Rows.Count;
            dt.Rows[curRow]["TDSAmt"] = txtTDSAmt.Text;
            dt.Rows[curRow]["DueDt"] = txtDueDt.Text;
            dt.Rows[curRow]["PrinceAmt"] = Convert.ToDouble(vPrin);
            dt.Rows[curRow]["ResAmt"] = (vPrin + vInt);

            //double drSum = Convert.ToDouble(dt.Compute("Sum(Princ)", ""));
            //if (Math.Floor(drSum) > Convert.ToDouble(lblAmount.Text))
            //{
            //    gblFuction.AjxMsgPopup("Loan Amount is Exceeded Rs." + (drSum - Convert.ToDouble(lblAmount.Text)));
            //    txtPrin.Text = "";
            //    return;
            //}
            Outstanding(curRow, gvManSchdl.Rows.Count);
            
            dt.AcceptChanges();
            ViewState["Manual"] = dt;
            gvManSchdl.DataSource = dt;
            gvManSchdl.DataBind();
        }


        /// <summary>
        /// 
        /// </summary>
        private void Outstanding(int curRow, int vSchNo)
        {
            DataTable dt = (DataTable)ViewState["Manual"];
            for (int j = curRow; j < vSchNo; j++)
            {
                if (j == 0)
                    dt.Rows[j]["Outstanding"] = Convert.ToDouble(txtLnAmt.Text) - Convert.ToDouble(dt.Rows[j]["PrinceAmt"]);
                else if (Convert.ToString(dt.Rows[j]["PrinceAmt"]) != "")
                {
                    if (Convert.ToString(dt.Rows[j - 1]["PrinceAmt"]) != "" && Convert.ToString(dt.Rows[j - 1]["Outstanding"]) != "")
                    {
                        if (j == vSchNo)
                            dt.Rows[j]["Outstanding"] = (Math.Floor(Convert.ToDouble(dt.Rows[j - 1]["Outstanding"]) - Convert.ToDouble(dt.Rows[j]["PrinceAmt"]))).ToString();
                        else
                            dt.Rows[j]["Outstanding"] = (Convert.ToDouble(dt.Rows[j - 1]["Outstanding"]) - Convert.ToDouble(dt.Rows[j]["PrinceAmt"])).ToString();
                    }
                    else
                    {
                        dt.Rows[j]["ResAmt"] = "";
                        dt.Rows[j]["PrinceAmt"] = 0;
                        gblFuction.AjxMsgPopup("Please Fill Previous Rows");
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtIntAmt_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Manual"];
            int curRow = 0;
            double vPrin = 0, vInt = 0, vTDS = 0, vTDTAmt = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;

            TextBox txtPrin = (TextBox)gvManSchdl.Rows[curRow].FindControl("txtPrincAmt");
            TextBox txtInt = (TextBox)gvManSchdl.Rows[curRow].FindControl("txtIntAmt");
            TextBox txtTDSAmt = (TextBox)gvManSchdl.Rows[curRow].FindControl("txtTDSAmt");
            if (txtPrin.Text != "")
                vPrin = Convert.ToDouble(txtPrin.Text);
            else
                vPrin = 0;

            if (txtInt.Text != "")
                vInt = Convert.ToDouble(txtInt.Text);
            else
                vInt = 0;
            if (txtTDSPerc.Text != "")
                vTDS = Convert.ToDouble(txtTDSPerc.Text);
            else
                vTDS = 0;
            vTDTAmt = (vTDS * vInt) / 100;
            txtTDSAmt.Text = vTDTAmt.ToString();
            dt.Rows[curRow]["TDSAmt"] = txtTDSAmt.Text;

            dt.Rows[curRow]["InstAmt"] = vInt;
            dt.Rows[curRow]["ResAmt"] = (vPrin + vInt);
            dt.Rows[curRow]["NetInstallAmt"] = (vPrin + vInt - vTDTAmt);
            dt.AcceptChanges();
            ViewState["Manual"] = dt;
            gvManSchdl.DataSource = dt;
            gvManSchdl.DataBind();
        }

        protected void txtTDSAmt_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Manual"];
            int curRow = 0;
            double vTDS = 0, vInst = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;

            vInst = Convert.ToDouble(dt.Rows[curRow]["ResAmt"]);
            TextBox txtTDS = (TextBox)gvManSchdl.Rows[curRow].FindControl("txtTDSAmt");
            if (txtTDS.Text != "")
                vTDS = Convert.ToDouble(txtTDS.Text);
            else
                vTDS = 0;

            //if (txtInt.Text != "")
            //    vInt = Convert.ToDouble(txtInt.Text);
            //else
            //    vInt = 0;

            dt.Rows[curRow]["TDSAmt"] = vTDS;
            dt.Rows[curRow]["NetInstallAmt"] = (vInst - vTDS);
            dt.AcceptChanges();
            ViewState["Manual"] = dt;
            gvManSchdl.DataSource = dt;
            gvManSchdl.DataBind();
        }

        protected void btnSave_Click1(object sender, EventArgs e)
        {
            tabLoanDisb.ActiveTabIndex = 3;
            txtIntAmt.Text="";
            GridViewRow LastRow = gvManSchdl.Rows[gvManSchdl.Rows.Count - 1];
            double splitter = Convert.ToDouble(gvManSchdl.Rows[gvManSchdl.Rows.Count - 1].Cells[7].Text.ToString());
            if (splitter == 0)
            {
               GetDepTable();
            }
            else
            {
                gblFuction.AjxMsgPopup("Outstanding would be 0");
            }

        }

        private void GetDepTable()
        {
            Int32 IntAmt = 0;
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[8] { new DataColumn("InstNo"), new DataColumn("DueDt"), new DataColumn("PrinceAmt"), new DataColumn("InstAmt"), new DataColumn("ResAmt"), new DataColumn("TDSAmt"), new DataColumn("NetInstallAmt"), new DataColumn("Outstanding") });
            foreach (GridViewRow gr in gvManSchdl.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {
                    int vSl = Convert.ToInt32(gr.Cells[0].Text);
                    TextBox txtDueDt = (TextBox)gr.Cells[1].FindControl("txtDueDate");
                    gr.Cells[1].Text = txtDueDt.Text;
                    TextBox txtPrin = (TextBox)gr.Cells[2].FindControl("txtPrincAmt");
                    gr.Cells[2].Text = txtPrin.Text;
                    TextBox txtInt = (TextBox)gr.Cells[3].FindControl("txtIntAmt");
                    gr.Cells[3].Text = txtInt.Text;

                    //TextBox txtPrin = (TextBox)gvManSchdl.Rows[gr.RowIndex].FindControl("txtPrincAmt");
                    //dt.Rows[gr.RowIndex]["Princ"] = txtPrin.Text;
                    //TextBox txtInt = (TextBox)gvManSchdl.Rows[gr.RowIndex].FindControl("txtIntAmt");
                    //dt.Rows[gr.RowIndex]["IntAmt"] = txtInt.Text;

                    double vInst = Convert.ToDouble(gr.Cells[4].Text);
                    TextBox txtTDS = (TextBox)gr.Cells[5].FindControl("txtTDSAmt");
                    gr.Cells[5].Text = txtTDS.Text;
                    double vnetInst = Convert.ToDouble(gr.Cells[6].Text);
                    double vPOS = Convert.ToDouble(gr.Cells[7].Text);

                    dt.Rows.Add(vSl,txtDueDt.Text, txtPrin.Text, txtInt.Text, vInst, txtTDS.Text, vnetInst, vPOS);
                    dt.AcceptChanges();
                    IntAmt = (IntAmt + Convert.ToInt32(txtInt.Text));
                }
                txtIntAmt.Text =Convert.ToString(IntAmt);
            }

            //gvMSchdl.DataSource = dt;
            //gvMSchdl.DataBind();
            ViewState["Sd"] = dt;
            
        }
        protected void btnAddNew1_Click(object sender, EventArgs e)
        {
            int curRow = 0, maxRow = 0;
            DataRow dr;
            DataTable dt= (DataTable)ViewState["Manual"];
            Button btnAdd = (Button)sender;
            GridViewRow gR = (GridViewRow)btnAdd.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtBankNm = (TextBox)gvManSchdl.Rows[curRow].FindControl("txtBankNm");
            TextBox txtChqNo = (TextBox)gvManSchdl.Rows[curRow].FindControl("txtChqNo");
            TextBox txtChqDt = (TextBox)gvManSchdl.Rows[curRow].FindControl("txtChqDt");
            dt.Rows[gR.RowIndex]["BankName"] = txtBankNm.Text;
            dt.Rows[gR.RowIndex]["ChequeNo"] = txtChqNo.Text;
            dt.Rows[gR.RowIndex]["ChequeDt"] = txtChqDt.Text;
            dt.AcceptChanges();
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.AcceptChanges();
            ViewState["Manual"] = dt;
            gvManSchdl.DataSource = dt;
            gvManSchdl.DataBind();
            //SetData();
        }
        //public void GetData(int vRow)
        //{
        //    DataTable dt= (DataTable)ViewState["Manual"];
        //    for (int i = 0; i <= vRow; i++)
        //    {
        //        dt.Rows[i]["BankName"]=
        //    }

        //}
        protected void ImDel1_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            dt = (DataTable)ViewState["Manual"];
            if (dt.Rows.Count > 1)
            {
                dt.Rows[gR.RowIndex].Delete();
                dt.AcceptChanges();
                ViewState["Manual"] = dt;
                gvManSchdl.DataSource = dt;
                gvManSchdl.DataBind();

            }
            else if (dt.Rows.Count == 1)
            {
                popManual(1);

                return;
            }
           

        }
        //protected void txtMatureAmt_TextChanged(object sender, EventArgs e)
        //{
        //    int rowindex = 0, vMaxRow = 0;
        //    TextBox txtVal = (TextBox)sender;
        //    GridViewRow gvr = (GridViewRow)txtVal.NamingContainer;
        //    rowindex = gvr.RowIndex;
        //    vMaxRow = gvFLDG.Rows.Count;
        //    TextBox txtMatureAmt = (TextBox)gvFLDG.Rows[rowindex].FindControl("txtMatureAmt");

        //    if (rowindex == vMaxRow - 1)
        //    {
        //        if (txtMatureAmt.Text.Trim() != "")
        //        {
        //            AddNewRow();
        //        }
        //        else
        //        {
        //            gblFuction.AjxMsgPopup("Please feed all values in Condition Details");
        //            txtMatureAmt.Text = "";
        //        }
        //    }

        //}
     
        public void AddNewRow()
        {
            if (ViewState["Mature"] != null)
            {
                DataTable dt = (DataTable)ViewState["Mature"];
                DataRow dr;
                if (dt.Rows.Count > 0)
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    for (int i = 0; i < dt.Rows.Count - 1; i++)
                    {
                        TextBox txtFLDGAccNo = (TextBox)gvFLDG.Rows[i].FindControl("txtFLDGAccNo");
                        TextBox txtFLDGAmt = (TextBox)gvFLDG.Rows[i].FindControl("txtFLDGAmt");
                        TextBox txtMatureDt = (TextBox)gvFLDG.Rows[i].FindControl("txtMatureDt");
                        TextBox txtMatureAmt = (TextBox)gvFLDG.Rows[i].FindControl("txtMatureAmt");
                        dt.Rows[i]["FLDGAcNo"] = txtFLDGAccNo.Text;
                        dt.Rows[i]["FLDGAmt"] = txtFLDGAmt.Text;
                        dt.Rows[i]["MatureDt"] = txtMatureDt.Text;
                        dt.Rows[i]["MatureAmt"] = txtMatureAmt.Text;
                    }
                    ViewState["Mature"] = dt;
                    gvFLDG.DataSource = dt;
                    gvFLDG.DataBind();
                }
            }
        }

        protected void gvManSchdl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    
                    CheckBox chkPaid = (CheckBox)e.Row.FindControl("chkPaid");
                    
                    if (e.Row.Cells[12].Text == "Y")
                    {
                        chkPaid.Checked = true;
                        
                    }
                    else
                    {
                        chkPaid.Checked = false;
                        
                    }

                    
                }
            }
            finally
            {
                
            }
        }

        protected void chkPaid_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            try
            {
                dt = (DataTable)ViewState["Manual"];
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                TextBox txtDueDt = (TextBox)row.FindControl("txtDueDate");
                CheckBox chkPaid = (CheckBox)row.FindControl("chkPaid");
                if (chkPaid.Checked == true)
                {
                    dt.Rows[row.RowIndex]["IsPaid"] = "Y";
                }
                else
                {
                    dt.Rows[row.RowIndex]["IsPaid"] = "N";
                }
                dt.Rows[row.RowIndex]["DueDt"] = txtDueDt.Text;
                dt.AcceptChanges();
                ViewState["Manual"] = dt;
            }
            finally
            {
                dt = null;
            }
        }
        private void PopPool()
        {
            DataTable dt = null;
            CGblIdGenerator oGbl = null;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oGbl = new CGblIdGenerator();
                dt = oGbl.PopComboMIS("N", "N", "AA", "PoolId", "PoolName", "PoolMst", "0", "AA", "AA", System.DateTime.Now, "0000");
                ddlPool.DataSource = dt;
                ddlPool.DataTextField = "PoolName";
                ddlPool.DataValueField = "PoolId";
                ddlPool.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlPool.Items.Insert(0, oLi);
            }
            finally
            {
                dt = null;
                oGbl = null;
            }
        }
        protected void gvFLDG_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow Row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            int vRow = Row.RowIndex;
            if (e.CommandName == "cmdNewRec")
            {
                AddNewRow();
                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), “FocusOnState”, “document.getElementById(‘” + ddlState.ClientID + “‘).focus(); “, true);
                //ScriptManager manager = ScriptManager.GetCurrent(this);
                //manager.SetFocus(“ddlState”);
            }
           
        }
    }
}