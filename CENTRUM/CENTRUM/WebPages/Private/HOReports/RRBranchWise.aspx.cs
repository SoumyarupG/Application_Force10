using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class RRBranchWise : CENTRUMBase
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
                popBranch();
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
                this.PageHeading = "Risk Rating (Branchwise)";              
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";               
                this.GetModuleByRole(mnuID.mnuRiskRatBrRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Risk Rating (Branchwise)", false);
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
                dt = oRO.GetInspRRByBranch(vBrCode);
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

        /// <summary>
        /// 
        /// </summary>
        private void popBranch()
        {
            DataTable dt = null;
            CUser oUsr = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBranch.Items.Insert(0, oli);
            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBranch.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Please select Branch");
                return;
            }
            DataGrid1.DataSource = null;
            DataGrid1.DataBind();
            ViewState["List"] = null;
            if (ddlBranch.SelectedIndex > 0) popMonth();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null, dt1 = null;
            CReports oRpt = null;
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
                    ds = oRpt.rptRRBranchWise(Convert.ToInt32(ddlMonth.SelectedValue));
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        dt.Rows.RemoveAt(0);
                        //if (dt.Rows[1][0].ToString() == "1")
                        //    dt.Rows[1][0] = "A.1";
                        //if (dt.Rows[2][0].ToString() == "2")
                        //    dt.Rows[2][0] = "A.2";
                        //if (dt.Rows[3][0].ToString() == "3")
                        //    dt.Rows[3][0] = "A.3";
                        //if (dt.Rows[4][0].ToString() == "4")
                        //    dt.Rows[4][0] = "A.4";
                        //if (dt.Rows[5][0].ToString() == "5")
                        //    dt.Rows[5][0] = "A.5";
                        //if (dt.Rows[6][0].ToString() == "6")
                        //    dt.Rows[6][0] = "A.6";
                        //if (dt.Rows[8][0].ToString() == "7")
                        //    dt.Rows[8][0] = "B.1";
                        //if (dt.Rows[9][0].ToString() == "8") 
                        //    dt.Rows[9][0] = "B.2";
                        //if (dt.Rows[10][0].ToString() == "9") 
                        //    dt.Rows[10][0] = "B.3";
                        //if (dt.Rows[11][0].ToString() == "10") 
                        //    dt.Rows[11][0] = "B.4";
                        //if (dt.Rows[12][0].ToString() == "11") 
                        //    dt.Rows[12][0] = "B.5";
                        //if (dt.Rows[13][0].ToString() == "12") 
                        //    dt.Rows[13][0] = "B.6";
                        //if (dt.Rows[14][0].ToString() == "13") 
                        //    dt.Rows[14][0] = "B.7";
                        //if (dt.Rows[15][0].ToString() == "14") 
                        //    dt.Rows[15][0] = "B.8";
                        //if (dt.Rows[16][0].ToString() == "15") 
                        //    dt.Rows[16][0] = "B.9";
                        //if (dt.Rows[17][0].ToString() == "16") 
                        //    dt.Rows[17][0] = "B.10";
                        //if (dt.Rows[18][0].ToString() == "17") 
                        //    dt.Rows[18][0] = "B.11";
                        //if (dt.Rows[19][0].ToString() == "18") 
                        //    dt.Rows[19][0] = "B.12";
                        //if (dt.Rows[20][0].ToString() == "19") 
                        //    dt.Rows[20][0] = "B.13";
                        //if (dt.Rows[21][0].ToString() == "20") 
                        //    dt.Rows[21][0] = "B.14";
                        //if (dt.Rows[22][0].ToString() == "21") 
                        //    dt.Rows[22][0] = "B.15";
                        //if (dt.Rows[23][0].ToString() == "22") 
                        //    dt.Rows[23][0] = "B.16";
                        //if (dt.Rows[24][0].ToString() == "23") 
                        //    dt.Rows[24][0] = "B.17";
                        //if (dt.Rows[26][0].ToString() == "24") 
                        //    dt.Rows[26][0] = "C.1";
                        //if (dt.Rows[27][0].ToString() == "25") 
                        //    dt.Rows[27][0] = "C.2";
                        dt.AcceptChanges();
                        DataGrid1.DataSource = dt;
                        DataGrid1.DataBind();
                        ViewState["List"] = dt;
                        hdSubDt.Value = Convert.ToString(dt1.Rows[0]["SubDt"]);
                    }
                }
                else
                {
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                    ViewState["List"] = null;
                }
            }
            finally
            {
                ds = null;
                dt = null; dt1 = null;
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
            if (ddlMonth.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Please select Date Range.");
                return;
            }     
            DataTable ds = (DataTable)ViewState["List"];           
            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();
            string vFileNm = "";
            vFileNm = "attachment;filename=Risk_Rating.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan='8'><b><u><font size='5'>" + gblValue.CompName + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='8'>" + CGblIdGenerator.GetBranchAddress1("0000") + "</td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='8'><b><u><font size='3'>Risk Rating Branch Wise</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=right'><b>Date Range:</b></td><td align=left'>" + ddlMonth.SelectedItem.Text + "</td><td align=right'><b>Branch:</b></td><td align=left'>" + ddlBranch.SelectedItem.Text + "</td><td></td>td></td><td align=right'><b>Submission Date:</b></td><td align=left'>" + hdSubDt.Value + "</td></tr>");
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
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
    }
}