using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class LoanReStructure : CENTRUMBAse
    {
        protected int cPgNo = 1;
        protected int vFlag = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtReSchDt.Text = (string)Session[gblValue.LoginDate];
                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                // LoadReSecheduleList();
                popCustomer();
                ViewState["StateEdit"] = null;
                StatusButton("View");
                tabLnSechedule.ActiveTabIndex = 0;
                // btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            }

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["CheckRefresh"] = Session["CheckRefresh"];
        }
        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Loan Restructure";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLnReStructure);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    //btnEdit.Visible = false;
                    //btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnEdit.Visible = false;
                    //btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //  btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Rescheduling", false);
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
                tabLnSechedule.ActiveTabIndex = 0;
                StatusButton("Add");
                ClearControls();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnGenSchedule_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request[txtNewLoanDate.UniqueID] as string == "")
                {
                    gblFuction.AjxMsgPopup("New Loan Date Can Not Be Blank");
                    return;
                }
                if (Request[txtLastRecDate.UniqueID] as string == "")
                {
                    gblFuction.AjxMsgPopup("Last Received Date of Prevoius Loan Can Not Be Blank");
                    return;
                }
                Int32 vCollDay = 0, vCollDayNo = 0, vCollType = 0;
                DataTable dt = null;
                DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
                DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
                DateTime vLastRecDate = gblFuction.setDate(Request[txtLastRecDate.UniqueID] as string);
                DateTime vNewLoanDt = gblFuction.setDate(Request[txtNewLoanDate.UniqueID] as string);

                if (vNewLoanDt < vLastRecDate)
                {
                    gblFuction.AjxMsgPopup("Loan Date Can Not Be Less Than Last Receive Date..");
                    return;
                }
                if (gblFuction.setDate(txtNewLoanDate.Text) < vFinFromDt || gblFuction.setDate(txtNewLoanDate.Text) > vFinToDt)
                {
                    gblFuction.AjxMsgPopup("New Loan Date should be within Logged In financial year.");
                    return;
                }
                if (Request[txtPOS.UniqueID] as string == "")
                {
                    gblFuction.AjxMsgPopup("Prevoius Outstanding Be Blank");
                    return;
                }
                if (Request[ddlLoanNo.UniqueID] as string == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Loan No...");
                    return;
                }
                if (Request[txtNewLoanAmt.UniqueID] as string == "")
                {
                    gblFuction.AjxMsgPopup("New Loan Amount Can Not Be Blank");
                    return;
                }
                if (Request[txtRIntRate.UniqueID] as string == "")
                {
                    gblFuction.AjxMsgPopup("New Reducing Interest Rate Can Not Be Blank");
                    return;
                }
                if (Request[txtNewTotInstNo.UniqueID] as string == "")
                {
                    gblFuction.AjxMsgPopup("New Total Installment No Can Not Be Blank");
                    return;
                }
                if (Request[txtNewLoanDate.UniqueID] as string == "")
                {
                    gblFuction.AjxMsgPopup("New Loan Date Can Not Be Blank");
                    return;
                }
                if (Request[txtRepayStDate.UniqueID] as string == "")
                {
                    gblFuction.AjxMsgPopup("New Repay Start Date Can Not Be Blank");
                    return;
                }
                //txtFlatIntRate
                Int32 vLoanTypeID = Convert.ToInt32((Request[hfLoanTypeId.UniqueID] as string == null) ? hfLoanTypeId.Value : Request[hfLoanTypeId.UniqueID] as string);
                decimal vPOSAmt = Convert.ToDecimal(Request[txtPOS.UniqueID] as string);
                decimal vNewLoanAmt = Convert.ToDecimal(Request[txtNewLoanAmt.UniqueID] as string);
                decimal vInstRate = Convert.ToDecimal(Request[txtRIntRate.UniqueID] as string);
                decimal vFInstRate = Convert.ToDecimal(Request[txtFIntRate.UniqueID] as string);
                Int32 vInstallNo = Convert.ToInt32(Request[txtNewTotInstNo.UniqueID] as string);
                Int32 vInstPeriod = Convert.ToInt32(Request[txtNewTotInstNo.UniqueID] as string);
                DateTime vStartDt = gblFuction.setDate(Request[txtRepayStDate.UniqueID] as string);
                DateTime vLoanDt = gblFuction.setDate(Request[txtNewLoanDate.UniqueID] as string);
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                string vPaySchedule = (Request[hfPaySchedule.UniqueID] as string == null) ? hfPaySchedule.Value : Request[hfPaySchedule.UniqueID] as string;
                string vSanctionId = (Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string;
                Int32 vFrDueday = Convert.ToInt32((gblFuction.setDate(Request[txtRepayStDate.UniqueID] as string) - gblFuction.setDate(Request[txtNewLoanDate.UniqueID] as string)).TotalDays);
                if (vPOSAmt > vNewLoanAmt)
                {
                    gblFuction.AjxMsgPopup("New Loan Amount can not be less than Prevoius Loan Outstanding Amount");
                    return;
                }
                GetScheduleR(vLoanTypeID, vNewLoanAmt, vFInstRate, vInstRate, vInstallNo, vInstPeriod, vStartDt, "L", "", "N", vBrCode, vPaySchedule, "", "", vCollDay, vCollDayNo, "G", vFrDueday, "", vCollType, vLoanDt, vSanctionId);
                tabLnSechedule.ActiveTabIndex = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabLnSechedule.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(ViewState["IsMig"]) == "Y")
                {
                    gblFuction.MsgPopup("Migrated Loan Cannot be Modified");
                    return;
                }
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                //if (SaveRecords("Delete") == true)
                //{
                //    StatusButton("Delete");
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                StatusButton("View");
                ViewState["StateEdit"] = null;
                ClearControls();
            }
            else
            {

            }

        }
        private Boolean SaveRecords(string Mode)
        {
            DateTime pDishbDate = gblFuction.setDate((Request[txtNewLoanDate.UniqueID] as string == null) ? txtNewLoanDate.Text : Request[txtNewLoanDate.UniqueID] as string);
            DateTime pStDate = gblFuction.setDate((Request[txtRepayStDate.UniqueID] as string == null) ? txtRepayStDate.Text : Request[txtRepayStDate.UniqueID] as string);
            Boolean vResult = false;
            DataTable dt = null;
            CDisburse ODisb = new CDisburse();
            string vCustId = "", vCustName = "", vPreLoanId = "", vInstType = "", pSchedule = "", vDishbMode = "J";
            string vXmlPreAC = string.Empty, vXmlAc = string.Empty;
            string vNarationL = string.Empty; string vNarationR = string.Empty;
            Decimal vPOSAmt = 0, vLnAmt = 0, vNewEMIAmt = 0, vPreEMIAmt = 0, vFIntRate = 0, vRIntRate = 0, vInterestAmt = 0;
            Int32 vCollDay = 0, vCollDayNo = 0, vCollType = 0, vErr = 0, vCycle = 1;
            CCollectionRoutine oCR = null;
            Int32 vFunderID = 0, vLoanTypeID = 0, pDisbSrl = 0, vInstallSize = 0, vInstalNo = 0;
            Decimal TotLnAmt = 0, InstPeriod = 0, LPFStax = 0, LPFKKTax = 0, LPFSBTax = 0, InsuStax = 0, InsuKKtax = 0, InsuSBTax = 0, AppCharge = 0, StampCharge = 0, NetDisb = 0,
              EMI = 0, AdvEMI = 0, AdvEMIPric = 0, AdvEMIInt = 0, ProceFees = 0, InsuAmt = 0, IntRate = 0, FIntRate = 0, AdvInt = 0, PreLnBal = 0,
              CGSTAmt = 0, SGSTAmt = 0, FLDGAmt = 0, vPreLnInt = 0, vIntDue = 0, vIntWaive = 0, vGrantTotal = 0, vBounceRecv = 0, vBounceWaive = 0,
              vPreBounceDue = 0, vPreLnOS = 0;

            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vTblDtl = Session[gblValue.ACVouDtl].ToString();
            string vFinYear = Session[gblValue.ShortYear].ToString();

            if (Request[txtLastRecDate.ClientID] as string == "")
            {
                gblFuction.AjxMsgPopup("Last Received Date Can Not Be Blank");
                return false;
            }
            if (Request[txtNewLoanDate.ClientID] as string == "")
            {
                gblFuction.AjxMsgPopup("New Loan Date Can Not Be Blank");
                return false;
            }
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            DateTime vLastRecDate = gblFuction.setDate(Request[txtLastRecDate.UniqueID] as string);
            DateTime vNewLoanDt = gblFuction.setDate(Request[txtNewLoanDate.UniqueID] as string);

            if (vNewLoanDt < vLastRecDate)
            {
                gblFuction.AjxMsgPopup("Loan Date Can Not Be Less Than Last Receive Date..");
                return false;
            }
            if (gblFuction.setDate(txtNewLoanDate.Text) < vFinFromDt || gblFuction.setDate(txtNewLoanDate.Text) > vFinToDt)
            {
                gblFuction.AjxMsgPopup("New Loan Date should be within Logged In financial year.");
                return false;
            }
            if (gblFuction.setDate(txtNewLoanDate.Text) < vFinFromDt || gblFuction.setDate(txtNewLoanDate.Text) > vFinToDt)
            {
                gblFuction.AjxMsgPopup("New Loan Date should be within Logged In financial year.");
                return false;
            }

            if (Request[txtPOS.UniqueID] as string == "")
            {
                gblFuction.AjxMsgPopup("Prevoius Outstanding Be Blank");
                return false;
            }
            if (Request[ddlLoanNo.UniqueID] as string == "-1")
            {
                gblFuction.AjxMsgPopup("Please Loan No...");
                return false;
            }
            if (Request[txtNewLoanAmt.UniqueID] as string == "")
            {
                gblFuction.AjxMsgPopup("New Loan Amount Can Not Be Blank");
                return false;
            }
            if (Request[txtRIntRate.UniqueID] as string == "")
            {
                gblFuction.AjxMsgPopup("New Reducing Interest Rate Can Not Be Blank");
                return false;
            }
            if (Request[txtNewTotInstNo.UniqueID] as string == "")
            {
                gblFuction.AjxMsgPopup("New Total Installment No Can Not Be Blank");
                return false;
            }
            if (Request[txtNewLoanDate.UniqueID] as string == "")
            {
                gblFuction.AjxMsgPopup("New Loan Date Can Not Be Blank");
                return false;
            }
            if (Request[txtRepayStDate.UniqueID] as string == "")
            {
                gblFuction.AjxMsgPopup("New Repay Start Date Can Not Be Blank");
                return false;
            }
            //vPreLoanId
            vCustId = (Request[ddlCust.UniqueID] as string == null) ? ddlCust.SelectedValue : Request[ddlCust.UniqueID] as string;
            vPreLoanId = (Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string;
            pSchedule = Convert.ToString((Request[hfPaySchedule.UniqueID] as string == null) ? hfPaySchedule.Value : Request[hfPaySchedule.UniqueID] as string);
            vLoanTypeID = Convert.ToInt32((Request[hfLoanTypeId.UniqueID] as string == null) ? hfLoanTypeId.Value : Request[hfLoanTypeId.UniqueID] as string);
            vInstType = "R";
            if (Convert.ToString((Request[txtPOS.UniqueID] as string == null) ? txtPOS.Text : Request[txtPOS.UniqueID] as string) != "")
            {
                vPOSAmt = Convert.ToDecimal(Request[txtPOS.UniqueID] as string);
                vPreLnOS = Convert.ToDecimal(Request[txtPOS.UniqueID] as string);
            }
            if (Convert.ToString((Request[txtPreBounceDue.UniqueID] as string == null) ? txtPreBounceDue.Text : Request[txtPreBounceDue.UniqueID] as string) != "")
                vPreBounceDue = Convert.ToDecimal(Request[txtPreBounceDue.UniqueID] as string);
            if (Convert.ToString((Request[txtBounceRec.UniqueID] as string == null) ? txtBounceRec.Text : Request[txtBounceRec.UniqueID] as string) != "")
                vBounceRecv = Convert.ToDecimal(Request[txtBounceRec.UniqueID] as string);
            if (Convert.ToString((Request[txtBounceWaive.UniqueID] as string == null) ? txtBounceWaive.Text : Request[txtBounceWaive.UniqueID] as string) != "")
                vBounceWaive = Convert.ToDecimal(Request[txtBounceWaive.UniqueID] as string);
            if (Convert.ToString((Request[txtPreIntDue.UniqueID] as string == null) ? txtPreIntDue.Text : Request[txtPreIntDue.UniqueID] as string) != "")
                vPreLnInt = Convert.ToDecimal(Request[txtPreIntDue.UniqueID] as string);
            if (Convert.ToString((Request[txtNewLoanAmt.UniqueID] as string == null) ? txtNewLoanAmt.Text : Request[txtNewLoanAmt.UniqueID] as string) != "")
            {
                TotLnAmt = Convert.ToDecimal(Request[txtNewLoanAmt.UniqueID] as string);
                NetDisb = Convert.ToDecimal(Request[txtNewLoanAmt.UniqueID] as string);
            }
            if (Convert.ToString((Request[txtRIntRate.UniqueID] as string == null) ? txtRIntRate.Text : Request[txtRIntRate.UniqueID] as string) != "")
                IntRate = Convert.ToDecimal(Request[txtRIntRate.UniqueID] as string);
            if (Convert.ToString((Request[txtFIntRate.UniqueID] as string == null) ? txtFIntRate.Text : Request[txtFIntRate.UniqueID] as string) != "")
                FIntRate = Convert.ToDecimal(Request[txtFIntRate.UniqueID] as string);

            if (Convert.ToString((Request[txtNewTotInstNo.UniqueID] as string == null) ? txtNewTotInstNo.Text : Request[txtNewTotInstNo.UniqueID] as string) != "")
            {
                vInstalNo = Convert.ToInt32(Request[txtNewTotInstNo.UniqueID] as string);
                vInstallSize = Convert.ToInt32(Request[txtNewTotInstNo.UniqueID] as string);
                InstPeriod = Convert.ToInt32(Request[txtNewTotInstNo.UniqueID] as string);
            }

            if (Convert.ToString((Request[txtNewEMIAmt.UniqueID] as string == null) ? txtNewEMIAmt.Text : Request[txtNewEMIAmt.UniqueID] as string) != "")
                EMI = Convert.ToDecimal(Request[txtNewEMIAmt.UniqueID] as string);
            //  DateTime vStartDt = gblFuction.setDate(Request[txtRepayStDate.UniqueID] as string);
            if (txtNewLoanDate.Text == "")
            {
                gblFuction.AjxMsgPopup("New Loan Date Can Not Be Empty");
                return false;
            }
            if (IntRate == 0)
            {
                gblFuction.AjxMsgPopup("Reducing Interest Rate Can Not Be Zero");
                return false;
            }
            if (NetDisb == 0)
            {
                gblFuction.AjxMsgPopup("New Loan Amount Can Not Be Zero");
                return false;
            }
            DateTime vLoanDt = gblFuction.setDate(Request[txtNewLoanDate.UniqueID] as string);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vPaySchedule = (Request[hfPaySchedule.UniqueID] as string == null) ? hfPaySchedule.Value : Request[hfPaySchedule.UniqueID] as string;
            string vSanctionId = (Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string;
            Int32 vFrDueday = Convert.ToInt32((gblFuction.setDate(Request[txtRepayStDate.UniqueID] as string) - gblFuction.setDate(Request[txtLnDt.UniqueID] as string)).TotalDays);
            if (vPOSAmt > TotLnAmt)
            {
                gblFuction.AjxMsgPopup("New Loan Amount can not be less than Prevoius Loan Outstanding Amount");
                return false;
            }

            vIntDue = (TotLnAmt - vPOSAmt - vBounceRecv);
            if (vIntDue > 0)
            {
                if (vPreLnInt > vIntDue)
                {
                    vIntDue = Math.Round((TotLnAmt - vPOSAmt - vBounceRecv), 0);
                    vIntWaive = Math.Round((vPreLnInt - vIntDue), 0);
                }
                else
                {
                    vIntDue = Math.Round((TotLnAmt - vPOSAmt - vBounceRecv), 0);
                    vIntWaive = 0;
                }
            }

            vGrantTotal = Math.Round((vPOSAmt + vIntDue + vBounceRecv - vIntWaive), 0);
            string vLoanAc = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.PrincipalLoanAc, vLoanTypeID, vBrCode);
            string vIntAc = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.LoanIntAc, vLoanTypeID, vBrCode);
            string vIntWaiveAc = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.PrincipalLoanAc, vLoanTypeID, vBrCode);
            string vBounceChrgeAc = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.BounceChrgAC, vLoanTypeID, vBrCode);

            // For Testing purpose we set Interest Ac as Restructure Ac.  
            string vRestructureAC = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.ReStructureAC, vLoanTypeID, vBrCode);


            //--- Account Posting For Previous Loan 

            DataTable dtPreAc = new DataTable();
            DataRow dr;
            DataColumn dc = new DataColumn();
            dc.ColumnName = "DtlId";
            dtPreAc.Columns.Add(dc);
            DataColumn dc1 = new DataColumn();
            dc1.ColumnName = "DescId";
            dtPreAc.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn();
            dc2.ColumnName = "DC";
            dtPreAc.Columns.Add(dc2);
            DataColumn dc3 = new DataColumn();
            dc3.ColumnName = "Amt";
            dc3.DataType = System.Type.GetType("System.Decimal");
            dtPreAc.Columns.Add(dc3);
            dtPreAc.TableName = "Table1";

            if (vGrantTotal > 0)
            {
                int i = 1;
                dr = dtPreAc.NewRow();
                dr["DescId"] = vRestructureAC;
                dr["DC"] = "D";
                dr["Amt"] = vGrantTotal;
                dr["DtlId"] = i;
                dtPreAc.Rows.Add(dr);
                dtPreAc.AcceptChanges();


                i = i + 1;
                dr = dtPreAc.NewRow();
                dr["DescId"] = vLoanAc;
                dr["DC"] = "C";
                dr["Amt"] = Convert.ToDecimal((Request[txtPOS.UniqueID] as string == null) ? txtPOS.Text : Request[txtPOS.UniqueID] as string);
                dr["DtlId"] = i;
                dtPreAc.Rows.Add(dr);
                dtPreAc.AcceptChanges();

                if (vIntDue > 0)
                {
                    i = i + 1;
                    dr = dtPreAc.NewRow();
                    dr["DescId"] = vIntAc;
                    dr["DC"] = "C";
                    dr["Amt"] = vIntDue;
                    dr["DtlId"] = i;
                    dtPreAc.Rows.Add(dr);
                    dtPreAc.AcceptChanges();
                }

                if (vIntWaive > 0)
                {
                    i = i + 1;
                    dr = dtPreAc.NewRow();
                    dr["DescId"] = vIntWaiveAc;
                    dr["DC"] = "D";
                    dr["Amt"] = vIntWaive;
                    dr["DtlId"] = i;
                    dtPreAc.Rows.Add(dr);
                    dtPreAc.AcceptChanges();
                }
                if (vBounceRecv > 0)
                {
                    i = i + 1;
                    dr = dtPreAc.NewRow();
                    dr["DescId"] = vBounceChrgeAc;
                    dr["DC"] = "C";
                    dr["Amt"] = vBounceRecv;
                    dr["DtlId"] = i;
                    dtPreAc.Rows.Add(dr);
                    dtPreAc.AcceptChanges();
                }
            }


            //---- Account Posting for Current Loan-----
            DataTable dtAccount = new DataTable();
            DataRow drNew;
            DataColumn dcNew = new DataColumn();
            dcNew.ColumnName = "DtlId";
            dtAccount.Columns.Add(dcNew);
            DataColumn dcNew1 = new DataColumn();
            dcNew1.ColumnName = "DescId";
            dtAccount.Columns.Add(dcNew1);
            DataColumn dcNew2 = new DataColumn();
            dcNew2.ColumnName = "DC";
            dtAccount.Columns.Add(dcNew2);
            DataColumn dcNew3 = new DataColumn();
            dcNew3.ColumnName = "Amt";
            dcNew3.DataType = System.Type.GetType("System.Decimal");
            dtAccount.Columns.Add(dcNew3);
            dtAccount.TableName = "Table1";

            if (Convert.ToDecimal((Request[txtNewLoanAmt.UniqueID] as string == null) ? txtNewLoanAmt.Text : Request[txtNewLoanAmt.UniqueID] as string) > 0)
            {
                int i = 1;
                drNew = dtAccount.NewRow();
                drNew["DescId"] = vRestructureAC;
                drNew["DC"] = "C";
                drNew["Amt"] = Convert.ToDecimal((Request[txtNewLoanAmt.UniqueID] as string == null) ? txtNewLoanAmt.Text : Request[txtNewLoanAmt.UniqueID] as string);
                drNew["DtlId"] = i;
                dtAccount.Rows.Add(drNew);
                dtAccount.AcceptChanges();

                i = i + 1;
                drNew = dtAccount.NewRow();
                drNew["DescId"] = vLoanAc;
                drNew["DC"] = "D";
                drNew["Amt"] = Convert.ToDecimal((Request[txtNewLoanAmt.UniqueID] as string == null) ? txtNewLoanAmt.Text : Request[txtNewLoanAmt.UniqueID] as string);
                drNew["DtlId"] = i;
                dtAccount.Rows.Add(drNew);
                dtAccount.AcceptChanges();
            }
            vCustName = ddlCust.SelectedItem.Text.ToString().Trim();
            vNarationL = "Being the Amt of Loan Disbursed for  " + vCustName;
            vNarationR = "Being the Amt Loan Adjustment for" + vCustName;
            vXmlPreAC = DataTableTOXml(dtPreAc);
            vXmlAc = DataTableTOXml(dtAccount);
            CDisburse oLD = new CDisburse();

            if (Mode == "Save")
            {
                vErr = oLD.SaveLoanRestructure(vPreLoanId, vCustId, vLoanTypeID, pDisbSrl, pDishbDate, TotLnAmt, vInstType, pSchedule,
                   IntRate, FIntRate, vInstalNo, InstPeriod, EMI, vInstallSize, pStDate, vDishbMode, vCycle, vFunderID, ProceFees, LPFStax, LPFKKTax, LPFSBTax,
                  InsuAmt, InsuStax, InsuKKtax, InsuSBTax, AppCharge, AdvEMIPric, AdvEMIInt, StampCharge, AdvInt, NetDisb, vLoanAc, "J", "",
                  gblFuction.setDate(txtNewLoanDate.Text), vBrCode, vXmlPreAC, vXmlAc, vTblMst, vTblDtl, vFinYear, this.UserID, vNarationL, vNarationR, vCollDay,
                  vCollDayNo, "", vCollType, PreLnBal, "", CGSTAmt, SGSTAmt, FLDGAmt, vPreLnOS, vBrCode, vRestructureAC, vIntDue, vIntWaive, vBounceRecv, vBounceWaive);

                if (vErr == 0)
                {
                    gblFuction.MsgPopup("Loan Restructure done successfully.");
                    vResult = true;
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                    vResult = false;
                }
            }
            //LoadReSecheduleList();
            tabLnSechedule.ActiveTabIndex = 0;
            return vResult;
        }
        private string DataTableTOXml(DataTable dt)
        {
            string sXml = "";
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                sXml = oSW.ToString();
            }
            return sXml;
        }
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnAdd.Enabled = false;
                    // btnEdit.Enabled = false;
                    // btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    // btnEdit.Enabled = true;
                    // btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    // btnEdit.Enabled = false;
                    //  btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);

                    break;
                case "View":
                    btnAdd.Enabled = true;
                    //  btnEdit.Enabled = false;
                    //  btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    //   btnEdit.Enabled = false;
                    //   btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        private void EnableControl(Boolean Status)
        {

            /*******************/
            txtReSchDt.Enabled = Status;
            ddlCust.Enabled = Status;
            ddlLoanNo.Enabled = Status;
            txtPOS.Enabled = Status;
            txtPreIntDue.Enabled = Status;
            txtLnDt.Enabled = Status;
            txtLastRecDate.Enabled = Status;
            txtLnAmt.Enabled = Status;
            txtNewLoanDate.Enabled = Status;
            txtFIntRate.Enabled = Status;
            txtRIntRate.Enabled = Status;
            txtNewLoanAmt.Enabled = Status;
            txtNewTotInstNo.Enabled = Status;
            txtRepayStDate.Enabled = Status;

            txtNewEMIAmt.Enabled = Status;
            txtRemarks.Enabled = Status;
            txtPreBounceDue.Enabled = Status;
            txtBounceRec.Enabled = Status;
            txtBounceWaive.Enabled = Status;
            /*************************/
        }
        private void ClearControls()
        {
            ddlCust.SelectedValue = "-1";
            ddlLoanNo.SelectedValue = "-1";
            hfLoanTypeId.Value = "";
            hfRepayStartDt.Value = "";
            hfPaySchedule.Value = "";
            txtTotInstNo.Text = "";
            txtPOS.Text = "";
            txtPreIntDue.Text = "";
            txtLnDt.Text = "";
            txtFIntRate.Text = "";
            txtRIntRate.Text = "";
            txtNewEMIAmt.Text = "";
            txtNewLoanAmt.Text = "";
            txtNewLoanDate.Text = "";
            txtNewTotInstNo.Text = "";
            txtLnAmt.Text = "";
            txtRemarks.Text = "";
            txtPreBounceDue.Text = "";
            txtBounceRec.Text = "";
            txtBounceWaive.Text = "";
        }
        //private void LoadReSecheduleList()
        //{
        //    DataTable dt = null;
        //    CDisburse oCD = null;
        //    string vBrCode = Session[gblValue.BrnchCode].ToString();
        //    DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

        //    try
        //    {
        //        oCD = new CDisburse();
        //        dt = oCD.GetReScheduleList();
        //        if (dt.Rows.Count > 0)
        //        {
        //            gvReSchdl.DataSource = dt;
        //            gvReSchdl.DataBind();
        //        }
        //        else
        //        {
        //            gvReSchdl.DataSource = null;
        //            gvReSchdl.DataBind();
        //        }
        //    }
        //    finally
        //    {
        //        oCD = null;
        //        dt = null;
        //    }
        //}
        private void popCustomer()
        {
            DataTable dt = null;
            CDisburse oCD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            try
            {
                oCD = new CDisburse();
                dt = oCD.GetCustNameForReSch("0000");
                if (dt.Rows.Count > 0)
                {
                    ddlCust.Items.Clear();
                    ddlCust.DataSource = dt;
                    ddlCust.DataTextField = "CompanyName";
                    ddlCust.DataValueField = "CustId";
                    ddlCust.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlCust.Items.Insert(0, oli);
                }

            }
            finally
            {
                oCD = null;
                dt = null;
            }
        }
        private void popLnNo(string pCustId)
        {

            DataTable dt = null;
            CDisburse oMem = null;
            try
            {
                oMem = new CDisburse();
                dt = oMem.GetLnNoByCustId(pCustId);
                if (dt.Rows.Count > 0)
                {
                    ddlLoanNo.Items.Clear();
                    ddlLoanNo.DataTextField = "LoanNo";
                    ddlLoanNo.DataValueField = "LoanId";
                    ddlLoanNo.DataSource = dt;
                    ddlLoanNo.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlLoanNo.Items.Insert(0, oli);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }

        }
        private void GetScheduleR(Int32 pLoanTypeID, decimal pLoanAmt, decimal pFInterest, decimal pInterest, Int32 pInstallNo, Int32 pInstPeriod,
                                DateTime pStatrDt, string pType, string pLoanID, string pIsDisburse, string pBranch, string pPaySchedule,
                                string pBank, string pChequeNo, Int32 pCollDay, Int32 pCollDayNo, string pLoanType, Int32 pFrDueday, string pPEType, Int32 vCollType, DateTime vLoanDt, string vSanctionId)
        {
            DataTable dt = null;
            CDisburse oLD = null;
            double VLoanInt = 0.0;
            oLD = new CDisburse();
            dt = oLD.GetScheduleR(pLoanTypeID, pLoanAmt, pFInterest, pInterest, pInstallNo, pInstPeriod, pStatrDt, pType, pLoanID, pIsDisburse, pBranch, pPaySchedule, pBank, pChequeNo, pCollDay, pCollDayNo, pLoanType, pFrDueday, pPEType, vCollType, vLoanDt, vSanctionId);
            if (dt.Rows.Count > 0)
            {
                gvSchdl.DataSource = dt;
                gvSchdl.DataBind();
                VLoanInt = Convert.ToDouble(dt.Compute("Sum(InstAmt)", ""));
                // txtIntAmt.Text = VLoanInt.ToString();
            }
            tabLnSechedule.ActiveTabIndex = 1;
        }

        private void CalculateEMI()
        {
            if (txtNewLoanAmt.Text != "" && txtRIntRate.Text != "" && txtNewTotInstNo.Text != "")
            {
                string vLnTypeId = "", vPaySchedule = "";
                Int32 pLnType = 0;
                decimal pLnAmt = 0, pRIntRate = 0, pInstNo = 0, EMI = 0;
                if (txtNewLoanAmt.Text == "")
                    pLnAmt = 0;
                else
                    pLnAmt = Convert.ToDecimal(txtNewLoanAmt.Text);
                if (txtRIntRate.Text == "")
                    pRIntRate = 0;
                else
                    pRIntRate = Convert.ToDecimal(txtRIntRate.Text);
                if (txtNewTotInstNo.Text == "")
                    pInstNo = 0;
                else
                    pInstNo = Convert.ToDecimal(txtNewTotInstNo.Text);
                vLnTypeId = Convert.ToString((Request[hfLoanTypeId.UniqueID] as string == null) ? hfLoanTypeId.Value : Request[hfLoanTypeId.UniqueID] as string);
                vPaySchedule = Convert.ToString((Request[hfPaySchedule.UniqueID] as string == null) ? hfPaySchedule.Value : Request[hfPaySchedule.UniqueID] as string);
                if (vLnTypeId != "")
                {
                    pLnType = Convert.ToInt32(vLnTypeId);
                }
                DataTable dt = new DataTable();
                CApplication OCA = new CApplication();
                dt = OCA.CalculateEMIRestructure(pLnAmt, pRIntRate, pInstNo, pLnType, vPaySchedule);
                if (dt.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(dt.Rows[0]["CalculatedEMI"].ToString()) == false)
                        EMI = Convert.ToDecimal(dt.Rows[0]["CalculatedEMI"]);
                }
                else
                {
                    EMI = 0;
                }
                txtNewEMIAmt.Text = EMI.ToString();
            }
        }
        protected void txtNewLoanAmt_TextChanged(object sender, EventArgs e)
        {
            CalculateEMI();
        }
        protected void txtRIntRate_TextChanged(object sender, EventArgs e)
        {
            CalculateEMI();
        }
        protected void txtNewTotInstNo_TextChanged(object sender, EventArgs e)
        {
            CalculateEMI();
        }
    }
}
