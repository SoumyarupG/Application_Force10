using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using FORCECA;
using FORCEBA;
using System.Web.Security;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class RptMonitoring : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                popBranch();
                DataGrid1.DataSource = null;
                DataGrid1.DataBind();
                lblCarriedOutBy.Text = "";
                if (ddlBranch.SelectedIndex > 0) popMonth();
                ViewState["SubDt"] = null;
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Monitoring Report";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuMonitoringRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                 if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Monitoring Report", false);

                    if (this.CanReport == "Y")
                    {
                        btnExcl.Visible = true;
                    }
                    else
                    {
                        btnExcl.Visible = false;
                    }
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx?e=random", true);
            }
        }

        private void popMonth()
        {
            DataTable dt = null;
            CIntIspPM oRO = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = ddlBranch.SelectedValue;
                oRO = new CIntIspPM();
                dt = oRO.Insp_GetInspMonitorByBranch(vBrCode);
                ddlMonth.DataSource = dt;
                ddlMonth.DataTextField = "DateRange";
                ddlMonth.DataValueField = "InspID";
                ddlMonth.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlMonth.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        private void popBranch()
        {
            DataTable dt = null;
            CUser oUsr = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));
                if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (Convert.ToString(row["BranchCode"]) != Convert.ToString(Session[gblValue.BrnchCode]))
                        {
                            row.Delete();
                        }
                    }
                    dt.AcceptChanges();
                }
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBranch.Items.Insert(0, oli);
                if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBranch.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Please select Branch");
                return;
            }
            DataGrid1.DataSource = null;
            DataGrid1.DataBind();
            if (ddlBranch.SelectedIndex > 0) popMonth();
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            CReports oRpt = null;
            double vTotQt = 0, vMrkObt = 0, vPerMark = 0, vPerMark1 = 0;
            string vGrding = string.Empty;

            try
            {
                if (ddlMonth.SelectedIndex <= 0)
                {
                    gblFuction.MsgPopup("Please select Date Range");
                    return;
                }
                if (ddlMonth.SelectedIndex > 0)
                {
                    oRpt = new CReports();
                    ds = oRpt.Insp_rptMonitoringBranchWise(Convert.ToInt32(ddlMonth.SelectedValue));
                    DataGrid1.DataSource = ds.Tables[0];
                    DataGrid1.DataBind();
                    DataGrid1.DataMember = "Table";
                    ViewState["List"] = ds;
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    //if (dt1.Rows.Count > 0)
                    //{
                    //    vTotQt = (dt1.Rows.Count-2) * 4;
                    //    vMrkObt = Int32.Parse(dt2.Rows[0]["MarksObtain"].ToString());
                    //    vPerMark = Math.Round((vMrkObt / vTotQt) * 100, 2);
                    //    vPerMark1 = Math.Round((vMrkObt / vTotQt) * 100, 0);
                    //    if (vPerMark1 >= 80 && vPerMark1 <= 100)
                    //        vGrding = "AA";
                    //    else if (vPerMark1 >= 65 && vPerMark1 <= 79)
                    //        vGrding = "A";
                    //    else if (vPerMark1 >= 45 && vPerMark1 <= 64)
                    //        vGrding = "BB";
                    //    else if (vPerMark1 <= 45)
                    //        vGrding = "B";
                    //    DataGrid1.DataSource = dt1;
                    //    DataGrid1.DataBind();
                    //    lblObt.Text = vMrkObt.ToString(); 
                    //    lblMax.Text = vTotQt.ToString();  
                    //    lblPer.Text = vPerMark.ToString();
                    //    lblGrd.Text = vGrding;
                    ViewState["SubDt"] = Convert.ToString(dt2.Rows[0]["SubDt"]);
                    //ViewState["List"] = ds;

                }

                else
                {
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                    lblCarriedOutBy.Text = "";
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    lblCarriedOutBy.Text = ds.Tables[1].Rows[0]["CarriedOutBy"].ToString();
                }
            }
            finally
            {
                ds = null;
                //dt1 = null;
                //dt2 = null;
                oRpt = null;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["List"] = null;
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            if (ddlMonth.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Please select Date Range.");
                return;
            }
            DataSet ds = (DataSet)ViewState["List"];
            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();
            string vFileNm = "";
            vFileNm = "attachment;filename=MonitoringReport.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' widht='100%'>");
            htw.WriteLine("<tr><td align='center' colspan='6'><b><u><font size='5'>" + gblValue.CompName + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align='center' colspan='6'>" + CGblIdGenerator.GetBranchAddress1("0000") + "</td></tr>");
            htw.WriteLine("<tr><td align='center' colspan='6'><b><u><font size='3'>Monitoring Report</font></u></b></td></tr>");
            htw.WriteLine("<tr><td colspan='2' align='left'><b>Carried Out Date : " + ddlMonth.SelectedItem.Text + "</b></td><td colspan='3' align=left'><b>Branch : " + ddlBranch.SelectedItem.Text + "</b></td><td colspan='2' align=left'><b>Submission Date : " + ViewState["SubDt"] + "</b></td></tr>");
            DataGrid1.RenderControl(htw);
            htw.WriteLine("<tr><td colspan='6'></td></tr>");
            htw.WriteLine("<tr><td colspan='6' align='left'><b> Carried Out By : " + lblCarriedOutBy.Text + "&nbsp&nbsp&nbsp&nbsp</b></td></tr>");
            htw.WriteLine("</table>");
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}