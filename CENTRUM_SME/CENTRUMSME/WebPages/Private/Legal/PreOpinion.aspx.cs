using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;
using Newtonsoft.Json;
using System.Xml;
using SendSms;

namespace CENTRUMSME.WebPages.Private.Legal
{
    public partial class PreOpinion : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                // txtAdmDtApp.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                ViewState["CusTID"] = null;
                ViewState["LoanAppId"] = null;
                hdUserID.Value = this.UserID.ToString();
                mView.ActiveViewIndex = 0;
                PreOpinionLegBrCode();
                PendingPreOpinionList();
                StatusButton("View");
                //txttext.Attributes.Add("onkeypress", "return numericOnly(this);");
            }
            else
            {

            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Preliminary Opinion";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuPreOpinion);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnAppAdd.Visible = false;
                    //btnAppEdit.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnAppAdd.Visible = true;
                    //btnAppEdit.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnAppAdd.Visible = true;
                    //btnAppEdit.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnAppAdd.Visible = false;
                    //btnAppEdit.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Preliminary Opinion", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void ViewAcess()
        {
            if (mView.ActiveViewIndex == 1)
            {
                this.Menu = false;
                this.PageHeading = "Loan Application";
                this.GetModuleByRole(mnuID.mnuAppLnApplication);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 2)
            {
                this.Menu = false;
                this.PageHeading = "Personal Discussion By BM";
                this.GetModuleByRole(mnuID.mnuPDBM);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 3)
            {
                this.Menu = false;
                this.PageHeading = "Personal Discussion By CM";
                this.GetModuleByRole(mnuID.mnuPDCM);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 4)
            {
                this.Menu = false;
                this.PageHeading = "Balance Sheet Information";
                this.GetModuleByRole(mnuID.mnuBSStatement);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 5)
            {
                this.Menu = false;
                this.PageHeading = "Profit And Loss Information";
                this.GetModuleByRole(mnuID.mnuPLStatement);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 6)
            {
                this.Menu = false;
                this.PageHeading = "Reference Information";
                this.GetModuleByRole(mnuID.mnuReference);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else
            {
                this.Menu = false;
                this.PageHeading = "Personal Discussion";
                this.GetModuleByRole(mnuID.mnuPD);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
        }
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    //btnAppAdd.Enabled = false;
                    //btnAppEdit.Enabled = false;
                    btnExit.Enabled = false;
                    // ClearControls();
                    break;
                case "Show":
                    //btnAppAdd.Enabled = false;
                    //btnAppEdit.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    //btnAppAdd.Enabled = false;
                    //btnAppEdit.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    //btnAppAdd.Enabled = true;
                    //btnAppEdit.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    //btnAppAdd.Enabled = true;
                    //btnAppEdit.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Exit":
                    //btnAppAdd.Visible = false;
                    //btnAppEdit.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        private void EnableControl(Boolean Status)
        {
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            PendingPreOpinionList();
        }
        private void PreOpinionLegBrCode()
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();
            dt = oDisb.GetBranchForPreOpinion(Session[gblValue.BrnchCode].ToString());
            if (dt.Rows.Count > 0)
            {
                ddlLegBr.DataSource = dt;
                ddlLegBr.DataValueField = "BranchCode";
                ddlLegBr.DataTextField = "BranchName";
                ddlLegBr.DataBind();
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    ListItem liSel = new ListItem("ALL", "0000");
                    ddlLegBr.Items.Insert(0, liSel);
                }
            }
            else
            {
                ddlLegBr.DataSource = null;
                ddlLegBr.DataBind();
            }
        }
        private void PendingPreOpinionList()
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBrCode = ddlLegBr.SelectedValue.ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vSerchType = ddlSearchType.SelectedValue.ToString();
            try
            {
                ds = oMem.GetPendingPreOpnList(txtSearch.Text.Trim(), vBrCode, vSerchType);
                gvLoanApp.DataSource = null;
                gvLoanApp.DataBind();
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvLoanApp.DataSource = dt1;
                        gvLoanApp.DataBind();
                    }
                    else
                    {
                        gvLoanApp.DataSource = null;
                        gvLoanApp.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt1 = null;
                oMem = null;
            }

        }
        private void ShowLoanRelationDetails(string pLnAppId)
        {
            if (pLnAppId != "")
            {

                // get Customer KYC Details
                GetCustKYCDetails(pLnAppId);
                // get Property Schedule
                GetPropertSchByAppId(pLnAppId);
                // get List Of Documents
                GetDocListByAppId(pLnAppId);
                // Legal Quaries
                GetLegalQuaries(pLnAppId);


            }
        }
        protected void gvLoanApp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vApplicantionID = "";
            vApplicantionID = Convert.ToString(e.CommandArgument);
            ViewState["LoanAppId"] = vApplicantionID;
            if (e.CommandName == "cmdShowInfo")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShowInfo");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvLoanApp.Rows)
                {
                    if ((gr.RowIndex) % 2 == 0)
                    {
                        gr.BackColor = backColor;
                        gr.ForeColor = foreColor;
                    }
                    else
                    {
                        gr.BackColor = System.Drawing.Color.White;
                        gr.ForeColor = foreColor;
                    }
                    gr.Font.Bold = false;
                    LinkButton lb = (LinkButton)gr.FindControl("btnShowInfo");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
                ShowLoanRelationDetails(vApplicantionID);
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        private void GetCustKYCDetails(string pLoanAppId)
        {
            ClearCustKYC();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            CApplication oCG = null;
            oCG = new CApplication();
            ds = oCG.GetCustKYCByAppId(pLoanAppId);
            dt = ds.Tables[0];
            dt1 = ds.Tables[1];
            if (dt.Rows.Count > 0)
            {
                txtKYCLnAppNo.Text = dt.Rows[0]["LoanAppId"].ToString();
                //txtKYCCustName.Text = dt.Rows[0]["CompanyName"].ToString();
                //txtKYCCoAppNm.Text = dt.Rows[0]["CoAppName"].ToString();
                txtKYCCustAge.Text = dt.Rows[0]["CustAge"].ToString();
                txtKYCCustPerAdd.Text = dt.Rows[0]["Address"].ToString();
                txtKYCCustDOB.Text = dt.Rows[0]["CustDOB"].ToString();
                txtKYCCustPIN.Text = dt.Rows[0]["PIN"].ToString();
                lblLegCustId.Text = dt.Rows[0]["CusTID"].ToString();
                if (Convert.ToInt32(dt1.Rows[0]["CountLnApp"].ToString()) <= 0)
                {
                    popCustForHighmark(dt.Rows[0]["CusTID"].ToString());
                    GetCoApp(dt.Rows[0]["CusTID"].ToString());
                    btnLegalKYCSave.Enabled = true;
                    btnLegalKYCUpdate.Enabled = false;
                    btnLegalKYCDelete.Enabled = false;
                }
                else
                {
                    GetLegalCust(pLoanAppId);
                    GetLegalCoApp(pLoanAppId);
                    btnLegalKYCSave.Enabled = false;
                    btnLegalKYCUpdate.Enabled = true;
                    btnLegalKYCDelete.Enabled = true;
                }
            }
            else
            {
               
                //txtKYCCustName.Text = "";
                //txtKYCCoAppNm.Text = "";
                txtKYCCustAge.Text = "";
                txtKYCCustPerAdd.Text = "";
                txtKYCCustDOB.Text = "";
                txtKYCCustPIN.Text = "";
            }
        }

        private void GetCoApp(string pCustId)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            dt = OMem.GetCoAppByCustId(pCustId);
            if (dt.Rows.Count > 0)
            {
                gvCoAppDtl.DataSource = dt;
                gvCoAppDtl.DataBind();
            }
            else
            {
                gvCoAppDtl.DataSource = null;
                gvCoAppDtl.DataBind();

            }
        }
     
        private void GetLegalQuaries(string pLoanAppId)
        {
            ClearLegQuaries();
            DataTable dt = null;
            DataTable dt1 = null;
            CApplication oCG = null;
            oCG = new CApplication();
            dt = oCG.GetLegalQuariesByAppId(pLoanAppId);
            if (dt.Rows.Count > 0)
            {
                ddlLegQueryAppYN.SelectedValue = dt.Rows[0]["LegalQueryAppYN"].ToString();
                txtLegQueryCustName.Text = dt.Rows[0]["CustName"].ToString();
                txtLegQuaLnAppNo.Text = dt.Rows[0]["LoanAppId"].ToString();
                lblLegQuaryId.Text = dt.Rows[0]["LegQuaryId"].ToString();
                txtQueyGenDate.Text = dt.Rows[0]["QueryGenDate"].ToString();
                txtQ1.Text = dt.Rows[0]["Q1"].ToString();
                txtQ2.Text = dt.Rows[0]["Q2"].ToString();
                txtQ3.Text = dt.Rows[0]["Q3"].ToString();
                txtQ4.Text = dt.Rows[0]["Q4"].ToString();
                txtQ5.Text = dt.Rows[0]["Q5"].ToString();

                txtResponseDt.Text = dt.Rows[0]["AnserDateTime"].ToString();
                txtA1.Text = dt.Rows[0]["A1"].ToString();
                txtA2.Text = dt.Rows[0]["A2"].ToString();
                txtA3.Text = dt.Rows[0]["A3"].ToString();
                txtA4.Text = dt.Rows[0]["A4"].ToString();
                txtA5.Text = dt.Rows[0]["A5"].ToString();
                btnLegQuaSave.Enabled = false;
                btnLegQuaUpdate.Enabled = true;
                btnLegQuaDelete.Enabled = true;
            }
            else
            {
                ddlLegQueryAppYN.SelectedValue = "Y";
                txtLegQuaLnAppNo.Text = pLoanAppId;
                dt1 = oCG.GetCustNameByLnAppId(pLoanAppId);
                txtLegQueryCustName.Text = dt1.Rows[0]["CustName"].ToString();
                txtQueyGenDate.Text = Session[gblValue.LoginDate].ToString();
                txtQ1.Text = "";
                txtQ2.Text = "";
                txtQ3.Text = "";
                txtQ4.Text = "";
                txtQ5.Text = "";

                txtA1.Text = "";
                txtA2.Text = "";
                txtA3.Text = "";
                txtA4.Text = "";
                txtA5.Text = "";
                txtResponseDt.Text = "";
                btnLegQuaSave.Enabled = true;
                btnLegQuaUpdate.Enabled = false;
                btnLegQuaDelete.Enabled = false;
            }
        }
        private void GetPropertSchByAppId(string pLoanAppId)
        {
            ClearPropertySchedule();
            DataTable dt = null;
            DataTable dt1 = null;
            CApplication oCG = null;
            oCG = new CApplication();
            dt = oCG.GetPropertSchByAppId(pLoanAppId);
            if (dt.Rows.Count > 0)
            {
                lblPropertyScheduleId.Text = dt.Rows[0]["PropertyScheduleId"].ToString();
                txtPropSchLnAppNo.Text = dt.Rows[0]["LoanAppId"].ToString();
                txtPropSchCustName.Text = dt.Rows[0]["CustName"].ToString();
                txtPropSchRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                txtBoundaryN.Text = dt.Rows[0]["NBoundary"].ToString();
                txtBoundaryS.Text = dt.Rows[0]["SBoundary"].ToString();
                txtBoundaryE.Text = dt.Rows[0]["EBoundary"].ToString();
                txtBoundaryW.Text = dt.Rows[0]["WBoundary"].ToString();
                txtPropSchDate.Text = dt.Rows[0]["EntryDate"].ToString();
                btnProSchSave.Enabled = false;
                btnProSchUpdate.Enabled = true;
                btnProSchDelete.Enabled = true;
            }
            else
            {
                dt1 = oCG.GetCustNameByLnAppId(pLoanAppId);
                txtPropSchCustName.Text = dt1.Rows[0]["CustName"].ToString();
                lblPropertyScheduleId.Text = "";
                txtPropSchLnAppNo.Text = pLoanAppId;
                txtPropSchRemarks.Text = "";
                txtBoundaryN.Text = "";
                txtBoundaryS.Text = "";
                txtBoundaryE.Text = "";
                txtBoundaryW.Text = "";
                txtPropSchDate.Text = Session[gblValue.LoginDate].ToString();
                btnProSchSave.Enabled = true;
                btnProSchUpdate.Enabled = false;
                btnProSchDelete.Enabled = false;
            }
        }
        private void GetDocListByAppId(string pLoanAppId)
        {
            ClearDocList();
            DataTable dt = null;
            CApplication oCG = null;
            DataTable dt1 = null;
            oCG = new CApplication();
            dt = oCG.GetDocListByLnAppId(pLoanAppId);
            if (dt.Rows.Count > 0)
            {
                lblDocListId.Text = dt.Rows[0]["DocListId"].ToString();
                txtDocListLoanAppNo.Text = dt.Rows[0]["LoanAppId"].ToString();
                txtDocListCustName.Text = dt.Rows[0]["CustName"].ToString();
                gvDocList.DataSource = dt;
                gvDocList.DataBind();
                btnDocListSave.Enabled = false;
                btnDocListUpdate.Enabled = true;
                btnDocListDelete.Enabled = true;
                dt.Columns.Remove("LoanAppId");
                dt.Columns.Remove("DocListId");
                dt.AcceptChanges();
                ViewState["DocDtl"] = dt;
            }
            else
            {
                lblDocListId.Text = "";
                txtDocListLoanAppNo.Text = pLoanAppId;
                dt1 = oCG.GetCustNameByLnAppId(pLoanAppId);
                txtDocListCustName.Text = dt1.Rows[0]["CustName"].ToString();
                GetDocList();
                btnDocListSave.Enabled = true;
                btnDocListUpdate.Enabled = false;
                btnDocListDelete.Enabled = false;
            }
        }
        private void ClearCustKYC()
        {
            txtKYCLnAppNo.Text = "";
            //txtKYCCustName.Text = "";
            //txtKYCCoAppNm.Text = "";
            txtKYCCustAge.Text = "";
            txtKYCCustPerAdd.Text = "";
            txtKYCCustDOB.Text = "";
            txtKYCCustPIN.Text = "";
            gvApp.DataSource = null;
            gvApp.DataBind();
            gvCoAppDtl.DataSource = null;
            gvCoAppDtl.DataBind();
        }
        private void ClearPropertySchedule()
        {
            lblPropertyScheduleId.Text = "";
            txtPropSchDate.Text = Session[gblValue.LoginDate].ToString();
            txtPropSchLnAppNo.Text = "";
            txtPropSchRemarks.Text = "";
            txtBoundaryN.Text = "";
            txtBoundaryS.Text = "";
            txtBoundaryE.Text = "";
            txtBoundaryW.Text = "";
        }
        private void ClearLegQuaries()
        {
            txtQueyGenDate.Text = Session[gblValue.LoginDate].ToString();
            txtQ1.Text = "";
            txtQ2.Text = "";
            txtQ3.Text = "";
            txtQ4.Text = "";
            txtQ5.Text = "";
            txtA1.Text = "";
            txtA2.Text = "";
            txtA3.Text = "";
            txtA4.Text = "";
            txtA5.Text = "";
            txtResponseDt.Text = "";
        }
        private void ClearDocList()
        {
            lblDocListId.Text = "";
            gvDocList.DataSource = null;
            gvDocList.DataBind();
        }
        protected void btnLegalKYCSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Save";
            SaveLegalKYC(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        protected void btnLegalKYCUpdate_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Edit";
            SaveLegalKYC(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        protected void btnLegalKYCDelete_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Delete";
            SaveLegalKYC(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        protected void btnProSchSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Save";
            SavePropertySchedule(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        protected void btnProSchUpdate_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Edit";
            SavePropertySchedule(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        protected void btnProSchDelete_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Delete";
            SavePropertySchedule(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }

        protected void btnLegQuaSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Save";
            SaveLegalQuaries(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        protected void btnLegQuaUpdate_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Edit";
            SaveLegalQuaries(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        protected void btnLegQuaDelete_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Delete";
            SaveLegalQuaries(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }

        protected void btnDocListSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Save";
            SaveDocList(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        protected void btnDocListUpdate_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Edit";
            SaveDocList(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        protected void btnDocListDelete_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Delete";
            SaveDocList(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }

        private void SavePropertySchedule(string pMode)
        {
            int pPcheduleId = 0, vErr = 0;
            string pLnAppId = "", pRemarks = "", pNBoundary = "", pSBoundary = "", pEBoundary = "", pWBoundary = "";
            CApplication OCA = new CApplication();
            if (lblPropertyScheduleId.Text != "")
                pPcheduleId = Convert.ToInt32(lblPropertyScheduleId.Text);
            pLnAppId = txtPropSchLnAppNo.Text.ToString();
            if (pLnAppId == "")
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank");
                return;
            }
            if (txtPropSchDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Date Can Not Be Blank");
                return;
            }
            pRemarks = txtPropSchRemarks.Text.ToString();
            pNBoundary = txtBoundaryN.Text.ToString();
            pSBoundary = txtBoundaryS.Text.ToString();
            pEBoundary = txtBoundaryE.Text.ToString();
            pWBoundary = txtBoundaryW.Text.ToString();
            if (string.IsNullOrEmpty(pNBoundary) == true || string.IsNullOrEmpty(pSBoundary) == true || string.IsNullOrEmpty(pEBoundary) == true || string.IsNullOrEmpty(pWBoundary) == true)
            {
                gblFuction.AjxMsgPopup("All Boundaries Should Be Filled Up, Any Of Boundary Can Not Be Blank..");
                return;
            }
            DateTime pDate = gblFuction.setDate(txtPropSchDate.Text);
            if (pMode == "Save")
            {
                vErr = OCA.SavePropertySchedule(pPcheduleId, pLnAppId, pDate, pRemarks, pNBoundary, pSBoundary, pEBoundary, pWBoundary, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save");
                if (vErr > 0)
                {
                    gblFuction.MsgPopup("Record Save Successfully");
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                    return;
                }
            }
            if (pMode == "Edit")
            {
                vErr = OCA.SavePropertySchedule(pPcheduleId, pLnAppId, pDate, pRemarks, pNBoundary, pSBoundary, pEBoundary, pWBoundary, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Edit");
                if (vErr > 0)
                {
                    gblFuction.MsgPopup("Record Update Successfully");
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                    return;
                }
            }
            if (pMode == "Delete")
            {
                vErr = OCA.SavePropertySchedule(pPcheduleId, pLnAppId, pDate, pRemarks, pNBoundary, pSBoundary, pEBoundary, pWBoundary, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Delete");
                if (vErr > 0)
                {
                    gblFuction.MsgPopup("Record Deleted Successfully");
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                    return;
                }
            }

        }
        private void SaveLegalQuaries(string pMode)
        {
            int pLegQuaryId = 0, vErr = 0;
            string pLnAppId = "", pQ1 = "", pQ2 = "", pQ3 = "", pQ4 = "", pQ5 = "",pLegalQueryAppYN="Y";
            CApplication OCA = new CApplication();
            if (lblLegQuaryId.Text != "")
                pLegQuaryId = Convert.ToInt32(lblLegQuaryId.Text);
            pLnAppId = txtLegQuaLnAppNo.Text.ToString();
            if (pLnAppId == "")
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank");
                return;
            }
            if (txtQueyGenDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Query Generation Date Can Not Be Blank");
                return;
            }
            pLegalQueryAppYN = ddlLegQueryAppYN.SelectedValue.ToString();
            if (ddlLegQueryAppYN.SelectedValue.ToString() == "Y")
            {
                pQ1 = txtQ1.Text.ToString();
                pQ2 = txtQ2.Text.ToString();
                pQ3 = txtQ3.Text.ToString();
                pQ4 = txtQ4.Text.ToString();
                pQ5 = txtQ5.Text.ToString();
            }
            if (pLegalQueryAppYN == "Y")
            {
                if (pQ1 == "" && pQ2 == "" && pQ3 == "" && pQ4 == "" && pQ5 == "")
                {
                    gblFuction.AjxMsgPopup("Please Input Atleast One Query When Legal Query Is Applicable");
                    return;
                }
            }
            DateTime pDate = gblFuction.setDate(txtQueyGenDate.Text);
            if (pMode == "Save")
            {
                vErr = OCA.SaveLegalQuaries(pLegQuaryId, pLnAppId, pDate, pQ1, pQ2, pQ3, pQ4, pQ5, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save",
                    pLegalQueryAppYN);
                if (vErr > 0)
                {
                    gblFuction.MsgPopup("Record Save Successfully");
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                    return;
                }
            }
            if (pMode == "Edit")
            {
                vErr = OCA.SaveLegalQuaries(pLegQuaryId, pLnAppId, pDate, pQ1, pQ2, pQ3, pQ4, pQ5, Convert.ToInt32(Session[gblValue.UserId].ToString()),
                    "Edit", pLegalQueryAppYN);
                if (vErr > 0)
                {
                    gblFuction.MsgPopup("Record Update Successfully");
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                    return;
                }
            }
            if (pMode == "Delete")
            {
                vErr = OCA.SaveLegalQuaries(pLegQuaryId, pLnAppId, pDate, pQ1, pQ2, pQ3, pQ4, pQ5, Convert.ToInt32(Session[gblValue.UserId].ToString()),
                    "Delete", pLegalQueryAppYN);
                if (vErr > 0)
                {
                    gblFuction.MsgPopup("Record Deleted Successfully");
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                    return;
                }
            }

        }
        private void SaveDocList(string pMode)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");

            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string pLnAppId = txtDocListLoanAppNo.Text.ToString();
            if (pLnAppId == "")
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank");
                return;
            }
            CApplication oFS = null;
            DataTable dtXml = CreateXmlData();
            int vErr = 0;
            string vXml = "";
            try
            {
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString();
                }
                oFS = new CApplication();
                if (pMode == "Save")
                {
                    vErr = oFS.SaveDocListBulk(pLnAppId, vXml, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Record Save Successfully");
                        return;
                    }
                    else
                    {
                        gblFuction.MsgPopup("Error: Data not Saved");
                        return;
                    }
                }
                if (pMode == "Edit")
                {
                    vErr = oFS.SaveDocListBulk(pLnAppId, vXml, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Record Updated Successfully");
                        return;
                    }
                    else
                    {
                        gblFuction.MsgPopup("Error: Data not Saved");
                        return;
                    }
                }
                if (pMode == "Delete")
                {
                    vErr = oFS.SaveDocListBulk(pLnAppId, vXml, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Delete");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Record Deleted Successfully");
                        return;
                    }
                    else
                    {
                        gblFuction.MsgPopup("Error: Data not Saved");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtXml = null;
                oFS = null;
            }
        }
        private void SaveLegalKYC(string pMode)
        {

            Int32 vErr = 0;
            CApplication oCG = new CApplication();
            // For Co Applicant
            DataRow drApp = null;
            DataTable dtXmlApp = new DataTable();
            dtXmlApp.Columns.Add(new DataColumn("CustID"));
            dtXmlApp.Columns.Add(new DataColumn("CompanyName"));
            dtXmlApp.Columns.Add(new DataColumn("IsActive"));
            foreach (GridViewRow gr in gvApp.Rows)
            {
                string pActive = "Y";
                drApp = dtXmlApp.NewRow();
                drApp["CustID"] = ((Label)gr.FindControl("lblCustIDId")).Text;
                drApp["CompanyName"] = ((Label)gr.FindControl("lblCustName")).Text;
                if (((CheckBox)gr.FindControl("chkApp")).Checked == true)
                    pActive = "Y";
                else
                    pActive = "N";
                drApp["IsActive"] = pActive;
                dtXmlApp.Rows.Add(drApp);
                dtXmlApp.AcceptChanges();
            }
            dtXmlApp.TableName = "TableAPP";


            // For Co Applicant
            DataRow dr = null;
            DataTable dtXml = new DataTable();
            dtXml.Columns.Add(new DataColumn("CoApplicantId"));
            dtXml.Columns.Add(new DataColumn("CoApplicantName"));
            dtXml.Columns.Add(new DataColumn("IsActive"));
            foreach (GridViewRow gr in gvCoAppDtl.Rows)
            {
                string pActive = "Y";
                dr = dtXml.NewRow();
                dr["CoApplicantId"] = ((Label)gr.FindControl("lblCoApplicantId")).Text;
                dr["CoApplicantName"] = ((Label)gr.FindControl("lblCoAppName")).Text;
                if (((CheckBox)gr.FindControl("chkCoApp")).Checked == true)
                    pActive = "Y";
                else
                    pActive = "N";
                dr["IsActive"] = pActive;
                dtXml.Rows.Add(dr);
                dtXml.AcceptChanges();
            }
            dtXml.TableName = "TableCoAPP";


            string vAppXml = "";
            using (StringWriter oSW = new StringWriter())
            {
                dtXmlApp.WriteXml(oSW);
                vAppXml = oSW.ToString().Replace("12:00:00 AM", "").Trim();
            }
            string vCAXml = "";
            using (StringWriter oSW = new StringWriter())
            {
                dtXml.WriteXml(oSW);
                vCAXml = oSW.ToString().Replace("12:00:00 AM", "").Trim();
            }

            string pLnAppId = txtKYCLnAppNo.Text.ToString();
            string pCustId = lblLegCustId.Text.ToString();
            if (pLnAppId == "")
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank");
                return;
            }
            if (pMode == "Save" )
            {
                vErr = oCG.SaveLegalKYC(pLnAppId, pCustId, vCAXml, vAppXml, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save");
                if (vErr == 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    return;
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                    return;
                }
            }
            if (pMode == "Edit")
            {
                vErr = oCG.SaveLegalKYC(pLnAppId, pCustId, vCAXml, vAppXml, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Edit");
                if (vErr == 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    return;
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                    return;
                }
            }
            if (pMode == "Delete")
            {
                vErr = oCG.SaveLegalKYC(pLnAppId, pCustId, vCAXml, vAppXml, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Delete");
                if (vErr == 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    return;
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                    return;
                }
            }
        }
        protected void gvDocList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CApplication oMem = new CApplication();
            DataTable dt = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlDocType = (e.Row.FindControl("ddlDocType") as DropDownList);
                Label lblDocTypeId = (e.Row.FindControl("lblDocTypeId") as Label);

                try
                {
                    dt = oMem.GetDocType();
                    if (dt.Rows.Count > 0)
                    {
                        ddlDocType.DataSource = dt;
                        ddlDocType.DataTextField = "DocTypeName";
                        ddlDocType.DataValueField = "DocTypeId";
                        ddlDocType.DataBind();
                        ListItem oli1 = new ListItem("<--Select-->", "-1");
                        ddlDocType.Items.Insert(0, oli1);
                    }
                    string DocTypeId = lblDocTypeId.Text;
                    if (DocTypeId != " ")
                        ddlDocType.SelectedValue = DocTypeId;
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
        }
        protected void gvApp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CApplication oMem = new CApplication();
            DataTable dt = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkApp = (e.Row.FindControl("chkApp") as CheckBox);
                Label lblCustAct = (e.Row.FindControl("lblCustAct") as Label);

                try
                {

                    string ActCust = lblCustAct.Text;
                    if (ActCust == "Y")
                        chkApp.Checked = true;
                    else
                        chkApp.Checked = false;
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
        }
        protected void gvCoAppDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CApplication oMem = new CApplication();
            DataTable dt = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkCoApp = (e.Row.FindControl("chkCoApp") as CheckBox);
                Label lblCAAct = (e.Row.FindControl("lblCAAct") as Label);

                try
                {

                    string ActCA = lblCAAct.Text;
                    if (ActCA == "Y")
                        chkCoApp.Checked = true;
                    else
                        chkCoApp.Checked = false;
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
        }
        private void GetDocList()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SLNo", typeof(int));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("DocName", typeof(string));
            dt.Columns.Add("DocNo", typeof(string));
            dt.Columns.Add("DocTypeId", typeof(int));

            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[0]["SlNo"] = 1;
            dt.Rows[0]["Date"] = Session[gblValue.LoginDate].ToString();

            gvDocList.DataSource = dt;
            gvDocList.DataBind();

            ViewState["DocDtl"] = dt;

        }
        private DataTable CreateXmlData()
        {
            DataTable dt = new DataTable("DocList");
            dt.Columns.Add("SLNo", typeof(int));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("DocName", typeof(string));
            dt.Columns.Add("DocNo", typeof(string));
            dt.Columns.Add("DocTypeId", typeof(int));
            DataRow dr;
            foreach (GridViewRow gr in gvDocList.Rows)
            {
                Label lblSLDoc = (Label)gr.FindControl("lblSLDoc");
                TextBox txtDocDate = (TextBox)gr.FindControl("txtDocDate");
                TextBox txtDocName = (TextBox)gr.FindControl("txtDocName");
                TextBox txtDocNo = (TextBox)gr.FindControl("txtDocNo");
                DropDownList ddlDocType = (DropDownList)gr.FindControl("ddlDocType");
                if (txtDocDate.Text != "")
                {
                    dr = dt.NewRow();
                    dr["SLNo"] = lblSLDoc.Text;
                    dr["Date"] = gblFuction.setDate(txtDocDate.Text.ToString());
                    dr["DocName"] = txtDocName.Text;
                    dr["DocNo"] = txtDocNo.Text;
                    dr["DocTypeId"] = ddlDocType.SelectedValue.ToString();
                    dt.Rows.Add(dr);
                }
            }
            dt.AcceptChanges();
            return dt;
        }
        protected void btnAddDocRow_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["DocDtl"];

                int curRow = 0, maxRow = 0, vRow = 0;
                ImageButton txtCur = (ImageButton)sender;
                GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
                curRow = gR.RowIndex;
                maxRow = gvDocList.Rows.Count;
                vRow = dt.Rows.Count - 1;
                DataRow dr;
                Label lblSLDoc = (Label)gvDocList.Rows[curRow].FindControl("lblSLDoc");
                TextBox txtDocDate = (TextBox)gvDocList.Rows[curRow].FindControl("txtDocDate");
                TextBox txtDocName = (TextBox)gvDocList.Rows[curRow].FindControl("txtDocName");
                TextBox txtDocNo = (TextBox)gvDocList.Rows[curRow].FindControl("txtDocNo");


                dt.Rows[curRow][0] = lblSLDoc.Text;
                if (txtDocDate.Text == "")
                {
                    gblFuction.AjxMsgPopup("Date Can Not Be Blank..");
                    return;
                }
                dt.Rows[curRow][1] = txtDocDate.Text;
                dt.Rows[curRow][2] = txtDocName.Text;
                dt.Rows[curRow][3] = txtDocNo.Text;

                if (((DropDownList)gvDocList.Rows[curRow].FindControl("ddlDocType")).SelectedIndex == -1)
                {
                    ((Label)gvDocList.Rows[curRow].FindControl("lblDocTypeId")).Text = "-1";
                }
                else
                {
                    ((Label)gvDocList.Rows[curRow].FindControl("lblDocTypeId")).Text = ((DropDownList)gvDocList.Rows[curRow].FindControl("ddlDocType")).SelectedValue.ToString();
                }
                Label lblDocTypeId = ((Label)gvDocList.Rows[curRow].FindControl("lblDocTypeId"));
                dt.Rows[curRow][4] = Convert.ToInt32(lblDocTypeId.Text);

                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dt.Rows[vRow + 1]["SlNo"] = Convert.ToInt32(((Label)gvDocList.Rows[vRow].FindControl("lblSLDoc")).Text) + 1;
                dt.Rows[vRow + 1]["Date"] = Session[gblValue.LoginDate].ToString();
                dt.AcceptChanges();

                ViewState["DocDtl"] = dt;
                gvDocList.DataSource = dt;
                gvDocList.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        protected void ImDelDoc_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["DocDtl"];
                if (dt.Rows.Count > 0)
                {
                    if ((dt.Rows.Count - 1) > gR.RowIndex)
                    {
                        gblFuction.AjxMsgPopup("Only Last Row Can Be Deleted ");
                        return;
                    }
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["DocDtl"] = dt;
                    gvDocList.DataSource = dt;
                    gvDocList.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }
        private void popCustForHighmark(string pCustId)
        {
            DataTable dt = new DataTable();
            CApplication OApp = new CApplication();
            dt = OApp.GetCustForHighmark(pCustId);
            if (dt.Rows.Count > 0)
            {
                gvApp.DataSource = dt;
                gvApp.DataBind();
            }
            else
            {
                gvApp.DataSource = null;
                gvApp.DataBind();

            }
        }
        private void GetLegalCust(string pLoanAppId)
        {
            DataTable dt = new DataTable();
            CApplication OCA = new CApplication();
            dt = OCA.GetLegalCust(pLoanAppId);
            if (dt.Rows.Count > 0)
            {
                gvApp.DataSource = dt;
                gvApp.DataBind();
            }
        }
        private void GetLegalCoApp(string pLoanAppId)
        {
            DataTable dt = new DataTable();
            CApplication OCA = new CApplication();
            dt = OCA.GetLegalCoApp(pLoanAppId);
            if (dt.Rows.Count > 0)
            {
                gvCoAppDtl.DataSource = dt;
                gvCoAppDtl.DataBind();
            }
        }
    }
}