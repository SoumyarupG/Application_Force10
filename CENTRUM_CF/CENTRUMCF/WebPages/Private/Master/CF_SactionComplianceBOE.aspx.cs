using System;
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

namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class CF_SactionComplianceBOE : CENTRUMBAse
    {
        protected int vPgNo = 1;
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
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                ViewState["Grid"] = null;
                tbBCBCM.ActiveTabIndex = 0;
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
                this.PageHeading = "BC-BOE/BM";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFBOE);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {

                    btnEdit.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnEdit.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {


                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanProcess == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "BC-BOE/BM", false);
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

                case "Show":
                    EnableControl(false, this.Page);
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    gvBOEDtl.Enabled = false;
                    break;

                case "View":
                    EnableControl(false, this.Page);
                    btnEdit.Enabled = false;
                    btnShow.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    gvBOEDtl.Enabled = false;
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
                    gvBOEDtl.Enabled = true;
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbBCBCM.ActiveTabIndex = 0;
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
                //GenerateBankingGrid(hdLeadId.Value);
                StatusButton("Edit");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["pathImage"];
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                DataTable dt = (DataTable)ViewState["BOE"];

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    string srlNo = dt.Rows[i]["SanctionId"].ToString();

                    if (dt.Rows[i]["ByteBOEYN"].ToString() == "Y")
                    {
                        byte[] imgArray = (byte[])(dt.Rows[i]["ByteBOEDoc"]);

                        string vkDocName = dt.Rows[i]["BOEFileNm"].ToString();

                        if (vkDocName.ToLower().Contains(".pdf"))
                        {
                            vkDocName = hdLeadId.Value.ToString() + "_" + srlNo + "_" + vkDocName;
                        }
                        else
                        {
                            vkDocName = srlNo + "_" + vkDocName;
                        }
                        if (MinioYN == "N")
                        {
                            string folderPath = string.Format("{0}/{1}", vPathImage, hdLeadId.Value);
                            System.IO.Directory.CreateDirectory(folderPath);
                            string filePath = string.Format("{0}/{1}", folderPath, vkDocName);
                            File.WriteAllBytes(filePath, imgArray);
                        }
                        else
                        {
                            CApiCalling oAC = new CApiCalling();
                            oAC.UploadFileMinio(imgArray, vkDocName, hdLeadId.Value, DocumentBucket, MinioUrl);
                        }
                    }

                }
                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                LoadBasicDetailsList(1);
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
            string vXmlBOE = "";
            Int32 vMaxFileSize = 0;
            CMember oMem = null;
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
                    dt = (DataTable)ViewState["BOE"];

                    dt.TableName = "Table";
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlBOE = oSW.ToString();
                    }

                    if (dt.Rows.Count > 0)
                    {
                        oMem = new CMember();
                        vErr = oMem.CF_SaveBOEDtl(vLeadId, vXmlBOE, vBrCode, UID, 0, ref vErrMsg);
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
                oMem = null;
            }

        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadBasicDetailsList(0);
        }
        private void LoadBasicDetailsList(Int32 pPgIndx)
        {
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            Int32 vTotRows = 0;
            string vBrCode = Session[gblValue.BrnchCode].ToString();


            try
            {
                dt = oMem.CF_GetSancCompBOE(vBrCode, txtSearch.Text.Trim(), pPgIndx, ref vTotRows);
                if (dt.Rows.Count > 0)
                {
                    gvBasicDet.DataSource = dt;
                    gvBasicDet.DataBind();
                }
                else
                {
                    gvBasicDet.DataSource = null;
                    gvBasicDet.DataBind();
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
            LoadBasicDetailsList(vPgNo);
            tbBCBCM.ActiveTabIndex = 0;
        }
        #region gvBasicDet_RowCommand
        protected void gvBasicDet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 pLeadID = 0;
            string vBrCode = "";
            DataTable dt, dt1 = null;
            DataSet ds = new DataSet();
            ClearControls(this.Page);
            CMember oMem = null;
            try
            {
                pLeadID = Convert.ToInt64(e.CommandArgument);
                ViewState["LeadID"] = pLeadID;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    lblApplNm1.Text = Convert.ToString(gvRow.Cells[2].Text);

                    if (btnShow != null)
                    {
                        lblBCPNm1.Text = Convert.ToString(btnShow.Text);
                    }

                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvBasicDet.Rows)
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
                    StatusButton("Show");
                    hdLeadId.Value = Convert.ToString(pLeadID);
                    GenerateBOEGrid(pLeadID);
                    tbBCBCM.ActiveTabIndex = 1;
                    pnlBCBCM.Enabled = true;



                }
            }
            finally
            {
                dt = null;
            }
        }



        private void GenerateBOEGrid(Int64 LeadID)
        {
            DataSet ds = null;
            DataTable dt = null;
            DataTable dt1 = null;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                ds = oMem.GenerateBOEGrid(LeadID);
                dt = ds.Tables[0];
                ViewState["BOE"] = dt;
                gvBOEDtl.DataSource = dt;
                gvBOEDtl.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        #endregion
        private Boolean GetData()
        {

            string vfilename = "BC-BOE", vBOEExt;
            int vMaxFileSize = Convert.ToInt32(MaxFileSize); byte[] vFuBOE = null;

            DataTable dt = (DataTable)ViewState["BOE"];
            foreach (GridViewRow gr in gvBOEDtl.Rows)
            {
                Label lblSacId = (Label)gvBOEDtl.Rows[gr.RowIndex].FindControl("lblSacId");

                TextBox txtBOERemarks = (TextBox)gvBOEDtl.Rows[gr.RowIndex].FindControl("txtBOERemarks");

                Label lblBOEFN = (Label)gvBOEDtl.Rows[gr.RowIndex].FindControl("lblBOEFN");
                FileUpload fuBOE = (FileUpload)gr.FindControl("fuBOE");


                if (txtBOERemarks.Text == "")
                {
                    gblFuction.AjxMsgPopup("BC-BOE/BM Remarks is Blank...");
                    return false;
                }

                if (fuBOE.HasFile == true)
                {
                    if (fuBOE.PostedFile.ContentLength > vMaxFileSize)
                    {
                        gblFuction.AjxMsgPopup("Maximum upload file Size exceed the limit.File Size Should Be Maximum " + Convert.ToString(FileSize));
                        return false;
                    }


                    vFuBOE = new byte[fuBOE.PostedFile.InputStream.Length + 1];
                    fuBOE.PostedFile.InputStream.Read(vFuBOE, 0, vFuBOE.Length);
                    vBOEExt = System.IO.Path.GetExtension(fuBOE.FileName).ToLower();
                    if (vBOEExt.ToLower() == ".jpg" || vBOEExt.ToLower() == ".png" || vBOEExt.ToLower() == ".jpeg" ||
                     vBOEExt.ToLower() == ".pdf" || vBOEExt.ToLower() == ".xlx" || vBOEExt.ToLower() == ".xlsx")
                    {
                        if ((vBOEExt.ToLower() != ".pdf") && (vBOEExt.ToLower() != ".xlx") && (vBOEExt.ToLower() != ".xlsx"))
                        {
                            vBOEExt = ".png";
                        }
                        dt.Rows[gr.RowIndex]["ByteBOEDoc"] = vFuBOE;
                        dt.Rows[gr.RowIndex]["BOEIsUp"] = "Y";
                        dt.Rows[gr.RowIndex]["BOEFileNm"] = vfilename + vBOEExt;
                        dt.Rows[gr.RowIndex]["BOEFileStrPath"] = DocumentBucketURL;
                        dt.Rows[gr.RowIndex]["ByteBOEYN"] = "Y";
                        // lblBCBCMFN.Text = vfilename + vBCMExt;
                    }
                    else
                    {
                        gblFuction.MsgPopup("This File Extension is Not Allowed Upload...");
                        return false;
                    }
                }
                else
                {
                    if (dt.Rows[gr.RowIndex]["ByteBOEYN"].ToString() == "N")
                    {
                        if (dt.Rows[gr.RowIndex]["BOEIsUp"].ToString() == "Y")
                        {
                            dt.Rows[gr.RowIndex]["BOEFileNm"] = lblBOEFN.Text;
                            dt.Rows[gr.RowIndex]["BOEFileStrPath"] = DocumentBucketURL;
                            dt.Rows[gr.RowIndex]["BOEIsUp"] = "Y";
                        }
                        else
                        {
                            dt.Rows[gr.RowIndex]["BOEIsUp"] = "N";
                            dt.Rows[gr.RowIndex]["BOEFileNm"] = "";
                            dt.Rows[gr.RowIndex]["BOEFileStrPath"] = "";
                            lblBOEFN.Text = "";
                        }
                    }

                }

                //Inserting data into datatable dt
                dt.Rows[gr.RowIndex]["SanctionId"] = Convert.ToInt32(lblSacId.Text == "" ? "" : lblSacId.Text);
                dt.Rows[gr.RowIndex]["BOERemarks"] = Convert.ToString(txtBOERemarks.Text == "" ? "" : txtBOERemarks.Text);

            }
            dt.AcceptChanges();
            ViewState["BOE"] = dt;
            gvBOEDtl.DataSource = dt;
            gvBOEDtl.DataBind();
            return true;
        }
        #region imgBCBCM_Click
        protected void imgBCBCM_Click(object sender, EventArgs e)
        {

            GridViewRow gvRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            Label lblSrlNo = (Label)gvRow.FindControl("lblSacId");
            Label lbFile = (Label)gvRow.FindControl("lblBCBCMFN");

            ViewImgDoc(hdLeadId.Value, lblSrlNo.Text, lbFile.Text);

        }
        #endregion
        #region imgHO_Click
        protected void imgHO_Click(object sender, EventArgs e)
        {

            GridViewRow gvRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            Label lblSrlNo = (Label)gvRow.FindControl("lblSacId");
            Label lbFile = (Label)gvRow.FindControl("lblHO");

            ViewImgDoc(hdLeadId.Value, lblSrlNo.Text, lbFile.Text);

        }
        #endregion
        #region imgBOE_Click
        protected void imgBOE_Click(object sender, EventArgs e)
        {

            GridViewRow gvRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            Label lblSrlNo = (Label)gvRow.FindControl("lblSacId");
            Label lbFile = (Label)gvRow.FindControl("lblBOEFN");

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
    }
}