using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class CollectionReverseDelete : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["InstNo"] = null;
                ViewState["StateEdit"] = null;
                txtTransDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                btnReverse.Enabled = false;
                btnDelete.Enabled = false;
                popCustomer();

            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Collection Delete Reverse";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCollDelRev);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnReverse.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnReverse.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnReverse.Visible = true;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnExit.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Collection Delete Reverse", false);
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
            try
            {
                oCD = new CDisburse();
                dt = oCD.GetCustNameForReSch(vBrCode);
                if (dt.Rows.Count > 0)
                {
                    ddlMem.Items.Clear();
                    ddlMem.DataSource = dt;
                    ddlMem.DataTextField = "CompanyName";
                    ddlMem.DataValueField = "CustId";
                    ddlMem.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlMem.Items.Insert(0, oli);
                }
            }
            finally
            {
                oCD = null;
                dt = null;
            }
        }

        protected void ddlMem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["InstNo"] = null;
            gvDtl.DataSource = null;
            gvDtl.DataBind();
            ddlLoanNo.Items.Clear();
            DataTable dt = null;
            CDisburse oMem = null;
            string vBrCode = "", vMemberId = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vMemberId = ddlMem.SelectedValue;
                oMem = new CDisburse();
                dt = oMem.GetLnNoByCustId(vMemberId);
                ddlLoanNo.DataSource = dt;
                ddlLoanNo.DataTextField = "LoanNo";
                ddlLoanNo.DataValueField = "LoanId";
                ddlLoanNo.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlLoanNo.Items.Insert(0, oLi);
            }
            finally
            {
                oMem = null;
                dt = null;
            }
        }

        protected void ddlLoanNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["InstNo"] = null;
            string vLoanID = ddlLoanNo.SelectedValue;
            GetCollectionDtl(vLoanID);
        }

        private void GetCollectionDtl(string pLoanID)
        {

            DataTable dt = null;
            CLoanRecovery oLnRec = new CLoanRecovery();
            dt = oLnRec.GetLoanCollection(Session[gblValue.BrnchCode].ToString(), pLoanID);

            gvDtl.DataSource = dt;
            gvDtl.DataBind();

        }

        protected void gvDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vInstNo = 0;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            vInstNo = Convert.ToInt32(e.CommandArgument);
            ViewState["InstNo"] = vInstNo;
            if (e.CommandName == "cmdShow")
            {

                DateTime vLoginDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                foreach (GridViewRow gr in gvDtl.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;

                btnReverse.Enabled = true;
                btnDelete.Enabled = true;
                UpdatePanel1.Update();
            }
        }

        protected void btnReverse_Click(object sender, EventArgs e)
        {
            try
            {
                string vStateEdit = "Rev";
                if (SaveRecords(vStateEdit) == true)
                {
                    gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                    gvDtl.DataSource = null;
                    gvDtl.DataBind();
                    ddlMem.Items.Clear();
                    ddlLoanNo.Items.Clear();
                    txtRemark.Text = "";
                    btnReverse.Enabled = false;
                    btnDelete.Enabled = false;
                    UpdatePanel1.Update();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string vStateEdit = "Del";
                if (SaveRecords(vStateEdit) == true)
                {
                    gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                    gvDtl.DataSource = null;
                    gvDtl.DataBind();
                    ddlMem.Items.Clear();
                    ddlLoanNo.Items.Clear();
                    txtRemark.Text = "";
                    btnDelete.Enabled = false;
                    btnReverse.Enabled = false;
                    UpdatePanel1.Update();
                    UpdatePanel2.Update();
                    upLnNo.Update();
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                gvDtl.DataSource = null;
                gvDtl.DataBind();
                ddlMem.Items.Clear();
                ddlLoanNo.Items.Clear();
                txtRemark.Text = "";
                ViewState["InstNo"] = null;
                btnDelete.Enabled = false;
                btnReverse.Enabled = false;
                UpdatePanel1.Update();
                UpdatePanel2.Update();
                upLnNo.Update();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Boolean ValidateFields(string pMode)
        {
            Boolean vResult = true;
            Int32 vInstNo = 0;
            CLoanRecovery oLR = null;
            DateTime vTransDate = gblFuction.setDate(txtTransDt.Text);
            string vRetMsg = "";
            if (ddlLoanNo.SelectedIndex == 0)
            {
                gblFuction.AjxMsgPopup("Loan No.Cannot be left blank...");
                gblFuction.focus("ct100_cph_Main_tabReSchdl_pnlDtls_ddlLoanNo");
                vResult = false;
            }

            
            if (ddlMem.SelectedValue == "-1")
            {
                gblFuction.AjxMsgPopup("Please select the Member");
                gblFuction.focus("ct100_cph_Main_tabReSchdl_pnlDtls_ddlMem");
                vResult = false;
            }

            if (txtRemark.Text == "")
            {
                gblFuction.AjxMsgPopup("Remarks Cannot be left blank...");
                gblFuction.focus("ct100_cph_Main_tabReSchdl_pnlDtls_txtRemark");
                vResult = false;
            }
            vInstNo = (Int32)ViewState["InstNo"];
            if (vInstNo == 0 || vInstNo == null)
            {
                gblFuction.AjxMsgPopup("Please select the installment no...");
                vResult = false;
            }


            oLR = new CLoanRecovery();
            vRetMsg = oLR.ChkLoanForRevDel(ddlLoanNo.SelectedValue, vInstNo, pMode, vTransDate);
            if (vRetMsg != "")
            {
                gblFuction.AjxMsgPopup(vRetMsg);
                return false;
            }

            return vResult;
        }

        private Boolean SaveRecords(string Mode)
        {

            Boolean vResult = false;
            Int32 vInstNo = (Int32)ViewState["InstNo"];
            DateTime vTransDate = gblFuction.setDate(txtTransDt.Text);
            Int32 vErr = 0;
            string vAcMst = Session[gblValue.ACVouMst].ToString();
            string vAcDtl = Session[gblValue.ACVouDtl].ToString();


            if (ValidateFields(Mode) == false)
                return false;
            string vLoanID = ddlLoanNo.SelectedValue;
            string vBrCode = Session[gblValue.BrnchCode].ToString();


            CLoanRecovery oLR = null;

            oLR = new CLoanRecovery();
            try
            {
                if (Mode == "Rev")
                {


                    vErr = oLR.ReverseCollection(vLoanID, vInstNo, "P", vLoanID + "-" + vInstNo.ToString(), vAcMst, vAcDtl, vBrCode, Convert.ToInt32(Session[gblValue.UserId]), vTransDate, Session[gblValue.ShortYear].ToString(), txtRemark.Text);

                    if (vErr == 0)
                    {
                        gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Del")
                {


                    vErr = oLR.DeleteCollection(vLoanID, vInstNo, "P", vLoanID + "-" + vInstNo.ToString(), vAcMst, vAcDtl, vBrCode, Convert.ToInt32(Session[gblValue.UserId]));
                    if (vErr == 0)
                    {
                        gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
    }
}