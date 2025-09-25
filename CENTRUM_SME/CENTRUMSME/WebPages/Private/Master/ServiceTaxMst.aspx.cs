using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class ServiceTaxMst : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
                    StatusButton("View");
                LoadGrid(0);
            }
        }
        #region METHODS
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    gblFuction.focus("ctl00_cph_Main_tabInsComp_pnlDtl_txtCmpName");
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    gblFuction.focus("ctl00_cph_Main_tabInsComp_pnlDtl_txtCmpName");
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Exit":
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        private void EnableControl(Boolean Status)
        {
            txtServTxrate.Enabled = Status;
            txtEffDt.Enabled = Status;
        }

        private void ClearControls()
        {

            txtEffDt.Text = "";
            txtServTxrate.Text = "";
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);
                
                this.Menu = false;
                this.PageHeading = "Service Tax Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuSecrvTx);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Service Tax Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            int vErr = 0;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            decimal vSerTax = Convert.ToDecimal(txtServTxrate.Text.Trim());
            decimal vKKTax = Convert.ToDecimal(txtKKtax.Text.Trim());
            decimal vSBTax = Convert.ToDecimal(txtSBTax.Text.Trim());
            decimal vGST = 0;
            if(txtGST.Text!="")
                vGST = Convert.ToDecimal(txtGST.Text.Trim());
            string vEffDt = gblFuction.setDate(txtEffDt.Text).ToString();

            CCreditBureau oCB = null;
            try
            {
                if (Mode == "Save")
                {
                    oCB = new CCreditBureau();
                    vErr = oCB.SaveServiceTax(vSerTax, vKKTax, vSBTax, vGST,vEffDt, "Save", 0);
                    if (vErr == 0)
                    {
                        vResult = true;
                    }
                    else if (vErr == 2)
                    {
                        gblFuction.MsgPopup("Effective Date should be Less than last Effective Date!!");
                        vResult = false;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oCB = new CCreditBureau();
                    vErr = oCB.SaveServiceTax(vSerTax, vKKTax, vSBTax, vGST, vEffDt, "Edit", Convert.ToInt16(ViewState["STId"]));
                    if (vErr == 0)
                    {
                        vResult = true;
                    }
                    else if (vErr == 2)
                    {
                        gblFuction.MsgPopup("Effective Date should be greater than last Effective Date!!");
                        vResult = false;
                    }
                    else if (vErr == 3)
                    {
                        gblFuction.MsgPopup("Only Last Effective datet can be Edit!!");
                        vResult = false;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {

                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCB = null;

            }
        }

        private void LoadGrid(int STId)
        {
            DataTable dt = null;
            CCreditBureau oCB = null;
            try
            {
                oCB = new CCreditBureau();
                dt = oCB.GetServiceTax("All", STId);
                if (dt.Rows.Count > 0)
                {
                    gvST.DataSource = dt;
                    gvST.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCB = null;
                dt = null;
            }
        }
        #endregion

        #region buttonClick
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["StateEdit"] = "Add";
                tabInsComp.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
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
                gblFuction.MsgPopup("Delete is not possible!!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                ViewState["StateEdit"] = "Edit";
                StatusButton("Edit");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            EnableControl(false);
            StatusButton("View");
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                if (vStateEdit == "Add" || vStateEdit == null)
                    vStateEdit = "Save";
                if (SaveRecords(vStateEdit) == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    StatusButton("Show");
                    ViewState["StateEdit"] = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        protected void gvST_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dt = null;
            int vSTId;
            CCreditBureau oCB = null;
            try
            {
                vSTId = Convert.ToInt32(e.CommandArgument);
                ViewState["STId"] = vSTId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    
                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvST.Rows)
                    {
                        if ((gr.RowIndex) % 2 == 0)
                        {
                            gr.BackColor = backColor;
                            gr.ForeColor = foreColor;
                        }
                        else
                        {
                            gr.BackColor = System.Drawing.Color.White;
                            gr.ForeColor = foreColor;
                        }
                        gr.Font.Bold = false;
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                        lb.Font.Bold = false;
                    }
                    gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#151B54");
                    gvRow.ForeColor = System.Drawing.Color.White;
                    gvRow.Font.Bold = true;
                    btnShow.ForeColor = System.Drawing.Color.White;
                    btnShow.Font.Bold = true; 

                    oCB = new CCreditBureau();
                    dt = oCB.GetServiceTax("Only", Convert.ToInt16(ViewState["STId"]));
                    if (dt.Rows.Count > 0)
                    {
                        txtEffDt.Text = dt.Rows[0]["EffectiveDate"].ToString();
                        txtServTxrate.Text = dt.Rows[0]["STPer"].ToString();
                        txtKKtax.Text = dt.Rows[0]["KKTax"].ToString();
                        txtSBTax.Text = dt.Rows[0]["SBTax"].ToString();
                        txtGST.Text = dt.Rows[0]["GST"].ToString();
                        tabInsComp.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}