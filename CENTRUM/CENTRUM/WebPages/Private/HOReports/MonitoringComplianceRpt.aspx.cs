using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;
using System.IO;
using System.Web.Security;


namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class MonitoringComplianceRpt : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                popBranch();
                if (ddlBranch.SelectedIndex > 0) popMonth();
                ViewState["SubDt"] = null;
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Monitoring Compliance Report";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuMonitoringComplRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Monitoring Compliance Report", false);

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
                dt = oRO.Insp_GetMonitoringComplanceByBranch(vBrCode);
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
            double vObtScr = 0, vRect = 0, vPrlRect = 0, vNonRect = 0, vIrrNo = 0, vMaxMrks = 0;
            Int32 vTRow = 0, vLRow = 0;

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
                    ds = oRpt.Insp_rptMonCompliance(Convert.ToInt32(ddlMonth.SelectedValue));
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    if (dt1.Rows.Count > 0)
                    {
                        //var vIrrNo1 = dt1.AsEnumerable().Sum(row => row.Field<int>("No Of Irre"));
                        //var vObtScr1 = dt1.AsEnumerable().Sum(row => row.Field<double>("Obtain Scores"));
                        //var vRect1 = dt1.AsEnumerable().Sum(row => row.Field<int>("Rectified"));
                        //var vPrlRect1 = dt1.AsEnumerable().Sum(row => row.Field<int>("Partial Rectified"));
                        //var vNonRect1 = dt1.AsEnumerable().Sum(row => row.Field<int>("Non Rectified"));

                        //vIrrNo = Convert.ToInt32(vIrrNo1);
                        //vObtScr = Convert.ToInt32(vObtScr1);
                        //vRect = Convert.ToInt32(vRect1);
                        //vPrlRect = Convert.ToInt32(vPrlRect1);
                        //vNonRect = Convert.ToInt32(vNonRect1);

                        //vTRow = dt1.Rows.Count - 2;
                        //vLRow = dt1.Rows.Count - 1;
                        //dt1.Rows[vTRow][3] = vObtScr;
                        //dt1.Rows[vTRow][4] = vIrrNo;
                        //dt1.Rows[vTRow][5] = vRect;
                        //dt1.Rows[vTRow][6] = vPrlRect;
                        //dt1.Rows[vTRow][7] = vNonRect;
                        hdCmpDt.Value = Convert.ToString(dt2.Rows[0]["CmpDate"]);
                        hdCmpBy.Value = Convert.ToString(dt2.Rows[0]["Compliance_By"]);
                        hdFilledBy.Value = Convert.ToString(dt2.Rows[0]["Filled_By"]);
                        hdSubDt.Value = Convert.ToString(dt2.Rows[0]["SubDt"]);

                        //if (vIrrNo > 0)
                        //{
                        //    dt1.Rows[vLRow][5] = Math.Round((vRect / vIrrNo) * 100, 2);
                        //    dt1.Rows[vLRow][6] = Math.Round((vPrlRect / vIrrNo) * 100, 2);
                        //    dt1.Rows[vLRow][7] = Math.Round((vNonRect / vIrrNo) * 100, 2);
                        //}
                        //else
                        //{
                        //    dt1.Rows[vLRow][5] = 0;
                        //    dt1.Rows[vLRow][6] = 0;
                        //    dt1.Rows[vLRow][7] = 0;
                        //}
                        dt1.AcceptChanges();
                        DataGrid1.DataSource = dt1;
                        DataGrid1.DataBind();
                        ViewState["SubDt"] = Convert.ToString(dt2.Rows[0]["SubDt"]);
                        ViewState["List"] = dt1;
                    }
                }
                else
                {
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                }
            }
            finally
            {
                ds = null;
                dt1 = null;
                dt2 = null;
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

            DataTable dt = (DataTable)ViewState["List"];
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();
            string vFileNm = "";
            vFileNm = "attachment;filename=Monitoring_Compliance.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' widht='100%'>");
            htw.WriteLine("<tr><td align='center' colspan='10'><b><u><font size='5'>" + gblValue.CompName + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align='center' colspan='10'>" + CGblIdGenerator.GetBranchAddress1("0000") + "</td></tr>");
            htw.WriteLine("<tr><td align='center' colspan='10'><b><u><font size='3'>Monitoring Compliance Report</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align='center' colspan='10'><b><u><font size='3'>Compliance Date : " + hdCmpDt.Value + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align='right'><b>Carried Out Date:</b></td><td align=left'>" + ddlMonth.SelectedItem.Text + "</td><td align=right'><b>Branch:</b></td><td align=left'>" + ddlBranch.SelectedItem.Text + "</td><td align=right'><b>Submission Date:</b></td><td align=left'>" + ViewState["SubDt"] + "</td></tr>");
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</td></tr>");
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