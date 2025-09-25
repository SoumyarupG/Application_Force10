using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using FORCEBA;
using FORCECA;
using System.Configuration;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class UdyamAadharUploadDownload : CENTRUMBase
    {
        private static string vFile;
        private static string vExcelPath = ConfigurationManager.AppSettings["DBPath"];
        private static string vSrvName = ConfigurationManager.AppSettings["SrvName"];
        private static string vDBName = ConfigurationManager.AppSettings["DBName"];
        private static string vPw = ConfigurationManager.AppSettings["PassPW"];
        private static string vReportUrl = ConfigurationManager.AppSettings["ReportUrl"];
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtto.Text = Session[gblValue.LoginDate].ToString();
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "PageDirect", "PageDirect();", true);
                }
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Udyam Aadhar Upload Download";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuUdyamUploadDownload);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnSave.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Udyam Aadhar Upload Download", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        public DataSet ExcelToDS(string Path)
        {
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            conn.Open();
            strExcel = "select * from [sheet1$]";
            try
            {
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "ImportIDBI");
                conn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                gblFuction.AjxMsgPopup("Error:Please Check Excel");
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //DataSet ds = null;
            //DataTable dt = null;
            if (fuExcl.HasFile == true)
            {
                if (!Directory.Exists(vExcelPath))
                {
                    Directory.CreateDirectory(vExcelPath);
                }
                vFile = fuExcl.FileName;
                FileInfo vfile = new FileInfo(vExcelPath + vFile);
                if (vfile.Exists)
                {
                    vfile.Delete();
                }
                fuExcl.SaveAs(vExcelPath + vFile);
                //ds = ExcelToDS(vExcelPath + fuExcl.FileName);
                //dt = ds.Tables[0];
                //if (dt.Rows.Count > 0)
                //    gblFuction.AjxMsgPopup("Upload Successful !!");
                if (vfile.Exists)
                {
                    gblFuction.AjxMsgPopup("Upload Successful !!");
                }
            }
            else
                gblFuction.MsgPopup("Please Select a File");
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null;
            ds = ExcelToDS(vExcelPath + vFile);
            dt = ds.Tables[0];
            string vSaveMsg = "";

            if (dt.Rows.Count > 0)
            {
                bool vMsg = false;
                foreach (DataRow dR in dt.Rows)
                {
                    if (Convert.ToString(dR["UAPFileNo"]) == "")
                    {
                        gblFuction.AjxMsgPopup("UAPFileNo cannot be left blank.");
                        vMsg = false;
                        break;
                    }
                    else if (Convert.ToString(dR["NameofEntrepreneur"]) == "")
                    {
                        gblFuction.AjxMsgPopup("Name of Entrepreneur cannot be left blank.");
                        vMsg = false;
                        break;
                    }
                    else if (Convert.ToString(dR["DACustomerID"]) == "")
                    {
                        gblFuction.AjxMsgPopup("DACustomerID cannot be left blank.");
                        vMsg = false;
                        break;
                    }
                    else if (Convert.ToString(dR["URNNo"]) == "")
                    {
                        gblFuction.AjxMsgPopup("URNNo cannot be left blank.");
                        vMsg = false;
                        break;
                    }
                    else if (Convert.ToString(dR["UAPCustomerID"]) == "")
                    {
                        gblFuction.AjxMsgPopup("UAP Customer ID cannot be left blank.");
                        vMsg = false;
                        break;
                    }
                    else if (Convert.ToString(dR["URNGenerationDate"]) == "")
                    {
                        gblFuction.AjxMsgPopup("URN Generation Date cannot be left blank.");
                        vMsg = false;
                        break;
                    }
                    //else if (Convert.ToString(dR["NICCode"]) == "")
                    //{
                    //    gblFuction.AjxMsgPopup("NICCode cannot be left blank.");
                    //    vMsg = false;
                    //    break;
                    //}
                    else if (Convert.ToString(dR["ProjectType"]) == "")
                    {
                        gblFuction.AjxMsgPopup("ProjectType cannot be left blank.");
                        vMsg = false;
                        break;
                    }
                    else
                    {
                        vMsg = true;
                    }
                }
                if (vMsg == true)
                {
                    vSaveMsg = SaveRecord(dt);
                    if (vSaveMsg != "0" && vSaveMsg == "")
                    {
                        FileInfo vfile = new FileInfo(vExcelPath + vFile);
                        if (vfile.Exists)
                        {
                            vfile.Delete();
                        }
                        gblFuction.AjxMsgPopup("Successfully Updated ..");
                    }
                    //else if (vSaveMsg != "")
                    //{
                    //    FileInfo vfile = new FileInfo(vExcelPath + vFile);
                    //    if (vfile.Exists)
                    //    {
                    //        vfile.Delete();
                    //    }
                    //    gblFuction.AjxMsgPopup("Successfully Updated ..But LoanId " + SaveRecord(dt) + " are not Updated");
                    //}
                }
            }
        }

        private string SaveRecord(DataTable dt)
        {
            string vXmlA = "";
            string vMsg = "";
            CNpsMember oAv = null;

            foreach (DataColumn dc in dt.Columns)
            {
                if (object.ReferenceEquals(dc.DataType, typeof(DateTime)))
                {
                    dc.DateTimeMode = DataSetDateTime.Unspecified;
                }
            }
            dt.AcceptChanges();
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vXmlA = oSW.ToString();

            }
            oAv = new CNpsMember();
            vMsg = oAv.UdyamAadharUpdate(vXmlA.Replace("_x0020_", "").Trim().Replace("'", ""), gblFuction.setDate(Session[gblValue.LoginDate].ToString()), Convert.ToInt32(Session[gblValue.UserId]));
            return vMsg;
        }

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            Int32 vRptUdyamDownload = Convert.ToInt32(Session[gblValue.RptUdyamDownload].ToString());
            if (vRptUdyamDownload != 0)
            {
                Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                if (unixTicks - vRptUdyamDownload > 300)
                {
                    Session[gblValue.RptUdyamDownload] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    Export("E");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Already Report Generate Request Is Executing ...Please Wait For 5 Mins..And Re Generate..");
                }
            }
            else
            {
                Session[gblValue.RptUdyamDownload] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                Export("E");
            }
        }

        protected void btnCsv_Click(object sender, EventArgs e)
        {
            Int32 vRptUdyamDownload = Convert.ToInt32(Session[gblValue.RptUdyamDownload].ToString());
            if (vRptUdyamDownload != 0)
            {
                Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                if (unixTicks - vRptUdyamDownload > 300)
                {
                    Session[gblValue.RptUdyamDownload] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    Export("C");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Already Report Generate Request Is Executing ...Please Wait For 5 Mins..And Re Generate..");
                }
            }
            else
            {
                Session[gblValue.RptUdyamDownload] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                Export("C");
            }
        }

        private void Export(string vCalFrom)
        {
            string vBranch = Session[gblValue.BrName].ToString(), vMsg = "";
            DateTime vDateTo = gblFuction.setDate(txtDtto.Text);
            DataTable dt = null;
            CApiCalling oAPI = new CApiCalling();

            var req = new UdyamReportRequest()
            {
                pDateto = txtDtto.Text,
                pProjectType = ddlProjectType.SelectedValue,
                pReportType = rdbSel.SelectedValue,
                pFormat = vCalFrom == "E" ? "Excel" : "CSV",
                pUserId = Convert.ToString(Session[gblValue.UserId]),
                pDBName = vDBName,
                pServerIP = vSrvName,
                pPassword = vPw,
                pCompanyName = gblValue.CompName,
                pCompanyAddress = CGblIdGenerator.GetBranchAddress1("0000")
            };
            string Requestdata = JsonConvert.SerializeObject(req);            
            vMsg = oAPI.GenerateReport("GenerateUNITYUdyamDownload", Requestdata, vReportUrl);
            gblFuction.AjxMsgPopup(vMsg);
            btnCsv.Enabled = false;
            btnExcl.Enabled = false;
        }
    }

    public class UdyamReportRequest
    {
        [DataMember]
        public string pDateto { get; set; }
        [DataMember]
        public string pProjectType { get; set; }
        [DataMember]
        public string pReportType { get; set; }
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