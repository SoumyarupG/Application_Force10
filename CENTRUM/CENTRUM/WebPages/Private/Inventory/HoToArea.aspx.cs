using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Inventory
{
    public partial class HoToArea : CENTRUMBase 
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
                txtRecDt.Text = Session[gblValue.LoginDate].ToString();  
                ViewState["StateEdit"] = null;
                ViewState["ItmData"] = null;
                ViewState["ItemData"] = null;
                popArea();
                popItem();
                LoadGrid(0);
                tbRole.ActiveTabIndex = 0;
                StatusButton("View");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popArea()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "AreaId", "AreaName", "AreaMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlArea.DataSource = dt;
                ddlArea.DataTextField = "AreaName";
                ddlArea.DataValueField = "AreaId";
                ddlArea.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlArea.Items.Insert(0, oli);
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
        private void popItem()
        {
            DataTable dt = null;
            CItem oItm = null;
            DateTime vDate = gblFuction.setDate(txtRecDt.Text);
            try
            {
                oItm = new CItem();
                dt = oItm.GetItemBalance(vDate, "0000", 0, "B");
                ViewState["ItemData"] = dt;
                SetInitilize();
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
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Head Office to Area";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuHoToArea);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Head Office to Area", false);
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
            string vMod = string.Empty;
            CItem oItm = null;
            try
            {
                DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                if (txtFrmDt.Text != "" || txtToDt.Text != "")
                    vMod = "C";
                oItm = new CItem();
                dt = oItm.GetHoToAreaPG(pPgIndx, ref vRows, vFrmDt, vToDt, vMod);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvItm_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dtItm = null;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    dtItm = (DataTable)ViewState["ItemData"];
                    DropDownList ddlItm = (DropDownList)e.Row.FindControl("ddlItm");
                    ddlItm.DataTextField = "Name";
                    ddlItm.DataValueField = "ItemId";
                    ddlItm.DataSource = dtItm;
                    ddlItm.DataBind();
                    ListItem oL = new ListItem("<-- Select -->", "-1");
                    ddlItm.Items.Insert(0, oL);
                    ddlItm.SelectedIndex = ddlItm.Items.IndexOf(ddlItm.Items.FindByValue(e.Row.Cells[5].Text));
                }
            }
            finally
            {
                dtItm = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvItm_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow Row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            int vRow = Row.RowIndex;
            if (e.CommandName == "cmdNewRec")
                NewRow();
            else if (e.CommandName == "cmdDelRec")
                DelRow(vRow);
        }

        /// <summary>
        /// 
        /// </summary>
        private void NewRow()
        {
            DataTable dtNew = null;
            if (ViewState["ItmData"] != null)
            {
                dtNew = (DataTable)ViewState["ItmData"];
                if (dtNew.Rows.Count > 0)
                {
                    DataRow dR = dtNew.NewRow();
                    dR["SlNo"] = dtNew.Rows.Count + 1;
                    dtNew.Rows.Add(dR);
                    ViewState["ItmData"] = dtNew;
                    for (int i = 0; i < dtNew.Rows.Count - 1; i++)
                    {
                        DropDownList ddlItm = (DropDownList)gvItm.Rows[i].FindControl("ddlItm");
                        dtNew.Rows[i]["ItemId"] = ddlItm.SelectedValue;
                        TextBox txtQty = (TextBox)gvItm.Rows[i].FindControl("txtQty");                       
                        dtNew.Rows[i]["BQty"] = getBalQtyById(Int32.Parse(ddlItm.SelectedValue)); //Get Balance Qty
                        dtNew.Rows[i]["Qty"] = txtQty.Text;
                        if (Int32.Parse(dtNew.Rows[i]["Qty"].ToString()) > Int32.Parse(dtNew.Rows[i]["BQty"].ToString()))
                        {
                            gblFuction.MsgPopup("Stock Overflow. Please rectify the record....");
                            dtNew.Rows[dtNew.Rows.Count - 2].Delete();
                            return;
                        }
                        dtNew.Rows[i]["ItemCode"] = ddlItm.SelectedItem.Text;
                    }
                }                               
                //Check duplicate row
                var vDup = dtNew.AsEnumerable()
                    .GroupBy(dr => dr.Field<string>("ItemId"))
                    .Where(g => g.Count() > 1)
                    .SelectMany(g => g)
                    .ToList();
                if (Convert.ToInt32(vDup.Count) > 0)
                {
                    gblFuction.MsgPopup("Duplicate Item. Please rectify the record....");
                    dtNew.Rows[dtNew.Rows.Count-2].Delete(); 
                    return;
                }
                gvItm.DataSource = dtNew;
                gvItm.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pItemId"></param>
        /// <returns></returns>
        private string getBalQtyById(Int32 pItemId)
        {
            string vBalQty = string.Empty;
            DataTable dt = (DataTable)ViewState["ItemData"];
            var vBals = from row in dt.AsEnumerable()
                          where row.Field<Int32>("ItemId") == pItemId
                          select row;
            DataTable dt1 = vBals.CopyToDataTable();
            vBalQty = Convert.ToString(dt1.Rows[0][2]);
            return vBalQty;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pItemName"></param>
        /// <returns></returns>
        private string getItemId(string pItemName)
        {
            string vItemId = string.Empty;
            DataTable dt = (DataTable)ViewState["ItemData"];
            var results = from rows in dt.AsEnumerable()
                          where rows.Field<string>("Name") == pItemName
                          select rows;
            DataTable dt1 = results.CopyToDataTable();
            vItemId =Convert.ToString(dt1.Rows[0][0]); 
            return vItemId;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRow"></param>
        private void DelRow(Int32 pRow)
        {
            DataTable dt = (DataTable)ViewState["ItmData"];
            dt.Rows[pRow].Delete();
            dt.AcceptChanges();
            ViewState["ItmData"] = dt;
            gvItm.DataSource = dt;
            gvItm.DataBind();
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
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtRecDt.Enabled = Status;         
            ddlArea.Enabled = Status;
            gvItm.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtRecDt.Text = Session[gblValue.LoginDate].ToString();          
            ddlArea.SelectedIndex = -1;
            SetInitilize();
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 dChk = Convert.ToInt32(ViewState["VenId"]), vAreaId = 0;
            CItem oItm = null;
            DataTable dtDtl = null, dt = null;
            string vXml = string.Empty;

            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                string vVendId = Convert.ToString(ViewState["VenId"]);
                vAreaId = Convert.ToInt32(ddlArea.SelectedValue);
                DateTime vChlnDt = gblFuction.setDate(string.Empty); //txtChDt.Text
                DateTime vRecvDt = gblFuction.setDate(txtRecDt.Text);

                dtDtl = (DataTable)ViewState["ItmData"];
                dt = new DataTable();
                dt = dtDtl.Clone();
                foreach (GridViewRow gr in gvItm.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["SlNo"] = 1;                    
                    DropDownList ddlItm = (DropDownList)gr.FindControl("ddlItm");
                    dr["ItemId"] =getItemId(ddlItm.SelectedItem.Text); 
                    TextBox txtQty = (TextBox)gr.FindControl("txtQty");
                    if (txtQty.Text == "")
                        dr["Qty"] = "0";
                    else
                        dr["Qty"] = txtQty.Text;   
                    if (dr["ItemId"].ToString() != "&nbsp;")
                    {
                        dt.Rows.Add(dr);
                        dt.AcceptChanges();
                    }
                }
                if (dt.Rows.Count <= 0)
                {
                    gblFuction.MsgPopup("Please enter valid data.");
                    return false;
                }
                dt.TableName = "Table1";
                vXml = DtToXml(dt);
                if (Mode == "Save")
                {
                    oItm = new CItem();
                    dChk = oItm.SaveHoToArea(ref dChk, vAreaId, "", vChlnDt, vRecvDt, this.UserID, vBrCode, vXml, "Save");
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
                    oItm = new CItem();
                    dChk = oItm.SaveHoToArea(ref dChk, vAreaId, "", vChlnDt, vRecvDt, this.UserID, vBrCode, vXml, "Edit");
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
                    oItm = new CItem();
                    dChk = oItm.SaveHoToArea(ref dChk, vAreaId, "", vChlnDt, vRecvDt, this.UserID, vBrCode, vXml, "Del");
                    if (dChk > 0)
                        vResult = true;
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
                oItm = null;
                dtDtl = null;
                dt = null;
            }
        }             


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtXml"></param>
        /// <returns></returns>
        private string DtToXml(DataTable dtXml)
        {
            string vXml = string.Empty;
            try
            {
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString();
                }
                return vXml;
            }
            finally
            {
                dtXml = null;
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
            Int32 vRecId = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            DataSet ds = null;
            CItem oItm = null;
            try
            {
                vRecId = Convert.ToInt32(e.CommandArgument);
                ViewState["VenId"] = vRecId;
                if (e.CommandName == "cmdShow")
                {
                    oItm = new CItem();
                    ds = oItm.GetHoToAreaById(vRecId);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        txtRecDt.Text = Convert.ToString(dt.Rows[0]["RecvDt"]);                     
                        ddlArea.SelectedIndex = ddlArea.Items.IndexOf(ddlArea.Items.FindByValue(Convert.ToString(dt.Rows[0]["AreaId"])));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbRole.ActiveTabIndex = 1;
                        StatusButton("Show");
                        btnAdd.Visible = true;
                    }
                    gvItm.DataSource = dt1;
                    gvItm.DataBind();
                    ViewState["ItmData"] = dt1;
                }
            }
            finally
            {
                dt = null;
                dt1 = null;
                ds = null;
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

        /// <summary>
        /// 
        /// </summary>
        private void SetInitilize()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SlNo", typeof(int)));
            dt.Columns.Add(new DataColumn("ItemId", typeof(string)));
            dt.Columns.Add(new DataColumn("ItemCode", typeof(string)));
            dt.Columns.Add(new DataColumn("Qty", typeof(string)));
            dt.Columns.Add(new DataColumn("BQty", typeof(string)));
            DataRow dr = dt.NewRow();
            dr["SlNo"] = 1;
            dr["ItemId"] = "0";
            dr["ItemCode"] = "";
            dr["Qty"] = "0";
            dr["BQty"] = "0";
            dt.Rows.Add(dr);
            ViewState["ItmData"] = dt;
            gvItm.DataSource = dt;
            gvItm.DataBind();
        }

        protected void txtRecDt_TextChanged(object sender, EventArgs e)
        {
            popItem();
        }
    }
}