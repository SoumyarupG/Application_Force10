using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class NpsPFRDReturn : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                //LoadGrid(txtFrmDt.Text, txtToDt.Text, 0);
                txtDt.Text = Session[gblValue.LoginDate].ToString();
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
                this.PageHeading = "Send to PFRDA/Return to Member";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuNpsSendPFRDA);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = true;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                    btnDone.Visible = true;
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "NPS Send to PF RD/Return to Member", false);
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
            if (ValidDate() == true) LoadGrid(txtFrmDt.Text, txtToDt.Text, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidDate()
        {
           
            DateTime vFinFrmDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
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
            if (txtDt.Text == "")
            {
                gblFuction.MsgPopup("Date of Remittance should not be blank.");
                gblFuction.focus("ctl00_cph_Main_txtRemittDt");
                vResult = false;
            }
            if (gblFuction.IsDate(txtDt.Text) == false)
            {
                gblFuction.MsgPopup(gblMarg.ValidDate);
                gblFuction.focus("ctl00_cph_Main_txtRemittDt");
                vResult = false;
            }
            if (gblFuction.setDate(txtDt.Text) < vFinFrmDt || gblFuction.setDate(txtDt.Text) > vFinToDt)
            {
                gblFuction.MsgPopup("Date of Remittance should be Within this Financial Year");
                gblFuction.focus("ctl00_cph_Main_txtRemittDt");
                vResult = false;
            }
            if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtDt.Text))
            {
                gblFuction.AjxMsgPopup("Day End already done...");
                vResult = false;
            }
            if (gblFuction.setDate(txtToDt.Text) > gblFuction.setDate(txtDt.Text))
            {
                gblFuction.AjxMsgPopup("To Date should not be greater than login date...");
                vResult = false;
            }
            foreach (GridViewRow gR in gvSanc.Rows)
            {
                CheckBox chkReturn = (CheckBox)gR.FindControl("chkReturn");
                TextBox txtChequeNo = (TextBox)gR.FindControl("txtChequeNo");
                if (chkReturn.Checked == true)
                {

                    if (txtChequeNo.Text == "")
                    {
                        gblFuction.AjxMsgPopup("Cheque Number Should not be blank...");
                        vResult = false;
                    }
                    
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
            CNpsMember oApp = null;
            DataTable dt = null;
            Int32 vErr = 0;
            string vXmlData = "";
            string vActMstTbl = Session[gblValue.ACVouMst].ToString();
            string vActDtlTbl = Session[gblValue.ACVouDtl].ToString();
            string vFinYear = Session[gblValue.ShortYear].ToString();
            double vTotAmt = 0;

            if (ValidDate() == true)
            {
                if (this.RoleId != 1)  //Admin and NPS AGM Role 
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtDt.Text))
                        {
                            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                            return;
                        }
                    }
                }
                try
                {
                    dt = GetTable();
                    if (dt.Rows.Count <= 0)
                    {
                        gblFuction.MsgPopup("No Records to Process... ");
                        return;
                    }
                    oApp = new CNpsMember();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    Double.TryParse(Request[txtTotAmt.UniqueID] as string,out vTotAmt);
                    vErr = oApp.NPS_SavePFRD(vXmlData, gblFuction.setDate(txtDt.Text), vTotAmt, 
                        vActMstTbl, vActDtlTbl, vFinYear, this.UserID);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
                        LoadGrid(txtFrmDt.Text, txtToDt.Text, 0);
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
        /// <param name="pAppMode"></param>
        /// <param name="pFromDt"></param>
        /// <param name="pToDt"></param>
        /// <param name="pBranch"></param>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(string pFromDt, string pToDt, Int32 pPgIndx)
        {
            DataTable dt = null;
            CNpsMember oMem = null;
            try
            {
                DateTime vFromDt = gblFuction.setDate(pFromDt);
                DateTime vToDt = gblFuction.setDate(pToDt);
                oMem = new CNpsMember();
                dt = oMem.NPS_GetSendToPFList(vFromDt, vToDt, ddlPRAN.SelectedValue);
                gvSanc.DataSource = dt;
                gvSanc.DataBind();
                foreach (GridViewRow gR in gvSanc.Rows)
                {
                    CheckBox chkSend = (CheckBox)gR.FindControl("chkSend");
                    if (ddlPRAN.SelectedValue == "N")
                    {
                        chkSend.Enabled = false;
                    }
                    else
                    {
                        chkSend.Enabled = true;
                    }
                }
                
            }
            finally
            {
                dt = null;
                oMem = null;
            }
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
        /// <returns></returns>
        private DataTable GetTable()
        {
            DataTable dt = new DataTable("Table1");
            DataColumn dc1 = new DataColumn("CollId");
            dt.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn("SendToPF");
            dt.Columns.Add(dc2);
            DataColumn dc3 = new DataColumn("Return");
            dt.Columns.Add(dc3);
            DataColumn dc4 = new DataColumn("ChequeNo");
            dt.Columns.Add(dc4);
            DataColumn dc5 = new DataColumn("BranchCode");
            dt.Columns.Add(dc5);
            DataColumn dc6 = new DataColumn("Amt");
            dt.Columns.Add(dc6);
            foreach (GridViewRow gR in gvSanc.Rows)
            {
                CheckBox chkSend = (CheckBox)gR.FindControl("chkSend");
                CheckBox chkReturn = (CheckBox)gR.FindControl("chkReturn");
                TextBox txtSend = (TextBox)gR.FindControl("txtSend");
                TextBox txtReturn = (TextBox)gR.FindControl("txtReturn");
                TextBox txtChequeNo = (TextBox)gR.FindControl("txtChequeNo");
                if (chkSend.Checked == true)
                {
                    DataRow dR = dt.NewRow();
                    dR["CollId"] = gR.Cells[6].Text.Trim();
                    dR["SendToPF"] = "Y";
                    dR["Return"] = "N";
                    dR["ChequeNo"] = "";
                    dR["BranchCode"] = gR.Cells[16].Text.Trim();
                    dR["Amt"] = gR.Cells[4].Text.Trim();
                    dt.Rows.Add(dR);
                }
                if (chkReturn.Checked == true)
                {
                    DataRow dR = dt.NewRow();
                    dR["CollId"] = gR.Cells[6].Text.Trim();
                    dR["SendToPF"] = "N";
                    dR["Return"] = "Y";
                    dR["ChequeNo"] = txtChequeNo.Text;
                    dR["BranchCode"] = gR.Cells[16].Text.Trim();
                    dR["Amt"] = gR.Cells[4].Text.Trim();
                    dt.Rows.Add(dR);
                }
            }
            return dt;
        }

        protected void chkSend_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            CheckBox checkbox2 = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            GridViewRow row1 = (GridViewRow)checkbox2.NamingContainer;
            CheckBox chkReturn = (CheckBox)row.FindControl("chkReturn");
            double vTotalAmt = 0;
            foreach (GridViewRow gR in gvSanc.Rows)
            {
                CheckBox chkApp = (CheckBox)gR.FindControl("chkSend");
                //TextBox txtSend = (TextBox)gR.FindControl("txtSend");
                if (chkApp.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[4].Text.Trim());
                } 
                
            }
            txtTotAmt.Text = Convert.ToString(vTotalAmt);
            
            UpTot.Update();
            if (checkbox.Checked == true)
            {
                chkReturn.Enabled = false;
            }
            else
            {
                chkReturn.Enabled = true;

            }
            
           
        }
        protected void chkReturn_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            CheckBox checkbox2 = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            GridViewRow row1 = (GridViewRow)checkbox2.NamingContainer;
            CheckBox chkSend = (CheckBox)row.FindControl("chkSend");
            double vTotalAmt = 0;
            foreach (GridViewRow gR in gvSanc.Rows)
            {
                CheckBox chkApp = (CheckBox)gR.FindControl("chkReturn");
                //TextBox txtSend = (TextBox)gR.FindControl("txtSend");
                if (chkApp.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[4].Text.Trim());
                }

            }
            txtReturnTotAmt.Text = Convert.ToString(vTotalAmt);
            if (checkbox.Checked == true)
            {
                chkSend.Enabled = false;
            }
            else
            {
                chkSend.Enabled = true;
                
            }

        }
        
    }
}