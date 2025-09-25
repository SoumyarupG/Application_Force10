using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class NpsMemSelPran : CENTRUMBase
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
                LoadGrid(txtFrmDt.Text, txtToDt.Text, 0);
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
                this.PageHeading = "Send to CRA-FC";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuNpsSndToAlainkit);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = true;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                    btnDone.Visible = true;
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Send to CRA-FC", false);
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
            if (ValidDate() == true) LoadGrid(txtFrmDt.Text, txtToDt.Text, 0);
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
            if (txtLotNo.Text == "")
            {
                gblFuction.MsgPopup("Lot No. should not be blank");
                gblFuction.focus("ctl00_cph_Main_txtLotNo");
                vResult = false;
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
            DataTable dt = null, dtExcel = null;
            Int32 vErr = 0;
            string vXmlData = "", vFileNm = "";           

            if (ValidDate() == true)
            {
                try
                {
                    dt = GetTable();
                    if (dt.Rows.Count <= 0)
                    {
                        gblFuction.MsgPopup("No Records to Process... ");  
                        return;
                    }
                    oApp = new CNpsMember();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }                   
                    vErr = oApp.NPS_UpdateMemList(vXmlData,txtLotNo.Text.Trim(), this.UserID);
                    if (vErr > 0)
                    {
                        dtExcel = GetExcelSheet();
                        System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                        DataGrid1.DataSource = dtExcel;
                        DataGrid1.DataBind();

                        tdx.Controls.Add(DataGrid1);
                        tdx.Visible = false;
                        vFileNm = "attachment;filename=Member List";
                        StringWriter sw = new StringWriter();
                        HtmlTextWriter htw = new HtmlTextWriter(sw);
                        htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
                        htw.WriteLine("<tr><td align=center' colspan='6'><b><u><font size='5'>Centrum Microcredit Pvt. Ltd.</font></u></b></td></tr>");
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
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                    }
                }
                finally
                {
                    oApp = null;
                    dt = null;
                    dtExcel = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAppMode"></param>
        /// <param name="pFromDt"></param>
        /// <param name="pToDt"></param>
        /// <param name="pBranch"></param>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(string pFromDt, string pToDt, Int32 pPgIndx)
        {
            DataTable dt = null;
            CNpsMember oMem = null;
            try
            {
                DateTime vFromDt = gblFuction.setDate(pFromDt);
                DateTime vToDt = gblFuction.setDate(pToDt);
                oMem = new CNpsMember();
                dt = oMem.NPS_GetMemSelectList(vFromDt, vToDt);
                gvSanc.DataSource = dt;                
                gvSanc.DataBind();
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
            DataColumn dc1 = new DataColumn("MemId");
            dt.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn("SendYN");
            dt.Columns.Add(dc2);           
            foreach (GridViewRow gR in gvSanc.Rows)
            {
                CheckBox chkApp = (CheckBox)gR.FindControl("chkApp");
                TextBox txtSend = (TextBox)gR.FindControl("txtSend");
                if (chkApp.Checked == true)
                {
                    DataRow dR = dt.NewRow();
                    dR["MemId"] =  gR.Cells[2].Text.Trim();
                    dR["SendYN"] = txtSend.Text;
                    dt.Rows.Add(dR);   
                }
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable GetExcelSheet()
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("Sl No", System.Type.GetType("System.String"));
            dt.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn("Membership Number", System.Type.GetType("System.String"));
            dt.Columns.Add(dc2);
            DataColumn dc3 = new DataColumn("Members Name", System.Type.GetType("System.String"));
            dt.Columns.Add(dc3);
            DataColumn dc4 = new DataColumn("Branch Name", System.Type.GetType("System.String"));
            dt.Columns.Add(dc4);
            DataColumn dc5 = new DataColumn("Gender", System.Type.GetType("System.String"));
            dt.Columns.Add(dc5);
            DataColumn dc6 = new DataColumn("Father Name", System.Type.GetType("System.String"));
            dt.Columns.Add(dc6);
            DataColumn dc7 = new DataColumn("Date of Birth", System.Type.GetType("System.String"));
            dt.Columns.Add(dc7);
            DataColumn dc8 = new DataColumn("Submit Date", System.Type.GetType("System.String"));
            dt.Columns.Add(dc8);
            DataColumn dc9 = new DataColumn("Lot No", System.Type.GetType("System.String"));
            dt.Columns.Add(dc9);
            foreach (GridViewRow gR in gvSanc.Rows)
            {
                CheckBox chkApp = (CheckBox)gR.FindControl("chkApp");
                if (chkApp.Checked == true)
                {
                    DataRow dR = dt.NewRow();
                    dR["Sl No"] = gR.Cells[0].Text.Trim();
                    dR["Membership Number"] = gR.Cells[2].Text.Trim();
                    dR["Members Name"] = gR.Cells[1].Text.Trim();
                    dR["Branch Name"] = gR.Cells[4].Text.Trim();
                    dR["Gender"] = gR.Cells[3].Text.Trim();
                    dR["Father Name"] = gR.Cells[5].Text.Trim();
                    dR["Date of Birth"] = gR.Cells[6].Text.Trim();
                    dR["Submit Date"] = gR.Cells[7].Text.Trim();
                    dR["Lot No"] = txtLotNo.Text.ToUpper();
                    dt.Rows.Add(dR);   
                }
            }
            return dt;
        }
    }
}