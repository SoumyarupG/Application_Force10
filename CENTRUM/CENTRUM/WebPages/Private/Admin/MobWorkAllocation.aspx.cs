using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;


namespace CENTRUM.WebPages.Private.Admin
{
    public partial class MobWorkAllocation : CENTRUMBase
    {
        private int vPgNo = 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch(Session[gblValue.UserName].ToString());
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                    //ddlBranch.Enabled = false;
                    popRO();
                    popEmp();
                }
                else
                {
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                    //ddlBranch.Enabled = true;
                }
                StatusButton("View");
                ViewState["StateEdit"] = null;
                //LoadGrid(0);
                tbRole.ActiveTabIndex = 0;
                StatusButton("View");
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
                dt = oRO.PopRO_DesigWise(vBrCode, "0", "0", vLogDt, this.UserID,"RO");
                ddlEo.DataSource = dt;
                ddlEo.DataTextField = "EoName";
                ddlEo.DataValueField = "EoId";
                ddlEo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlEo.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        private void popEmp()
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
                dt = oRO.PopRO_DesigWise(vBrCode, "0", "0", vLogDt, this.UserID, "CO,BM,ABM,RO");
                dllAlloicEmp.DataSource = dt;
                dllAlloicEmp.DataTextField = "EoName";
                dllAlloicEmp.DataValueField = "EoId";
                dllAlloicEmp.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                dllAlloicEmp.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            popRO();
            popEmp();
            
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Work Allocation";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuMobWorkAlloc);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Mobile Work Allocation", false);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(0);
        }


        /// <summary>
        /// 
        /// </summary>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vRows = 0;
            CEO oItm = null;
            try
            {
                oItm = new CEO();
                dt = oItm.GetWorkAllocationPG(pPgIndx, ref vRows, gblFuction.setDate(txtDtFrm.Text), gblFuction.setDate(txtToDt.Text), Session[gblValue.BrnchCode].ToString());
                gvWorkAlloc.DataSource = dt;
                gvWorkAlloc.DataBind();
                lblTotPg.Text = CalTotPages(vRows).ToString();
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
                oItm = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRows"></param>
        /// <returns></returns>
        private int CalTotPages(double pRows)
        {
            int vPgs = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vPgs;
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
                case "Prev":
                    vPgNo = Int32.Parse(lblCrPg.Text) - 1;
                    break;
                case "Next":
                    vPgNo = Int32.Parse(lblCrPg.Text) + 1;
                    break;
            }
            LoadGrid(vPgNo);
            tbRole.ActiveTabIndex = 0;
        }

        /// <summary>
        /// Status Button
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
                case "Exit":
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //private Boolean ValidateFields()
        //{
        //    Boolean vResult = true;
        //    if (txtName.Text.Trim() == "")
        //    {
        //        gblFuction.MsgPopup("Role cannot left blank..");
        //        vResult = false;
        //    }
        //    return vResult;
        //}

        /// <summary>
        /// Enable Control
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtAllocationDt.Enabled = Status;
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                ddlBranch.Enabled = false;
            }
            else
            {
                ddlBranch.Enabled = Status;
            }
            ddlEo.Enabled = Status;
            dllAlloicEmp.Enabled = Status;
        }

        /// <summary>
        /// Clear Controls
        /// </summary>
        private void ClearControls()
        {
            txtAllocationDt.Text = "";
            if (Session[gblValue.BrnchCode].ToString() == "0000")
                ddlBranch.SelectedIndex = -1;
            ddlEo.SelectedIndex = -1;
            dllAlloicEmp.SelectedIndex = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 dChk = Convert.ToInt32(ViewState["WorkAllocID"]), vRec = 0, vErr = 0;
            CEO oCEO = null;
            string vErrMsg = "";
            //CGblIdGenerator oGbl = null;

            try
            {
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                string vBrCode = ddlBranch.SelectedValue;
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                string vWorkAllockId = Convert.ToString(ViewState["WorkAllocID"]);

                if (Mode == "Save")
                {
                    oCEO = new CEO();
                    //oGbl = new CGblIdGenerator();

                    vErr = oCEO.SaveWorkAllocation(ref dChk, gblFuction.setDate(txtAllocationDt.Text), dllAlloicEmp.SelectedValue,
                        ddlEo.SelectedValue, vBrCode, this.UserID, "Save",ref vErrMsg);
                    if (vErr == 0)
                    {
                        ViewState["WorkAllocID"] = dChk;
                        vResult = true;
                    }
                    else if (vErr == 1)
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                    else if (vErr == 2)
                    {
                        gblFuction.MsgPopup(vErrMsg);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {

                    oCEO = new CEO();

                    vErr = oCEO.SaveWorkAllocation(ref dChk, gblFuction.setDate(txtAllocationDt.Text), dllAlloicEmp.SelectedValue,
                       ddlEo.SelectedValue, vBrCode, this.UserID, "Edit", ref vErrMsg);
                    
        

                    if (vErr == 0)
                    {
                        vResult = true;
                    }
                    else if (vErr == 1)
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                    else if (vErr == 2)
                    {
                        gblFuction.MsgPopup(vErrMsg);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {

                    oCEO = new CEO();

                    vErr = oCEO.SaveWorkAllocation(ref dChk, gblFuction.setDate(txtAllocationDt.Text), dllAlloicEmp.SelectedValue,
                       ddlEo.SelectedValue, vBrCode, this.UserID, "Del", ref vErrMsg);

                    
                    

                    if (vErr == 0)
                    {
                        vResult = true;
                    }
                    else if (vErr == 1)
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                    else if (vErr == 2)
                    {
                        gblFuction.MsgPopup(vErrMsg);
                        vResult = false;
                    }
                    
                }
                return vResult;
            }
            finally
            {
                oCEO = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                if (vStateEdit == "" || vStateEdit == null)
                    vStateEdit = "Save";
                if (SaveRecords(vStateEdit) == true)
                {
                    gblFuction.MsgPopup(gblMarg.SaveMsg);
                    LoadGrid(0);
                    StatusButton("View");
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
        protected void gvWorkAlloc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vWorkAllockId = 0;
            DataTable dt = null;
            CEO oItm = null;
            try
            {
                vWorkAllockId = Convert.ToInt32(e.CommandArgument);
                ViewState["WorkAllocID"] = vWorkAllockId;
                if (e.CommandName == "cmdShow")
                {
                    oItm = new CEO();
                    dt = oItm.GetWorkAllocById(vWorkAllockId);
                    if (dt.Rows.Count > 0)
                    {
                        txtAllocationDt.Text = Convert.ToString(dt.Rows[0]["AllocDate"]);
                        ddlEo.SelectedIndex = ddlEo.Items.IndexOf(ddlEo.Items.FindByValue(dt.Rows[0]["AbsentEOID"].ToString()));
                        dllAlloicEmp.SelectedIndex = dllAlloicEmp.Items.IndexOf(dllAlloicEmp.Items.FindByValue(dt.Rows[0]["WorkingEOID"].ToString()));
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["Branchcode"].ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbRole.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oItm = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                tbRole.ActiveTabIndex = 0;
                EnableControl(false);
                StatusButton("View");
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
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
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
                    StatusButton("Delete");
                    tbRole.ActiveTabIndex = 0;
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
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanAdd == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                ViewState["StateEdit"] = null;
                tbRole.ActiveTabIndex = 1;
                StatusButton("Add");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}