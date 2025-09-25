using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Web.UI;
using System.Web;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class AgeWise : CENTRUMBase
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
                txtProvision1.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                txtProvision2.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                txtProvision3.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                txtProvision4.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                txtProvision5.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                txtProvision6.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                txtProvision7.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                txtDate.Text = Convert.ToString(Session[gblValue.LoginDate]);

                //PopCO();
                PopGroup();
                //PopMarket();
                PopCO();
                PopLoanType();
                PopFunder();
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
                this.PageHeading = "Age wise Default List";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuAgeDefltSheetRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Age wise Default List", false);
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
        private void PopCO()
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CGblIdGenerator oCG = new CGblIdGenerator();
            CEO oRO = null;
            try
            {
                //dt = oCM.GetCOPop(vBrCode, "SCO,CO,TCO,JTCO,GO");
                //oCG = new CGblIdGenerator();
                //dt = oCG.PopComboMIS("D", "N", "AA", "EoId", "EoName", "EoMst", vBrCode, "BranchCode", "Tra_DropDate", vLogDt, vBrCode);
                //ddlCO.DataSource = dt;
                //ddlCO.DataTextField = "EOName";
                //ddlCO.DataValueField = "EOID";
                //ddlCO.DataBind();
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlCO.DataSource = dt;
                ddlCO.DataTextField = "EoName";
                ddlCO.DataValueField = "Eoid";
                ddlCO.DataBind();
                ListItem oL1 = new ListItem("<-- Select -->", "-1");
                ddlCO.Items.Insert(0, oL1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }



        private void PopGroup()
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            Int32 vBrId = Convert.ToInt32(vBrCode);
            CGblIdGenerator oCG = new CGblIdGenerator();
            try
            {

                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "GroupId", "GroupName", "GroupMst", vBrId, "BranchCode", "AA", vLogDt, vBrCode);
                ddlGr.DataSource = dt;
                ddlGr.DataTextField = "GroupName";
                ddlGr.DataValueField = "GroupId";
                ddlGr.DataBind();
                ListItem oL1 = new ListItem("<-- Select -->", "-1");
                ddlGr.Items.Insert(0, oL1);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }


        //private void PopMarket()
        //{
        //    DataTable dt = null;
        //    string vBrCode = Session[gblValue.BrnchCode].ToString();
        //    DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //    Int32 vBrId = Convert.ToInt32(vBrCode);
        //    CGblIdGenerator oCG = new CGblIdGenerator();
        //    try
        //    {

        //        oCG = new CGblIdGenerator();
        //        dt = oCG.PopComboMIS("N", "N", "AA", "MarketID", "Market", "MarketMst", vBrId, "BranchCode", "AA", vLogDt, vBrCode);
        //        ddlMarket.DataSource = dt;
        //        ddlMarket.DataTextField = "Market";
        //        ddlMarket.DataValueField = "MarketID";
        //        ddlMarket.DataBind();
        //        ListItem oL1 = new ListItem("<-- Select -->", "-1");
        //        ddlMarket.Items.Insert(0, oL1);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
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
        private void PopLoanType()
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            CGblIdGenerator oCG = new CGblIdGenerator();
            Int32 vBrId = Convert.ToInt32(vBrCode);
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanType", "LoanTypeMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                ddlLT.DataSource = dt;
                ddlLT.DataTextField = "LoanType";
                ddlLT.DataValueField = "LoanTypeId";
                ddlLT.DataBind();
                ListItem oL1 = new ListItem("<-- Select -->", "-1");
                ddlLT.Items.Insert(0, oL1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopFunder()
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            CGblIdGenerator oCG = new CGblIdGenerator();
            Int32 vBrId = Convert.ToInt32(vBrCode);
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                dt = oCG.PopComboMIS("N", "N", "AA", "FundSourceID", "FundSource", "FundSourceMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                ddlFS.DataSource = dt;
                ddlFS.DataTextField = "FundSource";
                ddlFS.DataValueField = "FundSourceID";
                ddlFS.DataBind();
                ListItem oL1 = new ListItem("<-- Select -->", "-1");
                ddlFS.Items.Insert(0, oL1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ddlMarket_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DataTable dtGr = null;
        //    CGblIdGenerator oGbl;
        //    string vBrCode = Session[gblValue.BrnchCode].ToString();
        //    try
        //    {
        //        if (Convert.ToInt32(ddlMarket.SelectedValue) > 0)
        //        {
        //            ddlLT.SelectedIndex = -1;
        //            ddlFS.SelectedIndex = -1;
        //            dtGr = new DataTable();
        //            oGbl = new CGblIdGenerator();
        //            dtGr = oGbl.PopComboMIS("S", "N", "AA", "Groupid", "GroupName", "GroupMSt", Convert.ToInt32(ddlMarket.SelectedValue), "MarketID", "Tra_DropDate", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);

        //            ddlGr.DataSource = dtGr;
        //            ddlGr.DataTextField = "GroupName";
        //            ddlGr.DataValueField = "Groupid";
        //            ddlGr.DataBind();
        //            ListItem oLi = new ListItem("<--Select-->", "-1");
        //            ddlGr.Items.Insert(0, oLi);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dtGr = null;
        //        oGbl = null;
        //    }
        //}



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCO_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataTable dtMk = null;
            //CGblIdGenerator oGbl;
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            //try
            //{
            //    if (Convert.ToInt32(ddlCO.SelectedValue) > 0)
            //    {
            //        ddlLT.SelectedIndex = -1;
            //        ddlFS.SelectedIndex = -1;
            //        dtMk = new DataTable();
            //        oGbl = new CGblIdGenerator();
            //        dtMk = oGbl.PopComboMIS("S", "N", "AA", "MarketID", "Market", "MarketMst", Convert.ToInt32(ddlCO.SelectedValue), "EOID", "Tra_DropDate", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);
            //        ddlMarket.DataSource = dtMk;
            //        ddlMarket.DataTextField = "Market";
            //        ddlMarket.DataValueField = "MarketID";
            //        ddlMarket.DataBind();
            //        ListItem oLi = new ListItem("<--Select-->", "-1");
            //        ddlMarket.Items.Insert(0, oLi);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    dtMk = null;
            //    oGbl = null;
            //}
            DataTable dtGr = null;
            CGblIdGenerator oGbl;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                if (Convert.ToInt32(ddlCO.SelectedValue) > 0)
                {
                    ddlLT.SelectedIndex = -1;
                    ddlFS.SelectedIndex = -1;
                    dtGr = new DataTable();
                    oGbl = new CGblIdGenerator();
                    dtGr = oGbl.PopComboMIS("S", "N", "AA", "Groupid", "GroupName", "GroupMSt", Convert.ToInt32(ddlCO.SelectedValue), "MarketID", "Tra_DropDate", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);

                    ddlGr.DataSource = dtGr;
                    ddlGr.DataTextField = "GroupName";
                    ddlGr.DataValueField = "Groupid";
                    ddlGr.DataBind();
                    ListItem oLi = new ListItem("<--Select-->", "-1");
                    ddlGr.Items.Insert(0, oLi);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtGr = null;
                oGbl = null;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ddlGr_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DataTable dtMk = null;
        //    CGblIdGenerator oGbl;
        //    string vBrCode = Session[gblValue.BrnchCode].ToString();
        //    try
        //    {
        //        if (Convert.ToInt32(ddlGr.SelectedValue) > 0)
        //        {
        //            ddlLT.SelectedIndex = -1;
        //            ddlFS.SelectedIndex = -1;
        //            ddlCO.SelectedIndex = -1;
        //            dtMk = new DataTable();
        //            oGbl = new CGblIdGenerator();
        //            dtMk = oGbl.PopComboMIS("S", "N", "AA", "MarketID", "Market", "MarketMst", Convert.ToInt32(ddlGr.SelectedIndex), "GroupId", "AA", gblFuction.setDate("01/01/1900"), vBrCode);
        //            ddlMarket.DataSource = dtMk;
        //            ddlMarket.DataTextField = "Market";
        //            ddlMarket.DataValueField = "MarketID";
        //            ddlMarket.DataBind();
        //            ListItem oLi = new ListItem("<--Select-->", "-1");
        //            ddlMarket.Items.Insert(0, oLi);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dtMk = null;
        //        oGbl = null;
        //    }
        //}



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbOpt.SelectedValue == "W")
            {
                Chk1.Text = "1 Week";
                Chk2.Text = "2 Week";
                Chk3.Text = "3 Week";
                Chk4.Text = "4 Week";
                Chk5.Text = "5 Week";
                Chk6.Text = "5-16 Week";
                Chk7.Text = "> 16 Week";
            }
            else
            {
                Chk1.Text = "1 to 30";
                Chk2.Text = "31 to 60";
                Chk3.Text = "61 to 90";
                Chk4.Text = "91 to 120";
                Chk5.Text = "121 to 180";
                Chk6.Text = "181 to 365";
                Chk7.Text = "> 1 Yr";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            DateTime vAsonDt = gblFuction.setDate(txtDate.Text);
            string vBrCode = Session[gblValue.BrnchCode].ToString(), vTitle = "";
            string vBranch = Session[gblValue.BrName].ToString();
            string vRptPath = "", vMode = "", vChk1 = "N", vChk2 = "N", vChk3 = "N", vChk4 = "N", vChk5 = "N", vChk6 = "N", vChk7 = "N", vGRId = "";
            double vProvision1 = 0, vProvision2 = 0, vProvision3 = 0, vProvision4 = 0, vProvision5 = 0, vProvision6 = 0, vProvision7 = 0;
            Int32 vLTId = 0, vCOId = 0, vMKID = 0, vFSId = 0;
            DataTable dt = null;
            //ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();

            //if (ddlMarket.SelectedIndex > 0) vMKID = Convert.ToInt32(ddlMarket.SelectedValue);
            if (ddlCO.SelectedIndex > 0) vMKID = Convert.ToInt32(ddlCO.SelectedValue);
            if (ddlCO.SelectedIndex > 0) vCOId = Convert.ToInt32(ddlCO.SelectedValue);
            if (ddlGr.SelectedIndex > 0) vGRId = ddlGr.SelectedValue;
            if (ddlLT.SelectedIndex > 0) vLTId = Convert.ToInt32(ddlLT.SelectedValue);
            if (ddlFS.SelectedIndex > 0) vFSId = Convert.ToInt32(ddlFS.SelectedValue);

            if (rbOpt.SelectedValue == "W") vMode = "W";
            else vMode = "M";

            if (Chk1.Checked == true) vChk1 = "Y";
            if (Chk2.Checked == true) vChk2 = "Y";
            if (Chk3.Checked == true) vChk3 = "Y";
            if (Chk4.Checked == true) vChk4 = "Y";
            if (Chk5.Checked == true) vChk5 = "Y";
            if (Chk6.Checked == true) vChk6 = "Y";
            if (Chk7.Checked == true) vChk7 = "Y";

            if (txtProvision1.Text != "") vProvision1 = Convert.ToDouble(txtProvision1.Text);
            if (txtProvision2.Text != "") vProvision2 = Convert.ToDouble(txtProvision2.Text);
            if (txtProvision3.Text != "") vProvision3 = Convert.ToDouble(txtProvision3.Text);
            if (txtProvision4.Text != "") vProvision4 = Convert.ToDouble(txtProvision4.Text);
            if (txtProvision5.Text != "") vProvision5 = Convert.ToDouble(txtProvision5.Text);
            if (txtProvision6.Text != "") vProvision6 = Convert.ToDouble(txtProvision6.Text);
            if (txtProvision7.Text != "") vProvision7 = Convert.ToDouble(txtProvision7.Text);
            if (rblDtlSumm.SelectedValue == "D")
            {

                if (vCOId == 0 || vGRId == "" || vLTId == 0 || vFSId == 0)
                {
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\AgeDefList.rpt";
                    if (vMode == "M")
                        vTitle = "Age Wise Loan Default List (Monthly)";
                    else
                        vTitle = "Age Wise Loan Default List (Weekly)";
                }
                if (vCOId > 0)
                {
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\AgeDefListCO.rpt";
                    if (vMode == "M")
                        vTitle = "Age Wise Loan Default List R.O Wise (Monthly)";
                    else
                        vTitle = "Age Wise Loan Default List R.O Wise (Weekly)";
                }
                if (vLTId > 0)
                {
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\AgeDefListFS.rpt";
                    if (vMode == "M")
                        vTitle = "Age Wise Loan Default List Loan Type Wise (Monthly)";
                    else
                        vTitle = "Age Wise Loan Default List Loan Type Wise (Weekly)";
                }
                if (vFSId > 0)
                {
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\AgeDefListFS.rpt";
                    if (vMode == "M")
                        vTitle = "Age Wise Loan Default List Funder Wise (Monthly)";
                    else
                        vTitle = "Age Wise Loan Default List Funder Wise (Weekly)";
                }
            }
            if (rblDtlSumm.SelectedValue == "D")
            {
                dt = oRpt.rptAgeWiseDefList(vAsonDt, vMode, vFSId, vLTId, vCOId, vMKID, vGRId, vChk1, vChk2, vChk3, vChk4, vChk5, vChk6, vChk7,
                                vProvision1, vProvision2, vProvision3, vProvision4, vProvision5, vProvision6, vProvision7, vBrCode);
            }
            else
            {
                dt = oRpt.rptAgeWiseDefListSumm(vAsonDt, vMode, vFSId, vLTId, vCOId, vMKID, vGRId, vChk1, vChk2, vChk3, vChk4, vChk5, vChk6, vChk7,
                                vProvision1, vProvision2, vProvision3, vProvision4, vProvision5, vProvision6, vProvision7, vBrCode);
            }
            if (rblDtlSumm.SelectedValue == "D")
            {
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                    rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                    rptDoc.SetParameterValue("pBranch", vBranch);
                    rptDoc.SetParameterValue("dtAsOn", txtDate.Text);
                    rptDoc.SetParameterValue("pTitle", vTitle);
                    if (pMode == "PDF")
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Age wise Default List");
                    else if (pMode == "Excel")
                        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Age wise Default List");

                    rptDoc.Dispose();
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
            else
            {
                string vFileNm;
                System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();

                tdx.Controls.Add(DataGrid1);
                tdx.Visible = false;
                vFileNm = "attachment;filename=Agewise";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
                //htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='13'><b><u><font size='5'>Age wise Default (Summary)</font></u></b></td></tr>");
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

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            if (rblDtlSumm.SelectedValue == "D")
            {
                SetParameterForRptData("PDF");
            }
            else
            {
                gblFuction.MsgPopup("Summary Can not be displayed in PDF Format");
                return;
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlLT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlLT.SelectedValue) > 0)
            {
                ddlCO.SelectedIndex = -1;
                ddlFS.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlFS.SelectedValue) > 0)
            {
                ddlCO.SelectedIndex = -1;
                ddlLT.SelectedIndex = -1;
            }
        }

    }
}
