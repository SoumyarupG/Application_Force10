using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;
using System.Web.UI;

namespace CENTRUM_VRIDDHIVYAPAR.WebPages.Private.Master
{
    public partial class LoanProductMaster : CENTRUMBAse
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
                LoadGrid(0);
                PopBCMst();
            }
        }

        private void PopBCMst()
        {
            DataTable dt = null;
            CGblIdGenerator oGbl = null;
            try
            {
                oGbl = new CGblIdGenerator();
                dt = oGbl.PopComboMIS("N", "N", "AA", "BCId", "BCName", "BCMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000"); 
                ddlBC.DataSource = dt;
                ddlBC.DataTextField = "BCName";
                ddlBC.DataValueField = "BCId";
                ddlBC.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlBC.Items.Insert(0, oli1);
            }
            finally
            {
                oGbl = null;
                dt = null;
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
                this.PageHeading = "Loan Product";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuProduct);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Product Master", false);
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
            txtProduct.Enabled = Status;
            txtShtName.Enabled = Status;
            txtProduct.Enabled = Status;
            chkBC.Enabled = Status;
            ddlBC.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtProduct.Text = "";
            txtShtName.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
            chkBC.Checked = false;
            
            ddlBC.SelectedIndex = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CProduct oPM = null;
            Int32 vRows = 0;
            try
            {
                oPM = new CProduct();
                dt = oPM.GetProductPG(pPgIndx, ref vRows);
                gvProd.DataSource = dt.DefaultView;
                gvProd.DataBind();
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
                oPM = null;
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
            tbProd.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vProdctId = 0;
            DataTable dt = null;
            CProduct oPM = null;
            try
            {
                vProdctId = Convert.ToInt32(e.CommandArgument);
                ViewState["ProdctId"] = vProdctId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvProd.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oPM = new CProduct();
                    dt = oPM.GetProductbyId(vProdctId);
                    if (dt.Rows.Count > 0)
                    {
                        txtProduct.Text = Convert.ToString(dt.Rows[0]["Product"]).Trim();
                        txtShtName.Text = Convert.ToString(dt.Rows[0]["SName"]).Trim();
                        if (Convert.ToString(dt.Rows[0]["IsBC"]).Trim() == "Y")
                        {
                            chkBC.Checked = true;
                            ddlBC.Enabled = true;
                        }
                        else
                        {
                            chkBC.Checked = false;
                            ddlBC.Enabled = false;
                        }
                        ddlBC.SelectedIndex = ddlBC.Items.IndexOf(ddlBC.Items.FindByValue(dt.Rows[0]["BCId"].ToString().Trim()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbProd.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oPM = null;
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
                ViewState["StateEdit"] = "Add";
                tbProd.ActiveTabIndex = 1;
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);

            if ((vStateEdit == "Add" || vStateEdit == null))
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                StatusButton("View");
                LoadGrid(0);
                tbProd.ActiveTabIndex = 1;
                ViewState["StateEdit"] = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbProd.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
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
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    LoadGrid(0);
                    ClearControls();
                    tbProd.ActiveTabIndex = 0;
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
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
            ViewState["ProdctId"] = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vSubId = Convert.ToString(ViewState["ProdctId"]), vIsBC = "N", vAsignedBc = "";
            Int32 vErr = 0, vRec = 0, vProdctId = 0;
            CProduct oPM = null;
            CGblIdGenerator oGbl = null;
            try
            {
                if (chkBC.Checked == true) vIsBC = "Y";

                if (vIsBC == "Y")
                {
                    if (Convert.ToInt32(ddlBC.SelectedValue) <= 0)
                    {
                        gblFuction.MsgPopup("Please Select BC Name...");
                        return false;
                    }
                }
                vProdctId = Convert.ToInt32(ViewState["ProdctId"]);
                if (Mode == "Save")
                {
                    oPM = new CProduct();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("LoanProductMst", "Product", txtProduct.Text.Replace("'", "''"), "", "", "ProductId", vSubId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Product Can not be Duplicate...");
                        return false;
                    }
                    if (vIsBC == "Y")
                    {
                        vAsignedBc = oPM.ChkAsignedBCMst(vProdctId, Convert.ToInt32(ddlBC.SelectedValue), "Add");
                        if (vAsignedBc.ToString().Trim() != "")
                        {
                            gblFuction.MsgPopup(vAsignedBc);
                            return false;
                        }
                    }
                    vErr = oPM.InsertProduct(ref vProdctId, txtProduct.Text.Replace("'", "''"), txtShtName.Text.Replace("'", "''"), this.UserID, "I", "Save", vIsBC, Convert.ToInt32(ddlBC.SelectedValue));
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["ProdctId"] = vProdctId;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oPM = new CProduct();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("LoanProductMst", "Product", txtProduct.Text.Replace("'", "''"), "", "", "ProductId", vSubId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Product Can not be Duplicate...");
                        return false;
                    }
                    if (vIsBC == "Y")
                    {
                        vAsignedBc = oPM.ChkAsignedBCMst(vProdctId, Convert.ToInt32(ddlBC.SelectedValue), "Edit");
                        if (vAsignedBc.ToString().Trim() != "")
                        {
                            gblFuction.MsgPopup(vAsignedBc);
                            return false;
                        }
                    }
                    vErr = oPM.InsertProduct(ref vProdctId, txtProduct.Text.Replace("'", "''"), txtShtName.Text.Replace("'", "''"), this.UserID, "E", "Edit", vIsBC, Convert.ToInt32(ddlBC.SelectedValue));
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
                    oGbl = new CGblIdGenerator();
                    vRec = 0;
                    if (vRec <= 0)
                    {
                        oPM = new CProduct();
                        vErr = oPM.InsertProduct(ref vProdctId, txtProduct.Text.Replace("'", "''"), txtShtName.Text.Replace("'", "''"), this.UserID, "D", "Del", vIsBC, Convert.ToInt32(ddlBC.SelectedValue));
                        if (vErr > 0)
                            vResult = true;
                        else
                        {
                            gblFuction.MsgPopup(gblPRATAM.DBError);
                            vResult = false;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.RecordUseMsg);
                        vResult = false;
                    }
                }
                return vResult;
            }
            finally
            {
                oPM = null;
                oGbl = null;
            }
        }

        
    }
}