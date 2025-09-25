using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class ReconRptCollPointWise : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                PopBranch(Session[gblValue.UserName].ToString());
                txtFromDt.Text = txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Recon Report Collection Point Wise";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuReconRptCollPointWise);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Recon Report Collection Point Wise", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                if (dt.Rows.Count > 0)
                {
                    dt = dt.AsEnumerable().Where(a => a.Field<string>("BranchCode") != "0000").CopyToDataTable();
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                    if (Session[gblValue.BrnchCode].ToString() != "0000")
                    {
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                        ddlBranch.Enabled = false;
                    }
                    else
                    {
                        ListItem liSel = new ListItem("<-- Select -->", "-1");
                        ddlBranch.Items.Insert(0, liSel);
                    }
                }
                else
                {
                    ddlBranch.Items.Clear();
                }
            }

            finally
            {
                dt = null;
                oUsr = null;
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            CCollectionPoint oQua = new CCollectionPoint();
            DataTable dt = null;
            dt = oQua.RptReconCollPointWise(ddlBranch.SelectedValue, gblFuction.setDate(txtFromDt.Text), gblFuction.setDate(txtToDt.Text));
            gvMIS.DataSource = dt;
            gvMIS.DataBind();
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if(gvMIS.Rows.Count > 0)
            {
                SetParameterForRptData("Excel");
            }
        }

        private void SetParameterForRptData(string pMode)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vFileNm = "attachment;filename=Recon_Report_Collection_Point_Wise";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='1' cellpadding='9' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan='9'><b><u><font size='3'>" + gblValue.CompName + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='9'><b><u><font size='2'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='9'><b><u><font size='2'>Recon Report Collection Point Wise</font></u></b></td></tr>");
            gvMIS.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='7'><b><u><font size='9'></font></u></b></td></tr>");
            htw.WriteLine("</table>");
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm + ".xls");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
            Response.Write(sw.ToString());
            Response.End();            
        }

        public override void VerifyRenderingInServerForm(Control control)
        {          
        }
    }
}