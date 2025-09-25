using System;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class IfscMaster : CENTRUMBase
    {
        protected int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;          
                //LoadGrid(0);
                tbIfsc.ActiveTabIndex = 0;
                StatusButton("View");               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "IFSC";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuIfsc);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "User Master", false);
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
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0;//vDistId = 0,
            string vSearch = "";
            CBank oBank = null;
            try
            {               
                vSearch = txtSearch.Text;
                oBank = new CBank();
                dt = oBank.GetIFSC(vSearch, pPgIndx, ref vTotRows);
                gvIfsc.DataSource = dt;
                gvIfsc.DataBind();
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
                oUser = null;
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
            tbIfsc.ActiveTabIndex = 0;
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
        private void ClearControls()
        {
            txtBank.Text = "";
            txtBranch.Text = "";
            txtIfsc.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            if (ViewState["UserId"] == null || ViewState["UserId"].ToString() != "1")
            {
                txtBranch.Enabled = Status;
                txtBank.Enabled = Status;
                txtIfsc.Enabled = Status;               
            }
            else if (ViewState["UserId"].ToString() == "1")
            {
                txtBranch.Enabled = Status;
                txtBank.Enabled = Status;
                txtIfsc.Enabled = Status; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbIfsc.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
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
                gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtUserNm");
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
                    gblFuction.AjxMsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {                   
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadGrid(0);
                    StatusButton("Delete");
                    tbIfsc.ActiveTabIndex = 0;
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
                tbIfsc.ActiveTabIndex = 1;
                StatusButton("Add");
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
        protected void gvIfsc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vIfscId = 0, vRow = 0;
            DataTable dt = null;
            CBank oBank = null;
            try
            {
                vIfscId = Convert.ToInt32(e.CommandArgument);
                ViewState["IfscId"] = vIfscId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvIfsc.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oBank = new CBank();
                    dt = oBank.GetIfscById(vIfscId);
                    if (dt.Rows.Count > 0)
                    {
                        txtBank.Text = Convert.ToString(dt.Rows[vRow]["BANKNAME"]).Trim();
                        txtBranch.Text=Convert.ToString(dt.Rows[vRow]["BRANCHNAME"]).Trim();
                        txtIfsc.Text=Convert.ToString(dt.Rows[vRow]["IFSC_CODE"]).Trim();
                        //lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        //lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tbIfsc.ActiveTabIndex = 1;
                        if (Session[gblValue.BrnchCode].ToString() == "0000")
                            StatusButton("Show");
                        else
                            StatusButton("Exit");
                    }
                }
            }
            finally
            {
                dt = null;
                oBank = null;
            }
        }



        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(0);
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
            Int32 vErr = 0, vRec = 0, vIfscId = 0,vUserId=0;
            CBank oBnk = null;
            CGblIdGenerator oGbl = null;
            vUserId=Convert.ToInt16(Session[gblValue.UserId].ToString());
            try
            {                
                if (Mode == "Save")
                {
                    oBnk = new CBank();
                    oGbl = new CGblIdGenerator();
                    vErr = oBnk.InsertIfsc(ref vIfscId, txtBank.Text.Trim(), txtBranch.Text.Trim(), txtIfsc.Text.Trim(),vUserId, "I", "Save");
                    if (vErr ==1)
                    {
                        vResult = true;
                        ViewState["IfscId"] = vIfscId;
                    }
                    else if (vErr == 2)
                    {
                        gblFuction.MsgPopup("Duplicate IFSC found..!!!");
                        vResult = false;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oBnk = new CBank();
                    oGbl = new CGblIdGenerator();
                    vIfscId = Convert.ToInt32(ViewState["IfscId"]);
                    vErr = oBnk.InsertIfsc(ref vIfscId, txtBank.Text.Trim(), txtBranch.Text.Trim(), txtIfsc.Text.Trim(),vUserId, "E", "Edit");
                    if (vErr == 1)
                    {
                        gblFuction.MsgPopup(gblMarg.EditMsg);
                        vResult = true;
                    }
                    else if (vErr == 2)
                    {
                        gblFuction.MsgPopup("Duplicate IFSC found..!!!");
                        vResult = false;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {                 
                    vRec = 0;
                    if (vRec <= 0)
                    {
                        oBnk = new CBank();
                        vErr = oBnk.InsertIfsc(ref vIfscId, txtBank.Text.Trim(), txtBranch.Text.Trim(), txtIfsc.Text.Trim(),vUserId, "D", "Delete");
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
                oBnk = null;
                oGbl = null;
            }
       
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
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
    }
}