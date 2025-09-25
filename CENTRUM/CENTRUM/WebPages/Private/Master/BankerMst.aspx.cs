using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class BankerMst : CENTRUMBase
    {
        protected int cPgNo = 1;
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

                //PopLedger();

            }
        }

        private void PopLedger()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "DescID", "[Desc]", "ACGenLed", vBrId, "BranchCode", "AA", vLogDt, "0000");
                ddlBanker.DataSource = dt;
                ddlBanker.DataTextField = "Desc";
                ddlBanker.DataValueField = "DescId";
                ddlBanker.DataBind();

                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBanker.Items.Insert(0, oli);
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Banker Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBankerMst);
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
                    //                    btnCancel.Visible = false;
                    ////                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    //                    btnCancel.Visible = false;
                    //                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Banker Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
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
                    //                    btnSave.Visible = false;
                    //                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtCmpName.Enabled = Status;
            txtBankName.Enabled = Status;
            txtCmpAdd.Enabled = Status;
            txtContPer1.Enabled = Status;
            txtContNo1.Enabled = Status;
            txtContPer2.Enabled = Status;
            txtContNo2.Enabled = Status;
            ddlBanker.Enabled = Status;
            txtDesig.Enabled = Status;
            ddltype.Enabled = Status;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtCmpName.Text = "";
            txtBankName.Text = "";
            txtCmpAdd.Text = "";
            txtContPer1.Text = "";
            txtContNo1.Text = "";
            txtContPer2.Text = "";
            txtContNo2.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
            txtDesig.Text = "";
            ddlBanker.SelectedIndex = -1;
            ddltype.SelectedIndex = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CBankers oIC = null;
            Int32 vRows = 0;
            try
            {
                oIC = new CBankers();
                dt = oIC.GetBankersPG(pPgIndx, ref vRows);
                gvInsComp.DataSource = dt.DefaultView;
                gvInsComp.DataBind();
                if (dt.Rows.Count <= 0)
                {
                    lblTotalPages.Text = "0";
                    lblCurrentPage.Text = "0";
                }
                else
                {
                    lblTotalPages.Text = CalTotPgs(vRows).ToString();
                    lblCurrentPage.Text = cPgNo.ToString();
                }
                if (cPgNo == 1)
                {
                    Btn_Previous.Enabled = false;
                    if (Int32.Parse(lblTotalPages.Text) > 0 && cPgNo != Int32.Parse(lblTotalPages.Text))
                        Btn_Next.Enabled = true;
                    else
                        Btn_Next.Enabled = false;
                }
                else
                {
                    Btn_Previous.Enabled = true;
                    if (cPgNo == Int32.Parse(lblTotalPages.Text))
                        Btn_Next.Enabled = false;
                    else
                        Btn_Next.Enabled = true;
                }
            }
            finally
            {
                dt = null;
                oIC = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRows"></param>
        /// <returns></returns>
        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblTotalPages.Text) - 1; //lblCurrentPage
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1; //lblTotalPages
                    break;
            }
            LoadGrid(cPgNo);
            tabInsComp.ActiveTabIndex = 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvInsComp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vBankersID = 0;
            DataTable dt = null;
            CBankers oIC = null;
            try
            {
                vBankersID = Convert.ToInt32(e.CommandArgument);
                ViewState["BankersID"] = vBankersID;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvInsComp.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oIC = new CBankers();
                    dt = oIC.GetBankersById(vBankersID);
                    if (dt.Rows.Count > 0)
                    {
                        txtCmpName.Text = Convert.ToString(dt.Rows[0]["BankerName"]).Trim();
                        txtBankName.Text = Convert.ToString(dt.Rows[0]["BankName"]).Trim();
                        txtCmpAdd.Text = Convert.ToString(dt.Rows[0]["Address"]).Trim();
                        txtContNo1.Text = Convert.ToString(dt.Rows[0]["ContNo1"]).Trim();
                        txtContNo2.Text = Convert.ToString(dt.Rows[0]["ContNo2"]).Trim();
                        txtContPer1.Text = Convert.ToString(dt.Rows[0]["ContPerson1"]).Trim();
                        txtContPer2.Text = Convert.ToString(dt.Rows[0]["ContPerson2"]).Trim();
                        txtDesig.Text = Convert.ToString(dt.Rows[0]["Desig"]).Trim();
                      //  ddlBanker.SelectedIndex = ddlBanker.Items.IndexOf(ddlBanker.Items.FindByValue(dt.Rows[0]["Descid"].ToString().Trim()));
                        //ddlBanker.SelectedIndex = ddlBanker.Items.IndexOf(ddlBanker.Items.FindByValue(Convert.ToString(dt.Rows[0]["BankerID"]).ToString()));
                        ddltype.SelectedIndex = ddltype.Items.IndexOf(ddltype.Items.FindByValue(Convert.ToString(dt.Rows[0]["Type"]).ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tabInsComp.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vSubId = Convert.ToString(ViewState["BankersID"]);
            Int32 vErr = 0, vRec = 0, vBankersID = 0;
            CBankers oIC = null;
            CGblIdGenerator oGbl = null;
            Int32 vBanker = Convert.ToInt32(ddltype.SelectedValue);
            try
            {
                vBankersID = Convert.ToInt32(ViewState["BankersID"]);


                //if (ddlBanker.SelectedIndex <= 0)
                //{
                //    gblFuction.AjxMsgPopup("Banker's Account not Selected...");
                //    return false;
                //}


                if (Mode == "Save")
                {
                    oIC = new CBankers();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("BankerMst", "BankerName", txtCmpName.Text.Replace("'", "''"), "", "", "BankerID", vSubId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Company Name Can not be Duplicate...");
                        return false;
                    }
                    vErr = oIC.InsertBankers(ref vBankersID, txtCmpName.Text.Replace("'", "''"), txtCmpAdd.Text.Replace("'", "''"),
                        txtContPer1.Text.Replace("'", "''"), txtContNo1.Text.Replace("'", "''"), txtEmailId1.Text.Trim().Replace("'", "''"),
                        txtContPer2.Text.Replace("'", "''"), txtContNo2.Text.Replace("'", "''"), txtEmailId2.Text.Trim().Replace("'", "''"),
                        vBanker,
                        this.UserID, "", "Save", txtBankName.Text.Replace("'", "''"),txtDesig.Text);
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["BankersID"] = vBankersID;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oIC = new CBankers();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("BankerMst", "BankerName", txtCmpName.Text.Replace("'", "''"), "", "", "BankerID", vSubId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Company Name Can not be Duplicate...");
                        return false;
                    }
                    vErr = oIC.InsertBankers(ref vBankersID, txtCmpName.Text.Replace("'", "''"), txtCmpAdd.Text.Replace("'", "''"),
                        txtContPer1.Text.Replace("'", "''"), txtContNo1.Text.Replace("'", "''"), txtEmailId1.Text.Trim().Replace("'", "''"),
                        txtContPer2.Text.Replace("'", "''"), txtEmailId2.Text.Trim().Replace("'", "''"), txtContNo2.Text.Replace("'", "''"),
                         vBanker,
                        this.UserID, "", "Edit", txtBankName.Text.Replace("'", "''"), txtDesig.Text);
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
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDelete(vBankersID, "BankersID", "BankLoanMst");
                    if (vRec <= 0)
                    {
                        oIC = new CBankers();
                        vErr = oIC.InsertBankers(ref vBankersID, txtCmpName.Text.Replace("'", "''"), txtCmpAdd.Text.Replace("'", "''"),
                        txtContPer1.Text.Replace("'", "''"), txtContNo1.Text.Replace("'", "''"), txtEmailId1.Text.Trim().Replace("'", "''"),
                        txtContPer2.Text.Replace("'", "''"), txtContNo2.Text.Replace("'", "''"), txtEmailId2.Text.Trim().Replace("'", "''"),
                        vBanker,
                        this.UserID, "", "Del", txtBankName.Text.Replace("'", "''"), txtDesig.Text);
                        if (vErr > 0)
                            vResult = true;
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.DBError);
                            vResult = false;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.RecordUseMsg);
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
                oIC = null;
                oGbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
            ViewState["BankersID"] = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                tabInsComp.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    LoadGrid(0);
                    ClearControls();
                    tabInsComp.ActiveTabIndex = 0;
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabInsComp.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(0);
                StatusButton("Show");
                ViewState["StateEdit"] = null;
            }
        }
    }
}