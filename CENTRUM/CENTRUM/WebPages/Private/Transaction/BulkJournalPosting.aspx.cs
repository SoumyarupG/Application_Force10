using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;
using System.Data.OleDb;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class BulkJournalPosting : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["dtPayment"] = null;
                txtPaymentDt.Text = Session[gblValue.LoginDate].ToString();
                dvExcelColl.Visible = true;
                
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Bulk Journal Posting (For Adjustment)";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBulkJournalPosting);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanAdd == "N" || this.CanAdd == null || this.CanAdd == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Bulk Journal Posting", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        



        private void Import_To_Grid(string FilePath, string Extension, string isHDR)
        {
            string conStr = "";
            String vXML = "";
            DataTable dtImport = null;
            CBulkJournalPosting AIM = null;
            DateTime vDate = gblFuction.setDate(txtPaymentDt.Text.Trim());
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

                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Table1";
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXML = oSW.ToString();
                    }
                }

                AIM = new CBulkJournalPosting();
                dtImport = AIM.GetBulkJournalPosting(vXML, vDate);

                gvCollection.DataSource = dtImport;
                gvCollection.DataBind();
                ViewState["dtPayment"] = dtImport;
            }
            catch (Exception ex)
            {
                throw ex;
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
                dvExcelColl.Visible = true;
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
            }
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vCreatedBy = 0;
            string vXml = "", vErrDesc = "";
            CBulkJournalPosting oLR = null;
            DateTime vDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = new DataTable();
            DataTable dtEod = new DataTable();
            CDayEnd oDE = null;
            DateTime LstEndDt = gblFuction.setDate("01/01/1900");

            string vActMstTbl = Session[gblValue.ACVouMst].ToString(),
               vActDtlTbl = Session[gblValue.ACVouDtl].ToString();
            DateTime FinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString()),
                FinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            string vFinYear = Session[gblValue.ShortYear].ToString();
            vCreatedBy = Convert.ToInt32(Session[gblValue.UserId]);

            if (gblFuction.setDate(txtPaymentDt.Text) != vDate)
            {
                gblFuction.AjxMsgPopup("Login Date and Recovery Date must be same.");
                return;
            }

            oDE = new CDayEnd();
            dtEod = oDE.GetLastEndDayDate(Session[gblValue.BrnchCode].ToString());

            if (dtEod.Rows.Count > 0)
            {
                LstEndDt = gblFuction.setDate(Convert.ToString(dtEod.Rows[0]["EndDate"]));
            }


            if (LstEndDt == gblFuction.setDate("01/01/1900"))
            {

                gblFuction.AjxMsgPopup("Day end not declared for this branch");
                return;
            }
            if (LstEndDt.AddDays(-1) >= gblFuction.setDate(txtPaymentDt.Text))
            {

                gblFuction.AjxMsgPopup("Day end already declared for this branch");
                return;
            }
            if (LstEndDt != gblFuction.setDate(txtPaymentDt.Text))
            {

                gblFuction.AjxMsgPopup("Day end pending for this branch");
                return;
            }

            if (Session[gblValue.EndDate] != null)
            {
                if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtPaymentDt.Text))
                {
                    gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                    return;
                }
            }

            //  btnSave.Enabled = false;
            //------------------XML----------------------------
            dt = (DataTable)ViewState["dtPayment"];
            dt.TableName = "Table1";
            foreach (DataColumn c in dt.Columns)
            {
                c.ColumnName = String.Join("", c.ColumnName.Split());
            }
            if (dt.Rows.Count > 0)
            {
                vXml = GetXml(dt);
            }



            try
            {
                oLR = new CBulkJournalPosting();
                vErrDesc = oLR.SaveBulkJournalPosting(vDate, vActMstTbl, vActDtlTbl, vFinYear, vXml, vCreatedBy);
                if (vErrDesc.Trim() == "")
                {
                    gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
                    ViewState["dtPayment"] = null;
                    gvCollection.DataSource = null;
                    gvCollection.DataBind();
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup(vErrDesc);
                    return;
                }

            }
            catch (Exception ex)
            {
                gblFuction.AjxMsgPopup(ex.ToString());
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

    }
}