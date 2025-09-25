using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.Data;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class HoNEFTApprCancel : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                txtAppDt.Text = Session[gblValue.LoginDate].ToString();
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                PopBranch(Session[gblValue.UserName].ToString());
                //LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);

            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "HO Disbursement Cancel";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHoNEFTCancel);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "HO Disbursement Cancel", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }


        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (ValidDate() == true)
            {
                try
                {
                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    if (rdbOpt.SelectedValue == "N")
                        LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);

                    else if (rdbOpt.SelectedValue == "A")
                        LoadGrid("A", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        private bool ValidDate()
        {
            Boolean vResult = true;
            if (txtFrmDt.Text.Trim() != "" || txtFrmDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtFrmDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtFrDt");
                    vResult = false;
                }
            }
            if (txtToDt.Text.Trim() != "" || txtToDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtToDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtToDt");
                    vResult = false;
                }
            }
            if (txtAppDt.Text.Trim() != "" || txtAppDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtAppDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtSancDt");
                    vResult = false;
                }
            }
            return vResult;
        }


        protected void btnDone_Click(object sender, EventArgs e)
        {
            CApplication oApp = null;
            DataTable dt = new DataTable();
            Int32 vErr = 0;
            string vXmlData = "";
            DateTime vSanDt = gblFuction.setDate(txtAppDt.Text);
            if (ValidDate() == true)
            {
                try
                {
                    dt = (DataTable)ViewState["Sanc"];
                    if (dt == null) return;
                    if (!dt.Columns.Contains("CancelReason"))
                    {
                        dt.Columns.Add("CancelReason", typeof(string));
                    }
                    Int32 vRow = 0;
                    for (vRow = 0; vRow < gvSanc.Rows.Count; vRow++)
                    {
                        CheckBox chkCancel = (CheckBox)gvSanc.Rows[vRow].FindControl("chkCancel");                       
                        DateTime vCBDate = gblFuction.setDate(gvSanc.Rows[vRow].Cells[32].Text);
                        DropDownList ddlResn = (DropDownList)gvSanc.Rows[vRow].FindControl("ddlResn");                       
                        //**********************Group Date Check***********************                      
                        if (chkCancel.Checked == true)
                        {
                            if (ddlResn.SelectedItem.Text == "")
                            {
                                gblFuction.MsgPopup("Reason Can not left blank. ");
                                ddlResn.Focus();
                                return;
                            }
                            dt.Rows[vRow]["CancelReason"] = ddlResn.SelectedItem.Text;
                        }                 
                        
                    }
                    dt.AcceptChanges();
                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    oApp = new CApplication();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    //-----------XML Save----------
                    if (dt.Rows.Count > 0)
                    {
                        vErr = oApp.HoNEFTCancel(vXmlData, this.UserID, vBrCode, "E", 0, vSanDt);
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup(gblMarg.SaveMsg);
                            LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                            rdbOpt.SelectedValue = "N";
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.DBError);
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("Please select atleast One row...");
                    }
                }
                finally
                {
                    oApp = null;
                    dt = null;                   
                    //dt.Columns.RemoveAt(34);
                }
            }
        }

        protected void txtAppDt_TextChanged(object sender, EventArgs e)
        {
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DateTime vAppDt = gblFuction.setDate(txtAppDt.Text);
            if (vAppDt > vLoginDt)
            {
                gblFuction.MsgPopup("Approved date should not grater than login date..");
                txtAppDt.Text = Session[gblValue.LoginDate].ToString();  //Convert.ToString(vLoginDt);
                return;
            }
        }

        private Boolean ValidateFields()//To Check
        {
            Boolean vResult = true;
            Int32 vRow = 0;
            for (vRow = 0; vRow < gvSanc.Rows.Count; vRow++)
            {
                CheckBox chkApp = (CheckBox)gvSanc.Rows[vRow].FindControl("chkAppr");
                CheckBox chkCan = (CheckBox)gvSanc.Rows[vRow].FindControl("chkCash");
                TextBox txtAccountNo = (TextBox)gvSanc.Rows[vRow].FindControl("txtAccountNo");
                TextBox txtIFSC = (TextBox)gvSanc.Rows[vRow].FindControl("txtIfsc");
                if (chkApp.Checked == true || chkCan.Checked == true)
                {

                    if (txtAccountNo.Text.Trim() == "")
                    {
                        gblFuction.MsgPopup("Please enter Account Number..");
                        vResult = false;
                    }
                    if (txtIFSC.Text.Trim() == "")
                    {
                        gblFuction.MsgPopup("Please enter IFSC Code..");
                        vResult = false;
                    }
                }
                if (chkApp.Checked == false && chkCan.Checked == false)
                {

                }
            }
            return vResult;
        }
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
                }

            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }

        private void LoadGrid(string pAppMode, string pFromDt, string pToDt, string pBranch, Int32 pPgIndx)
        {
            DataTable dt = null;
            CApplication oLS = null;
            Int32 vRows = 0; 
            string vGroupId, vMarketId = null;

            if (ddlBranch.SelectedValues == "")
            {
                gblFuction.AjxMsgPopup("Please Select branch...");
                return;
            }
            if (hdGrpId.Value == "-1" || hdGrpId.Value == "" || txtGroup.Text.Trim() == "")
            {
                vGroupId = "";
            }
            else
            {
                vGroupId = hdGrpId.Value;
            }
            if (hdCntrId.Value == "-1" || hdCntrId.Value == "" || txtCenter.Text.Trim() == "")
            {
                vMarketId = "";
            }
            else
            {
                vMarketId = hdCntrId.Value;
            }

            try
            {
                string vBrCode = pBranch;
                DateTime vFromDt = gblFuction.setDate(pFromDt);
                DateTime vToDt = gblFuction.setDate(pToDt);
                oLS = new CApplication();
                dt = oLS.GetNEFTHoApprList(vFromDt, vToDt, pAppMode, ddlBranch.SelectedValues.Replace("|", ","), pPgIndx, ref vRows, vGroupId, vMarketId);
                ViewState["Sanc"] = dt;
                gvSanc.DataSource = dt;
                gvSanc.DataBind();
                //lblTotalPages.Text = CalTotPgs(vRows).ToString();
                //lblCurrentPage.Text = cPgNo.ToString();
                //if (cPgNo == 0)
                //{
                //    Btn_Previous.Enabled = false;
                //    if (Int32.Parse(lblTotalPages.Text) > 1)
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
                oLS = null;
            }
        }
        public void GetData()
        {
            DataTable dt = (DataTable)ViewState["Sanc"];
            foreach (GridViewRow gr in gvSanc.Rows)
            {
                CheckBox chkCash = (CheckBox)gr.FindControl("chkCash");
                CheckBox chkNEFT = (CheckBox)gr.FindControl("chkNEFT");
                if (chkCash.Checked == true)
                {
                    dt.Rows[gr.RowIndex]["CashApproveYN"] = "Y";
                    dt.Rows[gr.RowIndex]["NEFTApproveYN"] = "N";
                    dt.Rows[gr.RowIndex]["HOApproveYN"] = "Y";
                }
            }
        }
        protected void chkCancel_CheckChanged(object sender, EventArgs e)
        {
            string vBranch = "";
            DataTable dt = null;
            try
            {
                dt = (DataTable)ViewState["Sanc"];
                vBranch = Session[gblValue.BrnchCode].ToString();
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkCancel = (CheckBox)row.FindControl("chkCancel");
                DropDownList ddlResn = (DropDownList)row.FindControl("ddlResn");
                if (checkbox.Checked == true)
                {
                    ddlResn.Enabled = true;
                    dt.Rows[row.RowIndex]["Cancel"] = "Y";
                    dt.Rows[row.RowIndex]["Approved"] = "N";
                }
                else
                {
                    dt.Rows[row.RowIndex]["Cancel"] = "N";
                    dt.Rows[row.RowIndex]["Approved"] = "Y";
                    ddlResn.Enabled = false;
                }
                dt.AcceptChanges();
                ViewState["Sanc"] = dt;
                upSanc.Update();
            }
            finally
            {
                dt = null;
            }
        }

        protected void txtAccountNo_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtAccountNo = (TextBox)gvRow.FindControl("txtAccountNo");
            dt = (DataTable)ViewState["Sanc"];

            if (txtAccountNo.Text != "")
            {
                dt.Rows[gvRow.RowIndex]["AccNo"] = txtAccountNo.Text;
                dt.AcceptChanges();
                return;
            }
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }
        protected void txtIfsc_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtIfsc = (TextBox)gvRow.FindControl("txtIfsc");
            dt = (DataTable)ViewState["Sanc"];

            if (txtIfsc.Text != "")
            {
                dt.Rows[gvRow.RowIndex]["IFSCCode"] = txtIfsc.Text;
                dt.AcceptChanges();
                return;
            }
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }


        protected void chkCash_CheckChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CheckBox checkbox = (CheckBox)sender;
            CheckBox checkbox2 = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            GridViewRow row1 = (GridViewRow)checkbox2.NamingContainer;
            CheckBox chkCancel = (CheckBox)row.FindControl("chkCancel");

            dt = (DataTable)ViewState["Sanc"];
            if (checkbox.Checked == true)
            {
                if (row.Cells[15].Text != "N")
                {
                    gblFuction.AjxMsgPopup("This application is already disbursed");
                    checkbox.Checked = false;
                    return;
                }
                else
                {
                    dt.Rows[row.RowIndex]["CashApproveYN"] = "Y";
                    chkCancel.Enabled = false;
                }
            }
            else
            {
                if (row.Cells[15].Text != "N")
                {
                    gblFuction.AjxMsgPopup("This application is already approved by HO");
                    checkbox.Checked = true;
                    return;
                }
                else
                {
                    dt.Rows[row.RowIndex]["CashApproveYN"] = "N";                    
                    chkCancel.Enabled = true;
                }
            }
            dt.AcceptChanges();
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }
        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }


        //protected void ChangePage(object sender, CommandEventArgs e)
        //{
        //    string vBrCode = Session[gblValue.BrnchCode].ToString();
        //    switch (e.CommandName)
        //    {
        //        case "Previous":
        //            cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
        //            break;
        //        case "Next":
        //            cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
        //            break;
        //    }

        //    LoadGrid(rdbOpt.SelectedValue, txtFrmDt.Text, txtToDt.Text, vBrCode, cPgNo);
        //    //tabLoanAppl.ActiveTabIndex = 0;
        //}
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }


        protected void gvSanc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = null;
            CCancelReason oCR = null;
            try               
            {
                DateTime vSanDt = gblFuction.setDate(txtAppDt.Text);
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Bind Approval
                    CheckBox chkCancel = (CheckBox)e.Row.FindControl("chkCancel");
                    CheckBox chkCash = (CheckBox)e.Row.FindControl("chkCash");
                    TextBox txtAccountNo = (TextBox)e.Row.FindControl("txtAccountNo");
                    TextBox txtIfsc = (TextBox)e.Row.FindControl("txtIfsc");
                    TextBox txtExpDisbDate = (TextBox)e.Row.FindControl("txtExpDisbDate");
                    DateTime vCBDate = gblFuction.setDate(e.Row.Cells[32].Text);
                    DropDownList ddlResn = (DropDownList)e.Row.FindControl("ddlResn");

                    if (e.Row.Cells[11].Text == "Y")
                    {
                        chkCancel.Checked = true;
                        txtExpDisbDate.Enabled = false;
                        txtIfsc.Enabled = false;
                        txtAccountNo.Enabled = false;
                    }
                    else if (e.Row.Cells[11].Text == "N")
                    {
                        chkCancel.Checked = false;
                    }
                     if(rdbOpt.SelectedValue=="N")
                    {
                         double vTotDays = (vSanDt - vCBDate).TotalDays;
                         if (vTotDays > 15)
                         {
                             e.Row.BackColor = System.Drawing.Color.PeachPuff;
                         }
                    }
                     oCR = new CCancelReason();
                     dt = oCR.GetCanResnByType("D");
                     ddlResn.DataSource = dt;
                     ddlResn.DataTextField = "CanReasonName";
                     ddlResn.DataValueField = "CanReasonId";
                     ddlResn.DataBind();
                     ListItem oL1 = new ListItem("<-- Select -->", "0");
                     ddlResn.Items.Insert(0, oL1);
                }
            }
            finally
            {

            }
        }
    }
}