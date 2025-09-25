using System;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using FORCEBA;
using FORCECA;
using CrystalDecisions.Shared;
using System.Web;
using System.Configuration;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HOAccLedgerDtl : CENTRUMBase
    {
        private static string vSrvName = ConfigurationManager.AppSettings["SrvName"];
        private static string vDBName = ConfigurationManager.AppSettings["DBName"];
        private static string vPw = ConfigurationManager.AppSettings["PassPW"];
        private static string vReportUrl = ConfigurationManager.AppSettings["ReportUrl"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch(Session[gblValue.UserName].ToString());
                CheckAllBr();
                PopList();
                CheckAll();
                popDetail();
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
                this.PageHeading = "Accounts Ledger Details";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.nmuLedgDtlsRpt);
                if (this.RoleId == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Bank Book", false);
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
        private void PopList()
        {
            DataTable dt = null;
            CAcGenled oGen = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oGen = new CAcGenled();
            dt = oGen.GetLedgerList();
            chkDtl.DataSource = dt;
            chkDtl.DataTextField = "Desc";
            chkDtl.DataValueField = "DescID";
            chkDtl.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckAll()
        {
            Int32 vRow;
            if (rblAlSel.SelectedValue == "rbAll")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = true;
                chkDtl.Enabled = false;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = false;
                chkDtl.Enabled = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAllBr();
        }
        /// <summary>
        /// 
        /// </summary>
        private void CheckAllBr()
        {
            Int32 vRow;
            string strin = "";
            if (ddlSel.SelectedValue == "C")
            {
                chkBr.Enabled = false;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                {
                    chkBr.Items[vRow].Selected = true;
                    if (strin == "")
                        strin = chkBr.Items[vRow].Value;
                    else
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                }
                ViewState["ID"] = strin;
            }
            else if (ddlSel.SelectedValue == "B")
            {
                ViewState["ID"] = null;
                chkBr.Enabled = true;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                    chkBr.Items[vRow].Selected = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void popDetail()
        {
            ViewState["Dtl"] = null;
            string str = "";
            for (int vRow = 0; vRow < chkDtl.Items.Count; vRow++)
            {
                if (chkDtl.Items[vRow].Selected == true)
                {
                    if (str == "")
                        str = chkDtl.Items[vRow].Value;
                    else if (str != "")
                        str = str + "," + chkDtl.Items[vRow].Value;
                }
            }
            if (str == "")
                ViewState["Dtl"] = 0;
            else
                ViewState["Dtl"] = str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidateDate()
        {
            bool vRst = true;
            if (gblFuction.CheckDtRange(txtDtFrm.Text, txtToDt.Text) == false)
            {
                vRst = false;
            }
            return vRst;
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
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            if (ValidateDate() == false)
            {
                gblFuction.MsgPopup("Please Set The Valid Date Range.");
                return;
            }
            else
            {
                GetData("PDF");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            if (ValidateDate() == false)
            {
                gblFuction.MsgPopup("Please Set The Valid Date Range.");
                return;
            }
            else
            {
                //GetData("Excel");
                //SetParameterForRptData("Excel");
                Int32 vRptHOAccLedgerDtlReport = Convert.ToInt32(Session[gblValue.RptHOAccLedgerDtlReport].ToString());
                if (vRptHOAccLedgerDtlReport != 0)
                {
                    Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    if (unixTicks - vRptHOAccLedgerDtlReport > 300)
                    {
                        Session[gblValue.RptHOAccLedgerDtlReport] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                        GenerateRptData("Excel");
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Already Report Generate Request Is Executing ...Please Wait For 5 Mins..And Re Generate..");
                    }
                }
                else
                {
                    Session[gblValue.RptHOAccLedgerDtlReport] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    GenerateRptData("E");

                }

            }
        }


       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
            popDetail();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //popDetail();
        }
        /// <summary>
        /// 
        /// </summary>
        private void GetBranch()
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
            {
                if (chkBr.Items[vRow].Selected == true)
                {
                    if (strin == "")
                        strin = chkBr.Items[vRow].Value;
                    else
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                }
            }
            ViewState["ID"] = strin;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFormat"></param>
        /// 




        private void SetParameterForRptData(string pMode)
        {
            GetBranch();
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = ViewState["ID"].ToString(); //Session[gblValue.BrnchCode].ToString();
            string vAcMst = Session[gblValue.ACVouMst].ToString();
            string vAcDtl = Session[gblValue.ACVouDtl].ToString();
            Int32 vFinYrNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            popDetail();
            string vDescID = ViewState["Dtl"].ToString();
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();

            TimeSpan t = gblFuction.setDate(txtToDt.Text) - gblFuction.setDate(txtDtFrm.Text);
            if (t.TotalDays > 2)
            {
                gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                return;
            }

            dt = oRpt.rptAcLedDetail_ExportExcel(gblFuction.setDate(txtDtFrm.Text), gblFuction.setDate(txtToDt.Text),
                                   Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(),
                                   vDescID, vBrCode, vFinYrNo, vFinFrom);

            dt.Columns.Remove("VoucherDt");
            dt.Columns.Remove("ChequeNo");
            dt.Columns.Remove("DescId");
            dt.AcceptChanges();
           
            if (pMode == "Excel")
            {
                string vFileNm = "attachment;filename=Acc_Ledger_Details_Consolidate_Report.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", vFileNm);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                Response.Write("<table border='1' cellpadding='5' widht='120%'>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>A/C Ledger Details (Consolidate) Report</font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>For the Period from " + txtDtFrm.Text + " to " + txtToDt.Text + "</font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'></font></b></td></tr>");
                string tab = string.Empty;
                Response.Write("<tr>");
                foreach (DataColumn dtcol in dt.Columns)
                {
                    Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
                }
                Response.Write("</tr>");
                foreach (DataRow dtrow in dt.Rows)
                {
                    Response.Write("<tr style='height:20px;'>");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        //if (dt.Columns[j].ColumnName == "GroupNo" || dt.Columns[j].ColumnName == "MemberNo" || dt.Columns[j].ColumnName == "LO Code")
                        //{
                        //    Response.Write("<td &nbsp nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                        //}
                        //else
                        //{
                        Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
                        //}
                    }
                    Response.Write("</tr>");
                }
                Response.Write("</table>");
                Response.End();
            }
        }

        private void GenerateRptData(string pMode)
        {
            GetBranch();
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = ViewState["ID"].ToString(); //Session[gblValue.BrnchCode].ToString();
            string vAcMst = Session[gblValue.ACVouMst].ToString();
            string vAcDtl = Session[gblValue.ACVouDtl].ToString();
            Int32 vFinYrNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            popDetail();
            string vDescID = ViewState["Dtl"].ToString();

            var req = new HOAccLedgerDtlRequest()
            {
                pFromDt      = txtDtFrm.Text,
                pToDt       = txtToDt.Text,
                pMst   = Session[gblValue.ACVouMst].ToString(),
                pDtl   = Session[gblValue.ACVouDtl].ToString(),
                pDescID     = vDescID,
                pBranch     = vBrCode,
                pYrNo = Convert.ToString(Session[gblValue.FinYrNo]),
                pFinFromDt = Session[gblValue.FinFromDt].ToString(),
                pFormat = pMode,
                pUserId = Convert.ToString(Session[gblValue.UserId]),
                pDBName = vDBName,
                pPassword = vPw,
                pServerIP = vSrvName,
                pCompanyName = gblValue.CompName,
                pCompanyAddress = CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString())
            };
            string Requestdata = JsonConvert.SerializeObject(req);
            CApiCalling API = new CApiCalling();
            string vMsg = API.GenerateReport("GenerateHOAccLedgerDtlReport", Requestdata, vReportUrl);
            gblFuction.AjxMsgPopup(vMsg);
            btnExcl.Enabled = false;
        }

        private void GetData(string pFormat)
        {
            GetBranch();
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\AcLedDtl.rpt";
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = ViewState["ID"].ToString(); //Session[gblValue.BrnchCode].ToString();
            string vAcMst = Session[gblValue.ACVouMst].ToString();
            string vAcDtl = Session[gblValue.ACVouDtl].ToString();
            popDetail();
            Int32 vFinYrNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            string vDescID = ViewState["Dtl"].ToString();
            DataTable dt = null;
            //ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();

            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());

            if (vFinFrom > gblFuction.setDate(txtDtFrm.Text) || gblFuction.setDate(txtDtFrm.Text) > vFinTo)
            {
                gblFuction.MsgPopup("From Date should be within this financial year.");
                return;
            }
            if (vFinFrom > gblFuction.setDate(txtToDt.Text) || gblFuction.setDate(txtToDt.Text) > vFinTo)
            {
                gblFuction.MsgPopup("To date should be within this financial year.");
                return;
            }

            dt = oRpt.rptAcLedDetail(gblFuction.setDate(txtDtFrm.Text), gblFuction.setDate(txtToDt.Text),
                                    Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(),
                                    vDescID, vBrCode, vFinYrNo, vFinFrom);
            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1("0000"));
                rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2("0000"));
                rptDoc.SetParameterValue("pBranch", "Consolidate");
                rptDoc.SetParameterValue("pTitle", "A/C Ledger Details (Consolidate)");
                rptDoc.SetParameterValue("DtFrom", txtDtFrm.Text);
                rptDoc.SetParameterValue("DtTo", txtToDt.Text);
                if (pFormat == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_AC_Ledger_Details");
                else
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_AC_Ledger_Details");

                rptDoc.Dispose();
                Response.ClearHeaders();
                Response.ClearContent();
            }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //private void PopBranch()
        //{
        //    //Int32 vBrId = 0;
        //    //string vBrCode = "";
        //    ViewState["ID"] = null;
        //    DataTable dt = null;
        //    CGblIdGenerator oCG = null;
        //    try
        //    {
        //        DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //        //vBrCode = (string)Session[gblValue.BrnchCode];
        //        //vBrId = Convert.ToInt32(vBrCode);
        //        oCG = new CGblIdGenerator();
        //        dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, "0000");
        //        chkBr.DataSource = dt;
        //        chkBr.DataTextField = "Name";
        //        chkBr.DataValueField = "BranchCode";
        //        chkBr.DataBind();
        //        CheckAll();
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oCG = null;
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUser"></param>
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
            ViewState["ID"] = null;
            try
            {
                if (dt.Rows.Count > 0)
                {
                    chkBr.DataSource = dt;
                    chkBr.DataTextField = "BranchName";
                    chkBr.DataValueField = "BranchCode";
                    chkBr.DataBind();
                    CheckAllBr();
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }

    }

    [DataContract]
    public class HOAccLedgerDtlRequest
    {
        [DataMember]
        public string pFromDt { get; set; }
        [DataMember]
        public string pToDt { get; set; }
        [DataMember]
        public string pMst { get; set; }
        [DataMember]
        public string pDtl { get; set; }
        [DataMember]
        public string pDescID { get; set; }
        [DataMember]
        public string pBranch { get; set; }
        [DataMember]
        public string pYrNo { get; set; }
        [DataMember]
        public string pFinFromDt { get; set; }        
        [DataMember]
        public string pFormat { get; set; }
        [DataMember]
        public string pUserId { get; set; }
        [DataMember]
        public string pDBName { get; set; }
        [DataMember]
        public string pServerIP { get; set; }
        [DataMember]
        public string pPassword { get; set; }
        [DataMember]
        public string pCompanyName { get; set; }
        [DataMember]
        public string pCompanyAddress { get; set; }
    }


}
