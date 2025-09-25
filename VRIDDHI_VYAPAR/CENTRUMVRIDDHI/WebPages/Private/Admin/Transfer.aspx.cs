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
using CENTRUMCA;
using CENTRUMBA;
using CENTRUMDA;

namespace CENTRUM_VRIDDHIVYAPAR.WebPages.Private.Admin
{
    public partial class Transfer : CENTRUMBAse
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
                popRO();
                ddlRoSrc.Enabled = true;
                ddlRoDes.Enabled = true;
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
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuSARALTransfr);
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
                dt = oGb.PopTransferMIS("Y", "EoMst", "CRE,CRM,ABM,BM,ALCE,BCLO,CO ,RO  ,UM  ", vLogDt, vBrCode);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //rblOptn.SelectedValue = "A";
            //chkSource.Items.Clear();
            //chkDestn.Items.Clear();
            //ddlRoSrc.SelectedIndex = -1;
            //if (rblSel.SelectedValue == "C")
            //{
            //    popRO();
            //    ddlRoSrc.Enabled = true;
            //    ddlRoDes.Enabled = true;
            //}
            LoadCheckList();
        }

        protected void ddlRoSrc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCheckList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ddlSrc_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoadCheckList();
        //}

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
                if (rblSel.SelectedValue == "C")
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
                vAllId += oLi.Value + ",";
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
            DataTable dt, dt1;
            Int32 vErr = 0, vChkSameCenter = 0;
            bool vRes = true;
            if (ddlRoSrc.SelectedIndex > 0 && ddlRoDes.SelectedIndex <= 0)
            {
                gblFuction.AjxMsgPopup("Please Select Destination LO to Transfer");
                gblFuction.focus("ctl00_cph_Main_tbTrns_pnlDtl_ddlRoSrc");
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
            if (ddlRoSrc.SelectedValue == ddlRoDes.SelectedValue && ddlRoSrc.SelectedIndex > 0)
            {
                gblFuction.AjxMsgPopup("Transfer in Same LO is not Allowed ...");
                return vRes = false;
            }
            if (chkDestn.Items.Count <= 0)
            {
                gblFuction.AjxMsgPopup("Nothing Found to be Transfered");
                return vRes = false;
            }
            return vRes;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveRecords()
        {
            if (ValidaFields() == false) return;

            string vBrCode = Session[gblValue.BrnchCode].ToString(), vDtlId = "", vXml = ""; ;
            DataTable dtXml = CreateTrData(ref vDtlId);
            int vErr = 0;
            using (StringWriter oSW = new StringWriter())
            {
                dtXml.WriteXml(oSW);
                vXml = oSW.ToString();
            }

            CTransfer oTr = new CTransfer();
            //if (rblSel.SelectedValue == "C")
            vErr = oTr.TransferMember(gblFuction.setDate(txtTrDt.Text), ddlRoDes.SelectedValue, vBrCode,
                    this.UserID, vXml, rblSel.SelectedValue, ddlRoSrc.SelectedValue, txtResn.Text.Trim().Replace("'", "''"), vDtlId);

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
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
    }
}