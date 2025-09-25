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
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class BulkUpload : CENTRUMBase
    {
        private static string vExcelPath = ConfigurationManager.AppSettings["DBPath"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDt.Text = Session[gblValue.LoginDate].ToString();
                //PopDtl(txtDt.Text.Trim());
                btnSave.Enabled = true;
                btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
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
                this.PageHeading = "Bulk Upload";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBlukUpload);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Bulk Upload", false);
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
        /// <param name="pDate"></param>
        private void PopDtl(string pDate)
        {
            CBranchFundTr oBT = null;
            DataTable dt = null;
            DateTime vDate = gblFuction.setDate(pDate);
            try
            {
                oBT = new CBranchFundTr();
                dt = oBT.GetBrFundTransferDetails(vDate, gblFuction.setDate(Session[gblValue.FinFromDt].ToString()), Convert.ToInt32(Session[gblValue.FinYrNo].ToString()), Convert.ToInt32(Session[gblValue.UserId].ToString()));
                dt.Columns.Add("BrDt");
                foreach (DataRow dr in dt.Rows)
                {
                    dr["BrDt"] = txtDt.Text.Trim();
                }
                gvDtl.DataSource = dt;
                gvDtl.DataBind();
                upDtl.Update();
            }
            catch
            {
                gvDtl.DataSource = null;
                gvDtl.DataBind();
            }
            finally
            {
                oBT = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CVoucher oVoucher = null;
            DataTable dtBrBank = null;
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlBrBank = (DropDownList)e.Row.FindControl("ddlBrBank");
                DropDownList ddlBranch = (DropDownList)e.Row.FindControl("ddlBranch");
                TextBox txtAmt = (TextBox)e.Row.FindControl("txtAmt");
                CheckBox chkTr = (CheckBox)e.Row.FindControl("chkTr");
                try
                {
                    oVoucher = new CVoucher();
                    ddlBrBank.Items.Clear();
                    dtBrBank = oVoucher.GetAcGenLedCB(e.Row.Cells[5].Text.Trim(), "J", "");
                    if (dtBrBank.Rows.Count > 0)
                    {
                        ddlBrBank.DataSource = dtBrBank;
                        ddlBrBank.DataTextField = "Desc";
                        ddlBrBank.DataValueField = "GenLedCode";
                        ddlBrBank.DataBind();
                    }
                    ListItem oLc = new ListItem("<--Select-->", "-1");
                    ddlBrBank.Items.Insert(0, oLc);
                    ddlBrBank.SelectedIndex = ddlBrBank.Items.IndexOf(ddlBrBank.Items.FindByValue(e.Row.Cells[7].Text.Trim()));
                    ddlBrBank.Enabled = false;

                    oGb = new CGblIdGenerator();
                    dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlBranch.Items.Insert(0, oli);
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(e.Row.Cells[5].Text.Trim()));
                    ddlBranch.Enabled = false;
                    txtAmt.Text = e.Row.Cells[6].Text.Trim();
                    txtAmt.Enabled = false;
                }
                finally
                {
                    oVoucher = null;
                    dtBrBank = null;
                    oGb = null;
                    dt = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String XmlList()
        {
            String vSpecXML = "";
            DataTable dt = new DataTable("List");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("BrBankID");
            dt.Columns.Add("Amount");
            dt.Columns.Add("IsChk");

            foreach (GridViewRow gR in gvDtl.Rows)
            {
                DropDownList ddlBrBank = (DropDownList)gR.FindControl("ddlBrBank");
                TextBox txtAmt = (TextBox)gR.FindControl("txtAmt");
                CheckBox chkTr = (CheckBox)gR.FindControl("chkTr");
                //if (chkTr.Checked == true)
                //{
                DataRow dr = dt.NewRow();
                dr["BranchCode"] = gR.Cells[5].Text;
                dr["BrBankID"] = ddlBrBank.SelectedValue.ToString();
                dr["Amount"] = txtAmt.Text.Trim();
                dr["IsChk"] = chkTr.Checked == true ? "Y" : "N";
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                //}
            }
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vSpecXML = oSW.ToString();
            }
            return vSpecXML;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtDt_TextChanged(object sender, EventArgs e)
        {
            PopDtl(txtDt.Text.Trim());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTr_CheckedChanged(object sender, EventArgs e)
        {
            CDayEnd oDE = null;
            DataTable dt = null;
            DateTime LstEndDt = gblFuction.setDate("01/01/1900");
            CheckBox chkTr = (CheckBox)sender;
            GridViewRow gR = (GridViewRow)chkTr.NamingContainer;
            DropDownList ddlBrBank = (DropDownList)gR.FindControl("ddlBrBank");
            DropDownList ddlBranch = (DropDownList)gR.FindControl("ddlBranch");
            TextBox txtAmt = (TextBox)gR.FindControl("txtAmt");
            TextBox txtBrDt = (TextBox)gR.FindControl("txtBrDt");
            oDE = new CDayEnd();
            dt = oDE.GetLastEndDayDate(Session[gblValue.BrnchCode].ToString());
            if (dt.Rows.Count > 0)
            {
                LstEndDt = gblFuction.setDate(Convert.ToString(dt.Rows[0]["EndDate"]));
            }

            if (ddlBranch.SelectedValue == "-1")
            {
                chkTr.Checked = false;
                gblFuction.AjxMsgPopup("Please Select Branch");
                return;
            }
            if (ddlBrBank.SelectedValue == "-1")
            {
                chkTr.Checked = false;
                gblFuction.AjxMsgPopup("Please Select Bank For Branch");
                return;
            }

            if (txtAmt.Text.ToString() == "" || txtAmt.Text.ToString() == "0")
            {
                chkTr.Checked = false;
                gblFuction.AjxMsgPopup("Please Select Amount");
                return;
            }
            if (this.RoleId != 1 && Convert.ToInt32(Session[gblValue.UserId]) != 69 && Convert.ToInt32(Session[gblValue.UserId]) != 70 && Convert.ToInt32(Session[gblValue.UserId]) != 1001)
            {
                if (LstEndDt == gblFuction.setDate("01/01/1900"))
                {
                    chkTr.Checked = false;
                    gblFuction.AjxMsgPopup("Day end not declared for this branch");
                    return;
                }
                if (LstEndDt.AddDays(-1) >= gblFuction.setDate(txtDt.Text))
                {
                    chkTr.Checked = false;
                    gblFuction.AjxMsgPopup("Day end already declared for this branch");
                    return;
                }
                if (LstEndDt != gblFuction.setDate(txtDt.Text))
                {
                    chkTr.Checked = false;
                    gblFuction.AjxMsgPopup("Day end pending for this branch");
                    return;
                }
            }

        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //protected Boolean Validations()
        //{
        //    return true;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected int SaveRecords()
        {
            int vRes = 0; string vXml = "";
            CBranchFundTr oBF = null;
            DateTime vDate = gblFuction.setDate(txtDt.Text.Trim());
            int vUserID = Convert.ToInt32(Session[gblValue.UserId]);
            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vTblDtl = Session[gblValue.ACVouDtl].ToString();
            String vFinYear = Session[gblValue.ShortYear].ToString();
            int vYearNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
            String vBranchCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                if (this.RoleId != 1)
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= vDate)
                        {
                            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                            return 1;
                        }
                    }
                }

                vXml = XmlList();
                oBF = new CBranchFundTr();
                vRes = oBF.SaveBulkUploadDetails(vXml, vDate, vTblMst, vTblDtl, vFinYear, vYearNo, vBranchCode, vUserID);
                return vRes;
            }
            catch
            {
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int vRes = 0;
            vRes = SaveRecords();
            gvDtl.DataSource = null;
            gvDtl.DataBind();
           // btnSave.Enabled = false;

            if (vRes == 0)
                gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
            else if (vRes == 1)
                gblFuction.AjxMsgPopup(gblMarg.DBError);
            else
                gblFuction.AjxMsgPopup("Validation Error !");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
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

            gvDtl.DataSource = dt;
            gvDtl.DataBind();

        }
    }
}