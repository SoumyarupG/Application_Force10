using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;
using System.Data;
using System.IO;

namespace CENTRUMSME.WebPages.Private.Admin
{
    public partial class TransferMember : CENTRUMBAse
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
                txtTrDate.Text = Session[gblValue.LoginDate].ToString();                
                PopBranch();
                PopMember();
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
        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Member Transfer";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuTransfr);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Member Transfer", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void PopMember()
        {
            chkDestn.Items.Clear();
            rblOptn.SelectedValue = "A";
            chkSource.Items.Clear();

            DataTable dt = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            try
            {
                DateTime vLogDt = gblFuction.setDate(txtTrDate.Text);
                oCG = new CGblIdGenerator();
                dt = oCG.PopMember(vBrCode,vLogDt);
                chkSource.DataSource = dt;
                chkSource.DataTextField = "CompanyName";
                chkSource.DataValueField = "CustId";
                chkSource.DataBind();
                foreach (ListItem oLi in chkSource.Items)
                    oLi.Selected = true;
                chkSource.Enabled = false;
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
            
            return vRes;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveRecords()
        {
            string vBrCode = "";
            if (ValidaFields() == false) return;

            
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
            vBrCode = ddlBr.SelectedValue;
            CTransfer oTr = new CTransfer();           
            vErr = oTr.InterBranchMemTrans(gblFuction.setDate(txtTrDate.Text),  vBrCode, Session[gblValue.BrnchCode].ToString(),
               Convert.ToInt32(Session[gblValue.UserId]), vXml, txtResn.Text.Trim().Replace("'", "''"));
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