using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class PAI : CENTRUMBase
    {
        protected int vPgNo = 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                //GetInsurancePolicyName();
                popPolicy();
                LoadGrid(1);
                StatusButton("View");
                tbPol.ActiveTabIndex = 0;
                txtCollDt.Text = Convert.ToString(Session[gblValue.LoginDate]);

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
                this.PageHeading = "PAI Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuPAIMaster);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    btnPrint.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "PAI", false);
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
                    btnPrint.Enabled = false;
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
                    btnPrint.Enabled = true;
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnPrint.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnPrint.Enabled = false;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnPrint.Enabled = false;
                    EnableControl(false);
                    break;
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
                tbPol.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
                txtCollDt.Text = Convert.ToString(Session[gblValue.LoginDate]);

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
                    LoadGrid(1);
                    ClearControls();
                    tbPol.ActiveTabIndex = 0;
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
                txtLoanNO.Enabled = false;

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
            tbPol.ActiveTabIndex = 0;
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
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0; ;
            CPAI oPol = null;
            try
            {
                oPol = new CPAI();
                dt = oPol.GetPAIPG(pPgIndx, ref vTotRows, txtSearch.Text.Trim());
                gvPol.DataSource = dt;
                gvPol.DataBind();

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
                oPol = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRows"></param>
        /// <returns></returns>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }
        protected void btnShowLoan_Click(object sender, EventArgs e)
        {
            GetMemListByLoan(txtLoanNO.Text.Trim());
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
            tbPol.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPol_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vRow = 0;
            Int32 vPAI_Id = 0;
            DataTable dt = null;
            CPAI oPol = null;
            try
            {
                vPAI_Id= Convert.ToInt32(e.CommandArgument);
                ViewState["PAI_Id"] = vPAI_Id;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvPol.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;

                    oPol = new CPAI();
                    dt = oPol.GetPAIDetails(vPAI_Id);

                    if (dt.Rows.Count > 0)
                    {
                        txtLoanNO.Text = Convert.ToString(dt.Rows[vRow]["LoanNo"]);
                        hdLoanId.Value = Convert.ToString(dt.Rows[vRow]["LoanId"]);
                        lblLoanNo.Text = Convert.ToString(dt.Rows[vRow]["LoanNo"]);
                        lblMemNo.Text = Convert.ToString(dt.Rows[vRow]["MemberNo"]);
                        lblBwNm.Text = Convert.ToString(dt.Rows[vRow]["BWName"]);
                        lblBwDOB.Text = Convert.ToString(dt.Rows[vRow]["B_DOB"]);
                        lblCwNm.Text = Convert.ToString(dt.Rows[vRow]["CWName"]);
                        lblCwDOB.Text = Convert.ToString(dt.Rows[vRow]["C_DOB"]);
                        lblLnDisb.Text = Convert.ToString(dt.Rows[vRow]["LoanDt"]);
                        lblRel.Text = Convert.ToString(dt.Rows[vRow]["HumanRelationName"]);
                        lblOccup.Text = Convert.ToString(dt.Rows[vRow]["OccupationName"]);
                        lblAddr.Text = Convert.ToString(dt.Rows[vRow]["MemAddr"]);

                        ddlPolNm.SelectedIndex = ddlPolNm.Items.IndexOf(ddlPolNm.Items.FindByValue(dt.Rows[vRow]["PolicyId"].ToString()));
                        GetInsurancePolicyName(Convert.ToInt32(dt.Rows[vRow]["PolicyId"]));
                        txtPreAmt.Text = Convert.ToString(dt.Rows[vRow]["PremiumAmount"]);//For overwriting Purpose
                        txtsumAm.Text = Convert.ToString(dt.Rows[vRow]["Sum_Assur_Amt"]); //For overwriting Purpose
                        ddltype.SelectedIndex = ddltype.Items.IndexOf(ddltype.Items.FindByValue(dt.Rows[vRow]["Type_Of_Bw"].ToString()));
                        txtCollDt.Text = Convert.ToString(dt.Rows[vRow]["PAI_CollDate"]);
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tbPol.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
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
            Int32 vErr = 0, vRec = 0;
            
            string vType_Of_Bw = Convert.ToString(ddltype.SelectedValue);
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vPAI_Id = Convert.ToInt32(ViewState["PAI_Id"]);
            string vLoanId =Convert.ToString(hdLoanId.Value);
            DateTime vCollDt = gblFuction.setDate(txtCollDt.Text);
            Int32 vPolicyId =Convert.ToInt32( ddlPolNm.SelectedValue);
            CPAI oPol = null;
            CGblIdGenerator oGbl = null;
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vTblDtl = Session[gblValue.ACVouDtl].ToString();
            string vFinYear = Session[gblValue.ShortYear].ToString();
            try
            {
                if (gblFuction.setDate(txtCollDt.Text) < vFinFromDt || gblFuction.setDate(txtCollDt.Text) > vFinToDt)
                {
                    gblFuction.AjxMsgPopup("Collection Date should login financial year.");
                    return false;
                }
                if (this.RoleId != 1)
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtCollDt.Text))
                        {
                            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                            return false;
                        }
                    }
                }
                if (Mode == "Save")
                {
                    oPol = new CPAI();
                    
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("PAIMst", "LoanId", vLoanId, "Type_Of_Bw", vType_Of_Bw, "PAI_Id", vPAI_Id.ToString(), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup(" Can not be Duplicate...");
                        return false;
                    }

                    vErr = oPol.SavePAI(ref vPAI_Id, vLoanId, vCollDt, vPolicyId, vType_Of_Bw, vBrCode, this.UserID, "Save", vTblMst, vTblDtl, vFinYear,Convert.ToDouble(txtPreAmt.Text),Convert.ToDouble(txtsumAm.Text));
                           
                    if (vErr > 0)
                    {
                        ViewState["PAI_Id"] = vPAI_Id;
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
                    oPol = new CPAI();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("PAIMst", "LoanId", vLoanId, "Type_Of_Bw", vType_Of_Bw, "PAI_Id", vPAI_Id.ToString(), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup(" Can not be Duplicate...");
                        return false;
                    }
                    vErr = oPol.SavePAI(ref vPAI_Id, vLoanId, vCollDt, vPolicyId, vType_Of_Bw, vBrCode, this.UserID, "Edit", vTblMst, vTblDtl, vFinYear, Convert.ToDouble(txtPreAmt.Text), Convert.ToDouble(txtsumAm.Text));
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



                    oPol = new CPAI();

                    vErr = oPol.SavePAI(ref vPAI_Id, vLoanId, vCollDt, vPolicyId, vType_Of_Bw, vBrCode, this.UserID, "Delet", vTblMst, vTblDtl, vFinYear, Convert.ToDouble(txtPreAmt.Text), Convert.ToDouble(txtsumAm.Text));

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
            finally
            {
                oPol = null;
                oGbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            
            ddltype.Enabled = Status;
            txtCollDt.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
                txtLoanNO.Text="";
                hdLoanId.Value="0";
                lblLoanNo.Text="";
                lblMemNo.Text="";
                lblBwNm.Text="";
                lblBwDOB.Text="";
                lblCwNm.Text="";
                lblCwDOB.Text="";
                lblLnDisb.Text="";
                lblRel.Text="";
                lblOccup.Text="";
                lblAddr.Text="";
                txtPreAmt.Text="";
                txtsumAm.Text="";
                txtTypPol.Text="";
                ddlPolNm.SelectedIndex=-1;
                txtCollDt.Text = "";

        }
        private void GetInsurancePolicyName(Int32 vPolId)
        {
            DataSet ds = null;
            DataTable dt1 = null, dt2 = null;
            CPAI oPol = new CPAI();
            //CPolicy oPol = null;
            Int32 vRow = 0;
            DateTime vCollDt = gblFuction.setDate(txtCollDt.Text);

            try
            {
                ds = oPol.GetInsurancePolicyName(vPolId, vCollDt);
                dt1 = ds.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    txtPreAmt.Text = Convert.ToString(dt1.Rows[vRow]["PremiumAmount"]);
                    txtsumAm.Text = Convert.ToString(dt1.Rows[vRow]["Sum_Assur_Amt"]);
                    txtTypPol.Text = Convert.ToString(dt1.Rows[vRow]["Type_Policy"]);

                }
                dt2 = ds.Tables[1];
                if (dt2.Rows.Count > 0)
                {
                    ddltype.DataSource = dt2;
                    ddltype.DataTextField = "BType";
                    ddltype.DataValueField = "Type_Of_Bw";
                    ddltype.DataBind();
                }
               
            }
            finally
            {
                oPol = null;
                ds = null;
            }
        }
        private void GetMemListByLoan(string pLoanNo)
        {

            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vBranch = Session[gblValue.BrnchCode].ToString();
            CPAI oMem = null;
            Int32 vRow = 0;
            try
            {
                oMem = new CPAI();
                dt = oMem.GetMemberbyLoanID(pLoanNo, vBranch);
                if (dt.Rows.Count > 0)
                {

                    hdLoanId.Value = Convert.ToString(dt.Rows[vRow]["LoanId"]);
                    lblLoanNo.Text = Convert.ToString(dt.Rows[vRow]["LoanNo"]);
                    lblMemNo.Text = Convert.ToString(dt.Rows[vRow]["MemberNo"]);
                    lblBwNm.Text = Convert.ToString(dt.Rows[vRow]["BWName"]);
                    lblBwDOB.Text = Convert.ToString(dt.Rows[vRow]["B_DOB"]);
                    lblCwNm.Text = Convert.ToString(dt.Rows[vRow]["CWName"]);
                    lblCwDOB.Text = Convert.ToString(dt.Rows[vRow]["C_DOB"]);
                    lblLnDisb.Text = Convert.ToString(dt.Rows[vRow]["LoanDt"]);
                    lblRel.Text = Convert.ToString(dt.Rows[vRow]["HumanRelationName"]);
                    lblOccup.Text = Convert.ToString(dt.Rows[vRow]["OccupationName"]);
                    lblAddr.Text = Convert.ToString(dt.Rows[vRow]["MemAddr"]);
                }
                else
                {
                    gblFuction.MsgPopup("No Record found...");
                }
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        protected void ddlPolNm_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetInsurancePolicyName(Convert.ToInt32(ddlPolNm.SelectedValue));
        }
        private void popPolicy()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "PolicyId", "PolicyName", "PolicyMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlPolNm.DataSource = dt;
                ddlPolNm.DataTextField = "PolicyName";
                ddlPolNm.DataValueField = "PolicyId";
                ddlPolNm.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlPolNm.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CPAI oPAI = null;
            string vRptPath = "";
            ReportDocument rptDoc = new ReportDocument();
            Int32 pPAI_Id;
            oPAI = new CPAI();
            pPAI_Id = Convert.ToInt32(ViewState["PAI_Id"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            //DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            dt = oPAI.GetPAIDetailsPrint(Session[gblValue.ACVouMst].ToString(), pPAI_Id, vBrCode);
            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\PAI_Insurance_Print.rpt";
            rptDoc.Load(vRptPath);
            rptDoc.SetDataSource(dt);
            rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
            rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
            rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
            rptDoc.SetParameterValue("pBranch", Session[gblValue.BrName].ToString());
            rptDoc.SetParameterValue("pLoginDate", (Session[gblValue.LoginDate].ToString()));
            rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "Insurance_Print");
            rptDoc.Close();
            rptDoc.Dispose();
           
        }

        
    }
}

