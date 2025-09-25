using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Admin
{
    public partial class LoanApplUpdate : CENTRUMBase
    {
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
                txtDtApp.Enabled = false;
                txtDtApp.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                tbCnt.ActiveTabIndex = 0;
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
                this.PageHeading = "Loan Application Update";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuAppUpdate);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                //{

                //    btnSave.Visible = false;
                //}
                //else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                //{
                //    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Application Update", false);
                //}
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
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


        protected void btnShow_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CLoan oLD = null;
            string vBrCode = string.Empty;
            string vLoanAppNo = txtApplicationNo.Text.ToString().Replace("'", "");
            vBrCode = (string)Session[gblValue.BrnchCode];
            oLD = new CLoan();
            dt = oLD.GetLoanAppDtlForUpdate(vLoanAppNo, vBrCode);

            if (dt.Rows.Count == 1)
            {
                ViewState["LoanAppno"] = vLoanAppNo;

                ViewState["LoanAppId"] = dt.Rows[0]["LOANAPPID"].ToString();
                txtDtApp.Text = dt.Rows[0]["APPDATE"].ToString();
                txtAppAmount.Text = Convert.ToString(dt.Rows[0]["LOANAPPAMT"]);
                txtSancAmount.Text = Convert.ToString(dt.Rows[0]["APPROVEDAMT"]);
                txtMemberNo.Text = dt.Rows[0]["MEMBERNO"].ToString();
                txtMemberName.Text = dt.Rows[0]["MEMBERNAME"].ToString();
                txtNEFT.Text = dt.Rows[0]["IsNEFTYN"].ToString();
                ddlNEFT.SelectedIndex = ddlNEFT.Items.IndexOf(ddlNEFT.Items.FindByValue(dt.Rows[0]["IsNEFTApprvYN"].ToString()));
                txtApproved.Text = dt.Rows[0]["APPROVED"].ToString();
                txtExpDisbDt.Text = dt.Rows[0]["EXPDATE"].ToString();
                ddlRTD.SelectedIndex = ddlRTD.Items.IndexOf(ddlRTD.Items.FindByValue(dt.Rows[0]["RtoD"].ToString()));

                ddlCancel.SelectedIndex = ddlCancel.Items.IndexOf(ddlCancel.Items.FindByValue(dt.Rows[0]["CANCEL"].ToString()));

               
                

                if (dt.Rows[0]["RtoD"].ToString() == "Y")
                {
                    txtExpDisbDt.Enabled = false;
                }
                else
                {
                    txtExpDisbDt.Enabled = true;
                }

                if (dt.Rows[0]["CANCEL"].ToString() == "Y")
                {
                    txtExpDisbDt.Enabled = false;
                    txtExpDisbDt.Text = "";
                    ddlRTD.SelectedIndex = ddlRTD.Items.IndexOf(ddlRTD.Items.FindByValue("N"));
                    ddlRTD.Enabled = false;
                }
                else
                {
                    txtExpDisbDt.Enabled = true;
                    ddlRTD.Enabled = true;
                }

                if (dt.Rows[0]["IsNEFTYN"].ToString() == "Y")
                {
                    txtExpDisbDt.Enabled = true;
                    ddlRTD.Enabled = true;
                    ddlCancel.Enabled = true;
                    ddlNEFT.Enabled = true;

                }
                else
                {
                    txtExpDisbDt.Enabled = true;
                    ddlRTD.Enabled = true;
                    ddlCancel.Enabled = true;
                    txtNEFT.Text = "N";
                    ddlNEFT.Enabled = false;
                }

            }
            else
            {
                ViewState["LoanAppno"] = null;
                ViewState["LoanAppId"] = null;

                txtDtApp.Text = Session[gblValue.LoginDate].ToString();
                txtAppAmount.Text = "";
                txtSancAmount.Text = "";
                txtMemberNo.Text = "";
                txtMemberName.Text = "";
                txtApproved.Text = "";
                txtExpDisbDt.Text = "";
                txtExpDisbDt.Enabled = true;
                ddlRTD.Enabled = true;
                ddlRTD.SelectedIndex = -1;
                ddlCancel.SelectedIndex = -1;
                txtExpDisbDt.Enabled = true;
                ddlRTD.Enabled = true;
                ddlCancel.Enabled = true;
                ddlNEFT.Enabled = true;
                txtNEFT.Text = "";
                gblFuction.AjxMsgPopup("Loan Application No Is Invalid/Loan Already Been Disbursed/Duplicate Loan Application No. ");
                return;
            }
        }

        protected void ddlRTD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRTD.SelectedValue == "Y")
            {
                txtExpDisbDt.Enabled = false;

            }
            else
            {
                txtExpDisbDt.Enabled = true;
            }

        }

        protected void ddlCancel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCancel.SelectedValue == "Y")
            {
                txtExpDisbDt.Enabled = false;
                txtExpDisbDt.Text = "";
                ddlRTD.SelectedIndex = ddlRTD.Items.IndexOf(ddlRTD.Items.FindByValue("N"));
                ddlRTD.Enabled = false;
            }
            else
            {
                txtExpDisbDt.Enabled = true;
                ddlRTD.Enabled = true;
                txtExpDisbDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveRecords() == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                ViewState["LoanAppno"] = null;
                ViewState["LoanAppId"] = null;

                txtDtApp.Text = Session[gblValue.LoginDate].ToString();
                txtAppAmount.Text = "";
                txtSancAmount.Text = "";
                txtMemberNo.Text = "";
                txtMemberName.Text = "";
                txtApproved.Text = "";
                txtExpDisbDt.Text = "";
                txtExpDisbDt.Enabled = true;
                ddlRTD.Enabled = true;
                ddlRTD.SelectedIndex = -1;
                ddlCancel.SelectedIndex = -1;
                txtExpDisbDt.Enabled = true;
                ddlRTD.Enabled = true;
                ddlNEFT.Enabled = true;
                txtNEFT.Text = "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords()
        {
            string vStateAppNo = Convert.ToString(ViewState["LoanAppno"]);
            string vStateAppId = Convert.ToString(ViewState["LoanAppId"]);

            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vIsPostPond = "";
            string vErrReason = "";
            Int32 vErr = 0;

            if (vStateAppNo != txtApplicationNo.Text.ToString())
            {
                gblFuction.MsgPopup("You have Changed Loan Application No After Clicking Show Button!");
                vResult = false;

            }
            CLoan oCent = null;

            if (chkIsPOstPond.Checked == true)
            {
                vIsPostPond = "Y";
            }
            else
            {
                vIsPostPond = "N";
            }

            try
            {

                oCent = new CLoan();
                vErr = oCent.UpdateLoanApplication(vBrCode, vStateAppId, this.UserID, gblFuction.setDate(txtExpDisbDt.Text), ddlRTD.SelectedValue, ddlCancel.SelectedValue, 
                                                   ddlNEFT.SelectedValue, vIsPostPond, ref  vErrReason);
                if (vErr == 1)
                {
                    vResult = true;
                }

                if (vErr ==9999)
                {
                    gblFuction.AjxMsgPopup(vErrReason);
                    vResult = false;
                }

                if (vErr == 0)
                {
                    gblFuction.MsgPopup(gblMarg.DBError);
                    vResult = false;
                }


                return vResult;
            }
            finally
            {
                oCent = null;
            }
        }

    }
}