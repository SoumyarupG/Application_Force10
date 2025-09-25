using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class SubExitRnMst : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                StatusButton("View");
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                popState();
                LoadGrid();
                tbDist.ActiveTabIndex = 0;
            }

        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Client Exit Reason Detail";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDistrictMst);
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
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "SubExitReason Master", false);
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
        private void EnableControl(Boolean Status)
        {
            txtSubExRn.Enabled = Status;
            ddlExRn.Enabled = Status;
        }
        private void ClearControls()
        {
            txtSubExRn.Text = "";
            ddlExRn.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
        }

        private void popState()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                //dt = oGb.PopComboMIS("N", "N", "AA", "StateId", "StateName", "StateMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                //dt = oGb.PopExRn();
                //ddlExRn.DataSource = dt;
                //ddlExRn.DataTextField = "ExitReason";
                //ddlExRn.DataValueField = "ExitReasonId";
                //ddlExRn.DataBind();
                //ListItem oli = new ListItem("<--Select-->", "-1");
                //ddlExRn.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

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
            try
            {
                if (this.CanAdd == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                ViewState["StateEdit"] = "Add";
                tbDist.ActiveTabIndex = 1;
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
                    LoadGrid();
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
            tbDist.ActiveTabIndex = 0;
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
                LoadGrid();
                StatusButton("View");
                ViewState["StateEdit"] = null;
                tbDist.ActiveTabIndex = 1;
            }
        }
        private void LoadGrid()
        {
            DataTable dt = null;
            //Int32 vSubExRnID = 0;
            string vBrCode = "";
            //CDistrict oDist = null;
            CSubExRn oSubExRn = null;
            try
            {
                //vSubExRnID = Convert.ToInt32(Session[gblValue.SubExRnID].ToString());
                vBrCode = Session[gblValue.BrnchCode].ToString();
                //oDist = new CDistrict();
                oSubExRn = new CSubExRn(); 
                //dt = oDist.GetDistrictList();
                //dt.PrimaryKey = new DataColumn[] { dt.Columns["SubExRnID"] };
                dt = oSubExRn.GetSubExRnList();
                dt.PrimaryKey = new DataColumn[] {dt.Columns["SubExRnID"] };
                ViewState["State"] = dt;
                gvSubExRn.DataSource = dt.DefaultView;
                gvSubExRn.DataBind();
                tbDist.ActiveTabIndex = 0;
            }
            finally
            {
                //oDist = null;
                oSubExRn = null;
                dt = null;
            }
        }
        protected void gvSubExRn_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 pZoneId = 0, vRow = 0;
            DataTable dt = null;
            try
            {
                pZoneId = Convert.ToInt32(e.CommandArgument);
                ViewState["SubExRnID"] = pZoneId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvSubExRn.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    dt = (DataTable)ViewState["State"];
                    vRow = dt.Rows.IndexOf(dt.Rows.Find(pZoneId));
                    if (dt.Rows.Count > 0)
                    {
                        txtSubExRn.Text = Convert.ToString(dt.Rows[vRow]["SubExitReason"]);
                        ddlExRn.SelectedIndex = ddlExRn.Items.IndexOf(ddlExRn.Items.FindByValue(dt.Rows[vRow]["ExitReasonId"].ToString()));
                        //lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tbDist.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
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
            string vSubERID = Convert.ToString(ViewState["SubExRnID"]);
            Int32 vErr = 0, vRec = 0, vSubExRnID = 0, vNewId = 0;
            //string vStatId = "";
            string vExRnId = "";
            //CDistrict oDist = null;
            CSubExRn oSubExRn = null;
            CGblIdGenerator oGbl = null;
            try
            {
                //DataTable dt = (DataTable)ViewState["State"];
                //vStatId = dt.Rows[0]["StateId"].ToString();
                vExRnId = ddlExRn.SelectedValue;
                vSubExRnID = Convert.ToInt32(ViewState["SubExRnID"]);
                if (Mode == "Save")
                {
                    //oDist = new CDistrict();
                    oSubExRn = new CSubExRn();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("SubExRnMst", "SubExitReason", txtSubExRn.Text.Replace("'", "''"), "ExitReasonId", vExRnId, "SubExRnId", vSubERID, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("SubExitReason Can not be Duplicate...");
                        return false;
                    }
                    vErr = oSubExRn.SaveSubExRn(ref vNewId, vSubExRnID, txtSubExRn.Text.Replace("'", "''"), Convert.ToInt32(vExRnId), this.UserID, "Save");
                    if (vErr > 0)
                    {
                        ViewState["SubExRnID"] = vNewId;
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
                    //oDist = new CDistrict();
                    oSubExRn = new CSubExRn();
                    oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("DistrictMst", "DistrictName", txtSubExRn.Text.Replace("'", "''"), "StateId", vStatId, "DistrictId", vSubERID, "Edit");
                    vRec = oGbl.ChkDuplicate("SubExRnMst", "SubExitReason", txtSubExRn.Text.Replace("'", "''"), "ExitReasonId", vExRnId, "SubExRnId", vSubERID, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("SubExit Reason Can not be Duplicate...");
                        return false;
                    }
                    //vErr = oDist.SaveDistrict(ref vNewId, vSubExRnID, txtSubExRn.Text.Replace("'", "''"), Convert.ToInt32(vStatId), this.UserID, "Edit");
                    vErr = oSubExRn.SaveSubExRn(ref vNewId, vSubExRnID, txtSubExRn.Text.Replace("'", "''"), Convert.ToInt32(vExRnId), this.UserID, "Edit");
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
                    //oDist = new CDistrict();
                    oSubExRn = new CSubExRn();
                    oGbl = new CGblIdGenerator();
                    //vErr = oGbl.ChkDelete(vSubExRnID, "SubExRnId", "BlockMst");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.RecordUseMsg);
                        return false;
                    }
                    //vErr = oDist.SaveDistrict(ref vNewId, vSubExRnID, txtDist.Text.Replace("'", "''"), Convert.ToInt32(vStatId), this.UserID, "Delet");
                    vErr = oSubExRn.SaveSubExRn(ref vNewId, vSubExRnID, txtSubExRn.Text.Replace("'", "''"), Convert.ToInt32(vExRnId), this.UserID, "Delete");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.DeleteMsg);
                        vResult = true;
                        ClearControls();
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
                oSubExRn = null;
                oGbl = null;
            }
        }
    }
}