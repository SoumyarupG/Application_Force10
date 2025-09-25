using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUM_VRIDDHIVYAPAR.WebPages.Private.Admin
{
    public partial class InterBrTransferCenter : CENTRUMBAse
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
                dt = oCG.PopTransferMIS("Y", "BranchMst", "", vLogDt, vBrCode);
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

        private void popCOSrc()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode =  (Session[gblValue.BrnchCode].ToString());

                oGb = new CGblIdGenerator();
                dt = oGb.PopTransferMIS("Y", "EoMst", "", vLogDt, vBrCode);
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlRoSrc.DataSource = dt;
                ddlRoSrc.DataTextField = "EoName";
                ddlRoSrc.DataValueField = "EoId";
                ddlRoSrc.DataBind();
                ddlRoSrc.Items.Insert(0, oli);
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
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Inter Branch Transfer";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuInterBrTransfr);
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
        private void popCODest()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = ddlBr.SelectedValue;
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
            //DataTable dt = null;
            //CGblIdGenerator oGb = null;
            //string vBrCode = "";
            //DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            //try
            //{
            //    vBrCode = (string)Session[gblValue.BrnchCode];
            //    oGb = new CGblIdGenerator();
            //    dt = oGb.PopTransferMIS("Y", "GroupMst", "", vLogDt, vBrCode);
            //    //foreach (DataRow row in dt.Rows)
            //    //{
            //    //    if (Convert.ToString(row["GroupName"]).Substring(0, 10) == "Individual")
            //    //        row.Delete();
            //    //}
            //    ddlGrpSrc.DataSource = dt;
            //    ddlGrpSrc.DataTextField = "GroupName";
            //    ddlGrpSrc.DataValueField = "Groupid";
            //    ddlGrpSrc.DataBind();
            //    ListItem oli = new ListItem("<--Select-->", "-1");
            //    ddlGrpSrc.Items.Insert(0, oli);
            //}
            //finally
            //{
            //    oGb = null;
            //    dt = null;
            //}
        }

        private void popGroupDest()
        {
            //DataTable dt = null;
            //CGblIdGenerator oGb = null;
            //string vBrCode = "";
            //DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            //try
            //{
            //    if (ddlType.SelectedValue == "W")
            //    {
            //        vBrCode = Session[gblValue.BrnchCode].ToString();
            //    }
            //    else
            //    {
            //        if (ddlType.SelectedValue == "B")
            //        {
            //            vBrCode = ddlBr.SelectedValue;
            //        }
            //    }

            //    oGb = new CGblIdGenerator();
            //    dt = oGb.PopTransferMIS("Y", "GroupMst", "", vLogDt, vBrCode);
            //    ListItem oli = new ListItem("<--Select-->", "-1");
            //    ddlGrpDes.DataSource = dt;
            //    ddlGrpDes.DataTextField = "GroupName";
            //    ddlGrpDes.DataValueField = "Groupid";
            //    ddlGrpDes.DataBind();
            //    ddlGrpDes.Items.Insert(0, oli);
            //}
            //finally
            //{
            //    oGb = null;
            //    dt = null;
            //}
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
            popCODest();
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
                    dt = oTr.PopMember(vRoId, vLogDt, vBrCode);
                }
                else
                {
                    dt = oTr.PopInitialApproach(vRoId, vLogDt, vBrCode);
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


        //protected void ddlGrpSrc_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    chkDestn.Items.Clear();
        //    rblOptn.SelectedValue = "A";
        //    chkSource.Items.Clear();
        //    DataTable dt = null;
        //    CGblIdGenerator oGb = null;
        //    string vBrCode = "";
        //    string vGroupId = "";
        //    DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

        //    try
        //    {
        //        vBrCode = (string)Session[gblValue.BrnchCode];
        //        vGroupId = ddlGrpSrc.SelectedValue;
        //        oGb = new CGblIdGenerator();
        //        dt = oGb.PopTransferMIS("Y", "MemberMst", vGroupId, vLogDt, vBrCode);
        //        chkSource.DataSource = dt;
        //        chkSource.DataTextField = "Member";
        //        chkSource.DataValueField = "Memberid";
        //        chkSource.DataBind();
        //        foreach (ListItem oLi in chkSource.Items)
        //            oLi.Selected = true;
        //        chkSource.Enabled = false;
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
                  //  vCnt = oTr.chkOpen(vID, rblOptn.SelectedValue);
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
                gblFuction.AjxMsgPopup("Please Select Destination LO to Transfer");
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
                gblFuction.AjxMsgPopup("Transfer in Same LO is not Allowed ...");
                return vRes = false;
            }
            //if (ddlGrpSrc.SelectedValue == ddlGrpDes.SelectedValue && ddlGrpSrc.SelectedIndex > 0)
            //{
            //    gblFuction.AjxMsgPopup("Transfer in Same Group is not Allowed ...");
            //    return vRes = false;
            //}
            if (chkDestn.Items.Count <= 0)
            {
                gblFuction.AjxMsgPopup("Nothing Found to be Transfered");
                return vRes = false;
            }

            //CTransfer oTr = new CTransfer();
            //DataTable dt1;
            //if (rblSel.SelectedValue == "M")
            //{
            //    Int32 vCountMem = 0;
            //    Int32 vMemExist = 0;
            //    foreach (ListItem oLi in chkDestn.Items)
            //    {
            //        vCountMem = vCountMem + 1;
            //    }
            //    dt1 = oTr.ChkMemberByGroupId(ddlGrpDes.SelectedValue, ddlBr.SelectedValue);
            //    vMemExist = Convert.ToInt32(dt1.Rows[0]["MemCount"].ToString());
            //    if ((vCountMem + vMemExist) > 5)
            //    {
            //        gblFuction.AjxMsgPopup("You can not transfer because More than 5 member in one Group is not allowed..");
            //        return vRes = false;
            //    }
            //}
            return vRes;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveRecords()
        {
            string vBrCode = "",vDestBrCode="";
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

            vDestBrCode = ddlBr.SelectedValue;
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

            vErr = oTr.InterBrTransferMember(gblFuction.setDate(txtTrDt.Text), ddlRoDes.SelectedValue, vBrCode,vDestBrCode,
                    Convert.ToInt32(Session[gblValue.UserId]), vXml, rblSel.SelectedValue, ddlRoSrc.SelectedValue, 
                    txtResn.Text.Trim().Replace("'", "''"), vDtlId);

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
