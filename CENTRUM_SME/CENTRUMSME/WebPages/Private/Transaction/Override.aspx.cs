using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.Data;
using System.IO;
using System.Configuration;
using PRATAM.Service_Equifax;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class Override : CENTRUMBAse
    {
        string pathImage = "", PathKYCImage = "", CustomerId = "", CCRUserName = "", CCRPassword = "", PCSUserName = "", PCSPassword = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch();
                txtFrmDt.Text = txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtFrmDtCB.Text = txtToDtCB.Text = Convert.ToString(Session[gblValue.LoginDate]);
                ViewState["Override"] = null;
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Override";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuOverride);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanAdd == "N" || this.CanAdd == null || this.CanAdd == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Override", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void PopBranch()
        {
            ddlBranch.Items.Clear();
            CMember oCM = new CMember();
            DataTable dt = new DataTable(); ;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();

                dt = oCM.GetBranchByBrCode(vBrCode);
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataBind();

                    ddlCBBranch.DataTextField = "BranchName";
                    ddlCBBranch.DataValueField = "BranchCode";
                    ddlCBBranch.DataSource = dt;
                    ddlCBBranch.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCM = null;
            }
        }

        private void LoadGrid()
        {
            CMember oCM = new CMember();
            DataTable dt = new DataTable(); ;
            try
            {
                string vBrCode = ddlBranch.SelectedValue;
                DateTime vFrmDt, vToDt;
                vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                vToDt = gblFuction.setDate(txtToDt.Text);

                dt = oCM.GetOverrideData(vBrCode, vFrmDt, vToDt, rdbOpt.SelectedValue);
                gvOverride.DataSource = dt;
                gvOverride.DataBind();
                ViewState["Override"] = dt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCM = null;
            }
        }

        private void LoadCBGrid()
        {
            CMember oCM = new CMember();
            DataTable dt = new DataTable(); ;
            try
            {
                string vBrCode = ddlCBBranch.SelectedValue;
                DateTime vFrmDt, vToDt;
                vFrmDt = gblFuction.setDate(txtFrmDtCB.Text);
                vToDt = gblFuction.setDate(txtToDtCB.Text);
                dt = oCM.Get15DaysOverCBData(vBrCode, vFrmDt, vToDt);
                gvCBChk.DataSource = dt;
                gvCBChk.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCM = null;
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void btnShowCB_Click(object sender, EventArgs e)
        {
            LoadCBGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            CMember oMr = null;
            DataTable dt = null;
            string vXmlData = "";
            int vErr = 0;
            try
            {
                dt = (DataTable)ViewState["Override"];
                if (dt == null) return;
                int cnt = dt.Rows.Count;

                for (int r = 0; r < cnt; r++)
                {
                    CheckBox chkAppYN = (CheckBox)gvOverride.Rows[r].FindControl("chkAppYN");
                    TextBox txtApprReason = (TextBox)gvOverride.Rows[r].FindControl("txtApprReason");
                    if (chkAppYN.Checked)
                    {
                        if (txtApprReason.Text == "")
                        {
                            gblFuction.AjxMsgPopup("Approved reason cannot be left blank.");
                            return;
                        }
                        dt.Rows[r]["ApproveYN"] = "Y";
                        dt.Rows[r]["ApproveReason"] = txtApprReason.Text;

                    }
                    else
                    {
                        dt.Rows[r]["ApproveYN"] = "N";
                        dt.Rows[r]["ApproveReason"] = "";
                    }
                }
                dt.AcceptChanges();

                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXmlData = oSW.ToString();
                }
                oMr = new CMember();
                vErr = oMr.SaveOverride(vXmlData, Convert.ToInt32(Session[gblValue.UserId]));
                if (vErr > 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    LoadGrid();
                    rdbOpt.SelectedValue = "N";
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMr = null;
                dt = null;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void gvOverride_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkAppYN = (CheckBox)e.Row.FindControl("chkAppYN");
                TextBox txtApprReason = (TextBox)e.Row.FindControl("txtApprReason");
                string AppYN = Convert.ToString(e.Row.Cells[8].Text);
                if (AppYN == "Y")
                {
                    chkAppYN.Checked = true;
                    txtApprReason.Enabled = true;
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#4F81BD");
                    e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                }
            }

        }

        protected void gvCBChk_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnVerify = (Button)e.Row.FindControl("btnVerify");              
                string AppYN = Convert.ToString(e.Row.Cells[10].Text);
                if (AppYN == "Y")
                {                    
                    btnVerify.Enabled = false;
                    btnVerify.Text = "Done";
                }
            }

        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            CCRUserName = ConfigurationManager.AppSettings["CCRUserName"];
            CCRPassword = ConfigurationManager.AppSettings["CCRPassword"];

            PCSUserName = ConfigurationManager.AppSettings["PCSUserName"];
            PCSPassword = ConfigurationManager.AppSettings["PCSPassword"];

            Button btnVerify = (Button)sender;
            GridViewRow gR = (GridViewRow)btnVerify.NamingContainer;
            DataTable dt = null;
            CApplication oCAp = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            int vErr = 0;
            string vCustId = gR.Cells[1].Text.Trim();
            string vCBID = gR.Cells[9].Text.Trim();
            string vCustomerType = gR.Cells[11].Text.Trim();
            string vLoanAppNo = gR.Cells[0].Text.Trim();
            string vBranch = Convert.ToString(ddlCBBranch.SelectedValue);
            string pErrorMsg = "",pStatusDesc = "";
            int pStatus = 0;        
           
            oCAp = new CApplication();
            CEquiFaxDataSubmission oEqui = new CEquiFaxDataSubmission();

            dt = oCAp.GetMemberInfo(vCustId, "E", vCustomerType);
            if (dt.Rows.Count > 0)
            {
                string pEqXml = "";
                //string pEqXml = "<root><InquiryResponseHeader><ClientID>028FZ00016</ClientID><CustRefField>123456</CustRefField><ReportOrderNO>6430865</ReportOrderNO><ProductCode>CCR</ProductCode><SuccessCode>1</SuccessCode><Date>2020-09-21</Date><Time>17:15:21</Time></InquiryResponseHeader><InquiryRequestInfo><InquiryPurpose>00</InquiryPurpose><FirstName>PARAMJIT SINGH</FirstName><InquiryAddresses><seq>1</seq><AddressType>H</AddressType><AddressLine1>VILLAGE ASMAN RANDHAWA PO MASTE KE TEH AND DISTT FEROZEPUR PUNJAB</AddressLine1><State>PB</State><Postal>152002</Postal></InquiryAddresses><InquiryPhones><seq>1</seq><PhoneType>M</PhoneType><Number>9463348097</Number></InquiryPhones><IDDetails><seq>1</seq><IDType>T</IDType><IDValue>AJJPS0032N</IDValue><Source>Inquiry</Source></IDDetails><IDDetails><seq>2</seq><IDType>O</IDType><Source>Inquiry</Source></IDDetails><DOB>1975-01-01</DOB><MFIDetails><FamilyDetails><seq>1</seq><AdditionalNameType>F</AdditionalNameType><AdditionalName>SURJIT SINGH</AdditionalName></FamilyDetails></MFIDetails></InquiryRequestInfo><Score><Type>ERS</Type><Version>3.1</Version></Score><CCRResponse><Status>1</Status><CommercialBureauResponse><Status>1</Status><hit_as_borrower>00</hit_as_borrower><hit_as_guarantor>00</hit_as_guarantor><InquiryResponseHeader><ClientID>028FZ00016</ClientID><CustRefField>123456</CustRefField><ReportOrderNO>6430865</ReportOrderNO><TranID>6430865</TranID><ProductCode>CCR</ProductCode><SuccessCode>1</SuccessCode><Date>2020-09-21</Date><Time>17:15:21</Time></InquiryResponseHeader><InquiryRequestInfo><InquiryPurpose>00</InquiryPurpose><FirstName>PARAMJIT SINGH</FirstName><InquiryAddresses><seq>1</seq><AddressType>Primary</AddressType><AddressLine1>VILLAGE ASMAN RANDHAWA PO MASTE KE TEH AND DISTT FEROZEPUR PUNJAB</AddressLine1><State>PB</State><Postal>152002</Postal></InquiryAddresses><InquiryPhones><seq>1</seq><PhoneType>M</PhoneType><Number>9463348097</Number></InquiryPhones><IDDetails><seq>1</seq><IDType>T</IDType><IDValue>AJJPS0032N</IDValue><Source>Inquiry</Source></IDDetails><DOB>1975-01-01</DOB><MFIDetails><FamilyDetails><seq>1</seq><AdditionalNameType>F</AdditionalNameType><AdditionalName>SURJIT SINGH</AdditionalName></FamilyDetails></MFIDetails></InquiryRequestInfo><CommercialBureauResponseDetails><IDAndContactInfo><CommercialPersonalInfo><roc_BusinessLegalConstitution>false</roc_BusinessLegalConstitution><roc_ClassActivity>false</roc_ClassActivity></CommercialPersonalInfo><CommercialIdentityInfo><roc_CIN>false</roc_CIN></CommercialIdentityInfo></IDAndContactInfo><CommercialCIRSummary><CommercialHeaderDetails><member_name>Indiabulls Housing Finance Ltd.</member_name></CommercialHeaderDetails><SeverityGrid><SeverityGridDetailsMap><_x0032_018-2019 /><_x0032_019-2020 /><_x0032_020-2021 /></SeverityGridDetailsMap></SeverityGrid><EquifaxScoresCommercial /></CommercialCIRSummary><EnquirySummary /></CommercialBureauResponseDetails></CommercialBureauResponse><CIRReportDataLst><InquiryResponseHeader><CustomerCode>IC01</CustomerCode><CustRefField>123456</CustRefField><ReportOrderNO>6430865</ReportOrderNO><ProductCode>PCS</ProductCode><SuccessCode>1</SuccessCode><Date>2020-09-21</Date><Time>17:15:21</Time><HitCode>10</HitCode><CustomerName>IC01</CustomerName></InquiryResponseHeader><InquiryRequestInfo><InquiryPurpose>Other</InquiryPurpose><FirstName>PARAMJIT SINGH</FirstName><InquiryAddresses><seq>1</seq><AddressType>Primary</AddressType><AddressLine1>VILLAGE ASMAN RANDHAWA PO MASTE KE TEH AND DISTT FEROZEPUR PUNJAB</AddressLine1><State>PB</State><Postal>152002</Postal></InquiryAddresses><InquiryPhones><seq>1</seq><PhoneType>M</PhoneType><Number>9463348097</Number></InquiryPhones><IDDetails><seq>1</seq><IDType>T</IDType><IDValue>AJJPS0032N</IDValue><Source>Inquiry</Source></IDDetails><DOB>1975-01-01</DOB><MFIDetails><FamilyDetails><seq>1</seq><AdditionalNameType>F</AdditionalNameType><AdditionalName>SURJIT SINGH</AdditionalName></FamilyDetails></MFIDetails></InquiryRequestInfo><Score><Type>ERS</Type><Version>3.1</Version></Score><CIRReportData><IDAndContactInfo><PersonalInfo><Name><FullName>PARAMJIT SINGH</FullName></Name><_x0020_AliasName /><DateOfBirth>1975-01-01</DateOfBirth><Gender>Female</Gender><Age><Age>45</Age></Age><PlaceOfBirthInfo /><TotalIncome>50001</TotalIncome></PersonalInfo><IdentityInfo><PANId><seq>1</seq><IdNumber>AJJPS0032N</IdNumber></PANId><VoterID><seq>1</seq><IdNumber>JJG1748623</IdNumber></VoterID><NationalIDCard><seq>1</seq><IdNumber>XXXXXXXXXXXX</IdNumber></NationalIDCard><NationalIDCard><seq>2</seq><ReportedDate>2017-10-31</ReportedDate><IdNumber>XXXXXXXXXXXX</IdNumber></NationalIDCard></IdentityInfo><AddressInfo><Seq>1</Seq><ReportedDate>2017-10-31</ReportedDate><Address>W/O PALANISAMY M 9843592974 8/840 A KARUPPARAN NAGAR MUMMOORTHI NAGAR POOLUVAPATTI TIRUPUR</Address><State>TN</State><Postal>641602</Postal></AddressInfo><AddressInfo><Seq>2</Seq><ReportedDate>2012-08-31</ReportedDate><Address>VILLAGE ASMAN RANDHAWA PO MASTE KE TEH AND DISTT FEROZEUMMOORTHI NAGAR POOLUVAPATTI TIRUPUR</Address><State>TN</State><Postal>641602</Postal></AddressInfo><AddressInfo><Seq>3</Seq><ReportedDate>2011-08-31</ReportedDate><Address>W/O PALANISAMY M 9843592974 8/840 A KARUPPARAN NAGAR TIRUPUR</Address><State>TN</State><Postal>641602</Postal></AddressInfo><PhoneInfo><seq>1</seq><typeCode>M</typeCode><ReportedDate>2012-08-31</ReportedDate><Number>9463348097</Number></PhoneInfo><PhoneInfo><seq>2</seq><typeCode>M</typeCode><ReportedDate>2017-10-31</ReportedDate><Number>919843592974</Number></PhoneInfo></IDAndContactInfo><RetailAccountDetails><seq>1</seq><AccountNumber>1363842036193</AccountNumber><Institution>Canara Bank</Institution><AccountType>Business Loan - Priority Sector- Agriculture</AccountType><OwnershipType>Individual</OwnershipType><Balance>73758</Balance><Open>Yes</Open><SanctionAmount>72000</SanctionAmount><DateReported>2018-01-31</DateReported><DateOpened>2017-10-25</DateOpened><InterestRate>9</InterestRate><RepaymentTenure>12</RepaymentTenure><InstallmentAmount>72000</InstallmentAmount><AccountStatus>Standard</AccountStatus><AssetClassification>Standard</AssetClassification><source>INDIVIDUAL</source><History48Months><key>01-18</key><PaymentStatus>STD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>12-17</key><PaymentStatus>STD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>11-17</key><PaymentStatus>STD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-17</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>2</seq><AccountNumber>1363842032037</AccountNumber><Institution>Canara Bank</Institution><AccountType>Business Loan - Priority Sector- Agriculture</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><Open>No</Open><SanctionAmount>100000</SanctionAmount><LastPaymentDate>2017-05-18</LastPaymentDate><DateReported>2017-05-31</DateReported><DateOpened>2016-09-09</DateOpened><DateClosed>2017-05-18</DateClosed><Reason>Closed Account</Reason><InterestRate>9</InterestRate><RepaymentTenure>12</RepaymentTenure><InstallmentAmount>100000</InstallmentAmount><AccountStatus>Closed Account</AccountStatus><AssetClassification>Standard</AssetClassification><source>INDIVIDUAL</source><History48Months><key>05-17</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>04-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>03-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>02-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>01-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>12-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>11-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>09-16</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>3</seq><AccountNumber>1363842029451</AccountNumber><Institution>Canara Bank</Institution><AccountType>Business Loan - Priority Sector- Agriculture</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><Open>No</Open><SanctionAmount>73000</SanctionAmount><LastPaymentDate>2016-09-09</LastPaymentDate><DateReported>2016-09-30</DateReported><DateOpened>2016-01-20</DateOpened><DateClosed>2016-09-09</DateClosed><Reason>Closed Account</Reason><InterestRate>9</InterestRate><RepaymentTenure>12</RepaymentTenure><InstallmentAmount>73000</InstallmentAmount><AccountStatus>Closed Account</AccountStatus><AssetClassification>Standard</AssetClassification><source>INDIVIDUAL</source><History48Months><key>09-16</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>08-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>07-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>06-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>05-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>04-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>03-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>02-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>01-16</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>4</seq><AccountNumber>1363842019129</AccountNumber><Institution>Canara Bank</Institution><AccountType>Business Loan - Priority Sector- Agriculture</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><Open>No</Open><SanctionAmount>33000</SanctionAmount><LastPaymentDate>2014-01-28</LastPaymentDate><DateReported>2014-01-31</DateReported><DateOpened>2013-08-19</DateOpened><DateClosed>2014-01-28</DateClosed><Reason>Closed Account</Reason><InterestRate>9</InterestRate><RepaymentTenure>12</RepaymentTenure><InstallmentAmount>33000</InstallmentAmount><AccountStatus>Closed Account</AccountStatus><AssetClassification>Standard</AssetClassification><source>INDIVIDUAL</source><History48Months><key>01-14</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>12-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>11-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>09-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>08-13</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>5</seq><AccountNumber>1363842019353</AccountNumber><Institution>Canara Bank</Institution><AccountType>Business Loan - Priority Sector- Agriculture</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><Open>No</Open><SanctionAmount>35000</SanctionAmount><LastPaymentDate>2014-01-28</LastPaymentDate><DateReported>2014-01-31</DateReported><DateOpened>2013-09-10</DateOpened><DateClosed>2014-01-28</DateClosed><Reason>Closed Account</Reason><InterestRate>10.45</InterestRate><RepaymentTenure>12</RepaymentTenure><InstallmentAmount>35000</InstallmentAmount><AccountStatus>Closed Account</AccountStatus><AssetClassification>Standard</AssetClassification><source>INDIVIDUAL</source><History48Months><key>01-14</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>12-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>11-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>09-13</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>6</seq><AccountNumber>1363842014483</AccountNumber><Institution>Canara Bank</Institution><AccountType>Business Loan - Priority Sector- Agriculture</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><Open>No</Open><SanctionAmount>110000</SanctionAmount><LastPaymentDate>2013-09-05</LastPaymentDate><DateReported>2013-11-30</DateReported><DateOpened>2012-09-12</DateOpened><DateClosed>2013-09-05</DateClosed><Reason>Closed Account</Reason><InterestRate>10.2</InterestRate><RepaymentTenure>12</RepaymentTenure><AccountStatus>Closed Account</AccountStatus><AssetClassification>Standard</AssetClassification><source>INDIVIDUAL</source><History48Months><key>11-13</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-13</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>09-13</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>08-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>07-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>06-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>05-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>04-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>03-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>02-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>01-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>12-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>11-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>09-12</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>7</seq><AccountNumber>1363842014121</AccountNumber><Institution>Canara Bank</Institution><AccountType>Business Loan - Priority Sector- Agriculture</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><Open>No</Open><SanctionAmount>40000</SanctionAmount><LastPaymentDate>2013-08-19</LastPaymentDate><DateReported>2013-08-31</DateReported><DateOpened>2012-08-16</DateOpened><DateClosed>2013-08-19</DateClosed><Reason>Closed Account</Reason><InterestRate>10.95</InterestRate><RepaymentTenure>12</RepaymentTenure><AccountStatus>Closed Account</AccountStatus><AssetClassification>Standard</AssetClassification><source>INDIVIDUAL</source><History48Months><key>08-13</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>07-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>06-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>05-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>04-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>03-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>02-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>01-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>12-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>11-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>09-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>08-12</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>8</seq><AccountNumber>1363842009411</AccountNumber><Institution>Canara Bank</Institution><AccountType>Business Loan - Priority Sector- Agriculture</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><Open>No</Open><SanctionAmount>50000</SanctionAmount><LastPaymentDate>2012-08-16</LastPaymentDate><DateReported>2012-08-31</DateReported><DateOpened>2011-08-19</DateOpened><DateClosed>2012-08-16</DateClosed><Reason>Closed Account</Reason><InterestRate>11.5</InterestRate><RepaymentTenure>12</RepaymentTenure><AccountStatus>Closed Account</AccountStatus><AssetClassification>Standard</AssetClassification><source>INDIVIDUAL</source><History48Months><key>08-12</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>07-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>06-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>05-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>04-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>03-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>02-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>01-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>12-11</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>11-11</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-11</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>09-11</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>08-11</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountsSummary><NoOfAccounts>8</NoOfAccounts><NoOfActiveAccounts>1</NoOfActiveAccounts><NoOfWriteOffs>0</NoOfWriteOffs><TotalPastDue>0.00</TotalPastDue><SingleHighestCredit>0.00</SingleHighestCredit><SingleHighestSanctionAmount>72000.00</SingleHighestSanctionAmount><TotalHighCredit>0.00</TotalHighCredit><AverageOpenBalance>73758.00</AverageOpenBalance><SingleHighestBalance>73758.00</SingleHighestBalance><NoOfPastDueAccounts>0</NoOfPastDueAccounts><NoOfZeroBalanceAccounts>0</NoOfZeroBalanceAccounts><RecentAccount>Business Loan - Priority Sector- Agriculture on 25-10-2017</RecentAccount><OldestAccount>Business Loan - Priority Sector- Agriculture on 19-08-2011</OldestAccount><TotalBalanceAmount>73758.00</TotalBalanceAmount><TotalSanctionAmount>72000.00</TotalSanctionAmount><TotalCreditLimit>0.0</TotalCreditLimit><TotalMonthlyPaymentAmount>72000.00</TotalMonthlyPaymentAmount></RetailAccountsSummary><ScoreDetails><Type>ERS</Type><Version>3.1</Version><Name>M001</Name><Value>684</Value><ScoringElements><type>RES</type><seq>1</seq><Description>Number of commercial trades</Description></ScoringElements><ScoringElements><type>RES</type><seq>2</seq><code>7a</code><Description>Delinquency or past due amount occurences</Description></ScoringElements><ScoringElements><type>RES</type><seq>3</seq><code>2f</code><Description>Vintage of trades</Description></ScoringElements><ScoringElements><type>RES</type><seq>4</seq><code>8b</code><Description>Number of agri loan trades</Description></ScoringElements><ScoringElements><type>RES</type><seq>5</seq><code>11a</code><Description>Number of or lack of agri loan trades</Description></ScoringElements></ScoreDetails><Enquiries><seq>0</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:53</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>1</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:52</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>2</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:50</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>3</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:49</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>4</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:46</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>5</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:46</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>6</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:43</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>7</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:42</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>8</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:39</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>9</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:38</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>10</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:36</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>11</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:35</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>12</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:32</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>13</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:31</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>14</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:28</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>15</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:27</Time><RequestPurpose>00</RequestPurpose></Enquiries><EnquirySummary><Purpose>ALL</Purpose><Total>318</Total><Past30Days>0</Past30Days><Past12Months>3</Past12Months><Past24Months>318</Past24Months><Recent>2020-02-06</Recent></EnquirySummary><OtherKeyInd><AgeOfOldestTrade>109</AgeOfOldestTrade><NumberOfOpenTrades>0</NumberOfOpenTrades><AllLinesEVERWritten>0.00</AllLinesEVERWritten><AllLinesEVERWrittenIn9Months>0</AllLinesEVERWrittenIn9Months><AllLinesEVERWrittenIn6Months>0</AllLinesEVERWrittenIn6Months></OtherKeyInd><RecentActivities><AccountsDeliquent>0</AccountsDeliquent><AccountsOpened>0</AccountsOpened><TotalInquiries>0</TotalInquiries><AccountsUpdated>0</AccountsUpdated></RecentActivities><DimensionalVariables><TDA_MESMI_CC_PSDAMT_24>0.0</TDA_MESMI_CC_PSDAMT_24><TDA_MESME_INS_PSDAMT_24>0.0</TDA_MESME_INS_PSDAMT_24><TDA_METSU_CC_PSDAMT_3>0.0</TDA_METSU_CC_PSDAMT_3><TDA_SUM_PF_PSDAMT_3>0.0</TDA_SUM_PF_PSDAMT_3></DimensionalVariables></CIRReportData></CIRReportDataLst><CIRReportDataLst><InquiryResponseHeader><CustomerCode>SKSM</CustomerCode><CustRefField>123456</CustRefField><ReportOrderNO>6430865</ReportOrderNO><ProductCode>MCS</ProductCode><SuccessCode>1</SuccessCode><Date>2020-09-21</Date><Time>17:15:21</Time><HitCode>10</HitCode><CustomerName>SKSM</CustomerName></InquiryResponseHeader><InquiryRequestInfo><InquiryPurpose>Other</InquiryPurpose><FirstName>PARAMJIT SINGH</FirstName><InquiryAddresses><seq>1</seq><AddressType>Primary</AddressType><AddressLine1>VILLAGE ASMAN RANDHAWA PO MASTE KE TEH AND DISTT FEROZEPUR PUNJAB</AddressLine1><State>PB</State><Postal>152002</Postal></InquiryAddresses><InquiryPhones><seq>1</seq><PhoneType>M</PhoneType><Number>9463348097</Number></InquiryPhones><IDDetails><seq>1</seq><IDType>T</IDType><IDValue>AJJPS0032N</IDValue><Source>Inquiry</Source></IDDetails><DOB>1975-01-01</DOB><MFIDetails><FamilyDetails><seq>1</seq><AdditionalNameType>F</AdditionalNameType><AdditionalName>SURJIT SINGH</AdditionalName></FamilyDetails></MFIDetails></InquiryRequestInfo><Score><Type>ERS</Type><Version>3.1</Version></Score><CIRReportData><IDAndContactInfo><PersonalInfo><Name><FullName>PARAMJIT SINGH</FullName></Name><_x0020_AliasName /><DateOfBirth>1975-01-01</DateOfBirth><Gender>Female</Gender><Age><Age>45</Age></Age><PlaceOfBirthInfo /><Occupation>DAILY LABOURER</Occupation><MaritalStatus>Married</MaritalStatus></PersonalInfo><IdentityInfo><PANId><seq>1</seq><ReportedDate>2015-10-14</ReportedDate><IdNumber>AJJPS0032N</IdNumber></PANId><VoterID><seq>1</seq><ReportedDate>2015-05-14</ReportedDate><IdNumber>ABK0198275</IdNumber></VoterID><NationalIDCard><seq>1</seq><ReportedDate>2015-10-14</ReportedDate><IdNumber>XXXXXXXXXXXX</IdNumber></NationalIDCard><RationCard><seq>1</seq><ReportedDate>2015-05-14</ReportedDate><IdNumber>746009953378</IdNumber></RationCard></IdentityInfo><AddressInfo><Seq>1</Seq><ReportedDate>2016-05-03</ReportedDate><Address>VILLAGE ASMAN RANDHAWA PO MASTE KE TEH AND DISTT FEROZEADHYA PRADESH</Address><State>MP</State><Postal>480001</Postal></AddressInfo><AddressInfo><Seq>2</Seq><ReportedDate>2016-04-13</ReportedDate><Address>456 LONIYA KARBAL LONIYA KARBAL CHHINDWARA CHHINDWARA MADHYA PRADESH</Address><State>MP</State><Postal>480001</Postal></AddressInfo><PhoneInfo><seq>1</seq><typeCode>H</typeCode><ReportedDate>2016-05-03</ReportedDate><Number>9463348097</Number></PhoneInfo><PhoneInfo><seq>2</seq><typeCode>H</typeCode><ReportedDate>2016-04-13</ReportedDate><Number>8959844196</Number></PhoneInfo></IDAndContactInfo><MicrofinanceAccountDetails><branchIDMFI>MPGL0936</branchIDMFI><kendraIDMFI>411279</kendraIDMFI><seq>0</seq><id>MPGL09360014615</id><AccountNumber>MPGL09360014615</AccountNumber><CurrentBalance>0</CurrentBalance><Institution>The Ratnakar Bank Limited</Institution><InstitutionType>MFI</InstitutionType><PastDueAmount>0</PastDueAmount><DisbursedAmount>24000</DisbursedAmount><LoanCategory>JLG Individual</LoanCategory><LoanPurpose>SERICULTURE</LoanPurpose><Open>No</Open><SanctionAmount>24000</SanctionAmount><LastPaymentDate>2016-09-02</LastPaymentDate><DateReported>2018-02-28</DateReported><DateOpened>2015-04-08</DateOpened><DateClosed>2016-09-02</DateClosed><Reason>Closed Account</Reason><LoanCycleID>2</LoanCycleID><DateSanctioned>2015-04-08</DateSanctioned><DateApplied>2015-04-08</DateApplied><AppliedAmount>24000</AppliedAmount><NoOfInstallments>52</NoOfInstallments><RepaymentTenure>Bi-weekly</RepaymentTenure><InstallmentAmount>780</InstallmentAmount><KeyPerson><Name>CHHOTE MIYAN</Name><RelationType>Husband</RelationType><associationType>K</associationType></KeyPerson><Nominee><Name>CHHOTE MIYAN</Name><RelationType>Husband</RelationType><associationType>N</associationType></Nominee><AccountStatus>Closed Account</AccountStatus><DaysPastDue>0</DaysPastDue><MaxDaysPastDue>0</MaxDaysPastDue><TypeOfInsurance>L</TypeOfInsurance><NumberOfMeetingsHeld>37</NumberOfMeetingsHeld><source>INDIVIDUAL</source><AdditionalMFIDetails><MFIClientFullname>HAMEEDA VI </MFIClientFullname><MFIDOB>1969-01-01</MFIDOB><MFIGender>Female</MFIGender><MemberId>616672889</MemberId><MFIIdentification><VoterID><IdNumber>ABK0198275</IdNumber></VoterID><NationalIDCard><IdNumber>XXXXXXXXXXXX</IdNumber></NationalIDCard><RationCard><IdNumber>746009953378</IdNumber></RationCard></MFIIdentification><MFIAddress><Seq>1</Seq><ReportedDate>2016-04-13</ReportedDate><Address>456 LONIYA KARBAL LONIYA KARBAL CHHINDWARA CHHINDWARA MADHYA PRADESH</Address><State>MP</State><Postal>480001</Postal></MFIAddress><MFIPhones><seq>0</seq><ReportedDate>2016-04-13</ReportedDate><Number>8959844196</Number></MFIPhones></AdditionalMFIDetails><BranchIDMFI>MPGL0936</BranchIDMFI><KendraIDMFI>411279</KendraIDMFI><History24Months><key>02-18</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>01-18</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>12-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>11-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>10-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>09-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>08-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>07-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>06-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>05-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>04-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>03-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>02-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>01-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>12-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>11-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>10-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>09-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>08-16</key><PaymentStatus>000</PaymentStatus></History24Months><History24Months><key>07-16</key><PaymentStatus>000</PaymentStatus></History24Months><History24Months><key>06-16</key><PaymentStatus>000</PaymentStatus></History24Months><History24Months><key>05-16</key><PaymentStatus>000</PaymentStatus></History24Months><History24Months><key>04-16</key><PaymentStatus>000</PaymentStatus></History24Months><History24Months><key>03-16</key><PaymentStatus>000</PaymentStatus></History24Months></MicrofinanceAccountDetails><MicrofinanceAccountDetails><branchIDMFI>MPGL0936</branchIDMFI><kendraIDMFI>81699</kendraIDMFI><seq>1</seq><id>MPGL09360010000</id><AccountNumber>MPGL09360010000</AccountNumber><CurrentBalance>0</CurrentBalance><Institution>The Ratnakar Bank Limited</Institution><InstitutionType>MFI</InstitutionType><PastDueAmount>0</PastDueAmount><DisbursedAmount>12000</DisbursedAmount><LoanCategory>Individual</LoanCategory><LoanPurpose>BAKERY</LoanPurpose><Open>No</Open><SanctionAmount>12000</SanctionAmount><LastPaymentDate>2015-04-03</LastPaymentDate><DateReported>2017-01-05</DateReported><DateOpened>2014-04-17</DateOpened><DateClosed>2015-04-03</DateClosed><Reason>Closed Account</Reason><LoanCycleID>1</LoanCycleID><DateSanctioned>2014-04-17</DateSanctioned><DateApplied>2014-04-17</DateApplied><AppliedAmount>12000</AppliedAmount><NoOfInstallments>50</NoOfInstallments><RepaymentTenure>Weekly</RepaymentTenure><InstallmentAmount>285</InstallmentAmount><KeyPerson><Name>SURJIT SINGH</Name><RelationType>Father</RelationType><associationType>K</associationType></KeyPerson><Nominee><Name>SURJIT SINGH</Name><RelationType>Father</RelationType><associationType>N</associationType></Nominee><AccountStatus>Closed Account</AccountStatus><DaysPastDue>0</DaysPastDue><MaxDaysPastDue>0</MaxDaysPastDue><TypeOfInsurance>L</TypeOfInsurance><NumberOfMeetingsHeld>0</NumberOfMeetingsHeld><source>INDIVIDUAL</source><AdditionalMFIDetails><MFIClientFullname>PARAMJIT SINGH</MFIClientFullname><MFIDOB>1975-01-01</MFIDOB><MFIGender>Female</MFIGender><MemberId>616672824</MemberId><MFIIdentification><PANId><IdNumber>AJJPS0032N</IdNumber></PANId></MFIIdentification><MFIAddress><Seq>1</Seq><ReportedDate>2016-05-03</ReportedDate><Address>VILLAGE ASMAN RANDHAWA PO MASTE KE TEH AND DISTT FEROZEADHYA PRADESH</Address><State>MP</State><Postal>480001</Postal></MFIAddress><MFIPhones><seq>0</seq><ReportedDate>2016-05-03</ReportedDate><Number>9463348097</Number></MFIPhones></AdditionalMFIDetails><BranchIDMFI>MPGL0936</BranchIDMFI><KendraIDMFI>81699</KendraIDMFI><History24Months><key>01-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>12-16</key><PaymentStatus>*</PaymentStatus></History24Months><History24Months><key>11-16</key><PaymentStatus>*</PaymentStatus></History24Months><History24Months><key>10-16</key><PaymentStatus>*</PaymentStatus></History24Months><History24Months><key>09-16</key><PaymentStatus>*</PaymentStatus></History24Months><History24Months><key>08-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>07-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>06-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>05-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>04-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>03-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>02-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>01-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>12-15</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>11-15</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>10-15</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>09-15</key><PaymentStatus>*</PaymentStatus></History24Months><History24Months><key>08-15</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>07-15</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>06-15</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>05-15</key><PaymentStatus>CLSD</PaymentStatus></History24Months></MicrofinanceAccountDetails><MicrofinanceAccountsSummary><id>INDIVIDUAL</id><NoOfActiveAccounts>0</NoOfActiveAccounts><TotalPastDue>0.00</TotalPastDue><NoOfPastDueAccounts>0</NoOfPastDueAccounts><RecentAccount>MicroFinance Personal Loan on 08-04-2015</RecentAccount><TotalBalanceAmount>0.00</TotalBalanceAmount><TotalMonthlyPaymentAmount>0.00</TotalMonthlyPaymentAmount><TotalWrittenOffAmount>0.00</TotalWrittenOffAmount><Id>INDIVIDUAL</Id></MicrofinanceAccountsSummary><IncomeDetails><occupation>DAILY LABOURER</occupation><monthlyIncome>58000</monthlyIncome><monthlyExpense>0</monthlyExpense><seq>2</seq><reportedDate>2016-05-03</reportedDate></IncomeDetails><IncomeDetails><occupation>DAILY LABOURER</occupation><monthlyIncome>58000</monthlyIncome><monthlyExpense>0</monthlyExpense><seq>1</seq><reportedDate>2016-04-13</reportedDate></IncomeDetails><familyDetailsInfo><numberOfDependents>0</numberOfDependents><relatives><AdditionalNameType>Father</AdditionalNameType><AdditionalName>SURJIT SINGH</AdditionalName></relatives><relatives><AdditionalNameType>Father</AdditionalNameType><AdditionalName>SURJIT SINGH</AdditionalName></relatives></familyDetailsInfo><ScoreDetails><Name>M001</Name><Value>53.0</Value></ScoreDetails><Enquiries><seq>0</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:52</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>1</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:52</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>2</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:49</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>3</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:48</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>4</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:46</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>5</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:45</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>6</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:42</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>7</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:41</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>8</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:38</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>9</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:38</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>10</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:35</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>11</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:34</Time><RequestPurpose>00</RequestPurpose></Enquiries><EnquirySummary /><OtherKeyInd><NumberOfOpenTrades>0</NumberOfOpenTrades></OtherKeyInd></CIRReportData></CIRReportDataLst></CCRResponse></root>";
                try
                {
                    //*************************** For Live ***************************************************                      
                    WebServiceSoapClient eq = new WebServiceSoapClient();

                    //************************************************GenderId 1 For MALE else Female****************************************
                    if (dt.Rows[0]["GenderId"].ToString() == "1")
                    {
                        pEqXml = eq.Equifax(dt.Rows[0]["FirstName"].ToString(), dt.Rows[0]["MiddleName"].ToString(), dt.Rows[0]["LastName"].ToString(), dt.Rows[0]["DOB"].ToString()
                            , dt.Rows[0]["AddressType"].ToString(), dt.Rows[0]["AddressLine1"].ToString(), dt.Rows[0]["StateName"].ToString(), dt.Rows[0]["PIN"].ToString(),
                             dt.Rows[0]["MobileNo"].ToString(), dt.Rows[0]["IDType"].ToString(), dt.Rows[0]["IDValue"].ToString(), dt.Rows[0]["AddType"].ToString(),
                              dt.Rows[0]["AddValue"].ToString(), dt.Rows[0]["CoAppRel"].ToString(), dt.Rows[0]["CoAppName"].ToString(),
                              "5750", PCSUserName, PCSPassword, "027FP27137", "9GH", " ", "PCS", "ERS", "3.1", "PRO");
                    }
                    else
                    {
                        pEqXml = eq.Equifax(dt.Rows[0]["FirstName"].ToString(), dt.Rows[0]["MiddleName"].ToString(), dt.Rows[0]["LastName"].ToString(), dt.Rows[0]["DOB"].ToString()
                            , dt.Rows[0]["AddressType"].ToString(), dt.Rows[0]["AddressLine1"].ToString(), dt.Rows[0]["StateName"].ToString(), dt.Rows[0]["PIN"].ToString(),
                             dt.Rows[0]["MobileNo"].ToString(), dt.Rows[0]["IDType"].ToString(), dt.Rows[0]["IDValue"].ToString(), dt.Rows[0]["AddType"].ToString(),
                              dt.Rows[0]["AddValue"].ToString(), dt.Rows[0]["CoAppRel"].ToString(), dt.Rows[0]["CoAppName"].ToString(),
                               "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");
                    }

                    //*************************************************************************
                    vErr = oCAp.UpdateEquifaxInformation(vLoanAppNo, Convert.ToInt32(vCBID), pEqXml, vBranch, "", Convert.ToInt32(Session[gblValue.UserId]), vLogDt, "P", pErrorMsg, ref pStatus, ref pStatusDesc);
                    if (vErr == 1)
                    {
                        string[] arr = pStatusDesc.Split(',');
                        string[] arr1 = arr[0].Split('=');
                        string vAcceptYN = arr1[1].ToString();
                        if (vAcceptYN == "Y")
                        {
                            btnVerify.Text = "Verified";
                            btnVerify.Enabled = false;
                            gR.Cells[6].Text = arr[1].ToString();

                        }
                        else
                        {
                            btnVerify.Text = "Cancel";
                            btnVerify.Enabled = false;
                            gR.Cells[6].Text = arr[1].ToString();
                        }
                        gblFuction.AjxMsgPopup(pStatusDesc);
                    }
                    if (vErr == 5)
                    {
                        gblFuction.AjxMsgPopup("Data Not Saved, Data Error...");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    pErrorMsg = ex.ToString();
                    gblFuction.AjxMsgPopup(pErrorMsg);
                }

            }

        }

        protected void btnDownloadCB_Click(object sender, EventArgs e)
        {
            ImageButton btnShow = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnShow.NamingContainer;
            int vCbId = Convert.ToInt32(gR.Cells[9].Text);
            SetParameterForRptData(vCbId, "PDF");
        }

        private void SetParameterForRptData(int pCbId, string pType)
        {
            DataSet ds = null;
            DataTable dt1 = null, dt2 = null, dt3 = null;
            CReports oRpt = null;
            string vRptPath = "";
            string vBranch = Session[gblValue.BrName].ToString();
            try
            {
                //cvar = 1;
                oRpt = new CReports();
                string enqstatusmsg = "";
                ds = oRpt.Equifax_Report(pCbId, ref  enqstatusmsg);
                if (!String.IsNullOrEmpty(enqstatusmsg))
                {
                    gblFuction.MsgPopup("Equifax Error : " + enqstatusmsg);
                    return;
                }
                else
                {
                    if (ds.Tables.Count == 2 && ds != null)
                    {
                        if (ds.Tables[0].Rows.Count == 0 && ds.Tables[1].Rows.Count == 0)
                        {
                            gblFuction.AjxMsgPopup("New User");
                            return;
                        }
                    }
                }
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[2];
                dt1.TableName = "CBPortDtl";
                dt2.TableName = "CBPortMst";
                dt3.TableName = "CBHistoryMonth";
                if (pType == "PDF")
                {
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptMemberCredit_New.rpt";
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(ds);
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "CreditSummaryReport");
                        Response.ClearHeaders();
                        Response.ClearContent();
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt1 = null;
                dt2 = null;
                oRpt = null;
            }
        }

        protected void ddlCBBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCBBranch.SelectedValue != "-1")
            {
                LoadCBGrid();
            }
        }
    }
}