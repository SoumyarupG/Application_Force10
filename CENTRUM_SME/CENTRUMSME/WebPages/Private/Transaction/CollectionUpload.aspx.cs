using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Text;
using Newtonsoft.Json;
using System.Configuration;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class CollectionUpload : CENTRUMBAse
    {
        string vMobService = ConfigurationManager.AppSettings["MobService"];
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["dtCollection"] = null;
                txtRecoveryDt.Text = Session[gblValue.LoginDate].ToString();
                GetCashBank();
                dvExcelColl.Visible = true;
                dvCSVColl.Visible = false;
                txtTotalAmount.Text = "0";
                txtTotalLoan.Text = "0";
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Collection Upload";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCollectionUpload);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanAdd == "N" || this.CanAdd == null || this.CanAdd == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Collection", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (fuCollUpdate.HasFile)
            {
                string FileName = Path.GetFileName(fuCollUpdate.PostedFile.FileName);
                string Extension = Path.GetExtension(fuCollUpdate.PostedFile.FileName);
                string FolderPath = "../../../Images/";
                string FilePath = Server.MapPath(FolderPath + FileName);
                fuCollUpdate.SaveAs(FilePath);

                if (Extension.ToUpper() == ".CSV")
                {
                    DataTable dt = new DataTable();
                    dt = ConvertCSVtoDataTable(FilePath);
                    gvCollectionCSV.DataSource = dt;
                    gvCollectionCSV.DataBind();
                    ViewState["dtCollection"] = dt;
                    dvExcelColl.Visible = false;
                    dvCSVColl.Visible = true;
                }
                else
                {
                    Import_To_Grid(FilePath, Extension, "Yes");
                    dvExcelColl.Visible = true;
                    dvCSVColl.Visible = false;
                }

                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
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
                int vTotalRows = dt.Rows.Count;
                if (vTotalRows <= 100)
                {
                    gvCollection.DataSource = dt;
                    gvCollection.DataBind();
                }
                if (vTotalRows > 0)
                {
                    txtTotalLoan.Text = Convert.ToString(dt.Rows.Count);
                    decimal vTotalAmount = Convert.ToDecimal(dt.Compute("SUM(Amount)", string.Empty));
                    txtTotalAmount.Text = Convert.ToString(vTotalAmount);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
            Int32 vErr = 0, vCreatedBy = 0, vRoleId = 0;
            string vXml = "";
            CLoanRecovery oLR = null;
            DateTime vDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = new DataTable();
            string vActMstTbl = Session[gblValue.ACVouMst].ToString(),
               vActDtlTbl = Session[gblValue.ACVouDtl].ToString();
            DateTime FinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString()),
                FinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            string vFinYear = Session[gblValue.ShortYear].ToString();
            vCreatedBy = Convert.ToInt32(Session[gblValue.UserId]);
            vRoleId = Convert.ToInt32(Session[gblValue.RoleId]);
            if (gblFuction.setDate(txtRecoveryDt.Text) != vDate)
            {
                gblFuction.AjxMsgPopup("Login Date and Recovery Date must be same.");
                return;
            }

            //  btnSave.Enabled = false;
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
            int vErr1 = 0;
            string vMsg = "";
            if (vCreatedBy != 1) //&& vCreatedBy != 13 && vCreatedBy != 69 && vCreatedBy != 1511 && vCreatedBy != 505 && vRoleId != 5
            {
                oLR = new CLoanRecovery();
                vErr1 = oLR.ChkDayEnd(vDate, vXml, ddlBank.SelectedValue, ref vMsg);
                if (vErr1 > 0)
                {
                    gblFuction.AjxMsgPopup(vMsg);
                    return;
                }
            }
            try
            {
                oLR = new CLoanRecovery();
                if (ddlFormat.SelectedValue == "E")
                {
                    var req = new InsertBulkCollection()
                    {
                        pAccDate = Convert.ToString(Session[gblValue.LoginDate]),
                        pTblMst = vActMstTbl,
                        pTblDtl = vActDtlTbl,
                        pFinYear = vFinYear,
                        pBankLedgr = ddlBank.SelectedValue,
                        pCollXml = vXml,
                        pBrachCode = vBrCode,
                        pCreatedBy = vCreatedBy.ToString(),

                    };

                    string Requestdata = JsonConvert.SerializeObject(req);
                    GenerateReport("InsertBulkCollection", Requestdata, vMobService);
                    ViewState["dtCollection"] = null;
                    gvCollection.DataSource = null;
                    gvCollection.DataBind();

                    //vErr = oLR.InsertBulkCollection(vDate, vActMstTbl, vActDtlTbl, vFinYear, ddlBank.SelectedValue, vXml, vBrCode, vCreatedBy);
                    //if (vErr == 0)
                    //{
                    //    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    //    ViewState["dtCollection"] = null;
                    //    gvCollection.DataSource = null;
                    //    gvCollection.DataBind();
                    //    return;
                    //}
                    //else
                    //{
                    //    gblFuction.MsgPopup(gblPRATAM.DBError);
                    //    return;
                    //}
                }
                else
                {
                //    string vErrMsg = oLR.chkFundFinaColl(vDate, vXml);
                //    if (vErrMsg.Trim() == "")
                //    {
                //        oLR = new CLoanRecovery();
                //        vErr = oLR.InsertBulkCollectionFundFina(vDate, vActMstTbl, vActDtlTbl, vFinYear, ddlBank.SelectedValue, vXml, vBrCode, vCreatedBy);
                //        if (vErr == 0)
                //        {
                //            gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                //            ViewState["dtCollection"] = null;
                //            gvCollection.DataSource = null;
                //            gvCollection.DataBind();
                //            return;
                //        }
                //        else
                //        {
                //            gblFuction.MsgPopup(gblPRATAM.DBError);
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        gblFuction.AjxMsgPopup(vErrMsg.Trim());
                //        return;
                //    }
                    gblFuction.AjxMsgPopup("Not Allowed");
                    return;
                }                
                txtTotalLoan.Text = "0";
                txtTotalAmount.Text = "0";
            }
            catch (Exception ex)
            {
                gblFuction.AjxMsgPopup(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            CLoanRecovery oLR = new CLoanRecovery();
            string pBranchCode = Session[gblValue.BrnchCode].ToString();
            dt = oLR.GetNachCollDtlByDate(gblFuction.setDate(txtRecoveryDt.Text), pBranchCode, ddlFormat.SelectedValue);
            //if (ddlFormat.SelectedValue == "E")
            //{
            ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
            wbook.Worksheets.Add(dt, txtRecoveryDt.Text.Replace("/", "") + "_Collection");
            // Prepare the response
            HttpResponse httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Provide you file name here
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"Nach_Collection.xlsx\"");

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();
            //}
            //else if (ddlFormat.SelectedValue == "C")
            //{                
            //    string csv = string.Empty;
            //    foreach (DataColumn column in dt.Columns)
            //    {
            //        //Add the Header row for CSV file.
            //        csv += column.ColumnName + ',';
            //    }
            //    //Add new line.
            //    csv += "\r\n";
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        foreach (DataColumn column in dt.Columns)
            //        {
            //            //Add the Data rows.
            //            csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
            //        }

            //        //Add new line.
            //        csv += "\r\n";
            //    }
            //    //Download the CSV file.
            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.AddHeader("content-disposition", "attachment;filename=Nach_Collection.csv");
            //    Response.Charset = "";
            //    Response.ContentType = "application/text";
            //    Response.Output.Write(csv);
            //    Response.Flush();
            //    Response.End();
            //}
        }

        private void GetCashBank()
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oVoucher = new CVoucher();
                dt = oVoucher.GetAcGenLedCB(vBrCode, "A", "");
                ddlBank.DataSource = dt;
                ddlBank.DataTextField = "Desc";
                ddlBank.DataValueField = "DescId";
                ddlBank.DataBind();
                ListItem Li = new ListItem("<-- Select -->", "-1");
                ddlBank.Items.Insert(0, Li);
            }
            finally
            {
                oVoucher = null;
                dt = null;
            }
        }

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }
            return dt;
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
                gblFuction.AjxMsgPopup("Collection Upload process is running on background.");
            }
        }
    }

    public class InsertBulkCollection
    {
        public string pAccDate { get; set; }
        public string pTblMst { get; set; }
        public string pTblDtl { get; set; }
        public string pFinYear { get; set; }
        public string pBankLedgr { get; set; }
        public string pCollXml { get; set; }
        public string pBrachCode { get; set; }
        public string pCreatedBy { get; set; }
    }

}