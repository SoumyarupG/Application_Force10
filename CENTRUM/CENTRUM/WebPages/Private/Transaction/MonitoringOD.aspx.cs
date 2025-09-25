using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FORCEBA;
using FORCECA;
using System.IO;
using System.Web.Security;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class MonitoringOD : CENTRUMBase
    {
        private int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                StatusButton("View");
                popBranch();
                LoadGrid(1);
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                tbEmp.ActiveTabIndex = 0;
                txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtCrStrDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtCrEndDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                calPPT.StartDate = gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate]));
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Overdue Monitoring";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuMonitoringOD);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
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
                    //                    btnCancel.Visible = false;
                    ////                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    //                    btnCancel.Visible = false;
                    //                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Monitoring Module", false);
                }

                //else
                //{
                //    btnAdd.Visible = false;
                //    btnEdit.Visible = false;
                //    btnDelete.Visible = false;
                //    btnSave.Visible = false;
                //    gblFuction.MsgPopup("Permission Not Set Properly.");
                //}
            }
            catch
            {
                Response.Redirect("~/Login.aspx?e=random", true);
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
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnDelete.Enabled = true;
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

        private void EnableControl(Boolean Status)
        {
            txtSDt.Enabled = Status;
            txtCrStrDt.Enabled = false;
            txtCrEndDt.Enabled = false;
            ddlBranch.Enabled = Status;
            gvPM.Enabled = Status;
            ddlEo.Enabled = Status;
            ddlCenter.Enabled = Status;
            ddlMember.Enabled = Status;
            chkAvalbleHome.Enabled = Status;
            chkLovisited.Enabled = Status;
            chkFraudSuspect.Enabled = Status;
            chkPayStatus.Enabled = Status;

            txtAvalbleHometRemarks.Enabled = Status;
            txtLovisitedRemarks.Enabled = Status;
            txtFraudSuspectRemarks.Enabled = Status;
            txtPayStatusRemarks.Enabled = Status;

            ddlPayStatusType.Enabled = false;
            txtPTPDate.Enabled = false;
            txtPaidAmount.Enabled = false;
        }

        private void ClearControls()
        {
            txtSDt.Text = Session[gblValue.LoginDate].ToString();
            txtCrStrDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            txtCrEndDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            //ddlBranch.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
            
            ddlEo.SelectedIndex = -1;
            ddlCenter.SelectedIndex = -1;
            ddlCenter.Items.Clear();
            ddlMember.SelectedIndex = -1;
            ddlMember.Items.Clear();

            chkAvalbleHome.Checked = false;
            chkLovisited.Checked = false;
            chkFraudSuspect.Checked = false;
            chkPayStatus.Checked = false;

            txtAvalbleHometRemarks.Text="";
            txtLovisitedRemarks.Text="";
            txtFraudSuspectRemarks.Text="";
            txtPayStatusRemarks.Text = "";

            ddlPayStatusType.SelectedIndex = -1;
            txtPTPDate.Text = "";
            txtPaidAmount.Text = "0";
        }

        private void popBranch()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");

                if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (Convert.ToString(row["BranchCode"]) != Convert.ToString(Session[gblValue.BrnchCode]))
                        {
                            row.Delete();
                        }
                    }
                    dt.AcceptChanges();
                }
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBranch.Items.Insert(0, oli);
                if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Convert.ToString(Session[gblValue.BrnchCode])));
                popRO(Convert.ToString(Session[gblValue.LoginDate]));
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popRO(string pdate)
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(pdate);
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oGb = new CGblIdGenerator();
                dt = oGb.PopTransferMIS("Y", "EoMst", "JRO,SRO,RO,BM,ABM,SBM,OPE,CO", vLogDt, vBrCode);
                ddlEo.DataSource = dt;
                ddlEo.DataTextField = "EoName";
                ddlEo.DataValueField = "EoId";
                ddlEo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlEo.Items.Insert(0, oli);


            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (this.CanAdd == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Add);
                return;
            }
            ViewState["StateEdit"] = "Add";
            tbEmp.ActiveTabIndex = 1;
            StatusButton("Add");
            ClearControls();

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
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadGrid(0);
                    ClearControls();
                    //tabPurps.ActiveTabIndex = 0;
                    tbEmp.ActiveTabIndex = 0;
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
            tbEmp.ActiveTabIndex = 1;
            ViewState["StateEdit"] = "Edit";
            StatusButton("Edit");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbEmp.ActiveTabIndex = 0;
            ClearControls();
            LoadGrid(0);
            EnableControl(false);
            StatusButton("View");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
                EnableControl(false);
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(0);
        }

        private void LoadGrid(Int32 pPgIndx)
        {

            DataTable dt = null;
            Int32 vTotRows = 0;
            CIntIspPM oAd = null;
            try
            {
                DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);

                oAd = new CIntIspPM();
                dt = oAd.Insp_GetMonitoringODPG(Session[gblValue.BrnchCode].ToString(), pPgIndx, vFrmDt, vToDt, ref vTotRows);
                gvEmp.DataSource = dt;
                gvEmp.DataBind();
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
                oAd = null;
                dt = null;
            }
        }

        private int CalTotPages(double pRows)
        {
            int vPgs = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vPgs;
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
            tbEmp.ActiveTabIndex = 0;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx", false);
        }

        private bool ValidateEntry()
        {
            bool vRst = false;
            

            if (gvPM.Rows.Count > 0)
                vRst = true;
            
            return vRst;
        }

        private DataTable DtDtlTran()
        {
            DataTable dt = new DataTable("dtDtl");
            DataRow dr;
            dt.Columns.Add(new DataColumn("Member"));
            dt.Columns.Add(new DataColumn("AvalbleHome"));
            dt.Columns.Add(new DataColumn("Lovisited"));
            dt.Columns.Add(new DataColumn("FraudSuspect"));
            dt.Columns.Add(new DataColumn("PayStatus"));
            dt.Columns.Add(new DataColumn("PayStatusType"));
            dt.Columns.Add(new DataColumn("PTPDate"));
            dt.Columns.Add(new DataColumn("PayStatusID"));
            dt.Columns.Add(new DataColumn("AvailabilityRemarks"));
            dt.Columns.Add(new DataColumn("LovisitedRemarks"));
            dt.Columns.Add(new DataColumn("FraudSuspectRemarks"));
            dt.Columns.Add(new DataColumn("PayStatusRemarks"));
            dt.Columns.Add(new DataColumn("MemberId"));
            dt.Columns.Add(new DataColumn("Center"));
            dt.Columns.Add(new DataColumn("MarketId"));
            dt.Columns.Add(new DataColumn("PaidAmount"));

            foreach (GridViewRow gr in gvPM.Rows)
            {
                
                dr = dt.NewRow();
                
                dr["Center"] = gr.Cells[0].Text;
                dr["Member"] = gr.Cells[1].Text;
                dr["AvalbleHome"] = gr.Cells[2].Text;
                dr["Lovisited"] = gr.Cells[3].Text;
                dr["FraudSuspect"] = gr.Cells[4].Text;
                dr["PayStatus"] = gr.Cells[5].Text;
                dr["PayStatusType"] = gr.Cells[6].Text.Replace("&nbsp;", "");
                dr["PTPDate"] = gr.Cells[7].Text.Replace("&nbsp;", "");
                dr["PayStatusID"] = gr.Cells[8].Text;
                dr["AvailabilityRemarks"] = gr.Cells[9].Text.Replace("&nbsp;", "");
                dr["LovisitedRemarks"] = gr.Cells[10].Text.Replace("&nbsp;", "");
                dr["FraudSuspectRemarks"] = gr.Cells[11].Text.Replace("&nbsp;", "");
                dr["PayStatusRemarks"] = gr.Cells[12].Text.Replace("&nbsp;", "");
                dr["MemberId"] = gr.Cells[13].Text;
                dr["MarketId"] = gr.Cells[14].Text;
                dr["PaidAmount"] = gr.Cells[15].Text;

                dt.Rows.Add(dr);

            }
            return dt;
        }

        protected void gvPM_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dt = new DataTable("dtDtl");
            try
            {
                if (e.CommandName == "cmdShow")
                {                    
                    DataRow dr;
                    dt.Columns.Add(new DataColumn("Member"));
                    dt.Columns.Add(new DataColumn("AvalbleHome"));
                    dt.Columns.Add(new DataColumn("Lovisited"));
                    dt.Columns.Add(new DataColumn("FraudSuspect"));
                    dt.Columns.Add(new DataColumn("PayStatus"));
                    dt.Columns.Add(new DataColumn("PayStatusType"));
                    dt.Columns.Add(new DataColumn("PTPDate"));
                    dt.Columns.Add(new DataColumn("PayStatusID"));
                    dt.Columns.Add(new DataColumn("AvailabilityRemarks"));
                    dt.Columns.Add(new DataColumn("LovisitedRemarks"));
                    dt.Columns.Add(new DataColumn("FraudSuspectRemarks"));
                    dt.Columns.Add(new DataColumn("PayStatusRemarks"));
                    dt.Columns.Add(new DataColumn("MemberId"));
                    dt.Columns.Add(new DataColumn("Center"));
                    dt.Columns.Add(new DataColumn("MarketId"));
                    dt.Columns.Add(new DataColumn("PaidAmount"));

                    foreach (GridViewRow gr in gvPM.Rows)
                    {

                        dr = dt.NewRow();

                        dr["Center"] = gr.Cells[0].Text;
                        dr["Member"] = gr.Cells[1].Text;
                        dr["AvalbleHome"] = gr.Cells[2].Text;
                        dr["Lovisited"] = gr.Cells[3].Text;
                        dr["FraudSuspect"] = gr.Cells[4].Text;
                        dr["PayStatus"] = gr.Cells[5].Text;
                        dr["PayStatusType"] = gr.Cells[6].Text.Replace("&nbsp;", "");
                        dr["PTPDate"] = gr.Cells[7].Text.Replace("&nbsp;", "");
                        dr["PayStatusID"] = gr.Cells[8].Text;
                        dr["AvailabilityRemarks"] = gr.Cells[9].Text.Replace("&nbsp;", "");
                        dr["LovisitedRemarks"] = gr.Cells[10].Text.Replace("&nbsp;", "");
                        dr["FraudSuspectRemarks"] = gr.Cells[11].Text.Replace("&nbsp;", "");
                        dr["PayStatusRemarks"] = gr.Cells[12].Text.Replace("&nbsp;", "");
                        dr["MemberId"] = gr.Cells[13].Text;
                        dr["MarketId"] = gr.Cells[14].Text;
                        dr["PaidAmount"] = gr.Cells[15].Text;

                        dt.Rows.Add(dr);

                    }
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int index = row.RowIndex;
                    if (index != gvPM.Rows.Count)
                    {
                        dt.Rows.RemoveAt(index);

                        
                    }
                    dt.AcceptChanges();
                    
                    gvPM.DataSource = dt;
                    gvPM.DataBind();
                    
                }
                
            }
            finally
            {
                dt = null;
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vXmlData = "";
            Int32 vInsPecId = 0, vErr = 0, vUserID = 0;
            double vTotalScore = 0;
            DateTime vSubDt = gblFuction.setDate(txtSDt.Text);
            DateTime vCrStrDt = gblFuction.setDate(txtCrStrDt.Text);
            DateTime vCrEndDt = gblFuction.setDate(txtCrEndDt.Text);
            CIntIspPM oAu = null;
            DataTable dt = new DataTable();
            DataTable dtDtl = new DataTable();

            try
            {
                //vUserID = Int32.Parse(Session[gblValue.UserId].ToString());
                vUserID = Convert.ToInt32(Session[gblValue.UserId]);
                //if (vCrStrDt >= vSubDt)
                //{
                //    gblFuction.MsgPopup("Carried out Start Date should be earlier date of Submission Date.");
                //    return false;
                //}
                //if (vCrEndDt > vSubDt)
                //{
                //    gblFuction.MsgPopup("Carried out End Date should be earlier date of Submission Date.");
                //    return false;
                //}
                //if (vTsp.Days < 30)
                //{
                //    gblFuction.MsgPopup("Days difference of Submission Date and Period Start Date should be atlest 30 days.");
                //    return false;
                //}

                //dt = (DataTable)ViewState["MN"];
                //if (dt == null) return false;

                if (ValidateEntry() == false)
                {
                    gblFuction.MsgPopup("At least one entry need to pass.");
                    return false;
                }

                dtDtl = DtDtlTran();
                if (dtDtl == null) return false;
                using (StringWriter oSW3 = new StringWriter())
                {
                    dtDtl.WriteXml(oSW3);
                    vXmlData = oSW3.ToString();
                }

                if (ViewState["InsPecId"] != null) vInsPecId = Convert.ToInt32(Convert.ToString(ViewState["InsPecId"]));
                if (Mode == "Save")
                {
                    oAu = new CIntIspPM();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("EOMst", "EmpCode", txtEmpCode.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Save");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("EMP Code can not be Duplicate...");
                    //    return false;
                    //}

                    vErr = oAu.Insp_SaveMonitorOD(ref vInsPecId, "MN", vSubDt, vCrStrDt, vCrEndDt, ddlBranch.SelectedValue, vXmlData, vUserID, "Save", "OD");
                    if (vErr > 0)
                    {
                        ViewState["InsPecId"] = vInsPecId;
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
                    oAu = new CIntIspPM();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("EOMst", "EMPCode", txtEmpCode.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Edit");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("EMP Code Can not be Duplicate...");
                    //    return false;
                    //}
                    vErr = oAu.Insp_SaveMonitorOD(ref vInsPecId, "MN", vSubDt, vCrStrDt, vCrEndDt, ddlBranch.SelectedValue, vXmlData, vUserID, "Edit", "OD");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oAu = new CIntIspPM();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDeleteString(vEoId, "EOID", "MarketMSt");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("The RO has group, you can not delete the RO.");
                    //    return false;
                    //}
                    vErr = oAu.Insp_SaveMonitorOD(ref vInsPecId, "MN", vSubDt, vCrStrDt, vCrEndDt, ddlBranch.SelectedValue, vXmlData, vUserID, "Del", "OD");
                    if (vErr > 0)
                    {
                        gvPM.DataSource = null;
                        gvPM.DataBind();
                        gblFuction.MsgPopup(gblMarg.DeleteMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                return vResult;
            }
            finally
            {
                oAu = null;
            }
        }

        protected void ddlEo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CIntIspPM oAd = null;
            try
            {
               
                DateTime vSDt = gblFuction.setDate(txtSDt.Text);
                ddlCenter.Items.Clear();
                ddlMember.Items.Clear();

                oAd = new CIntIspPM();
                dt = oAd.Insp_GetODCenterList(Session[gblValue.BrnchCode].ToString(), ddlEo.SelectedValue, vSDt);
                ddlCenter.DataSource = dt;
                ddlCenter.DataTextField = "Market";
                ddlCenter.DataValueField = "Marketid";
                ddlCenter.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCenter.Items.Insert(0, oli);
            }
            finally
            {
                oAd = null;
                dt = null;
            }
        }

        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CIntIspPM oAd = null;
            try
            {

                DateTime vSDt = gblFuction.setDate(txtSDt.Text);
                ddlMember.Items.Clear();

                oAd = new CIntIspPM();
                dt = oAd.Insp_GetODMemberList(Session[gblValue.BrnchCode].ToString(), ddlCenter.SelectedValue, vSDt);
                ddlMember.DataSource = dt;
                ddlMember.DataTextField = "Member";
                ddlMember.DataValueField = "MemberID";
                ddlMember.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlMember.Items.Insert(0, oli);
            }
            finally
            {
                oAd = null;
                dt = null;
            }
        }

        protected void chkPayStatus_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkPayStatus.Checked)
                ddlPayStatusType.Enabled = false;
            else
                ddlPayStatusType.Enabled = true;
            ddlPayStatusType.SelectedIndex = -1;
            txtPTPDate.Text = "";
            txtPaidAmount.Text = "0";
        }

        protected void ddlPayStatusType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPTPDate.Text = "";
            txtPaidAmount.Text = "0";
            if (ddlPayStatusType.SelectedValue == "PTP")
            {
                txtPTPDate.Enabled = true;
                
            }
            else
            {
                txtPTPDate.Enabled = false;
                
            }

            if (ddlPayStatusType.SelectedValue == "P")
            {
                txtPaidAmount.Enabled = true;
            }
            else
            {
                txtPaidAmount.Enabled = false;
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (ddlCenter.SelectedIndex <= 0)
            {
                gblFuction.AjxMsgPopup("Center Cannot be left blank.");
            }
            else if (ddlMember.SelectedIndex <= 0)
            {
                gblFuction.AjxMsgPopup("Member Cannot be left blank.");
            }
            else if (chkPayStatus.Checked == true && ddlPayStatusType.SelectedValue == "PTP" && txtPTPDate.Text=="")
            {
                gblFuction.AjxMsgPopup("PTP Date Cannot be left blank.");
            }
            else if (chkPayStatus.Checked == true && ddlPayStatusType.SelectedValue == "P" && (Convert.ToDouble(txtPaidAmount.Text)<=0 || txtPaidAmount.Text == ""))
            {
                gblFuction.AjxMsgPopup("Paid amount should be valid value.");
            }
            else if (chkPayStatus.Checked == true && ddlPayStatusType.SelectedIndex <= 0)
            {
                gblFuction.AjxMsgPopup("Payment Status Type Cannot be left blank.");
            }
            else
            {

                DataTable dt = null;
                DataRow dr;

                dt = new DataTable();
                DataColumn dc = new DataColumn();
                dc.ColumnName = "Member";
                dt.Columns.Add(dc);
                DataColumn dc1 = new DataColumn();
                dc1.ColumnName = "AvalbleHome";
                dt.Columns.Add(dc1);
                DataColumn dc2 = new DataColumn();
                dc2.ColumnName = "Lovisited";
                dt.Columns.Add(dc2);
                DataColumn dc3 = new DataColumn();
                dc3.ColumnName = "FraudSuspect";
                dt.Columns.Add(dc3);
                DataColumn dc4 = new DataColumn();
                dc4.ColumnName = "PayStatus";
                dt.Columns.Add(dc4);
                dr = dt.NewRow();
                DataColumn dc5 = new DataColumn();
                dc5.ColumnName = "PayStatusType";
                dt.Columns.Add(dc5);
                DataColumn dc6 = new DataColumn();
                dc6.ColumnName = "PTPDate";
                dt.Columns.Add(dc6);
                DataColumn dc7 = new DataColumn();
                dc7.ColumnName = "PayStatusID";
                dt.Columns.Add(dc7);
                dr = dt.NewRow();
                DataColumn dc8 = new DataColumn();
                dc8.ColumnName = "AvailabilityRemarks";
                dt.Columns.Add(dc8);
                DataColumn dc9 = new DataColumn();
                dc9.ColumnName = "LovisitedRemarks";
                dt.Columns.Add(dc9);
                //DataColumn dc10 = new DataColumn();
                //dc10.ColumnName = "LovisitedRemarks";
                //dt.Columns.Add(dc10);
                DataColumn dc11 = new DataColumn();
                dc11.ColumnName = "FraudSuspectRemarks";
                dt.Columns.Add(dc11);
                DataColumn dc12 = new DataColumn();
                dc12.ColumnName = "PayStatusRemarks";
                dt.Columns.Add(dc12);
                DataColumn dc13 = new DataColumn();
                dc13.ColumnName = "MemberId";
                dt.Columns.Add(dc13);
                DataColumn dc14 = new DataColumn();
                dc14.ColumnName = "Center";
                dt.Columns.Add(dc14);
                DataColumn dc15 = new DataColumn();
                dc15.ColumnName = "MarketId";
                dt.Columns.Add(dc15);
                DataColumn dc16 = new DataColumn();
                dc16.ColumnName = "PaidAmount";
                dt.Columns.Add(dc16);

                foreach (GridViewRow row in gvPM.Rows)
                {
                    dr = dt.NewRow();
                    dr["Center"] = row.Cells[0].Text;
                    dr["Member"] = row.Cells[1].Text;
                    dr["AvalbleHome"] = row.Cells[2].Text;
                    dr["Lovisited"] = row.Cells[3].Text;
                    dr["FraudSuspect"] = row.Cells[4].Text;
                    dr["PayStatus"] = row.Cells[5].Text;
                    dr["PayStatusType"] = row.Cells[6].Text.Replace("&nbsp;", "");
                    dr["PTPDate"] = row.Cells[7].Text.Replace("&nbsp;","");
                    dr["PayStatusID"] = row.Cells[8].Text;
                    dr["AvailabilityRemarks"] = row.Cells[9].Text.Replace("&nbsp;", "");
                    dr["LovisitedRemarks"] = row.Cells[10].Text.Replace("&nbsp;", "");
                    dr["FraudSuspectRemarks"] = row.Cells[11].Text.Replace("&nbsp;", "");
                    dr["PayStatusRemarks"] = row.Cells[12].Text.Replace("&nbsp;", "");
                    dr["MemberId"] = row.Cells[13].Text;
                    dr["MarketId"] = row.Cells[14].Text;
                    dr["PaidAmount"] = row.Cells[15].Text;
                    dt.Rows.Add(dr);
                    
                }

                dr = dt.NewRow();

                dr["Center"] = ddlCenter.SelectedItem.Text.ToString();
                dr["Member"] = ddlMember.SelectedItem.Text.ToString();

                if (chkAvalbleHome.Checked == true)
                    dr["AvalbleHome"] = "Y";
                else
                    dr["AvalbleHome"] = "N";

                if (chkLovisited.Checked == true)
                    dr["Lovisited"] = "Y";
                else
                    dr["Lovisited"] = "N";

                if (chkFraudSuspect.Checked == true)
                    dr["FraudSuspect"] = "Y";
                else
                    dr["FraudSuspect"] = "N";

                if (chkPayStatus.Checked == true)
                    dr["PayStatus"] = "Y";
                else
                    dr["PayStatus"] = "N";

                if (ddlPayStatusType.SelectedValue == "-1")
                    dr["PayStatusType"] = "";
                else
                    dr["PayStatusType"] = ddlPayStatusType.SelectedItem.Text.ToString();
                dr["PayStatusID"] = ddlPayStatusType.SelectedValue.ToString();
                dr["PTPDate"] = txtPTPDate.Text;

                dr["AvailabilityRemarks"] = txtAvalbleHometRemarks.Text;
                dr["LovisitedRemarks"] = txtLovisitedRemarks.Text;
                dr["FraudSuspectRemarks"] = txtFraudSuspectRemarks.Text;
                dr["PayStatusRemarks"] = txtPayStatusRemarks.Text;

                dr["MemberId"] = ddlMember.SelectedValue.ToString();
                dr["MarketId"] = ddlCenter.SelectedValue.ToString();
                dr["PaidAmount"] = txtPaidAmount.Text;

                dt.Rows.Add(dr);
                dt.AcceptChanges();
                gvPM.DataSource = dt;
                gvPM.DataBind();

                ddlCenter.SelectedIndex = 0;
                ddlMember.SelectedIndex = 0;

                chkAvalbleHome.Checked = false;
                chkLovisited.Checked = false;
                chkFraudSuspect.Checked = false;
                chkPayStatus.Checked = false;

                txtAvalbleHometRemarks.Text = "";
                txtLovisitedRemarks.Text = "";
                txtFraudSuspectRemarks.Text = "";
                txtPayStatusRemarks.Text = "";

                ddlPayStatusType.SelectedIndex = 0;
                txtPTPDate.Text = "";
                txtPaidAmount.Text = "0";
                ddlPayStatusType.Enabled = false;
                txtPTPDate.Enabled = false;
                txtPaidAmount.Enabled = false;
                
            }
        }

        protected void gvEmp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int pInspID = 0;
            CIntIspPM oAu = null;
            DataTable dt = null, dt1 = null;
            DataSet ds = null;
            try
            {
                pInspID = Convert.ToInt32(e.CommandArgument);
                ViewState["InsPecId"] = pInspID;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvEmp.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;

                    ClearControls();
                    oAu = new CIntIspPM();
                    ds = oAu.Insp_GetMonitorCmplODById(pInspID);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];

                    if (dt.Rows.Count > 0)
                    {
                        //txtCmpDt.Text = Convert.ToString(dt1.Rows[0]["CmplDt"]);
                        txtSDt.Text = Convert.ToString(dt.Rows[0]["SubDt"]);
                        txtCrStrDt.Text = Convert.ToString(dt.Rows[0]["CrdFrmDt"]);
                        txtCrEndDt.Text = Convert.ToString(dt.Rows[0]["CrdToDt"]);
                        popBranch();
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["Branch"].ToString()));
                        
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();

                        if (dt1.Rows.Count > 0)
                        {
                            //lblUser.Text = "Last Modified By : " + dt1.Rows[0]["UserName"].ToString();
                            //lblDate.Text = "Last Modified Date : " + dt1.Rows[0]["CreationDateTime"].ToString();
                            gvPM.DataSource = dt1;
                            gvPM.DataBind();
                            ViewState["BC"] = dt1;
                        }

                        tbEmp.ActiveTabIndex = 1;
                        StatusButton("Show");
                        EnableControl(false);
                    }
                }
            }
            finally
            {
                dt = null;
                oAu = null;
            }
        }
    }
}