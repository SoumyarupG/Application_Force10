using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Net;


namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class ClaimeStatementAndDeathRecipt : CENTRUMBase
    {
        string ImagePath = "";
        string DeathDocBucket = ConfigurationManager.AppSettings["DeathDocBucket"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        string PathDeathDoc = ConfigurationManager.AppSettings["PathDeathDoc"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["DtLoan"] = null;
                ViewState["StateEdit"] = null;
                PopBranch(Session[gblValue.UserName].ToString());
                GetDeathMember(ddlBrch.SelectedValue, "");
                LoadGrid(ddlBrch.SelectedValue, "N");
                PopGender();
                StatusButton("View");
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Death Document Upload";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuClaimStatementDischarge);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Death Document Upload", false);
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
                    btnPdf.Enabled = true;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":

                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnPdf.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnPdf.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":

                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnPdf.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    //ClearControls();
                    break;
                case "Delete":

                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnPdf.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        private void ClearControls()
        {
            ddlMem.SelectedIndex = -1;
            lblMemNmId.Text = "";
            //ddlBrch.SelectedIndex = -1;
            lblNomIdPrfExt.Text = "";
            lblBP.Text = "";
            lblCS.Text = "";
            lblDC.Text = "";
            lblLoanId.Text = "";

            txtBankName.Text = "";
            txtBranchName.Text = "";
            txtAccNo.Text = "";
            txtIfsc.Text = "";
            txtBenFishName.Text = "";
            txtNomName.Text = "";
            txtNomRelation.Text = "";
            ddlNomGen.SelectedIndex = -1;
            txtNomAge.Text = "";
            txtNomMob.Text = "";
            txtNomPin.Text = "";
            txtNomAddress.Text = "";
            ddlLoanNo.Items.Clear();
           
        }

        private void PopGender()
        {
            Dictionary<string, string> oGen = new Dictionary<string, string>();
            oGen.Add("<---Select--->", "-1");
            oGen.Add("Male", "M");
            oGen.Add("Female", "F");
            oGen.Add("Transgender", "O");

            ddlNomGen.DataSource = oGen;
            ddlNomGen.DataValueField = "value";
            ddlNomGen.DataTextField = "key";
            ddlNomGen.DataBind();
        }

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                if (dt.Rows.Count > 0)
                {
                    dt = dt.AsEnumerable().Where(a => a.Field<string>("BranchCode") != "0000").CopyToDataTable();
                    ddlBrch.DataSource = dt;
                    ddlBrch.DataTextField = "BranchName";
                    ddlBrch.DataValueField = "BranchCode";
                    ddlBrch.DataBind();
                    if (Session[gblValue.BrnchCode].ToString() != "0000")
                    {
                        ddlBrch.SelectedIndex = ddlBrch.Items.IndexOf(ddlBrch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                        ddlBrch.Enabled = false;
                    }
                    else
                    {
                        ListItem liSel = new ListItem("<-- Select -->", "-1");
                        ddlBrch.Items.Insert(0, liSel);
                    }
                }
                else
                {
                    ddlBrch.Items.Clear();
                }
            }

            finally
            {
                dt = null;
                oUsr = null;
            }
        }

        private void EnableControl(Boolean Status)
        {
            ddlMem.Enabled = Status;
            ddlBrch.Enabled = false;
            ddlLoanNo.Enabled = Status;
            ddlWhoDied.Enabled = Status;
            rdbOpt.Enabled = Status;
            fuNphoto.Enabled = Status;
            fuCS.Enabled = Status;
            fuBP.Enabled = Status;
            fuDC.Enabled = Status;
            txtAccNo.Enabled = Status;
            txtIfsc.Enabled = Status;          
            txtNomName.Enabled = false;
            txtNomRelation.Enabled = false;
            ddlNomGen.Enabled = Status;
            txtNomAge.Enabled = false;
            txtNomMob.Enabled = Status;
            txtNomPin.Enabled = Status;
            txtNomAddress.Enabled = false;
        }

        protected void ddlBrch_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDeathMember(ddlBrch.SelectedValue, "");
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(ddlBrch.SelectedValue, chkSendBack.Checked == true ? "S" : "N");
        }

        private void GetDeathMember(string vBrachCode, string vMode)
        {
            DataTable dt = null;
            CMember oCM = null;
            try
            {
                oCM = new CMember();
                dt = oCM.GetDeathMember(vBrachCode, gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vMode);
                ddlMem.Items.Clear();
                ddlMem.DataTextField = "MemberName";
                ddlMem.DataValueField = "MemberID";
                ddlMem.DataSource = dt;
                ddlMem.DataBind();
                ViewState["DtLoan"] = dt;
                ListItem oItm = new ListItem();
                oItm.Text = "<--- Select --->";
                oItm.Value = "-1";
                ddlMem.Items.Insert(0, oItm);
            }
            finally
            {
                oCM = null;
                dt = null;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            string vDate = Session[gblValue.LoginDate].ToString();
            string vLoanId = ddlLoanNo.SelectedValue.ToString();
            string vRptPath = "", vRptType = LblICId.Text;
            string vRptName = "";
            DataTable dt = null;
            CReports oRpt = null;
            try
            {
                oRpt = new CReports();
                dt = oRpt.GetInsuClaimFormDtl(vLoanId);
                if (dt.Rows.Count > 0)
                {
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        if (vRptType == "1" || vRptType == "5")
                        {
                            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\HdfcClaimForm-1.rpt";
                            vRptName = ddlLoanNo.SelectedItem.Text + "_" + gblFuction.setDate(vDate).ToString("yyyyMMdd") + "_Hdfc_Claim_Form";
                        }
                        else if (vRptType == "3")
                        {
                            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LicClaimForm.rpt";
                            vRptName = ddlLoanNo.SelectedItem.Text + "_" + gblFuction.setDate(vDate).ToString("yyyyMMdd") + "_LIC_Claim_Form";

                        }
                        else if (vRptType == "2" || vRptType == "4")
                        {
                            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\kotakClaimForm-1.rpt";
                            vRptName = ddlLoanNo.SelectedItem.Text + "_" + gblFuction.setDate(vDate).ToString("yyyyMMdd") + "_Kotak_Claim_Form";
                        }
                        else if (vRptType == "6")
                        {
                            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\AgeasClaimForm.rpt";
                            vRptName = ddlLoanNo.SelectedItem.Text + "_" + gblFuction.setDate(vDate).ToString("yyyyMMdd") + "_Ageas_Claim_Form";
                        }
                        else if (vRptType == "7")
                        {
                            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\AgeasClaimFormNew.rpt";
                            vRptName = ddlLoanNo.SelectedItem.Text + "_" + gblFuction.setDate(vDate).ToString("yyyyMMdd") + "_Ageas_Claim_Form";
                        }
                        else
                        {

                        }
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt);
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, vRptName);
                        Response.ClearHeaders();
                        Response.ClearContent();
                    }
                }
                else
                {
                }

            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }

        protected void ddlMem_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = (DataTable)ViewState["DtLoan"];
                DataTable newTable = new DataTable();
                newTable = dt.Select("MemberID = '" + ddlMem.SelectedValue.Trim() + "'").CopyToDataTable();

                ddlLoanNo.DataTextField = "LoanNo";
                ddlLoanNo.DataValueField = "LoanId";
                ddlLoanNo.DataSource = newTable;
                ddlLoanNo.DataBind();
                ListItem liSel = new ListItem("<-- Select -->", "-1");
                ddlLoanNo.Items.Insert(0, liSel);
            }
            finally
            {
                dt = null;
            }
        }

        protected void ddlWhoDied_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLoanNo.SelectedIndex != 0)
            {
                if (ddlWhoDied.SelectedIndex != 0)
                {
                    GetDeathMemNomInfo();
                }
                //else
                //{
                //    gblFuction.AjxMsgPopup("Please Select Who Died.");
                //    return;
                //}
            }
            //else
            //{
            //    gblFuction.AjxMsgPopup("Please Select Loan No.");
            //    return;
            //}
        }
        protected void ddlLoanNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLoanNo.SelectedIndex != 0)
            {
                if (ddlWhoDied.SelectedIndex != 0)
                {
                    GetDeathMemNomInfo();
                }
                //else
                //{
                //    gblFuction.AjxMsgPopup("Please Select Who Died.");
                //    return;
                //}
            }
            //else
            //{
            //    gblFuction.AjxMsgPopup("Please Select Loan No.");
            //    return;
            //}
        }

        protected void GetDeathMemNomInfo()
        {
            CClaimeStatement oCc = null;
            DataTable dt = new DataTable();
            try
            {
                oCc = new CClaimeStatement();
                dt = oCc.GetDeathMemNomInfo(ddlLoanNo.SelectedValue, ddlWhoDied.SelectedValue);
                if (dt.Rows.Count > 0)
                {
                    //ddlWhoDied.Enabled = false;
                    txtNomName.Text = dt.Rows[0]["NomName"].ToString();
                    txtNomAddress.Text = dt.Rows[0]["NomAddress"].ToString();
                    txtNomRelation.Text = dt.Rows[0]["HumanRelationName"].ToString();
                    txtNomAge.Text = dt.Rows[0]["NomAge"].ToString();
                    LblICId.Text = dt.Rows[0]["ICId"].ToString();
                    ddlNomGen.SelectedIndex = ddlNomGen.Items.IndexOf(ddlNomGen.Items.FindByValue(Convert.ToString(dt.Rows[0]["Gender"])));
                    txtNomMob.Text = dt.Rows[0]["Mobile"].ToString();
                    txtNomPin.Text = dt.Rows[0]["Pin"].ToString();
                    txtAccNo.Text = dt.Rows[0]["AccNo"].ToString();
                    txtIfsc.Text = dt.Rows[0]["IFSCCode"].ToString();
                    txtBenFishName.Text = dt.Rows[0]["BenFishName"].ToString();
                    txtBankName.Text = dt.Rows[0]["BankName"].ToString();
                    txtBranchName.Text = dt.Rows[0]["BankBranch"].ToString();

                }
            }
            finally
            {
                dt = null;
                oCc = null;
            }
        }
        
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["StateEdit"] = "Add";
                tabClaimeState.ActiveTabIndex = 1;
                StatusButton("Add");
                ddlWhoDied.Enabled = true;
                GetDeathMember(ddlBrch.SelectedValue, "");
            }
            finally
            {
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabClaimeState.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if ((vStateEdit == "Add" || vStateEdit == null))
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                StatusButton("View");
                LoadGrid(ddlBrch.SelectedValue, "N");
                tabClaimeState.ActiveTabIndex = 1;
                ViewState["StateEdit"] = null;
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vLoanId = ddlLoanNo.SelectedValue;
            string vWhoDied = ddlWhoDied.SelectedValue;
            DateTime vDeathDate = gblFuction.setDate(txtDDate.Text);
           // string vWhoDied = "S";
            Int32 vErr = 0;
            string vNomIdProofDoc = "", vDeathCertificateDoc = "", vNomBankPassbookDoc = "", vClaimentStatementDoc = "";
            if (fuNphoto.HasFile)
            {
                vNomIdProofDoc = System.IO.Path.GetExtension(fuNphoto.FileName);
            }
            else
            {
                vNomIdProofDoc = lblNomIdPrfExt.Text;
            }

            if (fuDC.HasFile)
            {
                vDeathCertificateDoc = System.IO.Path.GetExtension(fuDC.FileName);
            }
            else
            {
                vDeathCertificateDoc = lblDC.Text;
            }

            if (fuCS.HasFile)
            {
                vClaimentStatementDoc = System.IO.Path.GetExtension(fuCS.FileName);
            }
            else
            {
                vClaimentStatementDoc = lblCS.Text;
            }

            if (fuBP.HasFile)
            {
                vNomBankPassbookDoc = System.IO.Path.GetExtension(fuBP.FileName);
            }
            else
            {
                vNomBankPassbookDoc = lblBP.Text;
            }

            CClaimeStatement oCc = null;
            try
            {
                if (Mode == "Save")
                {
                    oCc = new CClaimeStatement();
                    vErr = oCc.SaveMemberDeathDocument(vLoanId, Convert.ToInt32(Session[gblValue.UserId]), vNomIdProofDoc, vDeathCertificateDoc,
                    vNomBankPassbookDoc, vClaimentStatementDoc, "Save", ddlNomGen.SelectedValue, txtNomMob.Text, txtNomPin.Text, txtBankName.Text
                    , txtBranchName.Text, txtAccNo.Text, txtIfsc.Text, txtBenFishName.Text, vWhoDied, vDeathDate);
                    if (vErr > 0)
                    {
                        if (fuNphoto.HasFile)
                        {
                            SaveImages(fuNphoto, "Nominee_PhotoId_Proof");
                        }
                        if (fuDC.HasFile)
                        {
                            SaveImages(fuDC, "Death_Certificate");
                        }
                        if (fuCS.HasFile)
                        {
                            SaveImages(fuCS, "Claiment_Statement");
                        }
                        if (fuBP.HasFile)
                        {
                            SaveImages(fuBP, "Nominee_Bank_PassBook");
                        }
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oCc = new CClaimeStatement();

                    vErr = oCc.SaveMemberDeathDocument(vLoanId, Convert.ToInt32(Session[gblValue.UserId]), vNomIdProofDoc, vDeathCertificateDoc,
                    vNomBankPassbookDoc, vClaimentStatementDoc, "Edit", ddlNomGen.SelectedValue, txtNomMob.Text, txtNomPin.Text, txtBankName.Text
                    , txtBranchName.Text, txtAccNo.Text, txtIfsc.Text, txtBenFishName.Text, vWhoDied, vDeathDate);
                    if (vErr > 0)
                    {
                        if (fuNphoto.HasFile)
                        {
                            SaveImages(fuNphoto, "Nominee_PhotoId_Proof");
                        }
                        if (fuDC.HasFile)
                        {
                            SaveImages(fuDC, "Death_Certificate");
                        }
                        if (fuCS.HasFile)
                        {
                            SaveImages(fuCS, "Claiment_Statement");
                        }
                        if (fuBP.HasFile)
                        {
                            SaveImages(fuBP, "Nominee_Bank_PassBook");
                        }
                        gblFuction.MsgPopup(gblMarg.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                return vResult;
            }
            finally
            {
                oCc = null;
            }
        }

        private string SaveImages(FileUpload flup, string imageName)
        {
            string isImageSaved = "N";
            try
            {
                string extension = System.IO.Path.GetExtension(flup.FileName);
                if (MinioYN == "N")
                {
                    ImagePath = ConfigurationManager.AppSettings["pathDeathMember"];
                    string folderPath = string.Format("{0}/{1}", ImagePath, ddlLoanNo.SelectedValue);
                    System.IO.Directory.CreateDirectory(folderPath);
                    flup.SaveAs(folderPath + "\\" + imageName + extension);
                    isImageSaved = "Y";
                }
                else
                {
                    string vImgName = extension.ToLower() == ".pdf" ? ddlLoanNo.SelectedValue + "_" + imageName + extension : imageName + extension;
                    CApiCalling oAC = new CApiCalling();
                    byte[] vFileByte = ConvertFileToByteArray(flup.PostedFile);
                    isImageSaved = oAC.UploadFileMinio(vFileByte, vImgName, ddlLoanNo.SelectedValue, DeathDocBucket, MinioUrl);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isImageSaved;
        }

        private void LoadGrid(string pBranchCode, string pMode)
        {
            DataTable dt = null;
            CClaimeStatement oPM = null;
            try
            {
                oPM = new CClaimeStatement();
                dt = oPM.GetClaimeStatementList(pBranchCode, pMode);
                gvClaime.DataSource = dt.DefaultView;
                gvClaime.DataBind();
            }
            finally
            {
                dt = null;
                oPM = null;
            }
        }

        protected void gvClaime_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vLoanId = "";
            DataTable dt = null;
            CClaimeStatement oPM = null;
            try
            {
                vLoanId = Convert.ToString(e.CommandArgument);
                ViewState["LoanId"] = vLoanId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvClaime.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oPM = new CClaimeStatement();
                    dt = oPM.GetClaimeStatementByLoanId(vLoanId);
                    if (dt.Rows.Count > 0)
                    {
                        GetDeathMember(ddlBrch.SelectedValue, vLoanId);
                        ddlMem.SelectedIndex = ddlMem.Items.IndexOf(ddlMem.Items.FindByValue(Convert.ToString(dt.Rows[0]["MemberID"])));
                        ddlMem_SelectedIndexChanged(new object(), new EventArgs());
                        ddlLoanNo.SelectedIndex = ddlLoanNo.Items.IndexOf(ddlLoanNo.Items.FindByValue(Convert.ToString(dt.Rows[0]["LoanId"])));
                        lblLoanId.Text = dt.Rows[0]["LoanId"].ToString();
                        if (dt.Rows[0]["NomIdProofDoc"].ToString() != "")
                        {
                            btnPhotoIdDwn.Visible = true;
                            lblNomIdPrfExt.Text = dt.Rows[0]["NomIdProofDoc"].ToString();
                        }
                        if (dt.Rows[0]["DeathCertificateDoc"].ToString() != "")
                        {
                            lblDC.Text = dt.Rows[0]["DeathCertificateDoc"].ToString();
                            btnDc.Visible = true;
                        }
                        if (dt.Rows[0]["NomBankPassbookDoc"].ToString() != "")
                        {
                            lblBP.Text = dt.Rows[0]["NomBankPassbookDoc"].ToString();
                            btnBP.Visible = true;
                        }
                        if (dt.Rows[0]["ClaimentStatementDoc"].ToString() != "")
                        {
                            btnCS.Visible = true;
                            lblCS.Text = dt.Rows[0]["ClaimentStatementDoc"].ToString();
                        }

                        txtNomName.Text = dt.Rows[0]["NomName"].ToString();
                        txtNomAddress.Text = dt.Rows[0]["NomAddress"].ToString();
                        txtNomRelation.Text = dt.Rows[0]["HumanRelationName"].ToString();
                        txtNomAge.Text = dt.Rows[0]["NomAge"].ToString();
                        LblICId.Text = dt.Rows[0]["ICId"].ToString();
                        ddlNomGen.SelectedIndex = ddlNomGen.Items.IndexOf(ddlNomGen.Items.FindByValue(Convert.ToString(dt.Rows[0]["NomGender"])));
                        txtNomMob.Text = dt.Rows[0]["NomMobile"].ToString();
                        txtNomPin.Text = dt.Rows[0]["NomPin"].ToString();
                        txtAccNo.Text = dt.Rows[0]["AccountNo"].ToString();
                        txtIfsc.Text = dt.Rows[0]["IfscCode"].ToString();
                        txtBenFishName.Text = dt.Rows[0]["BenFishName"].ToString();
                        txtBankName.Text = dt.Rows[0]["BankName"].ToString();
                        txtBranchName.Text = dt.Rows[0]["BankBranch"].ToString();
                        txtDDate.Text = dt.Rows[0]["DDate"].ToString();
                        if (dt.Rows[0]["WhoDied"].ToString() != "")
                        {
                            ddlWhoDied.SelectedValue = dt.Rows[0]["WhoDied"].ToString();
                        }
                        else
                        {
                            ddlWhoDied.SelectedIndex = -1;
                        }
                        tabClaimeState.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oPM = null;
            }
        }

        protected void DownloadDoc(string vLoanId, string vFileName)
        {
            try
            {               
                ImagePath = ConfigurationManager.AppSettings["pathDeathMember"];
                string filename = string.Format("{0}/{1}/{2}", ImagePath, vLoanId, vFileName);
                Int32 flength = filename.Length;
                string fname;
                fname = vFileName;
                if (fname != "")
                {                   
                    if (File.Exists(filename))
                    {
                        Response.AddHeader("Content-Type", "Application/octet-stream");
                        Response.AddHeader("Content-Disposition", "attachment;   filename=" + fname);
                        Response.WriteFile(filename);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        string[] arrPathNetwork = PathDeathDoc.Split(',');
                        string vPathDeathDoc = "";
                        for (int i = 0; i <= arrPathNetwork.Length - 1; i++)
                        {
                            if (isValidUrl(arrPathNetwork[i] + "DeathMember/" + vLoanId + "/" + vFileName))
                            {
                                vPathDeathDoc = arrPathNetwork[i] + "DeathMember/" + vLoanId + "/" + vFileName;
                                break;
                            }
                            else if (isValidUrl(arrPathNetwork[i] + "jlgdeathmember/" + vLoanId + "_" + vFileName))
                            {
                                vPathDeathDoc = arrPathNetwork[i] + "jlgdeathmember/" + vLoanId + "_" + vFileName;
                                break;
                            }
                        }
                        if (vPathDeathDoc != "")
                        {
                            WebClient cln = new WebClient();
                            byte[] vDoc = cln.DownloadData(vPathDeathDoc);
                            Response.AddHeader("Content-Type", "Application/octet-stream");
                            Response.AddHeader("Content-Disposition", "attachment;   filename=" + fname);
                            Response.BinaryWrite(vDoc);
                            Response.Flush();
                            Response.End();
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("No Data Found..");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        protected void btnPhotoIdDwn_Click(object sender, EventArgs e)
        {
            DownloadDoc(lblLoanId.Text, "Nominee_PhotoId_Proof" + lblNomIdPrfExt.Text);
        }

        protected void btnBP_Click(object sender, EventArgs e)
        {
            DownloadDoc(lblLoanId.Text, "Nominee_Bank_PassBook" + lblBP.Text);
        }

        protected void btnDc_Click(object sender, EventArgs e)
        {
            DownloadDoc(lblLoanId.Text, "Death_Certificate" + lblDC.Text);
        }

        protected void btnCS_Click(object sender, EventArgs e)
        {
            DownloadDoc(lblLoanId.Text, "Claiment_Statement" + lblCS.Text);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.CanEdit == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Edit);
                return;
            }
            ViewState["StateEdit"] = "Edit";
            StatusButton("Edit");
            ddlMem.Enabled = false;
            ddlLoanNo.Enabled = false;
            ddlWhoDied.Enabled = true;
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

        #region URLExist
        public static bool isValidUrl(string pUrl)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pUrl);
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
    }
}