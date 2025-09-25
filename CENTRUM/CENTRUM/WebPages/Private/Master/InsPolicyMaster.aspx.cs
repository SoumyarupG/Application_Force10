using System;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;


namespace CENTRUM.WebPages.Private.Master
{
    public partial class InsPolicyMaster : CENTRUMBase
    {
       
        protected int vPgNo = 1;

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
                GetDebitLedger(Session[gblValue.BrnchCode].ToString());
                GetCreditLedger(Session[gblValue.BrnchCode].ToString());
                LoadGrid(1);
                StatusButton("View");
                tbPol.ActiveTabIndex = 0;
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
                this.PageHeading = "Insurance Policy Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuInsPolicyMaster);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "InsPolicyMaster", false);
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
        private void GetDebitLedger(string vBrCode)
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            try
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }

                oVoucher = new CVoucher();
                dt = oVoucher.GetAcGenLedCB(vBrCode, "A", "");
                ddlDbLed.DataSource = dt;
                ddlDbLed.DataTextField = "Desc";
                ddlDbLed.DataValueField = "DescId";
                ddlDbLed.DataBind();
                ListItem Li = new ListItem("<-- Select -->", "-1");
                ddlDbLed.Items.Insert(0, Li);
            }
            finally
            {
                oVoucher = null;
                dt = null;
            }
        }
        
        private void GetCreditLedger(string vBranch)
        {
            DataTable dt = null;
            
            CVoucher oVoucher = null;
            //SortedList Obj = new SortedList();
            try
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    vBranch = Session[gblValue.BrnchCode].ToString();
                }
                oVoucher = new CVoucher();
                dt = oVoucher.GetAcGenLedCB(vBranch, "V", "");                
                dt.AcceptChanges();
                ddlCrLed.DataSource = dt;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    Obj.Add(I, dr["SubSiLedYN"].ToString());  
                //    I = I + 1;
                //}
                ddlCrLed.DataTextField = "Desc";
                ddlCrLed.DataValueField = "DescId";
                //ddlCrLed.ExtraField = Obj;
                ddlCrLed.DataBind();
                ListItem liSel = new ListItem("<--- Select --->", "-1");
                ddlCrLed.Items.Insert(0, liSel);
            }
            finally
            {
                oVoucher = null;
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
                tbPol.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
                txtAgrDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                
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
                    LoadGrid(1);
                    ClearControls();
                    tbPol.ActiveTabIndex = 0;
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
            tbPol.ActiveTabIndex = 0;
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
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0; ;           
            CPolicy oPol = null;
            try
            {
                oPol = new CPolicy();
                dt = oPol.GetPolicyPG(pPgIndx, ref vTotRows, txtSearch.Text.Trim());
                gvPol.DataSource = dt;
                gvPol.DataBind();
               
                lblTotPg.Text = CalTotPages(vTotRows).ToString();
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
                oPol = null;
                dt=null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRows"></param>
        /// <returns></returns>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
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
            tbPol.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPol_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32  vRow=0;
            Int32 vPolId = 0;
            DataTable dt = null;
            CPolicy oPol = null;
            try
            {
                vPolId = Convert.ToInt32(e.CommandArgument);
                ViewState["PolId"] = vPolId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvPol.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    
                    oPol = new CPolicy();
                    dt = oPol.GetPolicyDetails(vPolId);
                                  
                    if (dt.Rows.Count > 0)
                    {
                        ddlDbLed.SelectedIndex = ddlDbLed.Items.IndexOf(ddlDbLed.Items.FindByValue(dt.Rows[vRow]["DDescId"].ToString()));
                        ddlCrLed.SelectedIndex = ddlCrLed.Items.IndexOf(ddlCrLed.Items.FindByValue(dt.Rows[vRow]["CDescId"].ToString()));

                        txtPolicyNm.Text = Convert.ToString(dt.Rows[vRow]["PolicyName"]);
                        txtComNm.Text = Convert.ToString(dt.Rows[vRow]["Ins_CompanyName"]);
                        txtAgrDt.Text = Convert.ToString(dt.Rows[vRow]["AgreementDate"]);
                       
                        txtPreAmt.Text = Convert.ToString(dt.Rows[vRow]["PremiumAmount"]);
                        txtOthr.Text = Convert.ToString(dt.Rows[vRow]["OtherFees"]);
                        txtterm.Text = Convert.ToString(dt.Rows[vRow]["Term_in_Year"]);
                        txtSumAm.Text = Convert.ToString(dt.Rows[vRow]["Sum_Assur_Amt"]);
                        txtTpPol.Text = Convert.ToString(dt.Rows[vRow]["Type_Policy"]);
                        chkBw.Checked = Convert.ToString(dt.Rows[0]["Chk_Ins_Bw"]).Trim() == "N" ? false : true;
                        chkCoBw.Checked = Convert.ToString(dt.Rows[0]["Chk_Ins_CBw"]).Trim() == "N" ? false : true;

                        ddlDbLed.SelectedIndex = ddlDbLed.Items.IndexOf(ddlDbLed.Items.FindByValue(dt.Rows[vRow]["DDescId"].ToString()));
                        ddlCrLed.SelectedIndex = ddlCrLed.Items.IndexOf(ddlCrLed.Items.FindByValue(dt.Rows[vRow]["CDescId"].ToString()));
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tbPol.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
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
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vPolId = Convert.ToInt32(ViewState["PolId"]);
            string vchkBw = "", vchkCoBw="";
            Int32 vErr = 0, vRec = 0, vterm=0;

            double vPreAmt = 0, vOthr = 0, vSumAm=0;

            DateTime vAgrDt = gblFuction.setDate(txtAgrDt.Text);
            CPolicy oPol = null;
            CGblIdGenerator oGbl = null;
            if (chkBw.Checked == true)
                vchkBw = "Y";
            else
                vchkBw = "N";
            if (chkCoBw.Checked == true)
                vchkCoBw = "Y";
            else
                vchkCoBw = "N";

            if (vchkBw == "N" && vchkCoBw == "N")
            {
                gblFuction.MsgPopup("Please select Borrower Or CoBorrower for Insurance...");
                return false;
            }

            try
            {

                vPreAmt = Convert.ToDouble(txtPreAmt.Text);
                vOthr = Convert.ToDouble(txtOthr.Text);
                vSumAm = Convert.ToDouble(txtSumAm.Text);
                vterm = Convert.ToInt32(txtterm.Text);
                if (Mode == "Save")
                {
                    oPol = new CPolicy();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("PolicyMst", "PolicyName", txtPolicyNm.Text.Replace("'", "''"), "", "", "PolicyId", vPolId.ToString(), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Insurance Policy Name Can not be Duplicate...");
                        return false;
                    }
                    vErr = oPol.SavePolicy(ref vPolId, txtPolicyNm.Text.Replace("'", "''"), txtComNm.Text.Replace("'", "''"),
                           vAgrDt, vPreAmt, vOthr, vSumAm, txtTpPol.Text.Replace("'", "''"), vchkBw, vchkCoBw,
                           ddlDbLed.SelectedValue, ddlCrLed.SelectedValue, vterm, this.UserID, "Save");
                    if (vErr > 0)
                    {
                        ViewState["PolId"] = vPolId;
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
                    oPol = new CPolicy();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("PolicyMst", "PolicyName", txtPolicyNm.Text.Replace("'", "''"), "", "", "PolicyId",vPolId.ToString(), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Insurance Policy Name Can not be Duplicate...");
                        return false;
                    }
                    vErr = oPol.SavePolicy(ref vPolId, txtPolicyNm.Text.Replace("'", "''"), txtComNm.Text.Replace("'", "''"),
                           vAgrDt, vPreAmt, vOthr, vSumAm, txtTpPol.Text.Replace("'", "''"), vchkBw, vchkCoBw,
                           ddlDbLed.SelectedValue, ddlCrLed.SelectedValue, vterm, this.UserID, "Edit");
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
                    vErr = oGbl.ChkDeleteString(vPolId.ToString(), "PolicyId", "PAIMst");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("This Policy has active Insurance, the system will not allow to delete.");
                        return false;
                    }
                    oPol = new CPolicy();
                    
                    vErr = oPol.SavePolicy(ref vPolId, txtPolicyNm.Text.Replace("'", "''"), txtComNm.Text.Replace("'", "''"),
                           vAgrDt, vPreAmt, vOthr, vSumAm, txtTpPol.Text.Replace("'", "''"), vchkBw, vchkCoBw,
                           ddlDbLed.SelectedValue, ddlCrLed.SelectedValue, vterm, this.UserID, "Delet");
                    
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
                oPol = null;
                oGbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtPolicyNm.Enabled = Status;
            txtComNm.Enabled = Status;
            txtAgrDt.Enabled = Status;
            txtPreAmt.Enabled = Status;
            txtOthr.Enabled = Status;
            txtSumAm.Enabled = Status;
            txtTpPol.Enabled = Status;
            chkBw.Enabled = Status;
            chkCoBw.Enabled = Status;
            ddlDbLed.Enabled = Status;
            ddlCrLed.Enabled = Status;
            txtterm.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtPolicyNm.Text = "";
            txtComNm.Text = "";
            txtAgrDt.Text = "";
            txtPreAmt.Text = "0";
            txtOthr.Text = "0";
            txtSumAm.Text = "0";
            txtTpPol.Text = "";
            chkBw.Checked = false;
            chkCoBw.Checked = false;
            ddlDbLed.SelectedIndex = -1;
            ddlCrLed.SelectedIndex = -1;
            txtterm.Text = "0";
            
        }
    }
}

       