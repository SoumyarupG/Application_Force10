using System;
using System.Collections;
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
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class ChqBounce : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                txtFrmDt.Text = Session[gblValue.FinFromDt].ToString();
                txtToDt.Text = Session[gblValue.FinToDt].ToString();
                txtChqDt.Text = Session[gblValue.LoginDate].ToString();
                PopCM();
                //LoadGrid();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.PageHeading = "Cheque Bounce";
                this.ShowBranchName = "Branch :: " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = "Financial Year :: " + Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuCollChqBounc);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnSave.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Cheque Bounce", false);
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
        private void PopCM()
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            CCM oCM = new CCM();
            dt = oCM.GetCOPop(vBrCode, "SCO,CO,TCO,JTCO,GO");
            ddlCo.DataTextField = "EOName";
            ddlCo.DataValueField = "EOId";
            ddlCo.DataSource = dt;
            ddlCo.DataBind();
            ListItem oItm = new ListItem();
            oItm.Text = "<--- Select --->";
            oItm.Value = "-1";
            ddlCo.Items.Insert(0, oItm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateDate()
        {
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            Boolean vResult = true;
            if (txtFrmDt.Text.Trim() == "")
            {
                gblFuction.MsgPopup("From Date Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtFromDt");
                vResult = false;
                return vResult;
            }
            if (txtFrmDt.Text.Trim() != "")
            {
                if (gblFuction.IsDate(txtFrmDt.Text) == false)
                {
                    gblFuction.MsgPopup("Please Enter Valid Date...");
                    gblFuction.focus("ctl00_cph_Main_txtFrDt");
                    vResult = false;
                    return vResult;
                }
            }
            if (txtToDt.Text.Trim() == "")
            {
                gblFuction.MsgPopup("To Date Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtToDt");
                vResult = false;
                return vResult;
            }
            if (txtToDt.Text.Trim() != "")
            {
                if (gblFuction.IsDate(txtToDt.Text) == false)
                {
                    gblFuction.MsgPopup("Please Enter Valid Date...");
                    gblFuction.focus("ctl00_cph_Main_txtToDt");
                    vResult = false;
                    return vResult;
                }
            }
            if (gblFuction.setDate(txtFrmDt.Text) > gblFuction.setDate(txtToDt.Text))
            {
                gblFuction.MsgPopup("From date should less than to date...");
                gblFuction.focus("ctl00_cph_Main_txtToDt");
                vResult = false;
            }
            if (gblFuction.setDate(txtFrmDt.Text) < vFinFromDt || gblFuction.setDate(txtFrmDt.Text) > vFinToDt)
            {
                gblFuction.AjxMsgPopup("The From date should login Financial year...");
                return false;
            }
            if (gblFuction.setDate(txtToDt.Text) < vFinFromDt || gblFuction.setDate(txtToDt.Text) > vFinToDt)
            {
                gblFuction.AjxMsgPopup("The To date should login Financial year...");
                return false;
            }
            if (txtChqDt.Text.Trim() == "")
            {
                gblFuction.MsgPopup("To Date Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtAsOn");
                vResult = false;
            }
            if (txtChqDt.Text.Trim() != "")
            {
                if (gblFuction.IsDate(txtChqDt.Text) == false)
                {
                    gblFuction.MsgPopup("Please Enter Valid Date...");
                    gblFuction.focus("ctl00_cph_Main_txtAsOn");
                    vResult = false;
                }
            }
            if (gblFuction.setDate(txtChqDt.Text) < vFinFromDt || gblFuction.setDate(txtChqDt.Text) > vFinToDt)
            {
                gblFuction.AjxMsgPopup("The as on date should login Financial year...");
                return false;
            }
            return vResult;
        }
        /// <summary>
        /// 
        /// </summary>
        //private void LoadGrid()
        //{
        //    CBounceCheque oChq = new CBounceCheque();
        //    DataTable dt = null;
        //    string vMemId = "";
        //    string vBrCode = Session[gblValue.BrnchCode].ToString();
        //    vMemId = "-1";
        //    dt = oChq.GetChequeBounceByMember(vMemId, gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), vBrCode);
        //    gvChq.DataSource = dt;
        //    gvChq.DataBind();
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string DataTableTOXml(DataTable dt)
        {
            string sXml = "";
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                sXml = oSW.ToString();
            }
            return sXml;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            DateTime vEntDt = gblFuction.setDate(txtChqDt.Text);
            string vFinSYear = Session[gblValue.ShortYear].ToString();
            string vXmlCB = string.Empty;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vAcHdTbl = Session[gblValue.ACVouMst].ToString();
            string vAcDtlTbl = Session[gblValue.ACVouDtl].ToString();
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            Boolean vResult = false;
            CBounceCheque oCB = null;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            DataTable dt = (DataTable)ViewState["ChequeBounce"];
            Int32 vErr = 0;
            try
            {
                if (Mode == "Save" || Mode == "Edit")
                {
                    if (ValidateDate() == false)
                        return false;

                    if (gvChq.Rows.Count == 0)
                    {
                        gblFuction.MsgPopup("No Data to Save..");
                        return true;
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        if (Convert.ToString(row["CheckBounce"]) == "N")
                            row.Delete();
                    }
                    dt.AcceptChanges();
                    vXmlCB = DataTableTOXml(dt);
                    oCB = new CBounceCheque();
                    vErr = oCB.UpdateChqBounce(vXmlCB, vBrCode, this.UserID, vAcHdTbl, vAcDtlTbl, "E", vEntDt, vFinSYear);
                    if (vErr == 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                return vResult;
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
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx");
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                if (vStateEdit == "" || vStateEdit == null)
                    vStateEdit = "Save";
                if (SaveRecords(vStateEdit) == true)
                {
                    gblFuction.MsgPopup(gblMarg.SaveMsg);
                    //LoadGrid();
                }
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
        protected void ddlCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMember.Items.Clear();
            ddlMember.Items.Clear();
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            try
            {
                CMember oMem = new CMember();
                if (Convert.ToString(ddlCo.SelectedValue) != "-1")
                {
                    dt = oMem.PopIndMember(Convert.ToString(ddlCo.SelectedValue), Session[gblValue.BrnchCode].ToString(), vLoginDt,"L");
                    ddlMember.DataTextField = "MemberName";
                    ddlMember.DataValueField = "MemberId";
                    ddlMember.DataSource = dt;
                    ddlMember.DataBind();
                    ListItem oItm = new ListItem();
                    oItm.Text = "<--- Select --->";
                    oItm.Value = "-1";
                    ddlMember.Items.Insert(0, oItm);
                }
                ddlMember.Focus();
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
        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (ValidateDate() == true)
            {
                CBounceCheque oChq = new CBounceCheque();
                DataTable dt = null;
                string vMemId = "";
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                if (ddlCo.SelectedIndex <= 0)
                    vMemId = "-1";
                else
                    vMemId = ddlMember.SelectedValue;
                dt = oChq.GetChequeBounceByMember(vMemId, gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), vBrCode);
                gvChq.DataSource = dt;
                gvChq.DataBind();
                ViewState["ChequeBounce"] = dt;
                dt.PrimaryKey = new[] { dt.Columns["RowID"]};
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkMemOnly_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CMember oMem = new CMember();
            ddlMember.Items.Clear();
            if (chkMemOnly.Checked == true)
            {
                ddlCo.Enabled = false;
                dt = oMem.PopIndMember(Convert.ToString(ddlCo.SelectedValue), vBrCode, vLoginDt, "LA");
                ddlMember.DataTextField = "MemberName";
                ddlMember.DataValueField = "MemberId";
                ddlMember.DataSource = dt;
                ddlMember.DataBind();
                ListItem oItm = new ListItem();
                oItm.Text = "<--- Select --->";
                oItm.Value = "-1";
                ddlMember.Items.Insert(0, oItm);
            }
            else
            {
                ddlMember.Items.Clear();
                ddlCo.Enabled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkBoun_CheckedChanged(object sender, EventArgs e)
        {
            Int32 vRow = 0;
            DataTable dt = (DataTable)ViewState["ChequeBounce"];

            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtBounceDt = (TextBox)gvRow.FindControl("txtBounceDt");
            if (txtBounceDt.Text != "")
            {
                if (gblFuction.setDate(gvRow.Cells[22].Text) > gblFuction.setDate(txtBounceDt.Text))
                {
                    gblFuction.MsgPopup("Cheeque bounce date can not before collection date...");
                    txtBounceDt.Text = "";
                    return;
                }
                else
                {
                    vRow = dt.Rows.IndexOf(dt.Rows.Find(Convert.ToInt32(gvRow.Cells[0].Text)));
                    dt.Rows[vRow]["CheckBounce"] = "Y";
                    dt.Rows[vRow]["CheckDate"] = txtBounceDt.Text;
                }
            }
            dt.AcceptChanges();
        }
    }
}