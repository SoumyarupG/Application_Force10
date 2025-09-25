using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.Web;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class RegionMaster : CENTRUMBase
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
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                popState();
                LoadGrid();
                tbReg.ActiveTabIndex = 0;
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
                this.PageHeading = "Region";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuRegionMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Region Master", false);
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
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            dt = oRpt.RptRegionMst();
            
            string vFileNm = "attachment;filename=Region_Master_Details.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
            Response.Write("<table border='1' cellpadding='5' widht='120%'>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>Region List</font></b></td></tr>");
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
                    if (dt.Columns[j].ColumnName.ToString() == "RBI_State_Code")
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
                tbReg.ActiveTabIndex = 1;
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
            tbReg.ActiveTabIndex = 0;
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
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid()
        {
            DataTable dt = null;            
            CRegion oReg = null;
            try
            {                
                oReg = new CRegion();
                dt = oReg.GetRegionList();               
                gvReg.DataSource = dt.DefaultView;
                gvReg.DataBind();
            }
            finally
            {
                oReg = null;
                dt=null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReg_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 pRegId = 0, vRow = 0;
            DataTable dt = null;
            CRegion oReg = null;
            try
            {
                pRegId = Convert.ToInt32(e.CommandArgument);
                ViewState["RegId"] = pRegId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvReg.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oReg = new CRegion();
                    dt = oReg.GetRegionDetails(pRegId);                    
                    if (dt.Rows.Count > 0)
                    {
                        txtRegion.Text = Convert.ToString(dt.Rows[vRow]["RegionName"]);
                        ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(dt.Rows[vRow]["StateId"].ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tbReg.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt=null;
                oReg = null;
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
            Int32 vErr = 0, vRec = 0, vStatId = 0, vNewId = 0, vRegId=0;
            CRegion oReg = null;
            CGblIdGenerator oGbl = null;
            try
            {
                if (ViewState["RegId"] != null)
                    vRegId=Convert.ToInt32(ViewState["RegId"].ToString());
                vStatId = Convert.ToInt32(ddlState.SelectedValue);                
                if (Mode == "Save")
                {
                    oReg = new CRegion();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("RegionMst", "RegionName", txtRegion.Text.Replace("'", "''"), "", "", "RegionId", vRegId.ToString(), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Region Can not be Duplicate...");
                        return false;
                    }
                    vErr = oReg.SaveRegion(ref vNewId, vRegId, txtRegion.Text.Replace("'", "''"), vStatId, this.UserID, "Save");
                    if (vErr > 0)
                    {
                        ViewState["RegId"] = vNewId;
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
                    oReg = new CRegion();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("RegionMst", "RegionName", txtRegion.Text.Replace("'", "''"), "", "", "RegionId", vRegId.ToString(), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Region Can not be Duplicate...");
                        return false;
                    }
                    vErr = oReg.SaveRegion(ref vNewId, vRegId, txtRegion.Text.Replace("'", "''"), vStatId, this.UserID, "Edit");
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
                    oGbl = new CGblIdGenerator();
                    oReg = new CRegion();
                    vErr = oGbl.ChkDelete(vRegId, "RegionId", "AreaMst");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.RecordUseMsg);
                        return false;
                    }
                    vErr = oReg.SaveRegion(ref vNewId, vRegId, txtRegion.Text.Replace("'", "''"), vStatId, this.UserID, "Delet");
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
            finally
            {
                oReg = null;
                oGbl = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtRegion.Enabled = Status;
            ddlState.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtRegion.Text = "";
            ddlState.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
        }
    }
}