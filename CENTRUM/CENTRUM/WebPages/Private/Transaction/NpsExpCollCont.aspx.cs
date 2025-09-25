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
    public partial class NpsExpCollCont : CENTRUMBase
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
                this.PageHeading = "Upload NPS Contribution";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuNpsUpldCont);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = true;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                    btnDone.Visible = true;
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Upload NPS Contribution", false);
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
            return vResult;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDone_Click(object sender, EventArgs e)
        {            
            DataTable dtExcel = null;           
            string vFileNm = "";

            if (ValidDate() == true)
            {
                try
                {                    
                    dtExcel = GetExcelSheet();
                    if (dtExcel.Rows.Count <= 0)
                    {
                        gblFuction.MsgPopup("No Records to Export Excel..");  
                        return;
                    }

                    System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                    DataGrid1.DataSource = dtExcel;
                    DataGrid1.DataBind();
                    tdx.Controls.Add(DataGrid1);
                    tdx.Visible = false;
                    vFileNm = "attachment;filename=NPS Contribution";
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
                }
                finally
                {
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
                dt = oMem.NPS_GetCollContribution(vFromDt, vToDt);
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
                    dR["MemId"] = gR.Cells[2].Text.Trim();
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
            DataColumn dc2 = new DataColumn("NL", System.Type.GetType("System.String"));
            dt.Columns.Add(dc2);
            DataColumn dc3 = new DataColumn("PRAN", System.Type.GetType("System.String"));
            dt.Columns.Add(dc3);
            DataColumn dc4 = new DataColumn("SUBSCRIBER NAME", System.Type.GetType("System.String"));
            dt.Columns.Add(dc4);
            DataColumn dc5 = new DataColumn("CONTRIBUTION", System.Type.GetType("System.String"));
            dt.Columns.Add(dc5);
            DataColumn dc6 = new DataColumn("CO-CONTRIBUTION", System.Type.GetType("System.String"));
            dt.Columns.Add(dc6);
            DataColumn dc7 = new DataColumn("SUBSCRIBERS CONTRIBUTION", System.Type.GetType("System.String"));
            dt.Columns.Add(dc7);           
            foreach (GridViewRow gR in gvSanc.Rows)
            {                
                DataRow dR = dt.NewRow();
                dR["Sl No"] = gR.Cells[0].Text.Trim();
                dR["NL"] = gR.Cells[2].Text.Trim();
                dR["PRAN"] = gR.Cells[3].Text.Trim();
                dR["SUBSCRIBER NAME"] = gR.Cells[4].Text.Trim();
                dR["CONTRIBUTION"] = gR.Cells[5].Text.Trim();
                dR["CO-CONTRIBUTION"] = gR.Cells[6].Text.Trim();
                dR["SUBSCRIBERS CONTRIBUTION"] = gR.Cells[7].Text.Trim();                   
                dt.Rows.Add(dR);               
            }
            return dt;
        }
    }
}
