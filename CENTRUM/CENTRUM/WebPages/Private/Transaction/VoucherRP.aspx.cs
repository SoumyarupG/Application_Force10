using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.Drawing;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class VoucherRP : CENTRUMBase
    {
        protected int vPgNo = 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (txtFromDt.Text == "" || txtToDt.Text == "")
            {
                txtFromDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            }
            if (!IsPostBack)
            {              
                ViewState["VoucherEdit"] = null;
                ViewState["VouDtl"] = null;
                PopBranch(Session[gblValue.UserName].ToString());
                chkApproveShow();
                if (hdAppYN.Value == "Y")
                    btnApp.Visible = true;
                else
                    btnApp.Visible = false;
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                    ddlBranch.Enabled = false;
                }
                else
                {
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                    ddlBranch.Enabled = true;
                }
                LoadGrid(1);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUser"></param>
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
                }
                else
                {
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
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
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Receipt/Payment";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuRecPay);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Receipt & Payment Voucher", false);
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
                    btnExit.Enabled = false;
                    ClearControls();                   
                    break;
                case "Show":
                    btnAdd.Enabled = true;                   
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;                  
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;                   
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    btnAdd.Enabled = true;                    
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            if (txtFromDt.Text.Trim() == "")
            {
                //EnableControl(true);
                gblFuction.MsgPopup("From Date Cannot be left blank.");
                gblFuction.focus("ctl00_cph_Main_txtFromDt");
                vResult = false;
            }
            if (txtFromDt.Text.Trim() != "")
            {
                if (gblFuction.IsDate(txtFromDt.Text) == false)
                {
                    //EnableControl(true);
                    gblFuction.MsgPopup("Please Enter Valid Date.");
                    gblFuction.focus("ctl00_cph_Main_txtFromDt");
                    vResult = false;
                }
            }
            if (txtToDt.Text.Trim() == "")
            {
                //EnableControl(true);
                gblFuction.MsgPopup("To Date Cannot be left blank.");
                gblFuction.focus("ctl00_cph_Main_txtToDt");
                vResult = false;
            }
            if (txtToDt.Text.Trim() != "")
            {
                if (gblFuction.IsDate(txtToDt.Text) == false)
                {
                    //EnableControl(true);
                    gblFuction.MsgPopup("Please Enter Valid Date.");
                    gblFuction.focus("ctl00_cph_Main_txtToDt");
                    vResult = false;
                }
            }
            return vResult;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vR = 0;
            CVoucher oVoucher = null;
            try
            {
                if (ValidateFields() == true)
                {
                    string vAcMst = Session[gblValue.ACVouMst].ToString();
                    string vAcDtl = Session[gblValue.ACVouDtl].ToString();
                    string vBrCode ;
                    if (Session[gblValue.BrnchCode].ToString() != "0000")                  
                        vBrCode = Session[gblValue.BrnchCode].ToString();                   
                    else                 
                        vBrCode = ddlBranch.SelectedValue.ToString();
                    
                    string vDtFrom = gblFuction.setStrDate(txtFromDt.Text);
                    string vDtTo = gblFuction.setStrDate(txtToDt.Text);
                    string vSearch = txtSearch.Text;
                    oVoucher = new CVoucher();
                    dt = oVoucher.GetVoucherlist(vAcMst, vAcDtl, vBrCode, vDtFrom, vDtTo, vSearch, "A", pPgIndx, ref vR); //VoucherTyp=R/P 
                    gvVouvher.DataSource = dt.DefaultView;
                    gvVouvher.DataBind();
                    if (dt.Rows.Count <= 0)
                    {
                        lblTotPg.Text = "0";
                        lblCrPg.Text = "0";
                    }
                    else
                    {
                        lblTotPg.Text = getTotPages(vR).ToString();
                        lblCrPg.Text = vPgNo.ToString();
                    }
                    if (vPgNo == 1)
                    {
                        btnPrv.Enabled = false;
                        if (Int32.Parse(lblTotPg.Text) > 0 && vPgNo != Int32.Parse(lblTotPg.Text))
                            btnNxt.Enabled = true;
                        else
                            btnNxt.Enabled = false;
                    }
                    else
                    {
                        btnPrv.Enabled = true;
                        if (vPgNo == Int32.Parse(lblTotPg.Text))
                            btnNxt.Enabled = false;
                        else
                            btnNxt.Enabled = true;
                    }
                }
            }
            finally
            {
                oVoucher = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalRows"></param>
        /// <returns></returns>
        private int getTotPages(double pRows)
        {         
            int vTPg = (int)Math.Ceiling(pRows / 15);
            return vTPg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    vPgNo = Int32.Parse(lblCrPg.Text) - 1;
                    break;
                case "Next":
                    vPgNo = Int32.Parse(lblCrPg.Text) + 1;
                    break;
            }
            LoadGrid(vPgNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVouvher_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vHeadId = "";
            CVoucher oVoucher = null;
            DataTable dtAc = null;
            try
            {
                for (int r = 0; r < gvVouvher.Rows.Count; r++)
                {
                    if (gvVouvher.Rows[r].ForeColor == Color.Red)
                        gvVouvher.Rows[r].ForeColor = Color.Black;
                }
                vHeadId = Convert.ToString(e.CommandArgument);
                string vBrCode ;
                if (Session[gblValue.BrnchCode].ToString() != "0000")             
                    vBrCode = Session[gblValue.BrnchCode].ToString();              
                else             
                    vBrCode = ddlBranch.SelectedValue.ToString();
                
                ViewState["HeadId"] = vHeadId;
                if (e.CommandName == "cmdDtl")
                {
                    Response.RedirectPermanent("~/Webpages/Private/Transaction/VoucherRPDtl.aspx?aId=" + vHeadId, false);
                    //Response.RedirectPermanent("~/Webpages/Private/Transaction/ActVouRP.aspx?aId=" + vHeadId, false);
                }
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    gvVouvher.Rows[row.RowIndex].ForeColor = System.Drawing.Color.Red;
                    oVoucher = new CVoucher();
                    ViewState["VouDtl"] = null;
                    dtAc = oVoucher.GetVoucherDtl(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), vHeadId, vBrCode);
                    ViewState["VouDtl"] = dtAc;
                    if (dtAc.Rows.Count > 0)
                    {
                        gvAcDtl.DataSource = dtAc.DefaultView;
                        gvAcDtl.DataBind();
                        StatusButton("Show");
                    }
                }               
            }        
            finally
            {
                oVoucher = null;
                dtAc = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, System.EventArgs e)
        {
            LoadGrid(1);
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["VoucherEdit"] = null;
                ViewState["VouDtl"] = null;
                Response.RedirectPermanent("~/WebPages/Public/Main.aspx", false);
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
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Response.RedirectPermanent("~/Webpages/Private/Transaction/VoucherRPDtl.aspx", false);
                //Response.RedirectPermanent("~/Webpages/Private/Transaction/ActVouRP.aspx", false);
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
        protected void btnApp_Click(object sender, EventArgs e)
        {
            CVoucher oApp = null;
            DataTable dt = null;
            Int32 vErr = 0;
            string vXmlData = "";
            string vActMstTbl = Session[gblValue.ACVouMst].ToString();           
            try
            {
                dt = GetTable();
                if (dt.Rows.Count <= 0)
                {
                    gblFuction.MsgPopup("No Records to Process... ");
                    return;
                }
                oApp = new CVoucher();
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXmlData = oSW.ToString();
                }
                vErr = oApp.ApproveVoucher(vXmlData, vActMstTbl, this.UserID);
                if (vErr > 0)
                {
                    gblFuction.MsgPopup(gblMarg.SaveMsg);
                    LoadGrid(1);
                }
                else
                {
                    gblFuction.MsgPopup(gblMarg.DBError);
                }
            }
            finally
            {
                oApp = null;
                dt = null;                
            }            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable GetTable()
        {
            DataTable dt = new DataTable("Table1");
            DataColumn dc1 = new DataColumn("VoucherNo");
            dt.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn("Appr");
            dt.Columns.Add(dc2);            
            foreach (GridViewRow gR in gvVouvher.Rows)
            {
                CheckBox chkApp = (CheckBox)gR.FindControl("chkApp");
                LinkButton txtVoucherNo = (LinkButton)gR.FindControl("btnShow");
                if (chkApp.Checked == true && chkApp.Enabled == true)
                {
                    DataRow dR = dt.NewRow();
                    dR["VoucherNo"] = txtVoucherNo.Text.Trim();
                    dR["Appr"] = "Y";
                    dt.Rows.Add(dR);
                }                
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        private void chkApproveShow()
        {
            DataTable dt = null;
            CRole oRl = null;
            Int32 vRoleId = Convert.ToInt32(Session[gblValue.RoleId].ToString());
            try
            {
                oRl = new CRole();
                dt = oRl.GetRoleById(vRoleId);
                if (dt.Rows.Count > 0)
                {
                    hdAppYN.Value = Convert.ToString(dt.Rows[0]["AppYN"]);
                }
            }
            finally
            {
                oRl = null;
                dt = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkApp_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CRole oRl = null;
            string pVoucherDt = "";
            try
            {
                Int32 vRoleId = Convert.ToInt32(Session[gblValue.RoleId].ToString());
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                pVoucherDt = row.Cells[5].Text;

                if (this.RoleId != 1)
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(pVoucherDt))
                        {
                            gblFuction.AjxMsgPopup("You can not approve, Day end already done..");
                            ((CheckBox)sender).Checked = false;
                        }
                    }
                }

                foreach (GridViewRow gR in gvVouvher.Rows)
                {
                    
                    CheckBox chkApp = (CheckBox)gR.FindControl("chkApp");
                    if (chkApp.Checked == true && chkApp.Enabled == true)
                    {
                        //if (Convert.ToInt32(gR.Cells[8].Text.Trim()) > 0)
                        //{
                        //    gblFuction.MsgPopup("This vouche already approved... ");
                        //    ((CheckBox)sender).Checked = false;
                        //}
                        oRl = new CRole();
                        dt = oRl.GetRoleById(vRoleId);
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["AppAmt"] is DBNull)
                            {
                                gblFuction.AjxMsgPopup("Approval power denied... ");
                                ((CheckBox)sender).Checked = false;
                            }
                            else
                            {
                                if (Convert.ToDouble(gR.Cells[4].Text.Trim()) > Convert.ToDouble(dt.Rows[0]["AppAmt"]))
                                {
                                    gblFuction.AjxMsgPopup("Approval power denied... ");
                                    ((CheckBox)sender).Checked = false;
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                dt = null;
                oRl = null;
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVouvher_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Bind Approval
                    CheckBox chkApp = (CheckBox)e.Row.FindControl("chkApp");                    
                    if ( Convert.ToInt32(e.Row.Cells[8].Text) > 0)
                    {
                        chkApp.Checked = true;
                        chkApp.Enabled = false;                        
                    }                    
                }
            }
            finally
            {
            }
        }
    }
}