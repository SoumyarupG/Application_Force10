using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;
using SendSms;
using System.Web.UI;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class ProvisionalDeathDec : CENTRUMBAse
    {
        protected int cPgNo = 0;

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
                ViewState["StateEdit"] = null;
                ViewState["PDDMember"] = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                PopBranch();
            }
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Provisional Death Declare";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuProvisionalDD);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (ValidDate() == true)
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidDate()
        {
            Boolean vResult = true;
            if (ddlBranch.SelectedValue == "-1")
            {
                gblFuction.MsgPopup("Please Select the Branch");
                vResult = false;
            }
            return vResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDone_Click(object sender, EventArgs e)
        {
            CProvisionalDeathDeclare oApp = null;
            DataTable dt = new DataTable();
            Int32 vErr = 0;
            string vXmlData = "";
            DateTime vEffDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vTblDtl = Session[gblValue.ACVouDtl].ToString();
            string vFinYear = Session[gblValue.ShortYear].ToString();

            if (this.RoleId != 1)
            {
                if (Session[gblValue.EndDate] != null)
                {
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= vEffDate)
                    {
                        gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                        return;
                    }
                    //else if (this.RoleId != 5 && this.RoleId !=10)
                    //{
                    //    gblFuction.AjxMsgPopup("You are not AM or Operation ..Can not Save..");
                    //    return;
                    //}
                }
            }

            if (ValidDate() == true)
            {
                try
                {
                    //dt = (DataTable)ViewState["PDDMember"];
                    dt = ((DataTable)ViewState["PDDMember"]).Copy();
                    if (dt == null)
                    {
                        gblFuction.MsgPopup("Nothing to Save");
                        return;
                    }
                    foreach (DataRow dr in dt.Select("ProvDeathDec='N'"))
                    {
                        dr.Delete();
                    }
                    dt.AcceptChanges();
                    if (dt.Rows.Count == 0)
                    {
                        gblFuction.MsgPopup("Nothing to Save");
                        return;
                    }
                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    oApp = new CProvisionalDeathDeclare();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    //-----------XML Save----------
                    vErr = oApp.SaveProvisionalDeathDec(vXmlData, this.UserID, ddlBranch.SelectedValue.ToString(), vEffDate);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        LoadGrid(txtSearch.Text, ddlBranch.SelectedValue.ToString());
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                    }
                }
                finally
                {
                    oApp = null;
                    dt = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()//To Check
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAppMode"></param>
        /// <param name="pFromDt"></param>
        /// <param name="pToDt"></param>
        /// <param name="pBranch"></param>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(string pSearch, string pBranch)
        {
            DataTable dt = null;
            CProvisionalDeathDeclare oLS = null;
            try
            {
                string vBrCode = pBranch;
                oLS = new CProvisionalDeathDeclare();
                dt = oLS.GetAllMemberDetailWithOpenLoan(pSearch, vBrCode, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                ViewState["PDDMember"] = dt;
                gvProvDeath.DataSource = dt;
                gvProvDeath.DataBind();
            }
            finally
            {
                dt = null;
                oLS = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProvDeath_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Bind Approval
                    CheckBox chkPDD = (CheckBox)e.Row.FindControl("chkPDD");
                    if (e.Row.Cells[0].Text == "Y")
                    {
                        chkPDD.Checked = true;
                    }
                    else if (e.Row.Cells[0].Text == "N")
                    {
                        chkPDD.Checked = false;
                    }
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkPDD_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            string vLoanId;
            try
            {
                dt = (DataTable)ViewState["PDDMember"];
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkPDD = (CheckBox)row.FindControl("chkPDD");
                vLoanId = row.Cells[8].Text;
                if (chkPDD.Checked == true)
                {
                    CProvisionalDeathDeclare oApp = new CProvisionalDeathDeclare();
                    Int32 vErr = 0;
                    vErr = oApp.chkAccrInt(vLoanId, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                    if (vErr > 0)
                    {
                        dt.Rows[row.RowIndex]["ProvDeathDec"] = "Y";
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Accured Account Already Posted.!");
                        chkPDD.Checked = false;
                    }
                }
                else
                {
                    dt.Rows[row.RowIndex]["ProvDeathDec"] = "N";
                }
                dt.AcceptChanges();
                ViewState["PDDMember"] = dt;
                upSanc.Update();
            }
            finally
            {
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
    }
}
