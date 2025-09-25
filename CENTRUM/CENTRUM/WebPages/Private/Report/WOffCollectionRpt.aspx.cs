using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web;
using System.IO;
using System.Runtime.Serialization;
using System.Net;
using System.Text;
using System.Configuration;
using Newtonsoft.Json;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class WOffCollectionRpt : CENTRUMBase
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
                //PopBranch(Session[gblValue.UserName].ToString());
                PopList();
                CheckAll();
                popDetail();
                PopBranchList();
            }

            if (Page.IsPostBack == false)
            {
                // Set default values.
                rblSel.SelectedIndex = 0;
                rblAlSel_SelectedIndexChanged(sender, e);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUser"></param>
        //private void PopBranch(string pUser)
        //{
        //    DataTable dt = null;
        //    CUser oUsr = null;
        //    try
        //    {
        //        oUsr = new CUser();
        //        dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
        //        if (dt.Rows.Count > 0)
        //        {
        //            ddlBranch.DataSource = dt;
        //            ddlBranch.DataTextField = "BranchName";
        //            ddlBranch.DataValueField = "BranchCode";
        //            ddlBranch.DataBind();
        //            ListItem liSel = new ListItem("<--- Select --->", "-1");
        //            ddlBranch.Items.Insert(0, liSel);
        //        }
        //        else
        //        {
        //            ListItem liSel = new ListItem("<--- Select --->", "-1");
        //            ddlBranch.Items.Insert(0, liSel);
        //        }
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oUsr = null;
        //    }
        //}
        private void PopBranchList()
        {
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            //string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oCG = new CGblIdGenerator();
            dt = oCG.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", vBrId, "BranchCode", "AA", vLogDt, "0000");

            ChkBranch.DataSource = dt;
            ChkBranch.DataTextField = "BranchName";
            ChkBranch.DataValueField = "BranchCode";
            ChkBranch.DataBind();

            if (rdbSel.SelectedValue == "rdbAll")
            {
                ChkBranch.Enabled = false;
                for (vRow = 0; vRow < ChkBranch.Items.Count; vRow++)
                {
                    ChkBranch.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = ChkBranch.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + ChkBranch.Items[vRow].Value + "";
                    }
                }
            }
            else if (rdbSel.SelectedValue == "rdbSelct")
            {
                for (vRow = 0; vRow < ChkBranch.Items.Count; vRow++)
                {
                    ChkBranch.Items[vRow].Selected = false;
                }
            }
            ViewState["BrCode"] = strin;
        }
        private void CheckBrAll()
        {
            Int32 vRow;
            string strin = "";
            if (rdbSel.SelectedValue == "rdbAll")
            {
                ChkBranch.Enabled = false;
                for (vRow = 0; vRow < ChkBranch.Items.Count; vRow++)
                {
                    ChkBranch.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = ChkBranch.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + ChkBranch.Items[vRow].Value + "";
                    }
                }
                ViewState["BrCode"] = strin;
            }
            else if (rdbSel.SelectedValue == "rdbSelct")
            {
                ViewState["BrCode"] = null;
                ChkBranch.Enabled = true;
                for (vRow = 0; vRow < ChkBranch.Items.Count; vRow++)
                    ChkBranch.Items[vRow].Selected = false;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    PopList();
        //    CheckAll();
        //    popDetail();
        //}
        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Bad Debt Written Off Loan Collection Report";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.nmuWOffCollectionRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Bad Debt Written Off Collection Report", false);
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
            CGblIdGenerator oCG = null;
            CEO oRO = null;
            string vBrCode = "0000";//Convert.ToString(ddlBranch.SelectedValue);            
            Int32 vBrId = Convert.ToInt32(vBrCode);
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            if (rblSel.SelectedValue == "rbEO")
            {
                //oCG = new CGblIdGenerator();
                //dt = oCM.GetCOPop(vBrCode, "SCO,CO,TCO,JTCO,GO");
                dt = new DataTable();
                //dt = oCG.PopComboMIS("D", "N", "AA", "EoId", "EoName", "EoMst", vBrCode, "BranchCode", "Tra_DropDate", vLogDt, vBrCode);
                //chkDtl.DataSource = dt;
                //chkDtl.DataTextField = "EoName";
                //chkDtl.DataValueField = "EoId";
                //chkDtl.DataBind();
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "EoName";
                chkDtl.DataValueField = "Eoid";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbLType")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanType", "LoanTypeMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "LoanType";
                chkDtl.DataValueField = "LoanTypeId";
                chkDtl.DataBind();
            }
            if (rblSel.SelectedValue == "rbFund")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "FundSourceId", "FundSource", "FundSourceMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "FundSource";
                chkDtl.DataValueField = "FundSourceId";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbGrp")
            {
                //oCG = new CGblIdGenerator();
                //dt = oCG.PopComboMIS("N", "N", "AA", "GroupId", "GroupName", "GroupMst", vBrId, "BranchCode", "AA", vLogDt, vBrCode);
                //chkDtl.DataSource = dt;
                //chkDtl.DataTextField = "GroupName";
                //chkDtl.DataValueField = "GroupId";
                //chkDtl.DataBind();

                //dt = oCG.PopTransferMIS("N", "GroupMst", "", vLogDt, vBrCode);
                //chkDtl.DataSource = dt;
                //chkDtl.DataTextField = "GroupName";
                //chkDtl.DataValueField = "GroupId";
                //chkDtl.DataBind();

                //vBrCode = (string)Session[gblValue.BrnchCode];
                //vBrId = Convert.ToInt32(vBrCode);
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "GroupCode", "GroupID", "GroupName", "GroupMst", 0, "EOID", "DropoutDt", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "GroupName";
                chkDtl.DataValueField = "GroupID";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbCyl")
            {
                chkDtl.Items.Clear();
                ListItem oLs1 = new ListItem();
                oLs1.Text = "1st Cycle";
                oLs1.Value = "1";
                chkDtl.Items.Add(oLs1);

                ListItem oLs2 = new ListItem();
                oLs2.Text = "2nd Cycle";
                oLs2.Value = "2";
                chkDtl.Items.Add(oLs2);

                ListItem oLs3 = new ListItem();
                oLs3.Text = "3rd Cycle";
                oLs3.Value = "3";
                chkDtl.Items.Add(oLs3);

                ListItem oLs4 = new ListItem();
                oLs4.Text = "4th Cycle";
                oLs4.Value = "4";
                chkDtl.Items.Add(oLs4);

                ListItem oLs5 = new ListItem();
                oLs5.Text = "5th Cycle";
                oLs5.Value = "5";
                chkDtl.Items.Add(oLs5);
            }

            if (rblSel.SelectedValue.Equals("rbWhole"))
            {
                chkDtl.Items.Clear();
            }
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopList();
            CheckAll();
            popDetail();
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
            popDetail();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = ChkBranchSel();
            string vRptPath = "", vTypeId = "", vType = "";
            string vBranch = "0000";//Convert.ToString(ddlBranch.SelectedItem);

            ReportDocument rptDoc = new ReportDocument();
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            vTypeId = ViewState["Dtl"].ToString();

            if (rblSel.SelectedValue == "rbEO")
                vType = "E";

            if (rblSel.SelectedValue == "rbLType")
                vType = "L";

            if (rblSel.SelectedValue == "rbFund")
                vType = "F";

            if (rblSel.SelectedValue == "rbGrp")
                vType = "G";

            if (rblSel.SelectedValue == "rbCyl")
                vType = "C";

            if (rblSel.SelectedValue == "rbWhole")
                vType = "A";

            if (vType == "A" && pMode == "Excel" && rbOpt.SelectedValue == "rbDtl")
            {
                //dt = oRpt.rptWOffLoanCollection_All(vFromDt, vToDt, vBrCode);
                //string attachment = "attachment; filename=Bad Debt Written Off Loan Collection List.xls";
                //Response.ClearContent();
                //Response.AddHeader("content-disposition", attachment);
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.ContentType = "application/vnd.ms-excel";
                //HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                //Response.Write("<table border='1' cellpadding='5' widht='120%'>");
                //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='4'>" + gblValue.CompName + " </font></b></td></tr>");               
                //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>From - " + txtDtFrm.Text + " To - " + txtToDt.Text + "</font></b></td></tr>");
                //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>Write-Off Collection Report (All)</font></b></td></tr>");
                //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'></font></b></td></tr>");
                //string tab = string.Empty;
                //Response.Write("<tr>");
                //foreach (DataColumn dc in dt.Columns)
                //{         
                //    Response.Write("<td><b>" + dc.ColumnName + "<b></td>");
                //}
                //Response.Write("</tr>");
                //int i;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    Response.Write("<tr style='height:20px;'>");
                //    for (i = 0; i < dt.Columns.Count; i++)
                //    {
                //        if (dt.Columns[i].ColumnName == "Recovery Officer ID")
                //        {
                //            Response.Write("<td &nbsp nowrap class='txt'>" + Convert.ToString(dr[i]) + "</td>");
                //        }
                //        else
                //        {
                //            Response.Write("<td nowrap>" + Convert.ToString(dr[i]) + "</td>");
                //        }                      
                //    }
                //    Response.Write("</tr>");
                //}
                //Response.Write("</table>");
                //Response.End();

                SetRptDataExcel(pMode);
            }
            else
            {

                if (rbOpt.SelectedValue == "rbDtl")
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\WOffCollectionDtl.rpt";
                if (rbOpt.SelectedValue == "rbSum")
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\WOffCollectionSum.rpt";
              
                dt = oRpt.rptWOffLoanCollection(vFromDt, vToDt, vBrCode, vType, vTypeId);
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                rptDoc.SetParameterValue("pAddress2", "");
                rptDoc.SetParameterValue("pBranch", vBranch);
                rptDoc.SetParameterValue("pType", vType);
                rptDoc.SetParameterValue("dtFrom", txtDtFrm.Text);
                rptDoc.SetParameterValue("dtTo", txtToDt.Text);
                if (pMode == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Bad Debt Written Off Loan Collection List");
                else if (pMode == "Excel")
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Bad Debt Written Off Loan Collection List");
                rptDoc.Dispose();
            }
        }

        private void SetRptDataExcel(string vMode)
        {                   
            CReports oRpt = null;

            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = ChkBranchSel();
            
            
            try
            {
                oRpt = new CReports();
                
                var req = new WOffCollReportRequest()
                {
                    pFromDt = txtDtFrm.Text,
                    pToDt = txtToDt.Text,
                    pBrCode = vBrCode,                
                    pFormat = vMode,
                    pUserId = Convert.ToString(Session[gblValue.UserId]),
                    pDBName = vDBName,
                    pPassword = vPw,
                    pServerIP = vSrvName,
                    pCompanyName = gblValue.CompName,
                    pCompanyAddress = CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()),                   
                };

                string Requestdata = JsonConvert.SerializeObject(req);
                Int32 vRptWOffColl = Convert.ToInt32(Session[gblValue.RptWOffColl].ToString());
                if (vRptWOffColl != 0)
                {
                    Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    if (unixTicks - vRptWOffColl > 300)
                    {
                        Session[gblValue.RptWOffColl] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                        GenerateReport("GenerateWOffColl", Requestdata);
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Already Report Generate Request Is Executing ...Please Wait For 5 Mins..And Re Generate..");
                    }
                }
                else
                {
                    Session[gblValue.RptWOffColl] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    GenerateReport("GenerateWOffColl", Requestdata);
                }
            }
            finally
            {                
                oRpt = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("PDF");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            //SetParameterForRptData("Excel");
            SetRptDataExcel("Excel");
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
        protected void rdbSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CheckAll();
            CheckBrAll();
            uprdbSel.Update();
        }
        public string ChkBranchSel()
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < ChkBranch.Items.Count; vRow++)
            {
                if (ChkBranch.Items[vRow].Selected == true)
                {
                    if (strin == "")
                    {
                        strin = ChkBranch.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + ChkBranch.Items[vRow].Value + "";
                    }
                }
            }
            return strin;

        }

        private void GenerateReport(string pApiName, string pRequestdata)
        {
            string vMsg = "";
            CApiCalling oAPI = new CApiCalling();
            try
            {
                vMsg = oAPI.GenerateReport(pApiName, pRequestdata, vReportUrl);
            }
            finally
            {
                gblFuction.AjxMsgPopup(vMsg);
                btnPdf.Enabled = false;
                btnExcl.Enabled = false;
            }
        }
    }

    [DataContract]
    public class WOffCollReportRequest
    {
        [DataMember]
        public string pFromDt { get; set; }
        [DataMember]
        public string pToDt { get; set; }
        [DataMember]
        public string pBrCode { get; set; }         
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

