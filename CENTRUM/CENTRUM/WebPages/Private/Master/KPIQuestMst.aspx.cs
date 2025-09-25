using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.Data;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class KPIQuestMst : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                StatusButton("View");
                LoadGrid(0);
                popDesignation();
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "KPI Question Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuKPIQuestMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Occupation Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
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
                    gblFuction.focus("ctl00_cph_Main_tabGp_pnlDtl_txtBlock");
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
                    gblFuction.focus("ctl00_cph_Main_tabGp_pnlDtl_txtBlock");
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
        private void ClearControls()
        {
            txtKPIQuest.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
            ddlDesig.SelectedIndex = -1;
            txtWtPerc.Text = "0";
            txtTarget.Text = "0";
        }
        private void popDesignation()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "DesignationId", "DesignationName", "DesignationMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlDesig.DataSource = dt;
                ddlDesig.DataTextField = "DesignationName";
                ddlDesig.DataValueField = "DesignationId";
                ddlDesig.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlDesig.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
            
        }
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblTotPg.Text) - 1; //lblCrPg
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCrPg.Text) + 1; //lblTotPg
                    break;
            }
            LoadGrid(cPgNo);
            tbOcup.ActiveTabIndex = 0;
        }
        private void EnableControl(Boolean Status)
        {
            txtKPIQuest.Enabled = Status;
            ddlDesig.Enabled = Status;
            txtWtPerc.Enabled = Status;
            txtTarget.Enabled = Status;
            txtDtFrm.Enabled = Status;
            txtToDt.Enabled = Status;
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
                LoadGrid(0);
                ViewState["StateEdit"] = "Add";
                tbOcup.ActiveTabIndex = 1;
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
                    LoadGrid(0);
                    ClearControls();
                    tbOcup.ActiveTabIndex = 0;
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
            LoadGrid(0);
            tbOcup.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(0);
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
            ViewState["PurposeId"] = null;
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vQuestId = Convert.ToString(ViewState["QuestId"]);
            Int32 vErr = 0, vRec = 0, vQstId = 0, vDesigId=0;
            CEmpKPI oOcu = null;
            CGblIdGenerator oGbl = null;
            try
            {
                vQstId = Convert.ToInt32(ViewState["QuestId"]);
                vDesigId = Convert.ToInt32(ddlDesig.SelectedValue);
                if (Mode == "Save")
                {
                    oOcu = new CEmpKPI();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("QuestionMst", "Question", txtKPIQuest.Text.Replace("'", "''"), "", "", "QuestionId", vQuestId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Question Can not be Duplicate...");
                        return false;
                    }
                    vErr = oOcu.InsertQuestion(ref vQstId, txtKPIQuest.Text.Replace("'", "''"), this.UserID, "I", "Save",
                        vDesigId, gblFuction.setDate(txtDtFrm.Text), gblFuction.setDate(txtToDt.Text), Convert.ToDouble(txtWtPerc.Text == "" ? "0" : txtWtPerc.Text),
                        Convert.ToDouble(txtTarget.Text == "" ? "0" : txtTarget.Text));
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["QuestId"] = vQstId;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oOcu = new CEmpKPI();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("QuestionMst", "Question", txtKPIQuest.Text.Replace("'", "''"), "", "", "QuestionId", vQuestId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Question Can not be Duplicate...");
                        return false;
                    }
                    vErr = oOcu.InsertQuestion(ref vQstId, txtKPIQuest.Text.Replace("'", "''"), this.UserID, "E", "Edit",
                        vDesigId, gblFuction.setDate(txtDtFrm.Text), gblFuction.setDate(txtToDt.Text), Convert.ToDouble(txtWtPerc.Text == "" ? "0" : txtWtPerc.Text),
                        Convert.ToDouble(txtTarget.Text == "" ? "0" : txtTarget.Text));
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
                    oGbl = new CGblIdGenerator();
                    vRec = 0;
                    if (vRec <= 0)
                    {
                        oOcu = new CEmpKPI();
                        vErr = oOcu.InsertQuestion(ref vQstId, txtKPIQuest.Text.Replace("'", "''"), this.UserID, "D", "Del",
                            vDesigId, gblFuction.setDate(txtDtFrm.Text), gblFuction.setDate(txtToDt.Text), Convert.ToDouble(txtWtPerc.Text == "" ? "0" : txtWtPerc.Text),
                        Convert.ToDouble(txtTarget.Text == "" ? "0" : txtTarget.Text));
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup(gblMarg.DeleteMsg);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.DBError);
                            vResult = false;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.RecordUseMsg);
                        vResult = false;
                    }
                }
                return vResult;
            }
            finally
            {
                oOcu = null;
                oGbl = null;
            }
        }
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CEmpKPI oOcu = null;
            Int32 vRows = 0;
            try
            {
                oOcu = new CEmpKPI();
                dt = oOcu.GetQuestPG(pPgIndx, ref vRows);
                gvOccu.DataSource = dt.DefaultView;
                gvOccu.DataBind();
                if (dt.Rows.Count <= 0)
                {
                    lblTotPg.Text = "0";
                    lblCrPg.Text = "0";
                }
                else
                {
                    lblTotPg.Text = CalTotPgs(vRows).ToString();
                    lblCrPg.Text = cPgNo.ToString();
                }
                if (cPgNo == 1)
                {
                    btnPrev.Enabled = false;
                    if (Int32.Parse(lblTotPg.Text) > 0 && cPgNo != Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                }
                else
                {
                    btnPrev.Enabled = true;
                    if (cPgNo == Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
                }
            }
            finally
            {
                dt = null;
                oOcu = null;
            }
        }
        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }
        protected void gvQuest_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vQstId = 0, vRow = 0;
            DataTable dt = null;
            CEmpKPI oOcu = null;
            try
            {
                vQstId = Convert.ToInt32(e.CommandArgument);
                ViewState["QuestId"] = vQstId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvOccu.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oOcu = new CEmpKPI();
                    dt = oOcu.GetQuestionbyId(vQstId);
                    if (dt.Rows.Count > 0)
                    {
                        txtKPIQuest.Text = Convert.ToString(dt.Rows[vRow]["Question"]).Trim();
                        txtDtFrm.Text = Convert.ToString(dt.Rows[vRow]["FromDate"]).Trim();
                        txtToDt.Text = Convert.ToString(dt.Rows[vRow]["ToDate"]).Trim();
                        ddlDesig.SelectedIndex = ddlDesig.Items.IndexOf(ddlDesig.Items.FindByValue(dt.Rows[vRow]["DesignationId"].ToString()));
                        txtWtPerc.Text = Convert.ToString(dt.Rows[vRow]["WtPerc"]).Trim();
                        txtTarget.Text = Convert.ToString(dt.Rows[vRow]["Target"]).Trim();
                        lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tbOcup.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oOcu = null;
            }
        }
    }
}