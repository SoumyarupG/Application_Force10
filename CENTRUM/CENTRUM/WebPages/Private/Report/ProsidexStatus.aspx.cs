using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;
using System.Web;
using System.IO;
using System.Configuration;
using System.Runtime.Serialization;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class ProsidexStatus : CENTRUMBase
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
                txtAsOnDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch(Session[gblValue.UserName].ToString());
                CheckAll();
                popDetail();
                PopState();
            }

            if (Page.IsPostBack == false)
            {               
                rblAlSel_SelectedIndexChanged(sender, e);
            }

        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Posidex Status";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuProsidexStatusRpt);

                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Posidex Status", false);
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
        }
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            ViewState["Id"] = null;
            try
            {
                DateTime vLogDt = gblFuction.setDate(txtAsOnDt.Text.ToString());
                oCG = new CGblIdGenerator();
                oUsr = new CUser();
                ViewState["Id"] = null;
                if (vBrCode == "0000")
                {
                    dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                    if (dt.Rows.Count > 0)
                    {
                        chkDtl.DataSource = dt;
                        chkDtl.DataTextField = "BranchName";
                        chkDtl.DataValueField = "BranchCode";
                        chkDtl.DataBind();
                        CheckAll();
                    }
                }
                else
                {
                    dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, vBrCode);
                    chkDtl.DataSource = dt;
                    chkDtl.DataTextField = "Name";
                    chkDtl.DataValueField = "BranchCode";
                    chkDtl.DataBind();
                    CheckAll();
                }

            }
            finally
            {
                dt = null;
                oUsr = null;
                oCG = null;
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
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            //SetParameterForRptData("PDF");
        }

        protected void btnStateWise_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CUser oUsr = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            string pUser = Session[gblValue.UserName].ToString();
            chkDtl.Items.Clear();
            ViewState["Id"] = null;
            try
            {
                DateTime vLogDt = gblFuction.setDate(txtAsOnDt.Text.ToString());
                oCG = new CGblIdGenerator();
                oUsr = new CUser();
                ViewState["Id"] = null;
                if (vBrCode == "0000")
                {
                    dt = oUsr.GetBranchByState(pUser, Convert.ToInt32(Session[gblValue.RoleId]), ddlState.SelectedValues.Replace("|", ","),ddlBrType.SelectedValue);
                    if (dt.Rows.Count > 0)
                    {
                        chkDtl.DataSource = dt;
                        chkDtl.DataTextField = "BranchName";
                        chkDtl.DataValueField = "BranchCode";
                        chkDtl.DataBind();
                        if (rblAlSel.SelectedValue == "rbAll")
                            CheckAll();
                    }
                }
                else
                {
                    dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, vBrCode);
                    chkDtl.DataSource = dt;
                    chkDtl.DataTextField = "Name";
                    chkDtl.DataValueField = "BranchCode";
                    chkDtl.DataBind();
                    if (rblAlSel.SelectedValue == "rbAll")
                        CheckAll();
                }

            }
            finally
            {
                dt = null;
                oUsr = null;
                oCG = null;
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
        }

        protected void btnCSV_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("CSV");
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
        //protected void chkDtl_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    popDetail();
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            DateTime vAsonDt = gblFuction.setDate(txtAsOnDt.Text);
            string vProjType = ddlProjType.SelectedValue;
            string vBranchCode = "";         
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            popDetail();
            vBranchCode = ViewState["Dtl"].ToString();
            //dt = oRpt.rptProxidexStatus(vAsonDt, vProjType, vBranchCode);
            //GenerateReport("GenerateIAStatRpt", Requestdata);
            //if (pMode == "Excel")
            //{
            //    string vFileNm = "attachment;filename=Proxidex Status Report.xls";
            //    Response.ClearContent();
            //    Response.AddHeader("content-disposition", vFileNm);
            //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //    Response.ContentType = "application/vnd.ms-excel";
            //    HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
            //    Response.Write("<table border='1' cellpadding='5' widht='120%'>");
            //    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
            //    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
            //    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>Proxidex Status For the As On Date " + txtAsOnDt.Text +  "</font></b></td></tr>");
            //    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'></font></b></td></tr>");
            //    string tab = string.Empty;
            //    Response.Write("<tr>");
            //    foreach (DataColumn dtcol in dt.Columns)
            //    {
            //        Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
            //    }
            //    Response.Write("</tr>");
            //    foreach (DataRow dtrow in dt.Rows)
            //    {
            //        Response.Write("<tr style='height:20px;'>");
            //        for (int j = 0; j < dt.Columns.Count; j++)
            //        {
            //            if (dt.Columns[j].ColumnName == "MemberNo" || dt.Columns[j].ColumnName == "Loan No")
            //            {
            //                Response.Write("<td &nbsp nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
            //            }
            //            else
            //            {
            //                Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
            //            }
            //        }
            //        Response.Write("</tr>");
            //    }
            //    Response.Write("</table>");
            //    Response.End();
            //}
            //else
            //{
            //    PrintTxt(dt);
            //}



            if (pMode == "Excel")
            {
                // SetParameterForRptData("Excel");
                var req = new ProsidexRptReq()
                {
                    pAsonDt = txtAsOnDt.Text,
                    pProjType = vProjType,
                    pBranch = vBranchCode,
                    pFormat = "Excel",
                    pUserId = Convert.ToString(Session[gblValue.UserId]),
                    pDBName = vDBName,
                    pPassword = vPw,
                    pServerIP = vSrvName
                };
                string Requestdata = JsonConvert.SerializeObject(req);
                //GenerateReport("GenerateOCRLogReport", Requestdata);
                Int32 vRptProsidexLog = Convert.ToInt32(Session[gblValue.RptProsidexLog].ToString());
                if (vRptProsidexLog != 0)
                {
                    Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    if (unixTicks - vRptProsidexLog > 300)
                    {
                        Session[gblValue.RptLoanSanctionLog] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                        GenerateReport("GenerateProsidexRpt", Requestdata);
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Already Report Generate Request Is Executing ...Please Wait For 5 Mins..And Re Generate..");
                    }
                }
                else
                {
                    Session[gblValue.RptLoanSanctionLog] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    GenerateReport("GenerateProsidexRpt", Requestdata);

                }
            }
            else
            {
                //SetParameterForRptData("CSV");
                var req = new ProsidexRptReq()
                {
                    pAsonDt = txtAsOnDt.Text,
                    pProjType = vProjType,
                    pBranch = vBranchCode,
                    pFormat = "CSV",
                    pUserId = Convert.ToString(Session[gblValue.UserId]),
                    pDBName = vDBName,
                    pPassword = vPw,
                    pServerIP = vSrvName
                };
                string Requestdata = JsonConvert.SerializeObject(req);
                //GenerateReport("GenerateOCRLogReport", Requestdata);
                Int32 vRptProsidexLog = Convert.ToInt32(Session[gblValue.RptProsidexLog].ToString());
                if (vRptProsidexLog != 0)
                {
                    Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    if (unixTicks - vRptProsidexLog > 300)
                    {
                        Session[gblValue.RptLoanSanctionLog] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                        GenerateReport("GenerateProsidexRpt", Requestdata);
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Already Report Generate Request Is Executing ...Please Wait For 5 Mins..And Re Generate..");
                    }
                }
                else
                {
                    Session[gblValue.RptLoanSanctionLog] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    GenerateReport("GenerateProsidexRpt", Requestdata);
                }
            }
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
                btnCSV.Enabled = false;
                btnExcl.Enabled = false;
            }
        }

        private void PrintTxt(DataTable dt)
        {
            string vFolderPath = "C:\\BijliReport";
            string vFileNm = "";
            vFileNm = vFolderPath + "\\PosidexStatus_Centrum.txt";

            try
            {
                if (System.IO.Directory.Exists(vFolderPath))
                {
                    foreach (var file in Directory.GetFiles(vFolderPath))
                    {
                        if (File.Exists(vFileNm) == true)
                            File.Delete(vFileNm);
                    }
                }
                else
                {
                    Directory.CreateDirectory(vFolderPath);
                }
                Write(dt, vFileNm);
                downloadfile(vFileNm);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                gblFuction.MsgPopup("Done");
                btnExit.Enabled = true;

            }
        }

        private void Write(DataTable dt, string outputFilePath)
        {
            int[] maxLengths = new int[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                maxLengths[i] = dt.Columns[i].ColumnName.Length;
                foreach (DataRow row in dt.Rows)
                {
                    if (!row.IsNull(i))
                    {
                        int length = row[i].ToString().Length;
                        if (length > maxLengths[i])
                        {
                            maxLengths[i] = length;
                        }
                    }
                }
            }
            using (StreamWriter sw = new StreamWriter(outputFilePath, false))
            {

                for (int i = 0; i < dt.Columns.Count; i++)
                {

                    sw.Write(dt.Columns[i].ColumnName.ToString().Trim() + '|');

                }
                sw.WriteLine();

                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!row.IsNull(i))
                        {
                            sw.Write(row[i].ToString().Trim() + '|');
                        }

                    }
                    sw.WriteLine();
                }
                sw.Close();
            }
        }

        private void downloadfile(string filename)
        {

            // check to see that the file exists 
            if (File.Exists(filename))
            {
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(filename);
                Response.End();
                File.Delete(filename);
            }
            else
            {
                gblFuction.AjxMsgPopup("File could not be found");
            }

        }

        //private void GenerateReport(string pApiName, string pRequestdata)
        //{
        //    try
        //    {
        //        string Requestdata = pRequestdata;
        //        string postURL = vReportUrl + "/" + pApiName;
        //        HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
        //        request.Method = "POST";
        //        request.ContentType = "application/json";
        //        request.Timeout = 2000;
        //        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
        //        byte[] data = Encoding.UTF8.GetBytes(Requestdata);
        //        request.ContentLength = data.Length;
        //        Stream requestStream = request.GetRequestStream();
        //        requestStream.Write(data, 0, data.Length);
        //        requestStream.Close();
        //    }
        //    finally
        //    {
        //        gblFuction.AjxMsgPopup("Please comeback after 5 min.");
        //    }
        //}

    }

    [DataContract]
    public class ProsidexRptReq
    {
        [DataMember]
        public string pAsonDt { get; set; }
        [DataMember]
        public string pProjType { get; set; }
        [DataMember]
        public string pBranch { get; set; }
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