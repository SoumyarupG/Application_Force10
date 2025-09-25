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
using System.Web.Security;
using System.Data.OleDb;
using ClosedXML.Excel;
using System.Configuration;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class CollectionBounceBulk : CENTRUMBAse
    {
        private static string vExcelPath = ConfigurationManager.AppSettings["DBPath"];
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ClearControls();
                PopBranch();
                ViewState["StateEdit"] = null;
                txtRecovryDt.Text = (string)Session[gblValue.LoginDate];
                txtBounceDate.Text = (string)Session[gblValue.LoginDate];
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Loan Collection Bounce Bulk";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCollBounceBulk);
                if (this.UserID == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnImport.Visible = false;
                    //btnDone.Visible = false;
                    //btnDel.Visible = false;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnImport.Visible = true;
                    // btnDone.Visible = true;
                    //btnDel.Visible = false;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnImport.Visible = true;
                    // btnDone.Visible = true;
                    //btnDel.Visible = false;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnImport.Visible = true;
                    // btnDone.Visible = true;
                    //btnDel.Visible = true;
                    return;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Collection Bounce Bulk", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }

        }
        private void ClearControls()
        {
            
           
           
        }
        private void PopBranch()
        {
            ddlBranch.Items.Clear();
            CMember oCM = new CMember();
            DataTable dt = new DataTable(); ;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                dt = oCM.GetBranchByBrCode(vBrCode);
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataBind();
                    if (vBrCode == "0000")
                    {
                        ListItem oItm = new ListItem("All", "A");
                        ddlBranch.Items.Insert(0, oItm);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCM = null;
            }
        }
        public static string GetCurrentFinancialYear(DateTime dt)
        {
            int CurrentYear = dt.Year;
            int PreviousYear = dt.Year - 1;
            int NextYear = dt.Year + 1;
            string PreYear = PreviousYear.ToString();
            string NexYear = NextYear.ToString();
            string CurYear = CurrentYear.ToString();
            string FinYear = null;

            if (dt.Month > 3)
                FinYear = CurYear + "-" + NexYear;
            else
                FinYear = PreYear + "-" + CurYear;
            return FinYear.Trim();
        }
        protected void btnDownLoad_Click(object sender, EventArgs e)
        {
            Session["dtRst"] = null;
           
            DataTable dt = new DataTable();
            CLoanRecovery oLR = new CLoanRecovery();
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());

            if (gblFuction.setDate(txtBounceDate.Text) < vFinFromDt || gblFuction.setDate(txtBounceDate.Text) > vFinToDt)
            {
                gblFuction.AjxMsgPopup("Bounce Date should be login financial year.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtRecovryDt");
                return;
            }
            try
            {
                string vBrCode = ddlBranch.SelectedValue;
                DateTime vRecvDt = gblFuction.setDate(txtRecovryDt.Text);
                DateTime vBounceDt = gblFuction.setDate(txtBounceDate.Text);
                dt = oLR.GetCollDtlForBounceByAccDate(vRecvDt, vBrCode, vBounceDt);
                if (dt.Rows.Count > 0)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("Sheet1");
                        ws.Cell(1, 1).Style.Font.Bold = true;
                        ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(1, 1).InsertTable(dt); //insert datatable
                        ws.Table("Table1").ShowAutoFilter = false; //remove default filter
                        ws.SheetView.FreezeRows(1); //freeze rows
                        ws.Columns().AdjustToContents(); //adjust column according to data
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "_BounceData.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found");
                    Session["dtRst"] = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oLR = null;
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (ddlBranch.SelectedValue == "-1")
            {
                gblFuction.MsgPopup("Please Select  Branch");
                ddlBranch.Focus();
                return;
            }

            int p = 0, b = 0;
            String vFileName = "", vFileType = "", strExcelConn = "", SheetName = "", vErrReason = "", vCollXml = "",
                vBrnch = "",vErrDesc = "";
            DateTime vChqDt = gblFuction.setDate("01/01/1900");
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            OleDbDataAdapter da = null;
            CLoanRecovery oLR = new CLoanRecovery();
            bool isExcelConOpen = false;
            OleDbConnection connExcel = null;

            vBrnch = ddlBranch.SelectedValue;

            string vActMstTbl = Session[gblValue.ACVouMst].ToString(),
               vActDtlTbl = Session[gblValue.ACVouDtl].ToString();
            DateTime FinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString()),
                FinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            string vFinYear = Session[gblValue.ShortYear].ToString();
            DateTime vAccDate = gblFuction.setDate(txtRecovryDt.Text);
            if ((vAccDate < FinFrom) || (vAccDate > FinTo))
            {
                gblFuction.AjxMsgPopup("Loan Recovery Date must be with in Login Financial Year");
                return;
            }
            if (fuColImport.HasFile)
            {
                if (!Directory.Exists(vExcelPath))
                {
                    Directory.CreateDirectory(vExcelPath);
                }
                vFileName = fuColImport.FileName;
                fuColImport.SaveAs(vExcelPath + vFileName);
                p = vFileName.LastIndexOf(".");
                vFileType = vFileName.Substring(p + 1);

                if (vFileType.Equals("xls") || vFileType.Equals("xlsx"))
                {
                    try
                    {
                        if (vFileType.Equals("xlsx"))
                            strExcelConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + vExcelPath + vFileName + ";" + "Extended Properties='Excel 12.0;ReadOnly=false; HDR=Yes'";
                        if (vFileType.Equals("xls"))
                            strExcelConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + vExcelPath + vFileName + ";" + "Extended Properties='Excel 8.0;ReadOnly=false;HDR=Yes'";

                        connExcel = new OleDbConnection(strExcelConn);
                        OleDbCommand cmdExcel = new OleDbCommand();
                        cmdExcel.Connection = connExcel;

                        connExcel.Open();
                        isExcelConOpen = true;
                        DataTable dtExcelSchema = new DataTable();
                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        da = new OleDbDataAdapter();
                        ds = new DataSet();

                        var abc = dtExcelSchema.AsEnumerable().Where(r => ((string)r["TABLE_NAME"]) == "Sheet1$").Count();

                        if (Convert.ToInt32(abc) == 1)
                        {
                            SheetName = "Sheet1$";
                        }
                        else if (Convert.ToInt32(abc) == 0)
                        {
                            gblFuction.MsgPopup("Please provide Sheet Name(Sheet1) Properly");
                            return;
                        }
                        else
                        {
                            gblFuction.MsgPopup("No Data Found");
                            return;
                        }

                        cmdExcel.CommandText = "SELECT BranchCode,LoanId, CustId, CustName,LastCollDate,InstallmentNo,ODDays,BounceChargeApplicable "
                         + "From [" + SheetName + "]" + " WHERE LoanId IS NOT NULL AND CustId IS NOT NULL "
                         + " AND BounceChargeApplicable  IS NOT NULL ";

                        da.SelectCommand = cmdExcel;
                        da.Fill(ds);
                        dt = new DataTable();
                        dt = ds.Tables[0];
                        dt.TableName = "Table1";
                        if (dt.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                if (string.IsNullOrEmpty(dt.Rows[j]["InstallmentNo"].ToString()) == true)
                                {
                                    gblFuction.MsgPopup("Please Check Record InstallmentNo Can Not Be Empty For Loan Id " + dt.Rows[j]["LoanId"].ToString());
                                    return;
                                }
                            }

                            foreach (DataRow dataRow in dt.Rows)
                            {
                                for (int j = 0; j < dataRow.ItemArray.Length; j++)
                                {
                                    if (dataRow.ItemArray[j] == DBNull.Value)
                                        dataRow.SetField(j, string.Empty);
                                }
                            }
                            using (StringWriter oSW = new StringWriter())
                            {
                                dt.WriteXml(oSW);
                                vCollXml = oSW.ToString().Replace("T00:00:00+05:30", "");
                            }
                        }
                        else
                        {
                            gblFuction.MsgPopup("Check Excel Sheet Properly.");
                            return;
                        }

                        connExcel.Close();
                        isExcelConOpen = false;

                        #region Save Bounce Entry

                        b = oLR.SaveBounceBulkExecl(vAccDate, vActMstTbl, vActDtlTbl, vFinYear,  vCollXml,
                            Convert.ToInt32(Session[gblValue.UserId].ToString()), ref vErrDesc);

                        if (b == 0)
                        {
                            gblFuction.AjxMsgPopup("Record Save Successfully");
                        }
                        else
                        {
                            if (vErrDesc != "")
                            {
                                gblFuction.MsgPopup(vErrDesc);
                                return;
                            }
                            else
                            {
                                gblFuction.MsgPopup(gblPRATAM.DBError);
                                return;
                            }
                        }
                        #endregion
                    }
                    finally
                    {
                        if (isExcelConOpen == true)
                        {
                            connExcel.Close();
                            isExcelConOpen = false;
                        }
                        if (dt.Rows.Count != 0)
                        {
                            if (File.Exists(vExcelPath + vFileName))
                            {
                                File.Delete(vExcelPath + vFileName);

                            }
                        }
                        dt = null;
                        ds = null;
                        da = null;
                        oLR = null;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("Please Select EXCEL File..");
                }
            }
            else
                gblFuction.MsgPopup("Please Select File To Upload ..");
        }
    }
}