using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class LoanReschedule : CENTRUMBAse
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
                LoadReSecheduleList();
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
                this.PageHeading = "Loan Rescheduling";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLnRescheduling);
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
                tabLnSechedule.ActiveTabIndex = 1;
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

                CDisburse ODisb = new CDisburse();
                Int32 vLnTypeId = 0, vLastInstNo = 0;
                Decimal vPOSAmt = 0, vLnAmt = 0, vNewEMIAmt = 0, vFIntRate = 0, vRIntRate = 0,vRPAmt=0,vRAAmt=0,vInterestAmt=0;
                string vLoanId = "", vPaySchedule = "",vDueMonth="",vModifiedDueMonth="";
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                if (Convert.ToString((Request[txtDueDate.UniqueID] as string == null) ? txtDueDate.Text : Request[txtDueDate.UniqueID] as string) == "")
                {
                    gblFuction.AjxMsgPopup("Due Date Can Not Be Blank...");
                    return;
                }
                if (Convert.ToString((Request[txtModDueDate.UniqueID] as string == null) ? txtModDueDate.Text : Request[txtModDueDate.UniqueID] as string) == "")
                {
                    gblFuction.AjxMsgPopup("Modified Due Date Can Not Be Blank...");
                    return;
                }
                if (Convert.ToString((Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string) == "")
                {
                    gblFuction.AjxMsgPopup("Select Loan No To Generate Re Scedule");
                    return;
                }
                else if (Convert.ToString((Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string) == "-1")
                {
                    gblFuction.AjxMsgPopup("Select Loan No To Generate Re Scedule");
                    return;
                }
                //Convert.ToString((Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string) == ""
                if (Request[txtPOS.UniqueID] as string == "")
                {
                    gblFuction.AjxMsgPopup("Outstanding Amount can not be empty...");
                    return;
                }
                if (Convert.ToDecimal(Request[txtPOS.UniqueID] as string) <= 0)
                {
                    gblFuction.AjxMsgPopup("Outstanding Amount can not be zero...");
                    return;
                }
                if (Convert.ToDecimal(Request[txtRIntRate.UniqueID] as string) <= 0)
                {
                    gblFuction.AjxMsgPopup("Reducing Interest Rate can not be zero...");
                    return;
                }
                if (Convert.ToInt32((Request[txtLastInstallNo.UniqueID] as string == null) ? txtLastInstallNo.Text : Request[txtLastInstallNo.UniqueID] as string) <= 0)
                {
                    gblFuction.AjxMsgPopup("Last Installment No can not be zero...");
                    return;
                }
                if (txtNewEMIAmt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Modified EMI Amount can not blank");
                    return ;
                }
                else if (Convert.ToDecimal(txtNewEMIAmt.Text) <= 0)
                {
                    gblFuction.AjxMsgPopup("Modified EMI Amount can not be less than or equal to zero...");
                    return ;
                }

                if (gblFuction.setDate(Convert.ToString((Request[txtLastRecDate.UniqueID] as string == null) ? txtLastRecDate.Text : Request[txtLastRecDate.UniqueID] as string)) > gblFuction.setDate(txtReSchDt.Text))
                {
                    gblFuction.AjxMsgPopup("Last Collection Date Can Not be grater Rescheduling date...");
                    return;
                }

                vDueMonth = getMonth(Convert.ToString((Request[txtDueDate.UniqueID] as string == null) ? txtDueDate.Text : Request[txtDueDate.UniqueID] as string));
                vModifiedDueMonth = getMonth(txtModDueDate.Text);
                if (Convert.ToInt32(vDueMonth) != Convert.ToInt32(vModifiedDueMonth))
                {
                    gblFuction.AjxMsgPopup("Actual Due Date and Modified Due Date will be in same Month...");
                    return;
                }
                vLoanId = (Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string;
                vLnTypeId = Convert.ToInt32((Request[hfLoanTypeId.UniqueID] as string == null) ? hfLoanTypeId.Value : Request[hfLoanTypeId.UniqueID] as string);

                vRPAmt = Convert.ToDecimal((Request[hfRPAmt.UniqueID] as string == null) ? hfRPAmt.Value : Request[hfRPAmt.UniqueID] as string);
                vRAAmt = Convert.ToDecimal((Request[hfRAAmt.UniqueID] as string == null) ? hfRAAmt.Value : Request[hfRAAmt.UniqueID] as string);


                vLastInstNo = Convert.ToInt32((Request[txtLastInstallNo.UniqueID] as string == null) ? txtLastInstallNo.Text : Request[txtLastInstallNo.UniqueID] as string);
                vPOSAmt = Convert.ToDecimal(Request[txtPOS.UniqueID] as string);
                vLnAmt = Convert.ToDecimal(Request[txtLnAmt.UniqueID] as string);
                vFIntRate = Convert.ToDecimal(Request[txtFIntRate.UniqueID] as string);
                vRIntRate = Convert.ToDecimal(Request[txtRIntRate.UniqueID] as string);
                DateTime vLnDt = gblFuction.setDate(Request[txtLnDt.UniqueID] as string);
                DateTime vStartDt = gblFuction.setDate(Request[hfRepayStartDt.UniqueID] as string);
                DateTime vDueDt = gblFuction.setDate(txtModDueDate.Text);
                DateTime vDefaultDt = vDueDt.AddMonths(1);
                //DateTime vDefaultDt = gblFuction.setDate(Request[hfDefaultDt.UniqueID] as string);
                vNewEMIAmt = Convert.ToDecimal(Request[txtNewEMIAmt.UniqueID] as string);
                vPaySchedule = (Request[hfPaySchedule.UniqueID] as string == null) ? hfPaySchedule.Value : Request[hfPaySchedule.UniqueID] as string;

                vInterestAmt = Math.Round(Convert.ToDecimal((vPOSAmt * vRIntRate*1)/1200),0);
                if (vNewEMIAmt < vInterestAmt)
                {
                    gblFuction.AjxMsgPopup("Newly Supplied EMI Amount can not be less than newly calculate Interest Amount "+Convert.ToString(vInterestAmt));
                    return;
                }
                DataTable dtSchedule = new DataTable();
                dtSchedule = ODisb.GetReSchedule(vLnTypeId, vPOSAmt, vRIntRate, vDueDt, vDefaultDt, vStartDt, vLoanId, vBrCode, vPaySchedule, "", "",
                    vNewEMIAmt, vLastInstNo, vRPAmt,vRAAmt);
                if (dtSchedule.Rows.Count > 0)
                {
                    gvSchdl.DataSource = dtSchedule;
                    gvSchdl.DataBind();
                }
                else
                {
                    gvSchdl.DataSource = null;
                    gvSchdl.DataBind();
                }
                tabLnSechedule.ActiveTabIndex = 2;
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
            CDisburse ODisb = new CDisburse();
            Int32 vLnTypeId = 0, vLastInstNo = 0,vPreTotInstNo=0;
            Decimal vPOSAmt = 0, vLnAmt = 0, vNewEMIAmt = 0, vPreEMIAmt = 0, vFIntRate = 0, vRIntRate = 0, vRPAmt = 0, vRAAmt = 0, vInterestAmt=0; 
            string vLoanId = "", vPaySchedule = "",vRemarks="" ,vDueMonth="",vModifiedDueMonth="";
            Int32 vErr = 0;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Boolean vResult = false;
            if (Convert.ToString((Request[txtDueDate.UniqueID] as string == null) ? txtDueDate.Text : Request[txtDueDate.UniqueID] as string) == "")
            {
                gblFuction.AjxMsgPopup("Due Date Can Not Be Blank...");
                return false;
            }
            if (Convert.ToString((Request[txtModDueDate.UniqueID] as string == null) ? txtModDueDate.Text : Request[txtModDueDate.UniqueID] as string) == "")
            {
                gblFuction.AjxMsgPopup("Modified Due Date Can Not Be Blank...");
                return false;
            }
            if (txtReSchDt.Text == "")
            {
                gblFuction.MsgPopup("ReSechedule Date Can Not Be Blank..");
                return false;
            }
            if (Convert.ToString((Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string) == "")
            {
                gblFuction.AjxMsgPopup("Select Loan No ....");
                return false;
            }
            else if (Convert.ToString((Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string) == "-1")
            {
                gblFuction.AjxMsgPopup("Select Loan No ...");
                return false;
            }
            if (Convert.ToDecimal(Request[txtPOS.UniqueID] as string) <= 0)
            {
                gblFuction.MsgPopup("Outstanding Amount can not be zero...");
                return false;
            }
            if (Convert.ToDecimal(Request[txtRIntRate.UniqueID] as string) <= 0)
            {
                gblFuction.MsgPopup("Reducing Interest Rate can not be zero...");
                return false;
            }
            if (Convert.ToInt32((Request[txtLastInstallNo.UniqueID] as string == null) ? txtLastInstallNo.Text : Request[txtLastInstallNo.UniqueID] as string) <= 0)
            {
                gblFuction.MsgPopup("Last Installment No can not be zero...");
                return false;
            }
            if (txtNewEMIAmt.Text == "")
            {
                gblFuction.MsgPopup("Modified EMI Amount can not blank");
                return false;
            }
            else if (Convert.ToDecimal(txtNewEMIAmt.Text)<=0)
            {
                gblFuction.MsgPopup("Modified EMI Amount can not be less than or equal to zero...");
                return false;
            }
            vInterestAmt = Math.Round(Convert.ToDecimal((vPOSAmt * vRIntRate * 1) / 1200), 0);
            if (vNewEMIAmt < vInterestAmt)
            {
                gblFuction.AjxMsgPopup("Newly Supplied EMI Amount can not be less than newly calculate Interest Amount " + Convert.ToString(vInterestAmt));
                return false;
            }
            if (gblFuction.setDate(Convert.ToString((Request[txtLastRecDate.UniqueID] as string == null) ? txtLastRecDate.Text : Request[txtLastRecDate.UniqueID] as string)) > gblFuction.setDate(txtReSchDt.Text))
            {
                gblFuction.AjxMsgPopup("Last Collection Date Can Not be grater Rescheduling date...");
                return false;
            }

            vDueMonth = getMonth(Convert.ToString((Request[txtDueDate.UniqueID] as string == null) ? txtDueDate.Text : Request[txtDueDate.UniqueID] as string));
            vModifiedDueMonth = getMonth(txtModDueDate.Text);
            if (Convert.ToInt32(vDueMonth) != Convert.ToInt32(vModifiedDueMonth))
            {
                gblFuction.AjxMsgPopup("Actual Due Date and Modified Due Date will be in same Month...");
                return false;
            }
            vLoanId = (Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string;
            vLnTypeId = Convert.ToInt32((Request[hfLoanTypeId.UniqueID] as string == null) ? hfLoanTypeId.Value : Request[hfLoanTypeId.UniqueID] as string);
            vLastInstNo = Convert.ToInt32((Request[txtLastInstallNo.UniqueID] as string == null) ? txtLastInstallNo.Text : Request[txtLastInstallNo.UniqueID] as string);
            vPOSAmt = Convert.ToDecimal(Request[txtPOS.UniqueID] as string);
            vLnAmt = Convert.ToDecimal(Request[txtLnAmt.UniqueID] as string);
            vFIntRate = Convert.ToDecimal(Request[txtFIntRate.UniqueID] as string);
            vRIntRate = Convert.ToDecimal(Request[txtRIntRate.UniqueID] as string);
            DateTime vLnDt = gblFuction.setDate(Request[txtLnDt.UniqueID] as string);
            DateTime vStartDt = gblFuction.setDate(Request[hfRepayStartDt.UniqueID] as string);
            DateTime vDueDt = gblFuction.setDate(txtModDueDate.Text);
            DateTime vDefaultDt = vDueDt.AddMonths(1);
            //DateTime vDueDt = gblFuction.setDate(Request[hfDueDt.UniqueID] as string);
            //DateTime vDefaultDt = gblFuction.setDate(Request[hfDefaultDt.UniqueID] as string);
            vNewEMIAmt = Convert.ToDecimal(Request[txtNewEMIAmt.UniqueID] as string);
            vPaySchedule = (Request[hfPaySchedule.UniqueID] as string == null) ? hfPaySchedule.Value : Request[hfPaySchedule.UniqueID] as string;

            vRPAmt = Convert.ToDecimal((Request[hfRPAmt.UniqueID] as string == null) ? hfRPAmt.Value : Request[hfRPAmt.UniqueID] as string);
            vRAAmt = Convert.ToDecimal((Request[hfRAAmt.UniqueID] as string == null) ? hfRAAmt.Value : Request[hfRAAmt.UniqueID] as string);

            DateTime vReScheDt = gblFuction.setDate(Request[txtReSchDt.UniqueID] as string);
            DateTime vLastRecDt = gblFuction.setDate(Request[txtLastRecDate.UniqueID] as string);
            decimal vPreIntDue = Convert.ToDecimal(Request[txtPreIntDue.UniqueID] as string);
            vPreEMIAmt = Convert.ToDecimal(Request[txtPrevEMIAMt.UniqueID] as string);
            vRemarks = txtRemarks.Text;
            vPreTotInstNo = Convert.ToInt32((Request[txtTotInstNo.UniqueID] as string == null) ? txtTotInstNo.Text : Request[txtTotInstNo.UniqueID] as string);

            if (Mode == "Save")
            {
                vErr = ODisb.InsertReScheduleMst(vLnTypeId, vPOSAmt, vFIntRate, vRIntRate, vDueDt, vDefaultDt, vStartDt, vLoanId, vBrCode, vPaySchedule, "", "",
                    vNewEMIAmt, vLastInstNo, vReScheDt, vLnDt, vLnAmt, vLastRecDt, vPreIntDue, vPreEMIAmt, vRemarks, vPreTotInstNo, 1, "I", 0, vRPAmt, vRAAmt);

                if (vErr == 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    vResult = true;
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                    vResult = false;
                }
            }
            LoadReSecheduleList();
            tabLnSechedule.ActiveTabIndex = 0;
            return vResult;
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
           
            //ddlCust.Enabled = Status;
            //ddlSancNo.Enabled = Status;
            //txtLnDt.Enabled = Status;
            //ddlSrcFund.Enabled = Status;
            //rdbPayMode.Enabled = Status;
            //txtChqNo.Enabled = Status;
            //txtChqDt.Enabled = Status;
           
            //txtStDt.Enabled = Status;
            //ddlBank.Enabled = Status;
            //ddlLedgr.Enabled = Status;
            //txtRefNo.Enabled = Status;
           
            //txtInsSerTax.Enabled = Status;
            /*************************/
        }

        private void ClearControls()
        {
            //txtReSchDt.Text = "";
            ddlCust.SelectedValue = "-1";
            ddlLoanNo.SelectedValue = "-1";
            hfLoanTypeId.Value = "";
            hfDueDt.Value = "";
            hfDefaultDt.Value = "";
            hfRepayStartDt.Value = "";
            hfPaySchedule.Value = "";
            txtTotInstNo.Text = "";
            txtPOS.Text = "";
            txtPreIntDue.Text = "";
            txtLnDt.Text = "";
            txtFIntRate.Text = "";
            txtRIntRate.Text = "";
            txtLastRecDate.Text = "";
            txtLastInstallNo.Text = "";
            txtLnAmt.Text = "";
            txtPrevEMIAMt.Text = "";
            txtNewEMIAmt.Text = "";
            txtRemarks.Text = "";
            txtModDueDate.Text = "";
            
        }

        private void LoadReSecheduleList()
        {
            DataTable dt = null;
            CDisburse oCD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            try
            {
                oCD = new CDisburse();
                dt = oCD.GetReScheduleList();
                if (dt.Rows.Count > 0)
                {
                    gvReSchdl.DataSource = dt;
                    gvReSchdl.DataBind();
                }
                else
                {
                    gvReSchdl.DataSource = null;
                    gvReSchdl.DataBind();
                }
            }
            finally
            {
                oCD = null;
                dt = null;
            }
        }
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

        public static string getMonth(string pDate)
        {
            string StrDD = "", StrMM = "", StrYYYY = "";
            DateTime dDate = System.DateTime.Now;
            if (pDate == "")
                dDate = Convert.ToDateTime("01/01/1900");
            else
            {
                if (pDate.Length == 9)
                    pDate = pDate.Insert(0, "0");

                StrDD = pDate.Substring(0, 2);
                StrMM = pDate.Substring(3, 2);
                StrYYYY = pDate.Substring(6, 4);
            }
            return StrMM;
        }
        //protected void gvReSchdl_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    String vReSchId = "";
        //    string vLonNo = "";
        //    DataTable dt = null;
        //    CDisburse oCD = null;
        //    try
        //    {
        //        vReSchId = Convert.ToString(e.CommandArgument);
        //        if (e.CommandName == "cmdShow")
        //        {
        //            GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        //            LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
        //            vLonNo = btnShow.Text;
        //            oCD = new CDisburse();
        //            dt = oCD.GetReSchedlDtlbyID(Convert.ToInt32(vReSchId), vLonNo);
        //            if (dt.Rows.Count > 0)
        //            {
        //                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
        //                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
        //                foreach (GridViewRow gr in gvReSchdl.Rows)
        //                {
        //                    if ((gr.RowIndex) % 2 == 0)
        //                    {
        //                        gr.BackColor = backColor;
        //                        gr.ForeColor = foreColor;
        //                    }
        //                    else
        //                    {
        //                        gr.BackColor = System.Drawing.Color.White;
        //                        gr.ForeColor = foreColor;
        //                    }
        //                    gr.Font.Bold = false;
        //                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
        //                    lb.ForeColor = System.Drawing.Color.SkyBlue;
        //                    lb.Font.Bold = false;
        //                }
        //                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#151B54");
        //                gvRow.ForeColor = System.Drawing.Color.White;
        //                gvRow.Font.Bold = true;
        //                btnShow.ForeColor = System.Drawing.Color.White;
        //                btnShow.Font.Bold = true;


        //                ddlCust.SelectedValue = dt.Rows[0]["CustId"].ToString();
        //                popLnNo(dt.Rows[0]["CustId"].ToString());
        //                ddlLoanNo.SelectedValue = dt.Rows[0]["LoanId"].ToString();
        //                txtReSchDt.Text = dt.Rows[0]["RescheduleDate"].ToString();
        //                txtTotInstNo.Text = dt.Rows[0]["ModifiedTotInstNo"].ToString();



        //                txtPOS.Text = dt.Rows[0]["POSAmt"].ToString();
        //                txtPreIntDue.Text = dt.Rows[0]["PreIntDue"].ToString();
        //                txtLnDt.Text = dt.Rows[0]["LoanDate"].ToString();
        //                txtFIntRate.Text = dt.Rows[0]["FIntRate"].ToString();
        //                txtRIntRate.Text = dt.Rows[0]["RIntRate"].ToString();
        //                txtLastRecDate.Text = dt.Rows[0]["LastRecDate"].ToString();
        //                txtLastInstallNo.Text = dt.Rows[0]["LastInstNo"].ToString();

        //                txtLnAmt.Text = dt.Rows[0]["LoanAmt"].ToString();
        //                txtPrevEMIAMt.Text = dt.Rows[0]["PreEMIAmt"].ToString();
        //                txtNewEMIAmt.Text = dt.Rows[0]["ModifiedEMIAmt"].ToString();
        //                txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
        //                tabLnSechedule.ActiveTabIndex = 1;
                       
        //                StatusButton("Show");
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oCD = null;
        //    }
        //}
        
    }
}
