using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using FORCEBA;
using FORCECA;
using System.Web.UI;
using System.Web;

namespace CENTRUM.WebPages.Private.BCOperation
{
    public partial class BC_New_Business_Summary : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                StatusButton("View");
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                tabBnsSum.ActiveTabIndex = 0;
                //popDistrict();
            }
        }
         private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "BC New Business Summary";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuNewBusinessSummary);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnPrint.Enabled = true;
                    //btnCancel.Enabled = true;
                    btnExit.Enabled = true;
                    ClearControls();
                    break;
                case "Show":
                    btnPrint.Enabled = false;
                    //btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnPrint.Enabled = true;
                    //btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnPrint.Enabled = true;
                    //btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    btnPrint.Enabled = false;
                    //btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Exit":
                    btnPrint.Visible = false;
                    //btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        private void EnableControl(Boolean Status)
        {
            //txtLoanPurpose.Enabled = Status;
            //ddlLoanSector.Enabled = Status;
        }
        private void ClearControls()
        {
            //txtLoanPurpose.Text = "";
            //ddlLoanSector.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
        }
        //private void popDistrict()
        //{
        //    DataTable dt = null;
        //    string vBrcode = Session[gblValue.BrnchCode].ToString();
        //    CTehsil oThl = null;
        //    try
        //    {
        //        oThl = new CTehsil();
        //        dt = oThl.GetDistbyBranch(vBrcode, "A");
        //        //dt = oGb.PopComboMIS("N", "N", "AA", "DistrictId", "DistrictName", "DistrictMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
        //        ddlDistrict.DataSource = dt;
        //        ddlDistrict.DataTextField = "DistrictName";
        //        ddlDistrict.DataValueField = "DistrictId";
        //        ddlDistrict.DataBind();
        //        ListItem oli = new ListItem("<--Select-->", "-1");
        //        ddlDistrict.Items.Insert(0, oli);
        //    }
        //    finally
        //    {
        //        oThl = null;
        //        dt = null;
        //    }
        //}

        //protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 DistId = 0;
        //    DistId = Convert.ToInt32(ddlDistrict.SelectedValue);
        //    PopBranch(DistId);

        //}
        //public void PopBranch(int Distid)
        //{
        //    DataTable dt = null;
        //    CBranch oCBr = null;
        //    try
        //    {
        //        oCBr = new CBranch();
        //        ddlBr.Items.Clear();
        //        dt = oCBr.GetBranchbyDistrict(Distid);
        //        ddlBr.DataSource = dt;
        //        ddlBr.DataTextField = "BranchName";
        //        ddlBr.DataValueField = "BranchCode";
        //        ddlBr.DataBind();
        //        ListItem oli = new ListItem("<--Select-->", "-1");
        //        ddlBr.Items.Insert(0, oli);
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oCBr = null;
        //    }

        //}

        //protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string vBrCode = "";
        //    vBrCode = ddlBr.SelectedValue.ToString();
        //    popFO(vBrCode);
        //}

        //private void popFO(string vBrCode)
        //{
        //    DataTable dt = null;
        //    CCM oCM = null;
        //    try
        //    {
        //        oCM = new CCM();
        //        dt = oCM.GetCOPop(vBrCode, "SCO,CO,TCO,JTCO,GO,UM");
        //        ddlFO.DataTextField = "EOName";
        //        ddlFO.DataValueField = "EOId";
        //        ddlFO.DataSource = dt;
        //        ddlFO.DataBind();
        //        ListItem oItm = new ListItem();
        //        oItm.Text = "<--- Select --->";
        //        oItm.Value = "-1";
        //        ddlFO.Items.Insert(0, oItm);
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oCM = null;
        //    }
        //}

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid();

        }
        private void LoadGrid()
        {
            DataTable dt = null;
            CBC_Repayment_Tracking oBC = null;
            string vMode = string.Empty;
            try
            {
                vMode = rdbSel.SelectedValue;
                DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                oBC = new CBC_Repayment_Tracking();
                //System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                //dt = oBC.rptBC_Business_Summary(gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), vMode);

                GridView gvBsnSum = new GridView();
                gvBsnSum.AutoGenerateColumns = false;

                dt = oBC.BC_rptBusiness_Summary(gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), vMode);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    BoundField boundfield = new BoundField();
                    boundfield.DataField = dt.Columns[i].ColumnName.ToString();
                    boundfield.HeaderText = dt.Columns[i].ColumnName.ToString();
                    gvBsnSum.Columns.Add(boundfield);
                }
                gvBsnSum.DataSource = dt;
                gvBsnSum.DataBind();
                gvBsnSum.Width = 1070;
                //gvBsnSum.Font.Size = 10;
                //gvBsnSum.ForeColor = System.Drawing.Color.Black;
                gvBsnSum.BackColor = System.Drawing.Color.White;
                gvBsnSum.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#1C5E55");
                gvBsnSum.HeaderStyle.ForeColor = System.Drawing.Color.White;
                gvBsnSum.HeaderStyle.Font.Bold = true;
                gvBsnSum.FooterStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#1C5E55");
                gvBsnSum.FooterStyle.Font.Bold = true;
                gvBsnSum.FooterStyle.ForeColor = System.Drawing.Color.White;
                gvBsnSum.RowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                gvBsnSum.EditRowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#7C6F57");
                gvBsnSum.PagerStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#666666");
                gvBsnSum.PagerStyle.ForeColor = System.Drawing.Color.White;
                gvBsnSum.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
                gvBsnSum.SelectedRowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#C5BBAF");
                gvBsnSum.SelectedRowStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                gvBsnSum.SelectedRowStyle.Font.Bold = true;

                Panel1.Controls.Add(gvBsnSum);
                ViewState["BusinessSummary"] = dt;

                //DataGrid1.DataSource = dt;
                //DataGrid1.DataBind();
                //ViewState["BusinessSummary"] = dt;
                //tdx.Controls.Add(DataGrid1);
                //tdx.Visible = false;
            }
            finally
            {
                dt = null;
                oBC = null;
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDetails();

        }
        private void PrintDetails()
        {
            //DataTable dt = null;
            //string vFileNm = "";
            //Int32 vDistId = 0;
            //string vBrCode;

            //CBC_Repayment_Tracking oBC = new CBC_Repayment_Tracking();
            //vDistId = Convert.ToInt32(ddlDistrict.SelectedValue);
            //vBrCode = ddlBr.SelectedValue;
            //if (Convert.ToInt32(ddlDistrict.SelectedValue) > 0)
            //{
            //    vDistId = Convert.ToInt32(ddlDistrict.SelectedValue);
            //}
            //else
            //{
            //    vDistId = 0;
            //}

            //if (vDistId > 0)
            //{
            //    vBrCode = ddlBr.SelectedValue;
            //}

            DataTable dt = null;
            string vFileNm = "";
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            dt = (DataTable)ViewState["BusinessSummary"];

            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();
            //tdx.Controls.Add(DataGrid1);
            //tdx.Visible = false;
            vFileNm = "attachment;filename=BC_New_Business_Summary.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='1' cellpadding='5' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan=" + dt.Columns.Count + "><b><u><font size='3'>BC New Business Summary</font></u></b></td></tr>");
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
            htw.WriteLine("</table>");
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(sw.ToString());
            Response.End();
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
    }
}
   