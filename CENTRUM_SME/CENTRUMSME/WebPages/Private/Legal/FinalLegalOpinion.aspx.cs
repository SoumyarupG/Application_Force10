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
    public partial class FinalLegalOpinion : CENTRUMBAse
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
                GetBranchForFinalLegal();
                hdUserID.Value = this.UserID.ToString();
                txtFinLegAppDate.Text = Session[gblValue.LoginDate].ToString();
                mView.ActiveViewIndex = 0;
                GetPendFinalLegalList();
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
                this.PageHeading = "Final Legal Opinion";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuFinalLegalOpinion);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Final Legal Opinion", false);
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
            GetPendFinalLegalList();
        }
        private void GetBranchForFinalLegal()
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();
            dt = oDisb.GetBranchForFinalLegal(Session[gblValue.BrnchCode].ToString());
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
        private void GetPendFinalLegalList()
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBrCode = ddlLegBr.SelectedValue.ToString();
            string vSearchType = ddlSearchType.SelectedValue.ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetPendFinalLegalList(txtSearch.Text.Trim(), vBrCode, vSearchType);
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
                // get Property Schedule
                GetPropertSchByAppId(pLnAppId);
                // get List Of Documents
                GetDocListByAppId(pLnAppId);
                // Get Passing History
                GetPassingHistory(pLnAppId);
                // Get Final Legal Approval Details
                GetFinalLegalApprovalDtl(pLnAppId);
                // Get Level Approval Range
                GetLevelApprovalRange();
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
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    btnProSchUpdate.Enabled = true;
                }
            }
            else
            {
                lblPropertyScheduleId.Text = "";
                txtPropSchLnAppNo.Text = pLoanAppId;
                dt1 = oCG.GetCustNameByLnAppId(pLoanAppId);
                txtPropSchCustName.Text = dt1.Rows[0]["CustName"].ToString();
                txtPropSchRemarks.Text = "";
                txtBoundaryN.Text = "";
                txtBoundaryS.Text = "";
                txtBoundaryE.Text = "";
                txtBoundaryW.Text = "";
                txtPropSchDate.Text = Session[gblValue.LoginDate].ToString();
                btnProSchUpdate.Enabled = false;
            }
        }
        private void GetDocListByAppId(string pLoanAppId)
        {
            ClearDocList();
            DataTable dt = null, dt1 = null;
            CApplication oCG = null;
            oCG = new CApplication();
            dt = oCG.GetDocListFinalLegalByLnAppId(pLoanAppId);
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
                dt1 = oCG.GetDocListByLnAppId(pLoanAppId);
                if (dt1.Rows.Count > 0)
                {
                    txtDocListLoanAppNo.Text = dt1.Rows[0]["LoanAppId"].ToString();
                    txtDocListCustName.Text = dt1.Rows[0]["CustName"].ToString();
                    gvDocList.DataSource = dt1;
                    gvDocList.DataBind();
                    dt1.Columns.Remove("LoanAppId");
                    dt1.Columns.Remove("DocListId");
                    dt1.AcceptChanges();
                    ViewState["DocDtl"] = dt1;
                }
                else
                {
                    lblDocListId.Text = "";
                    txtDocListLoanAppNo.Text = pLoanAppId;
                    GetDocList();
                }
                btnDocListSave.Enabled = true;
                btnDocListUpdate.Enabled = false;
                btnDocListDelete.Enabled = false;
            }
        }
        private void GetPassingHistory(string pLoanAppId)
        {
            ClearPassingHistory();
            DataTable dt = null;
            DataTable dt1 = null;
            CApplication oCG = null;
            oCG = new CApplication();
            dt = oCG.GetLegalPassHistoryByAppId(pLoanAppId);
            if (dt.Rows.Count > 0)
            {
                txtLegPassHisLnAppNo.Text = dt.Rows[0]["LoanAppId"].ToString();
                txtLegPassHisCusName.Text = dt.Rows[0]["CustName"].ToString();
                lblPassHisId.Text = dt.Rows[0]["PassingHisId"].ToString();
                txtPassHisDate.Text = dt.Rows[0]["EntryDate"].ToString();
                txtPassHis1.Text = dt.Rows[0]["PassHis1"].ToString();
                txtPassHis2.Text = dt.Rows[0]["PassHis2"].ToString();
                txtPassHis3.Text = dt.Rows[0]["PassHis3"].ToString();
                txtPassHis4.Text = dt.Rows[0]["PassHis4"].ToString();
                txtPassHis5.Text = dt.Rows[0]["PassHis5"].ToString();
                txtLegalNote1.Text = dt.Rows[0]["LegalNote1"].ToString();
                txtLegalNote2.Text = dt.Rows[0]["LegalNote2"].ToString();
                txtLegalNote3.Text = dt.Rows[0]["LegalNote3"].ToString();
                txtLegReport.Text = dt.Rows[0]["LegalReport"].ToString();
                txtPropOwnNm.Text = dt.Rows[0]["PropertyOwnerNm"].ToString();

                btnLegPassingHisSave.Enabled = false;
                btnLegPassingHisUpdate.Enabled = true;
                btnLegPassingHisDelete.Enabled = true;
            }
            else
            {
                txtLegPassHisLnAppNo.Text = pLoanAppId;
                dt1 = oCG.GetCustNameByLnAppId(pLoanAppId);
                txtLegPassHisCusName.Text = dt1.Rows[0]["CustName"].ToString();
                txtPassHisDate.Text = Session[gblValue.LoginDate].ToString();
                txtPassHis1.Text = "";
                txtPassHis2.Text = "";
                txtPassHis3.Text = "";
                txtPassHis4.Text = "";
                txtPassHis5.Text = "";
                txtLegalNote1.Text = "MODT to be executed by ............... to in favor of the........";
                txtLegalNote2.Text = "";
                txtLegalNote3.Text = "";
                txtLegReport.Text = "";
                txtPropOwnNm.Text = "";
                btnLegPassingHisSave.Enabled = true;
                btnLegPassingHisUpdate.Enabled = false;
                btnLegPassingHisDelete.Enabled = false;
            }
        }
        private void GetFinalLegalApprovalDtl(string pLoanAppId)
        {
            ClearFinalLegalApprove();
            DataTable dt = null;
            CApplication oCG = null;
            oCG = new CApplication();
            dt = oCG.GetLnAppDetailsForFinalLeg(pLoanAppId);
            if (dt.Rows.Count > 0)
            {
                txtFinalLegAppLnAppId.Text = dt.Rows[0]["LoanAppId"].ToString();
                txtFinalLegLnAppDate.Text = dt.Rows[0]["ApplicationDt"].ToString();
                txtFinalLegCustName.Text = dt.Rows[0]["CompanyName"].ToString();
                txtFinalLegAppAmt.Text = dt.Rows[0]["AppAmount"].ToString();
                txtFinalLegLnTenure.Text = dt.Rows[0]["Tenure"].ToString();
                lblSanctionYN.Text = dt.Rows[0]["SanctionYN"].ToString();
                txtFinalLegLnPurpose.Text = dt.Rows[0]["PurposeName"].ToString();
                txtFinalLegLnType.Text = dt.Rows[0]["LoanTypeName"].ToString();
                if (string.IsNullOrEmpty(dt.Rows[0]["FinalLegApproveDt"].ToString()) == true)
                    txtFinLegAppDate.Text = Session[gblValue.LoginDate].ToString();
                else
                    txtFinLegAppDate.Text = dt.Rows[0]["FinalLegApproveDt"].ToString();
                txtFinalLegRemarks.Text = dt.Rows[0]["FinalLegRemarks"].ToString();
                ddlFinLegApprove.SelectedIndex = ddlFinLegApprove.Items.IndexOf(ddlFinLegApprove.Items.FindByValue(Convert.ToString(dt.Rows[0]["FinalLegalApproveStatus"])));

                hdFinalLegalAppStatus.Value = dt.Rows[0]["FinalLegalApproveStatus"].ToString();
                if (dt.Rows[0]["LegLevel1"].ToString() == "Y")
                    chkLegLevel1.Checked = true;
                else
                    chkLegLevel1.Checked = false;
                if (dt.Rows[0]["LegLevel2"].ToString() == "Y")
                    chkLegLevel2.Checked = true;
                else
                    chkLegLevel2.Checked = false;
                if (dt.Rows[0]["LegLevel3"].ToString() == "Y")
                    chkLegLevel3.Checked = true;
                else
                    chkLegLevel3.Checked = false;
                if (dt.Rows[0]["LegLevel4"].ToString() == "Y")
                    chkLegLevel4.Checked = true;
                else
                    chkLegLevel4.Checked = false;
                if (dt.Rows[0]["LegLevel5"].ToString() == "Y")
                    chkLegLevel5.Checked = true;
                else
                    chkLegLevel5.Checked = false;
                txtLegalCommnts.Text = dt.Rows[0]["FinalLegComments"].ToString();
                btnSaveFinLegApprove.Enabled = true;
            }
            else
            {
                txtFinalLegAppLnAppId.Text = "";
                txtFinLegAppDate.Text = Session[gblValue.LoginDate].ToString();
                txtFinalLegLnAppDate.Text = "";
                txtFinalLegCustName.Text = "";
                txtFinalLegAppAmt.Text = "";
                txtFinalLegLnTenure.Text = "";
                lblSanctionYN.Text = "";
                txtFinalLegLnPurpose.Text = "";
                txtFinalLegLnType.Text = "";
                txtFinalLegRemarks.Text = "";
                txtLegalCommnts.Text = "";
                ddlFinLegApprove.SelectedIndex = -1;
                btnSaveFinLegApprove.Enabled = false;
                chkLegLevel1.Checked = false;
                chkLegLevel2.Checked = false;
                chkLegLevel3.Checked = false;
                chkLegLevel4.Checked = false;
                chkLegLevel5.Checked = false;
                hdFinalLegalAppStatus.Value = "";
            }
        }
        
        private void ClearPassingHistory()
        {
            txtLegPassHisLnAppNo.Text = "";
            lblPassHisId.Text = "";
            txtPassHisDate.Text = Session[gblValue.LoginDate].ToString();
            txtPassHis1.Text = "";
            txtPassHis2.Text = "";
            txtPassHis3.Text = "";
            txtPassHis4.Text = "";
            txtPassHis5.Text = "";
            txtLegalNote1.Text = "MODT to be executed by ............... to in favor of the........";
            txtLegalNote2.Text = "";
            txtLegalNote3.Text = "";
            txtLegReport.Text = "";
            txtPropOwnNm.Text = "";
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
        private void ClearDocList()
        {
            lblDocListId.Text = "";
            gvDocList.DataSource = null;
            gvDocList.DataBind();
        }
        private void ClearFinalLegalApprove()
        {
            txtFinalLegAppLnAppId.Text = "";
            txtFinLegAppDate.Text = Session[gblValue.LoginDate].ToString();
            txtFinalLegLnAppDate.Text = "";
            txtFinalLegCustName.Text = "";
            txtFinalLegAppAmt.Text = "";
            txtFinalLegLnTenure.Text = "";
            lblSanctionYN.Text = "";
            txtFinalLegLnPurpose.Text = "";
            txtFinalLegLnType.Text = "";
            txtFinalLegRemarks.Text = "";
            txtLegalCommnts.Text = "";
            ddlFinLegApprove.SelectedIndex = -1;
            chkLegLevel1.Checked = false;
            chkLegLevel2.Checked = false;
            chkLegLevel3.Checked = false;
            chkLegLevel4.Checked = false;
            chkLegLevel5.Checked = false;
            hdFinalLegalAppStatus.Value = "";
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
                    vErr = oFS.SaveDocListFinalLegalBulk(pLnAppId, vXml, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save");
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
                    vErr = oFS.SaveDocListFinalLegalBulk(pLnAppId, vXml, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save");
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
                    vErr = oFS.SaveDocListFinalLegalBulk(pLnAppId, vXml, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Delete");
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
        private void SaveLegalPassHistory(string pMode)
        {
            int pLegPassHisuaryId = 0, vErr = 0;
            string pLnAppId = "", pPassHis1 = "", pPassHis2 = "", pPassHis3 = "", pPassHis4 = "", pPassHis5 = "",
                pLegNote1 = "", pLegNote2 = "", pLegNote3 = "", pLegComments = "",pLegReport="",pPropOwnerNm="";
            CApplication OCA = new CApplication();
            if (lblPassHisId.Text != "")
                pLegPassHisuaryId = Convert.ToInt32(lblPassHisId.Text);
            pLnAppId = txtLegPassHisLnAppNo.Text.ToString();
            if (pLnAppId == "")
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank");
                return;
            }
            if (txtPassHisDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Entry Date Can Not Be Blank");
                return;
            }
            pPassHis1 = txtPassHis1.Text.ToString();
            pPassHis2 = txtPassHis2.Text.ToString();
            pPassHis3 = txtPassHis3.Text.ToString();
            pPassHis4 = txtPassHis4.Text.ToString();
            pPassHis5 = txtPassHis5.Text.ToString();
            pLegNote1 = txtLegalNote1.Text.ToString();
            pLegNote2 = txtLegalNote2.Text.ToString();
            pLegNote3 = txtLegalNote3.Text.ToString();
            pLegComments = txtLegalCommnts.Text.ToString();
            pLegReport = txtLegReport.Text.ToString();
            pPropOwnerNm = txtPropOwnNm.Text.ToString();
            DateTime pDate = gblFuction.setDate(txtPassHisDate.Text);
            if (pMode == "Save")
            {
                vErr = OCA.SaveLegalPassHistory(pLegPassHisuaryId, pLnAppId, pDate, pPassHis1, pPassHis2, pPassHis3, pPassHis4, pPassHis5,
                    pLegNote1, pLegNote2, pLegNote3,pLegReport,pPropOwnerNm,
                    Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save");
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
                vErr = OCA.SaveLegalPassHistory(pLegPassHisuaryId, pLnAppId, pDate, pPassHis1, pPassHis2, pPassHis3, pPassHis4, pPassHis5,
                    pLegNote1, pLegNote2, pLegNote3, pLegReport, pPropOwnerNm,
                    Convert.ToInt32(Session[gblValue.UserId].ToString()), "Edit");
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
                vErr = OCA.SaveLegalPassHistory(pLegPassHisuaryId, pLnAppId, pDate, pPassHis1, pPassHis2, pPassHis3, pPassHis4, pPassHis5,
                     pLegNote1, pLegNote2, pLegNote3, pLegReport, pPropOwnerNm,
                    Convert.ToInt32(Session[gblValue.UserId].ToString()), "Delete");
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
        protected void btnLegPassingHisSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Save";
            SaveLegalPassHistory(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        protected void btnLegPassingHisUpdate_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Edit";
            SaveLegalPassHistory(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        protected void btnLegPassingHisDelete_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Delete";
            SaveLegalPassHistory(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        protected void btnSaveFinLegApprove_Click(object sender, EventArgs e)
        {
            CMember oMem = new CMember();
            try
            {
                if (txtFinalLegAppLnAppId.Text.ToString() == "")
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab For Final Legal Approval..");
                    return;
                }
                else
                {
                    if (txtFinLegAppDate.Text == "")
                    {
                        gblFuction.AjxMsgPopup("Final Legal Approval Date Can Not Be Blank");
                        return;
                    }
                    if (ddlFinLegApprove.SelectedValue == "-1")
                    {
                        gblFuction.AjxMsgPopup("Please Select Final Legal Approval Status");
                        return;
                    }
                    if (txtFinalLegLnAppDate.Text != "")
                    {
                        if (gblFuction.setDate(txtFinLegAppDate.Text) < gblFuction.setDate(txtFinalLegLnAppDate.Text))
                        {
                            gblFuction.AjxMsgPopup("Final Legal Approval Date Can Not Be Less than Loan Application Date");
                            return;
                        }
                    }
                    if (lblSanctionYN.Text == "Y")
                    {
                        gblFuction.MsgPopup("After Final Sanction You are Not Allowed To Change Final Legal Approval..");
                        return;
                    }
                    string pLnAppId = txtFinalLegAppLnAppId.Text.ToString();
                    DateTime pLegAppDate = gblFuction.setDate(txtFinLegAppDate.Text);
                    string pLegAppStatus = ddlFinLegApprove.SelectedValue.ToString().Trim();
                    string pFinalLegRemarks = txtFinalLegRemarks.Text.ToString();

                    string pLevel1 = "", pLevel2 = "", pLevel3 = "", pLevel4 = "", pLevel5 = "", pFinalLegApprove = "";
                    if (chkLegLevel1.Checked == true)
                        pLevel1 = "Y";
                    if (chkLegLevel2.Checked == true)
                        pLevel2 = "Y";
                    if (chkLegLevel3.Checked == true)
                        pLevel3 = "Y";
                    if (chkLegLevel4.Checked == true)
                        pLevel4 = "Y";
                    if (chkLegLevel5.Checked == true)
                        pLevel5 = "Y";
                    pFinalLegApprove = hdFinalLegalAppStatus.Value.ToString();

                    Int32 vErr = oMem.SaveFinalLegalApprove(pLnAppId, pFinalLegApprove, pLegAppDate, pFinalLegRemarks, Convert.ToInt32(Session[gblValue.UserId].ToString()), Convert.ToInt32(Session[gblValue.UserId].ToString()),
                        pLevel1, pLevel2, pLevel3, pLevel4, pLevel5);
                    if (vErr > 0)
                    {
                        gblFuction.AjxMsgPopup("Final Legal Complete...");
                        ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
                        return;
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup(gblPRATAM.DBError);
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

                oMem = null;
            }

        }
        private void GetLevelApprovalRange()
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            DateTime pDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            dt = OMem.GetLegalLevelRange(pDate);

            if (dt.Rows.Count > 0)
            {
                lblLevel1Amt.Text = dt.Rows[0]["legLevel1"].ToString();
                lblLevel2Amt.Text = dt.Rows[0]["legLevel2"].ToString();
                lblLevel3Amt.Text = dt.Rows[0]["legLevel3"].ToString();
                lblLevel4Amt.Text = dt.Rows[0]["legLevel4"].ToString();
                lblLevel5Amt.Text = dt.Rows[0]["legLevel5"].ToString();
            }
            else
            {
                lblLevel1Amt.Text = "0";
                lblLevel2Amt.Text = "0";
                lblLevel3Amt.Text = "0";
                lblLevel4Amt.Text = "0";
                lblLevel5Amt.Text = "0";
            }
        }
        private Int32 ApprovalLevel(Int32 RoleId, string Mode)
        {
            int Output = 0;
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            dt = OMem.GetLevelApprovalByRoleId(RoleId, Mode);
            if (dt.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dt.Rows[0]["ApprovalLevel"].ToString()) == false)
                {
                    Output = Convert.ToInt32(dt.Rows[0]["ApprovalLevel"].ToString());
                }
            }
            return Output;
        }
        private bool CheckApproval()
        {
            bool vResult = true;
            int x = ApprovalLevel(Convert.ToInt32(Session[gblValue.RoleId].ToString()), "L");
            if (chkLegLevel1.Checked == true)
            {
                if (x == 7)
                    vResult = true;
                else if(x>7)
                    vResult = true;
                else
                    vResult = false;
            }
            if (chkLegLevel2.Checked == true)
            {
                if (x == 8)
                    vResult = true;
                else if (x > 8)
                    vResult = true;
                else
                    vResult = false;
            }
            if (chkLegLevel3.Checked == true)
            {
                if (x == 9)
                    vResult = true;
                else if (x > 9)
                    vResult = true;
                else
                    vResult = false;
            }
            if (chkLegLevel4.Checked == true)
            {
                if (x == 10)
                    vResult = true;
                else if (x > 10)
                    vResult = true;
                else
                    vResult = false;
            }
            if (chkLegLevel5.Checked == true)
            {
                if (x == 11)
                    vResult = true;
                else
                    vResult = false;
            }
            return vResult;
        }
        protected void chkLegLevel1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLegLevel1.Checked == true)
            {
                if (Convert.ToInt32(Session[gblValue.RoleId].ToString()) != 1)
                {
                    if (CheckApproval() == false)
                    {
                        chkLegLevel1.Checked = false;
                        gblFuction.AjxMsgPopup("You Donot Have Level 1 Approval Permission, Please Check Role..");
                        return;
                    }
                    if (lblLevel1Amt.Text != "" && txtFinLegAppDate.Text != "")
                    {
                        if (Convert.ToDouble(lblLevel1Amt.Text) >= Convert.ToDouble(txtFinalLegAppAmt.Text))
                        {
                            hdFinalLegalAppStatus.Value = "A"; // A--- Final PD Approve
                        }
                    }
                }
                else
                {
                    if (lblLevel1Amt.Text != "" && txtFinalLegAppAmt.Text != "")
                    {
                        if (Convert.ToDouble(lblLevel1Amt.Text) >= Convert.ToDouble(txtFinalLegAppAmt.Text))
                        {
                            hdFinalLegalAppStatus.Value = "A"; // A--- Final PD Approve
                        }
                    }
                }

            }
        }
        protected void chkLegLevel2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLegLevel2.Checked == true)
            {
                if (Convert.ToInt32(Session[gblValue.RoleId].ToString()) != 1)
                {
                    if (CheckApproval() == false)
                    {
                        chkLegLevel2.Checked = false;
                        gblFuction.AjxMsgPopup("You Donot Have Level 2 Approval Permission, Please Check Role..");
                        return;
                    }
                    if (chkLegLevel1.Checked == false)
                    {
                        chkLegLevel2.Checked = false;
                        gblFuction.AjxMsgPopup("Level 1 Approval Should Be Done Before Level 2 Approval..");
                        return;
                    }
                    if (lblLevel2Amt.Text != "" && txtFinalLegAppAmt.Text != "")
                    {
                        if (Convert.ToDouble(lblLevel2Amt.Text) >= Convert.ToDouble(txtFinalLegAppAmt.Text))
                        {
                            hdFinalLegalAppStatus.Value = "A"; // A--- Final PD Approve
                        }
                    }
                }
                if (lblLevel2Amt.Text != "" && txtFinalLegAppAmt.Text != "")
                {
                    if (Convert.ToDouble(lblLevel2Amt.Text) >= Convert.ToDouble(txtFinalLegAppAmt.Text))
                    {
                        hdFinalLegalAppStatus.Value = "A"; // A--- Final PD Approve
                    }
                }
            }
        }
        protected void chkLegLevel3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLegLevel3.Checked == true)
            {
                if (Convert.ToInt32(Session[gblValue.RoleId].ToString()) != 1)
                {
                    if (CheckApproval() == false)
                    {
                        chkLegLevel3.Checked = false;
                        gblFuction.AjxMsgPopup("You Donot Have  Level 3 Approval Permission, Please Check Role..");
                        return;
                    }
                    if (chkLegLevel1.Checked == false || chkLegLevel2.Checked == false)
                    {
                        chkLegLevel3.Checked = false;
                        gblFuction.AjxMsgPopup("Level 1 / Level 2 Approval Should Be Done Before Level 3 Approval..");
                        return;
                    }
                    if (lblLevel3Amt.Text != "" && txtFinalLegAppAmt.Text != "")
                    {
                        if (Convert.ToDouble(lblLevel3Amt.Text) >= Convert.ToDouble(txtFinalLegAppAmt.Text))
                        {
                            hdFinalLegalAppStatus.Value = "A"; // A--- Final PD Approve
                        }
                    }
                }
                if (lblLevel3Amt.Text != "" && txtFinalLegAppAmt.Text != "")
                {
                    if (Convert.ToDouble(lblLevel3Amt.Text) >= Convert.ToDouble(txtFinalLegAppAmt.Text))
                    {
                        hdFinalLegalAppStatus.Value = "A"; // A--- Final PD Approve
                    }
                }
            }
        }
        protected void chkLegLevel4_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLegLevel4.Checked == true)
            {
                if (Convert.ToInt32(Session[gblValue.RoleId].ToString()) != 1)
                {
                    if (CheckApproval() == false)
                    {
                        chkLegLevel4.Checked = false;
                        gblFuction.AjxMsgPopup("You Donot Have  Level 4 Approval Permission, Please Check Role..");
                        return;
                    }
                    if (chkLegLevel1.Checked == false || chkLegLevel2.Checked == false || chkLegLevel3.Checked == false)
                    {
                        chkLegLevel4.Checked = false;
                        gblFuction.AjxMsgPopup("Level 1 / Level 2/ Level 3 Approval Should Be Done Before Level 4 Approval..");
                        return;
                    }
                    if (lblLevel4Amt.Text != "" && txtFinalLegAppAmt.Text != "")
                    {
                        if (Convert.ToDouble(lblLevel4Amt.Text) >= Convert.ToDouble(txtFinalLegAppAmt.Text))
                        {
                            hdFinalLegalAppStatus.Value = "A"; // A--- Final PD Approve
                        }
                    }
                }
                if (lblLevel4Amt.Text != "" && txtFinalLegAppAmt.Text != "")
                {
                    if (Convert.ToDouble(lblLevel4Amt.Text) >= Convert.ToDouble(txtFinalLegAppAmt.Text))
                    {
                        hdFinalLegalAppStatus.Value = "A"; // A--- Final PD Approve
                    }
                }
            }
        }
        protected void chkLegLevel5_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLegLevel5.Checked == true)
            {
                if (Convert.ToInt32(Session[gblValue.RoleId].ToString()) != 1)
                {
                    if (CheckApproval() == false)
                    {
                        chkLegLevel5.Checked = false;
                        gblFuction.AjxMsgPopup("You Donot Have  Level 5 Approval Permission, Please Check Role..");
                        return;
                    }
                    if (chkLegLevel1.Checked == false || chkLegLevel2.Checked == false || chkLegLevel3.Checked == false || chkLegLevel4.Checked == false)
                    {
                        chkLegLevel5.Checked = false;
                        gblFuction.AjxMsgPopup("Level 1 / Level 2/ Level 3/ Level 4 Approval Should Be Done Before Level 5 Approval..");
                        return;
                    }
                    if (lblLevel5Amt.Text != "" && txtFinalLegAppAmt.Text != "")
                    {
                        if (Convert.ToDouble(lblLevel5Amt.Text) >= Convert.ToDouble(txtFinalLegAppAmt.Text))
                        {
                            hdFinalLegalAppStatus.Value = "A"; // A--- Final PD Approve
                        }
                    }
                }
                if (lblLevel5Amt.Text != "" && txtFinalLegAppAmt.Text != "")
                {
                    if (Convert.ToDouble(lblLevel5Amt.Text) >= Convert.ToDouble(txtFinalLegAppAmt.Text))
                    {
                        hdFinalLegalAppStatus.Value = "A"; // A--- Final PD Approve
                    }
                }
            }
        }
       
        protected void btnProSchUpdate_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Edit";
            SavePropertySchedule(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        private void SavePropertySchedule(string pMode)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
            {
                gblFuction.AjxMsgPopup("Please log in to Head Office to edit Property Schedule,Branch Do Not Have Permission....");
                return;
            }
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
    }
}