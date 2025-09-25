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
    public partial class HOLegal : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                //////hdUserID.Value = this.UserID.ToString();
                LoanLegBrCode();
                LoadHOLegList();
                ViewState["StateEdit"] = null;
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
                    btnSave.Enabled = true;
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
        private void LoanLegBrCode()
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();
            dt = oDisb.GetBranchForHOLegal(Session[gblValue.BrnchCode].ToString());
            if (dt.Rows.Count > 0)
            {
                ddlLegBr.DataSource = dt;
                ddlLegBr.DataValueField = "BranchCode";
                ddlLegBr.DataTextField = "BranchName";
                ddlLegBr.DataBind();
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    ListItem liSel = new ListItem("ALL", "ALL");
                    ddlLegBr.Items.Insert(0, liSel);
                }
            }
            else
            {
                ddlLegBr.DataSource = null;
                ddlLegBr.DataBind();
            }
        }
        private void ClearControls()
        {

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
                this.PageHeading = "Head Office Legal Dcument Received";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHOLegal);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Head Office Legal Dcument Received", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void LoadHOLegList()
        {
            DataTable dt = null;
            CApplication oCA = null;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                vBrCode = ddlLegBr.SelectedValue.ToString();
                oCA = new CApplication();
                dt = oCA.GetHOLegalList(vBrCode, ddlSearchType.SelectedValue.ToString());
                if (dt.Rows.Count > 0)
                {
                    gvHOLegal.DataSource = dt;
                    gvHOLegal.DataBind();
                }
                else
                {
                    gvHOLegal.DataSource = null;
                    gvHOLegal.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadHOLegList();
        }
        protected void gvLegDone_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            CApplication oCA = new CApplication();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                string  vFileType = "", vFileName = "", filename = "";
                //string vFolderPath = "E:\\WebApps\\PratamMobService\\Legal";
                string vFolderPath = ConfigurationManager.AppSettings["LegalShowPath"];
                if (e.CommandName == "cmdDown")
                {
                    GridViewRow gvRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    vFileType = e.CommandArgument.ToString();
                    vFileName = gvRow.Cells[6].Text.ToString();
                    filename = vFolderPath + "\\" + vFileType + "\\" + vFileName;
                    
                    if (vFileName != "" && vFileName != "&nbsp;")
                    {
                        Response.AddHeader("Content-Type", "Application/octet-stream");
                        Response.AddHeader("Content-Disposition", "attachment;   filename=" + vFileName);
                        Response.WriteFile(filename);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("No Attachement Found...");
                        return;
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
            SaveRecords();
        }
        protected void gvLegDone_RowDataBound(object senser, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblHORecYN = (Label)e.Row.FindControl("lblHORecYN");
                TextBox txtApprvDt = (TextBox)e.Row.FindControl("txtApprvDt");
                CheckBox chkApp = (CheckBox)e.Row.FindControl("cbApprv");
                if (lblHORecYN.Text == "N")
                    chkApp.Checked = false;
                else
                    chkApp.Checked = true;
                if (txtApprvDt.Text == "")
                {
                    txtApprvDt.Text = Session[gblValue.LoginDate].ToString();
                }
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
        protected void cbApprvSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk;
            foreach (GridViewRow rowItem in gvHOLegal.Rows)
            {
                chk = (CheckBox)(rowItem.Cells[8].FindControl("cbApprv"));
                chk.Checked = ((CheckBox)sender).Checked;
            }
        }
        private DataTable CreateXmlData()
        {
            DataTable dt = new DataTable("HOLegal");
            dt.Columns.Add("LoanAppId");
            dt.Columns.Add("HORecYN");
            dt.Columns.Add("HORecDate");
            DataRow dr;
            string HoRecYN = "";
            foreach (GridViewRow gr in gvHOLegal.Rows)
            {
                TextBox txtApprvDt = (TextBox)gr.FindControl("txtApprvDt");
                if (txtApprvDt.Text != "")
                {
                    CheckBox cbApprv = (CheckBox)gr.FindControl("cbApprv");
                    if (cbApprv.Checked == true)
                        HoRecYN = "Y";
                    else
                        HoRecYN = "N";
                    dr = dt.NewRow();
                    dr["LoanAppId"] = gr.Cells[1].Text;
                    dr["HORecYN"] = HoRecYN;
                    dr["HORecDate"] = gblFuction.setDate(txtApprvDt.Text.ToString());
                    dt.Rows.Add(dr);
                }
            }
            dt.AcceptChanges();
            return dt;
        }
        private void SaveRecords()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            if (ValidateFields() == false)
                return;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            CFundSource oFS = null;
            DataTable dtXml = CreateXmlData();
            int vErr = 0;
            string vXml = "";
            try
            {
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString();
                }
                oFS = new CFundSource();
                vErr = oFS.SaveHOLegalBulk(vXml, this.UserID);
                if (vErr > 0)
                {
                    gblFuction.MsgPopup("Record Updated Successfully");
                    return;
                }
                else
                {
                    gblFuction.MsgPopup("Error: Data not Saved");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtXml = null;
                oFS = null;
            }
        }
        private bool ValidateFields()
        {
            bool vRes = true;
            if (gvHOLegal.Rows.Count <= 0)
            {
                gblFuction.MsgPopup("Nothing Found to be Saved ..");
                return vRes = false;
            }
            return vRes;
        }
    }
}