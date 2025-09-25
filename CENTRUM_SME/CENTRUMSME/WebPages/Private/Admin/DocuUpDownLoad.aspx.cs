using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Admin
{
    public partial class DocuUpDownLoad : CENTRUMBAse
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                StatusButton("View");
                tabAdtPart.ActiveTabIndex = 0;
                txtFmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                LoadGrid();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);
                
                this.Menu = false;
                this.PageHeading = "Documents UpLoad";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDocUpLoad);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
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
                    btnCancel.Visible = false;
                    //btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Document UpLoad", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
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
                    //gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_ddlLoanSector");
                    break;
                case "Show":
                    btnAdd.Enabled = false;
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
                    //gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_ddlLoanSector");
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
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
                case "Load":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtDuDt.Enabled = Status;
            fuAttach.Enabled = Status;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtDuDt.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            lblDate.Text = "";
            lblUser.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vYrNo"></param>
        private void LoadGrid()
        {
            CDocUpLoad oAu = null;
            DataTable dt = null;
            DateTime vFrmDt, vTodt;
            string vBrCode = Convert.ToString(Session[gblValue.BrnchCode]);
            vFrmDt = gblFuction.setDate(txtFmDt.Text);
            vTodt = gblFuction.setDate(txtToDt.Text);
            try
            {
                oAu = new CDocUpLoad();
                dt = oAu.GetAllUpLoad(vFrmDt,vTodt, vBrCode);
                if (dt.Rows.Count > 0)
                {
                    gvAdtPart.DataSource = dt;
                    gvAdtPart.DataBind();
                }
            }
            finally
            {
                oAu = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAdtPart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vDocUpID = "";
            Int32 vRow = 0;
            DataTable dt = null;
            CDocUpLoad oAD = null;
            try
            {
                vDocUpID = Convert.ToString(e.CommandArgument);
                ViewState["DocUpID"] = vDocUpID;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    ViewState["AuditID"] = gvRow.Cells[0].Text;
                    /**
                    foreach (GridViewRow gr in gvAdtPart.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    **/

                    Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvAdtPart.Rows)
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

                    oAD = new CDocUpLoad();
                    dt = oAD.GetUpDtlById(vDocUpID);
                    if (dt.Rows.Count > 0)
                    {
                        txtDS.Text = Convert.ToString(dt.Rows[vRow]["DocSub"]).Trim();
                        txtDuDt.Text = Convert.ToString(dt.Rows[vRow]["DocUpDt"]).Trim();
                        txtDet.Text = Convert.ToString(dt.Rows[vRow]["DocSubDetails"]).Trim();
                        lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tabAdtPart.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                    else
                    {
                        //txtDS.Text = btnShow.Text;
                        tabAdtPart.ActiveTabIndex = 1;
                        StatusButton("View");
                    }
                }
                if (e.CommandName == "cmdDownld")
                {
                    string vFileName = "Attachment";
                    string vFileTyp = "";
                    GridViewRow gvR = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    ImageButton imgDownload = (ImageButton)gvR.FindControl("imgDownload");
                    oAD = new CDocUpLoad();
                    dt = oAD.GetDocUpAttachById(vDocUpID);
                    if (dt.Rows.Count > 0)
                    {
                        byte[] fileData = (byte[])dt.Rows[0]["AttachDoc"];
                        vFileTyp = GetFileType(Convert.ToString(dt.Rows[0]["AttachType"]));
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = vFileTyp;
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + vFileName + "" + Convert.ToString(dt.Rows[0]["AttachType"]));
                        BinaryWriter bw = new BinaryWriter(Response.OutputStream);
                        bw.Write(fileData);
                        bw.Close();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("No Attachment Found");
                        return;
                    }
                }
            }
            finally
            {
                //dt.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFileTyp"></param>
        /// <returns></returns>
        private string GetFileType(string pFileTyp)
        {
            string vRst = "";
            switch (pFileTyp)
            {
                case ".txt":
                    vRst = "application/notepad";
                    break;
                case ".doc":
                    vRst = "application/ms-word";
                    break;
                case ".docx":
                    vRst = "application/vnd.ms-word.document.12";
                    break;
                case ".xls":
                    vRst = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    vRst = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".pdf":
                    vRst = "application/vnd.pdf";
                    break;
                case ".zip":
                    vRst = "application/zip";
                    break;
                case ".rar":
                    vRst = "application/WinRar";
                    break;
                case ".exe":
                    vRst = "application/executable";
                    break;
                default:
                    vRst = "";
                    break;
            }
            return vRst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidateField()
        {
            bool vRes = true;
            DateTime dtm;
           

            //if (fuAttach.HasFile==false)
            //{
            //    gblFuction.MsgPopup("Please Attach a File");
            //    return vRes = false;
            //}
            if (txtDuDt.Text.Trim() == "" || gblFuction.IsDate(txtDuDt.Text) == false)
            {
                gblFuction.MsgPopup("Please Enter a Valid Date");
                gblFuction.focus("ctl00_cph_Main_tabAdtPart_pnlDtls_txtStDt");
                return vRes = false;
            }
         
            if (txtDS.Text =="")
            {
                gblFuction.MsgPopup("Document Subject Cannot be Left Blank ..");
                gblFuction.focus("ctl00_cph_Main_tabAdtPart_pnlDtls_ddlAccept");
                return vRes = false;
            }
            return vRes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            byte[] vDoc = null;
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vDocUpID = Convert.ToString(ViewState["DocUpID"]);
            string vBranch = Convert.ToString(Session[gblValue.BrnchCode]);
            string vAttchTyp = "";
            Int32 vErr = 0; 
            CDocUpLoad oAD = null;
            try
            {
                if (ValidateField() == false && Mode != "Delete")
            return false;

                if (Mode == "Save")
                {
                    if (fuAttach.HasFile == false)
                    {
                        gblFuction.MsgPopup("Please Attach a File");
                        return false;
                    }
                    if (fuAttach.PostedFile.InputStream.Length > 4194304)
                    {
                        gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
                        return false;
                    }
                    if (fuAttach.HasFile)
                    {
                        vDoc = new byte[fuAttach.PostedFile.InputStream.Length + 1];
                        fuAttach.PostedFile.InputStream.Read(vDoc, 0, vDoc.Length);
                        vAttchTyp = System.IO.Path.GetExtension(fuAttach.FileName).ToLower();
                    }
                    oAD = new CDocUpLoad();
                    vErr = oAD.InsertDocUp(vDocUpID, gblFuction.setDate(txtDuDt.Text.Trim()), vDoc, vAttchTyp,
                        txtDS.Text.Trim().Replace("'", ""), txtDet.Text.Trim().Replace("'", ""), this.UserID, "Save", vBranch);
                    if (vErr > 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    if (fuAttach.HasFile == false)
                    {
                        gblFuction.MsgPopup("Please Attach a File");
                        return false;
                    }
                    if (fuAttach.PostedFile.InputStream.Length > 4194304)
                    {
                        gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
                        return false;
                    }
                    if (fuAttach.HasFile)
                    {
                        vDoc = new byte[fuAttach.PostedFile.InputStream.Length + 1];
                        fuAttach.PostedFile.InputStream.Read(vDoc, 0, vDoc.Length);
                        vAttchTyp = System.IO.Path.GetExtension(fuAttach.FileName).ToLower();
                    }
                   oAD = new CDocUpLoad();
                    vErr = oAD.InsertDocUp(vDocUpID, gblFuction.setDate(txtDuDt.Text.Trim()), vDoc, vAttchTyp,
                        txtDS.Text.Trim().Replace("'", ""), txtDet.Text.Trim().Replace("'", ""), this.UserID, "Edit", vBranch);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oAD = new CDocUpLoad();
                    vErr = oAD.InsertDocUp(vDocUpID, gblFuction.setDate(txtDuDt.Text.Trim()), vDoc, vAttchTyp,
                        txtDS.Text.Trim().Replace("'", ""), txtDet.Text.Trim().Replace("'", ""), this.UserID, "Del", vBranch);
                    if (vErr > 0)
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
                oAD = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
            ViewState["DocUpID"] = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                tabAdtPart.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    LoadGrid();
                    ClearControls();
                    tabAdtPart.ActiveTabIndex = 0;
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabAdtPart.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                LoadGrid();
                StatusButton("View");
                tabAdtPart.ActiveTabIndex = 0;
                ViewState["StateEdit"] = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_OnClick(object sender, EventArgs e)
        {
            LoadGrid();
        }
    }
}