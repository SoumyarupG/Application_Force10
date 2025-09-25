using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Configuration;
using System.Web.Script.Serialization;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Collections.Generic;
using CENTRUM.Service_Equifax;
using System.Xml;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class LoanUtilCheck : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                PopBranch();
                popRO();
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                    ddlBranch.Enabled = false;
                }
            }
        }

        #region "Drop down Index Change"
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            popRO();
            GetGrpByEo(ddlLo.SelectedValue);
        }
        protected void ddlLo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLo.SelectedIndex > 0) PopCenter(ddlLo.SelectedValue);
        }

        private void PopCenter(string vCOID)
        {
            DataTable dtGr = null;
            CGblIdGenerator oGbl = null;
            try
            {
                ddlCenter.Items.Clear();
                ddlCenter.SelectedIndex = -1;
                ddlGroup.Items.Clear();
                ddlGroup.SelectedIndex = -1;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oGbl = new CGblIdGenerator();
                //dtGr = oGbl.PopComboMIS("S", "N", "", "MarketID", "Market", "MarketMst", vCOID, "EOID", "DropoutDt", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);
                dtGr = oGbl.PopTransferMIS("Y", "MarketMst", vCOID, gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);
                dtGr.AcceptChanges();
                ddlCenter.DataSource = dtGr;
                ddlCenter.DataTextField = "Market";
                ddlCenter.DataValueField = "MarketID";
                ddlCenter.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlCenter.Items.Insert(0, oLi);
            }
            finally
            {
                dtGr = null;
                oGbl = null;
            }
        }
        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCenter.SelectedIndex > 0) PopGroup(ddlCenter.SelectedValue);
        }

        private void PopGroup(string vCenterID)
        {
            ddlGroup.Items.Clear();
            ddlGroup.SelectedIndex = -1;

            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("D", "N", "AA", "GroupID", "GroupName", "GroupMst", vCenterID, "MarketID", "Tra_DropDate", vLogDt, vBrCode);
                ddlGroup.DataSource = dt;
                ddlGroup.DataTextField = "GroupName";
                ddlGroup.DataValueField = "GroupID";
                ddlGroup.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlGroup.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
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
        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode;
            vBrCode = ddlBranch.SelectedValue.ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlLo.DataSource = dt;
                ddlLo.DataTextField = "EoName";
                ddlLo.DataValueField = "EoId";
                ddlLo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlLo.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }
        private void GetGrpByEo(string pEoId)
        {
            DataTable dt = null;
            CCollectionRoutine oGb = null;
            try
            {
                oGb = new CCollectionRoutine();
                dt = oGb.GetGrpByEo(pEoId, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                if (dt.Rows.Count > 0)
                {
                    ddlGroup.DataSource = dt;
                    ddlGroup.DataTextField = "GroupName";
                    ddlGroup.DataValueField = "Groupid";
                    ddlGroup.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlGroup.Items.Insert(0, oli);
                }
                else
                {
                    ddlGroup.Items.Clear();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlGroup.Items.Insert(0, oli);
                }
            }
            finally
            {
                dt = null;
                oGb = null;
            }
        }
        private void LoadGrid()
        {
            string vBrchCode = null, vGroupId = null;
            CLoanUtil oGbl = null;
            DataTable dt = null;
            DateTime vDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrchCode = ddlBranch.SelectedValue.ToString();
                vGroupId = ddlGroup.SelectedIndex > 0 ? Convert.ToString(ddlGroup.SelectedValue) : "";

                if (vGroupId == "-1" || vGroupId == "")
                {
                    gblFuction.AjxMsgPopup("No Group Selected...");
                    return;
                }

                if ((vGroupId == "") && (hdGrpId.Value != "-1"))
                    vGroupId = hdGrpId.Value;

                if (vBrchCode == "-1" || vBrchCode == "")
                {
                    gblFuction.AjxMsgPopup("No Branch Selected...");
                    return;
                }



                oGbl = new CLoanUtil();
                dt = oGbl.GetLoanUtilityCheck(vBrchCode, vGroupId, vDate);
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
            DataTable dt, dt1 = null;
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

                        oGbl = new CLoanUtil();
                        dt1 = new DataTable();
                        dt1 = oGbl.GetLUCQsAnsAS(Convert.ToInt32(dt.Rows[0]["LoanUtilizationID"]));
                        gvQA.DataSource = dt1;
                        gvQA.DataBind();
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
                                                             ddlLucDaoneVia.SelectedValue.Trim(), Convert.ToInt32(txtUtilizationAmt.Text.Trim()),
                                                             gblFuction.setDate(txtVerificationDate.Text), chkIsSamePurpose.Checked == true ? true : false);
                if (_returnval > 0)
                {
                    Resetvalues();
                    LoadGrid();
                    gblFuction.MsgPopup("Loan Utilization update successfully...");
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

        protected void gvQA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string vAnsType = "", vAns = "";
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlAns = (DropDownList)e.Row.FindControl("ddlAns");
                    TextBox txtAns = (TextBox)e.Row.FindControl("txtAns");
                    vAnsType = e.Row.Cells[3].Text.Trim();
                    vAns = e.Row.Cells[4].Text.Trim();
                    if (vAnsType == "Manual")
                    {
                        txtAns.Visible = true;
                        ddlAns.Visible = false;
                        txtAns.Text = vAns;
                    }
                    else
                    {
                        string[] QnsArr = vAns.Split(',');
                        txtAns.Visible = false;
                        ddlAns.Visible = true;
                        ddlAns.DataSource = QnsArr;
                        ddlAns.DataBind();
                    }
                }
            }
            finally
            {
            }
        }
    }
}