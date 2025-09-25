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
    public partial class IntInspRR : CENTRUMBase
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
                this.PageHeading = "Risk Rating";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuIntRR);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Risk Rating", false);
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
            ddlAQ1.Enabled = Status;
            txtRemA1.Enabled = Status;
            ddlAQ2.Enabled = Status;
            txtRemA2.Enabled = Status;
            ddlAQ3.Enabled = Status;
            txtRemA3.Enabled = Status;
            ddlAQ4.Enabled = Status;
            txtRemA4.Enabled = Status;
            ddlAQ5.Enabled = Status;
            txtRemA5.Enabled = Status;
            ddlAQ6.Enabled = Status;
            txtRemA6.Enabled = Status;
            ddlBQ1.Enabled = Status;
            txtRemB1.Enabled = Status;
            ddlBQ2.Enabled = Status;
            txtRemB2.Enabled = Status;
            ddlBQ3.Enabled = Status;
            txtRemB3.Enabled = Status;
            ddlBQ4.Enabled = Status;
            txtRemB4.Enabled = Status;
            ddlBQ5.Enabled = Status;
            txtRemB5.Enabled = Status;
            ddlBQ6.Enabled = Status;
            txtRemB6.Enabled = Status;
            ddlBQ7.Enabled = Status;
            txtRemB7.Enabled = Status;
            ddlBQ8.Enabled = Status;
            txtRemB8.Enabled = Status;
            ddlBQ9.Enabled = Status;
            txtRemB9.Enabled = Status;
            ddlBQ10.Enabled = Status;
            txtRemB10.Enabled = Status;
            ddlBQ11.Enabled = Status;
            txtRemB11.Enabled = Status;
            ddlBQ12.Enabled = Status;
            txtRemB12.Enabled = Status;
            ddlBQ13.Enabled = Status;
            txtRemB13.Enabled = Status;
            ddlBQ14.Enabled = Status;
            txtRemB14.Enabled = Status;
            ddlBQ15.Enabled = Status;
            txtRemB15.Enabled = Status;
            ddlBQ16.Enabled = Status;
            txtRemB16.Enabled = Status;
            ddlBQ17.Enabled = Status;
            txtRemB17.Enabled = Status;
            ddlCQ1.Enabled = Status;
            txtRemC1.Enabled = Status;
            ddlCQ2.Enabled = Status;
            txtRemC2.Enabled = Status;
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

            ddlAQ1.SelectedIndex = -1;
            txtQA1Scr.Text = "4";
            txtRemA1.Text = "";
            ddlAQ2.SelectedIndex = -1;
            txtQA2Scr.Text = "4";
            txtRemA2.Text = "";
            ddlAQ3.SelectedIndex = -1;
            txtQA3Scr.Text = "4";
            txtRemA3.Text = "";
            ddlAQ4.SelectedIndex = -1;
            txtQA4Scr.Text = "4";
            txtRemA4.Text = "";
            ddlAQ5.SelectedIndex = -1;
            txtQA5Scr.Text = "4";
            txtRemA5.Text = "";
            ddlAQ6.SelectedIndex = -1;
            txtQ6AScr.Text = "4";
            txtRemA6.Text = "";

            ddlBQ1.SelectedIndex = -1;
            txtBQ1Scr.Text = "4";
            txtRemB1.Text = "";
            ddlBQ2.SelectedIndex = -1;
            txtBQ2Scr.Text = "4";
            txtRemB2.Text = "";
            ddlBQ3.SelectedIndex = -1;
            txtBQ3Scr.Text = "4";
            txtRemB3.Text = "";
            ddlBQ4.SelectedIndex = -1;
            txtBQ4Scr.Text = "4";
            txtRemB4.Text = "";
            ddlBQ5.SelectedIndex = -1;
            txtBQ5Scr.Text = "4";
            txtRemB5.Text = "";
            ddlBQ6.SelectedIndex = -1;
            txtBQ6Scr.Text = "4";
            txtRemB6.Text = "";
            ddlBQ7.SelectedIndex = -1;
            txtBQ7Scr.Text = "4";
            txtRemB7.Text = "";
            ddlBQ8.SelectedIndex = -1;
            txtBQ8Scr.Text = "4";
            txtRemB8.Text = "";
            ddlBQ9.SelectedIndex = -1;
            txtBQ9Scr.Text = "4";
            txtRemB9.Text = "";
            ddlBQ10.SelectedIndex = -1;
            txtBQ10Scr.Text = "4";
            txtRemB10.Text = "";
            ddlBQ11.SelectedIndex = -1;
            txtBQ11Scr.Text = "4";
            txtRemB11.Text = "";
            ddlBQ12.SelectedIndex = -1;
            txtQB12Scr.Text = "4";
            txtRemB12.Text = "";
            ddlBQ13.SelectedIndex = -1;
            txtBQ13Scr.Text = "4";
            txtRemB13.Text = "";
            ddlBQ14.SelectedIndex = -1;
            txtBQ14Scr.Text = "4";
            txtRemB14.Text = "";
            ddlBQ15.SelectedIndex = -1;
            txtBQ15Scr.Text = "4";
            txtRemB15.Text = "";
            ddlBQ16.SelectedIndex = -1;
            txtBQ16Scr.Text = "4";
            txtRemB16.Text = "";
            ddlBQ17.SelectedIndex = -1;
            txtBQ17Scr.Text = "4";
            txtRemB17.Text = "";
            ddlCQ1.SelectedIndex = -1;
            txtCQ1Scr.Text = "4";
            txtRemC1.Text = "";
            ddlCQ2.SelectedIndex = -1;
            txtCQ2Scr.Text = "4";
            txtRemC2.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
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
            LoadGrid(1);
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
                dt = oAd.GetIntInspRRPG(pPgIndx,vFrmDt, vToDt , ref vTotRows);
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
            Response.Redirect("~/WebPages/Public/Main.aspx");
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
                    dt = oAu.GetIntInspRRById(pInspID);
                    if (dt.Rows.Count > 0)
                    {
                        txtSDt.Text = Convert.ToString(dt.Rows[0]["SubDt"]);
                        txtCrStrDt.Text = Convert.ToString(dt.Rows[0]["CrdFrmDt"]);
                        txtCrEndDt.Text = Convert.ToString(dt.Rows[0]["CrdToDt"]);
                        txtPrStrDt.Text = Convert.ToString(dt.Rows[0]["PrdFrmDt"]);
                        txtPrEndDt.Text = Convert.ToString(dt.Rows[0]["PrdToDt"]);
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["Branch"].ToString()));

                        ddlAQ1.SelectedIndex = ddlAQ1.Items.IndexOf(ddlAQ1.Items.FindByValue(dt.Rows[0]["A1"].ToString()));
                        txtQA1Scr.Text = Convert.ToString(dt.Rows[0]["AS1"]);
                        txtRemA1.Text = Convert.ToString(dt.Rows[0]["AR1"]);
                        ddlAQ2.SelectedIndex = ddlAQ2.Items.IndexOf(ddlAQ2.Items.FindByValue(dt.Rows[0]["A2"].ToString()));
                        txtQA2Scr.Text = Convert.ToString(dt.Rows[0]["AS2"]);
                        txtRemA2.Text = Convert.ToString(dt.Rows[0]["AR2"]);
                        ddlAQ3.SelectedIndex = ddlAQ3.Items.IndexOf(ddlAQ3.Items.FindByValue(dt.Rows[0]["A3"].ToString()));
                        txtQA3Scr.Text = Convert.ToString(dt.Rows[0]["AS3"]);
                        txtRemA3.Text = Convert.ToString(dt.Rows[0]["AR3"]);
                        ddlAQ4.SelectedIndex = ddlAQ4.Items.IndexOf(ddlAQ4.Items.FindByValue(dt.Rows[0]["A4"].ToString()));
                        txtQA4Scr.Text = Convert.ToString(dt.Rows[0]["AS4"]);
                        txtRemA4.Text = Convert.ToString(dt.Rows[0]["AR4"]);
                        ddlAQ5.SelectedIndex = ddlAQ5.Items.IndexOf(ddlAQ5.Items.FindByValue(dt.Rows[0]["A5"].ToString()));
                        txtQA5Scr.Text = Convert.ToString(dt.Rows[0]["AS5"]);
                        txtRemA5.Text = Convert.ToString(dt.Rows[0]["AR5"]);
                        ddlAQ6.SelectedIndex = ddlAQ6.Items.IndexOf(ddlAQ6.Items.FindByValue(dt.Rows[0]["A6"].ToString()));
                        txtQ6AScr.Text = Convert.ToString(dt.Rows[0]["AS6"]);
                        txtRemA6.Text = Convert.ToString(dt.Rows[0]["AR6"]);
                        ddlBQ1.SelectedIndex = ddlBQ1.Items.IndexOf(ddlBQ1.Items.FindByValue(dt.Rows[0]["B1"].ToString()));
                        txtBQ1Scr.Text = Convert.ToString(dt.Rows[0]["BS1"]);
                        txtRemB1.Text = Convert.ToString(dt.Rows[0]["BR1"]);
                        ddlBQ2.SelectedIndex = ddlBQ2.Items.IndexOf(ddlBQ2.Items.FindByValue(dt.Rows[0]["B2"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["BS2"]) == "-1")
                            txtBQ2Scr.Text = "N/A";
                        else
                            txtBQ2Scr.Text = Convert.ToString(dt.Rows[0]["BS2"]);
                        txtRemB2.Text = Convert.ToString(dt.Rows[0]["BR2"]);
                        ddlBQ3.SelectedIndex = ddlBQ3.Items.IndexOf(ddlBQ3.Items.FindByValue(dt.Rows[0]["B3"].ToString()));
                        txtBQ3Scr.Text = Convert.ToString(dt.Rows[0]["BS3"]);
                        txtRemB3.Text = Convert.ToString(dt.Rows[0]["BR3"]);
                        ddlBQ4.SelectedIndex = ddlBQ4.Items.IndexOf(ddlBQ4.Items.FindByValue(dt.Rows[0]["B4"].ToString()));
                        txtBQ4Scr.Text = Convert.ToString(dt.Rows[0]["BS4"]);
                        txtRemB4.Text = Convert.ToString(dt.Rows[0]["BR4"]);
                        ddlBQ5.SelectedIndex = ddlBQ5.Items.IndexOf(ddlBQ5.Items.FindByValue(dt.Rows[0]["B5"].ToString()));
                        txtBQ5Scr.Text = Convert.ToString(dt.Rows[0]["BS5"]);
                        txtRemB5.Text = Convert.ToString(dt.Rows[0]["BR5"]);
                        ddlBQ6.SelectedIndex = ddlBQ6.Items.IndexOf(ddlBQ6.Items.FindByValue(dt.Rows[0]["B6"].ToString()));
                        txtBQ6Scr.Text = Convert.ToString(dt.Rows[0]["BS6"]);
                        txtRemB6.Text = Convert.ToString(dt.Rows[0]["BR6"]);
                        ddlBQ7.SelectedIndex = ddlBQ7.Items.IndexOf(ddlBQ7.Items.FindByValue(dt.Rows[0]["B7"].ToString()));
                        txtBQ7Scr.Text = Convert.ToString(dt.Rows[0]["BS7"]);
                        txtRemB7.Text = Convert.ToString(dt.Rows[0]["BR7"]);
                        ddlBQ8.SelectedIndex = ddlBQ8.Items.IndexOf(ddlBQ8.Items.FindByValue(dt.Rows[0]["B8"].ToString()));
                        txtBQ8Scr.Text = Convert.ToString(dt.Rows[0]["BS8"]);
                        txtRemB8.Text = Convert.ToString(dt.Rows[0]["BR8"]);
                        ddlBQ9.SelectedIndex = ddlBQ9.Items.IndexOf(ddlBQ9.Items.FindByValue(dt.Rows[0]["B9"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["BS9"]) == "-1")
                            txtBQ9Scr.Text = "N/A";
                        else
                            txtBQ9Scr.Text = Convert.ToString(dt.Rows[0]["BS9"]);
                        txtRemB9.Text = Convert.ToString(dt.Rows[0]["BR9"]);
                        ddlBQ10.SelectedIndex = ddlBQ10.Items.IndexOf(ddlBQ10.Items.FindByValue(dt.Rows[0]["B10"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["BS10"]) == "-1")
                            txtBQ10Scr.Text = "N/A";
                        else
                            txtBQ10Scr.Text = Convert.ToString(dt.Rows[0]["BS10"]);
                        txtRemB10.Text = Convert.ToString(dt.Rows[0]["BR10"]);
                        ddlBQ11.SelectedIndex = ddlBQ11.Items.IndexOf(ddlBQ11.Items.FindByValue(dt.Rows[0]["B11"].ToString()));
                        txtBQ11Scr.Text = Convert.ToString(dt.Rows[0]["BS11"]);
                        txtRemB11.Text = Convert.ToString(dt.Rows[0]["BR11"]);
                        ddlBQ12.SelectedIndex = ddlBQ12.Items.IndexOf(ddlBQ12.Items.FindByValue(dt.Rows[0]["B12"].ToString()));
                        txtQB12Scr.Text = Convert.ToString(dt.Rows[0]["BS12"]);
                        txtRemB12.Text = Convert.ToString(dt.Rows[0]["BR12"]);
                        ddlBQ13.SelectedIndex = ddlBQ13.Items.IndexOf(ddlBQ13.Items.FindByValue(dt.Rows[0]["B13"].ToString()));
                        txtBQ13Scr.Text = Convert.ToString(dt.Rows[0]["BS13"]);
                        txtRemB13.Text = Convert.ToString(dt.Rows[0]["BR13"]);
                        ddlBQ14.SelectedIndex = ddlBQ14.Items.IndexOf(ddlBQ14.Items.FindByValue(dt.Rows[0]["B14"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["BS14"]) == "-1")
                            txtBQ14Scr.Text = "N/A";
                        else
                            txtBQ14Scr.Text = Convert.ToString(dt.Rows[0]["BS14"]);
                        txtRemB14.Text = Convert.ToString(dt.Rows[0]["BR14"]);
                        ddlBQ15.SelectedIndex = ddlBQ15.Items.IndexOf(ddlBQ15.Items.FindByValue(dt.Rows[0]["B15"].ToString()));
                        txtBQ15Scr.Text = Convert.ToString(dt.Rows[0]["BS15"]);
                        txtRemB15.Text = Convert.ToString(dt.Rows[0]["BR15"]);
                        ddlBQ16.SelectedIndex = ddlBQ16.Items.IndexOf(ddlBQ16.Items.FindByValue(dt.Rows[0]["B16"].ToString()));
                        txtBQ16Scr.Text = Convert.ToString(dt.Rows[0]["BS16"]);
                        txtRemB16.Text = Convert.ToString(dt.Rows[0]["BR16"]);
                        ddlBQ17.SelectedIndex = ddlBQ17.Items.IndexOf(ddlBQ17.Items.FindByValue(dt.Rows[0]["B17"].ToString()));
                        txtBQ17Scr.Text = Convert.ToString(dt.Rows[0]["BS17"]);
                        txtRemB17.Text = Convert.ToString(dt.Rows[0]["BR17"]);
                        ddlCQ1.SelectedIndex = ddlCQ1.Items.IndexOf(ddlCQ1.Items.FindByValue(dt.Rows[0]["C1"].ToString()));
                        if (Convert.ToString(dt.Rows[0]["CS1"]) == "-1")
                            txtCQ1Scr.Text = "N/A";
                        else
                            txtCQ1Scr.Text = Convert.ToString(dt.Rows[0]["CS1"]);
                        txtRemC1.Text = Convert.ToString(dt.Rows[0]["CR1"]);
                        ddlCQ2.SelectedIndex = ddlCQ2.Items.IndexOf(ddlCQ2.Items.FindByValue(dt.Rows[0]["C2"].ToString()));
                        txtCQ2Scr.Text = Convert.ToString(dt.Rows[0]["CS2"]);
                        txtRemC2.Text = Convert.ToString(dt.Rows[0]["CR2"]);
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
            Int32 vAS1 = 0, vAS2 = 0, vAS3 = 0, vAS4 = 0, vAS5 = 0, vAS6 = 0, vBS1 = 0, vBS2 = 0, vBS3 = 0, vBS4 = 0, vBS5 = 0, vBS6 = 0, vBS7 = 0, vBS8 = 0;
            Int32 vBS9 = 0, vBS10 = 0, vBS11 = 0, vBS12 = 0, vBS13 = 0, vBS14 = 0, vBS15 = 0, vBS16 = 0, vBS17 = 0, vCS1 = 0, vCS2=0;
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vInsPecId = 0, vUserID=0, vErr = 0;
            DateTime vSubDt = gblFuction.setDate(txtSDt.Text);
            DateTime vCrStrDt = gblFuction.setDate(txtCrStrDt.Text);
            DateTime vCrEndDt = gblFuction.setDate(txtCrEndDt.Text);
            DateTime vPrStrDt = gblFuction.setDate(txtPrStrDt.Text);
            DateTime vPrEndDt = gblFuction.setDate(txtPrEndDt.Text);
            TimeSpan vTsp = (vSubDt - vPrStrDt);
            CIntIspPM oAu = null;

            try
            {
                //Validate
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

                vUserID = Int32.Parse(Session[gblValue.UserId].ToString()); 
                if (ViewState["InsPecId"] != null)
                    vInsPecId = Convert.ToInt32(Convert.ToString(ViewState["InsPecId"]));

                if (txtQA1Scr.Text != "")
                    vAS1 = Convert.ToInt32(txtQA1Scr.Text);
                if (txtQA2Scr.Text != "")
                    vAS2 = Convert.ToInt32(txtQA2Scr.Text);
                if (txtQA3Scr.Text != "")
                    vAS3 = Convert.ToInt32(txtQA3Scr.Text);
                if (txtQA4Scr.Text != "")
                    vAS4 = Convert.ToInt32(txtQA4Scr.Text);
                if (txtQA5Scr.Text != "")
                    vAS5 = Convert.ToInt32(txtQA5Scr.Text);
                if (txtQ6AScr.Text != "")
                    vAS6 = Convert.ToInt32(txtQ6AScr.Text);

                if (txtBQ1Scr.Text != "")
                    vBS1 = Convert.ToInt32(txtBQ1Scr.Text);
                if (txtBQ2Scr.Text != "N/A")
                    vBS2 = Convert.ToInt32(txtBQ2Scr.Text);
                else if (txtBQ2Scr.Text == "N/A")
                    vBS2 = -1;
                if (txtBQ3Scr.Text != "")
                    vBS3 = Convert.ToInt32(txtBQ3Scr.Text);
                if (txtBQ4Scr.Text != "")
                    vBS4 = Convert.ToInt32(txtBQ4Scr.Text);
                if (txtBQ5Scr.Text != "")
                    vBS5 = Convert.ToInt32(txtBQ5Scr.Text);
                if (txtBQ6Scr.Text != "")
                    vBS6 = Convert.ToInt32(txtBQ6Scr.Text);
                if (txtBQ7Scr.Text != "")
                    vBS7 = Convert.ToInt32(txtBQ7Scr.Text);
                if (txtBQ8Scr.Text != "")
                    vBS8 = Convert.ToInt32(txtBQ8Scr.Text);
                if (txtBQ9Scr.Text != "N/A")
                    vBS9 = Convert.ToInt32(txtBQ9Scr.Text);
                else if (txtBQ9Scr.Text == "N/A")
                    vBS9 = -1;
                if (txtBQ10Scr.Text != "N/A")
                    vBS10 = Convert.ToInt32(txtBQ10Scr.Text);
                else if (txtBQ10Scr.Text == "N/A")
                    vBS10 = -1;
                if (txtBQ11Scr.Text != "")
                    vBS11 = Convert.ToInt32(txtBQ11Scr.Text);
                if (txtQB12Scr.Text != "")
                    vBS12 = Convert.ToInt32(txtQB12Scr.Text);
                if (txtBQ13Scr.Text != "")
                    vBS13 = Convert.ToInt32(txtBQ13Scr.Text);
                if (txtBQ14Scr.Text != "N/A")
                    vBS14 = Convert.ToInt32(txtBQ14Scr.Text);
                else if (txtBQ14Scr.Text == "N/A")
                    vBS14 = -1;
                if (txtBQ15Scr.Text != "")
                    vBS15 = Convert.ToInt32(txtBQ15Scr.Text);
                if (txtBQ16Scr.Text != "")
                    vBS16 = Convert.ToInt32(txtBQ16Scr.Text);
                if (txtBQ17Scr.Text != "")
                    vBS17 = Convert.ToInt32(txtBQ17Scr.Text);

                if (txtCQ1Scr.Text != "N/A")
                    vCS1 = Convert.ToInt32(txtCQ1Scr.Text);
                else if (txtCQ1Scr.Text == "N/A")
                    vCS1 = -1;
                if (txtCQ2Scr.Text != "")
                    vCS2 = Convert.ToInt32(txtCQ2Scr.Text);
                 
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

                    vErr = oAu.SaveIntInspRR(ref vInsPecId, "RR", vSubDt, vCrStrDt, vCrEndDt, vPrStrDt, vPrEndDt, ddlBranch.SelectedValue, vUserID,
                               Convert.ToInt32(ddlAQ1.SelectedValue), vAS1, txtRemA1.Text.Replace("'", "''"), Convert.ToInt32(ddlAQ2.SelectedValue), vAS2, txtRemA2.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlAQ3.SelectedValue), vAS3, txtRemA3.Text.Replace("'", "''"), Convert.ToInt32(ddlAQ4.SelectedValue), vAS4, txtRemA4.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlAQ5.SelectedValue), vAS5, txtRemA5.Text.Replace("'", "''"), Convert.ToInt32(ddlAQ6.SelectedValue), vAS6, txtRemA6.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ1.SelectedValue), vBS1, txtRemB1.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ2.SelectedValue), vBS2, txtRemB2.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ3.SelectedValue), vBS3, txtRemB3.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ4.SelectedValue), vBS4, txtRemB4.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ5.SelectedValue), vBS5, txtRemB5.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ6.SelectedValue), vBS6, txtRemB6.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ7.SelectedValue), vBS7, txtRemB7.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ8.SelectedValue), vBS8, txtRemB8.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ9.SelectedValue), vBS9, txtRemB9.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ10.SelectedValue), vBS10, txtRemB10.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ11.SelectedValue), vBS11, txtRemB11.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ12.SelectedValue), vBS12, txtRemB12.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ13.SelectedValue), vBS13, txtRemB13.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ14.SelectedValue), vBS14, txtRemB14.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ15.SelectedValue), vBS15, txtRemB15.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ16.SelectedValue), vBS16, txtRemB16.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ17.SelectedValue), vBS17, txtRemB17.Text.Replace("'", "''"), Convert.ToInt32(ddlCQ1.SelectedValue), vCS1, txtRemC1.Text.Replace("'","''"),
                               Convert.ToInt32(ddlCQ2.SelectedValue), vCS2, txtRemC2.Text.Replace("'", "''"), "Save");
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

                    vErr = oAu.SaveIntInspRR(ref vInsPecId, "RR", vSubDt, vCrStrDt, vCrEndDt, vPrStrDt, vPrEndDt, ddlBranch.SelectedValue, vUserID,
                               Convert.ToInt32(ddlAQ1.SelectedValue), vAS1, txtRemA1.Text.Replace("'", "''"), Convert.ToInt32(ddlAQ2.SelectedValue), vAS2, txtRemA2.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlAQ3.SelectedValue), vAS3, txtRemA3.Text.Replace("'", "''"), Convert.ToInt32(ddlAQ4.SelectedValue), vAS4, txtRemA4.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlAQ5.SelectedValue), vAS5, txtRemA5.Text.Replace("'", "''"), Convert.ToInt32(ddlAQ6.SelectedValue), vAS6, txtRemA6.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ1.SelectedValue), vBS1, txtRemB1.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ2.SelectedValue), vBS2, txtRemB2.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ3.SelectedValue), vBS3, txtRemB3.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ4.SelectedValue), vBS4, txtRemB4.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ5.SelectedValue), vBS5, txtRemB5.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ6.SelectedValue), vBS6, txtRemB6.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ7.SelectedValue), vBS7, txtRemB7.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ8.SelectedValue), vBS8, txtRemB8.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ9.SelectedValue), vBS9, txtRemB9.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ10.SelectedValue), vBS10, txtRemB10.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ11.SelectedValue), vBS11, txtRemB11.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ12.SelectedValue), vBS12, txtRemB12.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ13.SelectedValue), vBS13, txtRemB13.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ14.SelectedValue), vBS14, txtRemB14.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ15.SelectedValue), vBS15, txtRemB15.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ16.SelectedValue), vBS16, txtRemB16.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ17.SelectedValue), vBS17, txtRemB17.Text.Replace("'", "''"), Convert.ToInt32(ddlCQ1.SelectedValue), vCS1, txtRemC1.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlCQ2.SelectedValue), vCS2, txtRemC2.Text.Replace("'", "''"), "Edit");
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
                    vErr = oAu.SaveIntInspRR(ref vInsPecId, "RR", vSubDt, vCrStrDt, vCrEndDt, vPrStrDt, vPrEndDt, ddlBranch.SelectedValue, vUserID,
                               Convert.ToInt32(ddlAQ1.SelectedValue), vAS1, txtRemA1.Text.Replace("'", "''"), Convert.ToInt32(ddlAQ2.SelectedValue), vAS2, txtRemA2.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlAQ3.SelectedValue), vAS3, txtRemA3.Text.Replace("'", "''"), Convert.ToInt32(ddlAQ4.SelectedValue), vAS4, txtRemA4.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlAQ5.SelectedValue), vAS5, txtRemA5.Text.Replace("'", "''"), Convert.ToInt32(ddlAQ6.SelectedValue), vAS6, txtRemA6.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ1.SelectedValue), vBS1, txtRemB1.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ2.SelectedValue), vBS2, txtRemB2.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ3.SelectedValue), vBS3, txtRemB3.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ4.SelectedValue), vBS4, txtRemB4.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ5.SelectedValue), vBS5, txtRemB5.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ6.SelectedValue), vBS6, txtRemB6.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ7.SelectedValue), vBS7, txtRemB7.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ8.SelectedValue), vBS8, txtRemB8.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ9.SelectedValue), vBS9, txtRemB9.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ10.SelectedValue), vBS10, txtRemB10.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ11.SelectedValue), vBS11, txtRemB11.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ12.SelectedValue), vBS12, txtRemB12.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ13.SelectedValue), vBS13, txtRemB13.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ14.SelectedValue), vBS14, txtRemB14.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ15.SelectedValue), vBS15, txtRemB15.Text.Replace("'", "''"), Convert.ToInt32(ddlBQ16.SelectedValue), vBS16, txtRemB16.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlBQ17.SelectedValue), vBS17, txtRemB17.Text.Replace("'", "''"), Convert.ToInt32(ddlCQ1.SelectedValue), vCS1, txtRemC1.Text.Replace("'", "''"),
                               Convert.ToInt32(ddlCQ2.SelectedValue), vCS2, txtRemC2.Text.Replace("'", "''"), "Del");
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
                //oGbl = null;
            }
        }     
    }
}