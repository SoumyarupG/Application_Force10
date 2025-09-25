using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;


namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class PoolTag : CENTRUMBase 
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
                txtFromDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtAsOnDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch(Session[gblValue.UserName].ToString());
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                //    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                //    ddlBranch.Enabled = false;
                //}
                //else
                //{
                //    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                //    ddlBranch.Enabled = true;
                //}
                PopPool();
                PopLoanType();
                PopPurpose();
                LoadGrid(1);
                StatusButton("View");
                tabPool.ActiveTabIndex = 0;
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        private void PopLoanType()
        {
            DataTable dt = null;
            CApplication oApp = null;
            try
            {
                oApp = new CApplication();
                dt = oApp.GetLoanTypeForApp("A",Session[gblValue.BrnchCode].ToString());
                ddlLnSchem.DataSource = dt;
                ddlLnSchem.DataTextField = "LoanType";
                ddlLnSchem.DataValueField = "LoanTypeId";
                ddlLnSchem.DataBind();
                
            }
            finally
            {
                dt = null;
                oApp = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopPurpose()
        {
            DataTable dt = null;
            CGblIdGenerator oGbl = null;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oGbl = new CGblIdGenerator();
                dt = oGbl.PopComboMIS("N", "N", "AA", "PurposeId", "Purpose", "LoanPurposeMst", "0", "AA", "AA", System.DateTime.Now, "0000");
                ddlLnPurps.DataSource = dt;
                ddlLnPurps.DataTextField = "Purpose";
                ddlLnPurps.DataValueField = "PurposeId";
                ddlLnPurps.DataBind();
            }
            finally
            {
                dt = null;
                oGbl = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUser"></param>
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                }
                
            }
            finally
            {
                dt = null;
                oUsr = null;
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
                this.PageHeading = "Pool Tagging";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuPoolTag);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
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
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Application", false);
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
        private void ClearControls()
        {
            ddlPool.SelectedIndex = -1;

            txtFromDt.Text = Session[gblValue.LoginDate].ToString();
            txtToDt.Text = Session[gblValue.LoginDate].ToString();
            txtNoofLoan.Text = "0";
            txtTotLoanAmt.Text = "0";
            txtTotOS.Text = "0";
            LblDate.Text = "";
            LblUser.Text = "";
            txtPos.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            ddlPool.Enabled = Status;
            txtFromDt.Enabled = Status;
            txtToDt.Enabled = Status;
            ddlBranch.Enabled = Status;
            ddlLnPurps.Enabled = Status;
            ddlLnSchem.Enabled = Status;
            btnShow.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopPool()
        {
            DataTable dt = null;
            CGblIdGenerator oGbl = null;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oGbl = new CGblIdGenerator();
                dt = oGbl.PopComboMIS("N", "N", "AA", "PoolId", "PoolName", "PoolMst", "0", "AA", "AA", System.DateTime.Now, "0000");
                ddlPool.DataSource = dt;
                ddlPool.DataTextField = "PoolName";
                ddlPool.DataValueField = "PoolId";
                ddlPool.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlPool.Items.Insert(0, oLi);
            }
            finally
            {
                dt = null;
                oGbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CPoolTag oPt = null;
            Int32 vRows = 0;
            string vBrCode = string.Empty;
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oPt = new CPoolTag();
                dt = oPt.GetPool(vBrCode, pPgIndx, ref vRows);
                gvList.DataSource = dt.DefaultView;
                gvList.DataBind();
                lblTotalPages.Text = CalTotPgs(vRows).ToString();
                lblCurrentPage.Text = cPgNo.ToString();
                if (cPgNo == 0)
                {
                    Btn_Previous.Enabled = false;
                    if (Int32.Parse(lblTotalPages.Text) > 1)
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
                oPt = null;
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
                    cPgNo = Int32.Parse(lblTotalPages.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }
            LoadGrid(cPgNo);
            tabPool.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            DataTable dt = null,dt1=null;
            CPoolTag oPt = null;
            DateTime vFromDt = gblFuction.setDate(txtFromDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            DateTime vAsOnDt = gblFuction.setDate(txtAsOnDt.Text);
            Double vLoanAmt = 0.0, vLoanOS=0.0;
            Int32 vLoanNo = 0;
            String vPurposeID = ddlLnPurps.SelectedValues.Replace("|", ",");
            String vLnSchem = ddlLnSchem.SelectedValues.Replace("|", ",");

            string vBrCode = string.Empty;
            try
            {

                vBrCode = ddlBranch.SelectedValues.Replace("|", ",");

                oPt = new CPoolTag();
                dt = oPt.GetPoolList(vFromDt, vToDt, vBrCode, vPurposeID, vLnSchem);
                dt1 = oPt.GetPoolPOS(vFromDt, vToDt,vAsOnDt, vBrCode, vPurposeID, vLnSchem);
                gvPool.DataSource = dt.DefaultView;
                gvPool.DataBind();
                ViewState["Pool"] = dt;
                foreach (DataRow dr in dt.Rows)
                {
                    vLoanAmt = vLoanAmt + Convert.ToDouble(dr["LoanAmt"].ToString());
                    vLoanOS = vLoanOS + Convert.ToDouble(dr["POS"].ToString());
                    vLoanNo = vLoanNo + 1;

                }
                txtNoofLoan.Text = Convert.ToString(vLoanNo);
                txtTotLoanAmt.Text = Convert.ToString(vLoanAmt);
                txtTotOS.Text = Math.Round(vLoanOS, 2).ToString(); 
                if (dt1.Rows.Count>0)
                {
                    txtPos.Text = Convert.ToString(dt1.Rows[0]["POS"]);
                }

            }
            finally
            {
                dt = null;
                dt1 = null;
                oPt = null;
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
                tabPool.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
                txtFromDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
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
                StatusButton("Edit");
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
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                //if (SaveRecords("Delete") == true)
                //{
                //    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                //    LoadGrid(1);
                //    StatusButton("Delete");
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabPool.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPool_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkPool = (CheckBox)e.Row.FindControl("chkPool");
                if (e.Row.Cells[10].Text == "Y")
                {
                    chkPool.Checked = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkPool_CheckedChanged(object sender, EventArgs e)
        {
            double vNoLoan=0, vLoanAmt=0, vLoanOS=0;
            DataTable dt = null;
            try
            {
                vNoLoan = Convert.ToDouble(txtNoofLoan.Text);
                vLoanAmt = Convert.ToDouble(txtTotLoanAmt.Text);
                vLoanOS = Convert.ToDouble(txtTotOS.Text);
                dt = (DataTable)ViewState["Pool"];
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkPool = (CheckBox)row.FindControl("chkPool");

                if (chkPool.Checked == true)
                {
                    dt.Rows[row.RowIndex]["PoolYN"] = "Y";
                    dt.AcceptChanges();
                    upSanc.Update();
                    vNoLoan = vNoLoan+1;
                    vLoanAmt = vLoanAmt + Convert.ToDouble(row.Cells[8].Text);
                    vLoanOS = vLoanOS + Convert.ToDouble(row.Cells[9].Text);
                    txtNoofLoan.Text = Convert.ToString(vNoLoan);
                    txtTotLoanAmt.Text = Convert.ToString(vLoanAmt);
                    txtTotOS.Text = Convert.ToString(vLoanOS);
                 }
                 else
                 {
                    dt.Rows[row.RowIndex]["PoolYN"] = "N";
                    dt.AcceptChanges();
                    upSanc.Update();
                    vNoLoan = vNoLoan-1;
                    vLoanAmt = vLoanAmt - Convert.ToDouble(row.Cells[8].Text);
                    vLoanOS = vLoanOS - Convert.ToDouble(row.Cells[9].Text);
                    txtNoofLoan.Text = Convert.ToString(vNoLoan);
                    txtTotLoanAmt.Text = Convert.ToString(vLoanAmt);
                    txtTotOS.Text = Convert.ToString(vLoanOS);
                }

                ViewState["Pool"] = dt;
                upSanc.Update();
            }
            finally
            {
                dt = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            CPoolTag oApp = null;
            DataTable dt = new DataTable();
            Int32 vErr = 0;

            string vXmlData = "";
            DateTime vPoolDt = gblFuction.setDate(txtFromDt.Text);
            if (ValidDate() == true)
            {
                try
                {
                    dt = (DataTable)ViewState["Pool"];
                    if (dt == null) return;
                    if (ValidateFields() == false) return;
                    string vBrCode = "-1";
                    oApp = new CPoolTag();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    //-----------XML Save----------
                    vErr = oApp.SavePool(vXmlData, Convert.ToInt32(ddlPool.SelectedValue), vPoolDt, vBrCode, this.UserID);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
                        LoadGrid(1);
                        StatusButton("View");
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                    }
                    
                }
                finally
                {
                    oApp = null;
                    dt = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidDate()
        {
            Boolean vResult = true;
            if (txtFromDt.Text.Trim() != "" || txtFromDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtFromDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtFromDt");
                    vResult = false;
                }
            }
            if (ddlPool.SelectedIndex<=0)
            {
                gblFuction.MsgPopup("Select Pool");
                gblFuction.focus("ctl00_cph_Main_ddlPool");
                vResult = false;
            }
            return vResult;
        }


        private Boolean ValidateFields()//To Check
        {
            Boolean vResult = true;

            if (gvPool.Rows.Count<=0)
            {
                gblFuction.MsgPopup("No records to save ");
                vResult = false;
            }
            return vResult;
        }
    }
}
