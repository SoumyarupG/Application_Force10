using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;
using SendSms;
using System.Web.UI;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Xml;
using System.Collections.Generic;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class RiskCatChng : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Risk Category Change";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuRiskCatChngMEL);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Risk Category Change", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string vLoanNo = Convert.ToString(txtLnNo.Text.Trim());
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DataTable dt = null;
            CLoan oLoan = null;

            dt = new DataTable();
            oLoan = new CLoan();

            if (txtLnNo.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please Give a Loan No...");
                return;
            }

            dt = oLoan.GetDetailsForRiskCat(vLoanNo, vBrCode);
            if (dt.Rows.Count > 0)
            {
                lblMemberNo.Text = dt.Rows[0]["MemberID"].ToString();
                lblMemName.Text = dt.Rows[0]["MemName"].ToString();
                lblExisRskcat.Text = dt.Rows[0]["ExisRskCat"].ToString();
                hdLoanId.Value = dt.Rows[0]["LoanId"].ToString();
            }
            else
            {
                gblFuction.MsgPopup("No Record Found...");
                txtLnNo.Text = "";
                lblMemberNo.Text = "";
                lblMemName.Text = "";
                lblExisRskcat.Text = "";
                return;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vLoanId = hdLoanId.Value.ToString();
            string vRiskVal = ddlRiskCat.SelectedValue.ToString();
            //Boolean vResult = false;
            Int32 vErr = 0;
            CLoan oLoan = null;
            oLoan = new CLoan();
            try
            {
                if (txtLnNo.Text.Trim() == "")
                {
                    gblFuction.MsgPopup("Please Give a Loan No...");
                    return;
                }

                if (ddlRiskCat.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Please Select A Risk Category Option...");
                    return;
                }


                vErr = oLoan.SaveRiskCategory(vLoanId, vRiskVal);
                if (vErr > 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully...");
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup("Record Not Save Successfully...");
                    return;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                oLoan = null;
            }

        }
    }
}