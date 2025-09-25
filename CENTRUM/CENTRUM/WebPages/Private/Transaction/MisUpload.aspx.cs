using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Configuration;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class MisUpload : CENTRUMBase
    {
        private static string vExcelPath = ConfigurationManager.AppSettings["DBPath"];
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "MIS Upload";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuMisUpload);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnSave.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "MIS Upload", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (fuAtt.HasFile)
            {
                string FileName = Path.GetFileName(fuAtt.PostedFile.FileName);
                string Extension = Path.GetExtension(fuAtt.PostedFile.FileName);
                if (!Directory.Exists(vExcelPath))
                {
                    Directory.CreateDirectory(vExcelPath);
                }
                fuAtt.SaveAs(vExcelPath + FileName);
                Import_To_Grid(vExcelPath + FileName, Extension, "Yes");
                if (File.Exists(vExcelPath + FileName))
                {
                    File.Delete(vExcelPath + FileName);
                }
            }
        }

        private void Import_To_Grid(string FilePath, string Extension, string isHDR)
        {
            string conStr = "";
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


            gvMIS.DataSource = dt;
            gvMIS.DataBind();
            // ViewState["dtAttendance"] = dt;
        }

        private String XmlFromGrid()
        {
            Int32 i = 0;
            String vXML = "";
            DataTable dt = new DataTable("Table1");
            dt.Columns.Add("TDate");
            dt.Columns.Add("Cdate");
            dt.Columns.Add("CollectionPoint");
            dt.Columns.Add("Amount");
            dt.Columns.Add("EmpCode");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("TransactionId");
            foreach (GridViewRow gr in gvMIS.Rows)
            {
                DataRow dr = dt.NewRow();
                Label lblTranDt = (Label)gr.FindControl("lblTranDt");
                dr["TDate"] = gblFuction.setDate(lblTranDt.Text);
                Label lblCollDt = (Label)gr.FindControl("lblCollDt");
                dr["Cdate"] = gblFuction.setDate(lblCollDt.Text);
                dr["CollectionPoint"] = gr.Cells[2].Text;
                dr["Amount"] = gr.Cells[3].Text; ;
                dr["EmpCode"] = gr.Cells[5].Text;
                dr["BranchCode"] = gr.Cells[6].Text;
                dr["TransactionId"] = gr.Cells[7].Text;
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                i++;
            }
            if (dt.Rows.Count > 0)
            {
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXML = oSW.ToString();
                }
            }
            return vXML;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                gvMIS.DataSource = null;
                gvMIS.DataBind();
                ViewState["StateEdit"] = null;
            }

        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;   
            CCollectionPoint oQua = null;
            string vXml = XmlFromGrid();
            Int32 vErr = 0;
            try
            {
                oQua = new CCollectionPoint();
                vErr = oQua.UploadMisData(vXml, Convert.ToInt32(Session[gblValue.UserId]));
                if (vErr > 0)
                {
                    vResult = true;
                }
                else
                {
                    gblFuction.MsgPopup(gblMarg.DBError);
                    vResult = false;
                }
                return vResult;
            }
            finally
            {
                oQua = null;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        decimal sumFooterValue = 0;
        protected void gvMIS_RowDataBound(object sender, GridViewRowEventArgs e)
        {            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblAmt = (Label)e.Row.FindControl("lblAmt");
                sumFooterValue += Convert.ToDecimal(lblAmt.Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lbl = (Label)e.Row.FindControl("lblTotal");
                lbl.Text ="Total-"+ sumFooterValue.ToString();
            }
        }
    }
}