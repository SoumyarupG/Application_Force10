using System;
using System.Data;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CENTRUMSME.WebPages.Private.HOReports
{
    public partial class HODemandSheet : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                // PopList();
                // popDetail();
                PopBranch(Session[gblValue.UserName].ToString());
                CheckAll();
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                //txtToDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Consolidate Demand Sheet";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHODemandSheet);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Consolidate Demand Sheet", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void GetBranch()
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
            {
                if (chkBr.Items[vRow].Selected == true)
                {
                    if (strin == "")
                        strin = chkBr.Items[vRow].Value;
                    else
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                }
            }
            ViewState["ID"] = strin;
        }

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
            ViewState["ID"] = null;
            try
            {

                if (dt.Rows.Count > 0)
                {
                    chkBr.DataSource = dt;
                    chkBr.DataTextField = "BranchName";
                    chkBr.DataValueField = "BranchCode";
                    chkBr.DataBind();
                    CheckAll();
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }

        private void CheckAll()
        {
            Int32 vRow;
            string strin = "";
            if (ddlSel.SelectedValue == "C")
            {
                chkBr.Enabled = false;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                {
                    chkBr.Items[vRow].Selected = true;
                    if (strin == "")
                        strin = chkBr.Items[vRow].Value;
                    else
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                }
                ViewState["ID"] = strin;
            }
            else if (ddlSel.SelectedValue == "B")
            {
                ViewState["ID"] = null;
                chkBr.Enabled = true;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                    chkBr.Items[vRow].Selected = false;
            }
        }

        //private void CheckAllCheck()
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

        //protected void rblSel_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    PopList();
        //    CheckAllCheck();
        //    popDetail();
        //}

        protected void ddlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
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

        private void GetData(string pMode)
        {
            GetBranch();
            //string vTitleType = "";
            string vFileNm = "";
            DateTime vFromDt = gblFuction.setDate(txtFrmDt.Text);
            String vToDate = vFromDt.AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy");
            //DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = "";
            //string vRptPath = "";
            string vBranch = Session[gblValue.BrName].ToString();
            DataTable dt = null;
            CReports oRpt = null;
            vBrCode = Session[gblValue.BrnchCode].ToString();
            //if (ddlSel.SelectedValue == "C")
            //    vBrCode = "";
            //else
            //    vBrCode = Convert.ToString(ViewState["ID"]);

            try
            {
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    oRpt = new CReports();
                    if (pMode == "PDF")
                    {
                        //vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\HOClientProfile.rpt";
                        //rptDoc.Load(vRptPath);
                        //dt = oRpt.rptClientProfile(vFromDt, vToDt, vBrCode);
                        //rptDoc.Load(vRptPath);
                        //rptDoc.SetDataSource(dt);
                        //rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                        //rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1("0000"));
                        //rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2("0000"));
                        //rptDoc.SetParameterValue("pBranch", ddlSel.SelectedItem.Text);

                        //rptDoc.SetParameterValue("dtFrom", txtFrmDt.Text);
                        //rptDoc.SetParameterValue("dtTo", txtToDt.Text);
                        //rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Consolidate_Loan_Disbursement_List_" + vTitleType + "_" + txtToDt.Text.Replace("/", "_"));
                        //Response.ClearContent();
                        //Response.ClearHeaders();
                    }
                    else if (pMode == "Excel")
                    {

                        System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                        dt = oRpt.rptHODemandSheet(vFromDt, vBrCode);
                        //string vStr = "";

                        //DataGrid1.DataSource = dt;
                        //DataGrid1.DataBind();
                        //for (int i = 0; i < DataGrid1.Items.Count; i++)
                        //{
                        //    DataGrid1.Items[i].Attributes.Add("class", "textmode");
                        //    DataGrid1.Items[i].Height = 20;
                        //    DataGrid1.Items[i].Cells[0].HorizontalAlign = HorizontalAlign.Right;
                        //    DataGrid1.Items[i].Cells[1].HorizontalAlign = HorizontalAlign.Left;
                        //}

                        //tdx.Controls.Add(DataGrid1);
                        tdx.Visible = false;
                        vFileNm = "attachment;filename=" + txtFrmDt.Text.Replace("/", "") + "_HO_DemandSheet_Report.xls";
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", vFileNm);
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.ms-excel";

                        HttpContext.Current.Response.Write("<style> .txt"+"\r\n"+"{mso-style-parent:style0;mso-number-format:\""+@"\@"+"\""+";} "+"\r\n"+"</style>");
                        Response.Write("<table border='1' cellpadding='0' width='100%'>");
                        Response.Write("<tr><td align=center' colspan='"+dt.Columns.Count+"'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                        Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + " </font></b></td></tr>");
                        Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress2(vBrCode) + " </font></b></td></tr>");
                        Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> Branch - " + vBrCode + " - " + vBranch + " </font></b></td></tr>");
                        Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>  Demand Sheet</font></u></b></td></tr>");
                        Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> From Date - " + txtFrmDt.Text + " - " + vToDate + " </font></b></td></tr>");
                        Response.Write("<tr>");

                        foreach (DataColumn dtCol in dt.Columns)
                        {
                            Response.Write("<td><b>"+dtCol.ColumnName+"<b></td>");
                        }
                        Response.Write("</tr>");
                        foreach (DataRow dr in dt.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                if (dt.Columns[j].ColumnName == "Customer No")
                                {
                                    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dt.Columns[j].ColumnName == "Customer Contact No")
                                {
                                    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dt.Columns[j].ColumnName == "LoanNo")
                                {
                                    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else
                                {
                                    Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                                }
                            }  
                        }
                        Response.Write("</tr>");
                        Response.Write("</table>");
                      
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }

        //protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    CheckAllCheck();
        //    popDetail();
        //}

        //protected void chkDtl_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    popDetail();
        //}

        protected void chkBr_SelectedIndexChanged(object sender, EventArgs e)
        {
            //PopList();
            //CheckAllCheck();
            //popDetail();
        }

        private bool ValidateDate()
        {
            bool vRst = true;
            //if (gblFuction.setDate(txtFrmDt.Text) > gblFuction.setDate(txtToDt.Text)) vRst = false;
            return vRst;
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            if (ValidateDate() == false)
            {
                gblFuction.MsgPopup("Please Set The Valid Date Range.");
                this.Page.SetFocus(txtFrmDt);
                return;
            }
            else
            {
                GetData("PDF");
            }
        }

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            if (ValidateDate() == false)
            {
                gblFuction.MsgPopup("Please Set The Valid Date Range.");
                this.Page.SetFocus(txtFrmDt);
                return;
            }
            else
            {
                GetData("Excel");
            }
        }

        //private void popCO()
        //{
        //    DataTable dt = null;
        //    CEO oCM = null;
        //    string vBrCode = Session[gblValue.BrnchCode].ToString();
        //    DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

        //    try
        //    {
        //        oCM = new CEO();
        //        dt = oCM.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
        //        chkDtl.DataSource = dt;
        //        chkDtl.DataTextField = "EOName";
        //        chkDtl.DataValueField = "EOID";
        //        chkDtl.DataBind();
        //    }
        //    finally
        //    {
        //        oCM = null;
        //        dt = null;
        //    }
        //}

    }
}
