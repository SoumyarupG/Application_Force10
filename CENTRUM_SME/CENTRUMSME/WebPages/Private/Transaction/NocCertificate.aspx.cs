using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;
using System.Data;
using System.Web.Security;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class NocCertificate : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                PopBranch();
                popCustomer(ddlBranch.SelectedValue);
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "NOC Letter";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuNocLetter);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnPrint.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnPrint.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnPrint.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Sanction", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void PopBranch()
        {
            ddlBranch.Items.Clear();
            CMember oCM = new CMember();
            DataTable dt = new DataTable();
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                if (vBrCode == "0000")
                {
                    dt = oCM.GetBranchForNOC();
                    if (dt.Rows.Count > 0)
                    {
                        ddlBranch.DataSource = dt;
                        ddlBranch.DataTextField = "BranchName";
                        ddlBranch.DataValueField = "BranchCode";
                        ddlBranch.DataBind();
                        ListItem oItm = new ListItem("All", "A");
                        ddlBranch.Items.Insert(0, oItm);
                        ListItem oItm1 = new ListItem("<---Select--->", "-1");
                        ddlBranch.Items.Insert(0, oItm1);
                    }
                    else
                    {
                        ddlBranch.DataSource = null;
                        ddlBranch.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCM = null;
            }
        }
        private void popCustomer(string vBrCode)
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();

            try
            {
                dt = oDisb.GetCustForNOC(vBrCode, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));

                ddlLoan.Items.Clear();
                ddlCust.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlCust.DataSource = dt;
                    ddlCust.DataTextField = "CompanyName";
                    ddlCust.DataValueField = "CustId";
                    ddlCust.DataBind();
                }
                else
                {
                    ddlCust.DataSource = null;
                    ddlCust.DataBind();
                }
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCust.Items.Insert(0, oli);
            }
            finally
            {
                oDisb = null;
                dt = null;
            }
        }
        private void LoadLoanNo()
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();

            string vBranch = (Request[ddlBranch.UniqueID] as string == null) ? ddlBranch.SelectedValue : Request[ddlBranch.UniqueID] as string;
            string vCustId = (Request[ddlCust.UniqueID] as string == null) ? ddlCust.SelectedValue : Request[ddlCust.UniqueID] as string;
            try
            {
                dt = oDisb.GetLoanNoForNOC(vBranch, vCustId, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                ddlLoan.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlLoan.DataSource = dt;
                    ddlLoan.DataTextField = "LoanId";
                    ddlLoan.DataValueField = "LoanId";
                    ddlLoan.DataBind();
                }
                else
                {
                    ddlLoan.DataSource = null;
                    ddlLoan.DataBind();
                }
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlLoan.Items.Insert(0, oli);
            }
            finally
            {
                oDisb = null;
                dt = null;
            }
        }
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            popCustomer(ddlBranch.SelectedValue);
        }
        protected void ddlCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLoanNo();
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string vLoanNo = "";
            vLoanNo = (Request[ddlLoan.UniqueID] as string == null) ? ddlLoan.SelectedValue : Request[ddlLoan.UniqueID] as string;
            DataTable dt = new DataTable();
            if (vLoanNo == "-1")
            {
                gblFuction.MsgPopup("Please Select Loan No");
                return;
            }
            CDisburse oNoc = new CDisburse();
            try
            {
                dt = oNoc.PrintNOC(gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vLoanNo);
                if (dt.Rows.Count > 0)
                {
                    Session["NOC"] = dt;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('../Report/NOCPrint.aspx','NOC Letter','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Records Found");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oNoc = null;
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
            ViewState["LoanID"] = null;
        }
    }
}