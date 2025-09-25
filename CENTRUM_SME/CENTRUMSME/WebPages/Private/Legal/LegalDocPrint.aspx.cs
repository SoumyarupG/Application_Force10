using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CENTRUMCA;
using CENTRUMBA;
using System.Drawing;

namespace CENTRUMSME.WebPages.Private.Legal
{
    public partial class LegalDocPrint : CENTRUMBAse
    {
        protected int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch();
                popCustomer();
            }
        }
        private void PopBranch()
        {
            CMember oCM = null;
            DataTable dt = null;
            oCM = new CMember();
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                dt = oCM.GetBranch();
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataBind();
                    if (Session[gblValue.BrnchCode].ToString() == "0000")
                    {
                        ListItem liSel = new ListItem("ALL", "0000");
                        ddlBranch.Items.Insert(0, liSel);
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
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Legal Documents Print";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuLegalDocPrint);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Legal Documents Print", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void popCustomer()
        {
            DataTable dt = null;
            CDisburse oCD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oCD = new CDisburse();
                dt = oCD.GetCustNameForLegAgreement(ddlBranch.SelectedValue.ToString());
                ddlCust.DataSource = dt;
                ddlCust.DataTextField = "CompanyName";
                ddlCust.DataValueField = "CustId";
                ddlCust.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCust.Items.Insert(0, oli);
            }
            finally
            {
                oCD = null;
                dt = null;
            }
        }
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            popCustomer();
        }
        protected void ddlCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pCustId = (Request[ddlCust.UniqueID] as string == null) ? ddlCust.SelectedValue : Request[ddlCust.UniqueID] as string;
            if (pCustId != "-1")
                PopSanctionNo(pCustId);
        }
        protected void PopSanctionNo(string pCustId)
        {
            CDisburse oMem = new CDisburse();
            DataTable dt = new DataTable();
            oMem = new CDisburse();
            dt = oMem.GetSancIdForAgreement(pCustId);
            ddlSancNo.Items.Clear();
            if (dt.Rows.Count > 0)
            {
                ddlSancNo.DataSource = dt;
                ddlSancNo.DataTextField = "SanctionNo";
                ddlSancNo.DataValueField = "SanctionID";
                ddlSancNo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlSancNo.Items.Insert(0, oli);
            }
            else
            {
                gblFuction.AjxMsgPopup("Final Sanction is not complete for that Customer....");
                ddlSancNo.DataSource = null;
                ddlSancNo.DataBind();
                return;
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
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            //switch (e.CommandName)
            //{
            //    case "Prev":
            //        vPgNo = Int32.Parse(lblCrPg.Text) - 1;
            //        break;
            //    case "Next":
            //        vPgNo = Int32.Parse(lblCrPg.Text) + 1;
            //        break;
            //}
            //PendingAgrList(vPgNo);
            //tbEmp.ActiveTabIndex = 0;
        }
        protected void lbMODTNew_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                DataSet ds = new DataSet();
                CMember Omem = new CMember();
                ds = Omem.GetMODTDPrintBySancId(vSancId);
                if (ds.Tables.Count > 0)
                {
                    Session["MODTDPrint"] = ds;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('MODTDPrint.aspx','MODT','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;

                }


            }

        }
        protected void lbIOMMAS_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            { //\\DC01\palash.b\CENTRUM_SME\CENTRUMSME\WebPages\Private\Agreement\MortgageRegisterPRATAM.aspx
                DataSet ds = new DataSet();
                CMember Omem = new CMember();
                ds = Omem.GetMortgageRegisterBySancId(vSancId);
                if (ds.Tables.Count > 0)
                {
                    Session["MortgRegis"] = ds;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('../../../WebPages/Private/Agreement/MortgageRegisterMAS.aspx','INDENTURE OF MORTGAGE(MAS)','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;
                }
            }

        }
        protected void lbIOMPRATAM_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                DataSet ds = new DataSet();
                CMember Omem = new CMember();
                ds = Omem.GetMortgageRegisterBySancId(vSancId);
                if (ds.Tables.Count > 0)
                {
                    Session["MortgRegis"] = ds;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('../../../WebPages/Private/Agreement/MortgageRegisterPRATAM.aspx','INDENTURE OF MORTGAGE(CENTRUMSME)','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;
                }
            }

        }
        protected void lbMODT_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                DataSet ds = new DataSet();
                CMember Omem = new CMember();
                ds = Omem.GetMODTBySancId(vSancId);
                if (ds.Tables.Count > 0)
                {
                    Session["MODT"] = ds;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('MODT.aspx','MODT','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;

                }
            }
        }
        protected void lbLOA_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                DataSet ds = new DataSet();
                CMember Omem = new CMember();
                DateTime pLOAGenDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                ds = Omem.GetLOARecordByLoanAppId(vSancId, pLOAGenDate);
                if (ds.Tables.Count > 0)
                {
                    Session["LOAPrint"] = ds;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('LOAPrint.aspx','LOA','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;

                }
            }
        }
        protected void lbLOD_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                DataSet ds = new DataSet();
                CMember Omem = new CMember();
                DateTime pLODGenDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                ds = Omem.GetLODRecordByLoanAppId(vSancId, pLODGenDate);
                if (ds.Tables.Count > 0)
                {
                    Session["LODPrint"] = ds;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('LODPrint.aspx','LOD','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;
                }
            }
        }
    }
}