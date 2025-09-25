using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.Web;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class DistrictMaster : CENTRUMBase
    {      
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
                ViewState["State"] = null;
                popState();
                LoadGrid();
                tbDist.ActiveTabIndex = 0;
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
                this.PageHeading = "District";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDistrictMst);
                if (Session[gblValue.BrnchCode].ToString() != "0000") btnPrint.Visible = false;
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "District Master", false);
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
                    btnPrint.Enabled = false;
                    ClearControls();                  
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnPrint.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnPrint.Enabled = false;
                    EnableControl(true);                   
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnPrint.Enabled = true;
                    EnableControl(false);                   
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnPrint.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popState()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;            
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "StateId", "StateName", "StateMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
             
                ddlState.DataSource = dt;
                ddlState.DataTextField = "StateName";
                ddlState.DataValueField = "StateId";
                ddlState.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlState.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid()
        {
            DataTable dt = null;
            //Int32 vDistId = 0;
            string vBrCode = "";
            CDistrict oDist = null;
            try
            {
                //vDistId = Convert.ToInt32(Session[gblValue.DistrictId].ToString());
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oDist = new CDistrict();
                dt = oDist.GetDistrictList();
                dt.PrimaryKey = new DataColumn[] { dt.Columns["DistrictId"] };
                ViewState["State"] = dt;
                gvDist.DataSource = dt.DefaultView;
                gvDist.DataBind();
                tbDist.ActiveTabIndex = 0;
            }
            finally
            {
                oDist = null;
                dt=null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDist_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 pZoneId = 0, vRow = 0;
            DataTable dt = null;
            try
            {
                pZoneId = Convert.ToInt32(e.CommandArgument);
                ViewState["DistId"] = pZoneId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvDist.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    dt = (DataTable)ViewState["State"];
                    vRow = dt.Rows.IndexOf(dt.Rows.Find(pZoneId));
                    if (dt.Rows.Count > 0)
                    {
                        txtDist.Text = Convert.ToString(dt.Rows[vRow]["DistrictName"]);
                        TxtCity.Text = Convert.ToString(dt.Rows[vRow]["IDBIShortName"]);
                        ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(dt.Rows[vRow]["StateId"].ToString()));
                        txtRBICode.Text = Convert.ToString(dt.Rows[vRow]["RBI_Code"]);
                        lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tbDist.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt=null;
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
            string vDisId = Convert.ToString(ViewState["DistId"]);
            Int32 vErr = 0, vRec = 0, vDistId = 0, vNewId = 0;
            string vStatId = "";
            CDistrict oDist = null;
            CGblIdGenerator oGbl = null;
            try
            { 
                //DataTable dt = (DataTable)ViewState["State"];
                //vStatId = dt.Rows[0]["StateId"].ToString();
                vStatId = ddlState.SelectedValue;
                vDistId = Convert.ToInt32(ViewState["DistId"]);
                if (Mode == "Save")
                {
                    oDist = new CDistrict();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("DistrictMst", "DistrictName", txtDist.Text.Replace("'", "''"), "StateId", vStatId, "DistrictId", vDisId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("District Can not be Duplicate...");
                        return false;
                    }
                    vErr = oDist.SaveDistrict(ref vNewId, vDistId, txtDist.Text.Replace("'", "''"), Convert.ToInt32(vStatId), this.UserID,TxtCity.Text.Replace("'", "''"), "Save",txtRBICode.Text.Replace("'", "''"));
                    if (vErr > 0)
                    {
                        ViewState["DistId"] = vNewId;
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
                    oDist = new CDistrict();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("DistrictMst", "DistrictName", txtDist.Text.Replace("'", "''"), "StateId", vStatId, "DistrictId", vDisId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("District Can not be Duplicate...");
                        return false;
                    }
                    vErr = oDist.SaveDistrict(ref vNewId, vDistId, txtDist.Text.Replace("'", "''"), Convert.ToInt32(vStatId), this.UserID,TxtCity.Text.Replace("'", "''"), "Edit",txtRBICode.Text.Replace("'", "''"));
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
                    oDist = new CDistrict();
                    oGbl = new CGblIdGenerator();
                    vErr = oGbl.ChkDelete(vDistId, "DistrictId", "BlockMst");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.RecordUseMsg);
                        return false;
                    }
                    vErr = oDist.SaveDistrict(ref vNewId, vDistId, txtDist.Text.Replace("'", "''"), Convert.ToInt32(vStatId), this.UserID,TxtCity.Text.Replace("'", "''"), "Delet",txtRBICode.Text);
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
                oDist = null;
                oGbl = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtDist.Enabled = Status;
            ddlState.Enabled = Status;
            TxtCity.Enabled = Status;
            txtRBICode.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtDist.Text = "";
            ddlState.SelectedIndex = -1;           
            lblDate.Text = "";
            lblUser.Text = "";
            TxtCity.Text = "";
            txtRBICode.Text = "";
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            dt = oRpt.RptDistrictMst();

            string vFileNm = "attachment;filename=District_Master_Details.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
            Response.Write("<table border='1' cellpadding='5' widht='120%'>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>District List</font></b></td></tr>");
            //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>As on Date " + txtAsOnDt.Text + "</font></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'></font></b></td></tr>");
            string tab = string.Empty;
            Response.Write("<tr>");
            foreach (DataColumn dtcol in dt.Columns)
            {
                Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dtrow in dt.Rows)
            {
                Response.Write("<tr style='height:20px;'>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName.ToString() == "RBI_State_Code" || dt.Columns[j].ColumnName.ToString() == "RBI_District_Code")
                    {
                        Response.Write("<td &nbsp nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                    }
                    else
                    {

                        Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
                    }
                }
                Response.Write("</tr>");
            }
            Response.Write("</table>");
            Response.End();

        }
    }   
}