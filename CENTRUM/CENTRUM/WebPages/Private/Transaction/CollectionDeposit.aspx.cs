using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.Data;
using System.IO;


namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class CollectionDeposit : CENTRUMBase
    {
        protected int cPgNo = 1;

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
                StatusButton("View");               
                txtCollDepositDate.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtCollDepositDate.Enabled = false;
                txtFrmDt.Text = txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                LoadGrid(0);
            }
        }

        private void GenerateGrid()
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable("Deposit");
                dt.Columns.Add("CollPointId");
                dt.Columns.Add("Amount");
                dt.Columns.Add("Eoid");
                DataRow dF;
                dF = dt.NewRow();
                dt.Rows.Add(dF);
                dt.AcceptChanges();
                ViewState["Deposit"] = dt;
                gvDeposit.DataSource = dt;
                gvDeposit.DataBind();
            }
            finally
            {
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
                this.PageHeading = "Collection Deposit";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCollDeposit);
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
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Collection Deposit", false);
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
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            //txtCollDepositDate.Enabled = Status;
            gvDeposit.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            //txtCollDepositDate.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
            gvDeposit.DataSource = null;
            gvDeposit.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CCollectionPoint oQua = null;
            Int32 vRows = 0;
            try
            {
                oQua = new CCollectionPoint();
                dt = oQua.GetCollDepositPG(pPgIndx, ref vRows, Convert.ToString(Session[gblValue.BrnchCode]),gblFuction.setDate(txtFrmDt.Text),gblFuction.setDate(txtToDt.Text));
                gvCollPoint.DataSource = dt.DefaultView;
                gvCollPoint.DataBind();
                if (dt.Rows.Count <= 0)
                {
                    lblTotPg.Text = "0";
                    lblCrPg.Text = "0";
                }
                else
                {
                    lblTotPg.Text = CalTotPgs(vRows).ToString();
                    lblCrPg.Text = cPgNo.ToString();
                }
                if (cPgNo == 1)
                {
                    btnPrev.Enabled = false;
                    if (Int32.Parse(lblTotPg.Text) > 0 && cPgNo != Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                }
                else
                {
                    btnPrev.Enabled = true;
                    if (cPgNo == Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
                }
            }
            finally
            {
                dt = null;
                oQua = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRows"></param>
        /// <returns></returns>
        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
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
                case "Previous":
                    cPgNo = Int32.Parse(lblTotPg.Text) - 1; //lblCrPg
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCrPg.Text) + 1; //lblTotPg
                    break;
            }
            LoadGrid(cPgNo);
            tbQly.ActiveTabIndex = 0;
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCollPoint_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vDepoId = 0;
            DataTable dt = null, dt2 = null;
            DataSet ds = null;
            CCollectionPoint oQua = null;
            try
            {
                vDepoId = Convert.ToInt32(e.CommandArgument);
                ViewState["DepoId"] = vDepoId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvCollPoint.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oQua = new CCollectionPoint();
                    ds = oQua.GetCollDepositbyId(vDepoId);
                    dt = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        txtCollDepositDate.Text = Convert.ToString(dt.Rows[0]["DepoDate"]).Trim();
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbQly.ActiveTabIndex = 1;

                        gvDeposit.DataSource = dt2.DefaultView;
                        gvDeposit.DataBind();
                        ViewState["Deposit"] = dt2;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oQua = null;
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
            string vDepositId = Convert.ToString(ViewState["DepoId"]);
            Int32 vErr = 0, vRec = 0, vDepoId = 0, vErr1 = 0;
            CCollectionPoint oQua = null;
            CLoanRecovery oLR = null;
            DataTable dtXml = null;
            string vXml = "";
            try
            {
                vDepoId = Convert.ToInt32(ViewState["DepoId"]);
                dtXml = GetDepositData();
                if (dtXml.Rows.Count == 0)
                {
                    gblFuction.AjxMsgPopup("Please add atleast one Deposit Details.");
                    return false;
                }

                if (Mode == "Save" || Mode == "Edit")
                {
                    foreach (DataRow dr in dtXml.Rows)
                    {
                        if (Convert.ToString(dr["Amount"]) == "")
                        {
                            gblFuction.AjxMsgPopup("Please enter valid amount..");
                            return false;
                        }
                        else
                        {
                            if (Convert.ToDouble(dr["Amount"]) == 0)
                            {
                                gblFuction.AjxMsgPopup("Please enter valid amount..");
                                return false;
                            }
                        }
                    }
                }
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString();
                }

                oLR = new CLoanRecovery();
                vErr1 = oLR.CashReconChkFortheDay(gblFuction.setDate(txtCollDepositDate.Text), Convert.ToString(Session[gblValue.BrnchCode]));
                if (vErr1 > 0)
                {
                    gblFuction.AjxMsgPopup("Cash Reconciliation already done for the day..");
                    return false;
                }

                if (this.RoleId != 1)
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtCollDepositDate.Text))
                        {
                            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                            return false;
                        }
                    }
                }
                if (Mode == "Save")
                {
                    oQua = new CCollectionPoint();
                    vErr = oQua.InsertDeposit(ref vDepoId, gblFuction.setDate(txtCollDepositDate.Text), Convert.ToString(Session[gblValue.BrnchCode]), vXml, Convert.ToInt32(Session[gblValue.UserId]), "I", "Save");
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["DepoId"] = vDepoId;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oQua = new CCollectionPoint();
                    vErr = oQua.InsertDeposit(ref vDepoId, gblFuction.setDate(txtCollDepositDate.Text), Convert.ToString(Session[gblValue.BrnchCode]), vXml, Convert.ToInt32(Session[gblValue.UserId]), "E", "Edit");
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
                    vRec = 0;
                    if (vRec <= 0)
                    {
                        oQua = new CCollectionPoint();
                        vErr = oQua.InsertDeposit(ref vDepoId, gblFuction.setDate(txtCollDepositDate.Text), Convert.ToString(Session[gblValue.BrnchCode]), vXml, Convert.ToInt32(Session[gblValue.UserId]), "D", "Delet");
                        if (vErr > 0)
                            vResult = true;
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
                oQua = null;
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
            ViewState["QualificationId"] = null;
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
                tbQly.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
                GenerateGrid();
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
                    ClearControls();
                    tbQly.ActiveTabIndex = 0;
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
                LoadGrid(0);
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
            tbQly.ActiveTabIndex = 0;
            EnableControl(false);
            ClearControls();
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
                LoadGrid(0);
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }

        protected void gvDeposit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = null, dt1 = null;
            CGblIdGenerator oGbl = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlCollPoint = (DropDownList)e.Row.FindControl("ddlCollPoint");
                    DropDownList ddlRO = (DropDownList)e.Row.FindControl("ddlRO");
                    oGbl = new CGblIdGenerator();
                    dt = oGbl.PopComboMIS("N", "N", "AA", "CollPointId", "CollPointName", "CollectionPointMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), "0000");
                    ddlCollPoint.DataSource = dt;
                    ddlCollPoint.DataTextField = "CollPointName";
                    ddlCollPoint.DataValueField = "CollPointId";
                    ddlCollPoint.DataBind();
                    ListItem oL1 = new ListItem("<-- Select -->", "-1");
                    ddlCollPoint.Items.Insert(0, oL1);
                    ddlCollPoint.SelectedIndex = ddlCollPoint.Items.IndexOf(ddlCollPoint.Items.FindByValue(e.Row.Cells[0].Text));

                    oRO = new CEO();
                    dt1 = oRO.PopRO(vBrCode, "0", "0", vLogDt, Convert.ToInt32(Session[gblValue.UserId].ToString()));
                    ddlRO.DataSource = dt1;
                    ddlRO.DataTextField = "EoName";
                    ddlRO.DataValueField = "Eoid";
                    ddlRO.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlRO.Items.Insert(0, oli);
                    ddlRO.SelectedIndex = ddlRO.Items.IndexOf(ddlRO.Items.FindByValue(e.Row.Cells[1].Text));
                }
            }
            finally
            {
                dt = null;
                dt1 = null;
                oRO = null;
                oGbl = null;
            }
        }

        protected void btnAddNew1_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            Int32 vR = 0;
            DataRow dr;
            dt = (DataTable)ViewState["Deposit"];
            if (dt.Rows.Count > 0)
            {
                vR = dt.Rows.Count - 1;
                DropDownList ddlCollPoint = (DropDownList)gvDeposit.Rows[vR].FindControl("ddlCollPoint");
                dt.Rows[vR]["CollPointId"] = ddlCollPoint.SelectedValue;

                DropDownList ddlRO = (DropDownList)gvDeposit.Rows[vR].FindControl("ddlRO");
                dt.Rows[vR]["Eoid"] = ddlRO.SelectedValue;

                TextBox txtAmt = (TextBox)gvDeposit.Rows[vR].FindControl("txtAmt");
                dt.Rows[vR]["Amount"] = txtAmt.Text == "" ? "0" : txtAmt.Text;

            }
            dt.AcceptChanges();

            if (dt.Rows[vR]["CollPointId"].ToString() != "-1")
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            else
            {
                gblFuction.MsgPopup("Please Select Collection Point Name...");
            }
            ViewState["Deposit"] = dt;
            gvDeposit.DataSource = dt;
            gvDeposit.DataBind();
        }

        protected void gvDeposit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDel2")
            {
                DataTable dt = null;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                dt = (DataTable)ViewState["Deposit"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[index].Delete();
                    dt.AcceptChanges();
                    ViewState["Deposit"] = dt;
                    gvDeposit.DataSource = dt;
                    gvDeposit.DataBind();
                }
                else
                {
                    gblFuction.MsgPopup("First Row can not be deleted.");
                    return;
                }
            }
        }

        private DataTable GetDepositData()
        {
            DataTable dt = new DataTable("Table1");
            dt.Columns.Add("RowNo");
            dt.Columns.Add("CollPointId");
            dt.Columns.Add("EoId");
            dt.Columns.Add("Amount");

            foreach (GridViewRow gr in gvDeposit.Rows)
            {
                Label lblRowNumber = (Label)gr.FindControl("lblRowNumber");
                DropDownList ddlCollPoint = (DropDownList)gr.FindControl("ddlCollPoint");
                DropDownList ddlRO = (DropDownList)gr.FindControl("ddlRO");
                TextBox txtAmt = (TextBox)gr.FindControl("txtAmt");
                if (ddlRO.SelectedValue != "-1" && ddlCollPoint.SelectedValue != "-1")
                {
                    DataRow dr = dt.NewRow();
                    dr["RowNo"] = lblRowNumber.Text;
                    dr["EoId"] = ddlRO.SelectedValue;
                    dr["CollPointId"] = ddlCollPoint.SelectedValue;
                    dr["Amount"] = txtAmt.Text;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
            }
            return dt;
        }
    }
}