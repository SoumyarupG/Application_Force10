using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.IO;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class RRConsolidated : CENTRUMBase
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
                this.PageHeading = "Risk Rating Consolidated";                
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";               
                this.GetModuleByRole(mnuID.mnuRiskRatCnld);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Risk Rating Consolidated", false);
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
        protected void btnShow_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null;
            CReports oRpt = null;
            string vMarks = "", vPrvMarks = "";
            double vCreditRisk = 0, vOptRisk = 0, vMrktRisk = 0, vTotCrRtk = 0, vPrevCrRtk = 0;
            try
            {
                if (txtFrmDt.Text != "" || txtToDt.Text != "")
                {                    
                    DateTime vStDate = gblFuction.setDate(txtFrmDt.Text);
                    DateTime vEndDate = gblFuction.setDate(txtToDt.Text);
                    DateTime vPrFrDt = vStDate.AddMonths(-1);
                    DateTime vPrToDt = vStDate.AddDays(-(vStDate.Day));
                 
                    oRpt = new CReports();
                    ds = oRpt.rptRRConsolidated(vStDate, vEndDate, vPrFrDt, vPrToDt);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        dt.Rows.RemoveAt(0);
                        //dt.Rows[1][0] = "A.1";
                        //dt.Rows[2][0] = "A.2";
                        //dt.Rows[3][0] = "A.3";
                        //dt.Rows[4][0] = "A.4";
                        //dt.Rows[5][0] = "A.5";
                        //dt.Rows[6][0] = "A.6";
                        //dt.Rows[9][0] = "B.1";
                        //dt.Rows[10][0] = "B.2";
                        //dt.Rows[11][0] = "B.3";
                        //dt.Rows[12][0] = "B.4";
                        //dt.Rows[13][0] = "B.5";
                        //dt.Rows[14][0] = "B.6";
                        //dt.Rows[15][0] = "B.7";
                        //dt.Rows[16][0] = "B.8";
                        //dt.Rows[17][0] = "B.9";
                        //dt.Rows[18][0] = "B.10";
                        //dt.Rows[19][0] = "B.11";
                        //dt.Rows[20][0] = "B.12";
                        //dt.Rows[21][0] = "B.13";
                        //dt.Rows[22][0] = "B.14";
                        //dt.Rows[23][0] = "B.15";
                        //dt.Rows[24][0] = "B.16";
                        //dt.Rows[25][0] = "B.17";
                        //dt.Rows[28][0] = "C.1";
                        //dt.Rows[29][0] = "C.2";      
                        dt.AcceptChanges();
                        vCreditRisk = Math.Round((Convert.ToDouble(dt.Rows[7][6]) / Convert.ToDouble(dt.Rows[7][2])) * 100, 2);
                        vOptRisk = Math.Round((Convert.ToDouble(dt.Rows[26][6]) / Convert.ToDouble(dt.Rows[26][2])) * 100, 2);
                        vMrktRisk = Math.Round((Convert.ToDouble(dt.Rows[30][6]) / Convert.ToDouble(dt.Rows[30][2])) * 100, 2);
                        hdCreditRisk.Value = vCreditRisk.ToString();
                        hdOptRisk.Value = vOptRisk.ToString();
                        hdMrktRisk.Value = vMrktRisk.ToString();
                        vTotCrRtk = Math.Round((Convert.ToDouble(dt.Rows[31][6]) / Convert.ToDouble(dt.Rows[31][2])) * 100, 2);
                        vPrevCrRtk = Math.Round((Convert.ToDouble(dt.Rows[31][7]) / Convert.ToDouble(dt.Rows[31][2])) * 100, 2);

                        if (vTotCrRtk > 0 && vTotCrRtk < 59)
                            vMarks = "High";
                        else if (vTotCrRtk > 60 && vTotCrRtk < 69)
                            vMarks = "Medium";
                        else if (vTotCrRtk > 70 && vTotCrRtk < 100)
                            vMarks = "Low";

                        if (vPrevCrRtk > 0 && vPrevCrRtk < 59)
                            vPrvMarks = "High";
                        else if (vPrevCrRtk > 60 && vPrevCrRtk < 69)
                            vPrvMarks = "Medium";
                        else if (vPrevCrRtk > 70 && vPrevCrRtk < 100)
                            vPrvMarks = "Low";
                        dt.Rows[32][5] = vTotCrRtk;
                        dt.Rows[32][6] = vMarks;
                        dt.Rows[32][7] = vPrvMarks;
                        dt.Rows[32][1] = "  Risk Assigned  " + vTotCrRtk + " (%) ";
                        dt.Rows[32][3] = 0;
                        dt.AcceptChanges();
                        DataGrid1.DataSource = dt;
                        DataGrid1.DataBind();
                        ViewState["List"] = dt;
                    }
                }
            }
            finally
            {
                ds = null;
                dt = null;
                oRpt = null;
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
                ViewState["List"] = null;
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            if (txtFrmDt.Text == "" || txtToDt.Text == "")
            {
                gblFuction.MsgPopup("Please select From To Date.");
                return;
            }
            DataTable dt = null;            
            try
            {
                dt = (DataTable)ViewState["List"];       
                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();
                string vFileNm = "";
                vFileNm = "attachment;filename=Risk_Rating.xls";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                htw.WriteLine("<table border='0' widht='100%'>");
                htw.WriteLine("<tr><td align=center' colspan='8'><b><u><font size='5'>" + gblValue.CompName + "</font></u></b></td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='8'>" + CGblIdGenerator.GetBranchAddress1("0000") + "</td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='8'><b><u><font size='3'>Risk Rating Consolidated</font></u></b></td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='8'><b><u><font size='3'>From Date: " + txtFrmDt.Text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; To Date: " + txtToDt.Text + "</font></u></b></td></tr>");
                DataGrid1.RenderControl(htw);
                htw.WriteLine("</td></tr>");
                htw.WriteLine("<tr><td colspan='8'></td></tr>");
                htw.WriteLine("<tr><td colspan='2'></td><td colspan='2'><b>Credit Risk:</b></td><td>" + hdCreditRisk.Value + "%" + "</td></tr>");                
                htw.WriteLine("<tr><td colspan='2'><b>Operational Risk:</b></td><td>" + hdOptRisk.Value + "%" + "</td></tr>");             
                htw.WriteLine("<tr><td colspan='2'><b>Market Risk:</b></td><td>" + hdMrktRisk.Value + "%" + "</td></tr>");
                htw.WriteLine("<tr><td colspan='8'><br></td></tr>");
                htw.WriteLine("<tr><td colspan='8'>Parameter of Rating: Low Risk: 70% to 100%, Medium Risk: 60% to 69% and High Risk: 0% to 59% </td></tr>");
                htw.WriteLine("</table>");
                Response.ClearContent();
                Response.AddHeader("content-disposition", vFileNm);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Write(sw.ToString());
                Response.End();
            }
            finally
            {
                dt = null;                
            }
        }
    }
}