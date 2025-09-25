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
    public partial class NpsCollRemitt : CENTRUMBase
    {
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
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                LoadGrid(txtFrmDt.Text, txtToDt.Text, 0);
                txtRemittDt.Text = Session[gblValue.LoginDate].ToString();
                txtRemittDt.Enabled = false;
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
                this.PageHeading = "NPS Collection Remittance";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuNpsCollRemitt);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = true;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                    btnDone.Visible = true;
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "NPS Collection Remittance", false);
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
            if (txtRemittDt.Text == "")
            {
                gblFuction.MsgPopup("Date of Remittance should not be blank.");
                gblFuction.focus("ctl00_cph_Main_txtRemittDt");
                vResult = false;
            }
            if (gblFuction.IsDate(txtRemittDt.Text) == false)
            {
                gblFuction.MsgPopup(gblMarg.ValidDate);
                gblFuction.focus("ctl00_cph_Main_txtRemittDt");
                vResult = false;
            }
            if (gblFuction.setDate(txtRemittDt.Text) < vFinFrmDt || gblFuction.setDate(txtRemittDt.Text) > vFinToDt)
            {
                gblFuction.MsgPopup("Date of Remittance should be Within this Financial Year");
                gblFuction.focus("ctl00_cph_Main_txtRemittDt");
                vResult = false;
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

            if (ValidDate() == true)
            {
                try
                {
                    dt = GetTable();
                    if (dt.Rows.Count <= 0)
                    {
                        gblFuction.MsgPopup("No Records to Process... ");
                        return;
                    }
                    if (this.RoleId != 1)  //Admin and NPS AGM Role 
                    {
                        if (Session[gblValue.EndDate] != null)
                        {
                            if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtRemittDt.Text))
                            {
                                gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                                return;
                            }
                        }
                    }
                    oApp = new CNpsMember();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    vErr = oApp.NPS_UpdateCollection(vXmlData, gblFuction.setDate(txtRemittDt.Text), Convert.ToDouble(txtTotAmt.Text), Session[gblValue.BrnchCode].ToString(),
                        vActMstTbl, vActDtlTbl, vFinYear, this.UserID);
                    if (vErr > 0)
                    {
                        LoadGrid(txtFrmDt.Text, txtToDt.Text, 0);
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
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
                dt = oMem.NPS_GetRemittList(vFromDt, vToDt, Session[gblValue.BrnchCode].ToString());
                gvSanc.DataSource = dt;
                gvSanc.DataBind();
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
            DataColumn dc2 = new DataColumn("RemittYN");
            dt.Columns.Add(dc2);
            foreach (GridViewRow gR in gvSanc.Rows)
            {
                CheckBox chkApp = (CheckBox)gR.FindControl("chkApp");
                TextBox txtSend = (TextBox)gR.FindControl("txtSend");
                if (chkApp.Checked == true)
                {
                    DataRow dR = dt.NewRow();
                    dR["CollId"] = gR.Cells[6].Text.Trim();
                    dR["RemittYN"] = txtSend.Text;
                    dt.Rows.Add(dR);
                }
            }
            return dt;
        }

        protected void chkApp_CheckedChanged(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable("Table1");
            //DataColumn dc1 = new DataColumn("MemId");
            double vTotalAmt = 0;
            //dt.Columns.Add(dc1);
            //DataColumn dc2 = new DataColumn("SendYN");
            //dt.Columns.Add(dc2);
            foreach (GridViewRow gR in gvSanc.Rows)
            {
                CheckBox chkApp = (CheckBox)gR.FindControl("chkApp");
                //TextBox txtSend = (TextBox)gR.FindControl("txtSend");
                if (chkApp.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[4].Text.Trim());
                }
            }
            txtTotAmt.Text = Convert.ToString(vTotalAmt);
            UpTot.Update();
        }
    }
}