using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.Data;
using System.IO;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class RptMonthWiseLnDtl : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopFinYear();

                PopBranch();
            }
        }
        private void PopFinYear()
        {
            DataTable dt = null;
            CFinYear oFinYr = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oFinYr = new CFinYear();
                dt = oFinYr.GetFinYearList("0000");
                if (dt.Rows.Count > 0)
                {
                    ddlFinYr.DataTextField = "FYear";
                    ddlFinYr.DataValueField = "YrNo";
                    ddlFinYr.DataSource = dt;
                    ddlFinYr.DataBind();
                    //ListItem liSel = new ListItem("<- Select ->", "-1");
                    //ddlFinYr.Items.Insert(0, liSel);
                    Session[gblValue.DistrictId] = dt.Rows[0]["AreaID"];
                    Session[gblValue.AgencyType] = dt.Rows[0]["AgencyType"];
                }
                else
                {
                    Response.Redirect("~/Private/WebPages/Admin/FinancialYear.aspx", false);
                }
            }
            finally
            {
                dt = null;
                oFinYr = null;
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Month Wise Demand Collection";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuMnWiseLnDtl);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Portfolio Activity", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
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
        protected void SetParameterForRptData(string pFormat)
        {
            string vFileNm = "", vRoID = "";
            DataSet ds = null;
            string vBrCode = ViewState["BrCode"].ToString();
            CReports oRpt = new CReports();
            


            //***************************************
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            ds = oRpt.rptMonthWiseColl(Convert.ToInt32(ddlFinYr.SelectedValue), vBrCode);
            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();

            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            vFileNm = "attachment;filename=Month Wise Demand Collection.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
            //htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='12' style='background-color:Orange;'><b><u><font size='5'>Unity Small Finance Bank Ltd.</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='12'><b><u><font size='5'>81/2, Acharya Jagadish Chandra Bose Rd</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='12'><b><u><font size='5'>Year " + ddlFinYr.SelectedItem.Text + "</font></u></b></td></tr>");

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
    }
}