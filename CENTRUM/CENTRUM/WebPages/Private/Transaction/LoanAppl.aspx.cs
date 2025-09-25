using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;
using System.Configuration;


namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class LoanAppl : CENTRUMBase
    {
        protected int cPgNo = 1;
        public static int cvar = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                InitBasePage();
                ViewState["StateEdit"] = null;
                ViewState["CGTID"] = null;
                txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtAppDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch(Session[gblValue.UserName].ToString());
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                    ddlBranch.Enabled = false;
                }
                else
                {
                    ddlBranch.Enabled = true;
                }
                popRO();
                LoadGrid(1);
                PopPurpose();
                //PopLoanType();
                StatusButton("View");
                tabLoanAppl.ActiveTabIndex = 0;
                LoadKYCDoc();
                popRelation();
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
                this.PageHeading = "Loan Application";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanApplication);
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
                    txtAppDt.Enabled = true;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    txtAppDt.Enabled = false;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    txtAppDt.Enabled = false;
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
        private void popRelation()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "HumanRelationId", "HumanRelationName", "HumanRelationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlNomRel.DataSource = dt;
                ddlNomRel.DataTextField = "HumanRelationName";
                ddlNomRel.DataValueField = "HumanRelationId";
                ddlNomRel.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlNomRel.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        private void EnableControl(Boolean Status)
        {
            ddlRO.Enabled = Status;
            txtAppDt.Enabled = Status;
            if (Session[gblValue.BrnchCode].ToString() == "0000")
                ddlBranch.Enabled = Status;
            ddlGroup.Enabled = Status;
            ddlCenter.Enabled = Status;
            ddlMemNo.Enabled = Status;
            ddlLnSchem.Enabled = Status;
            ddlLnPurps.Enabled = Status;
            ddlSubPur.Enabled = Status;
            txtProcFee.Enabled = Status;
            //txtLnAmt.Enabled = Status;
            txtSrvTax.Enabled = Status;
            txtDisbDt.Enabled = Status;
            chkIsTopUp.Enabled = Status;
            gvKycDoc.Enabled = Status;
            gvGuar.Enabled = Status;
            txtspnm.Enabled = Status;
            txtNomAge.Enabled = Status;
            txtNomAdd.Enabled = Status;
            ddlNomRel.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            ddlRO.SelectedIndex = -1;
            txtAppNo.Text = "";
            txtAppDt.Text = Session[gblValue.LoginDate].ToString();
            ddlCenter.SelectedIndex = -1;
            ddlCenter.Items.Clear();
            ddlGroup.SelectedIndex = -1;
            ddlGroup.Items.Clear();
            ddlMemNo.SelectedIndex = -1;
            ddlMemNo.Items.Clear();
            ddlLnSchem.SelectedIndex = -1;
            ddlLnPurps.SelectedIndex = -1;
            ddlSubPur.SelectedIndex = -1;
            txtProcFee.Text = "0";
            txtLnAmt.Text = "0";
            txtSrvTax.Text = "0";
            DateTime vDisbDt = gblFuction.setDate(txtAppDt.Text).AddDays(7);
            txtDisbDt.Text = gblFuction.putStrDate(vDisbDt);
            chkIsTopUp.Checked = false;
            LblDate.Text = "";
            LblUser.Text = "";
            PopLoanType();
            txtspnm.Text = "";
            txtNomAge.Text = "0";
            txtNomAdd.Text = "";
            ddlNomRel.SelectedIndex = -1;
            gvKycDoc.DataSource = null;
            gvKycDoc.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CApplication oCG = null;
            Int32 vRows = 0;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                cvar = 0;
                vMode = rdbSel.SelectedValue;
                vBrCode = (string)Session[gblValue.BrnchCode];
                DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                oCG = new CApplication();
                dt = oCG.GetApplicationList(vMode, vBrCode, vFrmDt, vToDt, txtSearch.Text.Trim(), pPgIndx, ref vRows);
                gvLoanAppl.DataSource = dt.DefaultView;
                gvLoanAppl.DataBind();
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
                oCG = null;
                cvar = 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //private void popRO()
        //{
        //    //DataTable dt = null;
        //    //CGblIdGenerator oGb = null;
        //    //string vBrCode = "";
        //    //Int32 vBrId = 0;
        //    //DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //    //try
        //    //{
        //    //    vBrCode = (string)Session[gblValue.BrnchCode];
        //    //    vBrId = Convert.ToInt32(vBrCode);
        //    //    oGb = new CGblIdGenerator();
        //    //    dt = oGb.PopComboMIS("D", "N", "AA", "EoId", "EoName", "EoMst", vBrCode, "BranchCode", "Tra_DropDate", vLogDt, vBrCode);
        //    //    ddlRO.DataSource = dt;
        //    //    ddlRO.DataTextField = "EoName";
        //    //    ddlRO.DataValueField = "EoId";
        //    //    ddlRO.DataBind();
        //    //    ListItem oli = new ListItem("<--Select-->", "-1");
        //    //    ddlRO.Items.Insert(0, oli);
        //    //}
        //    //finally
        //    //{
        //    //    oGb = null;
        //    //    dt = null;
        //    //}
        //    DataTable dt = null;
        //    CEO oRO = null;
        //    string vBrCode = Session[gblValue.BrnchCode].ToString();
        //    DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //    try
        //    {
        //        oRO = new CEO();
        //        dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
        //        ddlRO.DataSource = dt;
        //        ddlRO.DataTextField = "EoName";
        //        ddlRO.DataValueField = "Eoid";
        //        ddlRO.DataBind();
        //        ListItem oli = new ListItem("<--Select-->", "-1");
        //        ddlRO.Items.Insert(0, oli);
        //    }
        //    finally
        //    {
        //        oRO = null;
        //        dt = null;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vEOID"></param>
        private void PopCenter(string vEOID)
        {
            ddlMemNo.Items.Clear();
            ddlGroup.Items.Clear();
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
        /// <param name="vCenterID"></param>
        private void PopGroup(string vCenterID)
        {
            ddlGroup.Items.Clear();
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
                dt = oGb.PopComboMIS("D", "N", "AA", "GroupID", "GroupName", "GroupMst", vCenterID, "MarketID", "Tra_DropDate", vLogDt, vBrCode);
                ddlGroup.DataSource = dt;
                ddlGroup.DataTextField = "GroupName";
                ddlGroup.DataValueField = "GroupID";
                ddlGroup.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlGroup.Items.Insert(0, oli);
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
        /// <param name="vGroupID"></param>
        private void PopMember(string vGroupID, string vMemId)
        {
            DataTable dt = null;
            string vBrCode = "";

            try
            {
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    vBrCode = ddlBranch.SelectedValue;
                }
                else
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }

                if (chkIsTopUp.Checked == true)
                {
                    CMember oMem = new CMember();
                    dt = oMem.GetMemListByGroupId(vGroupID, vBrCode);
                    ddlMemNo.DataSource = dt;
                    ddlMemNo.DataTextField = "MemberCode";
                    ddlMemNo.DataValueField = "MemberId";
                    ddlMemNo.DataBind();
                }
                else
                {
                    CApplication oApp = new CApplication();
                    dt = oApp.GetCGTMember(vGroupID, vMemId, vBrCode);
                    ddlMemNo.DataTextField = "Member";
                    ddlMemNo.DataValueField = "MemberId";
                    ddlMemNo.DataSource = dt;
                    ddlMemNo.DataBind();
                }
                ListItem oItm = new ListItem();
                oItm.Text = "<--- Select --->";
                oItm.Value = "-1";
                ddlMemNo.Items.Insert(0, oItm);
                ddlMemNo.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
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

                if (chkIsTopUp.Checked == true)
                {
                    if (Session[gblValue.BrnchCode].ToString() != "0000")
                        dt = oApp.GetLoanTypeForApp("Y", ddlBranch.SelectedValue);
                    else
                        dt = oApp.GetLoanTypeForAppHO("Y", ddlBranch.SelectedValue);
                    ddlLnSchem.DataSource = dt;
                    ddlLnSchem.DataTextField = "LoanType";
                    ddlLnSchem.DataValueField = "LoanTypeId";
                    ddlLnSchem.DataBind();

                }
                else
                {
                    if (Session[gblValue.BrnchCode].ToString() != "0000")
                        dt = oApp.GetLoanTypeForApp("N", ddlBranch.SelectedValue);
                    else
                        dt = oApp.GetLoanTypeForAppHO("N", ddlBranch.SelectedValue);
                    ddlLnSchem.DataSource = dt;
                    ddlLnSchem.DataTextField = "LoanType";
                    ddlLnSchem.DataValueField = "LoanTypeId";
                    ddlLnSchem.DataBind();
                }
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlLnSchem.Items.Insert(0, oLi);


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
            CSubPurpose oGb = null;
            try
            {
                if (ddlLnSchem.SelectedValue == "9" || ddlLnSchem.SelectedValue == "10" || ddlLnSchem.SelectedValue == "23" || ddlLnSchem.SelectedValue == "24")
                {
                    oGb = new CSubPurpose();
                    dt = oGb.PopPurposeEdu(gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate])));
                    ddlLnPurps.DataSource = dt;
                    ddlLnPurps.DataTextField = "Purpose";
                    ddlLnPurps.DataValueField = "PurposeID";
                    ddlLnPurps.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlLnPurps.Items.Insert(0, oli);
                }
                else
                {
                    oGb = new CSubPurpose();
                    dt = oGb.PopPurpose(gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate])));
                    ddlLnPurps.DataSource = dt;
                    ddlLnPurps.DataTextField = "Purpose";
                    ddlLnPurps.DataValueField = "PurposeID";
                    ddlLnPurps.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlLnPurps.Items.Insert(0, oli);
                }
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void PopSubPurpose(Int32 vPurposeID)
        {
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CSubPurpose oGb = null;
            string vBrCode = "";
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oGb = new CSubPurpose();
                dt = oGb.PopSubPurpose(gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate])), vPurposeID);

                ddlSubPur.DataSource = dt;
                ddlSubPur.DataTextField = "SubPurpose";
                ddlSubPur.DataValueField = "SubPurposeID";
                ddlSubPur.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlSubPur.Items.Insert(0, oli);
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
        protected void ddlRO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRO.SelectedIndex > 0) PopCenter(ddlRO.SelectedValue);
            ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(ddlRO.SelectedValue));
            if (ddlCenter.SelectedIndex > 0) PopGroup(ddlCenter.SelectedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCenter.SelectedIndex > 0) PopGroup(ddlCenter.SelectedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMemNo.SelectedIndex = -1;
            PopMember(ddlGroup.SelectedValue, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMemNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblMemNm.Text = ddlMemNo.SelectedItem.Text;
            DataTable dt = null, dt1 = null;
            CApplication oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            DateTime vLogDt = gblFuction.setDate("");
            DateTime vAppDt = gblFuction.setDate(txtAppDt.Text);
            string vMemNo = "";
            try
            {
                if (ddlMemNo.SelectedIndex > 0 && ddlGroup.SelectedIndex > 0)
                {
                    vMemNo = ddlMemNo.SelectedValue;
                    oCG = new CApplication();
                    dt = oCG.GetCGTMember(ddlGroup.SelectedValue, ddlMemNo.SelectedValue, vBrCode);
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["CGTID"] = dt.Rows[0]["CGTID"].ToString();
                        //if (gblFuction.setDate(txtAppDt.Text).Year - gblFuction.setDate(dt.Rows[0]["M_DOB"].ToString()).Year > 57)
                        //{
                        //    gblFuction.AjxMsgPopup("Member Age Exceed 57 Years..Can not Apply for Loan");
                        //    ddlMemNo.SelectedIndex = 0;
                        //    return;
                        //}
                    }
                }

                fillLnDtl(vMemNo, vAppDt);

            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }


        public void fillLnDtl(string Vmember, DateTime vAppDt)
        {
            DataSet ds = null;
            DataTable dt = null, dt1 = null;
            CApplication oCG = null;

            try
            {
                oCG = new CApplication();
                ds = oCG.GetLoanDtlByMember(Vmember, vAppDt);
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];

                txtspnm.Text = dt.Rows[0]["GuarName"].ToString();
                txtNomAge.Text = dt.Rows[0]["GuarAge"].ToString();
                txtNomAdd.Text = dt.Rows[0]["CoBrwrAddr"].ToString();
                ddlNomRel.SelectedIndex = ddlNomRel.Items.IndexOf(ddlNomRel.Items.FindByValue(dt.Rows[0]["Relation"].ToString()));
                gvlndtl.DataSource = dt1;
                gvlndtl.DataBind();
                
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
        protected void ddlLnSchem_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CApplication oCG = null;
            Int32 vLoanTypeID = 0;
            Int32 vApLoanCycle = 0;
            if (ddlMemNo.SelectedIndex <= 0)
            {
                gblFuction.AjxMsgPopup("Select Member");
                ddlLnSchem.SelectedIndex = -1;
            }

            try
            {
                PopPurpose();
                vLoanTypeID = Convert.ToInt32(ddlLnSchem.SelectedValue);
                oCG = new CApplication();
                dt = oCG.GetLoanAmtAndCycleByLTypeID(vLoanTypeID, ddlMemNo.SelectedValue);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["PrvLoanYN"].ToString() == "N")
                    {
                        gblFuction.AjxMsgPopup("The Member can not avail this Loan Scheme ");
                        ddlLnSchem.SelectedIndex = -1;
                        return;
                    }
                    else
                    {
                        txtLnAmt.Text = dt.Rows[0]["LoanAmt"].ToString();
                        txtLnCycle.Text = dt.Rows[0]["LoanCycle"].ToString();
                        vApLoanCycle = Convert.ToInt32(dt.Rows[0]["ApLoanCycle"].ToString());
                        if (Convert.ToInt32(txtLnCycle.Text) < vApLoanCycle)
                        {
                            gblFuction.AjxMsgPopup("Applied Amount is applicable for" + vApLoanCycle.ToString() + "and Above");
                            ddlLnSchem.SelectedIndex = -1;
                        }
                    }
                }
                else
                {
                    txtLnAmt.Text = "";
                    txtLnCycle.Text = "";
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
        protected void gvLoanAppl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vAppId = "", vMode="";
            DataTable dt = null, dtkycdoc;
            DataSet ds = null;
            int row = 0;
            CApplication oCG = null;
            try
            {
                vAppId = Convert.ToString(e.CommandArgument);
                ViewState["AppId"] = vAppId;
                
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    row = gr.RowIndex;
                    oCG = new CApplication();
                    ds = oCG.GetApplicationDtl(vAppId);
                    dt = ds.Tables[0];
                    dtkycdoc = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["IsTopup"].ToString().Trim() == "Y")
                            chkIsTopUp.Checked = true;
                        else
                            chkIsTopUp.Checked = false;
                        txtAppNo.Text = dt.Rows[0]["LoanAppNo"].ToString();
                        txtAppDt.Text = dt.Rows[0]["AppDate"].ToString();
                        ddlRO.SelectedIndex = ddlRO.Items.IndexOf(ddlRO.Items.FindByValue(dt.Rows[0]["EOID"].ToString()));
                        PopCenter(dt.Rows[0]["EOID"].ToString());
                        ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(dt.Rows[0]["MarketID"].ToString()));
                        PopGroup(dt.Rows[0]["MarketID"].ToString());
                        ddlGroup.SelectedIndex = ddlGroup.Items.IndexOf(ddlGroup.Items.FindByValue(dt.Rows[0]["GroupID"].ToString()));
                        PopMember(dt.Rows[0]["GroupID"].ToString(), dt.Rows[0]["MemberID"].ToString());
                        ddlMemNo.SelectedIndex = ddlMemNo.Items.IndexOf(ddlMemNo.Items.FindByValue(dt.Rows[0]["MemberID"].ToString()));
                        txtspnm.Text = dt.Rows[0]["NomName"].ToString();
                        txtNomAge.Text = dt.Rows[0]["NomAge"].ToString();
                        txtNomAdd.Text = dt.Rows[0]["NomAddress"].ToString();
                        ddlNomRel.SelectedIndex = ddlNomRel.Items.IndexOf(ddlNomRel.Items.FindByValue(dt.Rows[0]["NomRel"].ToString()));
                        hdnLoanAppId.Value = dt.Rows[0]["MemberID"].ToString();
                        PopLoanType();
                        ddlLnSchem.SelectedIndex = ddlLnSchem.Items.IndexOf(ddlLnSchem.Items.FindByValue(dt.Rows[0]["LoanTypeId"].ToString()));
                        ddlLnPurps.SelectedIndex = ddlLnPurps.Items.IndexOf(ddlLnPurps.Items.FindByValue(dt.Rows[0]["PurposeID"].ToString()));
                        PopSubPurpose(Convert.ToInt32(ddlLnPurps.SelectedValue));
                        ddlSubPur.SelectedIndex = ddlSubPur.Items.IndexOf(ddlSubPur.Items.FindByValue(dt.Rows[0]["SubPurposeID"].ToString()));
                        txtLnAmt.Text = dt.Rows[0]["LoanAppAmt"].ToString();
                        ViewState["LoanAppAmt"] = txtLnAmt.Text;
                        txtLnCycle.Text = dt.Rows[0]["LoanCycle"].ToString();
                        txtDisbDt.Text = dt.Rows[0]["ExpDate"].ToString();
                        txtProcFee.Text = dt.Rows[0]["LnApplnPFees"].ToString();
                        txtSrvTax.Text = dt.Rows[0]["LnApplnSTax"].ToString();
                        LblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        LblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tabLoanAppl.ActiveTabIndex = 1;
                        DateTime vAppDt = gblFuction.setDate(txtAppDt.Text);
                        if (dtkycdoc.Rows.Count > 0)
                        {
                            dtkycdoc.TableName = "Table2";
                            gvGuar.DataSource = dtkycdoc;
                            gvGuar.DataBind();
                          
                           
                            ViewState["GuarInfo"] = dtkycdoc;
                            SetData();
                        }
                        else
                        {
                            LoadGuar();
                        }
                        //fillLnDtl(dt.Rows[0]["MemberID"].ToString(), vAppDt);                       
                        StatusButton("Show");

                    }



                }
                if (e.CommandName == "cmdRpt")
                {
                    GridViewRow gr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    row = gr.RowIndex;
                    vAppId = Convert.ToString(e.CommandArgument);
                    
                    GetData(vAppId, "PDF");

                }
                if (e.CommandName == "cmdApplRpt")
                {
                    
                    GridViewRow gr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    row = gr.RowIndex;
                    DropDownList ddlreport = (DropDownList)gvLoanAppl.Rows[gr.RowIndex].FindControl("ddlreport");
                    vMode = ddlreport.SelectedValue;
                  
                    vAppId = Convert.ToString(e.CommandArgument);

                    SetParameterForRptData(vMode,vAppId, "PDF");

                }
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }
        public void GetData(string pLoanAppId, string pMode)
        {
            string vBrCode = "", vTitle = "", vRptPath = "";

            DataTable dt = null;
            CReports oRpt = null;
            string vBranch = Session[gblValue.BrName].ToString();
            try
            {
                vTitle = "Loan Approval Report";
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    oRpt = new CReports();
                    dt = oRpt.RptLoanApplication(pLoanAppId);

                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptLoanApplication.rpt";
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                    rptDoc.SetParameterValue("pAddress2", "");
                    rptDoc.SetParameterValue("pBranch", vBranch);
                    rptDoc.SetParameterValue("pTitle", vTitle);
                    //rptDoc.SetParameterValue("dtFrom", "01/01/1900");
                    //rptDoc.SetParameterValue("dtTo", txtToDt.Text);

                    if (pMode == "PDF")
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Loan Application");

                    rptDoc.Dispose();
                    Response.ClearContent();
                    Response.ClearHeaders();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oRpt = null;
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
                tabLoanAppl.ActiveTabIndex = 1;
                StatusButton("Add");
                //ClearControls();
                txtAppDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                //LoadKYCDoc();
                LoadGuar();
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
                //gvKycDoc.Enabled = true;
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
            tabLoanAppl.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
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
            tabLoanAppl.ActiveTabIndex = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {



            Boolean vResult = false;
            //DataTable dt = null;
            string vStateEdit = string.Empty, vBrCode = string.Empty, vAppNo = string.Empty;
            Int32 vErr = 0, vPurpId = 0, vSubPurpId = 0, vLnTypeId = 0, vCycle = 0, vCGTID = 0, vYrNo = 0, vErr1 = 0;
            string vAppId = "", vMemId = "", vTopup = "", vMsg = "", vslno="";
            double vLnAmt = 0, vProcFees = 0, vSrvTax = 0, vPrevLoanAppAmt = 0;
            DateTime vAppDt = gblFuction.setDate(txtAppDt.Text);
            DateTime vExPectedDt = gblFuction.setDate(txtDisbDt.Text);
            CApplication oCG = null;
           
            DataTable dtKYC = null, dtGuar=null;
            string vXmlKYC = "", vXmlGuar="";
            try
            {
                vCGTID = Convert.ToInt32(ViewState["CGTID"]);
                vMemId = ddlMemNo.SelectedValue;
                vLnTypeId = Convert.ToInt32(ddlLnSchem.SelectedValue);
                vPurpId = Convert.ToInt32(ddlLnPurps.SelectedValue);
                vSubPurpId = Convert.ToInt32(ddlSubPur.SelectedValue);
                vYrNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
                vCycle = Convert.ToInt32(txtLnCycle.Text);

                
                //using (StringWriter oSW = new StringWriter())
                //{
                //    dtKYC.WriteXml(oSW);
                //    vXmlKYC = oSW.ToString();
                //}
                if (txtLnAmt.Text == "")
                {
                    gblFuction.MsgPopup("Application amount can not blank...");
                    return false;
                }
                vLnAmt = Convert.ToDouble(txtLnAmt.Text);

                if (txtProcFee.Text != "")
                    vProcFees = Convert.ToDouble(txtProcFee.Text);

                if (txtSrvTax.Text != "")
                    vSrvTax = Convert.ToDouble(txtSrvTax.Text);


                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    vBrCode = ddlBranch.SelectedValue;
                }
                else
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }
                vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                vAppId = Convert.ToString(ViewState["AppId"]);

                if (chkIsTopUp.Checked == true)
                    vTopup = "Y";
                else
                    vTopup = "N";

                oCG = new CApplication();

                //vErr = oCG.ChkTopUpLoan(vMemId, vLnTypeId);
                //if (vErr > 0)
                //{
                //    gblFuction.MsgPopup("Another top-up loan exists for this member. Can not save");
                //    return false;
                //}
                vMsg = oCG.ChkLoanOther(vMemId, vLnTypeId, "A");
                if (vMsg != "")
                {
                    gblFuction.MsgPopup(vMsg);
                    return false;
                }

                vMsg = oCG.ChkPOSMemberWise(vMemId, vLnTypeId, vAppDt);
                if (vMsg != "")
                {
                    gblFuction.MsgPopup(vMsg);
                    return false;
                }
                //vErr = oCG.chkUploadDocByLoanType(vLnTypeId, vBrCode);
                //if (vErr == 0)
                //{
                //    if (GetDoc() == false)
                //    {
                //        return false;
                //    }
                //}
                //dtKYC = (DataTable)ViewState["KYCDoc"];
                if (GetGuarInfo() == false)
                {
                    return false;
                }
                dtGuar = (DataTable)ViewState["GuarInfo"];
                using (StringWriter oSw = new StringWriter())
                {
                    dtGuar.WriteXml(oSw);
                    vXmlGuar=oSw.ToString();
                }
                vMsg = oCG.ChkNEFT(vMemId, vLnTypeId, vBrCode);
                if (vMsg != "")
                {
                    gblFuction.MsgPopup(vMsg);
                    return false;
                }
                
                
                if (Mode == "Save")
                {
                    if (ValidateFields() == false) return false;
                    if (this.RoleId != 1)                           //    && this.RoleId != 4     1 for Admin                 4 for BM discussed with Prodip as on 2nd Sep/2014
                    {
                        if (Session[gblValue.EndDate] != null)
                        {
                            if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtAppDt.Text))
                            {
                                gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                                return false;
                            }
                        }
                    }

                    vErr = oCG.ChkSaveApplication(vMemId, vBrCode);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("An Application pending for disbursement.");
                        return false;
                    }
                    //vErr = oCG.ChkAppMember(vMemId, vAppDt);
                    //if (vErr > 0)
                    //{
                    //    gblFuction.MsgPopup("Invalid Age for Member or Spouse. or Borrower..Can not save");
                    //    return false;
                    //}
                    if (vTopup == "N")
                    {
                        //vErr = oCG.ChkMemberOpenLoan(vMemId, vAppDt);
                        //if (vErr > 0)
                        //{
                        //    gblFuction.MsgPopup("Loan Exists. Can not save");
                        //    return false;
                        //}
                        vErr = oCG.ChkLoanApp(vMemId, vAppDt);
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup("Expected closing date of the previous loan is more than 100 days. Can not save");
                            return false;
                        }
                    }
                    vErr1 = oCG.chkLoanAppNo(vBrCode, vAppDt);
                    if (vErr1 > 0)
                    {
                        gblFuction.MsgPopup("Invalid no of Loan Application");
                        return false;
                    }
                    this.GetModuleByRole(mnuID.mnuLoanApplication);
                    vErr = oCG.InsertApplication(ref vAppNo, vAppDt, vMemId, vLnTypeId, vLnAmt, vPurpId, vSubPurpId,
                                vExPectedDt, vCycle, vProcFees, vSrvTax, vCGTID, vBrCode, Convert.ToInt32(Session[gblValue.UserId].ToString()), "I", vYrNo, vTopup,
                                vXmlGuar, txtspnm.Text, txtNomAdd.Text, Convert.ToInt32(txtNomAge.Text == "" ? "0" : txtNomAge.Text), Convert.ToInt32(ddlNomRel.SelectedValue),
                                 ddlRO.SelectedValue.ToString(), 0, "", 0, 0, 0,
                                0, "");
                    if (vErr == 0)
                    {
                        hdnLoanAppId.Value = vAppId;
                        ViewState["AppId"] = vAppId;
                        txtAppNo.Text = vAppNo;
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
                    if (ValidateFields() == false) return false;

                    oCG = new CApplication();
                    vErr = oCG.ChkEditApplication(vAppId, vMemId, vBrCode);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Approved or Cancelled Application cannot be Edited.");
                        return false;
                    }

                    vPrevLoanAppAmt = Convert.ToDouble(ViewState["LoanAppAmt"]);
                    if (Convert.ToInt32(Session[gblValue.UserId].ToString()) != 1)
                    {
                        if (vPrevLoanAppAmt < vLnAmt)
                        {
                            gblFuction.MsgPopup("Loan application amount should not be more than previous amount.");
                            return false;
                        }
                    }
                    //vErr = oCG.chkVerified(vslno, vDocId);
                    //if (vErr > 0)
                    //{
                    //    gblFuction.MsgPopup("Already Verified or Cancelled.");
                    //    return false;
                    //}
                    this.GetModuleByRole(mnuID.mnuLoanApplication);
                    vErr = oCG.UpdateApplication(vAppId, vAppDt, vMemId, vLnTypeId, vLnAmt, vPurpId, vSubPurpId,
                                vExPectedDt, vCycle, vProcFees, vSrvTax, vBrCode, vTopup, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Edit",

                                vXmlGuar, txtspnm.Text, txtNomAdd.Text, Convert.ToInt32(txtNomAge.Text == "" ? "0" : txtNomAge.Text), Convert.ToInt32(ddlNomRel.SelectedValue),0,"");//, "A");
                    if (vErr == 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oCG = new CApplication();
                    vErr = oCG.ChkEditApplication(vAppId, vMemId, vBrCode);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Approved or Cancelled Application cannot be Deleted.");
                        return false;
                    }
                    else
                    {
                        this.GetModuleByRole(mnuID.mnuLoanApplication);
                        vErr = oCG.UpdateApplication(vAppId, vAppDt, vMemId, vLnTypeId, vLnAmt, vPurpId, vSubPurpId,
                                vExPectedDt, vCycle, vProcFees, vSrvTax, vBrCode, vTopup, this.UserID, "Delet",

                                vXmlGuar, txtspnm.Text, txtNomAdd.Text, Convert.ToInt32(txtNomAge.Text == "" ? "0" : txtNomAge.Text), Convert.ToInt32(ddlNomRel.SelectedValue),0,"");//, "A");
                        if (vErr == 0)
                            vResult = true;
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.DBError);
                            vResult = false;
                        }
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
                oCG = null;
                //dt = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DateTime vAppDate = gblFuction.setDate(txtAppDt.Text);
            DateTime vExpDisbDt = gblFuction.setDate(txtDisbDt.Text);
            string vUserId=Session[gblValue.UserId].ToString();           
            Int32 vUsrId = Convert.ToInt32(vUserId);
            string vLnCycle = txtLnCycle.Text;
            Int32 vLoanCycle = Convert.ToInt32(vLnCycle);
            CApplication oCG = oCG = new CApplication();
            CDisburse oLD = null;
            string vMsg = "";


            if (txtAppDt.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Loan Application Date cannot be empty.");
                gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtAppDt");
                vResult = false;
            }
            else
            {
                //DateTime vAppDate = gblFuction.setDate(txtAppDt.Text);
                if (vAppDate < vFinFrom || vAppDate > vFinTo)
                {
                    EnableControl(true);
                    gblFuction.MsgPopup("Loan Application Date should be Financial Year.");
                    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtAppDt");
                    vResult = false;
                }
                if (vAppDate > vLoginDt)
                {
                    EnableControl(true);
                    gblFuction.MsgPopup("Loan Application Date should not be greater than login date.");
                    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtAppDt");
                    vResult = false;
                }
            }
            if (txtDisbDt.Text != "")
            {
                if (vExpDisbDt <= vAppDate)
                {
                    EnableControl(true);
                    gblFuction.MsgPopup("Expected Disbursement Date should not be Lesser than Loan Application Date.");
                    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtDisbDt");
                    vResult = false;
                }
            }

            //if (Convert.ToInt32(txtNomAge.Text) > 100)
            //{
            //    gblFuction.MsgPopup("Nominee Age can not be grater than 100...");
            //    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtNomAge");
            //    vResult = false;
            //}

            if (ddlMemNo.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.MsgPopup("Please select the member...");
                gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_ddlMemNo");
                vResult = false;
            }
            if (ddlLnSchem.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.MsgPopup("Please select the Loan Scheme...");
                gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_ddlLnSchem");
                vResult = false;
            }
            if (ddlLnPurps.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.MsgPopup("Please select the Loan Purpose...");
                gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_ddlLnPurps");
                vResult = false;
            }
            if (ddlSubPur.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.MsgPopup("Please select the Loan Sub Purpose...");
                gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_ddlSubPur");
                vResult = false;
            }
            if (txtLnAmt.Text.Trim() == "" || txtLnAmt.Text.Trim() == "0")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Loan Amount should be grater than Zero.");
                gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtLnAmt");
                vResult = false;
            }
            //if (txtProcFee.Text.Trim() == "")
            //{
            //    EnableControl(true);
            //    gblFuction.MsgPopup("Processing fees should not be blank.");
            //    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtProcFee");
            //    vResult = false;
            //}
            //if (txtSrvTax.Text.Trim() == "")
            //{
            //    EnableControl(true);
            //    gblFuction.MsgPopup("Services should not be blank");
            //    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtSrvTax");
            //    vResult = false;
            //}
            //if (txtspnm.Text.Trim() == "")
            //{
            //    EnableControl(true);
            //    gblFuction.MsgPopup("Nominee Name should not be blank.");
            //    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtspnm");
            //    vResult = false;
            //}
            if (txtNomAge.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Nominee Age should not be blank.");
                gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtNomAge");
                vResult = false;
            }
            if (oCG.ChkMemBankInf(ddlMemNo.SelectedValue, Convert.ToInt32(ddlLnSchem.SelectedValue)) == 1)
            {
                gblFuction.MsgPopup("Member Bank Information required for this scheme");
                vResult = false;
            }
            oLD = new CDisburse();
            vMsg = oLD.chkMembersInfo("", ddlMemNo.SelectedValue, "LA", vUsrId, vLoanCycle);
            if (vMsg != "")
            {
                gblFuction.MsgPopup(vMsg);
                vResult = false;
            }
            oLD = null;
            oCG = null;
            return vResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtAppDt_TextChanged(object sender, EventArgs e)
        {
            //DateTime vDisbDt = gblFuction.setDate(txtAppDt.Text).AddDays(10);
            //txtDisbDt.Text = gblFuction.putStrDate(vDisbDt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlLnPurps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLnPurps.SelectedIndex > 0) PopSubPurpose(Convert.ToInt32(ddlLnPurps.SelectedValue));
        }
        protected void chkIsTopUp_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CApplication oApp = null;
            try
            {
                oApp = new CApplication();
                //if (chkIsTopUp.Checked == true)
                //{
                //    if (Session[gblValue.BrnchCode].ToString() == "0000")
                //        dt = oApp.GetLoanTypeForApp("Y", "0000");
                //    else
                //        dt = oApp.GetLoanTypeForApp("Y", ddlBranch.SelectedValue);
                //    ddlLnSchem.DataSource = dt;
                //    ddlLnSchem.DataTextField = "LoanType";
                //    ddlLnSchem.DataValueField = "LoanTypeId";
                //    ddlLnSchem.DataBind();

                //}
                //else
                //{
                //    if (Session[gblValue.BrnchCode].ToString() == "0000")
                //        dt = oApp.GetLoanTypeForApp("N", "0000");
                //    else
                //        dt = oApp.GetLoanTypeForApp("N", ddlBranch.SelectedValue);
                //    ddlLnSchem.DataSource = dt;
                //    ddlLnSchem.DataTextField = "LoanType";
                //    ddlLnSchem.DataValueField = "LoanTypeId";
                //    ddlLnSchem.DataBind();
                //}
                //ListItem oLi = new ListItem("<--Select-->", "-1");
                //ddlLnSchem.Items.Insert(0, oLi);
                PopLoanType();

                ddlCenter.SelectedIndex = -1;
                ddlGroup.SelectedIndex = -1;
                ddlMemNo.SelectedIndex = -1;
                ddlMemNo.Items.Clear();
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
                }
                else
                {
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode;
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
            }
            else
            {
                vBrCode = ddlBranch.SelectedValue.ToString();
            }

            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlRO.DataSource = dt;
                ddlRO.DataTextField = "EoName";
                ddlRO.DataValueField = "EoId";
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBranch.SelectedIndex > 0)
            {
                popRO();
                PopLoanType();
            }

        }
        private bool SetUploads(FileUpload vFu, ref byte[] vFile, ref string vType, byte[] vImage, string vImgType, ref string vfilename)
        {
            if (vFu.HasFile == false)
            {
                if (vImage == null)
                {
                    //gblFuction.MsgPopup("Please Attach a File");
                    gblFuction.focus(vFu.UniqueID.Replace("$", "_"));
                    vFile = null;
                    vType = "";
                    return false;
                }
                else
                {
                    vFile = vImage;
                    vType = vImgType.ToLower();
                    vfilename = System.IO.Path.GetFileName(vFu.FileName).ToUpper();
                    string imageBase64Data = Convert.ToBase64String(vFile);
                    string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                }
            }
            if (vFu.PostedFile.InputStream.Length > 1048576)
            {
                gblFuction.MsgPopup("Maximum upload file Size Is 1(MB).");
                return false;
            }
            if (vFu.HasFile)
            {
                vFile = new byte[vFu.PostedFile.InputStream.Length + 1];
                vFu.PostedFile.InputStream.Read(vFile, 0, vFile.Length);
                vType = System.IO.Path.GetExtension(vFu.FileName).ToLower();
                vfilename = System.IO.Path.GetFileName(vFu.FileName).ToUpper();
                string imageBase64Data = Convert.ToBase64String(vFile);
                string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                //img.ImageUrl = imageDataURL;  
            }
            return true;
        }

        private void LoadKYCDoc()
        {
            ViewState["KYCDoc"] = null;
            DataTable dt = new DataTable("Table1");
            dt.Columns.Add("SLNo");
            dt.Columns.Add("Doc_Type");
            
            dt.Columns.Add("Doc_Image", typeof(byte[]));
            dt.Columns.Add("image_type");
            dt.Columns.Add("File_name");
            dt.Columns.Add("DocImageVerfied");
            dt.Columns.Add("DocTypeVal");
            dt.AcceptChanges();
            ViewState["KYCDoc"] = dt;
            NewKYCDoc(0);
        }
        private void LoadGuar()
        {
            ViewState["GuarInfo"] = null;
            DataTable dt = new DataTable("Table2");
            dt.Columns.Add("SLNo");
            dt.Columns.Add("Name");
            dt.Columns.Add("Village");
            dt.Columns.Add("PO");
            dt.Columns.Add("PS");
            dt.Columns.Add("District");
            dt.AcceptChanges();
            ViewState["GuarInfo"] = dt;
            NewGuar(0);
        }
        private void NewKYCDoc(Int32 vRow)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["KYCDoc"];
                DataRow dr;
                dr = dt.NewRow();
                dt.Rows.Add();
                if(dt.Rows.Count>0)
                //dt.Rows[gvKycDoc.Rows.Count]["SLNo"] = gvKycDoc.Rows.Count + 1;
                    dt.Rows[vRow]["SLNo"] = vRow + 1;
                else
                dt.Rows[vRow]["SLNo"] = 0;
                dt.Rows[vRow]["Doc_Type"] = -1;
                dt.Rows[vRow]["DocTypeval"] = -1;
                dt.Rows[vRow]["Doc_Image"] = null;
                dt.Rows[vRow]["image_type"] = null;
                dt.Rows[vRow]["File_name"] = null;
                dt.Rows[vRow]["DocImageVerfied"] = null;
                dt.AcceptChanges();
                ViewState["KYCDoc"] = dt;
                gvKycDoc.DataSource = dt;
                gvKycDoc.DataBind();
                SetData();
                //DropDownList ddlBrBorrow = (DropDownList)gvBorrow.Rows[vRow].FindControl("ddlBrBorrow");
                //popBrBorrow(ddlBrBorrow);
                //upColl.Update();

            }
            finally
            {
                //oTr = null;
                //dt = null;
            }
        }
        private void NewGuar(Int32 vRow)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["GuarInfo"];
                DataRow dr;
                dr = dt.NewRow();
                dt.Rows.Add();
                if (dt.Rows.Count > 0)
                    //dt.Rows[gvKycDoc.Rows.Count]["SLNo"] = gvKycDoc.Rows.Count + 1;

                dt.Rows[vRow]["Name"] = " ";
                dt.Rows[vRow]["Village"] =" ";
                dt.Rows[vRow]["PO"] = " ";
                dt.Rows[vRow]["PS"] = " ";
                dt.Rows[vRow]["District"] = " ";
               
                dt.AcceptChanges();
                ViewState["GuarInfo"] = dt;
                gvGuar.DataSource = dt;
                gvGuar.DataBind();
                //SetData();
                //DropDownList ddlBrBorrow = (DropDownList)gvBorrow.Rows[vRow].FindControl("ddlBrBorrow");
                //popBrBorrow(ddlBrBorrow);
                //upColl.Update();

            }
            finally
            {
                //oTr = null;
                //dt = null;
            }
        }
        private void SetData()
        {
            DataTable dt = null;
            //byte[] vFUKYcDoc = null;
            //string vFUKYcDocType = "";
            int i = 0;

            dt = (DataTable)ViewState["KYCDoc"];
            foreach (DataRow gr in dt.Rows)
            {
                
                DropDownList ddlDocType = (DropDownList)gvKycDoc.Rows[i].FindControl("ddlDocType");
                //FileUpload FUKYcDoc = (FileUpload)gvKycDoc.Rows[i].FindControl("FUKYcDoc");
                ddlDocType.SelectedIndex = ddlDocType.Items.IndexOf(ddlDocType.Items.FindByValue(Convert.ToString(dt.Rows[i]["DocTypeVal"])));
                i++;
            }
        }
        protected void ImgAdd_Click(object sender, EventArgs e)
        {
            //            //DataTable dt = GetData();
            int curRow = 0, maxRow = 0;
            ImageButton ImgAdd = (ImageButton)sender;
            GridViewRow gr = (GridViewRow)ImgAdd.NamingContainer;
            curRow = gr.RowIndex;
            maxRow = gvKycDoc.Rows.Count;
            if (GetDoc() == true)
            {
                DataTable dt = (DataTable)ViewState["KYCDoc"];


                if (curRow == maxRow - 1)
                {
                    NewKYCDoc(gvKycDoc.Rows.Count);
                    gr.Enabled = false;
                }
            }
        }
        
      protected void ImgAdd1_Click(object sender, EventArgs e)
        {
            //            //DataTable dt = GetData();
            int curRow = 0, maxRow = 0;
            ImageButton ImgAdd = (ImageButton)sender;
            GridViewRow gr = (GridViewRow)ImgAdd.NamingContainer;
            curRow = gr.RowIndex;
            maxRow = gvGuar.Rows.Count;
            if (GetGuarInfo() == true)
            {
                DataTable dt = (DataTable)ViewState["GuarInfo"];


                if (curRow == maxRow - 1)
                {
                    NewGuar(gvGuar.Rows.Count);
                    gr.Enabled = false;
                }
            }
        }
        protected void ImDel_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton ImDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)ImDel.NamingContainer;
            GridView gv = (GridView)ImDel.Parent.Parent.NamingContainer;
            //        //GridViewRow PrevGr = gv.Rows[gR.RowIndex - 1];
           
                dt = (DataTable)ViewState["KYCDoc"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["KYCDoc"] = dt;
                    gvKycDoc.DataSource = dt;
                    gvKycDoc.DataBind();
                    SetData();
                    //PrevGr.Enabled = true;
                    //TotalPaid();
                }
                else if(dt.Rows.Count == 1)
                {                                                   
                    gblFuction.MsgPopup("First Row can not be deleted.");
                    return;
                }
            }
        protected void ImDel1_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton ImDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)ImDel.NamingContainer;
            GridView gv = (GridView)ImDel.Parent.Parent.NamingContainer;

            dt = (DataTable)ViewState["GuarInfo"];
            if (dt.Rows.Count > 1)
            {
                dt.Rows[gR.RowIndex].Delete();
                dt.AcceptChanges();
                ViewState["GuarInfo"] = dt;
                gvGuar.DataSource = dt;
                gvGuar.DataBind();
                //SetData();
                //PrevGr.Enabled = true;
                //TotalPaid();
            }
            else if (dt.Rows.Count == 1)
            {
                //gblFuction.MsgPopup("First Row can not be deleted.");
               // return;
                dt.Rows[gR.RowIndex].Delete();
                dt.AcceptChanges();
                ViewState["GuarInfo"] = dt;
                gvGuar.DataSource = dt;
                gvGuar.DataBind();
            }
        }

        private Boolean GetDoc()
        {
            DataTable dt = null;
            byte[] vFUKYcDoc = null;
            byte[] vImg = null;
            string vFUKYcDocType = "", vImgType = "", vfilename = "";
            dt = (DataTable)ViewState["KYCDoc"];
            foreach (GridViewRow gr in gvKycDoc.Rows)
            {
                dt.Rows[gr.RowIndex]["SLNo"] = gr.RowIndex + 1;
                DropDownList ddlDocType = (DropDownList)gr.FindControl("ddlDocType");
                FileUpload FUKYcDoc = (FileUpload)gr.FindControl("FUKYcDoc");
                Label lbFile=(Label)gr.FindControl("lbFile");
                
                //if (dt.Rows[gr.RowIndex]["Doc_Image"] != System.DBNull.Value)
                //{
                //    vImg = (byte[])dt.Rows[gr.RowIndex]["Doc_Image"];
                //}
                //vImgType = Convert.ToString(dt.Rows[gr.RowIndex]["image_type"]);
                //SetUploads(FUKYcDoc, ref vFUKYcDoc, ref vFUKYcDocType, vImg, vImgType, ref vfilename);
                

                if (gr.RowIndex <= gvKycDoc.Rows.Count - 1)
                {
                    if (FUKYcDoc.HasFile == true)
                    {

                        if (FUKYcDoc.PostedFile.InputStream.Length > 1000000)
                        {
                            gblFuction.MsgPopup("Maximum upload file Size exceed the limit.");
                            return false;
                        }
                        vFUKYcDoc = new byte[FUKYcDoc.PostedFile.InputStream.Length + 1];
                        FUKYcDoc.PostedFile.InputStream.Read(vFUKYcDoc, 0, vFUKYcDoc.Length);
                        vFUKYcDocType = System.IO.Path.GetExtension(FUKYcDoc.FileName).ToLower();
                        vfilename = System.IO.Path.GetFileName(FUKYcDoc.FileName).ToUpper();
                        dt.Rows[gr.RowIndex]["Doc_Image"] = vFUKYcDoc;
                        dt.Rows[gr.RowIndex]["image_type"] = vFUKYcDocType;
                        dt.Rows[gr.RowIndex]["File_name"] = vfilename;
                        if (ddlDocType.SelectedIndex <= 0)
                        {
                            gblFuction.MsgPopup("Please Add Document type");
                            return false;
                        }
                        else
                        {
                            dt.Rows[gr.RowIndex]["Doc_Type"] = ddlDocType.SelectedItem.Text;
                            dt.Rows[gr.RowIndex]["DocTypeVal"] = Convert.ToInt32(ddlDocType.SelectedValue);
                        }
                        
                        
                    }
                    else
                    {
                        if (lbFile.Text == "")
                        {
                            gblFuction.AjxMsgPopup("Please Select a File to Upload...");
                            return false;
                        }
                    }
                    
                      
                }

            }
            dt.AcceptChanges();
            ViewState["KYCDoc"] = dt;
            gvKycDoc.DataSource = dt;
            gvKycDoc.DataBind();
            return true;
        }

        private Boolean GetGuarInfo()
        {
            DataTable dt = null;
            dt = (DataTable)ViewState["GuarInfo"];
            foreach (GridViewRow gr in gvGuar.Rows)
            {
                dt.Rows[gr.RowIndex]["SLNo"] = gr.RowIndex + 1;
                TextBox txtGuarName = (TextBox)gr.FindControl("txtGuarName");
                TextBox txtGuarVill = (TextBox)gr.FindControl("txtGuarVill");
                TextBox txtGuarPO = (TextBox)gr.FindControl("txtGuarPO");
                TextBox txtGuarPS = (TextBox)gr.FindControl("txtGuarPS");
                TextBox txtGuarDist = (TextBox)gr.FindControl("txtGuarDist");


               
                //if (gr.RowIndex <= gvGuar.Rows.Count - 1)
                //{
                //    if (txtGuarName.Text.Trim() !=string.Empty)
                //    {
                //        dt.Rows[gr.RowIndex]["Name"] = txtGuarName.Text.Trim();
                        
                //        dt.Rows[gr.RowIndex]["Village"] = txtGuarVill.Text.Trim();
                //        dt.Rows[gr.RowIndex]["PO"] = txtGuarPO.Text.Trim();
                //        dt.Rows[gr.RowIndex]["PS"] = txtGuarPS.Text.Trim();
                //        dt.Rows[gr.RowIndex]["District"] = txtGuarDist.Text.Trim();
                //    }
                //    //else
                //    //{
                //    //    gblFuction.AjxMsgPopup("Enter Witness Name");
                //    //    return false;
                //    //}
                //}
                if (gr.RowIndex <= gvGuar.Rows.Count - 1)
                {
                    if (txtGuarName.Text.Trim() != string.Empty)
                    {
                        dt.Rows[gr.RowIndex]["Name"] = txtGuarName.Text.Trim();
                        if (txtGuarVill.Text.Trim() != string.Empty)
                        {
                            dt.Rows[gr.RowIndex]["Village"] = txtGuarVill.Text.Trim();
                        }
                        else
                        {
                            dt.Rows[gr.RowIndex]["Village"] = "";
                        }
                        if (txtGuarPO.Text.Trim() != string.Empty)
                        {
                            dt.Rows[gr.RowIndex]["PO"] = txtGuarPO.Text.Trim();
                        }
                        else
                        {
                            dt.Rows[gr.RowIndex]["PO"] = "";
                        }
                        if (txtGuarPS.Text.Trim() != string.Empty)
                        {
                            dt.Rows[gr.RowIndex]["PS"] = txtGuarPS.Text.Trim();
                        }
                        else
                        {
                            dt.Rows[gr.RowIndex]["PS"] = "";
                        }
                        if (txtGuarDist.Text.Trim() != string.Empty)
                        {
                            dt.Rows[gr.RowIndex]["District"] = txtGuarDist.Text.Trim();
                        }
                        else
                        {
                            dt.Rows[gr.RowIndex]["District"] = "";
                        }
                        //dt.Rows[gr.RowIndex]["PO"] = txtGuarPO.Text.Trim();
                        //dt.Rows[gr.RowIndex]["PS"] = txtGuarPS.Text.Trim();
                        //dt.Rows[gr.RowIndex]["District"] = txtGuarDist.Text.Trim();
                    }
                    //else
                    //{
                    //    gblFuction.AjxMsgPopup("Enter Witness Name");
                    //    return false;
                    //}
                }

            }
            dt.AcceptChanges();
            ViewState["GuarInfo"] = dt;
            gvGuar.DataSource = dt;
            gvGuar.DataBind();
            return true;
        }

        protected void gvKycDoc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    FileUpload FUKYcDoc = (FileUpload)e.Row.FindControl("FUKYcDoc");
                    DropDownList ddlDocType = (DropDownList)e.Row.FindControl("ddlDocType");
                    if (e.Row.Cells[8].Text == "Y")
                    {
                        FUKYcDoc.Enabled = false;
                    }
                    ddlDocType.SelectedIndex = ddlDocType.Items.IndexOf(ddlDocType.Items.FindByValue(e.Row.Cells[9].Text.Trim()));
                }
            }
            finally
            {
            }
        }
        protected void gvLoanAppl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string vAppId = "";
            //vAppId = (ViewState["AppId"]).ToString();
            try
            {
                //vAppId = Convert.ToString(e.CommandArgument);
                if (vAppId != "")
                {
                    vAppId = (ViewState["AppId"]).ToString();
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlreport = (DropDownList)e.Row.FindControl("ddlreport");
                    //if (ddlreport.SelectedIndex == 1)
                    //{
                    //    //printHindiform(vAppId);

                    //    Console.WriteLine("Hindi");
                    //}
                    //if (ddlreport.SelectedIndex == 2)
                    //{
                    //    //printBengaliform(vAppId);
                    //    Console.WriteLine("Bengali");
                    //}


                }
            }
            finally
            {
            }
        }
        //protected void ddlreport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string vMode = "", vAppId = "";

        //    DropDownList ddlreport = (DropDownList)sender;
        //    GridViewRow row = (GridViewRow)ddlreport.NamingContainer;
        //    try
        //    {
        //        if (row != null)
        //        {
        //            vMode = ((DropDownList)(row.FindControl("ddlreport"))).SelectedValue;
        //            vAppId = row.Cells[11].Text; 
        //            //if(cvar==1)
        //            if (ddlreport.SelectedIndex > 0)
        //            {
        //                ddlreport.SelectedIndex = -1;
        //                SetParameterForRptData(vAppId, "PDF");
        //            }

        //        }
        //    }
        //    finally
        //    {

        //    }

        //}
        private void SetParameterForRptData(string vMode, string pAppId, string pType)
        {
           
            DataSet ds = null;
            DataTable dt1 = null, dt2 = null;
            CReports oRpt = null;
            string vRptPath = "", vBrCode = "", vTitle = "";
            string vBranch = Session[gblValue.BrName].ToString();
           
           
            try
            {
                //cvar = 1;
                oRpt = new CReports();
                ds = oRpt.RptLoanAppForm(pAppId);
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt1.TableName = "LoanApplFormFront";
                dt2.TableName = "LoanApplFormBack";

                if (vMode == "B")
                {                   
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanAppForm_Bengali.rpt";
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        if (pType == "PDF")
                        {
                            rptDoc.Load(vRptPath);
                            rptDoc.SetDataSource(ds);
                            //rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                            //rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                            //rptDoc.SetParameterValue("pAddress2", "");
                            //rptDoc.SetParameterValue("pBranch", vBranch);
                            //rptDoc.SetParameterValue("pTitle", vTitle);
                            rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "LoanApplicationForm_Bengali");
                            Response.ClearHeaders();
                            Response.ClearContent();
                        }
                        
                    }
                }
                else if (vMode == "H")
                {

                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanAppForm_Hindi.rpt";
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        
                            rptDoc.Load(vRptPath);
                            rptDoc.SetDataSource(ds);
                            //rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                            //rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                            //rptDoc.SetParameterValue("pAddress2", "");
                            //rptDoc.SetParameterValue("pBranch", vBranch);
                            //rptDoc.SetParameterValue("pTitle", vTitle);
                            rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "LoanApplicationForm_Hindi");
                            Response.ClearHeaders();
                            Response.ClearContent();
                        
                    }
                }             
             
            }
            finally
            {
                dt1 = null;
                dt2 = null;
                oRpt = null;
                cvar = 0;
            }
        }
    }
}



    
    
