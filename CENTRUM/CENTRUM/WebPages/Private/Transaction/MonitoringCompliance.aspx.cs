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
    public partial class MonitoringCompliance : CENTRUMBase
    {
        private int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtCmpDt.Text = Session[gblValue.LoginDate].ToString();
                StatusButton("View");
                //popBranch();
                LoadGrid(1);
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                tbEmp.ActiveTabIndex = 0;
                txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Branch Visit Compliance";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuMonitoringCompl);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Monitoring Compliance", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx?e=random", true);
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
                    //btnAdd.Enabled = true;
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
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = true;
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
            gvPM.Enabled = Status;
            txtCrStrDt.Enabled = false;
            txtCrEndDt.Enabled = false;
        }


        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtCmpDt.Text = Session[gblValue.LoginDate].ToString();
            txtSDt.Text = "";
            txtCrStrDt.Text = "";
            txtCrEndDt.Text = "";
            ddlBranch.SelectedIndex = -1;
            gvEmp.DataSource = null;
            gvEmp.DataBind();
            lblDate.Text = "";
            lblUser.Text = "";
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
            //LoadQuestions();
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
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oAd = new CIntIspPM();
                dt = oAd.Insp_GetMonitorCmpliancePG(pPgIndx, vFrmDt, vToDt, ref vTotRows, vBrCode,"BR");
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

                    ClearControls();
                    oAu = new CIntIspPM();
                    ds = oAu.Insp_GetMonitorCmplById(pInspID);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];

                    if (dt.Rows.Count > 0)
                    {
                        //txtCmpDt.Text = Convert.ToString(dt1.Rows[0]["CmplDt"]);
                        txtSDt.Text = Convert.ToString(dt.Rows[0]["SubDt"]);
                        txtCrStrDt.Text = Convert.ToString(dt.Rows[0]["CrdFrmDt"]);
                        txtCrEndDt.Text = Convert.ToString(dt.Rows[0]["CrdToDt"]);
                        popBranch();
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["Branch"].ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();

                        if (dt1.Rows.Count > 0)
                        {
                            lblUser.Text = "Last Modified By : " + dt1.Rows[0]["UserName"].ToString();
                            lblDate.Text = "Last Modified Date : " + dt1.Rows[0]["CreationDateTime"].ToString();
                            gvPM.DataSource = dt1;
                            gvPM.DataBind();
                            ViewState["BC"] = dt1;
                        }

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
            DateTime vCmplDt = gblFuction.setDate(txtCmpDt.Text);
            DateTime vSubDt = gblFuction.setDate(txtSDt.Text);
            DateTime vCrStrDt = gblFuction.setDate(txtCrStrDt.Text);
            DateTime vCrEndDt = gblFuction.setDate(txtCrEndDt.Text);
            CIntIspPM oAu = null;
            DataTable dt = new DataTable();
            DataTable dtDtl = new DataTable();

            if (vSubDt > vCmplDt)
            {
                gblFuction.MsgPopup("Compliance Date should be > Submission Date.");
                vResult = false;
            }

            if (Session[gblValue.EndDate] != null)
            {
                if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtCmpDt.Text))
                {
                    gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                    return false;
                }
            }

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


                vXmlData = XmlFromGrid();

                if (ViewState["InsPecId"] != null) vInsPecId = Convert.ToInt32(Convert.ToString(ViewState["InsPecId"]));
                //if (Mode == "Save")
                //{
                //    oAu = new CIntIspPM();
                //    //oGbl = new CGblIdGenerator();
                //    //vRec = oGbl.ChkDuplicate("EOMst", "EmpCode", txtEmpCode.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Save");
                //    //if (vRec > 0)
                //    //{
                //    //    gblFuction.MsgPopup("EMP Code can not be Duplicate...");
                //    //    return false;
                //    //}

                //    vErr = oAu.Insp_SavePM(ref vInsPecId, "PM", vSubDt, vCrStrDt, vCrEndDt, vPrStrDt, vPrEndDt, ddlBranch.SelectedValue, vTotalScore, vXmlData, vUserID, "Save");
                //    if (vErr > 0)
                //    {
                //        ViewState["InsPecId"] = vInsPecId;
                //        vResult = true;
                //    }
                //    else
                //    {
                //        gblFuction.MsgPopup(gblMarg.DBError);
                //        vResult = false;
                //    }
                //}
                if (Mode == "Edit")
                {
                    oAu = new CIntIspPM();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("EOMst", "EMPCode", txtEmpCode.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Edit");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("EMP Code Can not be Duplicate...");
                    //    return false;
                    //}
                    vErr = oAu.Insp_SaveMonitorCompliance(ref vInsPecId, vCmplDt, vXmlData, vUserID, "Edit");
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
                    vErr = oAu.Insp_SaveMonitorCompliance(ref vInsPecId, vCmplDt, vXmlData, vUserID, "Del");
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

        private String XmlFromGrid()
        {
            Int32 i = 0;
            String vXML = "";
            DataTable dt = new DataTable("dtDtl");
            dt.Columns.Add(new DataColumn("ItemId"));
            dt.Columns.Add(new DataColumn("ItemTypeId"));
            dt.Columns.Add(new DataColumn("SlNo"));
            dt.Columns.Add(new DataColumn("ComplianceYN"));
            dt.Columns.Add(new DataColumn("Remarks"));
            foreach (GridViewRow gr in gvPM.Rows)
            {
                TextBox txtCmplRemarks = (TextBox)gr.FindControl("txtCmplRemarks");
                DropDownList ddlMonitComplYN = (DropDownList)gr.FindControl("ddlMonitComplYN");
                DataRow dr = dt.NewRow();
                dr["ItemId"] = gr.Cells[8].Text.Trim();
                dr["ItemTypeId"] = gr.Cells[9].Text.Trim();
                dr["SlNo"] = gr.Cells[10].Text.Trim();
                dr["ComplianceYN"] = ddlMonitComplYN.SelectedValue.ToString().Trim();
                dr["Remarks"] = txtCmplRemarks.Text.ToString().Trim();

                dt.Rows.Add(dr);
                dt.AcceptChanges();
                i++;
            }
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vXML = oSW.ToString();
            }
            return vXML;

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
                    DropDownList ddlMonitComplYN = (DropDownList)e.Row.FindControl("ddlMonitComplYN");
                    TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
                    TextBox txtCmplRemarks = (TextBox)e.Row.FindControl("txtCmplRemarks");

                    if (e.Row.Cells[11].Text == "-1")
                    {
                        ddlQuestionYN.SelectedIndex = ddlQuestionYN.Items.IndexOf(ddlQuestionYN.Items.FindByValue("-1"));
                    }
                    else
                    {
                        ddlQuestionYN.SelectedIndex = ddlQuestionYN.Items.IndexOf(ddlQuestionYN.Items.FindByValue(e.Row.Cells[11].Text));
                    }

                    if (e.Row.Cells[12].Text == "-1")
                    {
                        ddlMonitComplYN.SelectedIndex = ddlMonitComplYN.Items.IndexOf(ddlMonitComplYN.Items.FindByValue("-1"));
                    }
                    else
                    {
                        ddlMonitComplYN.SelectedIndex = ddlMonitComplYN.Items.IndexOf(ddlMonitComplYN.Items.FindByValue(e.Row.Cells[12].Text));
                    }
                }
            }
            finally
            {
                dt = null;
                oAD = null;
            }
        }
    }
}