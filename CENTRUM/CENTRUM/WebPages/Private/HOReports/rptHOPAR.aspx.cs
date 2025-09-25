using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class rptHOPAR : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtAsDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["ID"] = null;
                PopList();
                PopBranch();
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "PAR Category";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHOPARCategoryRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Portfolio Ageing", false);
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
            CReports oCr = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            Int32 vBrId = Convert.ToInt32(vBrCode);
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            chkDtl.DataSource = null;
            chkDtl.Items.Clear();
            DateTime vFromdt = gblFuction.setDate(txtAsDt.Text);
            PopBranch();

            /************** 1. Age Wise **************/
            if (rdbOpt.SelectedValue == "rdbAge")
            {
                oCr = new CReports();
                dt = oCr.rptMemberAge(vFromdt);
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "M_Age";
                chkDtl.DataValueField = "M_Age";
                chkDtl.DataBind();
            }

            /************** 2. Purpose Wise **************/
            if (rdbOpt.SelectedValue == "rdbPurp")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "PurposeID", "Purpose", "LoanPurposeMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "Purpose";
                chkDtl.DataValueField = "PurposeID";
                chkDtl.DataBind();
            }

            /************** 3. Branch Wise **************/
            if (rdbOpt.SelectedValue == "rdbBr")
            {
                PopBranch();
            }

            /************** 4. Product Wise **************/
            if (rdbOpt.SelectedValue == "rdbProd")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanType", "LoanTypeMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "LoanType";
                chkDtl.DataValueField = "LoanTypeId";
                chkDtl.DataBind();
            }

            /************** 5. Caste Wise **************/
            if (rdbOpt.SelectedValue == "rdbCaste")
            {
                Dictionary<string, Int32> oDic = new Dictionary<string, Int32>();
                oDic.Add("General", 1);
                oDic.Add("SC", 2);
                oDic.Add("ST", 3);
                oDic.Add("OBC", 4);
                oDic.Add("Minority", 5);
                chkDtl.DataSource = oDic;
                chkDtl.DataValueField = "value";
                chkDtl.DataTextField = "key";
                chkDtl.DataBind();
            }

            /************** 6. Area Wise **************/
            if (rdbOpt.SelectedValue == "rdbArea")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "AreaId", "AreaName", "AreaMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "AreaName";
                chkDtl.DataValueField = "AreaId";
                chkDtl.DataBind();                
            }

            /************** 7. District Wise **************/
            if (rdbOpt.SelectedValue == "rdbDist")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "DistrictID", "DistrictName", "DistrictMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "DistrictName";
                chkDtl.DataValueField = "DistrictID";
                chkDtl.DataBind();
            }

            /************** 8. Balance Tenure Wise **************/
            if (rdbOpt.SelectedValue == "rdbBalTen")
            {
                //gblFuction.AjxMsgPopup("Under Process....");
                chkDtl.Items.Clear();
                chkDtl.DataSource = null;
            }

            /************** 9. IRR Rate Wise **************/
            if (rdbOpt.SelectedValue == "rdbIRR")
            {
                oCr = new CReports();
                dt = oCr.rptIrrRate();
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "InstRate";
                chkDtl.DataValueField = "InstRate";
                chkDtl.DataBind();
            }

            /************** 10. Gross Loan Wise **************/
            if (rdbOpt.SelectedValue == "rdbGrsLn")
            {
                oCr = new CReports();
                dt = oCr.rptLoanAmt();
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "LoanAmt";
                chkDtl.DataValueField = "LoanAmt";
                chkDtl.DataBind();
            }

            /************** 11. Group Size Wise **************/
            if (rdbOpt.SelectedValue == "rdbGrSize")
            {
                oCr = new CReports();
                dt = oCr.rptGroupSize(vFromdt);
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "GroupSize";
                chkDtl.DataValueField = "GroupSize";
                chkDtl.DataBind();
            }

            /************** 10. Installment Frequency Wise **************/
            if (rdbOpt.SelectedValue == "rdbInstFrq")
            {
                Dictionary<string, string> oDic = new Dictionary<string, string>();
                oDic.Add("Weekly", "W");
                oDic.Add("Fortnightly", "F");
                oDic.Add("Monthly", "M");
                chkDtl.DataSource = oDic;
                chkDtl.DataValueField = "value";
                chkDtl.DataTextField = "key";
                chkDtl.DataBind();                
            }
            if (rdbOpt.SelectedValue == "rdbLnAmt")
            {
                Dictionary<string, string> oDic = new Dictionary<string, string>();
                oDic.Add("Below 5 K", "< 5000");
                oDic.Add("5-10 K", "BETWEEN 5000 AND 10000");
                oDic.Add("Above 10 to 15K", "BETWEEN 10000 AND 15000");
                oDic.Add("Above 15 to 20K", "BETWEEN 15000 AND 20000");
                oDic.Add("Above 20 to 30K", "BETWEEN 20000 AND 30000");
                oDic.Add("Above 30 to 50K", "BETWEEN 30000 AND 50000");
                oDic.Add("Above 50K", "> 50000");
                chkDtl.DataSource = oDic;
                chkDtl.DataValueField = "value";
                chkDtl.DataTextField = "key";
                chkDtl.DataBind();     
            }

            /************** 11. Location Type Wise **************/
            if (rdbOpt.SelectedValue == "rdbLoc")
            {
                Dictionary<string, Int32> oDic = new Dictionary<string, Int32>();
                oDic.Add("Rural", 1);
                oDic.Add("Semi Urban", 2);
                oDic.Add("Urban", 3);
                chkDtl.DataSource = oDic;
                chkDtl.DataValueField = "value";
                chkDtl.DataTextField = "key";
                chkDtl.DataBind();
            }

            /************** 12. Cycle Wise **************/
            if (rdbOpt.SelectedValue == "rdbCycle")
            {
                Dictionary<string, Int32> oDic = new Dictionary<string, Int32>();
                oDic.Add("Cycle 1", 1);
                oDic.Add("Cycle 2", 2);
                oDic.Add("Cycle 3", 3);
                oDic.Add("Cycle 4", 4);
                oDic.Add("Cycle 5", 5);
                oDic.Add("Cycle 6", 6);
                oDic.Add("Cycle 7", 7);
                oDic.Add("Cycle 8", 8);
                oDic.Add("Cycle 9", 9);
                oDic.Add("Cycle 10", 10);
                chkDtl.DataSource = oDic;
                chkDtl.DataValueField = "value";
                chkDtl.DataTextField = "key";
                chkDtl.DataBind();
            }


            /************** 13. Month Wise **************/
            if (rdbOpt.SelectedValue == "rdbMnth")
            {
                PopBranch();
            }

            /************** 14. Original Tenure Wise **************/
            if (rdbOpt.SelectedValue == "rdbOrgTnr")
            {
                oCr = new CReports();
                dt = oCr.rptLoanTenure();
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "TotalInstNo";
                chkDtl.DataValueField = "TotalInstNo";
                chkDtl.DataBind();
            }

            /************** 15. Education Qualification Wise **************/
            if (rdbOpt.SelectedValue == "rdbEduQli")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "QualificationId", "QualificationName", "QualificationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "QualificationName";
                chkDtl.DataValueField = "QualificationId";
                chkDtl.DataBind();
            }


            /************** 16. Occupation Wise **************/ 
            if (rdbOpt.SelectedValue == "rdbOccu")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "OccupationId", "OccupationName", "OccupationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "OccupationName";
                chkDtl.DataValueField = "OccupationId";
                chkDtl.DataBind();
            }

            /************** 16. Religion Wise **************/ 
            if (rdbOpt.SelectedValue == "rdbReli")
            {
                Dictionary<string, string> oDic = new Dictionary<string, string>();
                oDic.Add("Buddhist", "R05");
                oDic.Add("Christian", "R03");
                oDic.Add("Hindu", "R01");
                oDic.Add("Jain", "R06");
                oDic.Add("Muslim", "R02");
                oDic.Add("Others", "R07");
                oDic.Add("Sikh", "R04");
                chkDtl.DataSource = oDic;
                chkDtl.DataValueField = "value";
                chkDtl.DataTextField = "key";
                chkDtl.DataBind();
            }

            /************** 2. Purpose Wise **************/
            if (rdbOpt.SelectedValue == "rdbSPurp")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "SubPurposeID", "SubPurpose", "SubPurposeMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "SubPurpose";
                chkDtl.DataValueField = "SubPurposeID";
                chkDtl.DataBind();
            }
            if (rdbOpt.SelectedValue == "rdbState")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "StateID", "StateName", "StateMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                chkBrDtl.DataSource = dt;
                chkBrDtl.DataTextField = "StateName";
                chkBrDtl.DataValueField = "StateID";
                chkBrDtl.DataBind();
                CheckBrAll();
                //chkBrDtl.DataSource = null;
                //chkBrDtl.DataBind();
                //upBrDtl.Update();

            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckAll()
        {
            Int32 vRow;
            string strin = "";
            if (rdbSel.SelectedValue == "rdbAll")
            {
                chkDtl.Enabled = false;
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                {
                    chkDtl.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkDtl.Items[vRow].Value + "";
                    }
                }
                ViewState["ID"] = strin;
            }
            else if (rdbSel.SelectedValue == "rdbSelct")
            {
                ViewState["ID"] = null;
                chkDtl.Enabled = true;
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = false;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopList();
            CheckAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
            {
                if (chkDtl.Items[vRow].Selected == true)
                {
                    if (strin == "")
                    {
                        strin = chkDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkDtl.Items[vRow].Value + "";
                    }
                }
            }
            ViewState["ID"] = strin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromdt = gblFuction.setDate(txtAsDt.Text);
            string vBrCode = ViewState["BrCode"].ToString();
            string vStrId = "", vRptPath = "", vMode = "A", vFileNm = "", vWithPDDDt= "";
            DataTable dt = null;
            string vBranch = Session[gblValue.BrName].ToString();
            ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();

            if (rdbOpt.SelectedValue != "rdbAll" && rdbOpt.SelectedValue != "rdbBr" && rdbOpt.SelectedValue != "rdbMnth" && rdbOpt.SelectedValue != "rdbBalTen" && rdbOpt.SelectedValue!="rdbState")
            {
                vStrId = ViewState["ID"].ToString();
                if (vStrId == null || vStrId == "")
                {
                    gblFuction.MsgPopup("Please Select the Records to print.");
                    return;
                }
            }

            if (chkWithPddDt.Checked == true)
                vWithPDDDt = "Y";
            else
                vWithPDDDt = "N";
            /************** 1. Age Wise **************/
            if (rdbOpt.SelectedValue == "rdbAge")
            {
                dt = oRpt.rptPAR_AgeWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }

            /************** 2. Purpose Wise **************/
            if (rdbOpt.SelectedValue == "rdbPurp")
            {
                dt = oRpt.rptPAR_PurposeWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }

            /************** 3. Branch Wise And All **************/
            if (rdbOpt.SelectedValue == "rdbAll" || rdbOpt.SelectedValue == "rdbBr")
            {
                dt = oRpt.rptPAR_BranchWise(vBrCode, vFromdt, vWithPDDDt);
            }

            /************** 4. Product Wise **************/
            if (rdbOpt.SelectedValue == "rdbProd")
            {
                dt = oRpt.rptPAR_ProductWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }

            /************** 5. Caste Wise **************/
            if (rdbOpt.SelectedValue == "rdbCaste")
            {
                dt = oRpt.rptPAR_CasteWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }

            /************** 6. Area Wise **************/
            if (rdbOpt.SelectedValue == "rdbArea")
            {
                dt = oRpt.rptPAR_AreaWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }   

            /************** 7. District Wise **************/
            if (rdbOpt.SelectedValue == "rdbDist")
            {
                dt = oRpt.rptPAR_DistrictWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }   

            /************** 8. Balance Tenure Wise **************/
            if (rdbOpt.SelectedValue == "rdbBalTen")
            {
                dt = oRpt.rptPAR_BalTenureWise(vBrCode, vFromdt, vWithPDDDt);
            }  

            /************** 9. IRR Rate Wise **************/
            if (rdbOpt.SelectedValue == "rdbIRR")
            {
                dt = oRpt.rptPAR_IrreWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }

            /************** 10. Gross Loan Wise **************/
            if (rdbOpt.SelectedValue == "rdbGrsLn")
            {
                dt = oRpt.rptPAR_GrossAmountWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }

            /************** 11. Group Size Wise **************/
            if (rdbOpt.SelectedValue == "rdbGrSize")
            {
                dt = oRpt.rptPAR_GroupSizeWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }

            /************** 10. Installment Frequency Wise **************/
            if (rdbOpt.SelectedValue == "rdbInstFrq")
            {
                dt = oRpt.rptPAR_InstFrqWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }

            /************** 11. Location Type Wise **************/
            if (rdbOpt.SelectedValue == "rdbLoc")
            {
                dt = oRpt.rptPAR_HouseHoldWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }

            /************** 12. Cycle Wise **************/
            if (rdbOpt.SelectedValue == "rdbCycle")
            {
                dt = oRpt.rptPAR_CycleWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }

            /************** 13. Month Wise **************/
            if (rdbOpt.SelectedValue == "rdbMnth")
            {
                dt = oRpt.rptPAR_MonthWise(vBrCode, vFromdt, vWithPDDDt);
            }

            /************** 14. Original Tenure Wise **************/
            if (rdbOpt.SelectedValue == "rdbOrgTnr")
            {
                dt = oRpt.rptPAR_TenureWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }

            /************** 15. Education Qualification Wise **************/
            if (rdbOpt.SelectedValue == "rdbEduQli")
            {
                dt = oRpt.rptPAR_QualificationWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }

            /************** 16. Occupation Wise **************/
            if (rdbOpt.SelectedValue == "rdbOccu")
            {
                dt = oRpt.rptPAR_OccupationWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }

            /************** 17. Religion Wise **************/
            if (rdbOpt.SelectedValue == "rdbReli")
            {
                dt = oRpt.rptPAR_ReligionWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }

            /************** 18. SubPurpose Wise **************/
            if (rdbOpt.SelectedValue == "rdbSPurp")
            {
                dt = oRpt.rptPAR_SubPurposeWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }
            if (rdbOpt.SelectedValue == "rdbLnAmt")
            {
                dt = oRpt.rptPAR_LnAmountWise(vBrCode, vStrId, vFromdt, vWithPDDDt);
            }
            if (rdbOpt.SelectedValue == "rdbState")
            {
                dt = oRpt.rptPAR_StateWise(vBrCode, vFromdt, vWithPDDDt);
            }
            
            if (pMode == "Excel")
            {

                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();

                tdx.Controls.Add(DataGrid1);
                tdx.Visible = false;
                vFileNm = "attachment;filename=PAR_Category_Report.xls";
                System.IO.StringWriter sw = new System.IO.StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                htw.WriteLine("<table border='1' cellpadding='17' widht='100%'>");
                htw.WriteLine("<tr><td align=center' colspan='17'><b><u><font size='5'>" + gblValue.CompName + "</font></u></b></td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='17'><b><u><font size='3'>" + CGblIdGenerator.GetBranchAddress1("0000") + "</font></u></b></td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='17'><b><u><font size='3'>PAR Category Report - " + rdbOpt.SelectedItem.Text.ToString() + " As On " + txtAsDt.Text + "</font></u></b></td></tr>");
                DataGrid1.RenderControl(htw);
                htw.WriteLine("</td></tr>");
                htw.WriteLine("<tr><td colspan='17'><b><u><font size='3'></font></u></b></td></tr>");
                htw.WriteLine("</table>");

                Response.ClearContent();
                Response.AddHeader("content-disposition", vFileNm);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
                Response.ContentType = "application/vnd.ms-excel";
                Response.Write(sw.ToString());
                Response.End();
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
    }
}
