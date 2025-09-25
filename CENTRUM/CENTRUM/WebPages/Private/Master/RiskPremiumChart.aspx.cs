
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class RiskPremiumChart : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
                    StatusButton("View");
                LoadGrid(0);
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {

                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Risk Premium Chart";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuRiskPremiumChart);
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
                    btnAdd.Visible = true;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = true;
                    btnEdit.Visible = true;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnAdd.Visible = true;
                    btnEdit.Visible = true;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Risk Premium Chart", false);
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
                    gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtEffectiveDate");
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
                    gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtEffectiveDate");
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
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
        private void ClearControls()
        {

            txtEffectiveDate.Text = Session[gblValue.LoginDate].ToString();
            
            lblUser.Text = "";
            lblDate.Text = "";
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(bool Status)
        {
            
            txtEffectiveDate.Enabled = Status;
            gvBranch.Enabled = Status;
            
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CRiskPremium oIC = null;
            Int32 vRows = 0;
            try
            {
                oIC = new CRiskPremium();
                dt = oIC.GetRiskPremiumChartPG(pPgIndx, ref vRows);
                gvLnScheme.DataSource = dt.DefaultView;
                gvLnScheme.DataBind();
                if (dt.Rows.Count <= 0)
                {
                    lblTotalPages.Text = "0";
                    lblCurrentPage.Text = "0";
                }
                else
                {
                    lblTotalPages.Text = CalTotPgs(vRows).ToString();
                    lblCurrentPage.Text = cPgNo.ToString();
                }
                if (cPgNo == 1)
                {
                    Btn_Previous.Enabled = false;
                    if (Int32.Parse(lblTotalPages.Text) > 0 && cPgNo != Int32.Parse(lblTotalPages.Text))
                        Btn_Next.Enabled = true;
                    else
                        Btn_Next.Enabled = false;
                }
                else
                {
                    Btn_Previous.Enabled = true;
                    if (cPgNo == Int32.Parse(lblTotalPages.Text))
                        Btn_Next.Enabled = false;
                    else
                        Btn_Next.Enabled = true;
                }
            }
            finally
            {
                dt = null;
                oIC = null;
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
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1; //lblCurrentPage
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1; //lblTotalPages
                    break;
            }
            LoadGrid(cPgNo);
            tabLnScheme.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLnScheme_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vRPId = 0;
            DataTable dt = null;
            CRiskPremium oLS = null;
            try
            {
                vRPId = Convert.ToInt32(e.CommandArgument);
                ViewState["RPId"] = vRPId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvLnScheme.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oLS = new CRiskPremium();
                    dt = oLS.GetRiskPremiumById(vRPId);
                    if (dt.Rows.Count > 0)
                    {
                        txtEffectiveDate.Text = Convert.ToString(dt.Rows[0]["RPEffectiveDt"]).Trim();
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        LoadDtl("Edit", vRPId);
                        tabLnScheme.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidateField()
        {
            bool vRes = true;
            
            if (gblFuction.IsDate(txtEffectiveDate.Text) == false)
            {
                gblFuction.MsgPopup("EffectiveDate Date should be in DD/MM/YYYY Format");
                gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtIntroDt");
                return vRes = false;
            }
            

            return vRes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            DataTable dtXml = TabletoXml();
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vSubId = Convert.ToString(ViewState["RPId"]), vErrorMsg = "", vXml = "";
            Int32 vErr = 0, vRec = 0, vRPId = 0;
            double vMinLnAmt = 0, vMaxLnAmt = 0;
            CRiskPremium oLS = null;
            
            try
            {
                if (ValidateField() == false)
                    return false;
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString();
                }

                

                vRPId = Convert.ToInt32(ViewState["RPId"]);
                
                if (Mode == "Save")
                {
                    oLS = new CRiskPremium();
                    vErr = oLS.InsertRiskPremiumChart(ref vRPId, gblFuction.setDate(txtEffectiveDate.Text), Mode, Convert.ToInt32(Session[gblValue.UserId]), vXml, ref vErrorMsg);
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["RPId"] = vRPId;
                    }

                    else
                    {
                        gblFuction.MsgPopup(vErrorMsg);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oLS = new CRiskPremium();

                    vErr = oLS.InsertRiskPremiumChart(ref vRPId, gblFuction.setDate(txtEffectiveDate.Text), Mode, Convert.ToInt32(Session[gblValue.UserId]), vXml, ref vErrorMsg);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrorMsg);
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
                oLS = null;
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable TabletoXml()
        {
            DataTable dt = new DataTable("Alloc");
            DataRow dr;
            dt.Columns.Add("RiskPremiumID", typeof(Int32));
            dt.Columns.Add("IRR", typeof(string));
            dt.Columns.Add("IsBC", typeof(string));
            dt.Columns.Add("Customer_Type", typeof(string));
            dt.Columns.Add("RiskPremium", typeof(float));
            foreach (GridViewRow gr in gvBranch.Rows)
            {
                TextBox txtRiskPremium = (TextBox)gr.FindControl("txtRiskPremium");
                dr = dt.NewRow();
                dt.Rows.Add(dr);                
                dt.Rows[gr.RowIndex]["RiskPremiumID"] = Convert.ToInt32(gr.Cells[1].Text);
                dt.Rows[gr.RowIndex]["IRR"] = Convert.ToString(gr.Cells[2].Text);
                dt.Rows[gr.RowIndex]["IsBC"] = Convert.ToString(gr.Cells[3].Text);
                dt.Rows[gr.RowIndex]["Customer_Type"] = Convert.ToString(gr.Cells[4].Text);
                dt.Rows[gr.RowIndex]["RiskPremium"] = Convert.ToDouble(txtRiskPremium.Text);
                
            }
            dt.AcceptChanges();
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void LoadDtl(string pMode, Int32 vRPId)
        {
            DataTable dt = null;
            CRiskPremium oLS = null;
            try
            {
                oLS = new CRiskPremium();
                dt = oLS.GetDtlForRP(pMode, vRPId);
                gvBranch.DataSource = dt;
                gvBranch.DataBind();
                
            }
            finally
            {
                oLS = null;
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
            ViewState["RPId"] = null;
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
                StatusButton("Add");
                ClearControls();
                LoadDtl("Add", 0);
                tabLnScheme.ActiveTabIndex = 1;
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
                    ClearControls();
                    StatusButton("Delete");
                    LoadGrid(0);
                    tabLnScheme.ActiveTabIndex = 0;
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
                StatusButton("Show");
                LoadGrid(0);
                ViewState["StateEdit"] = null;
            }
        }

    }
}

