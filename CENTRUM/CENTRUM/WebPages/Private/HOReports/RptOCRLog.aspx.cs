using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Configuration;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class RptOCRLog : CENTRUMBase
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
                txtFromDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "OCR Log";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuOCRLog);
                if (this.UserID == 1) return;
                if (this.CanView == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "OCR LOG", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void btnExcl_Click(object sender, EventArgs e)
        {
            TimeSpan t = gblFuction.setDate(txtToDt.Text.Trim()) - gblFuction.setDate(txtFromDt.Text.Trim());
            if (t.TotalDays > 2)
            {
                gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                return;
            }
            // SetParameterForRptData("Excel");
            var req = new OCRReportRequest()
            {
                pFromDt = txtFromDt.Text,
                pToDt = txtToDt.Text,
                pFormat = "Excel",
                pUserId = Convert.ToString(Session[gblValue.UserId]),
                pDBName = vDBName,
                pPassword = vPw,
                pServerIP = vSrvName
            };
            string Requestdata = JsonConvert.SerializeObject(req);
            //GenerateReport("GenerateOCRLogReport", Requestdata);
            Int32 vRptOCRLog = Convert.ToInt32(Session[gblValue.RptOCRLog].ToString());
            if (vRptOCRLog != 0)
            {
                Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                if (unixTicks - vRptOCRLog > 300)
                {
                    Session[gblValue.RptOCRLog] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    GenerateReport("GenerateOCRLogReport", Requestdata);
                }
                else
                {
                    gblFuction.AjxMsgPopup("Already Report Generate Request Is Executing ...Please Wait For 5 Mins..And Re Generate..");
                }
            }
            else
            {
                Session[gblValue.RptOCRLog] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                GenerateReport("GenerateOCRLogReport", Requestdata);

            }
        }
        protected void btnCSV_Click(object sender, EventArgs e)
        {
            TimeSpan t = gblFuction.setDate(txtToDt.Text.Trim()) - gblFuction.setDate(txtFromDt.Text.Trim());
            if (t.TotalDays > 2)
            {
                gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                return;
            }
            //SetParameterForRptData("CSV");
            var req = new OCRReportRequest()
            {
                pFromDt = txtFromDt.Text,
                pToDt = txtToDt.Text,
                pFormat = "CSV",
                pUserId = Convert.ToString(Session[gblValue.UserId]),
                pDBName = vDBName,
                pPassword = vPw,
                pServerIP = vSrvName
            };
            string Requestdata = JsonConvert.SerializeObject(req);
            //GenerateReport("GenerateOCRLogReport", Requestdata);
            Int32 vRptOCRLog = Convert.ToInt32(Session[gblValue.RptOCRLog].ToString());
            if (vRptOCRLog != 0)
            {
                Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                if (unixTicks - vRptOCRLog > 300)
                {
                    Session[gblValue.RptOCRLog] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    GenerateReport("GenerateOCRLogReport", Requestdata);
                }
                else
                {
                    gblFuction.AjxMsgPopup("Already Report Generate Request Is Executing ...Please Wait For 5 Mins..And Re Generate..");
                }
            }
            else
            {
                Session[gblValue.RptOCRLog] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                GenerateReport("GenerateOCRLogReport", Requestdata);

            }
        }


        private void SetParameterForRptData(string pMode)
        {
            DataTable dt = null;
            CReports oRpt = new CReports();
            string vFileNm = "";
            try
            {
                oRpt = new CReports();
                dt = oRpt.RptOCRLog(gblFuction.setDate(txtFromDt.Text), gblFuction.setDate(txtToDt.Text));
                if (dt.Rows.Count > 0)
                {
                    if (pMode == "Excel")
                    {
                        vFileNm = "attachment;filename=OCR_Log_Report_" + DateTime.Now + ".xls";
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", vFileNm);
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.ms-excel";
                        HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                        Response.Write("<table border='1' cellpadding='5' widht='120%'>");
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
                                Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
                            }
                            Response.Write("</tr>");
                        }
                        Response.Write("</table>");
                        Response.End();
                    }
                    else
                    {
                        PrintTxt(dt);
                        //vFileNm = "attachment;filename=OCR_Log_Report_" + DateTime.Now + ".csv";
                        //string csv = string.Empty;
                        //foreach (DataColumn column in dt.Columns)
                        //{
                        //    //Add the Header row for CSV file.
                        //    csv += column.ColumnName + ',';
                        //}

                        ////Add new line.
                        //csv += "\r\n";
                        //foreach (DataRow row in dt.Rows)
                        //{
                        //    foreach (DataColumn column in dt.Columns)
                        //    {
                        //        //Add the Data rows.
                        //        csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                        //    }

                        //    //Add new line.
                        //    csv += "\r\n";
                        //}
                        ////Download the CSV file.
                        //Response.Clear();
                        //Response.Buffer = true;
                        //Response.AddHeader("content-disposition", vFileNm);
                        //Response.Charset = "";
                        //Response.ContentType = "application/text";
                        //Response.Output.Write(csv);
                        //Response.Flush();
                        //Response.End();
                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Data Found.");
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }
        private void PrintTxt(DataTable dt)
        {
            string vFolderPath = "C:\\BijliReport";
            string vFileNm = "";
            vFileNm = vFolderPath + "\\OCR_Log_Report.csv";

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
                StreamWriter sw = new StreamWriter(vFileNm, false);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sw.Write(dt.Columns[i]);
                    if (i < dt.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            string value = dr[i].ToString();
                            if (value.Contains(','))
                            {
                                value = String.Format("\"{0}\"", value);
                                sw.Write(value);
                            }
                            else
                            {
                                sw.Write(dr[i].ToString());
                            }
                        }
                        if (i < dt.Columns.Count - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
                // Write(dt, vFileNm);
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

    }

    [DataContract]
    public class OCRReportRequest
    {
        [DataMember]
        public string pFromDt { get; set; }
        [DataMember]
        public string pToDt { get; set; }
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
    }
}