using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FORCEBA;
using FORCECA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class IntInspPM : CENTRUMBase
    {
        private int vPgNo = 1;

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
                popBranch();
                LoadGrid(1);
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                tbEmp.ActiveTabIndex = 0;
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
                this.PageHeading = "Process Management";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuIntPM);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Process Management", false);
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
            txtSDt.Enabled = Status;
            txtCrStrDt.Enabled = Status;
            txtCrEndDt.Enabled = Status;
            txtPrStrDt.Enabled = Status;
            txtPrEndDt.Enabled = Status;
            ddlBranch.Enabled = Status;
            ddlQ1.Enabled = Status;
            txtRem1.Enabled = Status;
            ddlQ2.Enabled = Status;
            txtRem2.Enabled = Status;
            ddlQ3.Enabled = Status;
            txtRem3.Enabled = Status;
            ddlQ4.Enabled = Status;
            txtRem4.Enabled = Status;
            ddlQ5.Enabled = Status;
            txtRem5.Enabled = Status;
            ddlQ6.Enabled = Status;
            txtRem6.Enabled = Status;
            ddlQ7.Enabled = Status;
            txtRem7.Enabled = Status;
            ddlQ8.Enabled = Status;
            txtRem8.Enabled = Status;
            ddlQ9.Enabled = Status;
            txtRem9.Enabled = Status;
            ddlQ10.Enabled = Status;
            txtRem10.Enabled = Status;
            ddlQ11.Enabled = Status;
            txtRem11.Enabled = Status;
            ddlQ12.Enabled = Status;
            txtRem12.Enabled = Status;
            ddlQ13.Enabled = Status;
            txtRem13.Enabled = Status;

            ddlQ101.Enabled = Status;
            txtRem101.Enabled = Status;

            ddlQ14.Enabled = Status;
            txtRem14.Enabled = Status;
            ddlQ15.Enabled = Status;
            txtRem15.Enabled = Status;
            ddlQ16.Enabled = Status;
            txtRem16.Enabled = Status;
            ddlQ17.Enabled = Status;
            txtRem17.Enabled = Status;
            ddlQ18.Enabled = Status;
            txtRem18.Enabled = Status;
            ddlQ19.Enabled = Status;
            txtRem19.Enabled = Status;
            ddlQ20.Enabled = Status;
            txtRem20.Enabled = Status;

            ddlQ100.Enabled = Status;
            txtRemLast.Enabled = Status;


            ddlQ103.Enabled = Status;
            txtRem103.Enabled = Status;


            ddlQ21.Enabled = Status;
            txtRem21.Enabled = Status;
            ddlQ22.Enabled = Status;
            txtRem22.Enabled = Status;
            ddlQ23.Enabled = Status;
            txtRem23.Enabled = Status;
            ddlQ24.Enabled = Status;
            txtRem24.Enabled = Status;
            ddlQ25.Enabled = Status;
            txtRem25.Enabled = Status;
            ddlQ26.Enabled = Status;
            txtRem26.Enabled = Status;

            ddlQ102.Enabled = Status;
            txtRem102.Enabled = Status;

            ddlQ27.Enabled = Status;
            txtRem27.Enabled = Status;
            ddlQ28.Enabled = Status;
            txtRem28.Enabled = Status;
            ddlQ29.Enabled = Status;
            txtRem29.Enabled = Status;
            ddlQ30.Enabled = Status;
            txtRem30.Enabled = Status;
            ddlQ31.Enabled = Status;
            txtRem31.Enabled = Status;
            ddlQ32.Enabled = Status;
            txtRem32.Enabled = Status;
            ddlQ33.Enabled = Status;
            txtRem33.Enabled = Status;
            ddlQ34.Enabled = Status;
            txtRem34.Enabled = Status;
            ddlQ35.Enabled = Status;
            txtRem35.Enabled = Status;
            ddlQ36.Enabled = Status;
            txtRem36.Enabled = Status;
            ddlQ37.Enabled = Status;
            txtRem37.Enabled = Status;
            ddlQ38.Enabled = Status;
            txtRem38.Enabled = Status;
            ddlQ39.Enabled = Status;
            txtRem39.Enabled = Status;
            ddlQ40.Enabled = Status;
            txtRem40.Enabled = Status;
            ddlQ41.Enabled = Status;
            txtRem41.Enabled = Status;
            ddlQ42.Enabled = Status;
            txtRem42.Enabled = Status;
            ddlQ43.Enabled = Status;
            txtRem43.Enabled = Status;
            ddlQ44.Enabled = Status;
            txtRem44.Enabled = Status;
            ddlQ45.Enabled = Status;
            txtRem45.Enabled = Status;
            ddlQ46.Enabled = Status;
            txtRem46.Enabled = Status;
            ddlQ47.Enabled = Status;
            txtRem47.Enabled = Status;

            ddlQ48.Enabled = Status;
            txtRem48.Enabled = Status;
            ddlQ49.Enabled = Status;
            txtRem49.Enabled = Status;
            ddlQ50.Enabled = Status;
            txtRem50.Enabled = Status;

            txtRem51.Enabled = Status;
            txtRem52.Enabled = Status;

            txtIre1.Enabled = Status;
            txtIre2.Enabled = Status;
            txtIre3.Enabled = Status;
            txtIre4.Enabled = Status;
            txtIre5.Enabled = Status;
            txtIre6.Enabled = Status;
            txtIre7.Enabled = Status;
            txtIre8.Enabled = Status;
            txtIre9.Enabled = Status;
            txtIre10.Enabled = Status;
            txtIre11.Enabled = Status;
            txtIre12.Enabled = Status;
            txtIre13.Enabled = Status;

            txtIre101.Enabled = Status;

            txtIre14.Enabled = Status;
            txtIre15.Enabled = Status;
            txtIre16.Enabled = Status;
            txtIre17.Enabled = Status;
            txtIre18.Enabled = Status;
            txtIre19.Enabled = Status;
            txtIre20.Enabled = Status;

            txtIre100.Enabled = Status;

            txtIre103.Enabled = Status;

            txtIre21.Enabled = Status;
            txtIre22.Enabled = Status;
            txtIre23.Enabled = Status;
            txtIre24.Enabled = Status;
            txtIre25.Enabled = Status;
            txtIre26.Enabled = Status;

            txtIre102.Enabled = Status;

            txtIre27.Enabled = Status;
            txtIre28.Enabled = Status;
            txtIre29.Enabled = Status;
            txtIre30.Enabled = Status;
            txtIre31.Enabled = Status;
            txtIre32.Enabled = Status;
            txtIre33.Enabled = Status;
            txtIre34.Enabled = Status;
            txtIre35.Enabled = Status;
            txtIre36.Enabled = Status;
            txtIre37.Enabled = Status;
            txtIre38.Enabled = Status;
            txtIre39.Enabled = Status;
            txtIre40.Enabled = Status;
            txtIre41.Enabled = Status;
            txtIre42.Enabled = Status;
            txtIre43.Enabled = Status;
            txtIre44.Enabled = Status;
            txtIre45.Enabled = Status;
            txtIre46.Enabled = Status;
            txtIre47.Enabled = Status;
            txtIre48.Enabled = Status;
            txtIre49.Enabled = Status;

            txtIre50.Enabled = Status;
            txtIre51.Enabled = Status;
            txtIre52.Enabled = Status;
            ddlQ201.Enabled = Status;
            txtRem201.Enabled = Status;
            txtIre201.Enabled = Status;
            ddlQ202.Enabled = Status;
            txtRem202.Enabled = Status;
            txtIre202.Enabled = Status;
            ddlQ203.Enabled = Status;
            txtRem203.Enabled = Status;
            txtIre203.Enabled = Status;
            ddlQ204.Enabled = Status;
            txtRem204.Enabled = Status;
            txtIre204.Enabled = Status;
            ddlQ205.Enabled = Status;
            txtRem205.Enabled = Status;
            txtIre205.Enabled = Status;
            ddlQ206.Enabled = Status;
            txtRem206.Enabled = Status;
            txtIre206.Enabled = Status;
            ddlQ207.Enabled = Status;
            txtRem207.Enabled = Status;
            txtIre207.Enabled = Status;
            ddlQ208.Enabled = Status;
            txtRem208.Enabled = Status;
            txtIre208.Enabled = Status;
            ddlQ209.Enabled = Status;
            txtRem209.Enabled = Status;
            txtIre209.Enabled = Status;
            txtOther.Enabled = Status;
            ddlQ301.Enabled = Status;
            txtQ301scr.Enabled = Status;
            txtIre301.Enabled = Status;
            txtRem301.Enabled = Status;
        }


        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtSDt.Text = Session[gblValue.LoginDate].ToString();
            txtCrStrDt.Text = "";
            txtCrEndDt.Text = "";
            txtPrStrDt.Text = "";
            txtPrEndDt.Text = "";
            ddlBranch.SelectedIndex = -1;
            ddlQ1.SelectedIndex = -1;            
            txtQ1Scr.Text = "4";
            txtRem1.Text = "";
            ddlQ2.SelectedIndex = -1;
            txtQ2Scr.Text = "4";
            txtRem2.Text = "";
            ddlQ3.SelectedIndex = -1;
            txtQ3Scr.Text = "4";
            txtRem3.Text = "";
            ddlQ4.SelectedIndex = -1;
            txtQ4Scr.Text = "4";
            txtRem4.Text = "";
            ddlQ5.SelectedIndex = -1;
            txtQ5Scr.Text = "4";
            txtRem5.Text = "";
            ddlQ6.SelectedIndex = -1;
            txtQ6Scr.Text = "4";
            txtRem6.Text = "";
            ddlQ7.SelectedIndex = -1;
            txtQ7Scr.Text = "4";
            txtRem7.Text = "";
            ddlQ8.SelectedIndex = -1;
            txtQ8Scr.Text = "4";
            txtRem8.Text = "";
            ddlQ9.SelectedIndex = -1;
            txtQ9Scr.Text = "4";
            txtRem9.Text = "";
            ddlQ10.SelectedIndex = -1;
            txtQ10Scr.Text = "4";
            txtRem10.Text = "";
            ddlQ11.SelectedIndex = -1;
            txtQ11Scr.Text = "4";
            txtRem11.Text = "";
            ddlQ12.SelectedIndex = -1;
            txtQ12Scr.Text = "4";
            txtRem12.Text = "";
            ddlQ13.SelectedIndex = -1;
            txtQ13Scr.Text = "4";
            txtRem13.Text = "";

            ddlQ101.SelectedIndex = -1;
            txtQ101Scr.Text = "4";
            txtRem101.Text = "";

            ddlQ14.SelectedIndex = -1;
            txtQ14Scr.Text = "4";
            txtRem14.Text = "";
            ddlQ15.SelectedIndex = -1;
            txtQ15Scr.Text = "4";
            txtRem15.Text = "";
            ddlQ16.SelectedIndex = -1;
            txtQ16Scr.Text = "4";
            txtRem16.Text = "";
            ddlQ17.SelectedIndex = -1;
            txtQ17Scr.Text = "4";
            txtRem17.Text = "";
            ddlQ18.SelectedIndex = -1;
            txtQ18Scr.Text = "4";
            txtRem18.Text = "";
            ddlQ19.SelectedIndex = -1;
            txtQ19Scr.Text = "4";
            txtRem19.Text = "";
            ddlQ20.SelectedIndex = -1;
            txtQ20Scr.Text = "4";
            txtRem20.Text = "";
            ddlQ100.SelectedIndex = -1;
            txtQ100Scr.Text = "0";
            txtRemLast.Text = "";


            ddlQ103.SelectedIndex = -1;
            txtQ103Scr.Text = "0";
            txtRem103.Text = "";




            ddlQ21.SelectedIndex = -1;
            txtQ21Scr.Text = "4";
            txtRem21.Text = "";
            ddlQ22.SelectedIndex = -1;
            txtQ22Scr.Text = "4";
            txtRem22.Text = "";
            ddlQ23.SelectedIndex = -1;
            txtQ23Scr.Text = "4";
            txtRem23.Text = "";
            ddlQ24.SelectedIndex = -1;
            txtQ24Scr.Text = "4";
            txtRem24.Text = "";
            ddlQ25.SelectedIndex = -1;
            txtQ25Scr.Text = "4";
            txtRem25.Text = "";
            ddlQ26.SelectedIndex = -1;
            txtQ26Scr.Text = "4";
            txtRem26.Text = "";

            ddlQ102.SelectedIndex = -1;
            txtQ102Scr.Text = "4";
            txtRem102.Text = "";

            ddlQ27.SelectedIndex = -1;
            txtQ27Scr.Text = "4";
            txtRem27.Text = "";
            ddlQ28.SelectedIndex = -1;
            txtQ28Scr.Text = "4";
            txtRem28.Text = "";
            ddlQ29.SelectedIndex = -1;
            txtQ29Scr.Text = "4";
            txtRem29.Text = "";
            ddlQ30.SelectedIndex = -1;
            txtQ30Scr.Text = "4";
            txtRem30.Text = "";
            ddlQ31.SelectedIndex = -1;
            txtQ31Scr.Text = "4";
            txtRem31.Text = "";
            ddlQ32.SelectedIndex = -1;
            txtQ32Scr.Text = "4";
            txtRem32.Text = "";
            ddlQ33.SelectedIndex = -1;
            txtQ33Scr.Text = "4";
            txtRem33.Text = "";
            ddlQ34.SelectedIndex = -1;
            txtQ34Scr.Text = "4";
            txtRem34.Text = "";
            ddlQ35.SelectedIndex = -1;
            txtQ35Scr.Text = "4";
            txtRem35.Text = "";
            ddlQ36.SelectedIndex = -1;
            txtQ36Scr.Text = "4";
            txtRem36.Text = "";
            ddlQ37.SelectedIndex = -1;
            txtQ37Scr.Text = "4";
            txtRem37.Text = "";
            ddlQ38.SelectedIndex = -1;
            txtQ38Scr.Text = "4";
            txtRem38.Text = "";
            ddlQ39.SelectedIndex = -1;
            txtQ39Scr.Text = "4";
            txtRem39.Text = "";
            ddlQ40.SelectedIndex = -1;
            txtQ40Scr.Text = "4";
            txtRem40.Text = "";
            ddlQ41.SelectedIndex = -1;
            txtQ41Scr.Text = "4";
            txtRem41.Text = "";
            ddlQ42.SelectedIndex = -1;
            txtQ42Scr.Text = "4";
            txtRem42.Text = "";
            ddlQ43.SelectedIndex = -1;
            txtQ43Scr.Text = "4";
            txtRem43.Text = "";
            ddlQ44.SelectedIndex = -1;
            txtQ44Scr.Text = "4";
            txtRem44.Text = "";
            ddlQ45.SelectedIndex = -1;
            txtQ45Scr.Text = "4";
            txtRem45.Text = "";
            ddlQ46.SelectedIndex = -1;
            txtQ46Scr.Text = "4";
            txtRem46.Text = "";
            ddlQ47.SelectedIndex = -1;
            txtQ47Scr.Text = "4";
            txtRem47.Text = "";

            ddlQ48.SelectedIndex = -1;
            txtQ48Scr.Text = "4";
            txtRem48.Text = "";
            ddlQ49.SelectedIndex = -1;
            txtQ49Scr.Text = "4";
            txtRem49.Text = "";
            ddlQ50.SelectedIndex = -1;
            txtQ50Scr.Text = "4";
            txtRem50.Text = "";

            txtRem51.Text = "";
            txtRem52.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";

            txtIre1.Text = "0";
            txtIre2.Text = "0";
            txtIre3.Text = "0";
            txtIre4.Text = "0";
            txtIre5.Text = "0";
            txtIre6.Text = "0";
            txtIre7.Text = "0";
            txtIre8.Text = "0";
            txtIre9.Text = "0";
            txtIre10.Text = "0";
            txtIre11.Text = "0";
            txtIre12.Text = "0";
            txtIre13.Text = "0";
            txtIre101.Text = "0";
            txtIre14.Text = "0";
            txtIre15.Text = "0";
            txtIre16.Text = "0";
            txtIre17.Text = "0";
            txtIre18.Text = "0";
            txtIre19.Text = "0";
            txtIre20.Text = "0";

            txtIre100.Text = "0";

            txtIre103.Text = "0";

            txtIre21.Text = "0";
            txtIre22.Text = "0";
            txtIre23.Text = "0";
            txtIre24.Text = "0";
            txtIre25.Text = "0";
            txtIre26.Text = "0";
            txtIre102.Text = "0";

            txtIre27.Text = "0";
            txtIre28.Text = "0";
            txtIre29.Text = "0";
            txtIre30.Text = "0";
            txtIre31.Text = "0";
            txtIre32.Text = "0";
            txtIre33.Text = "0";
            txtIre34.Text = "0";
            txtIre35.Text = "0";
            txtIre36.Text = "0";
            txtIre37.Text = "0";
            txtIre38.Text = "0";
            txtIre39.Text = "0";
            txtIre40.Text = "0";
            txtIre41.Text = "0";
            txtIre42.Text = "0";
            txtIre43.Text = "0";
            txtIre44.Text = "0";
            txtIre45.Text = "0";
            txtIre46.Text = "0";
            txtIre47.Text = "0";
            txtIre48.Text = "0";
            txtIre49.Text = "0";
            txtIre50.Text = "0";
            txtIre51.Text = "0";
            txtIre52.Text = "0";
            ddlQ201.SelectedIndex = -1;
            txtRem201.Text = "";
            txtQ201Scr.Text = "4";
            txtIre201.Text = "0";
            ddlQ202.SelectedIndex = -1;
            txtRem202.Text = "";
            txtQ202Scr.Text = "4";
            txtIre202.Text = "0";
            ddlQ203.SelectedIndex = -1;
            txtRem203.Text = "";
            txtQ203Scr.Text = "4";
            txtIre203.Text = "0";
            ddlQ204.SelectedIndex = -1;
            txtRem204.Text = "";
            txtQ204Scr.Text = "4";
            txtIre204.Text = "0";
            ddlQ205.SelectedIndex = -1;
            txtRem205.Text = "";
            txtQ205Scr.Text = "4";
            txtIre205.Text = "0";
            ddlQ206.SelectedIndex = -1;
            txtRem206.Text = "";
            txtQ206Scr.Text = "4";
            txtIre206.Text = "0";
            ddlQ207.SelectedIndex = -1;
            txtRem207.Text = "";
            txtQ207Scr.Text = "4";
            txtIre207.Text = "0";
            ddlQ208.SelectedIndex = -1;
            txtRem208.Text = "";
            txtQ208Scr.Text = "4";
            txtIre208.Text = "0";
            ddlQ209.SelectedIndex = -1;
            txtRem209.Text = "";
            txtQ209Scr.Text = "4";
            txtIre209.Text = "0";
            txtOther.Text = "";
            ddlQ301.SelectedIndex = -1;
            txtQ301scr.Text = "4";
            txtIre301.Text = "0";
            txtRem301.Text = "";
        }


        /// <summary>
        /// 
        /// </summary>
        private void popBranch()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBranch.Items.Insert(0, oli);
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

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {            
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(0);        
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0;
            CIntIspPM oAd = null;
            try
            {
                DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                oAd = new CIntIspPM();
                dt = oAd.GetIntInspPMPG(pPgIndx, vFrmDt, vToDt, ref vTotRows);
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
                oAd = null;
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
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx", false);
        }
               

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEmp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int pInspID = 0;
            CIntIspPM oAu = null;
            DataTable dt = null;           
            try
            {
                pInspID = Convert.ToInt32(e.CommandArgument);
                ViewState["InsPecId"] = pInspID;
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
                    oAu = new CIntIspPM();
                    dt = oAu.GetIntInspPMById(pInspID);                    
                    if (dt.Rows.Count > 0)
                    {
                        txtSDt.Text = Convert.ToString(dt.Rows[0]["SubDt"]);
                        txtCrStrDt.Text = Convert.ToString(dt.Rows[0]["CrdFrmDt"]);
                        txtCrEndDt.Text = Convert.ToString(dt.Rows[0]["CrdToDt"]);
                        txtPrStrDt.Text = Convert.ToString(dt.Rows[0]["PrdFrmDt"]);
                        txtPrEndDt.Text = Convert.ToString(dt.Rows[0]["PrdToDt"]);
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["Branch"].ToString()));

                        ddlQ1.SelectedIndex = ddlQ1.Items.IndexOf(ddlQ1.Items.FindByValue(dt.Rows[0]["A1"].ToString()));
                        txtQ1Scr.Text = Convert.ToString(dt.Rows[0]["S1"]);
                        txtRem1.Text = Convert.ToString(dt.Rows[0]["R1"]);
                        ddlQ2.SelectedIndex = ddlQ2.Items.IndexOf(ddlQ2.Items.FindByValue(dt.Rows[0]["A2"].ToString()));
                        txtQ2Scr.Text = Convert.ToString(dt.Rows[0]["S2"]);
                        txtRem2.Text = Convert.ToString(dt.Rows[0]["R2"]);
                        ddlQ3.SelectedIndex = ddlQ3.Items.IndexOf(ddlQ3.Items.FindByValue(dt.Rows[0]["A3"].ToString()));
                        txtQ3Scr.Text = Convert.ToString(dt.Rows[0]["S3"]);
                        txtRem3.Text = Convert.ToString(dt.Rows[0]["R3"]);
                        ddlQ4.SelectedIndex = ddlQ4.Items.IndexOf(ddlQ4.Items.FindByValue(dt.Rows[0]["A4"].ToString()));

                        if (Convert.ToString(dt.Rows[0]["S4"]) == "-1")
                            txtQ4Scr.Text = "N/A";
                        else
                            txtQ4Scr.Text = Convert.ToString(dt.Rows[0]["S4"]);
                        txtRem4.Text = Convert.ToString(dt.Rows[0]["R4"]);
                        ddlQ5.SelectedIndex = ddlQ5.Items.IndexOf(ddlQ5.Items.FindByValue(dt.Rows[0]["A5"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S5"]) == "-1")
                            txtQ5Scr.Text = "N/A";
                        else
                            txtQ5Scr.Text = Convert.ToString(dt.Rows[0]["S5"]);
                        txtRem5.Text = Convert.ToString(dt.Rows[0]["R5"]);
                        ddlQ6.SelectedIndex = ddlQ6.Items.IndexOf(ddlQ6.Items.FindByValue(dt.Rows[0]["A6"].ToString()));
                        txtQ6Scr.Text = Convert.ToString(dt.Rows[0]["S6"]);
                        txtRem6.Text = Convert.ToString(dt.Rows[0]["R6"]);
                        ddlQ7.SelectedIndex = ddlQ7.Items.IndexOf(ddlQ7.Items.FindByValue(dt.Rows[0]["A7"].ToString()));
                        txtQ7Scr.Text = Convert.ToString(dt.Rows[0]["S7"]);
                        txtRem7.Text = Convert.ToString(dt.Rows[0]["R7"]);
                        ddlQ8.SelectedIndex = ddlQ8.Items.IndexOf(ddlQ8.Items.FindByValue(dt.Rows[0]["A8"].ToString()));
                        txtQ8Scr.Text = Convert.ToString(dt.Rows[0]["S8"]);
                        txtRem8.Text = Convert.ToString(dt.Rows[0]["R8"]);
                        ddlQ9.SelectedIndex = ddlQ9.Items.IndexOf(ddlQ9.Items.FindByValue(dt.Rows[0]["A9"].ToString()));
                        txtQ9Scr.Text = Convert.ToString(dt.Rows[0]["S9"]);
                        txtRem9.Text = Convert.ToString(dt.Rows[0]["R9"]);
                        ddlQ10.SelectedIndex = ddlQ10.Items.IndexOf(ddlQ10.Items.FindByValue(dt.Rows[0]["A10"].ToString()));
                        txtQ10Scr.Text = Convert.ToString(dt.Rows[0]["S10"]);
                        txtRem10.Text = Convert.ToString(dt.Rows[0]["R10"]);
                        ddlQ11.SelectedIndex = ddlQ11.Items.IndexOf(ddlQ11.Items.FindByValue(dt.Rows[0]["A11"].ToString()));
                        txtQ11Scr.Text = Convert.ToString(dt.Rows[0]["S11"]);
                        txtRem11.Text = Convert.ToString(dt.Rows[0]["R11"]);
                        ddlQ12.SelectedIndex = ddlQ12.Items.IndexOf(ddlQ12.Items.FindByValue(dt.Rows[0]["A12"].ToString()));
                        txtQ12Scr.Text = Convert.ToString(dt.Rows[0]["S12"]);
                        txtRem12.Text = Convert.ToString(dt.Rows[0]["R12"]);
                        ddlQ13.SelectedIndex = ddlQ13.Items.IndexOf(ddlQ13.Items.FindByValue(dt.Rows[0]["A13"].ToString()));
                        txtQ13Scr.Text = Convert.ToString(dt.Rows[0]["S13"]);
                        txtRem13.Text = Convert.ToString(dt.Rows[0]["R13"]);

                        ddlQ101.SelectedIndex = ddlQ101.Items.IndexOf(ddlQ101.Items.FindByValue(dt.Rows[0]["A101"].ToString()));
                        txtQ101Scr.Text = Convert.ToString(dt.Rows[0]["S101"]);
                        txtRem101.Text = Convert.ToString(dt.Rows[0]["R101"]);


                        ddlQ14.SelectedIndex = ddlQ14.Items.IndexOf(ddlQ14.Items.FindByValue(dt.Rows[0]["A14"].ToString()));
                        txtQ14Scr.Text = Convert.ToString(dt.Rows[0]["S14"]);
                        txtRem14.Text = Convert.ToString(dt.Rows[0]["R14"]);
                        ddlQ15.SelectedIndex = ddlQ15.Items.IndexOf(ddlQ15.Items.FindByValue(dt.Rows[0]["A15"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S15"]) == "-1")
                            txtQ15Scr.Text = "N/A";
                        else
                            txtQ15Scr.Text = Convert.ToString(dt.Rows[0]["S15"]);
                        txtRem15.Text = Convert.ToString(dt.Rows[0]["R15"]);


                        ddlQ16.SelectedIndex = ddlQ16.Items.IndexOf(ddlQ16.Items.FindByValue(dt.Rows[0]["A16"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S16"]) == "-1")
                            txtQ16Scr.Text = "N/A";
                        else
                        txtQ16Scr.Text = Convert.ToString(dt.Rows[0]["S16"]);
                        txtRem16.Text = Convert.ToString(dt.Rows[0]["R16"]);



                        ddlQ17.SelectedIndex = ddlQ17.Items.IndexOf(ddlQ17.Items.FindByValue(dt.Rows[0]["A17"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S17"]) == "-1")
                            txtQ17Scr.Text = "N/A";
                        else
                            txtQ17Scr.Text = Convert.ToString(dt.Rows[0]["S17"]);
                        txtRem17.Text = Convert.ToString(dt.Rows[0]["R17"]);
                        ddlQ18.SelectedIndex = ddlQ18.Items.IndexOf(ddlQ18.Items.FindByValue(dt.Rows[0]["A18"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S18"]) == "-1")
                            txtQ18Scr.Text = "N/A";
                        else
                            txtQ18Scr.Text = Convert.ToString(dt.Rows[0]["S18"]);
                        txtRem18.Text = Convert.ToString(dt.Rows[0]["R18"]);
                        ddlQ19.SelectedIndex = ddlQ19.Items.IndexOf(ddlQ19.Items.FindByValue(dt.Rows[0]["A19"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S19"]) == "-1")
                            txtQ19Scr.Text = "N/A";
                        else
                            txtQ19Scr.Text = Convert.ToString(dt.Rows[0]["S19"]);
                        txtRem19.Text = Convert.ToString(dt.Rows[0]["R19"]);
                        ddlQ20.SelectedIndex = ddlQ20.Items.IndexOf(ddlQ20.Items.FindByValue(dt.Rows[0]["A20"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S20"]) == "-1")
                            txtQ20Scr.Text = "N/A";
                        else
                            txtQ20Scr.Text = Convert.ToString(dt.Rows[0]["S20"]);
                        txtRem20.Text = Convert.ToString(dt.Rows[0]["R20"]);

                        ddlQ100.SelectedIndex = ddlQ100.Items.IndexOf(ddlQ100.Items.FindByValue(dt.Rows[0]["A100"].ToString()));
                        txtQ100Scr.Text = Convert.ToString(dt.Rows[0]["S100"]);
                        txtRemLast.Text = Convert.ToString(dt.Rows[0]["R100"]);


                        ddlQ103.SelectedIndex = ddlQ103.Items.IndexOf(ddlQ103.Items.FindByValue(dt.Rows[0]["A103"].ToString()));
                        txtQ103Scr.Text = Convert.ToString(dt.Rows[0]["S103"]);
                        txtRem103.Text = Convert.ToString(dt.Rows[0]["R103"]);




                        ddlQ21.SelectedIndex = ddlQ21.Items.IndexOf(ddlQ21.Items.FindByValue(dt.Rows[0]["A21"].ToString()));
                        txtQ21Scr.Text = Convert.ToString(dt.Rows[0]["S21"]);
                        txtRem21.Text = Convert.ToString(dt.Rows[0]["R21"]);
                        ddlQ22.SelectedIndex = ddlQ22.Items.IndexOf(ddlQ22.Items.FindByValue(dt.Rows[0]["A22"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S22"]) == "-1")
                            txtQ22Scr.Text = "N/A";
                        else
                            txtQ22Scr.Text = Convert.ToString(dt.Rows[0]["S22"]);
                        txtRem22.Text = Convert.ToString(dt.Rows[0]["R22"]);
                        ddlQ23.SelectedIndex = ddlQ23.Items.IndexOf(ddlQ23.Items.FindByValue(dt.Rows[0]["A23"].ToString()));
                        txtQ23Scr.Text = Convert.ToString(dt.Rows[0]["S23"]);
                        txtRem23.Text = Convert.ToString(dt.Rows[0]["R23"]);
                        ddlQ24.SelectedIndex = ddlQ24.Items.IndexOf(ddlQ24.Items.FindByValue(dt.Rows[0]["A24"].ToString()));
                        txtQ24Scr.Text = Convert.ToString(dt.Rows[0]["S24"]);
                        txtRem24.Text = Convert.ToString(dt.Rows[0]["R24"]);
                        ddlQ25.SelectedIndex = ddlQ25.Items.IndexOf(ddlQ25.Items.FindByValue(dt.Rows[0]["A25"].ToString()));
                        txtQ25Scr.Text = Convert.ToString(dt.Rows[0]["S25"]);
                        txtRem25.Text = Convert.ToString(dt.Rows[0]["R25"]);
                        ddlQ26.SelectedIndex = ddlQ26.Items.IndexOf(ddlQ26.Items.FindByValue(dt.Rows[0]["A26"].ToString()));
                        txtQ26Scr.Text = Convert.ToString(dt.Rows[0]["S26"]);
                        txtRem26.Text = Convert.ToString(dt.Rows[0]["R26"]);

                        ddlQ102.SelectedIndex = ddlQ102.Items.IndexOf(ddlQ102.Items.FindByValue(dt.Rows[0]["A102"].ToString()));
                        txtQ102Scr.Text = Convert.ToString(dt.Rows[0]["S102"]);
                        txtRem102.Text = Convert.ToString(dt.Rows[0]["R102"]);

                        ddlQ27.SelectedIndex = ddlQ27.Items.IndexOf(ddlQ27.Items.FindByValue(dt.Rows[0]["A27"].ToString()));
                        txtQ27Scr.Text = Convert.ToString(dt.Rows[0]["S27"]);
                        txtRem27.Text = Convert.ToString(dt.Rows[0]["R27"]);
                        ddlQ28.SelectedIndex = ddlQ28.Items.IndexOf(ddlQ28.Items.FindByValue(dt.Rows[0]["A28"].ToString()));
                        txtQ28Scr.Text = Convert.ToString(dt.Rows[0]["S28"]);
                        txtRem28.Text = Convert.ToString(dt.Rows[0]["R28"]);
                        ddlQ29.SelectedIndex = ddlQ29.Items.IndexOf(ddlQ29.Items.FindByValue(dt.Rows[0]["A29"].ToString()));
                        txtQ29Scr.Text = Convert.ToString(dt.Rows[0]["S29"]);
                        txtRem29.Text = Convert.ToString(dt.Rows[0]["R29"]);
                        ddlQ30.SelectedIndex = ddlQ30.Items.IndexOf(ddlQ30.Items.FindByValue(dt.Rows[0]["A30"].ToString()));
                        txtQ30Scr.Text = Convert.ToString(dt.Rows[0]["S30"]);
                        txtRem30.Text = Convert.ToString(dt.Rows[0]["R30"]);
                        ddlQ31.SelectedIndex = ddlQ31.Items.IndexOf(ddlQ31.Items.FindByValue(dt.Rows[0]["A31"].ToString()));
                        txtQ31Scr.Text = Convert.ToString(dt.Rows[0]["S31"]);
                        txtRem31.Text = Convert.ToString(dt.Rows[0]["R31"]);
                        ddlQ32.SelectedIndex = ddlQ32.Items.IndexOf(ddlQ32.Items.FindByValue(dt.Rows[0]["A32"].ToString()));
                        txtQ32Scr.Text = Convert.ToString(dt.Rows[0]["S32"]);
                        txtRem32.Text = Convert.ToString(dt.Rows[0]["R32"]);
                        ddlQ33.SelectedIndex = ddlQ33.Items.IndexOf(ddlQ33.Items.FindByValue(dt.Rows[0]["A33"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S33"]) == "-1")
                            txtQ33Scr.Text = "N/A";
                        else
                            txtQ33Scr.Text = Convert.ToString(dt.Rows[0]["S33"]);

                        txtRem33.Text = Convert.ToString(dt.Rows[0]["R33"]);
                        ddlQ34.SelectedIndex = ddlQ34.Items.IndexOf(ddlQ34.Items.FindByValue(dt.Rows[0]["A34"].ToString()));                        
                        txtQ34Scr.Text = Convert.ToString(dt.Rows[0]["S34"]);
                        txtRem34.Text = Convert.ToString(dt.Rows[0]["R34"]);
                        ddlQ35.SelectedIndex = ddlQ35.Items.IndexOf(ddlQ35.Items.FindByValue(dt.Rows[0]["A35"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S35"]) == "-1")
                            txtQ35Scr.Text = "N/A";
                        else
                            txtQ35Scr.Text = Convert.ToString(dt.Rows[0]["S35"]);
                        txtRem35.Text = Convert.ToString(dt.Rows[0]["R35"]);
                        ddlQ36.SelectedIndex = ddlQ36.Items.IndexOf(ddlQ36.Items.FindByValue(dt.Rows[0]["A36"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S36"]) == "-1")
                            txtQ36Scr.Text = "N/A";
                        else
                            txtQ36Scr.Text = Convert.ToString(dt.Rows[0]["S36"]);
                        txtRem36.Text = Convert.ToString(dt.Rows[0]["R36"]);
                        ddlQ37.SelectedIndex = ddlQ37.Items.IndexOf(ddlQ37.Items.FindByValue(dt.Rows[0]["A37"].ToString()));
                        txtQ37Scr.Text = Convert.ToString(dt.Rows[0]["S37"]);
                        txtRem37.Text = Convert.ToString(dt.Rows[0]["R37"]);
                        ddlQ38.SelectedIndex = ddlQ38.Items.IndexOf(ddlQ38.Items.FindByValue(dt.Rows[0]["A38"].ToString()));
                        txtQ38Scr.Text = Convert.ToString(dt.Rows[0]["S38"]);
                        txtRem38.Text = Convert.ToString(dt.Rows[0]["R38"]);
                        ddlQ39.SelectedIndex = ddlQ39.Items.IndexOf(ddlQ39.Items.FindByValue(dt.Rows[0]["A39"].ToString()));
                        txtQ39Scr.Text = Convert.ToString(dt.Rows[0]["S39"]);
                        txtRem39.Text = Convert.ToString(dt.Rows[0]["R39"]);
                        ddlQ40.SelectedIndex = ddlQ40.Items.IndexOf(ddlQ40.Items.FindByValue(dt.Rows[0]["A40"].ToString()));
                        txtQ40Scr.Text = Convert.ToString(dt.Rows[0]["S40"]);
                        txtRem40.Text = Convert.ToString(dt.Rows[0]["R40"]);
                        ddlQ301.SelectedIndex = ddlQ301.Items.IndexOf(ddlQ301.Items.FindByValue(dt.Rows[0]["A41"].ToString()));
                        txtQ301scr.Text = Convert.ToString(dt.Rows[0]["S41"]);
                        txtRem301.Text = Convert.ToString(dt.Rows[0]["R41"]);
                        ddlQ42.SelectedIndex = ddlQ42.Items.IndexOf(ddlQ42.Items.FindByValue(dt.Rows[0]["A42"].ToString()));
                        txtQ42Scr.Text = Convert.ToString(dt.Rows[0]["S42"]);
                        txtRem42.Text = Convert.ToString(dt.Rows[0]["R42"]);
                        ddlQ43.SelectedIndex = ddlQ43.Items.IndexOf(ddlQ43.Items.FindByValue(dt.Rows[0]["A43"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S43"]) == "-1")
                            txtQ43Scr.Text = "N/A";
                        else
                            txtQ43Scr.Text = Convert.ToString(dt.Rows[0]["S43"]);
                        txtRem43.Text = Convert.ToString(dt.Rows[0]["R43"]);
                        ddlQ44.SelectedIndex = ddlQ44.Items.IndexOf(ddlQ44.Items.FindByValue(dt.Rows[0]["A44"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S44"]) == "-1")
                            txtQ44Scr.Text = "N/A";
                        else
                            txtQ44Scr.Text = Convert.ToString(dt.Rows[0]["S44"]);
                        txtRem44.Text = Convert.ToString(dt.Rows[0]["R44"]);
                        ddlQ45.SelectedIndex = ddlQ45.Items.IndexOf(ddlQ45.Items.FindByValue(dt.Rows[0]["A45"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S45"]) == "-1")
                            txtQ45Scr.Text = "N/A";
                        else
                            txtQ45Scr.Text = Convert.ToString(dt.Rows[0]["S45"]);
                        txtRem45.Text = Convert.ToString(dt.Rows[0]["R45"]);
                        ddlQ46.SelectedIndex = ddlQ46.Items.IndexOf(ddlQ46.Items.FindByValue(dt.Rows[0]["A46"].ToString()));
                        txtQ46Scr.Text = Convert.ToString(dt.Rows[0]["S46"]);
                        txtRem46.Text = Convert.ToString(dt.Rows[0]["R46"]);

                        ddlQ47.SelectedIndex = ddlQ47.Items.IndexOf(ddlQ47.Items.FindByValue(dt.Rows[0]["A47"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S47"]) == "-1")
                            txtQ47Scr.Text = "N/A";
                        else
                            txtQ47Scr.Text = Convert.ToString(dt.Rows[0]["S47"]);
                        txtRem47.Text = Convert.ToString(dt.Rows[0]["R47"]);

                        //////////////////////
                        ddlQ48.SelectedIndex = ddlQ48.Items.IndexOf(ddlQ48.Items.FindByValue(dt.Rows[0]["A48"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S48"]) == "-1")
                            txtQ48Scr.Text = "N/A";
                        else
                            txtQ48Scr.Text = Convert.ToString(dt.Rows[0]["S48"]);
                        txtRem48.Text = Convert.ToString(dt.Rows[0]["R48"]);

                        ddlQ49.SelectedIndex = ddlQ49.Items.IndexOf(ddlQ49.Items.FindByValue(dt.Rows[0]["A49"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S49"]) == "-1")
                            txtQ49Scr.Text = "N/A";
                        else
                            txtQ49Scr.Text = Convert.ToString(dt.Rows[0]["S49"]);
                        txtRem49.Text = Convert.ToString(dt.Rows[0]["R49"]);

                        ddlQ50.SelectedIndex = ddlQ50.Items.IndexOf(ddlQ50.Items.FindByValue(dt.Rows[0]["A50"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["S50"]) == "-1")
                            txtQ50Scr.Text = "N/A";
                        else
                            txtQ50Scr.Text = Convert.ToString(dt.Rows[0]["S50"]);
                        txtRem50.Text = Convert.ToString(dt.Rows[0]["R50"]);

                        ddlQ201.SelectedIndex = ddlQ201.Items.IndexOf(ddlQ201.Items.FindByValue(dt.Rows[0]["A201"].ToString()));
                        txtQ201Scr.Text = Convert.ToString(dt.Rows[0]["S201"]);
                        txtRem201.Text = Convert.ToString(dt.Rows[0]["R201"]);

                        ddlQ202.SelectedIndex = ddlQ202.Items.IndexOf(ddlQ202.Items.FindByValue(dt.Rows[0]["A202"].ToString()));
                        txtQ202Scr.Text = Convert.ToString(dt.Rows[0]["S202"]);
                        txtRem202.Text = Convert.ToString(dt.Rows[0]["R202"]);

                        ddlQ203.SelectedIndex = ddlQ203.Items.IndexOf(ddlQ203.Items.FindByValue(dt.Rows[0]["A203"].ToString()));
                        txtQ203Scr.Text = Convert.ToString(dt.Rows[0]["S203"]);
                        txtRem203.Text = Convert.ToString(dt.Rows[0]["R203"]);

                        ddlQ204.SelectedIndex = ddlQ204.Items.IndexOf(ddlQ204.Items.FindByValue(dt.Rows[0]["A204"].ToString()));
                        txtQ204Scr.Text = Convert.ToString(dt.Rows[0]["S204"]);
                        txtRem204.Text = Convert.ToString(dt.Rows[0]["R204"]);

                        ddlQ205.SelectedIndex = ddlQ205.Items.IndexOf(ddlQ205.Items.FindByValue(dt.Rows[0]["A205"].ToString()));
                        txtQ205Scr.Text = Convert.ToString(dt.Rows[0]["S205"]);
                        txtRem205.Text = Convert.ToString(dt.Rows[0]["R205"]);

                        ddlQ206.SelectedIndex = ddlQ206.Items.IndexOf(ddlQ206.Items.FindByValue(dt.Rows[0]["A206"].ToString()));
                        txtQ206Scr.Text = Convert.ToString(dt.Rows[0]["S206"]);
                        txtRem206.Text = Convert.ToString(dt.Rows[0]["R206"]);

                        ddlQ207.SelectedIndex = ddlQ207.Items.IndexOf(ddlQ207.Items.FindByValue(dt.Rows[0]["A207"].ToString()));
                        txtQ207Scr.Text = Convert.ToString(dt.Rows[0]["S207"]);
                        txtRem207.Text = Convert.ToString(dt.Rows[0]["R207"]);

                        ddlQ208.SelectedIndex = ddlQ208.Items.IndexOf(ddlQ208.Items.FindByValue(dt.Rows[0]["A208"].ToString()));
                        txtQ208Scr.Text = Convert.ToString(dt.Rows[0]["S208"]);
                        txtRem208.Text = Convert.ToString(dt.Rows[0]["R208"]);

                        ddlQ209.SelectedIndex = ddlQ209.Items.IndexOf(ddlQ209.Items.FindByValue(dt.Rows[0]["A209"].ToString()));
                        txtQ209Scr.Text = Convert.ToString(dt.Rows[0]["S209"]);
                        txtRem209.Text = Convert.ToString(dt.Rows[0]["R209"]);
                        /////////////////
                        txtRem51.Text = Convert.ToString(dt.Rows[0]["R51"]);
                        txtRem52.Text = Convert.ToString(dt.Rows[0]["R52"]);

                        txtIre1.Text = Convert.ToString(dt.Rows[0]["IR1"]);
                        txtIre2.Text = Convert.ToString(dt.Rows[0]["IR2"]);
                        txtIre3.Text = Convert.ToString(dt.Rows[0]["IR3"]);
                        txtIre4.Text = Convert.ToString(dt.Rows[0]["IR4"]);
                        txtIre5.Text = Convert.ToString(dt.Rows[0]["IR5"]);
                        txtIre6.Text = Convert.ToString(dt.Rows[0]["IR6"]);
                        txtIre7.Text = Convert.ToString(dt.Rows[0]["IR7"]);
                        txtIre8.Text = Convert.ToString(dt.Rows[0]["IR8"]);
                        txtIre9.Text = Convert.ToString(dt.Rows[0]["IR9"]);
                        txtIre10.Text = Convert.ToString(dt.Rows[0]["IR10"]);
                        txtIre11.Text = Convert.ToString(dt.Rows[0]["IR11"]);
                        txtIre12.Text = Convert.ToString(dt.Rows[0]["IR12"]);
                        txtIre13.Text = Convert.ToString(dt.Rows[0]["IR13"]);

                        txtIre101.Text = Convert.ToString(dt.Rows[0]["IR101"]);

                        txtIre14.Text = Convert.ToString(dt.Rows[0]["IR14"]);
                        txtIre15.Text = Convert.ToString(dt.Rows[0]["IR15"]);
                        txtIre16.Text = Convert.ToString(dt.Rows[0]["IR16"]);
                        txtIre17.Text = Convert.ToString(dt.Rows[0]["IR17"]);
                        txtIre18.Text = Convert.ToString(dt.Rows[0]["IR18"]);
                        txtIre19.Text = Convert.ToString(dt.Rows[0]["IR19"]);
                        txtIre20.Text = Convert.ToString(dt.Rows[0]["IR20"]);

                        txtIre100.Text = Convert.ToString(dt.Rows[0]["IR100"]);


                        txtIre103.Text = Convert.ToString(dt.Rows[0]["IR103"]);



                        txtIre21.Text = Convert.ToString(dt.Rows[0]["IR21"]);
                        txtIre22.Text = Convert.ToString(dt.Rows[0]["IR22"]);
                        txtIre23.Text = Convert.ToString(dt.Rows[0]["IR23"]);
                        txtIre24.Text = Convert.ToString(dt.Rows[0]["IR24"]);
                        txtIre25.Text = Convert.ToString(dt.Rows[0]["IR25"]);
                        txtIre26.Text = Convert.ToString(dt.Rows[0]["IR26"]);

                        txtIre102.Text = Convert.ToString(dt.Rows[0]["IR102"]);

                        txtIre27.Text = Convert.ToString(dt.Rows[0]["IR27"]);
                        txtIre28.Text = Convert.ToString(dt.Rows[0]["IR28"]);
                        txtIre29.Text = Convert.ToString(dt.Rows[0]["IR29"]);
                        txtIre30.Text = Convert.ToString(dt.Rows[0]["IR30"]);
                        txtIre31.Text = Convert.ToString(dt.Rows[0]["IR31"]);
                        txtIre32.Text = Convert.ToString(dt.Rows[0]["IR32"]);
                        txtIre33.Text = Convert.ToString(dt.Rows[0]["IR33"]);
                        txtIre34.Text = Convert.ToString(dt.Rows[0]["IR34"]);
                        txtIre35.Text = Convert.ToString(dt.Rows[0]["IR35"]);
                        txtIre36.Text = Convert.ToString(dt.Rows[0]["IR36"]);
                        txtIre37.Text = Convert.ToString(dt.Rows[0]["IR37"]);
                        txtIre38.Text = Convert.ToString(dt.Rows[0]["IR38"]);
                        txtIre39.Text = Convert.ToString(dt.Rows[0]["IR39"]);
                        txtIre40.Text = Convert.ToString(dt.Rows[0]["IR40"]);
                        txtIre41.Text = Convert.ToString(dt.Rows[0]["IR41"]);
                        txtIre42.Text = Convert.ToString(dt.Rows[0]["IR42"]);
                        txtIre43.Text = Convert.ToString(dt.Rows[0]["IR43"]);
                        txtIre44.Text = Convert.ToString(dt.Rows[0]["IR44"]);
                        txtIre45.Text = Convert.ToString(dt.Rows[0]["IR45"]);
                        txtIre46.Text = Convert.ToString(dt.Rows[0]["IR46"]);
                        txtIre47.Text = Convert.ToString(dt.Rows[0]["IR47"]);

                        txtIre48.Text = Convert.ToString(dt.Rows[0]["IR48"]);
                        txtIre49.Text = Convert.ToString(dt.Rows[0]["IR49"]);
                        txtIre50.Text = Convert.ToString(dt.Rows[0]["IR50"]);

                        txtIre51.Text = Convert.ToString(dt.Rows[0]["IR51"]);
                        txtIre52.Text = Convert.ToString(dt.Rows[0]["IR52"]);
                        txtIre201.Text = Convert.ToString(dt.Rows[0]["IR201"]);
                        txtIre202.Text = Convert.ToString(dt.Rows[0]["IR202"]);
                        txtIre203.Text = Convert.ToString(dt.Rows[0]["IR203"]);
                        txtIre204.Text = Convert.ToString(dt.Rows[0]["IR204"]);
                        txtIre205.Text = Convert.ToString(dt.Rows[0]["IR205"]);
                        txtIre206.Text = Convert.ToString(dt.Rows[0]["IR206"]);
                        txtIre207.Text = Convert.ToString(dt.Rows[0]["IR207"]);
                        txtIre208.Text = Convert.ToString(dt.Rows[0]["IR208"]);
                        txtIre209.Text = Convert.ToString(dt.Rows[0]["IR209"]);
                        txtOther.Text = Convert.ToString(dt.Rows[0]["Other"]);
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
                oAu = null;
            }
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {            
            Int32 vS1 = 0, vS2 = 0, vS3 = 0, vS4 = 0, vS5 = 0, vS6 = 0, vS7 = 0, vS8 = 0, vS9 = 0, vS10 = 0, vS11 = 0;
            Int32 vS12 = 0, vS13 = 0, vS101 = 0, vS14 = 0, vS15 = 0, vS16 = 0, vS17 = 0, vS18 = 0, vS19 = 0, vS20 = 0, vS100 = 0, vS103 = 0, vS21 = 0, vS22 = 0;
            Int32 vS23 = 0, vS24 = 0, vS25 = 0, vS26 = 0, vS102=0, vS27 = 0, vS28 = 0, vS29 = 0, vS30 = 0, vS31 = 0, vS32 = 0;
            Int32 vS33 = 0, vS34 = 0, vS35 = 0, vS36 = 0, vS37 = 0, vS38 = 0, vS39 = 0, vS40 = 0, vS41 = 0, vS42 = 0;
            Int32 vS43 = 0, vS44 = 0, vS45 = 0, vS46 = 0, vS47 = 0, vS48 = 0, vS49 = 0, vS50 = 0, vS201 = 0, vS202 = 0, vS203 = 0, vS204 = 0, vS205 = 0;
            Int32 vS206 = 0, vS207 = 0, vS208 = 0, vS209 = 0;

            Int32 vIR1 = 0, vIR2 = 0, vIR3 = 0, vIR4 = 0, vIR5 = 0, vIR6 = 0, vIR7 = 0, vIR8 = 0, vIR9 = 0, vIR10 = 0;
            Int32 vIR11 = 0, vIR12 = 0, vIR13 = 0, vIR101=0, vIR14 = 0, vIR15 = 0, vIR16 = 0, vIR17 = 0, vIR18 = 0, vIR19 = 0;
            Int32 vIR20 = 0, vIR100 = 0, vIR103 = 0, vIR21 = 0, vIR22 = 0, vIR23 = 0, vIR24 = 0, vIR25 = 0, vIR26 = 0, vIR102 = 0, vIR27 = 0, vIR28 = 0;
            Int32 vIR29 = 0, vIR30 = 0, vIR31 = 0, vIR32 = 0, vIR33 = 0, vIR34 = 0, vIR35 = 0, vIR36 = 0, vIR37 = 0;
            Int32 vIR38 = 0, vIR39 = 0, vIR40 = 0, vIR41 = 0, vIR42 = 0, vIR43 = 0, vIR44 = 0, vIR45 = 0, vIR46 = 0;
            Int32 vIR47 = 0, vIR48 = 0, vIR49 = 0, vIR50 = 0, vIR51 = 0, vIR52 = 0, vIR201 = 0, vIR202 = 0, vIR203 = 0, vIR204 = 0, vIR205 = 0;
            Int32 vIR206 = 0, vIR207 = 0, vIR208 = 0, vIR209 = 0;
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vInsPecId = 0, vErr = 0, vUserID=0;            
            DateTime vSubDt = gblFuction.setDate(txtSDt.Text);
            DateTime vCrStrDt = gblFuction.setDate(txtCrStrDt.Text);
            DateTime vCrEndDt = gblFuction.setDate(txtCrEndDt.Text);
            DateTime vPrStrDt = gblFuction.setDate(txtPrStrDt.Text);            
            DateTime vPrEndDt = gblFuction.setDate(txtPrEndDt.Text);
            TimeSpan vTsp = (vSubDt - vPrStrDt);
            CIntIspPM oAu = null;

            try
            {
                 vUserID = Int32.Parse(Session[gblValue.UserId].ToString()); 
                 if (vCrStrDt >= vSubDt) 
                 {
                     gblFuction.MsgPopup("Carried out Start Date should be earlier date of Submission Date.");
                    return false;
                 }
                 if (vCrEndDt > vSubDt)
                 {
                    gblFuction.MsgPopup("Carried out End Date should be earlier date of Submission Date.");
                    return false;
                 }
                 if (vTsp.Days < 30)
                 {
                     gblFuction.MsgPopup("Days difference of Submission Date and Period Start Date should be atlest 30 days.");
                     return false;
                 }
                 if (vPrEndDt >= vSubDt)
                 {
                     gblFuction.MsgPopup("Period End Date should be earlier date of Submission Date.");
                     return false;
                 }                 


                if (ViewState["InsPecId"] != null) vInsPecId = Convert.ToInt32(Convert.ToString(ViewState["InsPecId"]));
                if (txtQ1Scr.Text != "") vS1 = Convert.ToInt32(txtQ1Scr.Text);
                if (txtQ2Scr.Text != "") vS2 = Convert.ToInt32(txtQ2Scr.Text);
                if (txtQ3Scr.Text != "") vS3 = Convert.ToInt32(txtQ3Scr.Text);
                if (txtQ4Scr.Text != "N/A") 
                    vS4 = Convert.ToInt32(txtQ4Scr.Text);
                else if (txtQ4Scr.Text == "N/A")
                    vS4 = -1;
                if (txtQ5Scr.Text != "N/A") 
                    vS5 = Convert.ToInt32(txtQ5Scr.Text);
                else if (txtQ5Scr.Text == "N/A")
                    vS5 = -1;
                if (txtQ6Scr.Text != "") vS6 = Convert.ToInt32(txtQ6Scr.Text);
                if (txtQ7Scr.Text != "") vS7 = Convert.ToInt32(txtQ7Scr.Text);
                if (txtQ8Scr.Text != "") vS8 = Convert.ToInt32(txtQ8Scr.Text);
                if (txtQ9Scr.Text != "") vS9 = Convert.ToInt32(txtQ9Scr.Text);
                if (txtQ10Scr.Text != "") vS10 = Convert.ToInt32(txtQ10Scr.Text);
                if (txtQ11Scr.Text != "") vS11 = Convert.ToInt32(txtQ11Scr.Text);
                if (txtQ12Scr.Text != "") vS12 = Convert.ToInt32(txtQ12Scr.Text);
                if (txtQ13Scr.Text != "") vS13 = Convert.ToInt32(txtQ13Scr.Text);

                if (txtQ101Scr.Text != "") vS101 = Convert.ToInt32(txtQ101Scr.Text);

                if (txtQ14Scr.Text != "") vS14 = Convert.ToInt32(txtQ14Scr.Text);
                if (txtQ15Scr.Text != "N/A") 
                    vS15 = Convert.ToInt32(txtQ15Scr.Text);
                else if (txtQ15Scr.Text == "N/A")
                    vS15 = -1;

                if (txtQ16Scr.Text != "N/A")
                    vS16 = Convert.ToInt32(txtQ16Scr.Text);
                else if (txtQ16Scr.Text == "N/A")
                    vS16 = -1;
                //if (txtQ16Scr.Text != "") vS16 = Convert.ToInt32(txtQ16Scr.Text);

                if (txtQ17Scr.Text != "N/A")
                    vS17 = Convert.ToInt32(txtQ17Scr.Text);
                else if (txtQ17Scr.Text == "N/A")
                    vS17 = -1;
                
                if (txtQ18Scr.Text != "N/A")
                    vS18 = Convert.ToInt32(txtQ18Scr.Text);
                else if (txtQ18Scr.Text == "N/A")
                    vS18 = -1;
                
                if (txtQ19Scr.Text != "N/A")
                    vS19 = Convert.ToInt32(txtQ19Scr.Text);
                else if (txtQ19Scr.Text == "N/A")
                    vS19 = -1;
                
                if (txtQ20Scr.Text != "N/A")
                    vS20 = Convert.ToInt32(txtQ20Scr.Text);
                else if (txtQ20Scr.Text == "N/A")
                    vS20 = -1;

                if (txtQ100Scr.Text != "") vS100 = Convert.ToInt32(txtQ100Scr.Text);

                if (txtQ103Scr.Text != "") vS103 = Convert.ToInt32(txtQ103Scr.Text);


                if (txtQ21Scr.Text != "") vS21 = Convert.ToInt32(txtQ21Scr.Text);
                
                if (txtQ22Scr.Text != "N/A")
                    vS22 = Convert.ToInt32(txtQ22Scr.Text);
                else if (txtQ22Scr.Text == "N/A")
                    vS22 = -1;
                
                if (txtQ23Scr.Text != "") vS23 = Convert.ToInt32(txtQ23Scr.Text);
                if (txtQ24Scr.Text != "") vS24 = Convert.ToInt32(txtQ24Scr.Text);
                if (txtQ25Scr.Text != "") vS25 = Convert.ToInt32(txtQ25Scr.Text);
                if (txtQ26Scr.Text != "") vS26 = Convert.ToInt32(txtQ26Scr.Text);

                if (txtQ102Scr.Text != "") vS102 = Convert.ToInt32(txtQ102Scr.Text);

                if (txtQ27Scr.Text != "") vS27 = Convert.ToInt32(txtQ27Scr.Text);
                if (txtQ28Scr.Text != "") vS28 = Convert.ToInt32(txtQ28Scr.Text);
                if (txtQ29Scr.Text != "") vS29 = Convert.ToInt32(txtQ29Scr.Text);
                if (txtQ30Scr.Text != "") vS30 = Convert.ToInt32(txtQ30Scr.Text);
                if (txtQ31Scr.Text != "") vS31 = Convert.ToInt32(txtQ31Scr.Text);
                if (txtQ32Scr.Text != "") vS32 = Convert.ToInt32(txtQ32Scr.Text);

                if (txtQ33Scr.Text != "N/A")
                    vS33 = Convert.ToInt32(txtQ33Scr.Text);
                else
                    vS33 = -1;
                
                if (txtQ34Scr.Text != "") vS34 = Convert.ToInt32(txtQ34Scr.Text);
                
                if (txtQ35Scr.Text != "N/A")
                    vS35 = Convert.ToInt32(txtQ35Scr.Text);
                else if (txtQ35Scr.Text == "N/A")
                    vS35 = -1;
                
                if (txtQ36Scr.Text != "N/A")
                    vS36 = Convert.ToInt32(txtQ36Scr.Text);
                else if (txtQ36Scr.Text == "N/A")
                    vS36 = -1;
                
                if (txtQ37Scr.Text != "") vS37 = Convert.ToInt32(txtQ37Scr.Text);
                if (txtQ38Scr.Text != "") vS38 = Convert.ToInt32(txtQ38Scr.Text);
                if (txtQ39Scr.Text != "") vS39 = Convert.ToInt32(txtQ39Scr.Text);
                if (txtQ40Scr.Text != "") vS40 = Convert.ToInt32(txtQ40Scr.Text);
                if (txtQ301scr.Text != "") vS41 = Convert.ToInt32(txtQ301scr.Text);
                if (txtQ42Scr.Text != "") vS42 = Convert.ToInt32(txtQ42Scr.Text);
                
                if (txtQ43Scr.Text != "N/A")
                    vS43 = Convert.ToInt32(txtQ43Scr.Text);
                else if (txtQ43Scr.Text == "N/A")
                    vS43 = -1;
                
                if (txtQ44Scr.Text != "N/A")
                    vS44 = Convert.ToInt32(txtQ44Scr.Text);
                else if (txtQ44Scr.Text == "N/A")
                    vS44 = -1;

                if (txtQ45Scr.Text != "N/A")
                    vS45 = Convert.ToInt32(txtQ45Scr.Text);
                else if (txtQ45Scr.Text == "N/A")
                    vS45 = -1;
                
                if (txtQ46Scr.Text != "")  vS46 = Convert.ToInt32(txtQ46Scr.Text);
                
                if (txtQ47Scr.Text != "N/A")
                    vS47 = Convert.ToInt32(txtQ47Scr.Text);
                else if (txtQ47Scr.Text == "N/A")
                    vS47 = -1;

                if (txtQ48Scr.Text != "N/A")
                    vS48 = Convert.ToInt32(txtQ48Scr.Text);
                else if (txtQ48Scr.Text == "N/A")
                    vS48 = -1;
                
                if (txtQ49Scr.Text != "N/A")
                    vS49 = Convert.ToInt32(txtQ49Scr.Text);
                else if (txtQ49Scr.Text == "N/A")
                    vS49 = -1;
                
                if (txtQ50Scr.Text != "N/A")
                    vS50 = Convert.ToInt32(txtQ50Scr.Text);
                else if (txtQ50Scr.Text == "N/A")
                    vS50 = -1;
                if (txtQ201Scr.Text != "") vS201 = Convert.ToInt32(txtQ201Scr.Text);
                if (txtQ202Scr.Text != "") vS202 = Convert.ToInt32(txtQ202Scr.Text);
                if (txtQ203Scr.Text != "") vS203 = Convert.ToInt32(txtQ203Scr.Text);
                if (txtQ204Scr.Text != "") vS204 = Convert.ToInt32(txtQ204Scr.Text);
                if (txtQ205Scr.Text != "") vS205 = Convert.ToInt32(txtQ205Scr.Text);
                if (txtQ206Scr.Text != "") vS206 = Convert.ToInt32(txtQ206Scr.Text);
                if (txtQ207Scr.Text != "") vS207 = Convert.ToInt32(txtQ207Scr.Text);
                if (txtQ208Scr.Text != "") vS208 = Convert.ToInt32(txtQ208Scr.Text);
                if (txtQ209Scr.Text != "") vS209 = Convert.ToInt32(txtQ209Scr.Text);


                if (txtIre1.Text != "") vIR1 = Convert.ToInt32(txtIre1.Text);
                if (txtIre2.Text != "") vIR2 = Convert.ToInt32(txtIre2.Text);
                if (txtIre3.Text != "") vIR3 = Convert.ToInt32(txtIre3.Text);
                if (txtIre4.Text != "") vIR4 = Convert.ToInt32(txtIre4.Text);
                if (txtIre5.Text != "") vIR5 = Convert.ToInt32(txtIre5.Text);
                if (txtIre6.Text != "") vIR6 = Convert.ToInt32(txtIre6.Text);
                if (txtIre7.Text != "") vIR7 = Convert.ToInt32(txtIre7.Text);
                if (txtIre8.Text != "") vIR8 = Convert.ToInt32(txtIre8.Text);
                if (txtIre9.Text != "") vIR9 = Convert.ToInt32(txtIre9.Text);
                if (txtIre10.Text != "") vIR10 = Convert.ToInt32(txtIre10.Text);
                if (txtIre11.Text != "") vIR11 = Convert.ToInt32(txtIre11.Text);
                if (txtIre12.Text != "") vIR12 = Convert.ToInt32(txtIre12.Text);
                if (txtIre13.Text != "") vIR13 = Convert.ToInt32(txtIre13.Text);

                if (txtIre101.Text != "") vIR101 = Convert.ToInt32(txtIre101.Text);

                if (txtIre14.Text != "") vIR14 = Convert.ToInt32(txtIre14.Text);
                if (txtIre15.Text != "") vIR15 = Convert.ToInt32(txtIre15.Text);
                if (txtIre16.Text != "") vIR16 = Convert.ToInt32(txtIre16.Text);
                if (txtIre17.Text != "") vIR17 = Convert.ToInt32(txtIre17.Text);
                if (txtIre18.Text != "") vIR18 = Convert.ToInt32(txtIre18.Text);
                if (txtIre19.Text != "") vIR19 = Convert.ToInt32(txtIre19.Text);
                if (txtIre20.Text != "") vIR20 = Convert.ToInt32(txtIre20.Text);

                if (txtIre100.Text != "") vIR100 = Convert.ToInt32(txtIre100.Text);

                if (txtIre103.Text != "") vIR103 = Convert.ToInt32(txtIre103.Text);

                if (txtIre21.Text != "") vIR21 = Convert.ToInt32(txtIre21.Text);
                if (txtIre22.Text != "") vIR22 = Convert.ToInt32(txtIre22.Text);
                if (txtIre23.Text != "") vIR23 = Convert.ToInt32(txtIre23.Text);
                if (txtIre24.Text != "") vIR24 = Convert.ToInt32(txtIre24.Text);
                if (txtIre25.Text != "") vIR25 = Convert.ToInt32(txtIre25.Text);
                if (txtIre26.Text != "") vIR26 = Convert.ToInt32(txtIre26.Text);

                if (txtIre102.Text != "") vIR102 = Convert.ToInt32(txtIre102.Text);

                if (txtIre27.Text != "") vIR27 = Convert.ToInt32(txtIre27.Text);
                if (txtIre28.Text != "") vIR28 = Convert.ToInt32(txtIre28.Text);
                if (txtIre29.Text != "") vIR29 = Convert.ToInt32(txtIre29.Text);
                if (txtIre30.Text != "") vIR30 = Convert.ToInt32(txtIre30.Text);
                if (txtIre31.Text != "") vIR31 = Convert.ToInt32(txtIre31.Text);
                if (txtIre32.Text != "") vIR32 = Convert.ToInt32(txtIre32.Text);
                if (txtIre33.Text != "") vIR33 = Convert.ToInt32(txtIre33.Text);
                if (txtIre34.Text != "") vIR34 = Convert.ToInt32(txtIre34.Text);
                if (txtIre35.Text != "") vIR35 = Convert.ToInt32(txtIre35.Text);
                if (txtIre36.Text != "") vIR36 = Convert.ToInt32(txtIre36.Text);
                if (txtIre37.Text != "") vIR37 = Convert.ToInt32(txtIre37.Text);
                if (txtIre38.Text != "") vIR38 = Convert.ToInt32(txtIre38.Text);
                if (txtIre39.Text != "") vIR39 = Convert.ToInt32(txtIre39.Text);
                if (txtIre40.Text != "") vIR40 = Convert.ToInt32(txtIre40.Text);
                if (txtIre41.Text != "") vIR41 = Convert.ToInt32(txtIre41.Text);
                if (txtIre42.Text != "") vIR42 = Convert.ToInt32(txtIre42.Text);
                if (txtIre43.Text != "") vIR43 = Convert.ToInt32(txtIre43.Text);
                if (txtIre44.Text != "") vIR44 = Convert.ToInt32(txtIre44.Text);
                if (txtIre45.Text != "") vIR45 = Convert.ToInt32(txtIre45.Text);
                if (txtIre46.Text != "") vIR46 = Convert.ToInt32(txtIre46.Text);
                if (txtIre47.Text != "") vIR47 = Convert.ToInt32(txtIre47.Text);
                if (txtIre48.Text != "") vIR48 = Convert.ToInt32(txtIre48.Text);
                if (txtIre49.Text != "") vIR49 = Convert.ToInt32(txtIre49.Text);
                if (txtIre50.Text != "") vIR50 = Convert.ToInt32(txtIre50.Text);
                if (txtIre51.Text != "") vIR51 = Convert.ToInt32(txtIre51.Text);
                if (txtIre52.Text != "") vIR52 = Convert.ToInt32(txtIre52.Text);
                if (txtIre201.Text != "") vIR201 = Convert.ToInt32(txtIre201.Text);
                if (txtIre202.Text != "") vIR202 = Convert.ToInt32(txtIre202.Text);
                if (txtIre203.Text != "") vIR203 = Convert.ToInt32(txtIre203.Text);
                if (txtIre204.Text != "") vIR204 = Convert.ToInt32(txtIre204.Text);
                if (txtIre205.Text != "") vIR205 = Convert.ToInt32(txtIre205.Text);
                if (txtIre206.Text != "") vIR206 = Convert.ToInt32(txtIre206.Text);
                if (txtIre207.Text != "") vIR207 = Convert.ToInt32(txtIre207.Text);
                if (txtIre208.Text != "") vIR208 = Convert.ToInt32(txtIre208.Text);
                if (txtIre209.Text != "") vIR209 = Convert.ToInt32(txtIre209.Text);

                if (Mode == "Save")
                {
                    oAu = new CIntIspPM();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("EOMst", "EmpCode", txtEmpCode.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Save");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("EMP Code can not be Duplicate...");
                    //    return false;
                    //}

                    vErr = oAu.SaveIntInspPM(ref vInsPecId, "PM", vSubDt, vCrStrDt, vCrEndDt, vPrStrDt, vPrEndDt, ddlBranch.SelectedValue, vUserID,
                               Convert.ToInt32(ddlQ1.SelectedValue), vS1, txtRem1.Text.Replace("'", "''"), Convert.ToInt32(ddlQ2.SelectedValue), vS2, txtRem2.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ3.SelectedValue), vS3, txtRem3.Text.Replace("'", "''"), Convert.ToInt32(ddlQ4.SelectedValue), vS4, txtRem4.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ5.SelectedValue), vS5, txtRem5.Text.Replace("'", "''"), Convert.ToInt32(ddlQ6.SelectedValue), vS6, txtRem6.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ7.SelectedValue), vS7, txtRem7.Text.Replace("'", "''"), Convert.ToInt32(ddlQ8.SelectedValue), vS8, txtRem8.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ9.SelectedValue), vS9, txtRem9.Text.Replace("'", "''"), Convert.ToInt32(ddlQ10.SelectedValue), vS10, txtRem10.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ11.SelectedValue), vS11, txtRem11.Text.Replace("'", "''"), Convert.ToInt32(ddlQ12.SelectedValue), vS12, txtRem12.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ13.SelectedValue), vS13, txtRem13.Text.Replace("'", "''"), Convert.ToInt32(ddlQ101.SelectedValue), vS101, txtRem101.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ14.SelectedValue), vS14, txtRem14.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ15.SelectedValue), vS15, txtRem15.Text.Replace("'", "''"), Convert.ToInt32(ddlQ16.SelectedValue), vS16, txtRem16.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ17.SelectedValue), 0, txtRem17.Text.Replace("'", "''"), Convert.ToInt32(ddlQ18.SelectedValue), vS18, txtRem18.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ19.SelectedValue), vS19, txtRem19.Text.Replace("'", "''"), Convert.ToInt32(ddlQ20.SelectedValue), vS20, txtRem20.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ100.SelectedValue), vS100, txtRemLast.Text.Replace("'", "''"),

                               Convert.ToInt32(ddlQ103.SelectedValue), vS103, txtRem103.Text.Replace("'", "''"),

                               Convert.ToInt32(ddlQ21.SelectedValue), vS21, txtRem21.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ22.SelectedValue), vS22, txtRem22.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ23.SelectedValue), vS23, txtRem23.Text.Replace("'", "''"), Convert.ToInt32(ddlQ24.SelectedValue), vS24, txtRem24.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ25.SelectedValue), vS25, txtRem25.Text.Replace("'", "''"), Convert.ToInt32(ddlQ26.SelectedValue), 0, txtRem26.Text.Replace("'", "''"),

                               Convert.ToInt32(ddlQ102.SelectedValue), vS102, txtRem102.Text.Replace("'", "''"),

                               Convert.ToInt32(ddlQ27.SelectedValue), vS27, txtRem27.Text.Replace("'", "''"), Convert.ToInt32(ddlQ28.SelectedValue), vS28, txtRem28.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ29.SelectedValue), vS29, txtRem29.Text.Replace("'", "''"), Convert.ToInt32(ddlQ30.SelectedValue), vS30, txtRem30.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ31.SelectedValue), vS31, txtRem31.Text.Replace("'", "''"), Convert.ToInt32(ddlQ32.SelectedValue), vS32, txtRem32.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ33.SelectedValue), vS33, txtRem33.Text.Replace("'", "''"), Convert.ToInt32(ddlQ34.SelectedValue), vS34, txtRem34.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ35.SelectedValue), vS35, txtRem35.Text.Replace("'", "''"), Convert.ToInt32(ddlQ36.SelectedValue), vS36, txtRem36.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ37.SelectedValue), vS37, txtRem37.Text.Replace("'", "''"), Convert.ToInt32(ddlQ38.SelectedValue), vS38, txtRem38.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ39.SelectedValue), vS39, txtRem39.Text.Replace("'", "''"), Convert.ToInt32(ddlQ40.SelectedValue), vS40, txtRem40.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ301.SelectedValue), vS41, txtRem301.Text.Replace("'", "''"), Convert.ToInt32(ddlQ42.SelectedValue), vS42, txtRem42.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ43.SelectedValue), vS43, txtRem43.Text.Replace("'", "''"), Convert.ToInt32(ddlQ44.SelectedValue), vS44, txtRem44.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ45.SelectedValue), vS45, txtRem45.Text.Replace("'", "''"), Convert.ToInt32(ddlQ46.SelectedValue), vS46, txtRem46.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ47.SelectedValue), vS47, txtRem47.Text.Replace("'", "''"), txtRem51.Text.Replace("'", "''"), txtRem52.Text.Replace("'", "''"), "Save",

                               Convert.ToInt32(ddlQ48.SelectedValue), vS48, txtRem48.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ49.SelectedValue), vS49, txtRem49.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ50.SelectedValue), vS50, txtRem50.Text.Replace("'", "''"), Convert.ToInt32(ddlQ201.SelectedValue), vS201, txtRem201.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ202.SelectedValue), vS202, txtRem202.Text.Replace("'", "''"), Convert.ToInt32(ddlQ203.SelectedValue), vS203, txtRem203.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ204.SelectedValue), vS204, txtRem204.Text.Replace("'", "''"), Convert.ToInt32(ddlQ205.SelectedValue), vS205, txtRem205.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ206.SelectedValue), vS206, txtRem206.Text.Replace("'", "''"), Convert.ToInt32(ddlQ207.SelectedValue), vS207, txtRem207.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ208.SelectedValue), vS208, txtRem208.Text.Replace("'", "''"), Convert.ToInt32(ddlQ209.SelectedValue), vS209, txtRem209.Text.Replace("'", "''"),

                               vIR1, vIR2, vIR3, vIR4, vIR5, vIR6, vIR7, vIR8, vIR9, vIR10, vIR11, vIR12, vIR13, vIR101, vIR14, vIR15, vIR16, vIR17, vIR18, vIR19, vIR20, vIR100,
                               vIR103,
                               vIR21, vIR22, vIR23, vIR24, vIR25, vIR26, vIR102, vIR27, vIR28, vIR29, vIR30, vIR31, vIR32, vIR33, vIR34, vIR35, vIR36, vIR37, vIR38, vIR39, vIR40,
                               vIR41, vIR42, vIR43, vIR44, vIR45, vIR46, vIR47, vIR48, vIR49, vIR50, vIR51, vIR52, vIR201, vIR202, vIR203, vIR204, vIR205, vIR206, vIR207, vIR208, vIR209,
                               txtOther.Text);
                    if (vErr > 0)                                                                                  
                    {                                                                                               
                        ViewState["InsPecId"] = vInsPecId;                                                          
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
                    oAu = new CIntIspPM();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("EOMst", "EMPCode", txtEmpCode.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Edit");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("EMP Code Can not be Duplicate...");
                    //    return false;
                    //}

                    vErr = oAu.SaveIntInspPM(ref vInsPecId, "PM", vSubDt, vCrStrDt, vCrEndDt, vPrStrDt, vPrEndDt, ddlBranch.SelectedValue, vUserID,
                               Convert.ToInt32(ddlQ1.SelectedValue), vS1, txtRem1.Text.Replace("'", "''"), Convert.ToInt32(ddlQ2.SelectedValue), vS2, txtRem2.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ3.SelectedValue), vS3, txtRem3.Text.Replace("'", "''"), Convert.ToInt32(ddlQ4.SelectedValue), vS4, txtRem4.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ5.SelectedValue), vS5, txtRem5.Text.Replace("'", "''"), Convert.ToInt32(ddlQ6.SelectedValue), vS6, txtRem6.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ7.SelectedValue), vS7, txtRem7.Text.Replace("'", "''"), Convert.ToInt32(ddlQ8.SelectedValue), vS8, txtRem8.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ9.SelectedValue), vS9, txtRem9.Text.Replace("'", "''"), Convert.ToInt32(ddlQ10.SelectedValue), vS10, txtRem10.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ11.SelectedValue), vS11, txtRem11.Text.Replace("'", "''"), Convert.ToInt32(ddlQ12.SelectedValue), vS12, txtRem12.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ13.SelectedValue), vS13, txtRem13.Text.Replace("'", "''"),Convert.ToInt32(ddlQ101.SelectedValue), vS101, txtRem101.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ14.SelectedValue), vS14, txtRem14.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ15.SelectedValue), vS15, txtRem15.Text.Replace("'", "''"), Convert.ToInt32(ddlQ16.SelectedValue), vS16, txtRem16.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ17.SelectedValue), 0, txtRem17.Text.Replace("'", "''"), Convert.ToInt32(ddlQ18.SelectedValue), vS18, txtRem18.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ19.SelectedValue), vS19, txtRem19.Text.Replace("'", "''"), Convert.ToInt32(ddlQ20.SelectedValue), vS20, txtRem20.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ100.SelectedValue), vS100, txtRemLast.Text.Replace("'", "''"),

                               Convert.ToInt32(ddlQ103.SelectedValue), vS103, txtRem103.Text.Replace("'", "''"),

                               Convert.ToInt32(ddlQ21.SelectedValue), vS21, txtRem21.Text.Replace("'", "''"), Convert.ToInt32(ddlQ22.SelectedValue), vS22, txtRem22.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ23.SelectedValue), vS23, txtRem23.Text.Replace("'", "''"), Convert.ToInt32(ddlQ24.SelectedValue), vS24, txtRem24.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ25.SelectedValue), vS25, txtRem25.Text.Replace("'", "''"), Convert.ToInt32(ddlQ26.SelectedValue), 0, txtRem26.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ102.SelectedValue), vS102, txtRem102.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ27.SelectedValue), vS27, txtRem27.Text.Replace("'", "''"), Convert.ToInt32(ddlQ28.SelectedValue), vS28, txtRem28.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ29.SelectedValue), vS29, txtRem29.Text.Replace("'", "''"), Convert.ToInt32(ddlQ30.SelectedValue), vS30, txtRem30.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ31.SelectedValue), vS31, txtRem31.Text.Replace("'", "''"), Convert.ToInt32(ddlQ32.SelectedValue), vS32, txtRem32.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ33.SelectedValue), vS33, txtRem33.Text.Replace("'", "''"), Convert.ToInt32(ddlQ34.SelectedValue), vS34, txtRem34.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ35.SelectedValue), vS35, txtRem35.Text.Replace("'", "''"), Convert.ToInt32(ddlQ36.SelectedValue), vS36, txtRem36.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ37.SelectedValue), vS37, txtRem37.Text.Replace("'", "''"), Convert.ToInt32(ddlQ38.SelectedValue), vS38, txtRem38.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ39.SelectedValue), vS39, txtRem39.Text.Replace("'", "''"), Convert.ToInt32(ddlQ40.SelectedValue), vS40, txtRem40.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ301.SelectedValue), vS41, txtRem301.Text.Replace("'", "''"), Convert.ToInt32(ddlQ42.SelectedValue), vS42, txtRem42.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ43.SelectedValue), vS43, txtRem43.Text.Replace("'", "''"), Convert.ToInt32(ddlQ44.SelectedValue), vS44, txtRem44.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ45.SelectedValue), vS45, txtRem45.Text.Replace("'", "''"), Convert.ToInt32(ddlQ46.SelectedValue), vS46, txtRem46.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ47.SelectedValue), vS47, txtRem47.Text.Replace("'", "''"), txtRem51.Text.Replace("'", "''"), txtRem52.Text.Replace("'", "''"), "Edit",

                               Convert.ToInt32(ddlQ48.SelectedValue), vS48, txtRem48.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ49.SelectedValue), vS49, txtRem49.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ50.SelectedValue), vS50, txtRem50.Text.Replace("'", "''"), Convert.ToInt32(ddlQ201.SelectedValue), vS201, txtRem201.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ202.SelectedValue), vS202, txtRem202.Text.Replace("'", "''"), Convert.ToInt32(ddlQ203.SelectedValue), vS203, txtRem203.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ204.SelectedValue), vS204, txtRem204.Text.Replace("'", "''"), Convert.ToInt32(ddlQ205.SelectedValue), vS205, txtRem205.Text.Replace("'", "''"),
                                Convert.ToInt32(ddlQ206.SelectedValue), vS206, txtRem206.Text.Replace("'", "''"), Convert.ToInt32(ddlQ207.SelectedValue), vS207, txtRem207.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ208.SelectedValue), vS208, txtRem208.Text.Replace("'", "''"), Convert.ToInt32(ddlQ209.SelectedValue), vS209, txtRem209.Text.Replace("'", "''"),

                               vIR1, vIR2, vIR3, vIR4, vIR5, vIR6, vIR7, vIR8, vIR9, vIR10, vIR11, vIR12, vIR13, vIR101, vIR14, vIR15, vIR16, vIR17, vIR18, vIR19, vIR20, vIR100,
                               vIR103,
                               vIR21, vIR22, vIR23, vIR24, vIR25, vIR26, vIR102, vIR27, vIR28, vIR29, vIR30, vIR31, vIR32, vIR33, vIR34, vIR35, vIR36, vIR37, vIR38, vIR39, vIR40,
                               vIR41, vIR42, vIR43, vIR44, vIR45, vIR46, vIR47, vIR48, vIR49, vIR50, vIR51, vIR52, vIR201, vIR202, vIR203, vIR204, vIR205, vIR206, vIR207, vIR208, vIR209,
                               txtOther.Text);
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
                    oAu = new CIntIspPM();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDeleteString(vEoId, "EOID", "MarketMSt");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("The RO has group, you can not delete the RO.");
                    //    return false;
                    //}
                    vErr = oAu.SaveIntInspPM(ref vInsPecId, "PM", vSubDt, vCrStrDt, vCrEndDt, vPrStrDt, vPrEndDt, ddlBranch.SelectedValue, vUserID,
                               Convert.ToInt32(ddlQ1.SelectedValue), vS1, txtRem1.Text.Replace("'", "''"), Convert.ToInt32(ddlQ2.SelectedValue), vS2, txtRem2.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ3.SelectedValue), vS3, txtRem3.Text.Replace("'", "''"), Convert.ToInt32(ddlQ4.SelectedValue), vS4, txtRem4.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ5.SelectedValue), vS5, txtRem5.Text.Replace("'", "''"), Convert.ToInt32(ddlQ6.SelectedValue), vS6, txtRem6.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ7.SelectedValue), vS7, txtRem7.Text.Replace("'", "''"), Convert.ToInt32(ddlQ8.SelectedValue), vS8, txtRem8.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ9.SelectedValue), vS9, txtRem9.Text.Replace("'", "''"), Convert.ToInt32(ddlQ10.SelectedValue), vS10, txtRem10.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ11.SelectedValue), vS11, txtRem11.Text.Replace("'", "''"), Convert.ToInt32(ddlQ12.SelectedValue), vS12, txtRem12.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ13.SelectedValue), vS13, txtRem13.Text.Replace("'", "''"), Convert.ToInt32(ddlQ101.SelectedValue), vS101, txtRem101.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ14.SelectedValue), vS14, txtRem14.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ15.SelectedValue), vS15, txtRem15.Text.Replace("'", "''"), Convert.ToInt32(ddlQ16.SelectedValue), vS16, txtRem16.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ17.SelectedValue), 0, txtRem17.Text.Replace("'", "''"), Convert.ToInt32(ddlQ18.SelectedValue), vS18, txtRem18.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ19.SelectedValue), vS19, txtRem19.Text.Replace("'", "''"), Convert.ToInt32(ddlQ20.SelectedValue), vS20, txtRem20.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ100.SelectedValue), vS100, txtRemLast.Text.Replace("'", "''"),


                               Convert.ToInt32(ddlQ103.SelectedValue), vS103, txtRem103.Text.Replace("'", "''"),

                               Convert.ToInt32(ddlQ21.SelectedValue), vS21, txtRem21.Text.Replace("'", "''"), Convert.ToInt32(ddlQ22.SelectedValue), vS22, txtRem22.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ23.SelectedValue), vS23, txtRem23.Text.Replace("'", "''"), Convert.ToInt32(ddlQ24.SelectedValue), vS24, txtRem24.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ25.SelectedValue), vS25, txtRem25.Text.Replace("'", "''"), Convert.ToInt32(ddlQ26.SelectedValue), 0, txtRem26.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ102.SelectedValue), vS102, txtRem102.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ27.SelectedValue), vS27, txtRem27.Text.Replace("'", "''"), Convert.ToInt32(ddlQ28.SelectedValue), vS28, txtRem28.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ29.SelectedValue), vS29, txtRem29.Text.Replace("'", "''"), Convert.ToInt32(ddlQ30.SelectedValue), vS30, txtRem30.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ31.SelectedValue), vS31, txtRem31.Text.Replace("'", "''"), Convert.ToInt32(ddlQ32.SelectedValue), vS32, txtRem32.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ33.SelectedValue), vS33, txtRem33.Text.Replace("'", "''"), Convert.ToInt32(ddlQ34.SelectedValue), vS34, txtRem34.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ35.SelectedValue), vS35, txtRem35.Text.Replace("'", "''"), Convert.ToInt32(ddlQ36.SelectedValue), vS36, txtRem36.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ37.SelectedValue), vS37, txtRem37.Text.Replace("'", "''"), Convert.ToInt32(ddlQ38.SelectedValue), vS38, txtRem38.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ39.SelectedValue), vS39, txtRem39.Text.Replace("'", "''"), Convert.ToInt32(ddlQ40.SelectedValue), vS40, txtRem40.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ301.SelectedValue), vS41, txtRem301.Text.Replace("'", "''"), Convert.ToInt32(ddlQ42.SelectedValue), vS42, txtRem42.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ43.SelectedValue), vS43, txtRem43.Text.Replace("'", "''"), Convert.ToInt32(ddlQ44.SelectedValue), vS44, txtRem44.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ45.SelectedValue), vS45, txtRem45.Text.Replace("'", "''"), Convert.ToInt32(ddlQ46.SelectedValue), vS46, txtRem46.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ47.SelectedValue), vS47, txtRem47.Text.Replace("'", "''"), txtRem51.Text.Replace("'", "''"), txtRem52.Text.Replace("'", "''"), "Del",

                               Convert.ToInt32(ddlQ48.SelectedValue), vS48, txtRem48.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ49.SelectedValue), vS49, txtRem49.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ50.SelectedValue), vS50, txtRem50.Text.Replace("'", "''"), Convert.ToInt32(ddlQ201.SelectedValue), vS201, txtRem201.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ202.SelectedValue), vS202, txtRem202.Text.Replace("'", "''"), Convert.ToInt32(ddlQ203.SelectedValue), vS203, txtRem203.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ204.SelectedValue), vS204, txtRem204.Text.Replace("'", "''"), Convert.ToInt32(ddlQ205.SelectedValue), vS205, txtRem205.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ206.SelectedValue), vS206, txtRem206.Text.Replace("'", "''"), Convert.ToInt32(ddlQ207.SelectedValue), vS207, txtRem207.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlQ208.SelectedValue), vS208, txtRem208.Text.Replace("'", "''"), Convert.ToInt32(ddlQ209.SelectedValue), vS209, txtRem209.Text.Replace("'", "''"),

                               vIR1, vIR2, vIR3, vIR4, vIR5, vIR6, vIR7, vIR8, vIR9, vIR10, vIR11, vIR12, vIR13, vIR101, vIR14, vIR15, vIR16, vIR17, vIR18, vIR19, vIR20, vIR100,
                               vIR103,
                               vIR21, vIR22, vIR23, vIR24, vIR25, vIR26, vIR102, vIR27, vIR28, vIR29, vIR30, vIR31, vIR32, vIR33, vIR34, vIR35, vIR36, vIR37, vIR38, vIR39, vIR40,
                               vIR41, vIR42, vIR43, vIR44, vIR45, vIR46, vIR47, vIR48, vIR49, vIR50, vIR51, vIR52, vIR201, vIR202, vIR203, vIR204, vIR205, vIR206, vIR207, vIR208, vIR209,
                               txtOther.Text);
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
                oAu = null;               
            }
        }  
    }
}