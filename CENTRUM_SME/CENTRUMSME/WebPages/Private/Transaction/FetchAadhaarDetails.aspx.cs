using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class FetchAadhaarDetails : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Fetch Aadhaar Details";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuFetchAadhaarDetails);
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

        protected void btnGo_Click(object sender, EventArgs e)
        {
            //string vLoanNo = Convert.ToString(txtLnNo.Text.Trim());
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            //DataTable dt = null;
            //CActivity oActiv = null;

            //dt = new DataTable();
            //oActiv = new CActivity();

            //if (txtLnNo.Text.Trim() == "")
            //{
            //    gblFuction.MsgPopup("Please Give a Loan No...");
            //    return;
            //}

            //dt = oActiv.GetDetailsForRiskCat(vLoanNo, vBrCode);
            //if (dt.Rows.Count > 0)
            //{
            //    lblMemberNo.Text = dt.Rows[0]["MemberID"].ToString();
            //    lblMemName.Text = dt.Rows[0]["MemName"].ToString();
            //    lblExisRskcat.Text = dt.Rows[0]["ExisRskCat"].ToString();
            //    hdLoanId.Value = dt.Rows[0]["LoanId"].ToString();
            //}
            //else
            //{
            //    gblFuction.MsgPopup("No Record Found...");
            //    txtLnNo.Text = "";
            //    lblMemberNo.Text = "";
            //    lblMemName.Text = "";
            //    lblExisRskcat.Text = "";
            //    return;
            }
        }

    }
