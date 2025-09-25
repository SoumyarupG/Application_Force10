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
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Admin
{
    public partial class Transfer : CENTRUMBase
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
                txtFrmDt.Text = System.DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
                txtToDt.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                txtTrDt.Text = Session[gblValue.LoginDate].ToString();
                //popRO();
                popCenter();
                ddlRoSrc.Enabled = true;
                ddlRoDes.Enabled = true;
                ddlCentrSrc.Enabled = true;
                ddlCentrDes.Enabled = true;
                ddlGrpSrc.Enabled = false;
                ddlGrpDes.Enabled = false;
                LoadGrid(1);
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
                this.PageHeading = "Transfer Within Branch";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuTransfr);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Transfer Within Branch", false);
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
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0;
            string vBrCode = "";
            CTransfer oBr = null;
            try
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oBr = new CTransfer();
                dt = oBr.GetTransferPG(gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), vBrCode, pPgIndx, ref vTotRows);
                gvTrn.DataSource = dt;
                gvTrn.DataBind();
                lblTotPg.Text = CalTotPages(vTotRows).ToString();
                lblCrPg.Text = vPgNo.ToString();
                if (vPgNo == 1)
                {
                    btnPrev.Enabled = false;
                    if (Int32.Parse(lblTotPg.Text) > 1)
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                }
                else
                {
                    btnPrev.Enabled = true;
                    if (vPgNo == Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
                }
            }
            finally
            {
                oBr = null;
                dt = null;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Prev":
                    vPgNo = Int32.Parse(lblCrPg.Text) - 1;
                    break;
                case "Next":
                    vPgNo = Int32.Parse(lblCrPg.Text) + 1;
                    break;
            }
            LoadGrid(vPgNo);
            tbTrns.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }

        /// <summary>
        /// 
        /// </summary>
        private void popRO()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";             
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];                
                oGb = new CGblIdGenerator();
                dt = oGb.PopTransferMIS("Y","EoMst", "JRO,SRO,RO,BM,ABM,SBM,CO", vLogDt, vBrCode);
                ddlRoSrc.DataSource = dt;
                ddlRoSrc.DataTextField = "EoName";
                ddlRoSrc.DataValueField = "EoId";
                ddlRoSrc.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlRoSrc.Items.Insert(0, oli);

                ddlRoDes.DataSource = dt;
                ddlRoDes.DataTextField = "EoName";
                ddlRoDes.DataValueField = "EoId";
                ddlRoDes.DataBind();
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
        private void popCenter()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";           
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];                
                oGb = new CGblIdGenerator();
                dt = oGb.PopTransferMIS("Y", "MarketMst", "", vLogDt, vBrCode);
                ddlCentrSrc.DataSource = dt;
                ddlCentrSrc.DataTextField = "Market";
                ddlCentrSrc.DataValueField = "MarketID";
                ddlCentrSrc.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCentrSrc.Items.Insert(0, oli);

                ddlCentrDes.DataSource = dt;
                ddlCentrDes.DataTextField = "Market";
                ddlCentrDes.DataValueField = "MarketID";
                ddlCentrDes.DataBind();
                ddlCentrDes.Items.Insert(0, oli);
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
                dt = oGb.PopTransferMIS("Y", "GroupMst", "", vLogDt, vBrCode);
                ddlGrpSrc.DataSource = dt;
                ddlGrpSrc.DataTextField = "GroupName";
                ddlGrpSrc.DataValueField = "Groupid";
                ddlGrpSrc.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlGrpSrc.Items.Insert(0, oli);

                ddlGrpDes.DataSource = dt;
                ddlGrpDes.DataTextField = "GroupName";
                ddlGrpDes.DataValueField = "Groupid";
                ddlGrpDes.DataBind();
                ddlGrpDes.Items.Insert(0, oli);
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
        protected void rblSel_SelectedIndexChanged(object sender,EventArgs e)
        {
            rblOptn.SelectedValue = "A";
            chkSource.Items.Clear();
            chkDestn.Items.Clear();
            ddlRoSrc.SelectedIndex = -1;
            ddlCentrSrc.SelectedIndex = -1;
            ddlGrpSrc.SelectedIndex = -1;
            if (rblSel.SelectedValue == "C")
            {
                popRO();
                ddlRoSrc.Enabled = true;
                ddlRoDes.Enabled = true;
                ddlCentrSrc.Enabled = false;
                ddlCentrDes.Enabled = false;
                ddlGrpSrc.Enabled = false;
                ddlGrpDes.Enabled = false;
            }
            if (rblSel.SelectedValue == "M")
            {
                popGroup();
                ddlRoSrc.Enabled = false;
                ddlRoDes.Enabled = false;
                ddlGrpSrc.Enabled = true;
                ddlGrpDes.Enabled = true;
                ddlCentrSrc.Enabled = false;
                ddlCentrDes.Enabled = false;
            }
            if (rblSel.SelectedValue == "G")
            {
                popCenter();
                ddlRoSrc.Enabled = false;
                ddlRoDes.Enabled = false;
                ddlGrpSrc.Enabled = false;
                ddlGrpDes.Enabled = false;
                ddlCentrSrc.Enabled = true;
                ddlCentrDes.Enabled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRoSrc_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkDestn.Items.Clear();
            rblOptn.SelectedValue = "A";
            chkSource.Items.Clear();
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "", vRoId="";           
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vRoId = ddlRoSrc.SelectedValue;
                oGb = new CGblIdGenerator();
                dt = oGb.PopTransferMIS("Y", "MarketMst", vRoId, vLogDt, vBrCode);
                chkSource.DataSource = dt;
                chkSource.DataTextField = "Market";
                chkSource.DataValueField = "MarketID";
                chkSource.DataBind();
                foreach (ListItem oLi in chkSource.Items)
                    oLi.Selected = true;
                chkSource.Enabled = false;
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
        protected void ddlCentrSrc_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkDestn.Items.Clear();
            rblOptn.SelectedValue = "A";
            chkSource.Items.Clear();
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "", vCntrId="";           
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vCntrId = ddlCentrSrc.SelectedValue;
                oGb = new CGblIdGenerator();
                dt = oGb.PopTransferMIS("Y", "GroupMst", vCntrId, vLogDt, vBrCode);
                chkSource.DataSource = dt;
                chkSource.DataTextField = "GroupName";
                chkSource.DataValueField = "Groupid";
                chkSource.DataBind();
                foreach (ListItem oLi in chkSource.Items)
                    oLi.Selected = true;
                chkSource.Enabled = false;
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
        protected void ddlGrpSrc_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkDestn.Items.Clear();
            rblOptn.SelectedValue = "A";
            chkSource.Items.Clear();
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "", vGrpId="";           
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vGrpId = ddlGrpSrc.SelectedValue;
                oGb = new CGblIdGenerator();
                dt = oGb.PopTransferMIS("Y", "MemberMst", vGrpId, vLogDt, vBrCode);
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
                oGb = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string,string> PopTransfer(CheckBoxList vContrl,string vMode)
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
            Dictionary<string, string> oDict = PopTransfer(chkSource,"Add");
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
            LoadGrid(1);
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
                dr = dt.NewRow();
                dr["SlNo"] = i;
                dr["Id"] = oLi.Value;
                dt.Rows.Add(dr);
                vAllId += oLi.Value+",";
                i++;
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
            CCollectionRoutine oCR = null;
            CMember oMem = null;
            DataTable dt,dt1;
            Int32 vErr = 0,vChkSameCenter = 0;
            bool vRes = true;
            string vId = "";
            if (ddlRoSrc.SelectedIndex > 0 && ddlRoDes.SelectedIndex <= 0)
            {
                gblFuction.AjxMsgPopup("Please Select Destination RO to Transfer");
                gblFuction.focus("ctl00_cph_Main_tbTrns_pnlDtl_ddlRoSrc");
                return vRes = false;
            }
            if (ddlCentrSrc.SelectedIndex > 0 && ddlCentrDes.SelectedIndex <= 0)
            {
                gblFuction.AjxMsgPopup("Please Select Destination Center to Transfer");
                gblFuction.focus("ctl00_cph_Main_tbTrns_pnlDtl_ddlCentrDes");
                return vRes = false;
            }
            if (ddlGrpSrc.SelectedIndex > 0 && ddlGrpDes.SelectedIndex <= 0)
            {
                gblFuction.AjxMsgPopup("Please Select Destination Group to Transfer");
                return vRes = false;
            }
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
            if (ddlRoSrc.SelectedValue == ddlRoDes.SelectedValue && ddlRoSrc.SelectedIndex>0)
            {
                gblFuction.AjxMsgPopup("Transfer in Same RO is not Allowed ...");
                return vRes = false;
            }
            if (ddlCentrSrc.SelectedValue == ddlCentrDes.SelectedValue && ddlCentrSrc.SelectedIndex>0)
            {
                gblFuction.AjxMsgPopup("Transfer in Same Center is not Allowed ...");
                return vRes = false;
            }
            if (ddlGrpSrc.SelectedValue == ddlGrpDes.SelectedValue && ddlGrpSrc.SelectedIndex>0)
            {
                gblFuction.AjxMsgPopup("Transfer in Same Group is not Allowed ...");
                return vRes = false;
            }
            if (chkDestn.Items.Count <= 0)
            {
                gblFuction.AjxMsgPopup("Nothing Found to be Transfered");
                return vRes = false;
            }
            oCR = new CCollectionRoutine();
            oMem = new CMember();
            if (rblSel.SelectedValue == "C")
            {
                foreach (ListItem oLi in chkDestn.Items)
                {
                    dt1 = oMem.ChkCollRoutineByMarketId(oLi.Value, Session[gblValue.BrnchCode].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        dt = oCR.GetCollRoutineDtls(Convert.ToInt32(dt1.Rows[0]["RoutineID"].ToString()));
                        if (dt.Rows.Count > 0)
                        {
                            vErr = oCR.ChkSameTimeByCo(ddlRoDes.SelectedValue, Session[gblValue.BrnchCode].ToString(), dt.Rows[0]["Colltime"].ToString(), dt.Rows[0]["Allow"].ToString(), Convert.ToInt32(dt.Rows[0]["CollDay"].ToString()), Convert.ToInt32(dt1.Rows[0]["RoutineID"].ToString()));
                            if (vErr > 0)
                            {
                                gblFuction.MsgPopup(oLi.Text + " Center has same collection time of another center.");
                                return vRes = false;
                            }
                        }
                    }
                }
            }

            CTransfer oTr = new CTransfer();
            if (rblSel.SelectedValue != "C")
            {
                foreach (ListItem oLi in chkDestn.Items)
                {
                    dt1 = oTr.ChkApplicationPendingById(oLi.Value, rblSel.SelectedValue);
                    if (dt1.Rows.Count > 0)
                    {

                        if (rblSel.SelectedValue == "G")
                        {
                            gblFuction.MsgPopup(oLi.Text + " Group has pending loan application .");
                            return vRes = false;
                        }

                        if (rblSel.SelectedValue == "M")
                        {
                            vChkSameCenter = oTr.ChkSameCenter(gblFuction.setDate(txtTrDt.Text), ddlGrpSrc.SelectedValue, ddlGrpDes.SelectedValue, Session[gblValue.BrnchCode].ToString(), oLi.Value);
                            if (vChkSameCenter == 1)
                            {
                                gblFuction.MsgPopup(oLi.Text + " Member has pending loan application .");
                                return vRes = false;
                            }
                        }
                    }   
                }
            }

            if (rblSel.SelectedValue != "C")
            {
                foreach (ListItem oLi in chkDestn.Items)
                {
                    dt1 = oTr.ChkOpenLoanById(oLi.Value, rblSel.SelectedValue, gblFuction.setDate(txtTrDt.Text),"","");
                    if (dt1.Rows.Count > 0)
                    {

                        if (rblSel.SelectedValue == "G")
                        {
                            gblFuction.MsgPopup(oLi.Text + " Group has Open loan.");
                            return vRes = false;
                        }
                        if (rblSel.SelectedValue == "M")
                        {
                            gblFuction.MsgPopup(oLi.Text + " Member has Open loan.");
                            return vRes = false;
                        }

                        //if (rblSel.SelectedValue == "M")
                        //{
                        //    vChkSameCenter = oTr.ChkSameCenter(gblFuction.setDate(txtTrDt.Text), ddlGrpSrc.SelectedValue, ddlGrpDes.SelectedValue, Session[gblValue.BrnchCode].ToString());
                        //    if (vChkSameCenter == 1)
                        //    {
                        //        gblFuction.MsgPopup(oLi.Text + " Source and Destination Center is Same. You can not transfer");
                        //        return vRes = false;
                        //    }
                        //}
                    }
                }
            }

            if (rblSel.SelectedValue == "M")
            {
                Int32 vCountMem = 0;
                Int32 vMemExist=0;
                int vOpenLoan = 0;
                foreach (ListItem oLi in chkDestn.Items)
                {
                    vCountMem = vCountMem + 1;
                }
                dt1 = oTr.ChkMemberByGroupId(ddlGrpDes.SelectedValue, Session[gblValue.BrnchCode].ToString());
                vMemExist = Convert.ToInt32(dt1.Rows[0]["MemCount"].ToString());
                if ((vCountMem + vMemExist) > 12)
                {
                    gblFuction.AjxMsgPopup("You can not transfer because More than 12 member in one Group is not allowed..");
                    return vRes = false;
                }
                oTr=new CTransfer();
                vOpenLoan = oTr.ChkGroupOpenLoan(ddlGrpDes.SelectedValue, gblFuction.setDate(txtTrDt.Text));
                if(vOpenLoan > 0)
                {
                    gblFuction.AjxMsgPopup("You can not transfer because this Group has open loan..");
                    return vRes = false;
                }
            }
            return vRes;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveRecords()
        {
            if (ValidaFields() == false) return;

            string vBrCode=Session[gblValue.BrnchCode].ToString(),vDtlId="",vXml = "";;
            DataTable dtXml = CreateTrData(ref vDtlId);
            int vErr = 0;             
            using (StringWriter oSW = new StringWriter())
            {
                dtXml.WriteXml(oSW);
                vXml = oSW.ToString();
            }

            CTransfer oTr = new CTransfer();
            if (rblSel.SelectedValue=="C")
            vErr = oTr.TransferCenter(gblFuction.setDate(txtTrDt.Text), ddlRoDes.SelectedValue, vBrCode, 
                this.UserID, vXml, rblSel.SelectedValue, ddlRoSrc.SelectedValue, txtResn.Text.Trim().Replace("'", "''"), vDtlId);

            if (rblSel.SelectedValue == "M")
                vErr = oTr.TransferCenter(gblFuction.setDate(txtTrDt.Text), ddlGrpDes.SelectedValue, vBrCode, 
                    this.UserID, vXml, rblSel.SelectedValue, ddlGrpSrc.SelectedValue, txtResn.Text.Trim().Replace("'", "''"), vDtlId);

            if (rblSel.SelectedValue == "G")
                vErr = oTr.TransferCenter(gblFuction.setDate(txtTrDt.Text), ddlCentrDes.SelectedValue, vBrCode, 
                    this.UserID, vXml, rblSel.SelectedValue, ddlCentrSrc.SelectedValue, txtResn.Text.Trim().Replace("'", "''"), vDtlId);
            
            if (vErr > 0)
                gblFuction.AjxMsgPopup("Transfer Successful");
            else
                gblFuction.AjxMsgPopup("Transfer Failed");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender,EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
    }
}