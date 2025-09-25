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

namespace CENTRUM.WebPages.Private.Report
{
    public partial class BranchFundTr : CENTRUMBase
    {
        private static string vExcelPath = ConfigurationManager.AppSettings["DBPath"];
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDt.Text = Session[gblValue.LoginDate].ToString();
                //PopDtl(txtDt.Text.Trim());
                btnSave.Enabled = true; 
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
                //this.PageHeading = "HO To Branch Fund Transfer (With Reverse)";
                this.PageHeading = "HO To Branch Fund Transfer Maker";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuBrFundTr);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "HO To Branch Fund Transfer Maker", false);
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
                dt = oBT.GetBrFundTrDetails(Session[gblValue.BrnchCode].ToString(),vDate);
                                              
                System.Data.DataColumn BrDt = new System.Data.DataColumn("BrDt", typeof(string));
                BrDt.DefaultValue = txtDt.Text.Trim();
                dt.Columns.Add(BrDt);
                dt.AcceptChanges();
                //foreach (DataRow dr in dt.Rows)
                //{                    
                //    dr["BrDt"] = txtDt.Text.Trim();
                //}
                gvDtl.DataSource = dt;
                gvDtl.DataBind();
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

        private void Import_To_Grid(string FilePath, string Extension, string isHDR)
        {
            string conStr = "";
            string vXml = "";
            CBranchFundTr oBT = null;
            DataTable dtImport = null;
            DateTime vDate = gblFuction.setDate(txtDt.Text.Trim());
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

            dt.TableName = "Table1";
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vXml = oSW.ToString();
            }

            oBT = new CBranchFundTr();
            dtImport = oBT.GetBrFundTrImport(vXml, vDate);

            System.Data.DataColumn BrDt = new System.Data.DataColumn("BrDt", typeof(string));
            BrDt.DefaultValue = txtDt.Text.Trim();
            dtImport.Columns.Add(BrDt);
            dtImport.AcceptChanges();


            gvDtl.DataSource = dtImport;
            gvDtl.DataBind();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CVoucher oVoucher = null;
            DataTable dtHOBank = null;
            DataTable dtBrBank = null;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                DropDownList ddlHOBank = (DropDownList)e.Row.FindControl("ddlHOBank");
                DropDownList ddlBrBank = (DropDownList)e.Row.FindControl("ddlBrBank");
                TextBox txtAmt = (TextBox)e.Row.FindControl("txtAmt");
                CheckBox chkTr = (CheckBox)e.Row.FindControl("chkTr");
                TextBox txtBrDt = (TextBox)e.Row.FindControl("txtBrDt");            
                try
                {
                    oVoucher = new CVoucher();                
                    ddlHOBank.Items.Clear();
                    dtHOBank = oVoucher.GetAcGenLedCB(Session[gblValue.BrnchCode].ToString(), "T", "");
                    if (dtHOBank.Rows.Count > 0)
                    {
                        ddlHOBank.DataSource = dtHOBank;
                        ddlHOBank.DataTextField = "Desc";
                        ddlHOBank.DataValueField = "DescId";
                        ddlHOBank.DataBind();
                    }
                    ListItem oLk = new ListItem("<--Select-->", "-1");
                    ddlHOBank.Items.Insert(0, oLk);
                    ddlHOBank.SelectedIndex = ddlHOBank.Items.IndexOf(ddlHOBank.Items.FindByValue(e.Row.Cells[9].Text.Trim()));
                    oVoucher = new CVoucher(); 
                    ddlBrBank.Items.Clear();
                    dtBrBank = oVoucher.GetAcGenLedCB(e.Row.Cells[7].Text.Trim(), "T", "");
                    if (dtBrBank.Rows.Count > 0)
                    {
                        ddlBrBank.DataSource = dtBrBank;
                        ddlBrBank.DataTextField = "Desc";
                        ddlBrBank.DataValueField = "DescId";
                        ddlBrBank.DataBind();
                    }
                    ListItem oLc = new ListItem("<--Select-->", "-1");
                    ddlBrBank.Items.Insert(0, oLc);
                    ddlBrBank.SelectedIndex = ddlBrBank.Items.IndexOf(ddlBrBank.Items.FindByValue(e.Row.Cells[10].Text.Trim()));                            
                    txtAmt.Text = e.Row.Cells[8].Text.Trim();
                    txtBrDt.Text = e.Row.Cells[12].Text.Trim();
                    chkTr.Checked = e.Row.Cells[11].Text.Trim() == "Y" ? true : false;

                    
                    
                    e.Row.Cells[9].Text = ddlHOBank.SelectedValue.ToString();
                    e.Row.Cells[10].Text = ddlBrBank.SelectedValue.ToString();
                    
            
                     
                }
                finally
                {
                    oVoucher = null;
                    dtHOBank = null;
                    dtBrBank = null;                 
                }
            }
        }

        private void Total()
        {
            double vTotAmt = 0;
            

            foreach (GridViewRow gR in gvDtl.Rows)
            {
                
                TextBox txtAmt = (TextBox)gR.FindControl("txtAmt");
                
                CheckBox chkTr = (CheckBox)gR.FindControl("chkTr");
                if (chkTr.Checked == true)
                {
                    
                    vTotAmt = vTotAmt + Convert.ToDouble(txtAmt.Text.Trim());
                    
                }
            }
            //using (StringWriter oSW = new StringWriter())
            //{
                txtGrdTot.Text = Convert.ToString(vTotAmt);
                
            //}
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String XmlList()
        {
            double vTotAmt = 0;
            String vSpecXML = "";
            DataTable dt = new DataTable("List");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("HOBankID");
            dt.Columns.Add("BrBankID");
            dt.Columns.Add("BrDt");
            dt.Columns.Add("Amount");
            dt.Columns.Add("IsChk");
     
            foreach (GridViewRow gR in gvDtl.Rows)
            {
                DropDownList ddlHOBank = (DropDownList)gR.FindControl("ddlHOBank");
                DropDownList ddlBrBank = (DropDownList)gR.FindControl("ddlBrBank");
                TextBox txtAmt = (TextBox)gR.FindControl("txtAmt");
                TextBox txtBrDt = (TextBox)gR.FindControl("txtBrDt");
                CheckBox chkTr = (CheckBox)gR.FindControl("chkTr");
                if (chkTr.Checked == true)
                {
                    DataRow dr = dt.NewRow();
                    dr["BranchCode"] = gR.Cells[7].Text;
                    dr["HOBankID"] = ddlHOBank.SelectedValue.ToString();
                    dr["BrBankID"] = ddlBrBank.SelectedValue.ToString();
                    dr["BrDt"] = gblFuction.setDate(txtBrDt.Text.Trim()).ToString();
                    dr["Amount"] = txtAmt.Text.Trim();
                    vTotAmt = vTotAmt + Convert.ToDouble(txtAmt.Text.Trim());
                    dr["IsChk"] = chkTr.Checked == true ? "Y" : "N";
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
            }
            using (StringWriter oSW = new StringWriter())
            {
                ViewState["TotAmt"] = vTotAmt;
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
            CheckBox chkTr = (CheckBox)sender;
            GridViewRow gR = (GridViewRow)chkTr.NamingContainer;
            DropDownList ddlHOBank = (DropDownList)gR.FindControl("ddlHOBank");
            DropDownList ddlBrBank = (DropDownList)gR.FindControl("ddlBrBank");
            TextBox txtAmt = (TextBox)gR.FindControl("txtAmt");
            TextBox txtBrDt = (TextBox)gR.FindControl("txtBrDt");
            if (ddlHOBank.SelectedValue == "-1")
            {
                chkTr.Checked = false;
                gblFuction.AjxMsgPopup("Please Select Bank For Ho");
                return;
            }         
            else if (ddlBrBank.SelectedValue == "-1")
            {
                chkTr.Checked = false;
                gblFuction.AjxMsgPopup("Please Select Bank For Branch");
                return;
            }
            else if (txtAmt.Text.ToString() == "" || txtAmt.Text.ToString() == "0")
            {
                chkTr.Checked = false;
                gblFuction.AjxMsgPopup("Please Select Amount");
                return;
            }
            else
            {
                gR.Cells[8].Text = txtAmt.Text.Trim() == "" ? "0" : txtAmt.Text.Trim();
                gR.Cells[12].Text = txtBrDt.Text.Trim() == "" ? txtDt.Text.Trim() : txtBrDt.Text.Trim();
                gR.Cells[9].Text = ddlHOBank.SelectedValue.ToString();
                gR.Cells[10].Text = ddlHOBank.SelectedValue.ToString();
                gR.Cells[11].Text = chkTr.Checked == true ? "Y" : "N";
            }
            Total(gR);
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
            int vUserID = this.UserID;
            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vTblDtl = Session[gblValue.ACVouDtl].ToString();
            String vFinYear = Session[gblValue.ShortYear].ToString();
            int vYearNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
            String vBranchCode = Session[gblValue.BrnchCode].ToString();
            DateTime LstEndDt = gblFuction.setDate("01/01/1900");
            CDayEnd oDE = null;
            DataTable dt = null;

            try
            {
                oDE = new CDayEnd();
                dt = oDE.GetLastEndDayDate(Session[gblValue.BrnchCode].ToString());
                if (dt.Rows.Count > 0)
                {
                    LstEndDt = gblFuction.setDate(Convert.ToString(dt.Rows[0]["EndDate"]));
                }

                if (LstEndDt == gblFuction.setDate("01/01/1900"))
                {
                    
                    gblFuction.AjxMsgPopup("Day end not declared for this branch");
                    return 1;
                }
                if (LstEndDt.AddDays(-1) >= gblFuction.setDate(txtDt.Text))
                {
                    
                    gblFuction.AjxMsgPopup("Day end already declared for this branch");
                    return 1;
                }
                if (LstEndDt != gblFuction.setDate(txtDt.Text))
                {
                    
                    gblFuction.AjxMsgPopup("Day end pending for this branch");
                    return 1;
                }

                if (Session[gblValue.EndDate] != null)
                {
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtDt.Text))
                    {
                        gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                        return 1;
                    }
                }

                vXml = XmlList();
                if (Convert.ToDouble(txtGrdTot.Text) != Convert.ToDouble(ViewState["TotAmt"]))
                {
                    gblFuction.MsgPopup("Total Amount Mis Match.");
                    return 1;
                }
                else
                {
                    oBF = new CBranchFundTr();
                    vRes = oBF.SaveBrFundTrDetails(vXml, vDate, vTblMst, vTblDtl, vFinYear, vYearNo, vBranchCode, vUserID);
                    return vRes;
                }
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
        protected void btnPrn_Click(object sender, EventArgs e)
        {
            DateTime vFromDt = gblFuction.setDate(txtDt.Text.Trim());
            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vRptPath = "";
            string vTitle = "Payment Voucher";
            ReportDocument rptDoc = new ReportDocument();
            DataTable dt = new DataTable();
            CReports oVoucher = new CReports();
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            
            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\FundTrVoucher.rpt";
            dt = oVoucher.rptFundTransVoucher(vTblMst);
            rptDoc.Load(vRptPath);
            rptDoc.SetDataSource(dt);
            rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
            rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
            rptDoc.SetParameterValue("pAddress2", "");

            rptDoc.SetParameterValue("pBranch", vBranch);
            rptDoc.SetParameterValue("dtFrom", txtDt.Text);
            rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Fund_Transfer_Voucher");
        
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            
             int vRes = 0;
           
            vRes=SaveRecords();
           
            gvDtl.DataSource = null;
            gvDtl.DataBind();
            btnSave.Enabled = false; 
           
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gvr"></param>
        private void Total(GridViewRow gvr)
        {
            int rowindex = 0, vMaxRow = 0;
            double vTotAmt = 0;
            rowindex = gvr.RowIndex;
            vMaxRow = gvDtl.Rows.Count;
            foreach (GridViewRow gr in gvDtl.Rows)
            {
                TextBox txtAmt = (TextBox)gr.FindControl("txtAmt");
                CheckBox chkTr = (CheckBox)gr.FindControl("chkTr");
                if (chkTr.Checked == true)
                {
                    if (txtAmt.Text != "" && txtAmt.Text != null && chkTr.Checked == true)
                        vTotAmt += Convert.ToDouble(txtAmt.Text);
                }
            }
            txtGrdTot.Text = vTotAmt.ToString();
            UpTot.Update();
            upDtl.Update();
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
                Total();
            }
        }
    }
}
