using System;
using System.Data;
using System.Web;
using System.Web.UI;
using FORCECA;
using FORCEBA;
using System.IO;
using Excel;
using System.Web.UI.WebControls;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class NpsPranData : CENTRUMBase
    {
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
                ViewState["StateEdit"] = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
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
                this.PageHeading = "Import PRAN Data";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuNpsImprtPrnData);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = true;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                    btnDone.Visible = true;
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Import PRAN Data", false);
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
        protected void btnShow_Click(object sender, EventArgs e)
        {
           DataTable dt = null;
           CNpsMember oMem = null;
           try
           {
               oMem = new CNpsMember();
               dt = oMem.NPS_RptMemList(gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), rdMod.SelectedValue);           
               GenReport(dt);
           }
           finally
           {
               dt = null;
               oMem = null;
           }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidDate()
        {
            Boolean vResult = true;
            if (txtFrmDt.Text.Trim() != "" || txtFrmDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtFrmDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtFrDt");
                    vResult = false;
                }
            }
            if (txtToDt.Text.Trim() != "" || txtToDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtToDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtToDt");
                    vResult = false;
                }
            }
            return vResult;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDone_Click(object sender, EventArgs e)
        {
            CNpsMember oApp = null;
            DataTable dt = null;
            Int32 vErr = 0;
            string vXmlData = "";

            if (ValidDate() == true)
            {
                try
                {
                    dt = GetTable();
                    if (dt.Rows.Count <= 0)
                    {
                        gblFuction.MsgPopup("Please Import Data before Save.");  
                        return;
                    }                        

                    oApp = new CNpsMember();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    vErr = oApp.NPS_UpdateMemPran(vXmlData, this.UserID);
                    if (vErr > 0)
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
                    else
                        gblFuction.MsgPopup(gblMarg.DBError);
                }
                finally
                {
                    oApp = null;
                    dt = null;                    
                }
            }
        }
      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable GetTable()
        {
            DataTable dt = new DataTable("Table1");
            DataColumn dc1 = new DataColumn("PranNo");
            dt.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn("PranDt");
            dt.Columns.Add(dc2);
            DataColumn dc3 = new DataColumn("Status");
            dt.Columns.Add(dc3);
            DataColumn dc4 = new DataColumn("MemID");
            dt.Columns.Add(dc4);  
            foreach (GridViewRow gR in gvDtl.Rows)
            {              
                DataRow dR = dt.NewRow();
                if (gR.Cells[1].Text.Trim() != "&nbsp;")
                {
                    dR["PranNo"] = gR.Cells[1].Text.Trim();
                    //dR["PranDt"] = gblFuction.setDate(gR.Cells[15].Text.Trim());
                    dR["PranDt"] = gR.Cells[15].Text.Trim();
                    dR["Status"] = gR.Cells[11].Text.Trim();
                    dR["MemID"] = gR.Cells[2].Text.Trim();
                    dt.Rows.Add(dR);
                }
            }
            return dt;
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDetail_Click(object sender, EventArgs e)
        {
            if (fuDetail.HasFile)
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                gvDtl.DataSource = null;
                gvDtl.DataBind();
                try
                {
                    using (IExcelDataReader oERDtl = ExcelReaderFactory.CreateBinaryReader(fuDetail.FileContent))
                    {
                        try
                        {
                            oERDtl.IsFirstRowAsColumnNames = true;
                            ds = oERDtl.AsDataSet();
                            dt = ds.Tables[0];
                        }
                        catch
                        {
                            gblFuction.MsgPopup("Data Import Error: " + oERDtl.ExceptionMessage);
                        }
                    }
                    //foreach (DataRow dR in dt.Rows)
                    //{
                    //    if (dR["Date Of Birth"] != DBNull.Value)
                    //        dR["Date Of Birth"] = gblFuction.putStrDate(System.DateTime.FromOADate(Convert.ToDouble(dR["Date Of Birth"].ToString())));
                    //    if (dR["PRAN Activation Date"] != DBNull.Value)
                    //        dR["PRAN Activation Date"] = gblFuction.putStrDate(System.DateTime.FromOADate(Convert.ToDouble(dR["PRAN Activation Date"].ToString())));
                    //}
                    dt.AcceptChanges();
                    gvDtl.DataSource = dt;
                    gvDtl.DataBind();
                }
                finally
                {
                    dt = null;
                    ds = null;
                }
            }
            else
                gblFuction.MsgPopup("Please Select Excel File..");  
        }

        /// <summary>
        /// 
        /// </summary>
        private void GenReport(DataTable pTblExl)
        {
           string vFileNm = "";
            if (rdMod.SelectedValue == "Y")
                vFileNm = "attachment;filename=Member List With PRAN No";
            else
                vFileNm = "attachment;filename=Member List With Out PRAN No";
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            DataGrid1.DataSource = pTblExl;
            DataGrid1.DataBind();
            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan='6'><b><u><font size='5'>Centrum Microcredit Limited</font></u></b></td></tr>");
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
            htw.WriteLine("</table>");
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "application/vnd.ms-excel";
            Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}