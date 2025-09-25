using System;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;
using System.Web;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HOLoanSanctionRpt : CENTRUMBase
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
                PopBranch();
                PopState();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Loan Sanction";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.nmuLoanSpficRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Sanction", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtFromDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = Convert.ToString(ViewState["BrCode"]);
            string vTag = "N", vRptPath = "", vTitle = "";
            string vBranch = Session[gblValue.BrName].ToString();
            //ReportDocument rptDoc = new ReportDocument();
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();

            //TimeSpan t = vToDt - vFromDt;
            //if (t.TotalDays > 2)
            //{
            //    gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
            //    return;
            //}

            if (rdbOpt.SelectedValue == "rdbNA")
            {
                vTag = "N";
                vTitle = "Loan Application(New).";             
            }
            else if (rdbOpt.SelectedValue == "rdbAP")
            {
                vTag = "Y";
                vTitle = "Loan Sanction.";               
            }
            else if (rdbOpt.SelectedValue == "rdbAD")
            {
                vTag = "D";
                vTitle = "Pre Disbursement Approval";               
            }
            else if (rdbOpt.SelectedValue == "rdbCN")
            {
                vTag = "C";
                vTitle = "Cancel Report.";               
            }
            else if (rdbOpt.SelectedValue == "rdbHOAD")
            {
                vTag = "H";
                vTitle = "HO Disbursement Approval.";               
            }
            
            //dt = oRpt.rptLoanSanction(vTag, vFromDt, vToDt, vBrCode);
            ////---------------------------------------Aadhar Musking----------------------------------------
            //if (Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
            //{
            //    foreach (DataRow dr in dt.Rows) // search whole table
            //    {
            //        if (Convert.ToString(dr["Id_Type"].ToString()) == "AADHAAR")
            //        {
            //            dr["Idnum"] = String.Format("{0}{1}", "********", Convert.ToString(dr["Idnum"]).Substring(Convert.ToString(dr["Idnum"]).Length - 4, 4));                          
            //        }
            //        if (Convert.ToString(dr["Id_Type2"].ToString()) == "AADHAAR")
            //        {
            //            dr["Idnum2"] = String.Format("{0}{1}", "********", Convert.ToString(dr["Idnum2"]).Substring(Convert.ToString(dr["Idnum2"]).Length - 4, 4));                        
            //        }
            //        if (rdbOpt.SelectedValue == "rdbAD")
            //        {
            //            if (Convert.ToString(dr["Co Id Proof"].ToString()) == "AADHAAR")
            //            {
            //                dr["Co Id Proof No"] = String.Format("{0}{1}", "********", Convert.ToString(dr["Co Id Proof No"]).Substring(Convert.ToString(dr["Co Id Proof No"]).Length - 4, 4));
            //            }
            //        }

            //    }
            //}
            //------------------------------------------------------------------------------------

            //using (ReportDocument rptDoc = new ReportDocument())
            //{
            //    rptDoc.Load(vRptPath);
            //    rptDoc.SetDataSource(dt);
            //    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
            //    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1("0000"));
            //    rptDoc.SetParameterValue("pAddress2", "");
            //    rptDoc.SetParameterValue("pBranch", vBranch);
            //    rptDoc.SetParameterValue("pTitle", vTitle);
            //    rptDoc.SetParameterValue("dtFrom", txtFromDt.Text);
            //    rptDoc.SetParameterValue("dtTo", txtToDt.Text);
            //    if (pMode == "PDF")
            //        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "HO_Loan_Sanction_Report");
            //    else if (pMode == "Excel")
            //        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "HO_Loan_Sanction_Report");

            //    rptDoc.Dispose();
            //    Response.ClearHeaders();
            //    Response.ClearContent();
            //}

            if (pMode == "Excel")
            {
                TimeSpan t = gblFuction.setDate(txtToDt.Text.Trim()) - gblFuction.setDate(txtFromDt.Text.Trim());
                if (t.TotalDays > 30)
                {
                    gblFuction.AjxMsgPopup("You can not downloand more than 30 days report.");
                    return;
                }
                // SetParameterForRptData("Excel");
                var req = new LoanSanctionReportRequest()
                {
                    prdbOpt = rdbOpt.SelectedValue ,
                    pViewAadhaar = Convert.ToString(Session[gblValue.ViewAAdhar]),
                    pTag = vTag,
                    pFromDt = txtFromDt.Text,
                    pToDt = txtToDt.Text,
                    pBrCode = vBrCode,
                    pFormat = "Excel",
                    pUserId = Convert.ToString(Session[gblValue.UserId]),
                    pDBName = vDBName,
                    pPassword = vPw,
                    pServerIP = vSrvName
                };
                string Requestdata = JsonConvert.SerializeObject(req);
                //GenerateReport("GenerateOCRLogReport", Requestdata);
                Int32 vRptLoanSanctionLog = Convert.ToInt32(Session[gblValue.RptLoanSanctionLog].ToString());
                if (vRptLoanSanctionLog != 0)
                {
                    Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    if (unixTicks - vRptLoanSanctionLog > 300)
                    {
                        Session[gblValue.RptLoanSanctionLog] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                        GenerateReport("GenerateLoanSanctionRpt", Requestdata);
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Already Report Generate Request Is Executing ...Please Wait For 5 Mins..And Re Generate..");
                    }
                }
                else
                {
                    Session[gblValue.RptLoanSanctionLog] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    GenerateReport("GenerateLoanSanctionRpt", Requestdata);

                }
            }
            else
            {
                TimeSpan t = gblFuction.setDate(txtToDt.Text.Trim()) - gblFuction.setDate(txtFromDt.Text.Trim());
                if (t.TotalDays > 30)
                {
                    gblFuction.AjxMsgPopup("You can not downloand more than 30 days report.");
                    return;
                }
                //SetParameterForRptData("CSV");
                var req = new LoanSanctionReportRequest()
                {
                    prdbOpt = rdbOpt.SelectedValue ,
                    pViewAadhaar = Convert.ToString(Session[gblValue.ViewAAdhar]),
                    pTag = vTag,
                    pFromDt = txtFromDt.Text,
                    pToDt = txtToDt.Text,
                    pBrCode = vBrCode,
                    pFormat = "CSV",
                    pUserId = Convert.ToString(Session[gblValue.UserId]),
                    pDBName = vDBName,
                    pPassword = vPw,
                    pServerIP = vSrvName
                };
                string Requestdata = JsonConvert.SerializeObject(req);
                //GenerateReport("GenerateOCRLogReport", Requestdata);
                Int32 vRptLoanSanctionLog = Convert.ToInt32(Session[gblValue.RptLoanSanctionLog].ToString());
                if (vRptLoanSanctionLog != 0)
                {
                    Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    if (unixTicks - vRptLoanSanctionLog > 300)
                    {
                        Session[gblValue.RptLoanSanctionLog] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                        GenerateReport("GenerateLoanSanctionRpt", Requestdata);
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Already Report Generate Request Is Executing ...Please Wait For 5 Mins..And Re Generate..");
                    }
                }
                else
                {
                    Session[gblValue.RptLoanSanctionLog] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    GenerateReport("GenerateLoanSanctionRpt", Requestdata);
                }
            }
        }

        private void PopBranch()
        {
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            DataTable dt = null;
            CUser oUsr = null;
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));

            chkBrDtl.DataSource = dt;
            chkBrDtl.DataTextField = "BranchName";
            chkBrDtl.DataValueField = "BranchCode";
            chkBrDtl.DataBind();

            if (ddlSel.SelectedValue == "rbAll")
            {
                chkBrDtl.Enabled = false;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
            }
            else if (ddlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = false;
                }
            }
            ViewState["BrCode"] = strin;
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

        protected void btnStateWise_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CUser oUsr = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            string pUser = Session[gblValue.UserName].ToString();
            chkBrDtl.Items.Clear();
            ViewState["Id"] = null;
            try
            {
                DateTime vLogDt = gblFuction.setDate(txtToDt.Text.ToString());
                oCG = new CGblIdGenerator();
                oUsr = new CUser();
                ViewState["Id"] = null;
                if (vBrCode == "0000")
                {
                    dt = oUsr.GetBranchByState(pUser, Convert.ToInt32(Session[gblValue.RoleId]), ddlState.SelectedValues.Replace("|", ","), ddlBrType.SelectedValue);
                    if (dt.Rows.Count > 0)
                    {
                        chkBrDtl.DataSource = dt;
                        chkBrDtl.DataTextField = "BranchName";
                        chkBrDtl.DataValueField = "BranchCode";
                        chkBrDtl.DataBind();
                        if (ddlSel.SelectedValue == "rbAll")
                            CheckBrAll();
                    }
                }
                else
                {
                    dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, vBrCode);
                    chkBrDtl.DataSource = dt;
                    chkBrDtl.DataTextField = "Name";
                    chkBrDtl.DataValueField = "BranchCode";
                    chkBrDtl.DataBind();
                    if (ddlSel.SelectedValue == "rbAll")
                        CheckBrAll();
                }

            }
            finally
            {
                dt = null;
                oUsr = null;
                oCG = null;
            }

        }
        private void CheckBrAll()
        {
            Int32 vRow;
            string strin = "";
            if (ddlSel.SelectedValue == "rbAll")
            {
                chkBrDtl.Enabled = false;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
            }
            else if (ddlSel.SelectedValue == "rbSel")
            {
                ViewState["BrCode"] = null;
                chkBrDtl.Enabled = true;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    chkBrDtl.Items[vRow].Selected = false;

            }
            ViewState["BrCode"] = strin;
        }

        protected void ddlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBrAll();
        }

        protected void chkBrDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
            {
                if (chkBrDtl.Items[vRow].Selected == true)
                {
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
            }
            ViewState["BrCode"] = strin;
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("PDF");
        }

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
        }

        protected void btnCSV_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("CSV");
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

        private void PrintTxt(DataTable dt)
        {
            string vFolderPath = "C:\\BijliReport";
            string vFileNm = "";
            vFileNm = vFolderPath + "\\LoanSanctionReport_Centrum.txt";

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
    public class LoanSanctionReportRequest
    {
        [DataMember]
        public string prdbOpt { get; set; }
        [DataMember]
        public string pViewAadhaar { get; set; }
        [DataMember]
        public string pTag { get; set; }
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