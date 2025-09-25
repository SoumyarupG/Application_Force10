using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CENTRUMCA;
using CENTRUMBA;
using System.Drawing;
using System.IO;
using System.Web;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class DashBoard : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                popBranch();
                StatusButton("View");
                ViewState["StateEdit"] = null;
                txtDt.Text = Session[gblValue.LoginDate].ToString();
                txtDt_TextChaged(sender, e);
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Dashboard";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuDashboard);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Dashboard", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void popBranch()
        {
            DataTable dt = null;
            CLoanRecovery oCD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oCD = new CLoanRecovery();
                dt = oCD.GetBranchByBrCode(vBrCode);
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
                if (vBrCode == "0000")
                {
                    ListItem oli = new ListItem("All", "A");
                    ddlBranch.Items.Insert(0, oli);
                }
            }
            finally
            {
                oCD = null;
                dt = null;
            }
        }
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
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            string vFileNm = "";
            string vBr="";
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBrCode = ddlBranch.SelectedValue.ToString();
            string vBranch = Session[gblValue.BrName].ToString();
            string vMonth = "", vYear = "";

            try
            {
                if (txtDt.Text == "")
                {
                    gblFuction.MsgPopup("Please Select Date..");
                    return;
                }
                DateTime InputDate = gblFuction.setDate(txtDt.Text);
                vMonth = GetMonthName(Convert.ToInt32(InputDate.Month));
                vYear = InputDate.Year.ToString();
                dt = oRpt.rptAtAGlance(InputDate, vBrCode);
                if (vBrCode == "A")
                    vBr = "All Branch";
                else
                    vBr = ddlBranch.SelectedItem.Text.ToString();
                vFileNm = "attachment;filename=" + vMonth + "-" + vYear + "_Dashboard_Report.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", vFileNm);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";

                HttpContext.Current.Response.Write("<style> .txt" + "\r\n" + "{mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                Response.Write("<table border='1' cellpadding='0' width='100%'>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + " </font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress2(vBrCode) + " </font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> Branch - " + vBr + " </font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>  Dashboard Report </font></u></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> For the Month - " + vMonth + " And Year - " + vYear + " </font></b></td></tr>");
                Response.Write("<tr>");

                foreach (DataColumn dtCol in dt.Columns)
                {
                    Response.Write("<td ><b>" + dtCol.ColumnName + "<b></td>");
                }
                Response.Write("</tr>");
                foreach (DataRow dr in dt.Rows)
                {
                    Response.Write("<tr style='height:20px;'>");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName == "SlNo")
                        {
                            Response.Write("<td align=left' class='txt' style='width:50px;' >" + Convert.ToString(dr[j]) + "</td>");
                        }
                        else if (dt.Columns[j].ColumnName == "Description")
                        {
                            Response.Write("<td align=left' class='txt' style='width:400px;'>" + Convert.ToString(dr[j]) + "</td>");
                        }
                        else if (dt.Columns[j].ColumnName == "Value")
                        {
                            Response.Write("<td align=left'  style='width:200px;' >" + Convert.ToString(dr[j]) + "</td>");
                        }
                        else
                            Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                    }
                }
                Response.Write("</tr>");
                Response.Write("</table>");
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    //btnAdd.Enabled = false;
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    //btnSave.Enabled = true;
                    //btnCancel.Enabled = true;
                    // btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    //btnAdd.Enabled = true;
                    //btnEdit.Enabled = true;
                    //btnDelete.Enabled = true;
                    //btnSave.Enabled = false;
                    //btnCancel.Enabled = false;
                    // btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    // btnAdd.Enabled = false;
                    // btnEdit.Enabled = false;
                    // btnDelete.Enabled = false;
                    // btnSave.Enabled = true;
                    // btnCancel.Enabled = true;
                    // btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    // btnAdd.Enabled = true;
                    // btnEdit.Enabled = false;
                    // btnDelete.Enabled = false;
                    // btnSave.Enabled = false;
                    // btnCancel.Enabled = false;
                    //  btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    //  btnAdd.Enabled = true;
                    //  btnEdit.Enabled = false;
                    //  btnDelete.Enabled = false;
                    //  btnSave.Enabled = false;
                    //  btnCancel.Enabled = false;
                    // btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Exit":
                    // btnAdd.Visible = false;
                    // btnEdit.Visible = false;
                    // btnDelete.Visible = false;
                    // btnSave.Visible = false;
                    // btnCancel.Visible = false;
                    // btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                ViewState["StateEdit"] = "Edit";
                StatusButton("Edit");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ClearControls()
        {
            // Details of Loans
            lblDisbNo.Text = "0";
            lblBorrowerNo.Text = "0";
            lblDisbAmt.Text = "0";
            lblLoanNo.Text = "0";
            lblLnOutstand.Text = "0";
            // Demand (Periodic)
            lblPreMnPrinDue.Text = "0";
            lblPreMnIntDue.Text = "0";
            lblCurMnPrinDue.Text = "0";
            lblCurMnIntDue.Text = "0";
            lblNextMnPrinDue.Text = "0";
            lblNextMnIntDue.Text = "0";
            //Collection(Periodic)
            lblPreMnPrinColl.Text = "0";
            lblPreMnIntColl.Text = "0";
            lblCurMnPrinColl.Text = "0";
            lblCurMnIntColl.Text = "0";
            lblNextMnPrinColl.Text = "0";
            lblNextMnIntColl.Text = "0";
            //PAR Details (Amount)
            lblPARCur.Text = "0";
            lblPAR30.Text = "0";
            lblPAR3160.Text = "0";
            lblPAR6190.Text = "0";
            lblPAR91120.Text = "0";
            lblPAR120180.Text = "0";
            lblPAR181365.Text = "0";
            lblPAR365.Text = "0";
            //PAR Details (Loans)
            lblPARCurLoans.Text = "0";
            lblPAR30Loans.Text = "0";
            lblPAR3160Loans.Text = "0";
            lblPAR6190Loans.Text = "0";
            lblPAR91120Loans.Text = "0";
            lblPAR120180Loans.Text = "0";
            lblPAR181365Loans.Text = "0";
            lblPAR365Loans.Text = "0";
            //PAR Details (%)
            lblPARCurPer.Text = "0";
            lblPAR30Per.Text = "0";
            lblPAR3160Per.Text = "0";
            lblPAR6190Per.Text = "0";
            lblPAR91120Per.Text = "0";
            lblPAR120180Per.Text = "0";
            lblPAR181365Per.Text = "0";
            lblPAR365Per.Text = "0";
            // Lender Demand
            lblLendPrincDemand.Text = "0";
            lblLendIntDemand.Text = "0";
        }
        private void EnableControl(Boolean Status)
        {
            //ddlCust.Enabled = Status;
            //ddlComment.Enabled = Status;
            //txtVerifDate.Enabled = Status;
            //fileuplodOffVer.Enabled = Status;
            //txtVarifyBy.Enabled = Status;
            //txtVerifDate.Enabled = Status;
        }
        private void GetRecord(string pDate,string pBrCode)
        {
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            //string vBrCode = ddlBranch.SelectedValue.ToString();
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            try
            {
                DateTime InputDate = gblFuction.setDate(pDate);
                dt = oRpt.rptDashBoard(InputDate, pBrCode);
                if (dt.Rows.Count > 0)
                {
                    // Details of Loans
                    lblDisbNo.Text = dt.Rows[0]["No of Disb"].ToString();
                    lblBorrowerNo.Text = dt.Rows[0]["TotBorrower"].ToString();
                    lblDisbAmt.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["LoanDisbAmt"]),0).ToString();
                    lblLoanNo.Text = dt.Rows[0]["No of Loans"].ToString();
                    lblLnOutstand.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["LoanOSAmt"]), 0).ToString();

                    // Demand (Periodic)
                    lblPreMnPrinDue.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["Prevoius Month Principal Due"]), 0).ToString();
                    lblPreMnIntDue.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["Prevoius Month Interest Due"]),0).ToString();
                    lblCurMnPrinDue.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["Current Period Principal Due"]),0).ToString();
                    lblCurMnIntDue.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["Current Period Interest Due"]),0).ToString();
                    lblNextMnPrinDue.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["Next Month Principal Due"]),0).ToString();
                    lblNextMnIntDue.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["Next Month Interest Due"]),0).ToString();

                    //Collection(Periodic)
                    lblPreMnPrinColl.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["Prevoius Month Principal Collection"]),0).ToString();
                    lblPreMnIntColl.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["Prevoius Month Interest Collection"]),0).ToString();
                    lblCurMnPrinColl.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["Current Month Principal Collection"]),0).ToString();
                    lblCurMnIntColl.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["Current Month Interest Collection"]),0).ToString();
                    lblNextMnPrinColl.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["Next Month Principal Collection"]),0).ToString();
                    lblNextMnIntColl.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["Next Month Interest Collection"]),0).ToString();

                    //PAR Details(Amount)
                    lblPARCur.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["PAR(Current)"]),0).ToString();
                    lblPAR30.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["PAR(0-30)"]),0).ToString();
                    lblPAR3160.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["PAR(31-60)"]),0).ToString();
                    lblPAR6190.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["PAR(61-90)"]),0).ToString();
                    lblPAR91120.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["PAR(91-120)"]), 0).ToString();
                    lblPAR120180.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["PAR(121-180)"]), 0).ToString();
                    lblPAR181365.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["PAR(181-365)"]), 0).ToString();
                    lblPAR365.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["PAR(>365)"]), 0).ToString();
                    //PAR Details(Loan)
                    lblPARCurLoans.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["CurLn"]), 0).ToString();
                    lblPAR30Loans.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["P130Ln"]), 0).ToString();
                    lblPAR3160Loans.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["P3160Ln"]), 0).ToString();
                    lblPAR6190Loans.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["P6190Ln"]), 0).ToString();
                    lblPAR91120Loans.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["P91120Ln"]), 0).ToString();
                    lblPAR120180Loans.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["P121180Ln"]), 0).ToString();
                    lblPAR181365Loans.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["P181365"]), 0).ToString();
                    lblPAR365Loans.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["P365Ln"]), 0).ToString();

                    //PAR Details(Percentage)
                    lblPARCurPer.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["CurLnPer"]), 2).ToString();
                    lblPAR30Per.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["P130LnPer"]), 2).ToString();
                    lblPAR3160Per.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["P3160LnPer"]), 2).ToString();
                    lblPAR6190Per.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["P6190LnPer"]), 2).ToString();
                    lblPAR91120Per.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["P91120LnPer"]), 2).ToString();
                    lblPAR120180Per.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["P121180LnPer"]), 2).ToString();
                    lblPAR181365Per.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["P181365Per"]), 2).ToString();
                    lblPAR365Per.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["P365LnPer"]), 2).ToString();




                    // Lender Demand
                    lblLendPrincDemand.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["LenderPrinDue"]), 0).ToString();
                    lblLendIntDemand.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["LenderIntDue"]), 0).ToString();
                }
                else
                {
                    // Details of Loans
                    lblDisbNo.Text = "0";
                    lblBorrowerNo.Text = "0";
                    lblDisbAmt.Text = "0";
                    lblLoanNo.Text = "0";
                    lblLnOutstand.Text = "0";
                    // Demand (Periodic)
                    lblPreMnPrinDue.Text = "0";
                    lblPreMnIntDue.Text = "0";
                    lblCurMnPrinDue.Text = "0";
                    lblCurMnIntDue.Text = "0";
                    lblNextMnPrinDue.Text = "0";
                    lblNextMnIntDue.Text = "0";
                    //Collection(Periodic)
                    lblPreMnPrinColl.Text = "0";
                    lblPreMnIntColl.Text = "0";
                    lblCurMnPrinColl.Text = "0";
                    lblCurMnIntColl.Text = "0";
                    lblNextMnPrinColl.Text = "0";
                    lblNextMnIntColl.Text = "0";
                    //PAR Details (Amount)
                    lblPARCur.Text = "0";
                    lblPAR30.Text = "0";
                    lblPAR3160.Text = "0";
                    lblPAR6190.Text = "0";
                    lblPAR91120.Text = "0";
                    lblPAR120180.Text = "0";
                    lblPAR181365.Text = "0";
                    lblPAR365.Text = "0";
                    //PAR Details (Loans)
                    lblPARCurLoans.Text = "0";
                    lblPAR30Loans.Text = "0";
                    lblPAR3160Loans.Text = "0";
                    lblPAR6190Loans.Text = "0";
                    lblPAR91120Loans.Text = "0";
                    lblPAR120180Loans.Text = "0";
                    lblPAR181365Loans.Text = "0";
                    lblPAR365Loans.Text = "0";
                    // Lender Demand
                    lblLendPrincDemand.Text = "0";
                    lblLendIntDemand.Text = "0";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }
        protected void txtDt_TextChaged(object sender, EventArgs e)
        {
            ClearControls();
            if (txtDt.Text == "")
            {
                gblFuction.MsgPopup("Date can not be blank...");
                return;
            }
            GetRecord(txtDt.Text.ToString(),ddlBranch.SelectedValue.ToString());
        }
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearControls();
            if (txtDt.Text == "")
            {
                gblFuction.MsgPopup("Date can not be blank...");
                return;
            }
            GetRecord(txtDt.Text.ToString(), ddlBranch.SelectedValue.ToString());
        }
        private string GetMonthName(int Month)
        {
            string Mn = "";

            if (Month == 1)
                Mn = "January";
            else if (Month == 2)
                Mn = "February";
            else if (Month == 3)
                Mn = "March";
            else if (Month == 4)
                Mn = "April";
            else if (Month == 5)
                Mn = "May";
            else if (Month == 6)
                Mn = "June";
            else if (Month == 7)
                Mn = "July";
            else if (Month == 8)
                Mn = "August";
            else if (Month == 9)
                Mn = "September";
            else if (Month == 10)
                Mn = "October";
            else if (Month == 11)
                Mn = "November";
            else if (Month == 12)
                Mn = "December";
            else
                Mn = "";
            return Mn;
        
        }
        public  String comma(decimal amount)
        {
            string result = "";
            string amt = "";
            string amt_paisa = "";
            
            amt = amount.ToString();
            int aaa = amount.ToString().IndexOf(".", 0);
            amt_paisa = amount.ToString().Substring(aaa + 1);

            if (amt == amt_paisa)
            {
                amt_paisa = "";
            }
            else
            {
                amt = amount.ToString().Substring(0, amount.ToString().IndexOf(".", 0));
                amt = (amt.Replace(",", "")).ToString();
            }
            switch (amt.Length)
            {
                case 9:
                    if (amt_paisa == "")
                    {
                        result = amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," + 
                                 amt.Substring(4, 2) + "," + amt.Substring(6, 3);
                    }
                    else
                    {
                        result = amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," + 
                                 amt.Substring(4, 2) + "," + amt.Substring(6, 3) + "." + 
                                 amt_paisa;
                    }
                    break;
                case 8:
                    if (amt_paisa == "")
                    {
                        result = amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," + 
                                 amt.Substring(3, 2) + "," + amt.Substring(5, 3);
                    }
                    else
                    {
                        result = amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," + 
                                 amt.Substring(3, 2) + "," + amt.Substring(5, 3) + "." + 
                                 amt_paisa;
                    }
                    break;
                case 7:
                    if (amt_paisa == "")
                    {
                        result = amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," + 
                                 amt.Substring(4, 3);
                    }
                    else
                    {
                        result = amt.Substring(0, 2) + "," + amt.Substring(2, 2) + "," + 
                                 amt.Substring(4, 3) + "." + amt_paisa;
                    }
                    break;
                case 6:
                    if (amt_paisa == "")
                    {
                        result = amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," + 
                                 amt.Substring(3, 3);
                    }
                    else
                    {
                        result = amt.Substring(0, 1) + "," + amt.Substring(1, 2) + "," + 
                                 amt.Substring(3, 3) + "." + amt_paisa;
                    }
                    break;
                case 5:
                    if (amt_paisa == "")
                    {
                        result = amt.Substring(0, 2) + "," + amt.Substring(2, 3);
                    }
                    else
                    {
                        result = amt.Substring(0, 2) + "," + amt.Substring(2, 3) + "." + 
                                 amt_paisa;
                    }
                    break;
                case 4:
                    if (amt_paisa == "")
                    {
                        result = amt.Substring(0, 1) + "," + amt.Substring(1, 3);
                    }
                    else
                    {
                        result = amt.Substring(0, 1) + "," + amt.Substring(1, 3) + "." + 
                                 amt_paisa;
                    }
                    break;
                default:
                    if (amt_paisa == "")
                    {
                        result = amt;
                    }
                    else
                    {
                        result = amt + "." + amt_paisa;
                    }
                    break;
            }
            return result;
        } 
    }
}