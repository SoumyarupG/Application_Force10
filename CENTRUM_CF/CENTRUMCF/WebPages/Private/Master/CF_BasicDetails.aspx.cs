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
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Reporting.WebForms;
using CENTRUMCA;
using CENTRUMBA;
using System.Collections.Generic;
using System.IO;

namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class CF_BasicDetails : CENTRUMBAse
    {
        protected int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            ceAppDt.EndDate = DateTime.Now;
            if (!IsPostBack)
            {
                txtPSLClass.Text = "Renewable Energy";
                txtPSLClass.Enabled = false;
                txtNatProp.Text = "Solar Finance";
                txtNatProp.Enabled = false;
                txtApplicant.Enabled = false;
                PopBranch();
                PopSolarSystemType();

                PopSegType();
                PopAssesMethod(0);
                PopAppliEntType();
                PopEPCMst();
                PopRO();
                txtAppDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                popState();
                popPartner();
                tbBasicDet.ActiveTabIndex = 0;
                StatusButton("View");

            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Basic Details";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFBasicDet);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnDelete.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanProcess == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Basic Details", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
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
                tbBasicDet.ActiveTabIndex = 1;
                StatusButton("Add");
                PopBCPropNo();
                ClearControls();
                txtPSLClass.Text = "Renewable Energy";
                txtPSLClass.Enabled = false;
                txtNatProp.Text = "Solar Finance";
                txtNatProp.Enabled = false;
                txtApplicant.Enabled = false;


            }
            catch (Exception ex)
            {
                throw ex;
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
                    txtPSLClass.Enabled = false;
                    txtNatProp.Enabled = false;
                    txtApplicant.Enabled = false;

                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    txtPSLClass.Enabled = false;
                    txtNatProp.Enabled = false;
                    txtApplicant.Enabled = false;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    txtPSLClass.Enabled = false;
                    txtNatProp.Enabled = false;
                    txtApplicant.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    txtPSLClass.Enabled = false;
                    txtNatProp.Enabled = false;
                    txtApplicant.Enabled = false;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    txtPSLClass.Enabled = false;
                    txtNatProp.Enabled = false;
                    txtApplicant.Enabled = false;
                    EnableControl(false);
                    break;
                case "Exit":
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    txtPSLClass.Enabled = false;
                    txtNatProp.Enabled = false;
                    txtApplicant.Enabled = false;
                    break;
            }
        }
        private void EnableControl(Boolean Status)
        {

            txtNatProp.Enabled = Status;
            ddlBCStste.Enabled = Status;
            ddlBranch.Enabled = Status;
            ddlBcRM.Enabled = Status;
            ddlEPC.Enabled = Status;
            ddlSolPwrSys.Enabled = Status;
            txtDistnce.Enabled = Status;
            ddlBCPropNo.Enabled = Status;
            txtAppNo.Enabled = false;
            txtAppDt.Enabled = Status;
            ddlEntityType.Enabled = Status;
            ddlSegType.Enabled = Status;
            ddlAssMethod.Enabled = Status;
            ddlPurFacilty.Enabled = Status;
            ddlPrioSec.Enabled = Status;
            txtPSLClass.Enabled = Status;
            ddlBCPartName.Enabled = Status;
            ddlEmpProfile.Enabled = Status;

        }
        private void ClearControls()
        {
            lblApplNm.Text = "";
            lblBCPNum.Text = "";
            txtNatProp.Text = "";
            ddlBCPartName.SelectedIndex = -1;
            ddlBCStste.SelectedIndex = -1;
            ddlBranch.SelectedIndex = -1;
            ddlBcRM.SelectedIndex = -1;
            ddlEPC.SelectedIndex = -1;
            ddlSolPwrSys.SelectedIndex = -1;
            txtDistnce.Text = "";
            txtApplicant.Text = "";
            txtAppNo.Text = "";
            txtAppDt.Text = "";
            ddlEntityType.SelectedIndex = -1;
            ddlSegType.SelectedIndex = -1;
            ddlAssMethod.SelectedIndex = -1;
            ddlPurFacilty.SelectedIndex = -1;
            ddlPrioSec.SelectedIndex = -1;
            txtPSLClass.Text = "";
            ddlBCPropNo.SelectedIndex = -1;
            ddlEmpProfile.SelectedIndex = -1;

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
                    LoadBasicDetailsList(1);
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
                ddlBCPropNo.Enabled = false;
                ddlBranch.Enabled = false;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbBasicDet.ActiveTabIndex = 0;
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
                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                LoadBasicDetailsList(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
                GetBasicDetailsById(Convert.ToInt32(hdBasicId.Value));
                StatusButton("View");
                btnEdit.Enabled = true;
            }
        }     
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 vErr = 0, vNewId = 0;
            string vBCPropNo = "", vNatProp = "", vBCPartName = "", vBCStste = "", vBrCode = "", vApplicant = "", vAppNo = "", vPurFacilty = "", vPrioSec = "", vPSLClass = "", vBasicId = "", vErrMsg = "";
            Int32 vBcRM = 0, vEPC = 0, vSolPwrSys = 0, vDistnce = 0, vEntityType = 0, vSegType = 0, vAssMethod = 0, vLeadId = 0;
            DateTime vAppDt = gblFuction.setDate(txtAppDt.Text.ToString()); string vEmpProfile = "";
            string vdate = Convert.ToString(Session[gblValue.LoginDate]);
            DateTime vLogDt = gblFuction.setDate(vdate);
            Int32 vBasicIDedit = 0, vBCPartnrID = 0;
            CMember oMem = null;
            try
            {
                if (hdBasicId.Value != "")
                {
                    vBasicId = hdBasicId.Value;
                    vBasicIDedit = Convert.ToInt32(vBasicId);
                }
                vBCPropNo = ddlBCPropNo.SelectedItem.Text.ToString().Trim();
                vNatProp = txtNatProp.Text.ToString().Trim();
                vBCPartName = ddlBCPartName.SelectedItem.ToString();
                vBCPartnrID = Convert.ToInt32(ddlBCPartName.SelectedValue);
                vBCStste = ddlBCStste.SelectedItem.ToString();
                vBrCode = ddlBranch.SelectedValue.ToString();
                vApplicant = txtApplicant.Text.ToString().Trim();
                vAppNo = txtAppNo.Text.ToString().Trim();
                vPurFacilty = ddlPurFacilty.SelectedValue.ToString().Trim();
                vPrioSec = ddlPrioSec.SelectedValue.ToString();
                vPSLClass = txtPSLClass.Text.ToString().Trim();
                vBcRM = Convert.ToInt32(ddlBcRM.SelectedValue);
                vEPC = Convert.ToInt32(ddlEPC.SelectedValue);
                vSolPwrSys = Convert.ToInt32(ddlSolPwrSys.SelectedValue);
                vDistnce = Convert.ToInt32(txtDistnce.Text.Trim());
                vEntityType = Convert.ToInt32(ddlEntityType.SelectedValue);
                vSegType = Convert.ToInt32(ddlSegType.SelectedValue);
                vAssMethod = Convert.ToInt32(ddlAssMethod.SelectedValue);
                vLeadId = Convert.ToInt32(hdLeadId.Value);
                vEmpProfile = ddlEmpProfile.SelectedValue.ToString().Trim();
                if (vAppDt > vLogDt)
                {
                    gblFuction.MsgPopup("Application Date Should Not Be Greater Than Login Date ...");
                    return false;
                }
                if (Mode == "Save")
                {
                    oMem = new CMember();
                    vErr = oMem.CF_SaveBasicDetails(ref vNewId, vLeadId, vNatProp, vBCPropNo, vBCPartName, vBCStste, vBrCode, vBcRM
                                                    , vEPC, vSolPwrSys, vDistnce, vApplicant, ref vAppNo, vAppDt, vEntityType, vSegType, vAssMethod
                                                    , vPurFacilty, vPrioSec, vPSLClass, this.UserID, "Save", Convert.ToInt32(Session[gblValue.FinYrNo]), 0, ref vErrMsg,
                                                    vEmpProfile, vBCPartnrID);
                    if (vErr == 0)
                    {
                        hdBasicId.Value = Convert.ToString(vNewId);
                        ViewState["BasicId"] = vNewId;
                        gblFuction.MsgPopup("New Application Number Generated" + " : " + vAppNo);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrMsg);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {

                    oMem = new CMember();
                    vErr = oMem.CF_UpdateBasicDetails(vBasicIDedit, vLeadId, vNatProp, vBCPropNo, vBCPartName, vBCStste, vBrCode, vBcRM
                                                    , vEPC, vSolPwrSys, vDistnce, vApplicant, vAppNo, vAppDt, vEntityType, vSegType, vAssMethod
                                                    , vPurFacilty, vPrioSec, vPSLClass, this.UserID, "Edit", 0, ref vErrMsg, vEmpProfile, vBCPartnrID);
                    if (vErr == 0)
                    {
                        hdBasicId.Value = Convert.ToString(vBasicIDedit);
                        ViewState["BasicId"] = vBasicIDedit;
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrMsg);
                        vResult = false;
                    }
                }
                else
                {
                    oMem = new CMember();
                    vErr = oMem.CF_UpdateBasicDetails(vBasicIDedit, vLeadId, vNatProp, vBCPropNo, vBCPartName, vBCStste, vBrCode, vBcRM
                                                    , vEPC, vSolPwrSys, vDistnce, vApplicant, vAppNo, vAppDt, vEntityType, vSegType, vAssMethod
                                                    , vPurFacilty, vPrioSec, vPSLClass, this.UserID, "Delete", 0, ref vErrMsg, vEmpProfile, vBCPartnrID);
                    if (vErr == 0)
                    {
                        gblFuction.AjxMsgPopup(gblPRATAM.DeleteMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrMsg);
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
                oMem = null;
            }

        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadBasicDetailsList(0);
        }
        protected void gvBasicDet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 pBasicId = 0;
            string vBrCode = "";           
            ClearControls();
            try
            {
                pBasicId = Convert.ToInt32(e.CommandArgument);
                vBrCode = Session[gblValue.BrnchCode].ToString();
                ViewState["BasicId"] = pBasicId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    lblApplNm.Text = Convert.ToString(gvRow.Cells[3].Text);
                    if (btnShow != null)
                    {
                        lblBCPNum.Text = Convert.ToString(btnShow.Text);
                    }
                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvBasicDet.Rows)
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
                    GetBasicDetailsById(pBasicId);
                }
            }
            finally
            {
               
            }
        }
        protected void GetBasicDetailsById(Int32 pBasicId)
        {
            DataTable dt = null;
            DataSet ds = new DataSet();

            try
            {
                CDistrict oDist = new CDistrict();
                dt = oDist.CF_GetBasicDetailsById(pBasicId);
                if (dt.Rows.Count > 0)
                {
                    hdBasicId.Value = Convert.ToString(dt.Rows[0]["BasicID"]);
                    hdLeadId.Value = Convert.ToString(dt.Rows[0]["LeadID"]);
                    //---------------Populate BCPropNo-----------------------
                    ddlBCPropNo.Items.Clear();
                    ddlBCPropNo.DataSource = dt;
                    ddlBCPropNo.DataTextField = "BCPropNo";
                    ddlBCPropNo.DataValueField = "LeadID";
                    ddlBCPropNo.DataBind();
                    //-------------------------------------------------------
                    ddlBCPropNo.SelectedIndex = ddlBCPropNo.Items.IndexOf(ddlBCPropNo.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                    txtNatProp.Text = Convert.ToString(dt.Rows[0]["NatOfPrpsl"]);
                    ddlBCPartName.SelectedIndex = ddlBCPartName.Items.IndexOf(ddlBCPartName.Items.FindByValue(dt.Rows[0]["BCPartnrID"].ToString()));
                    ddlBCStste.SelectedIndex = ddlBCStste.Items.IndexOf(ddlBCStste.Items.FindByText(dt.Rows[0]["BCState"].ToString()));
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                    ddlBcRM.SelectedIndex = ddlBcRM.Items.IndexOf(ddlBcRM.Items.FindByValue(dt.Rows[0]["BCRMID"].ToString()));
                    ddlEPC.SelectedIndex = ddlEPC.Items.IndexOf(ddlEPC.Items.FindByValue(dt.Rows[0]["EPCId"].ToString()));
                    ddlSolPwrSys.SelectedIndex = ddlSolPwrSys.Items.IndexOf(ddlSolPwrSys.Items.FindByValue(dt.Rows[0]["SolPwrSysId"].ToString()));
                    txtDistnce.Text = Convert.ToString(dt.Rows[0]["SourceDistnc"]);
                    txtApplicant.Text = Convert.ToString(dt.Rows[0]["ApplName"]);
                    txtApplicant.Enabled = false;
                    txtAppNo.Text = Convert.ToString(dt.Rows[0]["AppNo"]);
                    txtAppDt.Text = Convert.ToString(dt.Rows[0]["AppDate"]);
                    ddlEntityType.SelectedIndex = ddlEntityType.Items.IndexOf(ddlEntityType.Items.FindByValue(dt.Rows[0]["ApplEntTypeId"].ToString()));
                    ddlSegType.SelectedIndex = ddlSegType.Items.IndexOf(ddlSegType.Items.FindByValue(dt.Rows[0]["SegTypeID"].ToString()));
                    ddlAssMethod.SelectedIndex = ddlAssMethod.Items.IndexOf(ddlAssMethod.Items.FindByValue(dt.Rows[0]["AssMtdId"].ToString()));
                    ddlPurFacilty.SelectedIndex = ddlPurFacilty.Items.IndexOf(ddlPurFacilty.Items.FindByValue(dt.Rows[0]["PurposeFclty"].ToString()));
                    ddlPrioSec.SelectedIndex = ddlPrioSec.Items.IndexOf(ddlPrioSec.Items.FindByValue(dt.Rows[0]["PriorSecID"].ToString()));
                    txtPSLClass.Text = Convert.ToString(dt.Rows[0]["PSLClass"]);
                    if (Convert.ToString(dt.Rows[0]["EmpProfile"]) != "")
                    {
                        ddlEmpProfile.SelectedValue = Convert.ToString(dt.Rows[0]["EmpProfile"]);
                    }
                    else
                    {
                        ddlEmpProfile.SelectedIndex = -1;
                    }
                    tbBasicDet.ActiveTabIndex = 1;
                    StatusButton("Show");
                    EnableControl(false);
                }
                else
                {
                    txtApplicant.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        private void PopBranch()
        {
            DataTable dt = null;
            CUser oUsr = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));
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
                if (vBrCode == "0000")
                {
                    ListItem oli = new ListItem("<--All-->", "A");
                    ddlBranch.Items.Insert(0, oli);
                }
            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }
        private void PopSolarSystemType()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetSolarPwrSystem();

                ddlSolPwrSys.DataSource = dt;
                ddlSolPwrSys.DataTextField = "SystemType";
                ddlSolPwrSys.DataValueField = "SysID";
                ddlSolPwrSys.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlSolPwrSys.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }
        private void PopAssesMethod(int pSegType)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetAssesMethod(pSegType);

                ddlAssMethod.DataSource = dt;
                ddlAssMethod.DataTextField = "MethodName";
                ddlAssMethod.DataValueField = "MethodID";
                ddlAssMethod.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlAssMethod.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }
        private void PopSegType()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetSegType();

                ddlSegType.DataSource = dt;
                ddlSegType.DataTextField = "SegType";
                ddlSegType.DataValueField = "SegID";
                ddlSegType.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlSegType.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }
        private void PopAppliEntType()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetAppliEntTypeMst();

                ddlEntityType.DataSource = dt;
                ddlEntityType.DataTextField = "AppEntType";
                ddlEntityType.DataValueField = "AppEntID";
                ddlEntityType.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlEntityType.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }
        private void PopEPCMst()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetEPCMst();

                ddlEPC.DataSource = dt;
                ddlEPC.DataTextField = "EpcName";
                ddlEPC.DataValueField = "EpcID";
                ddlEPC.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlEPC.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }
        private void PopRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlBcRM.DataSource = dt;
                ddlBcRM.DataTextField = "EoName";
                ddlBcRM.DataValueField = "Eoid";
                ddlBcRM.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBcRM.Items.Insert(0, oli);


            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }
        private void PopBCPropNo()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ddlBCPropNo.Items.Clear();
                oRO = new CEO();
                dt = oRO.CF_GetBCPropNoByBranch(vBrCode);
                ddlBCPropNo.DataSource = dt;
                ddlBCPropNo.DataTextField = "BCPropNo";
                ddlBCPropNo.DataValueField = "LeadID";
                ddlBCPropNo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBCPropNo.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }
        protected void ddlSegType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopAssesMethod(Convert.ToInt32(ddlSegType.SelectedValue));
        }
        protected void ddlPrioSec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPrioSec.SelectedValue == "Y")
            {
                txtPSLClass.Text = "Renewable Energy";
            }
            else
            {
                txtPSLClass.Text = "";
            }
        }
        protected void ddlBCPropNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBCPropNo = Convert.ToString(ddlBCPropNo.SelectedItem.Text);
            try
            {
                oRO = new CEO();
                dt = oRO.CF_GetApplicantByBCPropNo(vBCPropNo, vBrCode);
                if (dt.Rows.Count > 0)
                {
                    hdLeadId.Value = Convert.ToString(dt.Rows[0]["LeadId"]);
                    txtApplicant.Text = Convert.ToString(dt.Rows[0]["AppName"]);
                }
                else
                {
                    hdLeadId.Value = "";
                    txtApplicant.Text = "";
                }
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }
        private void LoadBasicDetailsList(Int32 pPgIndx)
        {
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            Int32 vTotRows = 0;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                dt = oMem.CF_GetBasicDetailsList(vBrCode, txtSearch.Text.Trim(), pPgIndx, ref vTotRows);
                if (dt.Rows.Count > 0)
                {
                    gvBasicDet.DataSource = dt;
                    gvBasicDet.DataBind();
                }
                else
                {
                    gvBasicDet.DataSource = null;
                    gvBasicDet.DataBind();
                }
                lblTotPg.Text = CalTotPages(vTotRows).ToString();
                lblCrPg.Text = vPgNo.ToString();
                if (vPgNo == 1)
                {
                    btnPrev.Enabled = false;
                    if (Int32.Parse(lblTotPg.Text) > 1)
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                }
                else
                {
                    btnPrev.Enabled = true;
                    if (vPgNo == Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        private int CalTotPages(double pRows)
        {
            int vPgs = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vPgs;
        }
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Prev":
                    vPgNo = Int32.Parse(lblCrPg.Text) - 1;
                    break;
                case "Next":
                    vPgNo = Int32.Parse(lblCrPg.Text) + 1;
                    break;
            }
            LoadBasicDetailsList(vPgNo);
            tbBasicDet.ActiveTabIndex = 0;
        }
        private void popState()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                dt = oMem.CF_GetState();

                ddlBCStste.DataSource = dt;
                ddlBCStste.DataTextField = "StateName";
                ddlBCStste.DataValueField = "StateId";
                ddlBCStste.DataBind();
                ddlBCStste.Items.Insert(0, oli1);


            }
            finally
            {
            }
        }
        private void popPartner()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                dt = oMem.CF_GetPartner();

                ddlBCPartName.DataSource = dt;
                ddlBCPartName.DataTextField = "BCPartnrName";
                ddlBCPartName.DataValueField = "BCPartnrID";
                ddlBCPartName.DataBind();
                ddlBCPartName.Items.Insert(0, oli1);
            }
            finally
            {
            }
        }

       
    }
}