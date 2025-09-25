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
using Newtonsoft.Json;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class FundSourceUpload : CENTRUMBase
    {
        string vMobService = ConfigurationManager.AppSettings["MobService"];
        private static string vFile;
        private static string vExcelPath = ConfigurationManager.AppSettings["DBPath"];
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
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
                this.PageHeading = "Source of Fund Bulk Update - Maker";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuSFIDUpdate);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Source of Fund Bulk Update", false);
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
            string Msg = "";
            ds = ExcelToDS(vExcelPath + vFile);
            dt = ds.Tables[0];

            string vXmlA = "";
            
            

            if (dt.Rows.Count > 0)
            {

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

                Msg = ValidateExcel(vXmlA);

                if (Msg.Length > 0)
                {
                    gblFuction.AjxMsgPopup(Msg);
                }
                else
                {
                    SaveRecord(vXmlA);   
                }
            }
            else
                gblFuction.AjxMsgPopup("Data not available to update ..");
        }

        private void SaveRecord(string pXmlA)
        {
            int vCreatedBy = 0;
            vCreatedBy = Convert.ToInt32(Session[gblValue.UserId]);
            var req = new InsertBulkFunderUpload()
            {
                pFunderXml = pXmlA,
                pLoginDt = (string)Session[gblValue.LoginDate],
                pCreatedBy = vCreatedBy.ToString()
            };

            string Requestdata = JsonConvert.SerializeObject(req);
            GenerateReport("InsertFundSourceUpload", Requestdata, vMobService);
            ViewState["dtCollection"] = null;
            
        }

        private string ValidateExcel(string pXmlA)
        {            
            string vMsg = "";
            CNpsMember oAv = null;           
            oAv = new CNpsMember();
            vMsg = oAv.ValidateFundSourceData(pXmlA.Replace("_x0020_", "").Trim().Replace("'", ""));
            return vMsg;
        }

        public class InsertBulkFunderUpload
        {
            
            public string pFunderXml { get; set; }
            public string pLoginDt { get; set; }
            public string pCreatedBy { get; set; }
            
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
                gblFuction.AjxMsgPopup("Fund Source Upload process is running on background.");
            }
        }
    }
}