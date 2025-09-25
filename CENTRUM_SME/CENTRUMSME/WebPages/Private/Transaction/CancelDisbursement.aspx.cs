using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;
using SendSms;
using System.Web.UI;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class CancelDisbursement : CENTRUMBAse
    {
        protected int cPgNo = 1;
        protected int vFlag = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
               
                txtFrmDt.Text = (string)Session[gblValue.LoginDate];
                txtToDt.Text = (string)Session[gblValue.LoginDate];
                PopBranch(Session[gblValue.UserName].ToString());
                LoadGrid(1);
                ViewState["StateEdit"] = null;
                StatusButton("Add");
                tabLoanDisb.ActiveTabIndex = 0;
               
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Loan Disbursement Cancel";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanDisbursementCancel);
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
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Disbursement Cancel", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CDisburse oLD = null;
            Int32 vRows = 0;
            string vBrCode = string.Empty;
            vBrCode = ddlBranch.SelectedValues.Replace("|", ",");
            string vSearch = txtSearch.Text.Trim();
            DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            oLD = new CDisburse();
            dt = oLD.GetDisburseListForCancel(vSearch, vFrmDt, vToDt, vBrCode, pPgIndx, ref vRows);
            gvLoanAppl.DataSource = dt.DefaultView;
            gvLoanAppl.DataBind();
            //lblTotalPages.Text = CalTotPgs(vRows).ToString();
            //lblCurrentPage.Text = cPgNo.ToString();
            //if (cPgNo == 0)
            //{
            //    Btn_Previous.Enabled = false;
            //    if (Int32.Parse(lblTotalPages.Text) > 1)
            //        Btn_Next.Enabled = true;
            //    else
            //        Btn_Next.Enabled = false;
            //}
            //else
            //{
            //    Btn_Previous.Enabled = true;
            //    if (cPgNo == Int32.Parse(lblTotalPages.Text))
            //        Btn_Next.Enabled = false;
            //    else
            //        Btn_Next.Enabled = true;
            //}
           // CalculateTotal();
        }

        private void CalculateTotal()
        {
            double DrAmt = 0.00;
            for (Int32 i = 0; i < gvLoanAppl.Rows.Count; i++)
            {
                DrAmt = Math.Round(DrAmt, 2) + Math.Round(Convert.ToDouble(gvLoanAppl.Rows[i].Cells[4].Text), 2);
            }
            txtDisAmt.Text = Convert.ToString(DrAmt);

        }

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
                    gblFuction.focus("ctl00_cph_Main_tabLnDisb_pnlDtl_ddlCo");
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

        private void EnableControl(Boolean Status)
        {
        }

        private void ClearControls()
        {
        }

        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }

        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
                case "GoTo":
                    if (Int32.Parse(txtGotoPg.Text) <= Int32.Parse(lblCurrentPage.Text))
                        cPgNo = Int32.Parse(txtGotoPg.Text);
                    break;
            }
            LoadGrid(cPgNo);
            tabLoanDisb.ActiveTabIndex = 0;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabLoanDisb.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = "";            
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                StatusButton("View");
                ViewState["StateEdit"] = null;
                LoadGrid(1);
                tabLoanDisb.ActiveTabIndex = 0;
            }
            else
            {
                ClearControls();
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

        protected void chkCancelYN_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            CheckBox ChkCancel = (CheckBox)row.FindControl("chkCancelYN");
            double vTotalAmt = 0;
            Int32 vTotCount = 0;
            foreach (GridViewRow gR in gvLoanAppl.Rows)
            {
                CheckBox chkCancelYN = (CheckBox)gR.FindControl("chkCancelYN");
                TextBox txtCancelReason = (TextBox)gR.FindControl("txtCancelReason");
                if (chkCancelYN.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[4].Text.Trim());
                    vTotCount = vTotCount + 1;
                }
                else
                {
                    txtCancelReason.Text = "";
                }

            }
            txtDisAmt.Text = Convert.ToString(vTotalAmt);
            txtTotalCount.Text = Convert.ToString(vTotCount);            
        }

        private String XmlFromGrid()
        {
            Int32 i = 0;
            String vXML = "";
            DataTable dt = new DataTable("Tr");
            dt.Columns.Add("SlNo");
            dt.Columns.Add("LoanId");
            dt.Columns.Add("CancelYN");
            dt.Columns.Add("Remarks");
            foreach (GridViewRow gr in gvLoanAppl.Rows)
            {
                CheckBox ChkCancel = (CheckBox)gr.FindControl("chkCancelYN");
                LinkButton txtLoanId = (LinkButton)gr.FindControl("btnShow");
                TextBox txtRemarks = (TextBox)gr.FindControl("txtCancelReason");
                string LoanId = txtLoanId.Text;
                if (ChkCancel.Checked == true)
                {
                    DataRow dr = dt.NewRow();
                    dr["SlNo"] = Convert.ToString(i);
                    dr["LoanId"] = LoanId;

                    if (ChkCancel.Checked == true)
                    {
                        dr["CancelYN"] = 'Y';
                        if (txtRemarks.Text == "")
                        {
                            gblFuction.AjxMsgPopup("Please Give a cancel reason for cancel selection...");
                            txtRemarks.Focus();
                            return "";
                        }
                        else
                        {
                            dr["Remarks"] = txtRemarks.Text;
                        }
                    }
                    else
                        dr["CancelYN"] = 'N';
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                    i++;
                }
            }
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vXML = oSW.ToString();
            }
            return vXML;

        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0;
            string vXml = "", pMsg = "";            
            string vdate = "";
            vdate = Convert.ToString(Session[gblValue.LoginDate]);
            DateTime vclosedt = gblFuction.setDate(vdate);
            vXml = XmlFromGrid();
            CDisburse oLD = null;
            try
            {
                if (Mode == "Save")
                {
                    oLD = new CDisburse();
                    vErr = oLD.InsertCancelNEFT(vXml,Convert.ToInt32(Session[gblValue.UserId].ToString()), "I", vclosedt, ref pMsg);
                    if (vErr == 2 || vErr == 3 || vErr == 4)
                    {
                        gblFuction.AjxMsgPopup(pMsg);
                        vResult = false;
                    }
                    else
                    {
                        if (vErr == 0)
                        {
                            gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup(gblPRATAM.DBError);
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
        }
    }
}