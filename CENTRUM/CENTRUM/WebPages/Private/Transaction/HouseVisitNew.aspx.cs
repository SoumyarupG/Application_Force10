using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.Configuration;
using System.Net;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Xml;


namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class HouseVisitNew : CENTRUMBase
    {
        protected int vPgNo = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //    StatusButton("Exit");
                //else
                StatusButton("View");
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                LoadGrid(1);
                popRO();
                popHvBy();
                tbHVisit.ActiveTabIndex = 0;
                GetHvQA();
            }

        }

        private void GetHvQA()
        {
            DataTable dt = new DataTable();
            CHouseVisit cHv = new CHouseVisit();
            dt = cHv.GetHvQA();
            popAns(ddlQ1, dt, 1);
            popAns(ddlQ2, dt, 2);
            popAns(ddlQ3, dt, 3);
            popAns(ddlQ4, dt, 4);
            popAns(ddlQ5, dt, 5);
            popAns(ddlQ6, dt, 6);
            popAns(ddlQ7, dt, 7);
            popAns(ddlQ8, dt, 8);
            popAns(ddlQ10, dt, 10);
            popAns(ddlQ11, dt, 11);
            popAns(ddlQ12, dt, 12);
            popAns(ddlQ13, dt, 13);
            popAns(ddlQ14, dt, 14);
            popAns(ddlQ15, dt, 15);
           
        }

        private void popAns(DropDownList ddl,DataTable dt,int Qno)
        {
            DataTable dt1 = new DataTable();
            dt1 = dt.Select("Qno = " + Qno ).CopyToDataTable();
            if (dt1.Rows.Count > 0)
            {
                ddl.DataSource = dt1;
                ddl.DataTextField = "Ans";
                ddl.DataValueField = "AnsNo";
                ddl.DataBind();
            }
            ListItem oLc = new ListItem("<--Select-->", "-1");
            ddl.Items.Insert(0, oLc);
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "House Visit";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHVisit);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 3) // role id 3 is for RO
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "GRT", false);
                else
                {
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
                    }
                    else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                    {
                        btnDelete.Visible = false;
                    }
                    else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                    {
                    }
                    else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    {
                        Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "House Visit", false);
                    }
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void EnableControl(Boolean Status)
        {
            ddlRo.Enabled = Status;
            ddlCentr.Enabled = Status;
            ddlGrup.Enabled = Status;
            ddlMem.Enabled = Status;
            ddlVisitBy.Enabled = Status;
            txtVisitDt.Enabled = Status;
            ddlQ1.Enabled = Status;
            ddlQ2.Enabled = Status;
            ddlQ3.Enabled = Status;
            ddlQ4.Enabled = Status;
            ddlQ5.Enabled = Status;
            ddlQ6.Enabled = Status;
            ddlQ7.Enabled = Status;
            ddlQ8.Enabled = Status;
            chkQ9.Enabled = Status;
            ddlQ10.Enabled = Status;
            ddlQ11.Enabled = Status;
            ddlQ12.Enabled = Status;
            txtExGDt.Enabled = Status;
            ddlQ13.Enabled = Status;
            ddlQ14.Enabled = Status;

            txtQ1Score.Enabled = false;
            txtQ2Score.Enabled = false;
            txtQ3Score.Enabled = false;
            txtQ4Score.Enabled = false;
            txtQ5Score.Enabled = false;
            txtQ6Score.Enabled = false;
            txtQ7Score.Enabled = false;
            txtQ8Score.Enabled = false;
            txtQ9Score.Enabled = false;
            txtQ10Score.Enabled = false;
            txtQ11Score.Enabled = false;
            txtQ12Score.Enabled = false;
            txtQ13Score.Enabled = false;
            txtQ14Score.Enabled = false;
            txtQ15Score.Enabled = false;

            txtQ1Weight.Enabled = false;
            txtQ2Weight.Enabled = false;
            txtQ3Weight.Enabled = false;
            txtQ4Weight.Enabled = false;
            txtQ5Weight.Enabled = false;
            txtQ6Weight.Enabled = false;
            txtQ7Weight.Enabled = false;
            txtQ8Weight.Enabled = false;
            txtQ9Weight.Enabled = false;
            txtQ10Weight.Enabled = false;
            txtQ11Weight.Enabled = false;
            txtQ12Weight.Enabled = false;
            txtQ13Weight.Enabled = false;
            txtQ14Weight.Enabled = false;
            txtQ15Weight.Enabled = false;

        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            ddlRo.SelectedIndex = -1;
            ddlCentr.SelectedIndex = -1;
            ddlGrup.SelectedIndex = -1;
            ddlMem.SelectedIndex = -1;
            ddlVisitBy.SelectedIndex = -1;
            txtVisitDt.Text = Session[gblValue.LoginDate].ToString();
            //txtLnCycl.Text="0";
            ddlQ1.SelectedIndex = -1;
            ddlQ2.SelectedIndex = -1;
            ddlQ3.SelectedIndex = -1;
            ddlQ4.SelectedIndex = -1;
            ddlQ5.SelectedIndex = -1;
            ddlQ6.SelectedIndex = -1;
            ddlQ7.SelectedIndex = -1;
            ddlQ8.SelectedIndex = -1;
            chkQ9.ClearSelection();
            ddlQ10.SelectedIndex = -1;
            ddlQ11.SelectedIndex = -1;
            ddlQ12.SelectedIndex = -1;
            txtExGDt.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
            ddlQ14.SelectedIndex = -1;
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }

        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0; ;
            string vBrCode = "";
            CHouseVisit oHv = null;
            try
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oHv = new CHouseVisit();
                dt = oHv.GetHVMasterPG(gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), vBrCode, pPgIndx, ref vTotRows);
                gvHVisit.DataSource = dt;
                gvHVisit.DataBind();
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
                oHv = null;
                dt.Dispose();
            }
        }

        private int CalTotPages(double pRows)
        {
            int vPgs = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vPgs;
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
                    ClearControls();
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
            }
        }

        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlRo.DataSource = dt;
                ddlRo.DataTextField = "EoName";
                ddlRo.DataValueField = "EoId";
                ddlRo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlRo.Items.Insert(0, oli);

            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        private void popHvBy()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oGb = new CGblIdGenerator();
                dt = oGb.PopTransferMIS("N", "EoMst", "ABM,BM,AM,SBM,AAM,ARM,RM,RO,CO", vLogDt, vBrCode);
                ddlVisitBy.DataSource = dt;
                ddlVisitBy.DataTextField = "EoName";
                ddlVisitBy.DataValueField = "EoId";
                ddlVisitBy.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlVisitBy.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void PopCenter(string pEoId)
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("S", "N", "AA", "MarketID", "Market", "MarketMSt", pEoId, "EoId", "AA", gblFuction.setDate("01/01/1900"), "0000");
                if (dt.Rows.Count > 0)
                {
                    ddlCentr.DataSource = dt;
                    ddlCentr.DataTextField = "Market";
                    ddlCentr.DataValueField = "MarketID";
                    ddlCentr.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlCentr.Items.Insert(0, oli);
                }

            }
            finally
            { }
        }

        private void popGroup(string vCenter)
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("D", "N", "AA", "Groupid", "GroupName", "GroupMSt", vCenter, "MarketID", "Tra_DropDate", vLogDt, vBrCode);
                ddlGrup.DataSource = dt;
                ddlGrup.DataTextField = "GroupName";
                ddlGrup.DataValueField = "Groupid";
                ddlGrup.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlGrup.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void PopMember(string vGrpId, string vMode)
        {
            DataTable dt = null;
            string vBrCode = "";
            CHouseVisit oHv = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oHv = new CHouseVisit();
                dt = oHv.GetHVMember(vBrCode, vGrpId, vLogDt, vMode);
                if (dt.Rows.Count > 0)
                {
                    ViewState["CGTID"] = dt;

                    ddlMem.DataSource = dt;
                    ddlMem.DataTextField = "Member";
                    ddlMem.DataValueField = "MemberID";
                    ddlMem.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlMem.Items.Insert(0, oli);
                }
                else
                {
                    ddlMem.Items.Clear();
                    ListItem oli = new ListItem("No More CGT Done", "-1");
                    ddlMem.Items.Insert(0, oli);
                }

            }
            finally
            {
                oHv = null;
                dt = null;
            }
        }

        protected void ddlRo_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopCenter(ddlRo.SelectedValue);
            //ddlCentr.SelectedIndex = ddlCentr.Items.IndexOf(ddlCentr.Items.FindByValue(ddlRo.SelectedValue));
            //popGroup(ddlCentr.SelectedValue);
        }

        protected void ddlCentr_SelectedIndexChanged(object sender, EventArgs e)
        {
            popGroup(ddlCentr.SelectedValue);
        }

        protected void chkQ9_SelectedIndexChanged(object sender, EventArgs e)
        {
            String vQ9AnsId = "";
            CHouseVisit cHv = new CHouseVisit();
            DataTable dt = new DataTable();
            foreach (ListItem item in chkQ9.Items)
            {
                if (item.Selected == true)
                {
                    if (vQ9AnsId == "")
                    {
                        vQ9AnsId = item.Value.ToString();
                    }
                    else
                    {
                        vQ9AnsId = vQ9AnsId + ',' + item.Value.ToString();
                    }
                }
            }

            dt = cHv.GetQnAnsDtlById(Convert.ToInt32(hdnQ9.Value), vQ9AnsId);

            if (dt.Rows.Count > 0)
            {
                txtQ9Score.Text = dt.Rows[0]["Score"].ToString();
                txtQ9Weight.Text = dt.Rows[0]["Weighted"].ToString();
            }
        }

        protected void ddlGrup_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopMember(ddlGrup.SelectedValue, "A");
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
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
                tbHVisit.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                    LoadGrid(1);
                    StatusButton("Delete");
                    tbHVisit.ActiveTabIndex = 0;
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbHVisit.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }

        protected void btnShwPotenMem_Click(object sender, EventArgs e)
        {
            string vUrl = hdnProtenUrl.Value;
            string url = vUrl + "BIJLI";
            string s = "window.open('" + url + "', '_blank', 'width=900,height=600,left=100,top=100,resizable=yes');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            return;
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
                StatusButton("Show");
                ViewState["StateEdit"] = null;
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
            tbHVisit.ActiveTabIndex = 0;
        }

        protected void gvHVisit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int vHVId = 0;
            DataTable dt = null;
            CHouseVisit oHv = null;
            try
            {
                vHVId = Convert.ToInt32(e.CommandArgument);
                ViewState["CGTBYID"] = vHVId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvHVisit.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oHv = new CHouseVisit();
                    dt = oHv.GetHouseVisitById_New(vHVId);
                    if (dt.Rows.Count > 0)
                    {
                        ddlRo.SelectedIndex = ddlRo.Items.IndexOf(ddlRo.Items.FindByValue(dt.Rows[0]["EOID"].ToString()));
                        PopCenter(ddlRo.SelectedValue);
                        ddlCentr.SelectedIndex = ddlCentr.Items.IndexOf(ddlCentr.Items.FindByValue(dt.Rows[0]["MarketID"].ToString()));
                        popGroup(ddlCentr.SelectedValue);
                        ddlGrup.SelectedIndex = ddlGrup.Items.IndexOf(ddlGrup.Items.FindByValue(dt.Rows[0]["Groupid"].ToString()));
                        PopMember(ddlGrup.SelectedValue, "E");
                        ddlMem.SelectedIndex = ddlMem.Items.IndexOf(ddlMem.Items.FindByValue(dt.Rows[0]["MemberID"].ToString()));
                        ddlVisitBy.SelectedIndex = ddlVisitBy.Items.IndexOf(ddlVisitBy.Items.FindByValue(dt.Rows[0]["GRTBy"].ToString()));
                        txtVisitDt.Text = Convert.ToString(dt.Rows[0]["HVDate"]);
                        txtExGDt.Text = Convert.ToString(dt.Rows[0]["EXPGRTdt"]);

                        ddlQ1.SelectedIndex = ddlQ1.Items.IndexOf(ddlQ1.Items.FindByValue(dt.Rows[0]["Q1A"].ToString()));
                        txtQ1Score.Text = Convert.ToString(dt.Rows[0]["Q1Score"]);
                        txtQ1Weight.Text = Convert.ToString(dt.Rows[0]["Q1Weighted"]);

                        ddlQ2.SelectedIndex = ddlQ2.Items.IndexOf(ddlQ2.Items.FindByValue(dt.Rows[0]["Q2A"].ToString()));
                        txtQ2Score.Text = Convert.ToString(dt.Rows[0]["Q2Score"]);
                        txtQ2Weight.Text = Convert.ToString(dt.Rows[0]["Q2Weighted"]);

                        ddlQ3.SelectedIndex = ddlQ3.Items.IndexOf(ddlQ3.Items.FindByValue(dt.Rows[0]["Q3A"].ToString()));
                        txtQ3Score.Text = Convert.ToString(dt.Rows[0]["Q3Score"]);
                        txtQ3Weight.Text = Convert.ToString(dt.Rows[0]["Q3Weighted"]);

                        ddlQ4.SelectedIndex = ddlQ4.Items.IndexOf(ddlQ4.Items.FindByValue(dt.Rows[0]["Q4A"].ToString()));
                        txtQ4Score.Text = Convert.ToString(dt.Rows[0]["Q4Score"]);
                        txtQ4Weight.Text = Convert.ToString(dt.Rows[0]["Q4Weighted"]);

                        ddlQ5.SelectedIndex = ddlQ5.Items.IndexOf(ddlQ5.Items.FindByValue(dt.Rows[0]["Q5A"].ToString()));
                        txtQ5Score.Text = Convert.ToString(dt.Rows[0]["Q5Score"]);
                        txtQ5Weight.Text = Convert.ToString(dt.Rows[0]["Q5Weighted"]);

                        ddlQ6.SelectedIndex = ddlQ6.Items.IndexOf(ddlQ6.Items.FindByValue(dt.Rows[0]["Q6A"].ToString()));
                        txtQ6Score.Text = Convert.ToString(dt.Rows[0]["Q6Score"]);
                        txtQ6Weight.Text = Convert.ToString(dt.Rows[0]["Q6Weighted"]);

                        ddlQ7.SelectedIndex = ddlQ7.Items.IndexOf(ddlQ7.Items.FindByValue(dt.Rows[0]["Q7A"].ToString()));
                        txtQ7Score.Text = Convert.ToString(dt.Rows[0]["Q7Score"]);
                        txtQ7Weight.Text = Convert.ToString(dt.Rows[0]["Q7Weighted"]);

                        ddlQ8.SelectedIndex = ddlQ8.Items.IndexOf(ddlQ8.Items.FindByValue(dt.Rows[0]["Q8A"].ToString()));
                        txtQ8Score.Text = Convert.ToString(dt.Rows[0]["Q8Score"]);
                        txtQ8Weight.Text = Convert.ToString(dt.Rows[0]["Q8Weighted"]);

                        ddlQ10.SelectedIndex = ddlQ10.Items.IndexOf(ddlQ10.Items.FindByValue(dt.Rows[0]["Q10A"].ToString()));
                        txtQ10Score.Text = Convert.ToString(dt.Rows[0]["Q10Score"]);
                        txtQ10Weight.Text = Convert.ToString(dt.Rows[0]["Q10Weighted"]);

                        ddlQ11.SelectedIndex = ddlQ11.Items.IndexOf(ddlQ11.Items.FindByValue(dt.Rows[0]["Q11A"].ToString()));
                        txtQ11Score.Text = Convert.ToString(dt.Rows[0]["Q11Score"]);
                        txtQ11Weight.Text = Convert.ToString(dt.Rows[0]["Q11Weighted"]);

                        ddlQ12.SelectedIndex = ddlQ12.Items.IndexOf(ddlQ12.Items.FindByValue(dt.Rows[0]["Q12A"].ToString()));
                        txtQ12Score.Text = Convert.ToString(dt.Rows[0]["Q12Score"]);
                        txtQ12Weight.Text = Convert.ToString(dt.Rows[0]["Q12Weighted"]);

                        ddlQ13.SelectedIndex = ddlQ13.Items.IndexOf(ddlQ13.Items.FindByValue(dt.Rows[0]["Q13A"].ToString()));
                        txtQ13Score.Text = Convert.ToString(dt.Rows[0]["Q13Score"]);
                        txtQ13Weight.Text = Convert.ToString(dt.Rows[0]["Q13Weighted"]);

                        ddlQ14.SelectedIndex = ddlQ14.Items.IndexOf(ddlQ14.Items.FindByValue(dt.Rows[0]["Q14A"].ToString()));
                        txtQ14Score.Text = Convert.ToString(dt.Rows[0]["Q14Score"]);
                        txtQ14Weight.Text = Convert.ToString(dt.Rows[0]["Q14Weighted"]);

                        ddlQ15.SelectedIndex = ddlQ15.Items.IndexOf(ddlQ15.Items.FindByValue(dt.Rows[0]["Q15A"].ToString()));
                        txtQ15Score.Text = Convert.ToString(dt.Rows[0]["Q15Score"]);
                        txtQ15Weight.Text = Convert.ToString(dt.Rows[0]["Q15Weighted"]);

                        List<string> vli = Convert.ToString(dt.Rows[0]["Q9A"]).Split(',').ToList<string>();
                        foreach (string vItem in vli)
                        {
                            for (int iLoop = 0; iLoop < chkQ9.Items.Count; iLoop++)
                            {
                                if (chkQ9.Items[iLoop].Value.ToString() == vItem)
                                {
                                    chkQ9.Items[iLoop].Selected = true;
                                }
                            }
                        }
                        txtQ9Score.Text = Convert.ToString(dt.Rows[0]["Q9Score"]);
                        txtQ9Weight.Text = Convert.ToString(dt.Rows[0]["Q9Weighted"]);

                        bool vViewPotenMem = false;
                        CRole oRl = new CRole();
                        DataTable dt1 = new DataTable();
                        dt1 = oRl.GetRoleById(Convert.ToInt32(Session[gblValue.RoleId].ToString()));
                        if (dt1.Rows.Count > 0)
                        {
                            vViewPotenMem = Convert.ToString(dt.Rows[0]["ShowPotential"]) == "Y" && Convert.ToString(dt1.Rows[0]["PotenMemYN"]) == "Y" ? true : false;
                        }
                        btnShwPotenMem.Visible = vViewPotenMem;
                        btnUpdateUcic.Visible = vViewPotenMem;
                        hdnProtenUrl.Value = (Convert.ToString(dt.Rows[0]["PotenURL"]));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbHVisit.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oHv = null;
            }
        }

        private bool ValidateFields()
        {
            bool vRes = false;
            //string pathImage = ConfigurationManager.AppSettings["PathImage"];
            //string vUrl = pathImage + "Member/";
            //WebRequest request = WebRequest.Create(vUrl + ddlMem.SelectedValue + "/PassbookImage.png");
            //try
            //{
            //    request.GetResponse();
            //     vRes = true;
            //}
            //catch (Exception ex)
            //{
            //    gblFuction.AjxMsgPopup("Please Upload Passbook Image..!");
            //    vRes = false;
            //}
            string vUrl = ConfigurationManager.AppSettings["pathMember"];
            bool fileExists = (File.Exists(vUrl + ddlMem.SelectedValue + "/PassbookImage.png") ? true : false);
            if (fileExists == true)
            {
                vRes = true;
            }
            else
            {
                gblFuction.AjxMsgPopup("Please Upload Passbook Image..!");
                vRes = false;
            }

            //if (gblFuction.IsEmail(txtEmail.Text.Trim()) == false)
            //{
            //    gblFuction.MsgPopup("Please Enter a Valid Email");
            //    return vRes = false;
            //}
            //if (gblFuction.IsDate(txtOpDt.Text.Trim()) == false)
            //{
            //    gblFuction.MsgPopup("Opening Date is not in DD/MM/YYYY");
            //    return vRes = false;
            //}
            //if (gblFuction.IsDate(txtAgDt.Text.Trim()) == false)
            //{
            //    gblFuction.MsgPopup("Agre Date is not in DD/MM/YYYY");
            //    return vRes = false;
            //}
            //if (gblFuction.IsDate(txtToDt.Text.Trim()) == false)
            //{
            //    gblFuction.MsgPopup("Valid Till is not in DD/MM/YYYY");
            //    return vRes = false;
            //}
            //if (gblFuction.setDate(txtAgDt.Text.Trim()) >= gblFuction.setDate(txtToDt.Text.Trim()))
            //{
            //    gblFuction.MsgPopup("Valid Till Cannot be less than Agre Date");
            //    return vRes = false;
            //}

            return vRes;
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            if (ValidateFields() == false)
            {
                vResult =  false;
                Mode = "";
            }
            int[] Q2 = new int[] { 0, 0, 3, 5, 7 };
            string vBrCode = Session[gblValue.BrnchCode].ToString();

            Int32 vCGTID = 0;
            DataTable dt = (DataTable)ViewState["CGTID"];

           
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vNewId = Convert.ToString(ViewState["HVId"]);
            Int32 vErr = 0, vQ1 = 0, vQ2 = 0, vQ3 = 0, vQ4 = 0, vQ5 = 0, vQ6 = 0, vQ7 = 0, vQ8 = 0, vQ10 = 0, vQ11 = 0, vQ12 = 0, vQ13 = 0, vQ14 = 0, vQ15 = 0;
            double vQ1Score = 0, vQ2Score = 0, vQ3Score = 0, vQ4Score = 0, vQ5Score = 0, vQ6Score = 0, vQ7Score = 0, vQ8Score = 0, vQ10Score = 0,
                vQ11Score = 0, vQ12Score = 0, vQ13Score = 0, vQ14Score = 0, vQ15Score = 0;
            double vQ9Score = 0, vTotalScore = 0 ;
            string vQ9 = "";

            CHouseVisit oHv = null;
            CGblIdGenerator oGbl = null;
            try
            {
                //if (chkGrt.Checked == true)
                //    vGrtYn = "Y";
                vQ1 = Convert.ToInt32(ddlQ1.SelectedValue) ;
                vQ2 = Convert.ToInt32(ddlQ2.SelectedValue);
                vQ3 = Convert.ToInt32(ddlQ3.SelectedValue);
                vQ4 = Convert.ToInt32(ddlQ4.SelectedValue);
                vQ5 = Convert.ToInt32(ddlQ5.SelectedValue);
                vQ6 = Convert.ToInt32(ddlQ6.SelectedValue);
                vQ7 = Convert.ToInt32(ddlQ7.SelectedValue);
                vQ8 = Convert.ToInt32(ddlQ8.SelectedValue);
                vQ10 = Convert.ToInt32(ddlQ10.SelectedValue);
                vQ11 = Convert.ToInt32(ddlQ11.SelectedValue);
                vQ12 = Convert.ToInt32(ddlQ12.SelectedValue);
                vQ13 = Convert.ToInt32(ddlQ13.SelectedValue);
                vQ14 = Convert.ToInt32(ddlQ14.SelectedValue);
                vQ15 = Convert.ToInt32(ddlQ15.SelectedValue);

                vQ1Score = txtQ1Score.Text == "" ? 0 : Convert.ToDouble(txtQ1Score.Text);
                vQ2Score = txtQ2Score.Text == "" ? 0 : Convert.ToDouble(txtQ2Score.Text);
                vQ3Score = txtQ3Score.Text == "" ? 0 : Convert.ToDouble(txtQ3Score.Text);
                vQ4Score = txtQ4Score.Text == "" ? 0 : Convert.ToDouble(txtQ4Score.Text);
                vQ5Score = txtQ5Score.Text == "" ? 0 : Convert.ToDouble(txtQ5Score.Text);
                vQ6Score = txtQ6Score.Text == "" ? 0 : Convert.ToDouble(txtQ6Score.Text);
                vQ7Score = txtQ7Score.Text == "" ? 0 : Convert.ToDouble(txtQ7Score.Text);
                vQ8Score = txtQ8Score.Text == "" ? 0 : Convert.ToDouble(txtQ8Score.Text);
                vQ9Score = txtQ9Score.Text == "" ? 0 : Convert.ToDouble(txtQ9Score.Text);
                vQ10Score = txtQ10Score.Text == "" ? 0 : Convert.ToDouble(txtQ10Score.Text);
                vQ11Score = txtQ11Score.Text == "" ? 0 : Convert.ToDouble(txtQ11Score.Text);
                vQ12Score = txtQ12Score.Text == "" ? 0 : Convert.ToDouble(txtQ12Score.Text);
                vQ13Score = txtQ13Score.Text == "" ? 0 : Convert.ToDouble(txtQ13Score.Text);
                vQ14Score = txtQ14Score.Text == "" ? 0 : Convert.ToDouble(txtQ14Score.Text);
                vQ15Score = txtQ15Score.Text == "" ? 0 : Convert.ToDouble(txtQ15Score.Text);

                DataRow[] result = dt.Select("MemberID = " + ddlMem.SelectedValue);

                foreach (ListItem vItem in chkQ9.Items)
                {
                    if (vItem.Selected == true)
                    {
                        if (vQ9 == "") vQ9 = vItem.Value;
                        else vQ9 = vQ9 + "," + vItem.Value;
                    }
                }
                vQ9Score = Convert.ToDouble(txtQ9Score.Text);
                vTotalScore = vQ1Score + vQ2Score + vQ3Score + vQ4Score + vQ5Score + vQ6Score + vQ7Score + vQ8Score + vQ9Score + vQ10Score + vQ11Score + vQ12Score + vQ13Score + vQ14Score + vQ15Score;
                if (Mode == "Save")
                {
                    vCGTID = Convert.ToInt32(result[0]["CGTId"].ToString());
                    oHv = new CHouseVisit();
                    oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("BranchMst", "BranchCode", txtBrCode.Text.Replace("'", "''"), "", "", "", vDisId, "Save");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("Branch Code Can not be Duplicate...");
                    //    return false;
                    //}
                    if (vTotalScore < 50)
                    {
                        gblFuction.MsgPopup("Total Score Can not be Lesser than 50");
                        return false;
                    }
                    vErr = oHv.SaveHouseStatusNew(vCGTID, ddlMem.SelectedValue, gblFuction.setDate(txtVisitDt.Text), ddlVisitBy.SelectedValue,
                                gblFuction.setDate(txtExGDt.Text), vQ1, vQ1Score, Convert.ToInt32(txtQ1Weight.Text), vQ2, vQ2Score, Convert.ToInt32(txtQ2Weight.Text), vQ3,
                                vQ3Score, Convert.ToInt32(txtQ3Weight.Text), vQ4, vQ4Score, Convert.ToInt32(txtQ4Weight.Text), vQ5, vQ5Score, Convert.ToInt32(txtQ5Weight.Text),
                                vQ6, vQ6Score,Convert.ToInt32(txtQ6Weight.Text), vQ7, vQ7Score,Convert.ToInt32(txtQ7Weight.Text), vQ8, vQ8Score,Convert.ToInt32(txtQ8Weight.Text),
                                vQ9, vQ9Score, Convert.ToInt32(txtQ9Weight.Text), vQ10, vQ10Score, Convert.ToInt32(txtQ10Weight.Text), vQ11, vQ11Score, Convert.ToInt32(txtQ1Weight.Text),
                                vQ12, vQ12Score,Convert.ToInt32(txtQ12Weight.Text), vQ13, vQ13Score,Convert.ToInt32(txtQ13Weight.Text), vQ14, vQ14Score,Convert.ToInt32(txtQ14Weight.Text),
                                vBrCode, this.UserID, "Save", vQ15, vQ15Score, Convert.ToInt32(txtQ15Weight.Text));
                    if (vErr > 0)
                    {
                        ViewState["HVId"] = vNewId;
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
                    if (vTotalScore < 50)
                    {
                        gblFuction.MsgPopup("Total Score Can not be Lesser than 50");
                        return false;
                    }

                    vCGTID = Convert.ToInt32(ViewState["CGTBYID"]);
                    oHv = new CHouseVisit();
                    vErr = oHv.SaveHouseStatusNew(vCGTID, ddlMem.SelectedValue, gblFuction.setDate(txtVisitDt.Text), ddlVisitBy.SelectedValue,
                                gblFuction.setDate(txtExGDt.Text), vQ1, vQ1Score, Convert.ToInt32(txtQ1Weight.Text), vQ2, vQ2Score, Convert.ToInt32(txtQ2Weight.Text), vQ3,
                                vQ3Score, Convert.ToInt32(txtQ3Weight.Text), vQ4, vQ4Score, Convert.ToInt32(txtQ4Weight.Text), vQ5, vQ5Score, Convert.ToInt32(txtQ5Weight.Text),
                                vQ6, vQ6Score, Convert.ToInt32(txtQ6Weight.Text), vQ7, vQ7Score, Convert.ToInt32(txtQ7Weight.Text), vQ8, vQ8Score, Convert.ToInt32(txtQ8Weight.Text),
                                vQ9, vQ9Score, Convert.ToInt32(txtQ9Weight.Text), vQ10, vQ10Score, Convert.ToInt32(txtQ10Weight.Text), vQ11, vQ11Score, Convert.ToInt32(txtQ1Weight.Text),
                                vQ12, vQ12Score, Convert.ToInt32(txtQ12Weight.Text), vQ13, vQ13Score, Convert.ToInt32(txtQ13Weight.Text), vQ14, vQ14Score, Convert.ToInt32(txtQ14Weight.Text),
                                vBrCode, this.UserID, "Edit", vQ15, vQ15Score, Convert.ToInt32(txtQ15Weight.Text));
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
                    vCGTID = Convert.ToInt32(ViewState["CGTBYID"]);
                    oHv = new CHouseVisit();

                    gblFuction.MsgPopup("Delete Not Possible");
                    vResult = false;
                    //vErr = oHv.SaveHouseStatus(vCGTID, ddlMem.SelectedValue, gblFuction.setDate(txtVisitDt.Text), ddlVisitBy.SelectedValue,
                    //    gblFuction.setDate(txtExGDt.Text), vQ1, vQ1, vQ2, Q2[vQ2], vQ3, vQ3, vQ4, vQ4, vQ5, vQ5, vQ6, vQ6, vQ7, vQ7, vQ8, vQ8, 
                    //    vQ9, vQ9, vQ10, vQ10, vQ11, vQ11, vQ12, vQ12, vQ13, vQ13, vQ14, vQ14, vBrCode, this.UserID, "Del",
                    //    vQ15, vQ15, vQ16, vQ16);
                    //if (vErr > 0)
                    //{
                    //    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    //    vResult = true;
                    //}
                    //else
                    //{
                    //    gblFuction.MsgPopup(gblMarg.DBError);
                    //    vResult = false;
                    //}
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oHv = null;
                //oGbl = null;
            }
        }

        protected void btnUpdateUcic_Click(object sender, EventArgs e)
        {
            string pMemberId = ddlMem.SelectedValue;
            int pCgtId = Convert.ToInt32(ViewState["CGTBYID"]);
            string pUcicId = getUcic(pMemberId, Convert.ToInt32(Session[gblValue.UserId]), pCgtId);
            int pErr = -1;
            CHouseVisit hv = new CHouseVisit();
            if (pUcicId != "")
            {
                pErr = hv.UpdateUcicId(pUcicId, pMemberId, pCgtId);
                if (pErr == 0)
                {
                    gblFuction.MsgPopup(gblMarg.EditMsg);
                }
                else
                {
                    gblFuction.MsgPopup(gblMarg.DBError);
                }
            }
            else
            {
                gblFuction.MsgPopup("Respose Error");
            }
        }

        public string getUcic(string pMemberId, int pCreatedBy, int pCgtId)
        {
            string vResponse = "", vUcic = "";
            CHouseVisit oHv = new CHouseVisit();
            try
            {
                string Requestdata = "{\"cust_id\" :" + "\"" + pMemberId + "\"" + ",\"source_system_name\":\"BIJLI\"}";
                //string postURL = "http://144.24.116.182:9002/UnitySfbWS/getUcic";
                string postURL = "https://ucic.unitybank.co.in:9002/UnitySfbWS/getUcic";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                dynamic res = JsonConvert.DeserializeObject(vResponse);
                if (res.ResponseCode == "200")
                {
                    vUcic = res.Ucic;
                }
                //----------------------------Save Log---------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oHv.SaveProsidexLogUCIC(pMemberId, pCgtId, vResponseXml, pCreatedBy, vUcic);
                //---------------------------------------------------------------------------------
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
                //----------------------------Save Log---------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oHv.SaveProsidexLogUCIC(pMemberId, pCgtId, vResponseXml, pCreatedBy, vUcic);
                //---------------------------------------------------------------------------------
            }
            return vUcic;
        }

        #region Common
        public string AsString(XmlDocument xmlDoc)
        {
            string strXmlText = null;
            if (xmlDoc != null)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (XmlTextWriter tx = new XmlTextWriter(sw))
                    {
                        xmlDoc.WriteTo(tx);
                        strXmlText = sw.ToString();
                    }
                }
            }
            return strXmlText;
        }
        #endregion

    }
}