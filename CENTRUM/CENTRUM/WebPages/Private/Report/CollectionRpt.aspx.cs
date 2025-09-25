using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class CollectionRpt : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopList();
                CheckAll();
                popDetail();
                txtDtFrm.Enabled = false;
                txtToDt.Enabled = false;
            }

            if (Page.IsPostBack == false)
            {
                // Set default values.
                rblSel.SelectedIndex = 0;
                rblAlSel_SelectedIndexChanged(sender, e);
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
                this.GetModuleByRole(mnuID.nmuCollectionRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Group Meeting", false);
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
        private void PopList()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            Int32 vBrId = Convert.ToInt32(vBrCode);
            CEO oRO = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            if (rblSel.SelectedValue == "rbEO")
            {
                //oCG = new CGblIdGenerator();
                //dt = oCM.GetCOPop(vBrCode, "SCO,CO,TCO,JTCO,GO");
                //dt = new DataTable();
                //dt = oCG.PopComboMIS("D", "N", "AA", "EoId", "EoName", "EoMst", vBrCode, "BranchCode", "Tra_DropDate", vLogDt, vBrCode);
                //chkDtl.DataSource = dt;
                //chkDtl.DataTextField = "EoName";
                //chkDtl.DataValueField = "EoId";
                //chkDtl.DataBind();


                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "EoName";
                chkDtl.DataValueField = "Eoid";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbLType")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanType", "LoanTypeMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "LoanType";
                chkDtl.DataValueField = "LoanTypeId";
                chkDtl.DataBind();
            }
            if (rblSel.SelectedValue == "rbFund")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "FundSourceId", "FundSource", "FundSourceMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "FundSource";
                chkDtl.DataValueField = "FundSourceId";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbGrp")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "GroupId", "GroupName", "GroupMst", vBrId, "BranchCode", "AA", vLogDt, vBrCode);
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "GroupName";
                chkDtl.DataValueField = "GroupId";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbCyl")
            {
                chkDtl.Items.Clear();
                ListItem oLs1 = new ListItem();
                oLs1.Text = "1st Cycle";
                oLs1.Value = "1";
                chkDtl.Items.Add(oLs1);

                ListItem oLs2 = new ListItem();
                oLs2.Text = "2nd Cycle";
                oLs2.Value = "2";
                chkDtl.Items.Add(oLs2);

                ListItem oLs3 = new ListItem();
                oLs3.Text = "3rd Cycle";
                oLs3.Value = "3";
                chkDtl.Items.Add(oLs3);

                ListItem oLs4 = new ListItem();
                oLs4.Text = "4th Cycle";
                oLs4.Value = "4";
                chkDtl.Items.Add(oLs4);

                ListItem oLs5 = new ListItem();
                oLs5.Text = "5th Cycle";
                oLs5.Value = "5";
                chkDtl.Items.Add(oLs5);
            }

            if (rblSel.SelectedValue.Equals("rbWhole"))
            {
                chkDtl.Items.Clear();
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
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vRptPath = "", vTypeId = "", vType = "";
            string vBranch = Session[gblValue.BrName].ToString();
            string vWoffColl;
            //ReportDocument rptDoc = new ReportDocument();
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            vTypeId = ViewState["Dtl"].ToString();

            if (rblSel.SelectedValue == "rbEO")
                vType = "E";

            if (rblSel.SelectedValue == "rbLType")
                vType = "L";

            if (rblSel.SelectedValue == "rbFund")
                vType = "F";

            if (rblSel.SelectedValue == "rbGrp")
                vType = "G";

            if (rblSel.SelectedValue == "rbCyl")
                vType = "C";

            if (rblSel.SelectedValue == "rbWhole")
                vType = "A";

            if (rbOpt.SelectedValue == "rbDtl")
                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\CollectionDtl.rpt";
            if (rbOpt.SelectedValue == "rbSum")
                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\CollectionSum.rpt";
            if (chkWoff.Checked == true)
            {
                vWoffColl = "Y";
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

            dt = oRpt.rptLoanCollection(vFromDt, vToDt, vBrCode, vType, vTypeId, vWoffColl);
            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                rptDoc.SetParameterValue("pAddress2", "");
                rptDoc.SetParameterValue("pBranch", vBranch);
                rptDoc.SetParameterValue("pType", vType);
                rptDoc.SetParameterValue("dtFrom", txtDtFrm.Text);
                rptDoc.SetParameterValue("dtTo", txtToDt.Text);
                rptDoc.SetParameterValue("WoffColl", vWoffColl);
                if (pMode == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Loan Collection List");
                else if (pMode == "Excel")
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Loan Collection List");

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
