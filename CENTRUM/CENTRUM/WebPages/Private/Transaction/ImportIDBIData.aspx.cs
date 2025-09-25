using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class ImportIDBIData : CENTRUMBase
    {
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

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Import IDBI Data";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuImprtIDBIData);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Import IDBI Data", false);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null;
            ds = ExcelToDS(vExcelPath + vFile);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                if (SaveRecord(dt) != "0" && SaveRecord(dt) == "")
                {
                    FileInfo vfile = new FileInfo(vExcelPath + vFile);
                    if (vfile.Exists)
                    {
                        vfile.Delete();
                    }
                    gblFuction.AjxMsgPopup("IDBI Data Successfully Updated ..");
                }
                else if (SaveRecord(dt) != "")
                {
                    FileInfo vfile = new FileInfo(vExcelPath + vFile);
                    if (vfile.Exists)
                    {
                        vfile.Delete();
                    }
                    gblFuction.AjxMsgPopup("IDBI Data Successfully Updated ..But LoanApplicationId " + SaveRecord(dt) + " are not Updated");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
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
            vMsg = oAv.ImportRDBI(vXmlA.Replace("_x0020_", "").Trim().Replace("'", ""));
            return vMsg;
        }
    }
}