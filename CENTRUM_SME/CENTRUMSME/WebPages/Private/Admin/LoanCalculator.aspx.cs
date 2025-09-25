using System;
using System.Collections.Generic;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Admin
{
    public partial class LoanCalculator : CENTRUMBAse
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
                txtMprd.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                txtIntRate.Focus();
                PopIntType();
                PopCollType("F");
                ClearControls();
                btnPrnt.Enabled = false;
                txtLnDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.PageHeading = "Loan Calculator";
                //this.ShowPageHeading = true;
                this.ShowBranchName = "Branch :: " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = "Financial Year :: " + Session[gblValue.FinYear].ToString();
                //this.ShowHOMenu = false;
                //this.GetModuleByRole(mnuID.mnuLoanCal);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                //{
                //    btnCal.Visible = false;
                //    btnPrnt.Visible = false;
                //}
                //else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                //{
                //    btnPrnt.Visible = false;
                //}
                //else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                //{
                //    btnPrnt.Visible = false;
                //}
                //else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                //{
                //}
                //else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                //{
                //    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Calculator", false);
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

        /// <summary>
        /// 
        /// </summary>
        private void PopCollType(string pType)
        {
            Dictionary<Int32, string> dColl = new Dictionary<int, string>();
            if (pType == "F")
            {
                dColl.Add(0, "<-- Select -->");
                dColl.Add(1, "Daily");
                dColl.Add(2, "Weekly");
                dColl.Add(3, "Fortnightly");
                dColl.Add(4, "Monthly");
                dColl.Add(5, "HalfYearly");
                dColl.Add(6, "Yearly");
            }
            else if (pType == "R")
            {
                dColl.Add(0, "<-- Select -->");
                dColl.Add(2, "Weekly");
                dColl.Add(3, "Fortnightly");
                dColl.Add(4, "Monthly");
            }
            ddlCalTyp.DataValueField = "key";
            ddlCalTyp.DataTextField = "value";
            ddlCalTyp.DataSource = dColl;
            ddlCalTyp.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtIntRate.Text = "0";
            txtInslNo.Text = "0";
            txtIntPrd.Text = "0";
            txtLnAmt.Text = "0";
            txtMprd.Text = "0";
            txtTotHDay.Text = "0";
            txtIntlAmt.Text = "0";
            txtActInltAmt.Text = "0";
            txtTotInt.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable LoadDtl()
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn();
            dc1.ColumnName = "SlNo";
            dt.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn();
            dc2.ColumnName = "DueDt";
            dt.Columns.Add(dc2);
            DataColumn dc3 = new DataColumn();
            dc3.ColumnName = "PrinceAmt";
            dc3.DataType = System.Type.GetType("System.Double");
            dt.Columns.Add(dc3);
            DataColumn dc4 = new DataColumn();
            dc4.DataType = System.Type.GetType("System.Double");
            dc4.ColumnName = "InstAmt";
            dt.Columns.Add(dc4);
            DataColumn dc5 = new DataColumn();
            dc5.ColumnName = "TotDue";
            dc5.DataType = System.Type.GetType("System.Double");
            dt.Columns.Add(dc5);
            DataColumn dc6 = new DataColumn();
            dc6.ColumnName = "OutStanding";
            dc6.DataType = System.Type.GetType("System.Double");
            dt.Columns.Add(dc6);
            DataColumn dc7 = new DataColumn();
            dc7.ColumnName = "IntOut";
            dc7.DataType = System.Type.GetType("System.Double");
            dt.Columns.Add(dc7);
            dt.AcceptChanges();
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            txtRevDt.Text = vStDt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkHday_CheckedChanged(object sender, EventArgs e)
        {
            DateTime vLogDt = gblFuction.setDate(txtLnDt.Text);
            txtRevDt.Text = "";
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
            txtRevDt.Text = vStDt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbHDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime vLogDt = gblFuction.setDate(txtLnDt.Text);
            txtRevDt.Text = "";
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
            else if (vType == 5)
                vStDt = getDueDt(vLogDt, vType, 1);
            txtRevDt.Text = vStDt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
            return vResult;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCal_Click(object sender, EventArgs e)
        {
            if (ValidteField() == false) return;

            ViewState["SchDt"] = null;
            DataTable dt = null;
            Int32 vIntNo = 0, vColMode = 0, vMortNo = 0, vCalType = 0, vActualInstNo = 0, vTotHoliDay = 0, vRec = 0, vIntType = 0, vDay = 0;
            vMortNo = Convert.ToInt32(txtMprd.Text);
            vCalType = Convert.ToInt32(ddlIntTyp.SelectedValue);

            double vBalInstAmt = 0, vCalInstAmt = 0, vOutBalAmt = 0, vPrinceAmt = 0, vInstAmt = 0, vInstlAmt = 0;
            double vTotInstAmt = 0, vTotInstAmtR = 0, vEMIAmt = 0, vIntPrd = 0, vLoanAmt = 0, vIntRate = 0;
            vIntPrd = Convert.ToDouble(txtIntPrd.Text);
            vLoanAmt = Convert.ToDouble(txtLnAmt.Text);
            vIntRate = Convert.ToDouble(txtIntRate.Text);
            vIntType = Convert.ToInt32(ddlIntTyp.SelectedValue);
            DateTime vStRecDt = gblFuction.setDate(txtRevDt.Text);
            DateTime vLastDate = vStRecDt;
            DateTime vLoanDt = gblFuction.setDate(txtLnDt.Text);
            DateTime vLastRecDt = gblFuction.setDate(txtRevDt.Text);
            vIntNo = Convert.ToInt32(txtInslNo.Text);
            vColMode = Convert.ToInt32(ddlCalTyp.SelectedValue);
            string vMort = rbMort.SelectedValue;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vpMode = ddlCalTyp.SelectedValue;
            TimeSpan ts = (vLastRecDt - vLoanDt);
            vDay = Convert.ToInt32(ts.Days);
            //CHoliday oHD = new CHoliday();
            //Get Functions For respective Values
            if (vColMode == 1) //For Actual Installment No.
            {
                for (int vI = 1; vI <= vIntNo; vI++)
                {
                    vStRecDt = vStRecDt.AddDays(1);
                    vRec = 0;// oHD.ChkHoliday(vStRecDt, vBrCode);
                    if (vRec == 1)
                        vTotHoliDay = vTotHoliDay + 1;
                }
                vActualInstNo = vIntNo - vTotHoliDay;
                vIntNo = vActualInstNo;
            }
            if (chkMort.Checked == true) //Moratarium
                vMortNo = Convert.ToInt32(txtMprd.Text);

            vTotInstAmt = Math.Ceiling((vLoanAmt * vIntRate * vIntPrd) / 1200);
            if (chkMort.Checked == true && rbMort.SelectedValue == "cmMP")
                vPrinceAmt = GetPrince(vIntType, vLoanAmt, vIntRate, (vIntNo - vMortNo));
            else
                vPrinceAmt = GetPrince(vIntType, vLoanAmt, vIntRate, vIntNo);

            if (chkMort.Checked == true && rbMort.SelectedValue == "cmpMI")
                vInstAmt = GetInterest(vIntType, vLoanAmt, vIntRate, (vIntNo - vMortNo), vIntPrd, vDay, vpMode);
            else
                vInstAmt = GetInterest(vIntType, vLoanAmt, vIntRate, vIntNo, vIntPrd, vDay, vpMode);

            vEMIAmt = GetEMIAmount(vColMode, vLoanAmt, vIntRate, vIntNo);

            //Data Row Handling
            DataRow dr;
            dt = LoadDtl();

            for (Int32 vR = 0; vR < vIntNo; vR++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                if (vCalType == 1) //Flat
                {
                    dt.Rows[vR]["SlNo"] = vR + 1;
                    if (vR == 0)
                    {
                        dt.Rows[vR]["SlNo"] = vR + 1;
                        dt.Rows[vR]["DueDt"] = gblFuction.putStrDate(vLastDate);
                    }
                    else
                    {
                        vLastDate = gblFuction.setDate((getDueDt(vLastDate, vColMode, vR)));
                        dt.Rows[vR]["DueDt"] = gblFuction.putStrDate(vLastDate);
                    }
                    if (vR < (vIntNo - 1))
                    {
                        if (vTotInstAmt > (vCalInstAmt + vInstAmt) && vLoanAmt > (vOutBalAmt + vPrinceAmt))
                        {
                            if (chkMort.Checked == true) //For Moratorium
                            {
                                if (rbMort.SelectedValue == "cmMP" && vMortNo > vR)
                                {
                                    dt.Rows[vR]["PrinceAmt"] = 0;
                                    dt.Rows[vR]["InstAmt"] = vInstAmt;
                                    dt.Rows[vR]["TotDue"] = vInstAmt;
                                    vCalInstAmt += vInstAmt;
                                    dt.Rows[vR]["OutStanding"] = vLoanAmt;
                                }
                                else if (rbMort.SelectedValue == "cmpMI" && vMortNo > vR)
                                {
                                    vOutBalAmt += vPrinceAmt;
                                    dt.Rows[vR]["PrinceAmt"] = vPrinceAmt;
                                    dt.Rows[vR]["InstAmt"] = 0;
                                    dt.Rows[vR]["TotDue"] = vPrinceAmt;
                                    dt.Rows[vR]["OutStanding"] = (vLoanAmt - vOutBalAmt);
                                }
                                else
                                {
                                    vOutBalAmt += vPrinceAmt;
                                    dt.Rows[vR]["PrinceAmt"] = vPrinceAmt;
                                    dt.Rows[vR]["InstAmt"] = vInstAmt;
                                    dt.Rows[vR]["TotDue"] = vPrinceAmt + vInstAmt;
                                    vCalInstAmt += vInstAmt;
                                    dt.Rows[vR]["OutStanding"] = (vLoanAmt - vOutBalAmt);
                                    vBalInstAmt = (vInstAmt - (vTotInstAmt - vCalInstAmt));
                                }
                            }
                            else if (chkMort.Checked == false)
                            {
                                vOutBalAmt += vPrinceAmt;
                                dt.Rows[vR]["PrinceAmt"] = vPrinceAmt;
                                dt.Rows[vR]["InstAmt"] = vInstAmt;
                                dt.Rows[vR]["TotDue"] = vPrinceAmt + vInstAmt;
                                vCalInstAmt += vInstAmt;
                                dt.Rows[vR]["OutStanding"] = (vLoanAmt - vOutBalAmt);
                                vBalInstAmt = (vInstAmt - (vTotInstAmt - vCalInstAmt));
                            }
                        }
                        else
                        {
                            if (Convert.ToDouble(dt.Rows[vR - 1]["OutStanding"]) > vPrinceAmt)
                                dt.Rows[vR]["PrinceAmt"] = vPrinceAmt;
                            else
                                dt.Rows[vR]["PrinceAmt"] = Convert.ToDouble(dt.Rows[vR - 1]["OutStanding"]);
                            if (vTotInstAmt - (vCalInstAmt) > vInstAmt)
                                dt.Rows[vR]["InstAmt"] = vInstAmt;
                            else
                                dt.Rows[vR]["InstAmt"] = vTotInstAmt - vCalInstAmt;
                            dt.Rows[vR]["TotDue"] = Convert.ToDouble(dt.Rows[vR]["PrinceAmt"]) + Convert.ToDouble(dt.Rows[vR]["InstAmt"]);
                            dt.Rows[vR]["OutStanding"] = Convert.ToDouble(dt.Rows[vR - 1]["OutStanding"]) - Convert.ToDouble(dt.Rows[vR]["PrinceAmt"]);
                            vCalInstAmt += Convert.ToDouble(dt.Rows[vR]["InstAmt"]);
                        }
                    }
                    if (vR == (vIntNo - 1))
                    {
                        dt.Rows[vR]["DueDt"] = gblFuction.putStrDate(vLastDate);
                        dt.Rows[vR]["PrinceAmt"] = Convert.ToDouble(dt.Rows[vR - 1]["OutStanding"]);
                        dt.Rows[vR]["InstAmt"] = vTotInstAmt - vCalInstAmt;
                        dt.Rows[vR]["TotDue"] = Convert.ToDouble(dt.Rows[vR]["PrinceAmt"]) + Convert.ToDouble(dt.Rows[vR]["InstAmt"]);
                        dt.Rows[vR]["OutStanding"] = 0;
                    }
                }
                else if (vCalType == 2) //Reducing
                {
                    //vInstAmt = GetInterest(vIntType, vLoanAmt, vIntRate, vIntNo - vR, vIntPrd);
                    dt.Rows[vR]["SlNo"] = vR + 1;
                    if (vR == 0)
                    {
                        vInstAmt = GetInterest(vIntType, vLoanAmt, vIntRate, vIntNo - vR, vIntPrd, vDay, vpMode);
                        dt.Rows[vR]["DueDt"] = gblFuction.putStrDate(vLastDate);
                    }
                    else
                    {
                        vLastDate = gblFuction.setDate((getDueDt(vLastDate, vColMode, vR)));
                        vStRecDt = gblFuction.setDate(dt.Rows[vR - 1]["DueDt"].ToString());
                        ts = (vLastDate - vStRecDt);
                        vDay = Convert.ToInt32(ts.Days);
                        vInstAmt = GetInterest(vIntType, vLoanAmt, vIntRate, vIntNo - vR, vIntPrd, vDay, vpMode);
                        dt.Rows[vR]["DueDt"] = gblFuction.putStrDate(vLastDate);
                    }

                    vOutBalAmt += (vEMIAmt - vInstAmt);
                    dt.Rows[vR]["IntOut"] = 0;
                    if (vR == 0)
                    {
                        vLoanAmt -= (vEMIAmt - vInstAmt);
                        dt.Rows[vR]["PrinceAmt"] = vEMIAmt - vInstAmt;
                        dt.Rows[vR]["InstAmt"] = vInstAmt;
                        dt.Rows[vR]["TotDue"] = vEMIAmt;
                        dt.Rows[vR]["OutStanding"] = vLoanAmt;
                        vPrinceAmt += (vEMIAmt - vInstAmt);
                        vTotInstAmtR += vInstAmt;
                    }
                    else
                    {
                        if (vR < (vIntNo - 1))
                        {

                            vLoanAmt -= (vEMIAmt - vInstAmt);
                            dt.Rows[vR]["PrinceAmt"] = vEMIAmt - vInstAmt;
                            dt.Rows[vR]["InstAmt"] = vInstAmt;
                            dt.Rows[vR]["TotDue"] = vEMIAmt;
                            dt.Rows[vR]["OutStanding"] = vLoanAmt;
                            vPrinceAmt += (vEMIAmt - vInstAmt);
                            vTotInstAmtR += vInstAmt;
                        }
                    }
                    if (vR == (vIntNo - 1))
                    {
                        dt.Rows[vR]["DueDt"] = getDueDt(gblFuction.setDate(dt.Rows[vR - 1]["DueDt"].ToString()), vColMode, vR + 1); //vStRecDt
                        dt.Rows[vR]["PrinceAmt"] = Convert.ToDouble(dt.Rows[vR - 1]["OutStanding"]);
                        dt.Rows[vR]["InstAmt"] = vInstAmt;
                        dt.Rows[vR]["TotDue"] = Convert.ToDouble(dt.Rows[vR - 1]["OutStanding"]) + vInstAmt;
                        dt.Rows[vR]["TotDue"] = ((Convert.ToInt32(dt.Rows[vR]["TotDue"]) / 5) + 1) * 5;
                        dt.Rows[vR]["OutStanding"] = 0;
                        vInstAmt = Convert.ToDouble(dt.Rows[vR]["TotDue"]) - Convert.ToDouble(dt.Rows[vR]["PrinceAmt"]);
                        dt.Rows[vR]["InstAmt"] = vInstAmt;

                        vPrinceAmt += Convert.ToDouble(dt.Rows[vR - 1]["OutStanding"]);
                        vTotInstAmtR += vInstAmt;
                    }
                }
                dt.AcceptChanges();
            }
            gvwLnSch.DataSource = dt;
            gvwLnSch.DataBind();
            ViewState["SchDt"] = dt;
            //Output Values of the Cyccle
            txtTotHDay.Text = hdnHdayNo.Value;//
            txtIntlAmt.Text = Convert.ToString(dt.Rows[1]["TotDue"]);
            txtActInltAmt.Text = vIntNo.ToString();
            txtTotInt.Text = Convert.ToString(dt.Compute("Sum(InstAmt)", ""));
            txtLstPayDt.Text = Convert.ToString(dt.Rows[vIntNo - 1]["DueDt"]);
            btnPrnt.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pStDate"></param>
        /// <param name="pColType"></param>
        /// <param name="pCount"></param>
        /// <returns></returns>
        private string getDueDt(DateTime pStDate, Int32 pColType, Int32 pCount)
        {
            string vRtDt = "";
            Int32 vHDay = 0;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            //CHoliday oHD = new CHoliday();

            if (pColType == 1) //Daily
            {
                pStDate = pStDate.AddDays(1);
                //if (chkHday.Checked == true)
                //{
                //    while (oHD.ChkHoliday(pStDate, vBrCode) == 1)
                //    {
                //        if (rbHDay.SelectedValue == "hdA")
                //            pStDate = pStDate.AddDays(1);
                //        else if (rbHDay.SelectedValue == "hdB")
                //            pStDate = pStDate.AddDays(-1);
                //        else if (rbHDay.SelectedValue == "hdS")
                //            pStDate = pStDate.AddDays(1);
                //        vHDay += 1;
                //    }
                //    vRtDt = gblFuction.putStrDate(pStDate);
                //}
                //else
                    vRtDt = gblFuction.putStrDate(pStDate);
            }
            else if (pColType == 2) //Weekly
            {
                pStDate = pStDate.AddDays(7);
                //if (chkHday.Checked == true)
                //{
                //    while (oHD.ChkHoliday(pStDate, vBrCode) == 1)
                //    {
                //        if (rbHDay.SelectedValue == "hdA")
                //            pStDate = pStDate.AddDays(1);
                //        else if (rbHDay.SelectedValue == "hdB")
                //            pStDate = pStDate.AddDays(-1);
                //        else if (rbHDay.SelectedValue == "hdS")
                //            pStDate = pStDate.AddDays(7);
                //        vHDay += 1;
                //    }
                //    vRtDt = gblFuction.putStrDate(pStDate);
                //}
                //else
                    vRtDt = gblFuction.putStrDate(pStDate);
            }
            else if (pColType == 3) //Fortnightly
            {
                pStDate = pStDate.AddDays(14);
                //if (chkHday.Checked == true)
                //{
                //    while (oHD.ChkHoliday(pStDate, vBrCode) == 1)
                //    {
                //        if (rbHDay.SelectedValue == "hdA")
                //            pStDate = pStDate.AddDays(1);
                //        else if (rbHDay.SelectedValue == "hdB")
                //            pStDate = pStDate.AddDays(-1);
                //        else if (rbHDay.SelectedValue == "hdS")
                //            pStDate = pStDate.AddDays(14);
                //        vHDay += 1;
                //    }
                //    vRtDt = gblFuction.putStrDate(pStDate);
                //}
                //else
                    vRtDt = gblFuction.putStrDate(pStDate);
            }
            else if (pColType == 4) //Monthly
            {
                pStDate = pStDate.AddMonths(1);
                //if (chkHday.Checked == true)
                //{
                //    while (oHD.ChkHoliday(pStDate, vBrCode) == 1)
                //    {
                //        if (rbHDay.SelectedValue == "hdA")
                //            pStDate = pStDate.AddDays(1);
                //        else if (rbHDay.SelectedValue == "hdB")
                //            pStDate = pStDate.AddDays(-1);
                //        else if (rbHDay.SelectedValue == "hdS")
                //            pStDate = pStDate.AddMonths(1);
                //        vHDay += 1;
                //    }
                //    vRtDt = gblFuction.putStrDate(pStDate);
                //}
                //else
                    vRtDt = gblFuction.putStrDate(pStDate);
            }
            else if (pColType == 5) //Halfyearly
            {
                pStDate = pStDate.AddMonths(6);
                //if (chkHday.Checked == true)
                //{
                //    while (oHD.ChkHoliday(pStDate, vBrCode) == 1)
                //    {
                //        if (rbHDay.SelectedValue == "hdA")
                //            pStDate = pStDate.AddDays(1);
                //        else if (rbHDay.SelectedValue == "hdB")
                //            pStDate = pStDate.AddDays(-1);
                //        else if (rbHDay.SelectedValue == "hdS")
                //            pStDate = pStDate.AddMonths(6);
                //        vHDay += 1;
                //    }
                //    vRtDt = gblFuction.putStrDate(pStDate);
                //}
                //else
                    vRtDt = gblFuction.putStrDate(pStDate);
            }
            else if (pColType == 6) //Yearly
            {
                pStDate = pStDate.AddMonths(12);
                //if (chkHday.Checked == true)
                //{
                //    while (oHD.ChkHoliday(pStDate, vBrCode) == 1)
                //    {
                //        if (rbHDay.SelectedValue == "hdA")
                //            pStDate = pStDate.AddDays(1);
                //        else if (rbHDay.SelectedValue == "hdB")
                //            pStDate = pStDate.AddDays(-1);
                //        else if (rbHDay.SelectedValue == "hdS")
                //            pStDate = pStDate.AddMonths(12);
                //        vHDay += 1;
                //    }
                //    vRtDt = gblFuction.putStrDate(pStDate);
                //}
                //else
                    vRtDt = gblFuction.putStrDate(pStDate);
            }
            hdnHdayNo.Value = vHDay.ToString();
            return vRtDt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="pLnAmt"></param>
        /// <param name="pRate"></param>
        /// <param name="pInstNo"></param>
        /// <returns></returns>
        private double GetPrince(Int32 pType, double pLnAmt, double pRate, double pInstNo)
        {
            double vPrince = 0;
            string vMode = "";
            if (rbPrin.SelectedValue == "cmPl")
                vMode = "Plus";
            else if (rbPrin.SelectedValue == "cmPm")
                vMode = "Minus";
            if (pType == 1)
            {
                vPrince = (pLnAmt / pInstNo);
                if (vMode == "Plus")
                    vPrince = Math.Ceiling(vPrince);
                else
                    vPrince = Math.Floor(vPrince);
            }
            return vPrince;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="pLnAmt"></param>
        /// <param name="pRate"></param>
        /// <param name="pInstNo"></param>
        /// <returns></returns>
        private double GetInterest(Int32 pType, double pLnAmt, double pRate, double pInstNo, double pIntPerd, Int32 pDay, string pMode)
        {
            double vInstAmt = 0;
            string vMode = "";
            if (rbInt.SelectedValue == "cmIp")
                vMode = "Plus";
            else if (rbInt.SelectedValue == "cmIm")
                vMode = "Minus";
            if (pType == 1)
            {
                vInstAmt = (pLnAmt * pRate * pIntPerd) / 1200;
                vInstAmt = (vInstAmt / pInstNo);
                if (vMode == "Plus")
                    vInstAmt = Math.Ceiling(vInstAmt);
                else if (vMode == "Minus")
                    vInstAmt = Math.Floor(vInstAmt);
            }
            else if (pType == 2)
            {
                if (pMode == "2")
                    vInstAmt = Math.Round((pLnAmt * pRate * pDay) / 36500, 0);
                else if (pMode == "3")
                    vInstAmt = Math.Round((pLnAmt * pRate * pDay) / 36500, 0);
                else if (pMode == "4")
                    vInstAmt = Math.Round((pLnAmt * pRate) / 1200, 0);

                if (vMode == "Plus")
                    vInstAmt = Math.Ceiling(vInstAmt);
                else if (vMode == "Minus")
                    vInstAmt = Math.Floor(vInstAmt);
            }
            return vInstAmt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="pLnAmt"></param>
        /// <param name="pRate"></param>
        /// <param name="pInstNo"></param>
        /// <returns></returns>
        private double GetEMIAmount(Int32 pType, double pLnAmt, double pRate, double pInstNo)
        {
            double vInstAmt = 0;
            Int32 vEMI = 0;
            if (pType == 1)
                vInstAmt = 0;
            else if (pType == 2)
                vInstAmt = (pRate / 5214.28); // Math.Round((pRate / 5214.28),4);
            else if (pType == 3)
                vInstAmt = (pRate / 2607.14); // Math.Round((pRate / 2607.14),4);
            else if (pType == 4)
                vInstAmt = (pRate / 1200);  //Math.Round((pRate / 1200), 4);            
            //vEMI = Math.Floor((pLnAmt * vInstAmt * Math.Pow((1 + vInstAmt), pInstNo))  /  (Math.Pow((1 + vInstAmt), pInstNo) - 1));
            //SET @vEMIAmt = Ceiling((@pLoanAmt * @pPerInst * Power((1 + @pPerInst), @pInstallNo)) / (Power((1 + @pPerInst), @pInstallNo) - 1));
            vEMI = Convert.ToInt32(Math.Ceiling((pLnAmt * vInstAmt * Math.Pow((1 + vInstAmt), pInstNo)) / (Math.Pow((1 + vInstAmt), pInstNo) - 1)));
            if (vEMI%5!=0)
                vEMI = ((vEMI / 5) + 1) * 5;

            return vEMI;
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
        protected void btnPrnt_Click(object sender, EventArgs e)
        {
            Report("PDF");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            Report("Excel");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void Report(string pMode)
        {
            if (ViewState["SchDt"] == null) return;

            string vRptPath = "", vBrName = "";
            vBrName = Session[gblValue.BrName].ToString();
            DataTable dt = null;
            //ReportDocument rptDoc = null;
            CReports oRpt = null;
            string vBranch = Session[gblValue.BrnchCode].ToString();
            try
            {
                dt = (DataTable)ViewState["SchDt"];
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    oRpt = new CReports();
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RepaySche.rpt";
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", "");//CGblIdGenerator.GetBranchAddress1(vBranch));
                    rptDoc.SetParameterValue("pAddress2", "");
                    rptDoc.SetParameterValue("pBranch", vBrName);
                    rptDoc.SetParameterValue("pTitle", "Test Repayment Schedule");
                    //rptDoc.SetParameterValue("pIntRate", txtIntRate.Text + " %");
                    //rptDoc.SetParameterValue("pIntType", ddlIntTyp.SelectedItem.Text);
                    //rptDoc.SetParameterValue("pInstlNo", txtInslNo.Text);
                    //rptDoc.SetParameterValue("pCollType", ddlCalTyp.SelectedItem.Text);
                    //rptDoc.SetParameterValue("pIntPrd", txtIntPrd.Text);
                    //rptDoc.SetParameterValue("pLnAmt", txtLnAmt.Text + ".00");
                    //rptDoc.SetParameterValue("pLnDate", txtLnDt.Text);
                    //rptDoc.SetParameterValue("pCollDate", txtRevDt.Text);
                    //rptDoc.SetParameterValue("pHDayNo", txtTotHDay.Text);
                    if (pMode == "PDF")
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Repayment_Schedule");
                    else if (pMode == "Excel")
                    {
                        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Repayment_Schedule");
                    }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkMort_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMort.Checked == true)
                rbMort.Enabled = true;
            else
                rbMort.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlIntTyp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlIntTyp.SelectedIndex == 2)
            {
                PopCollType("R");
                chkMort.Enabled = false;
                txtMprd.Enabled = false;
                rbMort.Enabled = false;
            }
            else
            {
                PopCollType("F");
                chkMort.Enabled = true;
                txtMprd.Enabled = true;
                rbMort.Enabled = true;
            }
        }
    }
}