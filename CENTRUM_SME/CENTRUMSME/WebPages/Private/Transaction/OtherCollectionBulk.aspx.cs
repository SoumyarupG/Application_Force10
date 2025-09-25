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
using Newtonsoft.Json;
using System.Configuration;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class OtherCollectionBulk : CENTRUMBAse
    {
        string vMobService = ConfigurationManager.AppSettings["MobService"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["dtCollection"] = null;
                txtRecoveryDt.Text = Session[gblValue.LoginDate].ToString();
                GetCashBankFrom();
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
                this.PageHeading = "Other Collection Bulk";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuOtherCollectionBulk);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanAdd == "N" || this.CanAdd == null || this.CanAdd == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Other Collection Bulk", false);
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
                dvExcelColl.Visible = true;
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

                gvCollection.DataSource = dt;
                gvCollection.DataBind();
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

        private void GetCashBankFrom()
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            string vBrCode = "";
            try
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oVoucher = new CVoucher();
                dt = oVoucher.GetAcGenLedCB(vBrCode, "A", "");
                ddlPayTo.DataSource = dt;
                ddlPayTo.DataTextField = "Desc";
                ddlPayTo.DataValueField = "DescId";
                ddlPayTo.DataBind();
                ListItem Li = new ListItem("<-- Select -->", "-1");
                ddlPayTo.Items.Insert(0, Li);
            }
            finally
            {
                oVoucher = null;
                dt = null;
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
            Int32 vErr = 0, vCreatedBy = 0;
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

            if (gblFuction.setDate(txtRecoveryDt.Text) != vDate)
            {
                gblFuction.AjxMsgPopup("Login Date and Recovery Date must be same.");
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
                if (Convert.ToInt32(Session[gblValue.RoleId]) != 1)
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= vDate)
                        {
                            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                            return ;
                        }
                    }
                }

                var req = new InsertBulkCollection()
                {
                    pAccDate = Convert.ToString(Session[gblValue.LoginDate]),
                    pTblMst = vActMstTbl,
                    pTblDtl = vActDtlTbl,
                    pFinYear = vFinYear,
                    pBankLedgr = ddlPayTo.SelectedValue,
                    pCollXml = vXml,                   
                    pCreatedBy = vCreatedBy.ToString(),
                };

                string Requestdata = JsonConvert.SerializeObject(req);
                GenerateReport("SaveOtherCollectionBulk", Requestdata, vMobService);
                ViewState["dtCollection"] = null;
                gvCollection.DataSource = null;
                gvCollection.DataBind();

                //oLR = new CLoanRecovery();
                //vErr = oLR.SaveOtherCollectionBulk(vDate, vActMstTbl, vActDtlTbl, vFinYear, ddlPayTo.SelectedValue, vXml, vCreatedBy);
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
                gblFuction.AjxMsgPopup("Collection Upload process is running on background.");
            }
        }

    }
}