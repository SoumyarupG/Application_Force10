using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;
using System.IO;

namespace CENTRUM_SARALVYAPAR.WebPages.Private.Master
{
    public partial class BCMaster : CENTRUMBAse
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    StatusButton("Exit");
                }
                else
                {
                    StatusButton("View");
                }
                ViewState["StateEdit"] = null;
                LoadGrid(0);
                //PopAssets();
                //PopLib();
                tabLoanAppl.ActiveTabIndex = 0;
                
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "BC Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBCMst);
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
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "BC Master", false);
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
                    //gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
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
                    //  gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
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
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnDelete.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        private void EnableControl(bool Status)
        {
            txtBCName.Enabled = Status;           

            txtRBIRegNo.Enabled = Status;
            txtPAN.Enabled = Status;
            txtBCAddress.Enabled = Status;
            gvBrdDrtrDtls.Enabled = Status;
            ddlBCType.Enabled = Status;

        }

        private void ClearControls()
        {
            txtBCName.Text = "";
            txtRBIRegNo.Text = "";
            txtPAN.Text = "";
            txtBCAddress.Text = "";
            ddlBCType.SelectedIndex = 0;
            txtIntpayableJLG.Text = "0";
            txtPFPayableJLG.Text = "0";
            txtEffDateJLG.Text = "";
            txtIntpayableSARAL.Text = "0";
            txtPFPayableSARAL.Text = "0";
            txtEffDateSARAL.Text = "";

        }

        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }

        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblTotPg.Text) - 1; //lblCrPg
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCrPg.Text) + 1; //lblTotPg
                    break;
            }
            LoadGrid(cPgNo);
            tabLoanAppl.ActiveTabIndex = 0;
        }

        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CLoanScheme oIC = null;
            Int32 vRows = 0;
            try
            {
                oIC = new CLoanScheme();
                dt = oIC.GetBCList(pPgIndx, ref vRows);
                gvBCList.DataSource = dt.DefaultView;
                gvBCList.DataBind();
                if (dt.Rows.Count <= 0)
                {
                    lblTotPg.Text = "0";
                    lblCrPg.Text = "0";
                }
                else
                {
                    lblTotPg.Text = CalTotPgs(vRows).ToString();
                    lblCrPg.Text = cPgNo.ToString();
                }
                if (cPgNo == 1)
                {
                    btnPrev.Enabled = false;
                    if (Int32.Parse(lblTotPg.Text) > 0 && cPgNo != Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                }
                else
                {
                    btnPrev.Enabled = true;
                    if (cPgNo == Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
                }
            }
            finally
            {
                dt = null;
                oIC = null;
            }
        }

        private bool ValidateDirectorsField()
        {
            bool vRst = true;
            DataTable dtDirectorsDtl = null;
            CMember oMem = null;
            DataRow dF;
            try
            {
                dtDirectorsDtl = new DataTable();
                dtDirectorsDtl = null;

                oMem = new CMember();
                dtDirectorsDtl = oMem.DirectorDtl();

                if (dtDirectorsDtl.Rows.Count == 0)
                {

                    dF = dtDirectorsDtl.NewRow();
                    dtDirectorsDtl.Rows.Add(dF);
                    dtDirectorsDtl.AcceptChanges();
                }

                if (dtDirectorsDtl != null)
                {
                    foreach (GridViewRow gR in gvBrdDrtrDtls.Rows)
                    {
                        TextBox txtDrtrName = (TextBox)gR.FindControl("txtDrtrName");
                        TextBox txtDrtrPhnNo = (TextBox)gR.FindControl("txtDrtrPhnNo");
                        TextBox txtDrtrAddress = (TextBox)gR.FindControl("txtDrtrAddress");
                        HiddenField hdnDrtrId = (HiddenField)gR.FindControl("hdnDrtrId");
                        
                        if (txtDrtrName.Text.Trim() != "" || txtDrtrPhnNo.Text.Trim() != "" || txtDrtrAddress.Text.Trim() != "")
                        {
                            if (txtDrtrName.Text.Trim() == "")
                            {
                                gblFuction.MsgPopup("Director Name should not be blank");
                                txtDrtrName.Focus();
                                vRst = false;
                                return false;
                            }

                            if (txtDrtrPhnNo.Text.Trim() == "")
                            {
                                gblFuction.MsgPopup("Director Phone No. should not be blank");
                                txtDrtrPhnNo.Focus();
                                vRst = false;
                                return false;
                            }

                            if (txtDrtrAddress.Text.Trim() == "")
                            {
                                gblFuction.MsgPopup("Director Address should not be blank");
                                txtDrtrAddress.Focus();
                                vRst = false;
                                return false;
                            }
                        }

                        if (txtDrtrName.Text.Trim() != "" && txtDrtrPhnNo.Text.Trim() != "" && txtDrtrAddress.Text.Trim() != "")
                        {
                           

                            dtDirectorsDtl.Rows[gR.RowIndex]["DirectorName"] = txtDrtrName.Text.Trim();
                            dtDirectorsDtl.Rows[gR.RowIndex]["DirectorPhnNo"] = txtDrtrPhnNo.Text.Trim();
                            dtDirectorsDtl.Rows[gR.RowIndex]["DirectorAddress"] = txtDrtrAddress.Text.Trim();
                            dtDirectorsDtl.Rows[gR.RowIndex]["DrtrId"] = hdnDrtrId.Value == "" || hdnDrtrId.Value == null ? "0" : hdnDrtrId.Value.ToString();

                            dtDirectorsDtl.AcceptChanges();
                            dF = dtDirectorsDtl.NewRow();
                            dtDirectorsDtl.Rows.Add(dF);
                        }
                        
                    }
                    for (int i = dtDirectorsDtl.Rows.Count - 1; i >= 0; i--)
                    {
                        if (dtDirectorsDtl.Rows[i]["DirectorName"] == DBNull.Value)
                        {
                            dtDirectorsDtl.Rows[i].Delete();
                        }
                    }
                    dtDirectorsDtl.AcceptChanges();

                        if (dtDirectorsDtl.Rows.Count >= 2)
                        {
                            ViewState["DirectorsDtls"] = dtDirectorsDtl;
                        }
                        else
                        {
                            gblFuction.MsgPopup("At least two Directors needs to be added");
                            vRst = false;
                            return false;
                        }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            return vRst;
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
                StatusButton("Add");
                ClearControls();
                //tbSchm.ActiveTabIndex = 1;
                PopulateDirectorGrid();
                tabLoanAppl.ActiveTabIndex = 1;
               

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
            LoadGrid(0);
            tabLoanAppl.ActiveTabIndex = 0;
            ClearControls();
            StatusButton("View");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                StatusButton("View");
                LoadGrid(0);
                ViewState["StateEdit"] = null;
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
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    ClearControls();
                    StatusButton("Delete");
                    //LoadGrid(0);
                    //tbSchm.ActiveTabIndex = 0;
                    tabLoanAppl.ActiveTabIndex = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
            ViewState["BCId"] = null;
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBCName = "", vRBIRegNo = "", vPAN = "",vBCAddress="";
            DataTable dtDirectorsDtl = null;
            Int32 vErr = 0, vRec = 0,vBCId=0; 
            string vMsg = "";
            CLoanScheme oLS = null;
            CGblIdGenerator oGbl = null;
            DataTable dt = null;

            try
            {
                vBCId = Convert.ToInt32(ViewState["BCId"]);
                vBCName = txtBCName.Text;
                vRBIRegNo = txtRBIRegNo.Text;
                vPAN = txtPAN.Text;
                vBCAddress = txtBCAddress.Text;

                #region "Directors Data"

                    if (ValidateDirectorsField() == false) return false;
                    string vXmlDirectors = "";
                    dtDirectorsDtl = new DataTable();

                    dtDirectorsDtl = (DataTable)ViewState["DirectorsDtls"];
                    dtDirectorsDtl.TableName = "tblDirectorsData";

                    using (StringWriter oSW = new StringWriter())
                    {
                        dtDirectorsDtl.WriteXml(oSW);
                        vXmlDirectors = oSW.ToString().Replace("T00:00:00+05:30", "");
                        //return false;
                    }
                 #endregion
                    if (ddlBCType.SelectedValue == "-1")
                    {
                        gblFuction.AjxMsgPopup("Please Select Type Of BC");
                        return false;
                    }

                    if (gblFuction.setDate(Session[gblValue.LoginDate].ToString()).AddDays(60) < gblFuction.setDate(txtEffDateJLG.Text))
                    {
                        gblFuction.AjxMsgPopup("JLG Effective date should not be more than 60 days from login in date..!!");
                        return false;
                    }

                    if (gblFuction.setDate(Session[gblValue.LoginDate].ToString()).AddDays(60) < gblFuction.setDate(txtEffDateSARAL.Text))
                    {
                        gblFuction.AjxMsgPopup("SARAL Effective date should not be more than 60 days from login in date..!!");
                        return false;
                    }

                if (Mode == "Save")
                {
                    
                    if (vPAN.Trim().Length < 10)
                    {
                        gblFuction.AjxMsgPopup("Please update Proper PAN Card Number of Member..!!");
                        return false;
                    }
                    oGbl = new CGblIdGenerator();
                    dt = new DataTable();
                    dt = oGbl.ChkDuplicateBCName(vBCName, "Save");

                    if (dt.Rows.Count > 0)
                    {
                        gblFuction.MsgPopup("BC Name can not be Duplicate...");
                        return false;
                    }

                    oLS = new CLoanScheme();
                    vErr = oLS.InsertBC(ref vBCId, vBCName, vRBIRegNo, vPAN, vBCAddress, vXmlDirectors, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save", ddlBCType.SelectedValue,
                        Convert.ToDouble(txtIntpayableJLG.Text.Trim()), Convert.ToDouble(txtPFPayableJLG.Text.Trim()), gblFuction.setDate(txtEffDateJLG.Text),
                        Convert.ToDouble(txtIntpayableSARAL.Text.Trim()), Convert.ToDouble(txtPFPayableSARAL.Text.Trim()), gblFuction.setDate(txtEffDateSARAL.Text));

                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["BCId"] = vBCId;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
               else if(Mode == "Edit")
                {
                    if (vPAN.Trim().Length < 10)
                    {
                        gblFuction.AjxMsgPopup("Please update Proper PAN Card Number of Member..!!");
                        return false;
                    }

                    oLS = new CLoanScheme();
                    vErr = oLS.InsertBC(ref vBCId, vBCName, vRBIRegNo, vPAN, vBCAddress, vXmlDirectors, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Edit",ddlBCType.SelectedValue,
                        Convert.ToDouble(txtIntpayableJLG.Text.Trim()), Convert.ToDouble(txtPFPayableJLG.Text.Trim()), gblFuction.setDate(txtEffDateJLG.Text),
                        Convert.ToDouble(txtIntpayableSARAL.Text.Trim()), Convert.ToDouble(txtPFPayableSARAL.Text.Trim()), gblFuction.setDate(txtEffDateSARAL.Text));
                    
                   if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oLS = new CLoanScheme();
                    vErr = oLS.InsertBC(ref vBCId, vBCName, vRBIRegNo, vPAN, vBCAddress, vXmlDirectors, Convert.ToInt32(Session[gblValue.UserId].ToString()), "Delete",ddlBCType.SelectedValue,
                        Convert.ToDouble(txtIntpayableJLG.Text.Trim()), Convert.ToDouble(txtPFPayableJLG.Text.Trim()), gblFuction.setDate(txtEffDateJLG.Text),
                        Convert.ToDouble(txtIntpayableSARAL.Text.Trim()), Convert.ToDouble(txtPFPayableSARAL.Text.Trim()), gblFuction.setDate(txtEffDateSARAL.Text));
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                return vResult;
            }

            finally
            {
                oLS = null;
                oGbl = null;
                dt = null;
            }
        }

        protected void gvBCList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vBCId = 0;
            DataSet ds = null;
            DataTable dt=null, dt1=null, dtJLG = null, dtSARAL = null;
            CLoanScheme oLS = null;
            try
            {
                vBCId = Convert.ToInt32(e.CommandArgument);
                ViewState["BCId"] = vBCId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvBCList.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oLS = new CLoanScheme();
                    ds = oLS.GetBCById(vBCId);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    dtJLG = ds.Tables[2];
                    dtSARAL = ds.Tables[3];
                    if (dt.Rows.Count > 0)
                    {

                        txtBCName.Text = Convert.ToString(dt.Rows[0]["BCName"]).Trim();
                        txtBCCode.Text = Convert.ToString(dt.Rows[0]["BCCode"]).ToString();
                        txtRBIRegNo.Text = Convert.ToString(dt.Rows[0]["RBIRegNo"]).ToString();
                        
                        txtPAN.Text=Convert.ToString(dt.Rows[0]["PAN"]).ToString();
                        txtBCAddress.Text = Convert.ToString(dt.Rows[0]["BCAddress"]).ToString();
                        ddlBCType.SelectedIndex = ddlBCType.Items.IndexOf(ddlBCType.Items.FindByValue(Convert.ToString(dt.Rows[0]["BCType"])));

                        if (dtJLG.Rows.Count > 0)
                        {
                            txtIntpayableJLG.Text = Convert.ToString(dtJLG.Rows[0]["Intpayable"]).Trim();
                            txtPFPayableJLG.Text = Convert.ToString(dtJLG.Rows[0]["PFPayable"]).Trim();
                            txtEffDateJLG.Text = Convert.ToString(dtJLG.Rows[0]["EffectiveDate"]).Trim();
                        }
                        else
                        {
                            txtIntpayableJLG.Text = "0";
                            txtPFPayableJLG.Text = "0";
                            txtEffDateJLG.Text = "";
                        }

                        if (dtSARAL.Rows.Count > 0)
                        {
                            txtIntpayableSARAL.Text = Convert.ToString(dtSARAL.Rows[0]["Intpayable"]).Trim();
                            txtPFPayableSARAL.Text = Convert.ToString(dtSARAL.Rows[0]["PFPayable"]).Trim();
                            txtEffDateSARAL.Text = Convert.ToString(dtSARAL.Rows[0]["EffectiveDate"]).Trim();
                        }
                        else
                        {
                            txtIntpayableSARAL.Text = "0";
                            txtPFPayableSARAL.Text = "0";
                            txtEffDateSARAL.Text = "";
                        }
                        //LoadRegion("Edit", vICId);
                        tabLoanAppl.ActiveTabIndex = 1;
                        if (Session[gblValue.BrnchCode].ToString() != "0000")
                            StatusButton("Exit");
                        else
                            StatusButton("Show");

                        if (dt1.Rows.Count > 0)
                        {
                            gvBrdDrtrDtls.DataSource = dt1;
                            gvBrdDrtrDtls.DataBind();
                            ViewState["DirectorsDtls"] = dt1;
                        }
                        else
                        {
                            PopulateDirectorGrid();
                        }
                    }
                }
            }
            finally
            {
                dt = null;
                dt1 = null;
                dtJLG = null;
                dtSARAL = null;
                oLS = null;
            }

        }

        protected void gvBrdDrtrDtls_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDel")
            {
                DataTable dt = null;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                dt = (DataTable)ViewState["DirectorsDtls"];
                if (dt.Rows.Count >= 1)
                {
                    dt.Rows[index].Delete();
                    dt.AcceptChanges();
                    ViewState["DirectorsDtls"] = dt;
                    gvBrdDrtrDtls.DataSource = dt;
                    gvBrdDrtrDtls.DataBind();
                }
                else
                {
                    gblFuction.MsgPopup("First Row can not be deleted.");
                    return;
                }
            }
            if (e.CommandName == "cmdAdd")
            {
                DataTable dt = null;
                Int32 vR = 0;
                DataRow dr;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                dt = (DataTable)ViewState["DirectorsDtls"];
                if (dt.Rows.Count >= 1)
                {
                    vR = dt.Rows.Count - 1;
                    TextBox txtDrtrName = (TextBox)gvBrdDrtrDtls.Rows[vR].FindControl("txtDrtrName");
                    TextBox txtDrtrPhnNo = (TextBox)gvBrdDrtrDtls.Rows[vR].FindControl("txtDrtrPhnNo");
                    TextBox txtDrtrAddress = (TextBox)gvBrdDrtrDtls.Rows[vR].FindControl("txtDrtrAddress");

                    if (txtDrtrName.Text != "" && txtDrtrPhnNo.Text != "" && txtDrtrAddress.Text != "")
                    {
                        dt.Rows[vR]["DirectorName"] = txtDrtrName.Text;
                        dt.Rows[vR]["DirectorPhnNo"] = txtDrtrPhnNo.Text;
                        dt.Rows[vR]["DirectorAddress"] = txtDrtrAddress.Text;
                        dt.AcceptChanges();
                        if (vR == dt.Rows.Count - 1)
                        {
                            dr = dt.NewRow();
                            dt.Rows.Add(dr);
                        }
                    }
                    ViewState["DirectorsDtls"] = dt;
                    gvBrdDrtrDtls.DataSource = dt;
                    gvBrdDrtrDtls.DataBind();
                    UpdatePanel1.Update();
                }

            }



        }

        private void PopulateDirectorGrid()
        {
            DataTable dt = new DataTable();
            DataRow dF;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                dt = oMem.DirectorDtl();
                if (dt.Rows.Count == 0)
                {

                    dF = dt.NewRow();
                    dt.Rows.Add(dF);
                    dt.AcceptChanges();
                }
                gvBrdDrtrDtls.DataSource = dt;
                gvBrdDrtrDtls.DataBind();
                ViewState["DirectorsDtls"] = dt;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        }
    }
