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
    public partial class BC_Cust_Rej_Recheck : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
                    StatusButton("View");
                ViewState["StateEdit"] = null;
                ViewState["SFTPUPLOAD"] = null;
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                tabPurps.ActiveTabIndex = 0;
                PopBranch(Session[gblValue.UserName].ToString());
                //popFO(Session[gblValue.BrnchCode].ToString());
                LoadGrid(0);
                //ChangeTab();
            }
        }
         /// <summary>
        /// 
        /// </summary>
        //protected void tabPurps_ActiveTabChanged(object senedr, EventArgs e)
        //{
        //    ChangeTab();
        //}
        //public void ChangeTab()
        //{
        //    if (tabPurps.ActiveTabIndex == 0)
        //    {
        //        btnSave.Visible = false;
        //        btnCancel.Visible = false;
        //        btnUndo.Visible = false;
        //    }
        //    else
        //    {
        //        btnUndo.Visible = true;
        //        btnSave.Visible = true;
        //        btnCancel.Visible = true;
        //    }
        //}
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);


                this.Menu = false;
                this.PageHeading = "Customer Rejection";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuReCheckCGTData);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnCancel.Visible = true;
                    //btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnCancel.Visible = true;
                    //btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    //btnCancel.Visible = true;
                    //btnSave.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Purpose Master", false);
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
                    //btnSave.Enabled = true;
                    //btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    //btnSave.Enabled = false;
                    //btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    //btnSave.Enabled = true;
                    //btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    //btnSave.Enabled = false;
                    //btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    //btnSave.Enabled = false;
                    //btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Exit":
                    //btnSave.Visible = false;
                    //btnCancel.Visible = false;
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
            string vBrCode, vCenterId = "A", vEOID = "A";
            ViewState["SFTPUPLOAD"] = null;
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
                    vCenterId = ddlCenter.SelectedValue;
                }

                dt = oBC.BC_Cust_Rej_Recheck_List(gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), ddlBranch.SelectedValue, vEOID, txtSrch.Text.Replace("'", "''"), pPgIndx, ref vRows, vCenterId);
                ViewState["SFTPUPLOAD"] = dt;
                gvCgtChk.DataSource = dt.DefaultView;
                gvCgtChk.DataBind();
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
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]),"IDBI");
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
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBranch.SelectedIndex > 0) popRO();
        }

        protected void ddlCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCo.SelectedIndex > 0) PopCenter(ddlCo.SelectedValue);
        }
        //private void popFO(string vBrCode)
        //{
        //    DataTable dt = null;
        //    //string vBrCode = Session[gblValue.BrnchCode].ToString();
        //    CCM oCM = null;
        //    try
        //    {
        //        oCM = new CCM();
        //        dt = oCM.GetCOPop(vBrCode, "SCO,CO,TCO,JTCO,GO,UM");
        //        ddlFO.DataTextField = "EOName";
        //        ddlFO.DataValueField = "EOId";
        //        ddlFO.DataSource = dt;
        //        ddlFO.DataBind();
        //        ListItem oItm = new ListItem();
        //        oItm.Text = "<--- All --->";
        //        oItm.Value = "A";
        //        ddlFO.Items.Insert(0, oItm);
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oCM = null;
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
        //protected void gvCgtChk_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    Int32 vEoId = 0;
        //    string vCgtId = "", vMemName = "", vSpnName = "", vBrCode = "", base64String = "";
        //    DataTable dt = null;
        //    CBCCgt oBC = null;

        //    try
        //    {
        //        vCgtId = e.CommandArgument.ToString();
        //        ViewState["CgtId"] = vCgtId;
        //        if (e.CommandName == "cmdShow")
        //        {
        //            GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        //            LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
        //            vMemName = btnShow.Text;
        //            vSpnName = gvRow.Cells[2].Text;
        //            vBrCode = gvRow.Cells[4].Text;
        //            vEoId = Convert.ToInt32(gvRow.Cells[5].Text);
        //            DateTime vCgtDt = gblFuction.setDate(gvRow.Cells[6].Text);
        //            foreach (GridViewRow gr in gvCgtChk.Rows)
        //            {
        //                LinkButton lb = (LinkButton)gr.FindControl("btnShow");
        //                lb.ForeColor = System.Drawing.Color.Black;
        //            }
        //            btnShow.ForeColor = System.Drawing.Color.Red;
        //            oBC = new CBCCgt();
        //            dt = oBC.BC_Cust_Rej_Recheck_By_Id(vCgtId);
        //            if (dt.Rows.Count > 0)
        //            {
        //                txtName.Text = Convert.ToString(dt.Rows[0]["MEM_NAME"]).Trim();
        //                hdCgtDt.Value = Convert.ToString(dt.Rows[0]["CGTDate"]).Trim();
        //                hdBrCode.Value = Convert.ToString(dt.Rows[0]["BranchCode"]).Trim();
        //                txtPhIdType.Text = Convert.ToString(dt.Rows[0]["PhotoIdType"]).Trim();
        //                hdPhotoId.Value = Convert.ToString(dt.Rows[0]["PhotoId"]).Trim();
        //                txtPhIdNo.Text = Convert.ToString(dt.Rows[0]["PhotoIdNo"]).Trim();
        //                txtAddIdType.Text = Convert.ToString(dt.Rows[0]["AddIdType"]).Trim();
        //                hdAddId.Value = Convert.ToString(dt.Rows[0]["AddId"]).Trim();
        //                txtAddIdNo.Text = Convert.ToString(dt.Rows[0]["AddIdNo"]).Trim();
        //                txtDOB.Text = Convert.ToString(dt.Rows[0]["DOB"]).Trim();
        //                txtAge.Text = Convert.ToString(dt.Rows[0]["Age"]).Trim();
        //                txtAdd.Text = Convert.ToString(dt.Rows[0]["HouseNo"]).Trim();
        //                txtAdd2.Text = Convert.ToString(dt.Rows[0]["LandMark"]).Trim();
        //                txtMohalla.Text = Convert.ToString(dt.Rows[0]["MahallaName"]).Trim();
        //                hdMahalla.Value = Convert.ToString(dt.Rows[0]["MahallaId"]).Trim();
        //                txtPin.Text = Convert.ToString(dt.Rows[0]["Pin"]).Trim();
        //                txtMobile.Text = Convert.ToString(dt.Rows[0]["ContactNo"]).Trim();
        //                txtSpouse.Text = Convert.ToString(dt.Rows[0]["SpouseName"]).Trim();
        //                hdFather.Value = Convert.ToString(dt.Rows[0]["FatherName"]).Trim();
        //                txtReligion.Text = Convert.ToString(dt.Rows[0]["Religion"]).Trim();
        //                hdReligion.Value = Convert.ToString(dt.Rows[0]["ReligionId"]).Trim();
        //                hdEoId.Value = vEoId.ToString();
        //                byte[] vKycImgFrnt = (byte[])dt.Rows[0]["KycImgFrnt"];
        //                base64String = Convert.ToBase64String(vKycImgFrnt, 0, vKycImgFrnt.Length);
        //                if (base64String != "")
        //                    imgKycFrnt.ImageUrl = "data:image/" + Convert.ToString(dt.Rows[0]["KycImgFrntType"]) + ";base64," + base64String;
        //                else
        //                    imgKycFrnt.ImageUrl = "~/Images/No_Image_Available.png";
        //                byte[] vKycImgBck = (byte[])dt.Rows[0]["KycImgBck"];
        //                base64String = Convert.ToBase64String(vKycImgBck, 0, vKycImgBck.Length);
        //                if (base64String != "")
        //                    imgKycBck.ImageUrl = "data:image/" + Convert.ToString(dt.Rows[0]["KycImgBckType"]) + ";base64," + base64String;
        //                else
        //                    imgKycBck.ImageUrl = "~/Images/No_Image_Available.png";

        //                tabPurps.ActiveTabIndex = 1;
        //                StatusButton("Add");
        //                ChangeTab();
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        dt.Dispose();
        //    }
        //}

        //private Boolean DueDuplicateCheck()
        //{
        //    CCGT oCG = null;
        //    string vRetMsg = "";
        //    Boolean vResult = false;

        //    oCG = new CCGT();
        //    vRetMsg = oCG.Mob_ChkODForCGT(gblFuction.setDate(hdCgtDt.Value), txtName.Text, txtSpouse.Text, Convert.ToInt32(hdPhotoId.Value), txtPhIdNo.Text, hdFather.Value, hdMahalla.Value, hdBrCode.Value);
        //    ViewState["RetMsg"] = vRetMsg;
        //    hdType.Value = vRetMsg[10].ToString();
        //    if (vRetMsg[10].ToString() == "F")
        //    {
        //        lblPsdDeDupChk.ForeColor = System.Drawing.Color.Red;
        //        lblMemId.ForeColor = System.Drawing.Color.Gray;
        //        lblMemberId.Text = "";
        //        lblFldDeDupChk.ForeColor = System.Drawing.Color.Gray;
        //        lblRsn.ForeColor = System.Drawing.Color.Gray;
        //        lblReason.Text = "";
        //        vResult = true;
        //    }
        //    else if (vRetMsg[10].ToString() == "Y")
        //    {
        //        lblPsdDeDupChk.ForeColor = System.Drawing.Color.Gray;
        //        lblMemId.ForeColor = System.Drawing.Color.Red;
        //        lblMemberId.Text = vRetMsg.Substring(11);
        //        lblFldDeDupChk.ForeColor = System.Drawing.Color.Red;
        //        lblRsn.ForeColor = System.Drawing.Color.Red;
        //        lblReason.Text = "Member Exist With Open Loan (With OD)";
        //        vResult = false;
        //    }
        //    else if (vRetMsg[10].ToString() == "N")
        //    {
        //        lblPsdDeDupChk.ForeColor = System.Drawing.Color.Gray;
        //        lblMemId.ForeColor = System.Drawing.Color.Red;
        //        lblMemberId.Text = vRetMsg.Substring(11);
        //        lblFldDeDupChk.ForeColor = System.Drawing.Color.Red;
        //        lblRsn.ForeColor = System.Drawing.Color.Red;
        //        lblReason.Text = "Member Exist With Open Loan (No OD)";
        //        vResult = false;
        //    }
        //    else if (vRetMsg[10].ToString() == "C")
        //    {
        //        lblPsdDeDupChk.ForeColor = System.Drawing.Color.Red;
        //        lblMemId.ForeColor = System.Drawing.Color.Red;
        //        lblMemberId.Text = vRetMsg.Substring(11);
        //        lblFldDeDupChk.ForeColor = System.Drawing.Color.Gray;
        //        lblRsn.ForeColor = System.Drawing.Color.Red;
        //        lblReason.Text = "Member Exist With Close Loan";
        //        vResult = true;
        //    }

        //    return vResult;

        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            //Int32 vErr = 0;
            //CBCCgt oBC = null;
            //try
            //{
            //    if (Mode == "Save")
            //    {
            //        //if (DueDuplicateCheck() == false)
            //        //{
            //        //    gblFuction.MsgPopup("Can not save the data ...Failed Due Duplicate check");
            //        //    return false;
            //        //}
            //    }
            //    //vOldYN = (ViewState["RetMsg"].ToString()[10].ToString() == "F") ? "N" : "Y";
            //    //vMemberId = (vOldYN == "N") ? "" : ViewState["RetMsg"].ToString().Substring(11);
            //    //vODYN = (ViewState["RetMsg"].ToString()[10].ToString() == "Y") ? "Y" : "N";
            //    //vEoId = Convert.ToInt32(hdEoId.Value);


            //    if (Mode == "Save")
            //    {
            //        //if (DueDuplicateCheck() == false)
            //        //{
            //        //    gblFuction.MsgPopup("Can not save the data ...Failed Due Duplicate check");
            //        //    vResult = false;
            //        //}

            //        oBC = new CBCCgt();
            //        vErr = oBC.BC_Save_Cust_Rej_Recheck(ViewState["CgtId"].ToString(), txtName.Text,
            //            Convert.ToInt32(hdPhotoId.Value), txtPhIdNo.Text, Convert.ToInt32(hdAddId.Value), txtAddIdNo.Text,
            //            gblFuction.setDate(txtDOB.Text), txtAdd.Text, hdMahalla.Value, txtPin.Text, txtMobile.Text, txtSpouse.Text,
            //            hdFather.Value, this.UserID, "S", txtAdd2.Text);
            //        if (vErr > 0)
            //            vResult = true;
            //        else
            //        {
            //            gblFuction.MsgPopup(gblMarg.DBError);
            //            vResult = false;
            //        }
            //    }
            //    else if (Mode == "Cancel")
            //    {
            //        oBC = new CBCCgt();
            //        vErr = oBC.BC_Save_Cust_Rej_Recheck(ViewState["CgtId"].ToString(), txtName.Text,
            //            Convert.ToInt32(hdPhotoId.Value), txtPhIdNo.Text, Convert.ToInt32(hdAddId.Value), txtAddIdNo.Text,
            //            gblFuction.setDate(txtDOB.Text), txtAdd.Text, hdMahalla.Value, txtPin.Text, txtMobile.Text, txtSpouse.Text,
            //            hdFather.Value, this.UserID, "C", txtAdd2.Text);
            //        if (vErr > 0)
            //            vResult = true;
            //        else
            //        {
            //            gblFuction.MsgPopup(gblMarg.DBError);
            //            vResult = false;
            //        }
            //    }
            return vResult;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    oBC = null;
            //}
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
        protected void btnUndo_Click(object sender, EventArgs e)
        {
            tabPurps.ActiveTabIndex = 0;
            ClearControls();
            LoadGrid(0);
            StatusButton("View");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (SaveRecords("Cancel") == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(0);
                StatusButton("Show");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            UploadRejected();
            LoadGrid(0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean UploadRejected()
        {
            Boolean vResult = false;
            Int32 vErr = 0, vCnt = 0;
            string vXmlCB = "", vMailBody = "";
            CBCCgt oBC = null;

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
                        dt.Rows[gr.RowIndex]["chkAgainUpLoad"] = "Y";
                    }
                    else
                    {
                        dt.Rows[gr.RowIndex]["chkAgainUpLoad"] = "N";
                    }

                }
                dt.AcceptChanges();
                foreach (DataRow dr in dt.Select("chkAgainUpLoad='N'"))
                {
                    dr.Delete();
                }
                dt.AcceptChanges();

                if (dt.Rows.Count == 0)
                    return false;


                for (int dc = 0; dc < dt.Columns.Count; dc++)
                {
                    if (dt.Columns[dc].ColumnName != "URNID" && dt.Columns[dc].ColumnName != "CGTID" && dt.Columns[dc].ColumnName != "chkAgainUpLoad")
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
                BC_IDBI_SFTPCom vObj = new BC_IDBI_SFTPCom();
                vResult = vObj.BC_GetIDBI_SFTP_Upload_AfterReject(vXmlCB, "CUST", Session[gblValue.LoginDate].ToString());
                if (vResult == true)
                {
                    return true;
                }
               
                else
                {
                    gblFuction.MsgPopup("Nothing to Upload...!!!");

                    vResult = false;
                }
            }
            finally
            {
                dt = null;

            }
            return vResult;
        }

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveRecords("Save") == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(0);
                StatusButton("Show");
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
        }
    }
}

   