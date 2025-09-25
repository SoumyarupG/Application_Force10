using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class DesignationMaster : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
                    StatusButton("View");
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                LoadGrid();
                tbStat.ActiveTabIndex = 0;
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);
                
                this.Menu = false;
                this.PageHeading = "Designation Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString()+ " ( Login Date " + Session[gblValue.LoginDate].ToString()  + " )";
                this.GetModuleByRole(mnuID.mnuDesignationMst);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;   
                //if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
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
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Designation Master", false);
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
                tbStat.ActiveTabIndex = 1;
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
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                //    gblFuction.MsgPopup("Branch can not Delete Account Group...");
                //    return;
                //}
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    LoadGrid();
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
                //if (Convert.ToInt32(ViewState["DesignationID"]) <= 18)
                //{
                //    gblFuction.MsgPopup("Can not Edit this Designation");
                //    return;
                //}
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                //    gblFuction.MsgPopup("Branch can not Edit Account Group...");
                //    return;
                //}
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
            tbStat.ActiveTabIndex = 0;
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
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                LoadGrid();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        private void LoadGrid()
        {
            DataTable dt = null;
            string vBrCode = "";
            CEO oDesig= null;
            try
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oDesig = new CEO();
                dt = oDesig.GetDesignationList();
                dt.PrimaryKey = new DataColumn[] { dt.Columns["DesignationID"] };
                ViewState["Designation"] = dt;
                gvStat.DataSource = dt.DefaultView;
                gvStat.DataBind();
            }
            finally
            {
                oDesig = null;
                dt=null;
            }
        }
        protected void gvStat_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 pDesignationID = 0, vRow = 0;
            DataTable dt = null;
            try
            {
                pDesignationID = Convert.ToInt32(e.CommandArgument);
                ViewState["DesignationID"] = pDesignationID;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvStat.Rows)
                    {
                        if ((gr.RowIndex) % 2 == 0)
                        {
                            gr.BackColor = backColor;
                            gr.ForeColor = foreColor;
                        }
                        else
                        {
                            gr.BackColor = System.Drawing.Color.White;
                            gr.ForeColor = foreColor;
                        }
                        gr.Font.Bold = false;
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                        lb.Font.Bold = false;
                    }
                    gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#151B54");
                    gvRow.ForeColor = System.Drawing.Color.White;
                    gvRow.Font.Bold = true;
                    btnShow.ForeColor = System.Drawing.Color.White;
                    btnShow.Font.Bold = true;

                    dt = (DataTable)ViewState["Designation"];
                    vRow = dt.Rows.IndexOf(dt.Rows.Find(pDesignationID));
                    if (dt.Rows.Count > 0)
                    {
                        txtDesignation.Text = Convert.ToString(dt.Rows[vRow]["Designation"]);
                        txtShortName.Text = Convert.ToString(dt.Rows[vRow]["SrtNm"]);
                        lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tbStat.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt=null;
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vDesignationID = Convert.ToString(ViewState["DesignationID"]), vShortname="";
            Int32 vErr = 0, vRec = 0, vDesigId = 0, vNewId=0;
            CEO oStat = null;
            CGblIdGenerator oGbl = null;
            try
            {
                vShortname = txtShortName.Text;
                vDesigId = Convert.ToInt32(ViewState["DesignationID"]);
                if (Mode == "Save")
                {
                    oStat = new CEO();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("DesignationMst", "Designation", txtDesignation.Text.Replace("'", "''"), "", "", "DesignationID", vDesignationID, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Designation Can not be Duplicate...");
                        return false;
                    }
                    vRec = oGbl.ChkDuplicate("DesignationMst", "SrtNm", txtShortName.Text.Replace("'", "''"), "", "", "DesignationID", vDesignationID, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Designation Short Name Can not be Duplicate...");
                        return false;
                    }
                    vErr = oStat.SaveDesignationMst(ref vNewId, vDesigId, txtDesignation.Text.Replace("'", "''"), vShortname, this.UserID, "Save");
                    if (vErr > 0)
                    {
                        ViewState["DesignationID"] = vNewId;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oStat = new CEO();
                    oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("StateMst", "StateName", txtState.Text.Replace("'", "''"), "", "", "StateId", vStaId, "Edit");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("State Can not be Duplicate...");
                    //    return false;
                    //}
                    vRec = oGbl.ChkDuplicate("DesignationMst", "Designation", txtDesignation.Text.Replace("'", "''"), "", "", "DesignationID", vDesignationID, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Designation Can not be Duplicate...");
                        return false;
                    }
                    vRec = oGbl.ChkDuplicate("DesignationMst", "SrtNm", txtShortName.Text.Replace("'", "''"), "", "", "DesignationID", vDesignationID, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Designation Short Name Can not be Duplicate...");
                        return false;
                    }
                    vErr = oStat.SaveDesignationMst(ref vNewId, vDesigId, txtDesignation.Text.Replace("'", "''"), vShortname, this.UserID, "Edit");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oStat = new CEO();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDelete(vDesigId, "DesignationID", "UserMst");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("The Designation Already Assigned, You Can Not Delete The Designation.");
                        return false;
                    }
                    vErr = oStat.SaveDesignationMst(ref vNewId, vDesigId, txtDesignation.Text.Replace("'", "''"), vShortname, this.UserID, "Delete");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
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
                oStat = null;
                oGbl = null;
            }
        }
        private void EnableControl(Boolean Status)
        {
            txtDesignation.Enabled = Status;
            if (Convert.ToInt32(ViewState["DesignationID"]) <= 18 && Convert.ToString(ViewState["StateEdit"])=="Edit")
            {
                txtShortName.Enabled = false;
            }
            else
            {
                txtShortName.Enabled = Status;
            }
            
        }
        private void ClearControls()
        {
            txtDesignation.Text = "";
            txtShortName.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
        }
    }   
}