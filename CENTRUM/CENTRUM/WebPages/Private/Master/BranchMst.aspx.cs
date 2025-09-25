using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class BranchMst : CENTRUMBase
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
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
                    StatusButton("View");
                txtOpDt.Text = Session[gblValue.LoginDate].ToString();
                txtDayBeginDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                popState();
                popBrType();
                LoadGrid(1);
                tbBrnh.ActiveTabIndex = 0;
                popRO();
                PopDisbBank();
                popBCName();
                popMainBranch();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void PopDisbBank()
        {
            DataTable dt = null;
            CNEFTTransfer oNeft = null;
            oNeft = new CNEFTTransfer();
            dt = oNeft.PopDisbBank();
            ddlBank.DataSource = dt;
            ddlBank.DataTextField = "Desc";
            ddlBank.DataValueField = "DescId";
            ddlBank.DataBind();
            ListItem olist = new ListItem("<--select-->", "-1");
            ddlBank.Items.Insert(0, olist);
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Branch";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBranchMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Branch Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void popRO()
        {
            DataTable dt = null, dt2 = null, dt3 = null,dtZH = null,dtSH = null ;
            CEO oRO = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oRO = new CEO();
                dt = oRO.PopRO_DesigWise("0000", "0", "0", vLogDt, this.UserID, "ARM");
                ddlAM.DataSource = dt;
                ddlAM.DataTextField = "EoName";
                ddlAM.DataValueField = "EoId";
                ddlAM.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlAM.Items.Insert(0, oli);

                ddlMELAM.DataSource = dt;
                ddlMELAM.DataTextField = "EoName";
                ddlMELAM.DataValueField = "EoId";
                ddlMELAM.DataBind();
                ddlMELAM.Items.Insert(0, oli);

                oRO = new CEO();
                dt2 = oRO.PopRO_DesigWise("0000", "0", "0", vLogDt, this.UserID, "UM");
                ddlUM.DataSource = dt2;
                ddlUM.DataTextField = "EoName";
                ddlUM.DataValueField = "EoId";
                ddlUM.DataBind();
                ddlUM.Items.Insert(0, oli);

                ddlMELUM.DataSource = dt2;
                ddlMELUM.DataTextField = "EoName";
                ddlMELUM.DataValueField = "EoId";
                ddlMELUM.DataBind();
                ddlMELUM.Items.Insert(0, oli);

                oRO = new CEO();
                dt3 = oRO.PopRO_DesigWise("0000", "0", "0", vLogDt, this.UserID, "CM");
                ddlCM.DataSource = dt3;
                ddlCM.DataTextField = "EoName";
                ddlCM.DataValueField = "EoId";
                ddlCM.DataBind();
                ddlCM.Items.Insert(0, oli);

                oRO = new CEO();
                dtZH = oRO.PopRO_DesigWise("0000", "0", "0", vLogDt, this.UserID, "ZH");
                ddlZH.DataSource = dtZH;
                ddlZH.DataTextField = "EoName";
                ddlZH.DataValueField = "EoId";
                ddlZH.DataBind();
                ddlZH.Items.Insert(0, oli);

                oRO = new CEO();
                dtZH = oRO.PopRO_DesigWise("0000", "0", "0", vLogDt, this.UserID, "ZH");
                ddlMelZH.DataSource = dtZH;
                ddlMelZH.DataTextField = "EoName";
                ddlMelZH.DataValueField = "EoId";
                ddlMelZH.DataBind();
                ddlMelZH.Items.Insert(0, oli);

                oRO = new CEO();
                dtSH = oRO.PopRO_DesigWise("0000", "0", "0", vLogDt, this.UserID, "SH");
                ddlSH.DataSource = dtSH;
                ddlSH.DataTextField = "EoName";
                ddlSH.DataValueField = "EoId";
                ddlSH.DataBind();
                ddlSH.Items.Insert(0, oli);

                oRO = new CEO();
                dtSH = oRO.PopRO_DesigWise("0000", "0", "0", vLogDt, this.UserID, "SH");
                ddlMelSH.DataSource = dtSH;
                ddlMelSH.DataTextField = "EoName";
                ddlMelSH.DataValueField = "EoId";
                ddlMelSH.DataBind();
                ddlMelSH.Items.Insert(0, oli);


            }
            finally
            {
                oRO = null;
                dt = null;
                dt2 = null;
                dt3 = null;
                dtSH = null;
                dtZH = null;
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
                    btnAdd.Enabled = false;
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
        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            popRegion(Convert.ToInt32(ddlState.SelectedValue));
        }

        /// <summary>
        /// 
        /// </summary>
        private void popRegion(Int32 vStateId)
        {
            ddlReg.Items.Clear();
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("S", "N", "AA", "RegionId", "RegionName", "RegionMst", vStateId, "StateId", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlReg.DataSource = dt;
                if (dt.Rows.Count > 0)
                {
                    ddlReg.DataTextField = "RegionName";
                    ddlReg.DataValueField = "RegionId";
                    ddlReg.DataBind();
                }
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlReg.Items.Insert(0, oli);
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
        protected void ddlReg_SelectedIndexChanged(object sender, EventArgs e)
        {
            popArea(Convert.ToInt32(ddlReg.SelectedValue));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vRegId"></param>
        private void popArea(Int32 vRegId)
        {
            ddlArea.Items.Clear();
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("S", "N", "AA", "AreaId", "AreaName", "AreaMst", vRegId, "RegionId", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlArea.DataSource = dt;
                if (dt.Rows.Count > 0)
                {
                    ddlArea.DataTextField = "AreaName";
                    ddlArea.DataValueField = "AreaId";
                    ddlArea.DataBind();
                }
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
        private void popDistrict(string pBrCode)
        {
            cblDist.Items.Clear();
            DataTable dt = null;
            CBranch oBr = null;
            try
            {
                oBr = new CBranch();
                dt = oBr.GetWorkingDistBtBranchCode(pBrCode);
                cblDist.DataSource = dt;
                cblDist.DataTextField = "DistrictName";
                cblDist.DataValueField = "DistrictId";
                cblDist.DataBind();
            }
            finally
            {
                oBr = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popBrType()
        {
            ListItem oL1 = new ListItem("<--Select-->", "-1");
            ddlBType.Items.Add(oL1);
            ListItem oL2 = new ListItem("Residential", "RR");
            ddlBType.Items.Add(oL2);
            ListItem oL3 = new ListItem("Non Residential", "NR");
            ddlBType.Items.Add(oL3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable dtDistrict()
        {
            DataTable dt = new DataTable("District");
            DataRow dr;
            dt.Columns.Add("Id");
            dt.Columns.Add("DistrictId");
            dt.Columns.Add("AllocationDt");
            int i = 0;
            foreach (ListItem li in cblDist.Items)
            {
                dr = dt.NewRow();
                dr["DistrictId"] = li.Value;
                dr["Id"] = i;
                dt.Rows.Add(dr);
                i++;
                if (li.Selected == true)
                {
                    dr["AllocationDt"] = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                }
            }
            dt.AcceptChanges();
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtXml"></param>
        /// <returns></returns>
        private string DtToXml()
        {
            DataTable dtXml = dtDistrict();
            string vXml = "";
            using (StringWriter oSw = new StringWriter())
            {
                dtXml.WriteXml(oSw);
                vXml = oSw.ToString();
            }
            return vXml;
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
                tbBrnh.ActiveTabIndex = 1;
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
                    LoadGrid(1);
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
                txtBrCode.Enabled = false;
                //ddlArea.Enabled = false;
                ddlState.Enabled = false;
                //ddlReg.Enabled = false;
                //txtBrName.Enabled = false;
                //txtOpDt.Enabled = false;
                txtDayBeginDt.Enabled = false;
                pnl1.Enabled = false;
                //chkBCYN.Enabled = false;
                //ddlPtnr.Enabled = false;
                //ddlMainBranch.Enabled = false;
                SetInitilize();
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
            tbBrnh.ActiveTabIndex = 0;
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
                StatusButton("Show");
                ViewState["StateEdit"] = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0; ;
            string vBrCode = "";
            CBranch oBr = null;
            try
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oBr = new CBranch();
                dt = oBr.GetBranchPG(vBrCode, pPgIndx, ref vTotRows);
                gvBrch.DataSource = dt;
                gvBrch.DataBind();
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
                oBr = null;
                dt = null;
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
            tbBrnh.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBrch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBCode = "";
            DataTable dt = null, dt1 = null;
            CBranch oBr = null;
            try
            {
                vBCode = Convert.ToString(e.CommandArgument);
                ViewState["BCode"] = vBCode;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvBrch.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oBr = new CBranch();
                    dt = oBr.GetBranchDetails(vBCode);
                    dt1 = oBr.GetKeyMtnDtlById(vBCode);
                    if (dt.Rows.Count > 0)
                    {
                        ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(dt.Rows[0]["StateID"].ToString()));
                        popRegion(Convert.ToInt32(ddlState.SelectedValue));
                        ddlReg.SelectedIndex = ddlReg.Items.IndexOf(ddlReg.Items.FindByValue(dt.Rows[0]["RegionID"].ToString()));
                        popArea(Convert.ToInt32(ddlReg.SelectedValue));
                        ddlArea.SelectedIndex = ddlArea.Items.IndexOf(ddlArea.Items.FindByValue(dt.Rows[0]["AreaID"].ToString()));
                        txtBrCode.Text = Convert.ToString(dt.Rows[0]["BranchCode"]);
                        txtBrName.Text = Convert.ToString(dt.Rows[0]["BranchName"]);
                        txtOpDt.Text = Convert.ToString(dt.Rows[0]["OpenDate"]);
                        txtDayBeginDt.Text = Convert.ToString(dt.Rows[0]["DayBeginDt"]);

                        popDistrict(vBCode);
                        txtDist.Text = Convert.ToString(dt.Rows[0]["District"]);
                        txtCityCode.Text = Convert.ToString(dt.Rows[0]["CityCode"]);
                        txtPrimarySolId.Text = Convert.ToString(dt.Rows[0]["PrimarySolId"]);
                        txtCityNo.Text = Convert.ToString(dt.Rows[0]["CityNo"]);
                        txtPS.Text = Convert.ToString(dt.Rows[0]["PS"]);
                        txtAdd.Text = Convert.ToString(dt.Rows[0]["Address"]);
                        txtPin.Text = Convert.ToString(dt.Rows[0]["PIN"]);
                        txtPhNo.Text = Convert.ToString(dt.Rows[0]["PhNo"]);
                        ddlBType.SelectedIndex = ddlBType.Items.IndexOf(ddlBType.Items.FindByValue(dt.Rows[0]["BranchType"].ToString()));
                        ddlAreaCategory.SelectedIndex = ddlAreaCategory.Items.IndexOf(ddlAreaCategory.Items.FindByValue(dt.Rows[0]["Area_Category"].ToString()));
                        ddlCbType.SelectedIndex = ddlCbType.Items.IndexOf(ddlCbType.Items.FindByValue(dt.Rows[0]["CbType"].ToString()));

                        txtEmail.Text = Convert.ToString(dt.Rows[0]["Email"]);
                        txtAgDt.Text = Convert.ToString(dt.Rows[0]["AgreementDate"]);
                        txtToDt.Text = Convert.ToString(dt.Rows[0]["ValidTill"]);
                        txtMRnt.Text = Convert.ToString(dt.Rows[0]["MonthlyRent"]);
                        txtAdv.Text = Convert.ToString(dt.Rows[0]["AdvPaid"]);

                        txtTrNo.Text = Convert.ToString(dt.Rows[0]["TrLicNo"]);
                        txtTrLicDt.Text = Convert.ToString(dt.Rows[0]["TrLicDt"]);
                        txtTrValDt.Text = Convert.ToString(dt.Rows[0]["TrValDt"]);
                        txtTrRemarks.Text = Convert.ToString(dt.Rows[0]["TrRemarks"]);

                        txtRbiPrt1.Text = Convert.ToString(dt.Rows[0]["RBIPart1Code"]);
                        txtRbiPrt2.Text = Convert.ToString(dt.Rows[0]["RBIPart2Code"]);
                        txtTier.Text = Convert.ToString(dt.Rows[0]["TIER"]);
                        txtCity.Text = Convert.ToString(dt.Rows[0]["City"]);

                        ddlPopulationType.SelectedIndex = ddlPopulationType.Items.IndexOf(ddlPopulationType.Items.FindByValue(dt.Rows[0]["POPULATIONTYPE"].ToString()));

                        if (Convert.ToString(dt.Rows[0]["AgencyType"]) == "Y")
                            chkAgencyType.Checked = true;
                        else
                            chkAgencyType.Checked = false;
                        if (Convert.ToString(dt.Rows[0]["BioMetryYN"]) == "Y")
                            cbBiometry.Checked = true;
                        else
                            cbBiometry.Checked = false;
                        if (Convert.ToString(dt.Rows[0]["IDBIYN"]) == "Y")
                            ChkOTP.Checked = true;
                        else
                            ChkOTP.Checked = false;

                        if (Convert.ToString(dt.Rows[0]["DigiDocYN"]) == "Y")
                            ChkDigiSign.Checked = true;
                        else
                            ChkDigiSign.Checked = false;

                        if (Convert.ToString(dt.Rows[0]["AadherBasedDigiDocYN"]) == "Y")
                            ChkAadharDigiSign.Checked = true;
                        else
                            ChkAadharDigiSign.Checked = false;

                        if (Convert.ToString(dt.Rows[0]["SaralOpenMktAllow"]) == "Y")
                            chkOpenMkt.Checked = true;
                        else
                            chkOpenMkt.Checked = false;

                        if (Convert.ToString(dt.Rows[0]["FreshCustNotAllow"]) == "Y")
                            chkFreshCustNotAllow.Checked = true;
                        else
                            chkFreshCustNotAllow.Checked = false;

                        if (Convert.ToString(dt.Rows[0]["CBSApplicableYN"]) == "Y")
                            chkCBS.Checked = true;
                        else
                            chkCBS.Checked = false;

                        if (Convert.ToString(dt.Rows[0]["ThirdWeekNotAllow"]) == "Y")
                            chkThirdWeekAllow.Checked = true;
                        else
                            chkThirdWeekAllow.Checked = false;

                        ddlAM.SelectedIndex = ddlAM.Items.IndexOf(ddlAM.Items.FindByValue(dt.Rows[0]["AMId"].ToString()));
                        ddlUM.SelectedIndex = ddlUM.Items.IndexOf(ddlUM.Items.FindByValue(dt.Rows[0]["UMId"].ToString()));

                        ddlMELAM.SelectedIndex = ddlMELAM.Items.IndexOf(ddlMELAM.Items.FindByValue(dt.Rows[0]["MELAMId"].ToString()));
                        ddlMELUM.SelectedIndex = ddlMELUM.Items.IndexOf(ddlMELUM.Items.FindByValue(dt.Rows[0]["MELUMId"].ToString()));

                        ddlZH.SelectedIndex = ddlZH.Items.IndexOf(ddlZH.Items.FindByValue(dt.Rows[0]["ZHId"].ToString()));
                        ddlMelZH.SelectedIndex = ddlMelZH.Items.IndexOf(ddlMelZH.Items.FindByValue(dt.Rows[0]["MelZHId"].ToString()));
                        ddlSH.SelectedIndex = ddlSH.Items.IndexOf(ddlSH.Items.FindByValue(dt.Rows[0]["SHId"].ToString()));
                        ddlMelSH.SelectedIndex = ddlMelSH.Items.IndexOf(ddlMelSH.Items.FindByValue(dt.Rows[0]["MelSHId"].ToString()));

                        ddlDisbBank.SelectedIndex = ddlDisbBank.Items.IndexOf(ddlDisbBank.Items.FindByValue(dt.Rows[0]["DisbBank"].ToString()));
                        ddlBank.SelectedIndex = ddlBank.Items.IndexOf(ddlBank.Items.FindByValue(dt.Rows[0]["Bank"].ToString()));
                        ddlCM.SelectedIndex = ddlCM.Items.IndexOf(ddlCM.Items.FindByValue(dt.Rows[0]["CMId"].ToString()));
                        txtEffectiveDate.Text = Convert.ToString(dt.Rows[0]["EffDate"]);

                        ddlBranchCat.SelectedIndex = ddlBranchCat.Items.IndexOf(ddlBranchCat.Items.FindByValue(dt.Rows[0]["BCBranchYN"].ToString()));

                        
                        ddlPtnr.SelectedIndex = ddlPtnr.Items.IndexOf(ddlPtnr.Items.FindByValue(dt.Rows[0]["BCPartner"].ToString()));
                        ddlMainBranch.SelectedIndex = ddlMainBranch.Items.IndexOf(ddlMainBranch.Items.FindByValue(dt.Rows[0]["MainBranchCode"].ToString()));

                        chkInactiveYN.Checked = dt.Rows[0]["EntType"].ToString() == "D" ? true : false;
                        txtInactiveDt.Text = dt.Rows[0]["InActiveDt"].ToString() == "01/01/1900" ? "" : dt.Rows[0]["InActiveDt"].ToString();

                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();

                        dt = oBr.GetBranchDistDtlByBranch(vBCode);
                        for (int i = 0; i < cblDist.Items.Count; i++)
                        {
                            DataRow[] dr = dt.Select("DistrictId='" + cblDist.Items[i].Value + "'");
                            if (dr.Count<DataRow>() > 0)
                            {
                                cblDist.Items[i].Selected = true;
                            }
                        }
                        tbBrnh.ActiveTabIndex = 1;
                        if (Session[gblValue.BrnchCode].ToString() != "0000")
                            StatusButton("Exit");
                        else
                            StatusButton("Show");
                    }

                    gvKey.DataSource = dt1;
                    gvKey.DataBind();
                    ViewState["ItmData"] = dt1;
                }
            }
            finally
            {
                dt = null;
                oBr = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool ValidateFields()
        {
            bool vRes = true;
            if (gblFuction.IsEmail(txtEmail.Text.Trim()) == false)
            {
                gblFuction.MsgPopup("Please Enter a Valid Email");
                return vRes = false;
            }
            if (gblFuction.IsDate(txtOpDt.Text.Trim()) == false)
            {
                gblFuction.MsgPopup("Opening Date is not in DD/MM/YYYY");
                return vRes = false;
            }
            if (gblFuction.IsDate(txtDayBeginDt.Text.Trim()) == false)
            {
                gblFuction.MsgPopup("Day Begin Date is not in DD/MM/YYYY");
                return vRes = false;
            }
            if ((gblFuction.IsDate(txtAgDt.Text.Trim()) == false) && (txtAgDt.Text.Trim() != ""))
            {
                gblFuction.MsgPopup("Agre Date is not in DD/MM/YYYY");
                return vRes = false;
            }
            if ((gblFuction.IsDate(txtToDt.Text.Trim()) == false) && (txtToDt.Text.Trim() != ""))
            {
                gblFuction.MsgPopup("Valid Till is not in DD/MM/YYYY");
                return vRes = false;
            }
            if ((gblFuction.setDate(txtAgDt.Text.Trim()) >= gblFuction.setDate(txtToDt.Text.Trim())) && (txtAgDt.Text.Trim() != "") && (txtToDt.Text.Trim() != ""))
            {
                gblFuction.MsgPopup("Valid Till Cannot be less than Agre Date");
                return vRes = false;
            }
            if (txtBrCode.Text.Trim().Length != 4)
            {
                gblFuction.MsgPopup("Branch Code should be 4 digit");
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
            if (ValidateFields() == false)
                return false;
            string vXml = DtToXml();
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vDisId = Convert.ToString(ViewState["BCode"]), vAgencyType = "N", vBiometryYn = "N";
            Int32 vErr = 0, vRec = 0;
            double vMRent = 0, vAdvPaid = 0;
            CBranch oBrch = null;
            CGblIdGenerator oGbl = null;
            DataTable dtDtl = null, dt = null;
            string vXmlMtn = string.Empty;
            string vBCBranchYN = "N";
            try
            {
                if (txtMRnt.Text.Trim() != "") vMRent = Convert.ToDouble(txtMRnt.Text);
                if (txtAdv.Text.Trim() != "") vAdvPaid = Convert.ToDouble(txtAdv.Text);
                if (chkAgencyType.Checked == true) vAgencyType = "Y";
                if (cbBiometry.Checked == true) vBiometryYn = "Y";
                dtDtl = (DataTable)ViewState["ItmData"];
                dt = new DataTable();
                dt = dtDtl.Clone();
                int vcount = 1;

                vBCBranchYN = ddlBranchCat.SelectedValue;
                
                foreach (GridViewRow gr in gvKey.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["SlNo"] = vcount;
                    TextBox txtMtnDt = (TextBox)gr.FindControl("txtMtnDt");
                    TextBox txtFrom = (TextBox)gr.FindControl("txtFrom");
                    TextBox txtTo = (TextBox)gr.FindControl("txtTo");
                    TextBox txtKeyNo = (TextBox)gr.FindControl("txtKeyNo");
                    TextBox txtDescr = (TextBox)gr.FindControl("txtDescr");

                    dr["MtnDt"] = gblFuction.setDate(txtMtnDt.Text);
                    if (txtFrom.Text == "&nbsp;")
                        dr["NameFrom"] = "";
                    else
                        dr["NameFrom"] = txtFrom.Text;

                    if (txtTo.Text == "&nbsp;")
                        dr["NameTo"] = "";
                    else
                        dr["NameTo"] = txtTo.Text;

                    if (txtKeyNo.Text == "&nbsp;")
                        dr["KeyNo"] = "";
                    else
                        dr["KeyNo"] = txtKeyNo.Text;

                    if (txtDescr.Text == "&nbsp;")
                        dr["Descr"] = "";
                    else
                        dr["Descr"] = txtDescr.Text;

                    if (dr["MtnDt"].ToString() != "&nbsp;")
                    {
                        dt.Rows.Add(dr);
                        dt.AcceptChanges();
                    }
                    vcount = vcount + 1;
                }
                if (dt.Rows.Count <= 0)
                {
                    gblFuction.MsgPopup("Please enter valid data.");
                    return false;
                }
                if (ddlBranchCat.SelectedValue == "Y")
                {
                    if (ddlPtnr.SelectedValue == "-1")
                    {
                        gblFuction.MsgPopup("Please Select a BC Partner...");
                        return false;

                    }
                    if (ddlMainBranch.SelectedValue == "-1")
                    {
                        gblFuction.MsgPopup("Please Select Main Branch...");
                        return false;
                    }
                }
                else
                {
                    if (ddlPtnr.SelectedValue != "-1")
                    {
                        gblFuction.MsgPopup("BC Partner can not be applicable for this branch category...");
                        return false;

                    }
                    if (ddlMainBranch.SelectedValue != "-1")
                    {
                        gblFuction.MsgPopup("Main Branch selction can not be applicable for this branch category...");
                        return false;
                    }
                }

                dt.TableName = "Table1";
                vXmlMtn = DtToMtnXml(dt);
                this.GetModuleByRole(mnuID.mnuBranchMst);
                if (!ddlBank.SelectedItem.Text.Contains(ddlDisbBank.SelectedValue))
                {
                    gblFuction.MsgPopup("Please Select Proper Bank Ledger...Bank Ledger is not belongs to " + ddlDisbBank.SelectedValue + " Bank. ");
                    return false;
                }
                if (Mode == "Save" || Mode == "Edit")
                {
                    if (ddlBranchCat.SelectedValue == "Y" || ddlBranchCat.SelectedValue == "N")
                    {
                        if (chkCBS.Checked == true)
                        {
                            gblFuction.MsgPopup("CBS can not be applicable for this Branch category...");
                            return false;
                        }
                    }
                    
                }
                if (Mode == "Save")
                {
                    oBrch = new CBranch();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("BranchMst", "BranchCode", txtBrCode.Text.Replace("'", "''"), "", "", "", vDisId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Branch Code Can not be Duplicate...");
                        return false;
                    }


                    vErr = oBrch.SaveBranch(txtBrCode.Text.Replace("'", "''"), txtBrName.Text.Replace("'", "''"), Convert.ToInt32(ddlState.SelectedValue),
                        Convert.ToInt32(ddlReg.SelectedValue), Convert.ToInt32(ddlArea.SelectedValue), gblFuction.setDate(txtOpDt.Text), gblFuction.setDate(txtDayBeginDt.Text), txtAdd.Text.Replace("'", "''"),
                        ddlBType.SelectedValue, txtDist.Text.Replace("'", "''"), txtCityCode.Text.Replace("'", "''"), txtCityNo.Text.Replace("'", "''"), txtPS.Text.Replace("'", "''"), txtPin.Text.Replace("'", "''"),
                        gblFuction.setDate(Session[gblValue.LoginDate].ToString()), txtPhNo.Text.Replace("'", "''"), txtEmail.Text.Replace("'", "''"),
                        gblFuction.setDate(txtAgDt.Text), gblFuction.setDate(txtToDt.Text), vMRent, vAdvPaid, vXml, vXmlMtn, vAgencyType, this.UserID, "Save",
                        txtTrNo.Text.Replace("'", "''"), gblFuction.setDate(txtTrLicDt.Text), gblFuction.setDate(txtTrValDt.Text), txtTrRemarks.Text.Replace("'", "''"), txtPrimarySolId.Text.Replace("'", "''"),
                        vBiometryYn, Convert.ToInt32(ddlAreaCategory.SelectedValue), ddlCbType.SelectedValue, ddlAM.SelectedValue, ddlUM.SelectedValue
                        , ddlDisbBank.SelectedValue, ddlBank.SelectedValue, ChkOTP.Checked == true ? "Y" : "N", ChkDigiSign.Checked == true ? "Y" : "N",
                        ChkAadharDigiSign.Checked == true ? "Y" : "N", ddlCM.SelectedValue, gblFuction.setDate(txtEffectiveDate.Text), vBCBranchYN, Convert.ToInt32(ddlPtnr.SelectedValue), ddlMainBranch.SelectedValue
                        , ddlMELAM.SelectedValue, ddlMELUM.SelectedValue, chkOpenMkt.Checked == true ? "Y" : "N", chkInactiveYN.Checked == true ? "Y" : "N", gblFuction.setDate(txtInactiveDt.Text), chkThirdWeekAllow.Checked == true ? "Y" : "N",
                        chkFreshCustNotAllow.Checked == true ? "Y" : "N", txtRbiPrt1.Text, txtRbiPrt2.Text, ddlPopulationType.SelectedValue, Convert.ToInt32(txtTier.Text), txtCity.Text,
                        ddlZH.SelectedValue, ddlMelZH.SelectedValue, ddlSH.SelectedValue, ddlMelSH.SelectedValue, chkCBS.Checked == true ? "Y" : "N");
                    if (vErr > 0)
                    {
                        ViewState["BCode"] = txtBrCode.Text;
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
                    oBrch = new CBranch();
                    vErr = oBrch.SaveBranch(txtBrCode.Text.Replace("'", "''"), txtBrName.Text.Replace("'", "''"), Convert.ToInt32(ddlState.SelectedValue),
                        Convert.ToInt32(ddlReg.SelectedValue), Convert.ToInt32(ddlArea.SelectedValue), gblFuction.setDate(txtOpDt.Text), gblFuction.setDate(txtDayBeginDt.Text), txtAdd.Text.Replace("'", "''"),
                        ddlBType.SelectedValue, txtDist.Text.Replace("'", "''"), txtCityCode.Text.Replace("'", "''"), txtCityNo.Text.Replace("'", "''"), txtPS.Text.Replace("'", "''"), txtPin.Text.Replace("'", "''"),
                        gblFuction.setDate(Session[gblValue.LoginDate].ToString()), txtPhNo.Text.Replace("'", "''"), txtEmail.Text.Replace("'", "''"),
                        gblFuction.setDate(txtAgDt.Text), gblFuction.setDate(txtToDt.Text), vMRent, vAdvPaid, vXml, vXmlMtn, vAgencyType, this.UserID, "Edit",
                        txtTrNo.Text.Replace("'", "''"), gblFuction.setDate(txtTrLicDt.Text), gblFuction.setDate(txtTrValDt.Text), txtTrRemarks.Text.Replace("'", "''"), txtPrimarySolId.Text.Replace("'", "''"),
                        vBiometryYn, Convert.ToInt32(ddlAreaCategory.SelectedValue), ddlCbType.SelectedValue, ddlAM.SelectedValue, ddlUM.SelectedValue
                        , ddlDisbBank.SelectedValue, ddlBank.SelectedValue, ChkOTP.Checked == true ? "Y" : "N", ChkDigiSign.Checked == true ? "Y" : "N",
                        ChkAadharDigiSign.Checked == true ? "Y" : "N", ddlCM.SelectedValue, gblFuction.setDate(txtEffectiveDate.Text), vBCBranchYN, Convert.ToInt32(ddlPtnr.SelectedValue), ddlMainBranch.SelectedValue
                        , ddlMELAM.SelectedValue, ddlMELUM.SelectedValue, chkOpenMkt.Checked == true ? "Y" : "N", chkInactiveYN.Checked == true ? "Y" : "N", gblFuction.setDate(txtInactiveDt.Text), chkThirdWeekAllow.Checked == true ? "Y" : "N",
                        chkFreshCustNotAllow.Checked == true ? "Y" : "N", txtRbiPrt1.Text, txtRbiPrt2.Text, ddlPopulationType.SelectedValue, Convert.ToInt32(txtTier.Text), txtCity.Text,
                        ddlZH.SelectedValue, ddlMelZH.SelectedValue, ddlSH.SelectedValue, ddlMelSH.SelectedValue, chkCBS.Checked == true ? "Y" : "N");
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
                    oBrch = new CBranch();
                    vErr = oBrch.SaveBranch(txtBrCode.Text.Replace("'", "''"), txtBrName.Text.Replace("'", "''"), Convert.ToInt32(ddlState.SelectedValue),
                        Convert.ToInt32(ddlReg.SelectedValue), Convert.ToInt32(ddlArea.SelectedValue), gblFuction.setDate(txtOpDt.Text), gblFuction.setDate(txtDayBeginDt.Text), txtAdd.Text.Replace("'", "''"),
                        ddlBType.SelectedValue, txtDist.Text.Replace("'", "''"), txtCityCode.Text.Replace("'", "''"), txtCityNo.Text.Replace("'", "''"), txtPS.Text.Replace("'", "''"), txtPin.Text.Replace("'", "''"),
                        gblFuction.setDate(Session[gblValue.LoginDate].ToString()), txtPhNo.Text.Replace("'", "''"), txtEmail.Text.Replace("'", "''"),
                        gblFuction.setDate(txtAgDt.Text), gblFuction.setDate(txtToDt.Text), vMRent, vAdvPaid, vXml, vXmlMtn, vAgencyType, this.UserID, "Del",
                        txtTrNo.Text.Replace("'", "''"), gblFuction.setDate(txtTrLicDt.Text), gblFuction.setDate(txtTrValDt.Text), txtTrRemarks.Text.Replace("'", "''"), txtPrimarySolId.Text.Replace("'", "''"),
                        vBiometryYn, Convert.ToInt32(ddlAreaCategory.SelectedValue), ddlCbType.SelectedValue, ddlAM.SelectedValue, ddlUM.SelectedValue
                          , ddlDisbBank.SelectedValue, ddlBank.SelectedValue, ChkOTP.Checked == true ? "Y" : "N", ChkDigiSign.Checked == true ? "Y" : "N",
                          ChkAadharDigiSign.Checked == true ? "Y" : "N", ddlCM.SelectedValue, gblFuction.setDate(txtEffectiveDate.Text), vBCBranchYN, Convert.ToInt32(ddlPtnr.SelectedValue), ddlMainBranch.SelectedValue
                          , ddlMELAM.SelectedValue, ddlMELUM.SelectedValue, chkOpenMkt.Checked == true ? "Y" : "N", chkInactiveYN.Checked == true ? "Y" : "N", gblFuction.setDate(txtInactiveDt.Text), chkThirdWeekAllow.Checked == true ? "Y" : "N",
                          chkFreshCustNotAllow.Checked == true ? "Y" : "N", txtRbiPrt1.Text, txtRbiPrt2.Text, ddlPopulationType.SelectedValue, Convert.ToInt32(txtTier.Text), txtCity.Text,
                          ddlZH.SelectedValue, ddlMelZH.SelectedValue, ddlSH.SelectedValue, ddlMelSH.SelectedValue, chkCBS.Checked == true ? "Y" : "N");
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
                oBrch = null;
                oGbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            cbBiometry.Enabled = Status;
            ddlState.Enabled = Status;
            ddlReg.Enabled = Status;
            ddlArea.Enabled = Status;
            txtBrCode.Enabled = Status;
            txtBrName.Enabled = Status;
            txtOpDt.Enabled = Status;
            txtDayBeginDt.Enabled = Status;
            cblDist.Enabled = Status;
            txtDist.Enabled = Status;
            txtPS.Enabled = Status;
            txtAdd.Enabled = Status;
            txtPin.Enabled = Status;
            txtPhNo.Enabled = Status;
            txtEmail.Enabled = Status;
            ddlBType.Enabled = Status;
            ddlAreaCategory.Enabled = Status;
            ddlCbType.Enabled = Status;
            txtAgDt.Enabled = Status;
            txtToDt.Enabled = Status;
            txtMRnt.Enabled = Status;
            txtAdv.Enabled = Status;
            txtTrNo.Enabled = Status;
            txtTrLicDt.Enabled = Status;
            txtTrValDt.Enabled = Status;
            txtTrRemarks.Enabled = Status;
            chkAgencyType.Enabled = Status;
            gvKey.Enabled = Status;
            txtPrimarySolId.Enabled = Status;
            txtCityCode.Enabled = Status;
            txtCityNo.Enabled = Status;
            ddlAM.Enabled = Status;
            ddlUM.Enabled = Status;
            ddlMELAM.Enabled = Status;
            ddlMELUM.Enabled = Status;

            ddlZH.Enabled = Status;
            ddlMelZH.Enabled = Status;
            ddlSH.Enabled = Status;
            ddlMelSH.Enabled = Status;

            ddlDisbBank.Enabled = Status;
            ddlBank.Enabled = Status;
            ChkOTP.Enabled = Status;
            ChkDigiSign.Enabled = Status;
            ChkAadharDigiSign.Enabled = Status;

            ddlCM.Enabled = Status;
            txtEffectiveDate.Enabled = Status;

            ddlBranchCat.Enabled = Status;
            ddlPtnr.Enabled = Status;
            ddlMainBranch.Enabled = Status;
            chkOpenMkt.Enabled = Status;
            chkThirdWeekAllow.Enabled = Status;

            chkInactiveYN.Enabled = Status;
            txtInactiveDt.Enabled = Status;
            chkFreshCustNotAllow.Enabled = Status;

            txtRbiPrt1.Enabled = Status;
            txtRbiPrt2.Enabled = Status;
            ddlPopulationType.Enabled = Status;
            txtTier.Enabled = Status;
            txtCity.Enabled = Status;
            chkCBS.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            cbBiometry.Checked = false;
            ddlState.SelectedIndex = -1;
            ddlReg.SelectedIndex = -1;
            ddlArea.SelectedIndex = -1;
            txtBrCode.Text = "";
            txtBrName.Text = "";
            txtOpDt.Text = "";
            txtDayBeginDt.Text = "";
            cblDist.SelectedIndex = -1;
            txtDist.Text = "";
            txtPS.Text = "";
            txtAdd.Text = "";
            txtPin.Text = "";
            txtPhNo.Text = "";
            txtEmail.Text = "";
            ddlBType.SelectedIndex = -1;
            ddlAreaCategory.SelectedIndex = -1;
            ddlCbType.SelectedIndex = -1;
            txtAgDt.Text = "";
            txtToDt.Text = "";
            txtMRnt.Text = "";
            txtAdv.Text = "";
            txtTrNo.Text = "";
            txtTrLicDt.Text = "";
            txtTrValDt.Text = "";
            txtTrRemarks.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
            txtPrimarySolId.Text = "";
            txtCityCode.Text = "";
            txtCityNo.Text = "";
            chkAgencyType.Checked = false;
            SetInitilize();
            ddlAM.SelectedIndex = -1;
            ddlUM.SelectedIndex = -1;
            ddlMELAM.SelectedIndex = -1;
            ddlMELUM.SelectedIndex = -1;

            ddlZH.SelectedIndex = -1;
            ddlMelZH.SelectedIndex = -1;
            ddlSH.SelectedIndex = -1;
            ddlMelSH.SelectedIndex = -1;

            ddlDisbBank.SelectedIndex = -1;
            ddlBank.SelectedIndex = -1;
            ChkOTP.Checked = false;
            ChkDigiSign.Checked = false;
            ChkAadharDigiSign.Checked = false;

            ddlCM.SelectedIndex = -1;
            txtEffectiveDate.Text = "";

            ddlBranchCat.SelectedIndex = -1;
            ddlPtnr.SelectedIndex = 0;
            ddlMainBranch.SelectedIndex = 0;
            chkOpenMkt.Checked = false;
            chkInactiveYN.Checked = false;
            txtInactiveDt.Text = "";

            chkThirdWeekAllow.Checked = false;
            chkFreshCustNotAllow.Checked = false;
            chkCBS.Checked = false;

            txtRbiPrt1.Text = "";
            txtRbiPrt2.Text = "";
            ddlPopulationType.SelectedIndex = -1;
            txtTier.Text = "";
            txtCity.Text = "";
        }

        private void SetInitilize()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SlNo", typeof(int)));
            dt.Columns.Add(new DataColumn("MtnDt", typeof(string)));
            dt.Columns.Add(new DataColumn("NameFrom", typeof(string)));
            dt.Columns.Add(new DataColumn("NameTo", typeof(string)));
            dt.Columns.Add(new DataColumn("KeyNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Descr", typeof(string)));
            DataRow dr = dt.NewRow();
            dr["SlNo"] = 1;
            dr["MtnDt"] = "";
            dr["NameFrom"] = "";
            dr["NameTo"] = "";
            dr["KeyNo"] = "";
            dr["Descr"] = "";
            dt.Rows.Add(dr);
            ViewState["ItmData"] = dt;
            gvKey.DataSource = dt;
            gvKey.DataBind();
        }

        protected void gvKey_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow Row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            int vRow = Row.RowIndex;
            TextBox txtMtnDt = (TextBox)gvKey.Rows[vRow].FindControl("txtMtnDt");
            TextBox txtFrom = (TextBox)gvKey.Rows[vRow].FindControl("txtFrom");
            TextBox txtTo = (TextBox)gvKey.Rows[vRow].FindControl("txtTo");
            TextBox txtKeyNo = (TextBox)gvKey.Rows[vRow].FindControl("txtKeyNo");
            TextBox txtDescr = (TextBox)gvKey.Rows[vRow].FindControl("txtDescr");

            if (e.CommandName == "cmdNewRec")
            {
                if (txtMtnDt.Text != "" && txtFrom.Text != "" && txtTo.Text != "" && txtKeyNo.Text != "")
                {
                    NewRow();
                }
                else
                {
                    gblFuction.MsgPopup("Please fill all collumn");
                    return;
                }
            }
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
                        TextBox txtMtnDt = (TextBox)gvKey.Rows[i].FindControl("txtMtnDt");
                        TextBox txtFrom = (TextBox)gvKey.Rows[i].FindControl("txtFrom");
                        TextBox txtTo = (TextBox)gvKey.Rows[i].FindControl("txtTo");
                        TextBox txtKeyNo = (TextBox)gvKey.Rows[i].FindControl("txtKeyNo");
                        TextBox txtDescr = (TextBox)gvKey.Rows[i].FindControl("txtDescr");

                        dtNew.Rows[i]["MtnDt"] = txtMtnDt.Text;
                        dtNew.Rows[i]["NameFrom"] = txtFrom.Text;
                        dtNew.Rows[i]["NameTo"] = txtTo.Text;
                        dtNew.Rows[i]["KeyNo"] = txtKeyNo.Text;
                        dtNew.Rows[i]["Descr"] = txtDescr.Text;
                    }
                }
                gvKey.DataSource = dtNew;
                gvKey.DataBind();
            }
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
            gvKey.DataSource = dt;
            gvKey.DataBind();
        }

        private string DtToMtnXml(DataTable dtXml)
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

        private void popBCName()
        {
            DataTable dt = null;

            CMember oMem = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oMem = new CMember();
                dt = oMem.GetBCName("0000", vLogDt);
                ddlPtnr.DataSource = dt;
                ddlPtnr.DataTextField = "BCName";
                ddlPtnr.DataValueField = "BCId";
                ddlPtnr.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlPtnr.Items.Insert(0, oli);



            }
            finally
            {
                //oRO = null;
                dt = null;

            }
        }

        private void popMainBranch()
        {
            DataTable dt = null;

            CMember oMem = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oMem = new CMember();
                dt = oMem.GetMainBranch("0000", vLogDt);
                ddlMainBranch.DataSource = dt;
                ddlMainBranch.DataTextField = "BranchName";
                ddlMainBranch.DataValueField = "BranchCode";
                ddlMainBranch.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlMainBranch.Items.Insert(0, oli);



            }
            finally
            {
                //oRO = null;
                dt = null;

            }
        }
    }
}