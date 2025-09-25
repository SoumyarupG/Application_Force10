using System;
using System.Data;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Configuration;
using System.Net;
using System.Text;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class EquiFaxDataSubmission : CENTRUMBase
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
                PopBranch(Session[gblValue.UserName].ToString());
                txtAsDt.Text = Session[gblValue.LoginDate].ToString();
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "EquiFax Data Submission";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHOEqFaxDataSub);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "EquiFax Submission Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void rdbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbMode.SelectedValue == "M")
            {
                tbl1Dt.Visible = true;
                tbl2Dt.Visible = false;
            }
            else if (rdbMode.SelectedValue == "W")
            {
                tbl1Dt.Visible = false;
                tbl2Dt.Visible = true;
            }
        }

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
                    CheckAll();
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }

        }

        private void CheckAll()
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

        protected void ddlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
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

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (rdbMode.SelectedValue == "M")
                PrintMonthlyRpt("Print");
            if (rdbMode.SelectedValue == "W")
                PrintWeeklyRpt("Print");
        }

        protected void btnCSV_Click(object sender, EventArgs e)
        {
            if (rdbMode.SelectedValue == "M")
                PrintMonthlyRpt("CSV");
            if (rdbMode.SelectedValue == "W")
                PrintWeeklyRpt("CSV");
        }

        private void PrintMonthlyRpt(string pFormat)
        {
            DataTable dtMem = null;
            CEquiFaxDataSubmission oHM = null;
            string vFolderPath = "C:\\EquiFax_WebM";
            string vBrCode = "A";
            string vFileNm = "", vIsBC = "";

            try
            {
                if (ddlSel.SelectedValue == "B")
                {
                    GetBranch();
                    vBrCode = Convert.ToString(ViewState["ID"]);
                }
                if (chkBC.Checked == true)
                    vIsBC = "Y";
                else
                    vIsBC = "N";
                //oHM = new CEquiFaxDataSubmission();
                //dtMem = oHM.GetEquiFaxMember(gblFuction.setDate(txtAsDt.Text.Trim()), vBrCode, vIsBC);
                var req = new EquifaxReportRequest()
                {
                    pFormat = pFormat,
                    pAsOnDt = txtAsDt.Text,
                    pBrCode = vBrCode,
                    pIsBC = vIsBC,
                    pUserId = Convert.ToString(Session[gblValue.UserId]),
                    pDBName = vDBName,
                    pPassword = vPw,
                    pServerIP = vSrvName
                };
                string Requestdata = JsonConvert.SerializeObject(req);
                GenerateReport("PrintMonthlyRpt", Requestdata);
                //vFileNm = vFolderPath + "\\Member_DataM.txt";

                //try
                //{
                //    if (System.IO.Directory.Exists(vFolderPath))
                //    {
                //        foreach (var file in Directory.GetFiles(vFolderPath))
                //        {
                //            if (File.Exists(vFileNm) == true)
                //                File.Delete(vFileNm);
                //        }
                //    }
                //    else
                //    {
                //        Directory.CreateDirectory(vFolderPath);
                //    }
                //    Write(dtMem, vFileNm);
                //    downloadfile(vFileNm);
                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}
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

        private void PrintWeeklyRpt(string pFormat)
        {
            DataTable dtMem = null;
            CEquiFaxDataSubmission oHM = null;
            string vFolderPath = "C:\\EquiFax_WebW";
            string vBrCode = "A";
            string vFileNm = "", vIsBC = "";

            try
            {
                if (ddlSel.SelectedValue == "B")
                {
                    GetBranch();
                    vBrCode = Convert.ToString(ViewState["ID"]);
                }
                if (chkBC.Checked == true)
                    vIsBC = "Y";
                else
                    vIsBC = "N";

                var req = new EquifaxReportWRequest()
                {
                    pFormat = pFormat,
                    pFromDt = txtFrmDt.Text,
                    pToDt = txtToDt.Text,
                    pBrCode = vBrCode,
                    pIsBC = vIsBC,
                    pUserId = Convert.ToString(Session[gblValue.UserId]),
                    pDBName = vDBName,
                    pPassword = vPw,
                    pServerIP = vSrvName
                };
                string Requestdata = JsonConvert.SerializeObject(req);
                GenerateReport("PrintWeeklyRpt", Requestdata);

                //oHM = new CEquiFaxDataSubmission();
                //dtMem = oHM.GetEquiFaxMemberW(gblFuction.setDate(txtFrmDt.Text.Trim()), gblFuction.setDate(txtToDt.Text.Trim()), vBrCode, vIsBC);
                //vFileNm = vFolderPath + "\\Member_DataW.txt";

                //try
                //{
                //    if (System.IO.Directory.Exists(vFolderPath))
                //    {
                //        foreach (var file in Directory.GetFiles(vFolderPath))
                //        {
                //            if (File.Exists(vFileNm) == true)
                //                File.Delete(vFileNm);
                //        }
                //    }
                //    else
                //    {
                //        Directory.CreateDirectory(vFolderPath);
                //    }
                //    Write(dtMem, vFileNm);
                //    downloadfile(vFileNm);
                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}
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

        static void Write(DataTable dt, string outputFilePath)
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
                int J = 0;
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!row.IsNull(i))
                        {
                            //sw.Write(row[i].ToString().PadRight(maxLengths[i] + 2));
                            sw.Write(row[i].ToString().PadRight(maxLengths[i]));
                        }
                        else
                        {
                            sw.Write(new string(' ', maxLengths[i] + 2));
                        }
                    }
                    J++;
                    if (dt.Rows.Count != J)
                        sw.WriteLine();
                }
                sw.Close();
            }
        }

        protected void downloadfile(string filename)
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
            try
            {
                string Requestdata = pRequestdata;
                string postURL = vReportUrl + "/" + pApiName;
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 2000;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                byte[] data = Encoding.UTF8.GetBytes(Requestdata);
                request.ContentLength = data.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
            }
            finally
            {
                gblFuction.AjxMsgPopup("Please comeback after few minutes.");
            }
        }

    }

    [DataContract]
    public class EquifaxReportRequest
    {
        [DataMember]
        public string pFormat { get; set; }
        [DataMember]
        public string pAsOnDt { get; set; }
        [DataMember]
        public string pBrCode { get; set; }
        [DataMember]
        public string pIsBC { get; set; }
        [DataMember]
        public string pUserId { get; set; }
        [DataMember]
        public string pDBName { get; set; }
        [DataMember]
        public string pServerIP { get; set; }
        [DataMember]
        public string pPassword { get; set; }
    }

    [DataContract]
    public class EquifaxReportWRequest
    {
        [DataMember]
        public string pFormat { get; set; }
        [DataMember]
        public string pFromDt { get; set; }
        [DataMember]
        public string pToDt { get; set; }
        [DataMember]
        public string pBrCode { get; set; }
        [DataMember]
        public string pIsBC { get; set; }
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