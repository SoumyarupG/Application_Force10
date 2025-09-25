using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.Data;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class MedClaim : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                hdUserID.Value = this.UserID.ToString();
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                txtMedDt.Text = Session[gblValue.LoginDate].ToString();
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                LoadGrid( 1);
                StatusButton("View");

            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Mediclaim";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuMedCL);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Mediclaim", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }


        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (ValidDate() == true)
            {
                try
                {
                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    
                   //LoadGrid( txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        protected void chkApp_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            TextBox txtMedAmt = (TextBox)row.FindControl("txtMedAmt");
            if (checkbox.Checked == true)
            {
                txtMedAmt.Text = row.Cells[8].Text;
            }
            else
            {
                txtMedAmt.Text = "0.0";
            }
        }
        private bool ValidDate()
        {
            Boolean vResult = true;
            if (txtFrmDt.Text.Trim() != "" || txtFrmDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtFrmDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtFrDt");
                    vResult = false;
                }
            }
            if (txtToDt.Text.Trim() != "" || txtToDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtToDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtToDt");
                    vResult = false;
                }
            }
            
            return vResult;
        }


        
        

        private Boolean ValidateFields()//To Check
        {
            Boolean vResult = true;
            //Int32 vRow = 0;
            //for (vRow = 0; vRow < gvSanc.Rows.Count; vRow++)
            //{
            //    CheckBox chkApp = (CheckBox)gvSanc.Rows[vRow].FindControl("chkApp");
            //    CheckBox chkCan = (CheckBox)gvSanc.Rows[vRow].FindControl("chkCan");
            //    TextBox txtSanAmt = (TextBox)gvSanc.Rows[vRow].FindControl("txtSanAmt");
            //    TextBox txtSanDt = (TextBox)gvSanc.Rows[vRow].FindControl("txtSanDt");
            //    TextBox txtReason = (TextBox)gvSanc.Rows[vRow].FindControl("txtReason");
            //    TextBox txtEQFDt = (TextBox)gvSanc.Rows[vRow].FindControl("txtEQFDt");
            //    TextBox txtNoMfi = (TextBox)gvSanc.Rows[vRow].FindControl("txtNoMfi");
            //    TextBox txtTOTOS = (TextBox)gvSanc.Rows[vRow].FindControl("txtTOTOS");
            //    if (chkApp.Checked == true || chkCan.Checked == true)
            //    {
            //        if (chkApp.Checked == true && txtSanDt.Text.Trim() == "")
            //        {
            //            gblFuction.MsgPopup("Please Enter Sanction Date..");
            //            vResult = false;
            //        }

            //        if (chkApp.Checked == true && txtSanAmt.Text.Trim() == "")
            //        {
            //            gblFuction.MsgPopup("Please Enter Sanction Amount..");
            //            vResult = false;
            //        }

            //        if (chkCan.Checked == true && txtReason.Text.Trim() == "")
            //        {
            //            gblFuction.MsgPopup("Please select the Reason for Cancel loan..");
            //            vResult = false;
            //        }

            //        if (chkCan.Checked == true && txtReason.Text.Trim() == "")
            //        {
            //            gblFuction.MsgPopup("Please select the Reason for Cancel loan..");
            //            vResult = false;
            //        }

            //        if (chkApp.Checked == true && gblFuction.IsDate(txtSanDt.Text) == false)
            //        {
            //            gblFuction.MsgPopup("Please enter Proper Sanction Date..");
            //            vResult = false;
            //        }

            //        if (txtEQFDt.Text.Trim() != "" && gblFuction.IsDate(txtEQFDt.Text) == false)
            //        {
            //            gblFuction.MsgPopup("Please enter Proper Eqi Fax Date..");
            //            vResult = false;
            //        }
            //        if (txtEQFDt.Text.Trim() != "" && gblFuction.IsDate(txtEQFDt.Text) == false)
            //        {
            //            gblFuction.MsgPopup("Please enter Proper Eqi Fax Date..");
            //            vResult = false;
            //        }

            //        if (txtNoMfi.Text.Trim() == "")
            //        {
            //            gblFuction.MsgPopup("No of MFI Should Not be Blank..");
            //            vResult = false;
            //        }

            //        if (txtTOTOS.Text.Trim() == "")
            //        {
            //            gblFuction.MsgPopup("Total Outstanding Should Not be Blank..");
            //            vResult = false;
            //        }
            //    }
            //    if (chkApp.Checked == false && chkCan.Checked == false)
            //    {

            //    }
            //}
            return vResult;
        }

        

        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }


        protected void ChangePage(object sender, CommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }

            LoadGrid( cPgNo);
            //tabLoanAppl.ActiveTabIndex = 0;
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
                ViewState["StateEdit"] = null;
                tabWoff.ActiveTabIndex = 1;
                StatusButton("Add");
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
                tabWoff.ActiveTabIndex = 0;
                EnableControl(false);
                StatusButton("View");
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
                if (this.RoleId != 1)
                {
                    //if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtWofDt.Text))
                    //{
                    //    gblFuction.AjxMsgPopup("You can not Delete, Day end already done..");
                    //    return;
                    //}
                }
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                //if (SaveRecords("Delete") == true)
                //{
                //    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                //    LoadGrid(0);
                //    StatusButton("Delete");
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                if (vStateEdit == "" || vStateEdit == null)
                    vStateEdit = "Save";
                if (SaveRecords(vStateEdit) == true)
                {
                    gblFuction.MsgPopup(gblMarg.SaveMsg);
                    LoadGrid(1);
                    StatusButton("View");
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
                if (this.RoleId != 1)
                {
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtMedDt.Text))
                    {
                        gblFuction.AjxMsgPopup("You can not edit, Day end already done..");
                        return;
                    }
                }
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                ViewState["StateEdit"] = "Edit";
                gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_ddlCo");
                StatusButton("Edit");
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


        private void ClearControls()
        {
            ddlCo.SelectedIndex = -1;
            ddlCenter.SelectedIndex = -1;
            txtMedAmt.Text = "0.0";
            //ddlMem.SelectedIndex = -1;
            txtMedDt.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            ddlLoan.SelectedIndex = -1;
            lblUser.Text = "";
            lblDate.Text = "";
        }


        private void EnableControl(bool Status)
        {
            ddlCo.Enabled = Status;
            //txtPrinOs.Enabled=Status;
            //txtWAmt.Enabled=Status;
            //ddlMem.Enabled = Status;
            txtMedDt.Enabled = Status;
            ddlLoan.Enabled = Status;
            ddlCenter.Enabled = Status;
        }
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    //gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    //btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    //gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        


        


     
      

      

       
        

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            DataTable dt = null;
            string vStateEdit = string.Empty, vBrCode = string.Empty, vAppNo = string.Empty;
            string vXmlLoan = string.Empty, vXmlAC = string.Empty, vDescID = string.Empty;
            Int32 vAppId = 0, vErr = 0, vYrNo = 0;
            double vMedAmt = 0, vPrinAmt = 0, vIntAmt = 0;
            string vLoanId = "", vLoanAc = "", vMedCLAC = "", vTblMst = "", vTblDtl = "", vFinYear = "", vMedCLYN="N";
            DateTime vMedCLDt = gblFuction.setDate(txtMedDt.Text);
            string vNaration = "Being the Amt of Mediclaim ";
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            CMediClaim oWF = null;
            CDisburse oLD = null;
            Int32 vLnTypId = 0;
            CGenParameter oGP = null;
            try
            {
               
                //if (Convert.ToDouble(txtMedAmt.Text) <= 0)
                //{
                //    gblFuction.MsgPopup("Mediclaim Amt should grater than zero...");
                //    return false;
                //}
                if (gblFuction.setDate(txtMedDt.Text) > vLoginDt)
                {
                    gblFuction.MsgPopup("Mediclaim date should not grater than login date...");
                    txtMedDt.Text = Session[gblValue.LoginDate].ToString();
                    return false;
                }
                if (gblFuction.setDate(hdnLoanDt.Value) > gblFuction.setDate(txtMedDt.Text))
                {
                    gblFuction.MsgPopup("Mediclaim date should not less than Loan date...");
                    return false;
                }
                oLD = new CDisburse();
                vLoanId = ddlLoan.SelectedValue;



                vMedCLAC = hdnMedCLAc.Value;


                if (vMedCLAC == "")
                {
                    gblFuction.MsgPopup("Mediclaim A/c is not Set for this Loan Type");
                    return false;
                }
                if (vMedCLDt < vFinFrom || vMedCLDt > vFinTo)
                {
                    gblFuction.MsgPopup("Mediclaim Date should be Financial Year.");
                    return false;
                }
                //vReasonId = Convert.ToInt32(ddlReason.SelectedValue);
                vYrNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());

                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }
                if (chkMedApp.Checked == true)
                {
                    vMedCLYN = "Y";
                }
                else
                {
                    vMedCLYN = "N";
                }
                vMedAmt = Convert.ToDouble(Request[txtMedAmt.UniqueID] as string == "" ? "0" : Request[txtMedAmt.UniqueID] as string);
                vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                vAppId = Convert.ToInt32(ViewState["AppId"]);
                vTblMst = Session[gblValue.ACVouMst].ToString();
                vTblDtl = Session[gblValue.ACVouDtl].ToString();
                vFinYear = Session[gblValue.ShortYear].ToString();

                //------Create Account table(Bank)----
                DataTable dtAccount = new DataTable();
                DataRow dr;
                DataColumn dc = new DataColumn("DC", System.Type.GetType("System.String"));
                dtAccount.Columns.Add(dc);
                DataColumn dc1 = new DataColumn("Amt", System.Type.GetType("System.Decimal"));
                dtAccount.Columns.Add(dc1);
                DataColumn dc2 = new DataColumn("DescId", System.Type.GetType("System.String"));
                dtAccount.Columns.Add(dc2);
                DataColumn dc3 = new DataColumn("DtlId", System.Type.GetType("System.String"));
                dtAccount.Columns.Add(dc3);
                dtAccount.TableName = "Table1";
                if (vMedAmt > 0)
                {
                    dr = dtAccount.NewRow();
                    dr["DescId"] = vMedCLAC;
                    dr["DC"] = "C";
                    dr["Amt"] = vMedAmt;
                    dr["DtlId"] = 1;
                    dtAccount.Rows.Add(dr);
                    dtAccount.AcceptChanges();

                    dr = dtAccount.NewRow();
                    dr["DescId"] = "C0001";
                    dr["DC"] = "D";
                    dr["Amt"] = vMedAmt;
                    dr["DtlId"] = 2;
                    dtAccount.Rows.Add(dr);
                    dtAccount.AcceptChanges();
                }
                vXmlAC = DataTableTOXml(dtAccount);

                if (Mode == "Save")
                {
                    if (this.RoleId != 1)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtMedDt.Text))
                        {
                            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                            return false;
                        }
                    }
                    oWF = new CMediClaim();

                    vErr = oWF.InsertMediClaim(vLoanId, vMedCLDt, vMedAmt, vMedCLYN,  vBrCode, this.UserID,  vTblMst, vTblDtl, vFinYear, vXmlAC, vNaration, "Save");
                    if (vErr == 0)
                    {
                        ViewState["AppId"] = vAppId;
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
                    oWF = new CMediClaim();

                    vErr = oWF.InsertMediClaim(vLoanId, vMedCLDt, vMedAmt, vMedCLYN, vBrCode, this.UserID, vTblMst, vTblDtl, vFinYear, vXmlAC, vNaration, "Edit");
                    if (vErr == 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                //else if (Mode == "Delete")
                //{
                //    oWF = new CWriteOff();
                //    //vErr = oWF.UpdateWriteOffMst(vAppId, vLoanId, vAppDt, vPrinAmt, vIntAmt, vLnAmt, "O",
                //    //    gblFuction.setDate(""), txtReason.Text.Replace("'", "''"), vBrCode, this.UserID, "D", 0, vTblMst,
                //    //    vTblDtl, vFinYear, vXmlAC, "Delet", vNaration);
                //    vErr = oWF.InsertWriteOffMst(vLoanId, vAppDt, vPrinAmt, vIntAmt, vLnAmt, "O",
                //        gblFuction.setDate(""), txtReason.Text.Replace("'", "''"), vBrCode, this.UserID, "I", 0, vTblMst,
                //        vTblDtl, vFinYear, vXmlAC, vAppvlDt, vNaration, "Del");
                //    if (vErr == 0)
                //        vResult = true;
                //    else
                //    {
                //        gblFuction.MsgPopup(gblMarg.DBError);
                //        vResult = false;
                //    }
                //}
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oWF = null;
                oLD = null;
                oGP = null;
            }
        }

        private string DataTableTOXml(DataTable dt)
        {
            string sXml = "";
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                sXml = oSW.ToString();
            }
            return sXml;
        }

        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CMediClaim oWF = null;
            Int32 vRows = 0;
            string vBrCode = string.Empty;
            try
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }
               
                DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                oWF = new CMediClaim();
                dt = oWF.GetMedCLList(vFrmDt, vToDt, pPgIndx, ref vRows);
                gvMedCL.DataSource = dt.DefaultView;
                gvMedCL.DataBind();
                lblTotalPages.Text = CalTotPgs(vRows).ToString();
                lblCurrentPage.Text = cPgNo.ToString();
                if (cPgNo == 0)
                {
                    Btn_Previous.Enabled = false;
                    if (Int32.Parse(lblTotalPages.Text) > 1)
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oWF = null;
            }
        }
        protected void gvMedCL_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vLoanId = "";
            DataTable dt = null;
            CMediClaim oMC = null;
            try
            {
                vLoanId = Convert.ToString(e.CommandArgument);
                ViewState["AppId"] = vLoanId;
                if (e.CommandName == "cmdShow")
                {
                    oMC = new CMediClaim();
                    dt = oMC.GetMedCLDetails(vLoanId);
                    if (dt.Rows.Count > 0)
                    {
                        
                        txtMedDt.Text = dt.Rows[0]["MedCLDt"].ToString();
                        popRO();
                        ddlCo.SelectedIndex = ddlCo.Items.IndexOf(ddlCo.Items.FindByValue(dt.Rows[0]["EoID"].ToString()));
                        PopGroup(ddlCo.SelectedValue);
                        ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(dt.Rows[0]["Groupid"].ToString()));
                        PopMember();
                        ddlMem.SelectedIndex = ddlMem.Items.IndexOf(ddlMem.Items.FindByValue(dt.Rows[0]["MemberID"].ToString()));
                        popLoan();
                        ddlLoan.SelectedIndex = ddlLoan.Items.IndexOf(ddlLoan.Items.FindByValue(dt.Rows[0]["LoanId"].ToString()));
                        if (dt.Rows[0]["MedClaimYN"].ToString() == "Y")
                        {
                            chkMedApp.Checked = true;
                            txtMedAmt.Text = dt.Rows[0]["MedAmt"].ToString();
                           
                        }
                        else
                        {
                            chkMedApp.Checked = false;
                            txtMedAmt.Text = "0";
                        }
                        hdnMedAmt.Value = dt.Rows[0]["MedAmt"].ToString();
                        hdnLoanDt.Value = dt.Rows[0]["LoanDt"].ToString();
                        hdnMedCLAc.Value = dt.Rows[0]["MedClaimAc"].ToString();
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tabWoff.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oMC = null;
            }
        }

        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode="";
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
            }
            

            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlCo.DataSource = dt;
                ddlCo.DataTextField = "EoName";
                ddlCo.DataValueField = "EoId";
                ddlCo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCo.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        private void PopGroup(string vCOID)
        {
            DataTable dtGr = null;
            CGblIdGenerator oGbl = null;
            try
            {
                ddlCenter.Items.Clear();
                string vBrCode = "";
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }
                
                oGbl = new CGblIdGenerator();
                dtGr = oGbl.PopComboMIS("S", "N", "AA", "GroupID", "GroupName", "GroupMst", vCOID, "MarketID", "DropoutDt", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);
                if (dtGr.Rows.Count > 0)
                {
                    ddlCenter.DataSource = dtGr;
                    ddlCenter.DataTextField = "GroupName";
                    ddlCenter.DataValueField = "GroupID";
                    ddlCenter.DataBind();
                }
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlCenter.Items.Insert(0, oLi);
            }
            finally
            {
                dtGr = null;
                oGbl = null;
            }
        }
        private void PopMember()
        {
            ddlMem.Items.Clear();
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            try
            {
                CMember oMem = new CMember();
                if (Convert.ToString(ddlCo.SelectedValue) != "-1")
                {
                    string vBrCode = "";
                    if (Session[gblValue.BrnchCode].ToString() != "0000")
                    {
                        vBrCode = Session[gblValue.BrnchCode].ToString();
                    }
                    dt = oMem.PopGrpMember(Convert.ToString(ddlCenter.SelectedValue), vBrCode, vLoginDt, "M");
                    ddlMem.DataTextField = "MemberName";
                    ddlMem.DataValueField = "MemberId";
                    ddlMem.DataSource = dt;
                    ddlMem.DataBind();
                    ListItem oItm = new ListItem();
                    oItm.Text = "<--- Select --->";
                    oItm.Value = "-1";
                    ddlMem.Items.Insert(0, oItm);
                }
                ddlMem.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void popLoan()
        {
            DataTable dt = null;
            CWriteOff oCG = null;
            string vMemId = "";
            try
            {
                vMemId = ddlMem.SelectedValue;
                string vBrCode = "";
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }
               
                oCG = new CWriteOff();
                dt = oCG.GetActvLoanMemWise(vMemId, vBrCode, gblFuction.setDate(txtMedDt.Text));
                ddlLoan.DataSource = dt;
                ddlLoan.DataTextField = "LoanNo";
                ddlLoan.DataValueField = "LoanId";
                ddlLoan.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlLoan.Items.Insert(0, oLi);
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }
        
    }
}