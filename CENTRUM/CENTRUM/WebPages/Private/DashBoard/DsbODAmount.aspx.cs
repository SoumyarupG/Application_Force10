using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.DashBoard
{
    public partial class DsbODAmount : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch(Session[gblValue.UserName].ToString());
                txtAson.Text = Session[gblValue.LoginDate].ToString();
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Default Amount";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHODfltAmt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Status Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //private void PopBranch()
        //{
        //    ViewState["ID"] = null;
        //    DataTable dt = null;
        //    CGblIdGenerator oCG = null;
        //    try
        //    {
        //        DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //        oCG = new CGblIdGenerator();
        //        dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, "0000");
        //        chkBr.DataSource = dt;
        //        chkBr.DataTextField = "Name";
        //        chkBr.DataValueField = "BranchCode";
        //        chkBr.DataBind();
        //        CheckAll();
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oCG = null;
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUser"></param>
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
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        private void ShowBar()
        {
            CDashBoard oDas = null;
            DataTable dt = null;
            string vBrCode = "A";
            ViewState["ODAmount"] = null;
            try
            {
                //if (ddlSel.SelectedValue == "B")
                //{
                GetBranch();
                vBrCode = Convert.ToString(ViewState["ID"]);
                //}
                oDas = new CDashBoard();
                dt = oDas.GetPARAmtChart(gblFuction.setDate(txtAson.Text.Trim()), vBrCode);
                ViewState["ODAmount"] = dt;
                chPos.DataSource = dt;
                chPos.Series["Series1"].XValueMember = "Branch";
                chPos.Series["Series1"].YValueMembers = "ODAmt";
                chPos.ChartAreas[0].AxisX.Interval = 1;
                chPos.ChartAreas[0].AxisX.Title = "Branch";
                chPos.ChartAreas[0].AxisY.Title = "Over Due Amount";
                chPos.Series["Series1"].ToolTip = "OD Amount = #VALY\nBranch = #VALX";
                chPos.Series["Series1"].PostBackValue = "#AXISLABEL";
                chPos.DataBind();
            }
            finally
            {
                oDas = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRefrsh_Click(object sender, EventArgs e)
        {
            ShowBar();
            mvPos.ActiveViewIndex = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPie_Click(object sender, EventArgs e)
        {
            GetBranch();
            string vBrCode = Convert.ToString(ViewState["ID"]);
            SetDtlChart(vBrCode, "C");
            mvPos.ActiveViewIndex = 1;
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
        protected void btnBack_Click(object sender, EventArgs e)
        {
            int vViewIndx = 0;
            vViewIndx = mvPos.ActiveViewIndex;
            if (vViewIndx > 0)
                mvPos.ActiveViewIndex = vViewIndx - 1;
            //switch (vViewIndx)
            //{
            //    case 1:
            //        ShowBar();
            //        break;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vBrcode"></param>
        private void SetDtlChart(string vBrCode, string pMode)
        {
            CDashBoard oDas = null;
            DataSet ds = null;
            DataTable dt1 = null, dt2 = null, dt3 = null, dt4 = null, dt5 = null, dt6 = null;
            ViewState["LoanCycle"] = null; ViewState["Funder"] = null; ViewState["Purpose"] = null;
            ViewState["Gender"] = null; ViewState["LoanType"] = null; ViewState["Caste"] = null;
            string vBrCodeP = "";

            try
            {
                if (pMode == "B")
                {
                    vBrCodeP = vBrCode.Substring(0, 3);
                }
                if (pMode == "C")
                {
                    vBrCodeP = vBrCode;
                }

                oDas = new CDashBoard();
                ds = oDas.GetPARAmtDtlForChart(gblFuction.setDate(txtAson.Text.Trim()), vBrCodeP, pMode);
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[2];
                dt4 = ds.Tables[3];
                dt5 = ds.Tables[4];
                dt6 = ds.Tables[5]; ;
                ViewState["LoanCycle"] = dt1; ViewState["Funder"] = dt2; ViewState["Purpose"] = dt3;
                ViewState["Gender"] = dt4; ViewState["LoanType"] = dt5; ViewState["Caste"] = dt6;

                chLnCycle.DataSource = dt1;
                chLnCycle.Series["Series1"].XValueMember = "LnCycle";
                chLnCycle.Series["Series1"].YValueMembers = "ODAmt";
                chLnCycle.ChartAreas[0].AxisX.Interval = 1;
                chLnCycle.Series["Series1"].ToolTip = "OD Amount = #VALY\nLoan Cycle = #VALX";
                chLnCycle.Series["Series1"]["PieDrawingStyle"] = "Concave";
                chLnCycle.Series["Series1"].IsVisibleInLegend = false;
                chLnCycle.DataBind();
                if (pMode == "B")
                {
                    rdblnCycle.Text = vBrCode + " (LoanCycle)";
                }
                if (pMode == "C")
                {
                    rdblnCycle.Text = " ConsoliDate LoanCycle";
                }

                chFunder.DataSource = dt2;
                chFunder.Series["Series1"].XValueMember = "FunderName";
                chFunder.Series["Series1"].YValueMembers = "ODAmt";
                chFunder.ChartAreas[0].AxisX.Interval = 1;
                chFunder.Series["Series1"].ToolTip = "OD Amount = #VALY\nFunderName = #VALX";
                chFunder.Series["Series1"]["PieLabelStyle"] = "Disabled";
                chFunder.Series["Series1"]["PieDrawingStyle"] = "Concave";
                //chPos.Series["Series1"].PostBackValue = "#AXISLABEL";
                chFunder.DataBind();

                if (pMode == "B")
                {
                    rdbFundr.Text = vBrCode + " (Funder)";
                }
                if (pMode == "C")
                {
                    rdbFundr.Text = "Consolidate Funder";
                }
                chPurpose.DataSource = dt3;
                chPurpose.Series["Series1"].XValueMember = "PurposeName";
                chPurpose.Series["Series1"].YValueMembers = "ODAmt";
                chPurpose.ChartAreas[0].AxisX.Interval = 1;
                chPurpose.Series["Series1"].ToolTip = "OD Amount = #VALY\nPurposeName = #VALX";
                chPurpose.Series["Series1"]["PieLabelStyle"] = "Disabled";
                chPurpose.Series["Series1"]["PieDrawingStyle"] = "Concave";
                //chPos.Series["Series1"].PostBackValue = "#AXISLABEL";
                chPurpose.DataBind();
                if (pMode == "B")
                {
                    rdbPurps.Text = vBrCode + " (Purpose)";
                }
                if (pMode == "C")
                {
                    rdbPurps.Text = " Consolidate Purpose";
                }
                chGender.DataSource = dt4;
                chGender.Series["Series1"].XValueMember = "Gender";
                chGender.Series["Series1"].YValueMembers = "ODAmt";
                chGender.ChartAreas[0].AxisX.Interval = 1;
                chGender.Series["Series1"].ToolTip = "OD Amount = #VALY\nGender = #VALX";
                chGender.Series["Series1"]["PieLabelStyle"] = "Disabled";
                chGender.Series["Series1"]["PieDrawingStyle"] = "Concave";
                //chPos.Series["Series1"].PostBackValue = "#AXISLABEL";
                chGender.DataBind();
                if (pMode == "B")
                {
                    rdbGndr.Text = vBrCode + " (Gender)";
                }
                if (pMode == "C")
                {
                    rdbGndr.Text = " Consolidate Gender";
                }


                chLnType.DataSource = dt5;
                chLnType.Series["Series1"].XValueMember = "LoanTypeName";
                chLnType.Series["Series1"].YValueMembers = "ODAmt";
                chLnType.ChartAreas[0].AxisX.Interval = 1;
                chLnType.Series["Series1"].ToolTip = "OD Amount = #VALY\nLoanTypeName = #VALX";
                chLnType.Series["Series1"]["PieLabelStyle"] = "Disabled";
                chLnType.Series["Series1"]["PieDrawingStyle"] = "Concave";
                //chLnType.Series["Series1"].PostBackValue = "#AXISLABEL";
                chLnType.DataBind();

                if (pMode == "B")
                {
                    rdbLnTyp.Text = vBrCode + " (Loan Type)";
                }
                if (pMode == "C")
                {
                    rdbLnTyp.Text = "Consolidate Loan Type";
                }

                chCaste.DataSource = dt6;
                chCaste.Series["Series1"].XValueMember = "Caste";
                chCaste.Series["Series1"].YValueMembers = "ODAmt";
                chCaste.ChartAreas[0].AxisX.Interval = 1;
                chCaste.Series["Series1"].ToolTip = "OD Amount = #VALY\nCaste = #VALX";
                chCaste.Series["Series1"]["PieLabelStyle"] = "Disabled";
                chCaste.Series["Series1"]["PieDrawingStyle"] = "Concave";
                //chLnType.Series["Series1"].PostBackValue = "#AXISLABEL";
                chCaste.DataBind();


                if (pMode == "B")
                {
                    rdbCaste.Text = vBrCode + " (Caste)";
                }
                if (pMode == "C")
                {
                    rdbCaste.Text = "Consolidate Caste";
                }
            }
            finally
            {
                oDas = null;
                ds = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (mvPos.ActiveViewIndex == 1)
            {
                if (rdblnCycle.Checked == true)
                {
                    dgChart.DataSource = (DataTable)ViewState["LoanCycle"];
                    dgChart.DataBind();
                }
                if (rdbFundr.Checked == true)
                {
                    dgChart.DataSource = (DataTable)ViewState["Funder"];
                    dgChart.DataBind();
                }
                if (rdbPurps.Checked == true)
                {
                    dgChart.DataSource = (DataTable)ViewState["Purpose"];
                    dgChart.DataBind();
                }
                if (rdbGndr.Checked == true)
                {
                    dgChart.DataSource = (DataTable)ViewState["Gender"];
                    dgChart.DataBind();
                }
                if (rdbLnTyp.Checked == true)
                {
                    dgChart.DataSource = (DataTable)ViewState["LoanType"];
                    dgChart.DataBind();
                }
                if (rdbCaste.Checked == true)
                {
                    dgChart.DataSource = (DataTable)ViewState["Caste"];
                    dgChart.DataBind();
                }
            }
            else
            {
                dgChart.DataSource = (DataTable)ViewState["ODAmount"];
                dgChart.DataBind();
            }
            string vFileNm = "attachment;filename=OD Amount";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            dgChart.RenderControl(htw);
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
        protected void chPos_Click(object sender, ImageMapEventArgs e)
        {
            SetDtlChart(e.PostBackValue, "B");
            mvPos.ActiveViewIndex = 1;
        }
    }
}
