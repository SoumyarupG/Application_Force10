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
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class DmndColl : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopList();

                string vBrCode = (string)Session[gblValue.BrnchCode];
                Int32 vBrId = Convert.ToInt32(vBrCode);
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                DataTable dt = null;
                CGblIdGenerator oCG = null;
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanType", "LoanTypeMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                ddlLT.DataSource = dt;
                ddlLT.DataTextField = "LoanType";
                ddlLT.DataValueField = "LoanTypeId";
                ddlLT.DataBind();
                PopBranch();
                PopState();
                txtDtFrm.Enabled = false;
                txtToDt.Enabled = false;
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
                this.PageHeading = "Date Wise Demand And Collection";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.nmuDemndCollRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Date Wise Demand And Collection", false);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopList()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            Int32 vBrId = Convert.ToInt32(vBrCode);
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CEO oRO = null;
            string strin="";
            ViewState["Eoid"]=null;

            if (rbList.SelectedValue == "rbLT")
            {
                ddlLT.Enabled = true;
                ddlFS.Items.Clear();
                ddlFS.Enabled = false;
                ddlLC.Items.Clear();
                ddlLC.Enabled = false;
                //rblRO.Enabled = false;

                //oCG = new CGblIdGenerator();
                //dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanType", "LoanTypeMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                //ddlLT.DataSource = dt;
                //ddlLT.DataTextField = "LoanType";
                //ddlLT.DataValueField = "LoanTypeId";
                //ddlLT.DataBind();
                
            }
            if (rbList.SelectedValue == "rbFS")
            {
                //ddlLT.Items.Clear();
                ddlLT.Enabled = false;
                ddlFS.Enabled = true;
                ddlLC.Items.Clear();
                ddlLC.Enabled = false;
                //rblRO.Enabled = false;

                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "FundSourceID", "FundSource", "FundSourceMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                ddlFS.DataSource = dt;
                ddlFS.DataTextField = "FundSource";
                ddlFS.DataValueField = "FundSourceID";
                ddlFS.DataBind();
                ListItem oL1 = new ListItem("<-- Select -->", "-1");
                ddlFS.Items.Insert(0, oL1);
            }
            if (rbList.SelectedValue == "rbRO")
            {
                //ddlLT.Items.Clear();
                ddlLT.Enabled = false;
                ddlRO.Enabled = true;
                ddlLC.Items.Clear();
                ddlLC.Enabled = false;
                rblRO.Enabled = true;

                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlRO.DataSource = dt;
                ddlRO.DataTextField = "EoName";
                ddlRO.DataValueField = "Eoid";
                ddlRO.DataBind();
                ListItem oL1 = new ListItem("<-- Select -->", "-1");
                ddlRO.Items.Insert(0, oL1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (strin == "")
                    {
                        strin = dt.Rows[i]["Eoid"].ToString();
                    }
                    else
                    {
                        strin = strin + "," + dt.Rows[i]["Eoid"].ToString();
                    }
                }
                ViewState["Eoid"]=strin;
            }
            if (rbList.SelectedValue == "rbLC")
            {
                //ddlLT.Items.Clear();
                ddlLT.Enabled = false;
                ddlFS.Items.Clear();
                ddlFS.Enabled = false;
                ddlRO.Items.Clear();
                ddlRO.Enabled = false;
                ddlLC.Enabled = true;
                //rblRO.Enabled = false;

                ddlLC.Items.Clear();
                ListItem oLx = new ListItem();
                oLx.Text = "<-- Select -->";
                oLx.Value = "-1";
                ddlLC.Items.Add(oLx);

                ListItem oLs1 = new ListItem();
                oLs1.Text = "1st Cycle";
                oLs1.Value = "1";
                ddlLC.Items.Add(oLs1);

                ListItem oLs2 = new ListItem();
                oLs2.Text = "2nd Cycle";
                oLs2.Value = "2";
                ddlLC.Items.Add(oLs2);

                ListItem oLs3 = new ListItem();
                oLs3.Text = "3rd Cycle";
                oLs3.Value = "3";
                ddlLC.Items.Add(oLs3);

                ListItem oLs4 = new ListItem();
                oLs4.Text = "4th Cycle";
                oLs4.Value = "4";
                ddlLC.Items.Add(oLs4);

                ListItem oLs5 = new ListItem();
                oLs5.Text = "5th Cycle and Above";
                oLs5.Value = "5";
                ddlLC.Items.Add(oLs5);


            }

            if (rbList.SelectedValue == "rbALL")
            {
                //ddlLT.Items.Clear();
                ddlLT.Enabled = false;
                ddlFS.Items.Clear();
                ddlFS.Enabled = false;
                ddlLC.Items.Clear();
                ddlLC.Enabled = false;
            }
        }

        protected void rbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopList();
        }

        protected void btnCSV_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("CSV");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = ViewState["BrCode"].ToString();
            string vRptPath = "", vTitle = "", vMode = "A";
            string vBranch = Session[gblValue.BrName].ToString();
            //ReportDocument rptDoc = new ReportDocument();
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            Int32 vFSID = 0, vLCID = 0;
            string vLTID = "";
            string vROID = "", vROval="";

            if (rbList.SelectedValue == "rbLT")
            {
                vTitle = "Demand and Collection Report (Loan Scheme Wise)";
                vMode = "L";
                vLTID = ddlLT.SelectedValues.Replace("|", ",");
                //if (Convert.ToInt32(ddlLT.SelectedValue) > 0)
                //    vLTID = Convert.ToInt32(ddlLT.SelectedValue);
            }
            if (rbList.SelectedValue == "rbFS")
            {
                vTitle = "Demand and Collection Report (Funder Source Wise)";
                vMode = "F";
                if (Convert.ToInt32(ddlFS.SelectedValue) > 0)
                    vFSID = Convert.ToInt32(ddlFS.SelectedValue);
            }
            if (rbList.SelectedValue == "rbLC")
            {
                vTitle = "Demand and Collection Report (Loan Cycle Wise)";
                vMode = "C";
                if (Convert.ToInt32(ddlLC.SelectedValue) > 0)
                    vLCID = Convert.ToInt32(ddlLC.SelectedValue);
            }
            if (rbList.SelectedValue == "rbRO")
            {
                vTitle = "Demand and Collection Report (RO Wise)";
                vMode = "E";
                if (rblRO.SelectedValue != "")
                    vROID = rblRO.SelectedValue;
                vROval = ViewState["Eoid"].ToString();
            }
            if (rbList.SelectedValue == "rbALL")
            {
                vTitle = "Demand and Collection Report ";
                vMode = "A";
            }

            TimeSpan t = vToDt - vFromDt;
            if (t.TotalDays > 2)
            {
                gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                return;
            }

            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DmdCollGrp.rpt";
            //dt = oRpt.rptDeemanCollBalance(vFromDt, vToDt, vLTID, vFSID, vLCID, vROID, vROval,  vBrCode, vMode);
            vROID = rblRO.SelectedValue;
            if (ddlProject.SelectedItem.Value == "J")
            {
                dt = oRpt.rptDCBxlsNew(vFromDt, vToDt, vBrCode, vROID);
            }
            else if (ddlProject.SelectedItem.Value == "M")
            {
                dt = oRpt.rptDCBxlsNew_MEL(vFromDt, vToDt, vBrCode, vROID);
            }
            else if (ddlProject.SelectedItem.Value == "S")
            {
                dt = oRpt.rptDCBxlsNew_SARAL(vFromDt, vToDt, vBrCode, vROID);
            }
            
            //if (rbList.SelectedValue != "rbRO")
            //{
            //    using (ReportDocument rptDoc = new ReportDocument())
            //    {

            //        rptDoc.Load(vRptPath);
            //        rptDoc.SetDataSource(dt);
            //        rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
            //        rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
            //        rptDoc.SetParameterValue("pAddress2", "");
            //        rptDoc.SetParameterValue("pTitle", vTitle);
            //        rptDoc.SetParameterValue("pBranch", vBranch);
            //        rptDoc.SetParameterValue("dtFrom", txtDtFrm.Text);
            //        rptDoc.SetParameterValue("dtTo", txtToDt.Text);
            //        rptDoc.SetParameterValue("pMode", vMode);
            //        if (pMode == "PDF")
            //            rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Demand_Collection_Balance" + txtToDt.Text.Replace("/", "_"));
            //        else if (pMode == "Excel")
            //            rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Demand_Collection_Balance" + txtToDt.Text.Replace("/", "_") + ".xls");
            //        rptDoc.Dispose();
            //        Response.ClearHeaders();
            //        Response.ClearContent();
            //    }
            //}
            //else
            //{
            //    if (pMode == "Excel")
            //    {
            //        System.Web.UI.WebControls.DataGrid dgDemandRO = new System.Web.UI.WebControls.DataGrid();
            //        if (vROID == "D")
            //        {
            //            dgDemandRO.DataSource = dt;
            //            dgDemandRO.DataBind();
            //            tdx.Controls.Add(dgDemandRO);
            //            tdx.Visible = false;
            //            string vFileNm = "attachment;filename=Demand Collection Details";
            //            StringWriter sw = new StringWriter();
            //            HtmlTextWriter htw = new HtmlTextWriter(sw);
            //            htw.WriteLine("<table border='0' cellpadding='5' width='100%'>");
            //            htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
            //            //htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='5'>" + Session[gblValue.CompName].ToString() + "</font></u></b></td></tr>");
            //            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.CompName + "</font></u></b></td></tr>");
            //            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.Address1 + "</font></u></b></td></tr>");
            //            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.Address2 + "</font></u></b></td></tr>");
            //            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + vBranch + "</font></u></b></td></tr>");
            //            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>Demand Collection Report(Details)</font></u></b></td></tr>");
            //            htw.WriteLine("<tr><td></td></tr>");
            //            htw.WriteLine("<tr><td align='right' colspan='" + ((dgDemandRO.Columns.Count - 1) / 2) + "'><b>From : " + gblFuction.setDate(txtDtFrm.Text) + " To : " + gblFuction.setDate(txtToDt.Text));
            //            htw.WriteLine("<tr><td></td></tr>");
            //            dgDemandRO.RenderControl(htw);
            //            htw.WriteLine("</table>");
            //            dgDemandRO.DataSource = null;
            //            dgDemandRO.DataBind();
            //            Response.ClearContent();
            //            Response.AddHeader("content-disposition", vFileNm);
            //            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //            Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
            //            this.EnableViewState = false;
            //            Response.Write(sw.ToString());
            //            Response.End();
            //        }
            //        if (vROID == "S")
            //        {
            //            dgDemandRO.DataSource = dt;
            //            dgDemandRO.DataBind();
            //            tdx.Controls.Add(dgDemandRO);
            //            tdx.Visible = false;
            //            string vFileNm = "attachment;filename=Demand Collection Summary";
            //            StringWriter sw = new StringWriter();
            //            HtmlTextWriter htw = new HtmlTextWriter(sw);
            //            htw.WriteLine("<table border='0' cellpadding='5' width='100%'>");
            //            htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
            //            //htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='5'>" + Session[gblValue.CompName].ToString() + "</font></u></b></td></tr>");
            //            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.CompName + "</font></u></b></td></tr>");
            //            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.Address1 + "</font></u></b></td></tr>");
            //            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.Address2 + "</font></u></b></td></tr>");
            //            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + vBranch + "</font></u></b></td></tr>");
            //            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>Demand Collection Report(Summary)</font></u></b></td></tr>");
            //            htw.WriteLine("<tr><td></td></tr>");
            //            htw.WriteLine("<tr><td align='right' colspan='" + ((dgDemandRO.Columns.Count - 1) / 2) + "'><b>From : " + gblFuction.setDate(txtDtFrm.Text) + " To : " + gblFuction.setDate(txtToDt.Text));
            //            htw.WriteLine("<tr><td></td></tr>");
            //            dgDemandRO.RenderControl(htw);
            //            htw.WriteLine("</table>");
            //            dgDemandRO.DataSource = null;
            //            dgDemandRO.DataBind();
            //            Response.ClearContent();
            //            Response.AddHeader("content-disposition", vFileNm);
            //            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //            Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
            //            this.EnableViewState = false;
            //            Response.Write(sw.ToString());
            //            Response.End();
            //        }
            //    }
                
            if (pMode == "Excel")
            {
                System.Web.UI.WebControls.DataGrid dgDemandRO = new System.Web.UI.WebControls.DataGrid();
                if (vROID == "D")
                {
                    dgDemandRO.DataSource = dt;
                    dgDemandRO.DataBind();
                    tdx.Controls.Add(dgDemandRO);
                    tdx.Visible = false;
                    string vFileNm = "attachment;filename=Demand Collection Details.xls";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    htw.WriteLine("<table border='0' cellpadding='5' width='100%'>");
                    htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
                    //htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='5'>" + Session[gblValue.CompName].ToString() + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.CompName + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.Address1 + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.Address2 + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + vBranch + "</font></u></b></td></tr>");
                    if (ddlProject.SelectedItem.Value == "J")
                    {
                        htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>Demand Collection Report JLG(Details)</font></u></b></td></tr>");
                    }
                    else
                    {
                        htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>Demand Collection Report MEL(Details)</font></u></b></td></tr>");
                    }
                    
                    htw.WriteLine("<tr><td></td></tr>");
                    htw.WriteLine("<tr><td align='center' colspan='" + dt.Columns.Count + "'><b>From : " + txtDtFrm.Text + " To : " + txtToDt.Text);
                    htw.WriteLine("<tr><td></td></tr>");
                    dgDemandRO.RenderControl(htw);
                    htw.WriteLine("</table>");
                    dgDemandRO.DataSource = null;
                    dgDemandRO.DataBind();
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    this.EnableViewState = false;
                    Response.Write(sw.ToString());
                    Response.End();
                }
                if (vROID == "S")
                {
                    dgDemandRO.DataSource = dt;
                    dgDemandRO.DataBind();
                    tdx.Controls.Add(dgDemandRO);
                    tdx.Visible = false;
                    string vFileNm = "attachment;filename=Demand Collection Summary.xls";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    htw.WriteLine("<table border='0' cellpadding='5' width='100%'>");
                    htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
                    //htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='5'>" + Session[gblValue.CompName].ToString() + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.CompName + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.Address1 + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.Address2 + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + vBranch + "</font></u></b></td></tr>");
                    if (ddlProject.SelectedItem.Value == "J")
                        htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>Demand Collection Report JLG(Summary)</font></u></b></td></tr>");
                    else
                        htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>Demand Collection Report MEL(Summary)</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td></td></tr>");
                    htw.WriteLine("<tr><td align='center' colspan='" + dt.Columns.Count + "'><b>From : " + txtDtFrm.Text + " To : " + txtToDt.Text);
                    htw.WriteLine("<tr><td></td></tr>");
                    dgDemandRO.RenderControl(htw);
                    htw.WriteLine("</table>");
                    dgDemandRO.DataSource = null;
                    dgDemandRO.DataBind();
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    this.EnableViewState = false;
                    Response.Write(sw.ToString());
                    Response.End();
                }
            }
            else
            {
                PrintTxt(dt);
            }
            
        }

        private void PrintTxt(DataTable dt)
        {
            string vFolderPath = "C:\\BijliReport";
            string vFileNm = "";
            vFileNm = vFolderPath + "\\DateWiseDemandCollection.txt";

            try
            {
                if (System.IO.Directory.Exists(vFolderPath))
                {
                    foreach (var file in Directory.GetFiles(vFolderPath))
                    {
                        if (File.Exists(vFileNm) == true)
                            File.Delete(vFileNm);
                    }
                }
                else
                {
                    Directory.CreateDirectory(vFolderPath);
                }
                Write(dt, vFileNm);
                downloadfile(vFileNm);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                gblFuction.MsgPopup("Done");
                btnExit.Enabled = true;

            }
        }


        private void Write(DataTable dt, string outputFilePath)
        {
            int[] maxLengths = new int[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                maxLengths[i] = dt.Columns[i].ColumnName.Length;
                foreach (DataRow row in dt.Rows)
                {
                    if (!row.IsNull(i))
                    {
                        int length = row[i].ToString().Length;
                        if (length > maxLengths[i])
                        {
                            maxLengths[i] = length;
                        }
                    }
                }
            }
            using (StreamWriter sw = new StreamWriter(outputFilePath, false))
            {

                for (int i = 0; i < dt.Columns.Count; i++)
                {

                    sw.Write(dt.Columns[i].ColumnName.ToString().Trim() + '|');

                }
                sw.WriteLine();

                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!row.IsNull(i))
                        {
                            sw.Write(row[i].ToString().Trim() + '|');
                        }

                    }
                    sw.WriteLine();
                }
                sw.Close();
            }
        }

        private void downloadfile(string filename)
        {

            // check to see that the file exists 
            if (File.Exists(filename))
            {
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(filename);
                Response.End();
                File.Delete(filename);
            }
            else
            {
                gblFuction.AjxMsgPopup("File could not be found");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("PDF");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
        }

        //-----------------------Branch--------------------
        private void PopBranch()
        {
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            DataTable dt = null;
            CUser oUsr = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
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
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                ViewState["BrCode"] = null;
                chkBrDtl.Enabled = true;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    chkBrDtl.Items[vRow].Selected = false;

            }
            ViewState["BrCode"] = strin;
        }

        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBrAll();
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

        private void PopState()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            oCG = new CGblIdGenerator();
            dt = oCG.PopComboMIS("N", "N", "AA", "StateId", "StateName", "StateMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
            ddlState.DataSource = dt;
            ddlState.DataTextField = "StateName";
            ddlState.DataValueField = "StateId";
            ddlState.DataBind();
        }

        protected void btnStateWise_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CUser oUsr = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            string pUser = Session[gblValue.UserName].ToString();
            DateTime vLogDt = gblFuction.setDate(txtToDt.Text.ToString());
            chkBrDtl.Items.Clear();
            ViewState["Id"] = null;
            try
            {
                oCG = new CGblIdGenerator();
                oUsr = new CUser();
                ViewState["Id"] = null;
                if (vBrCode == "0000")
                {
                    dt = oUsr.GetBranchByState(pUser, Convert.ToInt32(Session[gblValue.RoleId]), ddlState.SelectedValues.Replace("|", ","), ddlBrType.SelectedValue);
                    if (dt.Rows.Count > 0)
                    {
                        chkBrDtl.DataSource = dt;
                        chkBrDtl.DataTextField = "BranchName";
                        chkBrDtl.DataValueField = "BranchCode";
                        chkBrDtl.DataBind();
                        if (rblAlSel.SelectedValue == "rbAll")
                            CheckBrAll();
                    }
                }
                else
                {
                    dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, vBrCode);
                    chkBrDtl.DataSource = dt;
                    chkBrDtl.DataTextField = "Name";
                    chkBrDtl.DataValueField = "BranchCode";
                    chkBrDtl.DataBind();
                    if (rblAlSel.SelectedValue == "rbAll")
                        CheckBrAll();
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
                oCG = null;
            }

        }
    }
}
