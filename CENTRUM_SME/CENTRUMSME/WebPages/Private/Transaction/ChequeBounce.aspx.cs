using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class ChequeBounce : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtBounceDt.Text = (string)Session[gblValue.LoginDate];
                ViewState["StateEdit"] = null;
                popCustomer();
                LoadBounceList();
                tabCollBounce.ActiveTabIndex = 0;
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["StateEdit"] = "Add";
                tabCollBounce.ActiveTabIndex = 1;
                StatusButton("Add");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void LoadBounceList()
        {
            CLoanRecovery ORecovery = new CLoanRecovery();
            DataTable dt = new DataTable();
            try
            {
                dt = ORecovery.GetBounceList();
                if (dt.Rows.Count > 0)
                {
                    gvCollBounce.DataSource = dt;
                    gvCollBounce.DataBind();
                }
                else
                {
                    gvCollBounce.DataSource = null;
                    gvCollBounce.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ORecovery = null;
                dt = null;
            }


        }
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    btnAdd.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnSave.Enabled = false;
                    btnExit.Enabled = true;
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    break;
            }
        }
        private void popCustomer()
        {
            DataTable dt = null;
            CLoanRecovery oCD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            try
            {
                oCD = new CLoanRecovery();
                dt = oCD.GetBounceCustName(rdbOpt.SelectedValue.ToString(), vBrCode);
                ddlCustName.DataSource = dt;
                ddlCustName.DataTextField = "CustName";
                ddlCustName.DataValueField = "CustId";
                ddlCustName.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCustName.Items.Insert(0, oli);
            }
            finally
            {
                oCD = null;
                dt = null;
            }
        }
        protected void rdbOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            popCustomer();
        }
        protected void ddlCustName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCustName.SelectedValue == "-1")
            {
                gblFuction.MsgPopup("Select Customer to load Loan No...");
                return;
            }
            string pCustId = ddlCustName.SelectedValue.ToString();
            string pMode = rdbOpt.SelectedValue.ToString();
            DataTable dt = null;
            CLoanRecovery oCD = null;
            try
            {
                oCD = new CLoanRecovery();
                dt = oCD.GetLoanNoForBounce(pCustId, pMode);
                ddlLoanNo.DataSource = dt;
                ddlLoanNo.DataTextField = "LoanNo";
                ddlLoanNo.DataValueField = "LoanId";
                ddlLoanNo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlLoanNo.Items.Insert(0, oli);
            }
            finally
            {
                oCD = null;
                dt = null;
            }
        }
        protected void ddlLoanNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLoanNo.SelectedValue == "-1")
            {
                gblFuction.MsgPopup("Select Loan No to load Serial No...");
                return;
            }
            string pLoanId = ddlLoanNo.SelectedValue.ToString();
            if (txtBounceDt.Text == "")
            {
                gblFuction.MsgPopup("Bounce Date Can Not Be Blank...");
                return;
            }
            DateTime pBounceDt = gblFuction.setDate(txtBounceDt.Text);
            string pLoanNo = ddlLoanNo.SelectedValue.ToString();

            DataTable dt = null;
            CLoanRecovery oCD = null;
            try
            {
                ddlSLNo.Items.Clear();
                oCD = new CLoanRecovery();
                dt = oCD.GetSLNoForBounce(pLoanNo, pBounceDt);
                ddlSLNo.DataSource = dt;
                ddlSLNo.DataTextField = "SlNo";
                ddlSLNo.DataValueField = "SlNo";
                ddlSLNo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlSLNo.Items.Insert(0, oli);
            }
            finally
            {
                oCD = null;
                dt = null;
            }
        }
        private void InitBasePage()
        {

            try
            {
                this.Menu = false;
                this.PageHeading = "Collection Bounce";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuChequeBounce);
                if (this.UserID == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;

                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;

                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;

                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnSave.Visible = true;

                    return;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Collection Bounce", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }

        }
        private void ClearControls()
        {
            txtBounceDt.Text = (string)Session[gblValue.LoginDate];
            txtPrinCollAmt.Text = "";
            txtIntCollAmt.Text = "";
            txtPenCollAmt.Text = "";
            txtWaveIntAmt.Text = "";
            txtTotCollAmt.Text = "";
            txtCollMode.Text = "";
            ddlCustName.SelectedIndex = -1;
            ddlLoanNo.SelectedIndex = -1;
            ddlSLNo.SelectedIndex = -1;
            txtBounceRec.Text = "";
            txtBounceWaive.Text = "";
            txtBounceAmt.Text = "";
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabCollBounce.ActiveTabIndex = 0;
            StatusButton("View");
        }
        private void GetCollDtl(string LoanNo, DateTime pBounceDate,string pSlNo)
        {
            CLoanRecovery ORecovery = new CLoanRecovery();
            DataTable dt = new DataTable();
            try
            {
                dt = ORecovery.GetLastCollDtlByLoanNo(LoanNo, pBounceDate, Convert.ToInt32(pSlNo));
                if (dt.Rows.Count > 0)
                {
                    //hdAccDate.Value = dt.Rows[0]["AccDate"].ToString();
                    hdOutStand.Value = dt.Rows[0]["Outstanding"].ToString();
                    txtPrinCollAmt.Text = dt.Rows[0]["PrinCollAmt"].ToString();
                    txtIntCollAmt.Text = dt.Rows[0]["IntCollAmt"].ToString();
                    txtPenCollAmt.Text = dt.Rows[0]["PenCollAmt"].ToString();
                    txtWaveIntAmt.Text = dt.Rows[0]["IntWaveAmt"].ToString();
                    txtTotCollAmt.Text = dt.Rows[0]["CollAmt"].ToString();
                    hdBalAmt.Value = dt.Rows[0]["BalanceAmt"].ToString();
                    txtCollMode.Text = dt.Rows[0]["CollectionMode"].ToString();
                    txtChqRefNo.Text = dt.Rows[0]["ChequeReffNo"].ToString();
                    hdCollMode.Value = dt.Rows[0]["CollMode"].ToString();
                    hdTransMode.Value = dt.Rows[0]["TransMode"].ToString();
                    hdDescId.Value = dt.Rows[0]["DescId"].ToString();
                    hdBankName.Value = dt.Rows[0]["BankName"].ToString();

                }
                else
                {
                    txtPrinCollAmt.Text = "0";
                    txtIntCollAmt.Text = "0";
                    txtPenCollAmt.Text = "0";
                    txtWaveIntAmt.Text = "0";
                    txtTotCollAmt.Text = "0";
                    txtCollMode.Text = "";
                    txtPrinCollAmt.Text = "0";
                    txtChqRefNo.Text = "";
                    hdCollMode.Value = "";
                    hdTransMode.Value = "";
                    hdDescId.Value = "";
                    hdBankName.Value = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ORecovery = null;
                dt = null;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            CLoanRecovery oLR = null;
            string vCollMode = "", vTransMode = "", vBankName = "", vChqRefNo = "", vSancId = "", vLoanId = "", vCustId = "",vLoanStatus="";
            Int32 vErr = 0, vSlNo = 0,vODDays=0;
            Decimal vPrinColl = 0, vIntColl = 0,  vWaveInt = 0, vTotColl = 0, vPrinOS = 0, vBounceAmt = 0, 
                vBalAmt = 0, vBounceRec = 0,vBounceWaived = 0,vIntDue=0,
                vPenCollAmt = 0, vPenWaiveAmt = 0, vPenCGST = 0, vPenSGST = 0,vPenDue=0,
                vVisitChargeCollAmt = 0, vVisitChargeWaiveAmt = 0, vVisitChargeCGST = 0, vVisitChargeSGST = 0, vVisitChargeDue = 0;
            string vBankLedgrAC = "";
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vXmlAC = string.Empty;
            string vNarationL = string.Empty;
            string vNarationF = string.Empty;

            try
            {

                if (txtBounceDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Please Select Bounce Date...");
                    return;
                }
                if (ddlSLNo.SelectedValue=="-1")
                {
                    gblFuction.AjxMsgPopup("Please Select SL No...");
                    return;
                }
                if (((Request[txtBounceAmt.UniqueID] as string == null) ? txtBounceAmt.Text : Request[txtBounceAmt.UniqueID] as string) == "")
                {
                    gblFuction.AjxMsgPopup("Please input Bounce Charge Amount.....");
                    return;
                }
                DateTime vAccDate = gblFuction.setDate(txtBounceDt.Text);
                string vBranch = Session[gblValue.BrnchCode].ToString();
                string vActMstTbl = Session[gblValue.ACVouMst].ToString();
                string vActDtlTbl = Session[gblValue.ACVouDtl].ToString();
                string vFinYear = Session[gblValue.ShortYear].ToString();
                vLoanStatus = rdbOpt.SelectedValue.ToString();

                // Financial Year Checking...
                if (vAccDate < gblFuction.setDate(Session[gblValue.FinFromDt].ToString()) || vAccDate > gblFuction.setDate(Session[gblValue.FinToDt].ToString()))
                {
                    gblFuction.AjxMsgPopup("Bounce Date Should Be in Login Financial Year...");
                    return;
                }
                vCustId = (Request[ddlCustName.UniqueID] as string == null) ? ddlCustName.SelectedValue : Request[ddlCustName.UniqueID] as string;
                vLoanId = (Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string;
                if (Convert.ToString((Request[txtPrinCollAmt.UniqueID] as string == null) ? txtPrinCollAmt.Text : Request[txtPrinCollAmt.UniqueID]) != "")
                    vPrinColl = Convert.ToDecimal((Request[txtPrinCollAmt.UniqueID] as string == null) ? txtPrinCollAmt.Text : Request[txtPrinCollAmt.UniqueID] as string);
                if (Convert.ToString((Request[txtIntCollAmt.UniqueID] as string == null) ? txtIntCollAmt.Text : Request[txtIntCollAmt.UniqueID]) != "")
                    vIntColl = Convert.ToDecimal((Request[txtIntCollAmt.UniqueID] as string == null) ? txtIntCollAmt.Text : Request[txtIntCollAmt.UniqueID] as string);
                if (Convert.ToString((Request[txtBounceRec.UniqueID] as string == null) ? txtBounceRec.Text : Request[txtBounceRec.UniqueID]) != "")
                    vBounceRec = Convert.ToDecimal((Request[txtBounceRec.UniqueID] as string == null) ? txtBounceRec.Text : Request[txtBounceRec.UniqueID] as string);
                if (Convert.ToString((Request[txtBounceWaive.UniqueID] as string == null) ? txtBounceWaive.Text : Request[txtBounceWaive.UniqueID]) != "")
                    vBounceWaived = Convert.ToDecimal((Request[txtBounceWaive.UniqueID] as string == null) ? txtBounceWaive.Text : Request[txtBounceWaive.UniqueID] as string);
                
                // For Delay Payment Charge
                if (Convert.ToString((Request[txtPenCollAmt.UniqueID] as string == null) ? txtPenCollAmt.Text : Request[txtPenCollAmt.UniqueID]) != "")
                    vPenCollAmt = Convert.ToDecimal((Request[txtPenCollAmt.UniqueID] as string == null) ? txtPenCollAmt.Text : Request[txtPenCollAmt.UniqueID] as string);
                if (Convert.ToString((Request[txtPenWaiveAmt.UniqueID] as string == null) ? txtPenWaiveAmt.Text : Request[txtPenWaiveAmt.UniqueID]) != "")
                    vPenWaiveAmt = Convert.ToDecimal((Request[txtPenWaiveAmt.UniqueID] as string == null) ? txtPenWaiveAmt.Text : Request[txtPenWaiveAmt.UniqueID] as string);
                if (Convert.ToString((Request[txtPenCGST.UniqueID] as string == null) ? txtPenCGST.Text : Request[txtPenCGST.UniqueID]) != "")
                    vPenCGST = Convert.ToDecimal((Request[txtPenCGST.UniqueID] as string == null) ? txtPenCGST.Text : Request[txtPenCGST.UniqueID] as string);
                if (Convert.ToString((Request[txtPenSGST.UniqueID] as string == null) ? txtPenSGST.Text : Request[txtPenSGST.UniqueID]) != "")
                    vPenSGST = Convert.ToDecimal((Request[txtPenSGST.UniqueID] as string == null) ? txtPenSGST.Text : Request[txtPenSGST.UniqueID] as string);
                if (hdPenDue.Value != "")
                    vPenDue = Convert.ToDecimal((Request[hdPenDue.UniqueID] as string == null) ? hdPenDue.Value : Request[hdPenDue.UniqueID] as string);

                // For Visit Charge
                if (Convert.ToString((Request[txtVisitChargeRec.UniqueID] as string == null) ? txtVisitChargeRec.Text : Request[txtVisitChargeRec.UniqueID]) != "")
                    vVisitChargeCollAmt = Convert.ToDecimal((Request[txtVisitChargeRec.UniqueID] as string == null) ? txtVisitChargeRec.Text : Request[txtVisitChargeRec.UniqueID] as string);
                if (Convert.ToString((Request[txtVisitChargeWaived.UniqueID] as string == null) ? txtVisitChargeWaived.Text : Request[txtVisitChargeWaived.UniqueID]) != "")
                    vVisitChargeWaiveAmt = Convert.ToDecimal((Request[txtVisitChargeWaived.UniqueID] as string == null) ? txtVisitChargeWaived.Text : Request[txtVisitChargeWaived.UniqueID] as string);
                if (Convert.ToString((Request[txtVisitChargeCGST.UniqueID] as string == null) ? txtVisitChargeCGST.Text : Request[txtVisitChargeCGST.UniqueID]) != "")
                    vVisitChargeCGST = Convert.ToDecimal((Request[txtVisitChargeCGST.UniqueID] as string == null) ? txtVisitChargeCGST.Text : Request[txtVisitChargeCGST.UniqueID] as string);
                if (Convert.ToString((Request[txtVisitChargeSGST.UniqueID] as string == null) ? txtVisitChargeSGST.Text : Request[txtVisitChargeSGST.UniqueID]) != "")
                    vVisitChargeSGST = Convert.ToDecimal((Request[txtVisitChargeSGST.UniqueID] as string == null) ? txtVisitChargeSGST.Text : Request[txtVisitChargeSGST.UniqueID] as string);
                if (hdVisitDue.Value != "")
                    vVisitChargeDue = Convert.ToDecimal((Request[hdVisitDue.UniqueID] as string == null) ? hdVisitDue.Value : Request[hdVisitDue.UniqueID] as string);
             
                if (Convert.ToString((Request[txtWaveIntAmt.UniqueID] as string == null) ? txtWaveIntAmt.Text : Request[txtWaveIntAmt.UniqueID]) != "")
                    vWaveInt = Convert.ToDecimal((Request[txtWaveIntAmt.UniqueID] as string == null) ? txtWaveIntAmt.Text : Request[txtWaveIntAmt.UniqueID] as string);
                if (Convert.ToString((Request[txtTotCollAmt.UniqueID] as string == null) ? txtTotCollAmt.Text : Request[txtTotCollAmt.UniqueID]) != "")
                    vTotColl = Convert.ToDecimal((Request[txtTotCollAmt.UniqueID] as string == null) ? txtTotCollAmt.Text : Request[txtTotCollAmt.UniqueID] as string);
                if (hdOutStand.Value != "")
                    vPrinOS = Convert.ToDecimal((Request[hdOutStand.UniqueID] as string == null) ? hdOutStand.Value : Request[hdOutStand.UniqueID] as string);
                if (hdSlNo.Value != "")
                    vSlNo = Convert.ToInt32((Request[hdSlNo.UniqueID] as string == null) ? hdSlNo.Value : Request[hdSlNo.UniqueID] as string);
                if (((Request[txtBounceAmt.UniqueID] as string == null) ? txtBounceAmt.Text : Request[txtBounceAmt.UniqueID] as string) != "")
                    vBounceAmt = Convert.ToDecimal((Request[txtBounceAmt.UniqueID] as string == null) ? txtBounceAmt.Text : Request[txtBounceAmt.UniqueID] as string);
                if (hdBalAmt.Value != "")
                    vBalAmt = Convert.ToDecimal((Request[hdBalAmt.UniqueID] as string == null) ? hdBalAmt.Value : Request[hdBalAmt.UniqueID] as string);
                if (hdIntDue.Value != "")
                    vIntDue = Convert.ToDecimal((Request[hdIntDue.UniqueID] as string == null) ? hdIntDue.Value : Request[hdIntDue.UniqueID] as string);
                


                // In Case Of Interest Due Consider OD Days
                //if (vIntDue > 0)
                //{
                    oLR = new CLoanRecovery();
                    DataTable dtODDays = oLR.CalODDaysByLoanId(vLoanId, vAccDate);
                    if (dtODDays.Rows.Count > 0)
                    {
                        vODDays = Convert.ToInt32(dtODDays.Rows[0]["ODDays"]);
                    }
                //}
                if (vTotColl <= 0)
                {
                    gblFuction.AjxMsgPopup("Total Collection Amount Can Not Be zero or Negetive...");
                    return;
                }
                if (cbBounce.Checked == false)
                {
                    gblFuction.AjxMsgPopup("Bounce Checkbox can not be unchecked....");
                    return;
                }
                CDisburse oLD = new CDisburse();
                DataTable dt = null;
                int vLoanTypeID = 0;
                string vLoanAc = "", vLoanIntAc = "", vPenalAc = "", vIntWaiveAc = "", vBounceChargeAc = "",vIntDueAC="",
                    vVisitChrgeAC = "", vCGSTAC, vSGSTAC = "";
                dt = oLD.GetDisbDtlbyLoanId(vLoanId, vBranch);
                if (dt.Rows.Count > 0)
                {
                    vLoanTypeID = Convert.ToInt32(dt.Rows[0]["LoanTypeId"]);
                    vLoanAc = Convert.ToString(dt.Rows[0]["LoanAC"]);
                    vLoanIntAc = Convert.ToString(dt.Rows[0]["InstAC"]);
                    vPenalAc = Convert.ToString(dt.Rows[0]["PenaltyChargeAC"]);
                    vIntWaiveAc = Convert.ToString(dt.Rows[0]["IntWaveAC"]);
                    vBounceChargeAc = Convert.ToString(dt.Rows[0]["BounceChrgAC"]);
                    vIntDueAC = Convert.ToString(dt.Rows[0]["IntDueAC"]);
                    vVisitChrgeAC = Convert.ToString(dt.Rows[0]["VisitChargeAC"]);
                    vCGSTAC = Convert.ToString(dt.Rows[0]["CGSTAC"]);
                    vSGSTAC = Convert.ToString(dt.Rows[0]["SGSTAC"]);
                }
                else
                {
                    gblFuction.AjxMsgPopup("Invalid Loan.");
                    return;
                }
                vCollMode = (Request[hdCollMode.UniqueID] as string == null) ? hdCollMode.Value : Request[hdCollMode.UniqueID] as string;
                vBankLedgrAC = (Request[hdDescId.UniqueID] as string == null) ? hdDescId.Value : Request[hdDescId.UniqueID] as string;
                vBankName = (Request[hdBankName.UniqueID] as string == null) ? hdBankName.Value : Request[hdBankName.UniqueID] as string;
                vChqRefNo = (Request[txtChqRefNo.UniqueID] as string == null) ? txtChqRefNo.Text : Request[txtChqRefNo.UniqueID] as string;
                vTransMode = (Request[hdTransMode.UniqueID] as string == null) ? hdTransMode.Value : Request[hdTransMode.UniqueID] as string;

                DataTable dtAccount = new DataTable();
                DataRow dr;
                DataColumn dc = new DataColumn();
                dc.ColumnName = "DC";
                dtAccount.Columns.Add(dc);
                DataColumn dc1 = new DataColumn();
                dc1.ColumnName = "Amt";
                dc1.DataType = System.Type.GetType("System.Decimal");
                dtAccount.Columns.Add(dc1);
                DataColumn dc2 = new DataColumn();
                dc2.ColumnName = "DescId";
                dtAccount.Columns.Add(dc2);
                DataColumn dc3 = new DataColumn();
                dc3.ColumnName = "DtlId";
                dtAccount.Columns.Add(dc3);
                dtAccount.TableName = "Table1";
                if (Convert.ToDecimal((Request[txtTotCollAmt.UniqueID] as string == null) ? txtTotCollAmt.Text : Request[txtTotCollAmt.UniqueID] as string) > 0)
                {
                    int i = 1;
                    dr = dtAccount.NewRow();
                    dr["DescId"] = vBankLedgrAC;
                    dr["DC"] = "C";
                    dr["Amt"] = Convert.ToDecimal((Request[txtTotCollAmt.UniqueID] as string == null) ? txtTotCollAmt.Text : Request[txtTotCollAmt.UniqueID] as string);
                    dr["DtlId"] = i;
                    dtAccount.Rows.Add(dr);
                    dtAccount.AcceptChanges();

                    if (Convert.ToDecimal((Request[txtPrinCollAmt.UniqueID] as string == null) ? txtPrinCollAmt.Text : Request[txtPrinCollAmt.UniqueID] as string) > 0)
                    {
                        i = i + 1;
                        dr = dtAccount.NewRow();
                        dr["DescId"] = vLoanAc;
                        dr["DC"] = "D";
                        dr["Amt"] = Convert.ToDecimal((Request[txtPrinCollAmt.UniqueID] as string == null) ? txtPrinCollAmt.Text : Request[txtPrinCollAmt.UniqueID] as string);
                        dr["DtlId"] = i;
                        dtAccount.Rows.Add(dr);
                        dtAccount.AcceptChanges();
                    }
                    if (vODDays > 90)
                    {
                        if (Convert.ToDecimal((Request[txtIntCollAmt.UniqueID] as string == null) ? txtIntCollAmt.Text : Request[txtIntCollAmt.UniqueID] as string) > 0)
                        {
                            i = i + 1;
                            dr = dtAccount.NewRow();
                            dr["DescId"] = vLoanIntAc;
                           // dr["DescId"] = vIntDueAC;
                            dr["DC"] = "D";
                            dr["Amt"] = Convert.ToDecimal((Request[txtIntCollAmt.UniqueID] as string == null) ? txtIntCollAmt.Text : Request[txtIntCollAmt.UniqueID] as string);
                            dr["DtlId"] = i;
                            dtAccount.Rows.Add(dr);
                            dtAccount.AcceptChanges();
                        }
                        if (vIntDue > 0)
                        {
                            i = i + 1;
                            dr = dtAccount.NewRow();
                            dr["DescId"] = vIntDueAC;
                            dr["DC"] = "C";
                            dr["Amt"] = vIntDue;
                            dr["DtlId"] = i;
                            dtAccount.Rows.Add(dr);
                            dtAccount.AcceptChanges();

                            i = i + 1;
                            dr = dtAccount.NewRow();
                            dr["DescId"] = vLoanIntAc;
                            dr["DC"] = "D";
                            dr["Amt"] = vIntDue;
                            dr["DtlId"] = i;
                            dtAccount.Rows.Add(dr);
                            dtAccount.AcceptChanges();
                        }
                    }
                    else
                    {
                        if (Convert.ToDecimal((Request[txtIntCollAmt.UniqueID] as string == null) ? txtIntCollAmt.Text : Request[txtIntCollAmt.UniqueID] as string) > 0)
                        {
                            i = i + 1;
                            dr = dtAccount.NewRow();
                            //dr["DescId"] = vLoanIntAc;
                            dr["DescId"] = vIntDueAC;
                            dr["DC"] = "D";
                            dr["Amt"] = Convert.ToDecimal((Request[txtIntCollAmt.UniqueID] as string == null) ? txtIntCollAmt.Text : Request[txtIntCollAmt.UniqueID] as string);
                            dr["DtlId"] = i;
                            dtAccount.Rows.Add(dr);
                            dtAccount.AcceptChanges();
                        }
                    }

                    if (vBounceRec > 0)
                    {
                        decimal vBounceCGST = 0, vBounceSGST = 0, vBounceCollAmt = 0;
                        vBounceCollAmt = Math.Round(((100 * vBounceRec / 118)),2);
                        vBounceCGST = Math.Round((vBounceCollAmt * 9 / 100), 2);
                        vBounceSGST = Math.Round((vBounceCollAmt * 9 / 100), 2);
                        // Net Bounce Collected
                        i = i + 1;
                        dr = dtAccount.NewRow();
                        dr["DescId"] = vBounceChargeAc;
                        dr["DC"] = "D";
                        dr["Amt"] = vBounceCollAmt;
                        dr["DtlId"] = i;
                        dtAccount.Rows.Add(dr);
                        dtAccount.AcceptChanges();
                        // Bounce CGST Amount
                        i = i + 1;
                        dr = dtAccount.NewRow();
                        dr["DescId"] = vCGSTAC;
                        dr["DC"] = "D";
                        dr["Amt"] = vBounceCGST;
                        dr["DtlId"] = i;
                        dtAccount.Rows.Add(dr);
                        dtAccount.AcceptChanges();
                        // Bounce CGST Amount
                        i = i + 1;
                        dr = dtAccount.NewRow();
                        dr["DescId"] = vSGSTAC;
                        dr["DC"] = "D";
                        dr["Amt"] = vBounceSGST;
                        dr["DtlId"] = i;
                        dtAccount.Rows.Add(dr);
                        dtAccount.AcceptChanges();
                        
                    }
                    
                    if (Convert.ToDecimal((Request[txtPenCollAmt.UniqueID] as string == null) ? txtPenCollAmt.Text : Request[txtPenCollAmt.UniqueID] as string) > 0)
                    {
                        if (vPenalAc == "")
                        {
                            gblFuction.AjxMsgPopup("Delay Payment AC Not Set In Loan Parameter");
                            return;
                        }
                        i = i + 1;
                        dr = dtAccount.NewRow();
                        dr["DescId"] = vPenalAc;
                        dr["DC"] = "D";
                        dr["Amt"] = Convert.ToDecimal((Request[txtPenCollAmt.UniqueID] as string == null) ? txtPenCollAmt.Text : Request[txtPenCollAmt.UniqueID] as string);
                        dr["DtlId"] = i;
                        dtAccount.Rows.Add(dr);
                        dtAccount.AcceptChanges();
                    }
                    if (Convert.ToDecimal((Request[txtPenCGST.UniqueID] as string == null) ? txtPenCGST.Text : Request[txtPenCGST.UniqueID] as string) > 0)
                    {
                        if (vCGSTAC == "")
                        {
                            gblFuction.AjxMsgPopup("CGST AC Not Set In Loan Parameter");
                            return;
                        }
                        i = i + 1;
                        dr = dtAccount.NewRow();
                        dr["DescId"] = vCGSTAC;
                        dr["DC"] = "D";
                        dr["Amt"] = Convert.ToDecimal((Request[txtPenCGST.UniqueID] as string == null) ? txtPenCGST.Text : Request[txtPenCGST.UniqueID] as string);
                        dr["DtlId"] = i;
                        dtAccount.Rows.Add(dr);
                        dtAccount.AcceptChanges();
                    }
                    if (Convert.ToDecimal((Request[txtPenSGST.UniqueID] as string == null) ? txtPenSGST.Text : Request[txtPenSGST.UniqueID] as string) > 0)
                    {
                        if (vSGSTAC == "")
                        {
                            gblFuction.AjxMsgPopup("SGST AC Not Set In Loan Parameter");
                            return;
                        }
                        i = i + 1;
                        dr = dtAccount.NewRow();
                        dr["DescId"] = vSGSTAC;
                        dr["DC"] = "D";
                        dr["Amt"] = Convert.ToDecimal((Request[txtPenSGST.UniqueID] as string == null) ? txtPenSGST.Text : Request[txtPenSGST.UniqueID] as string);
                        dr["DtlId"] = i;
                        dtAccount.Rows.Add(dr);
                        dtAccount.AcceptChanges();
                    }
                    if (Convert.ToDecimal((Request[txtVisitChargeRec.UniqueID] as string == null) ? txtVisitChargeRec.Text : Request[txtVisitChargeRec.UniqueID] as string) > 0)
                    {
                        if (vVisitChrgeAC == "" || vVisitChrgeAC == "-1")
                        {
                            gblFuction.AjxMsgPopup("Visiting Charge  AC Not Set in Loan Parameter");
                            return;
                        }
                        i = i + 1;
                        dr = dtAccount.NewRow();
                        dr["DescId"] = vVisitChrgeAC;
                        dr["DC"] = "D";
                        dr["Amt"] = Convert.ToDecimal((Request[txtVisitChargeRec.UniqueID] as string == null) ? txtVisitChargeRec.Text : Request[txtVisitChargeRec.UniqueID] as string);
                        dr["DtlId"] = i;
                        dtAccount.Rows.Add(dr);
                        dtAccount.AcceptChanges();
                    }
                    if (Convert.ToDecimal((Request[txtVisitChargeCGST.UniqueID] as string == null) ? txtVisitChargeCGST.Text : Request[txtVisitChargeCGST.UniqueID] as string) > 0)
                    {
                        if (vCGSTAC == "" || vCGSTAC == "-1")
                        {
                            gblFuction.AjxMsgPopup("CGST AC Not Set In Loan Parameter");
                            return;
                        }
                        i = i + 1;
                        dr = dtAccount.NewRow();
                        dr["DescId"] = vCGSTAC;
                        dr["DC"] = "D";
                        dr["Amt"] = Convert.ToDecimal((Request[txtVisitChargeCGST.UniqueID] as string == null) ? txtVisitChargeCGST.Text : Request[txtVisitChargeCGST.UniqueID] as string);
                        dr["DtlId"] = i;
                        dtAccount.Rows.Add(dr);
                        dtAccount.AcceptChanges();
                    }
                    if (Convert.ToDecimal((Request[txtVisitChargeSGST.UniqueID] as string == null) ? txtVisitChargeSGST.Text : Request[txtVisitChargeSGST.UniqueID] as string) > 0)
                    {
                        if (vSGSTAC == "" || vSGSTAC == "-1")
                        {
                            gblFuction.AjxMsgPopup("SGST AC Not Set In Loan Parameter");
                            return;
                        }
                        i = i + 1;
                        dr = dtAccount.NewRow();
                        dr["DescId"] = vSGSTAC;
                        dr["DC"] = "D";
                        dr["Amt"] = Convert.ToDecimal((Request[txtVisitChargeSGST.UniqueID] as string == null) ? txtVisitChargeSGST.Text : Request[txtVisitChargeSGST.UniqueID] as string);
                        dr["DtlId"] = i;
                        dtAccount.Rows.Add(dr);
                        dtAccount.AcceptChanges();
                    }

                    if (Convert.ToDecimal((Request[txtWaveIntAmt.UniqueID] as string == null) ? txtWaveIntAmt.Text : Request[txtWaveIntAmt.UniqueID] as string) > 0)
                    {
                        i = i + 1;
                        dr = dtAccount.NewRow();
                        dr["DescId"] = vIntWaiveAc;
                        dr["DC"] = "C";
                        dr["Amt"] = Convert.ToDecimal((Request[txtWaveIntAmt.UniqueID] as string == null) ? txtWaveIntAmt.Text : Request[txtWaveIntAmt.UniqueID] as string);
                        dr["DtlId"] = i;
                        dtAccount.Rows.Add(dr);
                        dtAccount.AcceptChanges();
                    }
                }
                dtAccount.TableName = "Table1";

                decimal vTotalAmt = vPrinColl + vIntColl - vWaveInt + vPenCollAmt + vPenCGST + vPenSGST + vVisitChargeCollAmt
                    + vVisitChargeCGST + vVisitChargeSGST + vBounceRec;
                if (vTotColl != vTotalAmt)
                {
                    gblFuction.AjxMsgPopup("Debit Credit Mismatch...");
                    return;
                }
                // string vCustName = lblCustName.Text.ToString();
                string vCustName = ddlCustName.SelectedItem.Text.ToString();
                //string vCustName = (Request[ddlCustName.UniqueID] as string == null) ? ddlCustName.SelectedItem.Text : Request[ddlCustName.UniqueID] as string;
                vNarationL = "Being the Amt Bounce of Loan Recovery for  " + vCustName;

                using (StringWriter oSW = new StringWriter())
                {
                    dtAccount.WriteXml(oSW);
                    vXmlAC = oSW.ToString();
                }

                ////vPenWaiveAmt = 0, vPenCGST = 0, vPenSGST = 0,vPenDue=0,
                ////vVisitChargeCollAmt = 0, vVisitChargeWaiveAmt = 0, vVisitChargeCGST = 0, vVisitChargeSGST = 0, vVisitChargeDue = 0;
                oLR = new CLoanRecovery();
                vErr = oLR.SaveBounceCollection(vLoanId, vAccDate, vSancId, vCustId, vPrinColl, vIntColl, vBounceRec,vBounceWaived, vWaveInt, vTotColl,
                    vPrinOS, vActMstTbl, vActDtlTbl, vFinYear, vCollMode, vTransMode, vBankLedgrAC, vBankName, vChqRefNo, vBranch, this.UserID, vXmlAC, 
                    vNarationL, vSlNo, vBounceAmt, vBalAmt,vLoanStatus,vIntDue,vODDays,
                    vPenCollAmt, vPenWaiveAmt, vPenCGST, vPenSGST,
                    vVisitChargeCollAmt, vVisitChargeWaiveAmt, vVisitChargeCGST, vVisitChargeSGST, vPenDue, vVisitChargeDue);
                if (vErr == 0)
                {
                    gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                    ClearControls();
                    tabCollBounce.ActiveTabIndex = 0;
                    StatusButton("View");
                }
                else
                {
                    gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                }

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
    }
}