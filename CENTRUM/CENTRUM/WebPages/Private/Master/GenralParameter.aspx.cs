using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.Collections;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class GenralParameter : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
                    StatusButton("View");
                txtEffDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                LoadGrid();
                tbGenParameter.ActiveTabIndex = 0;
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "General Parameter";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuJLGGeneralParameter);
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
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "General Parameter", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

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
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
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

        
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanAdd == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                ViewState["StateEdit"] = "Add";
                tbGenParameter.ActiveTabIndex = 1;
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
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadGrid();
                    StatusButton("Delete");
                }
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
            tbGenParameter.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }

        private void LoadGrid()
        {
            DataTable dt = null;
            string vBrCode = "";
            CGenParameter oGenP = null;
            try
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oGenP = new CGenParameter();
                dt = oGenP.GetGeneralParameterList();
                if (dt.Rows.Count > 0)
                {
                    gvGenParameter.DataSource = dt;
                    gvGenParameter.DataBind();
                }
                else
                {
                    gvGenParameter.DataSource = null;
                    gvGenParameter.DataBind();
                }
            }
            finally
            {
                oGenP = null;
                dt = null;
            }
        }

        protected void gvGenParameter_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 pId = 0, vRow = 0;
            string vBrCode = "";
            DataTable dt = null;
            try
            {
                pId = Convert.ToInt32(e.CommandArgument);
                vBrCode = Session[gblValue.BrnchCode].ToString();
                ViewState["Id"] = pId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvGenParameter.Rows)
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

                    CGenParameter oGenP = new CGenParameter();
                    dt = oGenP.GetGeneralParameterById(pId);
                    if (dt.Rows.Count > 0)
                    {
                        txtEffDt.Text = Convert.ToString(dt.Rows[0]["EffectDate"]);
                        txtMinAdvAmt.Text = Convert.ToString(dt.Rows[0]["MinAdvAmt"]);
                        txtMonthlyAdvCountLimit.Text = Convert.ToString(dt.Rows[0]["MonthlyAdvCountLimit"]);
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbGenParameter.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vDisId = Convert.ToString(ViewState["Id"]);
            Int32 vErr = 0, vRec = 0, vStateId = 0, vId = 0, vNewId = 0, vMonthlyAdvCountLimit = 0;
            Decimal vMinAdvAmt = 0;
            CGenParameter oGenp = null;
            try
            {
                if (txtEffDt.Text == "")
                {
                    gblFuction.MsgPopup("Effective Date Can Not Be Empty..");
                    return false;
                }
                DateTime pEffDt = gblFuction.setDate(txtEffDt.Text.ToString());
                if (txtMinAdvAmt.Text != "")
                    vMinAdvAmt = Convert.ToDecimal(txtMinAdvAmt.Text);
                if (txtMonthlyAdvCountLimit.Text != "")
                    vMonthlyAdvCountLimit = Convert.ToInt32(txtMonthlyAdvCountLimit.Text);
                if (Mode == "Save")
                {
                    oGenp = new CGenParameter();
                    vErr = oGenp.SaveGeneralParameter(ref vNewId, vId, pEffDt, vMinAdvAmt, vMonthlyAdvCountLimit, this.UserID, "Save");
                    if (vErr > 0)
                    {
                        ViewState["Id"] = vNewId;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    vId = Convert.ToInt32(ViewState["Id"]);
                    oGenp = new CGenParameter();
                    vErr = oGenp.SaveGeneralParameter(ref vNewId, vId, pEffDt, vMinAdvAmt, vMonthlyAdvCountLimit, this.UserID, "Edit");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oGenp = new CGenParameter();
                    vErr = oGenp.SaveGeneralParameter(ref vNewId, vId, pEffDt, vMinAdvAmt, vMonthlyAdvCountLimit, this.UserID, "Delete");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.DeleteMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oGenp = null;
            }
        }

        private void EnableControl(Boolean Status)
        {
            txtMinAdvAmt.Enabled = Status;
            txtMonthlyAdvCountLimit.Enabled = Status;
            
        }

        private void ClearControls()
        {
            txtMinAdvAmt.Text = "0.00";
            txtMonthlyAdvCountLimit.Text = "0";
            lblDate.Text = "";
            lblUser.Text = "";
            
        }

        
    }
}