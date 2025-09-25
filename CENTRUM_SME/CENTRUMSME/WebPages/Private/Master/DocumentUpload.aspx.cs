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


namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class DocumentUpload : CENTRUMBAse
    {
        string vPathImage = "", vPathNetworkDrive1 = "", vPathNetworkDrive2 = "";
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string DocumentBucket = ConfigurationManager.AppSettings["DocumentBucket"];
        string AllDownloadPath = ConfigurationManager.AppSettings["AllDownloadPath"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
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
                this.PageHeading = "Document Upload";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDocUpload);
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

        protected void gvEmpDoc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int i = e.Row.RowIndex;
                try
                {
                    FileUpload FUKYcDoc = (FileUpload)e.Row.FindControl("FUKYcDoc");
                    DropDownList ddlDocType = (DropDownList)e.Row.FindControl("ddlDocType");
                    if (e.Row.Cells[8].Text == "Y" || e.Row.Cells[8].Text == "N")
                    {
                        FUKYcDoc.Enabled = false;
                        ddlDocType.Enabled = false;
                    }
                    ddlDocType.SelectedIndex = ddlDocType.Items.IndexOf(ddlDocType.Items.FindByValue(e.Row.Cells[7].Text.Trim()));
                }
                finally
                {

                }
            }

        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            //            //DataTable dt = GetData();
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
        }

        protected void btnKYCDoc_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["pathImage"];
            vPathNetworkDrive1 = ConfigurationManager.AppSettings["PathNetworkDrive1"];
            vPathNetworkDrive2 = ConfigurationManager.AppSettings["PathNetworkDrive2"];
            Button btn = sender as Button;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lbFile = (Label)gvrow.FindControl("lbFile");
            if (lbFile.Text != "")
            {
                string folderPath = string.Format("{0}/{1}", vPathImage, hdnLoanAppNo.Value);
                string filePath = string.Format("{0}/{1}", folderPath, lbFile.Text);
                if (File.Exists(filePath))
                {
                    Response.ContentType = "image/jpg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + lbFile.Text + "\"");
                    Response.TransmitFile(filePath);
                    Response.End();
                }
                else
                {
                    WebClient cln = null;
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    string vImage = hdnLoanAppNo.Value + "/" + lbFile.Text, vImgPath = "";
                    string[] ActNetPath = AllDownloadPath.Split(',');
                    for (int i = 0; i <= ActNetPath.Length - 1; i++)
                    {
                        if (ValidUrlChk(ActNetPath[i] + vImage))
                        {
                            vImgPath = ActNetPath[i] + vImage;
                            break;
                        }
                        else if (ValidUrlChk(ActNetPath[i] + vImage.Replace("/", "_")))
                        {
                            vImgPath = ActNetPath[i] + vImage.Replace("/", "_");
                            break;
                        }
                    }

                    if (vImgPath != "")
                    {
                        cln = new WebClient();
                        vDoc = cln.DownloadData(vImgPath);
                        Response.AddHeader("Content-Type", "image/jpg");
                        Response.AddHeader("Content-Disposition", "attachment;   filename=" + lbFile.Text);
                        Response.BinaryWrite(vDoc);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("File not Exists");
                    }
                }
            }
        }

        private Boolean GetDoc()
        {
            DataTable dt = null;
            byte[] vFUKYcDoc = null;
            byte[] data = null;
            string vFUKYcDocType = "", vfilename = "";
            dt = (DataTable)ViewState["LoanAppDoc"];
            foreach (GridViewRow gr in gvEmpDoc.Rows)
            {
                dt.Rows[gr.RowIndex]["SLNo"] = gr.RowIndex + 1;
                DropDownList ddlDocType = (DropDownList)gr.FindControl("ddlDocType");
                FileUpload FUKYcDoc = (FileUpload)gr.FindControl("FUKYcDoc");
                Label lbFile = (Label)gr.FindControl("lbFile");

                if (gr.RowIndex <= gvEmpDoc.Rows.Count - 1)
                {
                    if (ddlDocType.SelectedIndex <= 0)
                    {
                        gblFuction.MsgPopup("Please Add Document type");
                        return false;
                    }
                    if (FUKYcDoc.HasFile == true)
                    {

                        if (FUKYcDoc.PostedFile.InputStream.Length > 2000000)
                        {
                            gblFuction.MsgPopup("Maximum upload file Size exceed the limit.");
                            return false;
                        }
                        vFUKYcDoc = new byte[FUKYcDoc.PostedFile.InputStream.Length + 1];
                        FUKYcDoc.PostedFile.InputStream.Read(vFUKYcDoc, 0, vFUKYcDoc.Length);
                        vFUKYcDocType = System.IO.Path.GetExtension(FUKYcDoc.FileName).ToLower();
                        vfilename = System.IO.Path.GetFileName(FUKYcDoc.FileName).ToUpper();
                        if (vFUKYcDocType == ".jpg" || vFUKYcDocType == ".png" || vFUKYcDocType == ".jpeg" || vFUKYcDocType == ".ico"
                            || vFUKYcDocType == ".JPG" || vFUKYcDocType == ".PNG" || vFUKYcDocType == ".JPEG")
                        {
                            Stream fileData = new MemoryStream(vFUKYcDoc);
                            if (IsValid)
                            {
                                // Bitmap image = Resize_Image(fileData, 400, 300);
                                Bitmap image = new Bitmap(fileData);

                                using (System.IO.MemoryStream sampleStream = new System.IO.MemoryStream())
                                {
                                    image.Save(sampleStream, image.RawFormat);
                                    data = sampleStream.ToArray();
                                }
                                dt.Rows[gr.RowIndex]["Doc_Image"] = data;
                            }
                        }
                        else
                        {
                            dt.Rows[gr.RowIndex]["Doc_Image"] = vFUKYcDoc;
                        }

                        if (ddlDocType.SelectedIndex <= 0)
                        {
                            gblFuction.MsgPopup("Please Add Document type");
                            return false;
                        }
                        else
                        {
                            dt.Rows[gr.RowIndex]["DocTypeId"] = Convert.ToInt32(ddlDocType.SelectedValue);
                            dt.Rows[gr.RowIndex]["DocType"] = ddlDocType.SelectedItem.Text + ".png";
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
                }
            }
            dt.AcceptChanges();
            ViewState["LoanAppDoc"] = dt;
            gvEmpDoc.DataSource = dt;
            gvEmpDoc.DataBind();
            return true;
        }

        //private Bitmap Resize_Image(Stream streamImage, int maxWidth, int maxHeight)
        //{
        //    Bitmap originalImage = new Bitmap(streamImage);
        //    int newWidth = originalImage.Width;
        //    int newHeight = originalImage.Height;
        //    double aspectRatio = (double)originalImage.Width / (double)originalImage.Height;

        //    if (aspectRatio <= 1 && originalImage.Width > maxWidth)
        //    {
        //        newWidth = maxWidth;
        //        newHeight = (int)Math.Round(newWidth / aspectRatio);
        //    }
        //    else if (aspectRatio > 1 && originalImage.Height > maxHeight)
        //    {
        //        newHeight = maxHeight;
        //        newWidth = (int)Math.Round(newHeight * aspectRatio);
        //    }

        //    Bitmap newImage = new Bitmap(originalImage, newWidth, newHeight);

        //    Graphics g = Graphics.FromImage(newImage);
        //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
        //    g.DrawImage(originalImage, 0, 0, newImage.Width, newImage.Height);

        //    originalImage.Dispose();

        //    return newImage;
        //}

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
                ddlDocType.SelectedIndex = ddlDocType.Items.IndexOf(ddlDocType.Items.FindByValue(Convert.ToString(dt.Rows[i]["DocTypeId"])));
                i++;
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            ShowEmpRecords();
        }

        private void ShowEmpRecords()
        {
            string vLoanAppId = "";

            DataTable dt = null;
            CMember oMem = null;
            try
            {
                vLoanAppId = hdnLoanAppNo.Value;
                oMem = new CMember();
                dt = oMem.GetDocByLoanAppId(vLoanAppId);
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
            dt.Columns.Add("Doc_Image", typeof(byte[]));
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

        private void EnableControl(Boolean Status)
        {
            txtLoanApplicationNo.Enabled = Status;
        }

        private void ClearControls()
        {
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
                tbQly.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
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
                //if (SaveRecords("Delete") == true)
                //{
                //    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                //    LoadGrid(0);
                //    ClearControls();
                //    tbQly.ActiveTabIndex = 0;
                //    StatusButton("Delete");
                //}
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
                //LoadGrid(0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbQly.ActiveTabIndex = 0;
            EnableControl(false);
            ClearControls();
            StatusButton("View");
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
                for (int i = 0; i < dtEmp.Rows.Count - 1; i++)
                {
                    if (dtEmp.Rows[i]["UploadYN"].ToString() == "N")
                    {
                        byte[] imgArray = (byte[])(dtEmp.Rows[i]["Doc_Image"]);
                        string vDocName = dtEmp.Rows[i]["DocType"].ToString();
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
                StatusButton("View");
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
            dt1 = (DataTable)ViewState["LoanAppDoc"];
            using (StringWriter oSW = new StringWriter())
            {
                dt1.WriteXml(oSW);
                vXmlData = oSW.ToString();
            }
            //if (Mode == "Save")
            //{
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
            //}
            return vResult;
        }

        public bool ValidUrlChk(string pUrl)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pUrl);
                request.Timeout = 15000;
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
    }
}