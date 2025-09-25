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
    public partial class LegalResolveQuery : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                InitBasePage();
                //////hdUserID.Value = this.UserID.ToString();
                GetBranchForLegResolve();
                GetPenResolveList();
                // txtResponseDt.Text = Session[gblValue.LoginDate].ToString();
                txtResolveDt.Text = Session[gblValue.LoginDate].ToString();
              
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
                this.PageHeading = "Resolve Against Legal  Query";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLegalResolveQuery);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Resolve Against Legal  Query", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void GetBranchForLegResolve()
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
            GetPenResolveList();
        }
        private void GetPenResolveList()
        {
            DataTable dt = null;
            CApplication oCA = null;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                // vMode = rdbSel.SelectedValue;
                vBrCode = (string)Session[gblValue.BrnchCode];
                oCA = new CApplication();
                dt = oCA.GetPenResolveListNew(ddlSearchType.SelectedValue.ToString(), ddlLegBr.SelectedValue.ToString(), txtSearch.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    gvPenResolve.DataSource = dt;
                    gvPenResolve.DataBind();
                }
                else
                {
                    gvPenResolve.DataSource = null;
                    gvPenResolve.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        protected void gvPenResolve_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    dt = oCG.GetQueryResolveByAppId(vLnAppId, "P");
                    if (dt.Rows.Count > 0)
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvPenResolve.Rows)
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
                        txtResCustName.Text = dt.Rows[0]["CustName"].ToString();
                        txtResLnAppNo.Text = dt.Rows[0]["LoanAppId"].ToString();
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
                        if (dt.Rows[0]["LegalQueryResolveDate"].ToString() == "")
                            txtResolveDt.Text = Session[gblValue.LoginDate].ToString();
                        else
                            txtResolveDt.Text = dt.Rows[0]["LegalQueryResolveDate"].ToString();

                        if (dt.Rows[0]["Q1ResolveYN"].ToString() == "Y")
                            chkResolveQ1.Checked = true;
                        else
                            chkResolveQ1.Checked = false;
                        if (dt.Rows[0]["Q2ResolveYN"].ToString() == "Y")
                            chkResolveQ2.Checked = true;
                        else
                            chkResolveQ2.Checked = false;
                        if (dt.Rows[0]["Q3ResolveYN"].ToString() == "Y")
                            chkResolveQ3.Checked = true;
                        else
                            chkResolveQ3.Checked = false;
                        if (dt.Rows[0]["Q4ResolveYN"].ToString() == "Y")
                            chkResolveQ4.Checked = true;
                        else
                            chkResolveQ4.Checked = false;
                        if (dt.Rows[0]["Q5ResolveYN"].ToString() == "Y")
                            chkResolveQ5.Checked = true;
                        else
                            chkResolveQ5.Checked = false;
                        btnSave.Enabled = true;
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
            GetPenResolveList();
            tabBranchLeg.ActiveTabIndex = 0;
        }
        private void SaveLegalResponse(string pMode)
        {
            int pLegQuaryId = 0, vErr = 0;
            string pLnAppId = "", pLegalFinalQueryResolveYN = "N",
                 pRes1 = "", pRes2 = "", pRes3 = "", pRes4 = "", pRes5 = "";
            CApplication OCA = new CApplication();
            if (lblLegQuaryId.Text != "")
                pLegQuaryId = Convert.ToInt32(lblLegQuaryId.Text);
            pLnAppId = txtResLnAppNo.Text.ToString();
            if (pLnAppId == "")
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank");
                return;
            }
            if (txtResolveDt.Text == "")
            {
                gblFuction.AjxMsgPopup("Resolve Date Can Not Be Blank");
                return;
            }
            DateTime pDate = gblFuction.setDate(txtResolveDt.Text);

            string pQ1 = "", pQ2 = "", pQ3 = "", pQ4 = "", pQ5 = "";
            pQ1 = txtQ1.Text.ToString();
            pQ2 = txtQ2.Text.ToString();
            pQ3 = txtQ3.Text.ToString();
            pQ4 = txtQ4.Text.ToString();
            pQ5 = txtQ5.Text.ToString();
            if (pQ1 != "" && chkResolveQ1.Checked == true)
                pRes1 = "Y";
            else if (pQ1 != "" && chkResolveQ1.Checked == false)
                pRes1 = "N";
            else
                pRes1 = "";

            if (pQ2 != "" && chkResolveQ2.Checked == true)
                pRes2 = "Y";
            else if (pQ2 != "" && chkResolveQ2.Checked == false)
                pRes2 = "N";
            else
                pRes2 = "";

            if (pQ3 != "" && chkResolveQ3.Checked == true)
                pRes3 = "Y";
            else if (pQ3 != "" && chkResolveQ3.Checked == false)
                pRes3 = "N";
            else
                pRes3 = "";

            if (pQ4 != "" && chkResolveQ4.Checked == true)
                pRes4 = "Y";
            else if (pQ4 != "" && chkResolveQ4.Checked == false)
                pRes4 = "N";
            else
                pRes4 = "";
            if (pQ5 != "" && chkResolveQ5.Checked == true)
                pRes5 = "Y";
            else if (pQ5 != "" && chkResolveQ5.Checked == false)
                pRes5 = "N";
            else
                pRes5 = "";

            pLegalFinalQueryResolveYN = FinalLegalResolveStatus();

            if (pMode == "Save")
            {
                vErr = OCA.SaveLegalResolve(pLegQuaryId, pLnAppId, pDate, pRes1, pRes2, pRes3, pRes4, pRes5, pLegalFinalQueryResolveYN, "Save");
                if (vErr > 0)
                {
                    gblFuction.MsgPopup("Resolve Save Successfully");
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
                vErr = OCA.SaveLegalResolve(pLegQuaryId, pLnAppId, pDate, pRes1, pRes2, pRes3, pRes4, pRes5, pLegalFinalQueryResolveYN, "Edit");
                if (vErr > 0)
                {
                    gblFuction.MsgPopup("Resolve Update Successfully");
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
                vErr = OCA.SaveLegalResolve(pLegQuaryId, pLnAppId, pDate, pRes1, pRes2, pRes3, pRes4, pRes5, pLegalFinalQueryResolveYN, "Delete");
                if (vErr > 0)
                {
                    gblFuction.MsgPopup("Resolve Deleted Successfully");
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                    return;
                }
            }

        }
        private string FinalLegalResolveStatus()
        {
            string pFinalResolveStatus = "N", pQ1 = "", pQ2 = "", pQ3 = "", pQ4 = "", pQ5 = "";

            pQ1 = txtQ1.Text.ToString();
            pQ2 = txtQ2.Text.ToString();
            pQ3 = txtQ3.Text.ToString();
            pQ4 = txtQ4.Text.ToString();
            pQ5 = txtQ5.Text.ToString();
            if (pQ1 != "" && chkResolveQ1.Checked == true)
                pFinalResolveStatus = "Y";
            else if (pQ1 != "" && chkResolveQ1.Checked == false)
            {
                pFinalResolveStatus = "N";
                return pFinalResolveStatus;
            }
            if (pQ2 != "" && chkResolveQ2.Checked == true)
                pFinalResolveStatus = "Y";
            else if (pQ2 != "" && chkResolveQ2.Checked == false)
            {
                pFinalResolveStatus = "N";
                return pFinalResolveStatus;
            }
            if (pQ3 != "" && chkResolveQ3.Checked == true)
                pFinalResolveStatus = "Y";
            else if (pQ3 != "" && chkResolveQ3.Checked == false)
            {
                pFinalResolveStatus = "N";
                return pFinalResolveStatus;
            }
            if (pQ4 != "" && chkResolveQ4.Checked == true)
                pFinalResolveStatus = "Y";
            else if (pQ4 != "" && chkResolveQ4.Checked == false)
            {
                pFinalResolveStatus = "N";
                return pFinalResolveStatus;
            }
            if (pQ5 != "" && chkResolveQ5.Checked == true)
                pFinalResolveStatus = "Y";
            else if (pQ5 != "" && chkResolveQ5.Checked == false)
            {
                pFinalResolveStatus = "N";
                return pFinalResolveStatus;
            }
            return pFinalResolveStatus;
        }
    }
}