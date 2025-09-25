using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;
using System.Data.OleDb;
using System.Configuration;
using CENTRUMSME;
using System.Web;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class BulkLendersEntry : CENTRUMBAse
    {
        protected int vPgNo = 1;
        private static string vExcelPath = ConfigurationManager.AppSettings["DBPath"];
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                popFunder();
                txtDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    //StatusButton("View");
                    btnImport.Enabled = true;
                }
                else
                {
                    //StatusButton("Exit");
                    btnImport.Enabled = false;
                }
                //popCB();          
            }
        }
        protected void popFunder()
        {
            DataTable dt = new DataTable();
            CReports OCR = new CReports();
            try
            {
                dt = OCR.FunderList();
                if (dt.Rows.Count > 0)
                {
                    ddlFunder.DataSource = dt;
                    ddlFunder.DataTextField = "FunderName";
                    ddlFunder.DataValueField = "FunderId";
                    ddlFunder.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlFunder.Items.Insert(0, oli);
                    //ListItem oli1 = new ListItem("All", "0");
                    //ddlFunder.Items.Add(oli1);
                }
                else
                {
                    ddlFunder.DataSource = null;
                    ddlFunder.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OCR = null;
                dt = null;
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Upload Lender Details";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBulkLenderEntry);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Upload Lender Details", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            String vFileName = "";
            String vFileType = "";
            int p = 0;
            String strExcelConn = "";
            String SheetName = "";
            String sXml = "";
            DataTable dt = null;
            DataSet ds = null;
            OleDbDataAdapter da = null;
            CMember oMem = new CMember();
            int b = 0;
            string vFunderId = ddlFunder.SelectedValue.ToString();

            if (fuDetail.HasFile)
            {
                if (!Directory.Exists(vExcelPath))
                {
                    Directory.CreateDirectory(vExcelPath);
                }
                vFileName = fuDetail.FileName;
                fuDetail.SaveAs(vExcelPath + vFileName);
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

                        OleDbConnection connExcel = new OleDbConnection(strExcelConn);
                        OleDbCommand cmdExcel = new OleDbCommand();
                        cmdExcel.Connection = connExcel;


                        connExcel.Open();
                        DataTable dtExcelSchema = new DataTable();
                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        da = new OleDbDataAdapter();
                        ds = new DataSet();
                        SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        try
                        {
                            cmdExcel.CommandText = "SELECT LoanNo as LoanNo ,SrNo as SrNo,DueDate as DueDate,Round(DisburseAmount,2) as DisburseAmount,Round(PrincipalRepaymentAmount,2) as PrincipalRepaymentAmount,Round(InterestRepaymentAmount,2) as InterestRepaymentAmount,Round(TotalMonthlyRepaymentAmount_EMI,2) as TotalMonthlyRepaymentAmount_EMI,Round(TDSAmount,2) as TDSAmount,Round(NetRepaymentAmt,2) as NetRepaymentAmt,Round(POSAmt,2) as POSAmt,Round(FLDG_OS_Amount,2) as FLDG_OS_Amount,Round(FLDGRefundAmount,2) as FLDGRefundAmount,Round(InterestFLDGAmount,2) as InterestFLDGAmount,Round(FLDG_TDS_Deduct,2) as FLDG_TDS_Deduct,Round(NetFLDGRefundAmount,2) as NetFLDGRefundAmount From [" + SheetName + "]" + "WHERE LoanNo IS NOT NULL";
                            da.SelectCommand = cmdExcel;
                            da.Fill(ds);
                        }
                        catch (Exception ex)
                        { 
                         gblFuction.MsgPopup("Check Head of Excel Sheet. Kindly follow same head as provided");
                         return;
                        }
                        dt = new DataTable();
                        dt = ds.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            using (StringWriter oSW = new StringWriter())
                            {
                                dt.WriteXml(oSW);
                                sXml = oSW.ToString().Replace("T00:00:00+05:30", "");
                            }
                        }
                        else
                        {
                            gblFuction.MsgPopup("Check Excell Sheet Properly.");
                            return;
                        }

                        connExcel.Close();

                        if (ddlFunder.SelectedIndex == 0)
                        {
                            gblFuction.MsgPopup("Please Select Lender to upload Excel...");
                            return;
                        }
                        if (txtDt.Text == "")
                        {
                            gblFuction.MsgPopup("Loan Date Can Not be Blank...");
                            return;
                        }
                        b = oMem.UploadLender(sXml, gblFuction.setDate(txtDt.Text.Trim()), Convert.ToInt32(vFunderId), 0, this.UserID);
                        if (b == 0)
                        {
                            gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                            ViewState["StateEdit"] = null;
                        }
                        else if (b == 3)
                        {
                            gblFuction.MsgPopup("Same Loan No already exist... Please Check..");
                        }
                        if (b == 1)
                        {
                            gblFuction.MsgPopup(gblPRATAM.DBError);
                        }

                        if (File.Exists(vExcelPath + vFileName))
                        {
                            File.Delete(vExcelPath + vFileName);

                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        dt = null;
                        ds = null;
                        da = null;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("Please Select EXCEL File..");
                }
            }
            else
                gblFuction.MsgPopup("Please Select File..");
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void lbLenderExcelFormat_Click(object sender, EventArgs e)
        {
            GetExcel();
        }
        protected void GetExcel()
        {
            DataTable dt = new DataTable();
            DataRow dr1;
            dt.Columns.Add("LoanNo", typeof(string));
            dt.Columns.Add("SrNo", typeof(string));
            dt.Columns.Add("DueDate", typeof(string));
            dt.Columns.Add("DisburseAmount", typeof(string));
            dt.Columns.Add("PrincipalRepaymentAmount", typeof(float));
            dt.Columns.Add("InterestRepaymentAmount", typeof(float));
            dt.Columns.Add("TotalMonthlyRepaymentAmount_EMI", typeof(float));
            dt.Columns.Add("TDSAmount", typeof(float));
            dt.Columns.Add("NetRepaymentAmt", typeof(float));
            dt.Columns.Add("POSAmt", typeof(string));
            dt.Columns.Add("FLDG_OS_Amount", typeof(float));
            dt.Columns.Add("FLDGRefundAmount", typeof(float));
            dt.Columns.Add("InterestFLDGAmount", typeof(float));
            dt.Columns.Add("FLDG_TDS_Deduct", typeof(float));
            dt.Columns.Add("NetFLDGRefundAmount", typeof(float));
            dr1 = dt.NewRow();
            dt.Rows.Add(dr1);

            string vFileNm = "";
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            vFileNm = "attachment;filename=" + "LenderExcelFormat.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";

            HttpContext.Current.Response.Write("<style> .txt" + "\r\n" + "{mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
            Response.Write("<table border='1' cellpadding='0' width='100%'>");
           // Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>Lender Excel Format</font></u></b></td></tr>");
            Response.Write("<tr>");

            foreach (DataColumn dtCol in dt.Columns)
            {
                Response.Write("<td><b>" + dtCol.ColumnName + "<b></td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in dt.Rows)
            {
                Response.Write("<tr style='height:20px;'>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName == "LoanNo")
                    {
                        Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                    }
                    else if (dt.Columns[j].ColumnName == "DueDate")
                    {
                        Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                    }
                    else
                    {
                        Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                    }
                }
            }
            Response.Write("</tr>");
            Response.Write("</table>");
            Response.Flush();
            Response.End();
        }
    }
}