using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class ExitReasonMst : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                StatusButton("View");
                LoadGrid(0);
            }

        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Client Exit Reason(Broad)";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuOccu);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "ExitReason Master", false);
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            //COccu oOcu = null;
            ExitReason oExRn = null;
            Int32 vRows = 0;
            try
            {
                //oOcu = new COccu();
                //dt = oOcu.GetOccuPG(pPgIndx, ref vRows);
                oExRn = new ExitReason();
                dt = oExRn.GetExRnPG(pPgIndx, ref vRows);

                gvExRn.DataSource = dt.DefaultView;
                gvExRn.DataBind();
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
                oExRn = null;
                //oOcu = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvExRn_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vExitReasonId = 0, vRow = 0;
            DataTable dt = null;
            //COccu oOcu = null;
            ExitReason oExRn = null;
            try
            {
                vExitReasonId = Convert.ToInt32(e.CommandArgument);
                ViewState["ExitReasonID"] = vExitReasonId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvExRn.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    //oOcu = new COccu();
                    oExRn = new ExitReason();
                    dt = oExRn.GetExRnbyId(vExitReasonId);
                    if (dt.Rows.Count > 0)
                    {
                        txtExRn.Text = Convert.ToString(dt.Rows[vRow]["ExitReason"]).Trim();
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
                //oOcu = null;
                oExRn = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vExitReasonId = Convert.ToString(ViewState["ExitReasonID"]);
            Int32 vErr = 0, vRec = 0, ExitReasonId = 0;
            //COccu oOcu = null;
            ExitReason oExRn = null;
            CGblIdGenerator oGbl = null;
            try
            {
                ExitReasonId = Convert.ToInt32(ViewState["ExitReasonID"]);
                if (Mode == "Save")
                {
                    //oOcu = new COccu();
                    oExRn = new ExitReason();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("ExitReasonMst", "ExitReason", txtExRn.Text.Replace("'", "''"), "", "", "ExitReasonId", vExitReasonId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("ExitReason Can not be Duplicate...");
                        return false;
                    }
                    //vErr = oOcu.InsertOccu(ref ExitReasonID, txtExRn.Text.Replace("'", "''"), this.UserID, "I", "Save");
                    vErr = oExRn.InsertExRn(ref ExitReasonId, txtExRn.Text.Replace("'", "''"), this.UserID, "I", "Save");
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["ExitReasonID"] = ExitReasonId;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    //oOcu = new COccu();
                    oExRn = new ExitReason();
                    oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("OccupationMst", "OccupationName", txtExRn.Text.Replace("'", "''"), "", "", "OccupationId", vExitReason, "Edit");
                    vRec = oGbl.ChkDuplicate("ExitReasonMst", "ExitReason", txtExRn.Text.Replace("'", "''"), "", "", "ExitReasonId", vExitReasonId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("ExitReason Can not be Duplicate...");
                        return false;
                    }
                    //vErr = oOcu.InsertOccu(ref ExitReasonID, txtExRn.Text.Replace("'", "''"), this.UserID, "E", "Edit");
                    //vErr = oOcu.InsertOccu(ref ExitReasonID, txtExRn.Text.Replace("'", "''"), this.UserID, "E", "Edit");
                    vErr = oExRn.InsertExRn(ref ExitReasonId, txtExRn.Text.Replace("'", "''"), this.UserID, "E", "Edit");
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
                    //oOcu = new COccu();
                    oExRn = new ExitReason();
                    oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("OccupationMst", "OccupationName", txtExRn.Text.Replace("'", "''"), "", "", "OccupationId", vExitReason, "Edit");
                    vRec = oGbl.ChkExRn(ref ExitReasonId);
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("ExitReason Can not be Deleted...");
                        return false;
                    }
                    //vErr = oOcu.InsertOccu(ref ExitReasonID, txtExRn.Text.Replace("'", "''"), this.UserID, "E", "Edit");
                    //vErr = oOcu.InsertOccu(ref ExitReasonID, txtExRn.Text.Replace("'", "''"), this.UserID, "E", "Edit");
                    else
                    {
                        vErr = oExRn.InsertExRn(ref ExitReasonId, txtExRn.Text.Replace("'", "''"), this.UserID, "D", "Delete");
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
                }
                return vResult;
                   
            }
            finally
            {
                oExRn = null;
                //oOcu = null;
                oGbl = null;
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
            ViewState["PurposeId"] = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            LoadGrid(0);
            tbOcup.ActiveTabIndex = 0;
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
                LoadGrid(0);
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtExRn.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtExRn.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
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
                    cPgNo = Int32.Parse(lblTotPg.Text) - 1; //lblCrPg
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCrPg.Text) + 1; //lblTotPg
                    break;
            }
            LoadGrid(cPgNo);
            tbOcup.ActiveTabIndex = 0;
        }
    }
}