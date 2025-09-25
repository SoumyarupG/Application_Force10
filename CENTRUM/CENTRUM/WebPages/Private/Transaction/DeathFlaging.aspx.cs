using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class DeathFlaging : CENTRUMBase
    {
        protected int cPgNo = 0;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                ViewState["DeathFlag"] = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                PopBranch();
                GetDeathFlagAccess();
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
                this.PageHeading = "Death Flaging";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDeathFlag);
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

        protected void GetDeathFlagAccess()
        {
            string vIsAccess = "";
             CDeathRelated oDr = null;
            try{
                 oDr = new CDeathRelated();
                vIsAccess = oDr.GetDeathFlagAccessRight(Convert.ToInt32(Session[gblValue.RoleId]));
                hdDAccess.Value = vIsAccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
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

        
        protected void btnDone_Click(object sender, EventArgs e)
        {
            CDeathRelated oDr = null;
            DataTable dt = new DataTable();
            Int32 vErr = 0;
            string vXmlData = "";
            DateTime vFlagDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vTblDtl = Session[gblValue.ACVouDtl].ToString();
            string vFinYear = Session[gblValue.ShortYear].ToString();

            if (this.RoleId != 1)
            {
                if (Session[gblValue.EndDate] != null)
                {
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= vFlagDt)
                    {
                        gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                        return;
                    }
                   
                }
            }

            if (ValidDate() == true)
            {
                try
                {
                    dt = (DataTable)ViewState["DeathFlag"];
                    if (dt == null)
                    {
                        gblFuction.MsgPopup("Nothing to Save");
                        return;
                    }
                    if (Convert.ToString(dt.Rows[0]["DeathFlag"]).Trim() == hdDFlag.Value.Trim())
                    {
                        gblFuction.MsgPopup("Please Check/Uncheck The Data To save");
                        return;
                    }
                   
                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    oDr = new CDeathRelated();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    //-----------XML Save----------
                    vErr = oDr.SaveDeathFlag(vXmlData, this.UserID, ddlBranch.SelectedValue.ToString(), vFlagDt);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
                        LoadGrid(txtSearch.Text, ddlBranch.SelectedValue.ToString());

                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                    }
                }
                finally
                {
                    oDr = null;
                    dt = null;
                }
            }
        }

        
        private Boolean ValidateFields()//To Check
        {
            return true;
        }
        
        private void LoadGrid(string pSearch, string pBranch)
        {
            DataTable dt = null;
            CDeathRelated oDr = null;
            try
            {
                string vBrCode = pBranch;
                oDr = new CDeathRelated();
                dt = oDr.GetAllMemForDFlag(pSearch, vBrCode, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                ViewState["DeathFlag"] = dt;
                if (dt.Rows.Count > 0)
                {
                    hdDFlag.Value = Convert.ToString(dt.Rows[0]["DeathFlag"]);
                }
                else
                {
                    hdDFlag.Value = "";
                }
                gvSanc.DataSource = dt;
                gvSanc.DataBind();
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
        
        protected void gvSanc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Bind Approval
                    CheckBox chkDFlag = (CheckBox)e.Row.FindControl("chkDFlag");


                    if (e.Row.Cells[0].Text == "I")
                    {
                        chkDFlag.Checked = true;
                        if (hdDAccess.Value == "Y")
                        {
                            chkDFlag.Enabled = true;
                        }
                        else
                        {
                            chkDFlag.Enabled = false;
                        }
                    }
                    

                }
            }
            finally
            {

            }
        }

        protected void chkDFlag_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            string vLoanId;
            try
            {
                dt = (DataTable)ViewState["DeathFlag"];
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkDFlag = (CheckBox)row.FindControl("chkDFlag");
                vLoanId = row.Cells[11].Text;
                if (chkDFlag.Checked == true)
                {
                    dt.Rows[row.RowIndex]["DeathFlag"] = "I";
                }
                else
                {
                    dt.Rows[row.RowIndex]["DeathFlag"] = "D";
                }
               
                dt.AcceptChanges();
                ViewState["DeathFlag"] = dt;
                upSanc.Update();
            }
            finally
            {
                dt = null;
            }
        }

        
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
    }
}