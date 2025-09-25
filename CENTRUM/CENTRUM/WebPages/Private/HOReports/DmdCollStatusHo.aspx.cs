using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using FORCECA;
using FORCEBA;
//using ClosedXML.Excel;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class DmdCollStatusHo : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtFDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtTDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                ViewState["ID"] = null;
                PopList();
                PopBranch();
                CheckAll();
                //popDetail();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Demand And Collection Status";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHODmndColSts);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Demand & Collection Status Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

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
        
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("PDF");
        }

        private void PopList()
        {
            Int32 vRow;
            string strin = "";
            ViewState["ID"] = null;
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            CEO oRo = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            
            //if (rdbOpt.SelectedValue == "rdbLoanType")
            //{
            //    oCG = new CGblIdGenerator();
            //    dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanType", "LoanTypeMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
            //    chkDtl.DataSource = dt;
            //    chkDtl.DataTextField = "LoanType";
            //    chkDtl.DataValueField = "LoanTypeId";
            //    chkDtl.DataBind();
            //}
            if (rdbOpt.SelectedValue == "rdbProduct")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "ProductId", "Product", "LoanProductMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "Product";
                chkDtl.DataValueField = "ProductId";
                chkDtl.DataBind();
            }

            if (rdbOpt.SelectedValue == "rdbAll")
            {
                chkDtl.DataSource = null;
                chkDtl.DataBind();
            }

            if (rdbSel.SelectedValue == "rdbAll")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                {
                    chkDtl.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkDtl.Items[vRow].Value + "";
                    }
                }
            }
            else if (rdbSel.SelectedValue == "rdbSelct")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                {
                    chkDtl.Items[vRow].Selected = false;
                }
            }
            ViewState["ID"] = strin;
        }

        private void PopBranch()
        {
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            DataTable dt = null;
            CUser oUsr = null;
            //string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));

            chkBrDtl.DataSource = dt;
            chkBrDtl.DataTextField = "BranchName";
            chkBrDtl.DataValueField = "BranchCode";
            chkBrDtl.DataBind();

            if (rblAlSel.SelectedValue == "rbAll")
            {
                chkBrDtl.Enabled = false;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = false;
                }
            }
            ViewState["BrCode"] = strin;
        }

        private void CheckAll()
        {
            Int32 vRow;
            string strin = "";
            if (rdbSel.SelectedValue == "rdbAll")
            {
                //chkDtl.Enabled = false;
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                {
                    chkDtl.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkDtl.Items[vRow].Value + "";
                    }
                }
                ViewState["ID"] = strin;
            }
            else if (rdbSel.SelectedValue == "rdbSelct")
            {
                ViewState["ID"] = null;
                chkDtl.Enabled = true;
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = false;

            }
        }
        //private void CheckAll()
        //{
        //    Int32 vRow;
        //    if (rblAlSel.SelectedValue == "rbAll")
        //    {
        //        for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
        //            chkDtl.Items[vRow].Selected = true;
        //        chkDtl.Enabled = false;
        //    }
        //    else if (rblAlSel.SelectedValue == "rbSel")
        //    {
        //        for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
        //            chkDtl.Items[vRow].Selected = false;
        //        chkDtl.Enabled = true;
        //    }
        //}

        //private void popDetail()
        //{
        //    ViewState["Dtl"] = null;
        //    string str = "";
        //    for (int vRow = 0; vRow < chkDtl.Items.Count; vRow++)
        //    {
        //        if (chkDtl.Items[vRow].Selected == true)
        //        {
        //            if (str == "")
        //                str = chkDtl.Items[vRow].Value;
        //            else if (str != "")
        //                str = str + "," + chkDtl.Items[vRow].Value;
        //        }
        //    }
        //    if (str == "")
        //        ViewState["Dtl"] = 0;
        //    else
        //        ViewState["Dtl"] = str;
        //}

        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBrAll();
        }

        protected void rdbOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopList();
            CheckAll();
        }

        protected void rdbSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
        }

        protected void chkDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
            {
                if (chkDtl.Items[vRow].Selected == true)
                {
                    if (strin == "")
                    {
                        strin = chkDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkDtl.Items[vRow].Value + "";
                    }
                }
            }
            //ViewState["BrCode"] = strin;
            ViewState["ID"] = strin;
        }


        protected void chkBrDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
            {
                if (chkBrDtl.Items[vRow].Selected == true)
                {
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
            }
            ViewState["BrCode"] = strin;
        }

        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtFDt.Text);
            DateTime vToDt = gblFuction.setDate(txtTDt.Text);
            string vBrCode = ViewState["BrCode"].ToString();
            string vID = ViewState["ID"].ToString();
            string vRptPath = "",vSumDtl="";
            string vBranch = Session[gblValue.BrName].ToString();
            ReportDocument rptDoc = new ReportDocument();
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            vSumDtl = "D";
            //vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DmdColStat.rpt";
            dt = oRpt.rptDlyDemand(vFromDt, vToDt, vBrCode, rdbSel.Text, vID);
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();
            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            string vFileNm = "attachment;filename=Demand_Collection_Status_Report.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
            Response.Write("<table border='1' cellpadding='0' widht='100%'>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.CompName + "</font></u></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.Address1 + "</font></u></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.Address2 + "</font></u></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + vBranch + "</font></u></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>Demand Collection Status Report</font></u></b></td></tr>");
            
            Response.Write("<tr><td align='center' colspan='" + dt.Columns.Count + "'><b>From : " + txtFDt.Text + " To : " + txtTDt.Text + "</b></td></tr>");
            Response.Write("<tr><td></td></tr>");
            string tab = string.Empty;
            Response.Write("<tr>");
            foreach (DataColumn dtcol in dt.Columns)
            {
                Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
            }
            Response.Write("</tr>");
            //foreach (DataColumn dtcol in dt.Columns)
            //{
            //    Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
            //}
            //Response.Write("</tr>");
            foreach (DataRow dtrow in dt.Rows)
            {
                Response.Write("<tr style='height:20px;'>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    if (dt.Columns[j].ColumnName == "LoanNo")
                    {
                        Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                    }

                    else
                    {
                        Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
                    }
                }
                Response.Write("</tr>");
            }
            Response.Write("</table>");
            Response.End();
        }
        private void CheckBrAll()
        {
            Int32 vRow;
            string strin = "";
            if (rblAlSel.SelectedValue == "rbAll")
            {
                chkBrDtl.Enabled = false;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
                ViewState["BrCode"] = strin;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                ViewState["BrCode"] = null;
                chkBrDtl.Enabled = true;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    chkBrDtl.Items[vRow].Selected = false;

            }
        }
    }
}
