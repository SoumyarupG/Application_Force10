using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;
using System.Configuration;
using System.Net;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class DigiDocManualUpload : CENTRUMBase
    {
        protected int cPgNo = 1;
        public static int cvar = 0;
        string path = "";
        string DigiDocBucket = ConfigurationManager.AppSettings["DigiDocBucket"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitBasePage();
                ViewState["StateEdit"] = null;
                ViewState["DocId"] = null;
                ViewState["AppId"] = null;
                txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtUploadDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                popLO();
                LoadGrid(1);
                StatusButton("View");
                tabLoanAppl.ActiveTabIndex = 0;
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Digital Document Manual Upload";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDigiDocMnuUpload);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Digital Document Manual Upload", false);
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
            fuDigiDocUpload.Enabled = Status;
            txtUploadDt.Enabled = Status;
            ddlLo.Enabled = Status;
            ddlCenter.Enabled = Status;
            ddlGroup.Enabled = Status;
            ddlMemNo.Enabled = Status;
            ddlAppNo.Enabled = Status;            
        }

        private void ClearControls()
        {
            ddlMemNo.SelectedIndex = -1;
            ddlAppNo.Items.Clear();
            fuDigiDocUpload.Dispose();
        }

        protected void gvLoanAppl_RowCommand(object sender, GridViewCommandEventArgs e) 
        { }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }

        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CApplication oCG = null;
            Int32 vRows = 0;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                cvar = 0;
                vMode = "";
                vBrCode = (string)Session[gblValue.BrnchCode];
                DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                oCG = new CApplication();
                dt = oCG.GetDigiDocManualUploadList(vBrCode, vFrmDt, vToDt, txtSearch.Text.Trim(), pPgIndx, ref vRows);
                gvLoanAppl.DataSource = dt.DefaultView;
                gvLoanAppl.DataBind();
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
            finally
            {
                dt = null;
                oCG = null;
                cvar = 1;
            }
        }

        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }

        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblTotalPages.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }
            LoadGrid(cPgNo);
            tabLoanAppl.ActiveTabIndex = 0;
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
                tabLoanAppl.ActiveTabIndex = 1;
                StatusButton("Add");
                txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtUploadDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        { }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        { }

        private void popLO()
        {
            DataTable dt = null;
            CEO oCEO = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            vBrCode = (string)Session[gblValue.BrnchCode];
            oCEO = new CEO();
            dt = oCEO.PopRO(Session[gblValue.BrnchCode].ToString(), "", "", vLogDt, 0);
            if (dt.Rows.Count > 0)
            {
                ddlLo.DataSource = dt;
                ddlLo.DataTextField = "EoName";
                ddlLo.DataValueField = "EoId";
                ddlLo.DataBind();
            }
            ListItem Li = new ListItem("<-- Select -->", "-1");
            ddlLo.Items.Insert(0, Li);
        }

        protected void ddlLo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLo.SelectedIndex > 0) PopCenter(ddlLo.SelectedValue);
        }

        private void PopCenter(string vEOID)
        {
            ddlMemNo.Items.Clear();
            ddlGroup.Items.Clear();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("D", "N", "AA", "MarketID", "Market", "MarketMSt", vEOID, "EOID", "Tra_DropDate", vLogDt, vBrCode);
                ddlCenter.DataSource = dt;
                ddlCenter.DataTextField = "Market";
                ddlCenter.DataValueField = "MarketID";
                ddlCenter.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCenter.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCenter.SelectedIndex > 0) PopGroup(ddlCenter.SelectedValue);
        }

        private void PopGroup(string vCenterID)
        {
            ddlGroup.Items.Clear();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("D", "N", "AA", "GroupID", "GroupName", "GroupMst", vCenterID, "MarketID", "Tra_DropDate", vLogDt, vBrCode);
                ddlGroup.DataSource = dt;
                ddlGroup.DataTextField = "GroupName";
                ddlGroup.DataValueField = "GroupID";
                ddlGroup.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlGroup.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMemNo.SelectedIndex = -1;
            PopMember(ddlGroup.SelectedValue, "");
        }

        private void PopMember(string vGroupID, string vMemId)
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {                
                CMember oMem = new CMember();
                dt = oMem.GetMemListByGroupId(vGroupID, vBrCode);
                ddlMemNo.DataSource = dt;
                ddlMemNo.DataTextField = "MemberCode";
                ddlMemNo.DataValueField = "MemberId";
                ddlMemNo.DataBind();
                ListItem oItm = new ListItem();
                oItm.Text = "<--- Select --->";
                oItm.Value = "-1";
                ddlMemNo.Items.Insert(0, oItm);
                ddlMemNo.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ddlMemNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                CApplication oLA = new CApplication();
                dt = oLA.GetDigiDocManUpLnAppList(ddlMemNo.SelectedValue.ToString(), vBrCode, vLogDt);
                ddlAppNo.DataSource = dt;
                ddlAppNo.DataTextField = "LoanAppNo";
                ddlAppNo.DataValueField = "LoanAppId";
                ddlAppNo.DataBind();
                ListItem oItm = new ListItem();
                oItm.Text = "<--- Select --->";
                oItm.Value = "-1";
                ddlAppNo.Items.Insert(0, oItm);
                ddlAppNo.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabLoanAppl.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = string.Empty, vBrCode = string.Empty, vDocId = string.Empty,vFileExt;
            Int32 vErr = 0;
            CApplication oCA = null;
            vBrCode = Session[gblValue.BrnchCode].ToString();
            path = ConfigurationManager.AppSettings["DigiDocpath"];
            string vMemId = Convert.ToString(ddlMemNo.SelectedValue);
            string vLnAppId = Convert.ToString(ddlAppNo.SelectedValue);
            DateTime vLogDt = gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate]));
            DateTime vDocUploadDt = gblFuction.setDate(txtUploadDt.Text);

            if (vMemId == "-1" || vMemId == "")
            { 
                gblFuction.MsgPopup("Please, Select Member");
                return false;
            }
            if (vLnAppId == "-1" || vLnAppId == "")
            {
                gblFuction.MsgPopup("Please, Select Application");
                return false;
            }

            if (fuDigiDocUpload.HasFile == false)
            {
                gblFuction.MsgPopup("Please, select digital document pdf file");
                return false;
            }
            else
            {
                vFileExt=System.IO.Path.GetExtension(fuDigiDocUpload.FileName);
                if (vFileExt.ToUpper() != ".PDF")
                { 
                    gblFuction.MsgPopup("Please, upload pdf file only");
                    return false;
                }
            }
            if (fuDigiDocUpload.PostedFile.ContentLength > 5242880)
            {
                gblFuction.AjxMsgPopup("File size must be with in 5MB.");
                return false;
            }
            try
            {
                oCA = new CApplication();
                vErr = oCA.InsertDigiDocManualUpload(ref vDocId, vMemId, vLnAppId, vLogDt, vDocUploadDt, vBrCode, Convert.ToInt32(Session[gblValue.UserId].ToString()),"I");
                if (vErr == 0)
                {
                    ViewState["DocId"] = vDocId;
                    ViewState["AppId"] = vLnAppId;
                    if (fuDigiDocUpload.HasFile)
                    {
                        UplodaFile(vLnAppId, vMemId, path, vFileExt);
                    }
                    vResult = true;
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCA = null;
            }
        }

        private void UplodaFile(string pAppId, string pMemberId, string pPath, string pFileExt)
        {
            if (MinioYN == "N")
            {
                string vFilePath = pPath + pAppId + pFileExt;
                if (File.Exists(vFilePath))
                {
                    File.Delete(vFilePath);
                }               
                fuDigiDocUpload.SaveAs(vFilePath);
            }
            else
            {
                CApiCalling oAC = new CApiCalling();
                byte[] vFile = ConvertFileToByteArray(fuDigiDocUpload.PostedFile);
                string vMessage = oAC.UploadFileMinio(vFile, pAppId + pFileExt, pAppId, DigiDocBucket, MinioUrl);
            }
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
    }
}
