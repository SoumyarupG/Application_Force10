using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.Data;
using System.IO;


namespace CENTRUM.WebPages.Private.Master
{
    public partial class EmpKPI : CENTRUMBase
    {
        protected static int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    gblFuction.AjxMsgPopup("You Cannot open this page from branch login...");
                else
                    StatusButton("View");
                PopBranch(Session[gblValue.UserName].ToString());
                txtKPIDt.Text = Session[gblValue.LoginDate].ToString();
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                LoadGrid(1);
                tabKPI.ActiveTabIndex = 0;
            }

        }
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
        private void EnableControl(Boolean Status)
        {
            txtSlfCom1.Enabled = Status;
            txtSupCom1.Enabled = Status;
            txtWtScr1.Enabled = Status;
            txtSupScr1.Enabled = Status;
            txtSlfScr1.Enabled = Status;
            txtWtPerc1.Enabled = Status;
            txtKeyResult1.Enabled = Status;
            txtSlfCom2.Enabled = Status;
            txtSupCom2.Enabled = Status;
            txtWtScr2.Enabled = Status;
            txtSupScr2.Enabled = Status;
            txtSlfScr2.Enabled = Status;
            txtWtPerc2.Enabled = Status;
            txtKeyResult2.Enabled = Status;
            txtSlfCom3.Enabled = Status;
            txtSupCom3.Enabled = Status;
            txtWtScr3.Enabled = Status;
            txtSupScr3.Enabled = Status;
            txtSlfScr3.Enabled = Status;
            txtWtPerc3.Enabled = Status;
            txtKeyResult3.Enabled = Status;
            txtSlfCom4.Enabled = Status;
            txtSupCom4.Enabled = Status;
            txtWtScr4.Enabled = Status;
            txtSupScr4.Enabled = Status;
            txtSlfScr4.Enabled = Status;
            txtWtPerc4.Enabled = Status;
            txtKeyResult4.Enabled = Status;
            txtSlfCom5.Enabled = Status;
            txtSupCom5.Enabled = Status;
            txtWtScr5.Enabled = Status;
            txtSupScr5.Enabled = Status;
            txtSlfScr5.Enabled = Status;
            txtWtPerc5.Enabled = Status;
            txtKeyResult5.Enabled = Status;
            txtDesig.Enabled = Status;
            txtDept.Enabled = Status;
            txtEmpCode.Enabled = Status;
            ddlBranch.Enabled = Status;
            ddlRO.Enabled = Status;
            txtKPIEndDt.Enabled = Status;
            txtKPIDt.Enabled = Status;
        }
        private void ClearControls()
        {
            txtSlfCom1.Text = "";
            txtSupCom1.Text = "";
            txtWtScr1.Text = "0";
            txtSupScr1.Text = "0";
            txtSlfScr1.Text = "0";
            txtWtPerc1.Text = "0";
            txtKeyResult1.Text = "";
            txtSlfCom2.Text = "";
            txtSupCom2.Text = "";
            txtWtScr2.Text = "0";
            txtSupScr2.Text = "0";
            txtSlfScr2.Text = "0";
            txtWtPerc2.Text = "0";
            txtKeyResult2.Text = "";
            txtSlfCom3.Text = "";
            txtSupCom3.Text = "";
            txtWtScr3.Text = "0";
            txtSupScr3.Text = "0";
            txtSlfScr3.Text = "0";
            txtWtPerc3.Text = "0";
            txtKeyResult3.Text = "";
            txtSlfCom4.Text = "";
            txtSupCom4.Text = "";
            txtWtScr4.Text = "0";
            txtSupScr4.Text = "0";
            txtSlfScr4.Text = "0";
            txtWtPerc4.Text = "0";
            txtKeyResult4.Text = "";
            txtSlfCom5.Text = "";
            txtSupCom5.Text = "";
            txtWtScr5.Text = "0";
            txtSupScr5.Text = "0";
            txtSlfScr5.Text = "0";
            txtWtPerc5.Text = "0";
            txtKeyResult5.Text = "";
            txtEmpCode.Text = "";
            txtDept.Text = "";
            txtDesig.Text = "";
            ddlRO.SelectedIndex = -1;
            ddlBranch.SelectedIndex = -1;
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Employee KPI(Upto 5 KPIs)";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuKPI);
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
                tabKPI.ActiveTabIndex = 1;
                ////tabLoanDisb.Tabs[0].Enabled = false;
                ////tabLoanDisb.Tabs[1].Enabled = true;
                ////tabLoanDisb.Tabs[2].Enabled = false;
                StatusButton("Add");
                ClearControls();
               // ViewState["Sdl"] = null;
                
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
            tabKPI.ActiveTabIndex = 0;
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
                    tabKPI.ActiveTabIndex = 0;
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
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
                ddlBranch.Enabled = false;
                ddlRO.Enabled = false;
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
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        private Boolean SaveRecords(string Mode)  //Check Account
        {

            Boolean vResult = false;
            DataTable dt = null, dtSDL = null, dtMtr = null;
            string vBranch = Session[gblValue.BrnchCode].ToString();
            string vNarationL = string.Empty;
            string vNarationF = string.Empty;
            string vXmlSdl = "", vXmlMature = "";
            int vErr = 0;
            //string vEmail = "", vBody = "", vSubject = "", vConEmailId="";
           

            int vKPIId = Convert.ToInt32(ViewState["KPIId"]);
            string vKeyResult1 = Convert.ToString(txtKeyResult1.Text.Trim() == "" ? "" : txtKeyResult1.Text.Trim());
            string vKeyResult2 = Convert.ToString(txtKeyResult2.Text.Trim() == "" ? "" : txtKeyResult2.Text.Trim());
            string vKeyResult3 = Convert.ToString(txtKeyResult3.Text.Trim() == "" ? "" : txtKeyResult3.Text.Trim());
            string vKeyResult4= Convert.ToString(txtKeyResult4.Text.Trim() == "" ? "" : txtKeyResult4.Text.Trim());
            string vKeyResult5 = Convert.ToString(txtKeyResult5.Text.Trim() == "" ? "" : txtKeyResult5.Text.Trim());

            decimal vWtPerc1 = 0, vWtPerc2 = 0, vWtPerc3 = 0, vWtPerc4 = 0, vWtPerc5 = 0;
            vWtPerc1 = Convert.ToDecimal(txtWtPerc1.Text == "" ? "0" : txtWtPerc1.Text);
            vWtPerc2 = Convert.ToDecimal(txtWtPerc2.Text == "" ? "0" : txtWtPerc2.Text);
            vWtPerc3 = Convert.ToDecimal(txtWtPerc3.Text == "" ? "0" : txtWtPerc3.Text);
            vWtPerc4 = Convert.ToDecimal(txtWtPerc4.Text == "" ? "0" : txtWtPerc4.Text);
            vWtPerc5 = Convert.ToDecimal(txtWtPerc5.Text == "" ? "0" : txtWtPerc5.Text);
            decimal vSlfSrc1 = 0, vSlfSrc2 = 0, vSlfSrc3 = 0, vSlfSrc4 = 0, vSlfSrc5 = 0;
            vSlfSrc1 = Convert.ToDecimal(txtSlfScr1.Text == "" ? "0" : txtSlfScr1.Text);
            vSlfSrc2 = Convert.ToDecimal(txtSlfScr2.Text == "" ? "0" : txtSlfScr2.Text);
            vSlfSrc3 = Convert.ToDecimal(txtSlfScr3.Text == "" ? "0" : txtSlfScr3.Text);
            vSlfSrc4 = Convert.ToDecimal(txtSlfScr4.Text == "" ? "0" : txtSlfScr4.Text);
            vSlfSrc5 = Convert.ToDecimal(txtSlfScr5.Text == "" ? "0" : txtSlfScr5.Text);
            decimal vSupScr1 = Convert.ToDecimal(txtSupScr1.Text.Trim() == "" ? "0" : txtSupScr1.Text.Trim());
            decimal vSupScr2 = Convert.ToDecimal(txtSupScr2.Text.Trim() == "" ? "0" : txtSupScr2.Text.Trim());
            decimal vSupScr3 = Convert.ToDecimal(txtSupScr3.Text.Trim() == "" ? "0" : txtSupScr3.Text.Trim());
            decimal vSupScr4 = Convert.ToDecimal(txtSupScr4.Text.Trim() == "" ? "0" : txtSupScr4.Text.Trim());
            decimal vSupScr5 = Convert.ToDecimal(txtSupScr5.Text.Trim() == "" ? "0" : txtSupScr5.Text.Trim());
            decimal vWtScr1 = Convert.ToDecimal(Request[txtWtScr1.UniqueID] as string == "0" ? txtWtScr1.Text : Request[txtWtScr1.UniqueID] as string);
            decimal vWtScr2 = Convert.ToDecimal(Request[txtWtScr2.UniqueID] as string == "0" ? txtWtScr2.Text : Request[txtWtScr2.UniqueID] as string);
            decimal vWtScr3 = Convert.ToDecimal(Request[txtWtScr3.UniqueID] as string == "0" ? txtWtScr3.Text : Request[txtWtScr3.UniqueID] as string);
            decimal vWtScr4 = Convert.ToDecimal(Request[txtWtScr4.UniqueID] as string == "0" ? txtWtScr4.Text : Request[txtWtScr4.UniqueID] as string);
            decimal vWtScr5 = Convert.ToDecimal(Request[txtWtScr5.UniqueID] as string == "0" ? txtWtScr5.Text : Request[txtWtScr5.UniqueID] as string);
            string vSlfCom1 = Convert.ToString(txtSlfCom1.Text.Trim() == "" ? "" : txtSlfCom1.Text.Trim());
            string vSlfCom2 = Convert.ToString(txtSlfCom2.Text.Trim() == "" ? "" : txtSlfCom2.Text.Trim());
            string vSlfCom3 = Convert.ToString(txtSlfCom3.Text.Trim() == "" ? "" : txtSlfCom3.Text.Trim());
            string vSlfCom4 = Convert.ToString(txtSlfCom4.Text.Trim() == "" ? "" : txtSlfCom4.Text.Trim());
            string vSlfCom5 = Convert.ToString(txtSlfCom5.Text.Trim() == "" ? "" : txtSlfCom5.Text.Trim());
            string vSupCom1 = Convert.ToString(txtSupCom1.Text.Trim() == "" ? "" : txtSupCom1.Text.Trim());
            string vSupCom2 = Convert.ToString(txtSupCom2.Text.Trim() == "" ? "" : txtSupCom2.Text.Trim());
            string vSupCom3 = Convert.ToString(txtSupCom3.Text.Trim() == "" ? "" : txtSupCom3.Text.Trim());
            string vSupCom4 = Convert.ToString(txtSupCom4.Text.Trim() == "" ? "" : txtSupCom4.Text.Trim());
            string vSupCom5 = Convert.ToString(txtSupCom5.Text.Trim() == "" ? "" : txtSupCom5.Text.Trim());
            DateTime vKPIDt = gblFuction.setDate(txtKPIDt.Text);
            DateTime vKPIEndDt = gblFuction.setDate(txtKPIEndDt.Text);
            CEmpKPI oKPI = null;


          

            if (Mode == "Save")
            {
                oKPI = new CEmpKPI();

                //vErr = oKPI.InsertEmpKPI(ref vKPIId, ddlBranch.SelectedValue, ddlRO.SelectedValue, vKPIDt, vKPIEndDt, vKeyResult1, vKeyResult2, vKeyResult3, vKeyResult4, vKeyResult5, vWtPerc1, vWtPerc2, vWtPerc3, vWtPerc4, vWtPerc5
                //            , vSlfSrc1, vSlfSrc2, vSlfSrc3, vSlfSrc4, vSlfSrc5, vSupScr1, vSupScr2, vSupScr3, vSupScr4, vSupScr5, vWtScr1, vWtScr2
                //            , vWtScr3, vWtScr4, vWtScr5, vSlfCom1, vSlfCom2, vSlfCom3, vSlfCom4, vSlfCom5, vSupCom1, vSupCom2, vSupCom3, vSupCom4, vSupCom5,
                //             this.UserID, "I");

                if (vErr > 0)
                {
                    ViewState["KPIId"] = vKPIId;
                   // txtLnNo.Text = vLoanNo;
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
                
                oKPI = new CEmpKPI();
                //vErr = oKPI.InsertEmpKPI(ref vKPIId, ddlBranch.SelectedValue, ddlRO.SelectedValue, vKPIDt, vKPIEndDt, vKeyResult1, vKeyResult2, vKeyResult3, vKeyResult4, vKeyResult5, vWtPerc1, vWtPerc2, vWtPerc3, vWtPerc4, vWtPerc5
                //            , vSlfSrc1, vSlfSrc2, vSlfSrc3, vSlfSrc4, vSlfSrc5, vSupScr1, vSupScr2, vSupScr3, vSupScr4, vSupScr5, vWtScr1, vWtScr2
                //            , vWtScr3, vWtScr4, vWtScr5, vSlfCom1, vSlfCom2, vSlfCom3, vSlfCom4, vSlfCom5, vSupCom1, vSupCom2, vSupCom3, vSupCom4, vSupCom5,
                //             this.UserID, "I");

                if (vErr == 0)
                {
                    ViewState["KPIId"] = vKPIId;
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
                oKPI = new CEmpKPI();
                //vErr = oKPI.InsertEmpKPI(ref vKPIId, ddlBranch.SelectedValue, ddlRO.SelectedValue, vKPIDt, vKPIEndDt, vKeyResult1, vKeyResult2, vKeyResult3, vKeyResult4, vKeyResult5, vWtPerc1, vWtPerc2, vWtPerc3, vWtPerc4, vWtPerc5
                //            , vSlfSrc1, vSlfSrc2, vSlfSrc3, vSlfSrc4, vSlfSrc5, vSupScr1, vSupScr2, vSupScr3, vSupScr4, vSupScr5, vWtScr1, vWtScr2
                //            , vWtScr3, vWtScr4, vWtScr5, vSlfCom1, vSlfCom2, vSlfCom3, vSlfCom4, vSlfCom5, vSupCom1, vSupCom2, vSupCom3, vSupCom4, vSupCom5,
                //             this.UserID, "I");
                
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
            //LoadGrid(0);
            return vResult;
        }
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBranch.SelectedIndex > 0)
            {
                popRO();
                //PopLoanType();
            }

        }
        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode;
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
            }
            else
            {
                vBrCode = ddlBranch.SelectedValue.ToString();
            }

            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                ddlRO.Items.Clear();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlRO.DataSource = dt;
                ddlRO.DataTextField = "EoName";
                ddlRO.DataValueField = "EoId";
                ddlRO.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlRO.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }
        protected void ddlRO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRO.SelectedIndex > 0)
                PopDetails(ddlRO.SelectedValue);
        }
        public void PopDetails(string EmpId)
        {
            string vBrCode = ddlBranch.SelectedValue;
            DataTable dt = new DataTable();
            CEmpKPI oKPI = null;
            try
            {
                oKPI = new CEmpKPI();
                dt = oKPI.GetEmpdetails(vBrCode, EmpId);
                if (dt.Rows.Count > 0)
                {
                    txtDept.Text = Convert.ToString(dt.Rows[0]["DeptName"]);
                    txtDesig.Text = Convert.ToString(dt.Rows[0]["DesignationName"]);
                    txtEmpCode.Text = Convert.ToString(dt.Rows[0]["EmpCode"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
                }
                else
                {
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Prev":
                    vPgNo = Int32.Parse(lblCrPg.Text) - 1;
                    break;
                case "Next":
                    vPgNo = Int32.Parse(lblCrPg.Text) + 1;
                    break;
            }
            LoadGrid(vPgNo);
            tabKPI.ActiveTabIndex = 0;
        }
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0;
            CEmpKPI oKPI = null;
            try
            {
                DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                oKPI = new CEmpKPI();
                dt = oKPI.GetKPIListPG(pPgIndx, vFrmDt, vToDt, ref vTotRows,this.UserID);
                gvEmpKPI.DataSource = dt;
                gvEmpKPI.DataBind();
                lblTotPg.Text = CalTotPages(vTotRows).ToString();
                lblCrPg.Text = vPgNo.ToString();
                if (vPgNo == 1)
                {
                    btnPrev.Enabled = false;
                    if (Int32.Parse(lblTotPg.Text) > 1)
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                }
                else
                {
                    btnPrev.Enabled = true;
                    if (vPgNo == Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
                }
            }
            finally
            {
                oKPI = null;
                dt = null;
            }
        }
        private int CalTotPages(double pRows)
        {
            int vPgs = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vPgs;
        }
        protected void gvEmpKPI_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vKPIId = 0;
            DataTable dt = null;
            DataTable dtDtl = null;
            DataSet ds = null;
            CEmpKPI oKPI = null;
            DataRow dr;
            try
            {
                vKPIId = Convert.ToInt32(e.CommandArgument);
                ViewState["KPIId"] = vKPIId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvEmpKPI.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oKPI = new CEmpKPI();
                    ds = oKPI.GetEmpKPIByID(vKPIId);
                    //dt = ds.Tables[0];
                    //dtDtl = ds.Tables[1];
                    //ViewState["DataTbl"] = dtDtl;
                    if (dt.Rows.Count > 0)
                    {
                        txtKPIDt.Text = Convert.ToString(dt.Rows[0]["KPIDate"]);
                        txtEmpCode.Text = Convert.ToString(dt.Rows[0]["EmpCode"]);
                        txtKPIEndDt.Text = Convert.ToString(dt.Rows[0]["KPIEndDate"]);
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                        popRO();
                        ddlRO.SelectedIndex = ddlRO.Items.IndexOf(ddlRO.Items.FindByValue(dt.Rows[0]["EmployeeId"].ToString()));
                        PopDetails(ddlRO.SelectedValue);
                        txtKeyResult1.Text = Convert.ToString(dt.Rows[0]["KeyResult1"]);
                        txtKeyResult2.Text = Convert.ToString(dt.Rows[0]["KeyResult2"]);
                        txtKeyResult3.Text = Convert.ToString(dt.Rows[0]["KeyResult3"]);
                        txtKeyResult4.Text = Convert.ToString(dt.Rows[0]["KeyResult4"]);
                        txtKeyResult5.Text = Convert.ToString(dt.Rows[0]["KeyResult5"]);
                        txtWtPerc1.Text = Convert.ToString(dt.Rows[0]["WtPerc1"]);
                        txtWtPerc2.Text = Convert.ToString(dt.Rows[0]["WtPerc2"]);
                        txtWtPerc3.Text = Convert.ToString(dt.Rows[0]["WtPerc3"]);
                        txtWtPerc4.Text = Convert.ToString(dt.Rows[0]["WtPerc4"]);
                        txtWtPerc5.Text = Convert.ToString(dt.Rows[0]["WtPerc5"]);
                        txtSlfScr1.Text = Convert.ToString(dt.Rows[0]["SelfScore1"]);
                        txtSlfScr2.Text = Convert.ToString(dt.Rows[0]["SelfScore2"]);
                        txtSlfScr3.Text = Convert.ToString(dt.Rows[0]["SelfScore3"]);
                        txtSlfScr4.Text = Convert.ToString(dt.Rows[0]["SelfScore4"]);
                        txtSlfScr5.Text = Convert.ToString(dt.Rows[0]["SelfScore5"]);
                        txtSupScr1.Text = Convert.ToString(dt.Rows[0]["SupScore1"]);
                        txtSupScr2.Text = Convert.ToString(dt.Rows[0]["SupScore2"]);
                        txtSupScr3.Text = Convert.ToString(dt.Rows[0]["SupScore3"]);
                        txtSupScr4.Text = Convert.ToString(dt.Rows[0]["SupScore4"]);
                        txtSupScr5.Text = Convert.ToString(dt.Rows[0]["SupScore5"]);
                        txtWtScr1.Text = Convert.ToString(dt.Rows[0]["WtdScore1"]);
                        txtWtScr2.Text = Convert.ToString(dt.Rows[0]["WtdScore2"]);
                        txtWtScr3.Text = Convert.ToString(dt.Rows[0]["WtdScore3"]);
                        txtWtScr4.Text = Convert.ToString(dt.Rows[0]["WtdScore4"]);
                        txtWtScr5.Text = Convert.ToString(dt.Rows[0]["WtdScore5"]);
                        txtSlfCom1.Text = Convert.ToString(dt.Rows[0]["SelfCmnts1"]);
                        txtSlfCom2.Text = Convert.ToString(dt.Rows[0]["SelfCmnts2"]);
                        txtSlfCom3.Text = Convert.ToString(dt.Rows[0]["SelfCmnts3"]);
                        txtSlfCom4.Text = Convert.ToString(dt.Rows[0]["SelfCmnts4"]);
                        txtSlfCom5.Text = Convert.ToString(dt.Rows[0]["SelfCmnts5"]);
                        txtSupCom1.Text = Convert.ToString(dt.Rows[0]["SupCmnts1"]);
                        txtSupCom2.Text = Convert.ToString(dt.Rows[0]["SupCmnts2"]);
                        txtSupCom3.Text = Convert.ToString(dt.Rows[0]["SupCmnts3"]);
                        txtSupCom4.Text = Convert.ToString(dt.Rows[0]["SupCmnts4"]);
                        txtSupCom5.Text = Convert.ToString(dt.Rows[0]["SupCmnts5"]);
                        //ddlInsp.SelectedIndex = ddlInsp.Items.IndexOf(ddlInsp.Items.FindByValue(dt.Rows[0]["EOID"].ToString()));
                        LblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        LblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tabKPI.ActiveTabIndex = 1;
                        StatusButton("Show");
                        EnableControl(false);
                    }
                    
                }
            }
            finally
            {
                dt = null;
                dtDtl = null;
                ds = null;
                oKPI = null;
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(0);
        }
    }
}