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
using System.Net;

namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class CF_NewLeadMst : CENTRUMBAse
    {
        protected int vPgNo = 1;
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string BucketURL = ConfigurationManager.AppSettings["BucketURL"];
        string CFLeadBucket = ConfigurationManager.AppSettings["CFLeadBucket"];
        string MaxFileSize = ConfigurationManager.AppSettings["MaxFileSize"];
        string FileSize = ConfigurationManager.AppSettings["FileSize"];
        string CFLeadBucketURL = ConfigurationManager.AppSettings["CFLeadBucketURL"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {

                txtLeadGenDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                //PopPropertyType();

                tbLeadGen.ActiveTabIndex = 0;
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
                this.PageHeading = "New Lead Generation";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFNewLeadGen);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "New Lead Generation", false);
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
                    EnableControl(true);
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;

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
                    break;
            }
        }
        private void EnableControl(Boolean Status)
        {

            txtCustName.Enabled = Status;
            txtLeadGenDt.Enabled = Status;
            txtMobNo.Enabled = Status;
            lblBranch.Enabled = Status;
            fuCAMUpld.Enabled = Status;
            txtBCPropNo.Enabled = Status;
        }
        private void ClearControls()
        {
            txtCustName.Text = "";
            txtMobNo.Text = "";
            lblBranch.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
            txtBCPropNo.Text = "";
            lblFileName.Text = "";
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                LoadLeadList(1);               
                ViewState["StateEdit"] = null;
                GetLeadDtlByLeadId(Convert.ToInt64(hdLeadId.Value));
                StatusButton("View");
                btnEdit.Enabled = true;
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 vErr = 0, vNewId = 0;
            string vBCPropNo = "", vAppName = "", vMobNo = "", vCamUpld = "", vBrCode = "", vErrMsg = "", vCamFileName = "", vLeadId = "", vCamFileExt = "", vFileStorePath = "", vFileName = "";
            Int64 vLeadIDedit = 0;

            DateTime vLeadGenDt = gblFuction.setDate(txtLeadGenDt.Text.ToString());
            Int32 vFileSize = 0, vMaxFileSize = 0;
            vMaxFileSize = Convert.ToInt32(MaxFileSize);
            string vdate = Convert.ToString(Session[gblValue.LoginDate]);
            DateTime vLogDt = gblFuction.setDate(vdate);
            CMember oMem = null;
            try
            {
                if (hdLeadId.Value != "")
                {
                    vLeadId = hdLeadId.Value;
                    vLeadIDedit = Convert.ToInt64(vLeadId);
                }
                vBCPropNo = txtBCPropNo.Text.ToString().Trim();
                vAppName = txtCustName.Text.ToString().Trim();
                vMobNo = txtMobNo.Text.ToString().Trim();

                vFileStorePath = CFLeadBucketURL;

                vBrCode = Session[gblValue.BrnchCode].ToString();
                if (Mode == "Save")
                {
                    if (vMobNo.Length < 10)
                    {
                        gblFuction.MsgPopup("Mobile No Should Be 10 Digit...");
                        return false;
                    }
                    if (vLeadGenDt > vLogDt)
                    {
                        gblFuction.MsgPopup("Lead Generation Date Should Not Be Greater Than Login Date ...");
                        return false;
                    }
                    vCamUpld = fuCAMUpld.HasFile == true ? "Y" : "N";
                    vCamFileName = fuCAMUpld.HasFile == true ? "CAM" : "";
                    vCamFileExt = System.IO.Path.GetExtension(fuCAMUpld.PostedFile.FileName);
                    if (vCamUpld == "N")
                    {
                        gblFuction.MsgPopup("Please Upload CAM");
                        return false;
                    }
                    if (fuCAMUpld.PostedFile.ContentLength > vMaxFileSize)
                    {
                        gblFuction.MsgPopup("Maximum upload file Size exceed the limit.File Size Should Be Maximum " + Convert.ToString(FileSize));
                        return false;
                    }
                    if (vCamUpld == "Y")
                    {
                        if (vCamFileExt.ToLower() == ".xls" || vCamFileExt.ToLower() == ".xlsx")
                        {
                            vFileName = vCamFileName + vCamFileExt;
                            
                        }
                        else
                        {
                            gblFuction.MsgPopup("Only Excel File Can Be Upload...");
                            return false;
                        }
                    }
                    else
                    {
                        vFileName = "";
                    }

                    
                    oMem = new CMember();
                    vErr = oMem.CF_SaveLeadMst(ref vNewId, vLeadGenDt, vBCPropNo, vBrCode, vAppName, vMobNo, vCamUpld, vFileStorePath, vFileName, this.UserID, "Save", 0, ref vErrMsg);
                    if (vErr == 0)
                    {
                        hdLeadId.Value = Convert.ToString(vNewId);
                        ViewState["LeadId"] = vNewId;
                        if (fuCAMUpld.HasFile)
                        {
                            SaveMemberImages(fuCAMUpld, Convert.ToString(vNewId), "CAM", vCamFileExt, "N");
                        }
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
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
                    if (vMobNo.Length < 10)
                    {
                        gblFuction.MsgPopup("Mobile No Should Be 10 Digit...");
                        return false;
                    }
                    if (vLeadGenDt > vLogDt)
                    {
                        gblFuction.MsgPopup("Lead Generation Date Should Not Be Greater Than Login Date ...");
                        return false;
                    }

                    vCamUpld = fuCAMUpld.HasFile == true ? "Y" : "N";
                    vCamFileName = fuCAMUpld.HasFile == true ? "CAM" : "";
                    vCamFileExt = System.IO.Path.GetExtension(fuCAMUpld.PostedFile.FileName);


                    if (vCamUpld == "Y")
                    {
                        if (vCamFileExt.ToLower() == ".xls" || vCamFileExt.ToLower() == ".xlsx")
                        {
                            vFileName = vCamFileName + vCamFileExt;

                        }
                        else
                        {
                            gblFuction.MsgPopup("Only Excel File Can Be Upload...");
                            return false;
                        }
                    }
                    else
                    {
                        vFileName = "";
                    }
                    
                    oMem = new CMember();
                    vErr = oMem.CF_UpdateLeadMst(vLeadIDedit, vLeadGenDt, vBCPropNo, vBrCode, vAppName, vMobNo, vCamUpld, vFileStorePath, vFileName, this.UserID, "Edit", 0, ref vErrMsg);
                    if (vErr == 0)
                    {
                        hdLeadId.Value = vLeadId;
                        ViewState["LeadId"] = vLeadIDedit;

                        if (fuCAMUpld.HasFile)
                        {
                            if (fuCAMUpld.PostedFile.ContentLength > vMaxFileSize)
                            {
                                gblFuction.MsgPopup("Maximum upload file Size exceed the limit.File Size Should Be Maximum " + Convert.ToString(FileSize));
                                return false;
                            }
                            else
                            {
                                SaveMemberImages(fuCAMUpld, Convert.ToString(vNewId), "CAM", vCamFileExt, "N");
                            }
                        }
                        gblFuction.AjxMsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrMsg);
                        vResult = false;
                    }
                }
                else
                {
                    oMem = new CMember();
                    vErr = oMem.CF_UpdateLeadMst(vLeadIDedit, vLeadGenDt, vBCPropNo, vBrCode, vAppName, vMobNo, vCamUpld, vFileStorePath, "", this.UserID, "Delet", 0, ref vErrMsg);
                    if (vErr == 0)
                    {
                        gblFuction.AjxMsgPopup(gblPRATAM.DeleteMsg);
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
                lblBranch.Text = Session[gblValue.BrName].ToString().Trim();
                btnCam.Enabled = false;

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
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    LoadLeadList(1);
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void LoadLeadList(Int32 pPgIndx)
        {

            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            Int32 vTotRows = 0;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                dt = oMem.CF_GetLeadList(vBrCode, txtSearch.Text.Trim(), pPgIndx, ref vTotRows);
                if (dt.Rows.Count > 0)
                {
                    gvLead.DataSource = dt;
                    gvLead.DataBind();
                }
                else
                {
                    gvLead.DataSource = null;
                    gvLead.DataBind();
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
            LoadLeadList(vPgNo);
            tbLeadGen.ActiveTabIndex = 0;
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
                //txtCustName.Enabled = false;
                //txtLeadGenDt.Enabled = false;
                //txtMobNo.Enabled = false;

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
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadLeadList(0);
        }       
        protected void gvLead_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 pLeadId = 0;
            string vBrCode = "";
            DataTable dt, dt1 = null;
            DataSet ds = new DataSet();
            ClearControls();
            try
            {
                pLeadId = Convert.ToInt32(e.CommandArgument);
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
                    GetLeadDtlByLeadId(pLeadId);
                }
            }
            finally
            {
                dt = null;
            }
        }
        protected void GetLeadDtlByLeadId(Int64 pLeadId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                CDistrict oDist = new CDistrict();
                dt = oDist.CF_GetLeadDtlByLeadId(pLeadId);
                if (dt.Rows.Count > 0)
                {
                    hdLeadId.Value = Convert.ToString(dt.Rows[0]["LeadId"]);
                    txtBCPropNo.Text = Convert.ToString(dt.Rows[0]["BCPropNo"]);
                    txtCustName.Text = Convert.ToString(dt.Rows[0]["AppName"]);
                    txtLeadGenDt.Text = Convert.ToString(dt.Rows[0]["LeadDate"]);
                    txtMobNo.Text = Convert.ToString(dt.Rows[0]["MobNo"]);
                    lblBranch.Text = Convert.ToString(dt.Rows[0]["Branch"]);
                    lblFileName.Text = Convert.ToString(dt.Rows[0]["CamFileName"]);
                    hdFileName.Value = Convert.ToString(dt.Rows[0]["CamFileName"]);
                    btnCam.Enabled = true;
                    tbLeadGen.ActiveTabIndex = 1;
                    StatusButton("Show");

                }
                else
                {
                    txtBCPropNo.Text = "";
                    txtCustName.Text = "";
                    txtLeadGenDt.Text = "";
                    txtMobNo.Text = "";
                    lblBranch.Text = "";
                    lblFileName.Text = "";
                    lblFileName.Text = "";
                    btnCam.Enabled = false;
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
        private string SaveMemberImages(FileUpload flup, string imageGroup, string imageName, string ImgExt, string isImageSaved)
        {
            CApiCalling oAC = new CApiCalling();
            byte[] ImgByte = ConvertFileToByteArray(flup.PostedFile);
            isImageSaved = oAC.UploadFileMinio(ImgByte, imageName + ImgExt, imageGroup, CFLeadBucket, MinioUrl);
            return isImageSaved;
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
        protected void btnCam_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vFile = "";
            string[] ActNetPath = CFLeadBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdLeadId.Value + "_" + lblFileName.Text;
                if (ValidUrlChk(ActNetImage))
                {
                    vFile = ActNetImage;
                    break;
                }
            }
            if (vFile != "")
            {
                WebClient cln = null;
                cln = new WebClient();
                byte[] vDoc = null;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                cln = new WebClient();
                vDoc = cln.DownloadData(vFile);
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdLeadId.Value + "_" + lblFileName.Text);
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