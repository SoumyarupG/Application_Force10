using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.Collections;
using System.Data;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class PreMatCloseApproval : CENTRUMBase
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
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                txtAppDt.Text = Session[gblValue.LoginDate].ToString();
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Enabled = false;
                PopBranch(Session[gblValue.UserName].ToString());
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                    ddlBranch.Enabled = false;
                    popCenter();
                }
                txtTotalAmt.Text = "0.00";
                txtTotalMem.Text = "0";

            }
        }


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
                this.PageHeading = "Pre Closer Approval";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuPreCloserApp);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Pre Closer Approval", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (ValidDate() == true)
            {
                try
                {
                    if (ddlCenter.SelectedValue == "")
                    {
                        gblFuction.AjxMsgPopup("Please Select Center...");
                        return;
                    }
                    // string vBrCode = Session[gblValue.BrnchCode].ToString();
                    string vCenterId = ddlCenter.SelectedValue;

                    LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vCenterId, 1);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private bool ValidDate()
        {
            Boolean vResult = true;
            if (txtFrmDt.Text.Trim() != "" || txtFrmDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtFrmDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtFrDt");
                    vResult = false;
                }
            }
            if (txtToDt.Text.Trim() != "" || txtToDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtToDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtToDt");
                    vResult = false;
                }
            }
            if (txtAppDt.Text.Trim() != "" || txtAppDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtAppDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtSancDt");
                    vResult = false;
                }
            }
            return vResult;
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            CLoanRecovery oApp = null;
            DataTable dt = new DataTable();
            Int32 vErr = 0;
            string vXmlData = "";
            //DateTime vSanDt = gblFuction.setDate(txtExDisbDt.Text);
            DateTime vSanDt = gblFuction.setDate(txtAppDt.Text);
            string vActMstTbl = Session[gblValue.ACVouMst].ToString();
            if (gvSanc.Rows.Count == 0)
            {
                gblFuction.AjxMsgPopup("No Records to Update.");
                return;
            }
            if (ValidDate() == true)
            {
                try
                {
                    dt = (DataTable)ViewState["Sanc"];
                    if (dt == null) return;
                    if (ValidateFields() == false) return;

                    string vCenterId = ddlCenter.SelectedValue;


                    oApp = new CLoanRecovery();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    //-----------XML Save----------
                    vErr = oApp.ApprovePreMatCls(vXmlData, this.UserID, "E", 0, vSanDt, vActMstTbl);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
                        LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vCenterId, 1);

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

        private Boolean ValidateFields()//To Check
        {
            Boolean vResult = true;
            return vResult;
        }

        private void LoadGrid(string pAppMode, string pFromDt, string pToDt, string pCenterId, Int32 pPgIndx)
        {
            DataTable dt = null;
            CLoanRecovery oLS = null;
            Int32 vRows = 0;


            try
            {
                string vCenterId = pCenterId;
                DateTime vFromDt = gblFuction.setDate(pFromDt);
                DateTime vToDt = gblFuction.setDate(pToDt);
                oLS = new CLoanRecovery();
                dt = oLS.GetPerMatAppList(vFromDt, vToDt, pAppMode, vCenterId, pPgIndx, ref vRows);
                ViewState["Sanc"] = dt;
                gvSanc.DataSource = dt;
                gvSanc.DataBind();
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
                oLS = null;
            }
        }

        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }

        protected void ChangePage(object sender, CommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }

            LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vBrCode, cPgNo);
            //tabLoanAppl.ActiveTabIndex = 0;
        }

        protected void chkApp_CheckedChanged(object sender, EventArgs e)
        {
            string vBranch = "";
            DataTable dt = null;
            Int32 vDisbDateCount = Convert.ToInt32(ViewState["DCount"]);
            string vMsg = "";
            try
            {
                dt = (DataTable)ViewState["Sanc"];

                vBranch = Session[gblValue.BrnchCode].ToString();
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkApp = (CheckBox)row.FindControl("chkApp");
                CheckBox chkCan = (CheckBox)row.FindControl("chkCan");
                LinkButton ReqNo = (LinkButton)row.FindControl("btnShow");

                if (chkApp.Checked)
                {
                    row.Cells[13].Text = "Y";
                    row.Cells[15].Text = "N";
                    dt.Rows[row.RowIndex]["Approved"] = "Y";
                    dt.Rows[row.RowIndex]["Cancel"] = "N";
                    chkCan.Enabled = false;
                    txtTotalMem.Text = Convert.ToString(Convert.ToInt32(txtTotalMem.Text) + 1);
                    txtTotalAmt.Text = Convert.ToString(Convert.ToDouble(txtTotalAmt.Text) + Convert.ToDouble(row.Cells[6].Text) + Convert.ToDouble(row.Cells[7].Text));
                }
                else
                {
                    chkApp.Enabled = true;
                    chkCan.Enabled = true;
                    row.Cells[15].Text = "N";
                    row.Cells[13].Text = "N";
                    dt.Rows[row.RowIndex]["Approved"] = "N";
                    dt.Rows[row.RowIndex]["Cancel"] = "N";
                    txtTotalMem.Text = Convert.ToString(Convert.ToInt32(txtTotalMem.Text) - 1);
                    txtTotalAmt.Text = Convert.ToString(Convert.ToDouble(txtTotalAmt.Text) - (Convert.ToDouble(row.Cells[6].Text) + Convert.ToDouble(row.Cells[7].Text)));
                }

                dt.AcceptChanges();
                ViewState["Sanc"] = dt;
                upSanc.Update();
            }
            finally
            {
                dt = null;
            }
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBranch.SelectedIndex > 0) popCenter();
        }

        private void popCenter()
        {
            DataTable dt = null;
            CLoanRecovery oRO = null;
            string vBrCode;

            vBrCode = ddlBranch.SelectedValue;
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            try
            {
                oRO = new CLoanRecovery();
                dt = oRO.PopCenterPreMatApp(vBrCode, vToDt);
                ddlCenter.DataSource = dt;
                ddlCenter.DataTextField = "Market";
                ddlCenter.DataValueField = "MarketID";
                ddlCenter.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCenter.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        protected void chkCan_CheckedChanged(object sender, EventArgs e)
        {
            int User = 0;
            User = Convert.ToInt32(Session[gblValue.UserId].ToString());
            DataTable dt = null;
            CheckBox checkbox = (CheckBox)sender;
            CheckBox checkbox2 = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            GridViewRow row1 = (GridViewRow)checkbox2.NamingContainer;
            CheckBox chkApp = (CheckBox)row.FindControl("chkApp");
            CheckBox chkCan = (CheckBox)row.FindControl("chkCan");

            dt = (DataTable)ViewState["Sanc"];
            if (checkbox.Checked == true)
            {
                chkApp.Enabled = false;
                row.Cells[15].Text = "Y";
                dt.Rows[row.RowIndex]["Cancel"] = "Y";
                row.Cells[13].Text = "N";
                dt.Rows[row.RowIndex]["Approved"] = "N";

                dt.AcceptChanges();
            }
            else
            {
                chkApp.Enabled = true;
                row.Cells[13].Text = "N";
                dt.Rows[row.RowIndex]["Cancel"] = "N";
                row.Cells[15].Text = "N";
                dt.Rows[row.RowIndex]["Approved"] = "N";
                dt.AcceptChanges();
            }

            dt.AcceptChanges();
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void gvSanc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Bind Approval
                    CheckBox chkApp = (CheckBox)e.Row.FindControl("chkApp");
                    CheckBox chkCan = (CheckBox)e.Row.FindControl("chkCan");

                    if (e.Row.Cells[13].Text == "Y")
                    {
                        chkApp.Checked = true;
                        chkCan.Enabled = false;
                        txtTotalMem.Text = Convert.ToString(Convert.ToInt32(txtTotalMem.Text) + 1);
                        txtTotalAmt.Text = Convert.ToString(Convert.ToDouble(txtTotalAmt.Text) + Convert.ToDouble(e.Row.Cells[6].Text) + Convert.ToDouble(e.Row.Cells[7].Text));
                    }
                    else if (e.Row.Cells[13].Text == "N")
                    {
                        chkApp.Checked = false;
                        chkCan.Enabled = true;
                    }

                    //Bind Calcel

                    if (e.Row.Cells[15].Text == "Y")
                    {
                        chkCan.Checked = true;
                        chkCan.Enabled = true;
                        chkApp.Enabled = false;
                    }
                    else if (e.Row.Cells[15].Text == "N")
                    {
                        chkCan.Checked = false;
                        chkApp.Enabled = true;
                    }

                }
            }
            finally
            {

            }
        }

    }
}