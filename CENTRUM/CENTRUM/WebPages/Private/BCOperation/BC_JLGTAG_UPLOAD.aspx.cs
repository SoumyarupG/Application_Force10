using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using FORCEBA;
using FORCECA;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using CENTRUM.WebSrvcs;
using System.Net.Mail;
namespace CENTRUM.WebPages.Private.BCOperation
{
    public partial class BC_JLGTAG_UPLOAD : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["myfirstLoad"] != null)
            //{
            InitBasePage();
            if (!IsPostBack)
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Add");
                else
                    StatusButton("Add");
                ViewState["StateEdit"] = null;
                ViewState["SFTPUPLOAD"] = null;
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                tabPurps.ActiveTabIndex = 0;
                PopBranch(Session[gblValue.UserName].ToString());
                // popFO();
                LoadGrid(0);
            }
            //}
            //else
            //{
            //    if (DateTime.Now > Convert.ToDateTime(Session["myfirstLoad"]).AddSeconds(60))
            //        gblFuction.MsgPopup("Hi");
            //} 
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);


                this.Menu = false;
                this.PageHeading = "SFTP UPLOAD JLG";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuSFTPJLGREUPLOAD);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    //btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "SFTP UPLOAD", false);
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
                    btnSave.Enabled = true;
                    //btnCancel.Enabled = true;
                    btnExit.Enabled = true;
                    ClearControls();
                    break;
                case "Show":
                    btnSave.Enabled = false;
                    //btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnSave.Enabled = true;
                    //btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnSave.Enabled = false;
                    //btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    btnSave.Enabled = false;
                    //btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Exit":
                    btnSave.Visible = false;
                    //btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBranch.SelectedIndex > 0) popRO();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            //txtLoanPurpose.Enabled = Status;
            //ddlLoanSector.Enabled = Status;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            //txtLoanPurpose.Text = "";
            //ddlLoanSector.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CBCCgt oBC = null;
            Int32 vRows = 0;
            string vBrCode, vCenterID  = "A", vEOID = "A";
            try
            {
                oBC = new CBCCgt();

                vBrCode = ddlBranch.SelectedValue;
                if (vBrCode != "A")
                {
                    vEOID = ddlCo.SelectedValue;
                }
                if (vEOID != "A")
                {
                    vCenterID = ddlCenter.SelectedValue;
                }

                dt = oBC.BC_Get_SFTP_JLGTAG_List(gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), ddlBranch.SelectedValue, vEOID, txtSrch.Text.Replace("'", "''"), pPgIndx, ref vRows, vCenterID);
                gvCgtChk.DataSource = dt.DefaultView;
                gvCgtChk.DataBind();
                ViewState["SFTPUPLOAD"] = dt;
                if (dt.Rows.Count <= 0)
                {
                    //lblTotalPages.Text = "0";
                    //lblCurrentPage.Text = "0";
                    lblCgtChkRmn.Text = "0";
                }
                else
                {
                    //lblTotalPages.Text = CalTotPgs(vRows).ToString();
                    //lblCurrentPage.Text = cPgNo.ToString();
                    lblCgtChkRmn.Text = vRows.ToString();
                }
                //if (cPgNo == 1)
                //{
                //    Btn_Previous.Enabled = false;
                //    if (Int32.Parse(lblTotalPages.Text) > 0 && cPgNo != Int32.Parse(lblTotalPages.Text))
                //        Btn_Next.Enabled = true;
                //    else
                //        Btn_Next.Enabled = false;
                //}
                //else
                //{
                //    Btn_Previous.Enabled = true;
                //    if (cPgNo == Int32.Parse(lblTotalPages.Text))
                //        Btn_Next.Enabled = false;
                //    else
                //        Btn_Next.Enabled = true;
                //}
            }
            finally
            {
                dt = null;
                oBC = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRows"></param>
        /// <returns></returns>
        //private int CalTotPgs(double pRows)
        //{
        //    int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
        //    return totPg;
        //}
        //protected void ddlFO_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 vEoID = 0;
        //    if (ddlFO.SelectedValue != "A")
        //    {
        //        vEoID = Convert.ToInt32(ddlFO.SelectedValue);
        //        popGroupDest(vEoID);
        //    }
        //}
        //private void popGroupDest(Int32 pEoID)
        //{
        //    DataTable dt = null;
        //    CGblIdGenerator oGb = null;
        //    string vBrCode = "";
        //    Int32 vBrId = 0, vEOID = 0;
        //    DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //    //vEOID = Convert.ToInt32(ddlFO.SelectedValue);
        //    ddlGroup.SelectedIndex = -1;
        //    ddlGroup.Items.Clear();
        //    try
        //    {
        //        //vBrId = Convert.ToInt32(ddlBr.SelectedValue.ToString());
        //        vBrCode = Convert.ToString(ddlBranch.SelectedValue);
        //        oGb = new CGblIdGenerator();
        //        dt = oGb.PopComboMIS("S", "N", "GroupCode", "GroupID", "GroupName", "GroupMst", pEoID, "EOID", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);

        //        ddlGroup.DataSource = dt;
        //        ddlGroup.DataTextField = "GroupName";
        //        ddlGroup.DataValueField = "Groupid";
        //        ddlGroup.DataBind();
        //        ListItem oli = new ListItem("<--ALL-->", "A");
        //        ddlGroup.Items.Insert(0, oli);
        //    }
        //    finally
        //    {
        //        oGb = null;
        //        dt = null;
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ChangePage(object sender, CommandEventArgs e)
        //{
        //    switch (e.CommandName)
        //    {
        //        case "Previous":
        //            cPgNo = Int32.Parse(lblCurrentPage.Text) - 1; //lblCurrentPage
        //            break;
        //        case "Next":
        //            cPgNo = Int32.Parse(lblCurrentPage.Text) + 1; //lblTotalPages
        //            break;
        //    }
        //    LoadGrid(cPgNo);
        //    tabPurps.ActiveTabIndex = 0;
        //}
        //private void popGroupDest(Int32 pEoID)
        //{
        //    DataTable dt = null;
        //    CGblIdGenerator oGb = null;
        //    string vBrCode = "";
        //    Int32 vBrId = 0, vEOID = 0;
        //    DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //    //vEOID = Convert.ToInt32(ddlFO.SelectedValue);
        //    ddlGroup.SelectedIndex = -1;
        //    ddlGroup.Items.Clear();
        //    try
        //    {
        //        //vBrId = Convert.ToInt32(ddlBr.SelectedValue.ToString());
        //        vBrCode = Convert.ToString(ddlBranch.SelectedValue);
        //        oGb = new CGblIdGenerator();
        //        dt = oGb.PopComboMIS("S", "N", "GroupCode", "GroupID", "GroupName", "GroupMst", pEoID, "EOID", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);

        //        ddlGroup.DataSource = dt;
        //        ddlGroup.DataTextField = "GroupName";
        //        ddlGroup.DataValueField = "Groupid";
        //        ddlGroup.DataBind();
        //        ListItem oli = new ListItem("<--ALL-->", "A");
        //        ddlGroup.Items.Insert(0, oli);
        //    }
        //    finally
        //    {
        //        oGb = null;
        //        dt = null;
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]), "IDBI");
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                    ListItem liSel = new ListItem("<--- All --->", "A");
                    ddlBranch.Items.Insert(0, liSel);
                    if (Session[gblValue.BrnchCode].ToString() != "0000")
                    {
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                        ddlBranch.Enabled = false;
                        popRO();
                    }
                }
                else
                {

                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        
        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode;
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
            }
            else
            {
                vBrCode = ddlBranch.SelectedValue.ToString();
            }

            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlCo.DataSource = dt;
                ddlCo.DataTextField = "EoName";
                ddlCo.DataValueField = "EoId";
                ddlCo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "A");
                ddlCo.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }
        protected void ddlCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCo.SelectedIndex > 0) PopCenter(ddlCo.SelectedValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vCOID"></param>
        private void PopCenter(string vCOID)
        {
            DataTable dtGr = null;
            CLoanRecovery oCL = null;
            try
            {
                ddlCenter.Items.Clear();
                string vBrCode;
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }
                else
                {
                    vBrCode = ddlBranch.SelectedValue.ToString();
                }
                oCL = new CLoanRecovery();
                dtGr = oCL.PopCenterWithCollDay(vCOID, gblFuction.setDate(txtFrmDt.Text), vBrCode, "W"); //With CollDay
                dtGr.AcceptChanges();
                ddlCenter.DataSource = dtGr;
                ddlCenter.DataTextField = "Market";
                ddlCenter.DataValueField = "MarketID";
                ddlCenter.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "A");
                ddlCenter.Items.Insert(0, oLi);
            }
            finally
            {
                dtGr = null;
                oCL = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCgtChk_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 vErr = 0, vCnt = 0;
            string vXmlCB = "", vMailBody = "";
            CBCCgt oBC = null;
            DataSet ds = null;
            DataTable dt = null;
            try
            {
                dt = (DataTable)ViewState["SFTPUPLOAD"];
                if (dt.Rows.Count == 0)
                {
                    return false;
                }

                foreach (GridViewRow gr in gvCgtChk.Rows)
                {
                    CheckBox chkYN = (CheckBox)gr.FindControl("chkYN");
                    if (chkYN.Checked)
                    {
                        dt.Rows[gr.RowIndex]["ChkSFTPUpload"] = "Y";
                    }
                    else
                    {
                        dt.Rows[gr.RowIndex]["ChkSFTPUpload"] = "N";
                    }

                }
                dt.AcceptChanges();
                foreach (DataRow dr in dt.Select("ChkSFTPUpload='N'"))
                {
                    dr.Delete();
                }
                dt.AcceptChanges();

                if (dt.Rows.Count == 0)
                    return false;


                for (int dc = 0; dc < dt.Columns.Count; dc++)
                {
                    if (dt.Columns[dc].ColumnName != "URNID" && dt.Columns[dc].ColumnName != "CGTID" && dt.Columns[dc].ColumnName != "ChkSFTPUpload")
                    {
                        dt.Columns.Remove(dt.Columns[dc]);
                        dc--;
                    }
                }
                dt.AcceptChanges();
                dt.TableName = "Table1";
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXmlCB = oSW.ToString();
                }

                oBC = new CBCCgt();
                ds = oBC.BC_Save_JLG_SFTPUPLOAD(vXmlCB, gblFuction.setDate(Session[gblValue.LoginDate].ToString())); // Data base save GetDate()
                if (ds.Tables[0].Rows.Count > 0)
                {
                    BC_IDBI_SFTPCom vObj = new BC_IDBI_SFTPCom();
                    vResult = vObj.BC_GetIDBI_SFTP_Upload("JLG", "");
                    if (vResult == true)
                    {//dayitmitra@gmail.com
                        SendToMail("prodip.mukherjee@jagaranmf.com", "Hi JLG_TAG File Has successfully Uploaded\n" + vMailBody, "SFTP_JLG_RETAG_FILE UPLOADED");
                    }
                }
                else
                {
                    gblFuction.MsgPopup("Error...!!!");
                    vResult = false;
                }
            }
            finally
            {
                dt = null;

            }
            return vResult;
        }

        /*
            vSubject = "Banker's Loan Disbursement";
            vBody = "Loan amount of Rs." + txtLnAmt.Text + " has been taken from " + 	ddlBanker.SelectedItem.Text + " on " + txtLnDt.Text;
            vEmail = ConfigurationManager.AppSettings["RcvEmail"];

            SendToMail(vEmail, vBody, vSubject);
            */

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMail"></param>
        /// <param name="pBody"></param>
        /// <param name="pSubject"></param>

        public static void SendToMail(string pMail, string pBody, string pSubject)
        {
            string vMTo = "", vBody = "";
            string vCompEmail = ConfigurationManager.AppSettings["CompEmail"];
            string vCompPwd = ConfigurationManager.AppSettings["CompPwd"];
            try
            {
                vMTo = pMail;
                if (vMTo != "")
                {
                    vBody = pBody;
                    MailMessage oM = new MailMessage();
                    oM.To.Add(vMTo);
                    oM.From = new MailAddress(vCompEmail);
                    oM.Subject = pSubject;
                    oM.Body = vBody;
                    //oM.Attachments.Add(new Attachment(AttachFileCreation()));
                    //oM.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

                    smtp.Host = "smtp.gmail.com";
                    smtp.Credentials = new System.Net.NetworkCredential(vCompEmail, vCompPwd);
                    smtp.EnableSsl = true;
                    //smtp.UseDefaultCredentials = true;

                    //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Timeout = 360000;
                    smtp.Send(oM);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string AttachFileCreation()
        {
            string vFolderPath = "C:\\IDBI_SFTP";
            string vBrCode = "A";
            string vFileNm = "SFTP_UPLOAD.xls";

            return "";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
            ViewState["PurposeId"] = null;
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
                tabPurps.ActiveTabIndex = 1;
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
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadGrid(0);
                    ClearControls();
                    tabPurps.ActiveTabIndex = 0;
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
            //if (SaveRecords("Cancel") == true)
            //{
            //    gblFuction.MsgPopup(gblMarg.SaveMsg);
            //    LoadGrid(0);
            //    StatusButton("Show");
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveRecords("Save") == true)
            {
                gblFuction.MsgPopup("JLG TAG File is Uploaded successfully");
                LoadGrid(0);
                StatusButton("Add");
            }
            else
            {
                gblFuction.MsgPopup("JLG TAG File is not Uploaded successfully");
                LoadGrid(0);
                StatusButton("Add");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(0);
            StatusButton("Add");
        }

        protected void chkAll_CheckChanged(object sender, EventArgs e)
        {
            CheckBox chkAll = (CheckBox)sender;
            GridViewRow gR = (GridViewRow)chkAll.NamingContainer;
            CheckBox ChkSFTPUpload = (CheckBox)gR.FindControl("ChkSFTPUpload");
            if (chkAll.Checked == false)
            {
                ChkSFTPUpload.Checked = false;
                ChkSFTPUpload.Enabled = true;
            }
            else
            {
                ChkSFTPUpload.Checked = true;
                ChkSFTPUpload.Enabled = false;
            }
        }

        protected void chkSelectAll_CheckedChange(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked == true)
            {
                foreach (GridViewRow gR in gvCgtChk.Rows)
                {
                    CheckBox chkYN = (CheckBox)gR.FindControl("chkYN");
                    chkYN.Checked = true;
                }
            }
            else
            {
                foreach (GridViewRow gR in gvCgtChk.Rows)
                {
                    CheckBox chkYN = (CheckBox)gR.FindControl("chkYN");
                    chkYN.Checked = false;
                }
            }
        }
    }
}