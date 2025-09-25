using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.Data;
using System.IO;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Reporting.WebForms;
using System.Net;
using System.Web.Services;
using Newtonsoft.Json;
using System.Text;
using System.Net.Sockets;
using CENTRUMCF.WebServices;


namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class CF_EPCMaster : CENTRUMBAse
    {
        protected int vPgNo = 1;
        string vPathImage = "";
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string DocumentBucket = ConfigurationManager.AppSettings["CFDocumentBucket"];
        string AllDownloadPath = ConfigurationManager.AppSettings["AllDownloadPath"];
        string DocumentBucketURL = ConfigurationManager.AppSettings["DocumentBucketURL"];
        string MaxFileSize = ConfigurationManager.AppSettings["MaxFileSize"];
        string MobUrl = ConfigurationManager.AppSettings["MobUrl"];

        protected void Page_Load(object sender, EventArgs e)
        {
            ceAppDt.EndDate = DateTime.Now;
            cePrtnrAge.EndDate = DateTime.Now;
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                hdnMaxFileSize.Value = MaxFileSize;
                tbEPCMaster.ActiveTabIndex = 0;
                popState();
                StatusButton("View");
                btnAddPrtnr.Enabled = false;

            }

        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "EPC Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFEPCMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "EPC Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true, this.Page);
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnAddPrtnr.Enabled = false;
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnAddPrtnr.Enabled = false;
                    EnableControl(false, this.Page);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnAddPrtnr.Enabled = false;
                    EnableControl(true, this.Page);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnAddPrtnr.Enabled = false;
                    EnableControl(false, this.Page);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnAddPrtnr.Enabled = false;
                    EnableControl(false, this.Page);
                    break;
                case "Exit":
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    btnAddPrtnr.Enabled = false;
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
                if (c is FileUpload)
                {
                    ((FileUpload)c).Enabled = Status;
                }
                if (c is CheckBox)
                {
                    ((CheckBox)c).Enabled = Status;
                }
                else if (c is DropDownList)
                {
                    ((DropDownList)c).Enabled = Status;
                }
                // Recursively disable controls inside the containers like
                // panels or group controls
                if (c.Controls.Count > 0)
                {
                    EnableControl(Status, c);
                }
                txtSearch.Enabled = true;
                btnShow.Enabled = true;
                ddlPriState.Enabled = false;
                ddlPrtState.Enabled = false;
                ddlPState.Enabled = false;
                ddlBGVState.Enabled = false;
                lbRseMsg.Enabled = Status;
                txtPriEPCCode.Enabled = false;
                txtRseMsg.Enabled = false;
                ddlPenyDrpStatus.Enabled = false;
            }
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
                tbEPCMaster.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls(this.Page);

                gvPrtnr.PageIndex = 0;
                gvPrtnr.DataSource = null;
                gvPrtnr.DataBind();
                btnAddPrtnr.Enabled = true;

                ViewState["Partner"] = null;


                txtACNo.Attributes.Add("value", "");
                txtReACNo.Attributes.Add("value", "");
                hdAcNo.Value = "";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ClearControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Text = "";
                }
                else if (c is DropDownList)
                {
                    ((DropDownList)c).SelectedIndex = -1;
                }
                if (c is Label)
                {
                    ((Label)c).Text = "";
                }

                // Recursively disable controls inside the containers like
                // panels or group controls
                if (c.Controls.Count > 0)
                {
                    ClearControls(c);

                }
            }

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbEPCMaster.ActiveTabIndex = 0;
            EnableControl(false, this.Page);
            StatusButton("View");
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
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
                if (ddlBuyBkScheme.SelectedValue == "N")
                {
                    ddlBuyBkRatio.Enabled = false;
                    ddlBuyBkRatio.SelectedValue = "-1";

                }
                else if (ddlBuyBkScheme.SelectedValue == "Y")
                {
                    ddlBuyBkRatio.Enabled = true;
                }
                btnAddPrtnr.Enabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadEPCList(0);
        }
        private void LoadEPCList(Int32 pPgIndx)
        {
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            Int32 vTotRows = 0;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                dt = oMem.CF_GetEPCList(vBrCode, txtSearch.Text.Trim(), pPgIndx, ref vTotRows);
                if (dt.Rows.Count > 0)
                {
                    gvEPC.DataSource = dt;
                    gvEPC.DataBind();
                }
                else
                {
                    gvEPC.DataSource = null;
                    gvEPC.DataBind();
                }
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
        private int CalTotPages(double pRows)
        {
            int vPgs = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vPgs;
        }
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
            LoadEPCList(vPgNo);
            tbEPCMaster.ActiveTabIndex = 0;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["pathImage"];
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {

                DataTable dt = (DataTable)ViewState["Partner"];

                DataTable dtUp = (DataTable)ViewState["Uploader"];

                #region dt
                if (dt != null)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        // PAN
                        if (dt.Rows[i]["BytePanYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dt.Rows[i]["BytePanDoc"]);

                            string vDocName = dt.Rows[i]["PrtnrPANFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }


                        // AADHAAR
                        if (dt.Rows[i]["ByteAadharYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dt.Rows[i]["ByteAadharDoc"]);

                            string vDocName = dt.Rows[i]["PrtnrAadharFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }

                        // CHouseNo
                        if (dt.Rows[i]["ByteCHouseNoYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dt.Rows[i]["ByteCHouseNoDoc"]);

                            string vDocName = dt.Rows[i]["PrtnrCHouseNoFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }

                        // CStreet
                        if (dt.Rows[i]["ByteCStreetYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dt.Rows[i]["ByteCStreetDoc"]);

                            string vDocName = dt.Rows[i]["PrtnrCStreetFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }

                        // PHouseNo
                        if (dt.Rows[i]["PrtnrPHouseNoYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dt.Rows[i]["PrtnrPHouseNoDoc"]);

                            string vDocName = dt.Rows[i]["PrtnrPHouseNoFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }


                        // PStreet
                        if (dt.Rows[i]["BytePStreetYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dt.Rows[i]["BytePStreetDoc"]);

                            string vDocName = dt.Rows[i]["PrtnrPStreetFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }
                    }
                }
                #endregion

                // GSTN

                #region dtUp

                if (dtUp != null)
                {
                    for (int i = 0; i <= dtUp.Rows.Count - 1; i++)
                    {
                        if (dtUp.Rows[i]["GSTNByteYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dtUp.Rows[i]["GSTNByteDoc"]);

                            string vDocName = dtUp.Rows[i]["GSTNFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }

                        // Trade
                        if (dtUp.Rows[i]["TradeByteYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dtUp.Rows[i]["TradeByteDoc"]);

                            string vDocName = dtUp.Rows[i]["TradeFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }

                        // Regn
                        if (dtUp.Rows[i]["RegnByteYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dtUp.Rows[i]["RegnByteDoc"]);

                            string vDocName = dtUp.Rows[i]["RegnFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }

                        // CPAN
                        if (dtUp.Rows[i]["CPANByteYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dtUp.Rows[i]["CPANByteDoc"]);

                            string vDocName = dtUp.Rows[i]["CPANFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }

                        // EntityType
                        if (dtUp.Rows[i]["ETypeByteYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dtUp.Rows[i]["ETypeByteDoc"]);

                            string vDocName = dtUp.Rows[i]["ETypeFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }

                        // DEED
                        if (dtUp.Rows[i]["DeedByteYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dtUp.Rows[i]["DeedByteDoc"]);

                            string vDocName = dtUp.Rows[i]["DeedFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }

                        // MOA
                        if (dtUp.Rows[i]["MOAByteYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dtUp.Rows[i]["MOAByteDoc"]);

                            string vDocName = dtUp.Rows[i]["MOAFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }

                        // AOA
                        if (dtUp.Rows[i]["AOAByteYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dtUp.Rows[i]["AOAByteDoc"]);

                            string vDocName = dtUp.Rows[i]["AOAFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }

                        // BankStatement
                        if (dtUp.Rows[i]["BnkSByteYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dtUp.Rows[i]["BnkSByteDoc"]);

                            string vDocName = dtUp.Rows[i]["BnkSFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }

                        // Cheque
                        if (dtUp.Rows[i]["CChqFByteYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dtUp.Rows[i]["CChqFByteDoc"]);

                            string vDocName = dtUp.Rows[i]["CChqFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }

                        // BNB
                        if (dtUp.Rows[i]["BNBByteYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dtUp.Rows[i]["BNBByteDoc"]);

                            string vDocName = dtUp.Rows[i]["BNBFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }

                        // BImage
                        if (dtUp.Rows[i]["BImgByteYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dtUp.Rows[i]["BImgByteDoc"]);

                            string vDocName = dtUp.Rows[i]["BImgFileNm"].ToString();

                            if (vDocName.ToLower().Contains(".pdf"))
                            {
                                vDocName = hdEPCId.Value.ToString() + "_" + vDocName;
                            }

                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdEPCId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vDocName, hdEPCId.Value, DocumentBucket, MinioUrl);
                            }
                        }
                    }
                }
                #endregion




                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);

                StatusButton("View");
                ViewState["StateEdit"] = null;
                ViewState["Partner"] = null;
                ViewState["Uploader"] = null;
                // ClearControls(this.Page);
                GetEPCDetails(Convert.ToInt64(hdEPCId.Value), "");
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false; DataTable dtP = null;
            Int32 vErr = 0, vNewId = 0; string vErrMsg = "";
            Int32 vMaxFileSize = 0;
            vMaxFileSize = Convert.ToInt32(MaxFileSize);
            DataTable dt = null;
            string vXmlPrtnr = "";
            Int32 vTotal = 0, vStake = 0;

            if (ViewState["Partner"] != null)
            {
                dtP = (DataTable)ViewState["Partner"];

                if (dtP != null)
                {
                    if (dtP.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtP.Rows.Count - 1; i++)
                        {
                            vStake = Convert.ToInt32(dtP.Rows[i]["PrtnrStateA"]);
                            vTotal += vStake;
                        }

                        if (vTotal != 100)
                        {
                            gblFuction.AjxMsgPopup("Total Stake Value must be equal to 100...");
                            return false;
                        }
                    }
                }

            }

            string vEpcName = "", vPriGSTNNo = "", vPriEPCCode = "", vPriRegnNo = "", vPriTradeLcnse = "", vPriEntityType = "", vPriCPAN = "",
            vPriPDeed = "", vPriMOA = "", vPriAOA = "", vPriCertNo = "", vPriEPCType = "", vBuyBkScheme = "", vEPCCategory = "", vPriDisbursalRatio = "",
            vPriHouseNo = "", vPriArea = "", vPriLandmark = "", vPriStreet = "", vPriVillage = "", vPriSubdistrict = "", vPriPostOfc = "", vPriLandline = "", vPriMobile = "",


            vACHolderNm = "", vBankNm = "", vBrCode = "",
            vACNo = "", vReACNo = "", vIFCE = "", vAccountType = "", vBankStatement = "", vCnclCq = "", vPenyDrpStatus = "", vRseMsg = "", vAuthority = "", vBGBName = "", vBGBBrdName = "", vBGBHrs = "", vBGBVintage = "",
            vBGBImage = "", vBGBStreet = "", vBGBArea = "", vBGBLocality = "", vBGBHouseNo = "", vBGBLandmark = "", vBGBVillage = "", vBGBSubDistrict = "", vBGBPostOffice = "",
             vBGBLandline = "", vBGBMobile = "",

            vPriGSTNFileName = "", vPriGSTNIsFileUpLd = "", vPriRegnNoFileName = "", vPriRegnNoIsFileUpLd = "",
            vPriTradeFileName = "", vPriTradeIsFileUpLd = "", vPriCPANFileName = "", vPriCPANIsFileUpLd = "", vPriDeedFileName = "",
            vPriDeedIsFileUpLd = "", vPriMOAFileName = "", vPriMOAIsFileUpLd = "",
            vPriAOAFileName = "", vPriAOAIsFileUpLd = "",



            vPriEntityIsFileUpLd = "",


            vBSIsFileUpLd = "", vCheqIsFileUpLd = "", vBBIsFileUpLd = "",
            vPriEntityFileNm = "",
            vIFSCFileNm = "", vBSFileNm = "", vCheqFileNm = "", vBBFileNm = "", vBGBFileNm = "", vBGBFileExt = "", vIFSCIsFileUpLd = "";

            string vPriGSTNFileExt = "", vPriTradeFileExt = "", vPriRegNoFileExt = "", vPriDeedFileExt = "", vPriCPANFileExt = "",
                   vPriMOAFileExt = "", vPriAOAFileExt = "",
                   vPriEntityFileExt = "",
                   vBSFileExt = "", vCheqFileExt = "", vBBFileExt = "", vBGBImageFileNm = "";

            string vGSTNByteYN = "N", vTradeByteYN = "N", vRegnByteYN = "N", vCPANByteYN = "N", vETypeByteYN = "N", vDeedByteYN = "N",
            vMOAByteYN = "N", vAOAByteYN = "N", vBnkSByteYN = "N", vCChqFByteYN = "N", vBNBByteYN = "N", vBImgByteYN = "N";

            byte[] vFuGSTN = null, vFuTrade = null, vFuRegn = null, vFuCPAN = null, vFuEType = null, vFuDeed = null,
            vFuMOA = null, vFuAOA = null, vFuBnkS = null, vFuCChq = null, vFuBNB = null, vFuBImg = null;

            Int32 vPriDist = 0, vPriState = 0;
            Int32 vBuyBkRatio = 0, vPriPin = 0, vBGBPincode = 0,
            vCreatedBy = 0, vBGBEmpNo = 0, vPrtnrPState = 0, vBGBDist = 0, vBGBState = 0;

            DateTime vBGBEstDt = gblFuction.setDate("");

            #region ddl

            if (hdnPriState.Value != "" && hdnPriState.Value != "0" && hdnPriState.Value != "-1")
            {
                ddlPriState.SelectedValue = hdnPriState.Value;
                if (hdnPriDist.Value != "" && hdnPriDist.Value != "0" && hdnPriDist.Value != "-1")
                {
                    PopDistrictByState(Convert.ToInt32(ddlPriState.SelectedValue), "Pri");
                    ddlPriDist.SelectedValue = hdnPriDist.Value;
                }
            }


            if (hdnBGVState.Value != "" && hdnBGVState.Value != "0" && hdnBGVState.Value != "-1")
            {
                ddlBGVState.SelectedValue = hdnBGVState.Value;
                if (hdnBGVDist.Value != "" && hdnBGVDist.Value != "0" && hdnBGVDist.Value != "-1")
                {
                    PopDistrictByState(Convert.ToInt32(ddlBGVState.SelectedValue), "BGV");
                    ddlBGVDist.SelectedValue = hdnBGVDist.Value;
                }
            }

            if (hdnPrtState.Value != "" && hdnPrtState.Value != "0" && hdnPrtState.Value != "-1")
            {
                ddlPrtState.SelectedValue = hdnPrtState.Value;
                if (hdnPrtDist.Value != "" && hdnPrtDist.Value != "0" && hdnPrtDist.Value != "-1")
                {
                    PopDistrictByState(Convert.ToInt32(ddlPrtState.SelectedValue), "PrtC");
                    ddlPrtDist.SelectedValue = hdnPrtDist.Value;
                }
            }

            if (hdnPState.Value != "" && hdnPState.Value != "0" && hdnPState.Value != "-1")
            {
                ddlPState.SelectedValue = hdnPState.Value;
                if (hdnPDist.Value != "" && hdnPDist.Value != "0" && hdnPDist.Value != "-1")
                {
                    PopDistrictByState(Convert.ToInt32(ddlPState.SelectedValue), "PrtP");
                    ddlPDist.SelectedValue = hdnPDist.Value;
                }
            }

            #endregion


            vEpcName = txtPriEPCName.Text.ToString().Trim(); vPriGSTNNo = txtPriGSTNNo.Text.ToString().Trim(); vPriEPCCode = txtPriEPCCode.Text.ToString().Trim();
            vPriRegnNo = txtPriRegnNo.Text.ToString().Trim(); vPriTradeLcnse = txtPriTradeLcnse.Text.ToString().Trim(); vPriEntityType = ddlPriEntityType.SelectedValue.ToString().Trim();
            vPriCPAN = txtPriCPAN.Text.ToString().Trim();
            vPriPDeed = txtPriPDeed.Text.ToString().Trim(); vPriMOA = txtPriMOA.Text.ToString().Trim(); vPriAOA = txtPriAOA.Text.ToString().Trim(); vPriCertNo = txtPriCertNo.Text.ToString().Trim();
            vPriEPCType = ddlPriEPCType.SelectedValue; ; vBuyBkScheme = ddlBuyBkScheme.SelectedValue; vEPCCategory = ddlEPCCategory.SelectedValue;

            vPriDisbursalRatio = hdPriDisbursalRatio.Value;


            vPriHouseNo = txtPriHouseNo.Text.ToString().Trim(); vPriArea = txtPriArea.Text.ToString().Trim(); vPriLandmark = txtPriLandmark.Text.ToString().Trim();
            vPriStreet = txtPriStreet.Text.ToString().Trim(); vPriVillage = txtPriVillage.Text.ToString().Trim(); vPriSubdistrict = txtPriSubdistrict.Text.ToString().Trim();
            vPriPostOfc = txtPriPostOfc.Text.ToString().Trim();

            if (hdnPriState.Value == "") hdnPriState.Value = "0"; if (hdnPriDist.Value == "") hdnPriDist.Value = "0";
            if (ddlPriState.SelectedValue == "" || ddlPriState.SelectedValue == "-1") vPriState = Convert.ToInt32(hdnPriState.Value); else vPriState = Convert.ToInt32(ddlPriState.SelectedValue);
            if (ddlPriDist.SelectedValue == "" || ddlPriDist.SelectedValue == "-1") vPriDist = Convert.ToInt32(hdnPriDist.Value); else vPriDist = Convert.ToInt32(ddlPriDist.SelectedValue);

            vPriLandline = txtPriLandline.Text.ToString().Trim(); vPriMobile = txtPriMobile.Text.ToString().Trim();

            vACHolderNm = txtACHolderNm.Text.ToString().Trim(); vBankNm = txtBankNm.Text.ToString().Trim(); vACNo = txtACNo.Text.ToString().Trim();
            vReACNo = txtReACNo.Text.ToString().Trim(); vIFCE = txtIFCE.Text.ToString().Trim(); vAccountType = ddlAccountType.SelectedValue;
            vBankStatement = ddlBankStatement.SelectedValue; vCnclCq = "";
            vPenyDrpStatus = ddlPenyDrpStatus.SelectedValue;
            vRseMsg = txtRseMsg.Text.ToString().Trim(); vAuthority = ddlAuthority.SelectedValue; vBGBName = txtBGBName.Text.ToString().Trim();
            vBGBBrdName = txtBGBBrdName.Text.ToString().Trim(); vBGBHrs = txtBGBHrs.Text.ToString().Trim(); vBGBVintage = txtBGBVintage.Text.ToString().Trim();
            vBGBImage = "N";
            vBGBStreet = txtBGBStreet.Text.ToString().Trim(); vBGBArea = txtBGBArea.Text.ToString().Trim();
            vBGBLocality = txtBGBLocality.Text.ToString().Trim(); vBGBHouseNo = txtBGBHouseNo.Text.ToString().Trim();
            vBGBLandmark = txtBGBLandmark.Text.ToString().Trim(); vBGBVillage = txtBGBVillage.Text.ToString().Trim(); vBGBSubDistrict = txtBGBSubDistrict.Text.ToString().Trim();
            vBGBPostOffice = txtBGBPostOffice.Text.ToString().Trim();


            if (hdnBGVState.Value == "") hdnBGVState.Value = "0"; if (hdnBGVDist.Value == "") hdnBGVDist.Value = "0";
            if (ddlBGVState.SelectedValue == "" || ddlBGVState.SelectedValue == "-1") vBGBState = Convert.ToInt32(hdnBGVState.Value); else vBGBState = Convert.ToInt32(ddlBGVState.SelectedValue);
            if (ddlBGVDist.SelectedValue == "" || ddlBGVDist.SelectedValue == "-1") vBGBDist = Convert.ToInt32(hdnBGVDist.Value); else vBGBDist = Convert.ToInt32(ddlBGVDist.SelectedValue);

            vBGBLandline = txtBGBLandline.Text.ToString().Trim();
            vBGBMobile = txtBGBMobile.Text.ToString().Trim();

            vCreatedBy = this.UserID;
            if (txtPriPin.Text.Trim() != "") vPriPin = Convert.ToInt32(txtPriPin.Text);

            // DateTime vPrtnrAge = gblFuction.setDate(txtPrtnrAge.Text.ToString());

            if (txtBGBPincode.Text.Trim() != "") vBGBPincode = Convert.ToInt32(txtBGBPincode.Text);
            vBuyBkRatio = Convert.ToInt32(ddlBuyBkRatio.SelectedValue);
            if (txtBGBEmpNo.Text.Trim() != "") vBGBEmpNo = Convert.ToInt32(txtBGBEmpNo.Text);


            vBGBEstDt = gblFuction.setDate(txtBGBEstDt.Text.ToString());
            vBrCode = Session[gblValue.BrnchCode].ToString();


            CMember oMem = null;
            try
            {

                if (Mode == "Save" || Mode == "Edit")
                {

                    vPriGSTNFileName = "GSTN"; vPriTradeFileName = "Trade"; vPriRegnNoFileName = "Reg";
                    vPriCPANFileName = "CPAN"; vPriDeedFileName = "Deed"; vPriMOAFileName = "MOA"; vPriAOAFileName = "AOA";
                    vPriEntityFileNm = "Entity";
                    vIFSCFileNm = "IFSC"; vBSFileNm = "BnkStmnt"; vCheqFileNm = "CnclCq"; vBBFileNm = "BusinessBoard"; vBGBImageFileNm = "BusinessImage";


                    #region FileUpload

                    DataTable dtUp = null;
                    if (ViewState["Uploader"] != null)
                    {
                        dtUp = (DataTable)ViewState["Uploader"];
                    }
                    else
                    {
                        dtUp = new DataTable();

                        // Add columns to the DataTable


                        dtUp.Columns.Add("GSTNFileNm", typeof(string));
                        dtUp.Columns.Add("GSTNIsUpLd", typeof(string));
                        dtUp.Columns.Add("GSTNByteDoc", typeof(byte[]));
                        dtUp.Columns.Add("GSTNByteYN", typeof(string));

                        dtUp.Columns.Add("TradeFileNm", typeof(string));
                        dtUp.Columns.Add("TradeIsUpLd", typeof(string));
                        dtUp.Columns.Add("TradeByteDoc", typeof(byte[]));
                        dtUp.Columns.Add("TradeByteYN", typeof(string));

                        dtUp.Columns.Add("RegnFileNm", typeof(string));
                        dtUp.Columns.Add("RegnIsUpLd", typeof(string));
                        dtUp.Columns.Add("RegnByteDoc", typeof(byte[]));
                        dtUp.Columns.Add("RegnByteYN", typeof(string));

                        dtUp.Columns.Add("CPANFileNm", typeof(string));
                        dtUp.Columns.Add("CPANIsUpLd", typeof(string));
                        dtUp.Columns.Add("CPANByteDoc", typeof(byte[]));
                        dtUp.Columns.Add("CPANByteYN", typeof(string));

                        dtUp.Columns.Add("ETypeFileNm", typeof(string));
                        dtUp.Columns.Add("ETypeIsUpLd", typeof(string));
                        dtUp.Columns.Add("ETypeByteDoc", typeof(byte[]));
                        dtUp.Columns.Add("ETypeByteYN", typeof(string));

                        dtUp.Columns.Add("DeedFileNm", typeof(string));
                        dtUp.Columns.Add("DeedIsUpLd", typeof(string));
                        dtUp.Columns.Add("DeedByteDoc", typeof(byte[]));
                        dtUp.Columns.Add("DeedByteYN", typeof(string));

                        dtUp.Columns.Add("MOAFileNm", typeof(string));
                        dtUp.Columns.Add("MOAIsUpLd", typeof(string));
                        dtUp.Columns.Add("MOAByteDoc", typeof(byte[]));
                        dtUp.Columns.Add("MOAByteYN", typeof(string));

                        dtUp.Columns.Add("AOAFileNm", typeof(string));
                        dtUp.Columns.Add("AOAIsUpLd", typeof(string));
                        dtUp.Columns.Add("AOAByteDoc", typeof(byte[]));
                        dtUp.Columns.Add("AOAByteYN", typeof(string));

                        dtUp.Columns.Add("BnkSFileNm", typeof(string));
                        dtUp.Columns.Add("BnkSsUpLd", typeof(string));
                        dtUp.Columns.Add("BnkSByteDoc", typeof(byte[]));
                        dtUp.Columns.Add("BnkSByteYN", typeof(string));

                        dtUp.Columns.Add("CChqFileNm", typeof(string));
                        dtUp.Columns.Add("CChqFIsUpLd", typeof(string));
                        dtUp.Columns.Add("CChqFByteDoc", typeof(byte[]));
                        dtUp.Columns.Add("CChqFByteYN", typeof(string));

                        dtUp.Columns.Add("BNBFileNm", typeof(string));
                        dtUp.Columns.Add("BNBIsUpLd", typeof(string));
                        dtUp.Columns.Add("BNBByteDoc", typeof(byte[]));
                        dtUp.Columns.Add("BNBByteYN", typeof(string));

                        dtUp.Columns.Add("BImgFileNm", typeof(string));
                        dtUp.Columns.Add("BImgIsUpLd", typeof(string));
                        dtUp.Columns.Add("BImgByteDoc", typeof(byte[]));
                        dtUp.Columns.Add("BImgByteYN", typeof(string));
                    }

                    DataRow row = dtUp.NewRow();

                    //START FOR GSTN Upload
                    vPriGSTNIsFileUpLd = fuGSTNUpld.HasFile == true ? "Y" : "N";
                    if (vPriGSTNIsFileUpLd == "Y")
                    {
                        //vPriGSTNFileExt = System.IO.Path.GetExtension(fuGSTNUpld.PostedFile.FileName);
                        //vPriGSTNFileName = fuGSTNUpld.HasFile == true ? vPriGSTNFileName + vPriGSTNFileExt : "";


                        vFuGSTN = new byte[fuGSTNUpld.PostedFile.InputStream.Length + 1];
                        fuGSTNUpld.PostedFile.InputStream.Read(vFuGSTN, 0, vFuGSTN.Length);
                        vPriGSTNFileExt = System.IO.Path.GetExtension(fuGSTNUpld.FileName).ToLower();
                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vPriGSTNFileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuGSTNUpld.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuGSTNUpld.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                        if ((vPriGSTNFileExt.ToLower() != ".pdf") && (vPriGSTNFileExt.ToLower() != ".xlx") && (vPriGSTNFileExt.ToLower() != ".xlsx"))
                        {
                            vPriGSTNFileExt = ".png";
                        }
                        vPriGSTNFileName = vPriGSTNFileName + vPriGSTNFileExt;
                        vGSTNByteYN = "Y";

                        ViewState["GSTNFileNm"] = vPriGSTNFileName;
                        ViewState["GSTNIsUpLd"] = vPriGSTNIsFileUpLd;
                        ViewState["GSTNByteDoc"] = vFuGSTN;
                        ViewState["GSTNByteYN"] = vGSTNByteYN;

                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            if (lblGSTNFileName.Text != "")
                            {
                                vPriGSTNIsFileUpLd = "Y";
                                vPriGSTNFileName = lblGSTNFileName.Text;
                            }
                            else
                            {
                                gblFuction.AjxMsgPopup("Please Upload MSME / GSTIN No. File...");
                                return false;
                            }
                        }
                        else if ((String)ViewState["GSTNByteYN"] != "Y")
                        {
                            gblFuction.AjxMsgPopup("Please Upload MSME / GSTIN No. File...");
                            return false;
                        }
                    }
                    // END For GSTN

                    //START FOR TRADE Upload
                    vPriTradeIsFileUpLd = fuTradeUpld.HasFile == true ? "Y" : "N";
                    if (vPriTradeIsFileUpLd == "Y")
                    {                        
                        vFuTrade = new byte[fuTradeUpld.PostedFile.InputStream.Length + 1];
                        fuTradeUpld.PostedFile.InputStream.Read(vFuTrade, 0, vFuTrade.Length);
                        vPriTradeFileExt = System.IO.Path.GetExtension(fuTradeUpld.FileName).ToLower();
                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vPriTradeFileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuTradeUpld.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuTradeUpld.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                        if ((vPriTradeFileExt.ToLower() != ".pdf") && (vPriTradeFileExt.ToLower() != ".xlx") && (vPriTradeFileExt.ToLower() != ".xlsx"))
                        {
                            vPriTradeFileExt = ".png";
                        }
                        vPriTradeFileName = vPriTradeFileName + vPriTradeFileExt;
                        vTradeByteYN = "Y";

                        ViewState["TradeFileNm"] = vPriTradeFileName;
                        ViewState["TradeIsUpLd"] = vPriTradeIsFileUpLd;
                        ViewState["TradeByteDoc"] = vFuTrade;
                        ViewState["TradeByteYN"] = vTradeByteYN;

                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            if (lblTradeFileName.Text != "")
                            {
                                vPriTradeIsFileUpLd = "Y";
                                vPriTradeFileName = lblTradeFileName.Text;
                            }
                            else
                            {
                                gblFuction.AjxMsgPopup("Please Upload Trade License File...");
                                return false;
                            }
                        }
                        else if ((String)ViewState["TradeByteYN"] != "Y")
                        {
                            gblFuction.AjxMsgPopup("Please Upload Trade License File...");
                            return false;
                        }
                    }
                    // END For TRADE

                    //START FOR Reg Upload
                    vPriRegnNoIsFileUpLd = fuRegNoUpld.HasFile == true ? "Y" : "N";
                    if (vPriRegnNoIsFileUpLd == "Y")
                    {
                        //vPriRegNoFileExt = System.IO.Path.GetExtension(fuRegNoUpld.PostedFile.FileName);
                        //vPriRegnNoFileName = fuRegNoUpld.HasFile == true ? vPriRegnNoFileName + vPriRegNoFileExt : "";

                        vFuRegn = new byte[fuRegNoUpld.PostedFile.InputStream.Length + 1];
                        fuRegNoUpld.PostedFile.InputStream.Read(vFuRegn, 0, vFuRegn.Length);
                        vPriRegNoFileExt = System.IO.Path.GetExtension(fuRegNoUpld.FileName).ToLower();
                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vPriRegNoFileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuRegNoUpld.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuRegNoUpld.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                        if ((vPriRegNoFileExt.ToLower() != ".pdf") && (vPriRegNoFileExt.ToLower() != ".xlx") && (vPriRegNoFileExt.ToLower() != ".xlsx"))
                        {
                            vPriRegNoFileExt = ".png";
                        }
                        vPriRegnNoFileName = vPriRegnNoFileName + vPriRegNoFileExt;
                        vRegnByteYN = "Y";

                        ViewState["RegnFileNm"] = vPriRegnNoFileName;
                        ViewState["RegnIsUpLd"] = vPriRegnNoIsFileUpLd;
                        ViewState["RegnByteDoc"] = vFuRegn;
                        ViewState["RegnByteYN"] = vRegnByteYN;

                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            if (lblRegNoFileName.Text != "")
                            {
                                vPriRegnNoIsFileUpLd = "Y";
                                vPriRegnNoFileName = lblRegNoFileName.Text;
                            }
                            else
                            {
                                gblFuction.AjxMsgPopup("Please Upload Udyam Regn. No. File...");
                                return false;
                            }
                        }
                        else if ((String)ViewState["RegnByteYN"] != "Y")
                        {
                            gblFuction.AjxMsgPopup("Please Upload Udyam Regn. No. File...");
                            return false;
                        }
                    }
                    // END For Reg




                    //START FOR CPAN Upload
                    vPriCPANIsFileUpLd = fuCPANUpld.HasFile == true ? "Y" : "N";
                    if (vPriCPANIsFileUpLd == "Y")
                    {
                        //vPriCPANFileExt = System.IO.Path.GetExtension(fuCPANUpld.PostedFile.FileName);
                        //vPriCPANFileName = fuCPANUpld.HasFile == true ? vPriCPANFileName + vPriCPANFileExt : "";

                        vFuCPAN = new byte[fuCPANUpld.PostedFile.InputStream.Length + 1];
                        fuCPANUpld.PostedFile.InputStream.Read(vFuCPAN, 0, vFuCPAN.Length);
                        vPriCPANFileExt = System.IO.Path.GetExtension(fuCPANUpld.FileName).ToLower();
                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vPriCPANFileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuCPANUpld.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuCPANUpld.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                        if ((vPriCPANFileExt.ToLower() != ".pdf") && (vPriCPANFileExt.ToLower() != ".xlx") && (vPriCPANFileExt.ToLower() != ".xlsx"))
                        {
                            vPriCPANFileExt = ".png";
                        }
                        vPriCPANFileName = vPriCPANFileName + vPriCPANFileExt;
                        vCPANByteYN = "Y";

                        ViewState["CPANFileNm"] = vPriCPANFileName;
                        ViewState["CPANIsUpLd"] = vPriCPANIsFileUpLd;
                        ViewState["CPANByteDoc"] = vFuCPAN;
                        ViewState["CPANByteYN"] = vCPANByteYN;

                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            if (lblCPANFileName.Text != "Y")
                            {
                                vPriCPANIsFileUpLd = "Y";
                                vPriCPANFileName = lblCPANFileName.Text;
                            }
                            else
                            {
                                gblFuction.AjxMsgPopup("Please Upload Company PAN File...");
                                return false;
                            }
                        }
                        else if ((String)ViewState["CPANByteYN"] == "N")
                        {
                            gblFuction.AjxMsgPopup("Please Upload Company PAN File...");
                            return false;
                        }
                    }
                    // END For CPAN

                    //START FOR Entity Upload
                    vPriEntityIsFileUpLd = fuEntityUpLd.HasFile == true ? "Y" : "N";
                    if (vPriEntityIsFileUpLd == "Y")
                    {
                        //vPriEntityFileExt = System.IO.Path.GetExtension(fuEntityUpLd.PostedFile.FileName);
                        //vPriEntityFileNm = fuEntityUpLd.HasFile == true ? vPriEntityFileNm + vPriEntityFileExt : "";

                        vFuEType = new byte[fuEntityUpLd.PostedFile.InputStream.Length + 1];
                        fuEntityUpLd.PostedFile.InputStream.Read(vFuEType, 0, vFuEType.Length);
                        vPriEntityFileExt = System.IO.Path.GetExtension(fuEntityUpLd.FileName).ToLower();
                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vPriEntityFileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuEntityUpLd.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuEntityUpLd.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                        if ((vPriEntityFileExt.ToLower() != ".pdf") && (vPriEntityFileExt.ToLower() != ".xlx") && (vPriEntityFileExt.ToLower() != ".xlsx"))
                        {
                            vPriEntityFileExt = ".png";
                        }
                        vPriEntityFileNm = vPriEntityFileNm + vPriEntityFileExt;
                        vETypeByteYN = "Y";

                        ViewState["ETypeFileNm"] = vPriEntityFileNm;
                        ViewState["ETypeIsUpLd"] = vPriEntityIsFileUpLd;
                        ViewState["ETypeByteDoc"] = vFuEType;
                        ViewState["ETypeByteYN"] = vETypeByteYN;

                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            if (lblEntityFileNm.Text != "")
                            {
                                vPriEntityIsFileUpLd = "Y";
                                vPriEntityFileNm = lblEntityFileNm.Text;
                            }
                            else
                            {
                                gblFuction.AjxMsgPopup("Please Upload Entity Type File...");
                                return false;
                            }
                        }
                        else if ((String)ViewState["ETypeByteYN"] != "Y")
                        {
                            gblFuction.AjxMsgPopup("Please Upload Entity Type File...");
                            return false;
                        }
                    }
                    // END For Entity

                    //START FOR DeeD Upload
                    if (ddlPriEntityType.SelectedValue == "P")
                    {
                        vPriDeedIsFileUpLd = fuDeedUpld.HasFile == true ? "Y" : "N";
                        if (vPriDeedIsFileUpLd == "Y")
                        {
                            //vPriDeedFileExt = System.IO.Path.GetExtension(fuDeedUpld.PostedFile.FileName);
                            //vPriDeedFileName = fuDeedUpld.HasFile == true ? vPriDeedFileName + vPriDeedFileExt : "";

                            vFuDeed = new byte[fuDeedUpld.PostedFile.InputStream.Length + 1];
                            fuDeedUpld.PostedFile.InputStream.Read(vFuDeed, 0, vFuDeed.Length);
                            vPriDeedFileExt = System.IO.Path.GetExtension(fuDeedUpld.FileName).ToLower();
                            //---------------------------------------------------------------
                            bool ValidPdf = false;
                            if (vPriDeedFileExt.ToLower() == ".pdf")
                            {
                                cFileValidate oFile = new cFileValidate();
                                ValidPdf = oFile.ValidatePdf(fuDeedUpld.FileBytes);
                                if (ValidPdf == false)
                                {
                                    gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                    fuDeedUpld.Focus();
                                    return false;
                                }
                            }
                            //------------------------------------------------------------
                            if ((vPriDeedFileExt.ToLower() != ".pdf") && (vPriDeedFileExt.ToLower() != ".xlx") && (vPriDeedFileExt.ToLower() != ".xlsx"))
                            {
                                vPriDeedFileExt = ".png";
                            }
                            vPriDeedFileName = vPriDeedFileName + vPriDeedFileExt;
                            vDeedByteYN = "Y";

                            ViewState["DeedFileNm"] = vPriDeedFileName;
                            ViewState["DeedIsUpLd"] = vPriDeedIsFileUpLd;
                            ViewState["DeedByteDoc"] = vFuDeed;
                            ViewState["DeedByteYN"] = vDeedByteYN;

                        }
                        else
                        {
                            if (Mode == "Edit")
                            {
                                if (lblDeedFileName.Text != "")
                                {
                                    vPriDeedIsFileUpLd = "Y";
                                    vPriDeedFileName = lblDeedFileName.Text;
                                }
                                else
                                {
                                    gblFuction.AjxMsgPopup("Please Upload Partnership Deed File...");
                                    return false;
                                }
                            }
                            else if ((String)ViewState["DeedByteYN"] != "Y")
                            {
                                gblFuction.AjxMsgPopup("Please Upload Partnership Deed File...");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        ViewState["DeedFileNm"] = "";
                        ViewState["DeedIsUpLd"] = "N";
                        ViewState["DeedByteDoc"] = vFuDeed;
                        ViewState["DeedByteYN"] = vDeedByteYN;
                    }
                    // END For DeeD

                    //START FOR MOA Upload
                    vPriMOAIsFileUpLd = fuMOAUpld.HasFile == true ? "Y" : "N";
                    if (vPriMOAIsFileUpLd == "Y")
                    {
                        //vPriMOAFileExt = System.IO.Path.GetExtension(fuMOAUpld.PostedFile.FileName);
                        //vPriMOAFileName = fuMOAUpld.HasFile == true ? vPriMOAFileName + vPriMOAFileExt : "";

                        vFuMOA = new byte[fuMOAUpld.PostedFile.InputStream.Length + 1];
                        fuMOAUpld.PostedFile.InputStream.Read(vFuMOA, 0, vFuMOA.Length);
                        vPriMOAFileExt = System.IO.Path.GetExtension(fuMOAUpld.FileName).ToLower();
                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vPriMOAFileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuMOAUpld.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuMOAUpld.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                        if ((vPriMOAFileExt.ToLower() != ".pdf") && (vPriMOAFileExt.ToLower() != ".xlx") && (vPriMOAFileExt.ToLower() != ".xlsx"))
                        {
                            vPriMOAFileExt = ".png";
                        }
                        vPriMOAFileName = vPriMOAFileName + vPriMOAFileExt;
                        vMOAByteYN = "Y";

                        ViewState["MOAFileNm"] = vPriMOAFileName;
                        ViewState["MOAIsUpLd"] = vPriMOAIsFileUpLd;
                        ViewState["MOAByteDoc"] = vFuMOA;
                        ViewState["MOAByteYN"] = vMOAByteYN;

                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            if (lblMOAFileName.Text != "")
                            {
                                vPriMOAIsFileUpLd = "Y";
                                vPriMOAFileName = lblMOAFileName.Text;
                            }
                            else
                            {
                                vPriMOAFileName = "";
                                vPriMOAIsFileUpLd = "N";
                            }
                        }
                        else
                        {
                            vPriMOAFileName = "";
                            vPriMOAIsFileUpLd = "N";
                        }
                    }
                    // END For MOA

                    //START FOR AOA Upload
                    vPriAOAIsFileUpLd = fuAOAUpld.HasFile == true ? "Y" : "N";
                    if (vPriAOAIsFileUpLd == "Y")
                    {
                        //vPriAOAFileExt = System.IO.Path.GetExtension(fuAOAUpld.PostedFile.FileName);
                        //vPriAOAFileName = fuAOAUpld.HasFile == true ? vPriAOAFileName + vPriAOAFileExt : "";

                        vFuAOA = new byte[fuAOAUpld.PostedFile.InputStream.Length + 1];
                        fuAOAUpld.PostedFile.InputStream.Read(vFuAOA, 0, vFuAOA.Length);
                        vPriAOAFileExt = System.IO.Path.GetExtension(fuAOAUpld.FileName).ToLower();
                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vPriAOAFileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuAOAUpld.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuAOAUpld.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                        if ((vPriAOAFileExt.ToLower() != ".pdf") && (vPriAOAFileExt.ToLower() != ".xlx") && (vPriAOAFileExt.ToLower() != ".xlsx"))
                        {
                            vPriAOAFileExt = ".png";
                        }
                        vPriAOAFileName = vPriAOAFileName + vPriAOAFileExt;
                        vAOAByteYN = "Y";

                        ViewState["AOAFileNm"] = vPriAOAFileName;
                        ViewState["AOAIsUpLd"] = vPriAOAIsFileUpLd;
                        ViewState["AOAByteDoc"] = vFuAOA;
                        ViewState["AOAByteYN"] = vAOAByteYN;

                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            if (lblAOAFileName.Text != "")
                            {
                                vPriAOAIsFileUpLd = "Y";
                                vPriAOAFileName = lblAOAFileName.Text;
                            }
                            else
                            {
                                vPriAOAFileName = "";
                                vPriAOAIsFileUpLd = "N";
                            }
                        }
                        else
                        {
                            vPriAOAFileName = "";
                            vPriAOAIsFileUpLd = "N";
                        }
                    }
                    // END For AOA




                    vIFSCFileNm = "";
                    vIFSCIsFileUpLd = "N";
                    // END For IFSC

                    //START FOR Bnk Stmnt
                    vBSIsFileUpLd = fuStaUpld.HasFile == true ? "Y" : "N";
                    if (vBSIsFileUpLd == "Y")
                    {

                        vFuBnkS = new byte[fuStaUpld.PostedFile.InputStream.Length + 1];
                        fuStaUpld.PostedFile.InputStream.Read(vFuBnkS, 0, vFuBnkS.Length);
                        vBSFileExt = System.IO.Path.GetExtension(fuStaUpld.FileName).ToLower();
                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vBSFileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuStaUpld.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuStaUpld.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                        if ((vBSFileExt.ToLower() != ".pdf") && (vBSFileExt.ToLower() != ".xlx") && (vBSFileExt.ToLower() != ".xlsx"))
                        {
                            vBSFileExt = ".png";
                        }
                        vBSFileNm = vBSFileNm + vBSFileExt;
                        vBnkSByteYN = "Y";

                        ViewState["BnkSFileNm"] = vBSFileNm;
                        ViewState["BnkSsUpLd"] = vBSIsFileUpLd;
                        ViewState["BnkSByteDoc"] = vFuBnkS;
                        ViewState["BnkSByteYN"] = vBnkSByteYN;

                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            if (lblBankFileNm.Text != "")
                            {
                                vBSIsFileUpLd = "Y";
                                vBSFileNm = lblBankFileNm.Text;
                            }
                            else
                            {
                                gblFuction.AjxMsgPopup("Please Upload Bank Statement File...");
                                return false;
                            }
                        }
                        else if ((String)ViewState["BnkSByteYN"] != "Y")
                        {
                            gblFuction.AjxMsgPopup("Please Upload Bank Statement File...");
                            return false;
                        }
                    }
                    // END For Bnk Stmnt

                    //START FOR Cncl Chq
                    vCheqIsFileUpLd = fuChqUpld.HasFile == true ? "Y" : "N";
                    if (vCheqIsFileUpLd == "Y")
                    {
                        //vCheqFileExt = System.IO.Path.GetExtension(fuChqUpld.PostedFile.FileName);
                        //vCheqFileNm = fuChqUpld.HasFile == true ? vCheqFileNm + vCheqFileExt : "";

                        vFuCChq = new byte[fuChqUpld.PostedFile.InputStream.Length + 1];
                        fuChqUpld.PostedFile.InputStream.Read(vFuCChq, 0, vFuCChq.Length);
                        vCheqFileExt = System.IO.Path.GetExtension(fuChqUpld.FileName).ToLower();
                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vCheqFileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuChqUpld.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuChqUpld.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                        if ((vCheqFileExt.ToLower() != ".pdf") && (vCheqFileExt.ToLower() != ".xlx") && (vCheqFileExt.ToLower() != ".xlsx"))
                        {
                            vCheqFileExt = ".png";
                        }
                        vCheqFileNm = vCheqFileNm + vCheqFileExt;
                        vCChqFByteYN = "Y";

                        ViewState["CChqFileNm"] = vCheqFileNm;
                        ViewState["CChqFIsUpLd"] = vCheqIsFileUpLd;
                        ViewState["CChqFByteDoc"] = vFuCChq;
                        ViewState["CChqFByteYN"] = vCChqFByteYN;

                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            if (lblChqFileNm.Text != "")
                            {
                                vCheqIsFileUpLd = "Y";
                                vCheqFileNm = lblChqFileNm.Text;
                            }
                            else
                            {
                                gblFuction.AjxMsgPopup("Please Upload Cancelled Cheque File...");
                                return false;
                            }
                        }
                        else if ((String)ViewState["CChqFByteYN"] == "N")
                        {
                            gblFuction.AjxMsgPopup("Please Upload Cancelled Cheque File...");
                            return false;
                        }
                    }

                    // END For Cncl Chq

                    //START FOR BusinessBoard
                    vBBIsFileUpLd = fuBUpld.HasFile == true ? "Y" : "N";
                    if (vBBIsFileUpLd == "Y")
                    {
                        //vBBFileExt = System.IO.Path.GetExtension(fuBUpld.PostedFile.FileName);
                        //vBBFileNm = fuBUpld.HasFile == true ? vBBFileNm + vBBFileExt : "";

                        vFuBNB = new byte[fuBUpld.PostedFile.InputStream.Length + 1];
                        fuBUpld.PostedFile.InputStream.Read(vFuBNB, 0, vFuBNB.Length);
                        vBBFileExt = System.IO.Path.GetExtension(fuBUpld.FileName).ToLower();
                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vBBFileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuBUpld.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuBUpld.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                        if ((vBBFileExt.ToLower() != ".pdf") && (vBBFileExt.ToLower() != ".xlx") && (vBBFileExt.ToLower() != ".xlsx"))
                        {
                            vBBFileExt = ".png";
                        }
                        vBBFileNm = vBBFileNm + vBBFileExt;
                        vBNBByteYN = "Y";

                        ViewState["BNBFileNm"] = vBBFileNm;
                        ViewState["BNBIsUpLd"] = vBBIsFileUpLd;
                        ViewState["BNBByteDoc"] = vFuBNB;
                        ViewState["BNBByteYN"] = vBNBByteYN;

                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            if (lblBsnsFileNm.Text != "")
                            {
                                vBBIsFileUpLd = "Y";
                                vBBFileNm = lblBsnsFileNm.Text;
                            }
                            else
                            {
                                gblFuction.AjxMsgPopup("Please Upload Business Name Board File...");
                                return false;
                            }
                        }
                        else if ((String)ViewState["BNBByteYN"] != "Y")
                        {
                            gblFuction.AjxMsgPopup("Please Upload Business Name Board File...");
                            return false;
                        }
                    }

                    // END For BusinessBoard

                    //START FOR BusinessImage
                    vBGBImage = fuBGBUpld.HasFile == true ? "Y" : "N";
                    if (vBGBImage == "Y")
                    {
                        //vBGBFileExt = System.IO.Path.GetExtension(fuBGBUpld.PostedFile.FileName);
                        //vBGBImageFileNm = fuBGBUpld.HasFile == true ? vBGBImageFileNm + vBGBFileExt : "";

                        if ((vBGBFileExt.ToLower() != ".pdf") && (vBGBFileExt.ToLower() != ".xlx") && (vBGBFileExt.ToLower() != ".xlsx"))
                        {
                            vBGBFileExt = ".png";
                        }

                        vFuBImg = new byte[fuBGBUpld.PostedFile.InputStream.Length + 1];
                        fuBGBUpld.PostedFile.InputStream.Read(vFuBImg, 0, vFuBImg.Length);
                        vBGBFileExt = System.IO.Path.GetExtension(fuBGBUpld.FileName).ToLower();
                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vBGBFileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuBGBUpld.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuBGBUpld.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                        if ((vBGBFileExt.ToLower() != ".pdf") && (vBGBFileExt.ToLower() != ".xlx") && (vBGBFileExt.ToLower() != ".xlsx"))
                        {
                            vBGBFileExt = ".png";
                        }
                        vBGBImageFileNm = vBGBImageFileNm + vBGBFileExt;
                        vBImgByteYN = "Y";

                        ViewState["BImgFileNm"] = vBGBImageFileNm;
                        ViewState["BImgIsUpLd"] = vBGBImage;
                        ViewState["BImgByteDoc"] = vFuBImg;
                        ViewState["BImgByteYN"] = vBImgByteYN;
                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            if (lblBGBFileNm.Text != "")
                            {
                                vBGBImage = "Y";
                                vBGBImageFileNm = lblBGBFileNm.Text;
                            }
                            else
                            {
                                gblFuction.AjxMsgPopup("Please Upload Business image by SO with selfie File...");
                                return false;
                            }
                        }
                        else if ((String)ViewState["BImgByteYN"] != "Y")
                        {
                            gblFuction.AjxMsgPopup("Please Upload Business image by SO with selfie File...");
                            return false;
                        }
                    }

                    // END For BusinessImage
                    if (ViewState["GSTNByteYN"] != null)
                    {
                        vGSTNByteYN = (string)ViewState["GSTNByteYN"];
                    }
                    if (ViewState["GSTNFileNm"] != null)
                    {
                        vPriGSTNFileName = (string)ViewState["GSTNFileNm"];
                    }
                    if (ViewState["GSTNByteDoc"] != null)
                    {
                        vFuGSTN = (byte[])ViewState["GSTNByteDoc"];
                    }

                    if (ViewState["RegnByteYN"] != null)
                    {
                        vRegnByteYN = (string)ViewState["RegnByteYN"];
                    }
                    if (ViewState["RegnFileNm"] != null)
                    {
                        vPriRegnNoFileName = (string)ViewState["RegnFileNm"];
                    }
                    if (ViewState["RegnByteDoc"] != null)
                    {
                        vFuRegn = (byte[])ViewState["RegnByteDoc"];
                    }

                    if (ViewState["TradeByteYN"] != null)
                    {
                        vTradeByteYN = (string)ViewState["TradeByteYN"];
                    }
                    if (ViewState["TradeFileNm"] != null)
                    {
                        vPriTradeFileName = (string)ViewState["TradeFileNm"];
                    }
                    if (ViewState["TradeByteDoc"] != null)
                    {
                        vFuTrade = (byte[])ViewState["TradeByteDoc"];
                    }

                    if (ViewState["CPANByteYN"] != null)
                    {
                        vCPANByteYN = (string)ViewState["CPANByteYN"];
                    }
                    if (ViewState["CPANFileNm"] != null)
                    {
                        vPriCPANFileName = (string)ViewState["CPANFileNm"];
                    }
                    if (ViewState["CPANByteDoc"] != null)
                    {
                        vFuCPAN = (byte[])ViewState["CPANByteDoc"];
                    }

                    if (ViewState["DeedByteYN"] != null)
                    {
                        vDeedByteYN = (string)ViewState["DeedByteYN"];
                    }
                    if (ViewState["DeedFileNm"] != null)
                    {
                        vPriDeedFileName = (string)ViewState["DeedFileNm"];
                    }
                    if (ViewState["DeedByteDoc"] != null)
                    {
                        vFuDeed = (byte[])ViewState["DeedByteDoc"];
                    }

                    if (ViewState["MOAByteYN"] != null)
                    {
                        vMOAByteYN = (string)ViewState["MOAByteYN"];
                    }
                    if (ViewState["MOAFileNm"] != null)
                    {
                        vPriMOAFileName = (string)ViewState["MOAFileNm"];
                    }
                    if (ViewState["MOAByteDoc"] != null)
                    {
                        vFuMOA = (byte[])ViewState["MOAByteDoc"];
                    }

                    if (ViewState["AOAByteYN"] != null)
                    {
                        vAOAByteYN = (string)ViewState["AOAByteYN"];
                    }
                    if (ViewState["AOAFileNm"] != null)
                    {
                        vPriAOAFileName = (string)ViewState["AOAFileNm"];
                    }
                    if (ViewState["AOAByteDoc"] != null)
                    {
                        vFuAOA = (byte[])ViewState["AOAByteDoc"];
                    }

                    if (ViewState["ETypeByteYN"] != null)
                    {
                        vETypeByteYN = (string)ViewState["ETypeByteYN"];
                    }
                    if (ViewState["ETypeFileNm"] != null)
                    {
                        vPriEntityFileNm = (string)ViewState["ETypeFileNm"];
                    }
                    if (ViewState["ETypeByteDoc"] != null)
                    {
                        vFuEType = (byte[])ViewState["ETypeByteDoc"];
                    }

                    if (ViewState["BnkSByteYN"] != null)
                    {
                        vBnkSByteYN = (string)ViewState["BnkSByteYN"];
                    }
                    if (ViewState["BnkSFileNm"] != null)
                    {
                        vBSFileNm = (string)ViewState["BnkSFileNm"];
                    }
                    if (ViewState["BnkSByteDoc"] != null)
                    {
                        vFuBnkS = (byte[])ViewState["BnkSByteDoc"];
                    }

                    if (ViewState["CChqFByteYN"] != null)
                    {
                        vCChqFByteYN = (string)ViewState["CChqFByteYN"];
                    }
                    if (ViewState["CChqFileNm"] != null)
                    {
                        vCheqFileNm = (string)ViewState["CChqFileNm"];
                    }
                    if (ViewState["CChqFByteDoc"] != null)
                    {
                        vFuCChq = (byte[])ViewState["CChqFByteDoc"];
                    }

                    if (ViewState["BNBByteYN"] != null)
                    {
                        vBNBByteYN = (string)ViewState["BNBByteYN"];
                    }
                    if (ViewState["BNBFileNm"] != null)
                    {
                        vBBFileNm = (string)ViewState["BNBFileNm"];
                    }
                    if (ViewState["BNBByteDoc"] != null)
                    {
                        vFuBNB = (byte[])ViewState["BNBByteDoc"];
                    }

                    if (ViewState["BImgByteYN"] != null)
                    {
                        vBImgByteYN = (string)ViewState["BImgByteYN"];
                    }
                    if (ViewState["BImgFileNm"] != null)
                    {
                        vBGBImageFileNm = (string)ViewState["BImgFileNm"];
                    }
                    if (ViewState["BImgByteDoc"] != null)
                    {
                        vFuBImg = (byte[])ViewState["BImgByteDoc"];
                    }

                    row["GSTNFileNm"] = vPriGSTNFileName;
                    row["GSTNIsUpLd"] = vPriGSTNIsFileUpLd;
                    row["GSTNByteDoc"] = vFuGSTN;
                    row["GSTNByteYN"] = vGSTNByteYN;

                    row["RegnFileNm"] = vPriRegnNoFileName;
                    row["RegnIsUpLd"] = vPriRegnNoIsFileUpLd;
                    row["RegnByteDoc"] = vFuRegn;
                    row["RegnByteYN"] = vRegnByteYN;

                    row["TradeFileNm"] = vPriTradeFileName;
                    row["TradeIsUpLd"] = vPriTradeIsFileUpLd;
                    row["TradeByteDoc"] = vFuTrade;
                    row["TradeByteYN"] = vTradeByteYN;

                    row["CPANFileNm"] = vPriCPANFileName;
                    row["CPANIsUpLd"] = vPriCPANIsFileUpLd;
                    row["CPANByteDoc"] = vFuCPAN;
                    row["CPANByteYN"] = vCPANByteYN;

                    row["DeedFileNm"] = vPriDeedFileName;
                    row["DeedIsUpLd"] = vPriDeedIsFileUpLd;
                    row["DeedByteDoc"] = vFuDeed;
                    row["DeedByteYN"] = vDeedByteYN;

                    row["MOAFileNm"] = vPriMOAFileName;
                    row["MOAIsUpLd"] = vPriMOAIsFileUpLd;
                    row["MOAByteDoc"] = vFuMOA;
                    row["MOAByteYN"] = vMOAByteYN;

                    row["AOAFileNm"] = vPriAOAFileName;
                    row["AOAIsUpLd"] = vPriAOAIsFileUpLd;
                    row["AOAByteDoc"] = vFuAOA;
                    row["AOAByteYN"] = vAOAByteYN;

                    row["ETypeFileNm"] = vPriEntityFileNm;
                    row["ETypeIsUpLd"] = vPriEntityIsFileUpLd;
                    row["ETypeByteDoc"] = vFuEType;
                    row["ETypeByteYN"] = vETypeByteYN;

                    row["BnkSFileNm"] = vBSFileNm;
                    row["BnkSsUpLd"] = vBSIsFileUpLd;
                    row["BnkSByteDoc"] = vFuBnkS;
                    row["BnkSByteYN"] = vBnkSByteYN;

                    row["CChqFileNm"] = vCheqFileNm;
                    row["CChqFIsUpLd"] = vCheqIsFileUpLd;
                    row["CChqFByteDoc"] = vFuCChq;
                    row["CChqFByteYN"] = vCChqFByteYN;

                    row["BNBFileNm"] = vBBFileNm;
                    row["BNBIsUpLd"] = vBBIsFileUpLd;
                    row["BNBByteDoc"] = vFuBNB;
                    row["BNBByteYN"] = vBNBByteYN;

                    row["BImgFileNm"] = vBGBImageFileNm;
                    row["BImgIsUpLd"] = vBGBImage;
                    row["BImgByteDoc"] = vFuBImg;
                    row["BImgByteYN"] = vBImgByteYN;



                    dtUp.Rows.Add(row);
                    dtUp.AcceptChanges();
                    ViewState["Uploader"] = dtUp;

                    #endregion

                    //START partner Details
                    dt = (DataTable)ViewState["Partner"];

                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            dt.TableName = "Table";


                            foreach (DataRow rw in dt.Rows)
                            {

                                var vPartnrAge = rw["PrtnrAge"];
                                if (vPartnrAge is string)
                                {
                                    string vPAge = rw["PrtnrAge"].ToString();
                                    DateTime vAge = gblFuction.setDate(vPAge);
                                    rw["PrtnrAge"] = vAge;
                                }
                            }

                            using (StringWriter oSW = new StringWriter())
                            {
                                dt.WriteXml(oSW);
                                vXmlPrtnr = oSW.ToString();
                            }
                        }
                    }


                    //END Partner Details

                    oMem = new CMember();
                    vErr = oMem.CF_SaveEPCMst(ref  vNewId, Convert.ToInt64(hdEPCId.Value), vEpcName, vPriGSTNNo, vPriGSTNFileName, vPriGSTNIsFileUpLd,
                    vPriEPCCode, vPriRegnNo, vPriRegnNoFileName, vPriRegnNoIsFileUpLd,
                    vPriTradeLcnse, vPriTradeFileName, vPriTradeIsFileUpLd, vPriEntityType, vPriCPAN,
                    vPriCPANFileName, vPriCPANIsFileUpLd, vPriPDeed, vPriDeedFileName, vPriDeedIsFileUpLd,
                    vPriMOA, vPriMOAFileName, vPriMOAIsFileUpLd, vPriAOA, vPriAOAFileName, vPriAOAIsFileUpLd, DocumentBucketURL,
                    vPriCertNo, vPriEPCType, vBuyBkScheme, vBuyBkRatio, vEPCCategory, vPriDisbursalRatio,
                    vPriHouseNo, vPriArea, vPriLandmark, vPriStreet, vPriVillage, vPriSubdistrict, vPriPostOfc, vPriPin, vPriDist,
                    vPriState, vPriLandline, vPriMobile, vACHolderNm, vBankNm,
                    vACNo, vReACNo, vIFCE, vAccountType, vBankStatement, vCnclCq, vPenyDrpStatus, vRseMsg, vAuthority, vBGBName, vBGBBrdName, vBGBEstDt, vBGBHrs,
                    vBGBVintage, vBGBImage, vBGBStreet, vBGBArea, vBGBEmpNo, vBGBLocality, vBGBHouseNo, vBGBLandmark, vBGBVillage, vBGBSubDistrict, vBGBPostOffice,
                    vBGBPincode, vBGBDist, vBGBState, vBGBLandline, vBGBMobile,
                    vPriEntityFileNm, vPriEntityIsFileUpLd, vIFSCFileNm, vBSIsFileUpLd, vBSFileNm, vBSIsFileUpLd, vCheqFileNm, vCheqIsFileUpLd, vBBFileNm, vBBIsFileUpLd,
                    vCreatedBy, vBrCode, Mode, 0, ref vErrMsg, vBGBImageFileNm, vXmlPrtnr);
                    if (vErr == 0)
                    {
                        if (Mode == "Save")
                        {
                            hdEPCId.Value = Convert.ToString(vNewId);
                            ViewState["EPCId"] = vNewId;
                        }
                        else if (Mode == "Edit")
                        {
                            ViewState["EPCId"] = Convert.ToInt32(hdEPCId.Value);
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

                else
                {
                    oMem = new CMember();
                    vErr = oMem.CF_DeleteEPCMst(Convert.ToInt64(hdEPCId.Value), this.UserID, "Delet", 0, ref vErrMsg);
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
                    return vResult;
                }

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
        #region SaveMemberImages
        private string SaveMemberImages(FileUpload flup, string imageGroup, string imageName, string ImgExt, string isImageSaved)
        {
            CApiCalling oAC = new CApiCalling();
            byte[] ImgByte = ConvertFileToByteArray(flup.PostedFile);
            isImageSaved = oAC.UploadFileMinio(ImgByte, imageName + ImgExt, imageGroup, DocumentBucket, MinioUrl);
            return isImageSaved;
        }
        #endregion
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
        #region gvEPC_RowCommand
        protected void gvEPC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 pEpcID = 0;
            string vBrCode = "";
            DataTable dt, dt1 = null;
            DataSet ds = new DataSet();
            ClearControls(this.Page);
            CMember oMem = null;
            try
            {
                pEpcID = Convert.ToInt64(e.CommandArgument);
                vBrCode = Session[gblValue.BrnchCode].ToString();
                ViewState["EpcID"] = pEpcID;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvEPC.Rows)
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
                    oMem = new CMember();
                    GetEPCDetails(pEpcID, vBrCode);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }
        #endregion
        protected void GetEPCDetails(Int64 EpcID, string vBrCode)
        {
            DataTable dt, dt1 = null;
            DataSet ds = new DataSet();
            ClearControls(this.Page);
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                ds = oMem.CF_GetEPCDtlById(EpcID, vBrCode);
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                if (dt.Rows.Count > 0)
                {
                    hdEPCId.Value = Convert.ToString(dt.Rows[0]["EpcID"]);
                    txtPriEPCName.Text = Convert.ToString(dt.Rows[0]["EpcName"]);
                    txtPriGSTNNo.Text = Convert.ToString(dt.Rows[0]["PriGSTNNo"]);
                    txtPriEPCCode.Text = Convert.ToString(dt.Rows[0]["PriEPCCode"]);
                    txtPriRegnNo.Text = Convert.ToString(dt.Rows[0]["PriRegnNo"]);
                    txtPriTradeLcnse.Text = Convert.ToString(dt.Rows[0]["PriTradeLcnse"]);
                    ddlPriEntityType.SelectedIndex = ddlPriEntityType.Items.IndexOf(ddlPriEntityType.Items.FindByValue(dt.Rows[0]["PriEntityType"].ToString()));
                    txtPriCPAN.Text = Convert.ToString(dt.Rows[0]["PriCPAN"]);
                    txtPriPDeed.Text = Convert.ToString(dt.Rows[0]["PriPDeed"]);
                    txtPriMOA.Text = Convert.ToString(dt.Rows[0]["PriMOA"]);
                    txtPriAOA.Text = Convert.ToString(dt.Rows[0]["PriAOA"]);
                    txtPriCertNo.Text = Convert.ToString(dt.Rows[0]["PriCertNo"]);
                    ddlPriEPCType.SelectedIndex = ddlPriEPCType.Items.IndexOf(ddlPriEPCType.Items.FindByValue(dt.Rows[0]["PriEPCType"].ToString()));

                    ddlBuyBkScheme.SelectedIndex = ddlBuyBkScheme.Items.IndexOf(ddlBuyBkScheme.Items.FindByValue(dt.Rows[0]["PriBuyBkScheme"].ToString()));

                    ddlBuyBkRatio.SelectedIndex = ddlBuyBkRatio.Items.IndexOf(ddlBuyBkRatio.Items.FindByValue(dt.Rows[0]["PriBuyBkRatio"].ToString()));
                    ddlEPCCategory.SelectedIndex = ddlEPCCategory.Items.IndexOf(ddlEPCCategory.Items.FindByValue(dt.Rows[0]["PriEPCCategory"].ToString()));

                    ddlPriDisbursalRatio.SelectedIndex = ddlPriDisbursalRatio.Items.IndexOf(ddlPriDisbursalRatio.Items.FindByValue(dt.Rows[0]["PriDisbursalRatio"].ToString()));
                    hdPriDisbursalRatio.Value = Convert.ToString(dt.Rows[0]["PriDisbursalRatio"]);
                    txtPriHouseNo.Text = Convert.ToString(dt.Rows[0]["PriHouseNo"]);
                    txtPriStreet.Text = Convert.ToString(dt.Rows[0]["PriStreet"]);
                    txtPriLandmark.Text = Convert.ToString(dt.Rows[0]["PriLandmark"]);
                    txtPriArea.Text = Convert.ToString(dt.Rows[0]["PriArea"]);
                    txtPriVillage.Text = Convert.ToString(dt.Rows[0]["PriVillage"]);
                    txtPriSubdistrict.Text = Convert.ToString(dt.Rows[0]["PriSubdistrict"]);
                    txtPriPostOfc.Text = Convert.ToString(dt.Rows[0]["PriPostOfc"]);
                    txtPriPin.Text = Convert.ToString(dt.Rows[0]["PriPin"]);


                    ddlPriState.SelectedIndex = ddlPriState.Items.IndexOf(ddlPriState.Items.FindByValue(dt.Rows[0]["PriState"].ToString()));
                    PopDistrictByState(Convert.ToInt32(ddlPriState.SelectedValue), "Pri");
                    ddlPriDist.SelectedIndex = ddlPriDist.Items.IndexOf(ddlPriDist.Items.FindByValue(dt.Rows[0]["PriDist"].ToString()));

                    txtPriLandline.Text = Convert.ToString(dt.Rows[0]["PriLandline"]);
                    txtPriMobile.Text = Convert.ToString(dt.Rows[0]["PriMobile"]);

                    txtACHolderNm.Text = Convert.ToString(dt.Rows[0]["ACHolderNm"]);
                    txtBankNm.Text = Convert.ToString(dt.Rows[0]["BankNm"]);

                    if (Convert.ToString(dt.Rows[0]["ACNo"]) == "")
                    {
                        txtACNo.Text = "";
                        hdAcNo.Value = "";
                        txtReACNo.Text = "";
                    }
                    else
                    {
                        txtACNo.Attributes.Add("value", Convert.ToString(dt.Rows[0]["ACNo"]));
                        hdAcNo.Value = Convert.ToString(dt.Rows[0]["ACNo"]);
                        txtReACNo.Attributes.Add("value", Convert.ToString(dt.Rows[0]["ACNo"]));
                    }


                    txtIFCE.Text = Convert.ToString(dt.Rows[0]["IFCE"]);
                    ddlAccountType.SelectedIndex = ddlAccountType.Items.IndexOf(ddlAccountType.Items.FindByValue(dt.Rows[0]["AccountType"].ToString()));

                    ddlBankStatement.SelectedIndex = ddlBankStatement.Items.IndexOf(ddlBankStatement.Items.FindByValue(dt.Rows[0]["BankStatement"].ToString()));

                    txtCnclCq.Text = Convert.ToString(dt.Rows[0]["CnclCq"]);
                    ddlPenyDrpStatus.Text = Convert.ToString(dt.Rows[0]["PenyDrpStatus"]);
                    txtRseMsg.Text = Convert.ToString(dt.Rows[0]["RseMsg"]);
                    ddlAuthority.SelectedIndex = ddlAuthority.Items.IndexOf(ddlAuthority.Items.FindByValue(dt.Rows[0]["Authority"].ToString()));
                    txtBGBName.Text = Convert.ToString(dt.Rows[0]["BGBName"]);
                    txtBGBBrdName.Text = Convert.ToString(dt.Rows[0]["BGBBrdName"]);
                    txtBGBEstDt.Text = Convert.ToString(dt.Rows[0]["BGBEstDt"]);
                    txtBGBHrs.Text = Convert.ToString(dt.Rows[0]["BGBHrs"]);
                    txtBGBVintage.Text = Convert.ToString(dt.Rows[0]["BGBVintage"]);
                    //txtBGBImage.Text = Convert.ToString(dt.Rows[0]["BGBImage"]);
                    txtBGBEmpNo.Text = Convert.ToString(dt.Rows[0]["BGBEmpNo"]);
                    txtBGBLocality.Text = Convert.ToString(dt.Rows[0]["BGBLocality"]);
                    txtBGBHouseNo.Text = Convert.ToString(dt.Rows[0]["BGBHouseNo"]);
                    txtBGBStreet.Text = Convert.ToString(dt.Rows[0]["BGBStreet"]);
                    txtBGBLandmark.Text = Convert.ToString(dt.Rows[0]["BGBLandmark"]);
                    txtBGBArea.Text = Convert.ToString(dt.Rows[0]["BGBArea"]);
                    txtBGBVillage.Text = Convert.ToString(dt.Rows[0]["BGBVillage"]);
                    txtBGBSubDistrict.Text = Convert.ToString(dt.Rows[0]["BGBSubDistrict"]);
                    txtBGBPostOffice.Text = Convert.ToString(dt.Rows[0]["BGBPostOffice"]);
                    txtBGBPincode.Text = Convert.ToString(dt.Rows[0]["BGBPincode"]);

                    ddlBGVState.SelectedIndex = ddlBGVState.Items.IndexOf(ddlBGVState.Items.FindByValue(dt.Rows[0]["BGBState"].ToString()));
                    PopDistrictByState(Convert.ToInt32(ddlBGVState.SelectedValue), "BGV");
                    ddlBGVDist.SelectedIndex = ddlBGVDist.Items.IndexOf(ddlBGVDist.Items.FindByValue(dt.Rows[0]["BGBDist"].ToString()));

                    txtBGBLandline.Text = Convert.ToString(dt.Rows[0]["BGBLandline"]);
                    txtBGBMobile.Text = Convert.ToString(dt.Rows[0]["BGBMobile"]);

                    lblGSTNFileName.Text = Convert.ToString(dt.Rows[0]["PriGSTNFileName"]);
                    lblTradeFileName.Text = Convert.ToString(dt.Rows[0]["PriTradeLcnseFileName"]);
                    lblRegNoFileName.Text = Convert.ToString(dt.Rows[0]["PriRegnNoFileName"]);
                    lblEntityFileNm.Text = Convert.ToString(dt.Rows[0]["PriEntityTypeFileName"]);
                    hdnEntityFileNm.Value = Convert.ToString(dt.Rows[0]["PriEntityTypeFileName"]);
                    lblCPANFileName.Text = Convert.ToString(dt.Rows[0]["PriCPANFileName"]);
                    lblDeedFileName.Text = Convert.ToString(dt.Rows[0]["PriPDeedFileName"]);
                    lblMOAFileName.Text = Convert.ToString(dt.Rows[0]["PriMOAFileName"]);
                    lblAOAFileName.Text = Convert.ToString(dt.Rows[0]["PriAOAFileName"]);
                    lblIFSCFileNm.Text = Convert.ToString(dt.Rows[0]["IFCEFileNm"]);
                    lblBankFileNm.Text = Convert.ToString(dt.Rows[0]["BankStatmntFileNm"]);
                    lblChqFileNm.Text = Convert.ToString(dt.Rows[0]["CnclCqFileNm"]);
                    lblBsnsFileNm.Text = Convert.ToString(dt.Rows[0]["BGBBrdNameFileNm"]);
                    lblBGBFileNm.Text = Convert.ToString(dt.Rows[0]["BGBImageFileNm"]);


                    tbEPCMaster.ActiveTabIndex = 1;
                    StatusButton("Show");

                }
                else
                {
                    ClearControls(this.Page);
                }

                if (dt1.Rows.Count > 0)
                {
                    hdbtnFlag.Value = "Y";
                }
                ViewState["Partner"] = dt1;
                gvPrtnr.DataSource = dt1;
                gvPrtnr.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        #region imgDoc_Click
        protected void imgDoc_Click(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            string CName = btn.CommandName.Trim();
            string FileName = string.Empty;
            string vBase64String = "";

            switch (CName)
            {
                case "GSTN":
                    FileName = lblGSTNFileName.Text.Trim();
                    break;
                case "Trade":
                    FileName = lblTradeFileName.Text.Trim();
                    break;
                case "Reg":
                    FileName = lblRegNoFileName.Text.Trim();
                    break;
                case "Entity":
                    FileName = lblEntityFileNm.Text.Trim();
                    break;
                case "CPAN":
                    FileName = lblCPANFileName.Text.Trim();
                    break;
                case "Deed":
                    FileName = lblDeedFileName.Text.Trim();
                    break;
                case "MOA":
                    FileName = lblMOAFileName.Text.Trim();
                    break;
                case "AOA":
                    FileName = lblAOAFileName.Text.Trim();
                    break;
                case "PAN":
                    FileName = lblPANFileNm.Text.Trim();
                    break;
                case "Aadhar":
                    FileName = lblAadharFileNm.Text.Trim();
                    break;
                case "CHouseNo":
                    FileName = lblHouseNoFileNm.Text.Trim();
                    break;
                case "CStreet":
                    FileName = lblStreetFileNm.Text.Trim();
                    break;
                case "PHouseNO":
                    FileName = lblHnFileNm.Text.Trim();
                    break;
                case "PStreet":
                    FileName = lblStrtFileNm.Text.Trim();
                    break;
                case "IFSC":
                    FileName = lblIFSCFileNm.Text.Trim();
                    break;
                case "BnkStmnt":
                    FileName = lblBankFileNm.Text.Trim();
                    break;
                case "CnclCq":
                    FileName = lblChqFileNm.Text.Trim();
                    break;
                case "BsnsBrd":
                    FileName = lblBsnsFileNm.Text.Trim();
                    break;
                case "BGBImg":
                    FileName = lblBGBFileNm.Text.Trim();
                    break;

            }

            string ActNetImage = "", vPdfFile = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdEPCId.Value + "_" + FileName;
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
                    vBase64String = GetBase64Image(FileName, hdEPCId.Value);
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
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdEPCId.Value + "_" + FileName);
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
        #endregion
        public string GetBase64Image(string pImageName, string pEPCId)
        {
            string ActNetImage = "", base64image = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = DocumentBucketURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + pEPCId + "_" + pImageName;
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
        #region ValidUrlChk
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
        #endregion
        #region Delete
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
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    LoadEPCList(1);
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        [WebMethod]
        public static List<Pin> GetPincode(string Pin)
        {
            DataTable dt = new DataTable();
            CCust360 oCa = new CCust360();
            List<Pin> empResult = new List<Pin>();
            dt = oCa.PopPincode(Pin);
            foreach (DataRow dR in dt.Rows)
            {
                Pin oP = new Pin
                {
                    Pincode = Convert.ToString(dR["Pincode"]),
                    StateId = Convert.ToInt32(dR["StateId"])
                };
                empResult.Add(oP);

            }
            return empResult;
        }

        public class Pin
        {
            public string Pincode { get; set; }
            public int StateId { get; set; }
        }

        [WebMethod]
        public static List<Dist> GetDist(string StateID)
        {
            DataTable dt = new DataTable();
            CUser oMem = new CUser();
            List<Dist> distResult = new List<Dist>();
            dt = oMem.CF_GetDisctrictByState(Convert.ToInt32(StateID));
            foreach (DataRow dR in dt.Rows)
            {
                Dist oP = new Dist
                {
                    DistId = Convert.ToInt32(dR["DistrictId"]),
                    DistName = Convert.ToString(dR["DistrictName"])
                };
                distResult.Add(oP);

            }
            return distResult;
        }

        public class Dist
        {
            public string DistName { get; set; }
            public int DistId { get; set; }
        }

        private void popState()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                dt = oMem.CF_GetState();

                ddlPriState.DataSource = dt;
                ddlPriState.DataTextField = "StateName";
                ddlPriState.DataValueField = "StateId";
                ddlPriState.DataBind();
                ddlPriState.Items.Insert(0, oli1);

                ddlPrtState.DataSource = dt;
                ddlPrtState.DataTextField = "StateName";
                ddlPrtState.DataValueField = "StateId";
                ddlPrtState.DataBind();
                ddlPrtState.Items.Insert(0, oli2);

                ddlPState.DataSource = dt;
                ddlPState.DataTextField = "StateName";
                ddlPState.DataValueField = "StateId";
                ddlPState.DataBind();
                ddlPState.Items.Insert(0, oli3);

                ddlBGVState.DataSource = dt;
                ddlBGVState.DataTextField = "StateName";
                ddlBGVState.DataValueField = "StateId";
                ddlBGVState.DataBind();
                ddlBGVState.Items.Insert(0, oli4);

            }
            finally
            {
            }
        }

        private void PopDistrictByState(int pStateId, string pTag)
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                switch (pTag)
                {
                    case "Pri":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlPriDist.Items.Insert(0, oli1);
                        ddlPriDist.DataSource = dt;
                        ddlPriDist.DataTextField = "DistrictName";
                        ddlPriDist.DataValueField = "DistrictId";
                        ddlPriDist.DataBind();
                        break;
                    case "PrtC":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlPrtDist.Items.Insert(0, oli2);
                        ddlPrtDist.DataSource = dt;
                        ddlPrtDist.DataTextField = "DistrictName";
                        ddlPrtDist.DataValueField = "DistrictId";
                        ddlPrtDist.DataBind();
                        break;
                    case "PrtP":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlPDist.Items.Insert(0, oli3);
                        ddlPDist.DataSource = dt;
                        ddlPDist.DataTextField = "DistrictName";
                        ddlPDist.DataValueField = "DistrictId";
                        ddlPDist.DataBind();
                        break;
                    case "BGV":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlBGVDist.Items.Insert(0, oli4);
                        ddlBGVDist.DataSource = dt;
                        ddlBGVDist.DataTextField = "DistrictName";
                        ddlBGVDist.DataValueField = "DistrictId";
                        ddlBGVDist.DataBind();
                        break;

                }
            }
            finally
            {
            }
        }
        protected void gvPrtnr_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 pLeadID = 0;
            DataSet ds = new DataSet();

            try
            {

                if (hdPriDisbursalRatio.Value != "")
                {
                    ddlPriDisbursalRatio.SelectedValue = hdPriDisbursalRatio.Value;
                }

                switch (e.CommandName)
                {
                    case "ViewData":
                        GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        Label lblSlNo = (Label)row.FindControl("lblSlNo");
                        hdRowId.Value = lblSlNo.Text;
                        hdbtnFlag.Value = "N";
                        ViewGridData();
                        break;
                    case "RowDel":
                        DataTable dt = null;
                        GridViewRow rw = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                        int index = rw.RowIndex;
                        dt = (DataTable)ViewState["Partner"];
                        if (dt.Rows.Count > 1)
                        {
                            dt.Rows[index].Delete();
                            dt.AcceptChanges();
                            ViewState["Partner"] = dt;
                            gvPrtnr.DataSource = dt;
                            gvPrtnr.DataBind();
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("First Row can not be deleted.");
                            return;
                        }
                        break;

                }
                if (hdAcNo.Value != "")
                {
                    txtReACNo.Attributes.Add("value", hdAcNo.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        protected void ViewGridData()
        {
            PopulateDDL();
            UploadFile();
            DataTable dt = null;
            if (ViewState["Partner"] != null)
            {
                dt = (DataTable)ViewState["Partner"];
            }

            if (dt.Rows.Count > 0)
            {
                txtPrtnrFullName.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrFullName"]);
                txtPrtnrAge.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrAge"]);
                txtPrtnrPAN.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPAN"]);
                txtPrtnrAadhar.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrAadhar"]);
                txtPrtnrMobile.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrMobile"]);
                ddlPrtnrRelatnship.SelectedIndex = ddlPrtnrRelatnship.Items.IndexOf(ddlPrtnrRelatnship.Items.FindByValue(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrRelatnship"].ToString()));
                ddlPrtnrActiveSince.SelectedIndex = ddlPrtnrActiveSince.Items.IndexOf(ddlPrtnrActiveSince.Items.FindByValue(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrActiveSince"].ToString()));
                txtPrtnrStateA.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrStateA"]);
                txtPrtnrCHouseNo.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCHouseNo"]);
                txtPrtnrCStreet.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCStreet"]);
                txtPrtnrCLandmark.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCLandmark"]);
                txtPrtnrCArea.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCArea"]);
                txtPrtnrCVillage.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCVillage"]);
                txtPrtnrCSubDistrict.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCSubDistrict"]);
                txtPrtnrCPostOffice.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCPostOffice"]);
                txtPrtnrCPincode1.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCPincode1"]);
                ddlPrtState.SelectedIndex = ddlPrtState.Items.IndexOf(ddlPrtState.Items.FindByValue(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCState"].ToString()));
                PopDistrictByState(Convert.ToInt32(ddlPrtState.SelectedValue), "PrtC");
                ddlPrtDist.SelectedIndex = ddlPrtDist.Items.IndexOf(ddlPrtDist.Items.FindByValue(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCDistrict"].ToString()));
                txtPrtnrClandline.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrClandline"]);
                txtPrtnrPHouseNo.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPHouseNo"]);
                txtPrtnrPStreet.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPStreet"]);
                txtPrtnrPLandmark.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPLandmark"]);
                txtPrtnrPArea.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPArea"]);
                txtPrtnrPVillage.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPVillage"]);
                txtPrtnrPSubDistrict.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPSubDistrict"]);
                txtPrtnrPPostOfc.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPPostOfc"]);
                txtPrtnrPPincode1.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPPincode1"]);
                ddlPState.SelectedIndex = ddlPState.Items.IndexOf(ddlPState.Items.FindByValue(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPState"].ToString()));
                PopDistrictByState(Convert.ToInt32(ddlPState.SelectedValue), "PrtP");
                ddlPDist.SelectedIndex = ddlPDist.Items.IndexOf(ddlPDist.Items.FindByValue(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPDistrict"].ToString()));
                txtPrtnrPLandline.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPLandline"]);
                lblPANFileNm.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPANFileNm"]);
                lblAadharFileNm.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrAadharFileNm"]);
                lblHouseNoFileNm.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCHouseNoFileNm"]);
                lblStreetFileNm.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCStreetFileNm"]);
                lblHnFileNm.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPHouseNoFileNm"]);
                lblStrtFileNm.Text = Convert.ToString(dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPStreetFileNm"]);
            }


        }
        protected void btnAddPrtnr_Click(object sender, EventArgs e)
        {
            LoadDataTable();
            if (hdAcNo.Value != "")
            {
                txtReACNo.Attributes.Add("value", hdAcNo.Value);
            }
        }
        protected void gvPrtnr_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int vTotal = 0, vStake = 0;
            try
            {
                if (gvPrtnr.DataSource != null)
                {
                    if (ViewState["Partner"] != null)
                    {
                        DataTable dt = (DataTable)ViewState["Partner"];
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            vStake = Convert.ToInt32(dt.Rows[i]["PrtnrStateA"]);
                            vTotal += vStake;
                        }

                        if (vTotal != 100)
                        {
                            gblFuction.AjxMsgPopup("Total Stake Value must be equal to 100...");
                            return;
                        }

                    }

                }


            }
            catch (Exception ex)
            { }
        }
        protected void LoadDataTable()
        {
            PopulateDDL();
            UploadFile();

            string Mode = "";
            DataTable dt = null;
            if (ViewState["Partner"] != null)
            {
                dt = (DataTable)ViewState["Partner"];
            }
            else
            {
                dt = new DataTable();

                // Add columns to the DataTable
                dt.Columns.Add("PrtnrId", typeof(int)); // Serial number column
                dt.Columns.Add("EpcID", typeof(string));
                dt.Columns.Add("PrtnrFullName", typeof(string));
                dt.Columns.Add("PrtnrAge", typeof(string));
                //dt.Columns.Add("PrtnrAge", typeof(DateTime));
                dt.Columns.Add("CAddress", typeof(string));
                dt.Columns.Add("PrtnrPAN", typeof(string));
                dt.Columns.Add("PrtnrPANFileNm", typeof(string));
                dt.Columns.Add("PrtnrPANIsUpLd", typeof(string));
                dt.Columns.Add("BytePanDoc", typeof(byte[]));
                dt.Columns.Add("BytePanYN", typeof(string));
                dt.Columns.Add("PrtnrAadhar", typeof(string));
                dt.Columns.Add("PrtnrAadharFileNm", typeof(string));
                dt.Columns.Add("PrtnrAadharIsUpLd", typeof(string));
                dt.Columns.Add("ByteAadharDoc", typeof(byte[]));
                dt.Columns.Add("ByteAadharYN", typeof(string));
                dt.Columns.Add("PrtnrMobile", typeof(string));
                dt.Columns.Add("PrtnrRelatnship", typeof(string));
                dt.Columns.Add("PrtnrActiveSince", typeof(string));
                dt.Columns.Add("PrtnrStateA", typeof(string));
                dt.Columns.Add("PrtnrCHouseNo", typeof(string));
                dt.Columns.Add("PrtnrCHouseNoFileNm", typeof(string));
                dt.Columns.Add("PrtnrCHouseNoIsUpLd", typeof(string));
                dt.Columns.Add("ByteCHouseNoDoc", typeof(byte[]));
                dt.Columns.Add("ByteCHouseNoYN", typeof(string));
                dt.Columns.Add("PrtnrCStreet", typeof(string));
                dt.Columns.Add("PrtnrCStreetFileNm", typeof(string));
                dt.Columns.Add("PrtnrCStreetIsUpLd", typeof(string));
                dt.Columns.Add("ByteCStreetDoc", typeof(byte[]));
                dt.Columns.Add("ByteCStreetYN", typeof(string));
                dt.Columns.Add("PrtnrCLandmark", typeof(string));
                dt.Columns.Add("PrtnrCArea", typeof(string));
                dt.Columns.Add("PrtnrCVillage", typeof(string));
                dt.Columns.Add("PrtnrCSubDistrict", typeof(string));
                dt.Columns.Add("PrtnrCPostOffice", typeof(string));
                dt.Columns.Add("PrtnrCPincode1", typeof(int));
                dt.Columns.Add("PrtnrCDistrict", typeof(int));
                dt.Columns.Add("PrtnrCState", typeof(int));
                dt.Columns.Add("PrtnrClandline", typeof(string));
                dt.Columns.Add("PrtnrPHouseNo", typeof(string));
                dt.Columns.Add("PrtnrPHouseNoFileNm", typeof(string));
                dt.Columns.Add("PrtnrPHouseNoIsUpLd", typeof(string));
                dt.Columns.Add("PrtnrPHouseNoDoc", typeof(byte[]));
                dt.Columns.Add("PrtnrPHouseNoYN", typeof(string));
                dt.Columns.Add("PrtnrPStreet", typeof(string));
                dt.Columns.Add("PrtnrPStreetFileNm", typeof(string));
                dt.Columns.Add("PrtnrPStreetIsUpLd", typeof(string));
                dt.Columns.Add("BytePStreetDoc", typeof(byte[]));
                dt.Columns.Add("BytePStreetYN", typeof(string));
                dt.Columns.Add("PrtnrPLandmark", typeof(string));
                dt.Columns.Add("PrtnrPArea", typeof(string));
                dt.Columns.Add("PrtnrPVillage", typeof(string));
                dt.Columns.Add("PrtnrPSubDistrict", typeof(string));
                dt.Columns.Add("PrtnrPPostOfc", typeof(string));
                dt.Columns.Add("PrtnrPPincode1", typeof(int));
                dt.Columns.Add("PrtnrPDistrict", typeof(int));
                dt.Columns.Add("PrtnrPState", typeof(int));
                dt.Columns.Add("PrtnrPLandline", typeof(string));
                dt.Columns.Add("EntityType", typeof(string));
            }


            string vPrtnrFullName = "", vPrtnrPAN = "", vPrtnrAadhar = "", vPrtnrMobile = "", vPrtnrRelatnship = "",
            vPrtnrActiveSince = "", vPrtnrStateA = "", vPrtnrCHouseNo = "", vPrtnrCStreet = "", vPrtnrCLandmark = "", vPrtnrCArea = "", vPrtnrCVillage = "", vPrtnrCSubDistrict = "",
            vPrtnrCPostOffice = "", vPrtnrClandline = "", vPrtnrPHouseNo = "", vPrtnrPStreet = "",
            vPrtnrPLandmark = "", vPrtnrPArea = "", vPrtnrPVillage = "", vPrtnrPSubDistrict = "", vPrtnrPPostOfc = "", vPrtnrPLandline = "";
            string vBytePanYN = "N", vByteAadharYN = "N", vByteCHouseNoYN = "N", vByteCStreetYN = "N", vPrtnrPHouseNoYN = "N", vBytePStreetYN = "N";

            // START File Upload Variables
            string vPrtPanFileNm = "", vPrtAadharFileNm = "", vPrtCHnFileNm = "", vPrtCStrtFileNm = "", vPrtPHnFileNm = "", vPrtPStrFileNm = "",

            vPrtPanIsFileUpLd = "", vPrtAadharIsFileUpLd = "", vPrtCHnIsFileUpLd = "",
            vPrtCStrtIsFileUpLd = "", vPrtPHnIsFileUpLd = "", vPrtPStrIsFileUpLd = "",

            vPrtPanFileExt = "", vPrtAadharFileExt = "", vPrtCHnFileExt = "",
            vPrtCStrtFileExt = "", vPrtPHnFileExt = "", vPrtPStrFileExt = "";

            vPrtPanFileNm = "PAN"; vPrtAadharFileNm = "Aadhar"; vPrtCHnFileNm = "CHouseNo";
            vPrtCStrtFileNm = "CStreet"; vPrtPHnFileNm = "PHouseNo"; vPrtPStrFileNm = "PStreet";

            byte[] vFuPanFile = null, vFuAadharFile = null, vFuCHnFile = null, vFuCStrtFile = null, vFuPHnFile = null, vFuPStrFile = null;

            //END File Upload Variables

            Int32 vPrtnrPPincode1 = 0, vPrtnrCPincode2 = 0, vPrtnrCPincode1 = 0, vPrtnrPPincode2 = 0,
            vPrtnrCDistrict = 0, vPrtnrCState = 0, vPrtnrPDistrict = 0, vPrtnrPState = 0;

            vPrtnrFullName = txtPrtnrFullName.Text.ToString().Trim(); vPrtnrPAN = txtPrtnrPAN.Text.ToString().Trim(); vPrtnrAadhar = txtPrtnrAadhar.Text.ToString().Trim();
            vPrtnrMobile = txtPrtnrMobile.Text.ToString().Trim(); vPrtnrRelatnship = ddlPrtnrRelatnship.SelectedValue;
            vPrtnrActiveSince = ddlPrtnrActiveSince.SelectedValue; vPrtnrStateA = txtPrtnrStateA.Text.ToString().Trim(); vPrtnrCHouseNo = txtPrtnrCHouseNo.Text.ToString().Trim();
            vPrtnrCStreet = txtPrtnrCStreet.Text.ToString().Trim(); vPrtnrCLandmark = txtPrtnrCLandmark.Text.ToString().Trim(); vPrtnrCArea = txtPrtnrCArea.Text.ToString().Trim();
            vPrtnrCVillage = txtPrtnrCVillage.Text.ToString().Trim(); vPrtnrCSubDistrict = txtPrtnrCSubDistrict.Text.ToString().Trim();
            vPrtnrCPostOffice = txtPrtnrCPostOffice.Text.ToString().Trim();

            if (hdnPrtState.Value == "") hdnPrtState.Value = "-1"; if (hdnPrtDist.Value == "") hdnPrtDist.Value = "-1";
            if (ddlPrtState.SelectedValue == "" || ddlPrtState.SelectedValue == "-1") vPrtnrCState = Convert.ToInt32(hdnPrtState.Value); else vPrtnrCState = Convert.ToInt32(ddlPrtState.SelectedValue);
            if (ddlPrtDist.SelectedValue == "" || ddlPrtDist.SelectedValue == "-1") vPrtnrCDistrict = Convert.ToInt32(hdnPrtDist.Value); else vPrtnrCDistrict = Convert.ToInt32(ddlPrtDist.SelectedValue);

            vPrtnrClandline = txtPrtnrClandline.Text.ToString().Trim();
            vPrtnrPHouseNo = txtPrtnrPHouseNo.Text.ToString().Trim(); vPrtnrPStreet = txtPrtnrPStreet.Text.ToString().Trim();
            vPrtnrPLandmark = txtPrtnrPLandmark.Text.ToString().Trim(); vPrtnrPArea = txtPrtnrPArea.Text.ToString().Trim();
            vPrtnrPVillage = txtPrtnrPVillage.Text.ToString().Trim(); vPrtnrPSubDistrict = txtPrtnrPSubDistrict.Text.ToString().Trim();
            vPrtnrPPostOfc = txtPrtnrPPostOfc.Text.ToString().Trim();

            if (hdnPState.Value == "") hdnPState.Value = "-1"; if (hdnPDist.Value == "") hdnPDist.Value = "-1";
            if (ddlPState.SelectedValue == "" || ddlPState.SelectedValue == "-1") vPrtnrPState = Convert.ToInt32(hdnPState.Value); else vPrtnrPState = Convert.ToInt32(ddlPState.SelectedValue);
            if (ddlPDist.SelectedValue == "" || ddlPDist.SelectedValue == "-1") vPrtnrPDistrict = Convert.ToInt32(hdnPDist.Value); else vPrtnrPDistrict = Convert.ToInt32(ddlPDist.SelectedValue);

            vPrtnrPLandline = txtPrtnrPLandline.Text.ToString().Trim();


            string vPrtnrAge = txtPrtnrAge.Text.ToString();

            //DateTime vPrtnrAge = gblFuction.setDate(txtPrtnrAge.Text);


            if (txtPrtnrPPincode1.Text.Trim() != "") vPrtnrPPincode1 = Convert.ToInt32(txtPrtnrPPincode1.Text);
            if (txtPrtnrCPincode2.Text.Trim() != "") vPrtnrCPincode2 = 0;
            if (txtPrtnrCPincode1.Text.Trim() != "") vPrtnrCPincode1 = Convert.ToInt32(txtPrtnrCPincode1.Text);
            if (txtPrtnrPPincode2.Text.Trim() != "") vPrtnrPPincode2 = 0;

            string CAddress = vPrtnrCHouseNo + ", " + vPrtnrCStreet + ", " + vPrtnrCArea + ", " + vPrtnrCVillage + ", " + vPrtnrCLandmark + " ," + vPrtnrCPostOffice + ", " + Convert.ToString(vPrtnrCPincode1);

            #region FileUpload_PartnerDetails

            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Edit") Mode = "Edit";
            else Mode = "Save";

            //START FOR PAN
            vPrtPanIsFileUpLd = fuPANUpld.HasFile == true ? "Y" : "N";
            if (vPrtPanIsFileUpLd == "Y")
            {
                vFuPanFile = new byte[fuPANUpld.PostedFile.InputStream.Length + 1];
                fuPANUpld.PostedFile.InputStream.Read(vFuPanFile, 0, vFuPanFile.Length);
                vPrtPanFileExt = System.IO.Path.GetExtension(fuPANUpld.FileName).ToLower();
                //---------------------------------------------------------------
                bool ValidPdf = false;
                if (vPrtPanFileExt.ToLower() == ".pdf")
                {
                    cFileValidate oFile = new cFileValidate();
                    ValidPdf = oFile.ValidatePdf(fuPANUpld.FileBytes);
                    if (ValidPdf == false)
                    {
                        gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                        fuPANUpld.Focus();
                        return ;
                    }
                }
                //------------------------------------------------------------
                if ((vPrtPanFileExt.ToLower() != ".pdf") && (vPrtPanFileExt.ToLower() != ".xlx") && (vPrtPanFileExt.ToLower() != ".xlsx"))
                {
                    vPrtPanFileExt = ".png";
                }
                vPrtPanFileNm = vPrtPanFileNm + vPrtPanFileExt;
                vBytePanYN = "Y";
                ViewState["BytePanYN"] = vBytePanYN;
                ViewState["PrtPanFileNm"] = vPrtPanFileNm;
                ViewState["BytePanDoc"] = vFuPanFile;

            }
            else
            {
                if (Mode == "Edit")
                {
                    if (lblPANFileNm.Text != "")
                    {
                        vPrtPanIsFileUpLd = "Y";
                        vPrtPanFileNm = lblPANFileNm.Text;
                    }
                    else
                    {
                        if ((string)ViewState["BytePanYN"] != "Y")
                        {
                            gblFuction.AjxMsgPopup("Please Upload PAN File...");
                            return;
                        }

                    }

                }
                else if ((string)ViewState["BytePanYN"] != "Y")
                {
                    gblFuction.AjxMsgPopup("Please Upload PAN File...");
                    return;
                }
            }
            // END For PAN

            //START FOR Aadhar
            vPrtAadharIsFileUpLd = fuAadharUpld.HasFile == true ? "Y" : "N";
            if (vPrtAadharIsFileUpLd == "Y")
            {
                vFuAadharFile = new byte[fuAadharUpld.PostedFile.InputStream.Length + 1];
                fuAadharUpld.PostedFile.InputStream.Read(vFuAadharFile, 0, vFuAadharFile.Length);
                vPrtAadharFileExt = System.IO.Path.GetExtension(fuAadharUpld.FileName).ToLower();
                //---------------------------------------------------------------
                bool ValidPdf = false;
                if (vPrtAadharFileExt.ToLower() == ".pdf")
                {
                    cFileValidate oFile = new cFileValidate();
                    ValidPdf = oFile.ValidatePdf(fuAadharUpld.FileBytes);
                    if (ValidPdf == false)
                    {
                        gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                        fuAadharUpld.Focus();
                        return ;
                    }
                }
                //------------------------------------------------------------
                if ((vPrtAadharFileExt.ToLower() != ".pdf") && (vPrtAadharFileExt.ToLower() != ".xlx") && (vPrtAadharFileExt.ToLower() != ".xlsx"))
                {
                    vPrtAadharFileExt = ".png";
                }
                vPrtAadharFileNm = vPrtAadharFileNm + vPrtAadharFileExt;
                vByteAadharYN = "Y";

                ViewState["ByteAadharYN"] = vByteAadharYN;
                ViewState["PrtAadharFileNm"] = vPrtAadharFileNm;
                ViewState["ByteAadharDoc"] = vFuAadharFile;
            }
            else
            {

                if (Mode == "Edit")
                {
                    if (lblAadharFileNm.Text != "")
                    {
                        vPrtAadharIsFileUpLd = "Y";
                        vPrtAadharFileNm = lblAadharFileNm.Text;
                    }
                    else
                    {
                        if ((string)ViewState["ByteAadharYN"] != "Y")
                        {
                            gblFuction.AjxMsgPopup("Please Upload Aadhaar File...");
                            return;
                        }
                    }
                }
                else if ((string)ViewState["ByteAadharYN"] != "Y")
                {
                    gblFuction.AjxMsgPopup("Please Upload Aadhaar File...");
                    return;
                }
            }
            // END For aadhar

            //START FOR CHouseNo
            vPrtCHnIsFileUpLd = fuCHouseNoUpld.HasFile == true ? "Y" : "N";
            if (vPrtCHnIsFileUpLd == "Y")
            {
                vFuCHnFile = new byte[fuCHouseNoUpld.PostedFile.InputStream.Length + 1];
                fuCHouseNoUpld.PostedFile.InputStream.Read(vFuCHnFile, 0, vFuCHnFile.Length);
                vPrtCHnFileExt = System.IO.Path.GetExtension(fuCHouseNoUpld.FileName).ToLower();
                //---------------------------------------------------------------
                bool ValidPdf = false;
                if (vPrtCHnFileExt.ToLower() == ".pdf")
                {
                    cFileValidate oFile = new cFileValidate();
                    ValidPdf = oFile.ValidatePdf(fuCHouseNoUpld.FileBytes);
                    if (ValidPdf == false)
                    {
                        gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                        fuCHouseNoUpld.Focus();
                        return ;
                    }
                }
                //------------------------------------------------------------
                if ((vPrtCHnFileExt.ToLower() != ".pdf") && (vPrtCHnFileExt.ToLower() != ".xlx") && (vPrtCHnFileExt.ToLower() != ".xlsx"))
                {
                    vPrtCHnFileExt = ".png";
                }
                vPrtCHnFileNm = vPrtCHnFileNm + vPrtCHnFileExt;
                vByteCHouseNoYN = "Y";

                ViewState["ByteCHnYN"] = vByteCHouseNoYN;
                ViewState["PrtCHnFileNm"] = vPrtCHnFileNm;
                ViewState["ByteCHnDoc"] = vFuCHnFile;
            }
            else
            {

                if (Mode == "Edit")
                {
                    if (lblHouseNoFileNm.Text != "")
                    {
                        vPrtCHnIsFileUpLd = "Y";
                        vPrtCHnFileNm = lblHouseNoFileNm.Text;
                    }
                    else
                    {
                        if ((string)ViewState["ByteCHnYN"] != "Y")
                        {
                            gblFuction.AjxMsgPopup("Please Upload Correspondence Address File...");
                            return;
                        }
                    }
                }
                else if ((string)ViewState["ByteCHnYN"] != "Y")
                {
                    gblFuction.AjxMsgPopup("Please Upload Correspondence Address File...");
                    return;
                }
            }
            // END For CHouseNo


            //START FOR PHouseNo
            vPrtPHnIsFileUpLd = fuPHNUpld.HasFile == true ? "Y" : "N";
            if (vPrtPHnIsFileUpLd == "Y")
            {
                vFuPHnFile = new byte[fuPHNUpld.PostedFile.InputStream.Length + 1];
                fuPHNUpld.PostedFile.InputStream.Read(vFuPHnFile, 0, vFuPHnFile.Length);
                vPrtPHnFileExt = System.IO.Path.GetExtension(fuPHNUpld.FileName).ToLower();
                //---------------------------------------------------------------
                bool ValidPdf = false;
                if (vPrtPHnFileExt.ToLower() == ".pdf")
                {
                    cFileValidate oFile = new cFileValidate();
                    ValidPdf = oFile.ValidatePdf(fuPHNUpld.FileBytes);
                    if (ValidPdf == false)
                    {
                        gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                        fuPHNUpld.Focus();
                        return ;
                    }
                }
                //------------------------------------------------------------
                if ((vPrtPHnFileExt.ToLower() != ".pdf") && (vPrtPHnFileExt.ToLower() != ".xlx") && (vPrtPHnFileExt.ToLower() != ".xlsx"))
                {
                    vPrtPHnFileExt = ".png";
                }
                vPrtPHnFileNm = vPrtPHnFileNm + vPrtPHnFileExt;
                vPrtnrPHouseNoYN = "Y";

                ViewState["BytePHnYN"] = vPrtnrPHouseNoYN;
                ViewState["PrtPHnFileNm"] = vPrtPHnFileNm;
                ViewState["BytePHnDoc"] = vFuPHnFile;
            }
            else
            {

                if (Mode == "Edit")
                {
                    if (lblHnFileNm.Text != "")
                    {
                        vPrtPHnIsFileUpLd = "Y";
                        vPrtPHnFileNm = lblHnFileNm.Text;
                    }
                    else
                    {
                        if ((string)ViewState["BytePHnYN"] != "Y")
                        {
                            gblFuction.AjxMsgPopup("Please Upload Permanent Address File...");
                            return;
                        }
                    }
                }
                else if ((string)ViewState["BytePHnYN"] != "Y")
                {
                    gblFuction.AjxMsgPopup("Please Upload Permanent Address File...");
                    return;
                }
            }
            // END For PHouseNo

            #endregion

            if (ViewState["BytePanYN"] != null)
            {
                vBytePanYN = (string)ViewState["BytePanYN"];
            }
            if (ViewState["PrtPanFileNm"] != null)
            {
                vPrtPanFileNm = (string)ViewState["PrtPanFileNm"];
            }
            if (ViewState["BytePanDoc"] != null)
            {
                vFuPanFile = (byte[])ViewState["BytePanDoc"];
            }

            if (ViewState["ByteAadharYN"] != null)
            {
                vByteAadharYN = (string)ViewState["ByteAadharYN"];
            }
            if (ViewState["PrtAadharFileNm"] != null)
            {
                vPrtAadharFileNm = (string)ViewState["PrtAadharFileNm"];
            }
            if (ViewState["ByteAadharDoc"] != null)
            {
                vFuAadharFile = (byte[])ViewState["ByteAadharDoc"];
            }

            if (ViewState["ByteCHnYN"] != null)
            {
                vByteCHouseNoYN = (string)ViewState["ByteCHnYN"];
            }
            if (ViewState["PrtCHnFileNm"] != null)
            {
                vPrtCHnFileNm = (string)ViewState["PrtCHnFileNm"];
            }
            if (ViewState["ByteCHnDoc"] != null)
            {
                vFuCHnFile = (byte[])ViewState["ByteCHnDoc"];
            }

            if (ViewState["BytePHnYN"] != null)
            {
                vPrtnrPHouseNoYN = (string)ViewState["BytePHnYN"];
            }
            if (ViewState["PrtPHnFileNm"] != null)
            {
                vPrtPHnFileNm = (string)ViewState["PrtPHnFileNm"];
            }
            if (ViewState["BytePHnDoc"] != null)
            {
                vFuPHnFile = (byte[])ViewState["BytePHnDoc"];
            }

            // START NOT REQUIRED
            if (ViewState["ByteCStrtYN"] != null)
            {
                vBytePanYN = (string)ViewState["ByteCStrtYN"];
            }
            if (ViewState["PrtCStrtFileNm"] != null)
            {
                vPrtCStrtFileNm = (string)ViewState["PrtCStrtFileNm"];
            }
            if (ViewState["ByteCStrtDoc"] != null)
            {
                vFuPanFile = (byte[])ViewState["ByteCStrtDoc"];
            }

            if (ViewState["BytePStreetYN"] != null)
            {
                vBytePanYN = (string)ViewState["BytePanYN"];
            }
            if (ViewState["PrtPStreetFileNm"] != null)
            {
                vPrtPStrFileNm = (string)ViewState["PrtPStreetFileNm"];
            }
            if (ViewState["BytePStreetDoc"] != null)
            {
                vFuPanFile = (byte[])ViewState["BytePStreetDoc"];
            }
            //END NOT REQUIRED


            if (hdRowId.Value == "0")
            {
                DataRow row = dt.NewRow();
                row["PrtnrId"] = dt.Rows.Count + 1;
                row["EpcID"] = "0";
                row["PrtnrFullName"] = vPrtnrFullName;
                row["PrtnrAge"] = vPrtnrAge;
                row["CAddress"] = CAddress;
                row["PrtnrPAN"] = vPrtnrPAN;
                row["PrtnrPANFileNm"] = vPrtPanFileNm;
                row["PrtnrPANIsUpLd"] = vPrtPanIsFileUpLd;
                row["BytePanDoc"] = vFuPanFile;
                row["BytePanYN"] = vBytePanYN;
                row["PrtnrAadhar"] = vPrtnrAadhar;
                row["PrtnrAadharFileNm"] = vPrtAadharFileNm;
                row["PrtnrAadharIsUpLd"] = vPrtAadharIsFileUpLd;
                row["ByteAadharDoc"] = vFuAadharFile;
                row["ByteAadharYN"] = vByteAadharYN;
                row["PrtnrMobile"] = vPrtnrMobile;
                row["PrtnrRelatnship"] = vPrtnrRelatnship;
                row["PrtnrActiveSince"] = vPrtnrActiveSince;
                row["PrtnrStateA"] = vPrtnrStateA;
                row["PrtnrCHouseNo"] = vPrtnrCHouseNo;
                row["PrtnrCHouseNoFileNm"] = vPrtCHnFileNm;
                row["PrtnrCHouseNoIsUpLd"] = vPrtCHnIsFileUpLd;
                row["ByteCHouseNoDoc"] = vFuCHnFile;
                row["ByteCHouseNoYN"] = vByteCHouseNoYN;
                row["PrtnrCStreet"] = vPrtnrCStreet;
                row["PrtnrCStreetFileNm"] = vPrtCStrtFileNm;
                row["PrtnrCStreetIsUpLd"] = vPrtCStrtIsFileUpLd;
                row["ByteCStreetDoc"] = vFuCStrtFile;
                row["ByteCStreetYN"] = vByteCStreetYN;
                row["PrtnrCLandmark"] = vPrtnrCLandmark;
                row["PrtnrCArea"] = vPrtnrCArea;
                row["PrtnrCVillage"] = vPrtnrCVillage;
                row["PrtnrCSubDistrict"] = vPrtnrCSubDistrict;
                row["PrtnrCPostOffice"] = vPrtnrCPostOffice;
                row["PrtnrCPincode1"] = vPrtnrCPincode1;
                row["PrtnrCDistrict"] = vPrtnrCDistrict;
                row["PrtnrCState"] = vPrtnrCState;
                row["PrtnrClandline"] = vPrtnrClandline;
                row["PrtnrPHouseNo"] = vPrtnrPHouseNo;
                row["PrtnrPHouseNoFileNm"] = vPrtPHnFileNm;
                row["PrtnrPHouseNoIsUpLd"] = vPrtPHnIsFileUpLd;
                row["PrtnrPHouseNoDoc"] = vFuPHnFile;
                row["PrtnrPHouseNoYN"] = vPrtnrPHouseNoYN;
                row["PrtnrPStreet"] = vPrtnrPStreet;
                row["PrtnrPStreetFileNm"] = vPrtPStrFileNm;
                row["PrtnrPStreetIsUpLd"] = vPrtPStrIsFileUpLd;
                row["BytePStreetDoc"] = vFuPStrFile;
                row["BytePStreetYN"] = vBytePStreetYN;
                row["PrtnrPLandmark"] = vPrtnrPLandmark;
                row["PrtnrPArea"] = vPrtnrPArea;
                row["PrtnrPVillage"] = vPrtnrPVillage;
                row["PrtnrPSubDistrict"] = vPrtnrPSubDistrict;
                row["PrtnrPPostOfc"] = vPrtnrPPostOfc;
                row["PrtnrPPincode1"] = vPrtnrPPincode1;
                row["PrtnrPDistrict"] = vPrtnrPDistrict;
                row["PrtnrPState"] = vPrtnrPState;
                row["PrtnrPLandline"] = vPrtnrPLandline;
                row["EntityType"] = "I";
                dt.Rows.Add(row);
                dt.AcceptChanges();
            }
            else
            {
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrId"] = hdRowId.Value;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["EpcID"] = "0";
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrFullName"] = vPrtnrFullName;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrAge"] = vPrtnrAge;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["CAddress"] = CAddress;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPAN"] = vPrtnrPAN;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPANFileNm"] = vPrtPanFileNm;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPANIsUpLd"] = "Y";
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["BytePanDoc"] = vFuPanFile;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["BytePanYN"] = vBytePanYN;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrAadhar"] = vPrtnrAadhar;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrAadharFileNm"] = vPrtAadharFileNm;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrAadharIsUpLd"] = "Y";
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["ByteAadharDoc"] = vFuAadharFile;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["ByteAadharYN"] = vByteAadharYN;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrMobile"] = vPrtnrMobile;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrRelatnship"] = vPrtnrRelatnship;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrActiveSince"] = vPrtnrActiveSince;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrStateA"] = vPrtnrStateA;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCHouseNo"] = vPrtnrCHouseNo;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCHouseNoFileNm"] = vPrtCHnFileNm;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCHouseNoIsUpLd"] = "Y";
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["ByteCHouseNoDoc"] = vFuCHnFile;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["ByteCHouseNoYN"] = vByteCHouseNoYN;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCStreet"] = vPrtnrCStreet;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCStreetFileNm"] = vPrtCStrtFileNm;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCStreetIsUpLd"] = vPrtCStrtIsFileUpLd;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["ByteCStreetDoc"] = vFuCStrtFile;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["ByteCStreetYN"] = vByteCStreetYN;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCLandmark"] = vPrtnrCLandmark;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCArea"] = vPrtnrCArea;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCVillage"] = vPrtnrCVillage;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCSubDistrict"] = vPrtnrCSubDistrict;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCPostOffice"] = vPrtnrCPostOffice;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCPincode1"] = vPrtnrCPincode1;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCDistrict"] = vPrtnrCDistrict;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrCState"] = vPrtnrCState;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrClandline"] = vPrtnrClandline;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPHouseNo"] = vPrtnrPHouseNo;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPHouseNoFileNm"] = vPrtPHnFileNm;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPHouseNoIsUpLd"] = "Y";
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPHouseNoDoc"] = vFuPHnFile;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPHouseNoYN"] = vPrtnrPHouseNoYN;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPStreet"] = vPrtnrPStreet;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPStreetFileNm"] = vPrtPStrFileNm;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPStreetIsUpLd"] = "Y";
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["BytePStreetDoc"] = vFuPStrFile;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["BytePStreetYN"] = vBytePStreetYN;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPLandmark"] = vPrtnrPLandmark;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPArea"] = vPrtnrPArea;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPVillage"] = vPrtnrPVillage;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPSubDistrict"] = vPrtnrPSubDistrict;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPPostOfc"] = vPrtnrPPostOfc;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPPincode1"] = vPrtnrPPincode1;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPDistrict"] = vPrtnrPDistrict;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPState"] = vPrtnrPState;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["PrtnrPLandline"] = vPrtnrPLandline;
                dt.Rows[Convert.ToInt32(hdRowId.Value) - 1]["EntityType"] = "I";

                hdRowId.Value = "0";
            }


            ViewState["Partner"] = dt;


            txtPrtnrFullName.Text = "";
            txtPrtnrAge.Text = "";
            txtPrtnrPAN.Text = "";
            txtPrtnrAadhar.Text = "";
            txtPrtnrMobile.Text = "";

            ddlPrtnrRelatnship.SelectedIndex = -1;
            ddlPrtnrActiveSince.SelectedValue = "-1";
            txtPrtnrStateA.Text = "";
            txtPrtnrStateA.Text = "";
            txtPrtnrCHouseNo.Text = "";
            txtPrtnrCStreet.Text = "";
            txtPrtnrCLandmark.Text = "";
            txtPrtnrCArea.Text = "";
            txtPrtnrCVillage.Text = "";
            txtPrtnrCSubDistrict.Text = "";
            txtPrtnrCPostOffice.Text = "";
            txtPrtnrClandline.Text = "";
            txtPrtnrPHouseNo.Text = "";
            txtPrtnrPStreet.Text = "";
            txtPrtnrPLandmark.Text = "";
            txtPrtnrPArea.Text = "";
            txtPrtnrPVillage.Text = "";
            txtPrtnrPSubDistrict.Text = "";
            txtPrtnrPPostOfc.Text = "";
            ddlPState.SelectedValue = "-1";
            ddlPDist.SelectedValue = "-1";
            ddlPrtState.SelectedValue = "-1";
            ddlPrtDist.SelectedValue = "-1";
            txtPrtnrPPincode1.Text = "";
            txtPrtnrCPincode2.Text = "";
            txtPrtnrCPincode1.Text = "";
            txtPrtnrPPincode2.Text = "";
            txtPrtnrPLandline.Text = "";

            lblPANFileNm.Text = "";
            lblAadharFileNm.Text = "";
            lblHouseNoFileNm.Text = "";
            lblStreetFileNm.Text = "";
            lblHnFileNm.Text = "";
            lblStrtFileNm.Text = "";

            Int32 vStake = 0, vTotal = 0;

            gvPrtnr.DataSource = dt;
            gvPrtnr.DataBind();

            if (ViewState["Partner"] != null)
            {
                DataTable dtP = (DataTable)ViewState["Partner"];
                for (int i = 0; i <= dtP.Rows.Count - 1; i++)
                {
                    vStake = Convert.ToInt32(dtP.Rows[i]["PrtnrStateA"]);
                    vTotal += vStake;
                }

                if (vTotal != 100)
                {
                    gblFuction.AjxMsgPopup("Total Stake Value must be equal to 100...");
                    return;
                }

            }

            if (hdPriDisbursalRatio.Value != "")
            {
                ddlPriDisbursalRatio.SelectedValue = hdPriDisbursalRatio.Value;
            }
        }
        protected void lbRseMsg_Click(object sender, EventArgs e)
        {
            string vMsg = "";
            vMsg = PennyDropService();
            txtRseMsg.Text = vMsg;
            if (hdPriDisbursalRatio.Value != "")
            {
                ddlPriDisbursalRatio.SelectedValue = hdPriDisbursalRatio.Value;
            }
            txtACNo.Attributes.Add("value", Convert.ToString(hdAcNo.Value));
            txtReACNo.Attributes.Add("value", Convert.ToString(hdAcNo.Value));
            gblFuction.AjxMsgPopup(vMsg);
        }
        protected string PennyDropService()
        {
            CMember oMem = new CMember(); string vMsg = ""; Int32 UID = 0;
            UID = Convert.ToInt32(Session[gblValue.UserId]);
            if (hdAcNo.Value.ToString().Trim() == "")
            {
                vMsg = "Bank Account can not be left blank.";
                ddlPenyDrpStatus.SelectedValue = "F";
                return vMsg;

            }
            if (txtIFCE.Text.Trim() == "")
            {
                vMsg = "IFSC Code can not be left blank.";
                ddlPenyDrpStatus.SelectedValue = "F";
                return vMsg;

            }
            int vErr = oMem.ChkEpcIFSC(hdAcNo.Value.Trim(), txtIFCE.Text.Trim());
            if (vErr == 99)
            {
                vMsg = "Penny Drop is Already Initiated...";
                return vMsg;
            }
            else if (vErr == 9999)
            {
                vMsg = "Invalid IFSC Code.";
                ddlPenyDrpStatus.SelectedValue = "F";
                return vMsg;
            }
            else
            {
                Int32 Status = 1;
                wsPennyDropService pd = new wsPennyDropService();
                vMsg = pd.PennyDropStatus(hdAcNo.Value, txtIFCE.Text, UID, "", "EPC", out Status);
                if (Status == 0)
                {
                    ddlPenyDrpStatus.SelectedValue = "S";
                }
                else
                {
                    ddlPenyDrpStatus.SelectedValue = "F";
                }
            }
            return vMsg;


        }
        protected void UploadFile()
        {
            #region FileUpload_PrimaryDetails


            string vPriEntityIsFileUpLd = "", vPriMOAIsFileUpLd = "", vPriAOAIsFileUpLd = "", vPriDeedIsFileUpLd = "", vPriCPANIsFileUpLd = "",
             vPriGSTNIsFileUpLd = "", vPriRegnNoIsFileUpLd = "", vPriTradeIsFileUpLd = "",


             vBSIsFileUpLd = "", vCheqIsFileUpLd = "", vBBIsFileUpLd = "",

              vBGBFileExt = "", vIFSCIsFileUpLd = "";

            string vPriGSTNFileExt = "", vPriTradeFileExt = "", vPriRegNoFileExt = "", vPriDeedFileExt = "", vPriCPANFileExt = "",
                   vPriMOAFileExt = "", vPriAOAFileExt = "",
                   vPriEntityFileExt = "", vBGBImage = "N",
                   vBSFileExt = "", vCheqFileExt = "", vBBFileExt = "";

            string vGSTNByteYN = "N", vTradeByteYN = "N", vRegnByteYN = "N", vCPANByteYN = "N", vETypeByteYN = "N", vDeedByteYN = "N",
            vMOAByteYN = "N", vAOAByteYN = "N", vBnkSByteYN = "N", vCChqFByteYN = "N", vBNBByteYN = "N", vBImgByteYN = "N";

            byte[] vFuGSTN = null, vFuTrade = null, vFuRegn = null, vFuCPAN = null, vFuEType = null, vFuDeed = null,
            vFuMOA = null, vFuAOA = null, vFuBnkS = null, vFuCChq = null, vFuBNB = null, vFuBImg = null;

            string vPriGSTNFileName = "GSTN", vPriTradeFileName = "Trade", vPriRegnNoFileName = "Reg",
            vPriCPANFileName = "CPAN", vPriDeedFileName = "Deed", vPriMOAFileName = "MOA", vPriAOAFileName = "AOA",
            vPriEntityFileNm = "Entity",
            vIFSCFileNm = "IFSC", vBSFileNm = "BnkStmnt", vCheqFileNm = "CnclCq", vBBFileNm = "BusinessBoard", vBGBImageFileNm = "BusinessImage";

            //START FOR GSTN Upload
            vPriGSTNIsFileUpLd = fuGSTNUpld.HasFile == true ? "Y" : "N";
            if (vPriGSTNIsFileUpLd == "Y")
            {
                vFuGSTN = new byte[fuGSTNUpld.PostedFile.InputStream.Length + 1];
                fuGSTNUpld.PostedFile.InputStream.Read(vFuGSTN, 0, vFuGSTN.Length);
                vPriGSTNFileExt = System.IO.Path.GetExtension(fuGSTNUpld.FileName).ToLower();
                //---------------------------------------------------------------
                bool ValidPdf = false;
                if (vPriGSTNFileExt.ToLower() == ".pdf")
                {
                    cFileValidate oFile = new cFileValidate();
                    ValidPdf = oFile.ValidatePdf(fuGSTNUpld.FileBytes);
                    if (ValidPdf == false)
                    {
                        gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                        fuGSTNUpld.Focus();
                        return;
                    }
                }
                //------------------------------------------------------------
                if ((vPriGSTNFileExt.ToLower() != ".pdf") && (vPriGSTNFileExt.ToLower() != ".xlx") && (vPriGSTNFileExt.ToLower() != ".xlsx"))
                {
                    vPriGSTNFileExt = ".png";
                }
                vPriGSTNFileName = vPriGSTNFileName + vPriGSTNFileExt;
                vGSTNByteYN = "Y";

                ViewState["GSTNFileNm"] = vPriGSTNFileName;
                ViewState["GSTNIsUpLd"] = "Y";
                ViewState["GSTNByteDoc"] = vFuGSTN;
                ViewState["GSTNByteYN"] = vGSTNByteYN;

            }

            // END For GSTN

            //START FOR TRADE Upload
            vPriTradeIsFileUpLd = fuTradeUpld.HasFile == true ? "Y" : "N";
            if (vPriTradeIsFileUpLd == "Y")
            {


                vFuTrade = new byte[fuTradeUpld.PostedFile.InputStream.Length + 1];
                fuTradeUpld.PostedFile.InputStream.Read(vFuTrade, 0, vFuTrade.Length);
                vPriTradeFileExt = System.IO.Path.GetExtension(fuTradeUpld.FileName).ToLower();
                //---------------------------------------------------------------
                bool ValidPdf = false;
                if (vPriTradeFileExt.ToLower() == ".pdf")
                {
                    cFileValidate oFile = new cFileValidate();
                    ValidPdf = oFile.ValidatePdf(fuTradeUpld.FileBytes);
                    if (ValidPdf == false)
                    {
                        gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                        fuTradeUpld.Focus();
                        return ;
                    }
                }
                //------------------------------------------------------------
                if ((vPriTradeFileExt.ToLower() != ".pdf") && (vPriTradeFileExt.ToLower() != ".xlx") && (vPriTradeFileExt.ToLower() != ".xlsx"))
                {
                    vPriTradeFileExt = ".png";
                }
                vPriTradeFileName = vPriTradeFileName + vPriTradeFileExt;
                vTradeByteYN = "Y";

                ViewState["TradeFileNm"] = vPriTradeFileName;
                ViewState["TradeIsUpLd"] = "Y";
                ViewState["TradeByteDoc"] = vFuTrade;
                ViewState["TradeByteYN"] = vTradeByteYN;

            }

            // END For TRADE

            //START FOR Reg Upload
            vPriRegnNoIsFileUpLd = fuRegNoUpld.HasFile == true ? "Y" : "N";
            if (vPriRegnNoIsFileUpLd == "Y")
            {

                vFuRegn = new byte[fuRegNoUpld.PostedFile.InputStream.Length + 1];
                fuRegNoUpld.PostedFile.InputStream.Read(vFuRegn, 0, vFuRegn.Length);
                vPriRegNoFileExt = System.IO.Path.GetExtension(fuRegNoUpld.FileName).ToLower();
                //---------------------------------------------------------------
                bool ValidPdf = false;
                if (vPriRegNoFileExt.ToLower() == ".pdf")
                {
                    cFileValidate oFile = new cFileValidate();
                    ValidPdf = oFile.ValidatePdf(fuRegNoUpld.FileBytes);
                    if (ValidPdf == false)
                    {
                        gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                        fuRegNoUpld.Focus();
                        return ;
                    }
                }
                //------------------------------------------------------------
                if ((vPriRegNoFileExt.ToLower() != ".pdf") && (vPriRegNoFileExt.ToLower() != ".xlx") && (vPriRegNoFileExt.ToLower() != ".xlsx"))
                {
                    vPriRegNoFileExt = ".png";
                }
                vPriRegnNoFileName = vPriRegnNoFileName + vPriRegNoFileExt;
                vRegnByteYN = "Y";

                ViewState["RegnFileNm"] = vPriRegnNoFileName;
                ViewState["RegnIsUpLd"] = "Y";
                ViewState["RegnByteDoc"] = vFuRegn;
                ViewState["RegnByteYN"] = vRegnByteYN;

            }

            // END For Reg




            //START FOR CPAN Upload
            vPriCPANIsFileUpLd = fuCPANUpld.HasFile == true ? "Y" : "N";
            if (vPriCPANIsFileUpLd == "Y")
            {

                vFuCPAN = new byte[fuCPANUpld.PostedFile.InputStream.Length + 1];
                fuCPANUpld.PostedFile.InputStream.Read(vFuCPAN, 0, vFuCPAN.Length);
                vPriCPANFileExt = System.IO.Path.GetExtension(fuCPANUpld.FileName).ToLower();
                //---------------------------------------------------------------
                bool ValidPdf = false;
                if (vPriCPANFileExt.ToLower() == ".pdf")
                {
                    cFileValidate oFile = new cFileValidate();
                    ValidPdf = oFile.ValidatePdf(fuCPANUpld.FileBytes);
                    if (ValidPdf == false)
                    {
                        gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                        fuCPANUpld.Focus();
                        return ;
                    }
                }
                //------------------------------------------------------------
                if ((vPriCPANFileExt.ToLower() != ".pdf") && (vPriCPANFileExt.ToLower() != ".xlx") && (vPriCPANFileExt.ToLower() != ".xlsx"))
                {
                    vPriCPANFileExt = ".png";
                }
                vPriCPANFileName = vPriCPANFileName + vPriCPANFileExt;
                vCPANByteYN = "Y";

                ViewState["CPANFileNm"] = vPriCPANFileName;
                ViewState["CPANIsUpLd"] = "Y";
                ViewState["CPANByteDoc"] = vFuCPAN;
                ViewState["CPANByteYN"] = vCPANByteYN;

            }

            // END For CPAN

            //START FOR Entity Upload
            vPriEntityIsFileUpLd = fuEntityUpLd.HasFile == true ? "Y" : "N";
            if (vPriEntityIsFileUpLd == "Y")
            {

                vFuEType = new byte[fuEntityUpLd.PostedFile.InputStream.Length + 1];
                fuEntityUpLd.PostedFile.InputStream.Read(vFuEType, 0, vFuEType.Length);
                vPriEntityFileExt = System.IO.Path.GetExtension(fuEntityUpLd.FileName).ToLower();
                //---------------------------------------------------------------
                bool ValidPdf = false;
                if (vPriEntityFileExt.ToLower() == ".pdf")
                {
                    cFileValidate oFile = new cFileValidate();
                    ValidPdf = oFile.ValidatePdf(fuEntityUpLd.FileBytes);
                    if (ValidPdf == false)
                    {
                        gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                        fuEntityUpLd.Focus();
                        return ;
                    }
                }
                //------------------------------------------------------------
                if ((vPriEntityFileExt.ToLower() != ".pdf") && (vPriEntityFileExt.ToLower() != ".xlx") && (vPriEntityFileExt.ToLower() != ".xlsx"))
                {
                    vPriEntityFileExt = ".png";
                }
                vPriEntityFileNm = vPriEntityFileNm + vPriEntityFileExt;
                vETypeByteYN = "Y";

                ViewState["ETypeFileNm"] = vPriEntityFileNm;
                ViewState["ETypeIsUpLd"] = "Y";
                ViewState["ETypeByteDoc"] = vFuEType;
                ViewState["ETypeByteYN"] = vETypeByteYN;

            }

            // END For Entity

            //START FOR DeeD Upload
            vPriDeedIsFileUpLd = fuDeedUpld.HasFile == true ? "Y" : "N";
            if (vPriDeedIsFileUpLd == "Y")
            {

                vFuDeed = new byte[fuDeedUpld.PostedFile.InputStream.Length + 1];
                fuDeedUpld.PostedFile.InputStream.Read(vFuDeed, 0, vFuDeed.Length);
                vPriDeedFileExt = System.IO.Path.GetExtension(fuDeedUpld.FileName).ToLower();
                //---------------------------------------------------------------
                bool ValidPdf = false;
                if (vPriDeedFileExt.ToLower() == ".pdf")
                {
                    cFileValidate oFile = new cFileValidate();
                    ValidPdf = oFile.ValidatePdf(fuDeedUpld.FileBytes);
                    if (ValidPdf == false)
                    {
                        gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                        fuDeedUpld.Focus();
                        return ;
                    }
                }
                //------------------------------------------------------------
                if ((vPriDeedFileExt.ToLower() != ".pdf") && (vPriDeedFileExt.ToLower() != ".xlx") && (vPriDeedFileExt.ToLower() != ".xlsx"))
                {
                    vPriDeedFileExt = ".png";
                }
                vPriDeedFileName = vPriDeedFileName + vPriDeedFileExt;
                vDeedByteYN = "Y";

                ViewState["DeedFileNm"] = vPriDeedFileName;
                ViewState["DeedIsUpLd"] = "Y";
                ViewState["DeedByteDoc"] = vFuDeed;
                ViewState["DeedByteYN"] = vDeedByteYN;

            }

            // END For DeeD

            //START FOR MOA Upload
            vPriMOAIsFileUpLd = fuMOAUpld.HasFile == true ? "Y" : "N";
            if (vPriMOAIsFileUpLd == "Y")
            {

                vFuMOA = new byte[fuMOAUpld.PostedFile.InputStream.Length + 1];
                fuMOAUpld.PostedFile.InputStream.Read(vFuMOA, 0, vFuMOA.Length);
                vPriMOAFileExt = System.IO.Path.GetExtension(fuMOAUpld.FileName).ToLower();
                //---------------------------------------------------------------
                bool ValidPdf = false;
                if (vPriMOAFileExt.ToLower() == ".pdf")
                {
                    cFileValidate oFile = new cFileValidate();
                    ValidPdf = oFile.ValidatePdf(fuMOAUpld.FileBytes);
                    if (ValidPdf == false)
                    {
                        gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                        fuMOAUpld.Focus();
                        return ;
                    }
                }
                //------------------------------------------------------------
                if ((vPriMOAFileExt.ToLower() != ".pdf") && (vPriMOAFileExt.ToLower() != ".xlx") && (vPriMOAFileExt.ToLower() != ".xlsx"))
                {
                    vPriMOAFileExt = ".png";
                }
                vPriMOAFileName = vPriMOAFileName + vPriMOAFileExt;
                vMOAByteYN = "Y";

                ViewState["MOAFileNm"] = vPriMOAFileName;
                ViewState["MOAIsUpLd"] = "Y";
                ViewState["MOAByteDoc"] = vFuMOA;
                ViewState["MOAByteYN"] = vMOAByteYN;

            }

            // END For MOA

            //START FOR AOA Upload
            vPriAOAIsFileUpLd = fuAOAUpld.HasFile == true ? "Y" : "N";
            if (vPriAOAIsFileUpLd == "Y")
            {

                vFuAOA = new byte[fuAOAUpld.PostedFile.InputStream.Length + 1];
                fuAOAUpld.PostedFile.InputStream.Read(vFuAOA, 0, vFuAOA.Length);
                vPriAOAFileExt = System.IO.Path.GetExtension(fuAOAUpld.FileName).ToLower();
                //---------------------------------------------------------------
                bool ValidPdf = false;
                if (vPriAOAFileExt.ToLower() == ".pdf")
                {
                    cFileValidate oFile = new cFileValidate();
                    ValidPdf = oFile.ValidatePdf(fuAOAUpld.FileBytes);
                    if (ValidPdf == false)
                    {
                        gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                        fuAOAUpld.Focus();
                        return ;
                    }
                }
                //------------------------------------------------------------
                if ((vPriAOAFileExt.ToLower() != ".pdf") && (vPriAOAFileExt.ToLower() != ".xlx") && (vPriAOAFileExt.ToLower() != ".xlsx"))
                {
                    vPriAOAFileExt = ".png";
                }
                vPriAOAFileName = vPriAOAFileName + vPriAOAFileExt;
                vAOAByteYN = "Y";

                ViewState["AOAFileNm"] = vPriAOAFileName;
                ViewState["AOAIsUpLd"] = "Y";
                ViewState["AOAByteDoc"] = vFuAOA;
                ViewState["AOAByteYN"] = vAOAByteYN;

            }

            // END For AOA


            //START FOR Bnk Stmnt
            vBSIsFileUpLd = fuStaUpld.HasFile == true ? "Y" : "N";
            if (vBSIsFileUpLd == "Y")
            {

                vFuBnkS = new byte[fuStaUpld.PostedFile.InputStream.Length + 1];
                fuStaUpld.PostedFile.InputStream.Read(vFuBnkS, 0, vFuBnkS.Length);
                vBSFileExt = System.IO.Path.GetExtension(fuStaUpld.FileName).ToLower();
                //---------------------------------------------------------------
                bool ValidPdf = false;
                if (vBSFileExt.ToLower() == ".pdf")
                {
                    cFileValidate oFile = new cFileValidate();
                    ValidPdf = oFile.ValidatePdf(fuStaUpld.FileBytes);
                    if (ValidPdf == false)
                    {
                        gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                        fuStaUpld.Focus();
                        return ;
                    }
                }
                //------------------------------------------------------------
                if ((vBSFileExt.ToLower() != ".pdf") && (vBSFileExt.ToLower() != ".xlx") && (vBSFileExt.ToLower() != ".xlsx"))
                {
                    vBSFileExt = ".png";
                }
                vBSFileNm = vBSFileNm + vBSFileExt;
                vBnkSByteYN = "Y";

                ViewState["BnkSFileNm"] = vBSFileNm;
                ViewState["BnkSsUpLd"] = "Y";
                ViewState["BnkSByteDoc"] = vFuBnkS;
                ViewState["BnkSByteYN"] = vBnkSByteYN;

            }

            // END For Bnk Stmnt

            //START FOR Cncl Chq
            vCheqIsFileUpLd = fuChqUpld.HasFile == true ? "Y" : "N";
            if (vCheqIsFileUpLd == "Y")
            {

                vFuCChq = new byte[fuChqUpld.PostedFile.InputStream.Length + 1];
                fuChqUpld.PostedFile.InputStream.Read(vFuCChq, 0, vFuCChq.Length);
                vCheqFileExt = System.IO.Path.GetExtension(fuChqUpld.FileName).ToLower();
                //---------------------------------------------------------------
                bool ValidPdf = false;
                if (vCheqFileExt.ToLower() == ".pdf")
                {
                    cFileValidate oFile = new cFileValidate();
                    ValidPdf = oFile.ValidatePdf(fuChqUpld.FileBytes);
                    if (ValidPdf == false)
                    {
                        gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                        fuChqUpld.Focus();
                        return ;
                    }
                }
                //------------------------------------------------------------
                if ((vCheqFileExt.ToLower() != ".pdf") && (vCheqFileExt.ToLower() != ".xlx") && (vCheqFileExt.ToLower() != ".xlsx"))
                {
                    vCheqFileExt = ".png";
                }
                vCheqFileNm = vCheqFileNm + vCheqFileExt;
                vCChqFByteYN = "Y";

                ViewState["CChqFileNm"] = vCheqFileNm;
                ViewState["CChqFIsUpLd"] = "Y";
                ViewState["CChqFByteDoc"] = vFuCChq;
                ViewState["CChqFByteYN"] = vCChqFByteYN;

            }


            // END For Cncl Chq

            //START FOR BusinessBoard
            vBBIsFileUpLd = fuBUpld.HasFile == true ? "Y" : "N";
            if (vBBIsFileUpLd == "Y")
            {

                vFuBNB = new byte[fuBUpld.PostedFile.InputStream.Length + 1];
                fuBUpld.PostedFile.InputStream.Read(vFuBNB, 0, vFuBNB.Length);
                vBBFileExt = System.IO.Path.GetExtension(fuBUpld.FileName).ToLower();
                //---------------------------------------------------------------
                bool ValidPdf = false;
                if (vBBFileExt.ToLower() == ".pdf")
                {
                    cFileValidate oFile = new cFileValidate();
                    ValidPdf = oFile.ValidatePdf(fuBUpld.FileBytes);
                    if (ValidPdf == false)
                    {
                        gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                        fuBUpld.Focus();
                        return ;
                    }
                }
                //------------------------------------------------------------
                if ((vBBFileExt.ToLower() != ".pdf") && (vBBFileExt.ToLower() != ".xlx") && (vBBFileExt.ToLower() != ".xlsx"))
                {
                    vBBFileExt = ".png";
                }
                vBBFileNm = vBBFileNm + vBBFileExt;
                vBNBByteYN = "Y";

                ViewState["BNBFileNm"] = vBBFileNm;
                ViewState["BNBIsUpLd"] = "Y";
                ViewState["BNBByteDoc"] = vFuBNB;
                ViewState["BNBByteYN"] = vBNBByteYN;

            }

            // END For BusinessBoard

            //START FOR BusinessImage
            vBGBImage = fuBGBUpld.HasFile == true ? "Y" : "N";
            if (vBGBImage == "Y")
            {

                if ((vBGBFileExt.ToLower() != ".pdf") && (vBGBFileExt.ToLower() != ".xlx") && (vBGBFileExt.ToLower() != ".xlsx"))
                {
                    vBGBFileExt = ".png";
                }

                vFuBImg = new byte[fuBGBUpld.PostedFile.InputStream.Length + 1];
                fuBGBUpld.PostedFile.InputStream.Read(vFuBImg, 0, vFuBImg.Length);
                vBGBFileExt = System.IO.Path.GetExtension(fuBGBUpld.FileName).ToLower();
                //---------------------------------------------------------------
                bool ValidPdf = false;
                if (vBGBFileExt.ToLower() == ".pdf")
                {
                    cFileValidate oFile = new cFileValidate();
                    ValidPdf = oFile.ValidatePdf(fuBGBUpld.FileBytes);
                    if (ValidPdf == false)
                    {
                        gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                        fuBGBUpld.Focus();
                        return ;
                    }
                }
                //------------------------------------------------------------
                if ((vBGBFileExt.ToLower() != ".pdf") && (vBGBFileExt.ToLower() != ".xlx") && (vBGBFileExt.ToLower() != ".xlsx"))
                {
                    vBGBFileExt = ".png";
                }
                vBGBImageFileNm = vBGBImageFileNm + vBGBFileExt;
                vBImgByteYN = "Y";

                ViewState["BImgFileNm"] = vBGBImageFileNm;
                ViewState["BImgIsUpLd"] = "Y";
                ViewState["BImgByteDoc"] = vFuBImg;
                ViewState["BImgByteYN"] = vBImgByteYN;
            }

            // END For BusinessImage


            #endregion

        }
        protected void PopulateDDL()
        {
            #region ddl

            if (hdnPriState.Value != "" && hdnPriState.Value != "0" && hdnPriState.Value != "-1")
            {
                ddlPriState.SelectedValue = hdnPriState.Value;
                if (hdnPriDist.Value != "" && hdnPriDist.Value != "0" && hdnPriDist.Value != "-1")
                {
                    PopDistrictByState(Convert.ToInt32(ddlPriState.SelectedValue), "Pri");
                    ddlPriDist.SelectedValue = hdnPriDist.Value;
                }
            }


            if (hdnBGVState.Value != "" && hdnBGVState.Value != "0" && hdnBGVState.Value != "-1")
            {
                ddlBGVState.SelectedValue = hdnBGVState.Value;
                if (hdnBGVDist.Value != "" && hdnBGVDist.Value != "0" && hdnBGVDist.Value != "-1")
                {
                    PopDistrictByState(Convert.ToInt32(ddlBGVState.SelectedValue), "BGV");
                    ddlBGVDist.SelectedValue = hdnBGVDist.Value;
                }
            }

            if (hdnPrtState.Value != "" && hdnPrtState.Value != "0" && hdnPrtState.Value != "-1")
            {
                ddlPrtState.SelectedValue = hdnPrtState.Value;
                if (hdnPrtDist.Value != "" && hdnPrtDist.Value != "0" && hdnPrtDist.Value != "-1")
                {
                    PopDistrictByState(Convert.ToInt32(ddlPrtState.SelectedValue), "PrtC");
                    ddlPrtDist.SelectedValue = hdnPrtDist.Value;
                }
            }

            if (hdnPState.Value != "" && hdnPState.Value != "0" && hdnPState.Value != "-1")
            {
                ddlPState.SelectedValue = hdnPState.Value;
                if (hdnPDist.Value != "" && hdnPDist.Value != "0" && hdnPDist.Value != "-1")
                {
                    PopDistrictByState(Convert.ToInt32(ddlPState.SelectedValue), "PrtP");
                    ddlPDist.SelectedValue = hdnPDist.Value;
                }
            }

            #endregion
        }
    }

}
