using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.IO;
using System.Web.UI;
using System.Web;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class AttendanceReg : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch(Session[gblValue.UserName].ToString());
                CheckAll();
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);
                this.Menu = false;
                this.PageHeading = "Attendance Register";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.GetModuleByRole(mnuID.mnuHRRptSal);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Attendance Register", false);
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
        /// <param name="pUser"></param>

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId].ToString()));
                if (dt.Rows.Count > 0)
                {
                    chkBranch.DataSource = dt;
                    chkBranch.DataTextField = "BranchName";
                    chkBranch.DataValueField = "BranchCode";
                    chkBranch.DataBind();
                    CheckAll();
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void CheckAll()
        {
            Int32 vRow;
            ViewState["Dtl"] = null;
            if (rblAlSel.SelectedValue == "rbAll")
            {
                for (vRow = 0; vRow < chkBranch.Items.Count; vRow++)
                {
                    chkBranch.Items[vRow].Selected = true;
                }
                chkBranch.Enabled = false;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkBranch.Items.Count; vRow++)
                    chkBranch.Items[vRow].Selected = false;
                chkBranch.Enabled = true;
            }

            popDetail();



        }

        /// <summary>
        /// 
        /// </summary>
        private void popDetail()
        {
            ViewState["Dtl"] = null;
            string str = "";
            for (int vRow = 0; vRow < chkBranch.Items.Count; vRow++)
            {
                if (chkBranch.Items[vRow].Selected == true)
                {
                    if (str == "")
                        str = chkBranch.Items[vRow].Value;
                    else if (str != "")
                        str = str + "," + chkBranch.Items[vRow].Value;
                }
            }
            if (str == "")
                ViewState["Dtl"] = 0;
            else
                ViewState["Dtl"] = str;
        }

        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
            popDetail();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            popDetail();

        }

        private bool ValidateDate()
        {
            bool vRst = true;
            if (ddlYear.SelectedValue == "-1")
            {
                gblFuction.AjxMsgPopup("Please select the Year...");
                vRst = false;
            }
            if (ddlMonth.SelectedValue == "-1")
            {
                gblFuction.AjxMsgPopup("Please select a Month...");
                vRst = false;
            }

            return vRst;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["Dtl"] = null;
                ViewState["SubDtl"] = null;
                Response.Redirect("~/WebPages/Public/Main.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnExc_Click(object sender, EventArgs e)
        {
            if (ValidateDate() == false)
            {
                return;
            }
            else
            {
                GetData("Excel");
            }
        }



        private void GetData(string pFormat)
        {

            Int32 vFinYrNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
            string vBrCode = "";
            DateTime vFromDt, vLastDt;
            string vSFromDt = "", vFileNm = "";
            Int32 vday = 0;
            DataTable dt = null;
            CReports oRpt = null;
            try
            {
                popDetail();

                vBrCode = ViewState["Dtl"].ToString();

                vSFromDt = ddlMonth.SelectedValue + "/" + "01/" + ddlYear.SelectedValue;
                vFromDt = Convert.ToDateTime(vSFromDt);
                vday = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue));
                vLastDt = vFromDt.AddDays(vday - 1);

                oRpt = new CReports();

                dt = oRpt.HRRptAttandance(vBrCode, vLastDt);
                dt.Columns.Remove("EOID");
                dt.AcceptChanges();

                System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();
                tdx.Controls.Add(DataGrid1);
                tdx.Visible = false;
                vFileNm = "attachment;filename=HR Attendance Report.xls";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
                htw.WriteLine("<tr><td align=center' colspan='33'><b><u><font size='5'>HR Attendance Register in the Month of  " + ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedValue + " </font></u></b></td></tr>");
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
            finally
            {
                dt = null;
                oRpt = null;
            }
        }        
    }
}