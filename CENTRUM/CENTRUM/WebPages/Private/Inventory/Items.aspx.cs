using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Inventory
{
    public partial class Items : CENTRUMBase
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
                StatusButton("View");
                ViewState["StateEdit"] = null;
                LoadGrid(0);
                tbRole.ActiveTabIndex = 0;
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
                this.PageHeading = "Item";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuItemMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Item", false);
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
            CItem oItm = null;
            try
            {
                oItm = new CItem();
                dt = oItm.GetItemsPG(pPgIndx, ref vRows, txtVName.Text.Replace("'","''"));
                gvRole.DataSource = dt;
                gvRole.DataBind();
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
            txtName.Enabled = Status;
            txtCode.Enabled = Status;
            txtRQty.Enabled = Status;
            ddlUnit.Enabled = Status;            
        }

        /// <summary>
        /// Clear Controls
        /// </summary>
        private void ClearControls()
        {
            txtName.Text = "";
            txtCode.Text = "";
            txtRQty.Text = "";
            ddlUnit.SelectedIndex = -1;         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 dChk = Convert.ToInt32(ViewState["VenId"]), vRec = 0;
            CItem oItm = null;
            CGblIdGenerator oGbl = null;

            try
            {
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                string vVendId = Convert.ToString(ViewState["VenId"]);

                if (Mode == "Save")
                {
                    oItm = new CItem();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("INV_ItemMst", "ItemName", txtName.Text.Replace("'", "''"), "", "", "ItemId", vVendId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Item Can not be Duplicate...");
                        return false;
                    }
                    dChk = oItm.SaveItem(ref dChk, txtCode.Text.Replace("'", "''"), txtName.Text.Replace("'", "''"),
                        Convert.ToInt32(ddlUnit.SelectedValue), Convert.ToDouble(txtRQty.Text), this.UserID, "Save");
                    if (dChk > 0)
                    {
                        ViewState["VenId"] = dChk;
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
                    oGbl = new CGblIdGenerator();
                    oItm = new CItem();
                    vRec = oGbl.ChkDuplicate("INV_ItemMst", "ItemName", txtName.Text.Replace("'", "''"), "", "", "ItemId", vVendId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Item Can not be Duplicate...");
                        return false;
                    }
                    
                    dChk = oItm.SaveItem(ref dChk, txtCode.Text.Replace("'", "''"), txtName.Text.Replace("'", "''"),
                        Convert.ToInt32(ddlUnit.SelectedValue), Convert.ToDouble(txtRQty.Text), this.UserID, "Edit");
                    if (dChk > 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oGbl = new CGblIdGenerator();
                    oItm = new CItem();
                    vRec = oGbl.ChkDelete(Convert.ToInt32(vVendId), "VenId", "INV_VendorMst");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.RecordUseMsg);
                        return false;
                    }
                    else
                    {
                        dChk = oItm.SaveItem(ref dChk, txtCode.Text.Replace("'", "''"), txtName.Text.Replace("'", "''"),
                        Convert.ToInt32(ddlUnit.SelectedValue), Convert.ToDouble(txtRQty.Text), this.UserID, "Del");
                        if (dChk > 0)
                            vResult = true;
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
                oItm = null;
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
        protected void gvRole_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vVendId = 0;
            DataTable dt = null;
            CItem oItm = null;
            try
            {
                vVendId = Convert.ToInt32(e.CommandArgument);
                ViewState["VenId"] = vVendId;
                if (e.CommandName == "cmdShow")
                {
                    oItm = new CItem();
                    dt = oItm.GetItemsById(vVendId);
                    if (dt.Rows.Count > 0)
                    {
                        txtName.Text = Convert.ToString(dt.Rows[0]["ItemName"]);
                        txtCode.Text = Convert.ToString(dt.Rows[0]["ItemCode"]);
                        ddlUnit.Text = Convert.ToString(dt.Rows[0]["Unit"]);
                        txtRQty.Text = Convert.ToString(dt.Rows[0]["RQty"]);
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