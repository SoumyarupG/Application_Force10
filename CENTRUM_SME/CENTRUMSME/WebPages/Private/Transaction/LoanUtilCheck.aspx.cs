using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using System.Data;
using CENTRUMBA;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class LoanUtilCheck : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                PopBranch();               
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                    ddlBranch.Enabled = false;
                }
            }
        }

        #region "Drop down Index Change"             
       
        protected void ddlUtilizationType_Change(object sender, EventArgs e)
        {
            String vUtilizationType = "";
            DataTable dt = null;
            CLoanUtil oGbl;
            try
            {
                oGbl = new CLoanUtil();
                vUtilizationType = ddlUtilizationType.SelectedValue.ToString();
                if ((vUtilizationType == "") && (vUtilizationType == "0"))
                {
                    gblFuction.AjxMsgPopup("No Utilization Type Selected...");
                    return;
                }

                if (vUtilizationType.ToUpper() == "FULLY USED" || vUtilizationType.ToUpper() == "NOT USED")
                {
                    txtUtilizationAmt.Enabled = false;
                    if (hfLoanId.Value != "")
                    {
                        dt = oGbl.GetLoanUtilityCheckByLoanNo(hfLoanId.Value);
                        if (dt.Rows.Count > 0)
                            txtUtilizationAmt.Text = vUtilizationType.ToUpper() == "FULLY USED" ? dt.Rows[0]["LoanAmt"].ToString() : "0";
                        else
                            txtUtilizationAmt.Text = "0";
                    }
                    else
                        txtUtilizationAmt.Text = "0";
                }
                else
                {
                    txtUtilizationAmt.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                dt = null;
            }
        }
        #endregion

        #region "Populate Operations"
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Loan Utilization Check";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuLoanUtilChk);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void PopBranch()
        {
            CGblIdGenerator oGb = null;
            DataTable dt = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), Session[gblValue.BrnchCode].ToString());
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }   
        private void LoadGrid()
        {
            string vBrchCode = null;
            CLoanUtil oGbl = null;
            DataTable dt = null;
            DateTime vDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrchCode = ddlBranch.SelectedValue.ToString();                             

                if (vBrchCode == "-1" || vBrchCode == "")
                {
                    gblFuction.AjxMsgPopup("No Branch Selected...");
                    return;
                }
                if (hdnCustId.Value == "")
                {
                    gblFuction.AjxMsgPopup("Please Enter Member No.");
                    return;
                }
                oGbl = new CLoanUtil();
                dt = oGbl.GetLoanUtilityCheck(vBrchCode,hdnCustId.Value, vDate);
                gvLoanUtilCheck.DataSource = dt;
                gvLoanUtilCheck.DataBind();
            }
            finally
            {
                oGbl = null;
                dt = null;
            }
        }
        private void Resetvalues()
        {
            txtLoanAccountId.Text = "";
            ddlLucDaoneVia.SelectedValue = "0";
            txtLoanPurpose.Text = "";
            txtLoanSubPurpose.Text = "";
            ddlUtilizationType.SelectedValue = "0";
            txtUtilizationAmt.Text = "";
            txtVerifiedBy.Text = "";
            txtVerificationDate.Text = "";
            txtRemarks.Text = "";
            txtLoanDt.Text = "";
        }
        #endregion

        #region "Click Operation"
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
            Resetvalues();
        }
        protected void gvLoanUtilCheck_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String pLoanId = "";
            DataTable dt = null;
            CLoanUtil oGbl = null;
            try
            {
                oGbl = new CLoanUtil();
                pLoanId = e.CommandArgument.ToString();
                if (e.CommandName == "cmdEdit")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton lnkEdit = (LinkButton)gvRow.FindControl("cmdEdit");
                    Label lblLoanId = (Label)gvRow.FindControl("lblLoanId");
                    Label lblLoanAmt = (Label)gvRow.FindControl("lblLoanAmt");
                    hfMaxUtilizedAmt.Value = lblLoanAmt.Text.Trim();
                    pLoanId = lblLoanId.Text.Trim();
                    hfLoanId.Value = pLoanId;
                    foreach (GridViewRow gr in gvLoanUtilCheck.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("cmdEdit");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    lnkEdit.ForeColor = System.Drawing.Color.Red;

                    dt = oGbl.GetLoanUtilityCheckByLoanNo(pLoanId);
                    if (dt.Rows.Count > 0)
                    {
                        txtLoanAccountId.Text = Convert.ToString(dt.Rows[0]["LoanNo"]);
                        ddlLucDaoneVia.SelectedValue = dt.Rows[0]["LoanUTLVia"].ToString() == "" ? "0" : dt.Rows[0]["LoanUTLVia"].ToString();
                        txtLoanPurpose.Text = dt.Rows[0]["Purpose"].ToString();
                        txtLoanSubPurpose.Text = dt.Rows[0]["SubPurpose"].ToString();
                        ddlUtilizationType.SelectedValue = dt.Rows[0]["LoanUTLType"].ToString() == "" ? "0" : dt.Rows[0]["LoanUTLType"].ToString();
                        txtUtilizationAmt.Text = dt.Rows[0]["LoanUTLAmt"].ToString();
                        if (dt.Rows[0]["LoanUTLType"].ToString() != "")
                            txtUtilizationAmt.Enabled = (dt.Rows[0]["LoanUTLType"].ToString().ToUpper() == "FULLY USED" || dt.Rows[0]["LoanUTLType"].ToString().ToUpper() == "NOT USED") ? false : true;
                        txtVerifiedBy.Text = Session[gblValue.UserName].ToString();
                        txtRemarks.Text = dt.Rows[0]["LoanUTLRemarks"].ToString();
                        txtVerificationDate.Text = dt.Rows[0]["VerificationDate"].ToString();
                        chkIsSamePurpose.Checked = Convert.ToBoolean(dt.Rows[0]["IssamePurpose"]) == true ? true : false;
                        txtLoanDt.Text = dt.Rows[0]["LoanDate"].ToString();
                    }
                }
            }
            finally
            {
                dt = null;
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            CLoanUtil oGbl = null;
            Int32 _returnval = 0;
            try
            {
                oGbl = new CLoanUtil();
                if (txtLoanAccountId.Text.Length == 0)
                {
                    gblFuction.AjxMsgPopup("Loan Account Id can not be blank..");
                    return;
                }
                if (ddlLucDaoneVia.SelectedValue == "0")
                {
                    gblFuction.AjxMsgPopup("No Luc Done Via Selected..");
                    return;
                }
                if (ddlUtilizationType.SelectedValue == "0")
                {
                    gblFuction.AjxMsgPopup("No Utilization Type Selected...");
                    return;
                }
                if (txtUtilizationAmt.Text.Length == 0)
                {
                    gblFuction.AjxMsgPopup("Loan Utilization Amount can not be blank..");
                    return;
                }
                if (ddlUtilizationType.SelectedValue.ToUpper() == "PARTIALLY USED")
                {
                    if (Convert.ToInt32(txtUtilizationAmt.Text) > Convert.ToInt32(hfMaxUtilizedAmt.Value))
                    {
                        gblFuction.AjxMsgPopup("Loan Utilization amount can not more than loan amount ..");
                        return;
                    }
                }
                if (txtVerifiedBy.Text.Length == 0)
                {
                    gblFuction.AjxMsgPopup("Verified by can not be blank..");
                    return;
                }
                if (txtVerificationDate.Text.Length == 0)
                {
                    gblFuction.AjxMsgPopup("Verification Date can not be blank..");
                    return;
                }
                if ((gblFuction.setDate(txtVerificationDate.Text) - gblFuction.setDate(txtLoanDt.Text)).TotalDays < 7)
                {
                    gblFuction.AjxMsgPopup("Between Loan Date and Utilization check date there must be 7 days gap..");
                    return;
                }
                _returnval = oGbl.UpdateLoanUtilizationCheck(hfLoanId.Value.ToString().Trim(), ddlUtilizationType.SelectedValue.ToString().Trim(),
                                                             Convert.ToInt32(Session[gblValue.UserId].ToString()), txtRemarks.Text.Trim(),
                                                             ddlLucDaoneVia.SelectedValue.Trim(), Convert.ToDouble(txtUtilizationAmt.Text.Trim()),
                                                             gblFuction.setDate(txtVerificationDate.Text), chkIsSamePurpose.Checked == true ? true : false);
                if (_returnval > 0)
                {
                    Resetvalues();
                    LoadGrid();
                    gblFuction.AjxMsgPopup("Loan Utilization update successfully...");                    
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        #endregion
    }
}