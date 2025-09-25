using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using System.Net.Mail;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class LeaveApplication : CENTRUMBase 
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                LoadGrid(1);
                PopLeaveType();
                StatusButton("View");
                tabLoanAppl.ActiveTabIndex = 0;
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Leave Application";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLeaveAppln);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
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
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Application", false);
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
        private void EnableControl(Boolean Status)
        {
            txtAppDt.Enabled = Status;
            ddlLeave.Enabled = Status;
            txtLvFrom.Enabled = Status;
            txtLvTo.Enabled = Status;
            txtReason.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtEmpCode.Text = "";
            txtEmployee.Text = "";
            txtAppDt.Text = Session[gblValue.LoginDate].ToString();
            ddlLeave.SelectedIndex = -1;
            txtLvFrom.Text = "";
            txtLvTo.Text = "";
            txtCLLvBal.Text = "";
            txtELLvBal.Text = "";
            //txtMLLvBal.Text = "";
            txtLvDays.Text = "";
            txtReason.Text = "";
            LblDate.Text = "";
            LblUser.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CLeaveApplication oCG = null;
            Int32 vRows = 0;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oCG = new CLeaveApplication();
                dt = oCG.GetLvApplicationList(this.UserID, pPgIndx, ref vRows);
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
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vEoID"></param>
        private void LoadLeaveBalance(string vEoID)
        {
            DataTable dt = null;
            CLeaveApplication oLv = null;
            DateTime vAppDate = gblFuction.setDate(txtAppDt.Text);
            try
            {
                oLv = new CLeaveApplication();
                if (vEoID != "" && txtAppDt.Text != "")
                {
                    dt = oLv.GetEmpLeaveBalById(vEoID, vAppDate);
                    txtCLLvBal.Text = dt.Rows[0]["CL"].ToString();
                    txtELLvBal.Text = dt.Rows[0]["EL"].ToString();
                    //txtMLLvBal.Text = dt.Rows[0]["ML"].ToString();
                }
                else
                {
                    gblFuction.MsgPopup("All paramaters are not filled");
                    return;
                }
            }
            finally
            {
                dt = null;
                oLv = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
         /// <summary>
        /// 
        /// </summary>
        private void PopLeaveType()
        {
            DataTable dt = null;
            CLeaveApplication oApp = null;
            try
            {
                oApp = new CLeaveApplication();
                dt = oApp.GetLeaveType();
                ddlLeave.DataSource = dt;
                ddlLeave.DataTextField = "LeaveName";
                ddlLeave.DataValueField = "LeaveId";
                ddlLeave.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlLeave.Items.Insert(0, oLi);
            }
            finally
            {
                dt = null;
                oApp = null;
            }
        }

 
         /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLoanAppl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vAppId = "";
            DataTable dt = null;
            CLeaveApplication oCG = null;
            try
            {
                vAppId = Convert.ToString(e.CommandArgument);
                ViewState["AppId"] = vAppId;
                if (e.CommandName == "cmdShow")
                {
                    oCG = new CLeaveApplication();
                    dt = oCG.GetLvApplicationDtl(vAppId);
                    if (dt.Rows.Count > 0)
                    {
                        txtAppNo.Text = dt.Rows[0]["LeaveAppNo"].ToString();
                        txtAppDt.Text = dt.Rows[0]["AppDate"].ToString();
                        ViewState["EoId"] = dt.Rows[0]["EoId"].ToString();
                        txtEmpCode.Text = dt.Rows[0]["EmpCode"].ToString();
                        txtEmployee.Text = dt.Rows[0]["EoName"].ToString();
                        ddlLeave.SelectedIndex = ddlLeave.Items.IndexOf(ddlLeave.Items.FindByValue(dt.Rows[0]["LeaveID"].ToString()));
                        txtLvFrom.Text = dt.Rows[0]["FromDate"].ToString();
                        txtLvTo.Text = dt.Rows[0]["ToDate"].ToString();
                        txtLvDays.Text = dt.Rows[0]["LeaveDays"].ToString();
                        txtReason.Text = dt.Rows[0]["Reason"].ToString();
                        LblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        LblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tabLoanAppl.ActiveTabIndex = 1;
                        StatusButton("Show");
                        LoadLeaveBalance(dt.Rows[0]["EoId"].ToString());
                    }
                }
            }
            finally
            {
                dt = null;
                oCG = null;
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
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CLeaveApplication OApp = null;
            string vEoID = "";
            try
            {

                if (this.CanAdd == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                OApp = new CLeaveApplication();
                dt = OApp.GetEmployeeByUserId(this.UserID);
                if (dt.Rows.Count > 0)
                {
                    StatusButton("Add");
                    ClearControls();
                    txtEmpCode.Text = dt.Rows[0]["EmpCode"].ToString();
                    txtEmployee.Text = dt.Rows[0]["EoName"].ToString();
                    vEoID = dt.Rows[0]["EoId"].ToString();
                    ViewState["EoId"] = vEoID ;
                    ViewState["StateEdit"] = "Add";
                    tabLoanAppl.ActiveTabIndex = 1;
                    txtAppDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                    LoadLeaveBalance(vEoID);
                }
                else
                {
                    gblFuction.MsgPopup("User Is not Employee");
                    return;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabLoanAppl.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
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
                    cPgNo = Int32.Parse(lblTotalPages.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }
            LoadGrid(cPgNo);
            tabLoanAppl.ActiveTabIndex = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            DataTable dt = null, dtCnt=null;
            string vStateEdit = string.Empty, vBrCode = string.Empty, vAppNo = string.Empty;
            Int32 vErr = 0, vLeaveId = 0, vFrdUId=0, vCntDays=0;
            string vAppId = "", vEOId = "", vEmail="", vBody="", vSubject="";
            double vLvDays = 0;
            double vClBal = 0;
            double vElBal = 0;
            double vMlBal = 0;
            double vDays = 0;
            DateTime vAppDt = gblFuction.setDate(txtAppDt.Text);
            DateTime vLvFrDt = gblFuction.setDate(txtLvFrom.Text);
            DateTime vLvToDt = gblFuction.setDate(txtLvTo.Text);
            vDays = (vLvFrDt - vLvToDt).TotalDays;
            CLeaveApplication oCG = null;
            vSubject = "Leave Application From " + txtEmployee.Text;
            vBody = "Code:" + txtEmpCode.Text + " Name: " + txtEmployee.Text + " Applied for " + ddlLeave.SelectedItem.Text + " From: " + txtLvFrom.Text + " To " + txtLvTo.Text;
            try
            {
                vEOId = Convert.ToString(ViewState["EoId"]);
                vLeaveId = Convert.ToInt32(ddlLeave.SelectedValue);


                if (txtLvDays.Text == "")
                {
                    gblFuction.MsgPopup("Leave Days cannot be blank...");
                    return false;
                }
                vLvDays = Convert.ToDouble(txtLvDays.Text);
                vClBal = Convert.ToDouble(txtCLLvBal.Text);
                vElBal = Convert.ToDouble(txtELLvBal.Text);
                //vMlBal = Convert.ToDouble(txtMLLvBal.Text);

                vBrCode = (string)Session[gblValue.BrnchCode];
                vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                vAppId = Convert.ToString(ViewState["AppId"]);
                vDays = Convert.ToInt32(txtLvDays.Text);
                oCG = new CLeaveApplication();
                if(ddlLeave.SelectedValue=="1")
                {
                    if (vDays  > 3)
                    {
                        gblFuction.AjxMsgPopup("You cannot take More than 3 CL at a time...");
                        return false;
                    }
                }
                 if(ddlLeave.SelectedValue=="2")
                {
                    if (vDays > 10)
                    {
                        gblFuction.AjxMsgPopup("You cannot take More than 10 EL at a time...");
                        return false;
                    }
                }
                 dt = oCG.GetSupervisoUID(vEOId, this.UserID, "U", gblFuction.setDate(txtAppDt.Text));
                if (dt.Rows.Count > 0)
                    vFrdUId = Convert.ToInt32(dt.Rows[0]["UserID"]);

                dt = oCG.GetSupervisorEmail(vEOId, this.UserID, "U", gblFuction.setDate(txtAppDt.Text));
                if (dt.Rows.Count > 0)
                    vEmail= dt.Rows[0]["email"].ToString();
                //if (ddlLeave.SelectedValue== "1")
                //{
                //    if (vClBal < vLvDays)
                //    {
                //        gblFuction.MsgPopup("CL Balance is not Enough...");
                //        return false;
                //    }
                //}
                //if (ddlLeave.SelectedValue == "2")
                //{
                //    if (vElBal < vLvDays)
                //    {
                //        gblFuction.MsgPopup("EL Balance is not Enough...");
                //        return false;
                //    }
                //}
                //if (ddlLeave.SelectedItem.Text == "MEDICAL LEAVE")
                //{
                //    if (vMlBal < vLvDays)
                //    {
                //        gblFuction.MsgPopup("ML Balance is not Enough...");
                //        return false;
                //    }
                //}


                if (Mode == "Save")
                {
                    if (ValidateFields() == false) return false;
                    oCG = new CLeaveApplication();
                    vErr = oCG.InsertLvApplication(ref vAppNo, vAppDt, vEOId, vLvFrDt, vLvToDt, vLeaveId, vLvDays,
                                                    txtReason.Text.Replace("'", "''"), vFrdUId, vBrCode, this.UserID, "I", vClBal, vElBal);
                    if (vErr == 0)
                    {
                        ViewState["AppId"] = vAppId;
                        txtAppNo.Text = vAppNo;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    
                    if (ValidateFields() == false) return false;

                    oCG = new CLeaveApplication();

                    vErr = oCG.UpdateLvApplication(vAppId, vAppDt, vEOId, vLvFrDt, vLvToDt, vLeaveId, vLvDays,
                            txtReason.Text.Replace("'", "''"), vFrdUId, vBrCode, this.UserID, "Edit", vClBal, vElBal);//, "A");
                    if (vErr == 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oCG = new CLeaveApplication();
                    vErr = oCG.UpdateLvApplication(vAppId, vAppDt, vEOId, vLvFrDt, vLvToDt, vLeaveId, vLvDays,
                            txtReason.Text.Replace("'", "''"), vFrdUId, vBrCode, this.UserID, "Delet", vClBal, vElBal);//, "A");
                    if (vErr == 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                SendToMail(vEmail,vBody,vSubject);
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCG = null;
                dt = null;
            }
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
                //CEMail oEm = new CEMail();
                //oEm.SaveEMailStat(pReqNo, vBody, vMTo, "", "ebankhelpdesk@gmail.com", "Internal Request Service No:" + pReqNo, "Save");
                //gblFuction.MsgPopup("Not able to Send Email.......");  
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CApplication oCG = oCG = new CApplication();

            if (txtAppDt.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Leave Application Date cannot be empty.");
                gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtAppDt");
                vResult = false;
            }
            else
            {
                DateTime vAppDate = gblFuction.setDate(txtAppDt.Text);
                if (vAppDate < vFinFrom || vAppDate > vFinTo)
                {
                    EnableControl(true);
                    gblFuction.MsgPopup("Leave Application Date should be Financial Year.");
                    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtAppDt");
                    vResult = false;
                }
                if (vAppDate > vLoginDt)
                {
                    EnableControl(true);
                    gblFuction.MsgPopup("Leave Application Date should not be greater than login date.");
                    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtAppDt");
                    vResult = false;
                }
            }

            if (txtLvFrom.Text == "" || gblFuction.IsDate(txtLvFrom.Text) == false)
            {
                gblFuction.AjxMsgPopup("Leave From date cannot be blank");
                gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtLvFrom");
                vResult = false;
            }

            if (txtLvTo.Text == "" || gblFuction.IsDate(txtLvTo.Text) == false)
            {
                gblFuction.AjxMsgPopup("Leave To date cannot be blank");
                gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtLvTo");
                vResult = false;
            }

            if (gblFuction.setDate(txtLvFrom.Text) > gblFuction.setDate(txtLvTo.Text))
            {
                gblFuction.AjxMsgPopup("From Date Cannot be greater than To date");
                gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtLvTo");
                vResult = false;
            }
            //if (gblFuction.setDate(txtAppDt.Text) >= gblFuction.setDate(txtLvFrom.Text))
            //{
            //    gblFuction.AjxMsgPopup("Application Date Cannot be greater than Leave date");
            //    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtAppDt");
            //    vResult = false;
            //}

            if (ddlLeave.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.MsgPopup("Please select Leave Type...");
                gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_ddlLeave");
                vResult = false;
            }
            if (txtLvDays.Text.Trim() == "" || txtLvDays.Text.Trim() == "0")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Leave Days should be grater than Zero.");
                gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtLvDays");
                vResult = false;
            }
            return vResult;
        }

        protected void txtLvFrom_TextChanged(object sender, EventArgs e)
        {
            int result = 0, vCntDays = 0;
            DataTable dtCnt = new DataTable();
            CLeaveApplication oCG = null;
            if (txtLvFrom.Text != "" && txtLvTo.Text != "")
            {
                oCG = new CLeaveApplication();
                DateTime vfromDt = gblFuction.setDate(txtLvFrom.Text);
                DateTime vtoDt = gblFuction.setDate(txtLvTo.Text);
                if (ddlLeave.SelectedValue == "1")
                {
                    dtCnt = oCG.GetHolidayCountCL(vfromDt, vtoDt, Session[gblValue.BrnchCode].ToString());
                    vCntDays = Convert.ToInt32(dtCnt.Rows[0]["Holiday"].ToString());
                }
                if (vfromDt > vtoDt)
                {
                    gblFuction.AjxMsgPopup("From Date cannot be greater than To Date");
                    txtLvFrom.Text = "";
                    return;
                }
                //txtLvDays.Text = Convert.ToString((vtoDt.Day - vfromDt.Day)+1);
                //DateTime vdiff = gblFuction.setDate(txtLvDays.Text);
                //vdiff = vtoDt - vfromDt;
                result = (int)((vtoDt - vfromDt).TotalDays);
                txtLvDays.Text = Convert.ToString(result - vCntDays+1);
            }
            
        }

        protected void txtLvTo_TextChanged(object sender, EventArgs e)
        {
            int result = 0, vCntDays=0;
            DataTable dtCnt = new DataTable();
            CLeaveApplication oCG = null;
            if (txtLvFrom.Text != "" && txtLvTo.Text != "")
            {
                oCG = new CLeaveApplication();
                DateTime vfromDt = gblFuction.setDate(txtLvFrom.Text);
                DateTime vtoDt = gblFuction.setDate(txtLvTo.Text);
                if (ddlLeave.SelectedValue == "1")
                {

                    dtCnt = oCG.GetHolidayCountCL(vfromDt, vtoDt, Session[gblValue.BrnchCode].ToString());
                    vCntDays = Convert.ToInt32(dtCnt.Rows[0]["Holiday"].ToString());
                }
                if (vfromDt > vtoDt)
                {
                    gblFuction.AjxMsgPopup("From Date cannot be greater than To Date");
                    txtLvTo.Text = "";
                    return;
                }
                //txtLvDays.Text = Convert.ToString((vtoDt.Day - vfromDt.Day) + 1);
                result = (int)((vtoDt - vfromDt).TotalDays);
                txtLvDays.Text = Convert.ToString(result + 1 - vCntDays);
            }
        }

    }
}
