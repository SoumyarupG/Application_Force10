using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class CGTTrans : CENTRUMBase
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
                txtFrDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                
                txtFCGD.Text = Session[gblValue.LoginDate].ToString();

                txtHVDt.Text = Session[gblValue.LoginDate].ToString();
                
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("View");
                else
                    StatusButton("Exit");
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                ViewState["CGTID"] = null;
                popRO();
                LoadGrid(1);
                //StatusButton("View");
                tabCgt.ActiveTabIndex = 0;
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
                this.PageHeading = "CGT";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCGT);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "CGT", false);
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
            ddlRo.Enabled = Status;
            ddlCenter.Enabled = Status;
            ddlMember.Enabled = Status;
            txtFCGD.Enabled = Status;
            ddlCGTBy.Enabled = Status;
            txtHVDt.Enabled = Status;
            ddlGrtYN.Enabled = Status;
            //txtExGDt.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            ddlRo.SelectedIndex = -1;
            ddlCenter.SelectedIndex = -1;
            ddlMember.SelectedIndex = -1;
            txtFCGD.Text= "";
            ddlCGTBy.SelectedIndex = -1;
            txtHVDt.Text = "";
            ddlGrtYN.SelectedIndex = -1;
            //txtExGDt.Text = "";
            lblGroupVal.Text = "";
            lblLIDtVal.Text = "";
            lblLnStatVal.Text = "";
            ViewState["CGTID"] = null;
            lblDate.Text = "";
            lblUser.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlRo.DataSource = dt;
                ddlRo.DataTextField = "EoName";
                ddlRo.DataValueField = "EoId";
                ddlRo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlRo.Items.Insert(0, oli);

                //oEo = new CEO();
                //dtEo = oEo.GetAllEoNotRo(vBrCode, vLogDt);
                ddlCGTBy.DataSource = dt;
                ddlCGTBy.DataTextField = "EoName";
                ddlCGTBy.DataValueField = "EoId";
                ddlCGTBy.DataBind();
                ddlCGTBy.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRo.SelectedIndex > 0)
                PopCenter(ddlRo.SelectedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vEOID"></param>
        private void PopCenter(string vEOID)
        {
            ddlMember.Items.Clear();
            lblGroupVal.Text = "";
            lblLIDtVal.Text = "";
            lblLnStatVal.Text = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("D", "N", "AA", "MarketID", "Market", "MarketMSt", vEOID, "EOID", "Tra_DropDate", vLogDt, vBrCode);
                ddlCenter.DataSource = dt;
                ddlCenter.DataTextField = "Market";
                ddlCenter.DataValueField = "MarketID";
                ddlCenter.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCenter.Items.Insert(0, oli);
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
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CCGT oCG = null;
            Int32 vRows = 0;
            string vBrCode = string.Empty;
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                DateTime vFrmDt = gblFuction.setDate(txtFrDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                oCG = new CCGT();
                dt = oCG.GetCgtPG(vBrCode, vFrmDt, vToDt, pPgIndx, ref vRows);
                gvList.DataSource = dt.DefaultView;
                gvList.DataBind();
                lblTotalPages.Text = CalTotPages(vRows).ToString();
                lblCurrentPage.Text = vPgNo.ToString();
                if (vPgNo == 0)
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
                    if (vPgNo == Int32.Parse(lblTotalPages.Text))
                        Btn_Next.Enabled = false;
                    else
                        Btn_Next.Enabled = true;
                }
            }
            finally
            {
                dt = null;
                oCG = null;
            }
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
                tabCgt.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
                //txtClsDt.Enabled = false;
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
        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCenter.SelectedIndex > 0)
                PopMember(ddlCenter.SelectedValue, "N");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vCentID"></param>
        /// <param name="vMode"></param>
        private void PopMember(string vCentID, string vMode)
        {
            DataTable dt = null;
            CCGT oCG = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            vCentID = ddlCenter.SelectedValue;
            lblGroupVal.Text = "";
            lblLIDtVal.Text = "";
            lblLnStatVal.Text = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oCG = new CCGT();
                dt = oCG.GetCGTMemberByMarketID(vCentID, vBrCode, vMode);
                ddlMember.DataSource = dt;
                ddlMember.DataTextField = "MemberName";
                ddlMember.DataValueField = "Memberid";
                ddlMember.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlMember.Items.Insert(0, oli);
            }
            finally
            {
                oCG = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMember_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CCGT oCG = null;
            string vBrCode = "", vMemID = "";
            Int32 vBrId = 0;
            vMemID = ddlMember.SelectedValue;
            //DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oCG = new CCGT();
                dt = oCG.GetMemberInfo(vMemID);
                lblGroupVal.Text = Convert.ToString(dt.Rows[0]["GroupName"]);
                ViewState["CGTID"] = Convert.ToString(dt.Rows[0]["CGTID"]);
            }
            finally
            {
                oCG = null;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Prev":
                    vPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;
                case "Next":
                    vPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }
            LoadGrid(vPgNo);
            tabCgt.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            if (ddlMember.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.MsgPopup("Member Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_tabCgt_pnlDtl_ddlMember");
                vResult = false;
                return vResult;
            }
            if (gblFuction.IsDate(txtFCGD.Text.Trim()) == false)
            {
                gblFuction.MsgPopup("Final CGT Date is not in DD/MM/YYYY");
                gblFuction.focus("ctl00_cph_Main_tabCgt_pnlDtl_txtFCGD");
                vResult = false;
                return vResult;
            }

            if (ddlCGTBy.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.MsgPopup("CGT by Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_tabCgt_pnlDtl_ddlCGTBy");
                vResult = false;
                return vResult;
            }

            if (gblFuction.IsDate(txtHVDt.Text.Trim()) == false)
            {
                gblFuction.MsgPopup("HV Date is not in DD/MM/YYYY");
                gblFuction.focus("ctl00_cph_Main_tabCgt_pnlDtl_txtHVDt");
                vResult = false;
                return vResult;
            }
            DateTime vFGCTDt = gblFuction.setDate(txtFCGD.Text);
            DateTime vHVDt = gblFuction.setDate(txtHVDt.Text);

            if (vHVDt < vFGCTDt)
            {
                gblFuction.MsgPopup("House Visit Date can not be Less than Final CGT Date...");
                vResult = false;
                return vResult;
            }
            if (ddlGrtYN.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.MsgPopup("GRT Eligible Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_tabCgt_pnlDtl_ddlGrtYN");
                vResult = false;
                return vResult;
            }

            //if (gblFuction.IsDate(txtExGDt.Text.Trim()) == false)
            //{
            //    gblFuction.MsgPopup("Expected GRT Date is not in DD/MM/YYYY");
            //    gblFuction.focus("ctl00_cph_Main_tabCgt_pnlDtl_txtExGDt");
            //    vResult = false;
            //    return vResult;
            //}

            if (ddlGrtYN.SelectedValue == "Y")
            {
                if (gblFuction.setDate(txtHVDt.Text) <= gblFuction.setDate(txtFCGD.Text))
                {
                    EnableControl(true);
                    gblFuction.MsgPopup("HV date should be greater than final CGT date..");
                    gblFuction.focus("ctl00_cph_Main_tabCgt_pnlDtl_txtHVDt");
                    vResult = false;
                    return vResult;
                }
            }
            return vResult;
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
            Int32 vCGTId = Convert.ToInt32(ViewState["CGTID"]);
            string vMemId = ddlMember.SelectedValue;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            Int32 vErr = 0;
            DataTable dt = null;
            CCGT oCG = null;

            try
            {
                if (ValidateFields() == false)
                    return false;

                if (Mode == "Save")
                {
                    oCG = new CCGT();
                    //dt = oCG.ChkDupAcGroupName(txtAcGrp.Text.Replace("'", "''"), 0, "S");
                    //if (dt.Rows.Count > 0)
                    //{
                    //    gblFuction.MsgPopup("Account Group Can not be Duplicate...");
                    //    gblFuction.focus("ctl00_cph_Main_tabAcGr_pnlDtl_txtAcGrp");
                    //    return false;
                    //}
                    vErr = oCG.SaveCGTMst(vCGTId, vMemId, gblFuction.setDate(txtFCGD.Text), ddlCGTBy.SelectedValue, 
                                ddlGrtYN.SelectedValue, gblFuction.setDate(txtHVDt.Text), vBrCode, this.UserID, "Save");
                    if (vErr == 1)
                    {
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
                    oCG = new CCGT();
                    dt = oCG.ChkCGT(vMemId, vCGTId, "Edit");
                    if (dt.Rows.Count > 0)
                    {
                        gblFuction.MsgPopup("You cannot edit this record because GRT already done..");
                        return false;
                    }
                    vErr = oCG.SaveCGTMst(vCGTId, vMemId, gblFuction.setDate(txtFCGD.Text), ddlCGTBy.SelectedValue,
                                ddlGrtYN.SelectedValue, gblFuction.setDate(txtHVDt.Text), vBrCode, this.UserID, "Edit");
                    if (vErr == 1)
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
                    oCG = new CCGT();
                    vErr = oCG.SaveCGTMst(vCGTId, vMemId, gblFuction.setDate("01/01/1900"), "",
                                "", gblFuction.setDate("01/01/1900"), "", this.UserID, "Delete");
                    if (vErr == 1)
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
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vCGTId = 0;
            DataTable dt = null;
            CCGT oCG = null;

            //CGblIdGenerator oGb = null;
            try
            {
                vCGTId = Convert.ToInt32(e.CommandArgument);
                ViewState["CGTID"] = vCGTId;
                if (e.CommandName == "cmdShow")
                {
                    oCG = new CCGT();
                    dt = oCG.GetCGTDetails(vCGTId);
                    if (dt.Rows.Count > 0)
                    {
                        
                        ddlRo.SelectedIndex = ddlRo.Items.IndexOf(ddlRo.Items.FindByValue(dt.Rows[0]["EOID"].ToString()));
                        PopCenter(ddlRo.SelectedValue);
                        ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(dt.Rows[0]["MarketID"].ToString()));
                        PopMember(ddlCenter.SelectedValue,"E");
                        ddlMember.SelectedIndex = ddlMember.Items.IndexOf(ddlMember.Items.FindByValue(dt.Rows[0]["MemberId"].ToString()));
                        lblGroupVal.Text = dt.Rows[0]["GroupName"].ToString();
                        lblLIDtVal.Text = dt.Rows[0]["LastInstDate"].ToString();
                        lblLnStatVal.Text = "";
                        ddlCGTBy.SelectedIndex = ddlCGTBy.Items.IndexOf(ddlCGTBy.Items.FindByValue(dt.Rows[0]["CGTBy"].ToString()));
                        txtFCGD.Text = dt.Rows[0]["FinalCGTdt"].ToString();
                        txtHVDt.Text = dt.Rows[0]["HVDate"].ToString();
                        //txtExGDt.Text = dt.Rows[0]["EXPGRTdt"].ToString();
                        ddlGrtYN.SelectedIndex = ddlGrtYN.Items.IndexOf(ddlGrtYN.Items.FindByValue(dt.Rows[0]["CGTPassYN"].ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tabCgt.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oCG = null;
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
                //txtClsDt.Enabled = false;
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
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadGrid(1);
                    ClearControls();
                    //tbCGT.ActiveTabIndex = 0;
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabCgt.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }
        
    }
}
