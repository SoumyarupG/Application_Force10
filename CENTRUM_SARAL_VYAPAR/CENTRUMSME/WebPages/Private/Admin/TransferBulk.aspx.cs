using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using System.IO;
using System.Data.OleDb;
using System.Data;
using CENTRUMBA;

namespace CENTRUM_SARALVYAPAR.WebPages.Private.Admin
{
    public partial class TransferBulk : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["dtCollection"] = null;

                txtTransferDt.Text = Session[gblValue.LoginDate].ToString();
                txtTransferDt.Enabled = false;
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Transfer Within Branch(Bulk)";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuSARALTransfrBulk);
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
                ViewState["dtTransfer"] = dt;
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
            Int32 vCreatedBy = 0, vErr = 0;
            string vXml = "";
            DateTime vDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DataTable dt = new DataTable();
            vCreatedBy = Convert.ToInt32(Session[gblValue.UserId]);
            if (gblFuction.setDate(txtTransferDt.Text) != vDate)
            {
                gblFuction.AjxMsgPopup("Login Date and Transfer Date must be same.");
                return;
            }
            //------------------XML----------------------------
            dt = (DataTable)ViewState["dtTransfer"];
            dt.TableName = "Transfer";
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
                CTransfer oTr = new CTransfer();
                vErr = oTr.TransferMemberBulk(gblFuction.setDate(txtTransferDt.Text), vBrCode, vCreatedBy, vXml, txtTransferReason.Text, "");
                if (vErr > 0)
                {
                    gblFuction.AjxMsgPopup("Transfer Successful");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Transfer Failed");
                }

            }
            catch (Exception ex)
            {
                gblFuction.AjxMsgPopup(ex.ToString());
            }
        }
    }
}