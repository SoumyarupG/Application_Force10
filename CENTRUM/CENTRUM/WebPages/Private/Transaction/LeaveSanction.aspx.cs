using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class LeaveSanction : CENTRUMBase
    {
        protected int cPgNo = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                LoadGrid("N", txtFrmDt.Text, txtToDt.Text, 1);

            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Leave Sanction";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLeaveSanc);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Application", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (ValidDate() == true)
            {
                try
                {
                    if (rdbOpt.SelectedValue == "N")
                        LoadGrid("N", txtFrmDt.Text, txtToDt.Text, 1);
                    else if (rdbOpt.SelectedValue == "A")
                        LoadGrid("A", txtFrmDt.Text, txtToDt.Text, 1);
                    else if (rdbOpt.SelectedValue == "C")
                        LoadGrid("C", txtFrmDt.Text, txtToDt.Text, 1);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDone_Click(object sender, EventArgs e)
        {
            int rowIndex = 0;
            string vBody = "", vSubject = "", vEmail="";
            CLeaveApplication oApp = null;
            DataTable dt = new DataTable();
            DataTable dtS = new DataTable();
            CUser oUser = null;
            Int32 vErr = 0;
            string vXmlData = "";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vSanDt = gblFuction.setDate(txtAppDt.Text);
            if (ValidDate() == true)
            {
                try
                {
                    
                    oUser = new CUser();
                    dtS = oUser.GetUserById(this.UserID);
                    dt = (DataTable)ViewState["Sanc"];
                    if (dt == null) return;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Approved"].ToString() == "Y")
                        {
                            vSubject = "Leave Sanction";
                            vEmail = dt.Rows[i]["email"].ToString();
                            vBody = "dear " + dt.Rows[i]["EoName"].ToString() + "(" + dt.Rows[i]["EmpCode"].ToString() + ") Your " + dt.Rows[i]["ShortName"].ToString() + " Leave Application No-" + dt.Rows[i]["LeaveAppNo"].ToString() + " has been sanctioned by " + Convert.ToString(dtS.Rows[0]["EoName"]) + "(" + Convert.ToString(dtS.Rows[0]["UserName"]) + ") for " + dt.Rows[i]["SnDays"].ToString() + " days From " + dt.Rows[i]["SnFromDate"].ToString() + " To " + dt.Rows[i]["SnToDate"].ToString();
                            SendToMail(vEmail, vBody, vSubject);
                        }
                        else if (dt.Rows[i]["Cancel"].ToString() == "Y")
                        {
                            vSubject = "Leave Cancel";
                            vEmail = dt.Rows[i]["email"].ToString();
                            vBody = "dear " + dt.Rows[i]["EoName"].ToString() + "(" + dt.Rows[i]["EmpCode"].ToString() + ") Your " + dt.Rows[i]["ShortName"].ToString() + " Leave Application No-" + dt.Rows[i]["LeaveAppNo"].ToString() + " has been Cancelled by " + Convert.ToString(dtS.Rows[0]["EoName"]) + "(" + Convert.ToString(dtS.Rows[0]["UserName"]) + ") for " + dt.Rows[i]["LeaveDays"].ToString() + " days From " + dt.Rows[i]["FromDate"].ToString() + " To " + dt.Rows[i]["ToDate"].ToString();
                            SendToMail(vEmail, vBody, vSubject);
                        }
                        rowIndex++;
                    }

                    if (ValidateFields() == false) return;
                    oApp = new CLeaveApplication();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    //-----------XML Save----------
                    vErr = oApp.UpdateLvSanction(vXmlData, this.UserID, "E", 0, vSanDt);


                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
                        LoadGrid("A", txtFrmDt.Text, txtToDt.Text, 1);
                        rdbOpt.SelectedValue = "A";
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
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()//To Check
        {
            Boolean vResult = true;
            Int32 vRow = 0;
            for (vRow = 0; vRow < gvSanc.Rows.Count; vRow++)
            {
                CheckBox chkApp = (CheckBox)gvSanc.Rows[vRow].FindControl("chkApp");
                CheckBox chkCan = (CheckBox)gvSanc.Rows[vRow].FindControl("chkCan");
                Label txtSanDt = (Label)gvSanc.Rows[vRow].FindControl("txtSanDt");
                TextBox txtSnFromDt = (TextBox)gvSanc.Rows[vRow].FindControl("txtSnFromDt");
                TextBox txtSnToDt = (TextBox)gvSanc.Rows[vRow].FindControl("txtSnToDt");
                Label txtSnDays = (Label)gvSanc.Rows[vRow].FindControl("txtSnDays");
                TextBox txtCanReason = (TextBox)gvSanc.Rows[vRow].FindControl("txtCanReason");

                if (chkApp.Checked == true && txtSanDt.Text.Trim() == "")
                {
                    gblFuction.MsgPopup("Please Enter Sanction Date..");
                    vResult = false;
                }

                if (chkApp.Checked == true && txtSnDays.Text == "")
                {
                    gblFuction.MsgPopup("Please Enter Sanction days..");
                    vResult = false;
                }
                if (chkApp.Checked == true && txtSnFromDt.Text == "")
                {
                    gblFuction.MsgPopup("Please Enter Sanction From Date");
                    vResult = false;
                }

                if (chkApp.Checked == true && txtSnToDt.Text == "")
                {
                    gblFuction.MsgPopup("Please Enter Sanction To Date");
                    vResult = false;
                }

                if (chkCan.Checked == true && txtCanReason.Text.Trim() == "")
                {
                    gblFuction.MsgPopup("Please select the Reason for Cancel loan..");
                    vResult = false;
                }

                if (chkApp.Checked == true && gblFuction.IsDate(txtSanDt.Text) == false)
                {
                    gblFuction.MsgPopup("Please enter Proper Sanction Date..");
                    vResult = false;
                }
            }
            return vResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAppMode"></param>
        /// <param name="pFromDt"></param>
        /// <param name="pToDt"></param>
        /// <param name="pBranch"></param>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(string pAppMode, string pFromDt, string pToDt, Int32 pPgIndx)
        {
            DataTable dt = null;
            CLeaveApplication oLS = null;
            Int32 totalRows = 0;
            try
            {
                DateTime vFromDt = gblFuction.setDate(pFromDt);
                DateTime vToDt = gblFuction.setDate(pToDt);
                oLS = new CLeaveApplication();
                dt = oLS.GetLvSanctionList(vFromDt, vToDt, pAppMode, pPgIndx, ref totalRows, this.UserID);
                ViewState["Sanc"] = dt;
                gvSanc.DataSource = dt;
                gvSanc.DataBind();
                lblTotalPages.Text = CalTotPgs(totalRows).ToString();
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
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dt = null;
                oLS = null;
            }
        }


        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }


        protected void ChangePage(object sender, CommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }
            LoadGrid(rdbOpt.SelectedValue.ToString(), txtFrmDt.Text, txtToDt.Text, cPgNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkApp_CheckedChanged(object sender, EventArgs e)
        {
            DateTime vAppDate;
            string vLnAppDt = "", vAppId = "";
            DataTable dt = null;

            try
            {
                dt = (DataTable)ViewState["Sanc"];
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkApp = (CheckBox)row.FindControl("chkApp");
                CheckBox chkCan = (CheckBox)row.FindControl("chkCan");
                Label txtSanDt = (Label)row.FindControl("txtSanDt");
                Label txtAppDate = (Label)row.FindControl("txtAppDate");
                Label txtLvFrom = (Label)row.FindControl("txtLvFrom");
                Label txtLvTo = (Label)row.FindControl("txtLvTo");
                Label txtLvDays = (Label)row.FindControl("txtLvDays");
                TextBox txtSnFromDt = (TextBox)row.FindControl("txtSnFromDt");
                TextBox txtSnToDt = (TextBox)row.FindControl("txtSnToDt");
                Label txtSnDays = (Label)row.FindControl("txtSnDays");
                TextBox txtCanReason = (TextBox)row.FindControl("txtCanReason");
                Button btnFrd = (Button)row.FindControl("btnFrd");

                vAppId = row.Cells[19].Text;
                if (txtAppDt.Text != "" || gblFuction.IsDate(txtAppDt.Text) == true)
                    vAppDate = gblFuction.setDate(txtAppDt.Text);
                else
                    gblFuction.AjxMsgPopup("Enter Sanction Date");

                if (chkApp.Checked == true && chkCan.Checked == false)
                {
                    vLnAppDt = txtAppDate.Text;
                    if (gblFuction.setDate(vLnAppDt) > gblFuction.setDate(txtAppDt.Text))
                    {
                        gblFuction.AjxMsgPopup("Sanction Date can not be less than the Application Date.");
                        row.Cells[17].Text = "N";
                        txtSanDt.Text = "";
                        chkApp.Checked = false;
                        chkCan.Checked = false;
                        btnFrd.Enabled = true;
                        dt.Rows[row.RowIndex]["Approved"] = "N";
                        dt.Rows[row.RowIndex]["SanDate"] = "";
                        dt.AcceptChanges();
                        upSanc.Update();
                        return;
                    }
                    else
                    {
                        row.Cells[17].Text = "Y";
                        txtSanDt.Text = txtAppDt.Text;
                        txtSnFromDt.Text = txtLvFrom.Text;
                        txtSnToDt.Text = txtLvTo.Text;
                        txtSnDays.Text = txtLvDays.Text;
                        txtCanReason.Text = "";
                        dt.Rows[row.RowIndex]["Approved"] = "Y";
                        dt.Rows[row.RowIndex]["SanDate"] = txtAppDt.Text;
                        dt.Rows[row.RowIndex]["SnFromDate"] = txtSnFromDt.Text;
                        dt.Rows[row.RowIndex]["SnToDate"] = txtSnToDt.Text;
                        dt.Rows[row.RowIndex]["SnDays"] = txtSnDays.Text;
                        dt.Rows[row.RowIndex]["CancelRsn"] = "";
                        dt.AcceptChanges();
                    }
                    txtCanReason.Enabled = false;
                    chkCan.Enabled = false;
                    btnFrd.Enabled = false;
                }
                else if (chkApp.Checked == true && chkCan.Checked == true)
                {
                    gblFuction.AjxMsgPopup("Sanction and Cancel can not be Selected.");
                    chkApp.Checked = false;
                    chkCan.Checked = false;
                    row.Cells[17].Text = "N";
                    row.Cells[18].Text = "N";
                    btnFrd.Enabled = false;
                    txtSanDt.Text = "";
                    txtSnFromDt.Text ="";
                    txtSnToDt.Text = "";
                    txtSnDays.Text = "0";
                    txtCanReason.Text = "";
                    dt.Rows[row.RowIndex]["Approved"] = "N";
                    dt.Rows[row.RowIndex]["Cancel"] = "N";
                    dt.Rows[row.RowIndex]["SanDate"] = txtSanDt.Text;
                    dt.Rows[row.RowIndex]["SnFromDate"] = txtSnFromDt.Text;
                    dt.Rows[row.RowIndex]["SnToDate"] = txtSnToDt.Text;
                    dt.Rows[row.RowIndex]["SnDays"] = txtSnDays.Text;
                    dt.Rows[row.RowIndex]["CancelRsn"] = "";
                    dt.AcceptChanges();
                    upSanc.Update();
                    return;
                }
                else
                {
                    //txtReason.Enabled = true;
                    chkCan.Enabled = true;
                    row.Cells[17].Text = "N";
                    txtSanDt.Text = "";
                    txtSnFromDt.Text = "";
                    txtSnToDt.Text = "";
                    txtSnDays.Text = "0";
                    txtCanReason.Text = "";
                    dt.Rows[row.RowIndex]["Approved"] = "N";
                    dt.Rows[row.RowIndex]["SanDate"] = "";
                    dt.Rows[row.RowIndex]["SnFromDate"] = "";
                    dt.Rows[row.RowIndex]["SnToDate"] = "";
                    dt.Rows[row.RowIndex]["SnDays"] = "0";
                    dt.Rows[row.RowIndex]["CancelRsn"] = "";
                    dt.AcceptChanges();
                }
                ViewState["Sanc"] = dt;
                upSanc.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkCan_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CheckBox checkbox = (CheckBox)sender;
            CheckBox checkbox2 = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            GridViewRow row1 = (GridViewRow)checkbox2.NamingContainer;
            CheckBox chkApp = (CheckBox)row.FindControl("chkApp");
            Label txtSanDt = (Label)row.FindControl("txtSanDt");
            TextBox txtCanReason = (TextBox)row.FindControl("txtCanReason");
            TextBox txtSnFromDt = (TextBox)row.FindControl("txtSnFromDt");
            TextBox txtSnToDt = (TextBox)row.FindControl("txtSnToDt");
            Label txtSnDays = (Label)row.FindControl("txtSnDays");
            Button btnFrd = (Button)row.FindControl("btnFrd");

            dt = (DataTable)ViewState["Sanc"];
            if (checkbox.Checked == true)
            {
                if (row.Cells[17].Text == "Y")
                {
                    checkbox2.Checked = false;
                    gblFuction.AjxMsgPopup("Please remove Sanction before make Cancel.");
                    row.Cells[18].Text = "N";
                    dt.Rows[row.RowIndex]["Cancel"] = "N";
                    dt.AcceptChanges();
                    upSanc.Update();
                    return;
                }
                txtCanReason.Enabled = true;
                txtSnFromDt.Enabled = false;
                txtSnToDt.Enabled = false;
                txtSnFromDt.Text = "";
                txtSnToDt.Text = "";
                txtSnDays.Text = "0";
                btnFrd.Enabled = false;
                chkApp.Enabled = false;
                row.Cells[18].Text = "Y";
                txtSanDt.Text = "";
                dt.Rows[row.RowIndex]["Cancel"] = "Y";
                dt.Rows[row.RowIndex]["SanDate"] = "";
                dt.Rows[row.RowIndex]["SnFromDate"] = "";
                dt.Rows[row.RowIndex]["SnToDate"] = "";
                dt.Rows[row.RowIndex]["SnDays"] = "0";
                dt.Rows[row.RowIndex]["CancelRsn"] = txtCanReason.Text;
                dt.AcceptChanges();
            }
            else
            {
                txtCanReason.Enabled = false;
                txtSnFromDt.Enabled = true;
                txtSnToDt.Enabled = true;
                chkApp.Enabled = true;
                btnFrd.Enabled = true;
                row.Cells[18].Text = "N";
                dt.Rows[row.RowIndex]["Cancel"] = "N";
                dt.AcceptChanges();
            }
            txtSanDt.Text = "";
            dt.Rows[row.RowIndex]["SanDate"] = "";
            dt.Rows[row.RowIndex]["SnFromDate"] = "";
            dt.Rows[row.RowIndex]["SnToDate"] = "";
            dt.Rows[row.RowIndex]["SnDays"] = "0";
            dt.Rows[row.RowIndex]["CancelRsn"] = txtCanReason.Text;
            dt.AcceptChanges();
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtSnFromDt_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            Label txtAppDate = (Label)gvRow.FindControl("txtAppDate");
            TextBox txtSnFromDt = (TextBox)gvRow.FindControl("txtSnFromDt");
            TextBox txtSnToDt = (TextBox)gvRow.FindControl("txtSnToDt");
            Label txtLvFrom = (Label)gvRow.FindControl("txtLvFrom");
            Label txtLvto = (Label)gvRow.FindControl("txtLvTo");
            Label txtSnDays = (Label)gvRow.FindControl("txtSnDays");
            
            dt = (DataTable)ViewState["Sanc"];

            DateTime vAppDate = gblFuction.setDate(txtAppDate.Text);

            if (txtSnFromDt.Text == "" || gblFuction.IsDate(txtSnFromDt.Text) == false)
            {
                gblFuction.AjxMsgPopup("Sanction From Date Invalid...");
                txtSnFromDt.Text = txtLvFrom.Text;
                dt.Rows[gvRow.RowIndex]["SnFromDate"] = txtLvFrom.Text;
                dt.Rows[gvRow.RowIndex]["SnDays"] = "0";
                dt.AcceptChanges();
                return;
            }
            DateTime vSnFromDate = gblFuction.setDate(txtSnFromDt.Text);

            if (vSnFromDate < vAppDate)
            {
                gblFuction.AjxMsgPopup("Sanction From Date Can not be Less Than Application Date");
                txtSnFromDt.Text = txtLvFrom.Text;
                dt.Rows[gvRow.RowIndex]["SnFromDate"] = txtAppDt.Text;
                dt.Rows[gvRow.RowIndex]["SnDays"] = "0";
                dt.AcceptChanges();
                return;
            }

            if (txtSnToDt.Text != "" || gblFuction.IsDate(txtSnToDt.Text) == true)
            {
                DateTime vSnToDate = gblFuction.setDate(txtSnToDt.Text);
                if (vSnToDate < vSnFromDate)
                {
                    gblFuction.AjxMsgPopup("Sanction To Date cannot be less than from Date...");
                    txtSnFromDt.Text = txtSnToDt.Text;
                    dt.Rows[gvRow.RowIndex]["SnFromDate"] = txtSnToDt.Text;
                    dt.Rows[gvRow.RowIndex]["SnDays"] = "1";
                    dt.AcceptChanges();
                    return;
                }
                txtSnDays.Text = Convert.ToString((vSnToDate.Day - vSnFromDate.Day)+1);
             }

            dt.Rows[gvRow.RowIndex]["SnFromDate"] = txtSnFromDt.Text;
            dt.Rows[gvRow.RowIndex]["SnDays"] = txtSnDays.Text;
            dt.AcceptChanges();
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }

        protected void txtSnToDt_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            Label txtAppDate = (Label)gvRow.FindControl("txtAppDate");
            TextBox txtSnFromDt = (TextBox)gvRow.FindControl("txtSnFromDt");
            TextBox txtSnToDt = (TextBox)gvRow.FindControl("txtSnToDt");
            Label txtLvFrom = (Label)gvRow.FindControl("txtLvFrom");
            Label txtLvto = (Label)gvRow.FindControl("txtLvTo");
            Label txtSnDays = (Label)gvRow.FindControl("txtSnDays");

            dt = (DataTable)ViewState["Sanc"];

            DateTime vAppDate = gblFuction.setDate(txtAppDate.Text);

            if (txtSnToDt.Text == "" || gblFuction.IsDate(txtSnToDt.Text) == false)
            {
                gblFuction.AjxMsgPopup("Sanction To Date Invalid...");
                txtSnToDt.Text = txtLvto.Text;
                dt.Rows[gvRow.RowIndex]["SnToDate"] = txtLvto.Text;
                dt.Rows[gvRow.RowIndex]["SnDays"] = "0";
                dt.AcceptChanges();
                return;
            }
            DateTime vSnToDate = gblFuction.setDate(txtSnToDt.Text);

            if (txtSnFromDt.Text != "" || gblFuction.IsDate(txtSnFromDt.Text) == true)
            {
                DateTime vSnFromDate = gblFuction.setDate(txtSnFromDt.Text);
                if (vSnToDate < vSnFromDate)
                {
                    gblFuction.AjxMsgPopup("Sanction To Date cannot be less than from Date...");
                    txtSnToDt.Text = txtSnFromDt.Text;
                    dt.Rows[gvRow.RowIndex]["SnToDate"] = txtSnFromDt.Text;
                    dt.Rows[gvRow.RowIndex]["SnDays"] = "1";
                    dt.AcceptChanges();
                    return;
                }
                txtSnDays.Text = Convert.ToString((vSnToDate.Day - vSnFromDate.Day) + 1);
            }

            dt.Rows[gvRow.RowIndex]["SnToDate"] = txtSnToDt.Text;
            dt.Rows[gvRow.RowIndex]["SnDays"] = txtSnDays.Text;
            dt.AcceptChanges();
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSanc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Bind Approval
                    CheckBox chkApp = (CheckBox)e.Row.FindControl("chkApp");
                    CheckBox chkCan = (CheckBox)e.Row.FindControl("chkCan");
                    TextBox txtCanReason = (TextBox)e.Row.FindControl("txtCanReason");
                    Label txtSanDt = (Label)e.Row.FindControl("txtSanDt");
                    Button btnFrd = (Button)e.Row.FindControl("btnFrd");

                    if (e.Row.Cells[17].Text == "Y")
                    {
                        chkApp.Checked = true;
                        chkCan.Enabled = false;
                        txtCanReason.Enabled = false;
                        txtSanDt.Enabled = true;
                        btnFrd.Enabled = false;
                    }
                    else if (e.Row.Cells[17].Text == "N")
                    {
                        chkApp.Checked = false;
                        chkCan.Enabled = true;
                        txtCanReason.Enabled = true;
                        txtSanDt.Enabled = false;
                        btnFrd.Enabled = true;
                    }

                    //Bind Calcel

                    if (e.Row.Cells[18].Text == "Y")
                    {
                        chkCan.Checked = true;
                        chkApp.Enabled = false;
                        txtCanReason.Enabled = true;
                        btnFrd.Enabled = false;
                    }
                    else if (e.Row.Cells[18].Text == "N")
                    {
                        chkCan.Checked = false;
                        chkApp.Enabled = true;
                        txtCanReason.Enabled = false;
                        btnFrd.Enabled = true;
                    }

                    if (e.Row.Cells[22].Text == Convert.ToString(this.UserID))
                    {
                        btnFrd.Enabled = true;
                        chkCan.Enabled = true;
                        chkApp.Enabled = true;
                    }
                    else if (e.Row.Cells[21].Text == Convert.ToString(this.UserID) && e.Row.Cells[22].Text == "0")
                    {
                        btnFrd.Enabled = true;
                        chkCan.Enabled = true;
                        chkApp.Enabled = true;
                    }
                    else
                    {
                        btnFrd.Enabled = false;
                        chkCan.Enabled = false;
                        chkApp.Enabled = false;
                    }

                }
            }
            finally
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSanc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vEoId = "";
            DataTable dt = null;
            CLeaveApplication oApp = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                if (e.CommandName == "cmdShow")
                {
                    vEoId = Convert.ToString(e.CommandArgument);
                    if (vEoId !="")
                    {
                        oApp = new CLeaveApplication();
                        dt = oApp.GetEoLeaveDtl(vEoId);
                        gvLnDtl.DataSource = dt;
                        gvLnDtl.DataBind();
                    }
                    else
                    {
                        gvLnDtl.DataSource = null;
                        gvLnDtl.DataBind();
                    }
                }
            }
            finally
            {
                dt = null;
                oApp = null;
            }
        }


        protected void btnFrd_Click(object sender, EventArgs e)
        {
            string vAppId = "", vEmail = "", vUID = "", vBody="", vSubject="";
            DataTable dt = null;
            DataTable dt1 = null;
            CLeaveApplication oLv = null;
            try
            {
                dt = (DataTable)ViewState["Sanc"];
                Button button = (Button)sender;
                GridViewRow row = (GridViewRow)button.NamingContainer;
                Label txtAppDate = (Label)row.FindControl("txtAppDate");
                Label txtLvFrom = (Label)row.FindControl("txtLvFrom");
                Label txtLvTo = (Label)row.FindControl("txtLvTo");
                Label txtLvDays = (Label)row.FindControl("txtLvDays");
                Label txtEmpCode = (Label)row.FindControl("txtEmpCode");
                Label txtEmp = (Label)row.FindControl("txtEmp");
                Label txtLvType = (Label)row.FindControl("txtLvType");

                vAppId = row.Cells[19].Text;
                oLv = new CLeaveApplication();
                dt1 = oLv.GetSupervisoUID("", this.UserID, "U", gblFuction.setDate(txtAppDt.Text));
                if (dt1.Rows.Count>0)
                    vUID = dt1.Rows[0]["UserID"].ToString();

                dt1 = oLv.GetSupervisorEmail("", this.UserID, "U", gblFuction.setDate(txtAppDt.Text));
                if (dt1.Rows.Count > 0)
                    vEmail = dt1.Rows[0]["email"].ToString();

                if (row.Cells[22].Text == Convert.ToString(this.UserID))
                {
                    dt.Rows[row.RowIndex]["FrdUId2"] = vUID;
                    row.Cells[23].Text = vUID;
                }
                if (row.Cells[21].Text == Convert.ToString(this.UserID) && row.Cells[22].Text == "0")
                {
                    dt.Rows[row.RowIndex]["FrdUId1"] = vUID;
                    row.Cells[22].Text = vUID;
                }
                else
                {
                    dt.Rows[row.RowIndex]["FrdUId"] = vUID;
                    row.Cells[21].Text = vUID;
                }
                vSubject = "Leave Application From " + txtEmp.Text;
                vBody = "Code:" + txtEmp.Text + " Name: " + txtEmp.Text + " Applied for " + txtLvType.Text + " From: " + txtLvFrom.Text + " To " + txtLvTo.Text;
                SendToMail(vEmail, vBody, vSubject);

                dt.AcceptChanges();
                ViewState["Sanc"] = dt;
                upSanc.Update();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                dt1 = null;
            }
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
                    oM.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Host = "smtp.gmail.com";
                    smtp.Credentials = new System.Net.NetworkCredential(vCompEmail, vCompPwd);
                    smtp.EnableSsl = true;
                    //smtp.Timeout = 360000;
                    smtp.Send(oM);
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //CEMail oEm = new CEMail();
                //oEm.SaveEMailStat(pReqNo, vBody, vMTo, "", "ebankhelpdesk@gmail.com", "Internal Request Service No:" + pReqNo, "Save");
                //gblFuction.MsgPopup("Not able to Send Email.......");  
            }
        }
        
    }
}
