using System;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using FORCEBA;
using FORCECA;

namespace  CENTRUM.WebPages.Private.SME
{
    public partial class LeadGeneration : CENTRUMBase
    {
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
                //if (Session[gblValue.BrnchCode].ToString() == "000")
                //    StatusButton("Exit");
                //else
                //    StatusButton("View");
                txtLeadGenDt.Text = Session[gblValue.LoginDate].ToString();
                txtCBupDate.Text = Session[gblValue.LoginDate].ToString();
                txtCBupDate1.Text = Session[gblValue.LoginDate].ToString();
                txtCBupDate2.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                ViewState["LeadId"] = null;
                hdUserID.Value = this.UserID.ToString();
                PopPropertyType();
                PopOccupation();
                LoadLeadList();
                tbLeadGen.ActiveTabIndex = 0;
                StatusButton("View");
                TxtDetails.Visible = false;
                txtOther.Visible = false;
            }
        }
        private void InitBasePage()
        {
            try
            {
                TxtDetails.Visible = false;
                txtOther.Visible = false;
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Lead Generation";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
               // this.GetModuleByRole(mnuID.mnuLeadGeneration);
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
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "General Parameter", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        //protected void btnUploadCBDoc_Click(object sender, EventArgs e)
        //{
        //    CMember oMem = new CMember();
        //    DataTable dt = new DataTable();
        //    if (ViewState["LeadId"] == null)
        //    {
        //        gblFuction.MsgPopup("Please Click on Lead first to select Lead...");
        //        return;
        //    }
        //    dt = oMem.GetLnAppPassYNByLoanid(ViewState["LoanAppId"].ToString());
        //    if (dt.Rows.Count > 0)
        //    {
        //        if (dt.Rows[0]["LnAppPassYN"].ToString() == "N")
        //        {
        //            gblFuction.MsgPopup("You Can Not Upload CB Documents unless it passed from Loan Application Section...");
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        gblFuction.MsgPopup("Please Tick Pass/Reject in Loan Application...");
        //    }
        //    string pLoanAppId = ViewState["LoanAppId"].ToString();
        //    string vAttchBankDocType = "", vDocDesc = "CB";
        //    byte[] vAttachOffDoc = null;
        //    string vAttchOffDocName = "";

        //    int pDocID = 0;
        //    int vError = 0;
        //    if (txtCBupDate.Text == "")
        //    {
        //        gblFuction.AjxMsgPopup("Please Select Upload Date....");
        //        return;
        //    }
        //    if (fuCustCBDoc.HasFile == false)
        //    {
        //        gblFuction.MsgPopup("Please Choose a file to attach....");
        //        return;
        //    }

        //    if (fuCustCBDoc.PostedFile.InputStream.Length > 4194304)
        //    {
        //        gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
        //        return;
        //    }
        //    if (fuCustCBDoc.HasFile)
        //    {
        //        vAttachOffDoc = new byte[fuCustCBDoc.PostedFile.InputStream.Length + 1];
        //        fuCustCBDoc.PostedFile.InputStream.Read(vAttachOffDoc, 0, vAttachOffDoc.Length);
        //        vAttchBankDocType = System.IO.Path.GetExtension(fuCustCBDoc.FileName).ToLower();
        //        vAttchOffDocName = System.IO.Path.GetFileNameWithoutExtension(fuCustCBDoc.FileName);
        //    }
        //    DateTime pUploadDate = gblFuction.setDate(txtCBupDate.Text.Trim());
        //    vError = oMem.SaveCustDoc(pDocID, pLoanAppId, vDocDesc, pUploadDate, vAttachOffDoc, vAttchBankDocType, vAttchOffDocName, Convert.ToInt32(hdUserID.Value), "Save", 0);
        //    if (vError == 0)
        //    {
        //        gblFuction.AjxMsgPopup("Lead Document Uploaded Successfully......");
        //        ShowUploadedCBDoc(pLoanAppId);
        //        return;
        //    }
        //    else
        //    {
        //        gblFuction.MsgPopup(gblWestCapital.DBError);
        //    }
        //}

        protected void btnshowdoc_Click(object sender, EventArgs e)
        {
            if (ViewState["LeadId"] == null)
            {
                gblFuction.MsgPopup("Please Click Lead to select..");
            }
            else
            {
                string vLeadId = "";
                vLeadId = ViewState["LeadId"].ToString();
                ShowUploadedDocuments(vLeadId);
            }
        }

        protected void RadioBtnYN_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioBtnYN.SelectedValue == "Y")
            {
                TxtDetails.Visible = true;
                if (ddlCompTYpe.SelectedValue == "O")
                {
                    txtOther.Visible = true;
                }
                else
                {
                    txtOther.Visible = false;
                }
            }
            else
            {
                TxtDetails.Visible = false;
            }
        }
        protected void ddlCompTYpe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCompTYpe.SelectedValue == "O")
            {
                txtOther.Visible = true;
            }
            else
            {
                txtOther.Visible = false;
            }
        }

        protected void btnshowdoc1_Click(object sender, EventArgs e)
        {

        }

        protected void btnshowdoc2_Click(object sender, EventArgs e)
        { 
        
        }
        protected void btnUploadCBDoc1_Click(object sender, EventArgs e)
        {

            if (ViewState["LeadId"] == null)
            {
                gblFuction.MsgPopup("Please Click Lead to select..");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLeadId = "", vFileName = "", vFilePath = "";
                vLeadId = ViewState["LeadId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fuCustCBDoc1.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select document first..");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLeadId + "-" + "2.pdf";
                        vFilePath = "../../../LeadDocument/" + vFileName;
                        string path = Server.MapPath("../../../LeadDocument/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            //vDel = OMem.DeleteLeadDocuments(vLeadId, vFileName, vFilePath, 0);
                        }

                        fuCustCBDoc1.SaveAs(Server.MapPath("../../../LeadDocument/" + vFileName));
                       // vErr = OMem.SaveLeadDocuments(vLeadId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("File Uploaded Successfully");
                            ShowUploadedDocuments(vLeadId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in  File Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }
            }

        }

        protected void btnUploadCBDoc_Click(object sender, EventArgs e)
        {

            if (ViewState["LeadId"] == null)
            {
                gblFuction.MsgPopup("Please Click Lead to select..");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLeadId = "", vFileName = "", vFilePath = "";
                vLeadId = ViewState["LeadId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fuCustCBDoc.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select document first..");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLeadId + "-" + "1.pdf";
                        vFilePath = "../../../LeadDocument/" + vFileName;
                        string path = Server.MapPath("../../../LeadDocument/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                           // vDel = OMem.DeleteLeadDocuments(vLeadId, vFileName, vFilePath, 0);
                        }

                        fuCustCBDoc.SaveAs(Server.MapPath("../../../LeadDocument/" + vFileName));
                       // vErr = OMem.SaveLeadDocuments(vLeadId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("File Uploaded Successfully");
                            ShowUploadedDocuments(vLeadId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in  File Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }
            }

        }

        protected void btnUploadCBDoc2_Click(object sender, EventArgs e)
        {

            if (ViewState["LeadId"] == null)
            {
                gblFuction.MsgPopup("Please Click Lead to select..");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLeadId = "", vFileName = "", vFilePath = "";
                vLeadId = ViewState["LeadId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fuCustCBDoc2.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select document first..");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLeadId + "-" + "3.pdf";
                        vFilePath = "../../../LeadDocument/" + vFileName;
                        string path = Server.MapPath("../../../LeadDocument/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                          //  vDel = OMem.DeleteLeadDocuments(vLeadId, vFileName, vFilePath, 0);
                        }

                        fuCustCBDoc2.SaveAs(Server.MapPath("../../../LeadDocument/" + vFileName));
                       // vErr = OMem.SaveLeadDocuments(vLeadId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("File Uploaded Successfully");
                            ShowUploadedDocuments(vLeadId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in  File Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }
            }

        }
        protected void ShowUploadedDocuments(string pLeadId)
        {
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
               // dt = OMem.GetUploadDocByLeadId(pLeadId);
                if (dt.Rows.Count > 0)
                {
                    gvLeadDoc.DataSource = dt;
                    gvLeadDoc.DataBind();
                }
                else
                {
                    gvLeadDoc.DataSource = null;
                    gvLeadDoc.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
                dt = null;
            }
        }
        
        protected void ShowUploadedCBDoc(string pLnAppId)
        {
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
               // dt = OMem.GetUploadCBDocByLoanId(pLnAppId, "CB");
                if (dt.Rows.Count > 0)
                {
                    gvLeadDoc.DataSource = dt;
                    gvLeadDoc.DataBind();
                }
                else
                {
                    gvLeadDoc.DataSource = null;
                    gvLeadDoc.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
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
                tbLeadGen.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
                txtLeadID.Enabled = false;
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
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadLeadList();
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
                txtLeadID.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbLeadGen.ActiveTabIndex = 0;
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
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadLeadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
                ClearControls();
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadLeadList();
        }
        private void LoadLeadList()
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
               // ds = oMem.GetLeadList(vBrCode, txtSearch.Text.Trim());
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvLead.DataSource = dt1;
                        gvLead.DataBind();
                    }
                    else
                    {
                        gvLead.DataSource = null;
                        gvLead.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt1 = null;
                oMem = null;
            }

        }
        protected void gvLead_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string pLeadId = "", vInstInv="";
            string vBrCode = "";
            DataTable dt = null;
            try
            {
                pLeadId = Convert.ToString(e.CommandArgument);
                vBrCode = Session[gblValue.BrnchCode].ToString();
                ViewState["LeadId"] = pLeadId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvLead.Rows)
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

                    CDistrict oDist = new CDistrict();
                   // dt = oDist.GetLeadDtlByLeadId(pLeadId);
                    if (dt.Rows.Count > 0)
                    {
                        hdLeadId.Value = Convert.ToString(dt.Rows[0]["LeadId"]);
                        txtLeadID.Text = Convert.ToString(dt.Rows[0]["LeadId"]);
                        txtLeadGenDt.Text = Convert.ToString(dt.Rows[0]["LeadGenerationDate"]);
                        txtCustName.Text = Convert.ToString(dt.Rows[0]["CustomerName"]);
                        txtAddress.Text = Convert.ToString(dt.Rows[0]["Address"]);
                        txtEmail.Text = Convert.ToString(dt.Rows[0]["Email"]);
                        txtMobNo.Text = Convert.ToString(dt.Rows[0]["MobNo"]);
                        txtMAdd1.Text = Convert.ToString(dt.Rows[0]["MAddress1"]).Trim();
                        txtMAdd2.Text = Convert.ToString(dt.Rows[0]["MAddress2"]).Trim();
                        txtMState.Text = Convert.ToString(dt.Rows[0]["MState"]).Trim();
                        txtMDist.Text = Convert.ToString(dt.Rows[0]["MDistrict"]).Trim();
                        txtMPIN.Text = Convert.ToString(dt.Rows[0]["MPIN"]).Trim();
                        if (dt.Rows[0]["LogInFeesCollYN"].ToString() == "Y")
                            ChkLogInFees.Checked = true;
                        else
                            ChkLogInFees.Checked = false;
                        txtTotLogFees.Text = Convert.ToString(dt.Rows[0]["TotalLoginFees"]);
                        txtNetLogFees.Text = Convert.ToString(dt.Rows[0]["NetLogInFees"]);
                        txtLogFeesCGST.Text = Convert.ToString(dt.Rows[0]["CGSTAmt"]);
                        txtLogFeesSGST.Text = Convert.ToString(dt.Rows[0]["SGSTAmt"]);
                        txtLogFeesIGST.Text = Convert.ToString(dt.Rows[0]["IGSTAmt"]);
                        vInstInv = Convert.ToString(dt.Rows[0]["InstInvestorYN"]);
                        ddlOccuType.SelectedIndex = ddlOccuType.Items.IndexOf(ddlOccuType.Items.FindByValue(dt.Rows[0]["OccupationId"].ToString()));
                        ddlPropertyType.SelectedIndex = ddlPropertyType.Items.IndexOf(ddlPropertyType.Items.FindByValue(dt.Rows[0]["PropertyTypeId"].ToString()));
                        ddlCompTYpe.SelectedIndex = ddlCompTYpe.Items.IndexOf(ddlCompTYpe.Items.FindByValue(dt.Rows[0]["CompanyType"].ToString()));
                        txtOther.Text = Convert.ToString(dt.Rows[0]["OtherDetails"]);
                        if (vInstInv == "Y")
                        {
                            RadioBtnYN.Items.FindByValue("Y").Selected = true;
                            TxtDetails.Text = Convert.ToString(dt.Rows[0]["InsDetails"]);
                            TxtDetails.Visible = true;
                        }
                        else
                        {
                            RadioBtnYN.Items.FindByValue("N").Selected = true;
                            TxtDetails.Text = Convert.ToString(dt.Rows[0]["InsDetails"]);
                            TxtDetails.Visible = false;
                        }
                        TxtPan.Text = Convert.ToString(dt.Rows[0]["PanNo"]);
                        hdGenParameterId.Value = Convert.ToString(dt.Rows[0]["GenParameterId"]);
                        tbLeadGen.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            //if (Session[gblValue.BrnchCode].ToString() == "000")
            //{
            //    gblFuction.MsgPopup("New Lead Generation/Update Existing Lead Information Can be Done From Branch.Please Log In To Branch");
            //    return false;
            //}
            Int32 vErr = 0, vProTypeId = 0, vOccTypeId = 0, vGenParId = 0,vRec = 0;
            Decimal vTotLogFees = 0, vNetLogFees = 0, vCGSTAmt = 0, vSGSTAmt = 0, vIGSTAmt = 0;
            string vMAddress1 = "", vMAddress2 = "", vMState = "", vMDistrict = "", vMPIN = "", vNewId = "", vLeadId = "", vCustomerName = "", vEmail = "", vMobNo = "", vAddress = "",vCompType="", vInstInv="",VcompDetails="",vInstDetails="",vPanNo="",
                vLogInFeesCollYN = "N", vBrCode = "", vErrMsg = "",vlastchar="",vMiddlestring="";
            CMember oMem = null;
            CGblIdGenerator oGbl = null;

            try
            {

               
                vCompType = ddlCompTYpe.SelectedValue;
                if (RadioBtnYN.SelectedValue == "Y")
                {
                    vInstInv = "Y";
                }
                else
                {
                    vInstInv = "N";
                }
                if (hdLeadId.Value != "")
                {
                    vLeadId = hdLeadId.Value;
                }
                VcompDetails = txtOther.Text.Replace("'", "''");
                vInstDetails = TxtDetails.Text.Replace("'", "''");
                vPanNo = TxtPan.Text.Replace("'", "''");
                vPanNo = vPanNo.Substring(0, 5);
                vMAddress1 = Convert.ToString((Request[txtMAdd1.UniqueID] as string == null) ? txtMAdd1.Text : Request[txtMAdd1.UniqueID] as string);
                vMAddress2 = Convert.ToString((Request[txtMAdd2.UniqueID] as string == null) ? txtMAdd2.Text : Request[txtMAdd2.UniqueID] as string);
                vMState = Convert.ToString((Request[txtMState.UniqueID] as string == null) ? txtMState.Text : Request[txtMState.UniqueID] as string);
                vMDistrict = Convert.ToString((Request[txtMDist.UniqueID] as string == null) ? txtMDist.Text : Request[txtMDist.UniqueID] as string);
                vMPIN = Convert.ToString((Request[txtMPIN.UniqueID] as string == null) ? txtMPIN.Text : Request[txtMPIN.UniqueID] as string);
                foreach (char item in vPanNo)
                {
                    if (char.IsDigit(item))
                    {
                        gblFuction.MsgPopup("Wrong PAN No Format...");
                        return false;
                    }
                }
                vMiddlestring = TxtPan.Text.Replace("'", "''");
                vMiddlestring = vMiddlestring.Substring(5, 4);
                foreach (char item in vMiddlestring)
                {
                    if (!char.IsDigit(item))
                    {
                        gblFuction.MsgPopup("Wrong PAN No Format...");
                        return false;
                    }
                }
                vlastchar = TxtPan.Text.Replace("'", "''");
                vlastchar = vlastchar.Substring(9, 1);
                foreach (char item in vlastchar)
                {
                    if (char.IsDigit(item))
                    {
                        gblFuction.MsgPopup("Wrong PAN No Format...Last character should not be number");
                        return false;
                    }
                }

                vPanNo = TxtPan.Text.Replace("'", "''");

                vCustomerName = txtCustName.Text.ToString();
                vEmail = txtEmail.Text.ToString();
                vMobNo = txtMobNo.Text.ToString();
                vAddress = txtAddress.Text.ToString();
                if (ChkLogInFees.Checked == true)
                    vLogInFeesCollYN = "Y";
                else
                    vLogInFeesCollYN = "N";
                vBrCode = Session[gblValue.BrnchCode].ToString();
                if (((Request[hdGenParameterId.UniqueID] as string == null) ? hdGenParameterId.Value : Request[hdGenParameterId.UniqueID] as string) != "")
                {
                    vGenParId = Convert.ToInt32((Request[hdGenParameterId.UniqueID] as string == null) ? hdGenParameterId.Value : Request[hdGenParameterId.UniqueID] as string);
                }
                else
                {
                    vGenParId = 0;
                }
                if (txtLeadGenDt.Text == "")
                {
                    gblFuction.MsgPopup("Lead Generation Date Can Not Be Empty..");
                    return false;
                }
                DateTime pLeadGenDt = gblFuction.setDate(txtLeadGenDt.Text.ToString());
                if (((Request[txtTotLogFees.UniqueID] as string == null) ? txtTotLogFees.Text : Request[txtTotLogFees.UniqueID] as string) != "")
                    vTotLogFees = Convert.ToDecimal(((Request[txtTotLogFees.UniqueID] as string == null) ? txtTotLogFees.Text : Request[txtTotLogFees.UniqueID] as string));
                if (((Request[txtNetLogFees.UniqueID] as string == null) ? txtNetLogFees.Text : Request[txtNetLogFees.UniqueID] as string) != "")
                    vNetLogFees = Convert.ToDecimal((Request[txtNetLogFees.UniqueID] as string == null) ? txtNetLogFees.Text : Request[txtNetLogFees.UniqueID] as string);
                if (((Request[txtLogFeesCGST.UniqueID] as string == null) ? txtLogFeesCGST.Text : Request[txtLogFeesCGST.UniqueID] as string) != "")
                    vCGSTAmt = Convert.ToDecimal((Request[txtLogFeesCGST.UniqueID] as string == null) ? txtLogFeesCGST.Text : Request[txtLogFeesCGST.UniqueID] as string);
                if (((Request[txtLogFeesSGST.UniqueID] as string == null) ? txtLogFeesSGST.Text : Request[txtLogFeesSGST.UniqueID] as string) != "")
                    vSGSTAmt = Convert.ToDecimal((Request[txtLogFeesSGST.UniqueID] as string == null) ? txtLogFeesSGST.Text : Request[txtLogFeesSGST.UniqueID] as string);
                if (((Request[txtLogFeesIGST.UniqueID] as string == null) ? txtLogFeesIGST.Text : Request[txtLogFeesIGST.UniqueID] as string) != "")
                    vIGSTAmt = Convert.ToDecimal((Request[txtLogFeesIGST.UniqueID] as string == null) ? txtLogFeesIGST.Text : Request[txtLogFeesIGST.UniqueID] as string);

                if (((Request[ddlPropertyType.UniqueID] as string == null) ? ddlPropertyType.SelectedValue : Request[ddlPropertyType.UniqueID] as string) != "-1")
                    vProTypeId = Convert.ToInt32(((Request[ddlPropertyType.UniqueID] as string == null) ? ddlPropertyType.SelectedValue : Request[ddlPropertyType.UniqueID] as string));
                if (((Request[ddlOccuType.UniqueID] as string == null) ? ddlOccuType.SelectedValue : Request[ddlOccuType.UniqueID] as string) != "-1")
                    vOccTypeId = Convert.ToInt32(((Request[ddlOccuType.UniqueID] as string == null) ? ddlOccuType.SelectedValue : Request[ddlOccuType.UniqueID] as string));

                if (vLogInFeesCollYN == "Y" && vTotLogFees <= 0)
                {
                    gblFuction.MsgPopup("LogIn Fees Amount Can Be Zero");
                    return false;
                }
                if (Mode == "Save")
                {
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("LeadMaster", "MobNo", txtMobNo.Text.Replace("'", "''"), "", "", "", "", Mode);
                    if (vRec > 0)
                    {
                        //gblFuction.MsgPopup("Mobile No Can not be Duplicate...");
                        //return false;
                    }
                    vRec = oGbl.ChkDuplicate("LeadMaster", "PanNo", TxtPan.Text.Replace("'", "''"), "", "", "", "", Mode);
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("PAN No Can not be Duplicate...");
                        return false;
                    }
                    oMem = new CMember();
                    //vErr = oMem.SaveLead(ref vNewId, vLeadId, pLeadGenDt, vCustomerName, vEmail, vMobNo, vAddress, vProTypeId, vOccTypeId, vLogInFeesCollYN,
                    //   vGenParId, vTotLogFees, vNetLogFees, vCGSTAmt, vSGSTAmt, vIGSTAmt,
                    //   vBrCode, this.UserID, "Save", 0, ref vErrMsg, vCompType, vInstInv, VcompDetails, vInstDetails, vPanNo, vMAddress1, vMAddress2, vMState, vMDistrict, vMPIN);
                    if (vErr == 0)
                    {
                        hdLeadId.Value = vNewId.Trim();
                        ViewState["LeadId"] = vNewId;
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
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
                    //vErr = oMem.UpdateLead(vLeadId, pLeadGenDt, vCustomerName, vEmail, vMobNo, vAddress, vProTypeId, vOccTypeId, vLogInFeesCollYN,
                    //   vGenParId, vTotLogFees, vNetLogFees, vCGSTAmt, vSGSTAmt, vIGSTAmt,
                    //   vBrCode, this.UserID, "Edit", 0, ref vErrMsg, vCompType, vInstInv, VcompDetails, vInstDetails, vPanNo, vMAddress1, vMAddress2, vMState, vMDistrict, vMPIN);

                    if (vErr == 0)
                    {
                        hdLeadId.Value = vLeadId;
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrMsg);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oMem = new CMember();
                    //vErr = oMem.UpdateLead(vLeadId, pLeadGenDt, vCustomerName, vEmail, vMobNo, vAddress, vProTypeId, vOccTypeId, vLogInFeesCollYN,
                    //   vGenParId, vTotLogFees, vNetLogFees, vCGSTAmt, vSGSTAmt, vIGSTAmt,
                    //   vBrCode, this.UserID, "Delete", 0, ref vErrMsg, vCompType, vInstInv, VcompDetails, vInstDetails, vPanNo, vMAddress1, vMAddress2, vMState, vMDistrict, vMPIN);
                    if (vErr == 0)
                    {
                        gblFuction.MsgPopup(gblMarg.DeleteMsg);
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

        protected void gvLeadDoc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDownld")
            {
                int vDocId = Convert.ToInt32(e.CommandArgument.ToString());
                DataTable dt = null;
               // CDocUpLoad oAD = null;
                string vFileTyp = "";
                string vFileName = "";
                GridViewRow gvR = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton imgDownload = (ImageButton)gvR.FindControl("imgDownload");
               // oAD = new CDocUpLoad();
               // dt = oAD.GetLeadDoc(Convert.ToInt32(vDocId));
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["AttachDocName"].ToString() != "")
                    {
                        byte[] fileData = (byte[])dt.Rows[0]["AttachDoc"];
                        vFileTyp = GetFileType(Convert.ToString(dt.Rows[0]["AttachType"]));
                        vFileName = dt.Rows[0]["AttachDocName"].ToString();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = vFileTyp;
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + vFileName + "" + Convert.ToString(dt.Rows[0]["AttachType"]));
                        BinaryWriter bw = new BinaryWriter(Response.OutputStream);
                        bw.Write(fileData);
                        bw.Close();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("No Attachment Found");
                        return;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("No Attachment Found");
                    return;
                }
            }

        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string filePath = gvLeadDoc.DataKeys[gvrow.RowIndex].Value.ToString();
                Response.ContentType = "pdf";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filePath + "\"");
                Response.TransmitFile(Server.MapPath(filePath));
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GetFileType(string pFileTyp)
        {
            string vRst = "";
            switch (pFileTyp)
            {
                case ".txt":
                    vRst = "application/notepad";
                    break;
                case ".doc":
                    vRst = "application/ms-word";
                    break;
                case ".docx":
                    vRst = "application/vnd.ms-word.document.12";
                    break;
                case ".xls":
                    vRst = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    vRst = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".pdf":
                    vRst = "application/vnd.pdf";
                    break;
                case ".zip":
                    vRst = "application/zip";
                    break;
                case ".rar":
                    vRst = "application/WinRar";
                    break;
                case ".exe":
                    vRst = "application/executable";
                    break;
                default:
                    vRst = "";
                    break;
            }
            return vRst;
        }
        private void EnableControl(Boolean Status)
        {
            txtAddress.Enabled = Status;
            txtCustName.Enabled = Status;
            txtEmail.Enabled = Status;
            txtLeadGenDt.Enabled = Status;
            txtMobNo.Enabled = Status;
            txtTotLogFees.Enabled = Status;
            txtNetLogFees.Enabled = Status;
            txtLogFeesCGST.Enabled = Status;
            txtLogFeesSGST.Enabled = Status;
            txtLogFeesIGST.Enabled = Status;
            ddlPropertyType.Enabled = Status;
            ddlOccuType.Enabled = Status;
            ChkLogInFees.Enabled = Status;
            ddlCompTYpe.Enabled = Status;
            txtOther.Enabled = Status;
            TxtPan.Enabled = Status;
            TxtDetails.Enabled = Status;
            txtMAdd1.Enabled = Status;
            txtMAdd2.Enabled = Status;
            txtMDist.Enabled = Status;
            txtMState.Enabled = Status;
            txtMPIN.Enabled = Status;
        }
        private void ClearControls()
        {
            txtAddress.Text = "";
            txtCustName.Text = "";
            txtEmail.Text = "";
            txtMobNo.Text = "";
            txtTotLogFees.Text = "0.00";
            txtNetLogFees.Text = "0.00";
            txtLogFeesCGST.Text = "0.00";
            txtLogFeesSGST.Text = "0.00";
            txtLogFeesIGST.Text = "0.00";
            ddlPropertyType.SelectedIndex = -1;
            ddlOccuType.SelectedIndex = -1;
            ddlCompTYpe.SelectedIndex = -1;
            txtOther.Text = "";
            TxtPan.Text = "";
            TxtDetails.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
            txtMAdd1.Text = "";
            txtMAdd2.Text = "";
            txtMState.Text = "";
            txtMDist.Text = "";
            txtMPIN.Text = "";
            ChkLogInFees.Checked = false;
            gvLeadDoc.Enabled = false;
        }
        private void PopPropertyType()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                //dt = oMem.GetCompanyAndPropertyType(2);
                if (dt.Rows.Count > 0)
                {
                    ddlPropertyType.DataSource = dt;
                    ddlPropertyType.DataTextField = "PropertypeName";
                    ddlPropertyType.DataValueField = "PropertyTypeID";
                    ddlPropertyType.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlPropertyType.Items.Insert(0, oli);
                }
                else
                {
                    ddlPropertyType.DataSource = null;
                    ddlPropertyType.DataBind();
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
        private void PopOccupation()
        {
            ddlOccuType.Items.Clear();
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
               // dt = oMem.GetOccupation();
                if (dt.Rows.Count > 0)
                {
                    ddlOccuType.DataSource = dt;
                    ddlOccuType.DataTextField = "Occupation";
                    ddlOccuType.DataValueField = "OccupationId";
                }
                else
                {
                    ddlOccuType.DataSource = null;
                }
                ddlOccuType.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlOccuType.Items.Insert(0, oli1);
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
    }
}