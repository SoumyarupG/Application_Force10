using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class CollectionHo : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopList();
                PopBranch();
                PopState();
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Collection";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHOColRpt);
                if (this.UserID == 1) return;
                //if (this.CanReport == "Y")
                //{
                //}
                //else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                //{
                //    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Repayment Schedule", false);
                //}
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

            if (rbList.SelectedValue == "rbLT")
            {
                ddlLT1.Enabled = true;
                ddlFS.Items.Clear();
                ddlFS.Enabled = false;
                ddlLC.Items.Clear();
                ddlLC.Enabled = false;

                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanType", "LoanTypeMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                ddlLT1.DataSource = dt;
                ddlLT1.DataTextField = "LoanType";
                ddlLT1.DataValueField = "LoanTypeId";
                ddlLT1.DataBind();
                ListItem oL1 = new ListItem("<-- Select -->", "-1");
               // ddlLT1.Items.Insert(0, oL1);
            }
            if (rbList.SelectedValue == "rbFS")
            {
               // ddlLT1.Items.Clear();
                ddlLT1.Enabled = false;
                ddlFS.Enabled = true;
                ddlLC.Items.Clear();
                ddlLC.Enabled = false;

                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "FundSourceID", "FundSource", "FundSourceMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                ddlFS.DataSource = dt;
                ddlFS.DataTextField = "FundSource";
                ddlFS.DataValueField = "FundSourceID";
                ddlFS.DataBind();
                ListItem oL1 = new ListItem("<-- Select -->", "-1");
                ddlFS.Items.Insert(0, oL1);
            }
            if (rbList.SelectedValue == "rbLC")
            {
                //ddlLT1.Items.Clear();
                ddlLT1.Enabled = false;
                ddlFS.Items.Clear();
                ddlFS.Enabled = false;
                ddlLC.Enabled = true;

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
                //ddlLT1.Items.Clear();
                ddlLT1.Enabled = false;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = ViewState["BrCode"].ToString();
            string vRptPath = "", vTitle = "", vMode = "A" ,vLTID ="";
            string vBranch = Session[gblValue.BrName].ToString();
            string vWoffColl;
            //ReportDocument rptDoc = new ReportDocument();
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            Int32 vFSID = 0, vLCID = 0;

            if (rbList.SelectedValue == "rbLT")
            {
                vTitle = "Collection Report (Loan Scheme Wise)";
                vMode = "L";
                //if (Convert.ToInt32(ddlLT1.SelectedValue) > 0)
                //    vLTID = Convert.ToInt32(ddlLT1.SelectedValue);
                vLTID = ddlLT1.SelectedValues.Replace("|", ",");
            }
            if (rbList.SelectedValue == "rbFS")
            {
                vTitle = "Collection Report (Funder Source Wise)";
                vMode = "F";
                if (Convert.ToInt32(ddlFS.SelectedValue) > 0)
                    vFSID = Convert.ToInt32(ddlFS.SelectedValue);
            }
            if (rbList.SelectedValue == "rbLC")
            {
                vTitle = "Collection Report (Loan Cycle Wise)";
                vMode = "C";
                if (Convert.ToInt32(ddlLC.SelectedValue) > 0)
                    vLCID = Convert.ToInt32(ddlLC.SelectedValue);
            }
            if (rbList.SelectedValue == "rbALL")
            {
                vTitle = "Collection Report ";
                vMode = "A";
            }
            if (chkWoff.Checked == true)
            {
                vWoffColl = "Y";
                vTitle = "Write off " + vTitle;
            }
            else
            {
                vWoffColl = "N";
            }

            TimeSpan t = vToDt - vFromDt;
            if (t.TotalDays > 2)
            {
                gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                return;
            }
            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\CollRpt.rpt";
            dt = oRpt.rptCollRpt(vFromDt, vToDt, vMode, vLTID, vFSID, vLCID, vBrCode, vWoffColl);
            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1("0000"));
                rptDoc.SetParameterValue("pTitle", vTitle);
                rptDoc.SetParameterValue("pBranch", vBranch);
                rptDoc.SetParameterValue("dtFrom", txtDtFrm.Text);
                rptDoc.SetParameterValue("dtTo", txtToDt.Text);
                rptDoc.SetParameterValue("pMode", vMode);
                if (pMode == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Collection_Balance" + txtToDt.Text.Replace("/", "_"));
                else if (pMode == "Excel")
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Collection_Balance" + txtToDt.Text.Replace("/", "_") + ".xls");

                rptDoc.Dispose();
                Response.ClearHeaders();
                Response.ClearContent();

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
