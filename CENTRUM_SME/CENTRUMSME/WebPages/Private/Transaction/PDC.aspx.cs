using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CENTRUMCA;
using System.IO;
using CENTRUMBA;
using System.Configuration;
using System.Net;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class PDC : CENTRUMBAse
    {
        string vPathImage = "";
        string PDCBucket = ConfigurationManager.AppSettings["PDCBucket"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                StatusButton("View");
                GenerateGrid1();
                ViewState["PDCID"] = 0;               
                txtFromDt.Text = txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                LoadGrid();
            }

        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "PDC";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuPDC);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "PDC", false);
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
            txtPDCDt.Enabled = Status;
            txtLoanAppNo.Enabled = Status;
            txtMemberName.Enabled = Status;
            gvChequeDtl.Enabled = Status;
            fuCheque.Enabled = Status;
        }

        private void ClearControls()
        {
            txtLoanAppNo.Text = "";
            txtMemberName.Text = "";
            txtPDCDt.Text = "";
            ViewState["PDCID"] = 0;
            GenerateGrid1();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            Int32 vR = 0;
            DataRow dr;
            dt = (DataTable)ViewState["ChequeDtl"];
            if (dt.Rows.Count > 0)
            {
                vR = dt.Rows.Count - 1;
                TextBox txtChequeNo = (TextBox)gvChequeDtl.Rows[vR].FindControl("txtChequeNo");
                TextBox txtMICRCode = (TextBox)gvChequeDtl.Rows[vR].FindControl("txtMICRCode");
                TextBox txtBankName = (TextBox)gvChequeDtl.Rows[vR].FindControl("txtBankName");
                TextBox txtIfsCode = (TextBox)gvChequeDtl.Rows[vR].FindControl("txtIfsCode");
                TextBox txtChequeDt = (TextBox)gvChequeDtl.Rows[vR].FindControl("txtChequeDt");

                if (txtChequeNo.Text == "")
                {
                    gblFuction.AjxMsgPopup("Cheque No can not be left blank");
                    return;
                }
                if (txtMICRCode.Text == "")
                {
                    gblFuction.AjxMsgPopup("MICR Code can not be left blank");
                    return;
                }
                if (txtBankName.Text == "")
                {
                    gblFuction.AjxMsgPopup("Bank Name can not be left blank");
                    return;
                }
                if (txtIfsCode.Text == "")
                {
                    gblFuction.AjxMsgPopup("IFS Code can not be left blank");
                    return;
                }
                if (txtChequeDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Cheque Date can not be left blank");
                    return;
                }

                dt.Rows[vR]["ChequeNo"] = txtChequeNo.Text;
                dt.Rows[vR]["MICRCode"] = txtMICRCode.Text;
                dt.Rows[vR]["BankName"] = txtBankName.Text;
                dt.Rows[vR]["IFSCode"] = txtIfsCode.Text;
                dt.Rows[vR]["ChequeDt"] = txtChequeDt.Text;
            }
            dt.AcceptChanges();

            //if (dt.Rows[vR]["AssetName"].ToString() != "-1")
            //{

            dr = dt.NewRow();
            dt.Rows.Add(dr);
            //}
            //else
            //{
            //    gblFuction.MsgPopup("Asset name is Blank...");
            //}
            ViewState["ChequeDtl"] = dt;
            gvChequeDtl.DataSource = dt;
            gvChequeDtl.DataBind();
        }

        protected void ImDel_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton ImDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)ImDel.NamingContainer;
            GridView gv = (GridView)ImDel.Parent.Parent.NamingContainer;

            dt = (DataTable)ViewState["ChequeDtl"];
            if (dt.Rows.Count > 1)
            {
                dt.Rows[gR.RowIndex].Delete();
                dt.AcceptChanges();
                ViewState["ChequeDtl"] = dt;
                gvChequeDtl.DataSource = dt;
                gvChequeDtl.DataBind();
                //SetData();             
            }
            else if (dt.Rows.Count == 1)
            {
                gblFuction.MsgPopup("First Row can not be deleted.");
                return;
            }
        }

        private void GenerateGrid1()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SLNo", typeof(int));
            dt.Columns.Add("ChequeNo", typeof(string));
            dt.Columns.Add("MICRCode", typeof(string));
            dt.Columns.Add("BankName", typeof(string));
            dt.Columns.Add("IFSCode", typeof(string));
            dt.Columns.Add("ChequeDt", typeof(string));
            ViewState["ChequeDtl"] = dt;
            try
            {
                DataRow dF;
                dF = dt.NewRow();
                dt.Rows.Add(dF);
                dt.AcceptChanges();

                gvChequeDtl.DataSource = dt;
                gvChequeDtl.DataBind();
                // StatusButton("Show");
            }

            finally
            {
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
                tbPDC.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbPDC.ActiveTabIndex = 0;
            EnableControl(false);
            ClearControls();
            StatusButton("View");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null || vStateEdit == "")
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                LoadGrid();
                StatusButton("Show");
                ViewState["StateEdit"] = null;
            }

        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 vErr = 0, vPDCId = 0;
            string vXml = "";
            CLoanRecovery oLR = null;
            string strpath = System.IO.Path.GetExtension(fuCheque.FileName);
            DataTable dt = (DataTable)ViewState["ChequeDtl"];
            vPDCId = Convert.ToInt32(ViewState["PDCID"].ToString());
            vPathImage = ConfigurationManager.AppSettings["pathPDC"];
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vXml = oSW.ToString();
            }
            try
            {
                if (Mode == "Save")
                {
                    oLR = new CLoanRecovery();
                    vErr = oLR.SavePDC(ref vPDCId, hdnLoanAppId.Value, vXml, gblFuction.setDate(txtPDCDt.Text), strpath,
                        Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save");

                    if (vErr == 1)
                    {
                        if (fuCheque.HasFile)
                        {                          
                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, vPDCId);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, "Cheque" + strpath);
                                fuCheque.SaveAs(filePath);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                byte[] vFileByte = ConvertFileToByteArray(fuCheque.PostedFile);
                                oAC.UploadFileMinio(vFileByte, strpath.ToLower() == ".pdf" ? vPDCId.ToString() + "_Cheque" + strpath : "Cheque" + strpath, vPDCId.ToString(), PDCBucket, MinioUrl);
                            }
                        }
                       
                        ViewState["PDCID"] = vPDCId;
                        vResult = true;
                    }

                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                if (Mode == "Edit")
                {
                    oLR = new CLoanRecovery();
                    vErr = oLR.SavePDC(ref vPDCId, hdnLoanAppId.Value, vXml, gblFuction.setDate(txtPDCDt.Text), strpath,
                        Convert.ToInt32(Session[gblValue.UserId].ToString()), "Edit");
                    if (vErr == 1)
                    {
                        if (fuCheque.HasFile)
                        {                            
                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, vPDCId);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, "Cheque" + strpath);
                                fuCheque.SaveAs(filePath);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                byte[] vFileByte = ConvertFileToByteArray(fuCheque.PostedFile);
                                oAC.UploadFileMinio(vFileByte, strpath.ToLower() == ".pdf" ? vPDCId.ToString() + "_Cheque" + strpath : "Cheque" + strpath, vPDCId.ToString(), PDCBucket, MinioUrl);
                            }
                        }
                        gblFuction.AjxMsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }

                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                if (Mode == "Delete")
                {
                    oLR = new CLoanRecovery();
                    vErr = oLR.SavePDC(ref vPDCId, hdnLoanAppId.Value, vXml, gblFuction.setDate(txtPDCDt.Text), strpath,
                        Convert.ToInt32(Session[gblValue.UserId].ToString()), "Delete");

                    if (vErr == 1)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oLR = null;
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
                    //LoadGrid(1);
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadGrid()
        {
            DataTable dt = null;
            string vBrCode = "";
            CLoanRecovery oBr = null;
            try
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oBr = new CLoanRecovery();
                dt = oBr.GetPDCList(gblFuction.setDate(txtFromDt.Text), gblFuction.setDate(txtToDt.Text),txtLoanAppIdSearch.Text, vBrCode);
                gvChequeDtlList.DataSource = dt;
                gvChequeDtlList.DataBind();

            }
            finally
            {
                oBr = null;
                dt = null;
            }
        }

        protected void gvChequeDtlList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 pPDCID = 0;
            CLoanRecovery oLR = new CLoanRecovery();
            DataSet ds = new DataSet();
            DataTable dt, dt1 = null;      

            if (e.CommandName == "cmdShow")
            {
                pPDCID = Convert.ToInt32(e.CommandArgument);
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                foreach (GridViewRow gr in gvChequeDtlList.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;
                ds = oLR.GetPDCByPDCId(pPDCID);
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                if (dt.Rows.Count > 0)
                {
                    txtLoanAppNo.Text = dt.Rows[0]["LoanAppNo"].ToString();
                    hdnLoanAppId.Value = dt.Rows[0]["LoanAppId"].ToString();
                    txtMemberName.Text = dt.Rows[0]["CompanyName"].ToString();
                    txtPDCDt.Text = dt.Rows[0]["PDCDate"].ToString();
                    ViewState["PDCID"] = pPDCID;
                }
                if (dt1.Rows.Count > 0)
                {
                    gvChequeDtl.DataSource = dt1;
                    gvChequeDtl.DataBind();
                    ViewState["ChequeDtl"] = dt1;
                }
                else
                {
                    GenerateGrid1();
                }
                tbPDC.ActiveTabIndex = 1;
                StatusButton("Show");
                EnableControl(false);
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

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["pathPDC"];
            ImageButton btn = sender as ImageButton;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lblPDCId = (Label)gvrow.FindControl("lblPDCId");
            Label lblDocType = (Label)gvrow.FindControl("lblDocType");
            if (lblPDCId.Text != "")
            {
                //string folderPath = string.Format("{0}/{1}", vPathImage, lblPDCId.Text);
                //string filePath = string.Format("{0}/{1}", folderPath, "Cheque" + lblDocType.Text);
                if (lblDocType.Text != "")
                {
                    string pathNetwork = ConfigurationManager.AppSettings["PdcURL"];
                    string[] arrPathNetwork = pathNetwork.Split(',');
                    string vPathDigiDoc = "";
                    for (int i = 0; i <= arrPathNetwork.Length - 1; i++)
                    {
                        if (URLExist(arrPathNetwork[i] + lblPDCId.Text + "_Cheque" + lblDocType.Text))
                        {
                            vPathDigiDoc = arrPathNetwork[i] + lblPDCId.Text + "_Cheque" + lblDocType.Text;
                            break;
                        }
                        if (URLExist(arrPathNetwork[i] + lblPDCId.Text + "/Cheque" + lblDocType.Text))
                        {
                            vPathDigiDoc = arrPathNetwork[i] + lblPDCId.Text + "/Cheque" + lblDocType.Text;
                            break;
                        }
                        //if (URLExist(arrPathNetwork[i] + lblPDCId.Text + "_Document" + lblDocType.Text))
                        //{
                        //    vPathDigiDoc = arrPathNetwork[i] + lblPDCId.Text + "_Document" + lblDocType.Text;
                        //    break;
                        //}
                    }
                    if (vPathDigiDoc != "")
                    {
                        WebClient cln = null;
                        byte[] vDoc = null;
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                        cln = new WebClient();
                        vDoc = cln.DownloadData(vPathDigiDoc);
                        Response.AddHeader("Content-Type", "Application/octet-stream");
                        Response.AddHeader("Content-Disposition", "attachment;   filename=" + lblPDCId.Text + "_Cheque" + lblDocType.Text);
                        Response.BinaryWrite(vDoc);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("File not Exists");
                    }
                }
                //if (File.Exists(filePath))
                //{
                //    if (lblDocType.Text.ToUpper() == ".JPG")
                //    {
                //        Response.ContentType = "image/jpg";
                //    }
                //    else if(lblDocType.Text.ToUpper() == ".PDF")
                //    {
                //        Response.ContentType = "application/pdf";
                //    }
                //    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "Cheque" + lblDocType.Text + "\"");
                //    Response.TransmitFile(filePath);
                //    Response.End();
                //    return;
                //}
                //else
                //{
                   
                //}
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
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

        #region URLExist
        private bool URLExist(string pPath)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pPath);
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
    }
}