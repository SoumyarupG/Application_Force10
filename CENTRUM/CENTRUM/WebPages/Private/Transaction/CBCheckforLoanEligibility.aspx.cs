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
using CENTRUM.Service_Equifax_CCR;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class CBCheckforLoanEligibility : CENTRUMBase
    {
        string CCRUserName = ConfigurationManager.AppSettings["CCRUserName"];
        string CCRPassword = ConfigurationManager.AppSettings["CCRPassword"];
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
                this.PageHeading = "CB Check for Loan Eligibility";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCBCheckForLnEligibility);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "CB Check for Loan Eligibility", false);
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

            dt.Columns.Add("Loan Account Number");
            dt.Columns.Add("Custoner ID");
            dt.Columns.Add("Applicant Name");
            dt.Columns.Add("AppDOB");
            dt.Columns.Add("Applicant Mobile Number");
            dt.Columns.Add("Applicant Gender");
            dt.Columns.Add("Applicant KYC Type 1");
            dt.Columns.Add("Applicant KYC ID Number 1");
            dt.Columns.Add("Applicant KYC Type 2");
            dt.Columns.Add("Applicant KYC ID Number 2");
            dt.Columns.Add("Applicant Address");
            dt.Columns.Add("Applicant City");
            dt.Columns.Add("Applicant District");
            dt.Columns.Add("Applicant Village");
            dt.Columns.Add("Applicant State");
            dt.Columns.Add("Applicant Pincode");
            dt.Columns.Add("Co-Applicant Name");
            dt.Columns.Add("COAPPDOB");
            dt.Columns.Add("Co-Applicant KYC Type 1");
            dt.Columns.Add("Co-Applicant KYC ID Number 1");
            dt.Columns.Add("Co-Applicant Address");
            dt.Columns.Add("Co-Applicant City");
            dt.Columns.Add("Co-Applicant District");
            dt.Columns.Add("Co-Applicant Village");
            dt.Columns.Add("Co-Applicant State");
            dt.Columns.Add("Co-Applicant Pincode");
            foreach (GridViewRow gr in gvMIS.Rows)
            {
                DataRow dr = dt.NewRow();

                dr["Loan Account Number"] = gr.Cells[0].Text;
                dr["Custoner ID"] = gr.Cells[1].Text;
                dr["Applicant Name"] = gr.Cells[2].Text;
                Label lblAppDOB = (Label)gr.FindControl("lblAppDOB");
                dr["AppDOB"] = gblFuction.setDate(lblAppDOB.Text);
                dr["Applicant Mobile Number"] = gr.Cells[4].Text;
                dr["Applicant Gender"] = gr.Cells[5].Text;
                dr["Applicant KYC Type 1"] = gr.Cells[6].Text;
                dr["Applicant KYC ID Number 1"] = gr.Cells[7].Text;
                dr["Applicant KYC Type 2"] = gr.Cells[8].Text;
                dr["Applicant KYC ID Number 2"] = gr.Cells[9].Text;
                dr["Applicant Address"] = gr.Cells[10].Text;
                dr["Applicant City"] = gr.Cells[11].Text;
                dr["Applicant District"] = gr.Cells[12].Text;
                dr["Applicant Village"] = gr.Cells[13].Text;
                dr["Applicant State"] = gr.Cells[14].Text;
                dr["Applicant Pincode"] = gr.Cells[15].Text;
                dr["Co-Applicant Name"] = gr.Cells[16].Text;
                Label lblCoAppDob = (Label)gr.FindControl("lblCoAppDob");
                dr["COAPPDOB"] = gblFuction.setDate(lblCoAppDob.Text);
                dr["Co-Applicant KYC Type 1"] = gr.Cells[18].Text;
                dr["Co-Applicant KYC ID Number 1"] = gr.Cells[19].Text;
                dr["Co-Applicant Address"] = gr.Cells[20].Text;
                dr["Co-Applicant City"] = gr.Cells[21].Text;
                dr["Co-Applicant District"] = gr.Cells[22].Text;
                dr["Co-Applicant Village"] = gr.Cells[23].Text;
                dr["Co-Applicant State"] = gr.Cells[24].Text;
                dr["Co-Applicant Pincode"] = gr.Cells[25].Text;
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
            CReLoanCB oRLC = null;
            string pEqXml = "", pStatusDesc = "";
            Int32 i = 0;
            int pCreatedBy = Convert.ToInt32(Session[gblValue.UserId]);
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = new DataTable("Table1");
            WebServiceSoapClient eq = null;

            dt.Columns.Add("Loan Account Number");
            dt.Columns.Add("Custoner ID");
            dt.Columns.Add("Applicant Name");
            dt.Columns.Add("AppDOB");
            dt.Columns.Add("Applicant Mobile Number");
            dt.Columns.Add("Applicant Gender");
            dt.Columns.Add("Applicant KYC Type 1");
            dt.Columns.Add("Applicant KYC ID Number 1");
            dt.Columns.Add("Applicant KYC Type 2");
            dt.Columns.Add("Applicant KYC ID Number 2");
            dt.Columns.Add("Applicant Address");
            dt.Columns.Add("Applicant City");
            dt.Columns.Add("Applicant District");
            dt.Columns.Add("Applicant Village");
            dt.Columns.Add("Applicant State");
            dt.Columns.Add("Applicant Pincode");
            dt.Columns.Add("Co-Applicant Name");
            dt.Columns.Add("COAPPDOB");
            dt.Columns.Add("Co-Applicant KYC Type 1");
            dt.Columns.Add("Co-Applicant KYC ID Number 1");
            dt.Columns.Add("Co-Applicant Address");
            dt.Columns.Add("Co-Applicant City");
            dt.Columns.Add("Co-Applicant District");
            dt.Columns.Add("Co-Applicant Village");
            dt.Columns.Add("Co-Applicant State");
            dt.Columns.Add("Co-Applicant Pincode");
            foreach (GridViewRow gr in gvMIS.Rows)
            {
                DataRow dr = dt.NewRow();

                dr["Loan Account Number"] = gr.Cells[0].Text;
                dr["Custoner ID"] = gr.Cells[1].Text;
                dr["Applicant Name"] = gr.Cells[2].Text;
                Label lblAppDOB = (Label)gr.FindControl("lblAppDOB");
                dr["AppDOB"] = gblFuction.setDate(lblAppDOB.Text);
                dr["Applicant Mobile Number"] = gr.Cells[4].Text;
                dr["Applicant Gender"] = gr.Cells[5].Text;
                dr["Applicant KYC Type 1"] = gr.Cells[6].Text;
                dr["Applicant KYC ID Number 1"] = gr.Cells[7].Text;
                dr["Applicant KYC Type 2"] = gr.Cells[8].Text;
                dr["Applicant KYC ID Number 2"] = gr.Cells[9].Text;
                dr["Applicant Address"] = gr.Cells[10].Text;
                dr["Applicant City"] = gr.Cells[11].Text;
                dr["Applicant District"] = gr.Cells[12].Text;
                dr["Applicant Village"] = gr.Cells[13].Text;
                dr["Applicant State"] = gr.Cells[14].Text;
                dr["Applicant Pincode"] = gr.Cells[15].Text;
                dr["Co-Applicant Name"] = gr.Cells[16].Text;
                Label lblCoAppDob = (Label)gr.FindControl("lblCoAppDob");
                dr["COAPPDOB"] = gblFuction.setDate(lblCoAppDob.Text);
                dr["Co-Applicant KYC Type 1"] = gr.Cells[18].Text;
                dr["Co-Applicant KYC ID Number 1"] = gr.Cells[19].Text;
                dr["Co-Applicant Address"] = gr.Cells[20].Text;
                dr["Co-Applicant City"] = gr.Cells[21].Text;
                dr["Co-Applicant District"] = gr.Cells[22].Text;
                dr["Co-Applicant Village"] = gr.Cells[23].Text;
                dr["Co-Applicant State"] = gr.Cells[24].Text;
                dr["Co-Applicant Pincode"] = gr.Cells[25].Text;
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                i++;
            }
            
            try
            {
                
                oRLC = new CReLoanCB();
                foreach (DataRow row in dt.Rows)
                {
                    int pCBId = 0;
                    string pEnquiryId = "";
                    Int32 vErr = 0;
                    string pErrorMsg = "";
                    int pStatus = 0;

                    vResult = false;

                    vErr = oRLC.SaveCBCheckforLoanEligibility(row["Loan Account Number"].ToString(), row["Custoner ID"].ToString(), 
                        row["Applicant Name"].ToString(), Convert.ToDateTime(row["AppDOB"].ToString()), row["Applicant Mobile Number"].ToString(), 
                        row["Applicant Gender"].ToString(), row["Applicant KYC Type 1"].ToString(), row["Applicant KYC ID Number 1"].ToString(), 
                        row["Applicant KYC Type 2"].ToString(), row["Applicant KYC ID Number 2"].ToString(), row["Applicant Address"].ToString(), 
                        row["Applicant City"].ToString(), row["Applicant District"].ToString(), row["Applicant Village"].ToString(), 
                        row["Applicant State"].ToString(), row["Applicant Pincode"].ToString(), row["Co-Applicant Name"].ToString(), 
                        Convert.ToDateTime(row["COAPPDOB"].ToString()), row["Co-Applicant KYC Type 1"].ToString(), row["Co-Applicant KYC ID Number 1"].ToString(), 
                        row["Co-Applicant Address"].ToString(), row["Co-Applicant City"].ToString(), row["Co-Applicant District"].ToString(), 
                        row["Co-Applicant Village"].ToString(), row["Co-Applicant State"].ToString(), row["Co-Applicant Pincode"].ToString(),
                        ref pCBId, ref pEnquiryId, pCreatedBy, vLogDt);

                    if (vErr == 0)
                    {
                        eq = new WebServiceSoapClient();
                        oRLC = new CReLoanCB();
                        if (row["Co-Applicant Name"].ToString() != "&nbsp;")
                        {
                            string pEqXmlOther = eq.Equifax(
                                                  row["Co-Applicant Name"].ToString(), "", "", Convert.ToDateTime(row["COAPPDOB"].ToString()).ToString("yyyy-MM-dd")
                                                  , "H", row["Co-Applicant Address"].ToString() + " ," + row["Co-Applicant City"].ToString() + " ,"
                                                  + row["Co-Applicant District"].ToString() + " ," + row["Co-Applicant Village"].ToString() + " ,"
                                                  + row["Co-Applicant State"].ToString() + " ," + row["Co-Applicant Pincode"].ToString(),
                                                  row["Co-Applicant State"].ToString(), row["Co-Applicant Pincode"].ToString()
                                                  , row["Applicant Mobile Number"].ToString(), row["Co-Applicant KYC Type 1"].ToString()
                                                  , row["Co-Applicant KYC ID Number 1"].ToString(), ""
                                                  , "", "", ""
                                                  , "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");

                            int pSlNo = 1;
                            string pMemCategory = "CoAppMem";
                            oRLC = new CReLoanCB();
                            oRLC.UpdateEquifaxInforCoApp_CBCheck(pEnquiryId, pCBId, pEqXmlOther, pSlNo, "0000", pCreatedBy, vLogDt, pMemCategory);
                        }
                        eq = new WebServiceSoapClient();
                        pEqXml = eq.Equifax(
                                row["Applicant Name"].ToString(), "", ""
                                , Convert.ToDateTime(row["AppDOB"].ToString()).ToString("yyyy-MM-dd"), "H",
                                row["Applicant Address"].ToString() + " ," + row["Applicant City"].ToString() + " ,"
                                              + row["Applicant District"].ToString() + " ," + row["Applicant Village"].ToString() + " ,"
                                              + row["Applicant State"].ToString() + " ," + row["Applicant Pincode"].ToString()
                                , row["Applicant State"].ToString(), row["Applicant Pincode"].ToString(), row["Applicant Mobile Number"].ToString()
                                , row["Applicant KYC Type 1"].ToString(), row["Applicant KYC ID Number 1"].ToString()
                                , row["Applicant KYC Type 2"].ToString(), row["Applicant KYC ID Number 2"].ToString()
                                , "", ""
                                , "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");
                        oRLC = new CReLoanCB();
                        vErr = oRLC.UpdateEquifaxInformation_CBCheck(pEnquiryId, pCBId, pEqXml, "0000", pCreatedBy, vLogDt, "P", pErrorMsg, ref pStatus, ref pStatusDesc);
                    }

                    vResult = true;
                }
                
                return vResult;
            }
            finally
            {
                oRLC = null;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        decimal sumFooterValue = 0;
        //protected void gvMIS_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        Label lblAmt = (Label)e.Row.FindControl("lblAmt");
        //        sumFooterValue += Convert.ToDecimal(lblAmt.Text);
        //    }
        //    if (e.Row.RowType == DataControlRowType.Footer)
        //    {
        //        Label lbl = (Label)e.Row.FindControl("lblTotal");
        //        lbl.Text = "Total-" + sumFooterValue.ToString();
        //    }
        //}
    }
}