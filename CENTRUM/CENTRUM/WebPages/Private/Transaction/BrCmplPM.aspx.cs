using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class BrCmplPM : CENTRUMBase
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
                txtCmpDt.Text = Session[gblValue.LoginDate].ToString();
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
                this.PageHeading = "Branch Compliance";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBrnchCmpl);
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
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    btnExit.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Branch Compliance", false);
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
            txtBR1.Enabled = Status;
            txtBR2.Enabled = Status;
            txtBR3.Enabled = Status;
            txtBR4.Enabled = Status;
            txtBR5.Enabled = Status;
            txtBR6.Enabled = Status;
            txtBR7.Enabled = Status;
            txtBR8.Enabled = Status;
            txtBR9.Enabled = Status;
            txtBR10.Enabled = Status;
            txtBR11.Enabled = Status;
            txtBR12.Enabled = Status;
            txtBR13.Enabled = Status;
            txtBR14.Enabled = Status;
            txtBR15.Enabled = Status;
            txtBR16.Enabled = Status;
            txtBR17.Enabled = Status;
            txtBR18.Enabled = Status;
            txtBR19.Enabled = Status;
            txtBR20.Enabled = Status;
            txtBR21.Enabled = Status;
            txtBR22.Enabled = Status;
            txtBR23.Enabled = Status;
            txtBR24.Enabled = Status;
            txtBR25.Enabled = Status;
            txtBR26.Enabled = Status;
            txtBR27.Enabled = Status;
            txtBR28.Enabled = Status;
            txtBR29.Enabled = Status;
            txtBR30.Enabled = Status;
            txtBR31.Enabled = Status;
            txtBR32.Enabled = Status;
            txtBR33.Enabled = Status;
            txtBR34.Enabled = Status;
            txtBR35.Enabled = Status;
            txtBR36.Enabled = Status;
            txtBR37.Enabled = Status;
            txtBR38.Enabled = Status;
            txtBR39.Enabled = Status;
            txtBR40.Enabled = Status;
            txtBR41.Enabled = Status;
            txtBR42.Enabled = Status;
            txtBR43.Enabled = Status;
            txtBR44.Enabled = Status;
            txtBR45.Enabled = Status;
            txtBR46.Enabled = Status;
            txtBR47.Enabled = Status;
            txtBR48.Enabled = Status;
            txtBR49.Enabled = Status;
            txtBR50.Enabled = Status;
            txtBR51.Enabled = Status;
            txtBR52.Enabled = Status;

            txtBPR1.Enabled = Status;
            txtBPR2.Enabled = Status;
            txtBPR3.Enabled = Status;
            txtBPR4.Enabled = Status;
            txtBPR5.Enabled = Status;
            txtBPR6.Enabled = Status;
            txtBPR7.Enabled = Status;
            txtBPR8.Enabled = Status;
            txtBPR9.Enabled = Status;
            txtBPR10.Enabled = Status;
            txtBPR11.Enabled = Status;
            txtBPR12.Enabled = Status;
            txtBPR13.Enabled = Status;
            txtBPR14.Enabled = Status;
            txtBPR15.Enabled = Status;
            txtBPR16.Enabled = Status;
            txtBPR17.Enabled = Status;
            txtBPR18.Enabled = Status;
            txtBPR19.Enabled = Status;
            txtBPR20.Enabled = Status;
            txtBPR21.Enabled = Status;
            txtBPR22.Enabled = Status;
            txtBPR23.Enabled = Status;
            txtBPR24.Enabled = Status;
            txtBPR25.Enabled = Status;
            txtBPR26.Enabled = Status;
            txtBPR27.Enabled = Status;
            txtBPR28.Enabled = Status;
            txtBPR29.Enabled = Status;
            txtBPR30.Enabled = Status;
            txtBPR31.Enabled = Status;
            txtBPR32.Enabled = Status;
            txtBPR33.Enabled = Status;
            txtBPR34.Enabled = Status;
            txtBPR35.Enabled = Status;
            txtBPR36.Enabled = Status;
            txtBPR37.Enabled = Status;
            txtBPR38.Enabled = Status;
            txtBPR39.Enabled = Status;
            txtBPR40.Enabled = Status;
            txtBPR41.Enabled = Status;
            txtBPR42.Enabled = Status;
            txtBPR43.Enabled = Status;
            txtBPR44.Enabled = Status;
            txtBPR45.Enabled = Status;
            txtBPR46.Enabled = Status;
            txtBPR47.Enabled = Status;
            txtBPR48.Enabled = Status;
            txtBPR49.Enabled = Status;

            txtBPR50.Enabled = Status;
            txtBPR51.Enabled = Status;
            txtBPR52.Enabled = Status;

            txtBNR1.Enabled = Status;
            txtBNR2.Enabled = Status;
            txtBNR3.Enabled = Status;
            txtBNR4.Enabled = Status;
            txtBNR5.Enabled = Status;
            txtBNR6.Enabled = Status;
            txtBNR7.Enabled = Status;
            txtBNR8.Enabled = Status;
            txtBNR9.Enabled = Status;
            txtBNR10.Enabled = Status;
            txtBNR11.Enabled = Status;
            txtBNR12.Enabled = Status;
            txtBNR13.Enabled = Status;
            txtBNR14.Enabled = Status;
            txtBNR15.Enabled = Status;
            txtBNR16.Enabled = Status;
            txtBNR17.Enabled = Status;
            txtBNR18.Enabled = Status;
            txtBNR19.Enabled = Status;
            txtBNR20.Enabled = Status;
            txtBNR21.Enabled = Status;
            txtBNR22.Enabled = Status;
            txtBNR23.Enabled = Status;
            txtBNR24.Enabled = Status;
            txtBNR25.Enabled = Status;
            txtBNR26.Enabled = Status;
            txtBNR27.Enabled = Status;
            txtBNR28.Enabled = Status;
            txtBNR29.Enabled = Status;
            txtBNR30.Enabled = Status;
            txtBNR31.Enabled = Status;
            txtBNR32.Enabled = Status;
            txtBNR33.Enabled = Status;
            txtBNR34.Enabled = Status;
            txtBNR35.Enabled = Status;
            txtBNR36.Enabled = Status;
            txtBNR37.Enabled = Status;
            txtBNR38.Enabled = Status;
            txtBNR39.Enabled = Status;
            txtBNR40.Enabled = Status;
            txtBNR41.Enabled = Status;
            txtBNR42.Enabled = Status;
            txtBNR43.Enabled = Status;
            txtBNR44.Enabled = Status;
            txtBNR45.Enabled = Status;
            txtBNR46.Enabled = Status;
            txtBNR47.Enabled = Status;
            txtBNR48.Enabled = Status;
            txtBNR49.Enabled = Status;

            txtBNR50.Enabled = Status;
            txtBNR51.Enabled = Status;
            txtBNR52.Enabled = Status;

            txtBRem1.Enabled = Status;
            txtBRem2.Enabled = Status;
            txtBRem3.Enabled = Status;
            txtBRem4.Enabled = Status;
            txtBRem5.Enabled = Status;
            txtBRem6.Enabled = Status;
            txtBRem7.Enabled = Status;
            txtBRem8.Enabled = Status;
            txtBRem9.Enabled = Status;
            txtBRem10.Enabled = Status;
            txtBRem11.Enabled = Status;
            txtBRem12.Enabled = Status;
            txtBRem13.Enabled = Status;
            txtBRem14.Enabled = Status;
            txtBRem15.Enabled = Status;
            txtBRem16.Enabled = Status;
            txtBRem17.Enabled = Status;
            txtBRem18.Enabled = Status;
            txtBRem19.Enabled = Status;
            txtBRem20.Enabled = Status;
            txtBRem21.Enabled = Status;
            txtBRem22.Enabled = Status;
            txtBRem23.Enabled = Status;
            txtBRem24.Enabled = Status;
            txtBRem25.Enabled = Status;
            txtBRem26.Enabled = Status;
            txtBRem27.Enabled = Status;
            txtBRem28.Enabled = Status;
            txtBRem29.Enabled = Status;
            txtBRem30.Enabled = Status;
            txtBRem31.Enabled = Status;
            txtBRem32.Enabled = Status;
            txtBRem33.Enabled = Status;
            txtBRem34.Enabled = Status;
            txtBRem35.Enabled = Status;
            txtBRem36.Enabled = Status;
            txtBRem37.Enabled = Status;
            txtBRem38.Enabled = Status;
            txtBRem39.Enabled = Status;
            txtBRem40.Enabled = Status;
            txtBRem41.Enabled = Status;
            txtBRem42.Enabled = Status;
            txtBRem43.Enabled = Status;
            txtBRem44.Enabled = Status;
            txtBRem45.Enabled = Status;
            txtBRem46.Enabled = Status;
            txtBRem47.Enabled = Status;
            txtBRem48.Enabled = Status;
            txtBRem49.Enabled = Status;

            txtBRem50.Enabled = Status;
            txtBRem51.Enabled = Status;
            txtBRem52.Enabled = Status;
        }


        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {

            lblDate.Text = "";
            lblUser.Text = "";

            txtBR1.Text = "0";
            txtBR2.Text = "0";
            txtBR3.Text = "0";
            txtBR4.Text = "0";
            txtBR5.Text = "0";
            txtBR6.Text = "0";
            txtBR7.Text = "0";
            txtBR8.Text = "0";
            txtBR9.Text = "0";
            txtBR10.Text = "0";
            txtBR11.Text = "0";
            txtBR12.Text = "0";
            txtBR13.Text = "0";
            txtBR14.Text = "0";
            txtBR15.Text = "0";
            txtBR16.Text = "0";
            txtBR17.Text = "0";
            txtBR18.Text = "0";
            txtBR19.Text = "0";
            txtBR20.Text = "0";
            txtBR21.Text = "0";
            txtBR22.Text = "0";
            txtBR23.Text = "0";
            txtBR24.Text = "0";
            txtBR25.Text = "0";
            txtBR26.Text = "0";
            txtBR27.Text = "0";
            txtBR28.Text = "0";
            txtBR29.Text = "0";
            txtBR30.Text = "0";
            txtBR31.Text = "0";
            txtBR32.Text = "0";
            txtBR33.Text = "0";
            txtBR34.Text = "0";
            txtBR35.Text = "0";
            txtBR36.Text = "0";
            txtBR37.Text = "0";
            txtBR38.Text = "0";
            txtBR39.Text = "0";
            txtBR40.Text = "0";
            txtBR41.Text = "0";
            txtBR42.Text = "0";
            txtBR43.Text = "0";
            txtBR44.Text = "0";
            txtBR45.Text = "0";
            txtBR46.Text = "0";
            txtBR47.Text = "0";
            txtBR48.Text = "0";
            txtBR49.Text = "0";

            txtBR50.Text = "0";
            txtBR51.Text = "0";
            txtBR52.Text = "0";

            txtBPR1.Text = "0";
            txtBPR2.Text = "0";
            txtBPR3.Text = "0";
            txtBPR4.Text = "0";
            txtBPR5.Text = "0";
            txtBPR6.Text = "0";
            txtBPR7.Text = "0";
            txtBPR8.Text = "0";
            txtBPR9.Text = "0";
            txtBPR10.Text = "0";
            txtBPR11.Text = "0";
            txtBPR12.Text = "0";
            txtBPR13.Text = "0";
            txtBPR14.Text = "0";
            txtBPR15.Text = "0";
            txtBPR16.Text = "0";
            txtBPR17.Text = "0";
            txtBPR18.Text = "0";
            txtBPR19.Text = "0";
            txtBPR20.Text = "0";
            txtBPR21.Text = "0";
            txtBPR22.Text = "0";
            txtBPR23.Text = "0";
            txtBPR24.Text = "0";
            txtBPR25.Text = "0";
            txtBPR26.Text = "0";
            txtBPR27.Text = "0";
            txtBPR28.Text = "0";
            txtBPR29.Text = "0";
            txtBPR30.Text = "0";
            txtBPR31.Text = "0";
            txtBPR32.Text = "0";
            txtBPR33.Text = "0";
            txtBPR34.Text = "0";
            txtBPR35.Text = "0";
            txtBPR36.Text = "0";
            txtBPR37.Text = "0";
            txtBPR38.Text = "0";
            txtBPR39.Text = "0";
            txtBPR40.Text = "0";
            txtBPR41.Text = "0";
            txtBPR42.Text = "0";
            txtBPR43.Text = "0";
            txtBPR44.Text = "0";
            txtBPR45.Text = "0";
            txtBPR46.Text = "0";
            txtBPR47.Text = "0";
            txtBPR48.Text = "0";
            txtBPR49.Text = "0";

            txtBPR50.Text = "0";
            txtBPR51.Text = "0";
            txtBPR52.Text = "0";

            txtBNR1.Text = "0";
            txtBNR2.Text = "0";
            txtBNR3.Text = "0";
            txtBNR4.Text = "0";
            txtBNR5.Text = "0";
            txtBNR6.Text = "0";
            txtBNR7.Text = "0";
            txtBNR8.Text = "0";
            txtBNR9.Text = "0";
            txtBNR10.Text = "0";
            txtBNR11.Text = "0";
            txtBNR12.Text = "0";
            txtBNR13.Text = "0";
            txtBNR14.Text = "0";
            txtBNR15.Text = "0";
            txtBNR16.Text = "0";
            txtBNR17.Text = "0";
            txtBNR18.Text = "0";
            txtBNR19.Text = "0";
            txtBNR20.Text = "0";
            txtBNR21.Text = "0";
            txtBNR22.Text = "0";
            txtBNR23.Text = "0";
            txtBNR24.Text = "0";
            txtBNR25.Text = "0";
            txtBNR26.Text = "0";
            txtBNR27.Text = "0";
            txtBNR28.Text = "0";
            txtBNR29.Text = "0";
            txtBNR30.Text = "0";
            txtBNR31.Text = "0";
            txtBNR32.Text = "0";
            txtBNR33.Text = "0";
            txtBNR34.Text = "0";
            txtBNR35.Text = "0";
            txtBNR36.Text = "0";
            txtBNR37.Text = "0";
            txtBNR38.Text = "0";
            txtBNR39.Text = "0";
            txtBNR40.Text = "0";
            txtBNR41.Text = "0";
            txtBNR42.Text = "0";
            txtBNR43.Text = "0";
            txtBNR44.Text = "0";
            txtBNR45.Text = "0";
            txtBNR46.Text = "0";
            txtBNR47.Text = "0";
            txtBNR48.Text = "0";
            txtBNR49.Text = "0";

            txtBNR50.Text = "0";
            txtBNR51.Text = "0";
            txtBNR52.Text = "0";

            txtBRem1.Text = "";
            txtBRem2.Text = "";
            txtBRem3.Text = "";
            txtBRem4.Text = "";
            txtBRem5.Text = "";
            txtBRem6.Text = "";
            txtBRem7.Text = "";
            txtBRem8.Text = "";
            txtBRem9.Text = "";
            txtBRem10.Text = "";
            txtBRem11.Text = "";
            txtBRem12.Text = "";
            txtBRem13.Text = "";
            txtBRem14.Text = "";
            txtBRem15.Text = "";
            txtBRem16.Text = "";
            txtBRem17.Text = "";
            txtBRem18.Text = "";
            txtBRem19.Text = "";
            txtBRem20.Text = "";
            txtBRem21.Text = "";
            txtBRem22.Text = "";
            txtBRem23.Text = "";
            txtBRem24.Text = "";
            txtBRem25.Text = "";
            txtBRem26.Text = "";
            txtBRem27.Text = "";
            txtBRem28.Text = "";
            txtBRem29.Text = "";
            txtBRem30.Text = "";
            txtBRem31.Text = "";
            txtBRem32.Text = "";
            txtBRem33.Text = "";
            txtBRem34.Text = "";
            txtBRem35.Text = "";
            txtBRem36.Text = "";
            txtBRem37.Text = "";
            txtBRem38.Text = "";
            txtBRem39.Text = "";
            txtBRem40.Text = "";
            txtBRem41.Text = "";
            txtBRem42.Text = "";
            txtBRem43.Text = "";
            txtBRem44.Text = "";
            txtBRem45.Text = "";
            txtBRem46.Text = "";
            txtBRem47.Text = "";
            txtBRem48.Text = "";
            txtBRem49.Text = "";

            txtBRem50.Text = "";
            txtBRem51.Text = "";
            txtBRem52.Text = "";
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
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oAd = new CIntIspPM();
                dt = oAd.GetBranchCmpliancePG(pPgIndx, vFrmDt, vToDt, ref vTotRows, vBrCode);
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
            DataSet ds = null;
            DataTable dt = null, dt1 = null;
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
                    ds = oAu.GetBranchCmplPMById(pInspID);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];

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
                    ddlQ41.SelectedIndex = ddlQ41.Items.IndexOf(ddlQ41.Items.FindByValue(dt.Rows[0]["A41"].ToString()));
                    txtQ41Scr.Text = Convert.ToString(dt.Rows[0]["S41"]);
                    txtRem41.Text = Convert.ToString(dt.Rows[0]["R41"]);
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

                    ////
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

                    ////                                        
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
                    txtIre14.Text = Convert.ToString(dt.Rows[0]["IR14"]);
                    txtIre15.Text = Convert.ToString(dt.Rows[0]["IR15"]);
                    txtIre16.Text = Convert.ToString(dt.Rows[0]["IR16"]);
                    txtIre17.Text = Convert.ToString(dt.Rows[0]["IR17"]);
                    txtIre18.Text = Convert.ToString(dt.Rows[0]["IR18"]);
                    txtIre19.Text = Convert.ToString(dt.Rows[0]["IR19"]);
                    txtIre20.Text = Convert.ToString(dt.Rows[0]["IR20"]);
                    txtIre21.Text = Convert.ToString(dt.Rows[0]["IR21"]);
                    txtIre22.Text = Convert.ToString(dt.Rows[0]["IR22"]);
                    txtIre23.Text = Convert.ToString(dt.Rows[0]["IR23"]);
                    txtIre24.Text = Convert.ToString(dt.Rows[0]["IR24"]);
                    txtIre25.Text = Convert.ToString(dt.Rows[0]["IR25"]);
                    txtIre26.Text = Convert.ToString(dt.Rows[0]["IR26"]);
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

                    if (dt1.Rows.Count > 0)
                    {
                        //txtCmpDt.Text = Convert.ToString(dt1.Rows[0]["CmplDt"]);
                        lblUser.Text = "Last Modified By : " + dt1.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt1.Rows[0]["CreationDateTime"].ToString();

                        txtBR1.Text = Convert.ToString(dt1.Rows[0]["BR1"]);
                        txtBR2.Text = Convert.ToString(dt1.Rows[0]["BR2"]);
                        txtBR3.Text = Convert.ToString(dt1.Rows[0]["BR3"]);
                        txtBR4.Text = Convert.ToString(dt1.Rows[0]["BR4"]);
                        txtBR5.Text = Convert.ToString(dt1.Rows[0]["BR5"]);
                        txtBR6.Text = Convert.ToString(dt1.Rows[0]["BR6"]);
                        txtBR7.Text = Convert.ToString(dt1.Rows[0]["BR7"]);
                        txtBR8.Text = Convert.ToString(dt1.Rows[0]["BR8"]);
                        txtBR9.Text = Convert.ToString(dt1.Rows[0]["BR9"]);
                        txtBR10.Text = Convert.ToString(dt1.Rows[0]["BR10"]);
                        txtBR11.Text = Convert.ToString(dt1.Rows[0]["BR11"]);
                        txtBR12.Text = Convert.ToString(dt1.Rows[0]["BR12"]);
                        txtBR13.Text = Convert.ToString(dt1.Rows[0]["BR13"]);
                        txtBR14.Text = Convert.ToString(dt1.Rows[0]["BR14"]);
                        txtBR15.Text = Convert.ToString(dt1.Rows[0]["BR15"]);
                        txtBR16.Text = Convert.ToString(dt1.Rows[0]["BR16"]);
                        txtBR17.Text = Convert.ToString(dt1.Rows[0]["BR17"]);
                        txtBR18.Text = Convert.ToString(dt1.Rows[0]["BR18"]);
                        txtBR19.Text = Convert.ToString(dt1.Rows[0]["BR19"]);
                        txtBR20.Text = Convert.ToString(dt1.Rows[0]["BR20"]);
                        txtBR21.Text = Convert.ToString(dt1.Rows[0]["BR21"]);
                        txtBR22.Text = Convert.ToString(dt1.Rows[0]["BR22"]);
                        txtBR23.Text = Convert.ToString(dt1.Rows[0]["BR23"]);
                        txtBR24.Text = Convert.ToString(dt1.Rows[0]["BR24"]);
                        txtBR25.Text = Convert.ToString(dt1.Rows[0]["BR25"]);
                        txtBR26.Text = Convert.ToString(dt1.Rows[0]["BR26"]);
                        txtBR27.Text = Convert.ToString(dt1.Rows[0]["BR27"]);
                        txtBR28.Text = Convert.ToString(dt1.Rows[0]["BR28"]);
                        txtBR29.Text = Convert.ToString(dt1.Rows[0]["BR29"]);
                        txtBR30.Text = Convert.ToString(dt1.Rows[0]["BR30"]);
                        txtBR31.Text = Convert.ToString(dt1.Rows[0]["BR31"]);
                        txtBR32.Text = Convert.ToString(dt1.Rows[0]["BR32"]);
                        txtBR33.Text = Convert.ToString(dt1.Rows[0]["BR33"]);
                        txtBR34.Text = Convert.ToString(dt1.Rows[0]["BR34"]);
                        txtBR35.Text = Convert.ToString(dt1.Rows[0]["BR35"]);
                        txtBR36.Text = Convert.ToString(dt1.Rows[0]["BR36"]);
                        txtBR37.Text = Convert.ToString(dt1.Rows[0]["BR37"]);
                        txtBR38.Text = Convert.ToString(dt1.Rows[0]["BR38"]);
                        txtBR39.Text = Convert.ToString(dt1.Rows[0]["BR39"]);
                        txtBR40.Text = Convert.ToString(dt1.Rows[0]["BR40"]);
                        txtBR41.Text = Convert.ToString(dt1.Rows[0]["BR41"]);
                        txtBR42.Text = Convert.ToString(dt1.Rows[0]["BR42"]);
                        txtBR43.Text = Convert.ToString(dt1.Rows[0]["BR43"]);
                        txtBR44.Text = Convert.ToString(dt1.Rows[0]["BR44"]);
                        txtBR45.Text = Convert.ToString(dt1.Rows[0]["BR45"]);
                        txtBR46.Text = Convert.ToString(dt1.Rows[0]["BR46"]);
                        txtBR47.Text = Convert.ToString(dt1.Rows[0]["BR47"]);
                        txtBR51.Text = Convert.ToString(dt1.Rows[0]["BR48"]);
                        txtBR52.Text = Convert.ToString(dt1.Rows[0]["BR49"]);

                        txtBR48.Text = Convert.ToString(dt1.Rows[0]["BR50"]);
                        txtBR49.Text = Convert.ToString(dt1.Rows[0]["BR51"]);
                        txtBR50.Text = Convert.ToString(dt1.Rows[0]["BR52"]);

                        txtBPR1.Text = Convert.ToString(dt1.Rows[0]["BPR1"]);
                        txtBPR2.Text = Convert.ToString(dt1.Rows[0]["BPR2"]);
                        txtBPR3.Text = Convert.ToString(dt1.Rows[0]["BPR3"]);
                        txtBPR4.Text = Convert.ToString(dt1.Rows[0]["BPR4"]);
                        txtBPR5.Text = Convert.ToString(dt1.Rows[0]["BPR5"]);
                        txtBPR6.Text = Convert.ToString(dt1.Rows[0]["BPR6"]);
                        txtBPR7.Text = Convert.ToString(dt1.Rows[0]["BPR7"]);
                        txtBPR8.Text = Convert.ToString(dt1.Rows[0]["BPR8"]);
                        txtBPR9.Text = Convert.ToString(dt1.Rows[0]["BPR9"]);
                        txtBPR10.Text = Convert.ToString(dt1.Rows[0]["BPR10"]);
                        txtBPR11.Text = Convert.ToString(dt1.Rows[0]["BPR11"]);
                        txtBPR12.Text = Convert.ToString(dt1.Rows[0]["BPR12"]);
                        txtBPR13.Text = Convert.ToString(dt1.Rows[0]["BPR13"]);
                        txtBPR14.Text = Convert.ToString(dt1.Rows[0]["BPR14"]);
                        txtBPR15.Text = Convert.ToString(dt1.Rows[0]["BPR15"]);
                        txtBPR16.Text = Convert.ToString(dt1.Rows[0]["BPR16"]);
                        txtBPR17.Text = Convert.ToString(dt1.Rows[0]["BPR17"]);
                        txtBPR18.Text = Convert.ToString(dt1.Rows[0]["BPR18"]);
                        txtBPR19.Text = Convert.ToString(dt1.Rows[0]["BPR19"]);
                        txtBPR20.Text = Convert.ToString(dt1.Rows[0]["BPR20"]);
                        txtBPR21.Text = Convert.ToString(dt1.Rows[0]["BPR21"]);
                        txtBPR22.Text = Convert.ToString(dt1.Rows[0]["BPR22"]);
                        txtBPR23.Text = Convert.ToString(dt1.Rows[0]["BPR23"]);
                        txtBPR24.Text = Convert.ToString(dt1.Rows[0]["BPR24"]);
                        txtBPR25.Text = Convert.ToString(dt1.Rows[0]["BPR25"]);
                        txtBPR26.Text = Convert.ToString(dt1.Rows[0]["BPR26"]);
                        txtBPR27.Text = Convert.ToString(dt1.Rows[0]["BPR27"]);
                        txtBPR28.Text = Convert.ToString(dt1.Rows[0]["BPR28"]);
                        txtBPR29.Text = Convert.ToString(dt1.Rows[0]["BPR29"]);
                        txtBPR30.Text = Convert.ToString(dt1.Rows[0]["BPR30"]);
                        txtBPR31.Text = Convert.ToString(dt1.Rows[0]["BPR31"]);
                        txtBPR32.Text = Convert.ToString(dt1.Rows[0]["BPR32"]);
                        txtBPR33.Text = Convert.ToString(dt1.Rows[0]["BPR33"]);
                        txtBPR34.Text = Convert.ToString(dt1.Rows[0]["BPR34"]);
                        txtBPR35.Text = Convert.ToString(dt1.Rows[0]["BPR35"]);
                        txtBPR36.Text = Convert.ToString(dt1.Rows[0]["BPR36"]);
                        txtBPR37.Text = Convert.ToString(dt1.Rows[0]["BPR37"]);
                        txtBPR38.Text = Convert.ToString(dt1.Rows[0]["BPR38"]);
                        txtBPR39.Text = Convert.ToString(dt1.Rows[0]["BPR39"]);
                        txtBPR40.Text = Convert.ToString(dt1.Rows[0]["BPR40"]);
                        txtBPR41.Text = Convert.ToString(dt1.Rows[0]["BPR41"]);
                        txtBPR42.Text = Convert.ToString(dt1.Rows[0]["BPR42"]);
                        txtBPR43.Text = Convert.ToString(dt1.Rows[0]["BPR43"]);
                        txtBPR44.Text = Convert.ToString(dt1.Rows[0]["BPR44"]);
                        txtBPR45.Text = Convert.ToString(dt1.Rows[0]["BPR45"]);
                        txtBPR46.Text = Convert.ToString(dt1.Rows[0]["BPR46"]);
                        txtBPR47.Text = Convert.ToString(dt1.Rows[0]["BPR47"]);
                        txtBPR51.Text = Convert.ToString(dt1.Rows[0]["BPR48"]);
                        txtBPR52.Text = Convert.ToString(dt1.Rows[0]["BPR49"]);

                        txtBPR48.Text = Convert.ToString(dt1.Rows[0]["BPR50"]);
                        txtBPR49.Text = Convert.ToString(dt1.Rows[0]["BPR51"]);
                        txtBPR50.Text = Convert.ToString(dt1.Rows[0]["BPR52"]);

                        txtBNR1.Text = Convert.ToString(dt1.Rows[0]["BNR1"]);
                        txtBNR2.Text = Convert.ToString(dt1.Rows[0]["BNR2"]);
                        txtBNR3.Text = Convert.ToString(dt1.Rows[0]["BNR3"]);
                        txtBNR4.Text = Convert.ToString(dt1.Rows[0]["BNR4"]);
                        txtBNR5.Text = Convert.ToString(dt1.Rows[0]["BNR5"]);
                        txtBNR6.Text = Convert.ToString(dt1.Rows[0]["BNR6"]);
                        txtBNR7.Text = Convert.ToString(dt1.Rows[0]["BNR7"]);
                        txtBNR8.Text = Convert.ToString(dt1.Rows[0]["BNR8"]);
                        txtBNR9.Text = Convert.ToString(dt1.Rows[0]["BNR9"]);
                        txtBNR10.Text = Convert.ToString(dt1.Rows[0]["BNR10"]);
                        txtBNR11.Text = Convert.ToString(dt1.Rows[0]["BNR11"]);
                        txtBNR12.Text = Convert.ToString(dt1.Rows[0]["BNR12"]);
                        txtBNR13.Text = Convert.ToString(dt1.Rows[0]["BNR13"]);
                        txtBNR14.Text = Convert.ToString(dt1.Rows[0]["BNR14"]);
                        txtBNR15.Text = Convert.ToString(dt1.Rows[0]["BNR15"]);
                        txtBNR16.Text = Convert.ToString(dt1.Rows[0]["BNR16"]);
                        txtBNR17.Text = Convert.ToString(dt1.Rows[0]["BNR17"]);
                        txtBNR18.Text = Convert.ToString(dt1.Rows[0]["BNR18"]);
                        txtBNR19.Text = Convert.ToString(dt1.Rows[0]["BNR19"]);
                        txtBNR20.Text = Convert.ToString(dt1.Rows[0]["BNR20"]);
                        txtBNR21.Text = Convert.ToString(dt1.Rows[0]["BNR21"]);
                        txtBNR22.Text = Convert.ToString(dt1.Rows[0]["BNR22"]);
                        txtBNR23.Text = Convert.ToString(dt1.Rows[0]["BNR23"]);
                        txtBNR24.Text = Convert.ToString(dt1.Rows[0]["BNR24"]);
                        txtBNR25.Text = Convert.ToString(dt1.Rows[0]["BNR25"]);
                        txtBNR26.Text = Convert.ToString(dt1.Rows[0]["BNR26"]);
                        txtBNR27.Text = Convert.ToString(dt1.Rows[0]["BNR27"]);
                        txtBNR28.Text = Convert.ToString(dt1.Rows[0]["BNR28"]);
                        txtBNR29.Text = Convert.ToString(dt1.Rows[0]["BNR29"]);
                        txtBNR30.Text = Convert.ToString(dt1.Rows[0]["BNR30"]);
                        txtBNR31.Text = Convert.ToString(dt1.Rows[0]["BNR31"]);
                        txtBNR32.Text = Convert.ToString(dt1.Rows[0]["BNR32"]);
                        txtBNR33.Text = Convert.ToString(dt1.Rows[0]["BNR33"]);
                        txtBNR34.Text = Convert.ToString(dt1.Rows[0]["BNR34"]);
                        txtBNR35.Text = Convert.ToString(dt1.Rows[0]["BNR35"]);
                        txtBNR36.Text = Convert.ToString(dt1.Rows[0]["BNR36"]);
                        txtBNR37.Text = Convert.ToString(dt1.Rows[0]["BNR37"]);
                        txtBNR38.Text = Convert.ToString(dt1.Rows[0]["BNR38"]);
                        txtBNR39.Text = Convert.ToString(dt1.Rows[0]["BNR39"]);
                        txtBNR40.Text = Convert.ToString(dt1.Rows[0]["BNR40"]);
                        txtBNR41.Text = Convert.ToString(dt1.Rows[0]["BNR41"]);
                        txtBNR42.Text = Convert.ToString(dt1.Rows[0]["BNR42"]);
                        txtBNR43.Text = Convert.ToString(dt1.Rows[0]["BNR43"]);
                        txtBNR44.Text = Convert.ToString(dt1.Rows[0]["BNR44"]);
                        txtBNR45.Text = Convert.ToString(dt1.Rows[0]["BNR45"]);
                        txtBNR46.Text = Convert.ToString(dt1.Rows[0]["BNR46"]);
                        txtBNR47.Text = Convert.ToString(dt1.Rows[0]["BNR47"]);
                        txtBNR51.Text = Convert.ToString(dt1.Rows[0]["BNR48"]);
                        txtBNR52.Text = Convert.ToString(dt1.Rows[0]["BNR49"]);

                        txtBNR48.Text = Convert.ToString(dt1.Rows[0]["BNR50"]);
                        txtBNR49.Text = Convert.ToString(dt1.Rows[0]["BNR51"]);
                        txtBNR50.Text = Convert.ToString(dt1.Rows[0]["BNR52"]);

                        txtBRem1.Text = Convert.ToString(dt1.Rows[0]["BRem1"]);
                        txtBRem2.Text = Convert.ToString(dt1.Rows[0]["BRem2"]);
                        txtBRem3.Text = Convert.ToString(dt1.Rows[0]["BRem3"]);
                        txtBRem4.Text = Convert.ToString(dt1.Rows[0]["BRem4"]);
                        txtBRem5.Text = Convert.ToString(dt1.Rows[0]["BRem5"]);
                        txtBRem6.Text = Convert.ToString(dt1.Rows[0]["BRem6"]);
                        txtBRem7.Text = Convert.ToString(dt1.Rows[0]["BRem7"]);
                        txtBRem8.Text = Convert.ToString(dt1.Rows[0]["BRem8"]);
                        txtBRem9.Text = Convert.ToString(dt1.Rows[0]["BRem9"]);
                        txtBRem10.Text = Convert.ToString(dt1.Rows[0]["BRem10"]);
                        txtBRem11.Text = Convert.ToString(dt1.Rows[0]["BRem11"]);
                        txtBRem12.Text = Convert.ToString(dt1.Rows[0]["BRem12"]);
                        txtBRem13.Text = Convert.ToString(dt1.Rows[0]["BRem13"]);
                        txtBRem14.Text = Convert.ToString(dt1.Rows[0]["BRem14"]);
                        txtBRem15.Text = Convert.ToString(dt1.Rows[0]["BRem15"]);
                        txtBRem16.Text = Convert.ToString(dt1.Rows[0]["BRem16"]);
                        txtBRem17.Text = Convert.ToString(dt1.Rows[0]["BRem17"]);
                        txtBRem18.Text = Convert.ToString(dt1.Rows[0]["BRem18"]);
                        txtBRem19.Text = Convert.ToString(dt1.Rows[0]["BRem19"]);
                        txtBRem20.Text = Convert.ToString(dt1.Rows[0]["BRem20"]);
                        txtBRem21.Text = Convert.ToString(dt1.Rows[0]["BRem21"]);
                        txtBRem22.Text = Convert.ToString(dt1.Rows[0]["BRem22"]);
                        txtBRem23.Text = Convert.ToString(dt1.Rows[0]["BRem23"]);
                        txtBRem24.Text = Convert.ToString(dt1.Rows[0]["BRem24"]);
                        txtBRem25.Text = Convert.ToString(dt1.Rows[0]["BRem25"]);
                        txtBRem26.Text = Convert.ToString(dt1.Rows[0]["BRem26"]);
                        txtBRem27.Text = Convert.ToString(dt1.Rows[0]["BRem27"]);
                        txtBRem28.Text = Convert.ToString(dt1.Rows[0]["BRem28"]);
                        txtBRem29.Text = Convert.ToString(dt1.Rows[0]["BRem29"]);
                        txtBRem30.Text = Convert.ToString(dt1.Rows[0]["BRem30"]);
                        txtBRem31.Text = Convert.ToString(dt1.Rows[0]["BRem31"]);
                        txtBRem32.Text = Convert.ToString(dt1.Rows[0]["BRem32"]);
                        txtBRem33.Text = Convert.ToString(dt1.Rows[0]["BRem33"]);
                        txtBRem34.Text = Convert.ToString(dt1.Rows[0]["BRem34"]);
                        txtBRem35.Text = Convert.ToString(dt1.Rows[0]["BRem35"]);
                        txtBRem36.Text = Convert.ToString(dt1.Rows[0]["BRem36"]);
                        txtBRem37.Text = Convert.ToString(dt1.Rows[0]["BRem37"]);
                        txtBRem38.Text = Convert.ToString(dt1.Rows[0]["BRem38"]);
                        txtBRem39.Text = Convert.ToString(dt1.Rows[0]["BRem39"]);
                        txtBRem40.Text = Convert.ToString(dt1.Rows[0]["BRem40"]);
                        txtBRem41.Text = Convert.ToString(dt1.Rows[0]["BRem41"]);
                        txtBRem42.Text = Convert.ToString(dt1.Rows[0]["BRem42"]);
                        txtBRem43.Text = Convert.ToString(dt1.Rows[0]["BRem43"]);
                        txtBRem44.Text = Convert.ToString(dt1.Rows[0]["BRem44"]);
                        txtBRem45.Text = Convert.ToString(dt1.Rows[0]["BRem45"]);
                        txtBRem46.Text = Convert.ToString(dt1.Rows[0]["BRem46"]);
                        txtBRem47.Text = Convert.ToString(dt1.Rows[0]["BRem47"]);
                        txtBRem51.Text = Convert.ToString(dt1.Rows[0]["BRem48"]);
                        txtBRem52.Text = Convert.ToString(dt1.Rows[0]["BRem49"]);

                        txtBRem48.Text = Convert.ToString(dt1.Rows[0]["BRem50"]);
                        txtBRem49.Text = Convert.ToString(dt1.Rows[0]["BRem51"]);
                        txtBRem50.Text = Convert.ToString(dt1.Rows[0]["BRem52"]);

                        tbEmp.ActiveTabIndex = 1;
                        StatusButton("Show");
                        EnableControl(false);
                        btnAdd.Enabled = false;
                    }
                    else
                    {
                        ClearControls();
                        tbEmp.ActiveTabIndex = 1;
                        StatusButton("Show");
                        EnableControl(false);
                        btnEdit.Enabled = false;
                    }
                }
            }
            finally
            {
                oAu = null;
                ds = null;
                dt = null; dt1 = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Int32 vBR1 = 0, vBR2 = 0, vBR3 = 0, vBR4 = 0, vBR5 = 0, vBR6 = 0, vBR7 = 0, vBR8 = 0, vBR9 = 0, vBR10 = 0,
                vBR11 = 0, vBR12 = 0, vBR13 = 0, vBR14 = 0, vBR15 = 0, vBR16 = 0, vBR17 = 0, vBR18 = 0, vBR19 = 0,
                vBR20 = 0, vBR21 = 0, vBR22 = 0, vBR23 = 0, vBR24 = 0, vBR25 = 0, vBR26 = 0, vBR27 = 0, vBR28 = 0,
                vBR29 = 0, vBR30 = 0, vBR31 = 0, vBR32 = 0, vBR33 = 0, vBR34 = 0, vBR35 = 0, vBR36 = 0, vBR37 = 0,
                vBR38 = 0, vBR39 = 0, vBR40 = 0, vBR41 = 0, vBR42 = 0, vBR43 = 0, vBR44 = 0, vBR45 = 0, vBR46 = 0,
                vBR47 = 0, vBR48 = 0, vBR49 = 0, vBR50 = 0, vBR51 = 0, vBR52 = 0;

            Int32 vBPR1 = 0, vBPR2 = 0, vBPR3 = 0, vBPR4 = 0, vBPR5 = 0, vBPR6 = 0, vBPR7 = 0, vBPR8 = 0, vBPR9 = 0,
                vBPR10 = 0, vBPR11 = 0, vBPR12 = 0, vBPR13 = 0, vBPR14 = 0, vBPR15 = 0, vBPR16 = 0, vBPR17 = 0,
                vBPR18 = 0, vBPR19 = 0, vBPR20 = 0, vBPR21 = 0, vBPR22 = 0, vBPR23 = 0, vBPR24 = 0, vBPR25 = 0,
                vBPR26 = 0, vBPR27 = 0, vBPR28 = 0, vBPR29 = 0, vBPR30 = 0, vBPR31 = 0, vBPR32 = 0, vBPR33 = 0,
                vBPR34 = 0, vBPR35 = 0, vBPR36 = 0, vBPR37 = 0, vBPR38 = 0, vBPR39 = 0, vBPR40 = 0, vBPR41 = 0,
                vBPR42 = 0, vBPR43 = 0, vBPR44 = 0, vBPR45 = 0, vBPR46 = 0, vBPR47 = 0, vBPR48 = 0, vBPR49 = 0,
                vBPR50 = 0, vBPR51 = 0, vBPR52 = 0;

            Int32 vBNR1 = 0, vBNR2 = 0, vBNR3 = 0, vBNR4 = 0, vBNR5 = 0, vBNR6 = 0, vBNR7 = 0, vBNR8 = 0, vBNR9 = 0,
                vBNR10 = 0, vBNR11 = 0, vBNR12 = 0, vBNR13 = 0, vBNR14 = 0, vBNR15 = 0, vBNR16 = 0, vBNR17 = 0,
                vBNR18 = 0, vBNR19 = 0, vBNR20 = 0, vBNR21 = 0, vBNR22 = 0, vBNR23 = 0, vBNR24 = 0, vBNR25 = 0,
                vBNR26 = 0, vBNR27 = 0, vBNR28 = 0, vBNR29 = 0, vBNR30 = 0, vBNR31 = 0, vBNR32 = 0, vBNR33 = 0,
                vBNR34 = 0, vBNR35 = 0, vBNR36 = 0, vBNR37 = 0, vBNR38 = 0, vBNR39 = 0, vBNR40 = 0, vBNR41 = 0,
                vBNR42 = 0, vBNR43 = 0, vBNR44 = 0, vBNR45 = 0, vBNR46 = 0, vBNR47 = 0, vBNR48 = 0, vBNR49 = 0,
                vBNR50 = 0, vBNR51 = 0, vBNR52 = 0;

            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vInsPecId = 0, vErr = 0, vUsrID = 0;
            DateTime vSubDt = gblFuction.setDate(txtSDt.Text);
            DateTime vCmplDt = gblFuction.setDate(txtCmpDt.Text);
            DateTime vCrStrDt = gblFuction.setDate(txtCrStrDt.Text);
            DateTime vCrEndDt = gblFuction.setDate(txtCrEndDt.Text);
            DateTime vPrStrDt = gblFuction.setDate(txtPrStrDt.Text);
            DateTime vPrEndDt = gblFuction.setDate(txtPrEndDt.Text);
            TimeSpan vTsp = (vSubDt - vPrStrDt);
            CIntIspPM oAu = null;

            if (vSubDt > vCmplDt)
            {
                gblFuction.MsgPopup("Compliance Date should be > Submission Date.");
                vResult = false;
            }

            if (Session[gblValue.EndDate] != null)
            {
                if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtCmpDt.Text))
                {
                    gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                    return false;
                }
            }

            try
            {
                vUsrID = Int32.Parse(Session[gblValue.UserId].ToString());

                if (ViewState["InsPecId"] != null)
                    vInsPecId = Convert.ToInt32(Convert.ToString(ViewState["InsPecId"]));

                if (txtBR1.Text != "")
                    vBR1 = Convert.ToInt32(txtBR1.Text);
                if (txtBR2.Text != "")
                    vBR2 = Convert.ToInt32(txtBR2.Text);
                if (txtBR3.Text != "")
                    vBR3 = Convert.ToInt32(txtBR3.Text);
                if (txtBR4.Text != "")
                    vBR4 = Convert.ToInt32(txtBR4.Text);
                if (txtBR5.Text != "")
                    vBR5 = Convert.ToInt32(txtBR5.Text);
                if (txtBR6.Text != "")
                    vBR6 = Convert.ToInt32(txtBR6.Text);
                if (txtBR7.Text != "")
                    vBR7 = Convert.ToInt32(txtBR7.Text);
                if (txtBR8.Text != "")
                    vBR8 = Convert.ToInt32(txtBR8.Text);
                if (txtBR9.Text != "")
                    vBR9 = Convert.ToInt32(txtBR9.Text);
                if (txtBR10.Text != "")
                    vBR10 = Convert.ToInt32(txtBR10.Text);
                if (txtBR11.Text != "")
                    vBR11 = Convert.ToInt32(txtBR11.Text);
                if (txtBR12.Text != "")
                    vBR12 = Convert.ToInt32(txtBR12.Text);
                if (txtBR13.Text != "")
                    vBR13 = Convert.ToInt32(txtBR13.Text);
                if (txtBR14.Text != "")
                    vBR14 = Convert.ToInt32(txtBR14.Text);
                if (txtBR15.Text != "")
                    vBR15 = Convert.ToInt32(txtBR15.Text);
                if (txtBR16.Text != "")
                    vBR16 = Convert.ToInt32(txtBR16.Text);
                if (txtBR17.Text != "")
                    vBR17 = Convert.ToInt32(txtBR17.Text);
                if (txtBR18.Text != "")
                    vBR18 = Convert.ToInt32(txtBR18.Text);
                if (txtBR19.Text != "")
                    vBR19 = Convert.ToInt32(txtBR19.Text);
                if (txtBR20.Text != "")
                    vBR20 = Convert.ToInt32(txtBR20.Text);
                if (txtBR21.Text != "")
                    vBR21 = Convert.ToInt32(txtBR21.Text);
                if (txtBR22.Text != "")
                    vBR22 = Convert.ToInt32(txtBR22.Text);
                if (txtBR23.Text != "")
                    vBR23 = Convert.ToInt32(txtBR23.Text);
                if (txtBR24.Text != "")
                    vBR24 = Convert.ToInt32(txtBR24.Text);
                if (txtBR25.Text != "")
                    vBR25 = Convert.ToInt32(txtBR25.Text);
                if (txtBR26.Text != "")
                    vBR26 = Convert.ToInt32(txtBR26.Text);
                if (txtBR27.Text != "")
                    vBR27 = Convert.ToInt32(txtBR27.Text);
                if (txtBR28.Text != "")
                    vBR28 = Convert.ToInt32(txtBR28.Text);
                if (txtBR29.Text != "")
                    vBR29 = Convert.ToInt32(txtBR29.Text);
                if (txtBR30.Text != "")
                    vBR30 = Convert.ToInt32(txtBR30.Text);
                if (txtBR31.Text != "")
                    vBR31 = Convert.ToInt32(txtBR31.Text);
                if (txtBR32.Text != "")
                    vBR32 = Convert.ToInt32(txtBR32.Text);
                if (txtBR33.Text != "")
                    vBR33 = Convert.ToInt32(txtBR33.Text);
                if (txtBR34.Text != "")
                    vBR34 = Convert.ToInt32(txtBR34.Text);
                if (txtBR35.Text != "")
                    vBR35 = Convert.ToInt32(txtBR35.Text);
                if (txtBR36.Text != "")
                    vBR36 = Convert.ToInt32(txtBR36.Text);
                if (txtBR37.Text != "")
                    vBR37 = Convert.ToInt32(txtBR37.Text);
                if (txtBR38.Text != "")
                    vBR38 = Convert.ToInt32(txtBR38.Text);
                if (txtBR39.Text != "")
                    vBR39 = Convert.ToInt32(txtBR39.Text);
                if (txtBR40.Text != "")
                    vBR40 = Convert.ToInt32(txtBR40.Text);
                if (txtBR41.Text != "")
                    vBR41 = Convert.ToInt32(txtBR41.Text);
                if (txtBR42.Text != "")
                    vBR42 = Convert.ToInt32(txtBR42.Text);
                if (txtBR43.Text != "")
                    vBR43 = Convert.ToInt32(txtBR43.Text);
                if (txtBR44.Text != "")
                    vBR44 = Convert.ToInt32(txtBR44.Text);
                if (txtBR45.Text != "")
                    vBR45 = Convert.ToInt32(txtBR45.Text);
                if (txtBR46.Text != "")
                    vBR46 = Convert.ToInt32(txtBR46.Text);
                if (txtBR47.Text != "")
                    vBR47 = Convert.ToInt32(txtBR47.Text);
                if (txtBR48.Text != "")
                    vBR48 = Convert.ToInt32(txtBR48.Text);
                if (txtBR49.Text != "")
                    vBR49 = Convert.ToInt32(txtBR49.Text);

                if (txtBR50.Text != "")
                    vBR50 = Convert.ToInt32(txtBR50.Text);
                if (txtBR51.Text != "")
                    vBR51 = Convert.ToInt32(txtBR51.Text);
                if (txtBR52.Text != "")
                    vBR52 = Convert.ToInt32(txtBR52.Text);

                if (txtBPR1.Text != "")
                    vBPR1 = Convert.ToInt32(txtBPR1.Text);
                if (txtBPR2.Text != "")
                    vBPR2 = Convert.ToInt32(txtBPR2.Text);
                if (txtBPR3.Text != "")
                    vBPR3 = Convert.ToInt32(txtBPR3.Text);
                if (txtBPR4.Text != "")
                    vBPR4 = Convert.ToInt32(txtBPR4.Text);
                if (txtBPR5.Text != "")
                    vBPR5 = Convert.ToInt32(txtBPR5.Text);
                if (txtBPR6.Text != "")
                    vBPR6 = Convert.ToInt32(txtBPR6.Text);
                if (txtBPR7.Text != "")
                    vBPR7 = Convert.ToInt32(txtBPR7.Text);
                if (txtBPR8.Text != "")
                    vBPR8 = Convert.ToInt32(txtBPR8.Text);
                if (txtBPR9.Text != "")
                    vBPR9 = Convert.ToInt32(txtBPR9.Text);
                if (txtBPR10.Text != "")
                    vBPR10 = Convert.ToInt32(txtBPR10.Text);
                if (txtBPR11.Text != "")
                    vBPR11 = Convert.ToInt32(txtBPR11.Text);
                if (txtBPR12.Text != "")
                    vBPR12 = Convert.ToInt32(txtBPR12.Text);
                if (txtBPR13.Text != "")
                    vBPR13 = Convert.ToInt32(txtBPR13.Text);
                if (txtBPR14.Text != "")
                    vBPR14 = Convert.ToInt32(txtBPR14.Text);
                if (txtBPR15.Text != "")
                    vBPR15 = Convert.ToInt32(txtBPR15.Text);
                if (txtBPR16.Text != "")
                    vBPR16 = Convert.ToInt32(txtBPR16.Text);
                if (txtBPR17.Text != "")
                    vBPR17 = Convert.ToInt32(txtBPR17.Text);
                if (txtBPR18.Text != "")
                    vBPR18 = Convert.ToInt32(txtBPR18.Text);
                if (txtBPR19.Text != "")
                    vBPR19 = Convert.ToInt32(txtBPR19.Text);
                if (txtBPR20.Text != "")
                    vBPR20 = Convert.ToInt32(txtBPR20.Text);
                if (txtBPR21.Text != "")
                    vBPR21 = Convert.ToInt32(txtBPR21.Text);
                if (txtBPR22.Text != "")
                    vBPR22 = Convert.ToInt32(txtBPR22.Text);
                if (txtBPR23.Text != "")
                    vBPR23 = Convert.ToInt32(txtBPR23.Text);
                if (txtBPR24.Text != "")
                    vBPR24 = Convert.ToInt32(txtBPR24.Text);
                if (txtBPR25.Text != "")
                    vBPR25 = Convert.ToInt32(txtBPR25.Text);
                if (txtBPR26.Text != "")
                    vBPR26 = Convert.ToInt32(txtBPR26.Text);
                if (txtBPR27.Text != "")
                    vBPR27 = Convert.ToInt32(txtBPR27.Text);
                if (txtBPR28.Text != "")
                    vBPR28 = Convert.ToInt32(txtBPR28.Text);
                if (txtBPR29.Text != "")
                    vBPR29 = Convert.ToInt32(txtBPR29.Text);
                if (txtBPR30.Text != "")
                    vBPR30 = Convert.ToInt32(txtBPR30.Text);
                if (txtBPR31.Text != "")
                    vBPR31 = Convert.ToInt32(txtBPR31.Text);
                if (txtBPR32.Text != "")
                    vBPR32 = Convert.ToInt32(txtBPR32.Text);
                if (txtBPR33.Text != "")
                    vBPR33 = Convert.ToInt32(txtBPR33.Text);
                if (txtBPR34.Text != "")
                    vBPR34 = Convert.ToInt32(txtBPR34.Text);
                if (txtBPR35.Text != "")
                    vBPR35 = Convert.ToInt32(txtBPR35.Text);
                if (txtBPR36.Text != "")
                    vBPR36 = Convert.ToInt32(txtBPR36.Text);
                if (txtBPR37.Text != "")
                    vBPR37 = Convert.ToInt32(txtBPR37.Text);
                if (txtBPR38.Text != "")
                    vBPR38 = Convert.ToInt32(txtBPR38.Text);
                if (txtBPR39.Text != "")
                    vBPR39 = Convert.ToInt32(txtBPR39.Text);
                if (txtBPR40.Text != "")
                    vBPR40 = Convert.ToInt32(txtBPR40.Text);
                if (txtBPR41.Text != "")
                    vBPR41 = Convert.ToInt32(txtBPR41.Text);
                if (txtBPR42.Text != "")
                    vBPR42 = Convert.ToInt32(txtBPR42.Text);
                if (txtBPR43.Text != "")
                    vBPR43 = Convert.ToInt32(txtBPR43.Text);
                if (txtBPR44.Text != "")
                    vBPR44 = Convert.ToInt32(txtBPR44.Text);
                if (txtBPR45.Text != "")
                    vBPR45 = Convert.ToInt32(txtBPR45.Text);
                if (txtBPR46.Text != "")
                    vBPR46 = Convert.ToInt32(txtBPR46.Text);
                if (txtBPR47.Text != "")
                    vBPR47 = Convert.ToInt32(txtBPR47.Text);
                if (txtBPR48.Text != "")
                    vBPR48 = Convert.ToInt32(txtBPR48.Text);
                if (txtBPR49.Text != "")
                    vBPR49 = Convert.ToInt32(txtBPR49.Text);

                if (txtBPR50.Text != "")
                    vBPR50 = Convert.ToInt32(txtBPR50.Text);
                if (txtBPR51.Text != "")
                    vBPR51 = Convert.ToInt32(txtBPR51.Text);
                if (txtBPR52.Text != "")
                    vBPR52 = Convert.ToInt32(txtBPR52.Text);

                ////////////////////////////////////////////////////////

                if (txtBNR1.Text != "")
                    vBNR1 = Convert.ToInt32(txtBNR1.Text);
                if (txtBNR2.Text != "")
                    vBNR2 = Convert.ToInt32(txtBNR2.Text);
                if (txtBNR3.Text != "")
                    vBNR3 = Convert.ToInt32(txtBNR3.Text);
                if (txtBNR4.Text != "")
                    vBNR4 = Convert.ToInt32(txtBNR4.Text);
                if (txtBNR5.Text != "")
                    vBNR5 = Convert.ToInt32(txtBNR5.Text);
                if (txtBNR6.Text != "")
                    vBNR6 = Convert.ToInt32(txtBNR6.Text);
                if (txtBNR7.Text != "")
                    vBNR7 = Convert.ToInt32(txtBNR7.Text);
                if (txtBNR8.Text != "")
                    vBNR8 = Convert.ToInt32(txtBNR8.Text);
                if (txtBNR9.Text != "")
                    vBNR9 = Convert.ToInt32(txtBNR9.Text);
                if (txtBNR10.Text != "")
                    vBNR10 = Convert.ToInt32(txtBNR10.Text);
                if (txtBNR11.Text != "")
                    vBNR11 = Convert.ToInt32(txtBNR11.Text);
                if (txtBNR12.Text != "")
                    vBNR12 = Convert.ToInt32(txtBNR12.Text);
                if (txtBNR13.Text != "")
                    vBNR13 = Convert.ToInt32(txtBNR13.Text);
                if (txtBNR14.Text != "")
                    vBNR14 = Convert.ToInt32(txtBNR14.Text);
                if (txtBNR15.Text != "")
                    vBNR15 = Convert.ToInt32(txtBNR15.Text);
                if (txtBNR16.Text != "")
                    vBNR16 = Convert.ToInt32(txtBNR16.Text);
                if (txtBNR17.Text != "")
                    vBNR17 = Convert.ToInt32(txtBNR17.Text);
                if (txtBNR18.Text != "")
                    vBNR18 = Convert.ToInt32(txtBNR18.Text);
                if (txtBNR19.Text != "")
                    vBNR19 = Convert.ToInt32(txtBNR19.Text);
                if (txtBNR20.Text != "")
                    vBNR20 = Convert.ToInt32(txtBNR20.Text);
                if (txtBNR21.Text != "")
                    vBNR21 = Convert.ToInt32(txtBNR21.Text);
                if (txtBNR22.Text != "")
                    vBNR22 = Convert.ToInt32(txtBNR22.Text);
                if (txtBNR23.Text != "")
                    vBNR23 = Convert.ToInt32(txtBNR23.Text);
                if (txtBNR24.Text != "")
                    vBNR24 = Convert.ToInt32(txtBNR24.Text);
                if (txtBNR25.Text != "")
                    vBNR25 = Convert.ToInt32(txtBNR25.Text);
                if (txtBNR26.Text != "")
                    vBNR26 = Convert.ToInt32(txtBNR26.Text);
                if (txtBNR27.Text != "")
                    vBNR27 = Convert.ToInt32(txtBNR27.Text);
                if (txtBNR28.Text != "")
                    vBNR28 = Convert.ToInt32(txtBNR28.Text);
                if (txtBNR29.Text != "")
                    vBNR29 = Convert.ToInt32(txtBNR29.Text);
                if (txtBNR30.Text != "")
                    vBNR30 = Convert.ToInt32(txtBNR30.Text);
                if (txtBNR31.Text != "")
                    vBNR31 = Convert.ToInt32(txtBNR31.Text);
                if (txtBNR32.Text != "")
                    vBNR32 = Convert.ToInt32(txtBNR32.Text);
                if (txtBNR33.Text != "")
                    vBNR33 = Convert.ToInt32(txtBNR33.Text);
                if (txtBNR34.Text != "")
                    vBNR34 = Convert.ToInt32(txtBNR34.Text);
                if (txtBNR35.Text != "")
                    vBNR35 = Convert.ToInt32(txtBNR35.Text);
                if (txtBNR36.Text != "")
                    vBNR36 = Convert.ToInt32(txtBNR36.Text);
                if (txtBNR37.Text != "")
                    vBNR37 = Convert.ToInt32(txtBNR37.Text);
                if (txtBNR38.Text != "")
                    vBNR38 = Convert.ToInt32(txtBNR38.Text);
                if (txtBNR39.Text != "")
                    vBNR39 = Convert.ToInt32(txtBNR39.Text);
                if (txtBNR40.Text != "")
                    vBNR40 = Convert.ToInt32(txtBNR40.Text);
                if (txtBNR41.Text != "")
                    vBNR41 = Convert.ToInt32(txtBNR41.Text);
                if (txtBNR42.Text != "")
                    vBNR42 = Convert.ToInt32(txtBNR42.Text);
                if (txtBNR43.Text != "")
                    vBNR43 = Convert.ToInt32(txtBNR43.Text);
                if (txtBNR44.Text != "")
                    vBNR44 = Convert.ToInt32(txtBNR44.Text);
                if (txtBNR45.Text != "")
                    vBNR45 = Convert.ToInt32(txtBNR45.Text);
                if (txtBNR46.Text != "")
                    vBNR46 = Convert.ToInt32(txtBNR46.Text);
                if (txtBNR47.Text != "")
                    vBNR47 = Convert.ToInt32(txtBNR47.Text);
                if (txtBNR48.Text != "")
                    vBNR48 = Convert.ToInt32(txtBNR48.Text);
                if (txtBNR49.Text != "")
                    vBNR49 = Convert.ToInt32(txtBNR49.Text);

                if (txtBNR50.Text != "")
                    vBNR50 = Convert.ToInt32(txtBNR50.Text);
                if (txtBNR51.Text != "")
                    vBNR51 = Convert.ToInt32(txtBNR51.Text);
                if (txtBNR52.Text != "")
                    vBNR52 = Convert.ToInt32(txtBNR52.Text);

                if (Mode == "Save")
                {
                    oAu = new CIntIspPM();

                    vErr = oAu.SaveBranchCompliance(vInsPecId, vCmplDt,
                        vBR1, vBR2, vBR3, vBR4, vBR5, vBR6, vBR7, vBR8, vBR9, vBR10, vBR11, vBR12, vBR13, vBR14, vBR15,
                        vBR16, vBR17, vBR18, vBR19, vBR20, vBR21, vBR22, vBR23, vBR24, vBR25, vBR26, vBR27, vBR28,
                        vBR29, vBR30, vBR31, vBR32, vBR33, vBR34, vBR35, vBR36, vBR37, vBR38, vBR39, vBR40, vBR41,
                        vBR42, vBR43, vBR44, vBR45, vBR46, vBR47, vBR51, vBR52, vBR48, vBR49, vBR50,
                        vBPR1, vBPR2, vBPR3, vBPR4, vBPR5, vBPR6, vBPR7, vBPR8, vBPR9, vBPR10, vBPR11, vBPR12, vBPR13,
                        vBPR14, vBPR15, vBPR16, vBPR17, vBPR18, vBPR19, vBPR20, vBPR21, vBPR22, vBPR23, vBPR24, vBPR25,
                        vBPR26, vBPR27, vBPR28, vBPR29, vBPR30, vBPR31, vBPR32, vBPR33, vBPR34, vBPR35, vBPR36, vBPR37,
                        vBPR38, vBPR39, vBPR40, vBPR41, vBPR42, vBPR43, vBPR44, vBPR45, vBPR46, vBPR47, vBPR51, vBPR52,
                        vBPR48, vBPR49, vBPR50,
                        vBNR1, vBNR2, vBNR3, vBNR4, vBNR5, vBNR6, vBNR7, vBNR8, vBNR9, vBNR10, vBNR11, vBNR12, vBNR13,
                        vBNR14, vBNR15, vBNR16, vBNR17, vBNR18, vBNR19, vBNR20, vBNR21, vBNR22, vBNR23, vBNR24, vBNR25,
                        vBNR26, vBNR27, vBNR28, vBNR29, vBNR30, vBNR31, vBNR32, vBNR33, vBNR34, vBNR35, vBNR36, vBNR37,
                        vBNR38, vBNR39, vBNR40, vBNR41, vBNR42, vBNR43, vBNR44, vBNR45, vBNR46, vBNR47, vBNR51, vBNR52,
                        vBNR48, vBNR49, vBNR50,
                        txtBRem1.Text.Replace("'", "''"), txtBRem2.Text.Replace("'", "''"), txtBRem3.Text.Replace("'", "''")
                        , txtBRem4.Text.Replace("'", "''"), txtBRem5.Text.Replace("'", "''"), txtBRem6.Text.Replace("'", "''")
                        , txtBRem7.Text.Replace("'", "''"), txtBRem8.Text.Replace("'", "''"), txtBRem9.Text.Replace("'", "''")
                        , txtBRem10.Text.Replace("'", "''"), txtBRem11.Text.Replace("'", "''"), txtBRem12.Text.Replace("'", "''")
                        , txtBRem13.Text.Replace("'", "''"), txtBRem14.Text.Replace("'", "''"), txtBRem15.Text.Replace("'", "''")
                        , txtBRem16.Text.Replace("'", "''"), txtBRem17.Text.Replace("'", "''"), txtBRem18.Text.Replace("'", "''")
                        , txtBRem19.Text.Replace("'", "''"), txtBRem20.Text.Replace("'", "''"), txtBRem21.Text.Replace("'", "''")
                        , txtBRem22.Text.Replace("'", "''"), txtBRem23.Text.Replace("'", "''"), txtBRem24.Text.Replace("'", "''")
                        , txtBRem25.Text.Replace("'", "''"), txtBRem26.Text.Replace("'", "''"), txtBRem27.Text.Replace("'", "''")
                        , txtBRem28.Text.Replace("'", "''"), txtBRem29.Text.Replace("'", "''"), txtBRem30.Text.Replace("'", "''")
                        , txtBRem31.Text.Replace("'", "''"), txtBRem32.Text.Replace("'", "''"), txtBRem33.Text.Replace("'", "''")
                        , txtBRem34.Text.Replace("'", "''"), txtBRem35.Text.Replace("'", "''"), txtBRem36.Text.Replace("'", "''")
                        , txtBRem37.Text.Replace("'", "''"), txtBRem38.Text.Replace("'", "''"), txtBRem39.Text.Replace("'", "''")
                        , txtBRem40.Text.Replace("'", "''"), txtBRem41.Text.Replace("'", "''"), txtBRem42.Text.Replace("'", "''")
                        , txtBRem43.Text.Replace("'", "''"), txtBRem44.Text.Replace("'", "''"), txtBRem45.Text.Replace("'", "''")
                        , txtBRem46.Text.Replace("'", "''"), txtBRem47.Text.Replace("'", "''"), txtBRem51.Text.Replace("'", "''"),
                        txtBRem52.Text.Replace("'", "''"), txtBRem48.Text.Replace("'", "''"), txtBRem49.Text.Replace("'", "''"),
                        txtBRem50.Text.Replace("'", "''"), vUsrID, "Save");

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

                    vErr = oAu.SaveBranchCompliance(vInsPecId, vCmplDt,
                        vBR1, vBR2, vBR3, vBR4, vBR5, vBR6, vBR7, vBR8, vBR9, vBR10, vBR11, vBR12, vBR13, vBR14, vBR15,
                        vBR16, vBR17, vBR18, vBR19, vBR20, vBR21, vBR22, vBR23, vBR24, vBR25, vBR26, vBR27, vBR28,
                        vBR29, vBR30, vBR31, vBR32, vBR33, vBR34, vBR35, vBR36, vBR37, vBR38, vBR39, vBR40, vBR41,
                        vBR42, vBR43, vBR44, vBR45, vBR46, vBR47, vBR51, vBR52, vBR48, vBR49, vBR50,
                        vBPR1, vBPR2, vBPR3, vBPR4, vBPR5, vBPR6, vBPR7, vBPR8, vBPR9, vBPR10, vBPR11, vBPR12, vBPR13,
                        vBPR14, vBPR15, vBPR16, vBPR17, vBPR18, vBPR19, vBPR20, vBPR21, vBPR22, vBPR23, vBPR24, vBPR25,
                        vBPR26, vBPR27, vBPR28, vBPR29, vBPR30, vBPR31, vBPR32, vBPR33, vBPR34, vBPR35, vBPR36, vBPR37,
                        vBPR38, vBPR39, vBPR40, vBPR41, vBPR42, vBPR43, vBPR44, vBPR45, vBPR46, vBPR47, vBPR51, vBPR52,
                        vBPR48, vBPR49, vBPR50,
                        vBNR1, vBNR2, vBNR3, vBNR4, vBNR5, vBNR6, vBNR7, vBNR8, vBNR9, vBNR10, vBNR11, vBNR12, vBNR13,
                        vBNR14, vBNR15, vBNR16, vBNR17, vBNR18, vBNR19, vBNR20, vBNR21, vBNR22, vBNR23, vBNR24, vBNR25,
                        vBNR26, vBNR27, vBNR28, vBNR29, vBNR30, vBNR31, vBNR32, vBNR33, vBNR34, vBNR35, vBNR36, vBNR37,
                        vBNR38, vBNR39, vBNR40, vBNR41, vBNR42, vBNR43, vBNR44, vBNR45, vBNR46, vBNR47, vBNR51, vBNR52,
                        vBNR48, vBNR49, vBNR50,
                        txtBRem1.Text.Replace("'", "''"), txtBRem2.Text.Replace("'", "''"), txtBRem3.Text.Replace("'", "''")
                        , txtBRem4.Text.Replace("'", "''"), txtBRem5.Text.Replace("'", "''"), txtBRem6.Text.Replace("'", "''")
                        , txtBRem7.Text.Replace("'", "''"), txtBRem8.Text.Replace("'", "''"), txtBRem9.Text.Replace("'", "''")
                        , txtBRem10.Text.Replace("'", "''"), txtBRem11.Text.Replace("'", "''"), txtBRem12.Text.Replace("'", "''")
                        , txtBRem13.Text.Replace("'", "''"), txtBRem14.Text.Replace("'", "''"), txtBRem15.Text.Replace("'", "''")
                        , txtBRem16.Text.Replace("'", "''"), txtBRem17.Text.Replace("'", "''"), txtBRem18.Text.Replace("'", "''")
                        , txtBRem19.Text.Replace("'", "''"), txtBRem20.Text.Replace("'", "''"), txtBRem21.Text.Replace("'", "''")
                        , txtBRem22.Text.Replace("'", "''"), txtBRem23.Text.Replace("'", "''"), txtBRem24.Text.Replace("'", "''")
                        , txtBRem25.Text.Replace("'", "''"), txtBRem26.Text.Replace("'", "''"), txtBRem27.Text.Replace("'", "''")
                        , txtBRem28.Text.Replace("'", "''"), txtBRem29.Text.Replace("'", "''"), txtBRem30.Text.Replace("'", "''")
                        , txtBRem31.Text.Replace("'", "''"), txtBRem32.Text.Replace("'", "''"), txtBRem33.Text.Replace("'", "''")
                        , txtBRem34.Text.Replace("'", "''"), txtBRem35.Text.Replace("'", "''"), txtBRem36.Text.Replace("'", "''")
                        , txtBRem37.Text.Replace("'", "''"), txtBRem38.Text.Replace("'", "''"), txtBRem39.Text.Replace("'", "''")
                        , txtBRem40.Text.Replace("'", "''"), txtBRem41.Text.Replace("'", "''"), txtBRem42.Text.Replace("'", "''")
                        , txtBRem43.Text.Replace("'", "''"), txtBRem44.Text.Replace("'", "''"), txtBRem45.Text.Replace("'", "''")
                        , txtBRem46.Text.Replace("'", "''"), txtBRem47.Text.Replace("'", "''"), txtBRem51.Text.Replace("'", "''"),
                        txtBRem52.Text.Replace("'", "''"), txtBRem48.Text.Replace("'", "''"), txtBRem49.Text.Replace("'", "''"),
                        txtBRem50.Text.Replace("'", "''"), vUsrID, "Edit");
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

                    vErr = oAu.SaveBranchCompliance(vInsPecId, vCmplDt,
                        vBR1, vBR2, vBR3, vBR4, vBR5, vBR6, vBR7, vBR8, vBR9, vBR10, vBR11, vBR12, vBR13, vBR14, vBR15,
                        vBR16, vBR17, vBR18, vBR19, vBR20, vBR21, vBR22, vBR23, vBR24, vBR25, vBR26, vBR27, vBR28,
                        vBR29, vBR30, vBR31, vBR32, vBR33, vBR34, vBR35, vBR36, vBR37, vBR38, vBR39, vBR40, vBR41,
                        vBR42, vBR43, vBR44, vBR45, vBR46, vBR47, vBR51, vBR52, vBR48, vBR49, vBR50,
                        vBPR1, vBPR2, vBPR3, vBPR4, vBPR5, vBPR6, vBPR7, vBPR8, vBPR9, vBPR10, vBPR11, vBPR12, vBPR13,
                        vBPR14, vBPR15, vBPR16, vBPR17, vBPR18, vBPR19, vBPR20, vBPR21, vBPR22, vBPR23, vBPR24, vBPR25,
                        vBPR26, vBPR27, vBPR28, vBPR29, vBPR30, vBPR31, vBPR32, vBPR33, vBPR34, vBPR35, vBPR36, vBPR37,
                        vBPR38, vBPR39, vBPR40, vBPR41, vBPR42, vBPR43, vBPR44, vBPR45, vBPR46, vBPR47, vBPR51, vBPR52,
                        vBPR48, vBPR49, vBPR50,
                        vBNR1, vBNR2, vBNR3, vBNR4, vBNR5, vBNR6, vBNR7, vBNR8, vBNR9, vBNR10, vBNR11, vBNR12, vBNR13,
                        vBNR14, vBNR15, vBNR16, vBNR17, vBNR18, vBNR19, vBNR20, vBNR21, vBNR22, vBNR23, vBNR24, vBNR25,
                        vBNR26, vBNR27, vBNR28, vBNR29, vBNR30, vBNR31, vBNR32, vBNR33, vBNR34, vBNR35, vBNR36, vBNR37,
                        vBNR38, vBNR39, vBNR40, vBNR41, vBNR42, vBNR43, vBNR44, vBNR45, vBNR46, vBNR47, vBNR51, vBNR52,
                        vBNR48, vBNR49, vBNR50,
                        txtBRem1.Text.Replace("'", "''"), txtBRem2.Text.Replace("'", "''"), txtBRem3.Text.Replace("'", "''")
                        , txtBRem4.Text.Replace("'", "''"), txtBRem5.Text.Replace("'", "''"), txtBRem6.Text.Replace("'", "''")
                        , txtBRem7.Text.Replace("'", "''"), txtBRem8.Text.Replace("'", "''"), txtBRem9.Text.Replace("'", "''")
                        , txtBRem10.Text.Replace("'", "''"), txtBRem11.Text.Replace("'", "''"), txtBRem12.Text.Replace("'", "''")
                        , txtBRem13.Text.Replace("'", "''"), txtBRem14.Text.Replace("'", "''"), txtBRem15.Text.Replace("'", "''")
                        , txtBRem16.Text.Replace("'", "''"), txtBRem17.Text.Replace("'", "''"), txtBRem18.Text.Replace("'", "''")
                        , txtBRem19.Text.Replace("'", "''"), txtBRem20.Text.Replace("'", "''"), txtBRem21.Text.Replace("'", "''")
                        , txtBRem22.Text.Replace("'", "''"), txtBRem23.Text.Replace("'", "''"), txtBRem24.Text.Replace("'", "''")
                        , txtBRem25.Text.Replace("'", "''"), txtBRem26.Text.Replace("'", "''"), txtBRem27.Text.Replace("'", "''")
                        , txtBRem28.Text.Replace("'", "''"), txtBRem29.Text.Replace("'", "''"), txtBRem30.Text.Replace("'", "''")
                        , txtBRem31.Text.Replace("'", "''"), txtBRem32.Text.Replace("'", "''"), txtBRem33.Text.Replace("'", "''")
                        , txtBRem34.Text.Replace("'", "''"), txtBRem35.Text.Replace("'", "''"), txtBRem36.Text.Replace("'", "''")
                        , txtBRem37.Text.Replace("'", "''"), txtBRem38.Text.Replace("'", "''"), txtBRem39.Text.Replace("'", "''")
                        , txtBRem40.Text.Replace("'", "''"), txtBRem41.Text.Replace("'", "''"), txtBRem42.Text.Replace("'", "''")
                        , txtBRem43.Text.Replace("'", "''"), txtBRem44.Text.Replace("'", "''"), txtBRem45.Text.Replace("'", "''")
                        , txtBRem46.Text.Replace("'", "''"), txtBRem47.Text.Replace("'", "''"), txtBRem51.Text.Replace("'", "''"),
                        txtBRem52.Text.Replace("'", "''"), txtBRem48.Text.Replace("'", "''"), txtBRem49.Text.Replace("'", "''"),
                        txtBRem50.Text.Replace("'", "''"), vUsrID, "Del");
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