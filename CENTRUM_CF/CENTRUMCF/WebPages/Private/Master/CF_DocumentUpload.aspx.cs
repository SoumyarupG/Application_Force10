using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CENTRUMBA;
using CENTRUMCA;
using System.IO;
using System.Drawing;
using System.Configuration;
using System.Net;


namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class CF_DocumentUpload : CENTRUMBAse
    {
        protected int cPgNo = 1;
        string vPathImage = "", vPathNetworkDrive1 = "", vPathNetworkDrive2 = "";
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
               
                btnShow.Enabled = false;
                if (Session[gblValue.LeadID] != null)
                {
                    GetDocuments();
                    Int64 LeadId = Convert.ToInt64(Session[gblValue.LeadID]);
                    CheckOprtnStatus(Convert.ToInt64(LeadId));
                }
                else
                {
                    StatusButton("Close");
                    gblFuction.MsgPopup("Please Select Lead from Loan Application Status screen and proceed...");
                    return;
                }
                tbQly.ActiveTabIndex = 0;
                txtLoanApplicationNo.Enabled = false;
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Document Upload";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFDocUpload);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Document Upload", false);
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
        protected void gvEmpDoc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int i = e.Row.RowIndex;
                DataTable dtIns = null;
                CApplication oApp = null;
                try
                {
                    oApp = new CApplication();
                    FileUpload FUKYcDoc = (FileUpload)e.Row.FindControl("FUKYcDoc");
                    DropDownList ddlDocType = (DropDownList)e.Row.FindControl("ddlDocType");
                    TextBox txtDocRemarks = (TextBox)e.Row.FindControl("txtDocRemarks");
                    TextBox txtDocPassword = (TextBox)e.Row.FindControl("txtDocPassword");
                    HiddenField hdnDocPassword = (HiddenField)e.Row.FindControl("hdnDocPassword");

                    ddlDocType.Items.Clear();
                    dtIns = oApp.GetDocumentMst("DOC");
                    if (dtIns.Rows.Count > 0)
                    {
                        ddlDocType.DataSource = dtIns;
                        ddlDocType.DataTextField = "DocName";
                        ddlDocType.DataValueField = "DocID";
                        ddlDocType.DataBind();
                    }
                    ListItem oLk = new ListItem("<--Select-->", "-1");
                    ddlDocType.Items.Insert(0, oLk);

                  
                    ddlDocType.SelectedIndex = ddlDocType.Items.IndexOf(ddlDocType.Items.FindByValue(e.Row.Cells[9].Text.Trim()));

                    txtLoanApplicationNo.Enabled = false;
                    if (ddlDocType.SelectedIndex > 0)
                    {
                        ddlDocType.Enabled = false;
                    }
                    else
                    {
                        ddlDocType.Enabled = true;
                    }

                    txtDocPassword.TextMode = TextBoxMode.Password;
                    txtDocPassword.Attributes.Add("value", hdnDocPassword.Value);

                }
                finally
                {

                }
            }
           
        }
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            int curRow = 0, maxRow = 0;
            ImageButton ImgAdd = (ImageButton)sender;
            GridViewRow gr = (GridViewRow)ImgAdd.NamingContainer;
            curRow = gr.RowIndex;
            maxRow = gvEmpDoc.Rows.Count;
            if (GetDoc() == true)
            {
                DataTable dt = (DataTable)ViewState["LoanAppDoc"];
                if (curRow == maxRow - 1)
                {
                    NewEmpDoc(gvEmpDoc.Rows.Count);
                    gr.Enabled = false;
                }
                foreach (GridViewRow gv in gvEmpDoc.Rows)
                {
                    DropDownList ddlDocType = (DropDownList)gvEmpDoc.Rows[gv.RowIndex].FindControl("ddlDocType");

                    if (ddlDocType.SelectedIndex > 0)
                    {
                        ddlDocType.Enabled = false;
                    }
                    else
                    {
                        ddlDocType.Enabled = true;
                    }
                }
            }
        }
        protected void ImDel_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton ImDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)ImDel.NamingContainer;
            GridView gv = (GridView)ImDel.Parent.Parent.NamingContainer;
            //        //GridViewRow PrevGr = gv.Rows[gR.RowIndex - 1];

            dt = (DataTable)ViewState["LoanAppDoc"];
            if (dt.Rows.Count > 1)
            {
                dt.Rows[gR.RowIndex].Delete();
                dt.AcceptChanges();
                ViewState["KYCDoc"] = dt;
                gvEmpDoc.DataSource = dt;
                gvEmpDoc.DataBind();
                SetData();
                //PrevGr.Enabled = true;
                //TotalPaid();
            }
            else if (dt.Rows.Count == 1)
            {
                gblFuction.MsgPopup("First Row can not be deleted.");
                return;
            }
            foreach (GridViewRow gr in gvEmpDoc.Rows)
            {

                DropDownList ddlDocType = (DropDownList)gvEmpDoc.Rows[gr.RowIndex].FindControl("ddlDocType");

                if (ddlDocType.SelectedIndex > 0)
                {
                    ddlDocType.Enabled = false;
                }
                else
                {
                    ddlDocType.Enabled = true;
                }

            }
        }
        protected void btnKYCDoc_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lbFile = (Label)gvrow.FindControl("lbFile");

            string vBase64String = "";
            string ActNetImage = "", vPdfFile = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');

            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLoanAppNo.Value + "_" + lbFile.Text;
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
                    vBase64String = GetBase64Image(lbFile.Text, hdnLoanAppNo.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUp();", true);
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
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLoanAppNo.Value + "_" + lbFile.Text);
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
        private Boolean GetDoc()
        {
            DataTable dt = null;
            byte[] vFUKYcDoc = null;
            byte[] data = null;
            string vFUKYcDocType = "", vfilename = "";
            dt = (DataTable)ViewState["LoanAppDoc"];
            Int32 vMaxFileSize = 0;
            vMaxFileSize = Convert.ToInt32(MaxFileSize);
            foreach (GridViewRow gr in gvEmpDoc.Rows)
            {
                dt.Rows[gr.RowIndex]["SLNo"] = gr.RowIndex + 1;
                DropDownList ddlDocType = (DropDownList)gr.FindControl("ddlDocType");
                TextBox txtDocRemarks = (TextBox)gr.FindControl("txtDocRemarks");
                TextBox txtDocPassword = (TextBox)gr.FindControl("txtDocPassword");
                FileUpload FUKYcDoc = (FileUpload)gr.FindControl("FUKYcDoc");
                Label lbFile = (Label)gr.FindControl("lbFile");
                HiddenField hdnDocImage = (HiddenField)gr.FindControl("hdnDocImage");

                if (gr.RowIndex <= gvEmpDoc.Rows.Count - 1)
                {
                    if (ddlDocType.SelectedIndex <= 0)
                    {
                        gblFuction.MsgPopup("Please Add Document type");
                        return false;
                    }

                    if (FUKYcDoc.HasFile == true)
                    {


                        if (FUKYcDoc.PostedFile.ContentLength > vMaxFileSize)
                        {
                            gblFuction.AjxMsgPopup("Maximum upload file Size exceed the limit.File Size Should Be Maximum " + Convert.ToString(FileSize));
                            return false;
                        }
                        vFUKYcDoc = new byte[FUKYcDoc.PostedFile.InputStream.Length + 1];
                        FUKYcDoc.PostedFile.InputStream.Read(vFUKYcDoc, 0, vFUKYcDoc.Length);
                        vFUKYcDocType = System.IO.Path.GetExtension(FUKYcDoc.FileName).ToLower();
                        vfilename = System.IO.Path.GetFileName(FUKYcDoc.FileName).ToUpper();

                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vFUKYcDocType.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(FUKYcDoc.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                FUKYcDoc.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------

                        if (vFUKYcDocType.ToLower() == ".jpg" || vFUKYcDocType.ToLower() == ".png" || vFUKYcDocType.ToLower() == ".jpeg" || vFUKYcDocType.ToLower() == ".ico")
                        {
                            Stream fileData = new MemoryStream(vFUKYcDoc);
                            if (IsValid)
                            {
                                Bitmap image = new Bitmap(fileData);

                                using (System.IO.MemoryStream sampleStream = new System.IO.MemoryStream())
                                {
                                    image.Save(sampleStream, image.RawFormat);
                                    data = sampleStream.ToArray();
                                }
                                dt.Rows[gr.RowIndex]["Doc_Image"] = data;

                                dt.Rows[gr.RowIndex]["ImageBase64"] = Convert.ToBase64String(data);
                                hdnDocImage.Value = Convert.ToBase64String(data);
                            }
                        }
                        else if (vFUKYcDocType.ToLower() == ".pdf" || vFUKYcDocType.ToLower() == ".xlx" || vFUKYcDocType.ToLower() == ".xlsx")
                        {
                            dt.Rows[gr.RowIndex]["Doc_Image"] = vFUKYcDoc;
                            hdnDocImage.Value = Convert.ToBase64String(vFUKYcDoc);
                            dt.Rows[gr.RowIndex]["ImageBase64"] = Convert.ToBase64String(vFUKYcDoc);
                        }
                        else
                        {
                            gblFuction.MsgPopup("This File Extension is Not Allowed to be Uploaded...");
                            return false;
                        }

                        if (ddlDocType.SelectedIndex <= 0)
                        {
                            gblFuction.MsgPopup("Please Add Document type");
                            return false;
                        }
                        else
                        {
                            dt.Rows[gr.RowIndex]["DocTypeId"] = Convert.ToInt32(ddlDocType.SelectedValue);
                            string vDocType = ddlDocType.SelectedItem.Text;
                            vDocType = vDocType.Replace(" ", "");
                            if (vFUKYcDocType.ToLower() != ".pdf")
                            {
                                dt.Rows[gr.RowIndex]["DocType"] = vDocType + ".png";
                            }
                            else
                            {
                                dt.Rows[gr.RowIndex]["DocType"] = vDocType + vFUKYcDocType;
                            }
                            dt.Rows[gr.RowIndex]["DocRemarks"] = txtDocRemarks.Text.Trim();
                            dt.Rows[gr.RowIndex]["DocPassword"] = txtDocPassword.Text.Trim();
                            dt.Rows[gr.RowIndex]["UploadYN"] = "N";
                        }
                    }
                    else
                    {
                        if (lbFile.Text == "")
                        {
                            gblFuction.AjxMsgPopup("Please Select a File to Upload...");
                            return false;
                        }
                    }
                    if (ddlDocType.Enabled == false)
                    {
                        dt.Rows[gr.RowIndex]["DocRemarks"] = txtDocRemarks.Text.Trim();
                        dt.Rows[gr.RowIndex]["DocPassword"] = txtDocPassword.Text.Trim();
                    }
                }
            }
            dt.AcceptChanges();
            ViewState["LoanAppDoc"] = dt;
            gvEmpDoc.DataSource = dt;
            gvEmpDoc.DataBind();
            return true;
        }
        private void NewEmpDoc(Int32 vRow)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["LoanAppDoc"];
                DataRow dr;
                dr = dt.NewRow();
                dt.Rows.Add();
                if (dt.Rows.Count > 0)
                    dt.Rows[vRow]["SLNo"] = vRow + 1;
                else
                    dt.Rows[vRow]["SLNo"] = 0;
                dt.Rows[vRow]["DocTypeId"] = -1;
                dt.Rows[vRow]["DocType"] = null;
                dt.Rows[vRow]["Doc_Image"] = null;
                dt.Rows[vRow]["ImageBase64"] = null;
                dt.Rows[vRow]["DocRemarks"] = null;
                dt.Rows[vRow]["DocPassword"] = null;
                dt.Rows[vRow]["UploadYN"] = null;
                dt.AcceptChanges();
                ViewState["LoanAppDoc"] = dt;
                gvEmpDoc.DataSource = dt;
                gvEmpDoc.DataBind();
                SetData();
            }
            finally
            {
            }
        }
        private void SetData()
        {
            DataTable dt = null;
            int i = 0;
            dt = (DataTable)ViewState["LoanAppDoc"];
            foreach (DataRow gr in dt.Rows)
            {
                DropDownList ddlDocType = (DropDownList)gvEmpDoc.Rows[i].FindControl("ddlDocType");
                HiddenField hdnDocPassword = (HiddenField)gvEmpDoc.Rows[i].FindControl("hdnDocPassword");
                TextBox txtDocPassword = (TextBox)gvEmpDoc.Rows[i].FindControl("txtDocPassword");
                txtDocPassword.TextMode = TextBoxMode.Password;
                txtDocPassword.Attributes.Add("value", hdnDocPassword.Value);

                ddlDocType.SelectedIndex = ddlDocType.Items.IndexOf(ddlDocType.Items.FindByValue(Convert.ToString(dt.Rows[i]["DocTypeId"])));

                i++;
            }
        }     
        private void ShowEmpRecords()
        {
            string vLoanAppId = "";
            DataTable dt = null;
            CApplication oMem = null;
            try
            {
                vLoanAppId = hdnLoanAppNo.Value;
                oMem = new CApplication();
                dt = oMem.CF_GetDocByLeadId(Convert.ToInt64(vLoanAppId));
                if (dt.Rows.Count > 0)
                {
                    gvEmpDoc.DataSource = dt;
                    gvEmpDoc.DataBind();
                    ViewState["LoanAppDoc"] = dt;



                }
                else
                {
                    LoadEmpDoc();
                }
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        private void LoadEmpDoc()
        {
            ViewState["KYCDoc"] = null;
            DataTable dt = new DataTable("Table1");
            dt.Columns.Add("SLNo");
            dt.Columns.Add("DocTypeId");
            dt.Columns.Add("DocType");
            dt.Columns.Add("DocRemarks");
            dt.Columns.Add("DocPassword");
            dt.Columns.Add("Doc_Image", typeof(byte[]));
            dt.Columns.Add("ImageBase64");
            dt.Columns.Add("UploadYN");
            dt.AcceptChanges();
            ViewState["LoanAppDoc"] = dt;
            NewEmpDoc(0);
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
                    gvEmpDoc.Enabled = false;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                   // btnExit.Enabled = false;
                    gvEmpDoc.Enabled = true;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    gvEmpDoc.Enabled = false;
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
           
        }
        private void ClearControls()
        {

            lblApplNm.Text = "";
            txtLoanApplicationNo.Text = "";
            hdnLoanAppNo.Value = "";
            lblDate.Text = "";
            lblUser.Text = "";
            gvEmpDoc.DataSource = null;
            gvEmpDoc.DataBind();
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
            ViewState["DepartmentId"] = null;
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
                tbQly.ActiveTabIndex = 0;
                StatusButton("Add");
                ClearControls();
                btnShow.Enabled = true;
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
            GetDocuments();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["pathImage"];
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null || vStateEdit == "")
                vStateEdit = "Save";
            DataTable dtEmp = (DataTable)ViewState["LoanAppDoc"];
            if (SaveRecords(vStateEdit) == true)
            {
                for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                {
                    if (dtEmp.Rows[i]["UploadYN"].ToString() == "N")
                    {
                        byte[] imgArray = (byte[])(dtEmp.Rows[i]["Doc_Image"]);
                        string vDocName = dtEmp.Rows[i]["DocType"].ToString();
                        if (vDocName.ToLower().Contains(".pdf"))
                        {
                            vDocName = hdnLoanAppNo.Value.ToString() + "_" + vDocName;
                        }
                        if (MinioYN == "N")
                        {
                            string folderPath = string.Format("{0}/{1}", vPathImage, hdnLoanAppNo.Value);
                            System.IO.Directory.CreateDirectory(folderPath);
                            string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                            File.WriteAllBytes(filePath, imgArray);
                        }
                        else
                        {
                            CApiCalling oAC = new CApiCalling();
                            oAC.UploadFileMinio(imgArray, vDocName, hdnLoanAppNo.Value, DocumentBucket, MinioUrl);
                        }
                    }
                }
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                GetDocuments();
                ViewState["StateEdit"] = null;
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            string vXmlData;
            CMember oMem = null;
            int vErr = 0;
            Boolean vResult = false;
            DataTable dt1 = null;

            if (GetDoc() == true)
            {
                dt1 = (DataTable)ViewState["LoanAppDoc"];
                using (StringWriter oSW = new StringWriter())
                {
                    dt1.WriteXml(oSW);
                    vXmlData = oSW.ToString();
                }

                oMem = new CMember();
                vErr = oMem.SaveDocument(hdnLoanAppNo.Value, gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vXmlData, Mode, this.UserID);
                if (vErr == 0)
                {
                    if (Mode == "Save")
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    else if (Mode == "Edit")
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
        protected void GetDocuments()
        {
            string vBrCode = "";
            DataTable dt;
            DataSet ds = new DataSet();
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
                    txtLoanApplicationNo.Text = Convert.ToString(Session[gblValue.BCPNO]);
                }
                if (Session[gblValue.LeadID] != null)
                {
                    hdnLoanAppNo.Value = Convert.ToString(Session[gblValue.LeadID]);
                    hdLeadID.Value = Convert.ToString(Session[gblValue.LeadID]);

                }

                CDistrict oDist = new CDistrict();
                dt = oDist.CF_GetUpldDocDtlByLeadID(Convert.ToInt64(hdnLoanAppNo.Value));
                if (dt.Rows.Count > 0)
                {

                    btnEdit.Enabled = true;
                    btnAdd.Enabled = false;
                    hdLeadID.Value = Convert.ToString(dt.Rows[0]["LeadID"]);
                    txtLoanApplicationNo.Text = Convert.ToString(dt.Rows[0]["BCPropNo"]);
                    lblApplNm.Text = Convert.ToString(dt.Rows[0]["AppName"]);
                    txtLoanApplicationNo.Enabled = false;
                    hdnLoanAppNo.Value = Convert.ToString(dt.Rows[0]["LeadID"]);
                    ShowEmpRecords();
                }
                else
                { 
                    if (Session[gblValue.BCPNO] != null)
                    {
                        txtLoanApplicationNo.Text = Convert.ToString(Session[gblValue.BCPNO]);
                    }
                    ShowEmpRecords();
                    btnEdit.Enabled = true;
                    btnAdd.Enabled = false;
                }

                tbQly.ActiveTabIndex = 0;
                ViewState["StateEdit"] = "Edit";
                StatusButton("Edit");

            }
            finally
            {
                dt = null;
            }
        }
        
    }
}