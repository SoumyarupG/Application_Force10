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
    public partial class LoanRecovryBulk : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["dtCollection"] = null;
                txtRecoveryDt.Text = Session[gblValue.LoginDate].ToString();
                dvExcelColl.Visible = true;
                LoadLedger();
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Installment Collection Bulk";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanRecoveryBulk);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanAdd == "N" || this.CanAdd == null || this.CanAdd == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Installment Collection Bulk", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void LoadLedger()
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            string vBranch = Session[gblValue.BrnchCode].ToString();
            oVoucher = new CVoucher();
            dt = oVoucher.GetAcGenLedCB(vBranch, rblLedType.SelectedValue, "");
            ddlLedger.DataSource = dt;
            ddlLedger.DataTextField = "Desc";
            ddlLedger.DataValueField = "DescId";
            ddlLedger.DataBind();
            ListItem liSel = new ListItem("<--- Select --->", "-1");
            ddlLedger.Items.Insert(0, liSel);
            if (rblLedType.SelectedValue == "C")
            {
                ddlLedger.SelectedIndex = ddlLedger.Items.IndexOf(ddlLedger.Items.FindByValue("C0001"));
                ddlLedger.Enabled = false;
            }
            else
            {
                ddlLedger.Enabled = true;
            }
        }

        protected void rblLedType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLedger();
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
            Int32  vCreatedBy = 0;
            string vXml = "",vErrDesc="";
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

            //  btnSave.Enabled = false;
            //------------------XML----------------------------
            dt = (DataTable)ViewState["dtCollection"];
            dt.TableName = "Table1";
            foreach (DataColumn c in dt.Columns)
            {
                c.ColumnName = String.Join("", c.ColumnName.Split());
            }
            if (dt.Rows.Count > 0)
            {
                vXml = GetXml(dt);
            }

            if (this.RoleId != 1)//&& vCreatedBy != 13 && vCreatedBy != 69 && vCreatedBy != 1511 && vCreatedBy != 505//CENTR - 7554 CENTR - 4103
            {
                if (Session[gblValue.EndDate] != null)
                {
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtRecoveryDt.Text))
                    {
                        gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                        return;
                    }
                }
            }

            try
            {
                oLR = new CLoanRecovery();
                vErrDesc = oLR.SaveCollectionBulk(vDate, vActMstTbl, vActDtlTbl, vFinYear, vXml, vCreatedBy, Convert.ToString(Session[gblValue.MultiColl]), "",ddlLedger.SelectedValue);
                if (vErrDesc.Trim() == "")
                {
                    gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
                    ViewState["dtCollection"] = null;
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