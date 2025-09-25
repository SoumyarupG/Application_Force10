using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;
using System.IO;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class DeathCancelAfterVerification : CENTRUMBAse
    {
        protected int cPgNo = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                ViewState["DeathDoc"] = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                PopBranch();

            }
        }
        private void PopBranch()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            vBrCode = (string)Session[gblValue.BrnchCode];
            vBrId = Convert.ToInt32(vBrCode);
            oCG = new CGblIdGenerator();
            dt = oCG.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
            if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToString(row["BranchCode"]) != Convert.ToString(Session[gblValue.BrnchCode]))
                    {
                        row.Delete();
                    }
                }
                dt.AcceptChanges();
            }
            ddlBranch.DataSource = dt;
            ddlBranch.DataTextField = "BranchName";
            ddlBranch.DataValueField = "BranchCode";
            ddlBranch.DataBind();
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                ListItem Li = new ListItem("<-- Select -->", "-1");
                ddlBranch.Items.Insert(0, Li);
                ddlBranch.Enabled = true;
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "cancel After Verification";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDeathDoccancelSME);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Provisional Death Declare", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                string vBrCode = ddlBranch.SelectedValue.ToString();
                LoadGrid(txtSearch.Text, vBrCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void LoadGrid(string pSearch, string pBranch)
        {
            DataTable dt = null;
            CDeathRelated oDr = null;
            try
            {
                string vBrCode = pBranch;
                oDr = new CDeathRelated();
                dt = oDr.GetDeath_Doc_Cancel_Aft_Approval(pSearch, vBrCode, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                ViewState["DeathDoc"] = dt;
                if (dt.Rows.Count > 0)
                {
                    hdLoanId.Value = Convert.ToString(dt.Rows[0]["LoanId"]);
                }
                gvDoc.DataSource = dt;
                gvDoc.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oDr = null;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            CDeathRelated oDr = null;
            DataTable dt = new DataTable();
            Int32 vErr = 0;
           
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            if (this.RoleId != 1)
            {
                if (Session[gblValue.EndDate] != null)
                {
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= vLoginDt)
                    {
                        gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                        return;
                    }

                }
            }


            try
            {
                dt = (DataTable)ViewState["DeathDoc"];
                if (dt == null)
                {
                    gblFuction.MsgPopup("No Data to Save");
                    return;
                }
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oDr = new CDeathRelated();
                
                vErr = oDr.SaveDeath_Doc_Cancel_Aft_Approval(hdLoanId.Value, vLoginDt, this.UserID);
                if (vErr > 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    LoadGrid(txtSearch.Text, ddlBranch.SelectedValue.ToString());
                    ViewState["DeathDoc"] = null;
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                }
            }
            finally
            {
                oDr = null;
                dt = null;
            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
    }
}