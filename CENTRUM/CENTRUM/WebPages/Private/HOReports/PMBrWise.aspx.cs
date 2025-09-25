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

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class PMBrWise : CENTRUMBase
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
                ViewState["SubDt"] = null; 
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
                this.PageHeading = "Process Management (Branchwise)";                
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";            
                this.GetModuleByRole(mnuID.mnuProcMgmtRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Process Management (Branchwise)", false);
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
                dt = oRO.GetInspPMByBranch(vBrCode);
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
            lblObt.Text = "";
            lblMax.Text = "";
            lblPer.Text = "";
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
                    ds = oRpt.rptPMBranchWise(Convert.ToInt32(ddlMonth.SelectedValue));
                    DataGrid1.DataSource = ds.Tables[0];
                    DataGrid1.DataBind();
                    DataGrid1.DataMember = "Table";
                    ViewState["List"] = ds;
                    //dt1 = ds.Tables[0];
                    //dt2 = ds.Tables[1];
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
                    //    ViewState["SubDt"] = Convert.ToString(dt2.Rows[0]["SubDt"]);
                       // ViewState["List"] = ds;

                    }

                else
                {
                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                    lblObt.Text = "";
                    lblMax.Text = "";
                    lblPer.Text = "";
                    lblObtScore.Text = "";
                    lblWeight.Text = "";
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    lblMax.Text = ds.Tables[1].Rows[0]["MaxMarks"].ToString();
                    lblWeight.Text = ds.Tables[1].Rows[0]["Weightage"].ToString();
                    lblObt.Text = ds.Tables[1].Rows[0]["ObtainMarks"].ToString();
                    lblObtScore.Text = ds.Tables[1].Rows[0]["Obtain Scores"].ToString();
                    lblPer.Text = ds.Tables[1].Rows[0]["Obtain Percentage"].ToString();
                    lblGrd.Text = ds.Tables[1].Rows[0]["Grading"].ToString();
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
            DataSet ds = (DataSet)ViewState["List"];
            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();           
            string vFileNm = "";
            vFileNm = "attachment;filename=Process_Management.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' widht='100%'>");
            htw.WriteLine("<tr><td align='center' colspan='6'><b><u><font size='5'>" + gblValue.CompName + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align='center' colspan='6'>" + CGblIdGenerator.GetBranchAddress1("0000") + "</td></tr>");
            htw.WriteLine("<tr><td align='center' colspan='6'><b><u><font size='3'>Module A: Process Management</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align='right'><b>Carried Out Date:</b></td><td align=left'>" + ddlMonth.SelectedItem.Text + "</td><td align=right'><b>Branch:</b></td><td align=left'>" + ddlBranch.SelectedItem.Text + "</td><td align=right'><b>Submission Date:</b></td><td align=left'>" + ViewState["SubDt"] + "</td></tr>");
            DataGrid1.RenderControl(htw);          
            htw.WriteLine("<tr><td colspan='6'></td></tr>");
            htw.WriteLine("<tr><td>&nbsp&nbsp&nbsp&nbsp&nbsp</td><td align='center'> Max. Marks: " + lblMax.Text + "&nbsp&nbsp&nbsp&nbsp</td>");
            htw.WriteLine("<td align='center'> Weightage: " + lblWeight.Text + "&nbsp&nbsp&nbsp&nbsp&nbsp</td>");
            htw.WriteLine("<td align='center'> Obtain Marks: " + lblObt.Text + "&nbsp&nbsp&nbsp&nbsp&nbsp</td>");
            htw.WriteLine("<td align='center'> Obtain Score: " + lblObtScore.Text + "&nbsp&nbsp&nbsp&nbsp&nbsp</td>");
            htw.WriteLine("<td align='center'> % of Marks: " + lblPer.Text + "&nbsp&nbsp&nbsp&nbsp&nbsp</td>");
            htw.WriteLine("<td align='center'> Grading : " + lblGrd.Text + "</td></tr>");
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