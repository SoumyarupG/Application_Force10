using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FORCEBA;
using FORCECA;
using System.IO;
using System.Web.Security;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class MonitoringOther : CENTRUMBase
    {
        private int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                StatusButton("View");
                popBranch();
                LoadGrid(1);
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                tbEmp.ActiveTabIndex = 0;
                txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtCrStrDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtCrEndDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Other Monitoring Module";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuMonitoringOth);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Monitoring Module", false);
                }
                
                //else
                //{
                //    btnAdd.Visible = false;
                //    btnEdit.Visible = false;
                //    btnDelete.Visible = false;
                //    btnSave.Visible = false;
                //    gblFuction.MsgPopup("Permission Not Set Properly.");
                //}
            }
            catch
            {
                Response.Redirect("~/Login.aspx?e=random", true);
            }
        }

        protected void ddlEo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CIntIspPM oAd = null;
            try
            {
                DateTime vSDt = gblFuction.setDate(txtSDt.Text);
                ddlCenter.Items.Clear();

                oAd = new CIntIspPM();
                dt = oAd.Insp_GetCenterList(ddlVisitType.SelectedValue, Session[gblValue.BrnchCode].ToString(), ddlEo.SelectedValue, vSDt);
                ddlCenter.DataSource = dt;
                ddlCenter.DataTextField = "Market";
                ddlCenter.DataValueField = "Marketid";
                ddlCenter.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCenter.Items.Insert(0, oli);
            }
            finally
            {
                oAd = null;
                dt = null;
            }
        }

        protected void ddlVisitType_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
            try
            {
                gvPM.DataSource = null;
                gvPM.DataBind();
                LoadQuestions();
                if(ddlVisitType.SelectedValue == "HV")
                    ddlMember.Enabled = true;
                else
                    ddlMember.Enabled = false;
                
            }
            finally
            {
                
            }
        }

        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {

            popMember();
        }

        private void popMember()
        {
            DataTable dt = null;
            CIntIspPM oAd = null;

            try
            {
                DateTime vSDt = gblFuction.setDate(txtSDt.Text);
                ddlMember.Items.Clear();
                if (ddlVisitType.SelectedValue == "HV")
                {
                    oAd = new CIntIspPM();
                    dt = oAd.Insp_GetHVMemberList(ddlCenter.SelectedValue, Session[gblValue.BrnchCode].ToString(), vSDt);
                    ddlMember.DataSource = dt;
                    ddlMember.DataTextField = "Member";
                    ddlMember.DataValueField = "Memberid";
                    ddlMember.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlMember.Items.Insert(0, oli);
                }
            }
            finally
            {
                oAd = null;
                dt = null;
            }
        }

        private void popCenter()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(txtSDt.Text);
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oGb = new CGblIdGenerator();
                dt = oGb.PopTransferMIS("Y", "MarketMst", "", vLogDt, vBrCode, "T");
                ddlCenter.DataSource = dt;
                ddlCenter.DataTextField = "Market";
                ddlCenter.DataValueField = "MarketID";
                ddlCenter.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCenter.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
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
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnDelete.Enabled = true;
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
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtSDt.Enabled = Status;
            txtCrStrDt.Enabled = false;
            txtCrEndDt.Enabled = false;
            ddlBranch.Enabled = Status;
            gvPM.Enabled = Status;
            ddlVisitType.Enabled = Status;
            ddlEo.Enabled = Status;
            ddlCenter.Enabled = Status;
            ddlMember.Enabled = false;
        }


        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtSDt.Text = Session[gblValue.LoginDate].ToString();
            txtCrStrDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            txtCrEndDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            //ddlBranch.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
            ddlVisitType.SelectedIndex = -1;
            ddlEo.SelectedIndex = -1;
            ddlCenter.SelectedIndex = -1;
            ddlMember.Items.Clear();
            ddlMember.SelectedIndex = -1;
        }


        /// <summary>
        /// 
        /// </summary>
        private void popBranch()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");

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
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBranch.Items.Insert(0, oli);
                if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Convert.ToString(Session[gblValue.BrnchCode])));
                popRO(Convert.ToString(Session[gblValue.LoginDate]));
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popRO(string pdate)
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(pdate);
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oGb = new CGblIdGenerator();
                dt = oGb.PopTransferMIS("Y", "EoMst", "JRO,SRO,RO,BM,ABM,SBM,OPE,CO", vLogDt, vBrCode);
                ddlEo.DataSource = dt;
                ddlEo.DataTextField = "EoName";
                ddlEo.DataValueField = "EoId";
                ddlEo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlEo.Items.Insert(0, oli);

                
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (this.CanAdd == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Add);
                return;
            }
            ViewState["StateEdit"] = "Add";
            tbEmp.ActiveTabIndex = 1;
            StatusButton("Add");
            ClearControls();
            
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
                    //tabPurps.ActiveTabIndex = 0;
                    tbEmp.ActiveTabIndex = 0;
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
            tbEmp.ActiveTabIndex = 1;
            ViewState["StateEdit"] = "Edit";
            StatusButton("Edit");
            if (ddlVisitType.SelectedValue == "HV")
                ddlMember.Enabled = true;
            else
                ddlMember.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbEmp.ActiveTabIndex = 0;
            ClearControls();
            LoadGrid(0);
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
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
                EnableControl(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(0);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0;
            CIntIspPM oAd = null;
            try
            {
                DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);

                oAd = new CIntIspPM();
                dt = oAd.Insp_GetMonitoringPG(Session[gblValue.BrnchCode].ToString(),pPgIndx, vFrmDt, vToDt, ref vTotRows,"FL");
                gvEmp.DataSource = dt;
                gvEmp.DataBind();
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
            finally
            {
                oAd = null;
                dt = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRows"></param>
        /// <returns></returns>
        private int CalTotPages(double pRows)
        {
            int vPgs = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vPgs;
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
                case "Prev":
                    vPgNo = Int32.Parse(lblCrPg.Text) - 1;
                    break;
                case "Next":
                    vPgNo = Int32.Parse(lblCrPg.Text) + 1;
                    break;
            }
            LoadGrid(vPgNo);
            tbEmp.ActiveTabIndex = 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx", false);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEmp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int pInspID = 0;
            CIntIspPM oAu = null;
            DataTable dt = null, dt1 = null;
            DataSet ds = null;
            try
            {
                pInspID = Convert.ToInt32(e.CommandArgument);
                ViewState["InsPecId"] = pInspID;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvEmp.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oAu = new CIntIspPM();
                    ds = oAu.Insp_GetMonitorById(pInspID);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];

                    if (dt.Rows.Count > 0)
                    {
                        txtSDt.Text = Convert.ToString(dt.Rows[0]["SubDt"]);
                        txtCrStrDt.Text = Convert.ToString(dt.Rows[0]["CrdFrmDt"]);
                        txtCrEndDt.Text = Convert.ToString(dt.Rows[0]["CrdToDt"]);
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["Branch"].ToString()));
                        ddlVisitType.SelectedIndex = ddlVisitType.Items.IndexOf(ddlVisitType.Items.FindByValue(dt.Rows[0]["VisitType"].ToString()));
                        popRO(txtSDt.Text);
                        ddlEo.SelectedIndex = ddlEo.Items.IndexOf(ddlEo.Items.FindByValue(dt.Rows[0]["EOid"].ToString()));
                        popCenter();
                        ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(dt.Rows[0]["MarketId"].ToString()));
                        if (dt.Rows[0]["VisitType"].ToString() == "HV")
                        {
                            popMember();
                        }
                        ddlMember.SelectedIndex = ddlMember.Items.IndexOf(ddlMember.Items.FindByValue(dt.Rows[0]["MemberId"].ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();

                        gvPM.DataSource = dt1;
                        gvPM.DataBind();
                        ViewState["MN"] = dt1;


                        tbEmp.ActiveTabIndex = 1;
                        StatusButton("Show");
                        EnableControl(false);
                    }
                }
            }
            finally
            {
                dt = null;
                oAu = null;
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
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vXmlData = "";
            Int32 vInsPecId = 0, vErr = 0, vUserID = 0;
            double vTotalScore = 0;
            DateTime vSubDt = gblFuction.setDate(txtSDt.Text);
            DateTime vCrStrDt = gblFuction.setDate(txtCrStrDt.Text);
            DateTime vCrEndDt = gblFuction.setDate(txtCrEndDt.Text);
            CIntIspPM oAu = null;
            DataTable dt = new DataTable();
            DataTable dtDtl = new DataTable();

            try
            {
                //vUserID = Int32.Parse(Session[gblValue.UserId].ToString());
                vUserID = Convert.ToInt32(Session[gblValue.UserId]);
                //if (vCrStrDt >= vSubDt)
                //{
                //    gblFuction.MsgPopup("Carried out Start Date should be earlier date of Submission Date.");
                //    return false;
                //}
                //if (vCrEndDt > vSubDt)
                //{
                //    gblFuction.MsgPopup("Carried out End Date should be earlier date of Submission Date.");
                //    return false;
                //}

                if (ddlVisitType.SelectedValue == "HV" && ddlMember.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Please select Member.");
                    return false;
                }
                //if (vTsp.Days < 30)
                //{
                //    gblFuction.MsgPopup("Days difference of Submission Date and Period Start Date should be atlest 30 days.");
                //    return false;
                //}

                //dt = (DataTable)ViewState["MN"];
                //if (dt == null) return false;

                if (ValidateEntry() == false)
                {
                    gblFuction.MsgPopup("At least one Question need to answer");
                    return false;
                }

                dtDtl = DtDtlTran();
                if (dtDtl == null) return false;
                using (StringWriter oSW3 = new StringWriter())
                {
                    dtDtl.WriteXml(oSW3);
                    vXmlData = oSW3.ToString();
                }

                if (ViewState["InsPecId"] != null) vInsPecId = Convert.ToInt32(Convert.ToString(ViewState["InsPecId"]));
                if (Mode == "Save")
                {
                    oAu = new CIntIspPM();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("EOMst", "EmpCode", txtEmpCode.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Save");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("EMP Code can not be Duplicate...");
                    //    return false;
                    //}

                    vErr = oAu.Insp_SaveMonitor(ref vInsPecId, "MN", vSubDt, vCrStrDt, vCrEndDt, ddlBranch.SelectedValue, vXmlData, vUserID, "Save",ddlVisitType.SelectedValue,ddlEo.SelectedValue, ddlCenter.SelectedValue, ddlMember.SelectedValue);
                    if (vErr > 0)
                    {
                        ViewState["InsPecId"] = vInsPecId;
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
                    oAu = new CIntIspPM();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("EOMst", "EMPCode", txtEmpCode.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Edit");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("EMP Code Can not be Duplicate...");
                    //    return false;
                    //}
                    vErr = oAu.Insp_SaveMonitor(ref vInsPecId, "MN", vSubDt, vCrStrDt, vCrEndDt, ddlBranch.SelectedValue, vXmlData, vUserID, "Edit", ddlVisitType.SelectedValue, ddlEo.SelectedValue, ddlCenter.SelectedValue, ddlMember.SelectedValue);
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
                    oAu = new CIntIspPM();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDeleteString(vEoId, "EOID", "MarketMSt");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("The RO has group, you can not delete the RO.");
                    //    return false;
                    //}
                    vErr = oAu.Insp_SaveMonitor(ref vInsPecId, "MN", vSubDt, vCrStrDt, vCrEndDt, ddlBranch.SelectedValue, vXmlData, vUserID, "Del", ddlVisitType.SelectedValue, ddlEo.SelectedValue, ddlCenter.SelectedValue, ddlMember.SelectedValue);
                    if (vErr > 0)
                    {
                        gvPM.DataSource = null;
                        gvPM.DataBind();
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
            finally
            {
                oAu = null;
            }
        }

        private bool ValidateEntry()
        {
            bool vRst = false;
            Int32 i=0;
            foreach (GridViewRow gr in gvPM.Rows)
            {
                DropDownList ddlQuestionYN = (DropDownList)gr.FindControl("ddlQuestionYN");
                //TextBox txtRemarks = (TextBox)gr.FindControl("txtRemarks");

                if (ddlQuestionYN.SelectedValue.ToString().Trim() != "-1")
                {
                    i = i + 1;
                }
            }

            if (i > 0)
                vRst = true;
            else
                vRst = false;
            return vRst;
        }

        private DataTable DtDtlTran()
        {
            DataTable dt = new DataTable("dtDtl");
            DataRow dr;
            dt.Columns.Add(new DataColumn("ItemId"));
            dt.Columns.Add(new DataColumn("ItemTypeId"));
            dt.Columns.Add(new DataColumn("QID"));
            dt.Columns.Add(new DataColumn("QuestionYN"));
            dt.Columns.Add(new DataColumn("Remarks"));

            foreach (GridViewRow gr in gvPM.Rows)
            {
                DropDownList ddlQuestionYN = (DropDownList)gr.FindControl("ddlQuestionYN");
                TextBox txtRemarks = (TextBox)gr.FindControl("txtRemarks");
                    dr = dt.NewRow();
                    dr["ItemId"] = gr.Cells[6].Text.Trim();
                    dr["ItemTypeId"] = gr.Cells[7].Text.Trim();
                    dr["QID"] = gr.Cells[9].Text.Trim();
                    dr["QuestionYN"] = ddlQuestionYN.SelectedValue.ToString().Trim();
                    dr["Remarks"] = txtRemarks.Text.ToString().Trim();
                    dt.Rows.Add(dr);
                
            }
            return dt;
        }

        protected void gvPM_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = null;
            CIntIspPM oAD = null;

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlQuestionYN = (DropDownList)e.Row.FindControl("ddlQuestionYN");
                    TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");

                    if (e.Row.Cells[10].Text == "-1")
                    {
                        ddlQuestionYN.SelectedIndex = ddlQuestionYN.Items.IndexOf(ddlQuestionYN.Items.FindByValue("-1"));
                    }
                    else
                    {
                        ddlQuestionYN.SelectedIndex = ddlQuestionYN.Items.IndexOf(ddlQuestionYN.Items.FindByValue(e.Row.Cells[10].Text));
                    }
                }
            }
            finally
            {
                dt = null;
                oAD = null;
            }
        }

        private void LoadQuestions()
        {
            DataTable dt = null;
            CIntIspPM oAD = null;

            try
            {
                oAD = new CIntIspPM();
                dt = oAD.Insp_GetInspectionList("MN",ddlVisitType.SelectedValue);
                ViewState["MN"] = dt;
                gvPM.DataSource = dt;
                gvPM.DataBind();
            }
            finally
            {
                dt = null;
                oAD = null;
            }
        }
    }
}