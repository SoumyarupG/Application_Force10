using System;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class GenLedDtl : CENTRUMBase
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
                popAccSubGroup();
                //PopSubsidiary();
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //PopBranchB();
                //else
                //PopBranch();
                if (Request.QueryString["sr"] != null)
                    GetAcGenLedger(Convert.ToString(Server.UrlDecode(Request.QueryString["sr"])));
                else if (Request.QueryString["GlId"] != null)
                    GetAcGenLedger(Convert.ToString(Server.UrlDecode(Request.QueryString["GlId"])));
                else
                    StatusButton("View");
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
                this.PageHeading = "Chart of Accounts";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuGenLed);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Account Ledger", false);
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
            txtGenCode.Enabled = Status;
            txtGenDesc.Enabled = Status;
            txtShNm.Enabled = Status;
            ddlSubGrp.Enabled = Status;
            txtAddr.Enabled = Status;
            txtPhNo.Enabled = Status;
            txtEmail.Enabled = Status;
            txtBranch.Enabled = Status;
            chkSVE.Enabled = Status;
            txtGlshCode.Enabled = Status;
            //chkSubId.Enabled = Status; 
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtGenCode.Text = "";
            txtGenDesc.Text = "";
            txtShNm.Text = "";
            ddlSubGrp.SelectedIndex = -1;
            txtAddr.Text = "";
            txtPhNo.Text = "";
            txtEmail.Text = "";
            lblUser.Text = "";
            lblDate.Text = "";
            txtBranch.Text = "";
            chkSVE.Checked = false;
            txtGlshCode.Text = "";
            //chkSubId.Checked = false;  
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopSubsidiary(bool vStatus)
        {
            ddlSubLed.Items.Clear();
            DataTable dt = null;
            CAcGenled oCb = null;
            string vBrCode = txtBranch.Text;
            if (vStatus == false)
            {
                ddlSubLed.DataSource = null;
                ddlSubLed.DataBind();
            }
            else
            {
                try
                {
                    oCb = new CAcGenled();
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    dt = oCb.GetAllSubsidairy(vBrCode, "T");         // T Stands for Tag
                    ddlSubLed.DataSource = dt;
                    ddlSubLed.DataTextField = "SubsidiaryLed";
                    ddlSubLed.DataValueField = "SubsidiaryId";
                    ddlSubLed.DataBind();
                    ddlSubLed.Items.Insert(0, liSel);
                }

                finally
                {
                    oCb = null;
                    dt.Dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popAccSubGroup()
        {
            DataTable dt = null;
            CGblIdGenerator oCb = null;
            try
            {
                oCb = new CGblIdGenerator();
                ListItem liSel = new ListItem("<--- Select --->", "-1");
                dt = oCb.PopComboMIS("N", "N", "AA", "AcSubGrpId", "SubGrp", "AcSubGrp", 0, "AA", "AA", System.DateTime.Now, "0000");
                ddlSubGrp.DataSource = dt;
                ddlSubGrp.DataTextField = "SubGrp";
                ddlSubGrp.DataValueField = "AcSubGrpId";
                ddlSubGrp.DataBind();
                ddlSubGrp.Items.Insert(0, liSel);
            }
            finally
            {
                oCb = null;
                dt.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //private void PopBranch()
        //{
        //    DataTable dt = null;
        //    CGblIdGenerator oCb = null;
        //    try
        //    {
        //        oCb = new CGblIdGenerator();
        //        ListItem liSel = new ListItem("<---ALL--->", "ALL");
        //        dt = oCb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", "AA", "BranchCode", "AA", System.DateTime.Now, "0000");
        //        ddlBranch.DataSource = dt;
        //        ddlBranch.DataValueField = "BranchCode";
        //        ddlBranch.DataTextField = "BranchName";
        //        ddlBranch.DataBind();
        //        ddlBranch.Items.Insert(0, liSel);
        //    }
        //    finally
        //    {
        //        oCb = null;
        //        dt.Dispose();
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        //private void PopBranchB()
        //{
        //    DataTable dt = null;
        //    CGblIdGenerator oCb = null;
        //    try
        //    {
        //        oCb = new CGblIdGenerator();
        //        ListItem liSel = new ListItem("<---ALL--->", "ALL");
        //        dt = oCb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", 0, "AA", "AA", System.DateTime.Now, Session[gblValue.BrnchCode].ToString());
        //        ddlBranch.DataSource = dt;
        //        ddlBranch.DataValueField = "BranchCode";
        //        ddlBranch.DataTextField = "BranchName";
        //        ddlBranch.DataBind();
        //        ddlBranch.Items.Insert(0, liSel);
        //    }
        //    finally
        //    {
        //        oCb = null;
        //        dt.Dispose();
        //    }
        //}

        protected void gvSubs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dt = null;
            //double vDrAmt = 0;
            //DataRow dr;
            try
            {
                if (e.CommandName == "cmdShow")
                {
                    dt = (DataTable)ViewState["SubLedDtl"];
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int index = row.RowIndex;
                    if (index != gvSubs.Rows.Count)
                    {
                        dt.Rows.RemoveAt(index);
                    }
                    dt.AcceptChanges();
                    ViewState["SubLedDtl"] = dt;
                    gvSubs.DataSource = dt;
                    gvSubs.DataBind();
                    hdEdit.Value = "-1";
                }
                else if (e.CommandName == "cmdEdit")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)row.FindControl("btnEdit");
                    int index = row.RowIndex;
                    dt = (DataTable)ViewState["SubLedDtl"];
                    foreach (GridViewRow gr in gvSubs.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnEdit");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    ddlDrCr.SelectedIndex = ddlDrCr.Items.IndexOf(ddlDrCr.Items.FindByValue(dt.Rows[index]["DrCr"].ToString()));
                    ddlSubLed.SelectedIndex = ddlSubLed.Items.IndexOf(ddlSubLed.Items.FindByValue(dt.Rows[index]["SubsidiaryId"].ToString()));
                    txtAmount.Text = dt.Rows[index]["OpBal"].ToString();
                    hdEdit.Value = index.ToString();
                }
            }
            finally
            {
                dt.Dispose();
            }
        }

        protected void txtBranch_OnTextChanged(object sender, EventArgs e)
        {
            PopSubsidiary(chkSubId.Checked);
        }

        protected void txtAmount_OnTextChanged(object sender, EventArgs e)
        {
            txtAmount.TextChanged += new EventHandler(btnApply_Click);
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            DataRow dr;
            Int32 vYrNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
            dt = (DataTable)ViewState["SubLedDtl"];
            if (ViewState["SubLedDtl"] != null && hdEdit.Value == "-1")
            {
                dr = dt.NewRow();

                dr["SubsidiaryId"] = ddlSubLed.SelectedValue.ToString();
                dr["SubsidiaryLed"] = ddlSubLed.SelectedItem.Text;
                dr["YearNo"] = vYrNo.ToString();
                if (ddlDrCr.SelectedValue == "D")
                {
                    dr["OpBal"] = txtAmount.Text;
                    dr["DrCr"] = "D";
                }
                else
                {
                    dr["OpBal"] = txtAmount.Text;
                    dr["DrCr"] = "C";
                }
                dt.Rows.Add(dr);

            }
            else if (hdEdit.Value == "-1")
            {
                dt = new DataTable();
                DataColumn dc = new DataColumn();
                dc.ColumnName = "DrCr";
                dt.Columns.Add(dc);
                DataColumn dc1 = new DataColumn();
                dc1.ColumnName = "OpBal";
                dc1.DataType = System.Type.GetType("System.Decimal");
                dt.Columns.Add(dc1);
                DataColumn dc3 = new DataColumn();
                dc3.ColumnName = "SubsidiaryId";
                dt.Columns.Add(dc3);
                DataColumn dc4 = new DataColumn();
                dc4.ColumnName = "SubsidiaryLed";
                dt.Columns.Add(dc4);
                DataColumn dc5 = new DataColumn();
                dc5.ColumnName = "YearNo";
                dt.Columns.Add(dc5);
                dr = dt.NewRow();

                dr["SubsidiaryId"] = ddlSubLed.SelectedValue.ToString();
                dr["SubsidiaryLed"] = ddlSubLed.SelectedItem.Text;
                dr["YearNo"] = vYrNo.ToString();
                if (ddlDrCr.SelectedValue == "D")
                {
                    dr["OpBal"] = txtAmount.Text;
                    dr["DrCr"] = "D";
                }
                else
                {
                    dr["OpBal"] = txtAmount.Text;
                    dr["DrCr"] = "C";
                }
                dt.Rows.Add(dr);
            }
            else if (hdEdit.Value != "-1")
            {
                Int32 vR = Convert.ToInt32(hdEdit.Value);
                dt.Rows[vR]["SubsidiaryId"] = ddlSubLed.SelectedValue.ToString();
                dt.Rows[vR]["SubsidiaryLed"] = ddlSubLed.SelectedItem.Text;
                dt.Rows[vR]["YearNo"] = vYrNo.ToString();
                if (ddlDrCr.SelectedValue == "D")
                {
                    dt.Rows[vR]["OpBal"] = txtAmount.Text;
                    dt.Rows[vR]["DrCr"] = "D";
                }
                else
                {
                    dt.Rows[vR]["OpBal"] = txtAmount.Text;
                    dt.Rows[vR]["DrCr"] = "C";
                }
            }
            dt.AcceptChanges();
            if (dt.Rows.Count > 0)
            {
                ViewState["SubLedDtl"] = dt;
            }
            else
            {
                ViewState["SubLedDtl"] = null;
            }

            gvSubs.DataSource = dt;
            gvSubs.DataBind();
            hdEdit.Value = "-1";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                gblFuction.AjxMsgPopup("Only Head Office Can Create or Edit Ledger ..");
                return false;
            }
            if (txtGenCode.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Ledger Code Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtGenCode");
                vResult = false;
            }
            if (txtGenDesc.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Ledger Name Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtGenDesc");
                vResult = false;
            }
            if (Convert.ToString(ddlSubGrp.SelectedItem) == "CASH IN HAND" && Convert.ToString(ViewState["DescId"]) == "C0001")
            {
                EnableControl(true);
                gblFuction.MsgPopup("You can not create the ledger under CASH IN HAND ...");
                gblFuction.focus("ctl00_cph_Main_ddlSubGrp");
                vResult = false;
            }
            if (ddlSubGrp.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.MsgPopup("Sub Group Cannot be left Blank...");
                gblFuction.focus("ctl00_cph_Main_ddlSubGrp");
                vResult = false;
            }
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                if (txtBranch.Text != Session[gblValue.BrnchCode].ToString())
                {
                    gblFuction.MsgPopup("Please Enter your Branch");
                    vResult = false;
                }
            }
            return vResult;
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
                ViewState["StateEdit"] = null;
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
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                //    if (ddlBranch.SelectedValue == "ALL")
                //    {
                //        gblFuction.MsgPopup("Branch can not Edit common Ledger");
                //        return;
                //    }
                //}
                ViewState["StateEdit"] = "Edit";
                gblFuction.focus("ctl00_cph_Main_txtGenCode");
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
            try
            {
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
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                //    if (ddlBranch.SelectedValue == "ALL")
                //    {
                //        gblFuction.MsgPopup("Branch can not delete common Ledger");
                //        return;
                //    }
                //}

                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGo_Click(object sender, EventArgs e)
        {
            string vSearch = txtSrch.Text;
            try
            {
                if (vSearch == "" || vSearch == null)
                    Response.Redirect("AcGenled.aspx?GlId=" + vSearch, false);
                else
                    vSearch = vSearch.Substring(0, 5).ToString();
                GetAcGenLedger(vSearch);
            }
            catch (Exception ex)
            {
                gblFuction.MsgPopup("No Records Found on Search Key Word." + vSearch.ToUpper());
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
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                if (vStateEdit == "" || vStateEdit == null)
                    vStateEdit = "Save";
                if (SaveRecords(vStateEdit) == true)
                {
                    gblFuction.MsgPopup(gblMarg.SaveMsg);
                    StatusButton("Show");
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
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Int32 vChkOpBal = 0, vChkVoucher = 0, vErr = 0;
            Int32 vYrNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
            string vChkDuplicate = "", vSystem = "", vActType = "", vSusidairyYN = "", vchkSVE = "", vDisburseAC = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vXmlData = "";
            Boolean vResult = false;
            CAcGenled oCAcGenled = null;
            if (ValidateFields() == false)
                return false;
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                string vGenLedId = Convert.ToString(ViewState["DescId"]);
                Int32 vddlSubGrp = Convert.ToInt32(ddlSubGrp.SelectedValue);
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                if (vddlSubGrp == 4)
                    vActType = "B";
                else
                    vActType = "G";
                if (chkSubId.Checked == true)
                {
                    vSusidairyYN = "Y";
                    DataTable dt = (DataTable)ViewState["SubLedDtl"];
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                }
                else
                {
                    vSusidairyYN = "N";
                    vXmlData = "";
                }
                if (chkSVE.Checked == true)
                {
                    vchkSVE = "Y";

                }
                else
                {
                    vchkSVE = "N";
                }
                if (chkDisbAC.Checked == true)
                    vDisburseAC = "Y";
                else
                    vDisburseAC = "N";

                if (Mode == "Save")
                {
                    oCAcGenled = new CAcGenled();
                    oCAcGenled.ChkDupLedger(txtGenCode.Text.Replace("'", "''"), txtGenDesc.Text.Replace("'", "''"), vGenLedId, "Save", ref vChkDuplicate, ref vSystem);
                    if (vChkDuplicate == "Y")
                    {
                        gblFuction.MsgPopup("Ledger Code or Name can not be Duplicate..");
                        return false;
                    }
                    vErr = oCAcGenled.InsertAcGenled(txtGenCode.Text.Replace("'", "''"), txtGenDesc.Text.Replace("'", "''"),
                        txtShNm.Text.Replace("'", "''"), Convert.ToInt32(ddlSubGrp.SelectedValue), vSusidairyYN, vActType,
                        txtAddr.Text.Replace("'", "''"), txtPhNo.Text.Replace("'", "''"), "N", txtEmail.Text.Replace("'", "''"),
                        txtBranch.Text, this.UserID, "I", 0, vXmlData, vchkSVE, txtAccNo.Text.Replace("'", "''"), vDisburseAC,
                        txtGlshCode.Text.Replace(" ", ""));
                    if (vErr == 0)
                    {
                        vResult = true;
                    }
                    else if (vErr == 2)
                    {
                        gblFuction.AjxMsgPopup("Failed: Duplicate GLSH Code Found.");
                        vResult = false;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oCAcGenled = new CAcGenled();
                    oCAcGenled.ChkDupLedger(txtGenCode.Text.Replace("'", "''"), txtGenDesc.Text.Replace("'", "''"), vGenLedId, "Edit", ref vChkDuplicate, ref vSystem);
                    if (vChkDuplicate == "Y")
                    {
                        gblFuction.MsgPopup("Ledger Code or Name can not be Duplicate..");
                        return false;
                    }
                    if (vSystem == "Y")
                    {
                        gblFuction.MsgPopup("System Ledger Can not be Edited..");
                        return false;
                    }
                    else
                    {
                        vErr = oCAcGenled.UpdateAcGenled(vGenLedId, txtGenCode.Text.Replace("'", "''"), txtGenDesc.Text.Replace("'", "''"),
                              txtShNm.Text.Replace("'", "''"), Convert.ToInt32(ddlSubGrp.SelectedValue), vSusidairyYN, vActType, txtAddr.Text.Replace("'", "''"),
                              txtPhNo.Text, "N", txtEmail.Text.Replace("'", "''"), txtBranch.Text, this.UserID, "E", 0, vXmlData, vYrNo, vchkSVE,
                              txtAccNo.Text.Replace("'", "''"), vDisburseAC, txtGlshCode.Text.Replace(" ", ""));
                        if (vErr == 0)
                        {
                            ViewState["DescId"] = vGenLedId;
                            vResult = true;
                        }
                        else if (vErr == 2)
                        {
                            gblFuction.AjxMsgPopup("Failed: Duplicate GLSH Code Found.");
                            vResult = false;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.DBError);
                            vResult = false;
                        }
                    }
                }
                else if (Mode == "Delete")
                {
                    oCAcGenled = new CAcGenled();
                    oCAcGenled.ChkDupLedger(txtGenCode.Text.Replace("'", "''"), txtGenDesc.Text.Replace("'", "''"), vGenLedId, "Edit", ref vChkDuplicate, ref vSystem);
                    if (vSystem == "Y")
                    {
                        gblFuction.MsgPopup("System Ledger Can not be Deleted..");
                        return false;
                    }
                    if (gvSubs.Rows.Count > 0)
                    {
                        gblFuction.MsgPopup("Ledger have Subsidiary Can not be Deleted..");
                        return false;
                    }
                    oCAcGenled.ChkDeleteACLedger(vGenLedId, Session[gblValue.ACVouDtl].ToString(), ref vChkVoucher, ref vChkOpBal);
                    if (vChkVoucher > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.RecordUseMsg);
                        return false;
                    }
                    if (vChkOpBal > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.RecordUseMsg);
                        vResult = true;
                    }
                    vErr = oCAcGenled.UpdateAcGenled(vGenLedId, txtGenCode.Text.Replace("'", "''"), txtGenDesc.Text.Replace("'", "''"),
                          txtShNm.Text.Replace("'", "''"), Convert.ToInt32(ddlSubGrp.SelectedValue), vSusidairyYN, vActType, txtAddr.Text.Replace("'", "''"),
                          txtPhNo.Text, "N", txtEmail.Text.Replace("'", "''"), txtBranch.Text, this.UserID, "D", 0, vXmlData, vYrNo, vchkSVE,
                          txtAccNo.Text.Replace("'", "''"), vDisburseAC, txtGlshCode.Text.Replace(" ", ""));
                    if (vErr == 0)
                    {
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
                vResult = false;
                throw ex;
            }
            finally
            {
                oCAcGenled = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pGenLedId"></param>
        private void GetAcGenLedger(string pGenLedId)
        {
            DataTable dt = null;
            DataSet ds = null;
            CAcGenled oAcGenLed = null;
            string vchkSVE = "";
            try
            {
                oAcGenLed = new CAcGenled();
                if (pGenLedId == "" || pGenLedId != null)
                {
                    ds = oAcGenLed.GetGenLedSubsidairyDtl(pGenLedId);
                    dt = ds.Tables[0];
                    //dt1 = ds.Tables[1];  
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["StateEdit"] = "Edit";
                        ViewState["DescId"] = dt.Rows[0]["DescId"].ToString();
                        txtGenCode.Text = dt.Rows[0]["GenLedCode"].ToString();
                        txtGenDesc.Text = dt.Rows[0]["Desc"].ToString();
                        txtShNm.Text = dt.Rows[0]["ShortName"].ToString();
                        txtAddr.Text = dt.Rows[0]["Address1"].ToString();
                        txtPhNo.Text = dt.Rows[0]["Phone"].ToString();
                        txtEmail.Text = dt.Rows[0]["EMail"].ToString();
                        txtAccNo.Text = dt.Rows[0]["AccNo"].ToString();
                        txtGlshCode.Text = dt.Rows[0]["GLSH_CODE"].ToString();
                        vchkSVE = dt.Rows[0]["SVEYN"].ToString();
                        if (vchkSVE == "Y")
                            chkSVE.Checked = true;
                        else
                            chkSVE.Checked = false;
                        if (dt.Rows[0]["DisburseAC"].ToString() == "Y")
                            chkDisbAC.Checked = true;
                        else
                            chkDisbAC.Checked = false;
                        ddlSubGrp.SelectedIndex = ddlSubGrp.Items.IndexOf(ddlSubGrp.Items.FindByValue(dt.Rows[0]["AcSubGrpId"].ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        txtBranch.Text = dt.Rows[0]["BranchCode"].ToString();
                        StatusButton("Show");
                    }
                    //if (dt1.Rows.Count > 0)
                    //{
                    //    gvSubs.DataSource = dt1;
                    //    gvSubs.DataBind();
                    //}                   
                }
            }
            finally
            {
                oAcGenLed = null;
                dt = null;
                //dt1 = null;
                ds = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkSubId_CheckedChanged(object sender, EventArgs e)
        {
            //if (gvSubs.Rows.Count > 0)
            //{
            //    gblFuction.MsgPopup("Susidiary can not be changed from General Ledger Account.");
            //    chkSubId.Checked = true;  
            //    return; 
            //}
        }
    }
}