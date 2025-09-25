using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using CENTRUMCA;
using CENTRUMBA;
using System.Net;
using System.IO;



namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class CF_BankingDetails : CENTRUMBAse
    {

        protected int cPgNo = 1;
        string vPathImage = "";
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string DocumentBucket = ConfigurationManager.AppSettings["CFDocumentBucket"];
        string AllDownloadPath = ConfigurationManager.AppSettings["AllDownloadPath"];
        string DocumentBucketURL = ConfigurationManager.AppSettings["DocumentBucketURL"];
        string MaxFileSize = ConfigurationManager.AppSettings["MaxFileSize"];
        string FileSize = ConfigurationManager.AppSettings["FileSize"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitBasePage();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;

                if (Session[gblValue.LeadID] != null)
                {
                    hdLeadId.Value = Convert.ToString(Session[gblValue.LeadID]);
                    GenerateBankingGrid(Convert.ToInt64(hdLeadId.Value));
                    tbBankDtl.ActiveTabIndex = 1;
                    StatusButton("Show");

                    CheckOprtnStatus(Convert.ToInt64(hdLeadId.Value));
                }
                else
                {
                    StatusButton("View");
                    gblFuction.MsgPopup("Please Select Lead from Loan Application Status screen and proceed...");
                    return;
                }

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
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Banking Details";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFBankingDtl);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {

                    btnCancel.Visible = false;
                    btnSave.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    // btnDelete.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanProcess == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Banking Details", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
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
                //GenerateBankingGrid(hdLeadId.Value);
                StatusButton("Edit");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbBankDtl.ActiveTabIndex = 1;
            StatusButton("Show");
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {

                case "Show":
                    EnableControl(false, this.Page);
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    ViewState["State"] = "Show";
                    gvBankDtl.Enabled = false;
                    break;

                case "View":
                    EnableControl(false, this.Page);
                    btnEdit.Enabled = false;
                    btnShow.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    gvBankDtl.Enabled = false;
                    ViewState["State"] = "View";
                    break;
                case "Exit":
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    break;
                case "Edit":
                    EnableControl(true, this.Page);
                    btnEdit.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ViewState["State"] = "Edit";
                    gvBankDtl.Enabled = true;
                    break;
            }
        }
        private void EnableControl(Boolean Status, Control parent)
        {

            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Enabled = Status;
                }
                else if (c is DropDownList)
                {
                    ((DropDownList)c).Enabled = Status;
                }
                else if (c is FileUpload)
                {
                    ((FileUpload)c).Enabled = Status;
                }

                // Recursively disable controls inside the containers like
                // panels or group controls
                if (c.Controls.Count > 0)
                {
                    EnableControl(Status, c);

                }

            }
            txtSearch.Enabled = true;
            btnShow.Enabled = true;

        }
        private void ClearControls()
        {
            txtRemarks.Text = "";
        }      
        protected void btnSave_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["pathImage"];
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                DataTable dt = (DataTable)ViewState["Bank"];

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    string srlNo = dt.Rows[i]["BankSlId"].ToString();

                    if (dt.Rows[i]["ByteBANKYN"].ToString() == "Y")
                    {
                        byte[] imgArray = (byte[])(dt.Rows[i]["ByteBankDoc"]);

                        string vBankDocName = dt.Rows[i]["BankSatementFileName"].ToString();

                        if (vBankDocName.ToLower().Contains(".pdf"))
                        {
                            vBankDocName = hdLeadId.Value.ToString() + "_" + srlNo + "_" + vBankDocName;
                        }
                        else
                        {
                            vBankDocName = srlNo + "_" + vBankDocName;
                        }
                        if (MinioYN == "N")
                        {
                            string folderPath = string.Format("{0}/{1}", vPathImage, hdLeadId.Value);
                            System.IO.Directory.CreateDirectory(folderPath);
                            string filePath = string.Format("{0}/{1}", folderPath, vBankDocName);
                            File.WriteAllBytes(filePath, imgArray);
                        }
                        else
                        {
                            CApiCalling oAC = new CApiCalling();
                            oAC.UploadFileMinio(imgArray, vBankDocName, hdLeadId.Value, DocumentBucket, MinioUrl);
                        }
                    }
                    if (dt.Rows[i]["ByteBsaYN"].ToString() == "Y")
                    {
                        byte[] imgArrayBsa = (byte[])(dt.Rows[i]["ByteBSADoc"]);

                        string vBsaDocName = dt.Rows[i]["BsaOutFileName"].ToString();
                        if (vBsaDocName.ToLower().Contains(".pdf"))
                        {
                            vBsaDocName = hdLeadId.Value.ToString() + "_" + srlNo + "_" + vBsaDocName;
                        }
                        else
                        {
                            vBsaDocName = srlNo + "_" + vBsaDocName;
                        }
                        if (MinioYN == "N")
                        {
                            string folderPath = string.Format("{0}/{1}", vPathImage, hdLeadId.Value);
                            System.IO.Directory.CreateDirectory(folderPath);
                            string filePath = string.Format("{0}/{1}", folderPath, vBsaDocName);
                            File.WriteAllBytes(filePath, imgArrayBsa);
                        }
                        else
                        {
                            CApiCalling oAC = new CApiCalling();
                            oAC.UploadFileMinio(imgArrayBsa, vBsaDocName, hdLeadId.Value, DocumentBucket, MinioUrl);
                        }
                    }


                }
                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                GenerateBankingGrid(Convert.ToInt64(hdLeadId.Value));
                ViewState["StateEdit"] = null;
                StatusButton("Show");
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 vErr = 0;
            string vBrCode = "", vErrMsg = "";
            DataTable dt = null;
            string vXmlBank = "", VDocumentFilePath = "";
            Int32 vMaxFileSize = 0;
            CDistrict oDis = null;
            Int32 UID = 0;
            Int64 vLeadId = 0;

            vMaxFileSize = Convert.ToInt32(MaxFileSize);
            UID = Convert.ToInt32(Session[gblValue.UserId]);
            try
            {
                if (hdLeadId.Value != "")
                {
                    vLeadId = Convert.ToInt64(hdLeadId.Value);
                }


                vBrCode = Session[gblValue.BrnchCode].ToString();



                if (GetData() == true)
                {
                    dt = (DataTable)ViewState["Bank"];
                    Int32 vNumOfObli = 0;
                    DataRow[] vrows;
                    vrows = dt.Select("AcType <> '' and AcType <> '-1'");
                    vNumOfObli = vrows.Length;
                    if (vNumOfObli == 0)
                    {
                        gblFuction.AjxMsgPopup("Please insert atleast one Bank Details");
                        return false;
                    }

                    dt.AcceptChanges();
                    dt.TableName = "Table";
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlBank = oSW.ToString();
                    }

                    if (dt.Rows.Count > 0)
                    {
                        oDis = new CDistrict();
                        vErr = oDis.CF_SaveBankingDtl(vLeadId, vXmlBank, vBrCode, VDocumentFilePath, UID, 0, ref vErrMsg, txtRemarks.Text);
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
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDis = null;
            }

        }      
        protected void gvBankDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDel")
            {
                DataTable dt = null;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                dt = (DataTable)ViewState["Bank"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[index].Delete();
                    dt.AcceptChanges();
                    ViewState["Bank"] = dt;
                    gvBankDtl.DataSource = dt;
                    gvBankDtl.DataBind();
                }
                else
                {
                    gblFuction.AjxMsgPopup("First Row can not be deleted.");
                    return;
                }
                CalculateTotalCost();
            }
        }
        private void CalculateTotalCost()
        {
            double TotalABB = 0; lblTotalABB.Text = ""; double Cal = 0;

            foreach (GridViewRow gr in gvBankDtl.Rows)
            {
                TextBox txtABB = (TextBox)gvBankDtl.Rows[gr.RowIndex].FindControl("txtABB");
                if (txtABB.Text != "")
                {
                    if (txtABB.Text != "0")
                    {
                        TotalABB += Convert.ToDouble(txtABB.Text);
                    }
                }
            }
            if (TotalABB != 0)
            {
                Cal = Math.Round(TotalABB / Convert.ToDouble(hdnTotalEMI.Value), 2);
                lblTotalABB.Text = Convert.ToString(Cal);
            }
            else
            {
                lblTotalABB.Text = "0";
            }

            GridViewRow footerRow = gvBankDtl.FooterRow;
            if (footerRow != null)
            {
                Label lblABBtotal = (Label)gvBankDtl.FooterRow.FindControl("lblABBtotal");

                if (lblABBtotal != null)
                {
                    if (TotalABB > 0)
                        lblABBtotal.Text = TotalABB.ToString();
                    else
                        lblABBtotal.Text = "0";
                }
                lblABBtotal.ForeColor = System.Drawing.Color.White;

            }
        }
        private void GenerateBankingGrid(Int64 LeadID)
        {
            ClearControls(this.Page);
            DataSet ds = null;
            DataTable dt = null;
            DataTable dt1 = null;
            CDistrict ODis = null;
            try
            {
                if (Session[gblValue.ApplNm] != null)
                {
                    lblApplNm.Text = Convert.ToString(Session[gblValue.ApplNm]);
                }
                if (Session[gblValue.BCPNO] != null)
                {
                    lblBCPNum.Text = Convert.ToString(Session[gblValue.BCPNO]);
                }
                ODis = new CDistrict();
                ds = ODis.CF_GenerateBankingGrid(LeadID);
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                if (dt.Rows.Count == 0)
                {
                    DataRow dF;
                    dF = dt.NewRow();
                    dt.Rows.Add(dF);
                    dt.AcceptChanges();
                }
               
                ViewState["Bank"] = dt;
                gvBankDtl.DataSource = dt;
                gvBankDtl.DataBind();
                if (dt.Rows.Count > 0)
                {
                    txtRemarks.Text = Convert.ToString(dt.Rows[0]["Remarks"]);
                }
                if (dt1.Rows.Count > 0)
                {
                    hdnTotalEMI.Value = Convert.ToString(dt1.Rows[0]["TotalEMI"]);

                }
                else
                {
                    hdnTotalEMI.Value = "0";
                    lblTotalABB.Text = "0";

                }
                CalculateTotalCost();
                tbBankDtl.ActiveTabIndex = 1;
            }

            finally
            {
            }
        }
        protected void txtABB_TextChanged(object sender, EventArgs e)
        {

            CalculateTotalCost();

        }
        protected void gvBankDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Bank"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlAcType = (DropDownList)e.Row.FindControl("ddlAcType");
                ddlAcType.SelectedIndex = ddlAcType.Items.IndexOf(ddlAcType.Items.FindByValue(e.Row.Cells[14].Text));

                HiddenField hdnBankStateUplYN = (HiddenField)e.Row.FindControl("hdnBankStateUplYN");
                Label lblBsaOutFN = (Label)e.Row.FindControl("lblBsaOutFN");
                HiddenField hdnBsaOutUplYN = (HiddenField)e.Row.FindControl("hdnBsaOutUplYN");

                ImageButton imgBankStatement = (ImageButton)e.Row.FindControl("imgBankStatement");
                ImageButton imgBsaOut = (ImageButton)e.Row.FindControl("imgBsaOut");


                if (hdnBsaOutUplYN.Value == "Y")
                {
                    imgBsaOut.Enabled = true;
                }
                else
                {
                    imgBsaOut.Enabled = false;
                }

                if (hdnBankStateUplYN.Value == "Y")
                {
                    imgBankStatement.Enabled = true;
                }
                else
                {
                    imgBankStatement.Enabled = false;
                }

            }
        }
        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            DataRow dr;
            if (GetData() == true)
            {
                DataTable dt = (DataTable)ViewState["Bank"];
                dt.AcceptChanges();
                dr = dt.NewRow();
                dt.Rows.Add(dr);

                ViewState["Bank"] = dt;
                gvBankDtl.DataSource = dt;
                gvBankDtl.DataBind();
                CalculateTotalCost();
            }
        }
        private Boolean GetData()
        {

            string vfilenameBank = "BankStatement", vfilenameBsu = "BSAOut", vBankExt, vBsaExt;
            int vMaxFileSize = Convert.ToInt32(MaxFileSize); byte[] vFuBankStatement, vFuBsaOut = null;

            DataTable dt = (DataTable)ViewState["Bank"];
            foreach (GridViewRow gr in gvBankDtl.Rows)
            {
                Label lblSrlNo = (Label)gvBankDtl.Rows[gr.RowIndex].FindControl("lblSrlNo");
                DropDownList ddlAcType = (DropDownList)gvBankDtl.Rows[gr.RowIndex].FindControl("ddlAcType");
                TextBox txtAcHoldNm = (TextBox)gvBankDtl.Rows[gr.RowIndex].FindControl("txtAcHoldNm");
                TextBox txtBankName = (TextBox)gvBankDtl.Rows[gr.RowIndex].FindControl("txtBankName");
                TextBox txtAccNo = (TextBox)gvBankDtl.Rows[gr.RowIndex].FindControl("txtAccNo");
                TextBox txtBTO = (TextBox)gvBankDtl.Rows[gr.RowIndex].FindControl("txtBTO");
                TextBox txtABB = (TextBox)gvBankDtl.Rows[gr.RowIndex].FindControl("txtABB");

                TextBox txtEcsNachReturns = (TextBox)gvBankDtl.Rows[gr.RowIndex].FindControl("txtEcsNachReturns");
                TextBox txtAvgMonthCredit = (TextBox)gvBankDtl.Rows[gr.RowIndex].FindControl("txtAvgMonthCredit");
                TextBox txtChequeReturns = (TextBox)gvBankDtl.Rows[gr.RowIndex].FindControl("txtChequeReturns");
                Label lblBankStatemenFN = (Label)gvBankDtl.Rows[gr.RowIndex].FindControl("lblBankStatemenFN");
                Label lblBsaOutFN = (Label)gvBankDtl.Rows[gr.RowIndex].FindControl("lblBsaOutFN");
                FileUpload fuBankStatement = (FileUpload)gr.FindControl("fuBankStatement");
                FileUpload fuBsaOut = (FileUpload)gr.FindControl("fuBsaOut");

                if (txtAcHoldNm.Text == "")
                {
                    gblFuction.AjxMsgPopup("AcHoldName is Blank...");
                    return false;
                }
                else if (ddlAcType.SelectedIndex <= 0)
                {
                    gblFuction.AjxMsgPopup("Please Select Account type");
                    return false;
                }
                else if (txtBankName.Text == "")
                {
                    gblFuction.AjxMsgPopup("Bank Name is Blank...");
                    return false;
                }

                else if (txtAccNo.Text == "")
                {
                    gblFuction.AjxMsgPopup("A/c No is Blank...");
                    return false;
                }
                else if (txtBTO.Text == "")
                {
                    gblFuction.AjxMsgPopup("BTO is Blank...");
                    return false;
                }
                else if (txtABB.Text == "")
                {
                    gblFuction.AjxMsgPopup("ABB(Rs.) is Blank...");
                    return false;
                }
                else if (txtAvgMonthCredit.Text == "")
                {
                    gblFuction.AjxMsgPopup("Avg. Monthly Credit is Blank...");
                    return false;
                }
                else if (txtEcsNachReturns.Text == "")
                {
                    gblFuction.AjxMsgPopup("ECS/NACH Returns is Blank...");
                    return false;
                }
                else if (txtChequeReturns.Text == "")
                {
                    gblFuction.AjxMsgPopup("Cheque Returns is Blank...");
                    return false;
                }

                if (fuBankStatement.HasFile == true)
                {
                    if (fuBankStatement.PostedFile.ContentLength > vMaxFileSize)
                    {
                        gblFuction.AjxMsgPopup("Maximum upload file Size exceed the limit.File Size Should Be Maximum " + Convert.ToString(FileSize));
                        return false;
                    }

                    vFuBankStatement = new byte[fuBankStatement.PostedFile.InputStream.Length + 1];
                    fuBankStatement.PostedFile.InputStream.Read(vFuBankStatement, 0, vFuBankStatement.Length);
                    vBankExt = System.IO.Path.GetExtension(fuBankStatement.FileName).ToLower();

                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vBankExt.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(fuBankStatement.FileBytes);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuBankStatement.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------

                    if (vBankExt.ToLower() == ".jpg" || vBankExt.ToLower() == ".png" || vBankExt.ToLower() == ".jpeg" || vBankExt.ToLower() == ".ico"
                        || vBankExt.ToLower() == ".pdf" || vBankExt.ToLower() == ".xlx" || vBankExt.ToLower() == ".xlsx")
                    {
                        if ((vBankExt.ToLower() != ".pdf") && (vBankExt.ToLower() != ".xlx") && (vBankExt.ToLower() != ".xlsx"))
                        {
                            vBankExt = ".png";
                        }
                        dt.Rows[gr.RowIndex]["ByteBankDoc"] = vFuBankStatement;
                        dt.Rows[gr.RowIndex]["BankSateUpYN"] = "Y";
                        dt.Rows[gr.RowIndex]["BankSatementFileName"] = vfilenameBank + vBankExt;
                        dt.Rows[gr.RowIndex]["BankSatementFileStoredPath"] = DocumentBucketURL;
                        dt.Rows[gr.RowIndex]["ByteBANKYN"] = "Y";
                        lblBankStatemenFN.Text = vfilenameBank + vBankExt;
                    }
                    else
                    {
                        gblFuction.MsgPopup("This File Extension is Not Allowed Upload...");
                        return false;
                    }
                }
                else
                {
                    if (dt.Rows[gr.RowIndex]["ByteBANKYN"].ToString() == "N")
                    {
                        if (dt.Rows[gr.RowIndex]["BankSateUpYN"].ToString() == "Y")
                        {
                            dt.Rows[gr.RowIndex]["BankSatementFileName"] = lblBankStatemenFN.Text;
                        }
                        else
                        {
                            dt.Rows[gr.RowIndex]["BankSateUpYN"] = "N";
                            dt.Rows[gr.RowIndex]["BankSatementFileName"] = "";
                            lblBankStatemenFN.Text = "";
                        }
                    }

                }
                if (fuBsaOut.HasFile == true)
                {
                    if (fuBsaOut.PostedFile.ContentLength > vMaxFileSize)
                    {
                        gblFuction.AjxMsgPopup("Maximum upload file Size exceed the limit.File Size Should Be Maximum " + Convert.ToString(FileSize));
                        return false;
                    }
                    vFuBsaOut = new byte[fuBsaOut.PostedFile.InputStream.Length + 1];
                    fuBsaOut.PostedFile.InputStream.Read(vFuBsaOut, 0, vFuBsaOut.Length);
                    vBsaExt = System.IO.Path.GetExtension(fuBsaOut.FileName).ToLower();

                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vBsaExt.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(fuBsaOut.FileBytes);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuBsaOut.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------

                    if (vBsaExt.ToLower() == ".jpg" || vBsaExt.ToLower() == ".png" || vBsaExt.ToLower() == ".jpeg" || vBsaExt.ToLower() == ".ico"
                        || vBsaExt.ToLower() == ".pdf" || vBsaExt.ToLower() == ".xlx" || vBsaExt.ToLower() == ".xlsx")
                    {
                        if ((vBsaExt.ToLower() != ".pdf") && (vBsaExt.ToLower() != ".xlx") && (vBsaExt.ToLower() != ".xlsx"))
                        {
                            vBsaExt = ".png";
                        }
                        dt.Rows[gr.RowIndex]["ByteBSADoc"] = vFuBsaOut;
                        dt.Rows[gr.RowIndex]["BsaOutUpYN"] = "Y";
                        dt.Rows[gr.RowIndex]["BsaOutFileName"] = vfilenameBsu + vBsaExt;
                        dt.Rows[gr.RowIndex]["BsaOutFileStoredPath"] = DocumentBucketURL;
                        dt.Rows[gr.RowIndex]["ByteBsaYN"] = "Y";
                        lblBsaOutFN.Text = vfilenameBsu + vBsaExt;
                    }
                    else
                    {
                        gblFuction.MsgPopup("This File Extension is Not Allowed to be Uploaded...");
                        return false;
                    }
                }
                else
                {
                    if (dt.Rows[gr.RowIndex]["ByteBsaYN"].ToString() == "N")
                    {
                        if (dt.Rows[gr.RowIndex]["BsaOutUpYN"].ToString() == "Y")
                        {
                            dt.Rows[gr.RowIndex]["BsaOutFileName"] = lblBsaOutFN.Text;
                        }
                        else
                        {
                            dt.Rows[gr.RowIndex]["BsaOutUpYN"] = "N";
                            dt.Rows[gr.RowIndex]["BsaOutFileName"] = "";
                            dt.Rows[gr.RowIndex]["BsaOutFileStoredPath"] = "";
                            lblBsaOutFN.Text = "";
                        }
                    }

                }



                //Inserting data into datatable dt
                dt.Rows[gr.RowIndex]["AcType"] = ddlAcType.SelectedValue;
                dt.Rows[gr.RowIndex]["BankSlId"] = Convert.ToInt32(lblSrlNo.Text == "" ? "" : lblSrlNo.Text);
                dt.Rows[gr.RowIndex]["AcHoldName"] = Convert.ToString(txtAcHoldNm.Text == "" ? "" : txtAcHoldNm.Text);
                dt.Rows[gr.RowIndex]["BankName"] = Convert.ToString(txtBankName.Text == "" ? "" : txtBankName.Text);
                dt.Rows[gr.RowIndex]["AccNo"] = Convert.ToString(txtAccNo.Text == "" ? "" : txtAccNo.Text);
                dt.Rows[gr.RowIndex]["BTO"] = Convert.ToDouble(txtBTO.Text == "" ? "0" : txtBTO.Text);
                dt.Rows[gr.RowIndex]["ABB"] = Convert.ToDouble(txtABB.Text == "" ? "0" : txtABB.Text);
                dt.Rows[gr.RowIndex]["AvgMonthCredit"] = Convert.ToDouble(txtAvgMonthCredit.Text == "" ? "0" : txtAvgMonthCredit.Text);
                dt.Rows[gr.RowIndex]["EcsNachReturnCharges"] = Convert.ToDouble(txtEcsNachReturns.Text == "" ? "0" : txtEcsNachReturns.Text);
                dt.Rows[gr.RowIndex]["CheckReturnCharges"] = Convert.ToDouble(txtChequeReturns.Text == "" ? "0" : txtChequeReturns.Text);


            }
            dt.AcceptChanges();
            ViewState["Bank"] = dt;
            gvBankDtl.DataSource = dt;
            gvBankDtl.DataBind();
            return true;
        }
        #region imgDoc_Click
        protected void imgDoc_Click(object sender, EventArgs e)
        {

            GridViewRow gvRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            Label lblSrlNo = (Label)gvRow.FindControl("lblSrlNo");
            Label lbFile = (Label)gvRow.FindControl("lblBankStatemenFN");

            ViewImgDoc(hdLeadId.Value, lblSrlNo.Text, lbFile.Text);

        }
        #endregion
        #region imgDocB_Click
        protected void imgDocB_Click(object sender, EventArgs e)
        {

            GridViewRow gvRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            Label lblSrlNo = (Label)gvRow.FindControl("lblSrlNo");
            Label lbFile = (Label)gvRow.FindControl("lblBsaOutFN");

            ViewImgDoc(hdLeadId.Value, lblSrlNo.Text, lbFile.Text);
        }
        #endregion
        protected void ViewImgDoc(string ID, string lblSrlNo, string FileName)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + ID + "_" + lblSrlNo + "_" + FileName;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image(lblSrlNo + "_" + FileName, ID);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + ID + "_" + lblSrlNo + "_" + FileName);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }
        public string GetBase64Image(string pImageName, string pLeadId)
        {
            string ActNetImage = "", base64image = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = DocumentBucketURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + pLeadId + "_" + pImageName;
                    if (ValidUrlChk(ActNetImage))
                    {
                        imgByte = DownloadByteImage(ActNetImage);
                        base64image = Convert.ToBase64String(imgByte);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return base64image;
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
        #region URLToByte
        public byte[] DownloadByteImage(string URL)
        {
            byte[] imgByte = null;
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var wc = new WebClient())
                {
                    imgByte = wc.DownloadData(URL);
                }
            }
            finally { }
            return imgByte;
        }
        #endregion
        private void ClearControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Text = "";
                }
                if (c is Label)
                {
                    ((Label)c).Text = "";
                }
                else if (c is DropDownList)
                {
                    ((DropDownList)c).SelectedIndex = -1;
                }

                // Recursively disable controls inside the containers like
                // panels or group controls
                if (c.Controls.Count > 0)
                {
                    ClearControls(c);

                }
            }

        }
        #region ConvertFileToByteArray
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
        #endregion
        private string SaveMemberImages(FileUpload flup, string imageGroup, string imageName, string ImgExt, string isImageSaved)
        {
            CApiCalling oAC = new CApiCalling();
            byte[] ImgByte = ConvertFileToByteArray(flup.PostedFile);
            isImageSaved = oAC.UploadFileMinio(ImgByte, imageName + ImgExt, imageGroup, DocumentBucket, MinioUrl);
            return isImageSaved;
        }

    }
}