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
    public partial class Mob_Role : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                StatusButton("View");
                ViewState["StateEdit"] = null;
                PopEmp();
                LoadGrid();
                tabRole.ActiveTabIndex = 0;
                StatusButton("View");
                popDevice();
            }
        }
   
       private void PopEmp()
        {    
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            CMob oCM = null;

            try
            {
                oCM = new CMob();
                dt = oCM.GetAllEoByBranch(vBrCode, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                ddlEmp.DataTextField = "EOName";
                ddlEmp.DataValueField = "EOId";
                ddlEmp.DataSource = dt;
                ddlEmp.DataBind();
                ListItem oItm = new ListItem("<--- Select --->", "-1");
                ddlEmp.Items.Insert(0, oItm);
            }
            finally
            {
                dt = null;
                oCM = null;
            }      
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Mobile User";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuUsrMobApp);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    
                    btnEdit.Visible = false;
                    
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    
                    btnEdit.Visible = false;
                    
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    
                    btnEdit.Visible = true;
                    
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    
                    btnEdit.Visible = true;
                    
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Mobile User Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void LoadGrid()
        {
            DataTable dt = null;
            CMob oRole = null;
            string vBranchCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oRole = new CMob();
                dt = oRole.GetAllMobUser(vBranchCode);
                ViewState["MobUser"] = dt;
                gvRole.DataSource = dt.DefaultView;
                gvRole.DataBind();
            }
            finally
            {
                dt = null;
                oRole = null;
            }
        }

        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    
                    
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    gblFuction.focus("ctl00_cph_Main_tabRole_pnlDtl_txtUser");
                    break;
                case "Show":
                   
                    btnEdit.Enabled = true;
                    
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    
                    btnEdit.Enabled = false;
                    
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    gblFuction.focus("ctl00_cph_Main_tabRole_pnlDtl_txtUser");
                    break;
                case "View":
                    
                    btnEdit.Enabled = false;
                    
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    
                    btnEdit.Enabled = false;
                    
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Exit":
                    
                    btnEdit.Visible = false;
                    
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        public void popDevice()
        {           
            DataTable dt = null;
            CMob oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CMob();
                dt = oRO.popDevice(vBrCode);
                ddlDevice.DataSource = dt;
                ddlDevice.DataTextField = "DeviceModel";
                ddlDevice.DataValueField = "DeviceID";
                ddlDevice.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlDevice.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            if (ddlEmp.SelectedValue=="-1")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Employee can not be left blank....");
                gblFuction.focus("ctl00_cph_Main_tabRole_pnlDtl_txtUser");
                vResult = false;
            }
            if (txtUser.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Name cannot be left blank....");
                gblFuction.focus("ctl00_cph_Main_tabRole_pnlDtl_txtUser");
                vResult = false;
            }
            
            
            return vResult;
        }

        private void EnableControl(Boolean Status)
        {
            ddlEmp.Enabled = Status;           
            txtUser.Enabled = false;
            ddlDevice.Enabled = Status;
            ddlAllowGalleryAccess.Enabled = Status;
            ddlActivateApkAccess.Enabled = Status;
            ddlManualEntry.Enabled = Status;
            ddlQRScan.Enabled = Status;
        }

        private void ClearControls()
        {
            txtUser.Text = "";
            ddlDevice.SelectedValue = "-1";
            ddlEmp.SelectedValue = "-1";

        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            //Int32 dChk = 0;
            CMob oRole = null;
            string vChkDuplicate = "";
            string vEoID = "-1", vMob_UserName = "";
            try
            {
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                Int32 dMobUserId = Convert.ToInt32(ViewState["Mob_UserId"]), vErr = 0;
                vEoID = ddlEmp.SelectedValue;
                vMob_UserName = txtUser.Text.Trim();               
                
                //if (Mode == "Save")
                //{
                //    if (ValidateFields() == false)
                //        return false;
                //    oRole = new CMob();
                //    oRole.ChkDuplicateMobUser(ddlEmp.SelectedValue, txtUser.Text.Replace("'", "''"), "Save", ref vChkDuplicate);
                //    if (vChkDuplicate == "Y")
                //    {
                //        gblFuction.MsgPopup("User Name can not be Duplicate..");
                //        return false;
                //    }
                //    dMobUserId = oRole.InsertMobUser(vEoID, vMob_UserName.Replace("'", "''"), vMob_Password.Replace("'", "''"), vMob_UserType, vAllow, vClientApp, vMemberMst, vCIBI, vLoanApp, vLoanSanc, vLoanDisb, vLoanColl,vNPS, ddlDevice.SelectedValue,vBrCode, this.UserID, vLogDt, "I");
                //    if (dMobUserId > 0)
                //    {
                //        ViewState["Mob_UserId"] = dMobUserId;
                //        vResult = true;
                //    }
                //    else
                //    {
                //        gblFuction.MsgPopup(gblMarg.DBError);
                //        vResult = false;
                //    }
                //}
                if (Mode == "Edit")
                {
                    if (ValidateFields() == false)
                        return false;
                    oRole = new CMob();
                    oRole.ChkDuplicateMobUser(ddlEmp.SelectedValue, txtUser.Text.Replace("'", "''"), "Edit", ref vChkDuplicate);
                    if (vChkDuplicate == "Y")
                    {
                        gblFuction.MsgPopup("User Name can not be Duplicate..");
                        return false;
                    }
                    vErr = oRole.InsertMobUser(dMobUserId, ddlDevice.SelectedValue, Convert.ToInt32(Session[gblValue.UserId].ToString()), vLogDt, "E", ddlAllowGalleryAccess.SelectedValue, ddlActivateApkAccess.SelectedValue, ddlManualEntry.SelectedValue,ddlQRScan.SelectedValue);
                    if (vErr > 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oRole = null;
            }
        }

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
                    LoadGrid();
                    StatusButton("View");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gvRole_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vMobUserId = 0;
            DataTable dt = null;
            CMob oRole = null;
            try
            {
                vMobUserId = Convert.ToInt32(e.CommandArgument);
                ViewState["Mob_UserId"] = vMobUserId;
                if (e.CommandName == "cmdShow")
                {
                    oRole = new CMob();
                    dt = oRole.GetMobUserById(vMobUserId);
                    if (dt.Rows.Count > 0)
                    {
                        txtUser.Text = Convert.ToString(dt.Rows[0]["Mob_UserName"]);
                        ddlEmp.SelectedIndex = ddlEmp.Items.IndexOf(ddlEmp.Items.FindByValue(dt.Rows[0]["EoID"].ToString()));
                        ddlDevice.SelectedIndex = ddlDevice.Items.IndexOf(ddlDevice.Items.FindByValue(dt.Rows[0]["DeviceID"].ToString()));
                        ddlAllowGalleryAccess.SelectedIndex = ddlAllowGalleryAccess.Items.IndexOf(ddlAllowGalleryAccess.Items.FindByValue(dt.Rows[0]["AllowGalleryAccess"].ToString()));
                        ddlActivateApkAccess.SelectedIndex = ddlActivateApkAccess.Items.IndexOf(ddlActivateApkAccess.Items.FindByValue(dt.Rows[0]["ActivateApkAccess"].ToString()));
                        ddlManualEntry.SelectedIndex = ddlManualEntry.Items.IndexOf(ddlManualEntry.Items.FindByValue(dt.Rows[0]["AllowManualEntry"].ToString()));
                        ddlQRScan.SelectedIndex = ddlQRScan.Items.IndexOf(ddlQRScan.Items.FindByValue(dt.Rows[0]["AllowQRScan"].ToString()));

                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tabRole.ActiveTabIndex = 1;
                        StatusButton("Show");
                        ddlEmp.Enabled = false;
                    }
                }
            }
            finally
            {
                dt = null;
                oRole = null;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                tabRole.ActiveTabIndex = 0;
                EnableControl(false);
                StatusButton("View");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["StateEdit"] = null;
                ViewState["MobUser"] = null;
                ViewState["Mob_UserId"] = null;
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
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
                gblFuction.focus("ctl00_cph_Main_tabRole_pnlDtl_txtUser");
                StatusButton("Edit");
                ddlEmp.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
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
                ViewState["StateEdit"] = null;
                tabRole.ActiveTabIndex = 1;
                StatusButton("Add");
                txtUser.Text = "";
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}