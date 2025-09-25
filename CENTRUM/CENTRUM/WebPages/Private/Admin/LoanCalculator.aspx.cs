using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Admin
{
    public partial class LoanCalculator : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            InitBasePage();
            if (!IsPostBack)
            {
                txtIntRate.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                txtInslNo.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                txtIntPrd.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                txtLnAmt.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                txtIntRate.Focus();
                PopIntType();
                PopCollType("F");
                ClearControls();
                btnPrnt.Enabled = false;
                txtLnDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Margdarshak.aspx", false);

                this.PageHeading = "Loan Calculator";
                //this.ShowPageHeading = true;
                this.ShowBranchName = "Branch :: " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = "Financial Year :: " + Session[gblValue.FinYear].ToString();
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuLoanCal);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            catch
            {
                Response.Redirect("~/Margdarshak.aspx", false);
            }
        }

        private void PopIntType()
        {
            Dictionary<Int32, string> dIntTyp = new Dictionary<int, string>();
            dIntTyp.Add(0, "<-- Select -->");
            dIntTyp.Add(1, "Flat");
            dIntTyp.Add(2, "Reducing");
            ddlIntTyp.DataTextField = "value";
            ddlIntTyp.DataValueField = "key";
            ddlIntTyp.DataSource = dIntTyp;
            ddlIntTyp.DataBind();
        }

        private void PopCollType(string pType)
        {
            Dictionary<Int32, string> dColl = new Dictionary<int, string>();
            if (pType == "F")
            {
                dColl.Add(0, "<-- Select -->");
                //dColl.Add(1, "Daily");
                dColl.Add(2, "Weekly");
                dColl.Add(3, "Fortnightly");
                dColl.Add(4, "Monthly");
                //dColl.Add(5, "HalfYearly");
                //dColl.Add(6, "Yearly");
            }
            else if (pType == "R")
            {
                dColl.Add(0, "<-- Select -->");
                dColl.Add(2, "Weekly");
                dColl.Add(3, "Fortnightly");
                dColl.Add(4, "Monthly");
                //dColl.Add(7, "Special Monthly");
            }
            ddlCalTyp.DataValueField = "key";
            ddlCalTyp.DataTextField = "value";
            ddlCalTyp.DataSource = dColl;
            ddlCalTyp.DataBind();
        }

        private void ClearControls()
        {
            txtIntRate.Text = "0";
            txtInslNo.Text = "0";
            txtIntPrd.Text = "0";
            txtLnAmt.Text = "0";
            //txtMprd.Text = "0";
            txtTotHDay.Text = "0";
            txtIntlAmt.Text = "0";
            txtActInltAmt.Text = "0";
            txtTotInt.Text = "0";
            txtInstCalcDay.Text = "0";
            txtInstAmt.Text = "0";
        }

        protected void ddlCalTyp_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime vLogDt = gblFuction.setDate(txtLnDt.Text);
            string vStDt = "";
            Int32 vType = Convert.ToInt32(ddlCalTyp.SelectedValue);
            if (vType == 1)
                vStDt = getDueDt(vLogDt, vType, 1);
            else if (vType == 2)
                vStDt = getDueDt(vLogDt, vType, 1);
            else if (vType == 3)
                vStDt = getDueDt(vLogDt, vType, 1);
            else if (vType == 4)
                vStDt = getDueDt(vLogDt, vType, 1);
            else if (vType == 5)
                vStDt = getDueDt(vLogDt, vType, 1);
            else if (vType == 6)
                vStDt = getDueDt(vLogDt, vType, 1);
            else if (vType == 7)
                vStDt = getDueDt(vLogDt, vType, 1);
            txtRevDt.Text = vStDt;
        }

        private string getDueDt(DateTime pStDate, Int32 pColType, Int32 pCount)
        {
            string vRtDt = "";
            Int32 vHDay = 0;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (pColType == 1)
            {
                pStDate = pStDate.AddDays(1);
                vRtDt = gblFuction.putStrDate(pStDate);
            }
            else if (pColType == 2)
            {
                pStDate = pStDate.AddDays(7);
                vRtDt = gblFuction.putStrDate(pStDate);
            }
            else if (pColType == 3)
            {
                pStDate = pStDate.AddDays(14);
                vRtDt = gblFuction.putStrDate(pStDate);
            }
            else if (pColType == 4)
            {
                pStDate = pStDate.AddMonths(1);
                vRtDt = gblFuction.putStrDate(pStDate);
            }
            else if (pColType == 5)
            {
                pStDate = pStDate.AddMonths(6);
                vRtDt = gblFuction.putStrDate(pStDate);
            }
            else if (pColType == 6)
            {
                pStDate = pStDate.AddMonths(12);
                vRtDt = gblFuction.putStrDate(pStDate);
            }
            //else if (pColType == 7)
            //{
            //    pStDate = pStDate.AddDays(28);
            //    vRtDt = gblFuction.putStrDate(pStDate);
            //}
            hdnHdayNo.Value = vHDay.ToString();
            return vRtDt;
        }

        private bool ValidteField()
        {
            bool vResult = true;
            if (txtLnDt.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Loan Date Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtLnDt");
                vResult = false;
            }
            if (txtIntRate.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Interest Rate Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtIntRate");
                vResult = false;
            }
            if (txtIntRate.Text.Trim() != "")
            {
                if (Convert.ToDouble(txtIntRate.Text) <= 0)
                {
                    gblFuction.MsgPopup("Interest Rate Cannot be ZERO.");
                    gblFuction.focus("ctl00_cph_Main_txtIntRate");
                    vResult = false;
                }
            }
            if (ddlIntTyp.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Interest Type Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_ddlIntTyp");
                vResult = false;
            }
            if (txtInslNo.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Installment No Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtInslNo");
                vResult = false;
            }
            if (txtInslNo.Text.Trim() != "")
            {
                if (Convert.ToDouble(txtInslNo.Text) <= 0)
                {
                    gblFuction.MsgPopup("Installment No Cannot be ZERO.");
                    gblFuction.focus("ctl00_cph_Main_txtInslNo");
                    vResult = false;
                }
            }
            if (ddlCalTyp.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Calculation Type Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_ddlCalTyp");
                vResult = false;
            }
            if (txtIntPrd.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Interest Period Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtIntPrd");
                vResult = false;
            }
            if (txtIntPrd.Text.Trim() != "")
            {
                if (Convert.ToDouble(txtIntPrd.Text) <= 0)
                {
                    gblFuction.MsgPopup("Interest Period Cannot be ZERO.");
                    gblFuction.focus("ctl00_cph_Main_txtIntPrd");
                    vResult = false;
                }
            }
            if (txtLnAmt.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Loan Amount Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtLnAmt");
                vResult = false;
            }
            if (txtLnAmt.Text.Trim() != "")
            {
                if (Convert.ToDouble(txtLnAmt.Text) <= 0)
                {
                    gblFuction.MsgPopup("Loan Amount Cannot be ZERO.");
                    gblFuction.focus("ctl00_cph_Main_txtLnAmt");
                    vResult = false;
                }
            }
            if (txtRevDt.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Recovery Date Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtRevDt");
                vResult = false;
            }
            if (txtInstAmt.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Installment Amount Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtInstAmt");
                vResult = false;
            }
            if (txtInstAmt.Text.Trim() != "")
            {
                if (Convert.ToDouble(txtInstAmt.Text) <= 0)
                {
                    gblFuction.MsgPopup("Installment Amount Cannot be ZERO.");
                    gblFuction.focus("ctl00_cph_Main_txtInstAmt");
                    vResult = false;
                }
            }

            if (txtInstCalcDay.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Interest Calculation Days Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtInstCalcDay");
                vResult = false;
            }
            if (txtInstCalcDay.Text.Trim() != "")
            {
                if (Convert.ToDouble(txtInstCalcDay.Text) <= 0)
                {
                    gblFuction.MsgPopup("Interest Calculation Days Cannot be ZERO.");
                    gblFuction.focus("ctl00_cph_Main_txtInstCalcDay");
                    vResult = false;
                }
            }
            if (ddlReduce.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Reducing Type Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_ddlReduce");
                vResult = false;
            }       

            return vResult;
        }

        //private DataTable LoadDtl()
        //{
        //    DataTable dt = new DataTable();
        //    DataColumn dc1 = new DataColumn();
        //    dc1.ColumnName = "SlNo";
        //    dt.Columns.Add(dc1);
        //    DataColumn dc2 = new DataColumn();
        //    dc2.ColumnName = "DueDt";
        //    dt.Columns.Add(dc2);
        //    DataColumn dc3 = new DataColumn();
        //    dc3.ColumnName = "PrinceAmt";
        //    dc3.DataType = System.Type.GetType("System.Double");
        //    dt.Columns.Add(dc3);
        //    DataColumn dc4 = new DataColumn();
        //    dc4.DataType = System.Type.GetType("System.Double");
        //    dc4.ColumnName = "InstAmt";
        //    dt.Columns.Add(dc4);
        //    DataColumn dc5 = new DataColumn();
        //    dc5.ColumnName = "TotDue";
        //    dc5.DataType = System.Type.GetType("System.Double");
        //    dt.Columns.Add(dc5);
        //    DataColumn dc6 = new DataColumn();
        //    dc6.ColumnName = "OutStanding";
        //    dc6.DataType = System.Type.GetType("System.Double");
        //    dt.Columns.Add(dc6);
        //    DataColumn dc7 = new DataColumn();
        //    dc7.ColumnName = "IntOut";
        //    dc7.DataType = System.Type.GetType("System.Double");
        //    dt.Columns.Add(dc7);
        //    dt.AcceptChanges();
        //    return dt;
        //}

        //protected void chkHday_CheckedChanged(object sender, EventArgs e)
        //{
        //    DateTime vLogDt = gblFuction.setDate(txtLnDt.Text);
        //    txtRevDt.Text = "";
        //    string vStDt = "";
        //    Int32 vType = Convert.ToInt32(ddlCalTyp.SelectedValue);
        //    if (vType == 1)
        //        vStDt = getDueDt(vLogDt, vType, 1);
        //    else if (vType == 2)
        //        vStDt = getDueDt(vLogDt, vType, 1);
        //    else if (vType == 3)
        //        vStDt = getDueDt(vLogDt, vType, 1);
        //    else if (vType == 4)
        //        vStDt = getDueDt(vLogDt, vType, 1);
        //    else if (vType == 5)
        //        vStDt = getDueDt(vLogDt, vType, 1);
        //    else if (vType == 6)
        //        vStDt = getDueDt(vLogDt, vType, 1);
        //    else if (vType == 7)
        //        vStDt = getDueDt(vLogDt, vType, 1);
        //    txtRevDt.Text = vStDt;
        //}

        //protected void rbHDay_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DateTime vLogDt = gblFuction.setDate(txtLnDt.Text);
        //    txtRevDt.Text = "";
        //    string vStDt = "";
        //    Int32 vType = Convert.ToInt32(ddlCalTyp.SelectedValue);
        //    if (vType == 1)
        //        vStDt = getDueDt(vLogDt, vType, 1);
        //    else if (vType == 2)
        //        vStDt = getDueDt(vLogDt, vType, 1);
        //    else if (vType == 3)
        //        vStDt = getDueDt(vLogDt, vType, 1);
        //    else if (vType == 4)
        //        vStDt = getDueDt(vLogDt, vType, 1);
        //    else if (vType == 5)
        //        vStDt = getDueDt(vLogDt, vType, 1);
        //    else if (vType == 5)
        //        vStDt = getDueDt(vLogDt, vType, 1);
        //    else if (vType == 7)
        //        vStDt = getDueDt(vLogDt, vType, 1);
        //    txtRevDt.Text = vStDt;
        //}

        protected void btnCal_Click(object sender, EventArgs e)
        {
            if (ValidteField() == false) return;
            ViewState["SchDt"] = null;
            //CDisburse oLD = null;
            //oLD = new CDisburse();
            DataTable dt = null;
            Int32 vIntNo = 0,  vMortNo = 0, vCalType = 0, vActualInstNo = 0, vTotHoliDay = 0, vRec = 0, vDay = 0,vIntPrd=0;
            Int32 vLoanTypeID = -999;
            string vColMode = "0", vIntType = "0";
            //string vGroupId = "";
            //vMortNo = Convert.ToInt32(txtMprd.Text);
            vCalType = Convert.ToInt32(ddlIntTyp.SelectedValue);
            double vBalInstAmt = 0.0, vCalInstAmt = 0.0, vOutBalAmt = 0.0, vPrinceAmt = 0.0, vInstAmt = 0.0;
            double vTotInstAmt = 0.0, vTotInstAmtR = 0.0, vEMIAmt = 0.0, vLoanAmt = 0.0, vIntRate = 0.0;
            vIntPrd = Convert.ToInt32(txtIntPrd.Text);
            vLoanAmt = Convert.ToDouble(txtLnAmt.Text);
            vIntRate = Convert.ToDouble(txtIntRate.Text);
            //vIntType = Convert.ToInt32(ddlIntTyp.SelectedValue);
            DateTime vStRecDt = gblFuction.setDate(txtRevDt.Text);
            DateTime vLastDate = vStRecDt;
            DateTime vLoanDt = gblFuction.setDate(txtLnDt.Text);
            DateTime vLastRecDt = gblFuction.setDate(txtRevDt.Text);

            vIntNo = Convert.ToInt32(txtInslNo.Text);
            if (Convert.ToInt32(ddlCalTyp.SelectedValue) == 2)
            {
                vColMode = "W";
            }
            else if (Convert.ToInt32(ddlCalTyp.SelectedValue) == 3)
            {
                vColMode = "F";
            }
            else if (Convert.ToInt32(ddlCalTyp.SelectedValue) == 4)
            {
                vColMode = "M";
            }

            if (Convert.ToInt32(ddlIntTyp.SelectedValue) == 1)
            {
                vIntType = "F";
            }
            else if (Convert.ToInt32(ddlIntTyp.SelectedValue) == 2)
            {
                vIntType = "R";
            }

            string vMort = rbMort.SelectedValue;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vpMode = ddlCalTyp.SelectedValue;
            TimeSpan ts = (vLastRecDt - vLoanDt);

            string vInstType,  vTrueRedYN;
            int vDenominator;
            vEMIAmt = Convert.ToDouble(txtInstAmt.Text);
            vInstType=ddlIntTyp.SelectedValue=="1"?"F":"R";
            vTrueRedYN = ddlReduce.SelectedValue;
            vDenominator=Convert.ToInt32(txtInstCalcDay.Text);
            //Get_Schedule(vLoanTypeID, vLoanAmt, vIntRate, vIntNo, Convert.ToInt32(vIntPrd), vStRecDt, vIntType, "", "N", vBrCode, vColMode, "", "", 0, 1, "G", 0, "");
            Get_Schedule(vLoanTypeID, vLoanAmt, vIntRate, vIntNo, vIntPrd, vStRecDt, "L", "", "N", "", vColMode, "", "", 1, 1, "G", 0, "", vEMIAmt,vInstType,vDenominator,vTrueRedYN,vLoanDt);
            btnPrnt.Enabled = true;
        }
            //vDay = Convert.ToInt32(ts.Days);
            //if (vColMode == 1)
            //{
            //    for (int vI = 1; vI <= vIntNo; vI++)
            //    {
            //        vStRecDt = vStRecDt.AddDays(1);
            //        vRec = 0;
            //        if (vRec == 1)
            //            vTotHoliDay = vTotHoliDay + 1;
            //    }
            //    vActualInstNo = vIntNo - vTotHoliDay;
            //    vIntNo = vActualInstNo;
            //}
            ////if (chkMort.Checked == true)
            ////    vMortNo = Convert.ToInt32(txtMprd.Text);
            //vTotInstAmt = Math.Ceiling((vLoanAmt * vIntRate * vIntPrd) / 1200);
            //if (rbMort.SelectedValue == "cmMP")  //chkMort.Checked == true &&
            //    vPrinceAmt = GetPrince(vIntType, vLoanAmt, vIntRate, (vIntNo - vMortNo));
            //else
            //    vPrinceAmt = GetPrince(vIntType, vLoanAmt, vIntRate, vIntNo);

            //if ( rbMort.SelectedValue == "cmpMI")  //chkMort.Checked == true &&
            //    vInstAmt = GetInterest(vIntType, vLoanAmt, vIntRate, (vIntNo - vMortNo), vIntPrd, vDay, vpMode);
            //else
            //    vInstAmt = GetInterest(vIntType, vLoanAmt, vIntRate, vIntNo, vIntPrd, vDay, vpMode);

            //vEMIAmt = GetEMIAmount(vColMode, vLoanAmt, vIntRate, vIntNo);



            //DataRow dr;
            //dt = LoadDtl();
            //for (Int32 vR = 0; vR < vIntNo; vR++)
            //{
            //    dr = dt.NewRow();
            //    dt.Rows.Add(dr);
            //    if (vCalType == 1)
            //    {
            //        dt.Rows[vR]["SlNo"] = vR + 1;
            //        if (vR == 0)
            //        {
            //            dt.Rows[vR]["SlNo"] = vR + 1;
            //            dt.Rows[vR]["DueDt"] = gblFuction.putStrDate(vLastDate);
            //        }
            //        else
            //        {
            //            vLastDate = gblFuction.setDate((getDueDt(vLastDate, vColMode, vR)));
            //            dt.Rows[vR]["DueDt"] = gblFuction.putStrDate(vLastDate);
            //        }
            //        if (vR < (vIntNo - 1))
            //        {
            //            if (vTotInstAmt > (vCalInstAmt + vInstAmt) && vLoanAmt > (vOutBalAmt + vPrinceAmt))
            //            {
            //                if (chkMort.Checked == true)
            //                {
            //                    if (rbMort.SelectedValue == "cmMP" && vMortNo > vR)
            //                    {
            //                        dt.Rows[vR]["PrinceAmt"] = 0;
            //                        dt.Rows[vR]["InstAmt"] = vInstAmt;
            //                        dt.Rows[vR]["TotDue"] = vInstAmt;
            //                        vCalInstAmt += vInstAmt;
            //                        dt.Rows[vR]["OutStanding"] = vLoanAmt;
            //                    }
            //                    else if (rbMort.SelectedValue == "cmpMI" && vMortNo > vR)
            //                    {
            //                        vOutBalAmt += vPrinceAmt;
            //                        dt.Rows[vR]["PrinceAmt"] = vPrinceAmt;
            //                        dt.Rows[vR]["InstAmt"] = 0;
            //                        dt.Rows[vR]["TotDue"] = vPrinceAmt;
            //                        dt.Rows[vR]["OutStanding"] = (vLoanAmt - vOutBalAmt);
            //                    }
            //                    else
            //                    {
            //                        vOutBalAmt += vPrinceAmt;
            //                        dt.Rows[vR]["PrinceAmt"] = vPrinceAmt;
            //                        dt.Rows[vR]["InstAmt"] = vInstAmt;
            //                        dt.Rows[vR]["TotDue"] = vPrinceAmt + vInstAmt;
            //                        vCalInstAmt += vInstAmt;
            //                        dt.Rows[vR]["OutStanding"] = (vLoanAmt - vOutBalAmt);
            //                        vBalInstAmt = (vInstAmt - (vTotInstAmt - vCalInstAmt));
            //                    }
            //                }
            //                else if (chkMort.Checked == false)
            //                {
            //                    vOutBalAmt += vPrinceAmt;
            //                    dt.Rows[vR]["PrinceAmt"] = vPrinceAmt;
            //                    dt.Rows[vR]["InstAmt"] = vInstAmt;
            //                    dt.Rows[vR]["TotDue"] = vPrinceAmt + vInstAmt;
            //                    vCalInstAmt += vInstAmt;
            //                    dt.Rows[vR]["OutStanding"] = (vLoanAmt - vOutBalAmt);
            //                    vBalInstAmt = (vInstAmt - (vTotInstAmt - vCalInstAmt));
            //                }
            //            }
            //            else
            //            {
            //                if (Convert.ToDouble(dt.Rows[vR - 1]["OutStanding"]) > vPrinceAmt)
            //                    dt.Rows[vR]["PrinceAmt"] = vPrinceAmt;
            //                else
            //                    dt.Rows[vR]["PrinceAmt"] = Convert.ToDouble(dt.Rows[vR - 1]["OutStanding"]);
            //                if (vTotInstAmt - (vCalInstAmt) > vInstAmt)
            //                    dt.Rows[vR]["InstAmt"] = vInstAmt;
            //                else
            //                    dt.Rows[vR]["InstAmt"] = vTotInstAmt - vCalInstAmt;
            //                dt.Rows[vR]["TotDue"] = Convert.ToDouble(dt.Rows[vR]["PrinceAmt"]) + Convert.ToDouble(dt.Rows[vR]["InstAmt"]);
            //                dt.Rows[vR]["OutStanding"] = Convert.ToDouble(dt.Rows[vR - 1]["OutStanding"]) - Convert.ToDouble(dt.Rows[vR]["PrinceAmt"]);
            //                vCalInstAmt += Convert.ToDouble(dt.Rows[vR]["InstAmt"]);
            //            }
            //        }
            //        if (vR == (vIntNo - 1))
            //        {
            //            dt.Rows[vR]["DueDt"] = gblFuction.putStrDate(vLastDate);
            //            dt.Rows[vR]["PrinceAmt"] = Convert.ToDouble(dt.Rows[vR - 1]["OutStanding"]);
            //            dt.Rows[vR]["InstAmt"] = vTotInstAmt - vCalInstAmt;
            //            dt.Rows[vR]["TotDue"] = Convert.ToDouble(dt.Rows[vR]["PrinceAmt"]) + Convert.ToDouble(dt.Rows[vR]["InstAmt"]);
            //            dt.Rows[vR]["OutStanding"] = 0;
            //        }
            //    }
            //    else if (vCalType == 2)
            //    {
            //        dt.Rows[vR]["SlNo"] = vR + 1;
            //        if (vR == 0)
            //        {
            //            vInstAmt = GetInterest(vIntType, vLoanAmt, vIntRate, vIntNo - vR, vIntPrd, vDay, vpMode);
            //            dt.Rows[vR]["DueDt"] = gblFuction.putStrDate(vLastDate);
            //        }
            //        else
            //        {
            //            vLastDate = gblFuction.setDate((getDueDt(vLastDate, vColMode, vR)));
            //            vStRecDt = gblFuction.setDate(dt.Rows[vR - 1]["DueDt"].ToString());
            //            ts = (vLastDate - vStRecDt);
            //            vDay = Convert.ToInt32(ts.Days);
            //            vInstAmt = GetInterest(vIntType, vLoanAmt, vIntRate, vIntNo - vR, vIntPrd, vDay, vpMode);
            //            dt.Rows[vR]["DueDt"] = gblFuction.putStrDate(vLastDate);
            //        }
            //        vOutBalAmt += (vEMIAmt - vInstAmt);
            //        dt.Rows[vR]["IntOut"] = 0;
            //        if (vR == 0)
            //        {
            //            if (chkMort.Checked == true)
            //            {
            //                if (rbMort.SelectedValue == "cmMP" && vMortNo > vR)
            //                {
            //                    vLoanAmt -= 0;
            //                    dt.Rows[vR]["PrinceAmt"] = 0;
            //                    dt.Rows[vR]["InstAmt"] = vInstAmt;
            //                    dt.Rows[vR]["TotDue"] = vInstAmt;
            //                    dt.Rows[vR]["OutStanding"] = vLoanAmt;
            //                    vPrinceAmt += 0;
            //                    vTotInstAmtR += vInstAmt;
            //                }
            //                else if (rbMort.SelectedValue == "cmpMI" && vMortNo > vR)
            //                {
            //                    vLoanAmt -= (vEMIAmt - 0);
            //                    dt.Rows[vR]["PrinceAmt"] = vEMIAmt - 0;
            //                    dt.Rows[vR]["InstAmt"] = 0;
            //                    dt.Rows[vR]["TotDue"] = vEMIAmt;
            //                    dt.Rows[vR]["OutStanding"] = vLoanAmt;
            //                    vPrinceAmt += (vEMIAmt - 0);
            //                    vTotInstAmtR += vInstAmt;
            //                }
            //                else
            //                {
            //                    vLoanAmt -= (vEMIAmt - vInstAmt);
            //                    dt.Rows[vR]["PrinceAmt"] = vEMIAmt - vInstAmt;
            //                    dt.Rows[vR]["InstAmt"] = vInstAmt;
            //                    dt.Rows[vR]["TotDue"] = vEMIAmt;
            //                    dt.Rows[vR]["OutStanding"] = vLoanAmt;
            //                    vPrinceAmt += (vEMIAmt - vInstAmt);
            //                    vTotInstAmtR += vInstAmt;
            //                }
            //            }
            //            else if (chkMort.Checked == false)
            //            {
            //                vLoanAmt -= (vEMIAmt - vInstAmt);
            //                dt.Rows[vR]["PrinceAmt"] = vEMIAmt - vInstAmt;
            //                dt.Rows[vR]["InstAmt"] = vInstAmt;
            //                dt.Rows[vR]["TotDue"] = vEMIAmt;
            //                dt.Rows[vR]["OutStanding"] = vLoanAmt;
            //                vPrinceAmt += (vEMIAmt - vInstAmt);
            //                vTotInstAmtR += vInstAmt;
            //            }
            //        }
            //        else
            //        {
            //            if (vR < (vIntNo - 1))
            //            {
            //                if (chkMort.Checked == true)
            //                {
            //                    if (rbMort.SelectedValue == "cmMP" && vMortNo > vR)
            //                    {
            //                        vLoanAmt -= 0;
            //                        dt.Rows[vR]["PrinceAmt"] = 0;
            //                        dt.Rows[vR]["InstAmt"] = vInstAmt;
            //                        dt.Rows[vR]["TotDue"] = vInstAmt;
            //                        dt.Rows[vR]["OutStanding"] = vLoanAmt;
            //                        vPrinceAmt += 0;
            //                        vTotInstAmtR += vInstAmt;
            //                    }
            //                    else if (rbMort.SelectedValue == "cmpMI" && vMortNo > vR)
            //                    {
            //                        vLoanAmt -= (vEMIAmt - 0);
            //                        dt.Rows[vR]["PrinceAmt"] = vEMIAmt - 0;
            //                        dt.Rows[vR]["InstAmt"] = 0;
            //                        dt.Rows[vR]["TotDue"] = vEMIAmt;
            //                        dt.Rows[vR]["OutStanding"] = vLoanAmt;
            //                        vPrinceAmt += (vEMIAmt - 0);
            //                        vTotInstAmtR += 0;
            //                    }
            //                    else
            //                    {
            //                        vLoanAmt -= (vEMIAmt - vInstAmt);
            //                        dt.Rows[vR]["PrinceAmt"] = vEMIAmt - vInstAmt;
            //                        dt.Rows[vR]["InstAmt"] = vInstAmt;
            //                        dt.Rows[vR]["TotDue"] = vEMIAmt;
            //                        dt.Rows[vR]["OutStanding"] = vLoanAmt;
            //                        vPrinceAmt += (vEMIAmt - vInstAmt);
            //                        vTotInstAmtR += vInstAmt;
            //                    }
            //                }
            //                else if (chkMort.Checked == false)
            //                {
            //                    vLoanAmt -= (vEMIAmt - vInstAmt);
            //                    dt.Rows[vR]["PrinceAmt"] = vEMIAmt - vInstAmt;
            //                    dt.Rows[vR]["InstAmt"] = vInstAmt;
            //                    dt.Rows[vR]["TotDue"] = vEMIAmt;
            //                    dt.Rows[vR]["OutStanding"] = vLoanAmt;
            //                    vPrinceAmt += (vEMIAmt - vInstAmt);
            //                    vTotInstAmtR += vInstAmt;
            //                }
            //            }
            //        }
            //        if (vR == (vIntNo - 1))
            //        {
            //            dt.Rows[vR]["DueDt"] = getDueDt(gblFuction.setDate(dt.Rows[vR - 1]["DueDt"].ToString()), vColMode, vR + 1);
            //            dt.Rows[vR]["PrinceAmt"] = Convert.ToDouble(dt.Rows[vR - 1]["OutStanding"]);
            //            dt.Rows[vR]["InstAmt"] = vInstAmt;
            //            dt.Rows[vR]["TotDue"] = Convert.ToDouble(dt.Rows[vR - 1]["OutStanding"]) + vInstAmt;
            //            dt.Rows[vR]["TotDue"] = ((Convert.ToInt32(dt.Rows[vR]["TotDue"]) / 5) + 1) * 5;
            //            dt.Rows[vR]["OutStanding"] = 0;
            //            vInstAmt = Convert.ToDouble(dt.Rows[vR]["TotDue"]) - Convert.ToDouble(dt.Rows[vR]["PrinceAmt"]);
            //            dt.Rows[vR]["InstAmt"] = vInstAmt;

            //            vPrinceAmt += Convert.ToDouble(dt.Rows[vR - 1]["OutStanding"]);
            //            vTotInstAmtR += vInstAmt;
            //        }
            //    }
            //    dt.AcceptChanges();
            //}
            //gvwLnSch.DataSource = dt;
            //gvwLnSch.DataBind();
            //ViewState["SchDt"] = dt;

            //txtTotHDay.Text = hdnHdayNo.Value;//
            ////txtIntlAmt.Text = Convert.ToString(dt.Rows[1]["TotDue"]);
            //txtActInltAmt.Text = vIntNo.ToString();
            //txtTotInt.Text = Convert.ToString(dt.Compute("Sum(InstAmt)", ""));
            //txtLstPayDt.Text = Convert.ToString(dt.Rows[vIntNo - 1]["DueDt"]);
        //    btnPrnt.Enabled = true;
        //}

        //private void Get_Schedule(string pMarketID, Int32 pLoanTypeID, string pLoanID, string pIsDisburse, string pType, double pLoanAmt,
        //                        DateTime pStatrDt, double pInstallment, Int32 pInstallNo, string pPaySchedule, double pIntRate, string pBranch)
        //{
        //    DataTable dt = null;
        //    CDisburse oLD = null;
        //    double VLoanInt = 0.0;
        //    oLD = new CDisburse();
        //    dt = oLD.Get_Schedule(pLoanTypeID, pLoanAmt, pIntRate,pInstallNo,pInstPrd,pStatrDt,pType,pLoanID,pIsDisburse,pBranch,pPaySchedule,"","",pLoanAmt, , pInstallment, , pIntRate);
        //    ViewState["Schedule"] = dt;
        //    if (dt.Rows.Count > 0)
        //    {
        //        //gvSchdl.DataSource = dt;
        //        //gvSchdl.DataBind();
        //        //foreach (DataRow dr in dt.Rows)
        //        //{
        //        //    VLoanInt = VLoanInt +  0 ;
        //        //}
        //        VLoanInt = Convert.ToDouble(dt.Compute("Sum(InstAmt)", ""));
        //        txtTotInt.Text = VLoanInt.ToString();


        //    }
        //}
        private void Get_Schedule(Int32 pLoanTypeID, double pLoanAmt, double pInterest, Int32 pInstallNo, Int32 pInstPeriod,
                 DateTime pStatrDt, string pType, string pLoanID, string pIsDisburse, string pBranch, string pPaySchedule,
                 string pBank, string pChequeNo, Int32 pCollDay, Int32 pCollDayNo, string pLoanType, Int32 pFrDueday, string pPEType,
            double pEmiAmt, string pInstType, int pDenominator, string pTrueRedYN, DateTime pLoanDt)
        {
            DataTable dt = null;
            CDisburse oLD = null;
            double VLoanInt = 0.0;
            oLD = new CDisburse();
            dt = oLD.LoanCalculator(pLoanAmt, pInterest, pInstallNo, pInstPeriod, pStatrDt, pType, pLoanID, pIsDisburse, pBranch, pPaySchedule, pBank, pChequeNo, pCollDay, pCollDayNo,
                pLoanType, pFrDueday, pPEType, pEmiAmt, pInstType, pDenominator, pTrueRedYN, pLoanDt);
            if (dt.Rows.Count > 0)
            {
                gvwLnSch.DataSource = dt;
                gvwLnSch.DataBind();
                ViewState["SchDt"] = dt;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    VLoanInt = VLoanInt +  0 ;
                //}
                //VInstAmt = Convert.ToDouble(dt.Compute("Sum(ResAmt)", ""));
                VLoanInt = Convert.ToDouble(dt.Compute("Sum(InstAmt)", ""));
                txtIntlAmt.Text = Convert.ToString(dt.Rows[0]["ResAmt"]);
                txtActInltAmt.Text = Convert.ToString(dt.Rows.Count);
                txtTotInt.Text = VLoanInt.ToString();
                txtLstPayDt.Text = String.Format("{0:dd/MM/yyyy}", dt.Rows[dt.Rows.Count - 1]["DueDt"]);
            }
        }

            

        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["SchDt"] = null;
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnPrnt_Click(object sender, EventArgs e)
        {
            Report("PDF");
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            Report("Excel");
        }

        private void Report(string pMode)
        {
            if (ViewState["SchDt"] == null) return;
            string vRptPath = "", vBrName = "";
            vBrName = Session[gblValue.BrName].ToString();
            DataTable dt = null;
            CReports oRpt = null;
            string vBranch = Session[gblValue.BrnchCode].ToString();
            try
            {
                dt = (DataTable)ViewState["SchDt"];
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    oRpt = new CReports();
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptRePayment.rpt";
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);

                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", "");
                    rptDoc.SetParameterValue("pAddress2", "");
                    rptDoc.SetParameterValue("pBranch", "");
                    rptDoc.SetParameterValue("pTitle", "Repayment Schedule");
                    rptDoc.SetParameterValue("pRO", "");
                    rptDoc.SetParameterValue("pMarket", "");
                    rptDoc.SetParameterValue("pGroup", "");
                    rptDoc.SetParameterValue("pMemName", "");
                    rptDoc.SetParameterValue("pMemNo", "");
                    rptDoc.SetParameterValue("pLnProduct", "");
                    rptDoc.SetParameterValue("pLoanNo", "");
                    rptDoc.SetParameterValue("pLoanAmt", "");
                    rptDoc.SetParameterValue("pDisbDt", "");
                    rptDoc.SetParameterValue("pMode", "L");
                    if (pMode == "PDF")
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Repayment_Schedule");
                    else if (pMode == "Excel")
                    {
                        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Repayment_Schedule");
                    }
                    rptDoc.Close();
                    rptDoc.Dispose();
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }

        //protected void chkMort_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkMort.Checked == true)
        //        rbMort.Enabled = true;
        //    else
        //        rbMort.Enabled = false;
        //}

        protected void ddlIntTyp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlIntTyp.SelectedIndex == 2)
            {
                PopCollType("R");
                //chkMort.Enabled = true;
                //txtMprd.Enabled = true;
                rbMort.Enabled = true;
            }
            else
            {
                PopCollType("F");
                //chkMort.Enabled = true;
                //txtMprd.Enabled = true;
                rbMort.Enabled = true;
            }
        }
    }
}