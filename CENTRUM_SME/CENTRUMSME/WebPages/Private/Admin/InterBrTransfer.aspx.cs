using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;


namespace CENTRUMSME.WebPages.Private.Admin
{
    public partial class InterBrTransfer : CENTRUMBAse
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
            if (!IsPostBack)
            {
                txtTrDt.Text = Session[gblValue.LoginDate].ToString();
                popCOSrc();
                //popCODest();
                ddlRoSrc.Enabled = true;
                ddlRoDes.Enabled = true;
                //ddlGrpSrc.Enabled = false;
                //ddlGrpDes.Enabled = false;
                btnTransfer.Enabled = true;
                PopBranch();
                ddlType.Enabled = false;
            }
        }

        private void PopBranch()
        {
            ViewState["ID"] = null;
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            try
            {
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                oCG = new CGblIdGenerator();
                dt = oCG.PopTransferMIS("N", "BranchMst", "", vLogDt, vBrCode);
                ddlBr.DataSource = dt;
                ddlBr.DataTextField = "BranchName";
                ddlBr.DataValueField = "BranchCode";
                ddlBr.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBr.Items.Insert(0, oli);
            }
            finally
            {
                dt = null;
                oCG = null;
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
                this.PageHeading = "Inter Branch Transfer";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuIntrBrTransfr);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Inter Branch group Transfer", false);
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
        /// <param name="pRows"></param>
        /// <returns></returns>
        private int CalTotPages(double pRows)
        {
            int vPgs = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vPgs;
        }

        /// <summary>
        /// 
        /// </summary>
        private void popCOSrc()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oGb = new CGblIdGenerator();
                dt = oGb.PopTransferMIS("Y", "EoMst", "", vLogDt, vBrCode);
                ddlRoSrc.DataSource = dt;
                ddlRoSrc.DataTextField = "EoName";
                ddlRoSrc.DataValueField = "EoId";
                ddlRoSrc.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlRoSrc.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popCODes()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = ddlBr.SelectedValue.ToString();
                oGb = new CGblIdGenerator();
                dt = oGb.PopTransferMIS("Y", "EoMst", "", vLogDt, vBrCode);
                ddlRoDes.DataSource = dt;
                ddlRoDes.DataTextField = "EoName";
                ddlRoDes.DataValueField = "EoId";
                ddlRoDes.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlRoDes.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>

        /// <summary>
        /// 
        /// </summary>
        private void popGroup()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oGb = new CGblIdGenerator();
                dt = oGb.PopTransferMIS("N", "GroupMst", "", vLogDt, vBrCode);
                //foreach (DataRow row in dt.Rows)
                //{
                //    if (Convert.ToString(row["GroupName"]).Substring(0, 10) == "Individual")
                //        row.Delete();
                //}
                //ddlGrpSrc.DataSource = dt;
                //ddlGrpSrc.DataTextField = "GroupName";
                //ddlGrpSrc.DataValueField = "Groupid";
                //ddlGrpSrc.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                //ddlGrpSrc.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popGroupDest()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                if (ddlType.SelectedValue == "W")
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }
                else
                {
                    if (ddlType.SelectedValue == "B")
                    {
                        vBrCode = ddlBr.SelectedValue;
                    }
                }

                oGb = new CGblIdGenerator();
                dt = oGb.PopTransferMIS("N", "GroupMst", "", vLogDt, vBrCode);
                ListItem oli = new ListItem("<--Select-->", "-1");
                //ddlGrpDes.DataSource = dt;
                //ddlGrpDes.DataTextField = "GroupName";
                //ddlGrpDes.DataValueField = "Groupid";
                //ddlGrpDes.DataBind();
                //ddlGrpDes.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlType.SelectedValue == "W")
                ddlBr.Enabled = false;
            else
                ddlBr.Enabled = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBr_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ddlRoDes.Items.Clear();
            //ddlCentrDes.Items.Clear();
            //ddlGrpDes.Items.Clear();
            popCODes();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCheckList();
        }

        private void LoadCheckList()
        {
            chkDestn.Items.Clear();
            rblOptn.SelectedValue = "A";
            chkSource.Items.Clear();
            DataTable dt = null;
            CTransfer oTr = null;
            string vBrCode = "", vRoId = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vRoId = ddlRoSrc.SelectedValue;
                oTr = new CTransfer();
                if (rblSel.SelectedValue == "M")
                {
                    dt = oTr.PopMember(vRoId);
                }
                else
                {
                    dt = oTr.PopLead(vRoId);
                }
                chkSource.DataSource = dt;
                chkSource.DataTextField = "Member";
                chkSource.DataValueField = "MemberID";
                chkSource.DataBind();
                foreach (ListItem oLi in chkSource.Items)
                    oLi.Selected = true;
                chkSource.Enabled = false;
            }
            finally
            {
                oTr = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRoSrc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCheckList();
        }
        
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, string> PopTransfer(CheckBoxList vContrl, string vMode)
        {
            Dictionary<string, string> oDict = new Dictionary<string, string>();
            if (vMode == "Add")
            {
                foreach (ListItem oLi in vContrl.Items)
                {
                    if (oLi.Selected == true)
                        oDict.Add(oLi.Value, oLi.Text);
                }
            }
            if (vMode == "Del")
            {
                foreach (ListItem oLi in vContrl.Items)
                {
                    if (oLi.Selected == false)
                        oDict.Add(oLi.Value, oLi.Text);
                }
            }
            return oDict;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFwd_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> oDict = PopTransfer(chkSource, "Add");
            chkDestn.DataSource = oDict;
            chkDestn.DataTextField = "value";
            chkDestn.DataValueField = "key";
            chkDestn.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> oDict = PopTransfer(chkDestn, "Del");
            chkDestn.DataSource = oDict;
            chkDestn.DataTextField = "value";
            chkDestn.DataValueField = "key";
            chkDestn.DataBind();
        }
        protected void chkDestn_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            gblFuction.MsgPopup("hello");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblOptn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblOptn.SelectedValue == "S")
            {
                chkSource.Enabled = true;
                chkSource.ClearSelection();
            }
            else
            {
                chkSource.Enabled = false;
                foreach (ListItem oLi in chkSource.Items)
                    oLi.Selected = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            SaveRecords();
        }
        /// <summary>
        /// 
        /// </summary>
        private Boolean CheckOpenTrData(ref string vAllId)
        {
            Boolean vChk = false;
            string vID = "";
            Int32 vCnt = 0;
            CTransfer oTr = null;
            oTr = new CTransfer();

            foreach (ListItem oLi in chkDestn.Items)
            {
                if (oLi.Selected == true)
                {
                    vID = oLi.Value;
                    vCnt = oTr.chkOpen(vID, rblOptn.SelectedValue);
                    if (vCnt > 0)
                    {
                        return true;
                    }
                }
            }
            return vChk;
        }
        /// <summary>
        /// 
        /// </summary>
        private DataTable CreateTrData(ref string vAllId)
        {
            DataTable dt = new DataTable("Tr");
            dt.Columns.Add("SlNo");
            dt.Columns.Add("Id");
            DataRow dr;
            int i = 1;
            foreach (ListItem oLi in chkDestn.Items)
            {
                if (oLi.Selected == true)
                {
                    dr = dt.NewRow();
                    dr["SlNo"] = i;
                    dr["Id"] = oLi.Value;
                    dt.Rows.Add(dr);
                    vAllId += oLi.Value + ",";
                    i++;
                }
            }
            dt.AcceptChanges();
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidaFields()
        {
            bool vRes = true;
            if (ddlRoSrc.SelectedIndex > 0 && ddlRoDes.SelectedIndex <= 0)
            {
                gblFuction.AjxMsgPopup("Please Select Destination RO to Transfer");
                gblFuction.focus("ctl00_cph_Main_tbTrns_pnlDtl_ddlRoSrc");
                return vRes = false;
            }
            //if (ddlGrpSrc.SelectedIndex > 0 && ddlGrpDes.SelectedIndex <= 0)
            //{
            //    gblFuction.AjxMsgPopup("Please Select Destination Group to Transfer");
            //    return vRes = false;
            //}
            if (txtResn.Text.Trim() == "")
            {
                gblFuction.AjxMsgPopup("Reason Cannot be Left Blank");
                return vRes = false;
            }
            if (gblFuction.IsDate(txtTrDt.Text.Trim()) == false)
            {
                gblFuction.AjxMsgPopup("Transfer Date is not in DD/MM/YYYY Format");
                return vRes = false;
            }
            if (ddlRoSrc.SelectedValue == ddlRoDes.SelectedValue && ddlRoSrc.SelectedIndex > 0)
            {
                gblFuction.AjxMsgPopup("Transfer in Same CRO is not Allowed ...");
                return vRes = false;
            }
            //if (ddlGrpSrc.SelectedValue == ddlGrpDes.SelectedValue && ddlGrpSrc.SelectedIndex > 0)
            //{
            //    gblFuction.AjxMsgPopup("Transfer in Same Group is not Allowed ...");
            //    return vRes = false;
            //}
            if (chkDestn.Items.Count <= 0)
            {
                gblFuction.AjxMsgPopup("Nothing Found to be Transferred");
                return vRes = false;
            }
            return vRes;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveRecords()
        {
            string vBrCode = "";
            if (ValidaFields() == false) return;

            if (ddlType.SelectedValue == "W")
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
            }
            else
            {
                if (ddlType.SelectedValue == "B")
                {
                    vBrCode = ddlBr.SelectedValue;
                    if (vBrCode == "-1")
                    {
                        gblFuction.MsgPopup("No Branch Selected");
                        return;
                    }
                    if (vBrCode == Session[gblValue.BrnchCode].ToString())
                    {
                        gblFuction.MsgPopup("Same Branch can not be Selected");
                        return;
                    }
                }
            }
            string vDtlId = "", vXml = ""; ;
            //if (rblOptn.SelectedValue == "M")
            //{
            //    if (CheckOpenTrData(ref vDtlId) == true)
            //    {
            //        gblFuction.MsgPopup("Loan Open .....Cannot Transfer");
            //        return;
            //    }
            //}
            DataTable dtXml = CreateTrData(ref vDtlId);

            if (dtXml.Rows.Count == 0)
            {
                gblFuction.MsgPopup("Nothing to Save");
                return;
            }

            int vErr = 0;
            using (StringWriter oSW = new StringWriter())
            {
                dtXml.WriteXml(oSW);
                vXml = oSW.ToString();
            }

            CTransfer oTr = new CTransfer();

            string vMsg = "";
            if (rblSel.Text == "M")
            {
                vMsg = oTr.ChkCollectionOntransferDate(gblFuction.setDate(txtTrDt.Text), Session[gblValue.BrnchCode].ToString(), vXml);
                if (vMsg != "")
                {
                    gblFuction.AjxMsgPopup(vMsg);
                    return;
                }
            }

            vErr = oTr.InterBranchTrans(gblFuction.setDate(txtTrDt.Text), vBrCode,ddlRoDes.SelectedValue, Session[gblValue.BrnchCode].ToString(),
                    this.UserID, rblSel.Text, vXml, txtResn.Text.Trim().Replace("'", "''"));

            if (vErr > 0)
            {
                gblFuction.AjxMsgPopup("Transfer Successful");
                btnTransfer.Enabled = false;
            }
            else
                gblFuction.AjxMsgPopup("Transfer Failed");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
    }
}
