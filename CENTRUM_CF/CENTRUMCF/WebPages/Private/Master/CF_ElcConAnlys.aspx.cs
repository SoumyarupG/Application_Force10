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
using System.Net;

namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class CF_ElcConAnlys : CENTRUMBAse
    {
        protected int cPgNo = 1;
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string DocumentBucket = ConfigurationManager.AppSettings["CFDocumentBucket"];
        string AllDownloadPath = ConfigurationManager.AppSettings["AllDownloadPath"];
        string DocumentBucketURL = ConfigurationManager.AppSettings["DocumentBucketURL"];
        string MaxFileSize = ConfigurationManager.AppSettings["MaxFileSize"];
        string FileSize = ConfigurationManager.AppSettings["FileSize"];
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                hdnMaxFileSize.Value = MaxFileSize;
                txtEntryDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                if (Session[gblValue.LeadID] != null)
                {
                    GetElecConAnlys();
                    StatusButton("Show");
                    btnEdit.Enabled = true;
                    Int64 LeadId = Convert.ToInt64(Session[gblValue.LeadID]);
                    CheckOprtnStatus(Convert.ToInt64(LeadId));
                }
                else
                {
                    StatusButton("Close");
                    gblFuction.MsgPopup("Please Select Lead from Loan Application Status screen and proceed...");
                    return;
                }
                tbBasicDet.ActiveTabIndex = 0;
               
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Electricity Consumption Analysis";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFElecConAnlys);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Electricity Consumption Analysis", false);
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
                if (hdIsUpload.Value == "Y")
                {
                    btnEbillDoc.Enabled = true;
                }
                else
                {
                    btnEbillDoc.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbBasicDet.ActiveTabIndex = 0;
          //  EnableControl(false);
            StatusButton("Show");
            btnEdit.Enabled = true;
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }       
        protected void GetElecConAnlys()
        {
            Int64 pAssMtdId = 0;
            string vBrCode = "", vIsFileUpload = "", vIsUpload = "";
            DataTable dt, dt1 = null;
            DataSet ds = new DataSet();
            string vStatus = "";
            ClearControls();
            try
            {
                
                vBrCode = Session[gblValue.BrnchCode].ToString();
              
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
                 
                    
                    
                    CDistrict oDist = new CDistrict();
                    CMember oMem = new CMember();
                    ds = oMem.CF_GetElecConDtlByLeadID(Convert.ToInt64(hdLeadId.Value));
                    dt = ds.Tables[0];                 
                    if (dt.Rows.Count > 0)
                    {
                        hdLeadId.Value = Convert.ToString(dt.Rows[0]["LeadID"]);
                        txtEntryDt.Text = Convert.ToString(dt.Rows[0]["EntryDt"]);
                        txtCnsmrNo.Text = Convert.ToString(dt.Rows[0]["CnsmrNo"]);
                        txtElcBrd.Text = Convert.ToString(dt.Rows[0]["ElcBrd"]);
                        txtLstBillAmt.Text = Convert.ToString(dt.Rows[0]["LstBillAmt"]);
                        txtLstUnit.Text = Convert.ToString(dt.Rows[0]["LstUnit"]);
                        txtP1BillAmt.Text = Convert.ToString(dt.Rows[0]["P1BillAmt"]);
                        txtP1Unit.Text = Convert.ToString(dt.Rows[0]["P1Unit"]);
                        txtP2BillAmt.Text = Convert.ToString(dt.Rows[0]["P2BillAmt"]);
                        txtP2Unit.Text = Convert.ToString(dt.Rows[0]["P2Unit"]);
                        txtP3BillAmt.Text = Convert.ToString(dt.Rows[0]["P3BillAmt"]);
                        txtP3Unit.Text = Convert.ToString(dt.Rows[0]["P3Unit"]);
                        txtP4BillAmt.Text = Convert.ToString(dt.Rows[0]["P4BillAmt"]);
                        txtP4Unit.Text = Convert.ToString(dt.Rows[0]["P4Unit"]);
                        txtP5BillAmt.Text = Convert.ToString(dt.Rows[0]["P5BillAmt"]);
                        txtP5Unit.Text = Convert.ToString(dt.Rows[0]["P5Unit"]);
                        txtP6BillAmt.Text = Convert.ToString(dt.Rows[0]["P6BillAmt"]);
                        txtP6Unit.Text = Convert.ToString(dt.Rows[0]["P6Unit"]);
                        txtP7BillAmt.Text = Convert.ToString(dt.Rows[0]["P7BillAmt"]);
                        txtP7Unit.Text = Convert.ToString(dt.Rows[0]["P7Unit"]);
                        txtP8BillAmt.Text = Convert.ToString(dt.Rows[0]["P8BillAmt"]);
                        txtP8Unit.Text = Convert.ToString(dt.Rows[0]["P8Unit"]);
                        txtP9BillAmt.Text = Convert.ToString(dt.Rows[0]["P9BillAmt"]);
                        txtP9Unit.Text = Convert.ToString(dt.Rows[0]["P9Unit"]);
                        txtP10BillAmt.Text = Convert.ToString(dt.Rows[0]["P10BillAmt"]);
                        txtP10Unit.Text = Convert.ToString(dt.Rows[0]["P10Unit"]);
                        txtP11BillAmt.Text = Convert.ToString(dt.Rows[0]["P11BillAmt"]);
                        txtP11Unit.Text = Convert.ToString(dt.Rows[0]["P11Unit"]);                      
                        txtAvUnit.Text = Convert.ToString(dt.Rows[0]["AvUnit"]);
                        txtEMIAvBill.Text = Convert.ToString(dt.Rows[0]["EMIAvBill"]);
                        lblEBillName.Text = Convert.ToString(dt.Rows[0]["FileName"]);
                        hdEBillName.Value = Convert.ToString(dt.Rows[0]["FileName"]);
                        hdIsUpload.Value = Convert.ToString(dt.Rows[0]["IsUpload"]);
                        vIsUpload = Convert.ToString(dt.Rows[0]["IsUpload"]);
                        txtAvBillAmt.Text = Convert.ToString(dt.Rows[0]["AvBillAmt"]);
                        hdEmiAmt.Value = Convert.ToString(dt.Rows[0]["TotalEMI"]);
                        if (vIsUpload == "Y")
                        {
                            btnEbillDoc.Enabled = true;
                        }
                        else
                        {
                            btnEbillDoc.Enabled = false;
                        }
                        
                    }
                    else
                    {
                        
                        txtEntryDt.Text = Session[gblValue.LoginDate].ToString();
                        txtCnsmrNo.Text = "";
                        txtElcBrd.Text = "";
                        lblEBillName.Text = "";
                        txtLstBillAmt.Text = "0";
                        txtLstBillAmt.Text = "0";
                        txtLstUnit.Text = "0";
                        txtP1BillAmt.Text = "0";
                        txtP1Unit.Text = "0";
                        txtP2BillAmt.Text = "0";
                        txtP2Unit.Text = "0";
                        txtP3BillAmt.Text = "0";
                        txtP3Unit.Text = "0";
                        txtP4BillAmt.Text = "0";
                        txtP4Unit.Text = "0";
                        txtP5BillAmt.Text = "0";
                        txtP5Unit.Text = "0";
                        txtP6BillAmt.Text = "0";
                        txtP6Unit.Text = "0";
                        txtP7BillAmt.Text = "0";
                        txtP7Unit.Text = "0";
                        txtP8BillAmt.Text = "0";
                        txtP8Unit.Text = "0";
                        txtP9BillAmt.Text = "0";
                        txtP9Unit.Text = "0";
                        txtP10BillAmt.Text = "0";
                        txtP10Unit.Text = "0";
                        txtP11BillAmt.Text = "0";
                        txtP11Unit.Text = "0";
                        txtAvBillAmt.Text = "0";
                        txtAvUnit.Text = "0";
                        //txtEMIAvBill.Text = "0";
                        lblEBillName.Text = "";
                        hdIsUpload.Value = "N";
                        btnEbillDoc.Enabled = false;
                        hdEmiAmt.Value = "0";
                        txtEMIAvBill.Text = "0";
                    }
                    
                    tbBasicDet.ActiveTabIndex = 0;
                    ViewState["StateEdit"] = "Edit";
                  //  StatusButton("Show");
                    btnEdit.Enabled = true;

                    
                
            }
            finally
            {
                dt = null;
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
                    btnEbillDoc.Enabled = true;

                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    //btnEmpDoc.Enabled = false;
                    fuEbillUpld.Enabled = false;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnEbillDoc.Enabled = true;
                    fuEbillUpld.Enabled = true;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnEbillDoc.Enabled = false;
                    fuEbillUpld.Enabled = false;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnEbillDoc.Enabled = false;
                    EnableControl(false);
                    break;
                case "Exit":
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    break;
                case "Close":
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
            txtEntryDt.Enabled = Status;
            txtCnsmrNo.Enabled = Status;
            txtElcBrd.Enabled = Status;
            fuEbillUpld.Enabled = Status;
            lblEBillName.Enabled = Status;
            txtLstBillAmt.Enabled = Status;
            txtLstUnit.Enabled = Status;
            txtP1BillAmt.Enabled = Status;
            txtP1Unit.Enabled = Status;
            txtP2BillAmt.Enabled = Status;
            txtP2Unit.Enabled = Status;
            txtP3BillAmt.Enabled = Status;
            txtP3Unit.Enabled = Status;
            txtP4BillAmt.Enabled = Status;
            txtP4Unit.Enabled = Status;
            txtP5BillAmt.Enabled = Status;
            txtP5Unit.Enabled = Status;
            txtP6BillAmt.Enabled = Status;
            txtP6Unit.Enabled = Status;
            txtP7BillAmt.Enabled = Status;
            txtP7Unit.Enabled = Status;
            txtP8BillAmt.Enabled = Status;
            txtP8Unit.Enabled = Status;
            txtP9BillAmt.Enabled = Status;
            txtP9Unit.Enabled = Status;
            txtP10BillAmt.Enabled = Status;
            txtP10Unit.Enabled = Status;
            txtP11BillAmt.Enabled = Status;
            txtP11Unit.Enabled = Status;
            txtAvBillAmt.Enabled = Status;
            txtAvUnit.Enabled = Status;
            txtEMIAvBill.Enabled = Status;

        }
        private void ClearControls()
        {
            txtEntryDt.Text = Session[gblValue.LoginDate].ToString();
            lblBCPNum.Text  = "";
            lblApplNm.Text = "";
            txtCnsmrNo.Text = "";
            txtElcBrd.Text = "";
            lblEBillName.Text = "";
            txtLstBillAmt.Text = "0";
            txtLstUnit.Text = "0";
            txtP1BillAmt.Text = "0";
            txtP1Unit.Text = "0";
            txtP2BillAmt.Text = "0";
            txtP2Unit.Text = "0";
            txtP3BillAmt.Text = "0";
            txtP3Unit.Text = "0";
            txtP4BillAmt.Text = "0";
            txtP4Unit.Text = "0";
            txtP5BillAmt.Text = "0";
            txtP5Unit.Text = "0";
            txtP6BillAmt.Text = "0";
            txtP6Unit.Text = "0";
            txtP7BillAmt.Text = "0";
            txtP7Unit.Text = "0";
            txtP8BillAmt.Text = "0";
            txtP8Unit.Text = "0";
            txtP9BillAmt.Text = "0";
            txtP9Unit.Text = "0";
            txtP10BillAmt.Text = "0";
            txtP10Unit.Text = "0";
            txtP11BillAmt.Text = "0";
            txtP11Unit.Text = "0";
            txtAvBillAmt.Text = "0";
            txtAvUnit.Text = "0";
            txtEMIAvBill.Text = "0";

        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {

        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);

                StatusButton("View");
                ViewState["StateEdit"] = null;
                GetElecConAnlys();
                StatusButton("Show");
                btnEdit.Enabled = true;
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 vErr = 0;
            DateTime vEntryDate = gblFuction.setDate(txtEntryDt.Text.ToString());
            Int32 vMaxFileSize = 0, vLeadID = 0;
            vMaxFileSize = Convert.ToInt32(MaxFileSize);
            string vBrCode = "", vFileUpld = "", vFileName = "", vErrMsg = "", vFileExt = "";
            string vdate = Convert.ToString(Session[gblValue.LoginDate]);
            DateTime vLogDt = gblFuction.setDate(vdate);
            CMember oMem = null;
            try
            {
                if (hdLeadId.Value != "")
                {
                    vLeadID = Convert.ToInt32(hdLeadId.Value);

                }
                if (Convert.ToString(txtCnsmrNo.Text.Trim()) == "")
                {
                    gblFuction.MsgPopup("Consumer No Should Not Be Left Blank..");
                    return false;
                }

                if (Convert.ToString(txtElcBrd.Text.Trim()) == "")
                {
                    gblFuction.MsgPopup("Electricity Board Should Not Be Left Blank..");
                    return false;
                }
                vBrCode = Session[gblValue.BrnchCode].ToString();
                
                if (txtLstBillAmt.Text == "") {txtLstBillAmt.Text = "0";}               
                if (txtLstUnit.Text == "") { txtLstUnit.Text = "0"; }
                if (Convert.ToInt32(txtLstBillAmt.Text) == 0 )
                {
                    gblFuction.MsgPopup("Last Month Bill Amount Should Not Be 0 or Blank..");
                    return false;
                }
                if (Convert.ToInt32(txtLstUnit.Text) == 0 )
                {
                    gblFuction.MsgPopup("Last Month Unit Should Not Be 0 or Blank..");
                    return false;
                }
                if (Convert.ToInt32(txtLstBillAmt.Text) == 0 && Convert.ToInt32(txtLstUnit.Text)>0)
                {
                    gblFuction.MsgPopup("Last Month Bill Amount Should Not Be 0..");
                    return false;
                }
                if (Convert.ToInt32(txtLstUnit.Text) == 0 && Convert.ToInt32(txtLstBillAmt.Text) > 0)
                {
                    gblFuction.MsgPopup("Last Month Unit Should Not Be 0..");
                    return false;
                }
                if (txtP1BillAmt.Text == "") { txtP1BillAmt.Text = "0"; }
                if (txtP1Unit.Text == "") { txtP1Unit.Text = "0"; }
                if (Convert.ToInt32(txtP1BillAmt.Text) == 0 && Convert.ToInt32(txtP1Unit.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 1 Bill Amount Should Not Be 0..");
                    return false;
                }
                if (Convert.ToInt32(txtP1Unit.Text) == 0 && Convert.ToInt32(txtP1BillAmt.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 1 Unit Should Not Be 0..");
                    return false;
                }
                if (txtP2BillAmt.Text == "") { txtP2BillAmt.Text = "0"; }
                if (txtP2Unit.Text == "") { txtP2Unit.Text = "0"; }
                if (Convert.ToInt32(txtP2BillAmt.Text) == 0 && Convert.ToInt32(txtP2Unit.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 2 Bill Amount Should Not Be 0..");
                    return false;
                }
                if (Convert.ToInt32(txtP2Unit.Text) == 0 && Convert.ToInt32(txtP2BillAmt.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 2 Unit Should Not Be 0..");
                    return false;
                }
                if (txtP3BillAmt.Text == "") { txtP3BillAmt.Text = "0"; }
                if (txtP3Unit.Text == "") { txtP3Unit.Text = "0"; }
                if (Convert.ToInt32(txtP3BillAmt.Text) == 0 && Convert.ToInt32(txtP3Unit.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 3 Bill Amount Should Not Be 0..");
                    return false;
                }
                if (Convert.ToInt32(txtP3Unit.Text) == 0 && Convert.ToInt32(txtP3BillAmt.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 3 Unit Should Not Be 0..");
                    return false;
                }
                if (txtP4BillAmt.Text == "") { txtP4BillAmt.Text = "0"; }
                if (txtP4Unit.Text == "") { txtP4Unit.Text = "0"; }
                if (Convert.ToInt32(txtP4BillAmt.Text) == 0 && Convert.ToInt32(txtP4Unit.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 4 Bill Amount Should Not Be 0..");
                    return false;
                }
                if (Convert.ToInt32(txtP4Unit.Text) == 0 && Convert.ToInt32(txtP4BillAmt.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 4 Unit Should Not Be 0..");
                    return false;
                }
                if (txtP5BillAmt.Text == "") { txtP5BillAmt.Text = "0"; }
                if (txtP5Unit.Text == "") { txtP5Unit.Text = "0"; }
                if (Convert.ToInt32(txtP5BillAmt.Text) == 0 && Convert.ToInt32(txtP5Unit.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 5 Bill Amount Should Not Be 0..");
                    return false;
                }
                if (Convert.ToInt32(txtP5Unit.Text) == 0 && Convert.ToInt32(txtP5BillAmt.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 5 Unit Should Not Be 0..");
                    return false;
                }
                if (txtP6BillAmt.Text == "") { txtP6BillAmt.Text = "0"; }
                if (txtP6Unit.Text == "") { txtP6Unit.Text = "0"; }
                if (Convert.ToInt32(txtP6BillAmt.Text) == 0 && Convert.ToInt32(txtP6Unit.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 6 Bill Amount Should Not Be 0..");
                    return false;
                }
                if (Convert.ToInt32(txtP6Unit.Text) == 0 && Convert.ToInt32(txtP6BillAmt.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 6 Unit Should Not Be 0..");
                    return false;
                }
                if (txtP7BillAmt.Text == "") { txtP7BillAmt.Text = "0"; }
                if (txtP7Unit.Text == "") { txtP7Unit.Text = "0"; }
                if (Convert.ToInt32(txtP7BillAmt.Text) == 0 && Convert.ToInt32(txtP7Unit.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 7 Bill Amount Should Not Be 0..");
                    return false;
                }
                if (Convert.ToInt32(txtP7Unit.Text) == 0 && Convert.ToInt32(txtP7BillAmt.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 7 Unit Should Not Be 0..");
                    return false;
                }
                if (txtP8BillAmt.Text == "") { txtP8BillAmt.Text = "0"; }
                if (txtP8Unit.Text == "") { txtP8Unit.Text = "0"; }
                if (Convert.ToInt32(txtP8BillAmt.Text) == 0 && Convert.ToInt32(txtP8Unit.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 8 Bill Amount Should Not Be 0..");
                    return false;
                }
                if (Convert.ToInt32(txtP8Unit.Text) == 0 && Convert.ToInt32(txtP8BillAmt.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 8 Unit Should Not Be 0..");
                    return false;
                }
                if (txtP9BillAmt.Text == "") { txtP9BillAmt.Text = "0"; }
                if (txtP9Unit.Text == "") { txtP9Unit.Text = "0"; }
                if (Convert.ToInt32(txtP9BillAmt.Text) == 0 && Convert.ToInt32(txtP9Unit.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 9 Bill Amount Should Not Be 0..");
                    return false;
                }
                if (Convert.ToInt32(txtP9Unit.Text) == 0 && Convert.ToInt32(txtP9BillAmt.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 9 Unit Should Not Be 0..");
                    return false;
                }
                if (txtP10BillAmt.Text == "") { txtP10BillAmt.Text = "0"; }
                if (txtP10Unit.Text == "") { txtP10Unit.Text = "0"; }
                if (Convert.ToInt32(txtP10BillAmt.Text) == 0 && Convert.ToInt32(txtP10Unit.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 10 Bill Amount Should Not Be 0..");
                    return false;
                }
                if (Convert.ToInt32(txtP10Unit.Text) == 0 && Convert.ToInt32(txtP10BillAmt.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 10 Unit Should Not Be 0..");
                    return false;
                }
                if (txtP11BillAmt.Text == "") { txtP11BillAmt.Text = "0"; }
                if (txtP11Unit.Text == "") { txtP11Unit.Text = "0"; }
                if (Convert.ToInt32(txtP11BillAmt.Text) == 0 && Convert.ToInt32(txtP11Unit.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 11 Bill Amount Should Not Be 0..");
                    return false;
                }
                if (Convert.ToInt32(txtP11Unit.Text) == 0 && Convert.ToInt32(txtP11BillAmt.Text) > 0)
                {
                    gblFuction.MsgPopup("Prev. Month 11 Unit Should Not Be 0..");
                    return false;
                }
                if (txtAvBillAmt.Text == "") { txtAvBillAmt.Text = "0"; }
                if (txtAvUnit.Text == "") { txtAvUnit.Text = "0"; }
                if (txtEMIAvBill.Text == "") { txtEMIAvBill.Text = "0"; }
                

                vFileUpld = fuEbillUpld.HasFile == true ? "Y" : "N";
                if (vFileUpld == "Y")
                {
                    vFileExt = System.IO.Path.GetExtension(fuEbillUpld.PostedFile.FileName).ToLower();
                    if (vFileExt.ToLower() == ".pdf")
                    {                        
                        vFileName = "ElectricityBill" + vFileExt;                        
                    }
                    else
                    {
                        gblFuction.MsgPopup("Only Pdf File Can Be Upload...");
                        return false;
                    }

                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vFileExt.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(fuEbillUpld.FileBytes);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuEbillUpld.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------
                }
                else
                {
                    if (hdIsUpload.Value == "N")
                    {
                        gblFuction.MsgPopup("Please Upload Electricity Bill...");
                        vFileName = "";
                        return false;
                    }
                    else
                    {
                        vFileName = lblEBillName.Text;
                    }
                }
                if (fuEbillUpld.PostedFile.ContentLength > vMaxFileSize)
                {
                    gblFuction.MsgPopup("Maximum upload file Size exceed the limit.File Size Should Be Maximum " + Convert.ToString(FileSize));
                    return false;
                }

                if (vEntryDate > vLogDt)
                {
                    gblFuction.MsgPopup("Entry Date Should Not Be Greater Than Login Date ...");
                    return false;
                }


                oMem = new CMember();
                vErr = oMem.CF_SaveElecConDtl(vLeadID, vEntryDate, Convert.ToString(txtCnsmrNo.Text.Trim()), Convert.ToString(txtElcBrd.Text.Trim()), Convert.ToDouble(txtLstBillAmt.Text),
                    Convert.ToDouble(txtLstUnit.Text), Convert.ToDouble(txtP1BillAmt.Text), Convert.ToDouble(txtP1Unit.Text), Convert.ToDouble(txtP2BillAmt.Text),
                    Convert.ToDouble(txtP2Unit.Text), Convert.ToDouble(txtP3BillAmt.Text), Convert.ToDouble(txtP3Unit.Text), Convert.ToDouble(txtP4BillAmt.Text),
                    Convert.ToDouble(txtP4Unit.Text), Convert.ToDouble(txtP5BillAmt.Text), Convert.ToDouble(txtP5Unit.Text), Convert.ToDouble(txtP6BillAmt.Text),
                    Convert.ToDouble(txtP6Unit.Text), Convert.ToDouble(txtP7BillAmt.Text), Convert.ToDouble(txtP7Unit.Text), Convert.ToDouble(txtP8BillAmt.Text),
                    Convert.ToDouble(txtP8Unit.Text), Convert.ToDouble(txtP9BillAmt.Text), Convert.ToDouble(txtP9Unit.Text), Convert.ToDouble(txtP10BillAmt.Text),
                    Convert.ToDouble(txtP10Unit.Text), Convert.ToDouble(txtP11BillAmt.Text), Convert.ToDouble(txtP11Unit.Text), Convert.ToDouble(txtAvBillAmt.Text),
                    Convert.ToDouble(txtAvUnit.Text), Convert.ToDouble(txtEMIAvBill.Text), vFileName, vFileUpld, DocumentBucketURL, vBrCode, this.UserID, 0, ref vErrMsg);
                if (vErr == 0)
                {
                    if (fuEbillUpld.HasFile)
                    {
                        SaveMemberImages(fuEbillUpld, Convert.ToString(vLeadID), Convert.ToString(vLeadID) + "_ElectricityBill", vFileExt, "N");
                    }
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    vResult = true;
                }
                else
                {
                    gblFuction.MsgPopup(vErrMsg);
                    vResult = false;
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
        private string SaveMemberImages(FileUpload flup, string imageGroup, string imageName, string ImgExt, string isImageSaved)
        {
            CApiCalling oAC = new CApiCalling();
            byte[] ImgByte = ConvertFileToByteArray(flup.PostedFile);
            isImageSaved = oAC.UploadFileMinio(ImgByte, imageName + ImgExt, imageGroup, DocumentBucket, MinioUrl);
            return isImageSaved;
        }
        private byte[] ConvertFileToByteArray(HttpPostedFile postedFile)
        {
            using (Stream stream = postedFile.InputStream)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
        public bool ValidUrlChk(string pUrl)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pUrl);
                request.Timeout = 5000;
                using (WebResponse response = request.GetResponse())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        protected void btnEbillDoc_Click(object sender, EventArgs e)
        {

            string ActNetImage = "", vPdfFile = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdLeadId.Value + "_" + lblEBillName.Text;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                WebClient cln = null;
                cln = new WebClient();
                byte[] vDoc = null;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                cln = new WebClient();
                vDoc = cln.DownloadData(vPdfFile);
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdLeadId.Value + "_" + lblEBillName.Text);
                Response.BinaryWrite(vDoc);
                Response.Flush();
                Response.End();
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }
        }
    }
}