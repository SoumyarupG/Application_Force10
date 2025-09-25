using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
namespace CENTRUM.WebPages.Private.Master
{
    public partial class NpsMember : CENTRUMBase
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
                StatusButton("View");
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                txtMemNo.Enabled = false; 
                popAgent();
                popRelation();
                PopRO();
                LoadGrid(1);
                tbEmp.ActiveTabIndex = 0;
                txtSubDt.Text = (string)Session[gblValue.LoginDate];
                txtSubDt.Enabled = false;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void popRelation()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "HumanRelationId", "HumanRelationName", "HumanRelationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlRel1.DataSource = dt;
                ddlRel1.DataTextField = "HumanRelationName";
                ddlRel1.DataValueField = "HumanRelationId";
                ddlRel1.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlRel1.Items.Insert(0, oli);

                ddlRel2.DataSource = dt;
                ddlRel2.DataTextField = "HumanRelationName";
                ddlRel2.DataValueField = "HumanRelationId";
                ddlRel2.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlRel2.Items.Insert(0, oli1);

                ddlRel3.DataSource = dt;
                ddlRel3.DataTextField = "HumanRelationName";
                ddlRel3.DataValueField = "HumanRelationId";
                ddlRel3.DataBind();
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ddlRel3.Items.Insert(0, oli2);
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
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Member";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuNpsMemMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Member Subscription", false);
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
                    btnDelete.Enabled = true;
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
        private void popAgent()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "AgntID", "Name", "NPS_AgencyMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlAgnt.DataSource = dt;
                ddlAgnt.DataTextField = "Name";
                ddlAgnt.DataValueField = "AgntID";
                ddlAgnt.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlAgnt.Items.Insert(0, oli);
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
        /// <param name="Status"></param>
        private void PopRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlRO.DataSource = dt;
                ddlRO.DataTextField = "EoName";
                ddlRO.DataValueField = "Eoid";
                ddlRO.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlRO.Items.Insert(0, oli);
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
        /// <param name="Status"></param>
        private void PopCenter(string pEoId)
        {
            DataTable dt = null;
            CLoanRecovery oCL = null;
            try
            {

                oCL = new CLoanRecovery();
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                dt = oCL.PopCenterWithCollDay(pEoId, gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode, "N"); //With No CollDay
                if (dt.Rows.Count >= 0)
                {
                    ViewState["CentreForDt"] = dt;
                    ddlCenter.DataSource = dt;
                    ddlCenter.DataTextField = "Market";
                    ddlCenter.DataValueField = "MarketID";
                    ddlCenter.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlCenter.Items.Insert(0, oli);
                }
                else
                {
                    ddlCenter.DataSource = null;  
                    ddlCenter.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCL = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRO_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vEoId = ddlRO.SelectedItem.Value;
            PopCenter(vEoId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtMemId.Enabled = Status;
            ddlOwn.Enabled = Status;
            if (ddlOwn.SelectedValue == "Y" )
                txtMemNo.Enabled = true;
            else
                txtMemNo.Enabled = false;
            txtMemNo.Enabled = Status;
            txtMemName.Enabled = Status;
            //txtSubDt.Enabled = Status;
            txtDoB.Enabled = Status;
            ddlGender.Enabled = Status;
            txtFather.Enabled = Status;
            txtAddress.Enabled = Status;
            txtVill.Enabled = Status;
            txtDist.Enabled = Status;
            ddlState.Enabled = Status;
            txtPin.Enabled = Status;
            txtPhNo.Enabled = Status;
            txtNom1.Enabled = Status;
            ddlRel1.Enabled = Status;
            txtAge1.Enabled = Status;
            txtPer1.Enabled = Status;
            txtGur1.Enabled = Status;
            txtNom2.Enabled = Status;
            ddlRel2.Enabled = Status;
            txtAge2.Enabled = Status;
            txtPer2.Enabled = Status;
            txtGur2.Enabled = Status;
            txtNom3.Enabled = Status;
            ddlRel3.Enabled = Status;
            txtAge3.Enabled = Status;
            txtPer3.Enabled = Status;
            txtGur3.Enabled = Status;
            txtDpAmt.Enabled = Status;
            if (Convert.ToString(Session[gblValue.AgencyType]) == "N")
            {
                ddlAgnt.Enabled = false;
            }
            else
            {
                ddlAgnt.Enabled = true;
            }
            ddlRO.Enabled = Status;
            ddlCenter.Enabled = Status;
        }


        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtMemId.Text = "";
            txtMemNo.Text = "";
            txtMemName.Text = "";
            txtSubDt.Text = (string)Session[gblValue.LoginDate];
            txtDoB.Text = "";
            ddlGender.SelectedIndex = -1;
            txtFather.Text = "";
            txtAddress.Text = "";
            txtVill.Text = "";
            txtDist.Text = "";
            ddlState.SelectedIndex = -1;
            txtPin.Text = "";
            txtPhNo.Text = "";
            txtNom1.Text = "";
            txtAge1.Text = "0";
            txtPer1.Text = "0";
            txtGur1.Text = "";
            txtNom2.Text = "";
            ddlRel2.SelectedIndex = -1; 
            txtAge2.Text = "0";
            txtPer2.Text = "0";
            txtGur2.Text = "";
            txtNom3.Text = "";
            ddlRel3.SelectedIndex = -1; 
            txtAge3.Text = "0";
            txtPer3.Text = "0";
            txtGur3.Text = "";
            txtDpAmt.Text = "0";
            ddlAgnt.SelectedIndex = -1;
            ddlRO.SelectedIndex = -1;
            ddlCenter.SelectedIndex = -1;            
            lblDate.Text = "";
            lblUser.Text = "";
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
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (this.CanAdd == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Add);
                return;
            }
            ViewState["StateEdit"] = "Add";
            tbEmp.ActiveTabIndex = 1;
            StatusButton("Add");
            ClearControls();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            CNpsMember oMem = null;
            string vEoId = Convert.ToString(ViewState["MemId"]);
            DataTable dt = null;
            try
            {
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                

                oMem = new CNpsMember();
                dt = oMem.NPS_GetIsRemitted(vEoId);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["IsRemitted"]) == "Y")
                    {
                        gblFuction.MsgPopup("This Member already remitted you can not delete this member");
                        return;
                    }
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
            CNpsMember oMem = null;
            string vEoId = Convert.ToString(ViewState["MemId"]);
            DataTable dt = null;
            
            oMem = new CNpsMember();
            dt = oMem.NPS_GetIsRemitted(vEoId);
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["IsRemitted"]) == "Y")
                {
                    txtDpAmt.Enabled = false;
                }
            }
            
            if (this.CanEdit == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Edit);
                return;
            }
            tbEmp.ActiveTabIndex = 1;
            ViewState["StateEdit"] = "Edit";
            StatusButton("Edit");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbEmp.ActiveTabIndex = 0;
            ClearControls();
            LoadGrid(0);
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
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
                EnableControl(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateField()
        {
            DataTable dt = null;
            CNpsParameter oPrm = null;
            Boolean vResult = true;
            if (txtAge1.Text== "")
            {
                gblFuction.MsgPopup("1st Nominee Age cannot be empty");
                vResult = false;
                return vResult;
            }
            if (txtPer1.Text == "")
            {
                gblFuction.MsgPopup("ist Nominee Percentage cannot be empty");
                vResult = false;
                return vResult;
            }
            if (txtAge2.Text == "")
            {
                gblFuction.MsgPopup("2nd Nominee Age cannot be empty");
                vResult = false;
                return vResult;
            }
            if (txtPer2.Text == "")
            {
                gblFuction.MsgPopup("2nd Nominee Percentage cannot be empty");
                vResult = false;
                return vResult;
            }
            if (txtAge3.Text == "")
            {
                gblFuction.MsgPopup("3rd Nominee Age cannot be empty");
                vResult = false;
                return vResult;
            }
            if (txtPer3.Text == "")
            {
                gblFuction.MsgPopup("3rd Nominee Percentage cannot be empty");
                vResult = false;
                return vResult;
            }
            if (txtNom1.Text != "")
            {
                if (ddlRel1.SelectedIndex < 0)
                {
                    gblFuction.MsgPopup("1st Nominee Relation Cannot be empty");
                    vResult = false;
                    return vResult;
                }
                if (txtAge1.Text == "0" || txtAge1.Text.Trim() == "")
                {
                    gblFuction.MsgPopup("1st Nominee Age Cannot be 0");
                    vResult = false;
                    return vResult;
                }
                if (Convert.ToInt32(txtAge1.Text) > 80)
                {

                    gblFuction.MsgPopup("1st NomineeAge should not exceed 80 yrs");
                    vResult = false;
                    return vResult;

                }
                if (Convert.ToInt32(txtAge1.Text) > 0 && Convert.ToInt32(txtAge1.Text) < 18)
                {
                    if (txtGur1.Text == "")
                    {
                        gblFuction.MsgPopup("1st Nominee Gurdian Cannot be empty");
                        vResult = false;
                        return vResult;
                    }
                }

            }
            if (txtNom2.Text != "")
            {
                if (ddlRel2.SelectedIndex < 0)
                {
                    gblFuction.MsgPopup("2nd Nominee Relation Cannot be empty");
                    vResult = false;
                    return vResult;
                }
                if (txtAge2.Text == "0" || txtAge2.Text.Trim() == "")
                {
                    gblFuction.MsgPopup("2nd Nominee Age Cannot be 0");
                    vResult = false;
                    return vResult;
                }
                if (Convert.ToInt32(txtAge2.Text) > 80)
                {

                    gblFuction.MsgPopup("2nd NomineeAge should not exceed 80 yrs");
                    vResult = false;
                    return vResult;

                }
                if (Convert.ToInt32(txtAge2.Text) > 0 && Convert.ToInt32(txtAge2.Text) < 18)
                {
                    if (txtGur2.Text == "")
                    {
                        gblFuction.MsgPopup("2nd Nominee Gurdian Cannot be empty");
                        vResult = false;
                        return vResult;
                    }
                }
            }
            if (txtNom3.Text != "")
            {
                if (ddlRel3.SelectedIndex < 0)
                {
                    gblFuction.MsgPopup("3rd Nominee Relation Cannot be empty");
                    vResult = false;
                    return vResult;
                }
                if (txtAge3.Text.Trim() == "0" || txtAge3.Text.Trim() == "")
                {
                    gblFuction.MsgPopup("3rd Nominee Age Cannot be 0 or Epmty");
                    vResult = false;
                    return vResult;
                }
                if (Convert.ToInt32(txtAge3.Text) > 80 )
                {
                   
                        gblFuction.MsgPopup("3rd NomineeAge should not exceed 80 yrs");
                        vResult = false;
                        return vResult;
                   
                }
                if (Convert.ToInt32(txtAge3.Text) > 0 && Convert.ToInt32(txtAge3.Text)<18)
                {
                    if (txtGur3.Text == "")
                    {
                        gblFuction.MsgPopup("3rd Nominee Gurdian Cannot be empty");
                        vResult = false;
                        return vResult;
                    }
                }
            }
            if (txtDpAmt.Text == "" || txtDpAmt.Text == "0")
            {
                gblFuction.MsgPopup("Deposit Amount Cannot be 0 or blank");
                vResult = false;
                return vResult;
            }
            if (Convert.ToDouble(txtDpAmt.Text) > 1200 || Convert.ToDouble(txtDpAmt.Text) < 100)
            {
                gblFuction.MsgPopup("Deposit Amount must be within 100 and 1200");
                vResult = false;
                return vResult;
            }
            if (Convert.ToDouble(txtDpAmt.Text) % 100 != 0 )
            {
                gblFuction.MsgPopup("Deposit Amount must be multiple of 100 ");
                vResult = false;
                return vResult;
            }
            if (ddlOwn.SelectedValue == "Y")
            {
                if (txtMemNo.Text == "")
                {
                    gblFuction.MsgPopup("Member No Cannot be empty");
                    vResult = false;
                    return vResult;
                }
            }
            if (Convert.ToDouble(txtDpAmt.Text) < 100 || Convert.ToDouble(txtDpAmt.Text) > 12000)
            {
                gblFuction.MsgPopup("Invalid Amount");
                vResult = false;
                return vResult;
            }
            Int32 vCurrYear = System.DateTime.Now.Year;
            Int32 vAge = Convert.ToInt32(txtDoB.Text.Substring(6, 4));
            Int32 vNoYr = vCurrYear - vAge;
            if (vNoYr < 18 || vNoYr > 60)
            {
                gblFuction.MsgPopup("Age is not between 18 and 60 years");
                vResult = false;
                return vResult;
            }

            if ((Convert.ToDouble(txtPer1.Text) + Convert.ToDouble(txtPer2.Text) + Convert.ToDouble(txtPer3.Text))!=100.0)
            {
                gblFuction.MsgPopup("Total of percentage must be 100%");
                vResult = false;
                return vResult;
            }
            if (Convert.ToString(Session[gblValue.AgencyType]) == "Y")
            {
                if (ddlAgnt.SelectedIndex < 1)
                {
                    gblFuction.MsgPopup("Agency cannot be left blank");
                    vResult = false;
                    return vResult;
                }
            }
            else
            {
                oPrm = new CNpsParameter();
                dt = oPrm.NPS_GetParameterList();
                if (dt.Rows[0]["NPSAc"].ToString().Trim() == "")
                {
                    gblFuction.AjxMsgPopup("NPS Account is not Set");
                    vResult = false;
                    return vResult;
                }
            }
            if (gblFuction.setDate(Session[gblValue.LoginDate].ToString()) < gblFuction.setDate(txtSubDt.Text))
            {
                gblFuction.AjxMsgPopup("Submission Dt. should not be greater than Login Date");
                vResult = false;
                return vResult;
            }
            
            return vResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0;
            CNpsMember oMem = null;
            string vBranch = Session[gblValue.BrnchCode].ToString();

            try
            {
                oMem = new CNpsMember();
                dt = oMem.NPS_GetMemListPG(vBranch,txtSearch.Text.Trim(), pPgIndx, ref vTotRows);
                gvEmp.DataSource = dt;
                gvEmp.DataBind();
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
                oMem = null;
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
            tbEmp.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEmp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vMemId = "";
            DataTable dt = null;
            CNpsMember oMem = null;
            try
            {
                vMemId = Convert.ToString(e.CommandArgument);
                ViewState["MemId"] = vMemId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvEmp.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oMem = new CNpsMember();
                    dt = oMem.NPS_GetMemberByID(vMemId);
                    if (dt.Rows.Count > 0)
                    {
                        txtMemId.Text = Convert.ToString(dt.Rows[0]["MemID"]);
                        txtMemNo.Text = Convert.ToString(dt.Rows[0]["MemNo"]);
                        txtMemName.Text = Convert.ToString(dt.Rows[0]["MemName"]);
                        ddlOwn.SelectedIndex = ddlOwn.Items.IndexOf(ddlOwn.Items.FindByValue(dt.Rows[0]["Own"].ToString()));  
                        txtSubDt.Text = Convert.ToString(dt.Rows[0]["SubmissionDt"]);
                        ddlRO.SelectedIndex = ddlRO.Items.IndexOf(ddlRO.Items.FindByValue(dt.Rows[0]["EoId"].ToString()));
                        PopCenter(ddlRO.SelectedValue);
                        ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(dt.Rows[0]["MarketID"].ToString()));      
                        txtDoB.Text = Convert.ToString(dt.Rows[0]["DOB"]);
                        ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindByValue(dt.Rows[0]["Gender"].ToString()));
                        txtFather.Text = Convert.ToString(dt.Rows[0]["Father"]);
                        txtAddress.Text = Convert.ToString(dt.Rows[0]["Address"]);
                        ddlAgnt.SelectedIndex = ddlAgnt.Items.IndexOf(ddlAgnt.Items.FindByValue(dt.Rows[0]["AgentId"].ToString()));      
                        txtVill.Text = Convert.ToString(dt.Rows[0]["Vill"]);
                        txtDist.Text = Convert.ToString(dt.Rows[0]["Dist"]);
                        ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(dt.Rows[0]["State"].ToString()));  
                        txtPin.Text = Convert.ToString(dt.Rows[0]["Pin"]);
                        txtPhNo.Text = Convert.ToString(dt.Rows[0]["PhNo"]);
                        txtNom1.Text = Convert.ToString(dt.Rows[0]["Nom1"]);
                        txtNom2.Text = Convert.ToString(dt.Rows[0]["Nom2"]);
                        txtNom3.Text = Convert.ToString(dt.Rows[0]["Nom3"]);
                        ddlRel1.SelectedIndex = ddlRel1.Items.IndexOf(ddlRel1.Items.FindByValue(dt.Rows[0]["Rel1"].ToString()));
                        ddlRel2.SelectedIndex = ddlRel2.Items.IndexOf(ddlRel2.Items.FindByValue(dt.Rows[0]["Rel2"].ToString()));
                        ddlRel3.SelectedIndex = ddlRel3.Items.IndexOf(ddlRel3.Items.FindByValue(dt.Rows[0]["Rel3"].ToString()));

                        txtAge1.Text = Convert.ToString(dt.Rows[0]["Age1"]);
                        txtAge2.Text = Convert.ToString(dt.Rows[0]["Age2"]);
                        txtAge3.Text = Convert.ToString(dt.Rows[0]["Age3"]);
                        txtPer1.Text = Convert.ToString(dt.Rows[0]["Per1"]);
                        txtPer2.Text = Convert.ToString(dt.Rows[0]["Per2"]);
                        txtPer3.Text = Convert.ToString(dt.Rows[0]["Per3"]);
                        txtGur1.Text = Convert.ToString(dt.Rows[0]["Gurdian1"]);
                        txtGur2.Text = Convert.ToString(dt.Rows[0]["Gurdian2"]);
                        txtGur3.Text = Convert.ToString(dt.Rows[0]["Gurdian3"]);
                        txtDpAmt.Text = Convert.ToString(dt.Rows[0]["DepositAmt"]);

                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbEmp.ActiveTabIndex = 1;
                        StatusButton("Show");
                        EnableControl(false);
                    }
                }
            }
            finally
            {
                dt = null;
                oMem = null;
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
            string vStateEdit = "",  vBrCode ="", vMemId ="", vOwn = "", vFinYr="", vAcMst="", vAcDtl="";
            vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            vBrCode = Session[gblValue.BrnchCode].ToString();
            vMemId = Convert.ToString(ViewState["MemId"]);
            Int32 vErr = 0, vAgntId=0;
            DateTime vDOB = gblFuction.setDate(txtDoB.Text);
            DateTime vSubDt = gblFuction.setDate(txtSubDt.Text);
            CNpsMember oMem = null;
            CGblIdGenerator oGbl = null;
            
            vAgntId = Convert.ToInt32(ddlAgnt.SelectedValue ); 
            vOwn = ddlOwn.SelectedValue.ToString();
            vFinYr=Session[gblValue.ShortYear].ToString();
            vAcMst=Session[gblValue.ACVouMst].ToString();
            vAcDtl=Session[gblValue.ACVouDtl].ToString();

                try
                {
                    if (ValidateField() == false)
                        return false;

                    if (this.RoleId != 1)
                    {
                        if (Session[gblValue.EndDate] != null)
                        {
                            if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtSubDt.Text))
                            {
                                gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                                return false;
                            }
                        }
                    }
                    if (Mode == "Save")
                    {
                        oMem = new CNpsMember();
                        //oGbl = new CGblIdGenerator();
                        //vRec = oGbl.ChkDuplicate("NPS_MemberMst", "MemNo", txtMemNo.Text.Replace("'", "''"), "", "", "MemID", vMemId.ToString(), "Save");
                        //if (vRec > 0)
                        //{
                        //    gblFuction.MsgPopup("Member No can not be Duplicate...");
                        //    return false;
                        //}
                        vErr = oMem.NPS_SaveMemMst(ref vMemId, txtMemNo.Text.Replace("'", "''"), vOwn, vSubDt, txtMemName.Text.Replace("'", "''"), ddlGender.SelectedValue,
                                    vDOB, txtFather.Text.Replace("'", "''"), txtAddress.Text.Replace("'", "''"), txtVill.Text.Replace("'", "''"),
                                    txtDist.Text.Replace("'", "''"), ddlState.SelectedValue, txtPin.Text.Replace("'", "''"),
                                    txtPhNo.Text.Replace("'", "''"), txtNom1.Text.Replace("'", "''"), ddlRel1.SelectedValue,
                                    Convert.ToInt32(txtAge1.Text), Convert.ToInt32(txtPer1.Text), txtGur1.Text.Replace("'", "''"),
                                    txtNom2.Text.Replace("'", "''"), ddlRel2.SelectedValue, Convert.ToInt32(txtAge2.Text),
                                    Convert.ToInt32(txtPer2.Text), txtGur2.Text.Replace("'", "''"), txtNom3.Text.Replace("'", "''"),
                                    ddlRel3.SelectedValue, Convert.ToInt32(txtAge3.Text), Convert.ToInt32(txtPer3.Text),
                                    txtGur3.Text.Replace("'", "''"), Convert.ToDouble(txtDpAmt.Text), vAgntId, ddlRO.SelectedValue, ddlCenter.SelectedValue
                                    , vBrCode, this.UserID, "Save", vFinYr, vAcMst, vAcDtl);
                        if (vErr > 0)
                        {
                            ViewState["MemId"] = vMemId;
                            txtMemId.Text = vMemId;
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
                        oMem = new CNpsMember();
                        oGbl = new CGblIdGenerator();
                        //vRec = oGbl.ChkDuplicate("EOMst", "EMPCode", txtEmpCode.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Edit");
                        //if (vRec > 0)
                        //{
                        //    gblFuction.MsgPopup("EMP Code Can not be Duplicate...");
                        //    return false;
                        //}
                        vErr = oMem.NPS_SaveMemMst(ref vMemId, txtMemNo.Text.Replace("'", "''"), vOwn, vSubDt, txtMemName.Text.Replace("'", "''"), ddlGender.SelectedValue,
                                    vDOB, txtFather.Text.Replace("'", "''"), txtAddress.Text.Replace("'", "''"), txtVill.Text.Replace("'", "''"),
                                    txtDist.Text.Replace("'", "''"), ddlState.SelectedValue, txtPin.Text.Replace("'", "''"),
                                    txtPhNo.Text.Replace("'", "''"), txtNom1.Text.Replace("'", "''"), ddlRel1.SelectedValue,
                                    Convert.ToInt32(txtAge1.Text), Convert.ToInt32(txtPer1.Text), txtGur1.Text.Replace("'", "''"),
                                    txtNom2.Text.Replace("'", "''"), ddlRel2.SelectedValue, Convert.ToInt32(txtAge2.Text),
                                    Convert.ToInt32(txtPer2.Text), txtGur2.Text.Replace("'", "''"), txtNom3.Text.Replace("'", "''"),
                                    ddlRel3.SelectedValue, Convert.ToInt32(txtAge3.Text), Convert.ToInt32(txtPer3.Text),
                                    txtGur3.Text.Replace("'", "''"), Convert.ToDouble(txtDpAmt.Text), vAgntId, ddlRO.SelectedValue, ddlCenter.SelectedValue, vBrCode, this.UserID, "Edit", vFinYr, vAcMst, vAcDtl);
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
                        oMem = new CNpsMember();
                        oGbl = new CGblIdGenerator();
                        //vRec = oGbl.ChkDeleteString(vEoId, "EOID", "MarketMSt");
                        //if (vRec > 0)
                        //{
                        //    gblFuction.MsgPopup("The RO has group, you can not delete the RO.");
                        //    return false;
                        //}
                        oMem = new CNpsMember();
                        vErr = oMem.NPS_SaveMemMst(ref vMemId, txtMemNo.Text.Replace("'", "''"), vOwn, vSubDt, txtMemName.Text.Replace("'", "''"), ddlGender.SelectedValue,
                                    vDOB, txtFather.Text.Replace("'", "''"), txtAddress.Text.Replace("'", "''"), txtVill.Text.Replace("'", "''"),
                                    txtDist.Text.Replace("'", "''"), ddlState.SelectedValue, txtPin.Text.Replace("'", "''"),
                                    txtPhNo.Text.Replace("'", "''"), txtNom1.Text.Replace("'", "''"), ddlRel1.SelectedValue,
                                    Convert.ToInt32(txtAge1.Text), Convert.ToInt32(txtPer1.Text), txtGur1.Text.Replace("'", "''"),
                                    txtNom2.Text.Replace("'", "''"), ddlRel2.SelectedValue, Convert.ToInt32(txtAge2.Text),
                                    Convert.ToInt32(txtPer2.Text), txtGur2.Text.Replace("'", "''"), txtNom3.Text.Replace("'", "''"),
                                    ddlRel3.SelectedValue, Convert.ToInt32(txtAge3.Text), Convert.ToInt32(txtPer3.Text),
                                    txtGur3.Text.Replace("'", "''"), Convert.ToDouble(txtDpAmt.Text), vAgntId, ddlRO.SelectedValue, ddlCenter.SelectedValue, vBrCode, this.UserID, "Delet", vFinYr, vAcMst, vAcDtl);
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
                    oMem = null;
                    oGbl = null;
                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlOwn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOwn.SelectedValue == "Y")
            {
                txtMemNo.Text = "";
                txtMemNo.Enabled = true;
            }   
            else
            {
                txtMemNo.Text = "";
                txtMemNo.Enabled = false;
            }   
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }
    }
}