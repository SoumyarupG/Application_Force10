using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMBA;
using System.Data;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class LevelRangeMst : CENTRUMBAse
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
                LoadGrid();
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
            txtEffDt.Enabled = Status;
            txtOpLevel1.Enabled = Status;
            txtOpLevel2.Enabled = Status;
            txtOpLevel3.Enabled = Status;
            txtOpLevel4.Enabled = Status;
            txtOpLevel5.Enabled = Status;
            txtOpLevel6.Enabled = Status;
            txtlegLevel1.Enabled = Status;
            txtlegLevel2.Enabled = Status;
            txtlegLevel3.Enabled = Status;
            txtlegLevel4.Enabled = Status;
            txtlegLevel5.Enabled = Status;
        }

        private void ClearControls()
        {

            txtEffDt.Text = "";
            txtOpLevel1.Text = "";
            txtOpLevel2.Text = "";
            txtOpLevel3.Text = "";
            txtOpLevel4.Text = "";
            txtOpLevel5.Text = "";
            txtOpLevel6.Text = "";
            txtlegLevel1.Text = "";
            txtlegLevel2.Text = "";
            txtlegLevel3.Text = "";
            txtlegLevel4.Text = "";
            txtlegLevel5.Text = "";
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Level Range Master";
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Level Range Master", false);
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
            int vLRId = 0, vErr = 0;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);

            decimal vOpLevel1 = Convert.ToDecimal(txtOpLevel1.Text.Trim());
            decimal vOpLevel2 = Convert.ToDecimal(txtOpLevel2.Text.Trim());
            decimal vOpLevel3 = Convert.ToDecimal(txtOpLevel3.Text.Trim());
            decimal vOpLevel4 = Convert.ToDecimal(txtOpLevel4.Text.Trim());
            decimal vOpLevel5 = Convert.ToDecimal(txtOpLevel5.Text.Trim());
            decimal vOpLevel6 = Convert.ToDecimal(txtOpLevel6.Text.Trim());

            decimal vlegLevel1 = Convert.ToDecimal(txtlegLevel1.Text.Trim());
            decimal vlegLevel2 = Convert.ToDecimal(txtlegLevel2.Text.Trim());
            decimal vlegLevel3 = Convert.ToDecimal(txtlegLevel3.Text.Trim());
            decimal vlegLevel4 = Convert.ToDecimal(txtlegLevel4.Text.Trim());
            decimal vlegLevel5 = Convert.ToDecimal(txtlegLevel5.Text.Trim());


            DateTime vEffDt = gblFuction.setDate(txtEffDt.Text);

            CCreditBureau oCB = null;
            try
            {
                if (Mode == "Save")
                {
                    oCB = new CCreditBureau();

                    vErr = oCB.ChkSaveLevelRange(vLRId, vEffDt, "Save");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Effective Date should be Greater than last Effective Date!!");
                        return false;
                    }


                    vErr = oCB.SaveLevelRange(vLRId, vOpLevel1, vOpLevel2, vOpLevel3, vOpLevel4, vOpLevel5, vOpLevel6,
                                              vlegLevel1, vlegLevel2, vlegLevel3, vlegLevel4, vlegLevel5, vEffDt,
                                              Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save"
                                              );
                    if (vErr > 0)
                    {
                        return true;
                    }

                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        return false;
                    }
                }
                else if (Mode == "Edit")
                {
                    vLRId = Convert.ToInt32(ViewState["LRId"]);
                    oCB = new CCreditBureau();
                    vErr = oCB.ChkSaveLevelRange(vLRId, vEffDt, "Edit");
                    if (vErr == 1)
                    {
                        gblFuction.MsgPopup("Effective Date should be Greater than last Effective Date!!");
                        return false;
                    }
                    if (vErr == 2)
                    {
                        gblFuction.MsgPopup("Only Last Effective date  can be Edit!!");
                        return false;
                    }
                    vErr = oCB.SaveLevelRange(vLRId, vOpLevel1, vOpLevel2, vOpLevel3, vOpLevel4, vOpLevel5, vOpLevel6,
                                               vlegLevel1, vlegLevel2, vlegLevel3, vlegLevel4, vlegLevel5, vEffDt,
                                               Convert.ToInt32(Session[gblValue.UserId].ToString()), "Edit"
                                               );
                    if (vErr > 0)
                    {
                        return true;
                    }

                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        return false;
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

        private void LoadGrid()
        {
            DataTable dt = null;
            CCreditBureau oCB = null;
            try
            {
                oCB = new CCreditBureau();
                dt = oCB.GetLevelRange();
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
                    LoadGrid();
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
            int vLRId;
            CCreditBureau oCB = null;
            try
            {
                vLRId = Convert.ToInt32(e.CommandArgument);
                ViewState["LRId"] = vLRId;
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
                    dt = oCB.GetLevelRangeByID(Convert.ToInt32(ViewState["LRId"]));
                    if (dt.Rows.Count > 0)
                    {
                        txtEffDt.Text = dt.Rows[0]["EffectiveDate"].ToString();
                        txtOpLevel1.Text = dt.Rows[0]["OpLevel1"].ToString();
                        txtOpLevel2.Text = dt.Rows[0]["OpLevel2"].ToString();
                        txtOpLevel3.Text = dt.Rows[0]["OpLevel3"].ToString();
                        txtOpLevel4.Text = dt.Rows[0]["OpLevel4"].ToString();
                        txtOpLevel5.Text = dt.Rows[0]["OpLevel5"].ToString();
                        txtOpLevel6.Text = dt.Rows[0]["OpLevel6"].ToString();
                        txtlegLevel1.Text = dt.Rows[0]["legLevel1"].ToString();
                        txtlegLevel2.Text = dt.Rows[0]["legLevel2"].ToString();
                        txtlegLevel3.Text = dt.Rows[0]["legLevel3"].ToString();
                        txtlegLevel4.Text = dt.Rows[0]["legLevel4"].ToString();
                        txtlegLevel5.Text = dt.Rows[0]["legLevel5"].ToString();
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