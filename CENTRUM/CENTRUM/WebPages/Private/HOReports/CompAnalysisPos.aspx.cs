using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.IO;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class CompAnalysisPos : CENTRUMBase 
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
                txtAsOn.Text = Session[gblValue.LoginDate].ToString();
                PopList();
                CheckAll();
                popDetail();
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Comparative Analysis";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHOComAnls);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Comparative  Analysis Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }


        private void PopList()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            Int32 vBrId = Convert.ToInt32(vBrCode);
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            if (rblSel.SelectedValue == "rbZ")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "ZoneId", "ZoneName", "ZoneMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "ZoneName";
                chkDtl.DataValueField = "ZoneId";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbS")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "StateId", "StateName", "StateMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "StateName";
                chkDtl.DataValueField = "StateId";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbR")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "RegionId", "RegionName", "RegionMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "RegionName";
                chkDtl.DataValueField = "RegionId";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbD")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "DistrictId", "DistrictName", "DistrictMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "DistrictName";
                chkDtl.DataValueField = "DistrictId";
                chkDtl.DataBind();
            }
            if (rblSel.SelectedValue == "rbB")
            {

                CUser oUsr = null;
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));

                //oCG = new CGblIdGenerator();
                //dt = oCG.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", vBrId, "BranchCode", "AA", vLogDt, "0000");

                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "BranchName";
                chkDtl.DataValueField = "BranchCode";
                chkDtl.DataBind();

            }

            if (rblSel.SelectedValue == "rbL")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "BlockID", "BlockName", "BlockMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "BlockName";
                chkDtl.DataValueField = "BlockID";
                chkDtl.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckAll()
        {
            Int32 vRow;
            if (rblAlSel.SelectedValue == "rbAll")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = true;
                chkDtl.Enabled = false;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = false;
                chkDtl.Enabled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popDetail()
        {
            ViewState["Dtl"] = null;
            string str = "";
            for (int vRow = 0; vRow < chkDtl.Items.Count; vRow++)
            {
                if (chkDtl.Items[vRow].Selected == true)
                {
                    if (str == "")
                        str = chkDtl.Items[vRow].Value;
                    else if (str != "")
                        str = str + "," + chkDtl.Items[vRow].Value;
                }
            }
            if (str == "")
                ViewState["Dtl"] = 0;
            else
                ViewState["Dtl"] = str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopList();
            CheckAll();
            popDetail();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        protected void chkDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            popDetail();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            string vFileNm = "", vType = "", vTypeId = ViewState["Dtl"].ToString();
            DataSet ds = null;
            CReports oRpt = new CReports();
            string vBrCode = (string)Session[gblValue.BrnchCode];
            string vBranch = Session[gblValue.BrName].ToString();
            DateTime vAsOn = gblFuction.setDate(txtAsOn.Text);
            int vNo = 0;
            //***************************************
            if (txtComp.Text != "" || txtComp.Text != "0")
            {
                vNo = Convert.ToInt32(txtComp.Text);
            }
            else
            {
                gblFuction.MsgPopup("Please Enter Comparative  Study Period in Month");
                return;
            }

            if (vTypeId == "")
            {
                gblFuction.MsgPopup("Please Select any Item From the List");
                return;
            }

            //if (ddlCat.SelectedIndex <= 0)
            //{
            //    gblFuction.MsgPopup("Please Select Category");
            //    return;
            //}

            if (rblSel.SelectedValue == "rbZ") vType = "Zone";
            if (rblSel.SelectedValue == "rbS") vType = "State";
            if (rblSel.SelectedValue == "rbR") vType = "Region";
            if (rblSel.SelectedValue == "rbD") vType = "District";
            if (rblSel.SelectedValue == "rbB") vType = "Branch";
            if (rblSel.SelectedValue == "rbL") vType = "Block";
            //***************************************

            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();


            if (ddlCat.SelectedValue == "P")
                ds = oRpt.rptDrillDown(vAsOn, vTypeId, vType, vNo, ddlTime.SelectedValue); // Principal OS
            else if (ddlCat.SelectedValue == "A")
                ds = oRpt.rptDrillDownPARAmt(vAsOn, vTypeId, vType, vNo, ddlTime.SelectedValue);//Over Due Amount
            else if (ddlCat.SelectedValue == "O")
                ds = oRpt.rptDrillDownODOSAmt(vAsOn, vTypeId, vType, vNo, ddlTime.SelectedValue);//OD Client's OS
            else if (ddlCat.SelectedValue == "L")
                ds = oRpt.rptDrillDownClient(vAsOn, vTypeId, vType, vNo, ddlTime.SelectedValue);//Loan Client
            else
                ds = oRpt.rptDrillDownODClient(vAsOn, vTypeId, vType, vNo, ddlTime.SelectedValue);//OD Client
            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();

            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            vFileNm = "attachment;filename=Comparative_Analysis_of_" + ddlCat.SelectedItem.Text.Replace(" ", "_") + "_As_on_" + txtAsOn.Text;
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
            //htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
            if (ddlCat.SelectedValue != "L" && ddlCat.SelectedValue != "C")
            {
                htw.WriteLine("<tr><td align=center' colspan='5'><b><u><font size='5'>Comparative  Analysis of " + ddlCat.SelectedItem.Text + " (Rs in Lacs) " + " As on " + txtAsOn.Text + " </font></u></b></td></tr>");
            }
            else
            {
                htw.WriteLine("<tr><td align=center' colspan='5'><b><u><font size='5'>Comparative  Analysis of " + ddlCat.SelectedItem.Text + " As on " + txtAsOn.Text + " </font></u></b></td></tr>");
            }
            //htw.WriteLine("<tr><td align=center' colspan='13'><b><u><font size='3'>Party Ledger</font></u></b></td></tr>");
            //htw.WriteLine("<tr><td colspan='2'><b>Member No:</b></td><td align='left' colspan='3'><b>" + vMemNo + "</b></td><td colspan='2'><b>Member Name:</b></td><td align='left' colspan='4'><b>" + vMemName + "</b></td></tr>");
            //htw.WriteLine("<tr><td colspan='2'><b>Spouce Name:</b></td><td align='left' colspan='3'><b>" + vSpouseNm + "</b></td><td colspan='2'><b>Fund Source:</b></td><td align='left' colspan='4'><b>" + vFundSource + "</b></td></tr>");
            //htw.WriteLine("<tr><td colspan='2'><b>Loan No:</b></td><td align='left' colspan='3'><b>" + vLnNo + "</b></td><td colspan='2'><b>Purpose:</b></td><td align='left' colspan='4'><b>" + vPurpose + "</b></td></tr>");
            //htw.WriteLine("<tr><td colspan='2'><b>Loan Amount:</b></td><td align='left' colspan='3'><b>" + vLoanAmt + "</b></td><td colspan='2'><b>Interest Amount:</b></td><td align='left' colspan='4'><b>" + vIntAmt + "</b></td></tr>");
            //htw.WriteLine("<tr><td colspan='2'><b>Disburse Date:</b></td><td align='left' colspan='3'><b>" + vDisbDt + "</b></td><td colspan='2'><b>Loan Scheme:</b></td><td align='left' colspan='4'><b>" + vLnProduct + "</b></td></tr>");
            //htw.WriteLine("<tr><td align=center' colspan='5'><b><u><font size='3'>Repayment Schedule</font></u></b></td><td align=center' colspan='7'><b><u><font size='3'>Collection Details</font></u></b></td></tr>");
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
    }
}
