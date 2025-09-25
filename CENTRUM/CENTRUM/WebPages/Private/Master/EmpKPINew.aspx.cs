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
    public partial class EmpKPINew : CENTRUMBase
    {
        protected static int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //    gblFuction.AjxMsgPopup("You Cannot open this page from branch login...");
                //else
                //    StatusButton("View");
                PopBranch(Session[gblValue.UserName].ToString());
                txtKPIDt.Text = Session[gblValue.LoginDate].ToString();
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                txtKPIStartDt.Text = Session[gblValue.LoginDate].ToString();
                txtKPIEndDt.Text = Session[gblValue.LoginDate].ToString();
                LoadDenom();
                LoadGrid(1);
                tabKPI.ActiveTabIndex = 0;
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Employee KPI";
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "KPI", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
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
            //LoadGrid(vPgNo);
            tabKPI.ActiveTabIndex = 0;
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
            txtRemarks.Enabled = Status;
            txtDesig.Enabled = Status;
            txtDept.Enabled = Status;
            txtEmpCode.Enabled = Status;
            ddlBranch.Enabled = Status;
            ddlRO.Enabled = Status;
            txtKPIEndDt.Enabled = Status;
            txtKPIStartDt.Enabled = Status;
            txtKPIDt.Enabled = Status;
            gvKPI.Enabled = Status;
        }
        private void ClearControls()
        {
            txtRemarks.Text = "";
            txtEmpCode.Text = "";
            txtDept.Text = "";
            txtDesig.Text = "";
            ddlRO.SelectedIndex = -1;
            ddlBranch.SelectedIndex = -1;
            //gvKPI.DataSource = null;
            //gvKPI.DataBind();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CEmpKPI OKPI = null;
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
                //gvKPI.DataSource = null;
                //gvKPI.DataBind();
                ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                ddlBranch.Enabled = false;
                popRO();
                OKPI = new CEmpKPI();
                dt = OKPI.GetEmpIDByUserID(this.UserID);
                if (dt.Rows.Count > 0)
                {
                    ddlRO.SelectedIndex = ddlRO.Items.IndexOf(ddlRO.Items.FindByValue(Convert.ToString(dt.Rows[0]["Eoid"])));
                }
                PopDetails(ddlRO.SelectedValue);
                LoadDenom();
                ddlRO.Enabled = false;
                // ViewState["Sdl"] = null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabKPI.ActiveTabIndex = 0;
            //tabLoanDisb.Tabs[0].Enabled = true;
            //tabLoanDisb.Tabs[1].Enabled = false;
            //tabLoanDisb.Tabs[2].Enabled = false;
            EnableControl(false);
            StatusButton("View");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                //if (SaveRecords("Delete") == true)
                //{
                //    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                //    LoadGrid(1);
                //    ClearControls();
                //    tabKPI.ActiveTabIndex = 0;
                //    StatusButton("Delete");
                //}
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
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(0);
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
                    hdnDesig.Value = Convert.ToString(dt.Rows[0]["DesignationId"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void gvKPI_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = null, dt1 = null, dt2 = null;
            CEmpKPI oGbl = null;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlQuestion = (DropDownList)e.Row.FindControl("ddlQuestion");
                    //ddlQuestion.SelectedIndex = ddlQuestion.Items.IndexOf(ddlQuestion.Items.FindByValue(e.Row.Cells[1].Text));

                    //DropDownList ddlRel = (DropDownList)e.Row.FindControl("ddlReltFam");
                    oGbl = new CEmpKPI();
                    dt = oGbl.GetQuestion(Convert.ToInt32(hdnDesig.Value == "" ? "0" : hdnDesig.Value), gblFuction.setDate(txtKPIStartDt.Text), gblFuction.setDate(txtKPIEndDt.Text));
                    ddlQuestion.DataSource = dt;
                    ViewState["Question"] = dt;
                    ddlQuestion.DataTextField = "Question";
                    ddlQuestion.DataValueField = "QuestionId";
                    ddlQuestion.DataBind();
                    ListItem oL1 = new ListItem("<-- Select -->", "-1");
                    ddlQuestion.Items.Insert(0, oL1);
                    ddlQuestion.SelectedIndex = ddlQuestion.Items.IndexOf(ddlQuestion.Items.FindByValue(e.Row.Cells[1].Text));
                    TextBox txtWtPerc = (TextBox)e.Row.FindControl("txtWtPerc");
                    TextBox txtSlfScr = (TextBox)e.Row.FindControl("txtSlfScr");
                    TextBox txtSupScr = (TextBox)e.Row.FindControl("txtSupScr");
                    TextBox txtWtScr = (TextBox)e.Row.FindControl("txtWtScr");
                    TextBox txtSlfCom = (TextBox)e.Row.FindControl("txtSlfCom");
                    TextBox txtSupCom = (TextBox)e.Row.FindControl("txtSupCom");
                    //txtWtPerc.Text = e.Row.Cells[11].Text;
                    txtSlfScr.Text = e.Row.Cells[14].Text;
                    txtSupScr.Text = e.Row.Cells[15].Text;
                    txtWtScr.Text = e.Row.Cells[16].Text;
                    txtSlfCom.Text = e.Row.Cells[17].Text;
                    txtSupCom.Text = e.Row.Cells[18].Text;
                    }
            }
            finally
            {
                dt = null;
                dt1 = null;
                dt2 = null;
                oGbl = null;
            }
        }
        protected void ddlQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Question"];
            DropDownList ddl = (DropDownList)sender;
            GridViewRow currentRow = (GridViewRow)ddl.NamingContainer;
            TextBox txtWtPerc = (TextBox)currentRow.FindControl("txtWtPerc");
            TextBox txtTarget = (TextBox)currentRow.FindControl("txtTarget");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["QuestionId"].ToString() == ddl.SelectedValue)
                {
                    txtWtPerc.Text = dt.Rows[i]["WtPerc"].ToString();
                    txtTarget.Text = dt.Rows[i]["Target"].ToString();
                }
            }
        }
        protected void gvKPI_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDel")
            {
                DataTable dt = null;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                dt = (DataTable)ViewState["KPI"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[index].Delete();
                    dt.AcceptChanges();
                    ViewState["KPI"] = dt;
                    gvKPI.DataSource = dt;
                    gvKPI.DataBind();
                }
                else
                {
                    gblFuction.MsgPopup("First Row can not be deleted.");
                    return;
                }
            }
            if (e.CommandName=="cmdAdd")
            {
                GetData();
                NewDenom(gvKPI.Rows.Count);
                
            }
        }
        private void LoadDenom()
        {
            ViewState["KPI"] = null;
            DataTable dt = new DataTable("Table1");
            dt.Columns.Add("RowId");
            dt.Columns.Add("QuestionId");
            dt.Columns.Add("WtPerc", typeof(float));
            dt.Columns.Add("Target", typeof(float));
            dt.Columns.Add("SelfScore", typeof(float));
            dt.Columns.Add("Achievement", typeof(float));
            dt.Columns.Add("SupScore", typeof(float));
            dt.Columns.Add("WtdScore", typeof(float));
            dt.Columns.Add("SelfCom");
            dt.Columns.Add("SupCom");

            dt.AcceptChanges();
            ViewState["KPI"] = dt;
            NewDenom(0);
        }
        private void NewDenom(int vRow)
        {
            DataTable dt = (DataTable)ViewState["KPI"];
            DataRow dr;
            dr = dt.NewRow();
            dt.Rows.Add();
            dt.Rows[vRow]["RowId"] = vRow + 1;
            dt.Rows[vRow]["QuestionId"] = -1;
            dt.Rows[vRow]["WtPerc"] = "0";
            dt.Rows[vRow]["Target"] = "0";
            dt.Rows[vRow]["SelfScore"] = "0";
            dt.Rows[vRow]["Achievement"] = "0";
            dt.Rows[vRow]["SupScore"] = "0";
            dt.Rows[vRow]["WtdScore"] = "0";
            dt.Rows[vRow]["SelfCom"] = " ";
            dt.Rows[vRow]["SupCom"] = "OK";


            dt.AcceptChanges();
            ViewState["KPI"] = dt;
            gvKPI.DataSource = dt;
            gvKPI.DataBind();
        }
        public void GetData()
        {
           DataTable dt = (DataTable)ViewState["KPI"];
           foreach (GridViewRow gr in gvKPI.Rows)
            {
                DropDownList ddlQuestion = (DropDownList)gvKPI.Rows[gr.RowIndex].FindControl("ddlQuestion");
                TextBox txtWtPerc = (TextBox)gvKPI.Rows[gr.RowIndex].FindControl("txtWtPerc");
                TextBox txtTarget = (TextBox)gvKPI.Rows[gr.RowIndex].FindControl("txtTarget");
                TextBox txtAchieve = (TextBox)gvKPI.Rows[gr.RowIndex].FindControl("txtAchieve");
                TextBox txtSlfScr = (TextBox)gvKPI.Rows[gr.RowIndex].FindControl("txtSlfScr");
                TextBox txtSupScr = (TextBox)gvKPI.Rows[gr.RowIndex].FindControl("txtSupScr");
                TextBox txtWtScr = (TextBox)gvKPI.Rows[gr.RowIndex].FindControl("txtWtScr");
                TextBox txtSlfCom = (TextBox)gvKPI.Rows[gr.RowIndex].FindControl("txtSlfCom");
                TextBox txtSupCom = (TextBox)gvKPI.Rows[gr.RowIndex].FindControl("txtSupCom");
                dt.Rows[gr.RowIndex]["QuestionId"] = ddlQuestion.SelectedValue;
                dt.Rows[gr.RowIndex]["WtPerc"] = Convert.ToDouble(txtWtPerc.Text == "" ? "0" : txtWtPerc.Text);
                dt.Rows[gr.RowIndex]["Target"] = Convert.ToDouble(txtTarget.Text == "" ? "0" : txtTarget.Text);
                dt.Rows[gr.RowIndex]["Achievement"] = Convert.ToDouble(txtAchieve.Text == "" ? "0" : txtAchieve.Text);
                dt.Rows[gr.RowIndex]["SelfScore"] = Convert.ToDouble(txtSlfScr.Text == "" ? "0" : txtSlfScr.Text);
                dt.Rows[gr.RowIndex]["SupScore"] = Convert.ToDouble(txtSupScr.Text == "" ? "0" : txtSupScr.Text);
                dt.Rows[gr.RowIndex]["WtdScore"] = Convert.ToDouble(txtWtScr.Text == "" ? "0" : txtWtScr.Text);
                dt.Rows[gr.RowIndex]["SelfCom"] =  txtSlfCom.Text;
                dt.Rows[gr.RowIndex]["SupCom"] =  txtSupCom.Text;
                dt.AcceptChanges();
            }
           ViewState["KPI"] = dt;
           gvKPI.DataSource = dt;
           gvKPI.DataBind();

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
            Int32 vSupervisorID1 = 0, vSupervisorID2 = 0;
           
            CLeaveApplication oCG = null;

            oCG = new CLeaveApplication();
            dtSDL = oCG.GetSupervisoUID("", this.UserID, "U",gblFuction.setDate(txtKPIDt.Text));
            if (dtSDL.Rows.Count > 0)
                vSupervisorID1 = Convert.ToInt32(dtSDL.Rows[0]["UserID"]);

            if (vSupervisorID1 > 0)
            {
                dtMtr = oCG.GetSupervisoUID("", vSupervisorID1, "U", gblFuction.setDate(txtKPIDt.Text));
                if (dtMtr.Rows.Count > 0)
                    vSupervisorID2 = Convert.ToInt32(dtMtr.Rows[0]["UserID"]);
            }

            //string vEmail = "", vBody = "", vSubject = "", vConEmailId="";


            int vKPIId = Convert.ToInt32(ViewState["KPIId"]);
            DateTime vKPIDt = gblFuction.setDate(txtKPIDt.Text);
            DateTime vKPIStDt = gblFuction.setDate(txtKPIStartDt.Text);
            DateTime vKPIEndDt = gblFuction.setDate(txtKPIEndDt.Text);
            CEmpKPI oKPI = null;
            GetData();
            dt = (DataTable)ViewState["KPI"];
            dt.TableName="Table1";
            
            using(StringWriter oSw= new StringWriter())
            {
                dt.WriteXml(oSw);
                vXmlSdl = oSw.ToString();
            }
            if (vKPIDt < vKPIEndDt)
            {
                gblFuction.AjxMsgPopup("KPI Date should not be before than KPI End Date ...");
                return false;  
            }

            if (Mode == "Save")
            {
                
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dt.Rows[i];
                    if (Convert.ToString(dr["QuestionId"]) == "-1")
                    {
                        gblFuction.AjxMsgPopup("Please select any qustion ...");
                        return false;   
                    }
                }

                oKPI = new CEmpKPI();

                vErr = oKPI.SaveKPI(ref vKPIId, ddlBranch.SelectedValue, ddlRO.SelectedValue, vKPIDt, vKPIStDt,vKPIEndDt, txtRemarks.Text, vXmlSdl,vSupervisorID1,vSupervisorID2,
                             this.UserID, "I");

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
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dt.Rows[i];
                    if (Convert.ToString(dr["QuestionId"]) == "-1")
                    {
                        gblFuction.AjxMsgPopup("Please select any qustion ...");
                        return false;
                    }
                }
                if (Convert.ToString(ViewState["IsSupperVisorApp1"]) == "Y")
                {
                    gblFuction.AjxMsgPopup("Record already edited by supervisor1. Edit not possible...");
                    return false;
                }

                oKPI = new CEmpKPI();
                vErr = oKPI.SaveKPI(ref vKPIId, ddlBranch.SelectedValue, ddlRO.SelectedValue, vKPIDt, vKPIStDt,vKPIEndDt, txtRemarks.Text, vXmlSdl,vSupervisorID1,vSupervisorID2,
                             this.UserID, "E");

                if (vErr > 0)
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
                if (Convert.ToString(ViewState["IsSupperVisorApp1"]) == "Y")
                {
                    gblFuction.AjxMsgPopup("Record already edited by supervisor. Delete not possible...");
                    return true;
                }
                oKPI = new CEmpKPI();
                vErr = oKPI.SaveKPI(ref vKPIId, ddlBranch.SelectedValue, ddlRO.SelectedValue, vKPIDt, vKPIStDt,vKPIEndDt, txtRemarks.Text, vXmlSdl,vSupervisorID1,vSupervisorID2,
                             this.UserID, "D");

                if (vErr > 0)
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
                    dt = ds.Tables[0];
                    dtDtl = ds.Tables[1];
                    //ViewState["DataTbl"] = dtDtl;
                    if (dt.Rows.Count > 0)
                    {
                        txtKPIDt.Text = Convert.ToString(dt.Rows[0]["KPIDate"]);
                        txtEmpCode.Text = Convert.ToString(dt.Rows[0]["EmpCode"]);
                        txtKPIStartDt.Text = Convert.ToString(dt.Rows[0]["KPIFromDate"]);
                        txtKPIEndDt.Text = Convert.ToString(dt.Rows[0]["KPIFromDate"]);
                        txtKPIEndDt.Text = Convert.ToString(dt.Rows[0]["KPIEndDate"]);
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                        popRO();
                        ddlRO.SelectedIndex = ddlRO.Items.IndexOf(ddlRO.Items.FindByValue(dt.Rows[0]["EmployeeId"].ToString()));
                        PopDetails(ddlRO.SelectedValue);
                        ViewState["IsSupperVisorApp1"] = Convert.ToString(dt.Rows[0]["Super1App"]);
                        //ddlInsp.SelectedIndex = ddlInsp.Items.IndexOf(ddlInsp.Items.FindByValue(dt.Rows[0]["EOID"].ToString()));
                        LblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        LblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tabKPI.ActiveTabIndex = 1;
                        StatusButton("Show");
                        EnableControl(false);
                    }
                    if (dtDtl.Rows.Count > 0)
                    {
                        gvKPI.DataSource = dtDtl;
                        gvKPI.DataBind();
                        ViewState["KPI"] = dtDtl;
                    }
                    else
                    {
                        LoadDenom();
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
        protected void GetKPIQuestion(object sender, EventArgs e)
        {
            LoadDenom();
        }
    }
}