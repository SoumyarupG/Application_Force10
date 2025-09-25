using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;
using SendSms;
using System.Web;
using System.Configuration;

namespace CENTRUMSME.WebPages.Private.Legal
{
    public partial class BranchLegal : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                InitBasePage();
                //////hdUserID.Value = this.UserID.ToString();
                LoadPendingLegList();
                txtBrLegDate.Text = Session[gblValue.LoginDate].ToString();
                LoadLegList();
                ViewState["StateEdit"] = null;
                btnSave.Enabled = false;
                StatusButton("View");
                tabBranchLeg.ActiveTabIndex = 0;
            }
        }
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnSave.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnSave.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnSave.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnSave.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnSave.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        private void ClearControls()
        {
            txtLnAppNo.Text = "";
            txtCustName.Text = "";
            txtCoAppNm.Text = "";
            txtBrLegDate.Text = Session[gblValue.LoginDate].ToString();

        }
        private void EnableControl(Boolean Status)
        {
            /*******************/
            //ddlBranch.Enabled = Status;
            //ddlLoanType.Enabled = Status;
            //ddlRpSchdle.Enabled = Status;
            //ddlInstType.Enabled = Status;
            //ddlCust.Enabled = Status;
            //ddlSancNo.Enabled = Status;
            //txtLnDt.Enabled = Status;
            //ddlSrcFund.Enabled = Status;
            //rdbPayMode.Enabled = Status;
            //txtChqNo.Enabled = Status;
            //txtChqDt.Enabled = Status;
            //txtStDt.Enabled = Status;
            //ddlBank.Enabled = Status;
            //ddlLedgr.Enabled = Status;
            //txtRefNo.Enabled = Status;
            //txtInsSerTax.Enabled = Status;
            //txtSancAmt.Enabled = Status;
            //txtSancDt.Enabled = Status;
            //txtLnCycle.Enabled = Status;
            /*************************/
        }
        private void InitBasePage()
        {
            try
            {

                this.Menu = false;
                this.PageHeading = "Branch Legal Process";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBrLegal);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                    return;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                    return;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                    return;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnSave.Visible = true;
                    return;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Branch Legal Process", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void LoadPendingLegList()
        {
            DataTable dt = null;
            CApplication oCA = null;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                // vMode = rdbSel.SelectedValue;
                vBrCode = (string)Session[gblValue.BrnchCode];
                oCA = new CApplication();
                dt = oCA.GetPenLegList("P", vBrCode);
                if (dt.Rows.Count > 0)
                {
                    gvPenLeg.DataSource = dt;
                    gvPenLeg.DataBind();
                }
                else
                {
                    gvPenLeg.DataSource = null;
                    gvPenLeg.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        private void LoadLegList()
        {
            DataTable dt = null;
            CApplication oCA = null;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                // vMode = rdbSel.SelectedValue;
                vBrCode = (string)Session[gblValue.BrnchCode];
                oCA = new CApplication();
                dt = oCA.GetLegList("X", vBrCode);
                if (dt.Rows.Count > 0)
                {
                    gvLegDone.DataSource = dt;
                    gvLegDone.DataBind();
                }
                else
                {
                    gvLegDone.DataSource = null;
                    gvLegDone.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        protected void gvPenLeg_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vLnAppId = "";
            DataTable dt = null;
            CApplication oCG = null;
            try
            {
                vLnAppId = Convert.ToString(e.CommandArgument);
                ViewState["LnSancId"] = vLnAppId;
                if (e.CommandName == "cmdShow")
                {
                    oCG = new CApplication();
                    dt = oCG.GetPenLegDtlByAppId(vLnAppId, "P");
                    if (dt.Rows.Count > 0)
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvPenLeg.Rows)
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
                        gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                        gvRow.ForeColor = System.Drawing.Color.White;
                        gvRow.Font.Bold = true;
                        btnShow.ForeColor = System.Drawing.Color.White;
                        btnShow.Font.Bold = true;

                        ViewState["StateEdit"] = "Add";

                        txtLnAppNo.Text = dt.Rows[0]["LoanAppId"].ToString();
                        txtCustName.Text = dt.Rows[0]["CompanyName"].ToString();
                        txtCoAppNm.Text = dt.Rows[0]["CoAppName"].ToString();
                        btnSave.Enabled = true;
                        tabBranchLeg.ActiveTabIndex = 2;
                    }
                }
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }
        protected void gvLegDone_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            CApplication oCA = new CApplication();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                string vFolderPath = "G:\\WebApps\\CentrumSmeMobService\\Legal", vFileType = "", vFileName = "", filename = "";
                if (e.CommandName == "cmdDown")
                {
                    GridViewRow gvRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    vFileType = e.CommandArgument.ToString();
                    vFileName = gvRow.Cells[7].Text.ToString();
                    filename = vFolderPath + "\\" + vFileType + "\\" + vFileName;
                    if (vFileName != "")
                    {
                        Response.AddHeader("Content-Type", "Application/octet-stream");
                        Response.AddHeader("Content-Disposition", "attachment;   filename=" + vFileName);
                        Response.WriteFile(filename);
                        Response.Flush();
                        Response.End();
                    }
                }
                else if (e.CommandName == "cmdDel")
                {
                    GridViewRow gvRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    vFileType = e.CommandArgument.ToString();
                    vFileName = gvRow.Cells[7].Text.ToString();
                    filename = vFolderPath + "\\" + vFileType + "\\" + vFileName;
                    string pDate = gvRow.Cells[5].Text.ToString();
                    int vDelRec = 0;

                    string pLnAppId = vFileType;
                    vDelRec = oCA.SaveBranchLegal(pLnAppId, gblFuction.setDate(pDate), this.UserID, "Delete", vBrCode, vFileName);
                    if (vDelRec > 0)
                    {
                        if ((System.IO.File.Exists(filename)))
                        {
                            System.IO.File.Delete(filename);
                        }
                        gblFuction.MsgPopup("Record deleted successfully.");
                    }
                }
            }
            finally
            {
                oCA = null;
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string pLnAppId = txtLnAppNo.Text.ToString().Trim();
            string vBrCode = (string)Session[gblValue.BrnchCode];
            Int32 vErr = 0, vRec = 0;
            string vFileNm = "";
            if (pLnAppId == "")
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Blank");
                return;
            }
            if (txtBrLegDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Date No Can Not Blank");
                return;
            }
            DateTime pDate = gblFuction.setDate(txtBrLegDate.Text.ToString());
            CApplication oCA = new CApplication();
            if (fuLegDoc.HasFile == true)
            {
                vFileNm = fuLegDoc.FileName;
                if (vFileNm == "")
                {
                    gblFuction.AjxMsgPopup("Upload File Name Can Not Be Empty");
                    return;
                }

                if (fuLegDoc.PostedFile.InputStream.Length > 600000)
                {
                    gblFuction.MsgPopup("Maximum upload file Size Is 500 KB.");
                    vFileNm = "";
                    return;
                }

            }
            //if (fuLegDoc.HasFile == false)
            //{
            //    gblFuction.AjxMsgPopup("Please Choose File To Upload...");
            //    return;
            //}

            vErr = oCA.SaveBranchLegal(pLnAppId, pDate, this.UserID, "Save", vBrCode, vFileNm);
            if (vErr > 0)
            {

                //vSubId = ddlMem.SelectedValue;
                if (fuLegDoc.HasFile)
                {
                    // Call a helper method routine to save the file.
                    if (fuLegDoc.PostedFile.InputStream.Length > 600000)
                    {
                        gblFuction.MsgPopup("Maximum upload file Size Is 500 KB.");
                        return;
                    }
                    string ext = System.IO.Path.GetExtension(this.fuLegDoc.PostedFile.FileName);
                    if (ext == ".pdf" || ext == ".PDF" || ext == ".xls" || ext == ".doc" || ext == ".docx" || ext == ".xlsx" || ext == ".jpeg" || ext == ".jpg" || ext == ".JPEG" || ext == ".JPG")
                    {
                        vFileNm = fuLegDoc.FileName;
                        //string vFolderPath = "E:\\WebApps\\PratamMobService\\Legal";
                        string vFolderPath = ConfigurationManager.AppSettings["LegalShowPath"];
                        vFolderPath = vFolderPath + "\\" + pLnAppId;
                        // Get the name of the file to upload.

                        //If Directory (Folder) does not exists. Create it.
                        if (!Directory.Exists(vFolderPath))
                        {
                            Directory.CreateDirectory(vFolderPath);
                        }

                        vFileNm = fuLegDoc.FileName;

                        // Create the path and file name to check for duplicates.
                        //string pathToCheck = vFolderPath + vFileNm;
                        string pathToCheck = vFolderPath + "\\" + vFileNm;

                        // Create a temporary file name to use for checking duplicates.
                        string tempfileName = "";

                        // Check to see if a file already exists with the
                        // same name as the file to upload.        
                        if (System.IO.File.Exists(pathToCheck))
                        {
                            int counter = 2;
                            while (System.IO.File.Exists(pathToCheck))
                            {
                                // if a file with this name already exists,
                                // prefix the filename with a number.
                                tempfileName = counter.ToString() + vFileNm;
                                pathToCheck = vFolderPath + tempfileName;
                                counter++;
                            }

                            vFileNm = tempfileName;
                            // Notify the user that the file name was changed.
                            gblFuction.MsgPopup("A file with the same name already exists." +
                                "<br />Your file was saved as " + vFileNm);
                            return;
                        }
                        else
                        {
                            vFolderPath = vFolderPath + "\\" + vFileNm;
                            fuLegDoc.SaveAs(vFolderPath);
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("only pdf/excel/doc/Jpeg file can be upload");
                        return;
                    }
                }
                else
                {
                    // Notify the user that a file was not uploaded.Uncomment Line For File Upload Mandatory
                    //gblFuction.MsgPopup("You did not specify a file to upload.");
                }
                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                ClearControls();
                LoadLegList();
                tabBranchLeg.ActiveTabIndex = 1;
            }
            else
            {
                gblFuction.AjxMsgPopup(gblPRATAM.DBError);
            }
        }


        private string getBase64String(FileUpload flup)
        {
            string base64String = "";

            try
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(flup.PostedFile.InputStream))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }
            catch (Exception ex)
            {
                //DBUtility.WriteErrorLog(ex);
            }

            return base64String;

        }

    }
}