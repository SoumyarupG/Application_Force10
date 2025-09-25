using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using System.Data;
using CENTRUMBA;
using System.IO;

namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class CF_Insurance_Charges : CENTRUMBAse
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
              
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;

                if (Session[gblValue.LeadID] != null)
                {
                    GetInsuCharges();                  
                    StatusButton("Edit");
                    Int64 LeadId = Convert.ToInt64(Session[gblValue.LeadID]);
                    CheckOprtnStatus(Convert.ToInt64(LeadId));
                }
                else
                {
                    StatusButton("Close");
                    gblFuction.MsgPopup("Please Select Lead from Loan Application Status screen and proceed...");
                    return;
                }
                tbBasicDet.ActiveTabIndex = 1;
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Insurance and Other Charges";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFInsurence);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {

                    btnEdit.Visible = true;
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Insurance and Other Charges", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        protected void CheckOprtnStatus(Int64 vLeadID)
        {
            Int32 vErr = 0;
            CMember oMem = null;
            oMem = new CMember();
            vErr = oMem.CF_chkOperatnStatus(vLeadID);
            if (vErr == 1)
            {
                btnSave.Enabled = false;
                btnEdit.Enabled = false;
                gblFuction.MsgPopup("This Lead is Under Process at Operation Stage.You can not Change or Update it...");
                return;
            }
            else
            {
                btnSave.Enabled = true;
                btnEdit.Enabled = true;
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
              //  LoadBasicDetailsList(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
              //  ClearControls();
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbBasicDet.ActiveTabIndex = 1;
        //    EnableControl(false);
            StatusButton("Show");
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }              
        void Calculation_Total()
        {
            DataTable dt = (DataTable)ViewState["InsuChrg"];
            double TotalCharge = 0, TotalGST = 0, GrandTotal = 0;

            foreach (GridViewRow gr in gvInsuChrgDtl.Rows)
            {
                TextBox txtChargeAmount = (TextBox)gvInsuChrgDtl.Rows[gr.RowIndex].FindControl("txtChargeAmount");
                TextBox txtGSTAmount = (TextBox)gvInsuChrgDtl.Rows[gr.RowIndex].FindControl("txtGSTAmount");
                TextBox txtGSTPercent = (TextBox)gvInsuChrgDtl.Rows[gr.RowIndex].FindControl("txtGSTPercent");
                TextBox txtChargePercent = (TextBox)gvInsuChrgDtl.Rows[gr.RowIndex].FindControl("txtChargePercent");
                TextBox txtFirstCreateDateTime = (TextBox)gvInsuChrgDtl.Rows[gr.RowIndex].FindControl("txtFirstCreateDateTime");
                TextBox txtModifiedDateTime = (TextBox)gvInsuChrgDtl.Rows[gr.RowIndex].FindControl("txtModifiedDateTime");

                if (txtChargePercent.Text == "0")
                {
                    txtChargeAmount.Enabled = true;
                }
                else
                { 
                    txtChargeAmount.Text = Convert.ToString(Math.Round(((Convert.ToDouble(hdLoanAmount.Value) * Convert.ToDouble(txtChargePercent.Text)) / 100), 2));
                    txtChargeAmount.Enabled = false;                   
                }
                if (txtGSTPercent.Text == "0")
                {
                    txtGSTAmount.Enabled = false;
                    txtGSTAmount.Text = "0";
                }
                else
                {
                    txtGSTAmount.Text = Convert.ToString(Math.Round(((Convert.ToDouble(txtChargeAmount.Text) * Convert.ToDouble(txtGSTPercent.Text)) / 100), 2));
                    txtGSTAmount.Enabled = false;
                }
                if (txtChargeAmount.Text != "0")
                {
                    TotalCharge += Convert.ToDouble(txtChargeAmount.Text);
                }
                if (txtGSTAmount.Text != "0")
                {
                    TotalGST += Convert.ToDouble(txtGSTAmount.Text);
                }
                GrandTotal = TotalCharge + TotalGST;
                txtTotal.Text = GrandTotal.ToString();
                txtTotalCharge.Text = TotalCharge.ToString();
                txtTotalGST.Text = TotalGST.ToString();

                dt.Rows[gr.RowIndex]["GSTAmount"] = txtGSTAmount.Text;
                dt.Rows[gr.RowIndex]["ChargeAmount"] = txtChargeAmount.Text;
                dt.Rows[gr.RowIndex]["ChargePercent"] = txtChargePercent.Text;
                dt.Rows[gr.RowIndex]["GSTPercent"] = txtGSTPercent.Text;
                dt.Rows[gr.RowIndex]["FirstCreateDateTime"] =  txtFirstCreateDateTime.Text;
                dt.Rows[gr.RowIndex]["ModifiedDateTime"] = txtModifiedDateTime.Text;

                dt.AcceptChanges();
            }
            gvInsuChrgDtl.DataSource = dt;
            gvInsuChrgDtl.DataBind();

            ViewState["InsuChrg"] = dt;
        }
        protected void Charge_TextChanged(object sender, EventArgs e)
        {
            Calculation_Total();
        }
        protected void GSTPercent_TextChanged(object sender, EventArgs e)
        {
            Calculation_Total();
        }
        protected void ChargePercent_TextChanged(object sender, EventArgs e)
        {
            Calculation_Total();
        }
        protected void GST_TextChanged(object sender, EventArgs e)
        {
            Calculation_Total();
        }
        protected void gvInsuChrgDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["InsuChrg"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtFirstCreateDateTime = (TextBox)e.Row.FindControl("txtFirstCreateDateTime");
                TextBox txtModifiedDateTime = (TextBox)e.Row.FindControl("txtModifiedDateTime");

                TextBox txtChargeAmount = (TextBox)e.Row.FindControl("txtChargeAmount");
                TextBox txtGSTAmount = (TextBox)e.Row.FindControl("txtGSTAmount");
                TextBox txtGSTPercent = (TextBox)e.Row.FindControl("txtGSTPercent");
                TextBox txtChargePercent = (TextBox)e.Row.FindControl("txtChargePercent");

                if (txtChargePercent.Text == "0")
                {
                    txtChargeAmount.Enabled = true;
                }
                else
                {
                    txtChargeAmount.Enabled = false;
                }
                if (txtGSTPercent.Text == "0")
                {
                   // txtGSTAmount.Enabled = true;
                    txtGSTAmount.Text = "0";
                }
                else
                {
                    txtGSTAmount.Enabled = false;
                }
                if (dt.Rows[e.Row.RowIndex]["FirstCreateDateTime"].ToString() == "")
                {
                    txtFirstCreateDateTime.Text = Session[gblValue.LoginDate].ToString();
                    txtFirstCreateDateTime.Enabled = false;
                }
                else
                {
                    txtFirstCreateDateTime.Enabled = false;
                }

                txtModifiedDateTime.Text = Session[gblValue.LoginDate].ToString();
                txtModifiedDateTime.Enabled = false;

                
            }
        }
        protected void GetInsuCharges()
        {
            Int32 pLeadId = 0;
            string vBrCode = "";
            DataSet ds = new DataSet();
            string vStatus = "";
            ClearControls();
            try
            {
               
                vBrCode = Session[gblValue.BrnchCode].ToString();
                ViewState["LeadId"] = pLeadId;
                hdLeadId.Value = Convert.ToString(pLeadId);
               

                if (Session[gblValue.ApplNm] != null)
                {
                    lblApplNm.Text = Convert.ToString(Session[gblValue.ApplNm]);
                }
                if (Session[gblValue.BCPNO] != null)
                {
                    lblBCPNum.Text = Convert.ToString(Session[gblValue.BCPNO]);
                }
                if (Session[gblValue.LeadID] != null)
                {
                    hdLeadId.Value = Convert.ToString(Session[gblValue.LeadID]);
                }
                if (Session[gblValue.InsuStatus] != null)
                {
                    vStatus = Convert.ToString(Session[gblValue.InsuStatus]);
                }

                CDistrict oDist = new CDistrict();
                if (vStatus != "Pending")
                {
                    ViewState["StateEdit"] = "Edit";
                    StatusButton("Edit");
                }
                else
                {
                    ViewState["StateEdit"] = "Add";
                    StatusButton("Add");
                }

                GenerateInsuranceChargeGrid(Convert.ToInt64(hdLeadId.Value));
                
            }
            finally
            {
            }
        }
        private void GenerateInsuranceChargeGrid(Int64 LeadID)
        {
            DataSet ds = null;
            DataTable dt = null;
            CDistrict ODis = null;
            try
            {
                ODis = new CDistrict();
                ds = ODis.CF_GetInsuranceChargeByLeadID(LeadID);
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    hdLoanAmount.Value = dt.Rows[0]["LoanAmount"].ToString();
                    ViewState["InsuChrg"] = dt;
                    gvInsuChrgDtl.DataSource = dt;
                    gvInsuChrgDtl.DataBind();

                    Calculation_Total();
                    tbBasicDet.ActiveTabIndex = 1;
                   // StatusButton("Show");
                    StatusButton("Edit");
                    ViewState["StateEdit"] = "Edit";
                }
                else
                {
                    hdLoanAmount.Value = "0";
                    gblFuction.MsgPopup("No Loan Amount to Calculate Charges.....");
                    tbBasicDet.ActiveTabIndex = 0;
                    StatusButton("View");
                    ViewState["StateEdit"] = "Add";
                }
            }

            finally
            {
            }
        }        
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;

                    break;
                case "Show":
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Exit":
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    break;
                case "Close":                   
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
            gvInsuChrgDtl.Enabled = Status;
        }
        private void ClearControls()
        {
            lblBCPNum.Text = "";
            lblApplNm.Text = "";
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = true;
            string vXmlInsuChrg = "";
            CDistrict oDis = null;
            string vLeadId = "", vBrCode = "", vErrMsg = "";
            Int32 vErr = 1;
            DataTable dt = (DataTable)ViewState["InsuChrg"];
            try
            {
                if (hdLeadId.Value != "")
                {
                    vLeadId = hdLeadId.Value;
                }


                vBrCode = Session[gblValue.BrnchCode].ToString();
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Table";
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlInsuChrg = oSW.ToString();
                    }

                    oDis = new CDistrict();
                    vErr = oDis.CF_SaveInsuChrg(vLeadId, vXmlInsuChrg, vBrCode, this.UserID, 0, ref vErrMsg);
                    if (vErr == 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrMsg);
                        vResult = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return vResult;

        }

    }
}