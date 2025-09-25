using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Web.Security;

namespace CENTRUM.WebPages.Private.Admin
{
    public partial class HODayBegin : CENTRUMBase
    {
        DateTime LstBeginDt = System.DateTime.Now;
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
                txtStDt.Visible = true;
                txtStDt.Visible = true;
                txtStDt.Text = Session[gblValue.LoginDate].ToString();
                LstBeginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                PopState();
                LoadGrid();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid()
        {
            DataTable dt = null;
            Int32 vYrNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
            DateTime vFinFrmDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vAsDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CDayEnd oDE = null;
            try
            {
                oDE = new CDayEnd();
                dt = oDE.GetDayBeginDatainHO(vAsDt, Convert.ToInt32(ddlState.SelectedValue));
                ViewState["DAYBEGIN"] = dt;
                gvCB.DataSource = dt;
                gvCB.DataBind();

            }
            finally
            {
                oDE = null;
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
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Day Begin Process";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDayBeginProc);

                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnEdit.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnEdit.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Day Begin", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void PopState()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            oCG = new CGblIdGenerator();
            dt = oCG.PopComboMIS("N", "N", "AA", "StateId", "StateName", "StateMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
            ddlState.DataSource = dt;
            ddlState.DataTextField = "StateName";
            ddlState.DataValueField = "StateId";
            ddlState.DataBind();
            ListItem Lst1 = new ListItem("<--- Select --->", "-1");
            ddlState.Items.Insert(0, Lst1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            string vBranch = Session[gblValue.BrnchCode].ToString();
            if (txtStDt.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Process Date Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtStDt");
                vResult = false;
            }
            if (txtStDt.Text.Trim() != "")
            {
                if (gblFuction.IsDate(txtStDt.Text.Trim()) == false)
                {
                    gblFuction.MsgPopup("Please Enter Valid Date...");
                    gblFuction.focus("ctl00_cph_Main_txtStDt");
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Int32 vRst = 0;
            CDayEnd oDE = null;
            DateTime vEndDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vBranch = Session[gblValue.BrnchCode].ToString();
            string vMsg = "";
            DateTime vDayBegindt = gblFuction.setDate(txtStDt.Text);
            DataTable dt = null;
            string vXmlData = "", ErrMsg = "";
            btnSave.Enabled = false;
            try
            {
                if (ValidateFields() == false) return;
                oDE = new CDayEnd();
                dt = (DataTable)ViewState["DAYBEGIN"];

                foreach (GridViewRow gv in gvCB.Rows)
                {
                    CheckBox chkDayBegin = (CheckBox)gv.FindControl("chkDayBegin");
                    if (chkDayBegin.Checked == true)
                        dt.Rows[gv.RowIndex]["DAYBEGIN"] = "Y";
                    else
                        dt.Rows[gv.RowIndex]["DAYBEGIN"] = "N";
                }
                dt.AcceptChanges();
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXmlData = oSW.ToString();
                }

                vRst = oDE.DayBeginProcess(Convert.ToInt32(Session[gblValue.UserId].ToString()), vDayBegindt, vXmlData, Convert.ToInt32(Session[gblValue.FinYrNo]), gblFuction.setStrDate(Session[gblValue.FinFromDt].ToString()), Session[gblValue.ShortYear].ToString(), ref ErrMsg);
                if (vRst == 2)
                {
                    gblFuction.AjxMsgPopup(ErrMsg.ToString());
                }
                else
                {
                    if (vRst == 0)
                    {
                        gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
                        LoadGrid();
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup(gblMarg.DBError);
                    }
                }
            }
            finally
            {
                oDE = null;
            }
        }
        //protected void btnPrint_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = null;
        //    CDayEnd oDE = null;
        //    oDE = new CDayEnd();
        //    string vFileNm = "";
        //    Int32 vYrNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
        //    DateTime vFinFrmDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
        //    DateTime vAsDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //    string vBranch = Session[gblValue.BrnchCode].ToString();
        //    System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
        //    dt = oDE.GetDayEndDatainHO(vFinFrmDt, vAsDt, vYrNo);
        //    DataGrid1.DataSource = dt;
        //    DataGrid1.DataBind();
        //    tdx.Controls.Add(DataGrid1);
        //    tdx.Visible = false;

        //    vFileNm = "attachment;filename=Day_end Report.xls";
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter htw = new HtmlTextWriter(sw);
        //    htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
        //    htw.WriteLine("<tr><td align=center' colspan='6'><b><u><font size='5'>Day End Report</font></u></b></td></tr>");
        //    DataGrid1.RenderControl(htw);
        //    htw.WriteLine("</td></tr>");
        //    htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
        //    htw.WriteLine("</table>");
        //    Response.ClearContent();
        //    Response.AddHeader("content-disposition", vFileNm);
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.ContentType = "application/vnd.ms-excel";
        //    Response.Write(sw.ToString());
        //    Response.End();
        //}

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }
    }
}