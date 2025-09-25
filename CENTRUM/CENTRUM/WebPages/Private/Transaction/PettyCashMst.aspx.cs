using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FORCEBA;
using FORCECA;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class PettyCashMst : CENTRUMBase
    {
        protected int cPgNo = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = "";
                ViewState["PettyMstId"] = "";
                StatusButton("View");
                LoadGrid(1);
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Petty Cash Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuPettyCashMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Petty Cash Master", false);
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
                tbQly.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
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
                LoadGrid(0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbQly.ActiveTabIndex = 0;
            EnableControl(false);
            ClearControls();
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
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadGrid(0);
                    ClearControls();
                    tbQly.ActiveTabIndex = 0;
                    StatusButton("Delete");
                }
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

        protected void gvPetty_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vPettyMstId = "";
            DataTable dt = null;
            CPettyCash oPetty = null;
            try
            {
                vPettyMstId = Convert.ToString(e.CommandArgument);
                ViewState["PettyMstId"] = vPettyMstId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvPetty.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oPetty = new CPettyCash();
                    dt = oPetty.GetPettyCashMstDtl(vPettyMstId);
                    if (dt.Rows.Count > 0)
                    {
                        txtExpenseHead.Text = Convert.ToString(dt.Rows[0]["ExpenseHead"]).Trim();
                        txtLed.Text = Convert.ToString(dt.Rows[0]["LedgerDesc"]).Trim();
                        hdLed.Value = Convert.ToString(dt.Rows[0]["DescId"]).Trim();
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbQly.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oPetty = null;
            }
        }

        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CPettyCash oPetty = null;
            Int32 vRows = 0;
            string vBrCode = string.Empty;
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oPetty = new CPettyCash();
                dt = oPetty.GetPettyCashMstPG(vBrCode, txtSearch.Text.Trim(), pPgIndx, ref vRows);
                gvPetty.DataSource = dt.DefaultView;
                gvPetty.DataBind();
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
                oPetty = null;
            }
        }

        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
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
            tbQly.ActiveTabIndex = 0;
        }

        private void EnableControl(Boolean Status)
        {
            txtExpenseHead.Enabled = Status;
            txtLed.Enabled = Status;
        }

        private void ClearControls()
        {
            txtExpenseHead.Text = "";
            txtLed.Text = "";
            ViewState["PettyMstId"] = "";
            hdLed.Value = "";
            lblDate.Text = "";
            lblUser.Text = "";
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

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vPettyMstId = Convert.ToString(ViewState["PettyMstId"]);
            Int32 vRec = 0, vErr = 0;
            CPettyCash oPetty = null;
            string vExpenseHead = txtExpenseHead.Text.Trim().ToUpper();
            string vDescId = Convert.ToString(hdLed.Value);
            string vLedgerDesc = txtLed.Text.Trim().ToUpper();
            CGblIdGenerator oGbl = null;

            if (vExpenseHead == "")
            {
                gblFuction.MsgPopup("Please enter expense head");
                return false;
            }
            if (vDescId == "")
            {
                gblFuction.MsgPopup("Please enter ledger");
                return false;
            }

            if (Mode != "Save")
            {
                oPetty = new CPettyCash();
                DataTable dtEditChk = new DataTable();
                dtEditChk = oPetty.PettyCashMstEditCheck(vPettyMstId);
                if (dtEditChk.Rows.Count > 0) 
                {
                    gblFuction.MsgPopup("Data in use modification not allowed.");
                    return false;
                }
            }

            try
            {
                if (Mode == "Save")
                {
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("PettyCashMst", "ExpenseHead", vExpenseHead.Replace("'", "''"), "", "", "ID", vPettyMstId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Expense Head Can not be Duplicate...");
                        return false;
                    }

                    oPetty = new CPettyCash();
                    vErr = oPetty.InsertPettyCashMst(ref vPettyMstId, vExpenseHead, vDescId, Convert.ToInt32(Session[gblValue.UserId]), "Save", vLedgerDesc);
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["PettyMstId"] = vPettyMstId;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("PettyCashMst", "ExpenseHead", vExpenseHead.Replace("'", "''"), "", "", "ID", vPettyMstId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Expense Head Can not be Duplicate...");
                        return false;
                    }

                    oPetty = new CPettyCash();
                    vErr = oPetty.InsertPettyCashMst(ref vPettyMstId, vExpenseHead, vDescId, Convert.ToInt32(Session[gblValue.UserId]), "Edit", vLedgerDesc);
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
                    oPetty = new CPettyCash();
                    vErr = oPetty.InsertPettyCashMst(ref vPettyMstId, vExpenseHead, vDescId, Convert.ToInt32(Session[gblValue.UserId]), "Delete", vLedgerDesc);
                    if (vErr > 0)
                    {
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
                oPetty = null;
            }            
        }
    }
}