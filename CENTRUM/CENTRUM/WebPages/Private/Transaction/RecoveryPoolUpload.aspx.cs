using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using FORCECA;
using System.IO;
using System.Data;
using FORCEBA;
using Newtonsoft.Json;
using System.Data.OleDb;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class RecoveryPoolUpload : CENTRUMBase
    {
        string vMobService = ConfigurationManager.AppSettings["MobService"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["dtCollection"] = null;
               
                txtRecoveryDt.Text = Session[gblValue.LoginDate].ToString();
                txtRecoveryDt.Enabled = false;
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Recovery Pool Upload";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuRecoveryPoolUpload);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanAdd == "N" || this.CanAdd == null || this.CanAdd == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Recovery Pool Upload", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (fuCollUpdate.HasFile)
            {
                string FileName = Path.GetFileName(fuCollUpdate.PostedFile.FileName);
                string Extension = Path.GetExtension(fuCollUpdate.PostedFile.FileName);
                string FolderPath = "../../../Images/";
                string FilePath = Server.MapPath(FolderPath + FileName);
                fuCollUpdate.SaveAs(FilePath);
                Import_To_Grid(FilePath, Extension, "Yes");
                SaveRecords();
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
            }
            else
            {
                gblFuction.AjxMsgPopup("Please Import a file to save..!!");
                fuCollUpdate.Focus();
            }
        }

        private void Import_To_Grid(string FilePath, string Extension, string isHDR)
        {
            string conStr = "";
            try
            {
                switch (Extension)
                {
                    case ".xls": //Excel 97-03
                        conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
                        break;
                    case ".xlsx": //Excel 07
                        conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR={1}'";
                        break;
                }
                conStr = String.Format(conStr, FilePath, isHDR);
                OleDbConnection connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                DataTable dt = new DataTable();
                cmdExcel.Connection = connExcel;

                //Get the name of First Sheet
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                connExcel.Close();

                //Read Data from First Sheet
                connExcel.Open();
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();

                ViewState["dtCollection"] = dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }


        private string GetXml(DataTable dt)
        {
            string vXmlData = "";
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vXmlData = oSW.ToString();
            }
            return vXmlData;
        }


        protected void SaveRecords()
        {
            Int32  vCreatedBy = 0;
            string vXml = "";
            DateTime vDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = new DataTable();
            vCreatedBy = Convert.ToInt32(Session[gblValue.UserId]);

            if (gblFuction.setDate(txtRecoveryDt.Text) != vDate)
            {
                gblFuction.AjxMsgPopup("Login Date and Entry Date must be same.");
                return;
            }
            //------------------XML----------------------------
            dt = (DataTable)ViewState["dtCollection"];
            dt.TableName = "Collection";
            foreach (DataColumn c in dt.Columns)
            {
                c.ColumnName = String.Join("", c.ColumnName.Split());
            }
            if (dt.Rows.Count > 0)
            {
                vXml = GetXml(dt);
            }
            //--------------------------------------------           
            try
            {

                var req = new UploadRecoveryPool()
                {
                    pLogin = Convert.ToString(Session[gblValue.LoginDate]),
                    pCollXml = vXml,
                    pCreatedBy = vCreatedBy.ToString(),
                };

                string Requestdata = JsonConvert.SerializeObject(req);
                GenerateReport("RecoveryPoolUpload", Requestdata, vMobService);
                ViewState["dtCollection"] = null;
                return;

            }
            catch (Exception ex)
            {
                gblFuction.AjxMsgPopup(ex.ToString());
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
                gblFuction.AjxMsgPopup("Other Recovery Pool Upload process is running on background.");
            }
        }
    }
    public class UploadRecoveryPool
    {
        public string pLogin { get; set; }
        public string pCollXml { get; set; }
        public string pCreatedBy { get; set; }
    }

}