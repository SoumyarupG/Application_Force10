using System;
using System.IO;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class EditFundSource : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["Funder"] = null;
                txtFrmDt.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                txtToDt.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                popBranch();
                popFunder();
                btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Edit Source of Fund";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuEdtSorceFnd);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Edit Source of Fund", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        //private void CalculateTotal()
        //{
        //    double DrAmt = 0.00, CrAmt = 0.00;
        //    for (Int32 i = 0; i < gvSecRef.Rows.Count; i++)
        //    {
        //        DrAmt = Math.Round(DrAmt, 2) + Math.Round(Convert.ToDouble(gvSecRef.Rows[i].Cells[8].Text), 2);
        //        CrAmt = Math.Round(CrAmt, 2) + Math.Round(Convert.ToDouble(gvSecRef.Rows[i].Cells[9].Text), 2);
        //    }
        //    txtDisAmt.Text = Convert.ToString(DrAmt);
        //    txtPOS.Text = Convert.ToString(CrAmt);
        //}
        private void LoadGrid(string vFunderId)
        {
            //string vFunId = ddlFunder.SelectedValue.ToString();
            string vBrCode = ddlBranch.SelectedValue.ToString();
            if (txtFrmDt.Text == "")
            {
                gblFuction.AjxMsgPopup("From Date Can Not Be Blank..");
                return;
            }
            if (txtToDt.Text == "")
            {
                gblFuction.AjxMsgPopup("To Date Can Not Be Blank..");
                return;
            }
            DateTime vFromDt = gblFuction.setDate(txtFrmDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            CFundSource oFs = null;
            DataTable dt = null;
            try
            {
                oFs = new CFundSource();
                dt = oFs.GetAllEditFundSource(vFunderId, vBrCode, vFromDt, vToDt);
                if (dt.Rows.Count > 0)
                {
                    gvSecRef.DataSource = dt;
                    gvSecRef.DataBind();
                }
                else
                {
                    gvSecRef.DataSource = null;
                    gvSecRef.DataBind();
                }
                ViewState["Sanc"] = dt;
            }
            finally
            {
                dt = null;
                oFs = null;
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(ddlFunder.SelectedValue.ToString());
            //CalculateTotal();
        }
        protected void gvSecRef_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = null;
            CLoanRecovery oCD = null;
            oCD = new CLoanRecovery();
            dt = oCD.GetFunder();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlFunder = (DropDownList)e.Row.FindControl("ddlFunSource");
                ddlFunder.DataSource = dt;
                ddlFunder.DataTextField = "FunderName";
                ddlFunder.DataValueField = "FunderId";
                ddlFunder.DataBind();
                ddlFunder.SelectedIndex = ddlFunder.Items.IndexOf(ddlFunder.Items.FindByValue(e.Row.Cells[0].Text.Trim()));
            }
        }
        private DataTable CreateXmlData()
        {
            DataTable dt = new DataTable("Funder");
            dt.Columns.Add("LoanId");
            dt.Columns.Add("FundAllocationDt");
            dt.Columns.Add("FunderId");
            dt.Columns.Add("BranchCode");
            DataRow dr;
            foreach (GridViewRow gr in gvSecRef.Rows)
            {
                if (((CheckBox)gr.FindControl("cbApprv")).Checked == true)
                {
                    DropDownList ddlFunder = (DropDownList)gr.FindControl("ddlFunSource");
                    dr = dt.NewRow();
                    dr["LoanId"] = gr.Cells[6].Text;
                    dr["FundAllocationDt"] = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                    dr["FunderId"] = ddlFunder.SelectedValue;
                    dr["BranchCode"] = gr.Cells[1].Text;
                    dt.Rows.Add(dr);
                }
            }
            dt.AcceptChanges();
            return dt;
        }
        private bool ValidateFields()
        {
            bool vRes = true;
            if (gvSecRef.Rows.Count <= 0)
            {
                gblFuction.MsgPopup("Nothing Found to be Saved ..");
                return vRes = false;
            }
            return vRes;
        }
        private void SaveRecords()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            if (ValidateFields() == false)
                return;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            CFundSource oFS = null;
            DataTable dtXml = CreateXmlData();
            int vErr = 0;
            string vXml = "";
            try
            {
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString();
                }
                oFS = new CFundSource();
                vErr = oFS.UpdateFunder(vXml, this.UserID, vBrCode);
                if (vErr > 0)
                {
                    gblFuction.MsgPopup("Record Updated Successfully");
                    return;
                }
                else
                {
                    gblFuction.MsgPopup("Error: Data not Saved");
                    return;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtXml = null;
                oFS = null;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecords();
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        private void popBranch()
        {
            DataTable dt = null;
            CUser oUsr = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUserLogin(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
                if (vBrCode == "0000")
                {
                    ListItem oli = new ListItem("All", "A");
                    ddlBranch.Items.Insert(0, oli);
                }
            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }
        private void popFunder()
        {
            DataTable dt = null;
            CLoanRecovery oCD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oCD = new CLoanRecovery();
                dt = oCD.GetFunder();
                ddlFunder.DataSource = dt;
                ddlFunder.DataTextField = "FunderName";
                ddlFunder.DataValueField = "FunderId";
                ddlFunder.DataBind();
                ListItem oli = new ListItem("All", "A");
                ddlFunder.Items.Insert(0, oli);
            }
            finally
            {
                oCD = null;
                dt = null;
            }
        }
        protected void cbApprvSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk;
            foreach (GridViewRow rowItem in gvSecRef.Rows)
            {
                chk = (CheckBox)(rowItem.Cells[11].FindControl("cbApprv"));
                chk.Checked = ((CheckBox)sender).Checked;
            }
        }
    }
}
