using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;
using SendSms;
using System.Web;

namespace CENTRUMSME.WebPages.Private.Legal
{
    public partial class BranchQueryResponse : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                InitBasePage();
                //////hdUserID.Value = this.UserID.ToString();
                GetBranchForLegResponse();
                LoadPendResponseList();
                txtResponseDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                tabBranchLeg.ActiveTabIndex = 0;
            }
        }
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnSave.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnSave.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnSave.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnSave.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnSave.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        private void ClearControls()
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
            txtResponseDt.Text = Session[gblValue.LoginDate].ToString();

        }
        private void EnableControl(Boolean Status)
        {
            /*******************/
            //ddlBranch.Enabled = Status;
            //ddlLoanType.Enabled = Status;
            //ddlRpSchdle.Enabled = Status;
            //ddlInstType.Enabled = Status;
            //ddlCust.Enabled = Status;
            //ddlSancNo.Enabled = Status;
            //txtLnDt.Enabled = Status;
            //ddlSrcFund.Enabled = Status;
            //rdbPayMode.Enabled = Status;
            //txtChqNo.Enabled = Status;
            //txtChqDt.Enabled = Status;
            //txtStDt.Enabled = Status;
            //ddlBank.Enabled = Status;
            //ddlLedgr.Enabled = Status;
            //txtRefNo.Enabled = Status;
            //txtInsSerTax.Enabled = Status;
            //txtSancAmt.Enabled = Status;
            //txtSancDt.Enabled = Status;
            //txtLnCycle.Enabled = Status;
            /*************************/
        }
        private void InitBasePage()
        {
            try
            {

                this.Menu = false;
                this.PageHeading = "Branch Response Against Query";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBranchQueryResponse);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                    return;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                    return;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                    return;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnSave.Visible = true;
                    return;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Branch Response Against Query", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void GetBranchForLegResponse()
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();
            dt = oDisb.GetBranchForLegResolve(Session[gblValue.BrnchCode].ToString());
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
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadPendResponseList();
        }
        private void LoadPendResponseList()
        {
            DataTable dt = null;
            CApplication oCA = null;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                vBrCode = ddlLegBr.SelectedValue.ToString();
                string vSearchType = ddlSearchType.SelectedValue.ToString();
                oCA = new CApplication();
                gvPenResponse.DataSource = null;
                gvPenResponse.DataBind();
                dt = oCA.GetPenResponseList(vSearchType, vBrCode, txtSearch.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    gvPenResponse.DataSource = dt;
                    gvPenResponse.DataBind();
                }
                else
                {
                    gvPenResponse.DataSource = null;
                    gvPenResponse.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        
        protected void gvPenResponse_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vLnAppId = "";
            DataTable dt = null;
            CApplication oCG = null;
            try
            {
                vLnAppId = Convert.ToString(e.CommandArgument);
                ViewState["LnSancId"] = vLnAppId;
                if (e.CommandName == "cmdShow")
                {
                    oCG = new CApplication();
                    dt = oCG.GetQueryResponseByAppId(vLnAppId, "P");
                    if (dt.Rows.Count > 0)
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvPenResponse.Rows)
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
                            LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                            lb.ForeColor = System.Drawing.Color.Black;
                            lb.Font.Bold = false;
                        }
                        gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                        gvRow.ForeColor = System.Drawing.Color.White;
                        gvRow.Font.Bold = true;
                        btnShow.ForeColor = System.Drawing.Color.White;
                        btnShow.Font.Bold = true;

                        ViewState["StateEdit"] = "Add";
                        if (ddlSearchType.SelectedValue.ToString() == "P")
                        {
                            btnSave.Enabled = true;
                            btnUpdate.Enabled = false;
                            btnDelete.Enabled = false;
                        }
                        else if (ddlSearchType.SelectedValue.ToString() == "D")
                        {
                            btnSave.Enabled = false;
                            btnUpdate.Enabled = true;
                            btnDelete.Enabled = true;
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Please Select Search Type As Pending/Done To Show Response Details");
                            return;
                        }
                        txtResCustName.Text = dt.Rows[0]["CustName"].ToString();
                        txtResLnAppNo.Text = dt.Rows[0]["LoanAppId"].ToString();
                        lblLegQuaryId.Text = dt.Rows[0]["LegQuaryId"].ToString();
                        txtQueyGenDate.Text = dt.Rows[0]["QueryGenDate"].ToString();
                        txtQ1.Text = dt.Rows[0]["Q1"].ToString();
                        txtQ2.Text = dt.Rows[0]["Q2"].ToString();
                        txtQ3.Text = dt.Rows[0]["Q3"].ToString();
                        txtQ4.Text = dt.Rows[0]["Q4"].ToString();
                        txtQ5.Text = dt.Rows[0]["Q5"].ToString();

                        if (dt.Rows[0]["AnserDateTime"].ToString() == "")
                            txtResponseDt.Text = Session[gblValue.LoginDate].ToString();
                        else
                            txtResponseDt.Text = dt.Rows[0]["AnserDateTime"].ToString();
                        txtA1.Text = dt.Rows[0]["A1"].ToString();
                        txtA2.Text = dt.Rows[0]["A2"].ToString();
                        txtA3.Text = dt.Rows[0]["A3"].ToString();
                        txtA4.Text = dt.Rows[0]["A4"].ToString();
                        txtA5.Text = dt.Rows[0]["A5"].ToString();
                        tabBranchLeg.ActiveTabIndex = 1;
                    }

                }
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Save";
            SaveLegalResponse(vStateEdit);
            LoadPendResponseList();
            tabBranchLeg.ActiveTabIndex = 0;
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Edit";
            SaveLegalResponse(vStateEdit);
            LoadPendResponseList();
            tabBranchLeg.ActiveTabIndex = 0;
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {

            string vStateEdit = "Delete";
            SaveLegalResponse(vStateEdit);
            LoadPendResponseList();
            tabBranchLeg.ActiveTabIndex = 0;
        }
        private void SaveLegalResponse(string pMode)
        {
            int pLegQuaryId = 0, vErr = 0;
            string pLnAppId = "", pA1 = "", pA2 = "", pA3 = "", pA4 = "", pA5 = "";
            CApplication OCA = new CApplication();
            if (lblLegQuaryId.Text != "")
                pLegQuaryId = Convert.ToInt32(lblLegQuaryId.Text);
            pLnAppId = txtResLnAppNo.Text.ToString();
            if (pLnAppId == "")
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank");
                return;
            }
            if (txtResponseDt.Text == "")
            {
                gblFuction.AjxMsgPopup("Response Date Can Not Be Blank");
                return;
            }
            pA1 = txtA1.Text.ToString();
            pA2 = txtA2.Text.ToString();
            pA3 = txtA3.Text.ToString();
            pA4 = txtA4.Text.ToString();
            pA5 = txtA5.Text.ToString();
            DateTime pDate = gblFuction.setDate(txtResponseDt.Text);
            if (pMode == "Save")
            {
                vErr = OCA.SaveLegalResponse(pLegQuaryId, pLnAppId, pDate, pA1, pA2, pA3, pA4, pA5, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save");
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
                vErr = OCA.SaveLegalResponse(pLegQuaryId, pLnAppId, pDate, pA1, pA2, pA3, pA4, pA5, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Edit");
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
                vErr = OCA.SaveLegalResponse(pLegQuaryId, pLnAppId, pDate, pA1, pA2, pA3, pA4, pA5, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Delete");
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