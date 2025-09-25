using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Web.Security;
using Newtonsoft.Json;
using System.Configuration;

namespace CENTRUM.WebPages.Private.Admin
{
    public partial class HODayEnd : CENTRUMBase
    {

        DateTime LstEndDt = System.DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //InitBasePage();
            DataTable dt = null;
            CDayEnd oDay = null;
            if (!IsPostBack)
            {
                try
                {
                    InitBasePage();
                    txtEndDt.Visible = true;
                    txtEndDt.Visible = true;
                    txtEndDt.Text = Session[gblValue.LoginDate].ToString();
                    LstEndDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                    PopState();
                    LoadGrid();
                }
                finally
                {
                    dt = null;
                    oDay = null;
                }
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Day End Process";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDayendProc);

                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Day End Process", false);
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
                dt = oDE.GetDayEndDatainHO(vAsDt, Convert.ToInt32(ddlState.SelectedValue));
                ViewState["DAYEND"] = dt;
                gvCB.DataSource = dt;
                gvCB.DataBind();
            }
            finally
            {
                oDE = null;
                dt = null;
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
            if (txtEndDt.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Process Date Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtEndDt");
                vResult = false;
            }
            if (txtEndDt.Text.Trim() != "")
            {
                if (gblFuction.IsDate(txtEndDt.Text.Trim()) == false)
                {
                    gblFuction.MsgPopup("Please Enter Valid Date...");
                    gblFuction.focus("ctl00_cph_Main_txtEndDt");
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
            Int32 vRst = 0, vRst1 = 0;
            CDayEnd oDE = null;
            DateTime vEndDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vBranch = Session[gblValue.BrnchCode].ToString();
            string vMsg = "";
            DateTime vDayEnddt = gblFuction.setDate(txtEndDt.Text);
            DataTable dt = null;
            string vXmlData = "", ErrMsg = "", ErrMsg1 = "";
            btnSave.Enabled = false;
            try
            {
                if (ValidateFields() == false) return;
                oDE = new CDayEnd();

                dt = (DataTable)ViewState["DAYEND"];
                foreach (GridViewRow gv in gvCB.Rows)
                {
                    CheckBox chkDayEnd = (CheckBox)gv.FindControl("chkDayEnd");
                    if (chkDayEnd.Checked == true)
                        dt.Rows[gv.RowIndex]["DAYEND"] = "Y";
                    else
                        dt.Rows[gv.RowIndex]["DAYEND"] = "N";


                }
                dt.AcceptChanges();
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXmlData = oSW.ToString();
                }
                //-----------------------------------Cash Reconcilation Check-----------------------
                vRst1 = oDE.ValidationforDayEnd(vXmlData, ref ErrMsg1);
                if (vRst1 > 0)
                {
                    gblFuction.AjxMsgPopup(ErrMsg1.ToString());
                    return;
                }
                //-----------------------------------------------------------------------------------

                var req = new HoDayEnd()
                {
                    pUserId = Session[gblValue.UserId].ToString(),
                    pDayEnddt = txtEndDt.Text,
                    pXmlData = vXmlData,
                    YearNo = Session[gblValue.FinYrNo].ToString(),
                    FinFrom = Session[gblValue.FinFromDt].ToString(),
                    pFinYear = Session[gblValue.ShortYear].ToString()
                }; 
                string Requestdata = JsonConvert.SerializeObject(req);
                string vMobService = ConfigurationManager.AppSettings["MobService"];
                GenerateReport("DayEndProcess", Requestdata, vMobService);

                //---------------------------------------------------------------------------------------
                //vRst = oDE.DayEndProcess(Convert.ToInt32(Session[gblValue.UserId].ToString()), vDayEnddt, vXmlData, Convert.ToInt32(Session[gblValue.FinYrNo]), gblFuction.setStrDate(Session[gblValue.FinFromDt].ToString()), Session[gblValue.ShortYear].ToString(), ref ErrMsg);
                //if (vRst == 2)
                //{
                //    gblFuction.AjxMsgPopup(ErrMsg.ToString());
                //}
                //else
                //{
                //    if (vRst == 0)
                //    {
                //        if (ErrMsg.Length > 0)
                //        {
                //            gblFuction.AjxMsgPopup(ErrMsg.ToString());
                //        }
                //        else
                //        {
                //            gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
                //        }
                //        LoadGrid();
                //    }
                //    else
                //    {
                //        gblFuction.AjxMsgPopup(gblMarg.DBError);
                //    }
                //}
                //------------------------------------------------------------------------------------------

            }
            finally
            {
                oDE = null;
            }
        }

        private void GenerateReport(string pApiName, string pRequestdata, string pReportUrl)
        {
            string vMsg = "";
            CApiCalling oAPI = new CApiCalling();
            try
            {
                vMsg = oAPI.GenerateReport(pApiName, pRequestdata, pReportUrl);
            }
            finally
            {
                gblFuction.AjxMsgPopup("Success... Day End Process is running at Back Ground ...Please wait for 15/20 mins.");
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
    public class HoDayEnd
    {
        public string pUserId { get; set; }
        public string pDayEnddt { get; set; }
        public string pXmlData { get; set; }
        public string YearNo { get; set; }
        public string FinFrom { get; set; }
        public string pFinYear { get; set; }
    }
}