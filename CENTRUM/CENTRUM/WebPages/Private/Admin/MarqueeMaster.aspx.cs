using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;


namespace CENTRUM.WebPages.Private.Admin
{
    public partial class MarqueeMaster : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                StatusButton("View");
                ViewState["StateEdit"] = null;
                LoadGrid();
                tabMarquee.ActiveTabIndex = 0;
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
                this.PageHeading = "Marquee";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuMarquee);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    //btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnEdit.Visible = false;
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Marquee", false);
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
        private void LoadGrid()
        {
            DataTable dt = null;
            Cmarquee oRole = null;
            try
            {
                oRole = new Cmarquee();
                dt = oRole.GetMarquee();
                ViewState["Marquee"] = dt;
                gvMarquee.DataSource = dt.DefaultView;
                gvMarquee.DataBind();
            }
            finally
            {
                dt = null;
                oRole = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            if (txtMarquee.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Marquee Name cannot left blank....");
                gblFuction.focus("ctl00_cph_Main_tabMarquee_pnlDtl_txtMarquee");
                vResult = false;
            }
            return vResult;
        }

        /// <summary>
        /// Enable Control
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtMarquee.Enabled = Status;
            chkActive.Enabled = Status;
        }

        /// <summary>
        /// Clear Controls
        /// </summary>
        private void ClearControls()
        {
            txtMarquee.Text = "";
            chkActive.Checked = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 dChk = 0, vMarqueeId = 0;
            Cmarquee oRole = null;
            CGblIdGenerator oGbl = null;
            string vActive = "";
            DataTable dt = null;
            Int32 vErr = 0;
            if (chkActive.Checked == true)
                vActive = "Y";
            else
                vActive = "N";
            try
            {
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                vMarqueeId = Convert.ToInt32(ViewState["MarqueeId"]);
                if (Mode == "Save")
                {
                    if (ValidateFields() == false)
                        return false;
                    oGbl = new CGblIdGenerator();
                    dChk = oGbl.ChkDuplicate("MarqueeMst", "MarqueeName", txtMarquee.Text.Replace("'", "''"), "", "", "MarqueeId", vMarqueeId.ToString(), "Save");
                    if (dChk > 0)
                    {
                        gblFuction.MsgPopup("Marquee Name can not be Duplicate..");
                        return false;
                    }
                    oRole = new Cmarquee();
                    if (vActive == "Y")
                    {
                        dt = oRole.GetActiveMarquee(0);
                        if (dt.Rows.Count > 0)
                        {
                            gblFuction.MsgPopup("Other Merquee is active now...");
                            return false;
                        }
                    }
                    vErr = oRole.SaveMarquee(ref vMarqueeId, txtMarquee.Text.Replace("'", "''"), vActive, this.UserID, "Save");
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["MarqueeId"] = vMarqueeId;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }

                }
                else if (Mode == "Edit")
                {
                    if (ValidateFields() == false)
                        return false;
                    oRole = new Cmarquee();
                    oGbl = new CGblIdGenerator();
                    dChk = oGbl.ChkDuplicate("MarqueeMst", "MarqueeName", txtMarquee.Text.Replace("'", "''"), "", "", "MarqueeId", vMarqueeId.ToString(), "Edit");
                    if (dChk > 0)
                    {
                        gblFuction.MsgPopup("Marquee Name can not be Duplicate..");
                        return false;
                    }
                    if (vActive == "Y")
                    {
                        dt = oRole.GetActiveMarquee(vMarqueeId);
                        if (dt.Rows.Count > 0)
                        {
                            gblFuction.MsgPopup("Other Merquee is active now...");
                            return false;
                        }
                    }
                    vErr = oRole.SaveMarquee(ref vMarqueeId, txtMarquee.Text.Replace("'", "''"), vActive, this.UserID, "Edit");
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

                    oRole = new Cmarquee();
                    vErr = oRole.SaveMarquee(ref vMarqueeId, txtMarquee.Text.Replace("'", "''"), vActive, this.UserID, "Del");
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
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oRole = null;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMarquee_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vMarqueeId = 0;
            DataTable dt = null;
            Cmarquee oRole = null;
            try
            {
                vMarqueeId = Convert.ToInt32(e.CommandArgument);
                ViewState["MarqueeId"] = vMarqueeId;
                if (e.CommandName == "cmdShow")
                {
                    oRole = new Cmarquee();
                    dt = oRole.GetMarqueeById(vMarqueeId);
                    if (dt.Rows.Count > 0)
                    {
                        txtMarquee.Text = Convert.ToString(dt.Rows[0]["MarqueeName"]);
                        if (Convert.ToString(dt.Rows[0]["Active"]) == "Y")
                            chkActive.Checked = true;
                        else
                            chkActive.Checked = false;
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tabMarquee.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oRole = null;
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
                tabMarquee.ActiveTabIndex = 0;
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
                ViewState["StateEdit"] = null;
                ViewState["Marquee"] = null;
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
                ViewState["StateEdit"] = "Edit";
                gblFuction.focus("ctl00_cph_Main_tabMarquee_pnlDtl_txtMarquee");
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
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadGrid();
                    StatusButton("Delete");
                    tabMarquee.ActiveTabIndex = 0;
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
                ViewState["StateEdit"] = null;
                tabMarquee.ActiveTabIndex = 1;
                StatusButton("Add");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnAdd.Enabled = false;
                    //btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    //btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
    }
}