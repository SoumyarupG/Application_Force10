using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using CENTRUMCA;
using CENTRUMBA;
using System.Net;
using System.IO;
using AjaxControlToolkit;

namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class CF_FinalDecision : CENTRUMBAse
    {
        protected int vPgNo = 1;
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string BucketURL = ConfigurationManager.AppSettings["BucketURL"];
        string CFDocumentBucket = ConfigurationManager.AppSettings["CFDocumentBucket"];
        string DocumentBucketURL = ConfigurationManager.AppSettings["DocumentBucketURL"];
        private static string vKarzaKey = ConfigurationManager.AppSettings["KarzaKey"];
        string vKarzaEnv = ConfigurationManager.AppSettings["KarzaEnv"];
        string MaxFileSize = ConfigurationManager.AppSettings["MaxFileSize"];
        string CFLeadBucketURL = ConfigurationManager.AppSettings["CFLeadBucketURL"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitBasePage();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;

                PopLeadList();
                popCaste();
                PopReligion();
                PopQualification();
                popStateCust();
                PopRelation();
                PopAssesMethod1();
             
                StatusButton("View");               
                if (Session[gblValue.LoginDate] != null)
                    txtFromDt.Text = gblFuction.putStrDate(gblFuction.setDate(Session[gblValue.LoginDate].ToString()).AddMonths(-1));
                if (Session[gblValue.LoginDate] != null)
                    txtToDt.Text = Session[gblValue.LoginDate].ToString();
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                LoadBasicDetailsList(1);
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Final Decision";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFFinalDec);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {

                    btnEdit.Visible = true;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnDelete.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanProcess == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Final Decision", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                if (ddlSacComm.SelectedValue == "-1")
                {
                    ViewState["StateEdit"] = "Add";
                }
                else
                {
                    ViewState["StateEdit"] = "Edit";
                }

                StatusButton("Edit");


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //tbBasicDet.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }


        private void DisableControl(Boolean Status, Control parent)
        {

            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Enabled = Status;
                }
                else if (c is DropDownList)
                {
                    ((DropDownList)c).Enabled = Status;
                }
                else if (c is FileUpload)
                {
                    ((FileUpload)c).Enabled = Status;
                }

                // Recursively disable controls inside the containers like
                // panels or group controls
                if (c.Controls.Count > 0)
                {
                    DisableControl(Status, c);

                }

            }
            txtSearch.Enabled = true;
            btnShow.Enabled = true;
            txtFromDt.Enabled = true;
            txtToDt.Enabled = true;
            btnEdit.Enabled = true;
            ddlStatus.Enabled = true;

        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadBasicDetailsList(0);
        }
        private void LoadBasicDetailsList(Int32 pPgIndx)
        {
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vFromDt = gblFuction.setDate(txtFromDt.Text.ToString());
            DateTime vToDt = gblFuction.setDate(txtToDt.Text.ToString());
            string vStatus = "";
            string vRecommRemarks = ddlStatus.SelectedValue;
            Int32 vTotRows = 0;

            switch (vRecommRemarks)
            {
                case "Hold":
                    vStatus = "H";
                    break;
                case "SendBack":
                    vStatus = "S";
                    break;
                case "Approve":
                    vStatus = "A";
                    break;
                case "Reject":
                    vStatus = "R";
                    break;
                case "Pending":
                    vStatus = "P";
                    break;
                case "All":
                    vStatus = "All";
                    break;
            }

            try
            {
                dt = oMem.CF_GetBasicDtlsForLoanApp(vBrCode, txtSearch.Text.Trim(), vFromDt, vToDt, vStatus, pPgIndx, ref vTotRows);
                if (dt.Rows.Count > 0)
                {
                    gvBasicDet.DataSource = dt;
                    gvBasicDet.DataBind();                   
                }
                else
                {
                    gvBasicDet.DataSource = null;
                    gvBasicDet.DataBind();
                }
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        private int CalTotPages(double pRows)
        {
            int vPgs = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vPgs;
        }
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
            LoadBasicDetailsList(vPgNo);
            MainTabContainer.ActiveTabIndex = 0;
        }
        private void ClearControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Text = "";
                }
                else if (c is DropDownList)
                {
                    ((DropDownList)c).SelectedIndex = -1;
                }
                else if (c is Label)
                {
                    ((Label)c).Text = "";
                }
                else if (c is HiddenField)
                {
                    ((HiddenField)c).Value = "";
                }

                // Recursively disable controls inside the containers like
                // panels or group controls
                if (c.Controls.Count > 0)
                {
                    ClearControls(c);

                }
            }

            foreach (GridViewRow row in gvEmpDoc.Rows)
            {
                // Check if the row is a data row
                if (row.RowType == DataControlRowType.DataRow)
                {
                    // Loop through controls in the row
                    foreach (Control control in row.Controls)
                    {
                        // Clear TextBox controls
                        if (control is TextBox)
                        {
                            ((TextBox)control).Text = string.Empty;
                        }
                        // Clear DropDownList controls
                        else if (control is DropDownList)
                        {
                            ((DropDownList)control).SelectedIndex = -1; // Or set to a default value
                        }
                        // Clear CheckBox controls
                        else if (control is CheckBox)
                        {
                            ((CheckBox)control).Checked = false;
                        }
                        else if (control is Label)
                        {
                            ((Label)control).Text = "";
                        }
                        else if (control is HiddenField)
                        {
                            ((HiddenField)control).Value = "";
                        }

                    }
                }
            }

            foreach (GridViewRow row in gvEmpDocV.Rows)
            {
                // Check if the row is a data row
                if (row.RowType == DataControlRowType.DataRow)
                {
                    // Loop through controls in the row
                    foreach (Control control in row.Controls)
                    {
                        // Clear TextBox controls
                        if (control is TextBox)
                        {
                            ((TextBox)control).Text = string.Empty;
                        }
                        // Clear DropDownList controls
                        else if (control is DropDownList)
                        {
                            ((DropDownList)control).SelectedIndex = -1; // Or set to a default value
                        }
                        // Clear CheckBox controls
                        else if (control is CheckBox)
                        {
                            ((CheckBox)control).Checked = false;
                        }
                        else if (control is Label)
                        {
                            ((Label)control).Text = "";
                        }
                        else if (control is HiddenField)
                        {
                            ((HiddenField)control).Value = "";
                        }
                    }
                }
            }

            foreach (GridViewRow row in gvEmpDocOth.Rows)
            {
                // Check if the row is a data row
                if (row.RowType == DataControlRowType.DataRow)
                {
                    // Loop through controls in the row
                    foreach (Control control in row.Controls)
                    {
                        // Clear TextBox controls
                        if (control is TextBox)
                        {
                            ((TextBox)control).Text = string.Empty;
                        }
                        // Clear DropDownList controls
                        else if (control is DropDownList)
                        {
                            ((DropDownList)control).SelectedIndex = -1; // Or set to a default value
                        }
                        // Clear CheckBox controls
                        else if (control is CheckBox)
                        {
                            ((CheckBox)control).Checked = false;
                        }
                        else if (control is Label)
                        {
                            ((Label)control).Text = "";
                        }
                        else if (control is HiddenField)
                        {
                            ((HiddenField)control).Value = "";
                        }
                    }
                }
            }

            foreach (GridViewRow row in gvObli.Rows) //Obligation Grid
            {
                // Check if the row is a data row
                if (row.RowType == DataControlRowType.DataRow)
                {
                    // Loop through controls in the row
                    foreach (Control control in row.Controls)
                    {
                        // Clear TextBox controls
                        if (control is TextBox)
                        {
                            ((TextBox)control).Text = string.Empty;
                        }
                        // Clear DropDownList controls
                        else if (control is DropDownList)
                        {
                            ((DropDownList)control).SelectedIndex = -1; // Or set to a default value
                        }
                        // Clear CheckBox controls
                        else if (control is CheckBox)
                        {
                            ((CheckBox)control).Checked = false;
                        }
                        else if (control is Label)
                        {
                            ((Label)control).Text = "";
                        }
                        else if (control is HiddenField)
                        {
                            ((HiddenField)control).Value = "";
                        }
                    }
                }
            }

            foreach (GridViewRow row in gvBankDtl.Rows) //Banking Details
            {
                // Check if the row is a data row
                if (row.RowType == DataControlRowType.DataRow)
                {
                    // Loop through controls in the row
                    foreach (Control control in row.Controls)
                    {
                        // Clear TextBox controls
                        if (control is TextBox)
                        {
                            ((TextBox)control).Text = string.Empty;
                        }
                        // Clear DropDownList controls
                        else if (control is DropDownList)
                        {
                            ((DropDownList)control).SelectedIndex = -1; // Or set to a default value
                        }
                        // Clear CheckBox controls
                        else if (control is CheckBox)
                        {
                            ((CheckBox)control).Checked = false;
                        }
                        else if (control is Label)
                        {
                            ((Label)control).Text = "";
                        }
                        else if (control is HiddenField)
                        {
                            ((HiddenField)control).Value = "";
                        }
                    }
                }
            }

            foreach (GridViewRow row in gvSolarDtl.Rows) //Solar System
            {
                // Check if the row is a data row
                if (row.RowType == DataControlRowType.DataRow)
                {
                    // Loop through controls in the row
                    foreach (Control control in row.Controls)
                    {
                        // Clear TextBox controls
                        if (control is TextBox)
                        {
                            ((TextBox)control).Text = string.Empty;
                        }
                        // Clear DropDownList controls
                        else if (control is DropDownList)
                        {
                            ((DropDownList)control).SelectedIndex = -1; // Or set to a default value
                        }
                        // Clear CheckBox controls
                        else if (control is CheckBox)
                        {
                            ((CheckBox)control).Checked = false;
                        }
                        else if (control is Label)
                        {
                            ((Label)control).Text = "";
                        }
                        else if (control is HiddenField)
                        {
                            ((HiddenField)control).Value = "";
                        }
                    }
                }
            }

            foreach (GridViewRow row in gvInsuChrgDtl.Rows) //Insurance and Charges
            {
                // Check if the row is a data row
                if (row.RowType == DataControlRowType.DataRow)
                {
                    // Loop through controls in the row
                    foreach (Control control in row.Controls)
                    {
                        // Clear TextBox controls
                        if (control is TextBox)
                        {
                            ((TextBox)control).Text = string.Empty;
                        }
                        // Clear DropDownList controls
                        else if (control is DropDownList)
                        {
                            ((DropDownList)control).SelectedIndex = -1; // Or set to a default value
                        }
                        // Clear CheckBox controls
                        else if (control is CheckBox)
                        {
                            ((CheckBox)control).Checked = false;
                        }
                        else if (control is Label)
                        {
                            ((Label)control).Text = "";
                        }
                        else if (control is HiddenField)
                        {
                            ((HiddenField)control).Value = "";
                        }
                    }
                }
            }

            foreach (GridViewRow row in gvDevDtl.Rows) //Deviation
            {
                // Check if the row is a data row
                if (row.RowType == DataControlRowType.DataRow)
                {
                    // Loop through controls in the row
                    foreach (Control control in row.Controls)
                    {
                        // Clear TextBox controls
                        if (control is TextBox)
                        {
                            ((TextBox)control).Text = string.Empty;
                        }
                        // Clear DropDownList controls
                        else if (control is DropDownList)
                        {
                            ((DropDownList)control).SelectedIndex = -1; // Or set to a default value
                        }
                        // Clear CheckBox controls
                        else if (control is CheckBox)
                        {
                            ((CheckBox)control).Checked = false;
                        }
                        else if (control is Label)
                        {
                            ((Label)control).Text = "";
                        }
                        else if (control is HiddenField)
                        {
                            ((HiddenField)control).Value = "";
                        }
                    }
                }
            }

            foreach (GridViewRow row in gvSancDtl.Rows) //Sanction Condition
            {
                // Check if the row is a data row
                if (row.RowType == DataControlRowType.DataRow)
                {
                    // Loop through controls in the row
                    foreach (Control control in row.Controls)
                    {
                        // Clear TextBox controls
                        if (control is TextBox)
                        {
                            ((TextBox)control).Text = string.Empty;
                        }
                        // Clear DropDownList controls
                        else if (control is DropDownList)
                        {
                            ((DropDownList)control).SelectedIndex = -1; // Or set to a default value
                        }
                        // Clear CheckBox controls
                        else if (control is CheckBox)
                        {
                            ((CheckBox)control).Checked = false;
                        }
                        else if (control is Label)
                        {
                            ((Label)control).Text = "";
                        }
                        else if (control is HiddenField)
                        {
                            ((HiddenField)control).Value = "";
                        }
                    }
                }
            }

            foreach (GridViewRow row in gvEmpDocument.Rows) //Document Upload
            {
                // Check if the row is a data row
                if (row.RowType == DataControlRowType.DataRow)
                {
                    // Loop through controls in the row
                    foreach (Control control in row.Controls)
                    {
                        // Clear TextBox controls
                        if (control is TextBox)
                        {
                            ((TextBox)control).Text = string.Empty;
                        }
                        // Clear DropDownList controls
                        else if (control is DropDownList)
                        {
                            ((DropDownList)control).SelectedIndex = -1; // Or set to a default value
                        }
                        // Clear CheckBox controls
                        else if (control is CheckBox)
                        {
                            ((CheckBox)control).Checked = false;
                        }
                        else if (control is Label)
                        {
                            ((Label)control).Text = "";
                        }
                        else if (control is HiddenField)
                        {
                            ((HiddenField)control).Value = "";
                        }
                    }
                }
            }

        }

        protected void gvEmpDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int i = e.Row.RowIndex;
                DataTable dtIns = null;
                CApplication oApp = null;
                try
                {
                    oApp = new CApplication();
                    FileUpload FUKYcDoc = (FileUpload)e.Row.FindControl("FUKYcDoc");
                    DropDownList ddlDocType = (DropDownList)e.Row.FindControl("ddlDocType");
                    TextBox txtDocRemarks = (TextBox)e.Row.FindControl("txtDocRemarks");
                    TextBox txtDocPassword = (TextBox)e.Row.FindControl("txtDocPassword");
                    HiddenField hdnDocPassword = (HiddenField)e.Row.FindControl("hdnDocPassword");

                    ddlDocType.Items.Clear();
                    dtIns = oApp.GetDocumentMst("DOC");
                    if (dtIns.Rows.Count > 0)
                    {
                        ddlDocType.DataSource = dtIns;
                        ddlDocType.DataTextField = "DocName";
                        ddlDocType.DataValueField = "DocID";
                        ddlDocType.DataBind();
                    }
                    ListItem oLk = new ListItem("<--Select-->", "-1");
                    ddlDocType.Items.Insert(0, oLk);


                    ddlDocType.SelectedIndex = ddlDocType.Items.IndexOf(ddlDocType.Items.FindByValue(e.Row.Cells[7].Text.Trim()));

                    txtDocPassword.TextMode = TextBoxMode.Password;
                    txtDocPassword.Attributes.Add("value", hdnDocPassword.Value);

                }
                finally
                {

                }
            }

        }

        protected void ClearLabel()
        {
            lblApplNmBD.Text = "";
            lblBCPNumBD.Text = "";
            lblBCPNum1.Text = "";
            lblBCPNum2.Text = "";
            lblBCPNum3.Text = "";
            lblApplNm1.Text = "";
            lblApplNm2.Text = "";
            lblApplNm3.Text = "";
            lblBCPNumIC.Text = "";
            lblBCPNumIC.Text = "";
            lblBCPNumIC.Text = "";
            lblApplNmEC.Text = "";
            txtLoanApplicationNo.Text = "";
            lblApplNmVR.Text = "";
            txtLoanApplicationNoV.Text = "";
            lblApplNmOR.Text = "";
            txtLoanApplicationNoOth.Text = "";
            lblBasicName.Text = "";
            lblApplNmED.Text = "";
            lblBasicNamebus.Text = "";
            lblApplNmBN.Text = "";
            lblBCPNo.Text = "";
            lblApplicantNm.Text = "";
            lblBCPNmOB1.Text = "";
            lblBCPNmOB2.Text = "";
            lblBCPNmOB3.Text = "";
            lblApplicantName.Text = "";
            lblAppName.Text = "";
            lblAppNameOB3.Text = "";
            lblAppNameEC.Text = "";
            lblBCPNmEC.Text = "";
            lblAppNameBK.Text = "";
            lblBCPNmBK.Text = "";
            lblAppNameSP.Text = "";
            lblBCPNmSP.Text = "";
            lblAppNmIN.Text = "";
            lblBCPNmIN.Text = "";
            lblAppNmLTV.Text = "";
            lblBCPNmLTV.Text = "";
            lblAppNameDV.Text = "";
            lblBCPNmDV.Text = "";
            lblAppNmSC.Text = "";
            lblBCPNmSC.Text = "";
            lblApplNmDU.Text = "";
            txtBCProposalNo.Text = "";
            lblProposalNo.Text = "";
            lblApplName.Text = "";
        }

        protected void btnShowInfo_Click(object sender, EventArgs e)
        {
            ClearControls(this.Page);

            ImageButton btn = (ImageButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            hdLeadId.Value = btn.CommandArgument;
            hdBasicId.Value = row.Cells[1].Text;
            hdMemberId.Value = row.Cells[2].Text;
            hdMemberId.Value = hdMemberId.Value.Replace("&nbsp;", "");
            if (hdMemberId.Value != "")
            {
                hdIdFlag.Value = "Y";
            }
            else
            {
                hdIdFlag.Value = "N";
            }

            hdAssMtdId.Value = row.Cells[3].Text;
            hdStatus.Value = row.Cells[4].Text;
            hdIncomeStatus.Value = row.Cells[5].Text;
            hdAssMtdTypId.Value = row.Cells[6].Text;
            ViewState["BcProNo"] = row.Cells[8].Text;
            ViewState["AppNm"] = row.Cells[9].Text;


            // START Storing data into session variable

            Session[gblValue.LeadID] = hdLeadId.Value;
            Session[gblValue.MemberID] = hdMemberId.Value;
            Session[gblValue.BCPNO] = row.Cells[8].Text;
            Session[gblValue.ApplNm] = row.Cells[9].Text;
            Session[gblValue.EmpStatus] = hdStatus.Value;
            Session[gblValue.AssMtdId] = hdAssMtdId.Value;
            Session[gblValue.IncomeStatus] = hdIncomeStatus.Value;
            Session[gblValue.AssMtdTypId] = hdAssMtdTypId.Value;
            Session[gblValue.InsuStatus] = row.Cells[15].Text;


            //END Storing data into session variable

            gblFuction.AjxMsgPopup("Proceed with Tab Selection...");
            return;
        }

        protected void txtSacDt_textChanged(object sender, EventArgs e)
        {
            Datepop();
        }
        private void Datepop()
        {

            DateTime originalDate;
            if (txtSacDt.Text != "")
            {
                originalDate = gblFuction.setDate(txtSacDt.Text.ToString());
                DateTime newDate = originalDate.AddDays(45);
                txtSacExDt.Text = newDate.ToString("dd/MM/yyyy");
            }
            else
            {
                txtSacExDt.Text = "";
            }

        }
        protected void gvDevDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                DropDownList ddlAuthority = (DropDownList)e.Row.FindControl("ddlAuthority");
                ddlAuthority.SelectedIndex = ddlAuthority.Items.IndexOf(ddlAuthority.Items.FindByValue(e.Row.Cells[0].Text));

            }
        }
        private void tabChangeFunction(int ActiveTabIndex)
        {
            ClearLabel();
            string vIsFileUpload, vIsUpload;
            DataTable dt = null, dt1 = null, dt2 = null, dt3 = null, dt4 = null, dt5 = null;
            DataSet ds = new DataSet();
            CDistrict oDist = new CDistrict();
            CMember oMem = new CMember();
            CCust360 oC360 = new CCust360();
            CCFIncDtl oDit = new CCFIncDtl();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            int MaxAuthVal = 0;

            if (hdBasicId.Value == "")
            {
                hdBasicId.Value = "0";
            }
            if (hdLeadId.Value == "")
            {
                hdLeadId.Value = "0";
            }
            if (hdAssMtdId.Value == "")
            {
                hdAssMtdId.Value = "0";
            }
            if (hdAssMtdTypId.Value == "")
            {
                hdAssMtdTypId.Value = "0";
            }

            switch (ActiveTabIndex)
            {
                case 0:

                    break;
                case 1:
                    #region BasicDtls
                    try
                    {
                        if (ViewState["AppNm"] != null)
                        {
                            lblApplNmBD.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblApplNmBD.Text = "";
                        }

                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPNumBD.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBCPNumBD.Text = "";
                        }
                        dt = oDist.CF_GetBasicDetailsById(Convert.ToInt32(hdBasicId.Value));
                        if (dt.Rows.Count > 0)
                        {
                            PopBranch();
                            PopSolarSystemType();

                            PopSegType();
                            PopAppliEntType();
                            PopEPCMst();
                            PopRO();
                            popState();
                            PopAssesMethod();
                            txtBCPartName.Text = "Choice Finserv Pvt. ltd.";
                            hdBasicId.Value = Convert.ToString(dt.Rows[0]["BasicID"]);
                            hdLeadId.Value = Convert.ToString(dt.Rows[0]["LeadID"]);
                            //ddlBCPropNo.SelectedIndex = ddlBCPropNo.Items.IndexOf(ddlBCPropNo.Items.FindByText(dt.Rows[0]["BCPropNo"].ToString()));
                            //---------------Populate BCPropNo-----------------------
                            ddlBCPropNo.Items.Clear();
                            ddlBCPropNo.DataSource = dt;
                            ddlBCPropNo.DataTextField = "BCPropNo";
                            ddlBCPropNo.DataValueField = "LeadID";
                            ddlBCPropNo.DataBind();
                            //-------------------------------------------------------
                            ddlBCPropNo.SelectedIndex = ddlBCPropNo.Items.IndexOf(ddlBCPropNo.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                            txtNatProp.Text = Convert.ToString(dt.Rows[0]["NatOfPrpsl"]);
                            if (Convert.ToString(dt.Rows[0]["BCPartnrName"]).Trim() != "") txtBCPartName.Text = Convert.ToString(dt.Rows[0]["BCPartnrName"]);


                            ddlBCStste.ClearSelection();
                            ListItem oli = new ListItem(Convert.ToString(dt.Rows[0]["BCState"]), "-1");
                            ddlBCStste.Items.Insert(0, oli);
                            ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                            ddlBcRM.SelectedIndex = ddlBcRM.Items.IndexOf(ddlBcRM.Items.FindByValue(dt.Rows[0]["BCRMID"].ToString()));
                            ddlEPC.SelectedIndex = ddlEPC.Items.IndexOf(ddlEPC.Items.FindByValue(dt.Rows[0]["EPCId"].ToString()));
                            ddlSolPwrSys.SelectedIndex = ddlSolPwrSys.Items.IndexOf(ddlSolPwrSys.Items.FindByValue(dt.Rows[0]["SolPwrSysId"].ToString()));
                            txtDistnce.Text = Convert.ToString(dt.Rows[0]["SourceDistnc"]);
                            txtApplicant.Text = Convert.ToString(dt.Rows[0]["ApplName"]);
                            txtAppNo.Text = Convert.ToString(dt.Rows[0]["AppNo"]);
                            txtAppDt.Text = Convert.ToString(dt.Rows[0]["AppDate"]);
                            ddlEntityType.SelectedIndex = ddlEntityType.Items.IndexOf(ddlEntityType.Items.FindByValue(dt.Rows[0]["ApplEntTypeId"].ToString()));
                            ddlSegType.SelectedIndex = ddlSegType.Items.IndexOf(ddlSegType.Items.FindByValue(dt.Rows[0]["SegTypeID"].ToString()));
                            ddlAssMethod.ClearSelection();
                            ListItem oli7 = new ListItem(Convert.ToString(dt.Rows[0]["MethodName"]), "-1");
                            ddlAssMethod.Items.Insert(0, oli7);
                            ddlPurFacilty.SelectedIndex = ddlPurFacilty.Items.IndexOf(ddlPurFacilty.Items.FindByValue(dt.Rows[0]["PurposeFclty"].ToString()));
                            ddlPrioSec.SelectedIndex = ddlPrioSec.Items.IndexOf(ddlPrioSec.Items.FindByValue(dt.Rows[0]["PriorSecID"].ToString()));
                            txtPSLClass.Text = Convert.ToString(dt.Rows[0]["PSLClass"]);
                            if (Convert.ToString(dt.Rows[0]["EmpProfile"]) != "")
                            {
                                ddlEmpProfile.SelectedValue = Convert.ToString(dt.Rows[0]["EmpProfile"]);
                            }
                            else
                            {
                                ddlEmpProfile.SelectedIndex = -1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion

                    #region Customer360
                    try
                    {
                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPNum1.Text = ViewState["BcProNo"].ToString();
                            lblBCPNum2.Text = ViewState["BcProNo"].ToString();
                            lblBCPNum3.Text = ViewState["BcProNo"].ToString();
                            lblBCPNum4.Text = ViewState["BcProNo"].ToString();
                            lblBCPNum5.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBCPNum1.Text = "";
                            lblBCPNum2.Text = "";
                            lblBCPNum3.Text = "";
                            lblApplNm4.Text = "";
                            lblApplNm5.Text = "";
                        }


                        if (ViewState["AppNm"] != null)
                        {
                            lblApplNm1.Text = ViewState["AppNm"].ToString();
                            lblApplNm2.Text = ViewState["AppNm"].ToString();
                            lblApplNm3.Text = ViewState["AppNm"].ToString();
                            lblApplNm4.Text = ViewState["AppNm"].ToString();
                            lblApplNm5.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblApplNm1.Text = "";
                            lblApplNm2.Text = "";
                            lblApplNm3.Text = "";
                            lblApplNm4.Text = "";
                            lblApplNm5.Text = "";
                        }

                        ds = oC360.CF_GetMemberDtl(hdMemberId.Value);
                        dt = ds.Tables[0];
                        dt1 = ds.Tables[1];
                        dt2 = ds.Tables[2];
                        dt3 = ds.Tables[3];
                        dt4 = ds.Tables[4];
                        dt5 = ds.Tables[5];

                        if (dt.Rows.Count > 0)
                        {
                            ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                            hdnLeadId.Value = dt.Rows[0]["LeadID"].ToString();
                            hdnAppId.Value = Convert.ToString(dt.Rows[0]["MemberID"]);
                            txtCustId.Text = Convert.ToString(dt.Rows[0]["MemberNo"]);
                            txtCustName.Text = Convert.ToString(dt.Rows[0]["MemberName"]);
                            ddlApplGender.SelectedIndex = ddlApplGender.Items.IndexOf(ddlApplGender.Items.FindByValue(dt.Rows[0]["Gender"].ToString()));
                            txtApplDOB.Text = Convert.ToString(dt.Rows[0]["DoB"]);
                            txtApplAge.Text = Convert.ToString(dt.Rows[0]["PresentAge"]);
                            txtApplAgeMaturity.Text = Convert.ToString(dt.Rows[0]["AgeAtLoanMaturity"]);
                            txtApplPan.Text = Convert.ToString(dt.Rows[0]["PanNo"]);
                            txtApplAadhRefNo.Text = Convert.ToString(dt.Rows[0]["AadhRefNo"]);
                            txtApplAadhNo.Text = Convert.ToString(dt.Rows[0]["AadhMaskedNo"]);

                            txtApplVoterNo.Text = Convert.ToString(dt.Rows[0]["VoterID"]);
                            ddlRelWithApp.SelectedIndex = ddlRelWithApp.Items.IndexOf(ddlRelWithApp.Items.FindByValue(dt.Rows[0]["RelWithApp"].ToString()));
                            txtAppMob.Text = Convert.ToString(dt.Rows[0]["MobNo"]);
                            ddlAppEdu.SelectedIndex = ddlAppEdu.Items.IndexOf(ddlAppEdu.Items.FindByValue(dt.Rows[0]["Education"].ToString()));
                            ddlApplMaritalStatus.SelectedIndex = ddlApplMaritalStatus.Items.IndexOf(ddlApplMaritalStatus.Items.FindByValue(dt.Rows[0]["MaritalStatus"].ToString()));
                            txtNoOfFamilyMem.Text = Convert.ToString(dt.Rows[0]["TotalNoFamMem"]);
                            txtNoOfDependent.Text = Convert.ToString(dt.Rows[0]["NoOfDependents"]);
                            ddlApplCast.SelectedIndex = ddlApplCast.Items.IndexOf(ddlApplCast.Items.FindByValue(dt.Rows[0]["Caste"].ToString()));
                            ddlApplReligion.SelectedIndex = ddlApplReligion.Items.IndexOf(ddlApplReligion.Items.FindByValue(dt.Rows[0]["Religion"].ToString()));
                            ddlAppMinorityYN.SelectedIndex = ddlAppMinorityYN.Items.IndexOf(ddlAppMinorityYN.Items.FindByValue(dt.Rows[0]["Minority"].ToString()));
                            txtApplEmail.Text = Convert.ToString(dt.Rows[0]["Email"]);
                            txtAppPerAddress.Text = Convert.ToString(dt.Rows[0]["PerAdd"]);


                            ddlApplState.SelectedIndex = ddlApplState.Items.IndexOf(ddlApplState.Items.FindByValue(dt.Rows[0]["PerState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlApplState.SelectedValue), "AppPer");
                            ddlApplDist.SelectedIndex = ddlApplDist.Items.IndexOf(ddlApplDist.Items.FindByValue(dt.Rows[0]["PerDist"].ToString()));

                            txtApplPin.Text = Convert.ToString(dt.Rows[0]["PerPin"]);
                            txtApplCurrAddress.Text = Convert.ToString(dt.Rows[0]["CurrAdd"]);

                            ddlApplCurrState.SelectedIndex = ddlApplCurrState.Items.IndexOf(ddlApplCurrState.Items.FindByValue(dt.Rows[0]["CurrState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlApplCurrState.SelectedValue), "AppCurr");
                            ddlApplCurrDist.SelectedIndex = ddlApplCurrDist.Items.IndexOf(ddlApplCurrDist.Items.FindByValue(dt.Rows[0]["CurrDist"].ToString()));

                            txtApplCurrPin.Text = Convert.ToString(dt.Rows[0]["CurrPin"]);
                            txtApplLandMark.Text = Convert.ToString(dt.Rows[0]["CurrLandmark"]);
                            ddlApplOwnshipStatus.SelectedIndex = ddlApplOwnshipStatus.Items.IndexOf(ddlApplOwnshipStatus.Items.FindByValue(dt.Rows[0]["OwnShipStatus"].ToString()));
                            ddlResiStabYrs.SelectedIndex = ddlResiStabYrs.Items.IndexOf(ddlResiStabYrs.Items.FindByValue(dt.Rows[0]["ResiStabYrs"].ToString()));

                            hdnAppPanVerifyYN.Value = Convert.ToString(dt.Rows[0]["PanVerifyYN"]);
                            hdnAppAadhVerifyYN.Value = Convert.ToString(dt.Rows[0]["AadhVerifyYN"]);
                            hdnAppVoterVerifyYN.Value = Convert.ToString(dt.Rows[0]["VoterVerifyYN"]);


                            if (Convert.ToString(dt.Rows[0]["PanVerifyYN"]) == "Y")
                            {
                                btnVerifyAppPan.Enabled = false;
                                txtApplPan.Enabled = false;
                                hdnAppPanVerifyYN.Value = "Y";
                            }
                            if (Convert.ToString(dt.Rows[0]["AadhVerifyYN"]) == "Y")
                            {
                                btnVerifyAppAadhaar.Enabled = false;
                                txtApplAadhNo.Enabled = false;
                                hdnAppAadhVerifyYN.Value = "Y";
                            }

                            if (Convert.ToString(dt.Rows[0]["VoterVerifyYN"]) == "Y")
                            {
                                btnVerifyAppVoter.Enabled = false;
                                txtApplVoterNo.Enabled = false;
                                hdnAppVoterVerifyYN.Value = "Y";
                            }

                            hdnAppOwnshipExt.Value = Convert.ToString(dt.Rows[0]["OwnShipExt"]);
                            lblAppOwnship.Text = "ApplOwnship" + hdnAppOwnshipExt.Value;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                            hdnCoApp1Id.Value = Convert.ToString(dt1.Rows[0]["CoAppID"]);
                            txtCoApp1Id.Text = Convert.ToString(dt1.Rows[0]["CoAppNo"]);
                            txtCoApp1Name.Text = Convert.ToString(dt1.Rows[0]["CoAppName"]);
                            ddlCoApp1Gender.SelectedIndex = ddlCoApp1Gender.Items.IndexOf(ddlCoApp1Gender.Items.FindByValue(dt1.Rows[0]["Gender"].ToString()));
                            txtCoApp1Dob.Text = Convert.ToString(dt1.Rows[0]["DoB"]);
                            txtCoApp1Age.Text = Convert.ToString(dt1.Rows[0]["PresentAge"]);
                            txtCoApp1AgeAtLoanMaturity.Text = Convert.ToString(dt1.Rows[0]["AgeAtLoanMaturity"]);
                            txtCoApp1Pan.Text = Convert.ToString(dt1.Rows[0]["PanNo"]);
                            txtCoApp1AadhRefNo.Text = Convert.ToString(dt1.Rows[0]["AadhRefNo"]);
                            txtCoApp1AadhNo.Text = Convert.ToString(dt.Rows[0]["AadhMaskedNo"]);
                            txtCoApp1Voter.Text = Convert.ToString(dt1.Rows[0]["VoterID"]);
                            ddlCoApp1RelWithApp.SelectedIndex = ddlRelWithApp.Items.IndexOf(ddlRelWithApp.Items.FindByValue(dt1.Rows[0]["RelWithApp"].ToString()));
                            txtCoApp1Mob.Text = Convert.ToString(dt1.Rows[0]["MobNo"]);
                            ddlCoApp1Edu.SelectedIndex = ddlCoApp1Edu.Items.IndexOf(ddlCoApp1Edu.Items.FindByValue(dt1.Rows[0]["Education"].ToString()));
                            ddlCoApp1Marital.SelectedIndex = ddlCoApp1Marital.Items.IndexOf(ddlCoApp1Marital.Items.FindByValue(dt1.Rows[0]["MaritalStatus"].ToString()));
                            txtCoApp1NoOfFamily.Text = Convert.ToString(dt1.Rows[0]["TotalNoFamMem"]);
                            txtCoApp1NoOfDependents.Text = Convert.ToString(dt1.Rows[0]["NoOfDependents"]);
                            ddlCoApp1Caste.SelectedIndex = ddlCoApp1Caste.Items.IndexOf(ddlCoApp1Caste.Items.FindByValue(dt1.Rows[0]["Caste"].ToString()));
                            ddlCoApp1Religion.SelectedIndex = ddlCoApp1Religion.Items.IndexOf(ddlCoApp1Religion.Items.FindByValue(dt1.Rows[0]["Religion"].ToString()));
                            ddlCoApp1Minority.SelectedIndex = ddlCoApp1Minority.Items.IndexOf(ddlCoApp1Minority.Items.FindByValue(dt1.Rows[0]["Minority"].ToString()));
                            txtCoApp1Email.Text = Convert.ToString(dt1.Rows[0]["Email"]);
                            txtCoApp1PerResiAdd.Text = Convert.ToString(dt1.Rows[0]["PerAdd"]);


                            ddlCoApp1PerState.SelectedIndex = ddlCoApp1PerState.Items.IndexOf(ddlCoApp1PerState.Items.FindByValue(dt1.Rows[0]["PerState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlCoApp1PerState.SelectedValue), "CoApp1Per");
                            ddlCoApp1PerDist.SelectedIndex = ddlCoApp1PerDist.Items.IndexOf(ddlCoApp1PerDist.Items.FindByValue(dt1.Rows[0]["PerDist"].ToString()));

                            txtCoApp1PerPin.Text = Convert.ToString(dt1.Rows[0]["PerPin"]);
                            txtCoApp1CurrAdd.Text = Convert.ToString(dt1.Rows[0]["CurrAdd"]);

                            ddlCoApp1CurrState.SelectedIndex = ddlCoApp1CurrState.Items.IndexOf(ddlCoApp1CurrState.Items.FindByValue(dt1.Rows[0]["CurrState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlCoApp1CurrState.SelectedValue), "CoApp1Curr");
                            ddlCoApp1CurrDist.SelectedIndex = ddlCoApp1CurrDist.Items.IndexOf(ddlCoApp1CurrDist.Items.FindByValue(dt1.Rows[0]["CurrDist"].ToString()));

                            txtCoApp1CurrPin.Text = Convert.ToString(dt1.Rows[0]["CurrPin"]);
                            txtCoApp1Landmark.Text = Convert.ToString(dt1.Rows[0]["CurrLandmark"]);
                            ddlCoApp1OwnShip.SelectedIndex = ddlCoApp1OwnShip.Items.IndexOf(ddlCoApp1OwnShip.Items.FindByValue(dt1.Rows[0]["OwnShipStatus"].ToString()));
                            ddlCoApp1ResiStabYrs.SelectedIndex = ddlCoApp1ResiStabYrs.Items.IndexOf(ddlCoApp1ResiStabYrs.Items.FindByValue(dt1.Rows[0]["ResiStabYrs"].ToString()));

                            hdnCoApp1PanVerifyYN.Value = Convert.ToString(dt1.Rows[0]["PanVerifyYN"]);
                            hdnCoApp1AadhVerifyYN.Value = Convert.ToString(dt1.Rows[0]["AadhVerifyYN"]);
                            hdnCoApp1VoterVerifyYN.Value = Convert.ToString(dt1.Rows[0]["VoterVerifyYN"]);


                            if (Convert.ToString(dt1.Rows[0]["PanVerifyYN"]) == "Y")
                            {
                                btnVerifyCoApp1Pan.Enabled = false;
                                txtCoApp1Pan.Enabled = false;
                                hdnCoApp1PanVerifyYN.Value = "Y";
                            }
                            if (Convert.ToString(dt1.Rows[0]["AadhVerifyYN"]) == "Y")
                            {
                                btnVerifyCoApp1Aadhaar.Enabled = false;
                                txtCoApp1AadhNo.Enabled = false;
                                hdnCoApp1AadhVerifyYN.Value = "Y";
                            }

                            if (Convert.ToString(dt1.Rows[0]["VoterVerifyYN"]) == "Y")
                            {
                                btnVerifyCoApp1Voter.Enabled = false;
                                txtCoApp1Voter.Enabled = false;
                                hdnCoApp1VoterVerifyYN.Value = "Y";
                            }

                            hdnCoApp1OwnShipExt.Value = Convert.ToString(dt1.Rows[0]["OwnShipExt"]);
                            lbloApp1OwnShip.Text = "CA1OwnshipProof" + hdnCoApp1OwnShipExt.Value;
                        }
                        if (dt2.Rows.Count > 0)
                        {
                            ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                            hdnCoApp2CustId.Value = Convert.ToString(dt2.Rows[0]["CoAppID"]);
                            txtCoApp2CustId.Text = Convert.ToString(dt2.Rows[0]["CoAppNo"]);
                            txtCoApp2CustName.Text = Convert.ToString(dt2.Rows[0]["CoAppName"]);
                            ddlCoApp2Gender.SelectedIndex = ddlCoApp2Gender.Items.IndexOf(ddlCoApp2Gender.Items.FindByValue(dt2.Rows[0]["Gender"].ToString()));
                            txtCoApp2Dob.Text = Convert.ToString(dt2.Rows[0]["DoB"]);
                            txtCoApp2Age.Text = Convert.ToString(dt2.Rows[0]["PresentAge"]);
                            txtCoApp2AgeAtLoanMaturity.Text = Convert.ToString(dt2.Rows[0]["AgeAtLoanMaturity"]);
                            txtCoApp2Pan.Text = Convert.ToString(dt2.Rows[0]["PanNo"]);
                            txtCoApp2AadhRefNo.Text = Convert.ToString(dt2.Rows[0]["AadhRefNo"]);
                            txtCoApp2AadhNo.Text = Convert.ToString(dt.Rows[0]["AadhMaskedNo"]);
                            txtCoApp2Voter.Text = Convert.ToString(dt2.Rows[0]["VoterID"]);
                            ddlCoApp2RelWithApp.SelectedIndex = ddlCoApp2RelWithApp.Items.IndexOf(ddlCoApp2RelWithApp.Items.FindByValue(dt2.Rows[0]["RelWithApp"].ToString()));
                            txtCoApp2Mob.Text = Convert.ToString(dt2.Rows[0]["MobNo"]);
                            ddlCoApp2Edu.SelectedIndex = ddlCoApp2Edu.Items.IndexOf(ddlCoApp2Edu.Items.FindByValue(dt2.Rows[0]["Education"].ToString()));
                            ddlCoApp2Marital.SelectedIndex = ddlCoApp2Marital.Items.IndexOf(ddlCoApp2Marital.Items.FindByValue(dt2.Rows[0]["MaritalStatus"].ToString()));
                            txtCoApp2NoOfFamily.Text = Convert.ToString(dt2.Rows[0]["TotalNoFamMem"]);
                            txtCoApp2NoOfDependents.Text = Convert.ToString(dt2.Rows[0]["NoOfDependents"]);
                            ddlCoApp2Caste.SelectedIndex = ddlCoApp2Caste.Items.IndexOf(ddlCoApp2Caste.Items.FindByValue(dt2.Rows[0]["Caste"].ToString()));
                            ddlCoApp2Religion.SelectedIndex = ddlCoApp2Religion.Items.IndexOf(ddlCoApp2Religion.Items.FindByValue(dt2.Rows[0]["Religion"].ToString()));
                            ddlCoApp2Minority.SelectedIndex = ddlCoApp2Minority.Items.IndexOf(ddlCoApp2Minority.Items.FindByValue(dt2.Rows[0]["Minority"].ToString()));
                            txtCoApp2Email.Text = Convert.ToString(dt2.Rows[0]["Email"]);
                            txtCoApp2PerResiAdd.Text = Convert.ToString(dt2.Rows[0]["PerAdd"]);


                            ddlCoApp2PerState.SelectedIndex = ddlCoApp2PerState.Items.IndexOf(ddlCoApp2PerState.Items.FindByValue(dt2.Rows[0]["PerState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlCoApp2PerState.SelectedValue), "CoApp2Per");
                            ddlCoApp2PerDist.SelectedIndex = ddlCoApp2PerDist.Items.IndexOf(ddlCoApp2PerDist.Items.FindByValue(dt2.Rows[0]["PerDist"].ToString()));

                            txtCoApp2PerPin.Text = Convert.ToString(dt2.Rows[0]["PerPin"]);
                            txtCoApp2CurrResiAdd.Text = Convert.ToString(dt2.Rows[0]["CurrAdd"]);

                            ddlCoApp2CurrState.SelectedIndex = ddlCoApp2CurrState.Items.IndexOf(ddlCoApp2CurrState.Items.FindByValue(dt2.Rows[0]["CurrState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlCoApp2CurrState.SelectedValue), "CoApp2Curr");
                            ddlCoApp2CurrDist.SelectedIndex = ddlCoApp2CurrDist.Items.IndexOf(ddlCoApp2CurrDist.Items.FindByValue(dt2.Rows[0]["CurrDist"].ToString()));

                            txtCoApp2CurrPin.Text = Convert.ToString(dt2.Rows[0]["CurrPin"]);
                            txtCoApp2Landmark.Text = Convert.ToString(dt2.Rows[0]["CurrLandmark"]);
                            ddlCoApp2Ownship.SelectedIndex = ddlCoApp2Ownship.Items.IndexOf(ddlCoApp2Ownship.Items.FindByValue(dt2.Rows[0]["OwnShipStatus"].ToString()));
                            ddlCoApp2ResiStabYrs.SelectedIndex = ddlCoApp2ResiStabYrs.Items.IndexOf(ddlCoApp2ResiStabYrs.Items.FindByValue(dt2.Rows[0]["ResiStabYrs"].ToString()));

                            hdnCoApp2PanVerifyYN.Value = Convert.ToString(dt2.Rows[0]["PanVerifyYN"]);
                            hdnCoApp2AadhVerifyYN.Value = Convert.ToString(dt2.Rows[0]["AadhVerifyYN"]);
                            hdnCoApp2VoterVerifyYN.Value = Convert.ToString(dt2.Rows[0]["VoterVerifyYN"]);



                            if (Convert.ToString(dt2.Rows[0]["PanVerifyYN"]) == "Y")
                            {
                                btnVerifyCoApp2Pan.Enabled = false;
                                txtCoApp2Pan.Enabled = false;
                                hdnCoApp2PanVerifyYN.Value = "Y";
                            }
                            if (Convert.ToString(dt2.Rows[0]["AadhVerifyYN"]) == "Y")
                            {
                                btnVerifyCoApp2Aadhaar.Enabled = false;
                                txtCoApp2AadhNo.Enabled = false;
                                hdnCoApp2AadhVerifyYN.Value = "Y";
                            }

                            if (Convert.ToString(dt2.Rows[0]["VoterVerifyYN"]) == "Y")
                            {
                                btnVerifyCoApp2Voter.Enabled = false;
                                txtCoApp2Voter.Enabled = false;
                                hdnCoApp2VoterVerifyYN.Value = "Y";
                            }
                            hdnCoApp2OwnShipExt.Value = Convert.ToString(dt2.Rows[0]["OwnShipExt"]);
                            lblCoApp2OwnShip.Text = "CA2OwnshipProof" + hdnCoApp2OwnShipExt.Value;
                        }
                        if (dt3.Rows.Count > 0)
                        {
                            ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                            hdnGuarId.Value = Convert.ToString(dt3.Rows[0]["CoAppID"]);
                            txtGuarId.Text = Convert.ToString(dt3.Rows[0]["CoAppNo"]);
                            txtGuarName.Text = Convert.ToString(dt3.Rows[0]["CoAppName"]);
                            ddlGuarGender.SelectedIndex = ddlGuarGender.Items.IndexOf(ddlGuarGender.Items.FindByValue(dt3.Rows[0]["Gender"].ToString()));
                            txtGuarDOB.Text = Convert.ToString(dt3.Rows[0]["DoB"]);
                            txtGuarAge.Text = Convert.ToString(dt3.Rows[0]["PresentAge"]);
                            txtGuarAgeAtLoanMaturity.Text = Convert.ToString(dt3.Rows[0]["AgeAtLoanMaturity"]);
                            txtGuarPan.Text = Convert.ToString(dt3.Rows[0]["PanNo"]);
                            txtGuarAadhRefNo.Text = Convert.ToString(dt3.Rows[0]["AadhRefNo"]);
                            txtGuarAadhNo.Text = Convert.ToString(dt.Rows[0]["AadhMaskedNo"]);
                            txtGuarVoter.Text = Convert.ToString(dt3.Rows[0]["VoterID"]);
                            ddlGuarRelWithApp.SelectedIndex = ddlGuarRelWithApp.Items.IndexOf(ddlGuarRelWithApp.Items.FindByValue(dt3.Rows[0]["RelWithApp"].ToString()));
                            txtGuarMob.Text = Convert.ToString(dt3.Rows[0]["MobNo"]);
                            ddlGuarEdu.SelectedIndex = ddlGuarEdu.Items.IndexOf(ddlGuarEdu.Items.FindByValue(dt3.Rows[0]["Education"].ToString()));
                            ddlGuarMarital.SelectedIndex = ddlGuarMarital.Items.IndexOf(ddlGuarMarital.Items.FindByValue(dt3.Rows[0]["MaritalStatus"].ToString()));
                            txtGuarNoOfFamily.Text = Convert.ToString(dt3.Rows[0]["TotalNoFamMem"]);
                            txtGuarNoOfDependents.Text = Convert.ToString(dt3.Rows[0]["NoOfDependents"]);
                            ddlGuarCaste.SelectedIndex = ddlGuarCaste.Items.IndexOf(ddlGuarCaste.Items.FindByValue(dt3.Rows[0]["Caste"].ToString()));
                            ddlGuarReligion.SelectedIndex = ddlGuarReligion.Items.IndexOf(ddlGuarReligion.Items.FindByValue(dt3.Rows[0]["Religion"].ToString()));
                            ddlGuarMinority.SelectedIndex = ddlGuarMinority.Items.IndexOf(ddlGuarMinority.Items.FindByValue(dt3.Rows[0]["Minority"].ToString()));
                            txtGuarEmail.Text = Convert.ToString(dt3.Rows[0]["Email"]);
                            txtGuarPerResiAdd.Text = Convert.ToString(dt3.Rows[0]["PerAdd"]);

                            ddlGuarPerState.SelectedIndex = ddlGuarPerState.Items.IndexOf(ddlGuarPerState.Items.FindByValue(dt3.Rows[0]["PerState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlGuarPerState.SelectedValue), "GuarPer");
                            ddlGuarPerDist.SelectedIndex = ddlGuarPerDist.Items.IndexOf(ddlGuarPerDist.Items.FindByValue(dt3.Rows[0]["PerDist"].ToString()));

                            txtGuarPerPin.Text = Convert.ToString(dt3.Rows[0]["PerPin"]);
                            txtGuarCurrResiAdd.Text = Convert.ToString(dt3.Rows[0]["CurrAdd"]);

                            ddlGuarCurrState.SelectedIndex = ddlGuarCurrState.Items.IndexOf(ddlGuarCurrState.Items.FindByValue(dt3.Rows[0]["CurrState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlGuarCurrState.SelectedValue), "GuarCurr");
                            ddlGuarCurrDist.SelectedIndex = ddlGuarCurrDist.Items.IndexOf(ddlGuarCurrDist.Items.FindByValue(dt3.Rows[0]["CurrDist"].ToString()));

                            txtGuarCurrPin.Text = Convert.ToString(dt3.Rows[0]["CurrPin"]);
                            txtGuarLandmark.Text = Convert.ToString(dt3.Rows[0]["CurrLandmark"]);
                            ddlGuarOwnStat.SelectedIndex = ddlGuarOwnStat.Items.IndexOf(ddlGuarOwnStat.Items.FindByValue(dt3.Rows[0]["OwnShipStatus"].ToString()));
                            ddlGuarResiStabYrs.SelectedIndex = ddlGuarResiStabYrs.Items.IndexOf(ddlGuarResiStabYrs.Items.FindByValue(dt3.Rows[0]["ResiStabYrs"].ToString()));

                            hdnGuarPanVerifyYN.Value = Convert.ToString(dt3.Rows[0]["PanVerifyYN"]);
                            hdnGuarAadhVerifyYN.Value = Convert.ToString(dt3.Rows[0]["AadhVerifyYN"]);
                            hdnGuarVoterVerifyYN.Value = Convert.ToString(dt3.Rows[0]["VoterVerifyYN"]);



                            if (Convert.ToString(dt3.Rows[0]["PanVerifyYN"]) == "Y")
                            {
                                btnVerifyGuarPan.Enabled = false;
                                txtGuarPan.Enabled = false;
                                hdnGuarPanVerifyYN.Value = "Y";
                            }
                            if (Convert.ToString(dt3.Rows[0]["AadhVerifyYN"]) == "Y")
                            {
                                btnVerifyGuarAadhaar.Enabled = false;
                                txtGuarAadhNo.Enabled = false;
                                hdnGuarAadhVerifyYN.Value = "Y";
                            }

                            if (Convert.ToString(dt3.Rows[0]["VoterVerifyYN"]) == "Y")
                            {
                                btnVerifyGuarVoter.Enabled = false;
                                txtGuarVoter.Enabled = false;
                                hdnGuarVoterVerifyYN.Value = "Y";
                            }
                            hdnGuarOwnShipExt.Value = Convert.ToString(dt3.Rows[0]["OwnShipExt"]);
                            lblGuarOwnShip.Text = "GOwnshipProof" + hdnGuarOwnShipExt.Value;
                        }
                        if (dt4.Rows.Count > 0)
                        {
                            ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                            hdnCoApp3CustId.Value = Convert.ToString(dt4.Rows[0]["CoAppID"]);
                            txtCoApp3CustId.Text = Convert.ToString(dt4.Rows[0]["CoAppNo"]);
                            txtCoApp3CustName.Text = Convert.ToString(dt4.Rows[0]["CoAppName"]);
                            ddlCoApp3Gender.SelectedIndex = ddlCoApp3Gender.Items.IndexOf(ddlCoApp3Gender.Items.FindByValue(dt4.Rows[0]["Gender"].ToString()));
                            txtCoApp3Dob.Text = Convert.ToString(dt4.Rows[0]["DoB"]);
                            txtCoApp3Age.Text = Convert.ToString(dt4.Rows[0]["PresentAge"]);
                            txtCoApp3AgeAtLoanMaturity.Text = Convert.ToString(dt4.Rows[0]["AgeAtLoanMaturity"]);
                            txtCoApp3Pan.Text = Convert.ToString(dt4.Rows[0]["PanNo"]);
                            txtCoApp3AadhRefNo.Text = Convert.ToString(dt4.Rows[0]["AadhRefNo"]);
                            txtCoApp3AadhNo.Text = Convert.ToString(dt4.Rows[0]["AadhMaskedNo"]);
                            txtCoApp3Voter.Text = Convert.ToString(dt4.Rows[0]["VoterID"]);
                            ddlCoApp3RelWithApp.SelectedIndex = ddlCoApp3RelWithApp.Items.IndexOf(ddlCoApp3RelWithApp.Items.FindByValue(dt4.Rows[0]["RelWithApp"].ToString()));
                            txtCoApp3Mob.Text = Convert.ToString(dt4.Rows[0]["MobNo"]);
                            ddlCoApp3Edu.SelectedIndex = ddlCoApp3Edu.Items.IndexOf(ddlCoApp3Edu.Items.FindByValue(dt4.Rows[0]["Education"].ToString()));
                            ddlCoApp3Marital.SelectedIndex = ddlCoApp3Marital.Items.IndexOf(ddlCoApp3Marital.Items.FindByValue(dt4.Rows[0]["MaritalStatus"].ToString()));
                            txtCoApp3NoOfFamily.Text = Convert.ToString(dt4.Rows[0]["TotalNoFamMem"]);
                            txtCoApp3NoOfDependents.Text = Convert.ToString(dt4.Rows[0]["NoOfDependents"]);
                            ddlCoApp3Caste.SelectedIndex = ddlCoApp3Caste.Items.IndexOf(ddlCoApp3Caste.Items.FindByValue(dt4.Rows[0]["Caste"].ToString()));
                            ddlCoApp3Religion.SelectedIndex = ddlCoApp3Religion.Items.IndexOf(ddlCoApp3Religion.Items.FindByValue(dt4.Rows[0]["Religion"].ToString()));
                            ddlCoApp3Minority.SelectedIndex = ddlCoApp3Minority.Items.IndexOf(ddlCoApp3Minority.Items.FindByValue(dt4.Rows[0]["Minority"].ToString()));
                            txtCoApp3Email.Text = Convert.ToString(dt4.Rows[0]["Email"]);
                            txtCoApp3PerResiAdd.Text = Convert.ToString(dt4.Rows[0]["PerAdd"]);


                            ddlCoApp3PerState.SelectedIndex = ddlCoApp3PerState.Items.IndexOf(ddlCoApp3PerState.Items.FindByValue(dt4.Rows[0]["PerState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlCoApp3PerState.SelectedValue), "CoApp3Per");
                            ddlCoApp3PerDist.SelectedIndex = ddlCoApp3PerDist.Items.IndexOf(ddlCoApp3PerDist.Items.FindByValue(dt4.Rows[0]["PerDist"].ToString()));

                            txtCoApp3PerPin.Text = Convert.ToString(dt4.Rows[0]["PerPin"]);
                            txtCoApp3CurrResiAdd.Text = Convert.ToString(dt4.Rows[0]["CurrAdd"]);

                            ddlCoApp3CurrState.SelectedIndex = ddlCoApp3CurrState.Items.IndexOf(ddlCoApp3CurrState.Items.FindByValue(dt4.Rows[0]["CurrState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlCoApp3CurrState.SelectedValue), "CoApp3Curr");
                            ddlCoApp3CurrDist.SelectedIndex = ddlCoApp3CurrDist.Items.IndexOf(ddlCoApp3CurrDist.Items.FindByValue(dt4.Rows[0]["CurrDist"].ToString()));

                            txtCoApp3CurrPin.Text = Convert.ToString(dt4.Rows[0]["CurrPin"]);
                            txtCoApp3Landmark.Text = Convert.ToString(dt4.Rows[0]["CurrLandmark"]);
                            ddlCoApp3Ownship.SelectedIndex = ddlCoApp3Ownship.Items.IndexOf(ddlCoApp3Ownship.Items.FindByValue(dt4.Rows[0]["OwnShipStatus"].ToString()));
                            ddlCoApp3ResiStabYrs.SelectedIndex = ddlCoApp3ResiStabYrs.Items.IndexOf(ddlCoApp3ResiStabYrs.Items.FindByValue(dt4.Rows[0]["ResiStabYrs"].ToString()));

                            hdnCoApp3PanVerifyYN.Value = Convert.ToString(dt4.Rows[0]["PanVerifyYN"]);
                            hdnCoApp3AadhVerifyYN.Value = Convert.ToString(dt4.Rows[0]["AadhVerifyYN"]);
                            hdnCoApp3VoterVerifyYN.Value = Convert.ToString(dt4.Rows[0]["VoterVerifyYN"]);

                            StatusButton("Show");

                            if (Convert.ToString(dt4.Rows[0]["PanVerifyYN"]) == "Y")
                            {
                                btnVerifyCoApp3Pan.Enabled = false;
                                txtCoApp3Pan.Enabled = false;
                                hdnCoApp3PanVerifyYN.Value = "Y";
                            }
                            if (Convert.ToString(dt4.Rows[0]["AadhVerifyYN"]) == "Y")
                            {
                                btnVerifyCoApp3Aadhaar.Enabled = false;
                                txtCoApp3AadhNo.Enabled = false;
                                hdnCoApp3AadhVerifyYN.Value = "Y";
                            }

                            if (Convert.ToString(dt4.Rows[0]["VoterVerifyYN"]) == "Y")
                            {
                                btnVerifyCoApp3Voter.Enabled = false;
                                txtCoApp3Voter.Enabled = false;
                                hdnCoApp3VoterVerifyYN.Value = "Y";
                            }
                            hdnCoApp3OwnShipExt.Value = Convert.ToString(dt4.Rows[0]["OwnShipExt"]);
                            lblCoApp3OwnShip.Text = "CA3OwnshipProof" + hdnCoApp3OwnShipExt.Value;
                        }
                        if (dt5.Rows.Count > 0)
                        {
                            ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                            hdnCoApp4CustId.Value = Convert.ToString(dt5.Rows[0]["CoAppID"]);
                            txtCoApp4CustId.Text = Convert.ToString(dt5.Rows[0]["CoAppNo"]);
                            txtCoApp4CustName.Text = Convert.ToString(dt5.Rows[0]["CoAppName"]);
                            ddlCoApp4Gender.SelectedIndex = ddlCoApp4Gender.Items.IndexOf(ddlCoApp4Gender.Items.FindByValue(dt5.Rows[0]["Gender"].ToString()));
                            txtCoApp4Dob.Text = Convert.ToString(dt5.Rows[0]["DoB"]);
                            txtCoApp4Age.Text = Convert.ToString(dt5.Rows[0]["PresentAge"]);
                            txtCoApp4AgeAtLoanMaturity.Text = Convert.ToString(dt5.Rows[0]["AgeAtLoanMaturity"]);
                            txtCoApp4Pan.Text = Convert.ToString(dt5.Rows[0]["PanNo"]);
                            txtCoApp4AadhRefNo.Text = Convert.ToString(dt5.Rows[0]["AadhRefNo"]);
                            txtCoApp4AadhNo.Text = Convert.ToString(dt5.Rows[0]["AadhMaskedNo"]);
                            txtCoApp4Voter.Text = Convert.ToString(dt5.Rows[0]["VoterID"]);
                            ddlCoApp4RelWithApp.SelectedIndex = ddlCoApp4RelWithApp.Items.IndexOf(ddlCoApp4RelWithApp.Items.FindByValue(dt5.Rows[0]["RelWithApp"].ToString()));
                            txtCoApp4Mob.Text = Convert.ToString(dt5.Rows[0]["MobNo"]);
                            ddlCoApp4Edu.SelectedIndex = ddlCoApp4Edu.Items.IndexOf(ddlCoApp4Edu.Items.FindByValue(dt5.Rows[0]["Education"].ToString()));
                            ddlCoApp4Marital.SelectedIndex = ddlCoApp4Marital.Items.IndexOf(ddlCoApp4Marital.Items.FindByValue(dt5.Rows[0]["MaritalStatus"].ToString()));
                            txtCoApp4NoOfFamily.Text = Convert.ToString(dt5.Rows[0]["TotalNoFamMem"]);
                            txtCoApp4NoOfDependents.Text = Convert.ToString(dt5.Rows[0]["NoOfDependents"]);
                            ddlCoApp4Caste.SelectedIndex = ddlCoApp4Caste.Items.IndexOf(ddlCoApp4Caste.Items.FindByValue(dt5.Rows[0]["Caste"].ToString()));
                            ddlCoApp4Religion.SelectedIndex = ddlCoApp4Religion.Items.IndexOf(ddlCoApp4Religion.Items.FindByValue(dt5.Rows[0]["Religion"].ToString()));
                            ddlCoApp4Minority.SelectedIndex = ddlCoApp4Minority.Items.IndexOf(ddlCoApp4Minority.Items.FindByValue(dt5.Rows[0]["Minority"].ToString()));
                            txtCoApp4Email.Text = Convert.ToString(dt5.Rows[0]["Email"]);
                            txtCoApp4PerResiAdd.Text = Convert.ToString(dt5.Rows[0]["PerAdd"]);


                            ddlCoApp4PerState.SelectedIndex = ddlCoApp4PerState.Items.IndexOf(ddlCoApp4PerState.Items.FindByValue(dt5.Rows[0]["PerState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlCoApp4PerState.SelectedValue), "CoApp4Per");
                            ddlCoApp4PerDist.SelectedIndex = ddlCoApp4PerDist.Items.IndexOf(ddlCoApp4PerDist.Items.FindByValue(dt5.Rows[0]["PerDist"].ToString()));

                            txtCoApp4PerPin.Text = Convert.ToString(dt5.Rows[0]["PerPin"]);
                            txtCoApp4CurrResiAdd.Text = Convert.ToString(dt5.Rows[0]["CurrAdd"]);

                            ddlCoApp4CurrState.SelectedIndex = ddlCoApp4CurrState.Items.IndexOf(ddlCoApp4CurrState.Items.FindByValue(dt5.Rows[0]["CurrState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlCoApp4CurrState.SelectedValue), "CoApp4Curr");
                            ddlCoApp4CurrDist.SelectedIndex = ddlCoApp4CurrDist.Items.IndexOf(ddlCoApp4CurrDist.Items.FindByValue(dt5.Rows[0]["CurrDist"].ToString()));

                            txtCoApp4CurrPin.Text = Convert.ToString(dt5.Rows[0]["CurrPin"]);
                            txtCoApp4Landmark.Text = Convert.ToString(dt5.Rows[0]["CurrLandmark"]);
                            ddlCoApp4Ownship.SelectedIndex = ddlCoApp4Ownship.Items.IndexOf(ddlCoApp4Ownship.Items.FindByValue(dt5.Rows[0]["OwnShipStatus"].ToString()));
                            ddlCoApp4ResiStabYrs.SelectedIndex = ddlCoApp4ResiStabYrs.Items.IndexOf(ddlCoApp4ResiStabYrs.Items.FindByValue(dt5.Rows[0]["ResiStabYrs"].ToString()));

                            hdnCoApp4PanVerifyYN.Value = Convert.ToString(dt5.Rows[0]["PanVerifyYN"]);
                            hdnCoApp4AadhVerifyYN.Value = Convert.ToString(dt5.Rows[0]["AadhVerifyYN"]);
                            hdnCoApp4VoterVerifyYN.Value = Convert.ToString(dt5.Rows[0]["VoterVerifyYN"]);

                            StatusButton("Show");

                            if (Convert.ToString(dt5.Rows[0]["PanVerifyYN"]) == "Y")
                            {
                                btnVerifyCoApp4Pan.Enabled = false;
                                txtCoApp4Pan.Enabled = false;
                                hdnCoApp4PanVerifyYN.Value = "Y";
                            }
                            if (Convert.ToString(dt5.Rows[0]["AadhVerifyYN"]) == "Y")
                            {
                                btnVerifyCoApp4Aadhaar.Enabled = false;
                                txtCoApp4AadhNo.Enabled = false;
                                hdnCoApp4AadhVerifyYN.Value = "Y";
                            }

                            if (Convert.ToString(dt5.Rows[0]["VoterVerifyYN"]) == "Y")
                            {
                                btnVerifyCoApp4Voter.Enabled = false;
                                txtCoApp4Voter.Enabled = false;
                                hdnCoApp4VoterVerifyYN.Value = "Y";
                            }
                            hdnCoApp4OwnShipExt.Value = Convert.ToString(dt5.Rows[0]["OwnShipExt"]);
                            lblCoApp4OwnShip.Text = "CA4OwnshipProof" + hdnCoApp4OwnShipExt.Value;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                        dt4 = null;
                        dt5 = null;
                    }
                    #endregion

                    break;
                case 2:
                    #region InternalChecks
                    try
                    {
                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPNumIC.Text = ViewState["BcProNo"].ToString();
                            lblBCPNumIC.Text = ViewState["BcProNo"].ToString();
                            lblBCPNumIC.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBCPNumIC.Text = "";
                            lblBCPNumIC.Text = "";
                            lblBCPNumIC.Text = "";
                        }

                        ds = oC360.CF_GetCust360IntChk(hdMemberId.Value);
                        dt = ds.Tables[0];
                        dt1 = ds.Tables[1];
                        dt2 = ds.Tables[2];
                        dt3 = ds.Tables[3];
                        dt4 = ds.Tables[4];
                        dt5 = ds.Tables[5];

                        if (dt.Rows.Count > 0)
                        {
                            hdnLeadId.Value = Convert.ToString(dt.Rows[0]["LeadID"]);
                            hdnAppId.Value = Convert.ToString(dt.Rows[0]["MemberID"]);
                            txtPanNo.Text = Convert.ToString(dt.Rows[0]["PanNo"]);
                            lblPanStatus.Text = Convert.ToString(dt.Rows[0]["PanVerifyPF"]);
                            txtVoterId.Text = Convert.ToString(dt.Rows[0]["VoterNo"]);
                            lblVoterStatus.Text = Convert.ToString(dt.Rows[0]["VoterVerifyPF"]);
                            lblDdupStatus.Text = Convert.ToString(dt.Rows[0]["DdupPF"]);
                            hdnElecBillExt.Value = Convert.ToString(dt.Rows[0]["ElecBillExt"]);
                            hdnFcuReportExt.Value = Convert.ToString(dt.Rows[0]["FCURptExt"]);
                            txtFCUDt.Text = Convert.ToString(dt.Rows[0]["FCUDt"]);
                            txtFCUExDt.Text = Convert.ToString(dt.Rows[0]["FCUExDt"]);

                            ddlCustomerIC.ClearSelection();
                            ListItem oli1 = new ListItem(Convert.ToString(dt.Rows[0]["Member"]), "-1");
                            ddlCustomerIC.Items.Insert(0, oli1);

                            if (Convert.ToString(dt.Rows[0]["ElecBillPF"]) == "Y")
                            {
                                lblElecBill.Text = "ElectricBill" + hdnElecBillExt.Value;
                            }
                            else
                            {
                                lblElecBill.Text = "";
                            }

                            if (Convert.ToString(dt.Rows[0]["FCURptUplYN"]) == "Y")
                            {
                                lblFcuRpt.Text = "FCUReport" + hdnFcuReportExt.Value;
                            }
                            else
                            {
                                lblFcuRpt.Text = "";
                            }

                            ddlAmlStatus.SelectedIndex = ddlAmlStatus.Items.IndexOf(ddlAmlStatus.Items.FindByText(dt.Rows[0]["AMLCheckPF"].ToString()));
                            if (Convert.ToString(dt.Rows[0]["AMLCheckPF"]) == "Pass")
                            {
                                hdnAmlPassYn.Value = "Y";
                            }
                            else
                            {
                                hdnAmlPassYn.Value = "N";
                            }
                            ddlVKycStatus.SelectedIndex = ddlVKycStatus.Items.IndexOf(ddlVKycStatus.Items.FindByText(dt.Rows[0]["VKYCVerifyYN"].ToString()));
                            if (Convert.ToString(dt.Rows[0]["VKYCVerifyYN"]) == "Pass")
                            {
                                hdnValVKYCYN.Value = "Y";
                            }
                            else
                            {
                                hdnValVKYCYN.Value = "N";
                            }

                            ddlElecBillStat.SelectedIndex = ddlElecBillStat.Items.IndexOf(ddlElecBillStat.Items.FindByText(dt.Rows[0]["ElecBillStatus"].ToString()));

                            ddlFCUStatus.SelectedIndex = ddlFCUStatus.Items.IndexOf(ddlFCUStatus.Items.FindByValue(dt.Rows[0]["FCUStatus"].ToString()));


                            ImgElecBill.Enabled = true;
                            imgFcuReport.Enabled = true;

                            // FOR CO-APPLICANT 1
                            #region CO-APPLICANT 1
                            if (dt1.Rows.Count > 0)
                            {

                                txtPanNoCA1.Text = Convert.ToString(dt1.Rows[0]["PanNo"]);
                                lblPanStatusCA1.Text = Convert.ToString(dt1.Rows[0]["PanVerifyPF"]);
                                txtVoterIdCA1.Text = Convert.ToString(dt1.Rows[0]["VoterNo"]);
                                lblVoterStatusCA1.Text = Convert.ToString(dt1.Rows[0]["VoterVerifyPF"]);
                                lblDdupStatusCA1.Text = Convert.ToString(dt1.Rows[0]["DdupPF"]);
                                hdnElecBillExtCA1.Value = Convert.ToString(dt1.Rows[0]["ElecBillExt"]);
                                hdnFcuReportExtCA1.Value = Convert.ToString(dt1.Rows[0]["FCURptExt"]);
                                lblCoApp1Name.Text = Convert.ToString(dt1.Rows[0]["CoAppName"]);
                                txtFCUDtCA1.Text = Convert.ToString(dt1.Rows[0]["FCUDt"]);
                                txtFCUExDtCA1.Text = Convert.ToString(dt1.Rows[0]["FCUExDt"]);

                                lblCustNameCA1.Text = Convert.ToString(dt1.Rows[0]["Member"]);

                                if (Convert.ToString(dt1.Rows[0]["ElecBillPF"]) == "Y")
                                {
                                    lblElecBillCA1.Text = "ElectricBill" + hdnElecBillExtCA1.Value;
                                }
                                else
                                {
                                    lblElecBillCA1.Text = "";
                                }

                                if (Convert.ToString(dt1.Rows[0]["FCURptUplYN"]) == "Y")
                                {
                                    lblFcuRptCA1.Text = "FCUReport" + hdnFcuReportExtCA1.Value;
                                }
                                else
                                {
                                    lblFcuRptCA1.Text = "";
                                }

                                ddlAmlStatusCA1.SelectedIndex = ddlAmlStatusCA1.Items.IndexOf(ddlAmlStatusCA1.Items.FindByText(dt1.Rows[0]["AMLCheckPF"].ToString()));
                                if (Convert.ToString(dt1.Rows[0]["AMLCheckPF"]) == "Pass")
                                {
                                    hdnAmlPassYnCA1.Value = "Y";
                                }
                                else
                                {
                                    hdnAmlPassYnCA1.Value = "N";
                                }
                                ddlVKycStatusCA1.SelectedIndex = ddlVKycStatusCA1.Items.IndexOf(ddlVKycStatusCA1.Items.FindByText(dt1.Rows[0]["VKYCVerifyYN"].ToString()));
                                if (Convert.ToString(dt1.Rows[0]["VKYCVerifyYN"]) == "Pass")
                                {
                                    hdnValVKYCYNCA1.Value = "Y";
                                }
                                else
                                {
                                    hdnValVKYCYNCA1.Value = "N";
                                }

                                ddlElecBillStatCA1.SelectedIndex = ddlElecBillStatCA1.Items.IndexOf(ddlElecBillStatCA1.Items.FindByText(dt1.Rows[0]["ElecBillStatus"].ToString()));

                                ddlFCUStatusCA1.SelectedIndex = ddlFCUStatusCA1.Items.IndexOf(ddlFCUStatusCA1.Items.FindByValue(dt1.Rows[0]["FCUStatus"].ToString()));


                                ImgElecBillCA1.Enabled = true;
                                imgFcuReportCA1.Enabled = true;

                                lblCoApp1Status.Text = "";
                                hdnCoApp1Status.Value = "Y";

                            }
                            else
                            {
                                lblCoApp1Status.Text = "Please validate KYC from Customer 360 Form";
                                lblCoApp1Status.ForeColor = System.Drawing.Color.Red;
                                hdnCoApp1Status.Value = "N";

                            }

                            #endregion

                            // FOR CO-APPLICANT 2
                            #region CO-APPLICANT 2
                            if (dt2.Rows.Count > 0)
                            {

                                txtPanNoCA2.Text = Convert.ToString(dt2.Rows[0]["PanNo"]);
                                lblPanStatusCA2.Text = Convert.ToString(dt2.Rows[0]["PanVerifyPF"]);
                                txtVoterIdCA2.Text = Convert.ToString(dt2.Rows[0]["VoterNo"]);
                                lblVoterStatusCA2.Text = Convert.ToString(dt2.Rows[0]["VoterVerifyPF"]);
                                lblDdupStatusCA2.Text = Convert.ToString(dt2.Rows[0]["DdupPF"]);
                                hdnElecBillExtCA2.Value = Convert.ToString(dt2.Rows[0]["ElecBillExt"]);
                                hdnFcuReportExtCA2.Value = Convert.ToString(dt2.Rows[0]["FCURptExt"]);
                                lblCoApp2Name.Text = Convert.ToString(dt2.Rows[0]["CoAppName"]);
                                txtFCUDtCA2.Text = Convert.ToString(dt2.Rows[0]["FCUDt"]);
                                txtFCUExDtCA2.Text = Convert.ToString(dt2.Rows[0]["FCUExDt"]);


                                lblCustNameCA2.Text = Convert.ToString(dt2.Rows[0]["Member"]);

                                if (Convert.ToString(dt2.Rows[0]["ElecBillPF"]) == "Y")
                                {
                                    lblElecBillCA2.Text = "ElectricBill" + hdnElecBillExtCA2.Value;
                                }
                                else
                                {
                                    lblElecBillCA2.Text = "";
                                }

                                if (Convert.ToString(dt2.Rows[0]["FCURptUplYN"]) == "Y")
                                {
                                    lblFcuRptCA2.Text = "FCUReport" + hdnFcuReportExtCA2.Value;
                                }
                                else
                                {
                                    lblFcuRptCA2.Text = "";
                                }

                                ddlAmlStatusCA2.SelectedIndex = ddlAmlStatusCA2.Items.IndexOf(ddlAmlStatusCA2.Items.FindByText(dt2.Rows[0]["AMLCheckPF"].ToString()));
                                if (Convert.ToString(dt2.Rows[0]["AMLCheckPF"]) == "Pass")
                                {
                                    hdnAmlPassYnCA2.Value = "Y";
                                }
                                else
                                {
                                    hdnAmlPassYnCA2.Value = "N";
                                }
                                ddlVKycStatusCA2.SelectedIndex = ddlVKycStatusCA2.Items.IndexOf(ddlVKycStatusCA2.Items.FindByText(dt2.Rows[0]["VKYCVerifyYN"].ToString()));
                                if (Convert.ToString(dt2.Rows[0]["VKYCVerifyYN"]) == "Pass")
                                {
                                    hdnValVKYCYNCA2.Value = "Y";
                                }
                                else
                                {
                                    hdnValVKYCYNCA2.Value = "N";
                                }

                                ddlElecBillStatCA2.SelectedIndex = ddlElecBillStatCA2.Items.IndexOf(ddlElecBillStatCA2.Items.FindByText(dt2.Rows[0]["ElecBillStatus"].ToString()));

                                ddlFCUStatusCA2.SelectedIndex = ddlFCUStatusCA2.Items.IndexOf(ddlFCUStatusCA2.Items.FindByValue(dt2.Rows[0]["FCUStatus"].ToString()));


                                ImgElecBillCA2.Enabled = true;
                                imgFcuReportCA2.Enabled = true;

                                lblCoApp2Status.Text = "";
                                hdnCoApp2Status.Value = "Y";

                            }
                            else
                            {
                                lblCoApp2Status.Text = "Please validate KYC from Customer 360 Form";
                                lblCoApp2Status.ForeColor = System.Drawing.Color.Red;
                                hdnCoApp2Status.Value = "N";

                            }
                            #endregion

                            // FOR Guarantor
                            #region Guarantor
                            if (dt3.Rows.Count > 0)
                            {

                                txtPanNoG.Text = Convert.ToString(dt3.Rows[0]["PanNo"]);
                                lblPanStatusG.Text = Convert.ToString(dt3.Rows[0]["PanVerifyPF"]);
                                txtVoterIdG.Text = Convert.ToString(dt3.Rows[0]["VoterNo"]);
                                lblVoterStatusG.Text = Convert.ToString(dt3.Rows[0]["VoterVerifyPF"]);
                                lblDdupStatusG.Text = Convert.ToString(dt3.Rows[0]["DdupPF"]);
                                hdnElecBillExtG.Value = Convert.ToString(dt3.Rows[0]["ElecBillExt"]);
                                hdnFcuReportExtG.Value = Convert.ToString(dt3.Rows[0]["FCURptExt"]);
                                lblGuarantorName.Text = Convert.ToString(dt3.Rows[0]["CoAppName"]);
                                txtFCUDtG.Text = Convert.ToString(dt3.Rows[0]["FCUDt"]);
                                txtFCUExDtG.Text = Convert.ToString(dt3.Rows[0]["FCUExDt"]);

                                lblCustNameG.Text = Convert.ToString(dt3.Rows[0]["Member"]);

                                if (Convert.ToString(dt3.Rows[0]["ElecBillPF"]) == "Y")
                                {
                                    lblElecBillG.Text = "ElectricBill" + hdnElecBillExtCA2.Value;
                                }
                                else
                                {
                                    lblElecBillG.Text = "";
                                }

                                if (Convert.ToString(dt3.Rows[0]["FCURptUplYN"]) == "Y")
                                {
                                    lblFcuRptG.Text = "FCUReport" + hdnFcuReportExtG.Value;
                                }
                                else
                                {
                                    lblFcuRptG.Text = "";
                                }

                                ddlAmlStatusG.SelectedIndex = ddlAmlStatusG.Items.IndexOf(ddlAmlStatusG.Items.FindByText(dt3.Rows[0]["AMLCheckPF"].ToString()));
                                if (Convert.ToString(dt3.Rows[0]["AMLCheckPF"]) == "Pass")
                                {
                                    hdnAmlPassYnG.Value = "Y";
                                }
                                else
                                {
                                    hdnAmlPassYnG.Value = "N";
                                }
                                ddlVKycStatusG.SelectedIndex = ddlVKycStatusG.Items.IndexOf(ddlVKycStatusG.Items.FindByText(dt3.Rows[0]["VKYCVerifyYN"].ToString()));
                                if (Convert.ToString(dt3.Rows[0]["VKYCVerifyYN"]) == "Pass")
                                {
                                    hdnValVKYCYNG.Value = "Y";
                                }
                                else
                                {
                                    hdnValVKYCYNG.Value = "N";
                                }

                                ddlElecBillStatG.SelectedIndex = ddlElecBillStatG.Items.IndexOf(ddlElecBillStatG.Items.FindByText(dt3.Rows[0]["ElecBillStatus"].ToString()));

                                ddlFCUStatusG.SelectedIndex = ddlFCUStatusG.Items.IndexOf(ddlFCUStatusG.Items.FindByValue(dt3.Rows[0]["FCUStatus"].ToString()));

                                ImgElecBillG.Enabled = true;
                                imgFcuReportG.Enabled = true;
                                hdnGuarantorStatus.Value = "Y";
                                lblGuarantorStatus.Text = "";


                            }
                            else
                            {
                                lblGuarantorStatus.Text = "Please validate KYC from Customer 360 Form";
                                lblGuarantorStatus.ForeColor = System.Drawing.Color.Red;
                                hdnGuarantorStatus.Value = "N";

                            }
                            #endregion

                            // FOR CO-APPLICANT 3
                            #region CO-APPLICANT 3
                            if (dt4.Rows.Count > 0)
                            {

                                txtPanNoCA3.Text = Convert.ToString(dt4.Rows[0]["PanNo"]);
                                lblPanStatusCA3.Text = Convert.ToString(dt4.Rows[0]["PanVerifyPF"]);
                                txtVoterIdCA3.Text = Convert.ToString(dt4.Rows[0]["VoterNo"]);
                                lblVoterStatusCA3.Text = Convert.ToString(dt4.Rows[0]["VoterVerifyPF"]);
                                lblDdupStatusCA3.Text = Convert.ToString(dt4.Rows[0]["DdupPF"]);
                                hdnElecBillExtCA3.Value = Convert.ToString(dt4.Rows[0]["ElecBillExt"]);
                                hdnFcuReportExtCA3.Value = Convert.ToString(dt4.Rows[0]["FCURptExt"]);
                                lblCoApp3Name.Text = Convert.ToString(dt4.Rows[0]["CoAppName"]);
                                txtFCUDtCA3.Text = Convert.ToString(dt4.Rows[0]["FCUDt"]);
                                txtFCUExDtCA3.Text = Convert.ToString(dt4.Rows[0]["FCUExDt"]);


                                lblCustNameCA3.Text = Convert.ToString(dt4.Rows[0]["Member"]);

                                if (Convert.ToString(dt4.Rows[0]["ElecBillPF"]) == "Y")
                                {
                                    lblElecBillCA3.Text = "ElectricBill" + hdnElecBillExtCA3.Value;
                                }
                                else
                                {
                                    lblElecBillCA3.Text = "";
                                }

                                if (Convert.ToString(dt4.Rows[0]["FCURptUplYN"]) == "Y")
                                {
                                    lblFcuRptCA3.Text = "FCUReport" + hdnFcuReportExtCA3.Value;
                                }
                                else
                                {
                                    lblFcuRptCA3.Text = "";
                                }

                                ddlAmlStatusCA3.SelectedIndex = ddlAmlStatusCA3.Items.IndexOf(ddlAmlStatusCA3.Items.FindByText(dt4.Rows[0]["AMLCheckPF"].ToString()));
                                if (Convert.ToString(dt4.Rows[0]["AMLCheckPF"]) == "Pass")
                                {
                                    hdnAmlPassYnCA3.Value = "Y";
                                    hdCoApp3AMLFlag.Value = "Y";
                                }
                                else if (Convert.ToString(dt4.Rows[0]["AMLCheckPF"]) == "Fail")
                                {
                                    hdnAmlPassYnCA3.Value = "Y";
                                    hdCoApp3AMLFlag.Value = "Y";
                                }
                                ddlVKycStatusCA3.SelectedIndex = ddlVKycStatusCA3.Items.IndexOf(ddlVKycStatusCA3.Items.FindByText(dt4.Rows[0]["VKYCVerifyYN"].ToString()));
                                if (Convert.ToString(dt4.Rows[0]["VKYCVerifyYN"]) == "Pass")
                                {
                                    hdnValVKYCYNCA3.Value = "Y";
                                }
                                else
                                {
                                    hdnValVKYCYNCA3.Value = "N";
                                }

                                ddlElecBillStatCA3.SelectedIndex = ddlElecBillStatCA3.Items.IndexOf(ddlElecBillStatCA3.Items.FindByText(dt4.Rows[0]["ElecBillStatus"].ToString()));

                                ddlFCUStatusCA3.SelectedIndex = ddlFCUStatusCA3.Items.IndexOf(ddlFCUStatusCA3.Items.FindByValue(dt4.Rows[0]["FCUStatus"].ToString()));

                                if (Convert.ToString(dt4.Rows[0]["VkycFlag"]) == "Y")
                                {
                                    hdCoApp3VkycYN.Value = "Y";
                                }
                                else
                                {
                                    hdCoApp3VkycYN.Value = "N";
                                }
                             
                                ImgElecBillCA3.Enabled = true;
                                imgFcuReportCA3.Enabled = true;

                                lblCoApp3Status.Text = "";
                                hdnCoApp3Status.Value = "Y";
                                btnValidateVkycCA3.Enabled = false;
                            }
                            else
                            {
                                lblCoApp3Status.Text = "Please validate KYC from Customer 360 Form";
                                lblCoApp3Status.ForeColor = System.Drawing.Color.Red;
                                hdnCoApp3Status.Value = "N";

                            }
                            #endregion

                            // FOR CO-APPLICANT 4
                            #region CO-APPLICANT 4
                            if (dt5.Rows.Count > 0)
                            {

                                txtPanNoCA4.Text = Convert.ToString(dt5.Rows[0]["PanNo"]);
                                lblPanStatusCA4.Text = Convert.ToString(dt5.Rows[0]["PanVerifyPF"]);
                                txtVoterIdCA4.Text = Convert.ToString(dt5.Rows[0]["VoterNo"]);
                                lblVoterStatusCA4.Text = Convert.ToString(dt5.Rows[0]["VoterVerifyPF"]);
                                lblDdupStatusCA4.Text = Convert.ToString(dt5.Rows[0]["DdupPF"]);
                                hdnElecBillExtCA4.Value = Convert.ToString(dt5.Rows[0]["ElecBillExt"]);
                                hdnFcuReportExtCA4.Value = Convert.ToString(dt5.Rows[0]["FCURptExt"]);
                                lblCoApp4Name.Text = Convert.ToString(dt5.Rows[0]["CoAppName"]);
                                txtFCUDtCA4.Text = Convert.ToString(dt5.Rows[0]["FCUDt"]);
                                txtFCUExDtCA4.Text = Convert.ToString(dt5.Rows[0]["FCUExDt"]);


                                lblCustNameCA4.Text = Convert.ToString(dt5.Rows[0]["Member"]);

                                if (Convert.ToString(dt5.Rows[0]["ElecBillPF"]) == "Y")
                                {
                                    lblElecBillCA4.Text = "ElectricBill" + hdnElecBillExtCA4.Value;
                                }
                                else
                                {
                                    lblElecBillCA4.Text = "";
                                }

                                if (Convert.ToString(dt5.Rows[0]["FCURptUplYN"]) == "Y")
                                {
                                    lblFcuRptCA4.Text = "FCUReport" + hdnFcuReportExtCA4.Value;
                                }
                                else
                                {
                                    lblFcuRptCA4.Text = "";
                                }

                                ddlAmlStatusCA4.SelectedIndex = ddlAmlStatusCA4.Items.IndexOf(ddlAmlStatusCA4.Items.FindByText(dt5.Rows[0]["AMLCheckPF"].ToString()));
                                if (Convert.ToString(dt5.Rows[0]["AMLCheckPF"]) == "Pass")
                                {
                                    hdnAmlPassYnCA4.Value = "Y";
                                    hdCoApp4AMLFlag.Value = "Y";
                                }
                                else if (Convert.ToString(dt5.Rows[0]["AMLCheckPF"]) == "Fail")
                                {
                                    hdnAmlPassYnCA4.Value = "Y";
                                    hdCoApp4AMLFlag.Value = "Y";
                                }
                                ddlVKycStatusCA4.SelectedIndex = ddlVKycStatusCA4.Items.IndexOf(ddlVKycStatusCA4.Items.FindByText(dt5.Rows[0]["VKYCVerifyYN"].ToString()));
                                if (Convert.ToString(dt5.Rows[0]["VKYCVerifyYN"]) == "Pass")
                                {
                                    hdnValVKYCYNCA4.Value = "Y";
                                }
                                else
                                {
                                    hdnValVKYCYNCA4.Value = "N";
                                }

                                ddlElecBillStatCA4.SelectedIndex = ddlElecBillStatCA4.Items.IndexOf(ddlElecBillStatCA4.Items.FindByText(dt5.Rows[0]["ElecBillStatus"].ToString()));

                                ddlFCUStatusCA4.SelectedIndex = ddlFCUStatusCA4.Items.IndexOf(ddlFCUStatusCA4.Items.FindByValue(dt5.Rows[0]["FCUStatus"].ToString()));

                                if (Convert.ToString(dt5.Rows[0]["VkycFlag"]) == "Y")
                                {
                                    hdCoApp4VkycYN.Value = "Y";
                                }
                                else
                                {
                                    hdCoApp4VkycYN.Value = "N";
                                }

                                ImgElecBillCA4.Enabled = true;
                                imgFcuReportCA4.Enabled = true;

                                lblCoApp4Status.Text = "";
                                hdnCoApp4Status.Value = "Y";
                                btnValidateVkycCA4.Enabled = false;

                            }
                            else
                            {
                                lblCoApp4Status.Text = "Please validate KYC from Customer 360 Form";
                                lblCoApp4Status.ForeColor = System.Drawing.Color.Red;
                                hdnCoApp4Status.Value = "N";

                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion

                    #region ExternalChk
                    try
                    {
                        if (ViewState["AppNm"] != null)
                        {
                            lblApplNmEC.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblApplNmEC.Text = "";
                        }

                        if (ViewState["BcProNo"] != null)
                        {
                            txtLoanApplicationNo.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            txtLoanApplicationNo.Text = "";
                        }
                        dt = oDist.CF_GetExtrnlChkDtlLeadID(Convert.ToInt64(hdLeadId.Value));
                        if (dt.Rows.Count > 0)
                        {
                            txtLoanApplicationNo.Text = Convert.ToString(dt.Rows[0]["BCPropNo"]);
                            ddlTrdRfChk.SelectedIndex = ddlTrdRfChk.Items.IndexOf(ddlTrdRfChk.Items.FindByValue(dt.Rows[0]["TrdChkId"].ToString()));
                            ShowEmpRecords();
                        }
                        else
                        {
                            ShowEmpRecords();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion

                    #region VendorRps
                    try
                    {
                        if (ViewState["AppNm"] != null)
                        {
                            lblApplNmVR.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblApplNmVR.Text = "";
                        }
                        if (ViewState["BcProNo"] != null)
                        {
                            txtLoanApplicationNoV.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            txtLoanApplicationNoV.Text = "";
                        }
                        dt = oDist.CF_GetOtherRepDtlByLeadID(Convert.ToInt64(hdLeadId.Value));
                        if (dt.Rows.Count > 0)
                        {
                            txtLoanApplicationNoV.Text = Convert.ToString(dt.Rows[0]["BCPropNo"]);
                            txtLoanApplicationNo.Enabled = false;
                            ShowEmpRecordsV();
                        }
                        else
                        {
                            ShowEmpRecordsV();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion

                    #region OtherRpts
                    try
                    {
                        if (ViewState["AppNm"] != null)
                        {
                            lblApplNmOR.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblApplNmOR.Text = "";
                        }
                        if (ViewState["BcProNo"] != null)
                        {
                            txtLoanApplicationNoOth.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            txtLoanApplicationNoOth.Text = "";
                        }
                        dt = oDist.CF_GetOtherRepDtlByLeadID(Convert.ToInt64(hdLeadId.Value));
                        if (dt.Rows.Count > 0)
                        {

                            txtLoanApplicationNoOth.Text = Convert.ToString(dt.Rows[0]["BCPropNo"]);
                            txtLoanApplicationNoOth.Enabled = false;
                            hdnLoanAppNo.Value = Convert.ToString(dt.Rows[0]["LeadID"]);
                            ShowEmpRecordsOth();
                        }
                        else
                        {
                            ShowEmpRecordsOth();

                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion

                    break;
                case 3:
                    #region Emp/Busdtls
                    try
                    {
                        if (Convert.ToInt64(hdAssMtdId.Value) == 1) // 1 for Salaried which will be used for Employment and the others is for Business
                        {
                            if (ViewState["BcProNo"] != null)
                            {

                                lblBasicName.Text = ViewState["BcProNo"].ToString();
                            }
                            else
                            {
                                lblBasicName.Text = "";
                            }

                            if (ViewState["AppNm"] != null)
                            {
                                lblApplNmED.Text = ViewState["AppNm"].ToString();
                            }
                            else
                            {
                                lblApplNmED.Text = "";
                            }
                        }
                        else
                        {
                            if (ViewState["BcProNo"] != null)
                            {
                                lblBasicNamebus.Text = ViewState["BcProNo"].ToString();
                            }
                            else
                            {
                                lblBasicNamebus.Text = "";
                            }
                            if (ViewState["AppNm"] != null)
                            {
                                lblApplNmBN.Text = ViewState["AppNm"].ToString();

                            }
                            else
                            {
                                lblApplNmBN.Text = "";
                            }
                        }

                        if (hdStatus.Value != "Pending")
                        {
                            ViewState["StateEdit"] = "Edit";

                            if (Convert.ToInt64(hdAssMtdId.Value) == 1) // 1 for Salaried which will be used for Employment and the others is for Business
                            {
                                pnlEmp.Visible = true;
                                pnlBus.Visible = false;
                                dt = oDist.CF_GetEmployDtlByLeadID(Convert.ToInt64(hdLeadId.Value), vBrCode);

                                if (dt.Rows.Count > 0)
                                {
                                    //  lblBasicName.Text = btnShow.Text;
                                    txtEmplr.Text = Convert.ToString(dt.Rows[0]["EmplrName"]);
                                    txtDesig.Text = Convert.ToString(dt.Rows[0]["Designation"]);
                                    txtJoinDt.Text = Convert.ToString(dt.Rows[0]["JoinDate"]);
                                    txtJobStbly.Text = Convert.ToString(dt.Rows[0]["CrJobStblty"]);
                                    txtTotExp.Text = Convert.ToString(dt.Rows[0]["TotExp"]);
                                    txtOfcAddrs.Text = Convert.ToString(dt.Rows[0]["OffcAddrs"]);
                                    txtDocDesc.Text = Convert.ToString(dt.Rows[0]["DocDescr"]);
                                    lblFileName.Text = Convert.ToString(dt.Rows[0]["EmpFileName"]);
                                    vIsFileUpload = Convert.ToString(dt.Rows[0]["IsFileUpload"]);
                                    if (vIsFileUpload == "Y")
                                    {
                                        btnEmpDoc.Enabled = true;
                                    }
                                    else
                                    {
                                        btnEmpDoc.Enabled = false;
                                    }
                                    //tbLoanAppDtl.ActiveTabIndex = 14;
                                }
                                else
                                {
                                    lblBasicName.Text = "BC Proposal No:- " + btnShow.Text;
                                    txtEmplr.Text = "";
                                    txtDesig.Text = "";
                                    txtJoinDt.Text = Session[gblValue.LoginDate].ToString();
                                    txtJobStbly.Text = "0";
                                    txtTotExp.Text = "0";
                                    txtOfcAddrs.Text = "";
                                    txtDocDesc.Text = "";
                                    lblFileName.Text = "";
                                    vIsFileUpload = "N";
                                    if (vIsFileUpload == "Y")
                                    {
                                        btnEmpDoc.Enabled = true;
                                    }
                                    else
                                    {
                                        btnEmpDoc.Enabled = false;
                                    }
                                    //tbLoanAppDtl.ActiveTabIndex = 14;
                                }
                            }
                            else
                            {
                                pnlEmp.Visible = false;
                                pnlBus.Visible = true;
                                dt = oDist.CF_GetBusinessDtlByLeadID(Convert.ToInt64(hdLeadId.Value), vBrCode);

                                if (dt.Rows.Count > 0)
                                {
                                    // lblBasicNamebus.Text = btnShow.Text;
                                    //tbLoanAppDtl.ActiveTabIndex = 13;
                                    txtBusiName.Text = Convert.ToString(dt.Rows[0]["BusiName"]);
                                    txtIndustry.Text = Convert.ToString(dt.Rows[0]["Industry"]);
                                    ddlBusiType.SelectedIndex = ddlBusiType.Items.IndexOf(ddlBusiType.Items.FindByValue(dt.Rows[0]["BusiType"].ToString()));
                                    txtIncDt.Text = Convert.ToString(dt.Rows[0]["IncorpDate"]);
                                    txtBusiVintYrs.Text = Convert.ToString(dt.Rows[0]["BusiVintYrs"]);
                                    ddlBusiStabYrs.SelectedIndex = ddlBusiStabYrs.Items.IndexOf(ddlBusiStabYrs.Items.FindByValue(dt.Rows[0]["BusiStabYrs"].ToString()));
                                    ddlBusiPremisOwnShip.SelectedIndex = ddlBusiPremisOwnShip.Items.IndexOf(ddlBusiPremisOwnShip.Items.FindByValue(dt.Rows[0]["BusiPremiOwn"].ToString()));

                                    txtUdyamRegnNo.Text = Convert.ToString(dt.Rows[0]["UdyamRegnNo"]);
                                    hdnUdyamRegnCertExt.Value = Convert.ToString(dt.Rows[0]["UdyamRegnFileName"]);
                                    txtBKYC2Name.Text = Convert.ToString(dt.Rows[0]["BKYC2Name"]);
                                    txtBKYC2No.Text = Convert.ToString(dt.Rows[0]["BKYC2No"]);
                                    hdnBKYC2Ext.Value = Convert.ToString(dt.Rows[0]["BKYC2FileName"]);
                                    txtBusiAddr.Text = Convert.ToString(dt.Rows[0]["BusiAddr"]);
                                    hdnBusiAddrOwnshipExt.Value = Convert.ToString(dt.Rows[0]["BusiAddrOwnFileName"]);

                                    if (dt.Rows[0]["UdyamRegnCertUpYN"].ToString() == "Y")
                                    {
                                        imgUdyamRegnCert.Enabled = true;
                                        lblUdyamRegnCertUpYN.Text = hdnUdyamRegnCertExt.Value;
                                    }
                                    else
                                    {
                                        imgUdyamRegnCert.Enabled = false;
                                        lblUdyamRegnCertUpYN.Text = "";
                                    }

                                    if (dt.Rows[0]["BKYC2UpYN"].ToString() == "Y")
                                    {
                                        imgBKYC2.Enabled = true;
                                        lblBKYC2UpYN.Text = hdnBKYC2Ext.Value;
                                    }
                                    else
                                    {
                                        imgBKYC2.Enabled = false;
                                        lblBKYC2UpYN.Text = "";
                                    }

                                    if (dt.Rows[0]["BusiAddrOwnUpYN"].ToString() == "Y")
                                    {
                                        imgBusiAddrOwnship.Enabled = true;
                                        lblBusiAddrOwnUpYN.Text = hdnBusiAddrOwnshipExt.Value;
                                    }
                                    else
                                    {
                                        imgBusiAddrOwnship.Enabled = false;
                                        lblBusiAddrOwnUpYN.Text = "";
                                    }
                                }
                            }

                        }
                        else
                        {
                            if (Convert.ToInt32(hdAssMtdId.Value) == 1)
                            {
                                pnlEmp.Visible = true;
                                pnlBus.Visible = false;
                                lblBasicName.Text = btnShow.Text;
                                //tbLoanAppDtl.ActiveTabIndex = 14;
                            }
                            else
                            {
                                pnlEmp.Visible = false;
                                pnlBus.Visible = true;
                                lblBasicNamebus.Text = btnShow.Text;
                                //tbLoanAppDtl.ActiveTabIndex = 13;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion

                    #region IncomeDetails
                    try
                    {
                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPNo.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBCPNo.Text = "";
                        }

                        if (ViewState["AppNm"] != null)
                        {
                            lblApplicantNm.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblApplicantNm.Text = "";
                        }

                        if (hdIncomeStatus.Value != "Pending")
                        {
                            ddlAssMethod.SelectedIndex = ddlAssMethod.Items.IndexOf(ddlAssMethod.Items.FindByValue(Convert.ToString(hdAssMtdTypId.Value)));

                            ViewState["StateEdit"] = "Edit";

                            ds = oDit.CF_GetIncomeDtlByLeadID(Convert.ToInt64(hdLeadId.Value));
                            dt = ds.Tables[0];

                            if (dt.Rows.Count > 0)
                            {
                                txtProfAfterTax.Text = Convert.ToString(dt.Rows[0]["ProfAfterTax"]);
                                txtDepreciation.Text = Convert.ToString(dt.Rows[0]["Depreciation"]);
                                txtAmortization.Text = Convert.ToString(dt.Rows[0]["Amortization"]);
                                txtInterest.Text = Convert.ToString(dt.Rows[0]["Interest"]);
                                txtTaxes.Text = Convert.ToString(dt.Rows[0]["Taxes"]);
                                txtTotalIncAnnual.Text = Convert.ToString(dt.Rows[0]["TotalIncAnnual"]);
                                txtTurnover.Text = Convert.ToString(dt.Rows[0]["Turnover"]);
                                txtGrossProfitMargin.Text = Convert.ToString(dt.Rows[0]["GrossProfitMargin"]);
                                txtGrossIncAnnual.Text = Convert.ToString(dt.Rows[0]["GrossIncAnnual"]);
                                txtNoMonthsConsiBS.Text = Convert.ToString(dt.Rows[0]["NoMonthsConsiBS"]);
                                txtBankTurnMonthBS.Text = Convert.ToString(dt.Rows[0]["BankTurnMonthBS"]);
                                txtProfMargMonthBS.Text = Convert.ToString(dt.Rows[0]["ProfMargMonthBS"]);
                                txtGrossIncMonthly.Text = Convert.ToString(dt.Rows[0]["GrossIncMonthly"]);
                                txtOtherInc.Text = Convert.ToString(dt.Rows[0]["OtherInc"]);
                                txtAverElecBill.Text = Convert.ToString(dt.Rows[0]["AverElecBill"]);
                                txtTotalIncMonthly.Text = Convert.ToString(dt.Rows[0]["TotalIncMonthly"]);
                                txtApplFoirPerc.Text = Convert.ToString(dt.Rows[0]["ApplFoirPerc"]);
                                txtDeviFoirPerc.Text = Convert.ToString(dt.Rows[0]["DeviFoirPerc"]);
                                txtFinalFoirConsi.Text = Convert.ToString(dt.Rows[0]["FinalFoirConsi"]);
                                txtFoirInc.Text = Convert.ToString(dt.Rows[0]["FoirInc"]);
                                txtExistOblig.Text = Convert.ToString(dt.Rows[0]["ExistOblig"]);
                                txtNetIncToUfsbEmi.Text = Convert.ToString(dt.Rows[0]["NetIncToUfsbEmi"]);
                                txtPerLakhEmi.Text = Convert.ToString(dt.Rows[0]["PerLakhEmi"]);
                                txtLoanEligiblity.Text = Convert.ToString(dt.Rows[0]["LoanEligiblity"]);
                            }


                            // tbLoanAppDtl.ActiveTabIndex = 15;

                            txtGrossProfitMargin.Text = hdnAssMthId.Value == "3" ? "80" : "0";

                        }
                        else
                        {
                            ddlAssMethod.SelectedIndex = ddlAssMethod.Items.IndexOf(ddlAssMethod.Items.FindByValue(Convert.ToString(hdAssMtdTypId.Value)));

                            txtGrossProfitMargin.Text = hdnAssMthId.Value == "3" ? "80" : "0";

                            ViewState["StateEdit"] = "Add";

                            ds = oDit.CF_CalculateEMIAmount(Convert.ToInt64(hdLeadId.Value));
                            dt = ds.Tables[0];

                            if (dt.Rows.Count > 0)
                            {
                                txtPerLakhEmi.Text = Convert.ToString(dt.Rows[0]["EMIAmt"]);
                            }

                        }

                        DynamicTableRow(Convert.ToInt32(hdAssMtdTypId.Value));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion

                    #region Obligation
                    try
                    {
                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPNmOB1.Text = ViewState["BcProNo"].ToString();
                            lblBCPNmOB2.Text = ViewState["BcProNo"].ToString();
                            lblBCPNmOB3.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBCPNmOB1.Text = "";
                            lblBCPNmOB2.Text = "";
                            lblBCPNmOB3.Text = "";
                        }

                        if (ViewState["AppNm"] != null)
                        {
                            lblApplicantName.Text = ViewState["AppNm"].ToString();
                            lblAppName.Text = ViewState["AppNm"].ToString();
                            lblAppNameOB3.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblApplicantName.Text = "";
                            lblAppName.Text = "";
                            lblAppNameOB3.Text = "";
                        }

                        ds = oMem.CF_GetObligationDtls(Convert.ToInt64(hdLeadId.Value));
                        dt = ds.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            hdLeadId.Value = Convert.ToString(dt.Rows[0]["LeadID"]);

                            txtAppScore.Text = Convert.ToString(dt.Rows[0]["Apt_CBScore"]);
                            txtCoAppScore.Text = Convert.ToString(dt.Rows[0]["CoApt1_CBScore"]);
                            txtApp2Score.Text = Convert.ToString(dt.Rows[0]["CoApt2_CBScore"]);
                            txtApp3Score.Text = Convert.ToString(dt.Rows[0]["CoApt3_CBScore"]);
                            txtApp4Score.Text = Convert.ToString(dt.Rows[0]["CoApt4_CBScore"]);
                            txtGuaScore.Text = Convert.ToString(dt.Rows[0]["Grntr_CBScore"]);
                            lblAppFileNm.Text = Convert.ToString(dt.Rows[0]["Apt_CBRptFileName"]);
                            lblCoAppFileName.Text = Convert.ToString(dt.Rows[0]["CoApt1_CBRptFileName"]);
                            lblCoApp2FileName.Text = Convert.ToString(dt.Rows[0]["CoApt2_CBRptFileName"]);
                            lblCoApp3FileName.Text = Convert.ToString(dt.Rows[0]["CoApt3_CBRptFileName"]);
                            lblCoApp4FileName.Text = Convert.ToString(dt.Rows[0]["CoApt4_CBRptFileName"]);
                            lblGuaFileName.Text = Convert.ToString(dt.Rows[0]["Grntr_CBRptFileName"]);

                            hdnAppFileNm.Value = Convert.ToString(dt.Rows[0]["Apt_CBRptFileName"]);
                            hdnCoAppFileName.Value = Convert.ToString(dt.Rows[0]["CoApt1_CBRptFileName"]);
                            hdnCoApp2FileName.Value = Convert.ToString(dt.Rows[0]["CoApt2_CBRptFileName"]);
                            hdnCoApp3FileName.Value = Convert.ToString(dt.Rows[0]["CoApt3_CBRptFileName"]);
                            hdnCoApp4FileName.Value = Convert.ToString(dt.Rows[0]["CoApt4_CBRptFileName"]);
                            hdnGuaFileName.Value = Convert.ToString(dt.Rows[0]["Grntr_CBRptFileName"]);

                            hdFlag.Value = "Y";

                            hdnApplVerifyEquiYN.Value = Convert.ToString(dt.Rows[0]["Apt_EquiYN"]);
                            if (Convert.ToString(dt.Rows[0]["Apt_EquiYN"]) == "Y")
                            {
                                txtApplCBScore.Text = Convert.ToString(dt.Rows[0]["Ap_CBScore"]);
                                lblApplVerifyEquiStatus.Text = "Success";
                                btnVerifyApplEqui.Enabled = false;
                            }
                            else
                            {
                                txtApplCBScore.Text = Convert.ToString(dt.Rows[0]["Ap_CBScore"]);
                                lblApplVerifyEquiStatus.Text = "";
                                btnVerifyApplEqui.Enabled = true;
                            }

                            txtAppDt1.Text = Convert.ToString(dt.Rows[0]["Apt_Dt"]);
                            txtAppExDt.Text = Convert.ToString(dt.Rows[0]["AppExDt"]);


                            hdnCA1VerifyEquiYN.Value = Convert.ToString(dt.Rows[0]["CoApt_EquiYN"]);
                            if (Convert.ToString(dt.Rows[0]["CoApt_EquiYN"]) == "Y")
                            {
                                txtCA1CBScore.Text = Convert.ToString(dt.Rows[0]["CoAp_CBScore"]);
                                lblCA1VerifyEquiStatus.Text = "Success";
                                btnVerifyCA1Equi.Enabled = false;
                            }
                            else
                            {
                                txtCA1CBScore.Text = Convert.ToString(dt.Rows[0]["CoAp_CBScore"]);
                                lblCA1VerifyEquiStatus.Text = "";
                                btnVerifyCA1Equi.Enabled = true;
                            }
                            txtCoAppDt.Text = Convert.ToString(dt.Rows[0]["CoApt_Dt"]);
                            txtCoAppExDt.Text = Convert.ToString(dt.Rows[0]["CoAppExDt"]);


                            hdnCA2VerifyEquiYN.Value = Convert.ToString(dt.Rows[0]["CoApt2_EquiYN"]);
                            if (Convert.ToString(dt.Rows[0]["CoApt2_EquiYN"]) == "Y")
                            {
                                txtCA2CBScore.Text = Convert.ToString(dt.Rows[0]["CoAp2_CBScore"]);
                                lblCA2VerifyEquiStatus.Text = "Success";
                                btnVerifyCA2Equi.Enabled = false;
                            }
                            else
                            {
                                txtCA2CBScore.Text = Convert.ToString(dt.Rows[0]["CoAp2_CBScore"]);
                                lblCA2VerifyEquiStatus.Text = "";
                                btnVerifyCA2Equi.Enabled = true;
                            }
                            txtCoApp2Dt.Text = Convert.ToString(dt.Rows[0]["CoApt2_Dt"]);
                            txtCoApp2ExDt.Text = Convert.ToString(dt.Rows[0]["CoApp2ExDt"]);

                            hdnCA3VerifyEquiYN.Value = Convert.ToString(dt.Rows[0]["CoAp3_EquiYN"]);
                            if (Convert.ToString(dt.Rows[0]["CoAp3_EquiYN"]) == "Y")
                            {
                                txtCA3CBScore.Text = Convert.ToString(dt.Rows[0]["CoAp3_CBScore"]);
                                lblCA3VerifyEquiStatus.Text = "Success";
                                btnVerifyCA3Equi.Enabled = false;
                            }
                            else
                            {
                                txtCA3CBScore.Text = Convert.ToString(dt.Rows[0]["CoAp3_CBScore"]);
                                lblCA3VerifyEquiStatus.Text = "";
                                btnVerifyCA3Equi.Enabled = true;
                            }
                            txtCoApp3Dt.Text = Convert.ToString(dt.Rows[0]["CoApt3_Dt"]);
                            txtCoApp3ExDt.Text = Convert.ToString(dt.Rows[0]["CoApp3ExDt"]);

                            hdnCA4VerifyEquiYN.Value = Convert.ToString(dt.Rows[0]["CoAp4_EquiYN"]);
                            if (Convert.ToString(dt.Rows[0]["CoAp3_EquiYN"]) == "Y")
                            {
                                txtCA4CBScore.Text = Convert.ToString(dt.Rows[0]["CoAp4_CBScore"]);
                                lblCA4VerifyEquiStatus.Text = "Success";
                                btnVerifyCA4Equi.Enabled = false;
                            }
                            else
                            {
                                txtCA4CBScore.Text = Convert.ToString(dt.Rows[0]["CoAp4_CBScore"]);
                                lblCA4VerifyEquiStatus.Text = "";
                                btnVerifyCA4Equi.Enabled = true;
                            }
                            txtCoApp4Dt.Text = Convert.ToString(dt.Rows[0]["CoApt4_Dt"]);
                            txtCoApp4ExDt.Text = Convert.ToString(dt.Rows[0]["CoApp4ExDt"]);

                            hdnGuarVerifyEquiYN.Value = Convert.ToString(dt.Rows[0]["Grntr_EquiYN"]);
                            if (Convert.ToString(dt.Rows[0]["Grntr_EquiYN"]) == "Y")
                            {
                                txtGuarCBScore.Text = Convert.ToString(dt.Rows[0]["Grnt_CBScore"]);
                                lblGuarVerifyEquiStatus.Text = "Success";
                                btnVerifyGuarEqui.Enabled = false;
                            }
                            else
                            {
                                txtGuarCBScore.Text = Convert.ToString(dt.Rows[0]["Grnt_CBScore"]);
                                lblGuarVerifyEquiStatus.Text = "";
                                btnVerifyGuarEqui.Enabled = true;
                            }
                            txtGuaDt.Text = Convert.ToString(dt.Rows[0]["Grntr_Dt"]);
                            txtGuaExDt.Text = Convert.ToString(dt.Rows[0]["GuaExDt"]);


                            lblApplicantName.Text = Convert.ToString(dt.Rows[0]["MemberName"]);
                            lblAppName.Text = Convert.ToString(dt.Rows[0]["MemberName"]);
                            hdnCoApp1.Value = Convert.ToString(dt.Rows[0]["CA1Flag"]);
                            hdnCoApp2.Value = Convert.ToString(dt.Rows[0]["CA2Flag"]);
                            hdnCoApp3.Value = Convert.ToString(dt.Rows[0]["CA3Flag"]);
                            hdnCoApp4.Value = Convert.ToString(dt.Rows[0]["CA4Flag"]);
                            hdnGrnr.Value = Convert.ToString(dt.Rows[0]["GFlag"]);


                            pnlCBDtls.Enabled = true;
                            pnlUSFBDtls.Enabled = true;
                            pnlOD.Enabled = true;

                            GenerateObligationGrid();

                        }
                        else
                        {
                            //NOTE : LEAD IS NOT ELIGIBLE FOR OBLIGATION TILL IT BE A MEMBER.
                            pnlCBDtls.Enabled = false;
                            pnlUSFBDtls.Enabled = false;
                            pnlOD.Enabled = false;

                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion

                    #region ElecConsumption
                    try
                    {
                        if (ViewState["AppNm"] != null)
                        {
                            lblAppNameEC.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblAppNameEC.Text = "";
                        }
                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPNmEC.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBCPNmEC.Text = "";
                        }

                        ds = oMem.CF_GetElecConDtlByLeadID(Convert.ToInt64(hdLeadId.Value));
                        dt = ds.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            hdLeadId.Value = Convert.ToString(dt.Rows[0]["LeadID"]);
                            txtEntryDt.Text = Convert.ToString(dt.Rows[0]["EntryDt"]);
                            txtCnsmrNo.Text = Convert.ToString(dt.Rows[0]["CnsmrNo"]);
                            txtElcBrd.Text = Convert.ToString(dt.Rows[0]["ElcBrd"]);
                            txtLstBillAmt.Text = Convert.ToString(dt.Rows[0]["LstBillAmt"]);
                            txtLstUnit.Text = Convert.ToString(dt.Rows[0]["LstUnit"]);
                            txtP1BillAmt.Text = Convert.ToString(dt.Rows[0]["P1BillAmt"]);
                            txtP1Unit.Text = Convert.ToString(dt.Rows[0]["P1Unit"]);
                            txtP2BillAmt.Text = Convert.ToString(dt.Rows[0]["P2BillAmt"]);
                            txtP2Unit.Text = Convert.ToString(dt.Rows[0]["P2Unit"]);
                            txtP3BillAmt.Text = Convert.ToString(dt.Rows[0]["P3BillAmt"]);
                            txtP3Unit.Text = Convert.ToString(dt.Rows[0]["P3Unit"]);
                            txtP4BillAmt.Text = Convert.ToString(dt.Rows[0]["P4BillAmt"]);
                            txtP4Unit.Text = Convert.ToString(dt.Rows[0]["P4Unit"]);
                            txtP5BillAmt.Text = Convert.ToString(dt.Rows[0]["P5BillAmt"]);
                            txtP5Unit.Text = Convert.ToString(dt.Rows[0]["P5Unit"]);
                            txtP6BillAmt.Text = Convert.ToString(dt.Rows[0]["P6BillAmt"]);
                            txtP6Unit.Text = Convert.ToString(dt.Rows[0]["P6Unit"]);
                            txtP7BillAmt.Text = Convert.ToString(dt.Rows[0]["P7BillAmt"]);
                            txtP7Unit.Text = Convert.ToString(dt.Rows[0]["P7Unit"]);
                            txtP8BillAmt.Text = Convert.ToString(dt.Rows[0]["P8BillAmt"]);
                            txtP8Unit.Text = Convert.ToString(dt.Rows[0]["P8Unit"]);
                            txtP9BillAmt.Text = Convert.ToString(dt.Rows[0]["P9BillAmt"]);
                            txtP9Unit.Text = Convert.ToString(dt.Rows[0]["P9Unit"]);
                            txtP10BillAmt.Text = Convert.ToString(dt.Rows[0]["P10BillAmt"]);
                            txtP10Unit.Text = Convert.ToString(dt.Rows[0]["P10Unit"]);
                            txtP11BillAmt.Text = Convert.ToString(dt.Rows[0]["P11BillAmt"]);
                            txtP11Unit.Text = Convert.ToString(dt.Rows[0]["P11Unit"]);
                            txtAvBillAmt.Text = Convert.ToString(dt.Rows[0]["AvBillAmt"]);
                            txtAvUnit.Text = Convert.ToString(dt.Rows[0]["AvUnit"]);
                            lblEBillName.Text = Convert.ToString(dt.Rows[0]["FileName"]);
                            hdIsUpload.Value = Convert.ToString(dt.Rows[0]["IsUpload"]);
                            vIsUpload = Convert.ToString(dt.Rows[0]["IsUpload"]);
                            hdEmiAmt.Value = Convert.ToString(dt.Rows[0]["TotalEMI"]);
                            txtEMIAvBill.Text = Convert.ToString(dt.Rows[0]["EMIAvBill"]);
                            if (vIsUpload == "Y")
                            {
                                btnEbillDoc.Enabled = true;
                            }
                            else
                            {
                                btnEbillDoc.Enabled = false;
                            }

                        }
                        else
                        {
                            txtEntryDt.Text = Session[gblValue.LoginDate].ToString();
                            txtCnsmrNo.Text = "";
                            txtElcBrd.Text = "";
                            lblEBillName.Text = "";
                            txtLstBillAmt.Text = "0";
                            txtLstBillAmt.Text = "0";
                            txtLstUnit.Text = "0";
                            txtP1BillAmt.Text = "0";
                            txtP1Unit.Text = "0";
                            txtP2BillAmt.Text = "0";
                            txtP2Unit.Text = "0";
                            txtP3BillAmt.Text = "0";
                            txtP3Unit.Text = "0";
                            txtP4BillAmt.Text = "0";
                            txtP4Unit.Text = "0";
                            txtP5BillAmt.Text = "0";
                            txtP5Unit.Text = "0";
                            txtP6BillAmt.Text = "0";
                            txtP6Unit.Text = "0";
                            txtP7BillAmt.Text = "0";
                            txtP7Unit.Text = "0";
                            txtP8BillAmt.Text = "0";
                            txtP8Unit.Text = "0";
                            txtP9BillAmt.Text = "0";
                            txtP9Unit.Text = "0";
                            txtP10BillAmt.Text = "0";
                            txtP10Unit.Text = "0";
                            txtP11BillAmt.Text = "0";
                            txtP11Unit.Text = "0";
                            txtAvBillAmt.Text = "0";
                            txtAvUnit.Text = "0";
                            lblEBillName.Text = "";
                            hdIsUpload.Value = "N";
                            btnEbillDoc.Enabled = false;
                            hdEmiAmt.Value = "0";
                            txtEMIAvBill.Text = "0";

                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion

                    #region Banking
                    try
                    {
                        if (ViewState["AppNm"] != null)
                        {
                            lblAppNameBK.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblAppNameBK.Text = "";
                        }
                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPNmBK.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBCPNmBK.Text = "";
                        }

                        ds = oDist.CF_GenerateBankingGrid(Convert.ToInt64(hdLeadId.Value));
                        dt = ds.Tables[0];
                        dt1 = ds.Tables[1];

                        gvBankDtl.DataSource = dt;
                        gvBankDtl.DataBind();
                        if (dt.Rows.Count > 0)
                        {
                            txtRemarks.Text = Convert.ToString(dt.Rows[0]["Remarks"]);
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            hdnTotalEMI.Value = Convert.ToString(dt1.Rows[0]["TotalEMI"]);
                            double TotalABB = 0; lblTotalABB.Text = ""; double Cal = 0;

                            foreach (GridViewRow gr in gvBankDtl.Rows)
                            {
                                TextBox txtABB = (TextBox)gvBankDtl.Rows[gr.RowIndex].FindControl("txtABB");
                                if (txtABB.Text != "")
                                {
                                    if (txtABB.Text != "0")
                                    {
                                        TotalABB += Convert.ToDouble(txtABB.Text);
                                    }
                                }
                            }
                            if (TotalABB != 0)
                            {
                                Cal = Math.Round(TotalABB / Convert.ToDouble(hdnTotalEMI.Value), 2);
                                lblTotalABB.Text = Convert.ToString(Cal);
                            }
                            GridViewRow footerRow = gvBankDtl.FooterRow;
                            if (footerRow != null)
                            {
                                Label lblABBtotal = (Label)gvBankDtl.FooterRow.FindControl("lblABBtotal");

                                if (lblABBtotal != null)
                                {
                                    if (TotalABB > 0)
                                        lblABBtotal.Text = TotalABB.ToString();
                                    else
                                        lblABBtotal.Text = "0";
                                }

                            }

                        }
                        else
                        {
                            hdnTotalEMI.Value = "0";
                            lblTotalABB.Text = "0";

                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion

                    break;
                case 4:
                    #region SolarPower
                    try
                    {
                        if (ViewState["AppNm"] != null)
                        {
                            lblAppNameSP.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblAppNameSP.Text = "";
                        }
                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPNmSP.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBCPNmSP.Text = "";
                        }

                        ds = oDist.CF_GenerateSolarSystemGrid(Convert.ToInt64(hdLeadId.Value));
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            txtAddress.Text = Convert.ToString(dt.Rows[0]["PropAddress"]);
                            ddlPropOwnership.SelectedIndex = ddlPropOwnership.Items.IndexOf(ddlPropOwnership.Items.FindByValue(dt.Rows[0]["PropOwnership"].ToString()));
                            txtPropOwnerName.Text = Convert.ToString(dt.Rows[0]["PropOwnerName"]);

                            ViewState["SolarId"] = Convert.ToString(dt.Rows[0]["SolPwrSysId"]);
                            ViewState["SolarType"] = Convert.ToString(dt.Rows[0]["SolarType"]);

                        }

                        gvSolarDtl.DataSource = dt;
                        gvSolarDtl.DataBind();
                        CalculateTotalCost();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion

                    #region InsurenceChargs
                    try
                    {
                        if (ViewState["AppNm"] != null)
                        {
                            lblAppNmIN.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblAppNmIN.Text = "";
                        }
                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPNmIN.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBCPNmIN.Text = "";
                        }

                        ds = oDist.CF_GetInsuranceChargeByLeadID(Convert.ToInt64(hdLeadId.Value));
                        dt = ds.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            hdLoanAmount.Value = dt.Rows[0]["LoanAmount"].ToString();
                            ViewState["InsuChrg"] = dt;
                            gvInsuChrgDtl.DataSource = dt;
                            gvInsuChrgDtl.DataBind();

                            Calculation_Total();
                        }

                        else
                        {
                            hdLoanAmount.Value = "0";
                            gblFuction.MsgPopup("No Loan Amount to Calculate Charges.....");

                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion

                    #region LTV
                    try
                    {
                        if (ViewState["AppNm"] != null)
                        {
                            lblAppNmLTV.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblAppNmLTV.Text = "";
                        }
                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPNmLTV.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBCPNmLTV.Text = "";
                        }

                        ds = oDist.CF_GetLTVComputationByLeadID(Convert.ToInt64(hdLeadId.Value));
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            ViewState["LTVCal"] = dt;
                            hdLoanAmountLTV.Value = dt.Rows[0]["LoanAmount"].ToString();
                            hdTotalCharges.Value = dt.Rows[0]["TotalCharges"].ToString();
                            hdLeadId.Value = Convert.ToString(dt.Rows[0]["LeadID"]);

                            txtQuotationCostVal.Text = dt.Rows[0]["QuotationCostVal"].ToString();
                            txtGovCostVal.Text = dt.Rows[0]["GovCostVal"].ToString();

                            txtMaxEligibleFundingA.Text = dt.Rows[0]["QuotationCostAllowFundPercent"].ToString();
                            txtMaxEligibleFundingB.Text = dt.Rows[0]["GovCostAllowFundPercent"].ToString();

                            txtMarginMoney.Text = dt.Rows[0]["MarginMoneyAmount"].ToString();
                            lblMarMon.Text = dt.Rows[0]["MoneyMarginReceiptFN"].ToString();
                            lblMarMonBank.Text = dt.Rows[0]["MoneyMarginBankRflctnFN"].ToString();
                            lblQuotCopy.Text = dt.Rows[0]["QuationCopyFN"].ToString();

                            hdnMarMon.Value = dt.Rows[0]["MoneyMarginReceiptFN"].ToString();
                            hdnMarMonBank.Value = dt.Rows[0]["MoneyMarginBankRflctnFN"].ToString();
                            hdnQuotCopy.Value = dt.Rows[0]["QuationCopyFN"].ToString();

                            Calculation();

                        }
                        else
                        {
                            txtQuotationCostVal.Text = "0";
                            txtGovCostVal.Text = "0";
                            txtMaxEligibleFundingA.Text = "80";
                            txtMaxEligibleFundingB.Text = "90";
                            txtMarginMoney.Text = "0";
                            txtAllowedFundingA.Text = "0";
                            txtAllowedFundingB.Text = "0";
                            txtMaxAllowedFunding.Text = "0";
                            txtFinalLTVWithoutInsuranceA.Text = "0";
                            txtFinalLTVWithInsuranceA.Text = "0";
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion

                    break;

                case 5:
                    #region Deviation
                    try
                    {
                        if (ViewState["AppNm"] != null)
                        {
                            lblAppNameDV.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblAppNameDV.Text = "";
                        }
                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPNmDV.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBCPNmDV.Text = "";
                        }

                        ds = oDist.CF_GenerateDeviationGrid(Convert.ToInt64(hdLeadId.Value));
                        dt = ds.Tables[0];
                        gvDevDtl.DataSource = dt;
                        gvDevDtl.DataBind();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion

                    #region Saction
                    try
                    {
                        if (ViewState["AppNm"] != null)
                        {
                            lblAppNmSC.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblAppNmSC.Text = "";
                        }
                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPNmSC.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBCPNmSC.Text = "";
                        }

                        ds = oDist.CF_GenerateSanctionGrid(Convert.ToInt64(hdLeadId.Value));
                        dt = ds.Tables[0];
                        gvSancDtl.DataSource = dt;
                        gvSancDtl.DataBind();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion

                    #region Recommendation
                    //try
                    //{
                    //    if (ViewState["BcProNo"] != null)
                    //    {
                    //        lblBCPNum.Text = ViewState["BcProNo"].ToString();
                    //    }
                    //    else
                    //    {
                    //        lblBCPNum.Text = "";
                    //    }

                    //    if (ViewState["AppNm"] != null)
                    //    {
                    //        lblApplNm.Text = ViewState["AppNm"].ToString();
                    //    }
                    //    else
                    //    {
                    //        lblApplNm.Text = "";
                    //    }


                    //    ds = oDist.CF_GenerateRecommendation(Convert.ToInt64(hdLeadId.Value));
                    //    dt = ds.Tables[0];
                    //    dt1 = ds.Tables[1];
                    //    dt2 = ds.Tables[2];
                    //    if (dt2.Rows.Count > 0)
                    //    {
                    //        ViewState["InsuChrg"] = dt2;
                    //    }
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        hdLoanAmountR.Value = dt.Rows[0]["LoanAmount"].ToString();
                    //        hdInsuCharge.Value = dt.Rows[0]["InsuCharge"].ToString();
                    //        hdFOIR.Value = dt.Rows[0]["FOIR"].ToString();
                    //        txtFOIR.Text = dt.Rows[0]["FOIR"].ToString();
                    //        hdExistingEMI.Value = dt.Rows[0]["ExistingEMI"].ToString();
                    //        txtExistingEMI.Text = dt.Rows[0]["ExistingEMI"].ToString();
                    //        hdNetSurplus.Value = dt.Rows[0]["NetSurplus"].ToString();
                    //        txtMonthlyNetSurplus.Text = dt.Rows[0]["NetSurplus"].ToString();
                    //        hdAllowFund.Value = dt.Rows[0]["AllowFund"].ToString();
                    //    }
                    //    else
                    //    {
                    //        hdInsuCharge.Value = "0";
                    //        hdFOIR.Value = "0";
                    //        txtFOIR.Text = "0";
                    //        hdExistingEMI.Value = "0";
                    //        txtExistingEMI.Text = "0";
                    //        hdNetSurplus.Value = "0";
                    //        txtMonthlyNetSurplus.Text = "0";
                    //    }
                    //    if (dt1.Rows.Count > 0)
                    //    {
                    //        txtLoanAmt.Text = dt1.Rows[0]["LoanAmt"].ToString();
                    //        txtROI.Text = dt1.Rows[0]["ROI"].ToString();
                    //        txtTenure.Text = dt1.Rows[0]["Tenure"].ToString();

                    //        txtIntRiskScore.Text = dt1.Rows[0]["InternalScore"].ToString();
                    //        ddlAction.SelectedValue = dt1.Rows[0]["Action"].ToString();
                    //        txtHoldRemarks.Text = dt1.Rows[0]["Remarks"].ToString();
                    //        lblFileNameR.Text = dt1.Rows[0]["FileName"].ToString();
                    //        hdnFileName.Value = dt1.Rows[0]["FileName"].ToString();


                    //    }
                    //    else
                    //    {
                    //        txtLoanAmt.Text = "0";
                    //        txtROI.Text = "0";
                    //        txtTenure.Text = "0";
                    //        txtIntRiskScore.Text = "0";
                    //        ddlAction.SelectedValue = "-1";
                    //        txtHoldRemarks.Text = "";
                    //        lblFileNameR.Text = "";

                    //    }
                    //    CalculationR();

                    //    if (lblFileNameR.Text != "")
                    //    {
                    //        btnUPAttachment.Enabled = true;
                    //    }
                    //    else
                    //    {
                    //        btnUPAttachment.Enabled = false;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw ex;
                    //}
                    //finally
                    //{
                    //    ds = null;
                    //    dt = null;
                    //    dt1 = null;
                    //    dt2 = null;
                    //    dt3 = null;
                    //}
                    #endregion
                    break;

                case 6:
                    #region Documents
                    try
                    {
                        if (ViewState["AppNm"] != null)
                        {
                            lblApplNmDU.Text = ViewState["AppNm"].ToString();

                        }
                        else
                        {
                            lblApplNmDU.Text = "";
                        }

                        if (ViewState["BcProNo"] != null)
                        {
                            txtBCProposalNo.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            txtBCProposalNo.Text = "";
                        }

                        dt = oDist.CF_GetUpldDocDtlByLeadID(Convert.ToInt64(hdLeadId.Value));
                        if (dt.Rows.Count > 0)
                        {
                            txtBCProposalNo.Text = Convert.ToString(dt.Rows[0]["BCPropNo"]);
                            ShowEmpRecordsDocUpld();
                        }
                        else
                        {
                            ShowEmpRecordsDocUpld();

                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion
                    break;

                case 7:
                    #region FinalDecision
                    try
                    {
                        if (ViewState["BcProNo"] != null)
                        {
                            lblProposalNo.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblProposalNo.Text = "";
                        }

                        if (ViewState["AppNm"] != null)
                        {
                            lblApplName.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblApplName.Text = "";
                        }

                        ds = oDist.CF_GenerateFinalDecision(Convert.ToInt64(hdLeadId.Value));
                        dt = ds.Tables[0];
                        dt1 = ds.Tables[1];
                        dt2 = ds.Tables[2];
                        dt3 = ds.Tables[3];
                        dt4 = ds.Tables[4];
                        dt5 = ds.Tables[5];

                        if (dt1.Rows.Count > 0)
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                ViewState["InsuChrg"] = dt2;
                            }
                            if (dt.Rows.Count > 0)
                            {
                                hdLoanAmountF.Value = dt.Rows[0]["LoanAmount"].ToString();
                                hdInsuChargeF.Value = dt.Rows[0]["InsuCharge"].ToString();
                                hdFOIRF.Value = dt.Rows[0]["FOIR"].ToString();
                                txtFOIRF.Text = dt.Rows[0]["FOIR"].ToString();
                                hdExistingEMIF.Value = dt.Rows[0]["ExistingEMI"].ToString();
                                txtExistingEMIF.Text = dt.Rows[0]["ExistingEMI"].ToString();
                                hdNetSurplusF.Value = dt.Rows[0]["NetSurplus"].ToString();
                                txtMonthlyNetSurplusF.Text = dt.Rows[0]["NetSurplus"].ToString();
                                hdAllowFundF.Value = dt.Rows[0]["AllowFund"].ToString();
                            }
                            else
                            {
                                hdInsuChargeF.Value = "0";
                                hdFOIRF.Value = "0";
                                txtFOIRF.Text = "0";
                                hdExistingEMIF.Value = "0";
                                txtExistingEMIF.Text = "0";
                                hdNetSurplusF.Value = "0";
                                txtMonthlyNetSurplusF.Text = "0";
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                txtLoanAmtF.Text = dt1.Rows[0]["LoanAmt"].ToString();
                                txtROIF.Text = dt1.Rows[0]["ROI"].ToString();
                                txtTenureF.Text = dt1.Rows[0]["Tenure"].ToString();

                                txtIntRiskScoreF.Text = dt1.Rows[0]["InternalScore"].ToString();
                                txtActionF.Text = dt1.Rows[0]["Action"].ToString();
                                txtHoldRemarksF.Text = dt1.Rows[0]["Remarks"].ToString();
                                lblFileNameF.Text = dt1.Rows[0]["FileName"].ToString();
                                hdnFileNameF.Value = dt1.Rows[0]["FileName"].ToString();

                                txtSacRemarks.Text = dt1.Rows[0]["SantionRemarks"].ToString();
                                txtSacDt.Text = dt1.Rows[0]["SantionDate"].ToString();
                                txtSacExDt.Text = Convert.ToString(dt1.Rows[0]["SantionExDate"]);


                                if (dt1.Rows[0]["SactionComm"].ToString() == "")
                                {
                                    //ListItem oli1 = new ListItem("<--Select-->", "-1");
                                    //ddlSacComm.Items.Insert(0, oli1);

                                    MaxAuthVal = Convert.ToInt32(dt5.Rows[0]["MaxAuthVal"]);

                                    BindSacComm(MaxAuthVal);

                                }
                                else
                                {
                                    ddlSacComm.SelectedValue = dt1.Rows[0]["SactionComm"].ToString();
                                }


                                if (dt1.Rows[0]["RecommendtRemarks"].ToString() == "")
                                {
                                    ddlRecommRemarks.SelectedIndex = -1;
                                }
                                else
                                {
                                    ddlRecommRemarks.SelectedValue = dt1.Rows[0]["RecommendtRemarks"].ToString();
                                }

                            }
                            else
                            {
                                txtLoanAmtF.Text = "0";
                                txtROIF.Text = "0";
                                txtTenureF.Text = "0";
                                txtIntRiskScoreF.Text = "0";
                                txtActionF.Text = "0";
                                txtHoldRemarksF.Text = "";
                                lblFileNameF.Text = "";
                                txtSacRemarks.Text = "";
                                txtSacDt.Text = "";
                                ddlSacComm.SelectedValue = "-1";
                                ddlRecommRemarks.SelectedValue = "-1";

                            }
                            if (dt3.Rows.Count > 0)
                            {
                                lblCAMFileName.Text = dt3.Rows[0]["CamFileName"].ToString();
                                hdnCAMFileName.Value = dt3.Rows[0]["CamFileName"].ToString();
                            }
                            CalculationF();


                            //  tbBasicDet.ActiveTabIndex = 1;
                            //  StatusButton("Show");
                            if (lblFileNameF.Text != "")
                            {
                                btnUPAttachmentF.Enabled = true;
                            }
                            else
                            {
                                btnUPAttachmentF.Enabled = false;
                            }
                            if (lblCAMFileName.Text != "")
                            {
                                btnCAM.Enabled = true;
                            }
                            else
                            {
                                btnCAM.Enabled = false;
                            }

                            gvDev.DataSource = dt4;
                            gvDev.DataBind();
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("This Lead is not Recommended for Final Decision...");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion
                    break;
            }
            DisableControl(false, this.Page);
        }

        protected void BindSacComm(int MaxAuthVal)
        {
            if (MaxAuthVal == 2)
            {
                ddlSacComm.Items.Clear();
                ddlSacComm.Items.Add(new ListItem("Sanction Committee 1", "Sanction Committee 1"));
                ddlSacComm.Items.Add(new ListItem("Sanction Committee 2", "Sanction Committee 2"));
                ddlSacComm.Items.Add(new ListItem("Sanction Committee 3", "Sanction Committee 3"));
                ddlSacComm.Items.Add(new ListItem("Sanction Committee 4", "Sanction Committee 4"));
            }
            else if (MaxAuthVal == 3)
            {
                ddlSacComm.Items.Clear();
                ddlSacComm.Items.Add(new ListItem("Sanction Committee 2", "Sanction Committee 2"));
                ddlSacComm.Items.Add(new ListItem("Sanction Committee 3", "Sanction Committee 3"));
                ddlSacComm.Items.Add(new ListItem("Sanction Committee 4", "Sanction Committee 4"));
            }
            else if (MaxAuthVal == 4)
            {
                ddlSacComm.Items.Clear();
                ddlSacComm.Items.Add(new ListItem("Sanction Committee 3", "Sanction Committee 3"));
                ddlSacComm.Items.Add(new ListItem("Sanction Committee 4", "Sanction Committee 4"));
            }
            else if (MaxAuthVal == 5)
            {
                ddlSacComm.Items.Clear();
                ddlSacComm.Items.Add(new ListItem("Sanction Committee 4", "Sanction Committee 4"));
            }
            else if (MaxAuthVal == 0)
            {
                ddlSacComm.Items.Clear();
                ddlSacComm.Items.Add(new ListItem("Sanction Committee 1", "Sanction Committee 1"));
                ddlSacComm.Items.Add(new ListItem("Sanction Committee 2", "Sanction Committee 2"));
                ddlSacComm.Items.Add(new ListItem("Sanction Committee 3", "Sanction Committee 3"));
                ddlSacComm.Items.Add(new ListItem("Sanction Committee 4", "Sanction Committee 4"));
            }
        }
        private void CalculateTotalCostBank()
        {
            double TotalABB = 0; lblTotalABB.Text = ""; double Cal = 0;

            foreach (GridViewRow gr in gvBankDtl.Rows)
            {
                TextBox txtABB = (TextBox)gvBankDtl.Rows[gr.RowIndex].FindControl("txtABB");
                if (txtABB.Text != "")
                {
                    if (txtABB.Text != "0")
                    {
                        TotalABB += Convert.ToDouble(txtABB.Text);
                    }
                }
            }
            if (TotalABB != 0)
            {
                Cal = Math.Round(TotalABB / Convert.ToDouble(hdnTotalEMI.Value), 2);
                lblTotalABB.Text = Convert.ToString(Cal);
            }
            else
            {
                lblTotalABB.Text = "0";
            }

            GridViewRow footerRow = gvBankDtl.FooterRow;
            if (footerRow != null)
            {
                Label lblABBtotal = (Label)gvBankDtl.FooterRow.FindControl("lblABBtotal");

                if (lblABBtotal != null)
                {
                    if (TotalABB > 0)
                        lblABBtotal.Text = TotalABB.ToString();
                    else
                        lblABBtotal.Text = "0";
                }

            }
        }
        protected void tbLoanAppDtl_ActiveTabChanged(object sender, EventArgs e)
        {
            TabContainer tabContainer = (TabContainer)sender;
            int ActiveTabIndex = tabContainer.ActiveTabIndex;
            if (ActiveTabIndex > 0)
            {
                tabChangeFunction(ActiveTabIndex);
            }
            this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
            this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
        }


        #region AllCalculations

        private void CalculationF()
        {

            double vLoanAmt = 0;
            double vInsuCharge = 0;
            double vROI = Convert.ToDouble(txtROIF.Text);
            double vTenure = Convert.ToDouble(txtTenureF.Text);
            double vFOIR = Convert.ToDouble(hdFOIRF.Value);
            double EMIAmt = 0, RoundEMIAmt = 0;

            if (hdLoanAmountF.Value == "0")
            {
                vLoanAmt = Convert.ToDouble(txtLoanAmtF.Text);
            }
            else
            {
                vLoanAmt = Convert.ToDouble(hdLoanAmountF.Value);
            }


            #region
            double vChargePercent = 0, vChargeAmount = 0, vGSTPercent = 0, vGSTAmount = 0, vGrandTotal = 0;
            DataTable dt = null;
            dt = (DataTable)ViewState["InsuChrg"];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        vChargePercent = Convert.ToDouble(dt.Rows[i]["ChargePercent"].ToString());
                        vGSTPercent = Convert.ToDouble(dt.Rows[i]["GSTPercent"].ToString());
                        if (vChargePercent == 0)
                        {
                            vChargeAmount = Convert.ToDouble(dt.Rows[i]["ChargeAmount"].ToString());
                        }
                        else
                        {
                            vChargeAmount = Math.Round(((Convert.ToDouble(vLoanAmt) * Convert.ToDouble(vChargePercent)) / 100), 2);
                        }
                        if (vGSTPercent == 0)
                        {
                            vGSTAmount = Convert.ToDouble(dt.Rows[i]["GSTAmount"].ToString());
                        }
                        else
                        {
                            vGSTAmount = Math.Round(((Convert.ToDouble(vChargeAmount) * Convert.ToDouble(vGSTPercent)) / 100), 2);
                        }

                        vGrandTotal = vGrandTotal + vChargeAmount + vGSTAmount;
                    }

                    vInsuCharge = vGrandTotal;
                }
                else
                {
                    vInsuCharge = 0;

                }
            }

            hdInsuChargeF.Value = vInsuCharge.ToString();
            #endregion


            double vExistingEMI = Convert.ToDouble(hdExistingEMIF.Value);

            txtTotalLoanAmountF.Text = Convert.ToString(vLoanAmt + vInsuCharge);
            txtTotalLoanAmountF.Text = Convert.ToString(vLoanAmt + vInsuCharge);

            EMIAmt = emi_calculatorF((vLoanAmt + vInsuCharge), vROI, vTenure);
            RoundEMIAmt = Math.Ceiling(EMIAmt);
            txtEMIF.Text = Convert.ToString(RoundEMIAmt);
            txtTotEMIF.Text = Convert.ToString(Math.Ceiling(EMIAmt + vExistingEMI));
        }


        double emi_calculatorF(double p, double r, double t)
        {
            double emi;

            r = r / (12 * 100); // one month interest
            emi = (p * r * Math.Pow(1 + r, t)) / (Math.Pow(1 + r, t) - 1);

            return (Math.Round(emi, 2));
        }

        double emi_calculator(double p, double r, double t)
        {
            double emi;

            r = r / (12 * 100); // one month interest
            emi = (p * r * Math.Pow(1 + r, t)) / (Math.Pow(1 + r, t) - 1);

            return (Math.Round(emi, 2));
        }

        //private void CalculationR()
        //{

        //    double vLoanAmt = 0;
        //    double vInsuCharge = 0;
        //    double vROI = Convert.ToDouble(txtROI.Text);
        //    double vTenure = Convert.ToDouble(txtTenure.Text);
        //    double vFOIR = Convert.ToDouble(hdFOIR.Value);
        //    if (hdLoanAmount.Value == "0")
        //    {
        //        vLoanAmt = Convert.ToDouble(txtLoanAmt.Text);
        //    }
        //    else
        //    {
        //        vLoanAmt = Convert.ToDouble(hdLoanAmount.Value);
        //    }


        //    #region
        //    double vChargePercent = 0, vChargeAmount = 0, vGSTPercent = 0, vGSTAmount = 0, vGrandTotal = 0;
        //    DataTable dt = null;
        //    dt = (DataTable)ViewState["InsuChrg"];
        //    if (dt != null)
        //    {
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                vChargePercent = Convert.ToDouble(dt.Rows[i]["ChargePercent"].ToString());
        //                vGSTPercent = Convert.ToDouble(dt.Rows[i]["GSTPercent"].ToString());
        //                if (vChargePercent == 0)
        //                {
        //                    vChargeAmount = Convert.ToDouble(dt.Rows[i]["ChargeAmount"].ToString());
        //                }
        //                else
        //                {
        //                    vChargeAmount = Math.Round(((Convert.ToDouble(vLoanAmt) * Convert.ToDouble(vChargePercent)) / 100), 2);
        //                }
        //                if (vGSTPercent == 0)
        //                {
        //                    vGSTAmount = Convert.ToDouble(dt.Rows[i]["GSTAmount"].ToString());
        //                }
        //                else
        //                {
        //                    vGSTAmount = Math.Round(((Convert.ToDouble(vChargeAmount) * Convert.ToDouble(vGSTPercent)) / 100), 2);
        //                }

        //                vGrandTotal = vGrandTotal + vChargeAmount + vGSTAmount;
        //            }

        //            vInsuCharge = vGrandTotal;
        //        }
        //        else
        //        {
        //            vInsuCharge = 0;

        //        }
        //    }

        //    hdInsuCharge.Value = vInsuCharge.ToString();
        //    #endregion


        //    double vExistingEMI = Convert.ToDouble(hdExistingEMI.Value);

        //    txtTotalLoanAmount.Text = Convert.ToString(vLoanAmt + vInsuCharge);
        //    txtTotalLoanAmount.Text = Convert.ToString(vLoanAmt + vInsuCharge);

        //    txtEMI.Text = Convert.ToString(emi_calculator((vLoanAmt + vInsuCharge), vROI, vTenure));
        //    txtTotEMI.Text = Convert.ToString(Convert.ToDouble(txtEMI.Text) + vExistingEMI);
        //}
        private void Calculation()
        {
            double vAlloweFundA = 0, vAlloweFundB = 0, vMaxAlloweFund = 0, vQval = 0, vGval = 0,
            vFinalLTVwithout = 0, vFinalLTVwith = 0, vLoanAmount = 0, VInsurCh = 0, vFundA = 0, vFundB;
            vLoanAmount = Convert.ToDouble(hdLoanAmountLTV.Value);
            VInsurCh = Convert.ToDouble(hdTotalCharges.Value);
            vQval = Convert.ToDouble(txtQuotationCostVal.Text);
            vGval = Convert.ToDouble(txtGovCostVal.Text);
            vFundA = Convert.ToDouble(txtMaxEligibleFundingA.Text);
            vFundB = Convert.ToDouble(txtMaxEligibleFundingB.Text);

            txtAllowedFundingA.Text = ""; txtAllowedFundingB.Text = ""; txtMaxAllowedFunding.Text = "";
            txtFinalLTVWithoutInsuranceA.Text = ""; txtFinalLTVWithInsuranceA.Text = "";


            if (vQval > 0 && vFundA > 0)
            {
                vAlloweFundA = (vQval * vFundA) / 100;
            }
            if (vGval > 0 && vFundB > 0)
            {
                vAlloweFundB = (vGval * vFundB) / 100;
            }
            if (vAlloweFundA > 0 && vAlloweFundB > 0)
            {
                if (vAlloweFundA < vAlloweFundB)
                    vMaxAlloweFund = vAlloweFundA;
                else
                    vMaxAlloweFund = vAlloweFundB;

                if (vLoanAmount > 0)
                {
                    vFinalLTVwithout = (vLoanAmount / vMaxAlloweFund) * 100;
                    vFinalLTVwith = ((vLoanAmount + VInsurCh) / vMaxAlloweFund) * 100;
                }
                else
                {
                    vFinalLTVwithout = 0;
                    vFinalLTVwith = 0;
                }
            }
            else
            {
                vMaxAlloweFund = 0;
                vFinalLTVwithout = 0;
                vFinalLTVwith = 0;
            }
            txtAllowedFundingA.Text = Convert.ToString(Math.Round(vAlloweFundA, 2));
            txtAllowedFundingB.Text = Convert.ToString(Math.Round(vAlloweFundB, 2));

            txtMaxAllowedFunding.Text = Convert.ToString(Math.Round(vMaxAlloweFund, 2));

            txtFinalLTVWithoutInsuranceA.Text = Convert.ToString(Math.Round(vFinalLTVwithout, 2));

            txtFinalLTVWithInsuranceA.Text = Convert.ToString(Math.Round(vFinalLTVwith, 2));

        }

        void Calculation_Total()
        {
            DataTable dt = (DataTable)ViewState["InsuChrg"];
            double TotalCharge = 0, TotalGST = 0, GrandTotal = 0;

            foreach (GridViewRow gr in gvInsuChrgDtl.Rows)
            {
                TextBox txtChargeAmount = (TextBox)gvInsuChrgDtl.Rows[gr.RowIndex].FindControl("txtChargeAmount");
                TextBox txtGSTAmount = (TextBox)gvInsuChrgDtl.Rows[gr.RowIndex].FindControl("txtGSTAmount");
                TextBox txtGSTPercent = (TextBox)gvInsuChrgDtl.Rows[gr.RowIndex].FindControl("txtGSTPercent");
                TextBox txtChargePercent = (TextBox)gvInsuChrgDtl.Rows[gr.RowIndex].FindControl("txtChargePercent");
                TextBox txtFirstCreateDateTime = (TextBox)gvInsuChrgDtl.Rows[gr.RowIndex].FindControl("txtFirstCreateDateTime");
                TextBox txtModifiedDateTime = (TextBox)gvInsuChrgDtl.Rows[gr.RowIndex].FindControl("txtModifiedDateTime");

                if (txtChargePercent.Text == "0")
                {
                    txtChargeAmount.Enabled = true;
                }
                else
                {
                    txtChargeAmount.Text = Convert.ToString(Math.Round(((Convert.ToDouble(hdLoanAmount.Value) * Convert.ToDouble(txtChargePercent.Text)) / 100), 2));
                    txtChargeAmount.Enabled = false;
                }
                if (txtGSTPercent.Text == "0")
                {
                    txtGSTAmount.Enabled = true;
                }
                else
                {
                    txtGSTAmount.Text = Convert.ToString(Math.Round(((Convert.ToDouble(txtChargeAmount.Text) * Convert.ToDouble(txtGSTPercent.Text)) / 100), 2));
                    txtGSTAmount.Enabled = false;
                }
                if (txtChargeAmount.Text != "0")
                {
                    TotalCharge += Convert.ToDouble(txtChargeAmount.Text);
                }
                if (txtGSTAmount.Text != "0")
                {
                    TotalGST += Convert.ToDouble(txtGSTAmount.Text);
                }
                GrandTotal = TotalCharge + TotalGST;
                txtTotal.Text = GrandTotal.ToString();
                txtTotalCharge.Text = TotalCharge.ToString();
                txtTotalGST.Text = TotalGST.ToString();

                dt.Rows[gr.RowIndex]["GSTAmount"] = txtGSTAmount.Text;
                dt.Rows[gr.RowIndex]["ChargeAmount"] = txtChargeAmount.Text;
                dt.Rows[gr.RowIndex]["ChargePercent"] = txtChargePercent.Text;
                dt.Rows[gr.RowIndex]["GSTPercent"] = txtGSTPercent.Text;
                dt.Rows[gr.RowIndex]["FirstCreateDateTime"] = txtFirstCreateDateTime.Text;
                dt.Rows[gr.RowIndex]["ModifiedDateTime"] = txtModifiedDateTime.Text;

                dt.AcceptChanges();
            }
            gvInsuChrgDtl.DataSource = dt;
            gvInsuChrgDtl.DataBind();

            ViewState["InsuChrg"] = dt;
        }

        private void CalculateTotalCost()
        {
            decimal totalCost = 0, Cal = 0, totalECost = 0;
            decimal vTCost = 0, vACost = 0;
            foreach (GridViewRow gr in gvSolarDtl.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {

                    TextBox txtTotalCost = (TextBox)gr.FindControl("txtTotalCost");
                    TextBox txtAllowedLTV = (TextBox)gr.FindControl("txtAllowedLTV");
                    Label lblEligibleCost = (Label)gr.FindControl("lblEligibleCost");

                    decimal rowCost = 0;
                    decimal.TryParse(txtTotalCost.Text, out rowCost); //parse the cost from the textbox
                    totalCost += Math.Round(rowCost, 2);


                    if (txtTotalCost != null && txtAllowedLTV != null)
                    {
                        if (txtTotalCost.Text != "" && txtAllowedLTV.Text != "")
                        {
                            if (txtTotalCost.Text != "0" && txtAllowedLTV.Text != "0")
                            {
                                vTCost = Convert.ToDecimal(txtTotalCost.Text);
                                vACost = Convert.ToDecimal(txtAllowedLTV.Text);

                                Cal = (vTCost * vACost) / 100;
                                lblEligibleCost.Text = Convert.ToString(Math.Round(Cal, 2));
                            }
                            else
                            {
                                lblEligibleCost.Text = "0";
                            }
                        }
                        else
                        {
                            lblEligibleCost.Text = "0";
                        }
                    }
                    else
                    {
                        lblEligibleCost.Text = "0";
                    }
                    decimal rowECost = 0;
                    decimal.TryParse(lblEligibleCost.Text, out rowECost); //parse the cost from the textbox
                    totalECost += Math.Round(rowECost, 2);

                }


            }
            GridViewRow footerRow = gvSolarDtl.FooterRow;
            if (footerRow != null)
            {
                Label lbltotal = (Label)gvSolarDtl.FooterRow.FindControl("lbltotal");
                Label lbltotalECost = (Label)gvSolarDtl.FooterRow.FindControl("lbltotalECost");
                if (lbltotal != null)
                {
                    if (totalCost > 0)
                        lbltotal.Text = totalCost.ToString();
                    else
                        lbltotal.Text = "0";
                }
                if (lbltotalECost != null)
                {
                    if (totalCost > 0)
                        lbltotalECost.Text = totalECost.ToString();
                    else
                        lbltotalECost.Text = "0";
                }
            }
            //UpFamily.Update();
        }

        #endregion

        #region ImageView
        //protected void btnUPAttachment_Click(object sender, EventArgs e)
        //{
        //    ViewImgDocR(hdLeadId.Value, hdnFileName.Value);
        //}
        //protected void ViewImgDocR(string ID, string FileName)
        //{
        //    string ActNetImage = "", vPdfFile = "";
        //    string[] ActNetPath = DocumentBucketURL.Split(',');
        //    for (int j = 0; j <= ActNetPath.Length - 1; j++)
        //    {
        //        ActNetImage = ActNetPath[j] + ID + "_" + FileName;
        //        if (ValidUrlChk(ActNetImage))
        //        {
        //            vPdfFile = ActNetImage;
        //            break;
        //        }
        //    }
        //    if (vPdfFile != "")
        //    {
        //        WebClient cln = null;
        //        cln = new WebClient();
        //        byte[] vDoc = null;
        //        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
        //        cln = new WebClient();
        //        vDoc = cln.DownloadData(vPdfFile);
        //        Response.AddHeader("Content-Type", "Application/octet-stream");
        //        Response.AddHeader("Content-Disposition", "attachment;   filename=" + ID + "_" + FileName);
        //        Response.BinaryWrite(vDoc);
        //        Response.Flush();
        //        Response.End();
        //    }
        //    else
        //    {
        //        gblFuction.MsgPopup("No File Found");
        //    }

        //}

        protected void btnKYCDocmnt_Click(object sender, EventArgs e)
        {

            Button btn = sender as Button;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lbFile = (Label)gvrow.FindControl("lbFile");
            HiddenField hdnDocImage = (HiddenField)gvrow.FindControl("hdnDocImage");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUp();", true);
            string vBase64String = "";
            //string vPdfFile = DocumentBucketURL + hdnLoanAppNo.Value + "_" + lbFile.Text;
            if (lbFile.Text != "")
            {
                if (hdnDocImage.Value != "")
                {
                    ImgDocumentImage.ImageUrl = "data:image;base64," + hdnDocImage.Value;
                }
                else if (lbFile.Text.ToLower().Contains(".pdf"))
                {
                    string ActNetImage = "", vPdfFile = "";
                    string[] ActNetPath = DocumentBucketURL.Split(',');
                    for (int j = 0; j <= ActNetPath.Length - 1; j++)
                    {
                        ActNetImage = ActNetPath[j] + hdLeadId.Value + "_" + lbFile.Text;
                        if (ValidUrlChk(ActNetImage))
                        {
                            vPdfFile = ActNetImage;
                            break;
                        }
                    }
                    if (vPdfFile != "")
                    {
                        WebClient cln = null;
                        cln = new WebClient();
                        byte[] vDoc = null;
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                        cln = new WebClient();
                        vDoc = cln.DownloadData(vPdfFile);
                        Response.AddHeader("Content-Type", "Application/octet-stream");
                        Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdLeadId.Value + "_" + lbFile.Text);
                        Response.BinaryWrite(vDoc);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("No File Found");

                    }
                }
                else
                {
                    vBase64String = GetBase64Image(lbFile.Text, hdLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
            }
        }

        protected void btnKYCDoc_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lbFile = (Label)gvrow.FindControl("lbFile");
            HiddenField hdnDocImage = (HiddenField)gvrow.FindControl("hdnDocImage");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUp();", true);
            string vBase64String = "";
            //string vPdfFile = DocumentBucketURL + hdnLoanAppNo.Value + "_" + lbFile.Text ;
            if (lbFile.Text != "")
            {
                if (hdnDocImage.Value != "")
                {
                    ImgDocumentImage.ImageUrl = "data:image;base64," + hdnDocImage.Value;
                }
                else if (lbFile.Text.ToLower().Contains(".pdf"))
                {
                    string ActNetImage = "", vPdfFile = "";
                    string[] ActNetPath = DocumentBucketURL.Split(',');
                    for (int j = 0; j <= ActNetPath.Length - 1; j++)
                    {
                        ActNetImage = ActNetPath[j] + hdLeadId.Value + "_" + lbFile.Text;
                        if (ValidUrlChk(ActNetImage))
                        {
                            vPdfFile = ActNetImage;
                            break;
                        }
                    }
                    if (vPdfFile != "")
                    {
                        WebClient cln = null;
                        cln = new WebClient();
                        byte[] vDoc = null;
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                        cln = new WebClient();
                        vDoc = cln.DownloadData(vPdfFile);
                        Response.AddHeader("Content-Type", "Application/octet-stream");
                        Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdLeadId.Value + "_" + lbFile.Text);
                        Response.BinaryWrite(vDoc);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("No File Found");
                    }
                }
                else
                {
                    vBase64String = GetBase64Image(lbFile.Text, hdLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
            }
        }

        protected void btnKYCDocV_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lbFile = (Label)gvrow.FindControl("lbFile");
            HiddenField hdnDocImage = (HiddenField)gvrow.FindControl("hdnDocImage");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUp();", true);
            string vBase64String = "";
            //string vPdfFile = DocumentBucketURL + hdnLoanAppNo.Value + "_" + lbFile.Text;
            if (lbFile.Text != "")
            {
                if (hdnDocImage.Value != "")
                {
                    ImgDocumentImage.ImageUrl = "data:image;base64," + hdnDocImage.Value;
                }
                else if (lbFile.Text.ToLower().Contains(".pdf"))
                {
                    string ActNetImage = "", vPdfFile = "";
                    string[] ActNetPath = DocumentBucketURL.Split(',');
                    for (int j = 0; j <= ActNetPath.Length - 1; j++)
                    {
                        ActNetImage = ActNetPath[j] + hdLeadId.Value + "_" + lbFile.Text;
                        if (ValidUrlChk(ActNetImage))
                        {
                            vPdfFile = ActNetImage;
                            break;
                        }
                    }
                    if (vPdfFile != "")
                    {
                        WebClient cln = null;
                        cln = new WebClient();
                        byte[] vDoc = null;
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                        cln = new WebClient();
                        vDoc = cln.DownloadData(vPdfFile);
                        Response.AddHeader("Content-Type", "Application/octet-stream");
                        Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdLeadId.Value + "_" + lbFile.Text);
                        Response.BinaryWrite(vDoc);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("No File Found");
                    }
                }
                else
                {
                    vBase64String = GetBase64Image(lbFile.Text, hdLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
            }
        }

        protected void btnKYCDocOth_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lbFile = (Label)gvrow.FindControl("lbFile");
            HiddenField hdnDocImage = (HiddenField)gvrow.FindControl("hdnDocImage");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUp();", true);
            string vBase64String = "";
            //string vPdfFile = DocumentBucketURL + hdnLoanAppNo.Value + "_" + lbFile.Text;
            if (lbFile.Text != "")
            {
                if (hdnDocImage.Value != "")
                {
                    ImgDocumentImage.ImageUrl = "data:image;base64," + hdnDocImage.Value;
                }
                else if (lbFile.Text.ToLower().Contains(".pdf"))
                {
                    string ActNetImage = "", vPdfFile = "";
                    string[] ActNetPath = DocumentBucketURL.Split(',');
                    for (int j = 0; j <= ActNetPath.Length - 1; j++)
                    {
                        ActNetImage = ActNetPath[j] + hdLeadId.Value + "_" + lbFile.Text;
                        if (ValidUrlChk(ActNetImage))
                        {
                            vPdfFile = ActNetImage;
                            break;
                        }
                    }
                    if (vPdfFile != "")
                    {
                        WebClient cln = null;
                        cln = new WebClient();
                        byte[] vDoc = null;
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                        cln = new WebClient();
                        vDoc = cln.DownloadData(vPdfFile);
                        Response.AddHeader("Content-Type", "Application/octet-stream");
                        Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdLeadId.Value + "_" + lbFile.Text);
                        Response.BinaryWrite(vDoc);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("No File Found");
                    }
                }
                else
                {
                    vBase64String = GetBase64Image(lbFile.Text, hdLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
            }
        }

        protected void ViewImgDoc(string ID, string FileName)
        {
            string vBase64String = "";
            string vPdfFile = DocumentBucketURL + ID + "_" + FileName;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUp();", true);
            if (FileName.Contains(".pdf"))
            {
                WebClient cln = null;
                cln = new WebClient();
                byte[] vDoc = null;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                cln = new WebClient();
                vDoc = cln.DownloadData(vPdfFile);
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Disposition", "attachment;   filename=" + ID + "_" + FileName);
                Response.BinaryWrite(vDoc);
                Response.Flush();
                Response.End();
            }
            else
            {
                vBase64String = GetBase64Image(FileName, ID);
                ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
            }
        }

        protected void imgBusiAddrOwnship_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnBusiAddrOwnshipExt.Value);
        }

        protected void imgBKYC2_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnBKYC2Ext.Value);
        }

        protected void imgUdyamRegnCert_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnUdyamRegnCertExt.Value);
        }

        protected void btnEmpDoc_Click(object sender, EventArgs e)
        {

            string ActNetImage = "", vPdfFile = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdLeadId.Value + "_" + lblFileName.Text;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                WebClient cln = null;
                cln = new WebClient();
                byte[] vDoc = null;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                cln = new WebClient();
                vDoc = cln.DownloadData(vPdfFile);
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdLeadId.Value + "_" + lblFileName.Text);
                Response.BinaryWrite(vDoc);
                Response.Flush();
                Response.End();
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }
        }

        protected void imgMarMon_Click(object sender, EventArgs e)
        {
            ViewImgDocLTV(hdLeadId.Value, hdnMarMon.Value);
        }
        protected void imgMarMonBank_Click(object sender, EventArgs e)
        {
            ViewImgDocLTV(hdLeadId.Value, hdnMarMonBank.Value);
        }
        protected void imgQuotCopy_Click(object sender, EventArgs e)
        {
            ViewImgDocLTV(hdLeadId.Value, hdnQuotCopy.Value);
        }

        protected void ViewImgDocLTV(string ID, string FileName)
        {
            string vBase64String = "";


            string ActNetImage = "", vPdfFile = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + ID + "_" + FileName;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                WebClient cln = null;
                cln = new WebClient();
                byte[] vDoc = null;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                cln = new WebClient();
                vDoc = cln.DownloadData(vPdfFile);
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Disposition", "attachment;   filename=" + ID + "_" + FileName);
                Response.BinaryWrite(vDoc);
                Response.Flush();
                Response.End();
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }

        #region imgDoc_Click
        protected void imgDoc_Click(object sender, EventArgs e)
        {

            GridViewRow gvRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            Label lblSrlNo = (Label)gvRow.FindControl("lblSrlNo");
            Label lbFile = (Label)gvRow.FindControl("lblBankStatemenFN");

            ViewImgDoc(hdLeadId.Value, lblSrlNo.Text, lbFile.Text);

        }
        #endregion

        #region imgDocB_Click
        protected void imgDocB_Click(object sender, EventArgs e)
        {

            GridViewRow gvRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            Label lblSrlNo = (Label)gvRow.FindControl("lblSrlNo");
            Label lbFile = (Label)gvRow.FindControl("lblBsaOutFN");

            ViewImgDoc(hdLeadId.Value, lblSrlNo.Text, lbFile.Text);
        }
        #endregion

        protected void ViewImgDoc(string ID, string lblSrlNo, string FileName)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + ID + "_" + lblSrlNo + "_" + FileName;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image(lblSrlNo + "_" + FileName, ID);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + ID + "_" + lblSrlNo + "_" + FileName);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }
        protected void btnEbillDoc_Click(object sender, EventArgs e)
        {

            string ActNetImage = "", vPdfFile = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdLeadId.Value + "_" + lblEBillName.Text;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                WebClient cln = null;
                cln = new WebClient();
                byte[] vDoc = null;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                cln = new WebClient();
                vDoc = cln.DownloadData(vPdfFile);
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdLeadId.Value + "_" + lblEBillName.Text);
                Response.BinaryWrite(vDoc);
                Response.Flush();
                Response.End();
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }
        }

        protected void imgApp_Click(object sender, EventArgs e)
        {
            ViewObImgDoc(hdLeadId.Value, hdnAppFileNm.Value);
        }
        protected void imgCoApp1_Click(object sender, EventArgs e)
        {
            ViewObImgDoc(hdLeadId.Value, hdnCoAppFileName.Value);
        }
        protected void imgCoApp2_Click(object sender, EventArgs e)
        {
            ViewObImgDoc(hdLeadId.Value, hdnCoApp2FileName.Value);
        }
        protected void imgCoApp3_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnCoApp3FileName.Value);
        }
        protected void imgCoApp4_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnCoApp4FileName.Value);
        }
        protected void imgGua_Click(object sender, EventArgs e)
        {
            ViewObImgDoc(hdLeadId.Value, hdnGuaFileName.Value);
        }

        protected void ViewObImgDoc(string ID, string FileName)
        {

            string ActNetImage = "", vPdfFile = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + ID + "_" + FileName;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                WebClient cln = null;
                cln = new WebClient();
                byte[] vDoc = null;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                cln = new WebClient();
                vDoc = cln.DownloadData(vPdfFile);
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Disposition", "attachment;   filename=" + ID + "_" + FileName);
                Response.BinaryWrite(vDoc);
                Response.Flush();
                Response.End();
            }
            else
            {
                gblFuction.AjxMsgPopup("No File Found");
            }
        }

        public string GetBase64Image(string pImageName, string pLeadId)
        {
            string ActNetImage = "", base64image = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = DocumentBucketURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + pLeadId + "_" + pImageName;
                    if (ValidUrlChk(ActNetImage))
                    {
                        imgByte = DownloadByteImage(ActNetImage);
                        base64image = Convert.ToBase64String(imgByte);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return base64image;
        }
        protected void ViewImgDoc(string ID, string FileName, string CustType, string Ext)
        {
            string vBase64String = "";
            string vPdfFile = DocumentBucketURL + ID + "_" + CustType + "_" + FileName + Ext;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUp();", true);
            if (Ext.Contains(".pdf"))
            {
                WebClient cln = null;
                cln = new WebClient();
                byte[] vDoc = null;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                cln = new WebClient();
                vDoc = cln.DownloadData(vPdfFile);
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Disposition", "attachment;   filename=" + ID + "_" + CustType + "_" + FileName + Ext);
                Response.BinaryWrite(vDoc);
                Response.Flush();
                Response.End();
            }
            else
            {
                vBase64String = GetBase64Image(CustType + "_" + FileName + Ext, ID);
                ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
            }
        }

        protected void ImgOwnshipPhoto_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "ApplOwnship", "A", hdnAppOwnshipExt.Value);
        }

        protected void imgCoApp1OwnProof_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "OwnshipProof", "CA1", hdnCoApp1OwnShipExt.Value);
        }

        protected void imgCoApp2OwnProof_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "OwnshipProof", "CA2", hdnCoApp2OwnShipExt.Value);
        }
        protected void imgCoApp3OwnProof_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "OwnshipProof", "CA3", hdnCoApp3OwnShipExt.Value);
        }
        protected void imgCoApp4OwnProof_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "OwnshipProof", "CA4", hdnCoApp4OwnShipExt.Value);
        }

        protected void imgGuarOwnProof_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "OwnshipProof", "G", hdnGuarOwnShipExt.Value);
        }

        protected void ImgElecBill_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_A_" + "ElectricBill" + hdnElecBillExt.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("A_" + "ElectricBill" + hdnElecBillExt.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "A_" + "ElectricBill" + hdnElecBillExt.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }

        protected void imgFcuReport_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_A_" + "FCUReport" + hdnFcuReportExt.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("A_" + "FCUReport" + hdnFcuReportExt.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "A_" + "FCUReport" + hdnFcuReportExt.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }


        }

        protected void imgFcuReportCA1_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_CA1_" + "FCUReport" + hdnFcuReportExtCA1.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("CA1_" + "FCUReport" + hdnFcuReportExtCA1.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "CA1_" + "FCUReport" + hdnFcuReportExtCA1.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }


        }
        protected void ImgElecBillCA1_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_CA1_" + "ElectricBill" + hdnElecBillExtCA1.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("CA1_" + "ElectricBill" + hdnElecBillExtCA1.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "CA1_" + "ElectricBill" + hdnElecBillExtCA1.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }

        protected void imgFcuReportCA2_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_CA2_" + "FCUReport" + hdnFcuReportExtCA2.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("CA2_" + "FCUReport" + hdnFcuReportExtCA2.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "CA2_" + "FCUReport" + hdnFcuReportExtCA2.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }
        protected void ImgElecBillCA2_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_CA2_" + "ElectricBill" + hdnElecBillExtCA2.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("CA2_" + "ElectricBill" + hdnElecBillExtCA2.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "CA2_" + "ElectricBill" + hdnElecBillExtCA2.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }
        }

        protected void imgFcuReportCA3_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_CA3_" + "FCUReport" + hdnFcuReportExtCA3.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("CA3_" + "FCUReport" + hdnFcuReportExtCA3.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "CA3_" + "FCUReport" + hdnFcuReportExtCA3.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }
        protected void ImgElecBillCA3_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_CA3_" + "ElectricBill" + hdnElecBillExtCA3.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("CA3_" + "ElectricBill" + hdnElecBillExtCA3.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "CA3_" + "ElectricBill" + hdnElecBillExtCA3.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }
        }

        protected void imgFcuReportCA4_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_CA4_" + "FCUReport" + hdnFcuReportExtCA4.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("CA4_" + "FCUReport" + hdnFcuReportExtCA4.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "CA4_" + "FCUReport" + hdnFcuReportExtCA4.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }
        protected void ImgElecBillCA4_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_CA4_" + "ElectricBill" + hdnElecBillExtCA4.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("CA4_" + "ElectricBill" + hdnElecBillExtCA4.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "CA4_" + "ElectricBill" + hdnElecBillExtCA4.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }
        }

        protected void imgFcuReportG_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_G_" + "FCUReport" + hdnFcuReportExtG.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("G_" + "FCUReport" + hdnFcuReportExtG.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "G_" + "FCUReport" + hdnFcuReportExtG.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }
        protected void ImgElecBillG_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_G_" + "ElectricBill" + hdnElecBillExtG.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("G_" + "ElectricBill" + hdnElecBillExtG.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "G_" + "ElectricBill" + hdnElecBillExtG.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }

        public bool ValidUrlChk(string pUrl)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pUrl);
                request.Timeout = 5000;
                using (WebResponse response = request.GetResponse())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        #region URLToByte
        public byte[] DownloadByteImage(string URL)
        {
            byte[] imgByte = null;
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var wc = new WebClient())
                {
                    imgByte = wc.DownloadData(URL);
                }
            }
            finally { }
            return imgByte;
        }
        #endregion

        #region ConvertFileToByteArray
        private byte[] ConvertFileToByteArray(HttpPostedFile postedFile)
        {
            using (Stream stream = postedFile.InputStream)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
        #endregion

        #endregion

        private void GenerateObligationGrid()
        {
            DataSet ds = null;
            DataTable dt = null;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                ds = oMem.GenerateObligationGrid(Convert.ToInt64(hdLeadId.Value));
                dt = ds.Tables[0];
                //DataRow dF;
                //dF = dt.NewRow();
                //dt.Rows.Add(dF);
                //dt.AcceptChanges();
                ViewState["Obligation"] = dt;
                gvObli.DataSource = dt;
                gvObli.DataBind();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        double TotalEMI = 0; lblTotalEMI.Text = "";
                        foreach (GridViewRow gr in gvObli.Rows)
                        {
                            DropDownList ddlFoir = (DropDownList)gvObli.Rows[gr.RowIndex].FindControl("ddlFoir");


                            TextBox txtEMI = (TextBox)gvObli.Rows[gr.RowIndex].FindControl("txtEMI");

                            if (ddlFoir.SelectedValue == "Y")
                            {
                                TotalEMI += Convert.ToDouble(txtEMI.Text);
                            }
                        }
                        lblTotalEMI.Text = Convert.ToString(TotalEMI);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void DynamicTableRow(int AssMthId)
        {
            switch (AssMthId)
            {
                case 1:
                    trHIncDtlFiP.Visible = false;
                    tr1IncDtlFiP.Visible = false;
                    tr2IncDtlFiP.Visible = false;
                    tr3IncDtlFiP.Visible = false;

                    trHIncDtlCommon.Visible = false;
                    tr1IncDtlCommon.Visible = false;
                    tr2IncDtlCommon.Visible = false;

                    tr1IncDtlMonthBS.Visible = false;
                    tr2IncDtlMonthBS.Visible = false;

                    break;
                case 2:
                    trHIncDtlFiP.Visible = false;
                    tr1IncDtlFiP.Visible = false;
                    tr2IncDtlFiP.Visible = false;
                    tr3IncDtlFiP.Visible = false;

                    trHIncDtlCommon.Visible = false;
                    tr1IncDtlCommon.Visible = false;
                    tr2IncDtlCommon.Visible = false;

                    tr1IncDtlMonthBS.Visible = true;
                    tr2IncDtlMonthBS.Visible = true;

                    break;
                case 3:
                    trHIncDtlFiP.Visible = false;
                    tr1IncDtlFiP.Visible = false;
                    tr2IncDtlFiP.Visible = false;
                    tr3IncDtlFiP.Visible = false;

                    trHIncDtlCommon.Visible = true;
                    tr1IncDtlCommon.Visible = true;
                    tr2IncDtlCommon.Visible = true;

                    tr1IncDtlMonthBS.Visible = false;
                    tr2IncDtlMonthBS.Visible = false;

                    break;
                case 4:
                    trHIncDtlFiP.Visible = true;
                    tr1IncDtlFiP.Visible = true;
                    tr2IncDtlFiP.Visible = true;
                    tr3IncDtlFiP.Visible = true;

                    trHIncDtlCommon.Visible = false;
                    tr1IncDtlCommon.Visible = false;
                    tr2IncDtlCommon.Visible = false;

                    tr1IncDtlMonthBS.Visible = false;
                    tr2IncDtlMonthBS.Visible = false;
                    break;
                case 5:

                    trHIncDtlFiP.Visible = false;
                    tr1IncDtlFiP.Visible = false;
                    tr2IncDtlFiP.Visible = false;
                    tr3IncDtlFiP.Visible = false;

                    trHIncDtlCommon.Visible = true;
                    tr1IncDtlCommon.Visible = true;
                    tr2IncDtlCommon.Visible = true;

                    tr1IncDtlMonthBS.Visible = false;
                    tr2IncDtlMonthBS.Visible = false;

                    break;
                case 6:
                    trHIncDtlFiP.Visible = false;
                    tr1IncDtlFiP.Visible = false;
                    tr2IncDtlFiP.Visible = false;
                    tr3IncDtlFiP.Visible = false;

                    trHIncDtlCommon.Visible = true;
                    tr1IncDtlCommon.Visible = true;
                    tr2IncDtlCommon.Visible = true;

                    tr1IncDtlMonthBS.Visible = false;
                    tr2IncDtlMonthBS.Visible = false;

                    break;
            }

        }

        private void ShowEmpRecordsDocUpld()
        {

            DataTable dt = null;
            CApplication oMem = null;
            try
            {
                oMem = new CApplication();
                dt = oMem.CF_GetDocByLeadId(Convert.ToInt64(hdLeadId.Value));
                if (dt.Rows.Count > 0)
                {
                    gvEmpDocument.DataSource = dt;
                    gvEmpDocument.DataBind();

                }
                else
                {
                    LoadEmpDocUpLd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        private void LoadEmpDocUpLd()
        {
            ViewState["KYCDoc"] = null;
            DataTable dt = new DataTable("Table1");
            dt.Columns.Add("SLNo");
            dt.Columns.Add("DocTypeId");
            dt.Columns.Add("DocType");
            dt.Columns.Add("DocRemarks");
            dt.Columns.Add("DocPassword");
            dt.Columns.Add("Doc_Image", typeof(byte[]));
            dt.Columns.Add("ImageBase64");
            dt.Columns.Add("UploadYN");
            dt.AcceptChanges();
            gvEmpDocument.DataSource = dt;
            gvEmpDocument.DataBind();
        }
        private void ShowEmpRecordsOth()
        {
            string vLoanAppId = "";
            DataTable dt = null;
            CApplication oMem = null;
            try
            {

                oMem = new CApplication();
                dt = oMem.CF_OthrRepDtlByLeadId(Convert.ToInt64(hdLeadId.Value));
                if (dt.Rows.Count > 0)
                {
                    gvEmpDocOth.DataSource = dt;
                    gvEmpDocOth.DataBind();
                }
                else
                {
                    LoadEmpDocOth();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        private void LoadEmpDocOth()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SLNo");
            dt.Columns.Add("DocTypeId");
            dt.Columns.Add("DocType");
            dt.Columns.Add("DocRemarks");
            dt.Columns.Add("DocPassword");
            dt.Columns.Add("Doc_Image", typeof(byte[]));
            dt.Columns.Add("ImageBase64");
            dt.Columns.Add("UploadYN");
            dt.AcceptChanges();
            gvEmpDocOth.DataSource = dt;
            gvEmpDocOth.DataBind();
        }


        private void ShowEmpRecordsV()
        {
            string vLoanAppId = "";
            DataTable dt = null;
            CApplication oMem = null;
            try
            {
                vLoanAppId = hdnLoanAppNo.Value;


                oMem = new CApplication();
                dt = oMem.CF_BCRepDtlByLeadId(Convert.ToInt64(hdLeadId.Value));
                if (dt.Rows.Count > 0)
                {
                    gvEmpDocV.DataSource = dt;
                    gvEmpDocV.DataBind();
                    ViewState["LoanAppDoc"] = dt;

                }
                else
                {
                    LoadEmpDocV();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        private void LoadEmpDocV()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("SLNo");
            dt.Columns.Add("DocTypeId");
            dt.Columns.Add("DocType");
            dt.Columns.Add("DocRemarks");
            dt.Columns.Add("DocPassword");
            dt.Columns.Add("Doc_Image", typeof(byte[]));
            dt.Columns.Add("ImageBase64");
            dt.Columns.Add("UploadYN");
            dt.Columns.Add("DocValue");
            dt.AcceptChanges();
            gvEmpDocV.DataSource = dt;
            gvEmpDocV.DataBind();

        }

        private void ShowEmpRecords()
        {

            DataTable dt = null;
            CApplication oMem = null;
            try
            {

                oMem = new CApplication();
                dt = oMem.CF_ExtrnlChkDtlByLeadId(Convert.ToInt64(hdLeadId.Value));
                if (dt.Rows.Count > 0)
                {
                    gvEmpDoc.DataSource = dt;
                    gvEmpDoc.DataBind();
                    ViewState["LoanAppDoc"] = dt;
                    ddlTrdRfChk.SelectedIndex = ddlTrdRfChk.Items.IndexOf(ddlTrdRfChk.Items.FindByValue(dt.Rows[0]["TrdChkId"].ToString()));
                }
                else
                {
                    LoadEmpDoc();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        private void LoadEmpDoc()
        {
            DataTable dt = new DataTable("Table1");
            dt.Columns.Add("SLNo");
            dt.Columns.Add("DocTypeId");
            dt.Columns.Add("DocType");
            dt.Columns.Add("DocRemarks");
            dt.Columns.Add("DocPassword");
            dt.Columns.Add("Doc_Image", typeof(byte[]));
            dt.Columns.Add("ImageBase64");
            dt.Columns.Add("UploadYN");
            dt.AcceptChanges();
            gvEmpDoc.DataSource = dt;
            gvEmpDoc.DataBind();
        }

        protected void gvBankDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Bank"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlAcType = (DropDownList)e.Row.FindControl("ddlAcType");
                ddlAcType.SelectedIndex = ddlAcType.Items.IndexOf(ddlAcType.Items.FindByValue(e.Row.Cells[12].Text));

            }
        }

        protected void gvEmpDoc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int i = e.Row.RowIndex;
                DataTable dtIns = null;
                CApplication oApp = null;
                try
                {
                    oApp = new CApplication();
                    FileUpload FUKYcDoc = (FileUpload)e.Row.FindControl("FUKYcDoc");
                    DropDownList ddlDocType = (DropDownList)e.Row.FindControl("ddlDocType");
                    TextBox txtDocRemarks = (TextBox)e.Row.FindControl("txtDocRemarks");
                    TextBox txtDocPassword = (TextBox)e.Row.FindControl("txtDocPassword");
                    HiddenField hdnDocPassword = (HiddenField)e.Row.FindControl("hdnDocPassword");
                    Button btnKYCDoc = (Button)e.Row.FindControl("btnKYCDoc");
                    FUKYcDoc.Enabled = false;

                    ddlDocType.Items.Clear();
                    dtIns = oApp.GetDocumentMst("EXTCHK");
                    if (dtIns.Rows.Count > 0)
                    {
                        ddlDocType.DataSource = dtIns;
                        ddlDocType.DataTextField = "DocName";
                        ddlDocType.DataValueField = "DocID";
                        ddlDocType.DataBind();
                    }


                    ddlDocType.SelectedIndex = ddlDocType.Items.IndexOf(ddlDocType.Items.FindByValue(e.Row.Cells[7].Text.Trim()));

                    txtDocPassword.TextMode = TextBoxMode.Password;
                    txtDocPassword.Attributes.Add("value", hdnDocPassword.Value);

                }
                finally
                {

                }
            }
        }

        protected void gvEmpDocV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int i = e.Row.RowIndex;
                DataTable dtIns = null;
                CApplication oApp = null;
                try
                {
                    oApp = new CApplication();
                    FileUpload FUKYcDoc = (FileUpload)e.Row.FindControl("FUKYcDoc");
                    DropDownList ddlDocType = (DropDownList)e.Row.FindControl("ddlDocType");
                    TextBox txtDocRemarks = (TextBox)e.Row.FindControl("txtDocRemarks");
                    TextBox txtDocPassword = (TextBox)e.Row.FindControl("txtDocPassword");
                    HiddenField hdnDocPassword = (HiddenField)e.Row.FindControl("hdnDocPassword");
                    DropDownList ddlValue = (DropDownList)e.Row.FindControl("ddlValue");
                    Button btnKYCDoc = (Button)e.Row.FindControl("btnKYCDoc");

                    ddlDocType.Items.Clear();
                    dtIns = oApp.GetDocumentMst("BCRep");
                    if (dtIns.Rows.Count > 0)
                    {
                        ddlDocType.DataSource = dtIns;
                        ddlDocType.DataTextField = "DocName";
                        ddlDocType.DataValueField = "DocID";
                        ddlDocType.DataBind();
                    }

                    ddlDocType.SelectedIndex = ddlDocType.Items.IndexOf(ddlDocType.Items.FindByValue(e.Row.Cells[8].Text.Trim()));
                    ddlValue.SelectedIndex = ddlValue.Items.IndexOf(ddlValue.Items.FindByValue(e.Row.Cells[10].Text.Trim()));
                    txtDocPassword.TextMode = TextBoxMode.Password;
                    txtDocPassword.Attributes.Add("value", hdnDocPassword.Value);

                }
                finally
                {

                }
            }
        }

        protected void gvEmpDocOth_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int i = e.Row.RowIndex;
                DataTable dtIns = null;
                CApplication oApp = null;
                try
                {
                    oApp = new CApplication();
                    FileUpload FUKYcDoc = (FileUpload)e.Row.FindControl("FUKYcDoc");
                    DropDownList ddlDocType = (DropDownList)e.Row.FindControl("ddlDocType");
                    TextBox txtDocRemarks = (TextBox)e.Row.FindControl("txtDocRemarks");
                    TextBox txtDocPassword = (TextBox)e.Row.FindControl("txtDocPassword");
                    HiddenField hdnDocPassword = (HiddenField)e.Row.FindControl("hdnDocPassword");
                    Button btnKYCDoc = (Button)e.Row.FindControl("btnKYCDoc");

                    ddlDocType.Items.Clear();
                    dtIns = oApp.GetDocumentMst("OthRep");
                    if (dtIns.Rows.Count > 0)
                    {
                        ddlDocType.DataSource = dtIns;
                        ddlDocType.DataTextField = "DocName";
                        ddlDocType.DataValueField = "DocID";
                        ddlDocType.DataBind();
                    }

                    ddlDocType.SelectedIndex = ddlDocType.Items.IndexOf(ddlDocType.Items.FindByValue(e.Row.Cells[7].Text.Trim()));
                    txtDocPassword.TextMode = TextBoxMode.Password;
                    txtDocPassword.Attributes.Add("value", hdnDocPassword.Value);
                }
                finally
                {

                }
            }
        }

        #region PopulateAllDropDown
        private void PopBranch()
        {
            DataTable dt = null;
            CUser oUsr = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));
                if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (Convert.ToString(row["BranchCode"]) != Convert.ToString(Session[gblValue.BrnchCode]))
                        {
                            row.Delete();
                        }
                    }
                    dt.AcceptChanges();
                }
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
                if (vBrCode == "0000")
                {
                    ListItem oli = new ListItem("<--All-->", "A");
                    ddlBranch.Items.Insert(0, oli);
                }
            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }

        private void PopSolarSystemType()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetSolarPwrSystem();

                ddlSolPwrSys.DataSource = dt;
                ddlSolPwrSys.DataTextField = "SystemType";
                ddlSolPwrSys.DataValueField = "SysID";
                ddlSolPwrSys.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlSolPwrSys.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }
        private void PopAssesMethod1()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetAssesMethod(0);

                ddlAssMethodID.DataSource = dt;
                ddlAssMethodID.DataTextField = "MethodName";
                ddlAssMethodID.DataValueField = "MethodID";
                ddlAssMethodID.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlAssMethodID.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }
        private void PopAssesMethod()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetAssesMethod(Convert.ToInt32(ddlSegType.SelectedValue));

                ddlAssMethod.DataSource = dt;
                ddlAssMethod.DataTextField = "MethodName";
                ddlAssMethod.DataValueField = "MethodID";
                ddlAssMethod.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlAssMethod.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }

        private void PopSegType()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetSegType();

                ddlSegType.DataSource = dt;
                ddlSegType.DataTextField = "SegType";
                ddlSegType.DataValueField = "SegID";
                ddlSegType.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlSegType.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }

        private void PopAppliEntType()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetAppliEntTypeMst();

                ddlEntityType.DataSource = dt;
                ddlEntityType.DataTextField = "AppEntType";
                ddlEntityType.DataValueField = "AppEntID";
                ddlEntityType.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlEntityType.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }

        private void PopEPCMst()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetEPCMst();

                ddlEPC.DataSource = dt;
                ddlEPC.DataTextField = "EpcName";
                ddlEPC.DataValueField = "EpcID";
                ddlEPC.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlEPC.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }

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
                ddlBcRM.DataSource = dt;
                ddlBcRM.DataTextField = "EoName";
                ddlBcRM.DataValueField = "Eoid";
                ddlBcRM.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBcRM.Items.Insert(0, oli);


            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        private void PopBCPropNo()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ddlBCPropNo.Items.Clear();
                oRO = new CEO();
                dt = oRO.CF_GetBCPropNoByBranch(vBrCode);
                ddlBCPropNo.DataSource = dt;
                ddlBCPropNo.DataTextField = "BCPropNo";
                ddlBCPropNo.DataValueField = "LeadID";
                ddlBCPropNo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBCPropNo.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }
        private void popState()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                dt = oMem.CF_GetState();

                ddlBCStste.DataSource = dt;
                ddlBCStste.DataTextField = "StateName";
                ddlBCStste.DataValueField = "StateId";
                ddlBCStste.DataBind();
                ddlBCStste.Items.Insert(0, oli1);


            }
            finally
            {
            }
        }

        private void PopLeadList()
        {
            string vBrCode = "";
            DataTable dt = new DataTable();
            CCust360 oMem = new CCust360();
            if (Session[gblValue.BrnchCode] != null)
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
            }
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                dt = oMem.CF_GetLeadFromBasicDtl(vBrCode);

                ddlCustomer.DataSource = dt;
                ddlCustomer.DataTextField = "BCPropNo";
                ddlCustomer.DataValueField = "LeadID";
                ddlCustomer.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        private void popCaste()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");
                dt = oMem.CF_GetCaste();

                ddlApplCast.DataSource = dt;
                ddlApplCast.DataTextField = "Caste";
                ddlApplCast.DataValueField = "CasteId";
                ddlApplCast.DataBind();
                ddlApplCast.Items.Insert(0, oli1);

                ddlCoApp1Caste.DataSource = dt;
                ddlCoApp1Caste.DataTextField = "Caste";
                ddlCoApp1Caste.DataValueField = "CasteId";
                ddlCoApp1Caste.DataBind();
                ddlCoApp1Caste.Items.Insert(0, oli2);

                ddlCoApp2Caste.DataSource = dt;
                ddlCoApp2Caste.DataTextField = "Caste";
                ddlCoApp2Caste.DataValueField = "CasteId";
                ddlCoApp2Caste.DataBind();
                ddlCoApp2Caste.Items.Insert(0, oli3);

                ddlCoApp3Caste.DataSource = dt;
                ddlCoApp3Caste.DataTextField = "Caste";
                ddlCoApp3Caste.DataValueField = "CasteId";
                ddlCoApp3Caste.DataBind();
                ddlCoApp3Caste.Items.Insert(0, oli5);

                ddlCoApp4Caste.DataSource = dt;
                ddlCoApp4Caste.DataTextField = "Caste";
                ddlCoApp4Caste.DataValueField = "CasteId";
                ddlCoApp4Caste.DataBind();
                ddlCoApp4Caste.Items.Insert(0, oli6);

                ddlGuarCaste.DataSource = dt;
                ddlGuarCaste.DataTextField = "Caste";
                ddlGuarCaste.DataValueField = "CasteId";
                ddlGuarCaste.DataBind();
                ddlGuarCaste.Items.Insert(0, oli4);
            }
            finally
            {
            }
        }

        private void PopReligion()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");

                dt = oMem.CF_GetReligion();
                ddlApplReligion.DataSource = dt;
                ddlApplReligion.DataTextField = "Religion";
                ddlApplReligion.DataValueField = "ReligionId";
                ddlApplReligion.DataBind();
                ddlApplReligion.Items.Insert(0, oli1);

                ddlCoApp1Religion.DataSource = dt;
                ddlCoApp1Religion.DataTextField = "Religion";
                ddlCoApp1Religion.DataValueField = "ReligionId";
                ddlCoApp1Religion.DataBind();
                ddlCoApp1Religion.Items.Insert(0, oli2);

                ddlCoApp2Religion.DataSource = dt;
                ddlCoApp2Religion.DataTextField = "Religion";
                ddlCoApp2Religion.DataValueField = "ReligionId";
                ddlCoApp2Religion.DataBind();
                ddlCoApp2Religion.Items.Insert(0, oli3);

                ddlCoApp3Religion.DataSource = dt;
                ddlCoApp3Religion.DataTextField = "Religion";
                ddlCoApp3Religion.DataValueField = "ReligionId";
                ddlCoApp3Religion.DataBind();
                ddlCoApp3Religion.Items.Insert(0, oli5);

                ddlCoApp4Religion.DataSource = dt;
                ddlCoApp4Religion.DataTextField = "Religion";
                ddlCoApp4Religion.DataValueField = "ReligionId";
                ddlCoApp4Religion.DataBind();
                ddlCoApp4Religion.Items.Insert(0, oli6);

                ddlGuarReligion.DataSource = dt;
                ddlGuarReligion.DataTextField = "Religion";
                ddlGuarReligion.DataValueField = "ReligionId";
                ddlGuarReligion.DataBind();
                ddlGuarReligion.Items.Insert(0, oli4);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        private void PopQualification()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");
                dt = oMem.CF_GetQualification();

                ddlAppEdu.DataSource = dt;
                ddlAppEdu.DataTextField = "QualificationName";
                ddlAppEdu.DataValueField = "QualificationId";
                ddlAppEdu.DataBind();
                ddlAppEdu.Items.Insert(0, oli1);

                ddlCoApp1Edu.DataSource = dt;
                ddlCoApp1Edu.DataTextField = "QualificationName";
                ddlCoApp1Edu.DataValueField = "QualificationId";
                ddlCoApp1Edu.DataBind();
                ddlCoApp1Edu.Items.Insert(0, oli2);

                ddlCoApp2Edu.DataSource = dt;
                ddlCoApp2Edu.DataTextField = "QualificationName";
                ddlCoApp2Edu.DataValueField = "QualificationId";
                ddlCoApp2Edu.DataBind();
                ddlCoApp2Edu.Items.Insert(0, oli3);

                ddlCoApp3Edu.DataSource = dt;
                ddlCoApp3Edu.DataTextField = "QualificationName";
                ddlCoApp3Edu.DataValueField = "QualificationId";
                ddlCoApp3Edu.DataBind();
                ddlCoApp3Edu.Items.Insert(0, oli5);

                ddlCoApp4Edu.DataSource = dt;
                ddlCoApp4Edu.DataTextField = "QualificationName";
                ddlCoApp4Edu.DataValueField = "QualificationId";
                ddlCoApp4Edu.DataBind();
                ddlCoApp4Edu.Items.Insert(0, oli6);

                ddlGuarEdu.DataSource = dt;
                ddlGuarEdu.DataTextField = "QualificationName";
                ddlGuarEdu.DataValueField = "QualificationId";
                ddlGuarEdu.DataBind();
                ddlGuarEdu.Items.Insert(0, oli4);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        private void popStateCust()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");
                ListItem oli7 = new ListItem("<--Select-->", "-1");
                ListItem oli8 = new ListItem("<--Select-->", "-1");
                ListItem oli9 = new ListItem("<--Select-->", "-1");
                ListItem oli10 = new ListItem("<--Select-->", "-1");
                ListItem oli11 = new ListItem("<--Select-->", "-1");
                ListItem oli12 = new ListItem("<--Select-->", "-1");

                dt = oMem.CF_GetState();

                ddlApplState.ClearSelection();
                ddlApplState.DataSource = dt;
                ddlApplState.DataTextField = "StateName";
                ddlApplState.DataValueField = "StateId";
                ddlApplState.DataBind();
                ddlApplState.Items.Insert(0, oli1);

                ddlApplCurrState.ClearSelection();
                ddlApplCurrState.DataSource = dt;
                ddlApplCurrState.DataTextField = "StateName";
                ddlApplCurrState.DataValueField = "StateId";
                ddlApplCurrState.DataBind();
                ddlApplCurrState.Items.Insert(0, oli2);

                ddlCoApp1PerState.ClearSelection();
                ddlCoApp1PerState.DataSource = dt;
                ddlCoApp1PerState.DataTextField = "StateName";
                ddlCoApp1PerState.DataValueField = "StateId";
                ddlCoApp1PerState.DataBind();
                ddlCoApp1PerState.Items.Insert(0, oli3);

                ddlCoApp1CurrState.ClearSelection();
                ddlCoApp1CurrState.DataSource = dt;
                ddlCoApp1CurrState.DataTextField = "StateName";
                ddlCoApp1CurrState.DataValueField = "StateId";
                ddlCoApp1CurrState.DataBind();
                ddlCoApp1CurrState.Items.Insert(0, oli4);

                ddlCoApp2PerState.ClearSelection();
                ddlCoApp2PerState.DataSource = dt;
                ddlCoApp2PerState.DataTextField = "StateName";
                ddlCoApp2PerState.DataValueField = "StateId";
                ddlCoApp2PerState.DataBind();
                ddlCoApp2PerState.Items.Insert(0, oli5);

                ddlCoApp2CurrState.ClearSelection();
                ddlCoApp2CurrState.DataSource = dt;
                ddlCoApp2CurrState.DataTextField = "StateName";
                ddlCoApp2CurrState.DataValueField = "StateId";
                ddlCoApp2CurrState.DataBind();
                ddlCoApp2CurrState.Items.Insert(0, oli6);

                ddlGuarPerState.ClearSelection();
                ddlGuarPerState.DataSource = dt;
                ddlGuarPerState.DataTextField = "StateName";
                ddlGuarPerState.DataValueField = "StateId";
                ddlGuarPerState.DataBind();
                ddlGuarPerState.Items.Insert(0, oli7);

                ddlGuarCurrState.ClearSelection();
                ddlGuarCurrState.DataSource = dt;
                ddlGuarCurrState.DataTextField = "StateName";
                ddlGuarCurrState.DataValueField = "StateId";
                ddlGuarCurrState.DataBind();
                ddlGuarCurrState.Items.Insert(0, oli8);

                ddlCoApp3PerState.DataSource = dt;
                ddlCoApp3PerState.DataTextField = "StateName";
                ddlCoApp3PerState.DataValueField = "StateId";
                ddlCoApp3PerState.DataBind();
                ddlCoApp3PerState.Items.Insert(0, oli9);

                ddlCoApp3CurrState.DataSource = dt;
                ddlCoApp3CurrState.DataTextField = "StateName";
                ddlCoApp3CurrState.DataValueField = "StateId";
                ddlCoApp3CurrState.DataBind();
                ddlCoApp3CurrState.Items.Insert(0, oli10);

                ddlCoApp4PerState.DataSource = dt;
                ddlCoApp4PerState.DataTextField = "StateName";
                ddlCoApp4PerState.DataValueField = "StateId";
                ddlCoApp4PerState.DataBind();
                ddlCoApp4PerState.Items.Insert(0, oli11);

                ddlCoApp4CurrState.DataSource = dt;
                ddlCoApp4CurrState.DataTextField = "StateName";
                ddlCoApp4CurrState.DataValueField = "StateId";
                ddlCoApp4CurrState.DataBind();
                ddlCoApp4CurrState.Items.Insert(0, oli12);
            }
            finally
            {
            }
        }

        private void PopRelation()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");
                dt = oMem.CF_GetRelation();

                ddlRelWithApp.ClearSelection();
                ddlRelWithApp.DataSource = dt;
                ddlRelWithApp.DataTextField = "Relation";
                ddlRelWithApp.DataValueField = "RelationId";
                ddlRelWithApp.DataBind();
                ddlRelWithApp.Items.Insert(0, oli1);

                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    if (dt.Rows[i]["Relation"].ToString() == "Self")
                    {
                        dt.Rows.RemoveAt(i);
                    }
                }

                ddlCoApp1RelWithApp.DataSource = dt;
                ddlCoApp1RelWithApp.DataTextField = "Relation";
                ddlCoApp1RelWithApp.DataValueField = "RelationId";
                ddlCoApp1RelWithApp.DataBind();
                ddlCoApp1RelWithApp.Items.Insert(0, oli2);

                ddlCoApp2RelWithApp.DataSource = dt;
                ddlCoApp2RelWithApp.DataTextField = "Relation";
                ddlCoApp2RelWithApp.DataValueField = "RelationId";
                ddlCoApp2RelWithApp.DataBind();
                ddlCoApp2RelWithApp.Items.Insert(0, oli3);

                ddlGuarRelWithApp.DataSource = dt;
                ddlGuarRelWithApp.DataTextField = "Relation";
                ddlGuarRelWithApp.DataValueField = "RelationId";
                ddlGuarRelWithApp.DataBind();
                ddlGuarRelWithApp.Items.Insert(0, oli4);

                ddlCoApp3RelWithApp.DataSource = dt;
                ddlCoApp3RelWithApp.DataTextField = "Relation";
                ddlCoApp3RelWithApp.DataValueField = "RelationId";
                ddlCoApp3RelWithApp.DataBind();
                ddlCoApp3RelWithApp.Items.Insert(0, oli5);

                ddlCoApp4RelWithApp.DataSource = dt;
                ddlCoApp4RelWithApp.DataTextField = "Relation";
                ddlCoApp4RelWithApp.DataValueField = "RelationId";
                ddlCoApp4RelWithApp.DataBind();
                ddlCoApp4RelWithApp.Items.Insert(0, oli6);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        private void PopDistrictByState(int pStateId, string pTag)
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");
                ListItem oli7 = new ListItem("<--Select-->", "-1");
                ListItem oli8 = new ListItem("<--Select-->", "-1");
                ListItem oli9 = new ListItem("<--Select-->", "-1");
                ListItem oli10 = new ListItem("<--Select-->", "-1");
                ListItem oli11 = new ListItem("<--Select-->", "-1");
                ListItem oli12 = new ListItem("<--Select-->", "-1");
                switch (pTag)
                {
                    case "AppPer":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlApplDist.ClearSelection();
                        ddlApplDist.Items.Insert(0, oli1);
                        ddlApplDist.DataSource = dt;
                        ddlApplDist.DataTextField = "DistrictName";
                        ddlApplDist.DataValueField = "DistrictId";
                        ddlApplDist.DataBind();
                        break;
                    case "AppCurr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlApplCurrDist.ClearSelection();
                        ddlApplCurrDist.Items.Insert(0, oli2);
                        ddlApplCurrDist.DataSource = dt;
                        ddlApplCurrDist.DataTextField = "DistrictName";
                        ddlApplCurrDist.DataValueField = "DistrictId";
                        ddlApplCurrDist.DataBind();
                        break;
                    case "CoApp1Per":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp1PerDist.ClearSelection();
                        ddlCoApp1PerDist.Items.Insert(0, oli3);
                        ddlCoApp1PerDist.DataSource = dt;
                        ddlCoApp1PerDist.DataTextField = "DistrictName";
                        ddlCoApp1PerDist.DataValueField = "DistrictId";
                        ddlCoApp1PerDist.DataBind();
                        break;
                    case "CoApp1Curr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp1CurrDist.ClearSelection();
                        ddlCoApp1CurrDist.Items.Insert(0, oli4);
                        ddlCoApp1CurrDist.DataSource = dt;
                        ddlCoApp1CurrDist.DataTextField = "DistrictName";
                        ddlCoApp1CurrDist.DataValueField = "DistrictId";
                        ddlCoApp1CurrDist.DataBind();
                        break;
                    case "CoApp2Per":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp2PerDist.ClearSelection();
                        ddlCoApp2PerDist.Items.Insert(0, oli5);
                        ddlCoApp2PerDist.DataSource = dt;
                        ddlCoApp2PerDist.DataTextField = "DistrictName";
                        ddlCoApp2PerDist.DataValueField = "DistrictId";
                        ddlCoApp2PerDist.DataBind();
                        break;
                    case "CoApp2Curr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp2CurrDist.ClearSelection();
                        ddlCoApp2CurrDist.Items.Insert(0, oli6);
                        ddlCoApp2CurrDist.DataSource = dt;
                        ddlCoApp2CurrDist.DataTextField = "DistrictName";
                        ddlCoApp2CurrDist.DataValueField = "DistrictId";
                        ddlCoApp2CurrDist.DataBind();
                        break;
                    case "GuarPer":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlGuarPerDist.ClearSelection();
                        ddlGuarPerDist.Items.Insert(0, oli7);
                        ddlGuarPerDist.DataSource = dt;
                        ddlGuarPerDist.DataTextField = "DistrictName";
                        ddlGuarPerDist.DataValueField = "DistrictId";
                        ddlGuarPerDist.DataBind();
                        break;
                    case "GuarCurr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlGuarCurrDist.ClearSelection();
                        ddlGuarCurrDist.Items.Insert(0, oli8);
                        ddlGuarCurrDist.DataSource = dt;
                        ddlGuarCurrDist.DataTextField = "DistrictName";
                        ddlGuarCurrDist.DataValueField = "DistrictId";
                        ddlGuarCurrDist.DataBind();
                        break;
                    case "CoApp3Per":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp3PerDist.Items.Insert(0, oli9);
                        ddlCoApp3PerDist.DataSource = dt;
                        ddlCoApp3PerDist.DataTextField = "DistrictName";
                        ddlCoApp3PerDist.DataValueField = "DistrictId";
                        ddlCoApp3PerDist.DataBind();
                        break;
                    case "CoApp3Curr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp3CurrDist.Items.Insert(0, oli10);
                        ddlCoApp3CurrDist.DataSource = dt;
                        ddlCoApp3CurrDist.DataTextField = "DistrictName";
                        ddlCoApp3CurrDist.DataValueField = "DistrictId";
                        ddlCoApp3CurrDist.DataBind();
                        break;
                    case "CoApp4Per":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp4PerDist.Items.Insert(0, oli11);
                        ddlCoApp4PerDist.DataSource = dt;
                        ddlCoApp4PerDist.DataTextField = "DistrictName";
                        ddlCoApp4PerDist.DataValueField = "DistrictId";
                        ddlCoApp4PerDist.DataBind();
                        break;
                    case "CoApp4Curr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp4CurrDist.Items.Insert(0, oli12);
                        ddlCoApp4CurrDist.DataSource = dt;
                        ddlCoApp4CurrDist.DataTextField = "DistrictName";
                        ddlCoApp4CurrDist.DataValueField = "DistrictId";
                        ddlCoApp4CurrDist.DataBind();
                        break;
                }
            }
            finally
            {
            }
        }

        #endregion

        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    break;
                case "Show":
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    EnableControl(true);
                    break;
                case "View":
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    EnableControl(false);
                    break;
                case "Exit":
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    break;
            }
        }
        private void EnableControl(Boolean Status)
        {
            ddlSacComm.Enabled = Status;
            txtSacRemarks.Enabled = Status;
            txtSacDt.Enabled = Status;
            ddlRecommRemarks.Enabled = Status;

        }
        private void ClearControls()
        {
            txtLoanAmtF.Text = "0";
            txtROIF.Text = "0";
            txtTenureF.Text = "0";
            txtActionF.Text = "0";
            txtIntRiskScoreF.Text = "0";
            txtHoldRemarksF.Text = "";
            txtTotalLoanAmountF.Text = "0";
            txtEMIF.Text = "0";
            txtExistingEMIF.Text = "0";
            lblFileNameF.Text = "";
            lblCAMFileName.Text = "";
            ddlSacComm.SelectedIndex = -1;
            txtSacRemarks.Text = "";
            txtSacDt.Text = "";
            ddlRecommRemarks.SelectedIndex = -1;
        }

        #region Save
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                LoadBasicDetailsList(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
                // ClearControls();
            }
            else
            {
                StatusButton("Edit");
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            CDistrict oDis = null;
            string vBrCode = "", vErrMsg = "";
            Int32 vErr = 1, UID = 0;
            Int64 vLeadId = 0;

            double vLoanAmt = 0, vROI = 0, vTenure = 0, vInternalScore = 0;
            string vAction = "", vRemarks = "", vIsUpload = "N",
                vSacComm = "", vSacRemarks = "", vRecommRemarks = "", vApprovedStatus = "";


            try
            {

                if (hdLeadId.Value != "")
                {
                    vLeadId = Convert.ToInt64(hdLeadId.Value);
                }


                vBrCode = Session[gblValue.BrnchCode].ToString();
                UID = Convert.ToInt32(Session[gblValue.UserId]);


                vSacComm = ddlSacComm.SelectedValue;
                vSacRemarks = txtSacRemarks.Text;

                DateTime vSacDt = gblFuction.setDate(txtSacDt.Text.ToString());
                DateTime vSacExDt = gblFuction.setDate(txtSacExDt.Text.ToString());
                vRecommRemarks = ddlRecommRemarks.SelectedValue;

                switch (vRecommRemarks)
                {
                    case "Hold":
                        vApprovedStatus = "H";
                        break;
                    case "SendBack":
                        vApprovedStatus = "S";
                        break;
                    case "Approve":
                        vApprovedStatus = "A";
                        break;
                    case "Reject":
                        vApprovedStatus = "R";
                        break;
                }

                oDis = new CDistrict();
                vErr = oDis.CF_SaveFinalDecision(vLeadId, UID, 0, ref vErrMsg, vSacComm, vSacRemarks, vSacDt, vRecommRemarks, vApprovedStatus, vSacExDt);
                if (vErr == 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    vResult = true;
                }
                else
                {
                    gblFuction.MsgPopup(vErrMsg);
                    vResult = false;
                }


            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return vResult;

        }
        #endregion



        protected void btnSacLetter_Click(object sender, EventArgs e)
        {
            gblFuction.AjxMsgPopup("File will be provided by USFB...");
            return;
        }
        protected void btnCam_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vFile = "";
            string[] ActNetPath = CFLeadBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdLeadId.Value + "_" + lblCAMFileName.Text;
                if (ValidUrlChk(ActNetImage))
                {
                    vFile = ActNetImage;
                    break;
                }
            }
            if (vFile != "")
            {
                WebClient cln = null;
                cln = new WebClient();
                byte[] vDoc = null;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                cln = new WebClient();
                vDoc = cln.DownloadData(vFile);
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdLeadId.Value + "_" + lblCAMFileName.Text);
                Response.BinaryWrite(vDoc);
                Response.Flush();
                Response.End();
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }
        }

        protected void btnUPAttachmentF_Click(object sender, EventArgs e)
        {
            ViewImgDocF(hdLeadId.Value, hdnFileNameF.Value);
        }
        protected void ViewImgDocF(string ID, string FileName)
        {
            string ActNetImage = "", vPdfFile = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + ID + "_" + FileName;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                WebClient cln = null;
                cln = new WebClient();
                byte[] vDoc = null;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                cln = new WebClient();
                vDoc = cln.DownloadData(vPdfFile);
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Disposition", "attachment;   filename=" + ID + "_" + FileName);
                Response.BinaryWrite(vDoc);
                Response.Flush();
                Response.End();
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }

    }
}