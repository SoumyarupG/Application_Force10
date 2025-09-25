using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Linq;
using CENTRUMBA;
using CENTRUMCA;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using System.Xml;
using SendSms;
using Newtonsoft.Json;
using System.Web;
namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class PreCAMBucket : CENTRUMBAse
    {
        protected int vPgNo = 1;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                InitBasePage();
                // txtAdmDtApp.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                ViewState["CusTID"] = null;
                ViewState["LoanAppId"] = null;
                hdUserID.Value = this.UserID.ToString();
                mView.ActiveViewIndex = 0;
                LoadPendingPreCAMList();
                StatusButton("View");
                btnSaveBS.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSaveBS, "").ToString());
                btnSavePL.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSaveBS, "").ToString());
                //txttext.Attributes.Add("onkeypress", "return numericOnly(this);");
            }
            else
            {

            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Pre CAM Bucket";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuPreCAMBucket);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnAppAdd.Visible = false;
                    //btnAppEdit.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnAppAdd.Visible = true;
                    //btnAppEdit.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnAppAdd.Visible = true;
                    //btnAppEdit.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnAppAdd.Visible = false;
                    //btnAppEdit.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Pre CAM Bucket", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        // Population of All Tab's Data on Click of Show Information in Loan Application
        private void ShowLoanRelationDetails(string pLnAppId)
        {
            if (pLnAppId != "")
            {
                // get CB Details
                LoadCBList(pLnAppId);
                // get CB Online Details
                LoadCBOnlineList(pLnAppId);
                // get CB Remarks
                GetCBRemarkByLnAppId(pLnAppId);
                // get Credit Bureau Doc
                ShowUploadedCBDoc(pLnAppId);
                // get uploaded FI Documents
                ShowUploadedFIDoc(pLnAppId);
                // get uploaded CB documents
                ShowUploadedDocuments(pLnAppId);
                //get Bank Statement Details
                LoadBankAcList(pLnAppId);
                //get bank Satement Summary Details
                LoadBankAcSummary(pLnAppId);
                // get bank statement Doc
                ShowUploadedBSDoc(pLnAppId);
                // get Bank Statement Remarks
                GetBSRemarkByLnAppId(pLnAppId);
                // get balance sheet list
                LoadBSList(pLnAppId);
                // get profit and loss list
                LoadPLList(pLnAppId);
                // get Fin Statement Uploaded Doc
                LoadFinStatDoc(pLnAppId);
                // get ITR File
                LoadITRFile(pLnAppId);
                // get Ratio Data
                LoadRatioData(pLnAppId);
                // get Financial Statement Remarks
                GetFSRemarkByLnAppId(pLnAppId);
                // Get Combine MOS
                GetCombineMOS(pLnAppId);
                // Get BankToTRRatio
                GetBankToTR(pLnAppId);
            }
        }
        protected Control GetControlThatCausedPostBack(Page page)
        {
            Control control = null;

            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if (ctrlname != null && ctrlname != string.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form)
                {
                    Control c = page.FindControl(ctl);
                    if (c is System.Web.UI.WebControls.Button || c is System.Web.UI.WebControls.ImageButton)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;

        }
        private void ViewAcess()
        {
            if (mView.ActiveViewIndex == 1)
            {
                this.Menu = false;
                this.PageHeading = "Loan Application";
                this.GetModuleByRole(mnuID.mnuAppLnApplication);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 2)
            {
                this.Menu = false;
                this.PageHeading = "Credit Bureau Information";
                this.GetModuleByRole(mnuID.mnuCB);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 3)
            {
                this.Menu = false;
                this.PageHeading = "Applicant Bank Account Information";
                this.GetModuleByRole(mnuID.mnuBankAC);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 4)
            {
                this.Menu = false;
                this.PageHeading = "Balance Sheet Information";
                this.GetModuleByRole(mnuID.mnuBSStatement);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 5)
            {
                this.Menu = false;
                this.PageHeading = "Profit And Loss Information";
                this.GetModuleByRole(mnuID.mnuPLStatement);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 6)
            {
                this.Menu = false;
                this.PageHeading = "Reference Information";
                this.GetModuleByRole(mnuID.mnuReference);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
        }
        
        private void popBankDetails()
        {
            try
            {
                DataTable dt = null;
                dt = GetBankDetails();
                if (dt.Rows.Count > 0)
                {
                    ViewState["BankDtl"] = dt;
                    gvBankDtl.DataSource = dt;
                    gvBankDtl.DataBind();
                }
                else
                {
                    gvBankDtl.DataSource = null;
                    gvBankDtl.DataBind();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void popBankIWOWDetails()
        {
            try
            {
                DataTable dt = null;
                dt = GetIWOWDetails();
                if (dt.Rows.Count > 0)
                {
                    ViewState["BankIWOWDtl"] = dt;
                    gvIWOWDtl.DataSource = dt;
                    gvIWOWDtl.DataBind();
                }
                else
                {
                    gvIWOWDtl.DataSource = null;
                    gvIWOWDtl.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void popITRDetails()
        {
            try
            {
                DataTable dt = null;
                dt = GetITRDetails();
                if (dt.Rows.Count > 0)
                {
                    ViewState["ITRDtl"] = dt;
                    gvITR.DataSource = dt;
                    gvITR.DataBind();
                }
                else
                {
                    gvITR.DataSource = null;
                    gvITR.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void popCBDetails()
        {
            DataTable dt = null;
            try
            {
                dt = GetCBDetails();
                ViewState["CBDtl"] = dt;
                if (dt.Rows.Count > 0)
                {
                    gvCBDtl.DataSource = dt;
                    gvCBDtl.DataBind();
                }
                else
                {
                    gvCBDtl.DataSource = null;
                    gvCBDtl.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }

        }
        private void popBSDetails()
        {
            //DataTable dt = null;
            //dt = GetBSDetails();
            //ViewState["BSDtl"] = dt;
            //gvBS.DataSource = dt;
            //gvBS.DataBind();
        }
        private void popPLDetails()
        {
            DataTable dt = null;
            try
            {
                dt = GetPLDetails();
                ViewState["PLDtl"] = dt;
                if (dt.Rows.Count > 0)
                {
                    gvIncome.DataSource = dt;
                    gvIncome.DataBind();
                }
                else
                {
                    gvIncome.DataSource = null;
                    gvIncome.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }

        }
        private void popMLAsset()
        {
            DataTable dt = null;
            try
            {
                dt = GetMachAsset();
                ViewState["MLAsset"] = dt;
                if (dt.Rows.Count > 0)
                {
                    gvMLAsset.DataSource = dt;
                    gvMLAsset.DataBind();
                }
                else
                {
                    gvMLAsset.DataSource = null;
                    gvMLAsset.DataBind();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }
        private DataTable GetBankDetails()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SlNo", typeof(int));
            dt.Columns.Add("AccDate", typeof(string));
            dt.Columns.Add("CalDebit", typeof(string));
            dt.Columns.Add("Debit", typeof(decimal));
            dt.Columns.Add("Transfer", typeof(decimal));
            dt.Columns.Add("LoanAvailed", typeof(decimal));
            dt.Columns.Add("CalCredit", typeof(string));
            dt.Columns.Add("Credit", typeof(decimal));
            dt.Columns.Add("SalesFig", typeof(decimal));
            dt.Columns.Add("CCLimit", typeof(decimal));
            dt.Columns.Add("Day5", typeof(decimal));
            dt.Columns.Add("Day10", typeof(decimal));
            dt.Columns.Add("Day15", typeof(decimal));
            dt.Columns.Add("Day20", typeof(decimal));
            dt.Columns.Add("Day25", typeof(decimal));


            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[0]["SlNo"] = 1;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[1]["SlNo"] = 2;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[2]["SlNo"] = 3;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[3]["SlNo"] = 4;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[4]["SlNo"] = 5;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[5]["SlNo"] = 6;
            return dt;
        }
        private DataTable GetIWOWDetails()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SlNo", typeof(int));
            dt.Columns.Add("ChqDate", typeof(string));
            dt.Columns.Add("IWReturn", typeof(decimal));
            dt.Columns.Add("OWReturn", typeof(decimal));
            dt.Columns.Add("EMIReturn", typeof(decimal));
            dt.Columns.Add("ChqClearDate", typeof(string));
            dt.Columns.Add("ChqNo", typeof(string));
            dt.Columns.Add("Reason", typeof(string));
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[0]["SlNo"] = 1;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[1]["SlNo"] = 2;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[2]["SlNo"] = 3;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[3]["SlNo"] = 4;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[4]["SlNo"] = 5;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[5]["SlNo"] = 6;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[6]["SlNo"] = 7;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[7]["SlNo"] = 8;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[8]["SlNo"] = 9;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[9]["SlNo"] = 10;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[10]["SlNo"] = 11;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[11]["SlNo"] = 12;
            return dt;
        }
        private DataTable GetCBDetails()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SLNo", typeof(int));
            // dt.Columns.Add("ApplicationId", typeof(string));//dr[ApplicationId]
            dt.Columns.Add("ReportId", typeof(string));
            dt.Columns.Add("ReportDate", typeof(string));
            dt.Columns.Add("CBNameId", typeof(int));
            dt.Columns.Add("OrgName", typeof(string));
            dt.Columns.Add("LoanType", typeof(string));
            dt.Columns.Add("Active", typeof(string));
            dt.Columns.Add("BankValName", typeof(string));
            dt.Columns.Add("LoanAmt", typeof(float));
            dt.Columns.Add("POSAmt", typeof(float));
            dt.Columns.Add("LoanDate", typeof(string));
            dt.Columns.Add("EMI", typeof(float));
            dt.Columns.Add("Tenure", typeof(int));
            dt.Columns.Add("EMIPaid", typeof(float));
            dt.Columns.Add("PMn1", typeof(string));
            dt.Columns.Add("PMn2", typeof(string));
            dt.Columns.Add("PMn3", typeof(string));
            dt.Columns.Add("PMn4", typeof(string));
            dt.Columns.Add("PMn5", typeof(string));
            dt.Columns.Add("PMn6", typeof(string));
            dt.Columns.Add("PMn7", typeof(string));
            dt.Columns.Add("Remarks", typeof(string));

            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[0]["SlNo"] = 1;
            return dt;

        }
        private DataTable GetBSDetails()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ApplicationId", typeof(string));
            dt.Columns.Add("BSDate", typeof(string));
            dt.Columns.Add("SrNo", typeof(int));
            dt.Columns.Add("ParticularsId", typeof(int));
            dt.Columns.Add("Particulars", typeof(string));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Columns.Add("Change", typeof(decimal));
            dt.Columns.Add("Remarks", typeof(string));
            return dt;
        }
        private DataTable GetPLDetails()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ApplicationId", typeof(string));
            dt.Columns.Add("PLDate", typeof(string));
            dt.Columns.Add("SrNo", typeof(int));
            dt.Columns.Add("ParticularsId", typeof(int));
            dt.Columns.Add("Particulars", typeof(string));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Columns.Add("Change", typeof(decimal));
            dt.Columns.Add("Remarks", typeof(string));
            return dt;
        }
        private DataTable GetRefDetails()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ApplicationId", typeof(string));
            dt.Columns.Add("RefName", typeof(string));
            dt.Columns.Add("RefAddr", typeof(string));
            dt.Columns.Add("RefMNo", typeof(string));
            dt.Columns.Add("RelWithAppId", typeof(int));
            dt.Columns.Add("RelWithApp", typeof(string));
            dt.Columns.Add("OffTelNo", typeof(string));
            dt.Columns.Add("CompName", typeof(string));
            dt.Columns.Add("PersonalRef", typeof(string));
            dt.Columns.Add("TradeRef", typeof(string));
            dt.Columns.Add("RefType", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("VarifiedBy", typeof(string));
            return dt;
        }
        private DataTable GetITRDetails()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SlNo", typeof(int));
            dt.Columns.Add("ITRFileDate", typeof(string));
            dt.Columns.Add("FinYear", typeof(string));
            dt.Columns.Add("ITRFiledYN", typeof(string));
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[0]["SlNo"] = 1;
            return dt;
        }
        private DataTable GetMachAsset()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SlNo", typeof(int));
            dt.Columns.Add("MachDesc", typeof(string));
            dt.Columns.Add("MachSupp", typeof(string));
            dt.Columns.Add("Place", typeof(string));
            dt.Columns.Add("Make", typeof(string));
            dt.Columns.Add("Model", typeof(string));
            dt.Columns.Add("Amount", typeof(decimal));
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[0]["SlNo"] = 1;
            return dt;
        }
        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["BankDtl"];
                int curRow = 0, maxRow = 0, vRow = 0;
                Button txtCur = (Button)sender;
                GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
                curRow = gR.RowIndex;
                maxRow = gvBankDtl.Rows.Count;
                vRow = dt.Rows.Count - 1;
                DataRow dr;
                Label lblSlNo = (Label)gvBankDtl.Rows[curRow].FindControl("lblSlNo");
                TextBox txtDate = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDate");
                TextBox txtCalDebit = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtCalDebit");
                TextBox txtDebit = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDebit");
                TextBox txtTransfer = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtTransfer");
                TextBox txtLoanAvailed = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtLoanAvailed");
                TextBox txtCalCredit = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtCalCredit");
                TextBox txtCredit = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtCredit");
                TextBox txtSalesFig = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtSalesFig");
                TextBox txtCCLimit = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtCCLimit");
                TextBox txtDay5 = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDay5");
                TextBox txtDay10 = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDay10");
                TextBox txtDay15 = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDay15");
                TextBox txtDay20 = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDay20");
                TextBox txtDay25 = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDay25");
                dt.Rows[curRow][0] = lblSlNo.Text;
                // dt.Rows[curRow][1] = Convert.ToDateTime(txtDate.Text).ToString("dd/MM/yyyy");
                if (txtDate.Text == "")
                {
                    //gblFuction.MsgPopup("Please Select Date to add record");
                    gblFuction.AjxMsgPopup("Please Select Date to add record");
                    return;
                }
                else
                {
                    dt.Rows[curRow][1] = (txtDate.Text);
                }

                if (txtCalDebit.Text != "")
                    dt.Rows[curRow][2] = (txtCalDebit.Text);
                else
                    dt.Rows[curRow][2] = 0;
                if (txtDebit.Text != "")
                    dt.Rows[curRow][3] = txtDebit.Text;
                else
                    dt.Rows[curRow][3] = 0;
                if (txtTransfer.Text != "")
                    dt.Rows[curRow][4] = txtTransfer.Text;
                else
                    dt.Rows[curRow][4] = 0;
                if (txtLoanAvailed.Text != "")
                    dt.Rows[curRow][5] = txtLoanAvailed.Text;
                else
                    dt.Rows[curRow][5] = 0;
                if (txtCalCredit.Text != "")
                    dt.Rows[curRow][6] = (txtCalCredit.Text);
                else
                    dt.Rows[curRow][6] = 0;
                if (txtCredit.Text != "")
                    dt.Rows[curRow][7] = txtCredit.Text;
                else
                    dt.Rows[curRow][7] = 0;
                if (txtSalesFig.Text != "")
                    dt.Rows[curRow][8] = txtSalesFig.Text;
                else
                    dt.Rows[curRow][8] = 0;

                if (txtCCLimit.Text != "")
                    dt.Rows[curRow][9] = txtCCLimit.Text;
                else
                    dt.Rows[curRow][9] = 0;

                if (txtDay5.Text != "")
                    dt.Rows[curRow][10] = txtDay5.Text;
                else
                    dt.Rows[curRow][10] = 0;
                if (txtDay10.Text != "")
                    dt.Rows[curRow][11] = txtDay10.Text;
                else
                    dt.Rows[curRow][11] = 0;
                if (txtDay15.Text != "")
                    dt.Rows[curRow][12] = txtDay15.Text;
                else
                    dt.Rows[curRow][12] = 0;

                if (txtDay20.Text != "")
                    dt.Rows[curRow][13] = txtDay20.Text;
                else
                    dt.Rows[curRow][13] = 0;
                if (txtDay25.Text != "")
                    dt.Rows[curRow][14] = txtDay25.Text;
                else
                    dt.Rows[curRow][14] = 0;
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dt.Rows[vRow + 1]["SlNo"] = Convert.ToInt32(((Label)gvBankDtl.Rows[vRow].FindControl("lblSlNo")).Text) + 1;
                dt.AcceptChanges();

                ViewState["BankDtl"] = dt;
                gvBankDtl.DataSource = dt;
                gvBankDtl.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnAddIWOWRow_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["BankIWOWDtl"];
                int curRow = 0, maxRow = 0, vRow = 0;
                Button txtCur = (Button)sender;
                GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
                curRow = gR.RowIndex;
                maxRow = gvIWOWDtl.Rows.Count;
                vRow = dt.Rows.Count - 1;
                DataRow dr;
                Label lblSlNo = (Label)gvIWOWDtl.Rows[curRow].FindControl("lblSlNo");
                TextBox txtChqDate = (TextBox)gvIWOWDtl.Rows[curRow].FindControl("txtChqDate");
                TextBox txtIWReturn = (TextBox)gvIWOWDtl.Rows[curRow].FindControl("txtIWReturn");
                TextBox txtOWReturn = (TextBox)gvIWOWDtl.Rows[curRow].FindControl("txtOWReturn");
                TextBox txtEMIReturn = (TextBox)gvIWOWDtl.Rows[curRow].FindControl("txtEMIReturn");
                TextBox txtClearedDt = (TextBox)gvIWOWDtl.Rows[curRow].FindControl("txtClearedDt");
                TextBox txtChequeNo = (TextBox)gvIWOWDtl.Rows[curRow].FindControl("txtChequeNo");
                TextBox txtReason = (TextBox)gvIWOWDtl.Rows[curRow].FindControl("txtReason");

                dt.Rows[curRow][0] = lblSlNo.Text;
                if (txtChqDate.Text == "")
                {
                    gblFuction.AjxMsgPopup("Please Select Date to add record");
                    return;
                }
                else
                {
                    dt.Rows[curRow][1] = (txtChqDate.Text);
                }

                if (txtIWReturn.Text != "")
                    dt.Rows[curRow][2] = (txtIWReturn.Text);
                else
                    dt.Rows[curRow][2] = 0;
                if (txtOWReturn.Text != "")
                    dt.Rows[curRow][3] = txtOWReturn.Text;
                else
                    dt.Rows[curRow][3] = 0;
                if (txtEMIReturn.Text != "")
                    dt.Rows[curRow][4] = txtEMIReturn.Text;
                else
                    dt.Rows[curRow][4] = 0;
                if (txtClearedDt.Text != "")
                    dt.Rows[curRow][5] = txtClearedDt.Text;
                else
                    dt.Rows[curRow][5] = null;

                if (txtChequeNo.Text != "")
                    dt.Rows[curRow][6] = txtChequeNo.Text;
                else
                    dt.Rows[curRow][6] = "";
                if (txtReason.Text != "")
                    dt.Rows[curRow][7] = (txtReason.Text);
                else
                    dt.Rows[curRow][7] = "";

                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dt.Rows[vRow + 1]["SlNo"] = Convert.ToInt32(((Label)gvIWOWDtl.Rows[vRow].FindControl("lblSlNo")).Text) + 1;
                dt.AcceptChanges();

                ViewState["BankIWOWDtl"] = dt;
                gvIWOWDtl.DataSource = dt;
                gvIWOWDtl.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnAddITR_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["ITRDtl"];
                int curRow = 0, maxRow = 0, vRow = 0;
                Button txtCur = (Button)sender;
                GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
                curRow = gR.RowIndex;
                maxRow = gvITR.Rows.Count;
                vRow = dt.Rows.Count - 1;
                DataRow dr;
                Label lblSlNo = (Label)gvITR.Rows[curRow].FindControl("lblSlNo");
                TextBox txtITRFileDate = (TextBox)gvITR.Rows[curRow].FindControl("txtITRFileDate");
                TextBox txtFinYear = (TextBox)gvITR.Rows[curRow].FindControl("txtFinYear");
                Label lblITRFiledYN = (Label)gvITR.Rows[curRow].FindControl("lblITRFiledYN");
                CheckBox chkITRFile = (CheckBox)gvITR.Rows[curRow].FindControl("chkITRFile");

                dt.Rows[curRow][0] = lblSlNo.Text;
                if (txtITRFileDate.Text == "")
                {
                    gblFuction.AjxMsgPopup("Please Select Date to add record");
                    return;
                }
                else
                {
                    dt.Rows[curRow][1] = (txtITRFileDate.Text);
                }

                if (txtFinYear.Text != "")
                    dt.Rows[curRow][2] = (txtFinYear.Text);
                else
                {
                    gblFuction.AjxMsgPopup("Please Give Input in Financial Year...");
                    return;
                }
                if (chkITRFile.Checked == true)
                    dt.Rows[curRow][3] = "Y";
                else
                    dt.Rows[curRow][3] = "N";
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dt.Rows[vRow + 1]["SlNo"] = Convert.ToInt32(((Label)gvITR.Rows[vRow].FindControl("lblSlNo")).Text) + 1;
                dt.AcceptChanges();

                ViewState["ITRDtl"] = dt;
                gvITR.DataSource = dt;
                gvITR.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnAddAsset_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["MLAsset"];
                int curRow = 0, maxRow = 0, vRow = 0;
                Button txtCur = (Button)sender;
                GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
                curRow = gR.RowIndex;
                maxRow = gvMLAsset.Rows.Count;
                vRow = dt.Rows.Count - 1;
                DataRow dr;
                Label lblSLNoMLAsset = (Label)gvMLAsset.Rows[curRow].FindControl("lblSLNoMLAsset");
                TextBox txtMachDesc = (TextBox)gvMLAsset.Rows[curRow].FindControl("txtMachDesc");
                TextBox txtMachSupp = (TextBox)gvMLAsset.Rows[curRow].FindControl("txtMachSupp");
                TextBox txtMake = (TextBox)gvMLAsset.Rows[curRow].FindControl("txtMake");
                TextBox txtPlace = (TextBox)gvMLAsset.Rows[curRow].FindControl("txtPlace");
                TextBox txtModel = (TextBox)gvMLAsset.Rows[curRow].FindControl("txtModel");
                TextBox txtAmount = (TextBox)gvMLAsset.Rows[curRow].FindControl("txtAmount");

                dt.Rows[curRow][0] = lblSLNoMLAsset.Text;
                if (txtMachDesc.Text == "")
                {
                    gblFuction.AjxMsgPopup("Machine Description Can Not Be Empty");
                    return;
                }
                else
                {
                    dt.Rows[curRow][1] = (txtMachDesc.Text);
                }
                dt.Rows[curRow][2] = (txtMachSupp.Text);
                dt.Rows[curRow][3] = (txtPlace.Text);
                dt.Rows[curRow][4] = (txtMake.Text);
                dt.Rows[curRow][5] = (txtModel.Text);
                if (txtAmount.Text == "")
                {
                    gblFuction.AjxMsgPopup("Amount Can Not Be Empty");
                    return;
                }
                else
                {
                    dt.Rows[curRow][6] = (txtAmount.Text);
                }

                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dt.Rows[vRow + 1]["SlNo"] = Convert.ToInt32(((Label)gvMLAsset.Rows[vRow].FindControl("lblSLNoMLAsset")).Text) + 1;
                dt.AcceptChanges();

                ViewState["MLAsset"] = dt;
                gvMLAsset.DataSource = dt;
                gvMLAsset.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnAddCBRow_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["CBDtl"];
                int curRow = 0, maxRow = 0, vRow = 0;
                Button txtCur = (Button)sender;
                GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
                curRow = gR.RowIndex;
                maxRow = gvBankDtl.Rows.Count;
                vRow = dt.Rows.Count - 1;
                DataRow dr;
                Label lblSLNoCB = (Label)gvCBDtl.Rows[curRow].FindControl("lblSLNoCB");
                TextBox txtReportId = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtReportId");
                TextBox txtReportDate = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtReportDate");
                Label lblCBNameId = (Label)gvCBDtl.Rows[curRow].FindControl("lblCBNameId");
                DropDownList ddlCBname = (DropDownList)gvCBDtl.Rows[curRow].FindControl("ddlCBname");
                TextBox txtOrgNameCB = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtOrgNameCB");
                TextBox txtLoanTypeCB = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtLoanTypeCB");
                Label lblIsActive = (Label)gvCBDtl.Rows[curRow].FindControl("lblIsActive");
                CheckBox chkCBIsActive = (CheckBox)gvCBDtl.Rows[curRow].FindControl("chkCBIsActive");
                TextBox txtBankValName = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtBankValName");
                TextBox txtLoanAmtCB = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtLoanAmtCB");
                TextBox txtPOSAmt = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtPOSAmt");
                TextBox txtLoanDateCB = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtLoanDateCB");
                TextBox txtEMICB = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtEMICB");
                TextBox txtTenureCB = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtTenureCB");
                TextBox txtEMIPaidCB = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtEMIPaidCB");
                TextBox txtPMn1 = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtPMn1");
                TextBox txtPMn2 = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtPMn2");
                TextBox txtPMn3 = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtPMn3");
                TextBox txtPMn4 = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtPMn4");
                TextBox txtPMn5 = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtPMn5");
                TextBox txtPMn6 = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtPMn6");
                TextBox txtPMn7 = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtPMn7");
                TextBox txtRemarks = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtRemarks");

                dt.Rows[curRow][0] = lblSLNoCB.Text;
                if (txtReportId.Text == "")
                {
                    gblFuction.AjxMsgPopup("Please input Unique Report Id");
                    return;
                }
                else
                {
                    dt.Rows[curRow][1] = (txtReportId.Text);
                }
                if (txtReportDate.Text != "")
                    dt.Rows[curRow][2] = (txtReportDate.Text);
                else
                    dt.Rows[curRow][2] = null;
                if (ddlCBname.SelectedValue != "-1")
                    dt.Rows[curRow][3] = ddlCBname.SelectedValue.ToString();
                else
                    dt.Rows[curRow][3] = "-1";
                if (txtOrgNameCB.Text != "")
                    dt.Rows[curRow][4] = (txtOrgNameCB.Text);
                else
                    dt.Rows[curRow][4] = " ";
                if (txtLoanTypeCB.Text != "")
                    dt.Rows[curRow][5] = txtLoanTypeCB.Text;
                else
                    dt.Rows[curRow][5] = "";
                if (chkCBIsActive.Checked == true)
                    dt.Rows[curRow][6] = "Y";
                else
                    dt.Rows[curRow][6] = "N";
                if (txtBankValName.Text != "")
                    dt.Rows[curRow][7] = txtBankValName.Text;
                else
                    dt.Rows[curRow][7] = "";

                if (txtLoanAmtCB.Text != "")
                    dt.Rows[curRow][8] = txtLoanAmtCB.Text;
                else
                {
                    gblFuction.AjxMsgPopup("Please input Loan Amount..");
                    return;
                }

                if (txtPOSAmt.Text != "")
                    dt.Rows[curRow][9] = txtPOSAmt.Text;
                else
                    dt.Rows[curRow][9] = 0;

                if (txtLoanDateCB.Text != "")
                    dt.Rows[curRow][10] = txtLoanDateCB.Text;
                else
                {
                    gblFuction.AjxMsgPopup("Please input Loan Date");
                    return;
                }
                if (txtEMICB.Text != "")
                    dt.Rows[curRow][11] = txtEMICB.Text;
                else
                    dt.Rows[curRow][11] = 0;
                if (txtTenureCB.Text != "")
                    dt.Rows[curRow][12] = txtTenureCB.Text;
                else
                    dt.Rows[curRow][12] = 0;
                if (txtEMIPaidCB.Text != "")
                    dt.Rows[curRow][13] = txtEMIPaidCB.Text;
                else
                    dt.Rows[curRow][13] = 0;
                if (txtPMn1.Text != "")
                    dt.Rows[curRow][14] = txtPMn1.Text;
                else
                    dt.Rows[curRow][14] = "";
                if (txtPMn2.Text != "")
                    dt.Rows[curRow][15] = txtPMn2.Text;
                else
                    dt.Rows[curRow][15] = "";
                if (txtPMn3.Text != "")
                    dt.Rows[curRow][16] = txtPMn3.Text;
                else
                    dt.Rows[curRow][16] = "";
                if (txtPMn4.Text != "")
                    dt.Rows[curRow][17] = txtPMn4.Text;
                else
                    dt.Rows[curRow][17] = "";
                if (txtPMn5.Text != "")
                    dt.Rows[curRow][18] = txtPMn5.Text;
                else
                    dt.Rows[curRow][18] = "";
                if (txtPMn6.Text != "")
                    dt.Rows[curRow][19] = txtPMn6.Text;
                else
                    dt.Rows[curRow][19] = "";
                if (txtPMn7.Text != "")
                    dt.Rows[curRow][20] = txtPMn7.Text;
                else
                    dt.Rows[curRow][20] = "";
                if (txtRemarks.Text != "")
                    dt.Rows[curRow][21] = txtRemarks.Text;
                else
                    dt.Rows[curRow][21] = "";
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dt.Rows[vRow + 1]["SLNo"] = Convert.ToInt32(((Label)gvCBDtl.Rows[vRow].FindControl("lblSLNoCB")).Text) + 1;
                dt.AcceptChanges();

                ViewState["CBDtl"] = dt;
                gvCBDtl.DataSource = dt;
                gvCBDtl.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        protected void txtCalDebit_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0, maxRow = 0, vRow = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtCalDebits = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtCalDebit");
            //GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;


            TextBox txtDbt = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDebit");

            string s = txtCalDebits.Text;
            string[] debits = s.Split('+');
            double i = 0;
            foreach (string debit in debits)
            {
                i = i + Convert.ToDouble(debit);

            }
            txtDbt.Text = i.ToString("0.00");

        }
        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0, maxRow = 0, vRow = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;

            // TextBox txtDate = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDate");
            //GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;
            if (curRow == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    TextBox txtDate = (TextBox)gvBankDtl.Rows[curRow + i].FindControl("txtDate");
                    DateTime CurrentDate = gblFuction.setDate(txtDate.Text);
                    TextBox txtNewDate = (TextBox)gvBankDtl.Rows[curRow + i + 1].FindControl("txtDate");
                    DateTime NewCurrentDate = CurrentDate.AddMonths(1);
                    txtNewDate.Text = NewCurrentDate.ToString("dd/MM/yyyy");
                }
            }
        }
        protected void txtCalCredit_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0, maxRow = 0, vRow = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtCalCredit = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtCalCredit");
            //GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;


            TextBox txtCredit = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtCredit");

            string s = txtCalCredit.Text;
            string[] debits = s.Split('+');
            double i = 0;
            foreach (string debit in debits)
            {
                i = i + Convert.ToDouble(debit);

            }
            txtCredit.Text = i.ToString("0.00");

        }
        protected void txtAmt_TextChaged(object sender, EventArgs e)
        {
            // total calulation part

            int curRow = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtAmt = (TextBox)gvLiability.Rows[curRow].FindControl("txtAmt");

            string s = txtAmt.Text;
            // s=s.Replace("-", "+");
            string[] debits = s.Split('+');
            double i = 0;
            foreach (string debit in debits)
            {
                if (debit != "")
                    i = i + Convert.ToDouble(debit);
            }
            txtAmt.Text = i.ToString("0.00");


            //string s1 = txtAmt.Text;
            //string[] debits1 = s.Split('-');
            //double m = 0;
            //foreach (string debit in debits)
            //{
            //    if (debit != "")
            //        m = m - Convert.ToDouble(debit);
            //}
            //txtAmt.Text = i.ToString("0.00");


            // modify total  liability section

            int a = gvLiability.Rows.Count;
            double liabilty = 0;
            for (int x = 0; x < a; x++)
            {
                TextBox txt = (TextBox)gvLiability.Rows[x].FindControl("txtAmt");
                string minus = txt.ToString().Substring(0, 1);
                Label id = (Label)gvLiability.Rows[x].FindControl("lblParticularsId");
                int p = Convert.ToInt32(id.Text);

                if (p == 5 || p == 6)
                {
                    if (txt.Text != "")
                        liabilty = liabilty - Convert.ToDouble(txt.Text);
                }
                else
                {
                    if (txt.Text != "")
                        liabilty = liabilty + Convert.ToDouble(txt.Text);
                }
            }
            txtTotLiability.Text = liabilty.ToString("0.00");
            GridViewRow myRow = ((Control)sender).Parent.Parent as GridViewRow;
            myRow.FindControl("txtAmt").Focus();
        }
        protected void txtAssetAmt_TextChaged(object sender, EventArgs e)
        {
            // total calulation part

            int curRow = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtAssetAmt = (TextBox)gvAsset.Rows[curRow].FindControl("txtAssetAmt");

            string s = txtAssetAmt.Text;
            string[] debits = s.Split('+');
            double i = 0;
            foreach (string debit in debits)
            {
                if (debit != "")
                    i = i + Convert.ToDouble(debit);

            }
            txtAssetAmt.Text = i.ToString("0.00");



            // modify total asset section

            int a = gvAsset.Rows.Count;
            double asset = 0;
            for (int x = 0; x < a; x++)
            {
                TextBox txt = (TextBox)gvAsset.Rows[x].FindControl("txtAssetAmt");
                Label id = (Label)gvAsset.Rows[x].FindControl("lblParticularsId");
                int p = Convert.ToInt32(id.Text);

                if (txt.Text == "")
                    asset = asset + 0;
                else
                    asset = asset + Convert.ToDouble(txt.Text);

            }
            txtTotAsset.Text = asset.ToString("0.00");

            GridViewRow myRow = ((Control)sender).Parent.Parent as GridViewRow;
            myRow.FindControl("txtAssetAmt").Focus();
        }
        protected void txtIncome_TextChaged(object sender, EventArgs e)
        {
            // total calulation part

            int curRow = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtIncome = (TextBox)gvIncome.Rows[curRow].FindControl("txtIncome");

            string s = txtIncome.Text;
            string[] debits = s.Split('+');
            double i = 0;
            foreach (string debit in debits)
            {
                if (debit != "")
                    i = i + Convert.ToDouble(debit);

            }
            txtIncome.Text = i.ToString("0.00");

            // modify total income section
            int a = gvIncome.Rows.Count;

            double income = 0;

            for (int x = 0; x < a; x++)
            {
                TextBox txt = (TextBox)gvIncome.Rows[x].FindControl("txtIncome");
                Label id = (Label)gvIncome.Rows[x].FindControl("lblParticularsIdPL");
                int p = Convert.ToInt32(id.Text);
                if (p == 8)
                {
                    if (txt.Text != "")
                        income = income - Convert.ToDouble(txt.Text);
                }
                else
                {
                    if (txt.Text == "")
                        income = income + 0;
                    else
                        income = income + Convert.ToDouble(txt.Text);
                }
            }
            txtTotIncome.Text = income.ToString("0.00");
            double expense = 0;
            if (txtTotExpense.Text != "")
                expense = Convert.ToDouble(txtTotExpense.Text);
            double netprofit = 0;
            netprofit = (income - expense);
            txtNetProfit.Text = netprofit.ToString("0.00");
            GridViewRow myRow = ((Control)sender).Parent.Parent as GridViewRow;
            myRow.FindControl("txtIncome").Focus();
        }
        protected void txtExpense_TextChaged(object sender, EventArgs e)
        {
            // total calulation part

            int curRow = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtExpense = (TextBox)gvExpense.Rows[curRow].FindControl("txtExpense");

            string s = txtExpense.Text;
            string[] debits = s.Split('+');
            double i = 0;
            foreach (string debit in debits)
            {
                if (debit != "")
                    i = i + Convert.ToDouble(debit);

            }
            txtExpense.Text = i.ToString("0.00");

            // calcualtion of total expense
            int a = gvExpense.Rows.Count;
            double expense = 0;
            for (int x = 0; x < a; x++)
            {
                TextBox txt = (TextBox)gvExpense.Rows[x].FindControl("txtExpense");
                Label id = (Label)gvExpense.Rows[x].FindControl("lblParticularsIdPL");
                int p = Convert.ToInt32(id.Text);
                if (txt.Text == "")
                    expense = expense + 0;
                else
                    expense = expense + Convert.ToDouble(txt.Text);
            }
            double income = 0;
            income = Convert.ToDouble(txtTotIncome.Text);
            txtTotExpense.Text = expense.ToString("0.00");
            double netprofit = 0;
            netprofit = (income - expense);
            txtNetProfit.Text = netprofit.ToString("0.00");
            GridViewRow myRow = ((Control)sender).Parent.Parent as GridViewRow;
            myRow.FindControl("txtExpense").Focus();
        }
        protected void ImDel1_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                DataRow dr = null;
                DataTable dtXml = new DataTable();
                dtXml.Columns.Add(new DataColumn("SlNo"));
                dtXml.Columns.Add(new DataColumn("AccDate"));
                dtXml.Columns.Add(new DataColumn("CalDebit"));
                dtXml.Columns.Add(new DataColumn("Debit"));
                dtXml.Columns.Add(new DataColumn("Transfer"));
                dtXml.Columns.Add(new DataColumn("LoanAvailed"));
                dtXml.Columns.Add(new DataColumn("CalCredit"));
                dtXml.Columns.Add(new DataColumn("Credit"));
                dtXml.Columns.Add(new DataColumn("SalesFig"));
                dtXml.Columns.Add(new DataColumn("CCLimit"));
                dtXml.Columns.Add(new DataColumn("Day5"));
                dtXml.Columns.Add(new DataColumn("Day10"));
                dtXml.Columns.Add(new DataColumn("Day15"));
                dtXml.Columns.Add(new DataColumn("Day20"));
                dtXml.Columns.Add(new DataColumn("Day25"));
                foreach (GridViewRow gr in gvBankDtl.Rows)
                {
                    if (((TextBox)gr.FindControl("txtDate")).Text != "")
                    {
                        dr = dtXml.NewRow();
                        dr["SlNo"] = ((Label)gr.FindControl("lblSlNo")).Text;

                        if (((TextBox)gr.FindControl("txtDate")).Text != "")
                            dr["AccDate"] = ((TextBox)gr.FindControl("txtDate")).Text;
                        else
                        {
                            dr["AccDate"] = DBNull.Value;
                        }
                        dr["CalDebit"] = ((TextBox)gr.FindControl("txtCalDebit")).Text;
                        if (((TextBox)gr.FindControl("txtDebit")).Text != "")
                            dr["Debit"] = ((TextBox)gr.FindControl("txtDebit")).Text;
                        else
                            dr["Debit"] = 0;
                        if (((TextBox)gr.FindControl("txtTransfer")).Text != "")
                            dr["Transfer"] = ((TextBox)gr.FindControl("txtTransfer")).Text;
                        else
                            dr["Transfer"] = 0;
                        if (((TextBox)gr.FindControl("txtLoanAvailed")).Text != "")
                            dr["LoanAvailed"] = ((TextBox)gr.FindControl("txtLoanAvailed")).Text;
                        else
                            dr["LoanAvailed"] = 0;
                        dr["CalCredit"] = ((TextBox)gr.FindControl("txtCalCredit")).Text;
                        if (((TextBox)gr.FindControl("txtCredit")).Text != "")
                            dr["Credit"] = ((TextBox)gr.FindControl("txtCredit")).Text;
                        else
                            dr["Credit"] = 0;
                        if (((TextBox)gr.FindControl("txtSalesFig")).Text != "")
                            dr["SalesFig"] = ((TextBox)gr.FindControl("txtSalesFig")).Text;
                        else
                            dr["SalesFig"] = 0;
                        if (((TextBox)gr.FindControl("txtCCLimit")).Text != "")
                            dr["CCLimit"] = ((TextBox)gr.FindControl("txtCCLimit")).Text;
                        else
                            dr["CCLimit"] = 0;
                        if (((TextBox)gr.FindControl("txtDay5")).Text != "")
                            dr["Day5"] = ((TextBox)gr.FindControl("txtDay5")).Text;
                        else
                            dr["Day5"] = 0;
                        if (((TextBox)gr.FindControl("txtDay10")).Text != "")
                            dr["Day10"] = ((TextBox)gr.FindControl("txtDay10")).Text;
                        else
                            dr["Day10"] = 0;
                        if (((TextBox)gr.FindControl("txtDay15")).Text != "")
                            dr["Day15"] = ((TextBox)gr.FindControl("txtDay15")).Text;
                        else
                            dr["Day15"] = 0;
                        if (((TextBox)gr.FindControl("txtDay20")).Text != "")
                            dr["Day20"] = ((TextBox)gr.FindControl("txtDay20")).Text;
                        else
                            dr["Day20"] = 0;
                        if (((TextBox)gr.FindControl("txtDay25")).Text != "")
                            dr["Day25"] = ((TextBox)gr.FindControl("txtDay25")).Text;
                        else
                            dr["Day25"] = 0;

                        dtXml.Rows.Add(dr);
                        dtXml.AcceptChanges();
                    }
                }
                ViewState["BankDtl"] = dtXml;
                dt = (DataTable)ViewState["BankDtl"];

                if (dt.Rows.Count > 1)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["BankDtl"] = dt;
                    gvBankDtl.DataSource = dt;
                    gvBankDtl.DataBind();
                }
                else
                {
                    popBankDetails();
                    gblFuction.MsgPopup("First Row can not be deleted.");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }
        protected void ImDelIWOW_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                DataRow dr1 = null;
                DataTable dtXml1 = new DataTable();
                dtXml1.Columns.Add(new DataColumn("SlNo"));
                dtXml1.Columns.Add(new DataColumn("ChqDate"));
                dtXml1.Columns.Add(new DataColumn("IWReturn"));
                dtXml1.Columns.Add(new DataColumn("OWReturn"));
                dtXml1.Columns.Add(new DataColumn("EMIReturn"));
                dtXml1.Columns.Add(new DataColumn("ChqClearDate"));
                dtXml1.Columns.Add(new DataColumn("ChqNo"));
                dtXml1.Columns.Add(new DataColumn("Reason"));

                foreach (GridViewRow gr in gvIWOWDtl.Rows)
                {
                    //if (((TextBox)gr.FindControl("txtChqDate")).Text != "")
                    //{
                    dr1 = dtXml1.NewRow();
                    dr1["SlNo"] = ((Label)gr.FindControl("lblSlNo")).Text;
                    if (((TextBox)gr.FindControl("txtChqDate")).Text != "")
                        dr1["ChqDate"] = ((TextBox)gr.FindControl("txtChqDate")).Text;
                    else
                    {
                        dr1["ChqDate"] = DBNull.Value;
                    }
                    if (((TextBox)gr.FindControl("txtIWReturn")).Text != "")
                        dr1["IWReturn"] = ((TextBox)gr.FindControl("txtIWReturn")).Text;
                    else
                        dr1["IWReturn"] = 0;
                    if (((TextBox)gr.FindControl("txtOWReturn")).Text != "")
                        dr1["OWReturn"] = ((TextBox)gr.FindControl("txtOWReturn")).Text;
                    else
                        dr1["OWReturn"] = 0;

                    if (((TextBox)gr.FindControl("txtEMIReturn")).Text != "")
                        dr1["EMIReturn"] = ((TextBox)gr.FindControl("txtEMIReturn")).Text;
                    else
                        dr1["EMIReturn"] = 0;

                    if (((TextBox)gr.FindControl("txtClearedDt")).Text != "")
                        dr1["ChqClearDate"] = ((TextBox)gr.FindControl("txtClearedDt")).Text;
                    else
                        dr1["ChqClearDate"] = DBNull.Value;
                    if (((TextBox)gr.FindControl("txtChequeNo")).Text != "")
                        dr1["ChqNo"] = ((TextBox)gr.FindControl("txtChequeNo")).Text;
                    else
                        dr1["ChqNo"] = "";
                    if (((TextBox)gr.FindControl("txtReason")).Text != "")
                        dr1["Reason"] = ((TextBox)gr.FindControl("txtReason")).Text;
                    else
                        dr1["Reason"] = "";
                    dtXml1.Rows.Add(dr1);
                    dtXml1.AcceptChanges();
                    // }
                }
                ViewState["BankIWOWDtl"] = dtXml1;
                dt = (DataTable)ViewState["BankIWOWDtl"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["BankIWOWDtl"] = dt;
                    gvIWOWDtl.DataSource = dt;
                    gvIWOWDtl.DataBind();
                }
                else
                {
                    popBankIWOWDetails();
                    gblFuction.MsgPopup("First Row can not be deleted.");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }
        protected void ImDelITR_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["ITRDtl"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["ITRDtl"] = dt;
                    gvITR.DataSource = dt;
                    gvITR.DataBind();
                }
                else
                {
                    popITRDetails();
                    gblFuction.MsgPopup("First Row can not be deleted.");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }
        protected void ImDelAsset_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["MLAsset"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["MLAsset"] = dt;
                    gvMLAsset.DataSource = dt;
                    gvMLAsset.DataBind();
                }
                else
                {
                    popMLAsset();
                    gblFuction.MsgPopup("First Row can not be deleted.");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }
        protected void ImDelRef_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["RefDtl"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["RefDtl"] = dt;
                    gvRef.DataSource = dt;
                    gvRef.DataBind();
                }
                else
                {
                    gblFuction.MsgPopup("First Row can not be deleted.");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }
        protected void ImDelCB_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["CBDtl"];
                if (dt.Rows.Count > 0)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["CBDtl"] = dt;
                    gvCBDtl.DataSource = dt;
                    gvCBDtl.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }
        protected void ImDelPL_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["PLDtl"];
                if (dt.Rows.Count > 0)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["PLDtl"] = dt;
                    gvIncome.DataSource = dt;
                    gvIncome.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }
        private void LoadCBList(string pApplicationId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetCBList(pApplicationId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvCB.DataSource = dt1;
                        gvCB.DataBind();
                    }
                    else
                    {
                        gvCB.DataSource = null;
                        gvCB.DataBind();
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
                oMem = null;
            }
        }
        private void LoadCBOnlineList(string pApplicationId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetCBOnlineList(pApplicationId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvOnlineCBList.DataSource = dt1;
                        gvOnlineCBList.DataBind();
                    }
                    else
                    {
                        gvOnlineCBList.DataSource = null;
                        gvOnlineCBList.DataBind();
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
                oMem = null;
            }
        }
        private void LoadBankAcList(string pApplicationId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetBankACList(pApplicationId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvBankAcList.DataSource = dt1;
                        gvBankAcList.DataBind();
                    }
                    else
                    {
                        gvBankAcList.DataSource = null;
                        gvBankAcList.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ds = null;
                dt1 = null;
                oMem = null;
            }

        }
        private void LoadBankAcSummary(string pApplicationId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetBankACSummary(pApplicationId);

                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            for (int j = 0; j < dt1.Columns.Count; j++)
                            {
                                if (string.IsNullOrEmpty(dt1.Rows[i][j].ToString()))
                                {
                                    dt1.Rows[i][j] = "0";
                                }
                            }
                        }
                        Session["AccountsDt"] = dt1;
                        gvBankAcSummary.DataSource = dt1;
                        gvBankAcSummary.DataBind();
                    }
                    else
                    {
                        gvBankAcSummary.DataSource = null;
                        gvBankAcSummary.DataBind();
                    }
                }
                else
                {
                    gvBankAcSummary.DataSource = null;
                    gvBankAcSummary.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ds = null;
                dt1 = null;
                oMem = null;
            }

        }
        private void LoadBSList(string pApplicationId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetBalanceSheetList(pApplicationId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gvBSList.DataSource = dt1;
                        gvBSList.DataBind();
                    }
                    else
                    {
                        gvBSList.DataSource = null;
                        gvBSList.DataBind();
                    }
                }
                else
                {
                    gvBSList.DataSource = null;
                    gvBSList.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ds = null;
                dt1 = null;
                oMem = null;
            }

        }
        private void LoadFinStatDoc(string pApplicationId)
        {
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            try
            {
                dt = oMem.GetFinStatDocUp(pApplicationId);
                if (dt.Rows.Count > 0)
                {
                    gvBSPLDoc.DataSource = dt;
                    gvBSPLDoc.DataBind();
                }
                else
                {
                    gvBSPLDoc.DataSource = null;
                    gvBSPLDoc.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        private void LoadITRFile(string pApplicationId)
        {
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            try
            {
                dt = oMem.GetITRFile(pApplicationId);
                if (dt.Rows.Count > 0)
                {
                    gvViewITR.DataSource = dt;
                    gvViewITR.DataBind();
                    ViewState["ITRDtl"] = dt;
                }
                else
                {
                    gvViewITR.DataSource = null;
                    gvViewITR.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        private void LoadPLList(string pApplicationId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetPLList(pApplicationId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvPLList.DataSource = dt1;
                        gvPLList.DataBind();
                    }
                    else
                    {
                        gvPLList.DataSource = null;
                        gvPLList.DataBind();
                    }
                }
                else
                {
                    gvPLList.DataSource = null;
                    gvPLList.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ds = null;
                dt1 = null;
                oMem = null;
            }

        }
        private void LoadRatioData(string pApplicationId)
        {
            GetRatio(pApplicationId);
        }
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    //btnAppAdd.Enabled = false;
                    //btnAppEdit.Enabled = false;
                    btnExit.Enabled = false;
                    // ClearControls();
                    break;
                case "Show":
                    //btnAppAdd.Enabled = false;
                    //btnAppEdit.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    //btnAppAdd.Enabled = false;
                    //btnAppEdit.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    //btnAppAdd.Enabled = true;
                    //btnAppEdit.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    //btnAppAdd.Enabled = true;
                    //btnAppEdit.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Exit":
                    //btnAppAdd.Visible = false;
                    //btnAppEdit.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadPendingPreCAMList();
        }
        private void ShowAllInitialLoanApp(string vCustId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CApplication ca = new CApplication();
            DataTable dt = new DataTable();
            dt = ca.GetAllInitialLoanApp(vCustId);
            try
            {
                if (dt.Rows.Count > 0)
                {
                    gvLoanApp.DataSource = dt;
                    gvLoanApp.DataBind();
                }
                else
                {
                    gvLoanApp.DataSource = null;
                    gvLoanApp.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ca = null;
                dt = null;
            }

        }
        private void ShowInitialLoanAppData(string pLnAppId, string vBrCode)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CApplication ca = new CApplication();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            try
            {
                ds = ca.GetInitLoanDtlByLoanId(pLnAppId, vBrCode);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    dt2 = ds.Tables[2];

                }
                if (dt.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 1;
                    ViewAcess();
                    hdfApplicationId.Value = pLnAppId;
                    btSaveApplication.Enabled = false;
                    btnUpdateApplication.Enabled = true;
                    ViewState["StateEdit"] = "Edit";
                    txtAppNo.Text = Convert.ToString(dt.Rows[0]["LoanAppNo"]).Trim();
                    txtAppDt.Text = Convert.ToString(dt.Rows[0]["ApplicationDt"]).Trim();
                    PopApplicant();
                    ddlLoanApplicantname.SelectedIndex = ddlLoanApplicantname.Items.IndexOf(ddlLoanApplicantname.Items.FindByValue(Convert.ToString(dt.Rows[0]["CustID"])));
                    PopPurpose();
                    PopLoanType();
                    ddlLnPurpose.SelectedIndex = ddlLnPurpose.Items.IndexOf(ddlLnPurpose.Items.FindByValue(Convert.ToString(dt.Rows[0]["PurposeID"])));
                    ddlLnScheme.SelectedIndex = ddlLnScheme.Items.IndexOf(ddlLnScheme.Items.FindByValue(Convert.ToString(dt.Rows[0]["LoanTypeId"])));
                    EnableMachinDtl(Convert.ToInt32(dt.Rows[0]["LoanTypeId"]));
                    txtAppLnAmt.Text = Convert.ToString(dt.Rows[0]["AppAmount"]).Trim();
                    txtTenure.Text = Convert.ToString(dt.Rows[0]["Tenure"]).Trim();
                    txtLnPurposeDetails.Text = dt.Rows[0]["MachDtl"].ToString();
                    popSourceName();
                    ddlSourceName.SelectedIndex = ddlSourceName.Items.IndexOf(ddlSourceName.Items.FindByValue(Convert.ToString(dt.Rows[0]["SourceID"])));
                    ddlLnAppStatus.SelectedIndex = ddlLnAppStatus.Items.IndexOf(ddlLnAppStatus.Items.FindByValue(Convert.ToString(dt.Rows[0]["PassYN"].ToString())));
                    //if (dt.Rows[0]["PassYN"].ToString() == "Y")
                    //    chkLnAppPass.Checked = true;
                    //else
                    //    chkLnAppPass.Checked = false;
                    txtLnAppPassDt.Text = dt.Rows[0]["PassorRejDate"].ToString();
                    txtLnAppRejReason.Text = dt.Rows[0]["RejReason"].ToString();
                    txtAddTerms.Text = dt.Rows[0]["AddTerms"].ToString();
                }
                if (dt1.Rows.Count > 0)
                {
                    gvCoAppDtl.DataSource = dt1;
                    gvCoAppDtl.DataBind();
                }
                else
                {
                    gvCoAppDtl.DataSource = null;
                    gvCoAppDtl.DataBind();
                }
                if (dt2.Rows.Count > 0)
                {
                    ViewState["MLAsset"] = dt2;
                    gvMLAsset.DataSource = dt2;
                    gvMLAsset.DataBind();
                }
                else
                {
                    popMLAsset();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ca = null;
                dt = null;

            }

        }
       
        private void GetCombineMOS(string LnAppId)
        {
            CMember Omem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = Omem.GetCombineMOS(LnAppId);
                if (dt.Rows.Count > 0)
                {
                    lblCombMOS.Text = dt.Rows[0]["CombMOS"].ToString();
                }
                else
                {
                    lblCombMOS.Text = "0";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Omem = null;
                dt = null;
            }
        }
        private void GetBankToTR(string LnAppId)
        {
            CMember Omem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = Omem.GetBankToTR(LnAppId);
                if (dt.Rows.Count > 0)
                {
                    lblBankingToTR.Text = dt.Rows[0]["BankingtoTR"].ToString();
                }
                else
                {
                    lblBankingToTR.Text = "0";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Omem = null;
                dt = null;
            }
        }
        private void ShowCBDetails(string pCBId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CMember ca = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = ca.GetCBDtlbyCBid(Convert.ToInt32(pCBId));
                if (dt.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 4;
                    ViewAcess();
                    hdfCBId.Value = Convert.ToString(dt.Rows[0]["CBId"]).Trim();
                    btnSaveCB.Enabled = false;
                    btnUpdateCB.Enabled = true;
                    btnDeleteCB.Enabled = true;
                    ViewState["StateEdit"] = "Edit";
                    txtLnAppCB.Text = Convert.ToString(dt.Rows[0]["ApplicationId"]).Trim();
                    gvCBDtl.DataSource = dt;
                    gvCBDtl.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ca = null;
                dt = null;
            }
        }
        private void ShowCBDetailsByAppId(string LnAppId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CMember ca = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = ca.GetCBDtlbyAppId(LnAppId);
                if (dt.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 2;
                    ViewAcess();
                    hdfCBId.Value = Convert.ToString(dt.Rows[0]["CBId"]).Trim();
                    btnSaveCB.Enabled = false;
                    btnUpdateCB.Enabled = true;
                    btnDeleteCB.Enabled = true;
                    ViewState["StateEdit"] = "Edit";
                    txtLnAppCB.Text = Convert.ToString(dt.Rows[0]["ApplicationId"]).Trim();
                    gvCBDtl.DataSource = dt;
                    gvCBDtl.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ca = null;
                dt = null;
            }
        }
        private void ShowBankAcDetails(string pBankAcId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CMember Omem = new CMember();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                ds = Omem.GetBankAcDtlbyAcId(Convert.ToInt32(pBankAcId));
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[2];
                dt4 = ds.Tables[3];
                if (dt1.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 3;
                    ViewAcess();
                    hfBankAcId.Value = Convert.ToString(dt1.Rows[0]["BankAccId"]).Trim();
                    btnBankAcSave.Enabled = false;
                    btnBankAcUpdate.Enabled = true;
                    btnDeleteBankAc.Enabled = true;
                    ViewState["StateEdit"] = "Edit";
                    txtAppIdBankAc.Text = Convert.ToString(dt1.Rows[0]["ApplicationId"]).Trim();
                    txtAcHolName.Text = Convert.ToString(dt1.Rows[0]["AcHolName"]).Trim();
                    txtBankAcSrNo.Text = Convert.ToString(dt1.Rows[0]["SerialNo"]).Trim();
                    txtBankName.Text = Convert.ToString(dt1.Rows[0]["BankName"]).Trim();
                    ddlACType.SelectedValue = Convert.ToString(dt1.Rows[0]["AccType"]).Trim();
                    txtAccNo.Text = Convert.ToString(dt1.Rows[0]["AccNo"]).Trim();
                    txtBankSinYr.Text = Convert.ToString(dt1.Rows[0]["BankingSinceYr"]).Trim();
                    txtBankAcRemarks.Text = Convert.ToString(dt1.Rows[0]["Remarks"]).Trim();
                    txtTransLimit.Text = Convert.ToString(dt1.Rows[0]["TransLimit"]).Trim();
                    txtCurrBalance.Text = Convert.ToString(dt1.Rows[0]["CurrntBalance"]).Trim();
                    if (Convert.ToString(dt1.Rows[0]["ShowinCombMOS"]) == "Y")
                        chkCombMOS.Checked = true;
                    else
                        chkCombMOS.Checked = false;
                    if (Convert.ToString(dt1.Rows[0]["SowInAgreement"]) == "Y")
                        chkShowAgrmnt.Checked = true;
                    else
                        chkShowAgrmnt.Checked = false;
                }
                if (dt2.Rows.Count > 0)
                {
                    ViewState["BankDtl"] = dt2;
                    gvBankDtl.DataSource = dt2;
                    gvBankDtl.DataBind();
                }
                else
                {
                    gvBankDtl.DataSource = null;
                    gvBankDtl.DataBind();
                    popBankDetails();

                }
                if (dt3.Rows.Count > 0)
                {
                    hfBankStatDocId.Value = Convert.ToString(dt3.Rows[0]["DocUpID"]).Trim();
                }
                if (dt4.Rows.Count > 0)
                {
                    ViewState["BankIWOWDtl"] = dt4;
                    gvIWOWDtl.DataSource = dt4;
                    gvIWOWDtl.DataBind();
                }
                else
                {
                    gvIWOWDtl.DataSource = null;
                    gvIWOWDtl.DataBind();
                    popBankIWOWDetails();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Omem = null;
                ds = null;
                dt1 = null;
                dt2 = null;
                dt3 = null;
            }
        }
        private void AccountDetails(string pBankAcId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CMember Omem = new CMember();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                ds = Omem.GetBankAcDtlbyAcId(Convert.ToInt32(pBankAcId));
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[3];
                if (dt2.Rows.Count > 0)
                {
                    gvAcDtl.DataSource = dt2;
                    gvAcDtl.DataBind();
                }
                else
                {
                    gvAcDtl.DataSource = null;
                    gvAcDtl.DataBind();
                }
                if (dt3.Rows.Count > 0)
                {
                    gvChqReturn.DataSource = dt3;
                    gvChqReturn.DataBind();
                }
                else
                {
                    gvChqReturn.DataSource = null;
                    gvChqReturn.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Omem = null;
                ds = null;
                dt1 = null;
                dt2 = null;
                dt3 = null;
            }
        }
        private void ShowBankStatDiffbyAcId(string pBankAcId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CMember Omem = new CMember();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                ds = Omem.GetBankStatDiffbyAcId(Convert.ToInt32(pBankAcId));
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvBankMOS.DataSource = dt1;
                        gvBankMOS.DataBind();
                    }
                    else
                    {
                        gvBankMOS.DataSource = dt1;
                        gvBankMOS.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Omem = null;
                ds = null;
                dt1 = null;
                dt2 = null;
            }
        }
        private void ShowMOSbyAcId(string pBankAcId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CMember Omem = new CMember();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                ds = Omem.GetMOSAcId(Convert.ToInt32(pBankAcId));
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    if (dt2.Rows.Count > 0)
                    {
                        ViewState["Records"] = dt2.Rows[0]["Records"].ToString();
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        gvBankMOS.DataSource = dt1;
                        gvBankMOS.DataBind();
                    }
                    else
                    {
                        gvBankMOS.DataSource = null;
                        gvBankMOS.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Omem = null;
                ds = null;
                dt1 = null;
                dt2 = null;
            }
        }
        private void ShowRefDetails(string pRefId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CMember Omem = new CMember();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                ds = Omem.GetRefDtlRefId(Convert.ToInt32(pRefId));
                dt1 = ds.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 8;
                    ViewAcess();
                    hfRefId.Value = Convert.ToString(dt1.Rows[0]["RefId"]).Trim();
                    btnSaveRef.Enabled = false;
                    btnUpdateRef.Enabled = true;
                    //btnDelRef.Enabled = true;
                    txtAppNoRef.Text = Convert.ToString(dt1.Rows[0]["ApplicationId"]).Trim();
                    txtRefName.Text = Convert.ToString(dt1.Rows[0]["RefName"]).Trim();
                    txtRefAddr.Text = Convert.ToString(dt1.Rows[0]["RefAddress"]).Trim();
                    txtRefMNo.Text = Convert.ToString(dt1.Rows[0]["RefMob"]).Trim();
                    PopRelation();
                    ddlRelWApp.SelectedIndex = ddlRelWApp.Items.IndexOf(ddlRelWApp.Items.FindByValue(Convert.ToString(dt1.Rows[0]["RelWithApplicant"])));

                    txtOffTelNoRef.Text = Convert.ToString(dt1.Rows[0]["OffTelNo"]).Trim();
                    txtCompNameRef.Text = Convert.ToString(dt1.Rows[0]["CompanyName"]).Trim();
                    txtStatus.Text = Convert.ToString(dt1.Rows[0]["Status"]).Trim();
                    txtVarifiedby.Text = Convert.ToString(dt1.Rows[0]["VarifiedBy"]);
                    if (dt1.Rows[0]["PersonalRef"].ToString() == "Y")
                    {
                        rblRefType.SelectedValue = "P";
                    }
                    else if (dt1.Rows[0]["TradeRef"].ToString() == "Y")
                    {
                        rblRefType.SelectedValue = "P";
                    }
                    else
                    {
                        rblRefType.SelectedIndex = -1;
                    }
                    gvRef.DataSource = null;
                    gvRef.DataBind();
                    ViewState["RefDtl"] = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Omem = null;
                ds = null;
                dt1 = null;
            }
        }
        private void LoadPendingPreCAMList()
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetPendingPreCAMList(txtSearch.Text.Trim(), vBrCode);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvLoanApp.DataSource = dt1;
                        gvLoanApp.DataBind();
                    }
                    else
                    {
                        gvLoanApp.DataSource = null;
                        gvLoanApp.DataBind();
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
                oMem = null;
            }

        }
        protected void gvBankAcList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBankAcId = "";
            vBankAcId = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvBankAcList.Rows)
                {
                    if ((gr.RowIndex) % 2 == 0)
                    {
                        gr.BackColor = backColor;
                        gr.ForeColor = foreColor;
                    }
                    else
                    {
                        gr.BackColor = System.Drawing.Color.White;
                        gr.ForeColor = foreColor;
                    }
                    gr.Font.Bold = false;
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
                ShowBankAcDetails(vBankAcId);
            }
            else if (e.CommandName == "cmdShowMOS")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShowDiff = (LinkButton)gvRow.FindControl("btnShowDiff");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvBankAcList.Rows)
                {
                    if ((gr.RowIndex) % 2 == 0)
                    {
                        gr.BackColor = backColor;
                        gr.ForeColor = foreColor;
                    }
                    else
                    {
                        gr.BackColor = System.Drawing.Color.White;
                        gr.ForeColor = foreColor;
                    }
                    gr.Font.Bold = false;
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShowDiff.ForeColor = System.Drawing.Color.White;
                btnShowDiff.Font.Bold = true;
                ShowMOSbyAcId(vBankAcId);

            }
            else if (e.CommandName == "cmdShowDtl")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShowDtl = (LinkButton)gvRow.FindControl("btnShowDtl");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvBankAcList.Rows)
                {
                    if ((gr.RowIndex) % 2 == 0)
                    {
                        gr.BackColor = backColor;
                        gr.ForeColor = foreColor;
                    }
                    else
                    {
                        gr.BackColor = System.Drawing.Color.White;
                        gr.ForeColor = foreColor;
                    }
                    gr.Font.Bold = false;
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShowDtl.ForeColor = System.Drawing.Color.White;
                btnShowDtl.Font.Bold = true;
                AccountDetails(vBankAcId);
            }
            else if (e.CommandName == "cmdDownld")
            {
                GridViewRow gvRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton imgAttachmnt = (ImageButton)gvRow.FindControl("imgDownload");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvBankAcList.Rows)
                {
                    if ((gr.RowIndex) % 2 == 0)
                    {
                        gr.BackColor = backColor;
                        gr.ForeColor = foreColor;
                    }
                    else
                    {
                        gr.BackColor = System.Drawing.Color.White;
                        gr.ForeColor = foreColor;
                    }
                    gr.Font.Bold = false;
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                imgAttachmnt.ForeColor = System.Drawing.Color.White;
                imgAttachmnt.Font.Bold = true;


                DataTable dt = null;
                CDocUpLoad oAD = null;
                string vFileTyp = "";
                string vFileName = "";
                GridViewRow gvR = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton imgDownload = (ImageButton)gvR.FindControl("imgDownload");
                oAD = new CDocUpLoad();
                dt = oAD.GetBankDocUpByBankAcId(Convert.ToInt32(vBankAcId));
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["AttachDoc"].ToString() != "")
                    {
                        byte[] fileData = (byte[])dt.Rows[0]["AttachDoc"];
                        vFileTyp = GetFileType(Convert.ToString(dt.Rows[0]["AttachType"]));
                        vFileName = dt.Rows[0]["AttachDocName"].ToString();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = vFileTyp;
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + vFileName + "" + Convert.ToString(dt.Rows[0]["AttachType"]));
                        BinaryWriter bw = new BinaryWriter(Response.OutputStream);
                        bw.Write(fileData);
                        bw.Close();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("No Attachment Found");
                        return;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("No Attachment Found");
                    return;
                }
            }
        }
        protected void gvCB_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            //string vCBId = "";
            //vCBId = Convert.ToString(e.CommandArgument);

            string vLnAppId = "";
            vLnAppId = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvCB.Rows)
                {
                    if ((gr.RowIndex) % 2 == 0)
                    {
                        gr.BackColor = backColor;
                        gr.ForeColor = foreColor;
                    }
                    else
                    {
                        gr.BackColor = System.Drawing.Color.White;
                        gr.ForeColor = foreColor;
                    }
                    gr.Font.Bold = false;
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
                ShowCBDetailsByAppId(vLnAppId);
                //ShowCBDetails(vCBId);

            }
        }
        protected void ResetCIBILGrid()
        {
            gvCBEnqMstDtl.DataSource = null;
            gvCBEnqMstDtl.DataBind();
            gvCBAddress.DataSource = null;
            gvCBAddress.DataBind();
            gvCBAccounts.DataSource = null;
            gvCBAccounts.DataBind();
            gvCBEnqDtl.DataSource = null;
            gvCBEnqDtl.DataBind();
        }
        protected void gvOnlineCBList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();


            string vReportID = "";
            vReportID = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShowCBOnlineRec")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShowInfo");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvOnlineCBList.Rows)
                {
                    if ((gr.RowIndex) % 2 == 0)
                    {
                        gr.BackColor = backColor;
                        gr.ForeColor = foreColor;
                    }
                    else
                    {
                        gr.BackColor = System.Drawing.Color.White;
                        gr.ForeColor = foreColor;
                    }
                    gr.Font.Bold = false;
                    LinkButton lb = (LinkButton)gr.FindControl("btnShowInfo");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
                if (string.IsNullOrEmpty(e.CommandArgument.ToString()) == true)
                {
                    ResetCIBILGrid();
                    gblFuction.AjxMsgPopup("No CB Record Found...");
                    return;

                }
                GetCBOnlineRecord(vReportID);
            }
            else if (e.CommandName == "cmdExcel")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnDwnldExcel");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvOnlineCBList.Rows)
                {
                    if ((gr.RowIndex) % 2 == 0)
                    {
                        gr.BackColor = backColor;
                        gr.ForeColor = foreColor;
                    }
                    else
                    {
                        gr.BackColor = System.Drawing.Color.White;
                        gr.ForeColor = foreColor;
                    }
                    gr.Font.Bold = false;
                    LinkButton lb = (LinkButton)gr.FindControl("btnDwnldExcel");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
                if (string.IsNullOrEmpty(e.CommandArgument.ToString()) == true)
                {
                    gblFuction.AjxMsgPopup("No CB Record Found...");
                    return;
                }
                DowloadOnlineCBRecord(vReportID);
            }
            else if (e.CommandName == "cmdExport")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnExport");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvOnlineCBList.Rows)
                {
                    if ((gr.RowIndex) % 2 == 0)
                    {
                        gr.BackColor = backColor;
                        gr.ForeColor = foreColor;
                    }
                    else
                    {
                        gr.BackColor = System.Drawing.Color.White;
                        gr.ForeColor = foreColor;
                    }
                    gr.Font.Bold = false;
                    LinkButton lb = (LinkButton)gr.FindControl("btnExport");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
                if (string.IsNullOrEmpty(e.CommandArgument.ToString()) == true)
                {
                    gblFuction.AjxMsgPopup("No CB Record Found...");
                    return;
                }
                ExportCBOnlineRecord(vReportID);
                string vLnAppNo = gvRow.Cells[1].Text.ToString().Trim();
                LoadCBList(vLnAppNo);
            }
        }
        protected void ExportCBOnlineRecord(string pReportId)
        {
            CMember OMem = new CMember();
            Int32 vErr = 0;
            try
            {
                vErr = OMem.ExportCBOnlineRecord(pReportId, Convert.ToInt32(hdUserID.Value), 0);
                if (vErr == 1)
                {
                    gblFuction.AjxMsgPopup("Export to Credit Bureau Successfully Done");
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Exported,Data Error");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        protected void GetCBOnlineRecord(string pReportId)
        {
            DataSet ds = new DataSet();
            DataTable dtApp = new DataTable();
            DataTable dtAdd = new DataTable();
            DataTable dtAccSum = new DataTable();
            DataTable dtEnqSum = new DataTable();
            DataTable dtAcc = new DataTable();
            DataTable dtEnqDtl = new DataTable();

            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetCBOnlineRecord(pReportId);
                if (ds.Tables.Count > 0)
                {
                    dtApp = ds.Tables[0];
                    dtAdd = ds.Tables[1];
                    dtAccSum = ds.Tables[2];
                    dtEnqSum = ds.Tables[3];
                    dtAcc = ds.Tables[4];
                    dtEnqDtl = ds.Tables[5];
                    // Applicant
                    if (dtApp.Rows.Count > 0)
                    {
                        gvCBEnqMstDtl.DataSource = dtApp;
                        gvCBEnqMstDtl.DataBind();
                    }
                    else
                    {
                        gvCBEnqMstDtl.DataSource = null;
                        gvCBEnqMstDtl.DataBind();
                    }
                    // Address
                    if (dtAdd.Rows.Count > 0)
                    {
                        gvCBAddress.DataSource = dtAdd;
                        gvCBAddress.DataBind();
                    }
                    else
                    {
                        gvCBAddress.DataSource = null;
                        gvCBAddress.DataBind();
                    }
                    // Account Summary
                    if (dtAccSum.Rows.Count > 0)
                    {
                        gvAccSummary.DataSource = dtAccSum;
                        gvAccSummary.DataBind();
                    }
                    else
                    {
                        gvAccSummary.DataSource = null;
                        gvAccSummary.DataBind();
                    }
                    // Enquiry Summary
                    if (dtEnqSum.Rows.Count > 0)
                    {
                        gvEnqSummary.DataSource = dtEnqSum;
                        gvEnqSummary.DataBind();
                    }
                    else
                    {
                        gvEnqSummary.DataSource = null;
                        gvEnqSummary.DataBind();
                    }
                    // Account Details
                    if (dtAcc.Rows.Count > 0)
                    {
                        gvCBAccounts.DataSource = dtAcc;
                        gvCBAccounts.DataBind();
                    }
                    else
                    {
                        gvCBAccounts.DataSource = null;
                        gvCBAccounts.DataBind();
                    }
                    // Enquiry Details
                    if (dtEnqDtl.Rows.Count > 0)
                    {
                        gvCBEnqDtl.DataSource = dtEnqDtl;
                        gvCBEnqDtl.DataBind();
                    }
                    else
                    {
                        gvCBEnqDtl.DataSource = null;
                        gvCBEnqDtl.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtApp = null;
                dtAdd = null;
                dtAccSum = null;
                dtEnqSum = null;
                dtAcc = null;
                dtEnqDtl = null;
                oMem = null;
            }

        }
        protected void DowloadOnlineCBRecord(string pReportId)
        {
            DataSet ds = new DataSet();
            DataTable dtApp = new DataTable();
            DataTable dtAdd = new DataTable();
            DataTable dtAccSum = new DataTable();
            DataTable dtEnqSum = new DataTable();
            DataTable dtAcc = new DataTable();
            DataTable dtEnqDtl = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetCBOnlineRecord(pReportId);
                if (ds.Tables.Count > 0)
                {
                    dtApp = ds.Tables[0];
                    dtAdd = ds.Tables[1];
                    dtAccSum = ds.Tables[2];
                    dtEnqSum = ds.Tables[3];
                    dtAcc = ds.Tables[4];
                    dtEnqDtl = ds.Tables[5];


                    string vFileNm = "";
                    System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                    string vTitle = "Online CB Record";
                    vFileNm = "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "_OnlineCBReport.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";

                    HttpContext.Current.Response.Write("<style> .txt" + "\r\n" + "{mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                    Response.Write("<table border='1' cellpadding='0' width='100%'>");
                    Response.Write("<tr><td align=center' colspan='" + dtAcc.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dtAcc.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + " </font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dtAcc.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress2(vBrCode) + " </font></b></td></tr>");
                    //Response.Write("<tr><td align=center' colspan='" + dtAdd.Columns.Count + "'><b><font size='3'> Branch - " + vBrCode + " - " + vBranch + " </font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dtAcc.Columns.Count + "'><b><u><font size='3'>  " + vTitle + "</font></u></b></td></tr>");

                    int TotCol = dtAcc.Columns.Count;


                    // For Applicant
                    Response.Write("<tr>");
                    foreach (DataColumn dtCol in dtApp.Columns)
                    {
                        Response.Write("<td style='text-align: center;color: White;background-color:#0897F6;'><b>" + dtCol.ColumnName + "<b></td>");
                    }
                    Response.Write("<td align=center' colspan='" + (TotCol - dtApp.Columns.Count) + "'></td>");
                    Response.Write("</tr>");
                    if (dtApp.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtApp.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dtApp.Columns.Count; j++)
                            {
                                if (dtApp.Columns[j].ColumnName == "ReportID")
                                {
                                    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtApp.Columns[j].ColumnName == "AadhaarNo")
                                {
                                    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtApp.Columns[j].ColumnName == "TelephoneNumber")
                                {
                                    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtApp.Columns[j].ColumnName == "ScoreValue")
                                {
                                    Response.Write("<td nowrap class='txt'  style='text-align: right;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtApp.Columns[j].ColumnName == "DateOfRequest")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtApp.Columns[j].ColumnName == "CustDOB")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else
                                {
                                    Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                                }
                            }
                            Response.Write("<td align=center' colspan='" + (TotCol - dtApp.Columns.Count) + "'></td>");
                            Response.Write("</tr>");
                        }
                    }
                    Response.Write("<tr></tr>");
                    // For Address
                    Response.Write("<tr>");
                    foreach (DataColumn dtCol in dtAdd.Columns)
                    {
                        Response.Write("<td style='text-align: center;color: White;background-color:#0897F6;'><b>" + dtCol.ColumnName + "<b></td>");
                    }
                    Response.Write("<td align=center' colspan='" + (TotCol - dtAdd.Columns.Count) + "'></td>");
                    Response.Write("</tr>");
                    if (dtAdd.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtAdd.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dtAdd.Columns.Count; j++)
                            {
                                if (dtAdd.Columns[j].ColumnName == "ReportID")
                                {
                                    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else
                                {
                                    Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                                }
                            }
                            Response.Write("<td align=center' colspan='" + (TotCol - dtAdd.Columns.Count) + "'></td>");
                            Response.Write("</tr>");
                        }
                    }
                    Response.Write("<tr></tr>");

                    // For Accounts Summary
                    Response.Write("<tr>");
                    foreach (DataColumn dtCol in dtAccSum.Columns)
                    {
                        Response.Write("<td style='text-align: center;color: White;background-color:#0897F6;'><b>" + dtCol.ColumnName + "<b></td>");
                    }
                    Response.Write("<td align=center' colspan='" + (TotCol - dtAccSum.Columns.Count) + "'></td>");
                    Response.Write("</tr>");
                    if (dtAccSum.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtAccSum.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dtAccSum.Columns.Count; j++)
                            {
                                if (dtAccSum.Columns[j].ColumnName == "Recent Date")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAccSum.Columns[j].ColumnName == "Oldest Date")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAccSum.Columns[j].ColumnName == "Total Accounts")
                                {
                                    Response.Write("<td nowrap style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAccSum.Columns[j].ColumnName == "Over Due Accounts")
                                {
                                    Response.Write("<td nowrap style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAccSum.Columns[j].ColumnName == "Zero Balance Accounts")
                                {
                                    Response.Write("<td nowrap style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else
                                {
                                    Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                                }
                            }
                            Response.Write("<td align=center' colspan='" + (TotCol - dtAccSum.Columns.Count) + "'></td>");
                            Response.Write("</tr>");
                        }

                    }
                    Response.Write("<tr></tr>");

                    // For Enquiry Summary
                    Response.Write("<tr>");
                    foreach (DataColumn dtCol in dtEnqSum.Columns)
                    {
                        Response.Write("<td style='text-align: center;color: White;background-color:#0897F6;'><b>" + dtCol.ColumnName + "<b></td>");
                    }
                    Response.Write("<td align=center' colspan='" + (TotCol - dtEnqSum.Columns.Count) + "'></td>");
                    Response.Write("</tr>");
                    if (dtEnqSum.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtEnqSum.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dtEnqSum.Columns.Count; j++)
                            {
                                if (dtEnqSum.Columns[j].ColumnName == "Recent")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtEnqSum.Columns[j].ColumnName == "Total Enquiry")
                                {
                                    Response.Write("<td nowrap style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtEnqSum.Columns[j].ColumnName == "Past 30 Days")
                                {
                                    Response.Write("<td nowrap style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtEnqSum.Columns[j].ColumnName == "Past 12 Months")
                                {
                                    Response.Write("<td nowrap style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtEnqSum.Columns[j].ColumnName == "Past 24 Months")
                                {
                                    Response.Write("<td nowrap style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else
                                {
                                    Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                                }
                            }
                            Response.Write("<td align=center' colspan='" + (TotCol - dtEnqSum.Columns.Count) + "'></td>");
                            Response.Write("</tr>");
                        }
                    }
                    Response.Write("<tr></tr>");

                    // For Accounts

                    Response.Write("<tr>");
                    foreach (DataColumn dtCol in dtAcc.Columns)
                    {
                        Response.Write("<td style='text-align: center;color: White;background-color:#0897F6;'><b>" + dtCol.ColumnName + "<b></td>");
                    }
                    Response.Write("</tr>");
                    if (dtAcc.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtAcc.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dtAcc.Columns.Count; j++)
                            {
                                if (dtAcc.Columns[j].ColumnName == "ReportID")
                                {
                                    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "PaymentHistory1")
                                {
                                    Response.Write("<td nowrap class='txt' style='width:300px;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "PaymentHistory2")
                                {
                                    Response.Write("<td nowrap class='txt' style='width:300px;' >" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "DateOpenedOrDisbursed")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;' >" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "DateOfLastPayment")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;' >" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "DateClosed")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;' >" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "DateReportedAndCertified")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;' >" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "PaymentHistoryStartDate")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;' >" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "PaymentHistoryEndDate")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;' >" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "Account Type")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "WrittenOffAndSettled")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "Ownership Type")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "Active/Close")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else
                                {
                                    Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                                }
                            }
                            Response.Write("</tr>");
                        }
                    }
                    Response.Write("<tr></tr>");

                    // For Enquiry Details
                    Response.Write("<tr>");
                    foreach (DataColumn dtCol in dtEnqDtl.Columns)
                    {
                        Response.Write("<td style='text-align: center;color: White;background-color:#0897F6;'><b>" + dtCol.ColumnName + "<b></td>");
                    }
                    Response.Write("<td align=center' colspan='" + (TotCol - dtEnqDtl.Columns.Count) + "'></td>");
                    Response.Write("</tr>");
                    if (dtEnqDtl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtEnqDtl.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dtEnqDtl.Columns.Count; j++)
                            {
                                if (dtEnqDtl.Columns[j].ColumnName == "ReportID")
                                {
                                    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtEnqDtl.Columns[j].ColumnName == "Date Of Enquiry")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtEnqDtl.Columns[j].ColumnName == "Enquiry Purpose")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else
                                {
                                    Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                                }
                            }
                            Response.Write("<td align=center' colspan='" + (TotCol - dtEnqDtl.Columns.Count) + "'></td>");
                            Response.Write("</tr>");
                        }
                    }
                    Response.Write("</table>");

                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtApp = null;
                dtAdd = null;
                dtAccSum = null;
                dtEnqSum = null;
                dtAcc = null;
                dtEnqDtl = null;
                oMem = null;
            }
        }

        protected void gvCoAppDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            //string vCBId = "";
            //vCBId = Convert.ToString(e.CommandArgument);

            string vLnAppId = "";
            vLnAppId = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvCoAppDtl.Rows)
                {
                    if ((gr.RowIndex) % 2 == 0)
                    {
                        gr.BackColor = backColor;
                        gr.ForeColor = foreColor;
                    }
                    else
                    {
                        gr.BackColor = System.Drawing.Color.White;
                        gr.ForeColor = foreColor;
                    }
                    gr.Font.Bold = false;
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
            }
            else if (e.CommandName == "cmdShowInfo")
            {
                if (string.IsNullOrEmpty(e.CommandArgument.ToString()) == false)
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int RowIndex = gvRow.RowIndex;
                    string ScoreValue = "";
                    ScoreValue = gvCoAppDtl.Rows[RowIndex].Cells[2].Text.ToString();
                    ScoreValue = ScoreValue.Replace("&nbsp;", "");

                    if (ScoreValue != "")
                    {
                        gblFuction.AjxMsgPopup("Online CB Check Already Done For this Co-Applicant as CB Score Already Showing...");
                        return;
                    }
                    if (txtAppNo.Text == "")
                    {
                        gblFuction.AjxMsgPopup("Please Save Loan Application First, Then Check CIBIL Online");
                        return;
                    }

                    string pLnAppId = "", pCoappId = "";
                    pLnAppId = txtAppNo.Text.ToString();
                    pCoappId = e.CommandArgument.ToString();

                    XmlDocument doc = new XmlDocument();
                    XmlDeclaration xDeclare = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    XmlElement documentRoot = doc.DocumentElement;
                    doc.InsertBefore(xDeclare, documentRoot);
                    XmlElement DCRequest = (XmlElement)doc.AppendChild(doc.CreateElement("DCRequest"));
                    DCRequest.SetAttribute("xmlns", "http://transunion.com/dc/extsvc");
                    XmlElement Authentication = (XmlElement)DCRequest.AppendChild(doc.CreateElement("Authentication"));

                    Authentication.SetAttribute("type", "OnDemand");
                    XmlElement UserId = (XmlElement)Authentication.AppendChild(doc.CreateElement("UserId"));
                    UserId.InnerText = "KUDOS_Dev_User";
                    XmlElement Password = (XmlElement)Authentication.AppendChild(doc.CreateElement("Password"));
                    Password.InnerText = "Password@123";

                    XmlElement RequestInfo = (XmlElement)DCRequest.AppendChild(doc.CreateElement("RequestInfo"));
                    XmlElement ExecutionMode = (XmlElement)RequestInfo.AppendChild(doc.CreateElement("ExecutionMode"));
                    ExecutionMode.InnerText = "NewWithContext";
                    XmlElement SolutionSetId = (XmlElement)RequestInfo.AppendChild(doc.CreateElement("SolutionSetId"));
                    SolutionSetId.InnerText = "2081";
                    XmlElement SolutionSetVersion = (XmlElement)RequestInfo.AppendChild(doc.CreateElement("SolutionSetVersion"));
                    SolutionSetVersion.InnerText = "2";

                    XmlElement Fields = (XmlElement)DCRequest.AppendChild(doc.CreateElement("Fields"));
                    XmlElement Field1 = (XmlElement)Fields.AppendChild(doc.CreateElement("Field"));
                    Field1.SetAttribute("key", "Applicants");

                    string AppLnAmt = Convert.ToInt32(Convert.ToDecimal(txtAppLnAmt.Text)).ToString();
                    DataTable dt = new DataTable();
                    CMember CMem = new CMember();
                    dt = CMem.GetCAXMLRecForCIBIL(e.CommandArgument.ToString());
                    string strField1 = dt.Rows[0][0].ToString();
                    strField1 = strField1.Replace("&amp;", "");
                    XmlCDataSection CData = doc.CreateCDataSection(strField1);
                    Field1.AppendChild(CData);
                    XmlElement Field2 = (XmlElement)Fields.AppendChild(doc.CreateElement("Field"));
                    Field2.SetAttribute("key", "ApplicationData");
                    string strField2 = "<ApplicationData><Purpose>51</Purpose><Amount>" + AppLnAmt + "</Amount><ScoreType>01</ScoreType><GSTStateCode>27</GSTStateCode><MemberCode>NB67098899_C2C</MemberCode><Password>lbW+juakg0</Password><CibilBureauFlag>FALSE</CibilBureauFlag><DSTuNtcFlag>FALSE</DSTuNtcFlag><IDVerificationFlag>FALSE</IDVerificationFlag><MFIBureauFlag>FALSE</MFIBureauFlag><NTCProductType>AL</NTCProductType><ConsumerConsentForUIDAIAuthentication>Y</ConsumerConsentForUIDAIAuthentication><MFIEnquiryAmount>100000</MFIEnquiryAmount><MFILoanPurpose>40</MFILoanPurpose><MFICenterReferenceNo></MFICenterReferenceNo><MFIBranchReferenceNo></MFIBranchReferenceNo><FormattedReport>False</FormattedReport></ApplicationData>";

                    XmlCDataSection CData1 = doc.CreateCDataSection(strField2);
                    Field2.AppendChild(CData1);
                    string strFinalXML = doc.InnerXml;



                    string vRtnXML = string.Empty;
                    AuthSms oAuth = new AuthSms();
                    //vRtnXML = @"{""Result"":true,""Data"":""\u003c?xml version=\""1.0\""?\u003e\u003cDCResponse\u003e\u003cStatus\u003eSuccess\u003c/Status\u003e\u003cAuthentication\u003e\u003cStatus\u003eSuccess\u003c/Status\u003e\u003cToken\u003e81f04e58-8640-4347-b420-39afa32924c4\u003c/Token\u003e\u003c/Authentication\u003e\u003cResponseInfo\u003e\u003cApplicationId\u003e360073862\u003c/ApplicationId\u003e\u003cSolutionSetInstanceId\u003e23d9ba33-0759-4009-a170-b9de501a4cff\u003c/SolutionSetInstanceId\u003e\u003cCurrentQueue\u003e\u003c/CurrentQueue\u003e\u003c/ResponseInfo\u003e\u003cContextData\u003e\u003cField key=\""Applicant\""\u003e\u003cApplicant\u003e\r\n\u003cAccounts\u003e\r\n\u003cAccount\u003e\r\n\u003cAccountNumber\u003e\r\n\u003c/AccountNumber\u003e\r\n\u003c/Account\u003e\r\n\u003c/Accounts\u003e\r\n\u003cMemberOtherId1Type\u003e\r\n\u003c/MemberOtherId1Type\u003e\r\n\u003cMemberOtherId1\u003e\r\n\u003c/MemberOtherId1\u003e\r\n\u003cMemberOtherId2Type\u003e\r\n\u003c/MemberOtherId2Type\u003e\r\n\u003cMemberOtherId2\u003e\r\n\u003c/MemberOtherId2\u003e\r\n\u003cMemberOtherId3Type\u003e\r\n\u003c/MemberOtherId3Type\u003e\r\n\u003cMemberOtherId3\u003e\r\n\u003c/MemberOtherId3\u003e\r\n\u003cKeyPersonName\u003e\r\n\u003c/KeyPersonName\u003e\r\n\u003cKeyPersonRelation\u003e\r\n\u003c/KeyPersonRelation\u003e\r\n\u003cMemberRelationName1\u003e\r\n\u003c/MemberRelationName1\u003e\r\n\u003cMemberRelationType1\u003e\r\n\u003c/MemberRelationType1\u003e\r\n\u003cMemberRelationName2\u003e\r\n\u003c/MemberRelationName2\u003e\r\n\u003cMemberRelationType2\u003e\r\n\u003c/MemberRelationType2\u003e\r\n\u003cMemberRelationName3\u003e\r\n\u003c/MemberRelationName3\u003e\r\n\u003cMemberRelationType3\u003e\r\n\u003c/MemberRelationType3\u003e\r\n\u003cMemberRelationName4\u003e\r\n\u003c/MemberRelationName4\u003e\r\n\u003cMemberRelationType4\u003e\r\n\u003c/MemberRelationType4\u003e\r\n\u003cNomineeName\u003e\r\n\u003c/NomineeName\u003e\r\n\u003cNomineeRelation\u003e\r\n\u003c/NomineeRelation\u003e\r\n\u003cAddresses\u003e\r\n\u003cAddress\u003e\r\n\u003cStateCode\u003e27\u003c/StateCode\u003e\r\n\u003cResidenceType\u003e01\u003c/ResidenceType\u003e\r\n\u003cPinCode\u003e411027\u003c/PinCode\u003e\r\n\u003cCity\u003ePUNE\u003c/City\u003e\r\n\u003cAddressType\u003e01\u003c/AddressType\u003e\r\n\u003cAddressLine5\u003e\r\n\u003c/AddressLine5\u003e\r\n\u003cAddressLine4\u003e\r\n\u003c/AddressLine4\u003e\r\n\u003cAddressLine3\u003eRIYADARSHINI NGR, OLD SANGAVI, PUNE \u003c/AddressLine3\u003e\r\n\u003cAddressLine2\u003eLD, B WING,  SAGER NR NATIONAL SCHOOL, P\u003c/AddressLine2\u003e\r\n\u003cAddressLine1\u003eSR NO 4/1/73, FL B-202, SHREE HARITAGE B\u003c/AddressLine1\u003e\r\n\u003c/Address\u003e\r\n\u003c/Addresses\u003e\r\n\u003cTelephones\u003e\r\n\u003cTelephone\u003e\r\n\u003cTelephoneType\u003e01\u003c/TelephoneType\u003e\r\n\u003cTelephoneNumber\u003e9823413581\u003c/TelephoneNumber\u003e\r\n\u003cTelephoneExtension\u003e\r\n\u003c/TelephoneExtension\u003e\r\n\u003c/Telephone\u003e\r\n\u003c/Telephones\u003e\r\n\u003cIdentifiers\u003e\r\n\u003cIdentifier\u003e\r\n\u003cIdType\u003e01\u003c/IdType\u003e\r\n\u003cIdNumber\u003eBACPP6592D\u003c/IdNumber\u003e\r\n\u003c/Identifier\u003e\r\n\u003cIdentifier\u003e\r\n\u003cIdType\u003e07\u003c/IdType\u003e\r\n\u003cIdNumber\u003e170650081989\u003c/IdNumber\u003e\r\n\u003c/Identifier\u003e\r\n\u003c/Identifiers\u003e\r\n\u003cCompanyName\u003e\r\n\u003c/CompanyName\u003e\r\n\u003cEmailAddress\u003e\r\n\u003c/EmailAddress\u003e\r\n\u003cGender\u003eMALE\u003c/Gender\u003e\r\n\u003cDateOfBirth\u003e17081986\u003c/DateOfBirth\u003e\r\n\u003cApplicantLastName\u003ePATEL\u003c/ApplicantLastName\u003e\r\n\u003cApplicantMiddleName\u003e\r\n\u003c/ApplicantMiddleName\u003e\r\n\u003cApplicantFirstName\u003eKISHOR MEKHJI\u003c/ApplicantFirstName\u003e\r\n\u003cDsCibilBureau\u003e\r\n\u003cDsCibilBureauData\u003e\r\n\u003cRequest\u003e\u003cRequest\u003e\u003cConsumerDetails\u003e\u003cCreditReportInquiry\u003e\u003cHeader\u003e\u003cSegmentTag\u003e TUEF \u003c/SegmentTag\u003e\u003cVersion\u003e 12 \u003c/Version\u003e\u003cReferenceNumber\u003e360073862\u003c/ReferenceNumber\u003e\u003cFutureUse1\u003e\u003c/FutureUse1\u003e\u003cMemberCode\u003eNB67098899_C2C\u003c/MemberCode\u003e\u003cPassword\u003elbW+juakg0\u003c/Password\u003e\u003cPurpose\u003e10\u003c/Purpose\u003e\u003cAmount\u003e100000\u003c/Amount\u003e\u003cFutureUse2\u003e\u003c/FutureUse2\u003e\u003cScoreType\u003e01\u003c/ScoreType\u003e\u003cOutputFormat\u003e01\u003c/OutputFormat\u003e\u003cResponseSize\u003e1\u003c/ResponseSize\u003e\u003cMediaType\u003eCC\u003c/MediaType\u003e\u003cAuthenticationMethod\u003eL\u003c/AuthenticationMethod\u003e\u003c/Header\u003e\u003cNames\u003e\u003cName\u003e \u003cConsumerName1\u003eKISHOR MEKHJI\u003c/ConsumerName1\u003e \u003cConsumerName2\u003e\u003c/ConsumerName2\u003e \u003cConsumerName3\u003ePATEL\u003c/ConsumerName3\u003e \u003cConsumerName4\u003e\u003c/ConsumerName4\u003e \u003cConsumerName5\u003e\u003c/ConsumerName5\u003e \u003cDateOfBirth\u003e17081986\u003c/DateOfBirth\u003e \u003cGender\u003eMALE\u003c/Gender\u003e \u003c/Name\u003e\u003c/Names\u003e\u003cIdentifications\u003e \u003cIdentification\u003e\u003cPanNo\u003eBACPP6592D\u003c/PanNo\u003e\u003cPassportNumber\u003e\u003c/PassportNumber\u003e\u003cDLNo\u003e\u003c/DLNo\u003e\u003cVoterId\u003e\u003c/VoterId\u003e\u003cUId\u003e\u003c/UId\u003e\u003cRationCardNo\u003e\u003c/RationCardNo\u003e\u003cAdditionalID1\u003e170650081989\u003c/AdditionalID1\u003e\u003cAdditionalID2\u003e\u003c/AdditionalID2\u003e\u003c/Identification\u003e\u003c/Identifications\u003e\u003cTelephones\u003e\u003cTelephone\u003e\u003cTelephoneNumber\u003e9823413581\u003c/TelephoneNumber\u003e\u003cTelephoneExtension\u003e\u003c/TelephoneExtension\u003e\u003cTelephoneType\u003e01\u003c/TelephoneType\u003e\u003c/Telephone\u003e\u003c/Telephones\u003e\u003cAddresses\u003e\u003cAddress\u003e\u003cAddressLine1\u003eSR NO 4/1/73, FL B-202, SHREE HARITAGE B\u003c/AddressLine1\u003e\u003cAddressLine2\u003eLD, B WING,  SAGER NR NATIONAL SCHOOL, P\u003c/AddressLine2\u003e\u003cAddressLine3\u003eRIYADARSHINI NGR, OLD SANGAVI, PUNE \u003c/AddressLine3\u003e\u003cAddressLine4\u003e\u003c/AddressLine4\u003e\u003cAddressLine5\u003e\u003c/AddressLine5\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e01\u003c/AddressCategory\u003e\u003cResidenceCode\u003e01\u003c/ResidenceCode\u003e\u003c/Address\u003e\u003c/Addresses\u003e\u003c/CreditReportInquiry\u003e\u003c/ConsumerDetails\u003e\u003c/Request\u003e\u003c/Request\u003e\r\n\u003c/DsCibilBureauData\u003e\r\n\u003cDsCibilBureauStatus\u003e\r\n\u003cTrail\u003e\u003c/Trail\u003e\r\n\u003c/DsCibilBureauStatus\u003e\r\n\u003cResponse\u003e\r\n\u003cCibilBureauResponse\u003e\r\n\u003cBureauResponseRaw\u003eTUEF12360073862                  0000NB67098899_C2C                100263130912228092018170049PN03N010107KISHORE0212MEKHJI PATEL0305PATEL07081708198608012ID03I010102010210BACPP6592DID03I020102060212492403246555PT03T01011102048606018030203PT03T020114211412345 EXT.030200PT03T0301109823413581030203PT03T0401109823413581030201EC03C010119BERABAZAR@GMAIL.COMEC03C020124BERABAZAR@REDIFFMAIL.COMEM03E01010252020831082018030204SC10CIBILTUSCR010201020210030828092018040500819130201250210PA03A010109PUNE PUNE06022707064110270802021008160520189001YPA03A020115AUNDH CAMP PUNE06022707064110270802020902011008190320189001YPA03A030140FLAT NO. B-202 HERITAGE BUILDING OLD SAN0227SANGAMNAGAR PUNE AUNDH CAMP0325PUNE NEAR NATIONAL SCHOOL0602270706411027080201100817102017PA03A040139S NO 4 1 62 CTM 3117 SHREE HERITAGE 2ND0235FLOOR FLAT NO B 202 SANGAM NAGAR NR0338NATIONAL SCHOOL OLD SANGVI PUNE 4110270602270706411027080204100831032015TL04T0010213NOT DISCLOSED040210050110808270620180908060820181108230920181304208414031002809007000000300801092018310801072018440203TL04T0020213NOT DISCLOSED04020505011080810052018090805072018110831072018120620000013061915402809000000000300801072018310801052018390236TL04T0030205KUDOS0310000000056604025105014080828032018090805072018110831072018120632000013063021542815000XXX000000XXX300801072018310801032018380516.00400513156TL04T0040213NOT DISCLOSED04025005014080828032018090830062018110830062018120630000013062731982812STDSTDSTDSTD300801062018310801032018TL04T0050213NOT DISCLOSED040210050110808100120180908290620181108310720181205287661306-50000282100000000000000000000030080107201831080101201836053000037046000450550000TL04T0060213NOT DISCLOSED04020505011080813122017090831072018110831072018120650000013064275402824000XXX000000000000000000300801072018310801122017TL04T0070213NOT DISCLOSED04020105011080824102017090810082018110831082018120684800013067279882833000000000000000000000000000000000300801082018310801102017TL04T0080213NOT DISCLOSED040252050140808070720171108310820181206100000130102821000000000000000000000300801082018310801022018350200380520.003902124004926345044000TL04T0090213NOT DISCLOSED040205050110808270920160908141220171008141220171108311220171206450000130102848000000000000000000000000000000000000000000000000300801122017310801092016TL04T0100205KUDOS03041164040251050140808080620160908010120171008270320171108310720171206300000130102836000XXXXXXXXXXXXXXX000XXXXXX000000000300801072017310801082016TL04T0110213NOT DISCLOSED0402510501108083005201609080508201811083108201812064150001306132429285400000000000000000000000000000000000000000000000000000029300000000000000000000000000000003008010820183108010520163902364005160654402034506417690TL04T0120213NOT DISCLOSED040205050110808310820150908270920161008280920161108300920161206301000130102839000000000000000000XXX000000000000000000300801092016310801092015TL04T0130213NOT DISCLOSED04020305011080806052015090810082018110831082018120670000013066467882854000000000000000000000000000000000000000000000000000000295400000000000000000000000000000000000000000000000000000030080108201831080109201534072328800350201380514.85390317840049797440203TL04T0140213NOT DISCLOSED0402020501108081103201509081005201511083105201512071100000130710955742803000300801052015310801052015TL04T0150213NOT DISCLOSED040202050110808110320150908100820181108310820181207110000013069875602854000000000000000000000000000000000000000000000000000000295400000000000000000000000000000000000000000000000000000030080108201831080109201534072328000350201380511.853903178400513202440203TL04T0160213NOT DISCLOSED040205050110808310720140908310820151008020920151108300920151206225000130102842000000000000000000000XXX000000000000000000300801092015310801082014TL04T0170213NOT DISCLOSED040210050110808300720130908210520151008101220151108311220151205318441302-428540000000000000000000000000850860530860560250260550860562936055026000026023000000XXX00000000000030080112201531080107201336051500044020345045310TL04T0180213NOT DISCLOSED040252050130808200220130908231020171008200220181108300620181206724500130142854000000000000STDXXX030030000000000000XXX000STDXXX0000002954000000000000000000000000XXXXXX000000000000000000000000300801062018310801072015390260400527168440203TL04T0190213NOT DISCLOSED0402050501108082507201209083107201410080208201411083008201412061500001301028540000000000000000000000000000000000000000000000000000002921000000000000000000000300801082014310801092011TL04T0200213NOT DISCLOSED0402020501408081104201209081004201510081004201511083004201512071200000130102854000STDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTD2954STDSTDSTDSTDSTDSTDSTDSTDXXXSTDSTDSTDSTDSTDSTDSTDSTDSTD300801042015310801052012350201400514286440203TL04T0210213NOT DISCLOSED040213050110808311020110908241120141008241120141108301120141205370001301028540000000000000000000000000000000000000000000000000260002954000000000000000000000000000000000000000000000000000000300801112014310801122011TL04T0220213NOT DISCLOSED04021305011080831102011090829062015100829062015110830062015120537000130102854000000000000000148117086056025025000000000000000000000295400000000000000000000000000000000000000000000000000000030080106201531080107201244020345043037TL04T0230213NOT DISCLOSED040210050110808010620110908280820171108020720181205638711305-176528540000000000000000000000000000000000000000000000000000002954000000000000000000000000000000000000000000000000000000300801072018310801082015360568700TL04T0240213NOT DISCLOSED040213050110808210120100908060220131008060220131108280220131205357661301028540000000000000000000000000000000000000000000000000000002954000000000000000000000000000000000000000000000000000000300801022013310801032010IQ04I0010108280920180405KUDOS0502100606100000IQ04I0020108260820180413NOT DISCLOSED050203060813000000IQ04I0030108170820180413NOT DISCLOSED050203060810000000IQ04I0040108020820180413NOT DISCLOSED0502050606200000IQ04I0050108170620180413NOT DISCLOSED05020006041000IQ04I0060108240520180413NOT DISCLOSED0502050606100000IQ04I0070108160520180413NOT DISCLOSED0502050606200000IQ04I0080108100520180413NOT DISCLOSED0502610606500000IQ04I0090108080520180413NOT DISCLOSED0502050606218000IQ04I0100108030420180413NOT DISCLOSED05021006041000IQ04I0110108300320180413NOT DISCLOSED05025106071000000IQ04I0120108290320180413NOT DISCLOSED050206060560000IQ04I0130108280320180413NOT DISCLOSED05025106071000000IQ04I0140108260320180413NOT DISCLOSED0502000606266000IQ04I0150108210320180413NOT DISCLOSED05025106071000000IQ04I0160108210320180413NOT DISCLOSED05020006011IQ04I0170108210320180413NOT DISCLOSED05025206071000000IQ04I0180108200320180405KUDOS0502610606100000IQ04I0190108190320180413NOT DISCLOSED05020006011IQ04I0200108160320180413NOT DISCLOSED05025106071500000IQ04I0210108050220180413NOT DISCLOSED05021006041000IQ04I0220108050120180413NOT DISCLOSED050210060510000IQ04I0230108151120170413NOT DISCLOSED0502050606600000IQ04I0240108141120170413NOT DISCLOSED050210060510000IQ04I0250108131120170413NOT DISCLOSED0502050606100000IQ04I0260108101120170413NOT DISCLOSED050210060550000IQ04I0270108041120170413NOT DISCLOSED05021006041000IQ04I0280108171020170413NOT DISCLOSED0502010606500000IQ04I0290108161020170413NOT DISCLOSED0502010606838000IQ04I0300108290820170413NOT DISCLOSED050210060510000IQ04I0310108120820170413NOT DISCLOSED0502610606500000IQ04I0320108020820170413NOT DISCLOSED05020206071000000IQ04I0330108120520170413NOT DISCLOSED050210060550000IQ04I0340108220420170413NOT DISCLOSED05020006071000000IQ04I0350108220420170413NOT DISCLOSED05025106071500000IQ04I0360108210420170413NOT DISCLOSED05025106071000000IQ04I0370108110420170413NOT DISCLOSED0502510606400000IQ04I0380108210320170405KUDOS0502510606200000IQ04I0390108240920160413NOT DISCLOSED0502050606500000IQ04I0400108090920160413NOT DISCLOSED0502050606500000IQ04I0410108170820160413NOT DISCLOSED050210060550000IQ04I0420108120820160413NOT DISCLOSED0502050606500000IQ04I0430108120820160413NOT DISCLOSED0502050606500000IQ04I0440108240620160413NOT DISCLOSED0502020606500000IQ04I0450108260520160413NOT DISCLOSED0502050606800000IQ04I0460108260520160405KUDOS0502000606500000IQ04I0470108310120160413NOT DISCLOSED0502050606800000IQ04I0480108050820150413NOT DISCLOSED0502050606400000IQ04I0490108050520150413NOT DISCLOSED0502020606700000IQ04I0500108260220150413NOT DISCLOSED050210060510000IQ04I0510108250220150413NOT DISCLOSED05020206072000000IQ04I0520108240720140413NOT DISCLOSED0502050606300000IQ04I0530108100720140413NOT DISCLOSED050206060550000IQ04I0540108250420140413NOT DISCLOSED0502050606300000IQ04I0550108120920130413NOT DISCLOSED0502050606400000IQ04I0560108240820130413NOT DISCLOSED050210060550000IQ04I0570108250720130413NOT DISCLOSED050210060550000IQ04I0580108170720130413NOT DISCLOSED050210060510000IQ04I0590108160720130413NOT DISCLOSED0502050606200000IQ04I0600108060320130413NOT DISCLOSED0502050606500000IQ04I0610108120220130413NOT DISCLOSED0502150606700000IQ04I0620108020120130413NOT DISCLOSED050210060510000IQ04I0630108271120120413NOT DISCLOSED0502510606800000IQ04I0640108240920120413NOT DISCLOSED05020206071400000IQ04I0650108310720120413NOT DISCLOSED0502510606500000IQ04I0660108130720120413NOT DISCLOSED0502050606200000IQ04I0670108100720120413NOT DISCLOSED0502050606200000IQ04I0680108100520120413NOT DISCLOSED0502050606300000IQ04I0690108210420120405KUDOS0502510606200000IQ04I0700108130320120413NOT DISCLOSED0502050606700000IQ04I0710108100320120413NOT DISCLOSED05020206071300000IQ04I0720108120120120413NOT DISCLOSED0502050606500000IQ04I0730108040120120413NOT DISCLOSED0502050606500000IQ04I0740108021120110413NOT DISCLOSED0502510606300000IQ04I0750108221020110413NOT DISCLOSED050213060537000IQ04I0760108221020110413NOT DISCLOSED050213060537000IQ04I0770108040820110413NOT DISCLOSED0502050606500000IQ04I0780108020820110413NOT DISCLOSED0502050606500000IQ04I0790108300720110413NOT DISCLOSED0502050606300000IQ04I0800108260720110413NOT DISCLOSED05021506071000000IQ04I0810108150720110413NOT DISCLOSED050202060510000IQ04I0820108150720110413NOT DISCLOSED0502000606900000IQ04I0830108050720110413NOT DISCLOSED05021006041000IQ04I0840108110520110413NOT DISCLOSED050205060510000IQ04I0850108030520110413NOT DISCLOSED05020306071500000IQ04I0860108290420110413NOT DISCLOSED050210060510000IQ04I0870108260420110413NOT DISCLOSED0502510606300000IQ04I0880108130420110413NOT DISCLOSED05020206072000000IQ04I0890108120420110413NOT DISCLOSED05020206071500000IQ04I0900108111020100413NOT DISCLOSED05020206071600000IQ04I0910108081020100413NOT DISCLOSED05020206071600000IQ04I0920108120120100413NOT DISCLOSED050213060535766IQ04I0930108041220090413NOT DISCLOSED050213060537000ES0700105210102**\u003c/BureauResponseRaw\u003e\r\n\u003cBureauResponseXml\u003e\u003cCreditReport\u003e\u003cHeader\u003e\u003cSegmentTag\u003eTUEF\u003c/SegmentTag\u003e\u003cVersion\u003e12\u003c/Version\u003e\u003cReferenceNumber\u003e360073862\u003c/ReferenceNumber\u003e\u003cMemberCode\u003eNB67098899_C2C                \u003c/MemberCode\u003e\u003cSubjectReturnCode\u003e1\u003c/SubjectReturnCode\u003e\u003cEnquiryControlNumber\u003e002631309122\u003c/EnquiryControlNumber\u003e\u003cDateProcessed\u003e28092018\u003c/DateProcessed\u003e\u003cTimeProcessed\u003e170049\u003c/TimeProcessed\u003e\u003c/Header\u003e\u003cNameSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eN01\u003c/SegmentTag\u003e\u003cConsumerName1FieldLength\u003e07\u003c/ConsumerName1FieldLength\u003e\u003cConsumerName1\u003eKISHORE\u003c/ConsumerName1\u003e\u003cConsumerName2FieldLength\u003e12\u003c/ConsumerName2FieldLength\u003e\u003cConsumerName2\u003eMEKHJI PATEL\u003c/ConsumerName2\u003e\u003cConsumerName3FieldLength\u003e05\u003c/ConsumerName3FieldLength\u003e\u003cConsumerName3\u003ePATEL\u003c/ConsumerName3\u003e\u003cDateOfBirthFieldLength\u003e08\u003c/DateOfBirthFieldLength\u003e\u003cDateOfBirth\u003e17081986\u003c/DateOfBirth\u003e\u003cGenderFieldLength\u003e01\u003c/GenderFieldLength\u003e\u003cGender\u003e2\u003c/Gender\u003e\u003c/NameSegment\u003e\u003cIDSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eI01\u003c/SegmentTag\u003e\u003cIDType\u003e01\u003c/IDType\u003e\u003cIDNumberFieldLength\u003e10\u003c/IDNumberFieldLength\u003e\u003cIDNumber\u003eBACPP6592D\u003c/IDNumber\u003e\u003c/IDSegment\u003e\u003cIDSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eI02\u003c/SegmentTag\u003e\u003cIDType\u003e06\u003c/IDType\u003e\u003cIDNumberFieldLength\u003e12\u003c/IDNumberFieldLength\u003e\u003cIDNumber\u003e492403246555\u003c/IDNumber\u003e\u003c/IDSegment\u003e\u003cTelephoneSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eT01\u003c/SegmentTag\u003e\u003cTelephoneNumberFieldLength\u003e11\u003c/TelephoneNumberFieldLength\u003e\u003cTelephoneNumber\u003e02048606018\u003c/TelephoneNumber\u003e\u003cTelephoneType\u003e03\u003c/TelephoneType\u003e\u003c/TelephoneSegment\u003e\u003cTelephoneSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eT02\u003c/SegmentTag\u003e\u003cTelephoneNumberFieldLength\u003e14\u003c/TelephoneNumberFieldLength\u003e\u003cTelephoneNumber\u003e211412345 EXT.\u003c/TelephoneNumber\u003e\u003cTelephoneType\u003e00\u003c/TelephoneType\u003e\u003c/TelephoneSegment\u003e\u003cTelephoneSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eT03\u003c/SegmentTag\u003e\u003cTelephoneNumberFieldLength\u003e10\u003c/TelephoneNumberFieldLength\u003e\u003cTelephoneNumber\u003e9823413581\u003c/TelephoneNumber\u003e\u003cTelephoneType\u003e03\u003c/TelephoneType\u003e\u003c/TelephoneSegment\u003e\u003cTelephoneSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eT04\u003c/SegmentTag\u003e\u003cTelephoneNumberFieldLength\u003e10\u003c/TelephoneNumberFieldLength\u003e\u003cTelephoneNumber\u003e9823413581\u003c/TelephoneNumber\u003e\u003cTelephoneType\u003e01\u003c/TelephoneType\u003e\u003c/TelephoneSegment\u003e\u003cEmailContactSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eC01\u003c/SegmentTag\u003e\u003cEmailIDFieldLength\u003e19\u003c/EmailIDFieldLength\u003e\u003cEmailID\u003eBERABAZAR@GMAIL.COM\u003c/EmailID\u003e\u003c/EmailContactSegment\u003e\u003cEmailContactSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eC02\u003c/SegmentTag\u003e\u003cEmailIDFieldLength\u003e24\u003c/EmailIDFieldLength\u003e\u003cEmailID\u003eBERABAZAR@REDIFFMAIL.COM\u003c/EmailID\u003e\u003c/EmailContactSegment\u003e\u003cEmploymentSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eE01\u003c/SegmentTag\u003e\u003cAccountType\u003e52\u003c/AccountType\u003e\u003cDateReportedCertified\u003e31082018\u003c/DateReportedCertified\u003e\u003cOccupationCode\u003e04\u003c/OccupationCode\u003e\u003c/EmploymentSegment\u003e\u003cScoreSegment\u003e\u003cLength\u003e10\u003c/Length\u003e\u003cScoreName\u003eCIBILTUSCR\u003c/ScoreName\u003e\u003cScoreCardName\u003e01\u003c/ScoreCardName\u003e\u003cScoreCardVersion\u003e10\u003c/ScoreCardVersion\u003e\u003cScoreDate\u003e28092018\u003c/ScoreDate\u003e\u003cScore\u003e00819\u003c/Score\u003e\u003cExclusionCode9FieldLength\u003e02\u003c/ExclusionCode9FieldLength\u003e\u003cExclusionCode9\u003e01\u003c/ExclusionCode9\u003e\u003cReasonCode1FieldLength\u003e02\u003c/ReasonCode1FieldLength\u003e\u003cReasonCode1\u003e10\u003c/ReasonCode1\u003e\u003c/ScoreSegment\u003e\u003cAddress\u003e\u003cAddressSegmentTag\u003ePA\u003c/AddressSegmentTag\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eA01\u003c/SegmentTag\u003e\u003cAddressLine1FieldLength\u003e09\u003c/AddressLine1FieldLength\u003e\u003cAddressLine1\u003ePUNE PUNE\u003c/AddressLine1\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCodeFieldLength\u003e06\u003c/PinCodeFieldLength\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e02\u003c/AddressCategory\u003e\u003cDateReported\u003e16052018\u003c/DateReported\u003e\u003cEnrichedThroughEnquiry\u003eY\u003c/EnrichedThroughEnquiry\u003e\u003c/Address\u003e\u003cAddress\u003e\u003cAddressSegmentTag\u003ePA\u003c/AddressSegmentTag\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eA02\u003c/SegmentTag\u003e\u003cAddressLine1FieldLength\u003e15\u003c/AddressLine1FieldLength\u003e\u003cAddressLine1\u003eAUNDH CAMP PUNE\u003c/AddressLine1\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCodeFieldLength\u003e06\u003c/PinCodeFieldLength\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e02\u003c/AddressCategory\u003e\u003cResidenceCode\u003e01\u003c/ResidenceCode\u003e\u003cDateReported\u003e19032018\u003c/DateReported\u003e\u003cEnrichedThroughEnquiry\u003eY\u003c/EnrichedThroughEnquiry\u003e\u003c/Address\u003e\u003cAddress\u003e\u003cAddressSegmentTag\u003ePA\u003c/AddressSegmentTag\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eA03\u003c/SegmentTag\u003e\u003cAddressLine1FieldLength\u003e40\u003c/AddressLine1FieldLength\u003e\u003cAddressLine1\u003eFLAT NO. B-202 HERITAGE BUILDING OLD SAN\u003c/AddressLine1\u003e\u003cAddressLine2FieldLength\u003e27\u003c/AddressLine2FieldLength\u003e\u003cAddressLine2\u003eSANGAMNAGAR PUNE AUNDH CAMP\u003c/AddressLine2\u003e\u003cAddressLine3FieldLength\u003e25\u003c/AddressLine3FieldLength\u003e\u003cAddressLine3\u003ePUNE NEAR NATIONAL SCHOOL\u003c/AddressLine3\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCodeFieldLength\u003e06\u003c/PinCodeFieldLength\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e01\u003c/AddressCategory\u003e\u003cDateReported\u003e17102017\u003c/DateReported\u003e\u003c/Address\u003e\u003cAddress\u003e\u003cAddressSegmentTag\u003ePA\u003c/AddressSegmentTag\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eA04\u003c/SegmentTag\u003e\u003cAddressLine1FieldLength\u003e39\u003c/AddressLine1FieldLength\u003e\u003cAddressLine1\u003eS NO 4 1 62 CTM 3117 SHREE HERITAGE 2ND\u003c/AddressLine1\u003e\u003cAddressLine2FieldLength\u003e35\u003c/AddressLine2FieldLength\u003e\u003cAddressLine2\u003eFLOOR FLAT NO B 202 SANGAM NAGAR NR\u003c/AddressLine2\u003e\u003cAddressLine3FieldLength\u003e38\u003c/AddressLine3FieldLength\u003e\u003cAddressLine3\u003eNATIONAL SCHOOL OLD SANGVI PUNE 411027\u003c/AddressLine3\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCodeFieldLength\u003e06\u003c/PinCodeFieldLength\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e04\u003c/AddressCategory\u003e\u003cDateReported\u003e31032015\u003c/DateReported\u003e\u003c/Address\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT001\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e10\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e27062018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e06082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e23092018\u003c/DateReportedAndCertified\u003e\u003cCurrentBalanceFieldLength\u003e04\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e2084\u003c/CurrentBalance\u003e\u003cAmountOverdueFieldLength\u003e03\u003c/AmountOverdueFieldLength\u003e\u003cAmountOverdue\u003e100\u003c/AmountOverdue\u003e\u003cPaymentHistory1FieldLength\u003e09\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e007000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01092018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01072018\u003c/PaymentHistoryEndDate\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT002\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e10052018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e05072018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e200000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e191540\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e09\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01052018\u003c/PaymentHistoryEndDate\u003e\u003cRepaymentTenureFieldLength\u003e02\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e36\u003c/RepaymentTenure\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT003\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e05\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e05\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eKUDOS\u003c/ReportingMemberShortName\u003e\u003cAccountNumberFieldLength\u003e10\u003c/AccountNumberFieldLength\u003e\u003cAccountNumber\u003e0000000566\u003c/AccountNumber\u003e\u003cAccountType\u003e51\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e28032018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e05072018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e320000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e302154\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e15\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000XXX000000XXX\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01032018\u003c/PaymentHistoryEndDate\u003e\u003cRateOfInterestFieldLength\u003e05\u003c/RateOfInterestFieldLength\u003e\u003cRateOfInterest\u003e16.00\u003c/RateOfInterest\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e13156\u003c/EmiAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT004\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e50\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e28032018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e30062018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e30062018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e300000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e273198\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e12\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003eSTDSTDSTDSTD\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01062018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01032018\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT005\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e10\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e10012018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e29062018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e28766\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e-50000\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e21\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01012018\u003c/PaymentHistoryEndDate\u003e\u003cCreditLimitFieldLength\u003e05\u003c/CreditLimitFieldLength\u003e\u003cCreditLimit\u003e30000\u003c/CreditLimit\u003e\u003cCashLimitFieldLength\u003e04\u003c/CashLimitFieldLength\u003e\u003cCashLimit\u003e6000\u003c/CashLimit\u003e\u003cActualPaymentAmountFieldLength\u003e05\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e50000\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT006\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e13122017\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e31072018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e500000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e427540\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e24\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000XXX000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01122017\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT007\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e01\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e24102017\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e848000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e727988\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e33\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01102017\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT008\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e52\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e07072017\u003c/DateOpenedOrDisbursed\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e100000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e21\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01022018\u003c/PaymentHistoryEndDate\u003e\u003cTypeOfCollateralFieldLength\u003e02\u003c/TypeOfCollateralFieldLength\u003e\u003cTypeOfCollateral\u003e00\u003c/TypeOfCollateral\u003e\u003cRateOfInterestFieldLength\u003e05\u003c/RateOfInterestFieldLength\u003e\u003cRateOfInterest\u003e20.00\u003c/RateOfInterest\u003e\u003cRepaymentTenureFieldLength\u003e02\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e12\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e04\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e9263\u003c/EmiAmount\u003e\u003cActualPaymentAmountFieldLength\u003e04\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e4000\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT009\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e27092016\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e14122017\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e14122017\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e31122017\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e450000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e48\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01122017\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092016\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT010\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e05\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e05\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eKUDOS\u003c/ReportingMemberShortName\u003e\u003cAccountNumberFieldLength\u003e04\u003c/AccountNumberFieldLength\u003e\u003cAccountNumber\u003e1164\u003c/AccountNumber\u003e\u003cAccountType\u003e51\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e08062016\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e01012017\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e27032017\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e31072017\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e300000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e36\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000XXXXXXXXXXXXXXX000XXXXXX000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072017\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01082016\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT011\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e51\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e30052016\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e05082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e415000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e132429\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e30\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01052016\u003c/PaymentHistoryEndDate\u003e\u003cRepaymentTenureFieldLength\u003e02\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e36\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e16065\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003cActualPaymentAmountFieldLength\u003e06\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e417690\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT012\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e31082015\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e27092016\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e28092016\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30092016\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e301000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e39\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000XXX000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01092016\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092015\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT013\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e03\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e06052015\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e700000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e646788\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092015\u003c/PaymentHistoryEndDate\u003e\u003cValueOfCollateralFieldLength\u003e07\u003c/ValueOfCollateralFieldLength\u003e\u003cValueOfCollateral\u003e2328800\u003c/ValueOfCollateral\u003e\u003cTypeOfCollateralFieldLength\u003e02\u003c/TypeOfCollateralFieldLength\u003e\u003cTypeOfCollateral\u003e01\u003c/TypeOfCollateral\u003e\u003cRateOfInterestFieldLength\u003e05\u003c/RateOfInterestFieldLength\u003e\u003cRateOfInterest\u003e14.85\u003c/RateOfInterest\u003e\u003cRepaymentTenureFieldLength\u003e03\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e178\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e04\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e9797\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT014\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e02\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e11032015\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10052015\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31052015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e07\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e1100000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e07\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e1095574\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e03\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01052015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01052015\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT015\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e02\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e11032015\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e07\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e1100000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e987560\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092015\u003c/PaymentHistoryEndDate\u003e\u003cValueOfCollateralFieldLength\u003e07\u003c/ValueOfCollateralFieldLength\u003e\u003cValueOfCollateral\u003e2328000\u003c/ValueOfCollateral\u003e\u003cTypeOfCollateralFieldLength\u003e02\u003c/TypeOfCollateralFieldLength\u003e\u003cTypeOfCollateral\u003e01\u003c/TypeOfCollateral\u003e\u003cRateOfInterestFieldLength\u003e05\u003c/RateOfInterestFieldLength\u003e\u003cRateOfInterest\u003e11.85\u003c/RateOfInterest\u003e\u003cRepaymentTenureFieldLength\u003e03\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e178\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e13202\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT016\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e31072014\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e31082015\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e02092015\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30092015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e225000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e42\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000XXX000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01092015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01082014\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT017\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e10\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e30072013\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e21052015\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e10122015\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e31122015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e31844\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e02\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e-4\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000085086053086056025026055086056\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e36\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e055026000026023000000XXX000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01122015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01072013\u003c/PaymentHistoryEndDate\u003e\u003cCreditLimitFieldLength\u003e05\u003c/CreditLimitFieldLength\u003e\u003cCreditLimit\u003e15000\u003c/CreditLimit\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003cActualPaymentAmountFieldLength\u003e04\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e5310\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT018\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e52\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e3\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e20022013\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e23102017\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e20022018\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30062018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e724500\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e4\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000STDXXX030030000000000000XXX000STDXXX000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000XXXXXX000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01062018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01072015\u003c/PaymentHistoryEndDate\u003e\u003cRepaymentTenureFieldLength\u003e02\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e60\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e27168\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT019\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e25072012\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e31072014\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e02082014\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30082014\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e150000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e21\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01082014\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092011\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT020\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e02\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e11042012\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10042015\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e10042015\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30042015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e07\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e1200000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000STDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTD\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003eSTDSTDSTDSTDSTDSTDSTDSTDXXXSTDSTDSTDSTDSTDSTDSTDSTDSTD\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01042015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01052012\u003c/PaymentHistoryEndDate\u003e\u003cTypeOfCollateralFieldLength\u003e02\u003c/TypeOfCollateralFieldLength\u003e\u003cTypeOfCollateral\u003e01\u003c/TypeOfCollateral\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e14286\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT021\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e13\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e31102011\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e24112014\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e24112014\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30112014\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e37000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000026000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01112014\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01122011\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT022\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e13\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e31102011\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e29062015\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e29062015\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30062015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e37000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000148117086056025025000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01062015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01072012\u003c/PaymentHistoryEndDate\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003cActualPaymentAmountFieldLength\u003e04\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e3037\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT023\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e10\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e01062011\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e28082017\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e02072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e63871\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e05\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e-1765\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01082015\u003c/PaymentHistoryEndDate\u003e\u003cCreditLimitFieldLength\u003e05\u003c/CreditLimitFieldLength\u003e\u003cCreditLimit\u003e68700\u003c/CreditLimit\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT024\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e13\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e21012010\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e06022013\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e06022013\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e28022013\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e35766\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01022013\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01032010\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI001\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e28092018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e100000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI002\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26082018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e03\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e08\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e13000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI003\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17082018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e03\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e08\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI004\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02082018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI005\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17062018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI006\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24052018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e100000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI007\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e16052018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI008\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10052018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e61\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI009\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e08052018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e218000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI010\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e03042018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI011\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e30032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI012\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e29032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e06\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e60000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI013\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e28032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI014\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e266000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI015\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI016\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e01\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI017\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e52\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI018\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e20032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e61\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e100000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI019\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e19032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e01\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI020\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e16032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI021\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05022018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI022\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05012018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI023\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e15112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e600000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI024\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e14112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI025\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e13112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e100000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI026\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI027\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e04112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI028\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17102017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e01\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI029\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e16102017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e01\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e838000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI030\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e29082017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI031\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12082017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e61\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI032\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02082017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI033\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12052017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI034\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e22042017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI035\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e22042017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI036\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21042017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI037\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e11042017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e400000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI038\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21032017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI039\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24092016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI040\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e09092016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI041\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17082016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI042\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12082016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI043\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12082016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI044\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24062016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI045\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26052016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e800000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI046\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26052016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI047\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e31012016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e800000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI048\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05082015\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e400000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI049\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05052015\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e700000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI050\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26022015\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI051\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e25022015\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e2000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI052\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24072014\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI053\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10072014\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e06\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI054\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e25042014\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI055\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12092013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e400000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI056\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24082013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI057\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e25072013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI058\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17072013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI059\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e16072013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI060\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e06032013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI061\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12022013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e15\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e700000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI062\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02012013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI063\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e27112012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e800000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI064\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24092012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1400000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI065\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e31072012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI066\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e13072012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI067\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10072012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI068\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10052012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI069\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21042012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI070\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e13032012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e700000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI071\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10032012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI072\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12012012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI073\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e04012012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI074\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02112011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI075\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e22102011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e13\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e37000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI076\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e22102011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e13\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e37000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI077\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e04082011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI078\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02082011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI079\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e30072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI080\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e15\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI081\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e15072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI082\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e15072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e900000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI083\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI084\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e11052011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI085\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e03052011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e03\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI086\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e29042011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI087\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26042011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI088\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e13042011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e2000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI089\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12042011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI090\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e11102010\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1600000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI091\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e08102010\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1600000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI092\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12012010\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e13\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e35766\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI093\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e04122009\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e13\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e37000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnd\u003e\u003cSegmentTag\u003eES07\u003c/SegmentTag\u003e\u003cTotalLength\u003e0010521\u003c/TotalLength\u003e\u003c/End\u003e\u003c/CreditReport\u003e\u003c/BureauResponseXml\u003e\r\n\u003cSecondaryReportXml\u003e\u003cRoot\u003e\u003c/Root\u003e\u003c/SecondaryReportXml\u003e\r\n\u003cIsSucess\u003eTrue\u003c/IsSucess\u003e\r\n\u003c/CibilBureauResponse\u003e\r\n\u003c/Response\u003e\r\n\u003c/DsCibilBureau\u003e\r\n\u003c/Applicant\u003e\r\n\u003c/Field\u003e\u003cField key=\""Applicants\""\u003e\u003cApplicants\u003e\r\n\u003cApplicant\u003e\r\n\u003cAccounts\u003e\r\n\u003cAccount\u003e\r\n\u003cAccountNumber\u003e\r\n\u003c/AccountNumber\u003e\r\n\u003c/Account\u003e\r\n\u003c/Accounts\u003e\r\n\u003cMemberOtherId1Type\u003e\r\n\u003c/MemberOtherId1Type\u003e\r\n\u003cMemberOtherId1\u003e\r\n\u003c/MemberOtherId1\u003e\r\n\u003cMemberOtherId2Type\u003e\r\n\u003c/MemberOtherId2Type\u003e\r\n\u003cMemberOtherId2\u003e\r\n\u003c/MemberOtherId2\u003e\r\n\u003cMemberOtherId3Type\u003e\r\n\u003c/MemberOtherId3Type\u003e\r\n\u003cMemberOtherId3\u003e\r\n\u003c/MemberOtherId3\u003e\r\n\u003cKeyPersonName\u003e\r\n\u003c/KeyPersonName\u003e\r\n\u003cKeyPersonRelation\u003e\r\n\u003c/KeyPersonRelation\u003e\r\n\u003cMemberRelationName1\u003e\r\n\u003c/MemberRelationName1\u003e\r\n\u003cMemberRelationType1\u003e\r\n\u003c/MemberRelationType1\u003e\r\n\u003cMemberRelationName2\u003e\r\n\u003c/MemberRelationName2\u003e\r\n\u003cMemberRelationType2\u003e\r\n\u003c/MemberRelationType2\u003e\r\n\u003cMemberRelationName3\u003e\r\n\u003c/MemberRelationName3\u003e\r\n\u003cMemberRelationType3\u003e\r\n\u003c/MemberRelationType3\u003e\r\n\u003cMemberRelationName4\u003e\r\n\u003c/MemberRelationName4\u003e\r\n\u003cMemberRelationType4\u003e\r\n\u003c/MemberRelationType4\u003e\r\n\u003cNomineeName\u003e\r\n\u003c/NomineeName\u003e\r\n\u003cNomineeRelation\u003e\r\n\u003c/NomineeRelation\u003e\r\n\u003cAddresses\u003e\r\n\u003cAddress\u003e\r\n\u003cStateCode\u003e27\u003c/StateCode\u003e\r\n\u003cResidenceType\u003e01\u003c/ResidenceType\u003e\r\n\u003cPinCode\u003e411027\u003c/PinCode\u003e\r\n\u003cCity\u003ePUNE\u003c/City\u003e\r\n\u003cAddressType\u003e01\u003c/AddressType\u003e\r\n\u003cAddressLine5\u003e\r\n\u003c/AddressLine5\u003e\r\n\u003cAddressLine4\u003e\r\n\u003c/AddressLine4\u003e\r\n\u003cAddressLine3\u003eRIYADARSHINI NGR, OLD SANGAVI, PUNE \u003c/AddressLine3\u003e\r\n\u003cAddressLine2\u003eLD, B WING,  SAGER NR NATIONAL SCHOOL, P\u003c/AddressLine2\u003e\r\n\u003cAddressLine1\u003eSR NO 4/1/73, FL B-202, SHREE HARITAGE B\u003c/AddressLine1\u003e\r\n\u003c/Address\u003e\r\n\u003c/Addresses\u003e\r\n\u003cTelephones\u003e\r\n\u003cTelephone\u003e\r\n\u003cTelephoneType\u003e01\u003c/TelephoneType\u003e\r\n\u003cTelephoneNumber\u003e9823413581\u003c/TelephoneNumber\u003e\r\n\u003cTelephoneExtension\u003e\r\n\u003c/TelephoneExtension\u003e\r\n\u003c/Telephone\u003e\r\n\u003c/Telephones\u003e\r\n\u003cIdentifiers\u003e\r\n\u003cIdentifier\u003e\r\n\u003cIdType\u003e01\u003c/IdType\u003e\r\n\u003cIdNumber\u003eBACPP6592D\u003c/IdNumber\u003e\r\n\u003c/Identifier\u003e\r\n\u003cIdentifier\u003e\r\n\u003cIdType\u003e07\u003c/IdType\u003e\r\n\u003cIdNumber\u003e170650081989\u003c/IdNumber\u003e\r\n\u003c/Identifier\u003e\r\n\u003c/Identifiers\u003e\r\n\u003cCompanyName\u003e\r\n\u003c/CompanyName\u003e\r\n\u003cEmailAddress\u003e\r\n\u003c/EmailAddress\u003e\r\n\u003cGender\u003eMALE\u003c/Gender\u003e\r\n\u003cDateOfBirth\u003e17081986\u003c/DateOfBirth\u003e\r\n\u003cApplicantLastName\u003ePATEL\u003c/ApplicantLastName\u003e\r\n\u003cApplicantMiddleName\u003e\r\n\u003c/ApplicantMiddleName\u003e\r\n\u003cApplicantFirstName\u003eKISHOR MEKHJI\u003c/ApplicantFirstName\u003e\r\n\u003cDsCibilBureau\u003e\r\n\u003cDsCibilBureauData\u003e\r\n\u003cRequest\u003e\u003cRequest\u003e\u003cConsumerDetails\u003e\u003cCreditReportInquiry\u003e\u003cHeader\u003e\u003cSegmentTag\u003e TUEF \u003c/SegmentTag\u003e\u003cVersion\u003e 12 \u003c/Version\u003e\u003cReferenceNumber\u003e360073862\u003c/ReferenceNumber\u003e\u003cFutureUse1\u003e\u003c/FutureUse1\u003e\u003cMemberCode\u003eNB67098899_C2C\u003c/MemberCode\u003e\u003cPassword\u003elbW+juakg0\u003c/Password\u003e\u003cPurpose\u003e10\u003c/Purpose\u003e\u003cAmount\u003e100000\u003c/Amount\u003e\u003cFutureUse2\u003e\u003c/FutureUse2\u003e\u003cScoreType\u003e01\u003c/ScoreType\u003e\u003cOutputFormat\u003e01\u003c/OutputFormat\u003e\u003cResponseSize\u003e1\u003c/ResponseSize\u003e\u003cMediaType\u003eCC\u003c/MediaType\u003e\u003cAuthenticationMethod\u003eL\u003c/AuthenticationMethod\u003e\u003c/Header\u003e\u003cNames\u003e\u003cName\u003e \u003cConsumerName1\u003eKISHOR MEKHJI\u003c/ConsumerName1\u003e \u003cConsumerName2\u003e\u003c/ConsumerName2\u003e \u003cConsumerName3\u003ePATEL\u003c/ConsumerName3\u003e \u003cConsumerName4\u003e\u003c/ConsumerName4\u003e \u003cConsumerName5\u003e\u003c/ConsumerName5\u003e \u003cDateOfBirth\u003e17081986\u003c/DateOfBirth\u003e \u003cGender\u003eMALE\u003c/Gender\u003e \u003c/Name\u003e\u003c/Names\u003e\u003cIdentifications\u003e \u003cIdentification\u003e\u003cPanNo\u003eBACPP6592D\u003c/PanNo\u003e\u003cPassportNumber\u003e\u003c/PassportNumber\u003e\u003cDLNo\u003e\u003c/DLNo\u003e\u003cVoterId\u003e\u003c/VoterId\u003e\u003cUId\u003e\u003c/UId\u003e\u003cRationCardNo\u003e\u003c/RationCardNo\u003e\u003cAdditionalID1\u003e170650081989\u003c/AdditionalID1\u003e\u003cAdditionalID2\u003e\u003c/AdditionalID2\u003e\u003c/Identification\u003e\u003c/Identifications\u003e\u003cTelephones\u003e\u003cTelephone\u003e\u003cTelephoneNumber\u003e9823413581\u003c/TelephoneNumber\u003e\u003cTelephoneExtension\u003e\u003c/TelephoneExtension\u003e\u003cTelephoneType\u003e01\u003c/TelephoneType\u003e\u003c/Telephone\u003e\u003c/Telephones\u003e\u003cAddresses\u003e\u003cAddress\u003e\u003cAddressLine1\u003eSR NO 4/1/73, FL B-202, SHREE HARITAGE B\u003c/AddressLine1\u003e\u003cAddressLine2\u003eLD, B WING,  SAGER NR NATIONAL SCHOOL, P\u003c/AddressLine2\u003e\u003cAddressLine3\u003eRIYADARSHINI NGR, OLD SANGAVI, PUNE \u003c/AddressLine3\u003e\u003cAddressLine4\u003e\u003c/AddressLine4\u003e\u003cAddressLine5\u003e\u003c/AddressLine5\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e01\u003c/AddressCategory\u003e\u003cResidenceCode\u003e01\u003c/ResidenceCode\u003e\u003c/Address\u003e\u003c/Addresses\u003e\u003c/CreditReportInquiry\u003e\u003c/ConsumerDetails\u003e\u003c/Request\u003e\u003c/Request\u003e\r\n\u003c/DsCibilBureauData\u003e\r\n\u003cDsCibilBureauStatus\u003e\r\n\u003cTrail\u003e\u003c/Trail\u003e\r\n\u003c/DsCibilBureauStatus\u003e\r\n\u003cResponse\u003e\r\n\u003cCibilBureauResponse\u003e\r\n\u003cBureauResponseRaw\u003eTUEF12360073862                  0000NB67098899_C2C                100263130912228092018170049PN03N010107KISHORE0212MEKHJI PATEL0305PATEL07081708198608012ID03I010102010210BACPP6592DID03I020102060212492403246555PT03T01011102048606018030203PT03T020114211412345 EXT.030200PT03T0301109823413581030203PT03T0401109823413581030201EC03C010119BERABAZAR@GMAIL.COMEC03C020124BERABAZAR@REDIFFMAIL.COMEM03E01010252020831082018030204SC10CIBILTUSCR010201020210030828092018040500819130201250210PA03A010109PUNE PUNE06022707064110270802021008160520189001YPA03A020115AUNDH CAMP PUNE06022707064110270802020902011008190320189001YPA03A030140FLAT NO. B-202 HERITAGE BUILDING OLD SAN0227SANGAMNAGAR PUNE AUNDH CAMP0325PUNE NEAR NATIONAL SCHOOL0602270706411027080201100817102017PA03A040139S NO 4 1 62 CTM 3117 SHREE HERITAGE 2ND0235FLOOR FLAT NO B 202 SANGAM NAGAR NR0338NATIONAL SCHOOL OLD SANGVI PUNE 4110270602270706411027080204100831032015TL04T0010213NOT DISCLOSED040210050110808270620180908060820181108230920181304208414031002809007000000300801092018310801072018440203TL04T0020213NOT DISCLOSED04020505011080810052018090805072018110831072018120620000013061915402809000000000300801072018310801052018390236TL04T0030205KUDOS0310000000056604025105014080828032018090805072018110831072018120632000013063021542815000XXX000000XXX300801072018310801032018380516.00400513156TL04T0040213NOT DISCLOSED04025005014080828032018090830062018110830062018120630000013062731982812STDSTDSTDSTD300801062018310801032018TL04T0050213NOT DISCLOSED040210050110808100120180908290620181108310720181205287661306-50000282100000000000000000000030080107201831080101201836053000037046000450550000TL04T0060213NOT DISCLOSED04020505011080813122017090831072018110831072018120650000013064275402824000XXX000000000000000000300801072018310801122017TL04T0070213NOT DISCLOSED04020105011080824102017090810082018110831082018120684800013067279882833000000000000000000000000000000000300801082018310801102017TL04T0080213NOT DISCLOSED040252050140808070720171108310820181206100000130102821000000000000000000000300801082018310801022018350200380520.003902124004926345044000TL04T0090213NOT DISCLOSED040205050110808270920160908141220171008141220171108311220171206450000130102848000000000000000000000000000000000000000000000000300801122017310801092016TL04T0100205KUDOS03041164040251050140808080620160908010120171008270320171108310720171206300000130102836000XXXXXXXXXXXXXXX000XXXXXX000000000300801072017310801082016TL04T0110213NOT DISCLOSED0402510501108083005201609080508201811083108201812064150001306132429285400000000000000000000000000000000000000000000000000000029300000000000000000000000000000003008010820183108010520163902364005160654402034506417690TL04T0120213NOT DISCLOSED040205050110808310820150908270920161008280920161108300920161206301000130102839000000000000000000XXX000000000000000000300801092016310801092015TL04T0130213NOT DISCLOSED04020305011080806052015090810082018110831082018120670000013066467882854000000000000000000000000000000000000000000000000000000295400000000000000000000000000000000000000000000000000000030080108201831080109201534072328800350201380514.85390317840049797440203TL04T0140213NOT DISCLOSED0402020501108081103201509081005201511083105201512071100000130710955742803000300801052015310801052015TL04T0150213NOT DISCLOSED040202050110808110320150908100820181108310820181207110000013069875602854000000000000000000000000000000000000000000000000000000295400000000000000000000000000000000000000000000000000000030080108201831080109201534072328000350201380511.853903178400513202440203TL04T0160213NOT DISCLOSED040205050110808310720140908310820151008020920151108300920151206225000130102842000000000000000000000XXX000000000000000000300801092015310801082014TL04T0170213NOT DISCLOSED040210050110808300720130908210520151008101220151108311220151205318441302-428540000000000000000000000000850860530860560250260550860562936055026000026023000000XXX00000000000030080112201531080107201336051500044020345045310TL04T0180213NOT DISCLOSED040252050130808200220130908231020171008200220181108300620181206724500130142854000000000000STDXXX030030000000000000XXX000STDXXX0000002954000000000000000000000000XXXXXX000000000000000000000000300801062018310801072015390260400527168440203TL04T0190213NOT DISCLOSED0402050501108082507201209083107201410080208201411083008201412061500001301028540000000000000000000000000000000000000000000000000000002921000000000000000000000300801082014310801092011TL04T0200213NOT DISCLOSED0402020501408081104201209081004201510081004201511083004201512071200000130102854000STDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTD2954STDSTDSTDSTDSTDSTDSTDSTDXXXSTDSTDSTDSTDSTDSTDSTDSTDSTD300801042015310801052012350201400514286440203TL04T0210213NOT DISCLOSED040213050110808311020110908241120141008241120141108301120141205370001301028540000000000000000000000000000000000000000000000000260002954000000000000000000000000000000000000000000000000000000300801112014310801122011TL04T0220213NOT DISCLOSED04021305011080831102011090829062015100829062015110830062015120537000130102854000000000000000148117086056025025000000000000000000000295400000000000000000000000000000000000000000000000000000030080106201531080107201244020345043037TL04T0230213NOT DISCLOSED040210050110808010620110908280820171108020720181205638711305-176528540000000000000000000000000000000000000000000000000000002954000000000000000000000000000000000000000000000000000000300801072018310801082015360568700TL04T0240213NOT DISCLOSED040213050110808210120100908060220131008060220131108280220131205357661301028540000000000000000000000000000000000000000000000000000002954000000000000000000000000000000000000000000000000000000300801022013310801032010IQ04I0010108280920180405KUDOS0502100606100000IQ04I0020108260820180413NOT DISCLOSED050203060813000000IQ04I0030108170820180413NOT DISCLOSED050203060810000000IQ04I0040108020820180413NOT DISCLOSED0502050606200000IQ04I0050108170620180413NOT DISCLOSED05020006041000IQ04I0060108240520180413NOT DISCLOSED0502050606100000IQ04I0070108160520180413NOT DISCLOSED0502050606200000IQ04I0080108100520180413NOT DISCLOSED0502610606500000IQ04I0090108080520180413NOT DISCLOSED0502050606218000IQ04I0100108030420180413NOT DISCLOSED05021006041000IQ04I0110108300320180413NOT DISCLOSED05025106071000000IQ04I0120108290320180413NOT DISCLOSED050206060560000IQ04I0130108280320180413NOT DISCLOSED05025106071000000IQ04I0140108260320180413NOT DISCLOSED0502000606266000IQ04I0150108210320180413NOT DISCLOSED05025106071000000IQ04I0160108210320180413NOT DISCLOSED05020006011IQ04I0170108210320180413NOT DISCLOSED05025206071000000IQ04I0180108200320180405KUDOS0502610606100000IQ04I0190108190320180413NOT DISCLOSED05020006011IQ04I0200108160320180413NOT DISCLOSED05025106071500000IQ04I0210108050220180413NOT DISCLOSED05021006041000IQ04I0220108050120180413NOT DISCLOSED050210060510000IQ04I0230108151120170413NOT DISCLOSED0502050606600000IQ04I0240108141120170413NOT DISCLOSED050210060510000IQ04I0250108131120170413NOT DISCLOSED0502050606100000IQ04I0260108101120170413NOT DISCLOSED050210060550000IQ04I0270108041120170413NOT DISCLOSED05021006041000IQ04I0280108171020170413NOT DISCLOSED0502010606500000IQ04I0290108161020170413NOT DISCLOSED0502010606838000IQ04I0300108290820170413NOT DISCLOSED050210060510000IQ04I0310108120820170413NOT DISCLOSED0502610606500000IQ04I0320108020820170413NOT DISCLOSED05020206071000000IQ04I0330108120520170413NOT DISCLOSED050210060550000IQ04I0340108220420170413NOT DISCLOSED05020006071000000IQ04I0350108220420170413NOT DISCLOSED05025106071500000IQ04I0360108210420170413NOT DISCLOSED05025106071000000IQ04I0370108110420170413NOT DISCLOSED0502510606400000IQ04I0380108210320170405KUDOS0502510606200000IQ04I0390108240920160413NOT DISCLOSED0502050606500000IQ04I0400108090920160413NOT DISCLOSED0502050606500000IQ04I0410108170820160413NOT DISCLOSED050210060550000IQ04I0420108120820160413NOT DISCLOSED0502050606500000IQ04I0430108120820160413NOT DISCLOSED0502050606500000IQ04I0440108240620160413NOT DISCLOSED0502020606500000IQ04I0450108260520160413NOT DISCLOSED0502050606800000IQ04I0460108260520160405KUDOS0502000606500000IQ04I0470108310120160413NOT DISCLOSED0502050606800000IQ04I0480108050820150413NOT DISCLOSED0502050606400000IQ04I0490108050520150413NOT DISCLOSED0502020606700000IQ04I0500108260220150413NOT DISCLOSED050210060510000IQ04I0510108250220150413NOT DISCLOSED05020206072000000IQ04I0520108240720140413NOT DISCLOSED0502050606300000IQ04I0530108100720140413NOT DISCLOSED050206060550000IQ04I0540108250420140413NOT DISCLOSED0502050606300000IQ04I0550108120920130413NOT DISCLOSED0502050606400000IQ04I0560108240820130413NOT DISCLOSED050210060550000IQ04I0570108250720130413NOT DISCLOSED050210060550000IQ04I0580108170720130413NOT DISCLOSED050210060510000IQ04I0590108160720130413NOT DISCLOSED0502050606200000IQ04I0600108060320130413NOT DISCLOSED0502050606500000IQ04I0610108120220130413NOT DISCLOSED0502150606700000IQ04I0620108020120130413NOT DISCLOSED050210060510000IQ04I0630108271120120413NOT DISCLOSED0502510606800000IQ04I0640108240920120413NOT DISCLOSED05020206071400000IQ04I0650108310720120413NOT DISCLOSED0502510606500000IQ04I0660108130720120413NOT DISCLOSED0502050606200000IQ04I0670108100720120413NOT DISCLOSED0502050606200000IQ04I0680108100520120413NOT DISCLOSED0502050606300000IQ04I0690108210420120405KUDOS0502510606200000IQ04I0700108130320120413NOT DISCLOSED0502050606700000IQ04I0710108100320120413NOT DISCLOSED05020206071300000IQ04I0720108120120120413NOT DISCLOSED0502050606500000IQ04I0730108040120120413NOT DISCLOSED0502050606500000IQ04I0740108021120110413NOT DISCLOSED0502510606300000IQ04I0750108221020110413NOT DISCLOSED050213060537000IQ04I0760108221020110413NOT DISCLOSED050213060537000IQ04I0770108040820110413NOT DISCLOSED0502050606500000IQ04I0780108020820110413NOT DISCLOSED0502050606500000IQ04I0790108300720110413NOT DISCLOSED0502050606300000IQ04I0800108260720110413NOT DISCLOSED05021506071000000IQ04I0810108150720110413NOT DISCLOSED050202060510000IQ04I0820108150720110413NOT DISCLOSED0502000606900000IQ04I0830108050720110413NOT DISCLOSED05021006041000IQ04I0840108110520110413NOT DISCLOSED050205060510000IQ04I0850108030520110413NOT DISCLOSED05020306071500000IQ04I0860108290420110413NOT DISCLOSED050210060510000IQ04I0870108260420110413NOT DISCLOSED0502510606300000IQ04I0880108130420110413NOT DISCLOSED05020206072000000IQ04I0890108120420110413NOT DISCLOSED05020206071500000IQ04I0900108111020100413NOT DISCLOSED05020206071600000IQ04I0910108081020100413NOT DISCLOSED05020206071600000IQ04I0920108120120100413NOT DISCLOSED050213060535766IQ04I0930108041220090413NOT DISCLOSED050213060537000ES0700105210102**\u003c/BureauResponseRaw\u003e\r\n\u003cBureauResponseXml\u003e\u003cCreditReport\u003e\u003cHeader\u003e\u003cSegmentTag\u003eTUEF\u003c/SegmentTag\u003e\u003cVersion\u003e12\u003c/Version\u003e\u003cReferenceNumber\u003e360073862\u003c/ReferenceNumber\u003e\u003cMemberCode\u003eNB67098899_C2C                \u003c/MemberCode\u003e\u003cSubjectReturnCode\u003e1\u003c/SubjectReturnCode\u003e\u003cEnquiryControlNumber\u003e002631309122\u003c/EnquiryControlNumber\u003e\u003cDateProcessed\u003e28092018\u003c/DateProcessed\u003e\u003cTimeProcessed\u003e170049\u003c/TimeProcessed\u003e\u003c/Header\u003e\u003cNameSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eN01\u003c/SegmentTag\u003e\u003cConsumerName1FieldLength\u003e07\u003c/ConsumerName1FieldLength\u003e\u003cConsumerName1\u003eKISHORE\u003c/ConsumerName1\u003e\u003cConsumerName2FieldLength\u003e12\u003c/ConsumerName2FieldLength\u003e\u003cConsumerName2\u003eMEKHJI PATEL\u003c/ConsumerName2\u003e\u003cConsumerName3FieldLength\u003e05\u003c/ConsumerName3FieldLength\u003e\u003cConsumerName3\u003ePATEL\u003c/ConsumerName3\u003e\u003cDateOfBirthFieldLength\u003e08\u003c/DateOfBirthFieldLength\u003e\u003cDateOfBirth\u003e17081986\u003c/DateOfBirth\u003e\u003cGenderFieldLength\u003e01\u003c/GenderFieldLength\u003e\u003cGender\u003e2\u003c/Gender\u003e\u003c/NameSegment\u003e\u003cIDSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eI01\u003c/SegmentTag\u003e\u003cIDType\u003e01\u003c/IDType\u003e\u003cIDNumberFieldLength\u003e10\u003c/IDNumberFieldLength\u003e\u003cIDNumber\u003eBACPP6592D\u003c/IDNumber\u003e\u003c/IDSegment\u003e\u003cIDSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eI02\u003c/SegmentTag\u003e\u003cIDType\u003e06\u003c/IDType\u003e\u003cIDNumberFieldLength\u003e12\u003c/IDNumberFieldLength\u003e\u003cIDNumber\u003e492403246555\u003c/IDNumber\u003e\u003c/IDSegment\u003e\u003cTelephoneSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eT01\u003c/SegmentTag\u003e\u003cTelephoneNumberFieldLength\u003e11\u003c/TelephoneNumberFieldLength\u003e\u003cTelephoneNumber\u003e02048606018\u003c/TelephoneNumber\u003e\u003cTelephoneType\u003e03\u003c/TelephoneType\u003e\u003c/TelephoneSegment\u003e\u003cTelephoneSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eT02\u003c/SegmentTag\u003e\u003cTelephoneNumberFieldLength\u003e14\u003c/TelephoneNumberFieldLength\u003e\u003cTelephoneNumber\u003e211412345 EXT.\u003c/TelephoneNumber\u003e\u003cTelephoneType\u003e00\u003c/TelephoneType\u003e\u003c/TelephoneSegment\u003e\u003cTelephoneSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eT03\u003c/SegmentTag\u003e\u003cTelephoneNumberFieldLength\u003e10\u003c/TelephoneNumberFieldLength\u003e\u003cTelephoneNumber\u003e9823413581\u003c/TelephoneNumber\u003e\u003cTelephoneType\u003e03\u003c/TelephoneType\u003e\u003c/TelephoneSegment\u003e\u003cTelephoneSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eT04\u003c/SegmentTag\u003e\u003cTelephoneNumberFieldLength\u003e10\u003c/TelephoneNumberFieldLength\u003e\u003cTelephoneNumber\u003e9823413581\u003c/TelephoneNumber\u003e\u003cTelephoneType\u003e01\u003c/TelephoneType\u003e\u003c/TelephoneSegment\u003e\u003cEmailContactSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eC01\u003c/SegmentTag\u003e\u003cEmailIDFieldLength\u003e19\u003c/EmailIDFieldLength\u003e\u003cEmailID\u003eBERABAZAR@GMAIL.COM\u003c/EmailID\u003e\u003c/EmailContactSegment\u003e\u003cEmailContactSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eC02\u003c/SegmentTag\u003e\u003cEmailIDFieldLength\u003e24\u003c/EmailIDFieldLength\u003e\u003cEmailID\u003eBERABAZAR@REDIFFMAIL.COM\u003c/EmailID\u003e\u003c/EmailContactSegment\u003e\u003cEmploymentSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eE01\u003c/SegmentTag\u003e\u003cAccountType\u003e52\u003c/AccountType\u003e\u003cDateReportedCertified\u003e31082018\u003c/DateReportedCertified\u003e\u003cOccupationCode\u003e04\u003c/OccupationCode\u003e\u003c/EmploymentSegment\u003e\u003cScoreSegment\u003e\u003cLength\u003e10\u003c/Length\u003e\u003cScoreName\u003eCIBILTUSCR\u003c/ScoreName\u003e\u003cScoreCardName\u003e01\u003c/ScoreCardName\u003e\u003cScoreCardVersion\u003e10\u003c/ScoreCardVersion\u003e\u003cScoreDate\u003e28092018\u003c/ScoreDate\u003e\u003cScore\u003e00819\u003c/Score\u003e\u003cExclusionCode9FieldLength\u003e02\u003c/ExclusionCode9FieldLength\u003e\u003cExclusionCode9\u003e01\u003c/ExclusionCode9\u003e\u003cReasonCode1FieldLength\u003e02\u003c/ReasonCode1FieldLength\u003e\u003cReasonCode1\u003e10\u003c/ReasonCode1\u003e\u003c/ScoreSegment\u003e\u003cAddress\u003e\u003cAddressSegmentTag\u003ePA\u003c/AddressSegmentTag\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eA01\u003c/SegmentTag\u003e\u003cAddressLine1FieldLength\u003e09\u003c/AddressLine1FieldLength\u003e\u003cAddressLine1\u003ePUNE PUNE\u003c/AddressLine1\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCodeFieldLength\u003e06\u003c/PinCodeFieldLength\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e02\u003c/AddressCategory\u003e\u003cDateReported\u003e16052018\u003c/DateReported\u003e\u003cEnrichedThroughEnquiry\u003eY\u003c/EnrichedThroughEnquiry\u003e\u003c/Address\u003e\u003cAddress\u003e\u003cAddressSegmentTag\u003ePA\u003c/AddressSegmentTag\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eA02\u003c/SegmentTag\u003e\u003cAddressLine1FieldLength\u003e15\u003c/AddressLine1FieldLength\u003e\u003cAddressLine1\u003eAUNDH CAMP PUNE\u003c/AddressLine1\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCodeFieldLength\u003e06\u003c/PinCodeFieldLength\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e02\u003c/AddressCategory\u003e\u003cResidenceCode\u003e01\u003c/ResidenceCode\u003e\u003cDateReported\u003e19032018\u003c/DateReported\u003e\u003cEnrichedThroughEnquiry\u003eY\u003c/EnrichedThroughEnquiry\u003e\u003c/Address\u003e\u003cAddress\u003e\u003cAddressSegmentTag\u003ePA\u003c/AddressSegmentTag\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eA03\u003c/SegmentTag\u003e\u003cAddressLine1FieldLength\u003e40\u003c/AddressLine1FieldLength\u003e\u003cAddressLine1\u003eFLAT NO. B-202 HERITAGE BUILDING OLD SAN\u003c/AddressLine1\u003e\u003cAddressLine2FieldLength\u003e27\u003c/AddressLine2FieldLength\u003e\u003cAddressLine2\u003eSANGAMNAGAR PUNE AUNDH CAMP\u003c/AddressLine2\u003e\u003cAddressLine3FieldLength\u003e25\u003c/AddressLine3FieldLength\u003e\u003cAddressLine3\u003ePUNE NEAR NATIONAL SCHOOL\u003c/AddressLine3\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCodeFieldLength\u003e06\u003c/PinCodeFieldLength\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e01\u003c/AddressCategory\u003e\u003cDateReported\u003e17102017\u003c/DateReported\u003e\u003c/Address\u003e\u003cAddress\u003e\u003cAddressSegmentTag\u003ePA\u003c/AddressSegmentTag\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eA04\u003c/SegmentTag\u003e\u003cAddressLine1FieldLength\u003e39\u003c/AddressLine1FieldLength\u003e\u003cAddressLine1\u003eS NO 4 1 62 CTM 3117 SHREE HERITAGE 2ND\u003c/AddressLine1\u003e\u003cAddressLine2FieldLength\u003e35\u003c/AddressLine2FieldLength\u003e\u003cAddressLine2\u003eFLOOR FLAT NO B 202 SANGAM NAGAR NR\u003c/AddressLine2\u003e\u003cAddressLine3FieldLength\u003e38\u003c/AddressLine3FieldLength\u003e\u003cAddressLine3\u003eNATIONAL SCHOOL OLD SANGVI PUNE 411027\u003c/AddressLine3\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCodeFieldLength\u003e06\u003c/PinCodeFieldLength\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e04\u003c/AddressCategory\u003e\u003cDateReported\u003e31032015\u003c/DateReported\u003e\u003c/Address\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT001\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e10\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e27062018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e06082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e23092018\u003c/DateReportedAndCertified\u003e\u003cCurrentBalanceFieldLength\u003e04\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e2084\u003c/CurrentBalance\u003e\u003cAmountOverdueFieldLength\u003e03\u003c/AmountOverdueFieldLength\u003e\u003cAmountOverdue\u003e100\u003c/AmountOverdue\u003e\u003cPaymentHistory1FieldLength\u003e09\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e007000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01092018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01072018\u003c/PaymentHistoryEndDate\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT002\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e10052018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e05072018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e200000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e191540\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e09\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01052018\u003c/PaymentHistoryEndDate\u003e\u003cRepaymentTenureFieldLength\u003e02\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e36\u003c/RepaymentTenure\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT003\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e05\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e05\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eKUDOS\u003c/ReportingMemberShortName\u003e\u003cAccountNumberFieldLength\u003e10\u003c/AccountNumberFieldLength\u003e\u003cAccountNumber\u003e0000000566\u003c/AccountNumber\u003e\u003cAccountType\u003e51\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e28032018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e05072018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e320000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e302154\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e15\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000XXX000000XXX\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01032018\u003c/PaymentHistoryEndDate\u003e\u003cRateOfInterestFieldLength\u003e05\u003c/RateOfInterestFieldLength\u003e\u003cRateOfInterest\u003e16.00\u003c/RateOfInterest\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e13156\u003c/EmiAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT004\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e50\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e28032018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e30062018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e30062018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e300000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e273198\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e12\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003eSTDSTDSTDSTD\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01062018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01032018\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT005\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e10\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e10012018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e29062018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e28766\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e-50000\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e21\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01012018\u003c/PaymentHistoryEndDate\u003e\u003cCreditLimitFieldLength\u003e05\u003c/CreditLimitFieldLength\u003e\u003cCreditLimit\u003e30000\u003c/CreditLimit\u003e\u003cCashLimitFieldLength\u003e04\u003c/CashLimitFieldLength\u003e\u003cCashLimit\u003e6000\u003c/CashLimit\u003e\u003cActualPaymentAmountFieldLength\u003e05\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e50000\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT006\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e13122017\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e31072018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e500000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e427540\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e24\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000XXX000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01122017\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT007\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e01\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e24102017\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e848000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e727988\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e33\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01102017\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT008\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e52\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e07072017\u003c/DateOpenedOrDisbursed\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e100000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e21\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01022018\u003c/PaymentHistoryEndDate\u003e\u003cTypeOfCollateralFieldLength\u003e02\u003c/TypeOfCollateralFieldLength\u003e\u003cTypeOfCollateral\u003e00\u003c/TypeOfCollateral\u003e\u003cRateOfInterestFieldLength\u003e05\u003c/RateOfInterestFieldLength\u003e\u003cRateOfInterest\u003e20.00\u003c/RateOfInterest\u003e\u003cRepaymentTenureFieldLength\u003e02\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e12\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e04\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e9263\u003c/EmiAmount\u003e\u003cActualPaymentAmountFieldLength\u003e04\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e4000\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT009\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e27092016\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e14122017\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e14122017\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e31122017\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e450000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e48\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01122017\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092016\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT010\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e05\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e05\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eKUDOS\u003c/ReportingMemberShortName\u003e\u003cAccountNumberFieldLength\u003e04\u003c/AccountNumberFieldLength\u003e\u003cAccountNumber\u003e1164\u003c/AccountNumber\u003e\u003cAccountType\u003e51\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e08062016\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e01012017\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e27032017\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e31072017\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e300000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e36\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000XXXXXXXXXXXXXXX000XXXXXX000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072017\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01082016\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT011\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e51\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e30052016\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e05082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e415000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e132429\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e30\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01052016\u003c/PaymentHistoryEndDate\u003e\u003cRepaymentTenureFieldLength\u003e02\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e36\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e16065\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003cActualPaymentAmountFieldLength\u003e06\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e417690\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT012\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e31082015\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e27092016\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e28092016\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30092016\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e301000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e39\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000XXX000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01092016\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092015\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT013\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e03\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e06052015\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e700000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e646788\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092015\u003c/PaymentHistoryEndDate\u003e\u003cValueOfCollateralFieldLength\u003e07\u003c/ValueOfCollateralFieldLength\u003e\u003cValueOfCollateral\u003e2328800\u003c/ValueOfCollateral\u003e\u003cTypeOfCollateralFieldLength\u003e02\u003c/TypeOfCollateralFieldLength\u003e\u003cTypeOfCollateral\u003e01\u003c/TypeOfCollateral\u003e\u003cRateOfInterestFieldLength\u003e05\u003c/RateOfInterestFieldLength\u003e\u003cRateOfInterest\u003e14.85\u003c/RateOfInterest\u003e\u003cRepaymentTenureFieldLength\u003e03\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e178\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e04\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e9797\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT014\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e02\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e11032015\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10052015\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31052015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e07\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e1100000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e07\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e1095574\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e03\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01052015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01052015\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT015\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e02\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e11032015\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e07\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e1100000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e987560\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092015\u003c/PaymentHistoryEndDate\u003e\u003cValueOfCollateralFieldLength\u003e07\u003c/ValueOfCollateralFieldLength\u003e\u003cValueOfCollateral\u003e2328000\u003c/ValueOfCollateral\u003e\u003cTypeOfCollateralFieldLength\u003e02\u003c/TypeOfCollateralFieldLength\u003e\u003cTypeOfCollateral\u003e01\u003c/TypeOfCollateral\u003e\u003cRateOfInterestFieldLength\u003e05\u003c/RateOfInterestFieldLength\u003e\u003cRateOfInterest\u003e11.85\u003c/RateOfInterest\u003e\u003cRepaymentTenureFieldLength\u003e03\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e178\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e13202\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT016\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e31072014\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e31082015\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e02092015\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30092015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e225000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e42\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000XXX000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01092015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01082014\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT017\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e10\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e30072013\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e21052015\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e10122015\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e31122015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e31844\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e02\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e-4\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000085086053086056025026055086056\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e36\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e055026000026023000000XXX000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01122015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01072013\u003c/PaymentHistoryEndDate\u003e\u003cCreditLimitFieldLength\u003e05\u003c/CreditLimitFieldLength\u003e\u003cCreditLimit\u003e15000\u003c/CreditLimit\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003cActualPaymentAmountFieldLength\u003e04\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e5310\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT018\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e52\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e3\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e20022013\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e23102017\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e20022018\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30062018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e724500\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e4\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000STDXXX030030000000000000XXX000STDXXX000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000XXXXXX000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01062018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01072015\u003c/PaymentHistoryEndDate\u003e\u003cRepaymentTenureFieldLength\u003e02\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e60\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e27168\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT019\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e25072012\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e31072014\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e02082014\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30082014\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e150000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e21\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01082014\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092011\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT020\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e02\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e11042012\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10042015\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e10042015\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30042015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e07\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e1200000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000STDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTD\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003eSTDSTDSTDSTDSTDSTDSTDSTDXXXSTDSTDSTDSTDSTDSTDSTDSTDSTD\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01042015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01052012\u003c/PaymentHistoryEndDate\u003e\u003cTypeOfCollateralFieldLength\u003e02\u003c/TypeOfCollateralFieldLength\u003e\u003cTypeOfCollateral\u003e01\u003c/TypeOfCollateral\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e14286\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT021\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e13\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e31102011\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e24112014\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e24112014\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30112014\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e37000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000026000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01112014\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01122011\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT022\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e13\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e31102011\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e29062015\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e29062015\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30062015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e37000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000148117086056025025000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01062015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01072012\u003c/PaymentHistoryEndDate\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003cActualPaymentAmountFieldLength\u003e04\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e3037\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT023\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e10\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e01062011\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e28082017\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e02072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e63871\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e05\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e-1765\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01082015\u003c/PaymentHistoryEndDate\u003e\u003cCreditLimitFieldLength\u003e05\u003c/CreditLimitFieldLength\u003e\u003cCreditLimit\u003e68700\u003c/CreditLimit\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT024\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e13\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e21012010\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e06022013\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e06022013\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e28022013\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e35766\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01022013\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01032010\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI001\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e28092018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e100000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI002\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26082018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e03\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e08\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e13000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI003\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17082018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e03\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e08\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI004\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02082018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI005\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17062018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI006\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24052018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e100000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI007\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e16052018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI008\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10052018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e61\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI009\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e08052018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e218000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI010\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e03042018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI011\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e30032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI012\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e29032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e06\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e60000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI013\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e28032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI014\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e266000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI015\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI016\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e01\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI017\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e52\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI018\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e20032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e61\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e100000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI019\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e19032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e01\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI020\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e16032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI021\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05022018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI022\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05012018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI023\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e15112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e600000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI024\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e14112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI025\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e13112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e100000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI026\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI027\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e04112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI028\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17102017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e01\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI029\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e16102017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e01\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e838000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI030\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e29082017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI031\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12082017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e61\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI032\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02082017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI033\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12052017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI034\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e22042017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI035\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e22042017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI036\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21042017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI037\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e11042017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e400000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI038\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21032017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI039\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24092016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI040\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e09092016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI041\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17082016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI042\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12082016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI043\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12082016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI044\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24062016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI045\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26052016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e800000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI046\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26052016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI047\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e31012016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e800000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI048\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05082015\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e400000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI049\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05052015\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e700000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI050\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26022015\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI051\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e25022015\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e2000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI052\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24072014\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI053\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10072014\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e06\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI054\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e25042014\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI055\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12092013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e400000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI056\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24082013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI057\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e25072013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI058\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17072013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI059\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e16072013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI060\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e06032013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI061\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12022013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e15\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e700000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI062\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02012013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI063\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e27112012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e800000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI064\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24092012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1400000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI065\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e31072012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI066\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e13072012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI067\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10072012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI068\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10052012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI069\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21042012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI070\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e13032012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e700000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI071\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10032012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI072\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12012012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI073\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e04012012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI074\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02112011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI075\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e22102011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e13\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e37000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI076\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e22102011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e13\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e37000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI077\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e04082011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI078\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02082011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI079\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e30072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI080\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e15\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI081\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e15072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI082\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e15072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e900000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI083\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI084\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e11052011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI085\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e03052011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e03\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI086\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e29042011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI087\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26042011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI088\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e13042011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e2000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI089\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12042011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI090\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e11102010\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1600000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI091\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e08102010\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1600000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI092\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12012010\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e13\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e35766\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI093\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e04122009\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e13\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e37000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnd\u003e\u003cSegmentTag\u003eES07\u003c/SegmentTag\u003e\u003cTotalLength\u003e0010521\u003c/TotalLength\u003e\u003c/End\u003e\u003c/CreditReport\u003e\u003c/BureauResponseXml\u003e\r\n\u003cSecondaryReportXml\u003e\u003cRoot\u003e\u003c/Root\u003e\u003c/SecondaryReportXml\u003e\r\n\u003cIsSucess\u003eTrue\u003c/IsSucess\u003e\r\n\u003c/CibilBureauResponse\u003e\r\n\u003c/Response\u003e\r\n\u003c/DsCibilBureau\u003e\r\n\u003c/Applicant\u003e\r\n\u003c/Applicants\u003e\r\n\u003c/Field\u003e\u003cField key=\""ApplicationData\""\u003e\u003cApplicationData\u003e\r\n\u003cFormattedReport\u003eFalse\u003c/FormattedReport\u003e\r\n\u003cMFIBranchReferenceNo\u003e\r\n\u003c/MFIBranchReferenceNo\u003e\r\n\u003cMFICenterReferenceNo\u003e\r\n\u003c/MFICenterReferenceNo\u003e\r\n\u003cMFILoanPurpose\u003e40\u003c/MFILoanPurpose\u003e\r\n\u003cMFIEnquiryAmount\u003e10000000\u003c/MFIEnquiryAmount\u003e\r\n\u003cConsumerConsentForUIDAIAuthentication\u003eY\u003c/ConsumerConsentForUIDAIAuthentication\u003e\r\n\u003cNTCProductType\u003eAL\u003c/NTCProductType\u003e\r\n\u003cMFIBureauFlag\u003eFALSE\u003c/MFIBureauFlag\u003e\r\n\u003cIDVerificationFlag\u003eFALSE\u003c/IDVerificationFlag\u003e\r\n\u003cDSTuNtcFlag\u003eFALSE\u003c/DSTuNtcFlag\u003e\r\n\u003cCibilBureauFlag\u003eFALSE\u003c/CibilBureauFlag\u003e\r\n\u003cPassword\u003elbW+juakg0\u003c/Password\u003e\r\n\u003cMemberCode\u003eNB67098899_C2C\u003c/MemberCode\u003e\r\n\u003cGSTStateCode\u003e27\u003c/GSTStateCode\u003e\r\n\u003cScoreType\u003e01\u003c/ScoreType\u003e\r\n\u003cAmount\u003e100000\u003c/Amount\u003e\r\n\u003cPurpose\u003e10\u003c/Purpose\u003e\r\n\u003cUser\u003eKUDOS_Dev_User\u003c/User\u003e\r\n\u003cBusinessUnitId\u003e410\u003c/BusinessUnitId\u003e\r\n\u003cApplicationId\u003e360073862\u003c/ApplicationId\u003e\r\n\u003cSolutionSetId\u003e2081\u003c/SolutionSetId\u003e\r\n\u003cEnvironmentTypeId\u003e2\u003c/EnvironmentTypeId\u003e\r\n\u003cEnvironmentType\u003eProduction\u003c/EnvironmentType\u003e\r\n\u003cMilestone\u003e\r\n\u003cStep\u003eCapture Quick Application Data\u003c/Step\u003e\r\n\u003cStep\u003eCIBIL Bureau call\u003c/Step\u003e\r\n\u003cStep\u003eIDVision Score\u003c/Step\u003e\r\n\u003cStep\u003eDomain Verification\u003c/Step\u003e\r\n\u003c/Milestone\u003e\r\n\u003cStart\u003e2018-09-28T11:30:48.5782469Z\u003c/Start\u003e\r\n\u003cInputValReasonCodes\u003e\r\n\u003c/InputValReasonCodes\u003e\r\n\u003cDTTrail\u003e\r\n\u003cStep\u003e\r\n\u003cName\u003eCapture Quick Application Data\u003c/Name\u003e\r\n\u003cDuration\u003e00:00:00.0156001\u003c/Duration\u003e\r\n\u003c/Step\u003e\r\n\u003cStep\u003e\r\n\u003cName\u003eCIBIL Bureau call\u003c/Name\u003e\r\n\u003cDuration\u003e00:00:01.3260023\u003c/Duration\u003e\r\n\u003c/Step\u003e\r\n\u003cStep\u003e\r\n\u003cName\u003eIDVision Score\u003c/Name\u003e\r\n\u003cDuration\u003e00:00:00.0156001\u003c/Duration\u003e\r\n\u003c/Step\u003e\r\n\u003cStep\u003e\r\n\u003cName\u003eDomain Verification\u003c/Name\u003e\r\n\u003cDuration\u003e00:00:00.0156001\u003c/Duration\u003e\r\n\u003c/Step\u003e\r\n\u003c/DTTrail\u003e\r\n\u003c/ApplicationData\u003e\r\n\u003c/Field\u003e\u003cField key=\""Decision\""\u003ePass\u003c/Field\u003e\u003c/ContextData\u003e\u003c/DCResponse\u003e""}";
                    vRtnXML = oAuth.SendCIBIL(strFinalXML);

                    //RootObjectResponse ror = new RootObjectResponse();

                    RootObjectResponse ror = JsonConvert.DeserializeObject<RootObjectResponse>(vRtnXML);

                    vRtnXML = ror.Data;


                    if (Convert.ToBoolean(ror.Result) == true)
                    {
                        Int32 vErr = 0;
                        string vResult = "", vstatus = "", vReportId = "", vScoreValue = "";
                        XmlDocument xd = new XmlDocument();
                        xd.LoadXml(vRtnXML);
                        XmlNodeList xnStatus = xd.SelectNodes("/DCResponse/Status");
                        foreach (XmlNode xn in xnStatus)
                        {
                            vstatus = xn.InnerText;
                        }
                        if (vstatus == "Success")
                        {
                            XmlNodeList xnList = xd.SelectNodes("/DCResponse/ContextData/Field[@key='Decision']");
                            foreach (XmlNode xn in xnList)
                            {
                                vResult = xn.InnerText;
                            }
                            if (vResult == "Pass")
                            {

                                // Get ReportId from Response XML
                                XmlNodeList XnReport = xd.SelectNodes("/DCResponse/ContextData/Field[@key='Applicants']/Applicants/Applicant/DsCibilBureau/Response/CibilBureauResponse/BureauResponseXml/CreditReport/Header/EnquiryControlNumber");
                                foreach (XmlNode xn in XnReport)
                                {
                                    vReportId = xn.InnerText;
                                }
                                // Get ScoreValue from Response XML
                                XmlNodeList XnScoreValue = xd.SelectNodes("/DCResponse/ContextData/Field[@key='Applicants']/Applicants/Applicant/DsCibilBureau/Response/CibilBureauResponse/BureauResponseXml/CreditReport/ScoreSegment/Score");
                                foreach (XmlNode xn in XnScoreValue)
                                {
                                    vScoreValue = xn.InnerText;
                                }
                                string vErrDesc = "";
                                vErr = CMem.CibilXMLPurser(Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(hdUserID.Value), gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vRtnXML, ref vErrDesc);
                                if (vErr == 0)
                                {
                                    // gblFuction.AjxMsgPopup("Success");
                                }
                                else
                                {
                                    gblFuction.AjxMsgPopup(vErrDesc);
                                    return;
                                }
                                // Save Respone XML
                                vErr = CMem.SaveCIBILResponseXML(vReportId, vRtnXML);
                                if (vErr == 0)
                                {
                                    //
                                }
                                else
                                {
                                    gblFuction.AjxMsgPopup("Response XML Save Failed..");
                                    return;
                                }

                                // Update LoanAppCoApp Relation Table with ReportId and Score

                                vErr = CMem.UpdateLnAppCoApp(pLnAppId, pCoappId, vReportId, vScoreValue);
                                if (vErr == 0)
                                {
                                    //
                                }
                                else
                                {
                                    gblFuction.AjxMsgPopup("Loan Application and Co-Applicant Relation Update Failed..");
                                    return;
                                }

                                string BrCode = Session[gblValue.BrnchCode].ToString();
                                ShowInitialLoanAppData(pLnAppId, BrCode);
                                ResetCIBILGrid();
                                LoadCBOnlineList(pLnAppId);
                            }
                            else
                            {
                                gblFuction.AjxMsgPopup("Failed");
                                return;
                            }
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Not Success");
                            return;
                        }
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("failed by Cibil");
                        return;
                    }
                }
            }
        }
        protected void gvLoanApp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vApplicantionID = "";
            vApplicantionID = Convert.ToString(e.CommandArgument);
            ViewState["LoanAppId"] = vApplicantionID;
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvLoanApp.Rows)
                {
                    if ((gr.RowIndex) % 2 == 0)
                    {
                        gr.BackColor = backColor;
                        gr.ForeColor = foreColor;
                    }
                    else
                    {
                        gr.BackColor = System.Drawing.Color.White;
                        gr.ForeColor = foreColor;
                    }
                    gr.Font.Bold = false;
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
                ShowInitialLoanAppData(vApplicantionID, vBrCode);
            }
            else if (e.CommandName == "cmdShowInfo")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShowInfo");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvLoanApp.Rows)
                {
                    if ((gr.RowIndex) % 2 == 0)
                    {
                        gr.BackColor = backColor;
                        gr.ForeColor = foreColor;
                    }
                    else
                    {
                        gr.BackColor = System.Drawing.Color.White;
                        gr.ForeColor = foreColor;
                    }
                    gr.Font.Bold = false;
                    LinkButton lb = (LinkButton)gr.FindControl("btnShowInfo");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
                ShowLoanRelationDetails(vApplicantionID);
            }
        }
        private void GetRatio(string pLoanAppId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CMember Omem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = Omem.GetRatio(pLoanAppId);
                if (dt.Rows.Count > 0)
                {
                    gvRatio.DataSource = dt;
                    gvRatio.DataBind();
                }
                else
                {
                    gvRatio.DataSource = null;
                    gvRatio.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Omem = null;
                dt = null;
            }
        }
        double totDebit = 0;
        double totCredit = 0;
        double totTrans = 0;
        double totSales = 0;
        double TotMOS = 0;
        double AvgMOS = 0;
        double NoofRecord = 0;
        double TotCreditSum = 0;
        double TotAvgUlilization = 0;
        double AvgAvgUlilization = 0;
        protected void gvBankDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // SetTabIndexes();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtDebit = (TextBox)e.Row.FindControl("txtDebit");
                if (txtDebit.Text != "")
                    totDebit += Double.Parse(txtDebit.Text);
                TextBox txtCredit = (TextBox)e.Row.FindControl("txtCredit");
                if (txtCredit.Text != "")
                    totCredit += Double.Parse(txtCredit.Text);
                TextBox txtTransfer = (TextBox)e.Row.FindControl("txtTransfer");
                if (txtTransfer.Text != "")
                    totTrans += Double.Parse(txtTransfer.Text);
                TextBox txtSalesFig = (TextBox)e.Row.FindControl("txtSalesFig");
                if (txtSalesFig.Text != "")
                    totSales += Double.Parse(txtSalesFig.Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotDebit = (Label)e.Row.FindControl("lblTotDebit");
                lblTotDebit.Text = totDebit.ToString();
                Label lblTotCredit = (Label)e.Row.FindControl("lblTotCredit");
                lblTotCredit.Text = totCredit.ToString();
                Label lblTotTrans = (Label)e.Row.FindControl("lblTotTrans");
                lblTotTrans.Text = totTrans.ToString();
                Label lblTotSales = (Label)e.Row.FindControl("lblTotSales");
                lblTotSales.Text = totSales.ToString();
            }
        }
        protected void gvAcDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // SetTabIndexes();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDebit = (Label)e.Row.FindControl("lblDebit");
                if (lblDebit.Text != "")
                    totDebit += Double.Parse(lblDebit.Text);
                Label lblCredit = (Label)e.Row.FindControl("lblCredit");
                if (lblCredit.Text != "")
                    totCredit += Double.Parse(lblCredit.Text);
                Label lblTransfer = (Label)e.Row.FindControl("lblTransfer");
                if (lblTransfer.Text != "")
                    totTrans += Double.Parse(lblTransfer.Text);
                Label lblSalesFig = (Label)e.Row.FindControl("lblSalesFig");
                if (lblSalesFig.Text != "")
                    totSales += Double.Parse(lblSalesFig.Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotDebit = (Label)e.Row.FindControl("lblTotDebit");
                lblTotDebit.Text = totDebit.ToString();
                Label lblTotCredit = (Label)e.Row.FindControl("lblTotCredit");
                lblTotCredit.Text = totCredit.ToString();
                Label lblTotTrans = (Label)e.Row.FindControl("lblTotTrans");
                lblTotTrans.Text = totTrans.ToString();
                Label lblTotSales = (Label)e.Row.FindControl("lblTotSales");
                lblTotSales.Text = totSales.ToString();
            }
        }
        protected void gvBankMOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (ViewState["Records"].ToString() != string.Empty)
                    NoofRecord = Convert.ToDouble(ViewState["Records"]);
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblMOS = (Label)e.Row.FindControl("lblPOS");
                    Label lblCreditSum = (Label)e.Row.FindControl("lblCreditSum");
                    Label lblAvgUtili = (Label)e.Row.FindControl("lblAvgUtili");
                    if (lblMOS.Text != "")
                        TotMOS += Double.Parse(lblMOS.Text);
                    if (lblCreditSum.Text != "")
                        TotCreditSum += Double.Parse(lblCreditSum.Text);
                    if (lblAvgUtili.Text != "")
                        TotAvgUlilization += Double.Parse(lblAvgUtili.Text);
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (ViewState["Records"].ToString() != string.Empty)
                    {
                        // Calculation of average MOS Accounwise
                        AvgMOS = Math.Round((TotMOS / NoofRecord), 2);
                        Label lbAveragePOS = (Label)e.Row.FindControl("lbAveragePOS");
                        lbAveragePOS.Text = AvgMOS.ToString("0.00");

                        // Calculation of Total Credit Summation Accounwise
                        Label lblTotCreditSum = (Label)e.Row.FindControl("lblTotCreditSum");
                        lblTotCreditSum.Text = TotCreditSum.ToString("0.00");

                        // Calculation of Average of Average Utilization

                        AvgAvgUlilization = Math.Round((TotAvgUlilization / NoofRecord), 2);
                        Label lblAvgAvgUtili = (Label)e.Row.FindControl("lblAvgAvgUtili");
                        lblAvgAvgUtili.Text = AvgAvgUlilization.ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SetTabIndexes()
        {
            short currentTabIndex = 0;
            //txtBankAcRemarks.TabIndex = ++currentTabIndex;

            foreach (GridViewRow gvr in gvBankDtl.Rows)
            {
                TextBox txtDate = (TextBox)gvr.FindControl("txtDate");
                txtDate.TabIndex = ++currentTabIndex;

                TextBox txtCalDebit = (TextBox)gvr.FindControl("txtCalDebit");
                txtCalDebit.TabIndex = ++currentTabIndex;

                TextBox txtCalCredit = (TextBox)gvr.FindControl("txtCalCredit");
                txtCalCredit.TabIndex = ++currentTabIndex;

                TextBox txtTransfer = (TextBox)gvr.FindControl("txtTransfer");
                txtTransfer.TabIndex = ++currentTabIndex;

                TextBox txtSalesFig = (TextBox)gvr.FindControl("txtSalesFig");
                txtSalesFig.TabIndex = ++currentTabIndex;

                TextBox txtLoanAvailed = (TextBox)gvr.FindControl("txtLoanAvailed");
                txtLoanAvailed.TabIndex = ++currentTabIndex;


                TextBox txtDay5 = (TextBox)gvr.FindControl("txtDay5");
                txtDay5.TabIndex = ++currentTabIndex;

                TextBox txtDay10 = (TextBox)gvr.FindControl("txtDay10");
                txtDay10.TabIndex = ++currentTabIndex;
                TextBox txtDay15 = (TextBox)gvr.FindControl("txtDay15");
                txtDay15.TabIndex = ++currentTabIndex;
                TextBox txtDay20 = (TextBox)gvr.FindControl("txtDay20");
                txtDay20.TabIndex = ++currentTabIndex;
                TextBox txtDay25 = (TextBox)gvr.FindControl("txtDay25");
                txtDay25.TabIndex = ++currentTabIndex;
            }

            //someLinkAfterGridView.TabIndex = ++currentTabIndex;
        }

        double liabilty = 0;
        double asset = 0;
        double income = 0;
        double expense = 0;
        double netproamt = 0;

        protected void gvLiability_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txt = (TextBox)e.Row.FindControl("txtAmt");
                Label id = (Label)e.Row.FindControl("lblParticularsId");
                int p = Convert.ToInt32(id.Text);
                if (p == 5 || p == 6)
                {
                    if (txt.Text == "")
                        liabilty = liabilty + 0;
                    else
                        liabilty = liabilty - Convert.ToDouble(txt.Text);
                }
                else
                {
                    if (txt.Text == "")
                        liabilty = liabilty + 0;
                    else
                        liabilty = liabilty + Convert.ToDouble(txt.Text);
                }

            }
            txtTotLiability.Text = liabilty.ToString("0.00");
        }
        protected void gvAsset_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txt = (TextBox)e.Row.FindControl("txtAssetAmt");
                Label id = (Label)e.Row.FindControl("lblParticularsId");
                int p = Convert.ToInt32(id.Text);

                if (txt.Text == "")
                    asset = asset + 0;
                else
                    asset = asset + Convert.ToDouble(txt.Text);
            }
            txtTotAsset.Text = asset.ToString("0.00");
        }

        protected void gvIncome_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txt = (TextBox)e.Row.FindControl("txtIncome");
                Label id = (Label)e.Row.FindControl("lblParticularsIdPL");
                int p = Convert.ToInt32(id.Text);
                if (p == 8)
                {
                    if (txt.Text != "")
                        income = income - Convert.ToDouble(txt.Text);
                }
                else
                {
                    if (txt.Text == "")
                        income = income + 0;
                    else
                        income = income + Convert.ToDouble(txt.Text);
                }
            }
            txtTotIncome.Text = income.ToString("0.00");
            if (txtTotExpense.Text != "")
                expense = Convert.ToDouble(txtTotExpense.Text);
            netproamt = (income - expense);
            txtNetProfit.Text = netproamt.ToString("0.00");
        }
        protected void gvExpense_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txt = (TextBox)e.Row.FindControl("txtExpense");
                Label id = (Label)e.Row.FindControl("lblParticularsIdPL");
                int p = Convert.ToInt32(id.Text);
                if (txt.Text == "")
                    expense = expense + 0;
                else
                    expense = expense + Convert.ToDouble(txt.Text);
            }
            txtTotExpense.Text = expense.ToString("0.00");
            if (txtTotIncome.Text != "")
                income = Convert.ToDouble(txtTotIncome.Text);
            netproamt = (income - expense);
            txtNetProfit.Text = netproamt.ToString("0.00");
        }
        protected void btnModifyPL_Click(object sender, EventArgs e)
        {
            CMember oMem = new CMember();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            try
            {
                if (txtModPLDate.Text == "")
                {
                    gblFuction.AjxMsgPopup("Profit/Loss Date can not be left blank...");
                    return;
                }
                else
                {
                    string StrDD, StrMM, StrYYYY;
                    string pDate = txtModPLDate.Text.ToString();
                    StrDD = pDate.Substring(0, 2);
                    StrMM = pDate.Substring(3, 2);
                    StrYYYY = pDate.Substring(6, 4);
                    if (StrDD != "31")
                    {
                        gblFuction.AjxMsgPopup("Day Part of selected date should be 31...");
                        return;
                    }
                    else if (StrMM != "03")
                    {
                        gblFuction.AjxMsgPopup("Month Part of selected date should be 03...");
                        return;
                    }
                }
                string vAppID = ViewState["LoanAppId"].ToString();
                DateTime vPLDate = gblFuction.setDate(txtModPLDate.Text);

                ds = oMem.GetPLDtlbyPLId(vAppID, vPLDate);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    dt2 = ds.Tables[2];
                    dt3 = ds.Tables[3];
                }
                if (dt.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 5;
                    ViewAcess();
                    hfPLId.Value = Convert.ToString(dt.Rows[0]["PLId"]).Trim();
                    btnSavePL.Enabled = false;
                    btnUpdatePL.Enabled = true;
                    btnDelPL.Enabled = true;
                    ViewState["StateEdit"] = "Edit";
                    txtAppIdPL.Text = Convert.ToString(dt.Rows[0]["AppId"]).Trim();
                    txtPLDate.Text = Convert.ToString(dt.Rows[0]["PLDate"]).Trim();
                    gvIncome.DataSource = dt;
                    gvIncome.DataBind();
                    txtModPLDate.Text = "";
                }
                if (dt1.Rows.Count > 0)
                {
                    gvExpense.DataSource = dt1;
                    gvExpense.DataBind();
                }
                else
                {
                    gvExpense.DataSource = null;
                    gvExpense.DataBind();
                }
                if (dt2.Rows.Count > 0)
                {
                    hfPLBSDocUpId.Value = dt2.Rows[0]["DocUpID"].ToString();
                }
                if (dt3.Rows.Count > 0)
                {
                    gvITR.DataSource = dt3;
                    gvITR.DataBind();
                    ViewState["ITRDtl"] = dt3;
                }
                else
                {
                    gvITR.DataSource = null;
                    gvITR.DataBind();
                    popITRDetails();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMem = null;
                ds = null;
                dt = null;
                dt1 = null;
                dt2 = null;
                dt3 = null;
            }
        }
        protected void btnModifyBS_Click(object sender, EventArgs e)
        {
            if (txtModBSData.Text == "")
            {
                gblFuction.AjxMsgPopup("Balance Sheet Date can not be left blank...");
                return;
            }
            else
            {
                string StrDD, StrMM, StrYYYY;
                string pDate = txtModBSData.Text.ToString();
                StrDD = pDate.Substring(0, 2);
                StrMM = pDate.Substring(3, 2);
                StrYYYY = pDate.Substring(6, 4);
                if (StrDD != "31")
                {
                    gblFuction.AjxMsgPopup("Day Part of selected date should be 31...");
                    return;
                }
                else if (StrMM != "03")
                {
                    gblFuction.AjxMsgPopup("Month Part of selected date should be 03...");
                    return;
                }
            }
            string vAppID = ViewState["LoanAppId"].ToString();
            DateTime vBSDate = gblFuction.setDate(txtModBSData.Text);
            CMember oMem = new CMember();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            ds = oMem.GetBSDtlbyBSid(vAppID, vBSDate);
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
            }
            if (dt.Rows.Count > 0)
            {
                mView.ActiveViewIndex = 4;
                ViewAcess();
                hfBSId.Value = Convert.ToString(dt.Rows[0]["BSId"]).Trim();
                btnSaveBS.Enabled = false;
                btnUpdateBS.Enabled = true;
                btnDelBS.Enabled = true;
                ViewState["StateEdit"] = "Edit";
                txtAppNoBS.Text = Convert.ToString(dt.Rows[0]["AppId"]).Trim();
                txtBSDate.Text = Convert.ToString(dt.Rows[0]["BSDate"]).Trim();
                gvLiability.DataSource = dt;
                gvLiability.DataBind();
                txtModBSData.Text = "";
            }
            if (dt1.Rows.Count > 0)
            {
                gvAsset.DataSource = dt1;
                gvAsset.DataBind();
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnNewApplication_Click(object sender, EventArgs e)
        {
            if (ViewState["CusTID"] == null)
            {
                gblFuction.MsgPopup("Please Selct Customer to Apply Loan");
                return;
            }
            else
            {
                ClearApplication();
                mView.ActiveViewIndex = 1;
                txtAppDt.Text = Session[gblValue.LoginDate].ToString();
                txtLnAppPassDt.Text = Session[gblValue.LoginDate].ToString();
                ViewAcess();
                PopApplicant();
                ddlLoanApplicantname.SelectedValue = ViewState["CusTID"].ToString();
                ddlLoanApplicantname_SelectedIndexChanged(sender, e);
                //  ddlLoanApplicantname.Enabled = false;
                popSourceName();
                PopPurpose();
                PopLoanType();
                popMLAsset();
                btSaveApplication.Enabled = true;
                btnUpdateApplication.Enabled = false;
            }
        }
        protected void btnNewRef_Click(object sender, EventArgs e)
        {
            if (ViewState["CusTID"] == null)
            {
                gblFuction.MsgPopup("Please Selct Customer to add Reference");
                return;
            }
            else
            {
                DataTable dt = new DataTable();
                CMember oMem = new CMember();
                dt = oMem.GetLoanAppNoByCustid(ViewState["CusTID"].ToString());
                if (dt.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 8;
                    ViewAcess();
                    ViewState["LoanAppId"] = dt.Rows[0]["LoanAppId"].ToString();
                    txtAppNoRef.Text = ViewState["LoanAppId"].ToString();
                    ClearRefControl();
                    PopRelation();
                    btnSaveRef.Enabled = true;
                    btnUpdateRef.Enabled = false;
                    //btnDelRef.Enabled = false;
                }
                else
                {
                    gblFuction.MsgPopup("Please Complete Loan Application To Add Reference Details...");
                }
            }
        }
        protected void btnAddCB_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            try
            {
                if (ViewState["LoanAppId"] == null)
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to add Credit Bureau Information..");
                    return;
                }
                else
                {
                    dt = oMem.GetLnAppPassYNByLoanid(ViewState["LoanAppId"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["LnAppPassYN"].ToString() != "Y")
                        {
                            gblFuction.MsgPopup("You Can Not Add CB Record as This Loan Application remains in Hold/Rejected/Cancelled State in  Loan Application Stage...");
                            return;
                        }
                        mView.ActiveViewIndex = 2;
                        ViewAcess();
                        txtLnAppCB.Text = ViewState["LoanAppId"].ToString();
                        popCBDetails();
                        PopCBName();
                        btnUpdateCB.Enabled = false;
                        btnDeleteCB.Enabled = false;
                        btnSaveCB.Enabled = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup("Please Select Pass in Loan Application Approval Section...");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        protected void btnAddBankAc_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable dtCBPass = new DataTable();
            DataTable dtSlNo = new DataTable();
            Int32 MaxSlNo = 0;
            CMember oMem = new CMember();
            try
            {
                if (ViewState["LoanAppId"] == null)
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to add Bank Account");
                    return;
                }
                else
                {

                    txtAppIdBankAc.Text = ViewState["LoanAppId"].ToString();
                    dtCBPass = oMem.GetCBPassByLoanId(ViewState["LoanAppId"].ToString());
                    if (dtCBPass.Rows.Count > 0)
                    {
                        if (dtCBPass.Rows[0]["CBPassYN"].ToString() == "N")
                        {
                            gblFuction.MsgPopup("You Can Not Add Bank Statement Record as Credit Bureau remains in Rejected State ...");
                            return;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("Please tick CB Pass/Reject Checkbox to add Bank Statement Record... ");
                        return;
                    }
                    dtSlNo = oMem.GetMaxBankAcSlNo(ViewState["LoanAppId"].ToString());
                    if (dtSlNo.Rows.Count > 0)
                    {
                        MaxSlNo = Convert.ToInt32(dtSlNo.Rows[0]["MaxSlNo"]) + 1;
                    }
                    txtBankAcSrNo.Text = MaxSlNo.ToString();
                    mView.ActiveViewIndex = 3;
                    ViewAcess();
                    btnBankAcUpdate.Enabled = false;
                    btnDeleteBankAc.Enabled = false;
                    btnBankAcSave.Enabled = true;
                    popBankDetails();
                    popBankIWOWDetails();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                dtCBPass = null;
                oMem = null;
            }
        }
        //protected void GetLoanAppDtlByLnAppId(string LoanID)
        //{
        //    CApplication OApp = new CApplication();
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        dt = OApp.GetLoanAppDtlLoanId(LoanID);
        //        if (dt.Rows.Count > 0)
        //        {
        //            PopPurpose();
        //            PopLoanType();
        //            ddlLnPur.SelectedIndex = ddlLnPurpose.Items.IndexOf(ddlLnPurpose.Items.FindByValue(Convert.ToString(dt.Rows[0]["PurposeID"])));
        //            ddlCAMLnScheme.SelectedIndex = ddlCAMLnScheme.Items.IndexOf(ddlCAMLnScheme.Items.FindByValue(Convert.ToString(dt.Rows[0]["LoanTypeId"])));
        //            AssementMatrixEnable(Convert.ToInt32(dt.Rows[0]["LoanTypeId"]));
        //            txtLnAmt.Text = dt.Rows[0]["AppAmount"].ToString();
        //            txtAppTenure.Text = dt.Rows[0]["Tenure"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        OApp = null;
        //        dt = null;
        //    }
        //}
        protected void GetCBRemarkByLnAppId(string LoanID)
        {
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = OMem.GetCBRemarksByLoanId(LoanID);
                if (dt.Rows.Count > 0)
                {
                    hfCBRemarkID.Value = dt.Rows[0]["RemarksID"].ToString();
                    txtCBPOsRemark.Text = dt.Rows[0]["Positive"].ToString();
                    txtCBNegRemark.Text = dt.Rows[0]["Negetive"].ToString();
                    txtCBMiti.Text = dt.Rows[0]["Mitigation"].ToString();
                    txtCIBILScore.Text = dt.Rows[0]["CIBILScore"].ToString();
                    txtCIBILScoreDate.Text = dt.Rows[0]["CIBILScoreDate"].ToString();
                    if (dt.Rows[0]["CBPassYN"].ToString() == "Y")
                        chkCBPass.Checked = true;
                    else
                        chkCBPass.Checked = false;
                    txtCBPassRejDate.Text = dt.Rows[0]["CBPassorRejDate"].ToString();
                    txtCBRejReason.Text = dt.Rows[0]["CBRejReason"].ToString();
                    btnSaveCBRemark.Enabled = false;
                    btnUpdateCBRemark.Enabled = true;
                }
                else
                {
                    hfCBRemarkID.Value = "";
                    txtCBPOsRemark.Text = "";
                    txtCBNegRemark.Text = "";
                    txtCBMiti.Text = "";
                    txtCIBILScore.Text = "";
                    chkCBPass.Checked = false;
                    txtCBPassRejDate.Text = "";
                    txtCBRejReason.Text = "";
                    btnSaveCBRemark.Enabled = true;
                    btnUpdateCBRemark.Enabled = false;
                    chkCBPass.Checked = false;
                    txtCBPassRejDate.Text = "";
                    txtCBRejReason.Text = "";
                    txtCIBILScoreDate.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
                dt = null;
            }
        }
        protected void GetBSRemarkByLnAppId(string LoanID)
        {
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = OMem.GetBSRemarksByLoanId(LoanID);
                if (dt.Rows.Count > 0)
                {
                    hfBSRemarkID.Value = dt.Rows[0]["RemarksID"].ToString();
                    txtBSPosRemark.Text = dt.Rows[0]["Positive"].ToString();
                    txtBSNegRemark.Text = dt.Rows[0]["Negetive"].ToString();
                    txtBSMiti.Text = dt.Rows[0]["Mitigation"].ToString();
                    if (dt.Rows[0]["BSPassYN"].ToString() == "Y")
                        chkBSPass.Checked = true;
                    else
                        chkBSPass.Checked = false;
                    txtBSPassRejDate.Text = dt.Rows[0]["BSPassorRejDate"].ToString();
                    txtBSRejReason.Text = dt.Rows[0]["BSRejReason"].ToString();
                    btnSaveBSRemark.Enabled = false;
                    btnUpdateBSRemark.Enabled = true;
                }
                else
                {
                    hfBSRemarkID.Value = "";
                    txtBSPosRemark.Text = "";
                    txtBSNegRemark.Text = "";
                    txtBSMiti.Text = "";
                    chkBSPass.Checked = true;
                    txtBSPassRejDate.Text = "";
                    txtBSRejReason.Text = "";
                    btnSaveBSRemark.Enabled = true;
                    btnUpdateBSRemark.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
                dt = null;
            }
        }
        protected void GetFSRemarkByLnAppId(string LoanID)
        {
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = OMem.GetFSRemarksByLoanId(LoanID);
                if (dt.Rows.Count > 0)
                {
                    hfFSRemarkID.Value = dt.Rows[0]["RemarksID"].ToString();
                    txtFSPosRemark.Text = dt.Rows[0]["Positive"].ToString();
                    txtFSNegRemark.Text = dt.Rows[0]["Negetive"].ToString();
                    txtFSMiti.Text = dt.Rows[0]["Mitigation"].ToString();
                    if (dt.Rows[0]["FSPassYN"].ToString() == "Y")
                        chkFSPass.Checked = true;
                    else
                        chkFSPass.Checked = false;
                    txtFSPassRejDate.Text = dt.Rows[0]["FSPassorRejDate"].ToString();
                    txtFSRejReason.Text = dt.Rows[0]["FSRejReason"].ToString();
                    btnSaveFSRemark.Enabled = false;
                    btnUpdateFSRemark.Enabled = true;
                }
                else
                {
                    hfFSRemarkID.Value = "";
                    txtFSPosRemark.Text = "";
                    txtFSNegRemark.Text = "";
                    txtFSMiti.Text = "";
                    txtFSPassRejDate.Text = "";
                    txtFSRejReason.Text = "";
                    chkFSPass.Checked = false;
                    btnSaveFSRemark.Enabled = true;
                    btnUpdateFSRemark.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
                dt = null;
            }
        }
        protected void btnAddRef_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["RefDtl"];
            DataRow dr = dt.NewRow();
            string vBrCode = Session[gblValue.BrnchCode].ToString();

            Int32 vRelWithAppId = 0;
            string vLnAppId = "", vRefName = "", vRefAddr = "", vRefMNo = "", vOffTelNo = "", vRelWithApp = "", vCompName = "", vPersonalRef = "",
                vTradeRef = "", vRefType = "", vStatus = "", vVarifiedBy = "";

            vLnAppId = Convert.ToString((Request[txtAppNoRef.UniqueID] as string == null) ? txtAppNoRef.Text : Request[txtAppNoRef.UniqueID] as string);
            vRefName = Convert.ToString((Request[txtRefName.UniqueID] as string == null) ? txtRefName.Text : Request[txtRefName.UniqueID] as string);
            vRefAddr = Convert.ToString((Request[txtRefAddr.UniqueID] as string == null) ? txtRefAddr.Text : Request[txtRefAddr.UniqueID] as string);
            vRefMNo = Convert.ToString((Request[txtRefMNo.UniqueID] as string == null) ? txtRefMNo.Text : Request[txtRefMNo.UniqueID] as string);
            vOffTelNo = Convert.ToString((Request[txtOffTelNoRef.UniqueID] as string == null) ? txtOffTelNoRef.Text : Request[txtOffTelNoRef.UniqueID] as string);
            vCompName = Convert.ToString((Request[txtCompNameRef.UniqueID] as string == null) ? txtCompNameRef.Text : Request[txtCompNameRef.UniqueID] as string);
            vRelWithApp = Convert.ToString((Request[ddlRelWApp.SelectedItem.Text] as string == null) ? ddlRelWApp.SelectedItem.Text : Request[ddlRelWApp.SelectedItem.Text] as string);
            if (ddlRelWApp.SelectedIndex != -1)
                vRelWithAppId = Convert.ToInt32((Request[ddlRelWApp.UniqueID] as string == null) ? ddlRelWApp.SelectedValue : Request[ddlRelWApp.UniqueID] as string);
            if (rblRefType.SelectedValue == "P")
            {
                vPersonalRef = "Y";
                vTradeRef = "N";
                vRefType = "Personal";
            }
            else
            {
                vPersonalRef = "N";
                vTradeRef = "Y";
                vRefType = "Trade";
            }
            vStatus = Convert.ToString((Request[txtStatus.UniqueID] as string == null) ? txtStatus.Text : Request[txtStatus.UniqueID] as string);
            vVarifiedBy = Convert.ToString((Request[txtVarifiedby.UniqueID] as string == null) ? txtVarifiedby.Text : Request[txtVarifiedby.UniqueID] as string);
            dr[0] = vLnAppId;//dr[ApplicationId]
            dr[1] = vRefName;//dr[RefName]
            dr[2] = vRefAddr;//dr[RefAddr]
            dr[3] = vRefMNo;//dr[RefMNo]
            dr[4] = vRelWithAppId;//dr[RelWithAppId]
            dr[5] = vRelWithApp;//dr[RelWithApp]
            dr[6] = vOffTelNo;//dr[OffTelNo]
            dr[7] = vCompName;//dr[CompName]
            dr[8] = vPersonalRef;//dr[PersonalRef]
            dr[9] = vTradeRef;//dr[TradeRef]
            dr[10] = vRefType;//dr[TradeRef]
            dr[11] = vStatus;//dr[Status]
            dr[12] = vVarifiedBy;//dr[VarifiedBy]
            dt.Rows.Add(dr);
            dt.AcceptChanges();

            ViewState["RefDtl"] = dt;
            if (dt.Rows.Count > 0)
            {
                gvRef.DataSource = dt;
                gvRef.DataBind();
                ClearRefControl();
            }
        }
        protected void btnAddFinStat_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable dtBSPass = new DataTable();
            CMember oMem = new CMember();
            try
            {
                if (ViewState["LoanAppId"] == null)
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to add Financial Data");
                    return;
                }
                else
                {
                    txtAppNoBS.Text = ViewState["LoanAppId"].ToString();
                    dtBSPass = oMem.GetBSPassByLoanId(ViewState["LoanAppId"].ToString());
                    if (dtBSPass.Rows.Count > 0)
                    {
                        if (dtBSPass.Rows[0]["BSPassYN"].ToString() == "N")
                        {
                            gblFuction.MsgPopup("You Can Not Add Financial Statement as Bank Statement remains in Rejected State ...");
                            return;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("Please tick Bank Statement Pass/Reject Checkbox to add Financial Statement... ");
                        return;
                    }
                    mView.ActiveViewIndex = 4;
                    ViewAcess();
                    ClearBSControl();
                    GetComGLForBalShet();
                    //  popBSDetails();
                    // PopCompanyGL();
                    btnSaveBS.Enabled = true;
                    btnUpdateBS.Enabled = false;
                    btnDelBS.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                dtBSPass = null;
                oMem = null;
            }
        }
        protected void btnAddPLStat_Click(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();
            DataTable dtBSPass = new DataTable();
            CMember oMem = new CMember();
            try
            {
                if (ViewState["LoanAppId"] == null)
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to add Financial Data");
                    return;
                }
                else
                {
                    txtAppIdPL.Text = ViewState["LoanAppId"].ToString();
                    dtBSPass = oMem.GetBSPassByLoanId(ViewState["LoanAppId"].ToString());
                    if (dtBSPass.Rows.Count > 0)
                    {
                        if (dtBSPass.Rows[0]["BSPassYN"].ToString() == "N")
                        {
                            gblFuction.MsgPopup("You Can Not Add PL Statement as Bank Statement remains in Rejected State ...");
                            return;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("Please tick Bank Statement Pass/Reject Checkbox to add PL Statement... ");
                        return;
                    }
                    mView.ActiveViewIndex = 5;
                    ViewAcess();
                    ClearPLControl();
                    GetComGLForPL();

                    // PopCompPLLed();
                    btnSavePL.Enabled = true;
                    btnUpdatePL.Enabled = false;
                    btnDelPL.Enabled = false;

                    dt1 = oMem.GetITRDetailsByLoanId(ViewState["LoanAppId"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        gvITR.DataSource = dt1;
                        gvITR.DataBind();
                    }
                    else
                    {
                        popITRDetails();
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
                dtBSPass = null;
                oMem = null;
            }

        }
        protected void btnBackApplication_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 0;
            ViewAcess();
        }

        protected void btnSaveCB_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";
            if (SaveCBRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 1;
                ViewAcess();
                LoadCBList(ViewState["LoanAppId"].ToString());
                gvCBDtl.DataSource = null;
                gvCBDtl.DataBind();
                ViewState["CBDtl"] = null;
                popCBDetails();
            }
        }
        protected void btnUpdateCB_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";
            if (SaveCBRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 2;
                ViewAcess();
                LoadCBList(ViewState["LoanAppId"].ToString());
                gvCBDtl.DataSource = null;
                gvCBDtl.DataBind();
                ViewState["CBDtl"] = null;
                popCBDetails();
                hdfCBId.Value = "";
            }
        }
        protected void btnDeleteCB_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";
            if (SaveCBRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 2;
                ViewAcess();
                LoadCBList(ViewState["LoanAppId"].ToString());
                gvCBDtl.DataSource = null;
                gvCBDtl.DataBind();
                ViewState["CBDtl"] = null;
                popCBDetails();
                hdfCBId.Value = "";
            }
        }
        protected void btnBackCB_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 2;
            ViewAcess();
            hdfCBId.Value = "";
        }
        protected void btnBackBS_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 4;
            ViewAcess();
            LoadBSList(ViewState["LoanAppId"].ToString());
            LoadPLList(ViewState["LoanAppId"].ToString());
        }
        protected void btnBackPL_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 4;
            ViewAcess();
            ClearPLControl();
            LoadBSList(ViewState["LoanAppId"].ToString());
            LoadPLList(ViewState["LoanAppId"].ToString());
        }
        protected void btnSaveBS_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";
            if (SaveBSRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 4;
                ViewAcess();
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
                gvLiability.DataSource = null;
                gvLiability.DataBind();
                gvAsset.DataSource = null;
                gvAsset.DataBind();
                ViewState["BSDtl"] = null;
                popBSDetails();
            }
        }
        protected void btnUpdateBS_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";
            if (SaveBSRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 4;
                ViewAcess();
                //  LoadCBList();
                gvLiability.DataSource = null;
                gvLiability.DataBind();
                gvAsset.DataSource = null;
                gvAsset.DataBind();
                ViewState["BSDtl"] = null;
                //LoadBSDetails(ViewState["LoanAppId"].ToString());
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnDelBS_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";
            if (SaveBSRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 4;
                ViewAcess();
                gvLiability.DataSource = null;
                gvLiability.DataBind();
                gvAsset.DataSource = null;
                gvAsset.DataBind();
                ViewState["BSDtl"] = null;
                //LoadBSDetails(ViewState["LoanAppId"].ToString());
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());

            }
        }
        protected void btnSavePL_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";
            if (SavePLRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 4;
                ViewAcess();
                // LoadPLDetails(ViewState["LoanAppId"].ToString());
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
                gvIncome.DataSource = null;
                gvIncome.DataBind();
                ViewState["PLDtl"] = null;
                popPLDetails();
                ClearPLControl();
                GetBankToTR(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnUpdatePL_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";
            if (SavePLRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 4;
                ViewAcess();
                //  LoadCBList();
                gvIncome.DataSource = null;
                gvIncome.DataBind();
                ViewState["PLDtl"] = null;
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
                ClearPLControl();
                GetBankToTR(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnDelPL_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";
            if (SavePLRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 4;
                ViewAcess();
                gvIncome.DataSource = null;
                gvIncome.DataBind();
                ViewState["PLDtl"] = null;
                LoadPLList(ViewState["LoanAppId"].ToString());
                GetBankToTR(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnSaveCBRemark_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Add Credit Bureau Remarks..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Save";
                if (SaveCBRemarks(vStateEdit) == true)
                {
                    switch (vStateEdit)
                    {
                        case "Save":
                            lblMsg.Text = gblPRATAM.SaveMsg;
                            break;
                        case "Edit":
                            lblMsg.Text = gblPRATAM.EditMsg;
                            break;
                        case "Delete":
                            lblMsg.Text = gblPRATAM.DeleteMsg;
                            break;
                    }
                }
            }
        }
        protected void btnUpdateCBRemark_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Update Credit Bureau Remarks..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Edit";
                if (SaveCBRemarks(vStateEdit) == true)
                {
                    switch (vStateEdit)
                    {
                        case "Save":
                            lblMsg.Text = gblPRATAM.SaveMsg;
                            break;
                        case "Edit":
                            lblMsg.Text = gblPRATAM.EditMsg;
                            break;
                        case "Delete":
                            lblMsg.Text = gblPRATAM.DeleteMsg;
                            break;
                    }
                }
            }
        }
        protected void btnSaveBSRemark_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Add Bank Statement Remarks..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Save";
                if (SaveBSRemarks(vStateEdit) == true)
                {
                    switch (vStateEdit)
                    {
                        case "Save":
                            lblMsg.Text = gblPRATAM.SaveMsg;
                            break;
                        case "Edit":
                            lblMsg.Text = gblPRATAM.EditMsg;
                            break;
                        case "Delete":
                            lblMsg.Text = gblPRATAM.DeleteMsg;
                            break;
                    }
                }
            }
        }
        protected void btnUpdateBSRemark_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Update Bank Statement Remarks..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Edit";
                if (SaveBSRemarks(vStateEdit) == true)
                {
                    switch (vStateEdit)
                    {
                        case "Save":
                            lblMsg.Text = gblPRATAM.SaveMsg;
                            break;
                        case "Edit":
                            lblMsg.Text = gblPRATAM.EditMsg;
                            break;
                        case "Delete":
                            lblMsg.Text = gblPRATAM.DeleteMsg;
                            break;
                    }
                }
            }
        }
        protected void btnSaveFSRemark_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Add Financial Statement Remarks..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Save";
                if (SaveFSRemarks(vStateEdit) == true)
                {
                    switch (vStateEdit)
                    {
                        case "Save":
                            lblMsg.Text = gblPRATAM.SaveMsg;
                            break;
                        case "Edit":
                            lblMsg.Text = gblPRATAM.EditMsg;
                            break;
                        case "Delete":
                            lblMsg.Text = gblPRATAM.DeleteMsg;
                            break;
                    }
                }
            }
        }
        protected void btnUpdateFSRemark_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Update Financial Statement Remarks..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Edit";
                if (SaveFSRemarks(vStateEdit) == true)
                {
                    switch (vStateEdit)
                    {
                        case "Save":
                            lblMsg.Text = gblPRATAM.SaveMsg;
                            break;
                        case "Edit":
                            lblMsg.Text = gblPRATAM.EditMsg;
                            break;
                        case "Delete":
                            lblMsg.Text = gblPRATAM.DeleteMsg;
                            break;
                    }
                    LoadBSList(ViewState["LoanAppId"].ToString());
                    LoadPLList(ViewState["LoanAppId"].ToString());
                }
            }
        }
        protected void btnSaveRef_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";
            if (SaveRefRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 1;
                ViewAcess();
                gvRef.DataSource = null;
                gvRef.DataBind();
                ViewState["RefDtl"] = null;
                ClearRefControl();
            }
        }
        protected void btnUpdateRef_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";
            if (SaveRefRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 1;
                ViewAcess();
                gvRef.DataSource = null;
                gvRef.DataBind();
                ViewState["RefDtl"] = null;
                ClearRefControl();
            }
        }
        protected void btnDelRef_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";
            if (SaveRefRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 1;
                ViewAcess();
            }
        }
        protected void btnBackRef_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 1;
            ViewAcess();
        }
        protected void btnBankAcSave_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";
            if (SaveBankRecord(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 3;
                ViewAcess();
                LoadBankAcList(ViewState["LoanAppId"].ToString());
                ClearBankAccControl();
                popBankDetails();
                //  LoadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
                LoadBankAcSummary(ViewState["LoanAppId"].ToString());
                // Get Combine MOS
                GetCombineMOS(ViewState["LoanAppId"].ToString());
                // Get BankToTRRatio
                GetBankToTR(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnBankAcUpdate_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";
            if (SaveBankRecord(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                //  mView.ActiveViewIndex = 0;
                //  tbMem.ActiveTabIndex = 4;
                ViewAcess();
                LoadBankAcList(ViewState["LoanAppId"].ToString());
                LoadBankAcSummary(ViewState["LoanAppId"].ToString());

                // Get Combine MOS
                GetCombineMOS(ViewState["LoanAppId"].ToString());
                // Get BankToTRRatio
                GetBankToTR(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnBackBankAc_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 3;
            ViewAcess();
            LoadBankAcSummary(ViewState["LoanAppId"].ToString());

        }
        protected void btnDeleteBankAc_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";
            if (SaveBankRecord(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 3;
                ViewAcess();
                LoadBankAcList(ViewState["LoanAppId"].ToString());
                ClearBankAccControl();
                popBankDetails();
                LoadBankAcSummary(ViewState["LoanAppId"].ToString());
                // Get Combine MOS
                GetCombineMOS(ViewState["LoanAppId"].ToString());
                // Get BankToTRRatio
                GetBankToTR(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btSaveApplication_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";

            if (SaveLoanAppRecords(vStateEdit) == true)
            {
                if (vStateEdit == "Save")
                    lblMsg.Text = gblPRATAM.SaveMsg;
                else if (vStateEdit == "Edit")
                    lblMsg.Text = gblPRATAM.EditMsg;
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 0;
                ViewAcess();
                ClearApplication();
                LoadPendingPreCAMList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnUpdateApplication_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";

            if (SaveLoanAppRecords(vStateEdit) == true)
            {
                if (vStateEdit == "Save")
                    lblMsg.Text = gblPRATAM.SaveMsg;
                else if (vStateEdit == "Edit")
                    lblMsg.Text = gblPRATAM.EditMsg;
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 0;
                ViewAcess();
                ClearApplication();
                LoadPendingPreCAMList();
                //  LoadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnDeleteApplication_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";

            if (SaveLoanAppRecords(vStateEdit) == true)
            {
                if (vStateEdit == "Save")
                    lblMsg.Text = gblPRATAM.SaveMsg;
                else if (vStateEdit == "Edit")
                    lblMsg.Text = gblPRATAM.EditMsg;
                else if (vStateEdit == "Delete")
                    lblMsg.Text = gblPRATAM.DeleteMsg;
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 0;
                ViewAcess();
                ClearApplication();
                LoadPendingPreCAMList();
                //  LoadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        private Boolean SaveLoanAppRecords(string Mode)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean vResult = false;
            string vStateEdit = string.Empty, vBrCode = string.Empty, vAppNo = string.Empty, vApplicantId = "", vAppId = "", vMachDtl = "", vAddTerms = "",vErrDesc="";
            Int32 vErr = 0, vPurpId = 0, vTenure = 0, vYrNo = 0, vLnTypeId = 0, vSourceId = 0;
            decimal vLnAmt = 0;

            CApplication oCG = new CApplication();
            string vXml = "", vXmlAsset = "", vPassYN = "", vRejReason = "";
            try
            {
                if (txtAppDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Please Select Loan Application Date");
                    txtAppDt.Focus();
                    return false;
                }
                else if (txtLnAppPassDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Loan Application Pass/Rejection Date Can Not Be Blank...");
                    txtLnAppPassDt.Focus();
                    return false;
                }
                else if (ddlLoanApplicantname.SelectedValue == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Applicant Name...");
                    ddlLoanApplicantname.Focus();
                    return false;
                }
                else if (ddlLnPurpose.SelectedValue == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Loan Purpose...");
                    ddlLnPurpose.Focus();
                    return false;
                }
                else if (ddlLnScheme.SelectedValue == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Loan Type...");
                    ddlLnScheme.Focus();
                    return false;
                }
                else if (txtTenure.Text == "")
                {
                    gblFuction.AjxMsgPopup("Tenure Can Not Be Empty...");
                    txtTenure.Focus();
                    return false;
                }
                else if (txtAppLnAmt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Loan Amount Can Not Be Empty...");
                    txtAppLnAmt.Focus();
                    return false;
                }
                else
                {

                }
                DateTime vAppDt = gblFuction.setDate(txtAppDt.Text);
                DateTime vPassorRejDate = gblFuction.setDate(txtLnAppPassDt.Text);
                vLnTypeId = Convert.ToInt32(Request[ddlLnScheme.UniqueID] as string);

                DataRow dr = null;
                DataTable dtXml = new DataTable();
                dtXml.Columns.Add(new DataColumn("CoApplicantId"));
                dtXml.Columns.Add(new DataColumn("CoApplicantName"));
                dtXml.Columns.Add(new DataColumn("ReportID"));
                dtXml.Columns.Add(new DataColumn("ScoreValue"));
                foreach (GridViewRow gr in gvCoAppDtl.Rows)
                {
                    if (((CheckBox)gr.FindControl("chkCoApp")).Checked == true)
                    {
                        dr = dtXml.NewRow();
                        dr["CoApplicantId"] = ((Label)gr.FindControl("lblCoApplicantId")).Text;
                        dr["CoApplicantName"] = ((Label)gr.FindControl("lblCoAppName")).Text;
                        dr["ReportID"] = ((Label)gr.FindControl("lblReportID")).Text;
                        dr["ScoreValue"] = ((Label)gr.FindControl("lblScoreValue")).Text;
                        dtXml.Rows.Add(dr);
                        dtXml.AcceptChanges();
                    }
                }
                dtXml.TableName = "Table1";
                // In Case of Machinary Loan ,Machine Asset Details will be inserted .... 
                if (vLnTypeId == 2)
                {
                    DataRow drAsset = null;
                    DataTable dtXmlAsset = new DataTable();
                    dtXmlAsset.Columns.Add("SlNo", typeof(int));
                    dtXmlAsset.Columns.Add("MachDesc", typeof(string));
                    dtXmlAsset.Columns.Add("MachSupp", typeof(string));
                    dtXmlAsset.Columns.Add("Place", typeof(string));
                    dtXmlAsset.Columns.Add("Make", typeof(string));
                    dtXmlAsset.Columns.Add("Model", typeof(string));
                    dtXmlAsset.Columns.Add("Amount", typeof(decimal));

                    foreach (GridViewRow gr in gvMLAsset.Rows)
                    {
                        if (((TextBox)gr.FindControl("txtMachDesc")).Text != "" && ((TextBox)gr.FindControl("txtAmount")).Text != "")
                        {
                            drAsset = dtXmlAsset.NewRow();
                            drAsset["SlNo"] = ((Label)gr.FindControl("lblSLNoMLAsset")).Text;
                            drAsset["MachDesc"] = ((TextBox)gr.FindControl("txtMachDesc")).Text;
                            drAsset["MachSupp"] = ((TextBox)gr.FindControl("txtMachSupp")).Text;
                            drAsset["Place"] = ((TextBox)gr.FindControl("txtPlace")).Text;
                            drAsset["Make"] = ((TextBox)gr.FindControl("txtMake")).Text;
                            drAsset["Model"] = ((TextBox)gr.FindControl("txtModel")).Text;
                            drAsset["Amount"] = ((TextBox)gr.FindControl("txtAmount")).Text;
                            dtXmlAsset.Rows.Add(drAsset);
                            dtXmlAsset.AcceptChanges();
                        }
                    }
                    dtXmlAsset.TableName = "Table2";

                    using (StringWriter oSW = new StringWriter())
                    {
                        dtXmlAsset.WriteXml(oSW);
                        vXmlAsset = oSW.ToString().Replace("12:00:00AM", "").Trim();
                    }
                }


                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString().Replace("12:00:00AM", "").Trim();
                }

                vApplicantId = (Request[ddlLoanApplicantname.UniqueID] as string == null) ? ddlLoanApplicantname.SelectedValue : Request[ddlLoanApplicantname.UniqueID] as string;
                vPurpId = Convert.ToInt32(Request[ddlLnPurpose.UniqueID] as string);
                //if (chkLnAppPass.Checked == true)
                //    vPassYN = "Y";
                //else
                //    vPassYN = "N";
                vPassYN = (Request[ddlLnAppStatus.UniqueID] as string == null) ? ddlLnAppStatus.SelectedValue : Request[ddlLnAppStatus.UniqueID] as string;
                vRejReason = (Request[txtLnAppRejReason.UniqueID] as string == null) ? txtLnAppRejReason.Text : Request[txtLnAppRejReason.UniqueID] as string;
                vAddTerms = (Request[txtAddTerms.UniqueID] as string == null) ? txtAddTerms.Text : Request[txtAddTerms.UniqueID] as string;
                vSourceId = Convert.ToInt32(Request[ddlSourceName.UniqueID] as string);
                vYrNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
                if (Request[txtTenure.UniqueID] as string != "")
                    vTenure = Convert.ToInt32(Request[txtTenure.UniqueID] as string);
                if (vTenure == 0)
                {
                    gblFuction.AjxMsgPopup("Tenure Can Not Be zero...");
                    txtTenure.Focus();
                    return false;
                }
                if (txtAppLnAmt.Text.ToString() != "")
                    decimal.TryParse(txtAppLnAmt.Text.ToString(), out vLnAmt);
                if (vLnAmt == 0)
                {
                    gblFuction.AjxMsgPopup("Loan Amount Can Not Be zero...");
                    txtAppLnAmt.Focus();
                    return false;
                }
                vBrCode = (string)Session[gblValue.BrnchCode];
                vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                vAppId = hdfApplicationId.Value;
                vMachDtl = txtLnPurposeDetails.Text.ToString().Trim();


                if (Mode == "Save")
                {
                    //if (ValidateFieldsForLnApp() == false) return false;

                    vErr = oCG.SaveInitialApplication(ref vAppNo, vApplicantId, vAppDt, vPurpId, vLnAmt, vTenure, "N",
                      vBrCode, Convert.ToInt32(hdUserID.Value), "I", vYrNo, vLnTypeId, vMachDtl, vSourceId, vXml, vXmlAsset, vPassYN, vPassorRejDate,
                      vRejReason, vAddTerms, ref vErrDesc);
                    if (vErr == 0)
                    {
                        ViewState["AppId"] = vAppId;
                        txtAppNo.Text = vAppNo;
                        ViewState["LoanAppId"] = vAppNo;
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    // if (ValidateFieldsForLnApp() == false) return false;

                    oCG = new CApplication();
                    vErr = oCG.UpdateApplication(vAppId, vApplicantId, vAppDt, vPurpId, vLnAmt, vTenure,
                         vBrCode, Convert.ToInt32(hdUserID.Value), "Edit", vLnTypeId, vMachDtl, vSourceId, vXml, vXmlAsset, vPassYN, vPassorRejDate,
                         vRejReason, vAddTerms,ref vErrDesc);
                    if (vErr == 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    //oCG = new CApplication();
                    //vErr = oCG.ChkEditApplication(vAppId, vMemId, vBrCode);
                    //if (vErr > 0)
                    //{
                    //    gblFuction.MsgPopup("Approved or Cancelled Application cannot be Deleted.");
                    //    return false;
                    //}
                    //else
                    //{
                    vErr = oCG.UpdateApplication(vAppId, vApplicantId, vAppDt, vPurpId, vLnAmt, vTenure,
                        vBrCode, Convert.ToInt32(hdUserID.Value), "Delete", vLnTypeId, vMachDtl, vSourceId, vXml, vXmlAsset, vPassYN, vPassorRejDate,
                        vRejReason, vAddTerms,ref vErrDesc);
                    if (vErr == 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                    //}
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCG = null;
            }
        }
        private Boolean SaveCBRecords(string Mode)
        {
            Boolean vResult = false;
            DataRow dr = null;
            DataTable dtXml = new DataTable();

            dtXml.Columns.Add("SLNo", typeof(int));
            dtXml.Columns.Add("ReportId", typeof(string));
            dtXml.Columns.Add("ReportDate", typeof(string));
            dtXml.Columns.Add("CBNameId", typeof(int));
            dtXml.Columns.Add("OrgName", typeof(string));
            dtXml.Columns.Add("LoanType", typeof(string));
            dtXml.Columns.Add("Active", typeof(string));
            dtXml.Columns.Add("BankValName", typeof(string));
            dtXml.Columns.Add("LoanAmt", typeof(float));
            dtXml.Columns.Add("POSAmt", typeof(float));
            dtXml.Columns.Add("LoanDate", typeof(string));
            dtXml.Columns.Add("EMI", typeof(float));
            dtXml.Columns.Add("Tenure", typeof(int));
            dtXml.Columns.Add("EMIPaid", typeof(float));
            dtXml.Columns.Add("PMn1", typeof(string));
            dtXml.Columns.Add("PMn2", typeof(string));
            dtXml.Columns.Add("PMn3", typeof(string));
            dtXml.Columns.Add("PMn4", typeof(string));
            dtXml.Columns.Add("PMn5", typeof(string));
            dtXml.Columns.Add("PMn6", typeof(string));
            dtXml.Columns.Add("PMn7", typeof(string));
            dtXml.Columns.Add("Remarks", typeof(string));

            foreach (GridViewRow gr in gvCBDtl.Rows)
            {
                if (((TextBox)gr.FindControl("txtReportId")).Text != "")
                {
                    dr = dtXml.NewRow();

                    dr["SLNo"] = ((Label)gr.FindControl("lblSLNoCB")).Text;
                    dr["ReportId"] = ((TextBox)gr.FindControl("txtReportId")).Text;

                    if (((TextBox)gr.FindControl("txtReportDate")).Text != "")
                        dr["ReportDate"] = gblFuction.setDate(((TextBox)gr.FindControl("txtReportDate")).Text);
                    else
                        dr["ReportDate"] = null;
                    if (((DropDownList)gr.FindControl("ddlCBname")).SelectedIndex == -1)
                    {
                        ((Label)gr.FindControl("lblCBNameId")).Text = "-1";
                    }
                    else
                    {
                        ((Label)gr.FindControl("lblCBNameId")).Text = ((DropDownList)gr.FindControl("ddlCBname")).SelectedValue.ToString();
                    }
                    dr["CBNameId"] = ((Label)gr.FindControl("lblCBNameId")).Text;
                    dr["OrgName"] = ((TextBox)gr.FindControl("txtOrgNameCB")).Text;
                    dr["LoanType"] = ((TextBox)gr.FindControl("txtLoanTypeCB")).Text;


                    if (((CheckBox)gr.FindControl("chkCBIsActive")).Checked == true)
                    {
                        ((Label)gr.FindControl("lblIsActive")).Text = "Y";
                    }
                    else
                    {
                        ((Label)gr.FindControl("lblIsActive")).Text = "N";
                    }
                    dr["Active"] = ((Label)gr.FindControl("lblIsActive")).Text;
                    dr["BankValName"] = ((TextBox)gr.FindControl("txtBankValName")).Text;
                    if (((TextBox)gr.FindControl("txtLoanAmtCB")).Text == "")
                        dr["LoanAmt"] = 0;
                    else
                        dr["LoanAmt"] = ((TextBox)gr.FindControl("txtLoanAmtCB")).Text;

                    if (((TextBox)gr.FindControl("txtPOSAmt")).Text == "")
                        dr["POSAmt"] = 0;
                    else
                        dr["POSAmt"] = ((TextBox)gr.FindControl("txtPOSAmt")).Text;
                    if (((TextBox)gr.FindControl("txtLoanDateCB")).Text != "")
                        dr["LoanDate"] = gblFuction.setDate(((TextBox)gr.FindControl("txtLoanDateCB")).Text);
                    else
                    {
                        gblFuction.AjxMsgPopup("Please Select Loan Date to Save record");
                        return false;
                    }
                    if (((TextBox)gr.FindControl("txtEMICB")).Text != "")
                        dr["EMI"] = ((TextBox)gr.FindControl("txtEMICB")).Text;
                    else
                        dr["EMI"] = 0;
                    if (((TextBox)gr.FindControl("txtTenureCB")).Text != "")
                        dr["Tenure"] = ((TextBox)gr.FindControl("txtTenureCB")).Text;
                    else
                        dr["Tenure"] = 0;
                    if (((TextBox)gr.FindControl("txtEMIPaidCB")).Text != "")
                        dr["EMIPaid"] = ((TextBox)gr.FindControl("txtEMIPaidCB")).Text;
                    else
                        dr["EMIPaid"] = 0;
                    if (((TextBox)gr.FindControl("txtPMn1")).Text.ToString() != "")
                        dr["PMn1"] = ((TextBox)gr.FindControl("txtPMn1")).Text.ToString();
                    else
                        dr["PMn1"] = "";

                    if (((TextBox)gr.FindControl("txtPMn2")).Text.ToString() != "")
                        dr["PMn2"] = ((TextBox)gr.FindControl("txtPMn2")).Text.ToString();
                    else
                        dr["PMn2"] = "";

                    if (((TextBox)gr.FindControl("txtPMn3")).Text.ToString() != "")
                        dr["PMn3"] = ((TextBox)gr.FindControl("txtPMn3")).Text.ToString();
                    else
                        dr["PMn3"] = "";
                    if (((TextBox)gr.FindControl("txtPMn4")).Text.ToString() != "")
                        dr["PMn4"] = ((TextBox)gr.FindControl("txtPMn4")).Text.ToString();
                    else
                        dr["PMn4"] = "";
                    if (((TextBox)gr.FindControl("txtPMn5")).Text.ToString() != "")
                        dr["PMn5"] = ((TextBox)gr.FindControl("txtPMn5")).Text.ToString();
                    else
                        dr["PMn5"] = "";
                    if (((TextBox)gr.FindControl("txtPMn6")).Text.ToString() != "")
                        dr["PMn6"] = ((TextBox)gr.FindControl("txtPMn6")).Text.ToString();
                    else
                        dr["PMn6"] = "";
                    if (((TextBox)gr.FindControl("txtPMn7")).Text.ToString() != "")
                        dr["PMn7"] = ((TextBox)gr.FindControl("txtPMn7")).Text.ToString();
                    else
                        dr["PMn7"] = "";
                    if (((TextBox)gr.FindControl("txtRemarks")).Text.ToString() != "")
                        dr["Remarks"] = ((TextBox)gr.FindControl("txtRemarks")).Text.ToString();
                    else
                        dr["Remarks"] = "";
                    dtXml.Rows.Add(dr);
                }


                dtXml.AcceptChanges();
            }
            dtXml.TableName = "Table1";

            string vXml = "";
            string vCBLnAppId = "";
            CMember oMem = new CMember();
            CGblIdGenerator oGbl = null;
            try
            {
                vCBLnAppId = (Request[txtLnAppCB.UniqueID] as string == null) ? txtLnAppCB.Text : Request[txtLnAppCB.UniqueID] as string;
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString().Replace("12:00:00 AM", "").Trim();
                }
                Int32 vErr = 0, vCBId = 0;
                string vMsg = "";


                if (hdfCBId.Value.ToString() != "")
                {
                    vCBId = Convert.ToInt32(hdfCBId.Value);
                }

                if (Mode == "Save")
                {
                    //if (ValidateApplicantField() == false) return false;
                    vErr = oMem.SaveCBData(vCBId, vCBLnAppId, vXml, Convert.ToInt32(hdUserID.Value), "Save", 0);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        //tdMsg.Visible = true;
                        lblValMsgCB.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        lblValMsgCB.Text = vMsg;
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    vErr = oMem.SaveCBData(vCBId, vCBLnAppId, vXml, Convert.ToInt32(hdUserID.Value), "Edit", 0);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = vMsg;
                        vResult = false;
                    }

                }
                else if (Mode == "Delete")
                {
                    DataTable dtPL = new DataTable();
                    dtPL = oMem.ChkDelPLRecord(vCBLnAppId);
                    if (Convert.ToInt32(dtPL.Rows[0]["CAMRec"]) > 0)
                    {
                        gblFuction.MsgPopup("CAM Report already made for this Loan Application No.. You can not delete CB Record..");
                        return false;
                    }
                    vErr = oMem.SaveCBData(vCBId, vCBLnAppId, vXml, Convert.ToInt32(hdUserID.Value), "Delete", 0);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                        tdMsg.Visible = false;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        vResult = false;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMem = null;
                oGbl = null;
            }
        }
        private Boolean SaveCBRemarks(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0, vRec = 0, vCBRemarksId = 0;
            string vLnAppId = "", vCBPositive = "", vCBNegative = "", vCBMitigation = "", vRemarksDesc = "", vCBPass = "", vCBRejReason = "";
            Decimal vCIBILScore = 0;
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            //  DateTime vCBPassRejDate = null;
            CGblIdGenerator oGbl = null;
            try
            {
                dt = oMem.GetLnAppPassYNByLoanid(ViewState["LoanAppId"].ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["LnAppPassYN"].ToString() == "N")
                    {
                        gblFuction.MsgPopup("You Can Not Save CB Remarks as This Loan Application remains in Rejected State...");
                        return false;
                    }
                }
                if (hfCBRemarkID.Value != "")
                {
                    vCBRemarksId = Convert.ToInt32(hfCBRemarkID.Value);
                }
                vLnAppId = ViewState["LoanAppId"].ToString();
                vCBPositive = Convert.ToString((Request[txtCBPOsRemark.UniqueID] as string == null) ? txtCBPOsRemark.Text : Request[txtCBPOsRemark.UniqueID] as string);
                vCBNegative = Convert.ToString((Request[txtCBNegRemark.UniqueID] as string == null) ? txtCBNegRemark.Text : Request[txtCBNegRemark.UniqueID] as string);
                vCBMitigation = Convert.ToString((Request[txtCBMiti.UniqueID] as string == null) ? txtCBMiti.Text : Request[txtCBMiti.UniqueID] as string);
                if (txtCIBILScore.Text != "")
                {
                    vCIBILScore = Convert.ToDecimal((Request[txtCIBILScore.UniqueID] as string == null) ? txtCIBILScore.Text : Request[txtCIBILScore.UniqueID] as string);
                    if (vCIBILScore > 0 && txtCIBILScoreDate.Text == "")
                    {
                        gblFuction.AjxMsgPopup("At the time of input CIBIL Score it is mandatory to give CIBIL Score Date...");
                        txtCIBILScoreDate.Focus();
                        return false;
                    }
                }

                if (chkCBPass.Checked == true)
                    vCBPass = "Y";
                else
                    vCBPass = "N";
                if (txtCBPassRejDate.Text == "")
                {
                    gblFuction.AjxMsgPopup("CIBIL Pass/Rejection Date Can Not Be Empty...");
                    txtCBPassRejDate.Focus();
                    return false;
                }
                DateTime vCIBILScoreDate = gblFuction.setDate(txtCIBILScoreDate.Text);
                DateTime vCBPassRejDate = gblFuction.setDate(txtCBPassRejDate.Text);
                vCBRejReason = Convert.ToString((Request[txtCBRejReason.UniqueID] as string == null) ? txtCBRejReason.Text : Request[txtCBRejReason.UniqueID] as string);
                vRemarksDesc = "CB";
                // Remarks Description...
                //CB----Credit Bureau Remarks
                //BS----Bank Statement Remarks
                //FS----Financial Statement Remarks
                //CM----CAM Remarks
                //PD1--Personal Discussion 1
                //PD2-- Personal Discussion 2
                if (Mode == "Save")
                {
                    oGbl = new CGblIdGenerator();
                    //if (vCBPositive == "" && vCBNegative == "" && vCBMitigation == "")
                    //{
                    //    gblFuction.MsgPopup("Please Give Input at least on field to Save CB Remarks Record...");
                    //    return false;
                    //}
                    vErr = oMem.SaveCBRemarks(ref vCBRemarksId, vRemarksDesc, vLnAppId, vCBPositive, vCBNegative, vCBMitigation, Convert.ToInt32(hdUserID.Value), "I", "Save", vCIBILScore, vCBPass, vCBPassRejDate, vCBRejReason, vCIBILScoreDate);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    //if (vCBPositive == "" && vCBNegative == "" && vCBMitigation == "")
                    //{
                    //    gblFuction.MsgPopup("Please Give Input at least on field to Update CB Remarks Record...");
                    //    return false;
                    //}
                    vErr = oMem.SaveCBRemarks(ref vCBRemarksId, vRemarksDesc, vLnAppId, vCBPositive, vCBNegative, vCBMitigation, Convert.ToInt32(hdUserID.Value), "E", "Edit", vCIBILScore, vCBPass, vCBPassRejDate, vCBRejReason, vCIBILScoreDate);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    if (vRec <= 0)
                    {
                        vErr = oMem.SaveCBRemarks(ref vCBRemarksId, vRemarksDesc, vLnAppId, vCBPositive, vCBNegative, vCBMitigation, Convert.ToInt32(hdUserID.Value), "D", "Delete", vCIBILScore, vCBPass, vCBPassRejDate, vCBRejReason, vCIBILScoreDate);
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblPRATAM.DBError);
                            vResult = false;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.RecordUseMsg);
                        vResult = false;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMem = null;
                dt = null;
            }
        }
        private Boolean SaveBSRemarks(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0, vRec = 0, vBSRemarksId = 0;
            string vLnAppId = "", vBSPositive = "", vBSNegative = "", vBSMitigation = "", vRemarksDesc = "", vBSPass = "", vBSRejReason = "";
            CMember oMem = new CMember();
            DataTable dtCBPass = new DataTable();
            CGblIdGenerator oGbl = null;
            try
            {
                txtAppIdBankAc.Text = ViewState["LoanAppId"].ToString();
                dtCBPass = oMem.GetCBPassByLoanId(ViewState["LoanAppId"].ToString());
                if (dtCBPass.Rows.Count > 0)
                {
                    if (dtCBPass.Rows[0]["CBPassYN"].ToString() == "N")
                    {
                        gblFuction.MsgPopup("You Can Not Add Bank Statement Remarks as Credit Bureau remains in Rejected State ...");
                        return false;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("Please tick CB Pass/Reject Checkbox to add Bank Statement Remarks... ");
                    return false;
                }
                if (hfBSRemarkID.Value != "")
                {
                    vBSRemarksId = Convert.ToInt32(hfBSRemarkID.Value);
                }
                if (chkBSPass.Checked == true)
                    vBSPass = "Y";
                else
                    vBSPass = "N";
                if (txtBSPassRejDate.Text == "")
                {
                    gblFuction.AjxMsgPopup("Bank Statement Pass/Rejection Date Can Not Be Empty...");
                    txtBSPassRejDate.Focus();
                    return false;
                }
                DateTime vCBPassRejDate = gblFuction.setDate(txtBSPassRejDate.Text);
                vBSRejReason = Convert.ToString((Request[txtBSRejReason.UniqueID] as string == null) ? txtBSRejReason.Text : Request[txtBSRejReason.UniqueID] as string);
                vLnAppId = ViewState["LoanAppId"].ToString();
                vBSPositive = Convert.ToString((Request[txtBSPosRemark.UniqueID] as string == null) ? txtBSPosRemark.Text : Request[txtBSPosRemark.UniqueID] as string);
                vBSNegative = Convert.ToString((Request[txtBSNegRemark.UniqueID] as string == null) ? txtBSNegRemark.Text : Request[txtBSNegRemark.UniqueID] as string);
                vBSMitigation = Convert.ToString((Request[txtBSMiti.UniqueID] as string == null) ? txtBSMiti.Text : Request[txtBSMiti.UniqueID] as string);
                vRemarksDesc = "BS";
                // Remarks Description...
                //CB----Credit Bureau Remarks
                //BS----Bank Statement Remarks
                //FS----Financial Statement Remarks
                //CM----CAM Remarks
                //PD1--Personal Discussion 1
                //PD2-- Personal Discussion 2
                if (Mode == "Save")
                {
                    oGbl = new CGblIdGenerator();
                    //if (vBSPositive == "" && vBSNegative == "" && vBSMitigation == "")
                    //{
                    //    gblFuction.MsgPopup("Please Give Input at least one field to Save Bank Statement Remarks...");
                    //    return false;
                    //}
                    vErr = oMem.SaveBSRemarks(ref vBSRemarksId, vRemarksDesc, vLnAppId, vBSPositive, vBSNegative, vBSMitigation,
                        Convert.ToInt32(hdUserID.Value), "I", "Save", vBSPass, vCBPassRejDate, vBSRejReason);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    //if (vBSPositive == "" && vBSNegative == "" && vBSMitigation == "")
                    //{
                    //    gblFuction.MsgPopup("Please Give Input at least one field to Update Bank Statement Remarks...");
                    //    return false;
                    //}
                    vErr = oMem.SaveBSRemarks(ref vBSRemarksId, vRemarksDesc, vLnAppId, vBSPositive, vBSNegative, vBSMitigation,
                        Convert.ToInt32(hdUserID.Value), "E", "Edit", vBSPass, vCBPassRejDate, vBSRejReason);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    if (vRec <= 0)
                    {
                        vErr = oMem.SaveBSRemarks(ref vBSRemarksId, vRemarksDesc, vLnAppId, vBSPositive, vBSNegative, vBSMitigation,
                            Convert.ToInt32(hdUserID.Value), "D", "Delete", vBSPass, vCBPassRejDate, vBSRejReason);
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblPRATAM.DBError);
                            vResult = false;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.RecordUseMsg);
                        vResult = false;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMem = null;
            }
        }
        private Boolean SaveFSRemarks(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0, vRec = 0, vFSRemarksId = 0;
            string vLnAppId = "", vFSPositive = "", vFSNegative = "", vFSMitigation = "", vRemarksDesc = "", vFinStatPass = "", vFinStatRejReason = "";
            CMember oMem = new CMember();
            DataTable dtBSPass = new DataTable();
            CGblIdGenerator oGbl = null;
            try
            {
                dtBSPass = oMem.GetBSPassByLoanId(ViewState["LoanAppId"].ToString());
                if (dtBSPass.Rows.Count > 0)
                {
                    if (dtBSPass.Rows[0]["BSPassYN"].ToString() == "N")
                    {
                        gblFuction.MsgPopup("You Can Not Add Financial Statement Remarks as Bank Statement remains in Rejected State ...");
                        return false;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("Please tick Bank Statement Pass/Reject Checkbox to add Financial Statement Remarks... ");
                    return false;
                }
                if (hfFSRemarkID.Value != "")
                {
                    vFSRemarksId = Convert.ToInt32(hfFSRemarkID.Value);
                }
                if (txtFSPassRejDate.Text == "")
                {
                    gblFuction.AjxMsgPopup("Financial Statement Pass/Rejection Date Can Not Be Blank...");
                    txtFSPassRejDate.Focus();
                    return false;
                }
                if (chkFSPass.Checked == true)
                    vFinStatPass = "Y";
                else
                    vFinStatPass = "N";
                DateTime vFinStatPassDate = gblFuction.setDate(txtFSPassRejDate.Text);
                vFinStatRejReason = Convert.ToString((Request[txtFSRejReason.UniqueID] as string == null) ? txtFSPosRemark.Text : Request[txtFSRejReason.UniqueID] as string);
                vLnAppId = ViewState["LoanAppId"].ToString();
                vFSPositive = Convert.ToString((Request[txtFSPosRemark.UniqueID] as string == null) ? txtFSPosRemark.Text : Request[txtFSPosRemark.UniqueID] as string);
                vFSNegative = Convert.ToString((Request[txtFSNegRemark.UniqueID] as string == null) ? txtFSNegRemark.Text : Request[txtFSNegRemark.UniqueID] as string);
                vFSMitigation = Convert.ToString((Request[txtFSMiti.UniqueID] as string == null) ? txtFSMiti.Text : Request[txtFSMiti.UniqueID] as string);
                vRemarksDesc = "FS";
                // Remarks Description...
                //CB----Credit Bureau Remarks
                //BS----Bank Statement Remarks
                //FS----Financial Statement Remarks
                //CM----CAM Remarks
                //PD1--Personal Discussion 1
                //PD2-- Personal Discussion 2
                if (Mode == "Save")
                {
                    oGbl = new CGblIdGenerator();
                    //if (vFSPositive == "" && vFSNegative == "" && vFSMitigation == "")
                    //{
                    //    gblFuction.MsgPopup("Please Give Input at least one field to Save Financial Statement Remarks...");
                    //    return false;
                    //}
                    vErr = oMem.SaveFSRemarks(ref vFSRemarksId, vRemarksDesc, vLnAppId, vFSPositive, vFSNegative, vFSMitigation, Convert.ToInt32(hdUserID.Value), "I", "Save", vFinStatPass, vFinStatPassDate, vFinStatRejReason);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    if (vFSPositive == "" && vFSNegative == "" && vFSMitigation == "")
                    {
                        gblFuction.MsgPopup("Please Give Input at least on field to Update Financial Statement Remarks...");
                        return false;
                    }
                    vErr = oMem.SaveFSRemarks(ref vFSRemarksId, vRemarksDesc, vLnAppId, vFSPositive, vFSNegative, vFSMitigation, Convert.ToInt32(hdUserID.Value), "E", "Edit", vFinStatPass, vFinStatPassDate, vFinStatRejReason);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    if (vRec <= 0)
                    {
                        vErr = oMem.SaveFSRemarks(ref vFSRemarksId, vRemarksDesc, vLnAppId, vFSPositive, vFSNegative, vFSMitigation, Convert.ToInt32(hdUserID.Value), "D", "Delete", vFinStatPass, vFinStatPassDate, vFinStatRejReason);
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblPRATAM.DBError);
                            vResult = false;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.RecordUseMsg);
                        vResult = false;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMem = null;
            }
        }
        private Boolean SaveBankRecord(string Mode)
        {
            // Generate DataTable For Applicant bank Account Details Section
            DataRow dr = null;
            DataTable dtXml = new DataTable();
            dtXml.Columns.Add(new DataColumn("SlNo"));
            dtXml.Columns.Add(new DataColumn("AccDate"));
            dtXml.Columns.Add(new DataColumn("Debit"));
            dtXml.Columns.Add(new DataColumn("Transfer"));
            dtXml.Columns.Add(new DataColumn("LoanAvailed"));
            dtXml.Columns.Add(new DataColumn("Credit"));
            dtXml.Columns.Add(new DataColumn("SalesFig"));
            dtXml.Columns.Add(new DataColumn("CCLimit"));
            dtXml.Columns.Add(new DataColumn("Day5"));
            dtXml.Columns.Add(new DataColumn("Day10"));
            dtXml.Columns.Add(new DataColumn("Day15"));
            dtXml.Columns.Add(new DataColumn("Day20"));
            dtXml.Columns.Add(new DataColumn("Day25"));
            foreach (GridViewRow gr in gvBankDtl.Rows)
            {
                if (((TextBox)gr.FindControl("txtDate")).Text != "")
                {
                    dr = dtXml.NewRow();
                    dr["SlNo"] = ((Label)gr.FindControl("lblSlNo")).Text;

                    if (((TextBox)gr.FindControl("txtDate")).Text != "")
                        dr["AccDate"] = gblFuction.setDate(((TextBox)gr.FindControl("txtDate")).Text);
                    else
                    {
                        gblFuction.AjxMsgPopup("Please Select Date to add record");
                        return false;
                    }
                    if (((TextBox)gr.FindControl("txtDebit")).Text != "")
                        dr["Debit"] = ((TextBox)gr.FindControl("txtDebit")).Text;
                    else
                        dr["Debit"] = 0;
                    if (((TextBox)gr.FindControl("txtTransfer")).Text != "")
                        dr["Transfer"] = ((TextBox)gr.FindControl("txtTransfer")).Text;
                    else
                        dr["Transfer"] = 0;
                    if (((TextBox)gr.FindControl("txtLoanAvailed")).Text != "")
                        dr["LoanAvailed"] = ((TextBox)gr.FindControl("txtLoanAvailed")).Text;
                    else
                        dr["LoanAvailed"] = 0;
                    if (((TextBox)gr.FindControl("txtCredit")).Text != "")
                        dr["Credit"] = ((TextBox)gr.FindControl("txtCredit")).Text;
                    else
                        dr["Credit"] = 0;
                    if (((TextBox)gr.FindControl("txtSalesFig")).Text != "")
                        dr["SalesFig"] = ((TextBox)gr.FindControl("txtSalesFig")).Text;
                    else
                        dr["SalesFig"] = 0;
                    if (((TextBox)gr.FindControl("txtCCLimit")).Text != "")
                        dr["CCLimit"] = ((TextBox)gr.FindControl("txtCCLimit")).Text;
                    else
                        dr["CCLimit"] = 0;
                    if (((TextBox)gr.FindControl("txtDay5")).Text != "")
                        dr["Day5"] = ((TextBox)gr.FindControl("txtDay5")).Text;
                    else
                        dr["Day5"] = 0;
                    if (((TextBox)gr.FindControl("txtDay10")).Text != "")
                        dr["Day10"] = ((TextBox)gr.FindControl("txtDay10")).Text;
                    else
                        dr["Day10"] = 0;
                    if (((TextBox)gr.FindControl("txtDay15")).Text != "")
                        dr["Day15"] = ((TextBox)gr.FindControl("txtDay15")).Text;
                    else
                        dr["Day15"] = 0;
                    if (((TextBox)gr.FindControl("txtDay20")).Text != "")
                        dr["Day20"] = ((TextBox)gr.FindControl("txtDay20")).Text;
                    else
                        dr["Day20"] = 0;
                    if (((TextBox)gr.FindControl("txtDay25")).Text != "")
                        dr["Day25"] = ((TextBox)gr.FindControl("txtDay25")).Text;
                    else
                        dr["Day25"] = 0;

                    dtXml.Rows.Add(dr);
                    dtXml.AcceptChanges();
                }
            }
            dtXml.TableName = "Table1";

            // Generate DataTable For Applicant bank Account IWOW Details Section
            DataRow dr1 = null;
            DataTable dtXml1 = new DataTable();
            dtXml1.Columns.Add(new DataColumn("SlNo"));
            dtXml1.Columns.Add(new DataColumn("ChqDate"));
            dtXml1.Columns.Add(new DataColumn("IWReturn"));
            dtXml1.Columns.Add(new DataColumn("OWReturn"));
            dtXml1.Columns.Add(new DataColumn("EMIReturn"));
            dtXml1.Columns.Add(new DataColumn("ChqClearDate"));
            dtXml1.Columns.Add(new DataColumn("ChqNo"));
            dtXml1.Columns.Add(new DataColumn("Reason"));

            foreach (GridViewRow gr in gvIWOWDtl.Rows)
            {
                if (((TextBox)gr.FindControl("txtChqDate")).Text != "")
                {
                    dr1 = dtXml1.NewRow();
                    dr1["SlNo"] = ((Label)gr.FindControl("lblSlNo")).Text;
                    if (((TextBox)gr.FindControl("txtChqDate")).Text != "")
                        dr1["ChqDate"] = gblFuction.setDate(((TextBox)gr.FindControl("txtChqDate")).Text);
                    else
                    {
                        gblFuction.AjxMsgPopup("Please Select Date to add record in Cheque Return Details Section");
                        return false;
                    }
                    if (((TextBox)gr.FindControl("txtIWReturn")).Text != "")
                        dr1["IWReturn"] = ((TextBox)gr.FindControl("txtIWReturn")).Text;
                    else
                        dr1["IWReturn"] = 0;
                    if (((TextBox)gr.FindControl("txtOWReturn")).Text != "")
                        dr1["OWReturn"] = ((TextBox)gr.FindControl("txtOWReturn")).Text;
                    else
                        dr1["OWReturn"] = 0;

                    if (((TextBox)gr.FindControl("txtEMIReturn")).Text != "")
                        dr1["EMIReturn"] = ((TextBox)gr.FindControl("txtEMIReturn")).Text;
                    else
                        dr1["EMIReturn"] = 0;

                    if (((TextBox)gr.FindControl("txtClearedDt")).Text != "")
                        dr1["ChqClearDate"] = gblFuction.setDate(((TextBox)gr.FindControl("txtClearedDt")).Text);
                    else
                        dr1["ChqClearDate"] = null;
                    if (((TextBox)gr.FindControl("txtChequeNo")).Text != "")
                        dr1["ChqNo"] = ((TextBox)gr.FindControl("txtChequeNo")).Text;
                    else
                        dr1["ChqNo"] = "";
                    if (((TextBox)gr.FindControl("txtReason")).Text != "")
                        dr1["Reason"] = ((TextBox)gr.FindControl("txtReason")).Text;
                    else
                        dr1["Reason"] = "";
                    dtXml1.Rows.Add(dr1);
                    dtXml1.AcceptChanges();
                }
            }
            dtXml1.TableName = "Table2";

            // Declaring variable for uploading bank statement document section
            string vAttchBankDocType = "";
            byte[] vAttachBankDoc = null;
            Int32 vAttachBankDocUpID = 0;
            string vAttchBankDocName = "";


            Boolean vResult = false;
            Int32 vBankAccId = 0, vError = 0, vNewId = 0, vSrNo = 0;
            string vApplicationId = "", vAcHolName = "", vBankname = "", vAccNo = "", vCombMOS = "", vShowAgrmnt = "",
                vAccType = "", vRemarks = "", vXml = "", vXmlIWOW = "";
            Double vTransLimit = 0, vCurrbal = 0, vBankingYr = 0;

            if (hfBankAcId.Value != "")
            {
                vBankAccId = Convert.ToInt32(hfBankAcId.Value);
            }
            if (txtBankAcSrNo.Text != "")
                vSrNo = Convert.ToInt32((Request[txtBankAcSrNo.UniqueID] as string == null) ? txtBankAcSrNo.Text : Request[txtBankAcSrNo.UniqueID] as string);
            vApplicationId = Convert.ToString((Request[txtAppIdBankAc.UniqueID] as string == null) ? txtAppIdBankAc.Text : Request[txtAppIdBankAc.UniqueID] as string);
            vAcHolName = Convert.ToString((Request[txtAcHolName.UniqueID] as string == null) ? txtAcHolName.Text : Request[txtAcHolName.UniqueID] as string);
            vBankname = Convert.ToString((Request[txtBankName.UniqueID] as string == null) ? txtBankName.Text : Request[txtBankName.UniqueID] as string);

            if (Convert.ToString((Request[txtAccNo.UniqueID] as string == null) ? txtAccNo.Text : Request[txtAccNo.UniqueID] as string) == "")
            {
                gblFuction.MsgPopup("Please Give Input Bank Account No...");
                return false;
            }
            if (Convert.ToString((Request[txtBankName.UniqueID] as string == null) ? txtBankName.Text : Request[txtBankName.UniqueID] as string) == "")
            {
                gblFuction.MsgPopup("Please Give Input Bank Name...");
                return false;
            }
            vAccNo = Convert.ToString((Request[txtAccNo.UniqueID] as string == null) ? txtAccNo.Text : Request[txtAccNo.UniqueID] as string);
            vAccType = Convert.ToString((Request[ddlACType.UniqueID] as string == null) ? ddlACType.SelectedValue : Request[ddlACType.UniqueID] as string);
            if (vAccType == "0")
            {
                gblFuction.MsgPopup("Please Select Bank Account Type..");
                return false;
            }
            vRemarks = Convert.ToString((Request[txtBankAcRemarks.UniqueID] as string == null) ? txtBankAcRemarks.Text : Request[txtBankAcRemarks.UniqueID] as string);
            if (txtBankSinYr.Text != "")
                vBankingYr = Convert.ToDouble((Request[txtBankSinYr.UniqueID] as string == null) ? txtBankSinYr.Text : Request[txtBankSinYr.UniqueID] as string);
            if (txtTransLimit.Text != "")
                vTransLimit = Convert.ToDouble((Request[txtTransLimit.UniqueID] as string == null) ? txtTransLimit.Text : Request[txtTransLimit.UniqueID] as string);
            if (txtCurrBalance.Text != "")
                vCurrbal = Convert.ToDouble((Request[txtCurrBalance.UniqueID] as string == null) ? txtCurrBalance.Text : Request[txtCurrBalance.UniqueID] as string);
            if (chkCombMOS.Checked == true)
                vCombMOS = "Y";
            else
                vCombMOS = "N";
            if (chkShowAgrmnt.Checked == true)
                vShowAgrmnt = "Y";
            else
                vShowAgrmnt = "N";
            CMember oMem = new CMember();
            try
            {

                if (hfBankStatDocId.Value != "")
                {
                    vAttachBankDocUpID = Convert.ToInt32(hfBankStatDocId.Value);
                }

                // Upload Bank Statement Documents Section
                if (fileuplodBankStat.PostedFile.InputStream.Length > 4194304)
                {
                    gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
                    return false;
                }
                if (fileuplodBankStat.HasFile)
                {
                    vAttachBankDoc = new byte[fileuplodBankStat.PostedFile.InputStream.Length + 1];
                    fileuplodBankStat.PostedFile.InputStream.Read(vAttachBankDoc, 0, vAttachBankDoc.Length);
                    vAttchBankDocType = System.IO.Path.GetExtension(fileuplodBankStat.FileName).ToLower();
                    vAttchBankDocName = System.IO.Path.GetFileNameWithoutExtension(fileuplodBankStat.FileName);
                }

                //Generate XML For Applicant bank Account Details Section
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString().Replace(" ", "").Trim().Replace("12:00:00AM", "").Trim();
                }
                //Generate XML For Applicant bank Account IWOW Details Section
                using (StringWriter oSW1 = new StringWriter())
                {
                    dtXml1.WriteXml(oSW1);
                    vXmlIWOW = oSW1.ToString().Replace(" ", "").Trim().Replace("12:00:00AM", "").Trim();
                }
                if (Mode == "Save")
                {
                    // Check For Duplicate Bank Acc No...
                    DataTable dtDupBankRec = new DataTable();
                    dtDupBankRec = oMem.ChkDupBankStatRec(vApplicationId, vAccNo);
                    if (Convert.ToInt32(dtDupBankRec.Rows[0]["BankAcRec"]) > 0)
                    {
                        gblFuction.MsgPopup("Bank Account No already exist... Kindly Check...");
                        return false;
                    }
                    vError = oMem.SaveBankStatement(ref vNewId, vBankAccId, vSrNo, vApplicationId, vAcHolName, vBankname, vAccType, vAccNo, vBankingYr, vRemarks,
                        vTransLimit, vCurrbal, Convert.ToInt32(hdUserID.Value), vXml, vXmlIWOW, "Save", 0, vAttachBankDocUpID, vAttachBankDoc, vAttchBankDocType, vAttchBankDocName, vCombMOS, vShowAgrmnt);

                    if (vError > 0)
                    {
                        hfBankAcId.Value = Convert.ToInt32(vNewId).ToString();
                        ViewState["BankAcId"] = vNewId;
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        vResult = false;
                    }

                }
                else if (Mode == "Edit")
                {
                    vError = oMem.SaveBankStatement(ref vNewId, vBankAccId, vSrNo, vApplicationId, vAcHolName, vBankname, vAccType, vAccNo, vBankingYr, vRemarks,
                        vTransLimit, vCurrbal, Convert.ToInt32(hdUserID.Value), vXml, vXmlIWOW, "Edit", 0, vAttachBankDocUpID, vAttachBankDoc, vAttchBankDocType, vAttchBankDocName, vCombMOS, vShowAgrmnt);

                    if (vError > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    // Check For Deleting Record()
                    DataTable dtDelBankRec = new DataTable();
                    dtDelBankRec = oMem.ChkDelBankRecord(vApplicationId);
                    if (Convert.ToInt32(dtDelBankRec.Rows[0]["CAMRec"]) > 0)
                    {
                        gblFuction.MsgPopup("CAM Report alredy made.. Can Not Delete Record..");
                        return false;
                    }
                    vError = oMem.SaveBankStatement(ref vNewId, vBankAccId, vSrNo, vApplicationId, vAcHolName, vBankname, vAccType, vAccNo, vBankingYr, vRemarks, vTransLimit, vCurrbal,
                            Convert.ToInt32(hdUserID.Value), vXml, vXmlIWOW, "Delete", 0, vAttachBankDocUpID, vAttachBankDoc, vAttchBankDocType, vAttchBankDocName, vCombMOS, vShowAgrmnt);

                    if (vError > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        vResult = false;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMem = null;
            }
        }
        private Boolean SaveBSRecords(string Mode)
        {

            Boolean vResult = false;
            DataRow dr = null;
            if (txtBSDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Balance Sheet Date can not be left blank...");
                return false;
            }
            else
            {
                string StrDD, StrMM, StrYYYY;
                string pDate = txtBSDate.Text.ToString();
                StrDD = pDate.Substring(0, 2);
                StrMM = pDate.Substring(3, 2);
                StrYYYY = pDate.Substring(6, 4);
                if (StrDD != "31")
                {
                    gblFuction.AjxMsgPopup("Day Part of selected date should be 31...");
                    return false;
                }
                else if (StrMM != "03")
                {
                    gblFuction.AjxMsgPopup("Month Part of selected date should be 03...");
                    return false;
                }
            }
            Double TotLiability = 0;
            Double TotAsset = 0;
            if (txtTotLiability.Text != "")
                TotLiability = Convert.ToDouble(txtTotLiability.Text);
            if (txtTotAsset.Text != "")
                TotAsset = Convert.ToDouble(txtTotAsset.Text);
            //if (TotAsset - TotLiability < 0.03)
            //{
            //    gblFuction.AjxMsgPopup("Difference between Total Asset and Total Liability must be greater than .03");
            //    return false;
            //}
            DateTime vBSDate = gblFuction.setDate(txtBSDate.Text);
            string vAppId = txtAppNoBS.Text.ToString().Trim();


            DataTable dtXml = new DataTable();
            dtXml.Columns.Add("ParticularsId", typeof(int));
            dtXml.Columns.Add("Particulars", typeof(string));
            dtXml.Columns.Add("Amount", typeof(float));
            foreach (GridViewRow gr in gvLiability.Rows)
            {
                dr = dtXml.NewRow();

                dr["ParticularsId"] = ((Label)gr.FindControl("lblParticularsId")).Text; ;
                dr["Particulars"] = ((Label)gr.FindControl("lblParticulars")).Text;
                if (((TextBox)gr.FindControl("txtAmt")).Text == "")
                    dr["Amount"] = 0;
                else
                    dr["Amount"] = ((TextBox)gr.FindControl("txtAmt")).Text;

                dtXml.Rows.Add(dr);
                dtXml.AcceptChanges();
            }
            foreach (GridViewRow gr in gvAsset.Rows)
            {
                dr = dtXml.NewRow();

                dr["ParticularsId"] = ((Label)gr.FindControl("lblParticularsId")).Text; ;
                dr["Particulars"] = ((Label)gr.FindControl("lblParticulars")).Text;
                if (((TextBox)gr.FindControl("txtAssetAmt")).Text == "")
                    dr["Amount"] = 0;
                else
                    dr["Amount"] = ((TextBox)gr.FindControl("txtAssetAmt")).Text;

                dtXml.Rows.Add(dr);
                dtXml.AcceptChanges();
            }
            dtXml.TableName = "Table1";

            string vXml = "";
            CMember oMem = new CMember();
            try
            {
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString().Replace(" ", "").Trim().Replace("12:00:00AM", "").Trim();
                }
                Int32 vErr = 0, vBSId = 0;
                string vMsg = "";


                if (hfBSId.Value.ToString() != "")
                {
                    vBSId = Convert.ToInt32(hfBSId.Value);
                }

                if (Mode == "Save")
                {
                    //if (ValidateApplicantField() == false) return false;
                    DataTable dtBS = new DataTable();
                    dtBS = oMem.DuplicateBSRecByLnNo(vAppId, vBSDate, Mode);
                    if (Convert.ToInt32(dtBS.Rows[0]["BSRec"]) > 0)
                    {
                        gblFuction.MsgPopup("Data Already Exist in this BS Date and Loan Application No... Kindly Check Balance Sheet Summary...");
                        return false;
                    }
                    vErr = oMem.SaveBSData(vBSId, vAppId, vBSDate, vXml, Convert.ToInt32(hdUserID.Value), "Save", 0);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = vMsg;
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    vErr = oMem.SaveBSData(vBSId, vAppId, vBSDate, vXml, Convert.ToInt32(hdUserID.Value), "Edit", 0);
                    if (vErr == 1)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = vMsg;
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    DataTable dtPL = new DataTable();
                    dtPL = oMem.ChkDelPLRecord(vAppId);
                    if (Convert.ToInt32(dtPL.Rows[0]["CAMRec"]) > 0)
                    {
                        gblFuction.MsgPopup("CAM Report has been made for this Loan Application No.. You can not delete PL Record..");
                        return false;
                    }
                    vErr = oMem.SaveBSData(vBSId, vAppId, vBSDate, vXml, Convert.ToInt32(hdUserID.Value), "Delete", 0);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                        tdMsg.Visible = false;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        vResult = false;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMem = null;
            }
        }
        private Boolean SavePLRecords(string Mode)
        {

            if (txtPLDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Profit/Loss Date can not be left blank...");
                return false;
            }
            else
            {
                string StrDD, StrMM, StrYYYY;
                string pDate = txtPLDate.Text.ToString();
                StrDD = pDate.Substring(0, 2);
                StrMM = pDate.Substring(3, 2);
                StrYYYY = pDate.Substring(6, 4);
                if (StrDD != "31")
                {
                    gblFuction.AjxMsgPopup("Day Part of selected date should be 31...");
                    return false;
                }
                else if (StrMM != "03")
                {
                    gblFuction.AjxMsgPopup("Month Part of selected date should be 03...");
                    return false;
                }
            }
            DateTime vPLDate = gblFuction.setDate(txtPLDate.Text);
            string vAppId = txtAppIdPL.Text.ToString().Trim();
            Boolean vResult = false;
            DataRow dr = null;
            DataTable dtXml = new DataTable();

            dtXml.Columns.Add("ParticularsId", typeof(int));
            dtXml.Columns.Add("Particulars", typeof(string));
            dtXml.Columns.Add("Amount", typeof(float));

            foreach (GridViewRow gr in gvIncome.Rows)
            {
                dr = dtXml.NewRow();

                dr["ParticularsId"] = ((Label)gr.FindControl("lblParticularsIdPL")).Text;
                dr["Particulars"] = ((Label)gr.FindControl("lblParticularsPL")).Text;
                if (((TextBox)gr.FindControl("txtIncome")).Text == "")
                    dr["Amount"] = 0;
                else
                    dr["Amount"] = ((TextBox)gr.FindControl("txtIncome")).Text;

                dtXml.Rows.Add(dr);
                dtXml.AcceptChanges();
            }
            foreach (GridViewRow gr in gvExpense.Rows)
            {
                dr = dtXml.NewRow();

                dr["ParticularsId"] = ((Label)gr.FindControl("lblParticularsIdPL")).Text;
                dr["Particulars"] = ((Label)gr.FindControl("lblParticularsPL")).Text;
                if (((TextBox)gr.FindControl("txtExpense")).Text == "")
                    dr["Amount"] = 0;
                else
                    dr["Amount"] = ((TextBox)gr.FindControl("txtExpense")).Text;

                dtXml.Rows.Add(dr);
                dtXml.AcceptChanges();
            }
            dtXml.TableName = "Table1";

            DataTable dtXmlITR = new DataTable();
            dtXmlITR.Columns.Add("ITRFileDate", typeof(string));
            dtXmlITR.Columns.Add("FinYear", typeof(string));
            dtXmlITR.Columns.Add("ITRFiledYN", typeof(string));
            foreach (GridViewRow gr in gvITR.Rows)
            {
                if (((TextBox)gr.FindControl("txtITRFileDate")).Text != "")
                {
                    dr = dtXmlITR.NewRow();
                    if (((TextBox)gr.FindControl("txtITRFileDate")).Text != "")
                        dr["ITRFileDate"] = gblFuction.setDate(((TextBox)gr.FindControl("txtITRFileDate")).Text);
                    else
                    {
                        gblFuction.AjxMsgPopup("Please give input ITR Filing Date....");
                        return false;
                    }
                    if (((TextBox)gr.FindControl("txtFinYear")).Text != "")
                        dr["FinYear"] = ((TextBox)gr.FindControl("txtFinYear")).Text;
                    else
                    {
                        gblFuction.AjxMsgPopup("Please give input Financial Year....");
                        return false;
                    }
                    if (((CheckBox)gr.FindControl("chkITRFile")).Checked == true)
                        dr["ITRFiledYN"] = "Y";
                    else
                        dr["ITRFiledYN"] = "N";

                    dtXmlITR.Rows.Add(dr);
                    dtXmlITR.AcceptChanges();
                }
            }
            dtXmlITR.TableName = "Table2";



            string vXml = "";
            string vITRXml = "";
            CMember oMem = new CMember();

            string vBSPLAtachDocType = "";
            byte[] vBSPLAttachDoc = null;
            Int32 vBSPLDocUpID = 0;
            string vBSPLAttachDocName = "";

            try
            {
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString().Replace(" ", "").Trim().Replace("12:00:00AM", "").Trim();
                }
                using (StringWriter oSW = new StringWriter())
                {
                    dtXmlITR.WriteXml(oSW);
                    vITRXml = oSW.ToString().Replace(" ", "").Trim().Replace("12:00:00AM", "").Trim();
                }
                Int32 vErr = 0, vPLId = 0;
                string vMsg = "";


                if (hfPLId.Value.ToString() != "")
                {
                    vPLId = Convert.ToInt32(hfPLId.Value);
                }

                if (hfPLBSDocUpId.Value != "")
                {
                    vBSPLDocUpID = Convert.ToInt32(hfPLBSDocUpId.Value);
                }
                if (fileuplodPLBS.PostedFile.InputStream.Length > 4194304)
                {
                    gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
                    return false;
                }
                if (fileuplodPLBS.HasFile)
                {
                    vBSPLAttachDoc = new byte[fileuplodPLBS.PostedFile.InputStream.Length + 1];
                    fileuplodPLBS.PostedFile.InputStream.Read(vBSPLAttachDoc, 0, vBSPLAttachDoc.Length);
                    vBSPLAtachDocType = System.IO.Path.GetExtension(fileuplodPLBS.FileName).ToLower();
                    vBSPLAttachDocName = System.IO.Path.GetFileNameWithoutExtension(fileuplodPLBS.FileName);
                }

                if (Mode == "Save")
                {
                    //if (ValidateApplicantField() == false) return false;
                    DataTable dtPL = new DataTable();
                    dtPL = oMem.DuplicatePLRecByLnNo(vAppId, vPLDate, Mode);
                    if (Convert.ToInt32(dtPL.Rows[0]["PLRec"]) > 0)
                    {
                        gblFuction.MsgPopup("Data Already Exist in this PL Date and Loan Application No... Kindly Check Profit and Loss Summary...");
                        return false;
                    }
                    vErr = oMem.SavePLData(vPLId, vAppId, vPLDate, vXml, vITRXml, Convert.ToInt32(hdUserID.Value), "Save", 0, vBSPLDocUpID, vBSPLAttachDoc, vBSPLAtachDocType, vBSPLAttachDocName);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = vMsg;
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    //DataTable dtPL = new DataTable();
                    //dtPL = oMem.DuplicatePLRecByLnNo(vAppId, vPLDate, Mode);
                    //if (Convert.ToInt32(dtPL.Rows[0]["PLRec"]) > 0)
                    //{
                    //    gblFuction.MsgPopup("Data Already Exist in this PL Date and Loan Application No... Kindly Check Profit and Loss Summary...");
                    //    return false;
                    //}
                    vErr = oMem.SavePLData(vPLId, vAppId, vPLDate, vXml, vITRXml, Convert.ToInt32(hdUserID.Value), "Edit", 0, vBSPLDocUpID, vBSPLAttachDoc, vBSPLAtachDocType, vBSPLAttachDocName);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = vMsg;
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    DataTable dtPL = new DataTable();
                    dtPL = oMem.ChkDelPLRecord(vAppId);
                    if (Convert.ToInt32(dtPL.Rows[0]["CAMRec"]) > 0)
                    {
                        gblFuction.MsgPopup("CAM Report has been made for this Loan Application No.. You can not delete PL Record..");
                        return false;
                    }
                    vErr = oMem.SavePLData(vPLId, vAppId, vPLDate, vXml, vITRXml, Convert.ToInt32(hdUserID.Value), "Delete", 0, vBSPLDocUpID, vBSPLAttachDoc, vBSPLAtachDocType, vBSPLAttachDocName);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                        tdMsg.Visible = false;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        vResult = false;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMem = null;
            }
        }
        private Boolean SaveRefRecords(string Mode)
        {
            Boolean vResult = false;
            DataRow dr = null;
            DataTable dtXml = new DataTable();
            dtXml.Columns.Add("ApplicationId", typeof(string));
            dtXml.Columns.Add("RefName", typeof(string));
            dtXml.Columns.Add("RefAddr", typeof(string));
            dtXml.Columns.Add("RefMNo", typeof(string));
            dtXml.Columns.Add("RelWithAppId", typeof(int));
            dtXml.Columns.Add("RelWithApp", typeof(string));
            dtXml.Columns.Add("OffTelNo", typeof(string));
            dtXml.Columns.Add("CompName", typeof(string));
            dtXml.Columns.Add("PersonalRef", typeof(string));
            dtXml.Columns.Add("TradeRef", typeof(string));
            dtXml.Columns.Add("RefType", typeof(string));
            dtXml.Columns.Add("Status", typeof(string));
            dtXml.Columns.Add("VarifiedBy", typeof(string));
            foreach (GridViewRow gr in gvRef.Rows)
            {
                dr = dtXml.NewRow();
                dr["ApplicationId"] = ((Label)gr.FindControl("lblAppIdPL")).Text;
                dr["RefName"] = ((Label)gr.FindControl("lblRefName")).Text;
                dr["RefAddr"] = ((Label)gr.FindControl("lblRefAddress")).Text;
                dr["RefMNo"] = ((Label)gr.FindControl("lblRefMob")).Text; ;
                dr["RelWithAppId"] = ((Label)gr.FindControl("lblRelWithAppId")).Text; ;
                dr["RelWithApp"] = ((Label)gr.FindControl("lblRelWithApplicant")).Text; ;
                dr["OffTelNo"] = ((Label)gr.FindControl("lblOffTelNo")).Text; ;
                dr["CompName"] = ((Label)gr.FindControl("lblCompanyName")).Text;
                dr["PersonalRef"] = ((Label)gr.FindControl("lblPersonalRef")).Text; ;
                dr["TradeRef"] = ((Label)gr.FindControl("lblTradeRef")).Text; ;
                dr["RefType"] = ((Label)gr.FindControl("lblRefType")).Text;
                dr["Status"] = ((Label)gr.FindControl("lblStatus")).Text;
                dr["VarifiedBy"] = ((Label)gr.FindControl("lblVarifiedBy")).Text;
                dtXml.Rows.Add(dr);
                dtXml.AcceptChanges();
            }
            dtXml.TableName = "Table1";

            string vXml = "";
            CMember oMem = new CMember();
            try
            {
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString();
                }
                Int32 vErr = 0, vRefId = 0;
                string vMsg = "";


                if (hfRefId.Value.ToString() != "")
                {
                    vRefId = Convert.ToInt32(hfRefId.Value);
                }

                if (Mode == "Save")
                {
                    //if (ValidateApplicantField() == false) return false;
                    vErr = oMem.SaveRefData(vRefId, vXml, Convert.ToInt32(hdUserID.Value), "Save", 0);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = vMsg;
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    vErr = oMem.SaveRefData(vRefId, vXml, Convert.ToInt32(hdUserID.Value), "Edit", 0);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = vMsg;
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    vErr = oMem.SaveRefData(vRefId, vXml, Convert.ToInt32(hdUserID.Value), "Delete", 0);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                        tdMsg.Visible = false;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        vResult = false;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMem = null;
            }
        }
        private void EnableControl(Boolean Status)
        {
        }
        private void popSourceName()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "SourceID", "SourceName", "SourceMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), "0000");
                ddlSourceName.DataSource = dt;
                if (dt.Rows.Count > 0)
                {
                    ddlSourceName.DataTextField = "SourceName";
                    ddlSourceName.DataValueField = "SourceID";
                    ddlSourceName.DataBind();
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlSourceName.Items.Insert(0, oli1);
                }
                else
                {
                    ddlSourceName.DataSource = null;
                    ddlSourceName.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        private void PopApplicant()
        {
           string vBrCode = (string)Session[gblValue.BrnchCode];
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetApplicantList(vBrCode);
                if (dt.Rows.Count > 0)
                {
                    ddlLoanApplicantname.DataSource = dt;
                    ddlLoanApplicantname.DataTextField = "CompanyName";
                    ddlLoanApplicantname.DataValueField = "CustId";
                    ddlLoanApplicantname.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlLoanApplicantname.Items.Insert(0, oli);
                }
                else
                {
                    ddlLoanApplicantname.DataSource = null;
                    ddlLoanApplicantname.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        private void PopRelation()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetRelationList();
                if (dt.Rows.Count > 0)
                {
                    ddlRelWApp.DataSource = dt;
                    ddlRelWApp.DataTextField = "Relation";
                    ddlRelWApp.DataValueField = "RelationId";
                    ddlRelWApp.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlRelWApp.Items.Insert(0, oli);
                }
                else
                {
                    ddlRelWApp.DataSource = null;
                    ddlRelWApp.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        private void PopPurpose()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetLoanPurposeList();
                if (dt.Rows.Count > 0)
                {
                    ddlLnPurpose.DataSource = dt;
                    ddlLnPurpose.DataTextField = "PurposeName";
                    ddlLnPurpose.DataValueField = "PurposeId";
                    ddlLnPurpose.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlLnPurpose.Items.Insert(0, oli);
                }
                else
                {
                    ddlLnPurpose.DataSource = null;
                    ddlLnPurpose.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        private void PopCBName()
        {
            //CMember oMem = new CMember();
            //DataTable dt = null;
            //try
            //{
            //    dt = oMem.GetCBName();
            //    if (dt.Rows.Count > 0)
            //    {
            //        ddlCBName.DataSource = dt;
            //        ddlCBName.DataTextField = "CBName";
            //        ddlCBName.DataValueField = "CBNameId";
            //        ddlCBName.DataBind();
            //        ListItem oli = new ListItem("<--Select-->", "-1");
            //        ddlCBName.Items.Insert(0, oli);
            //    }
            //    else
            //    {
            //        ddlCBName.DataSource = null;
            //        ddlCBName.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    dt = null;
            //    oMem = null;
            //}
        }
        private void PopCompanyGL()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetCompanyGLDetails();
                if (dt.Rows.Count > 0)
                {
                    //ddlParticulars.DataSource = dt;
                    //ddlParticulars.DataTextField = "AcGenLed";
                    //ddlParticulars.DataValueField = "AcGenLedId";
                    //ddlParticulars.DataBind();
                    //ListItem oli = new ListItem("<--Select-->", "-1");
                    //ddlParticulars.Items.Insert(0, oli);

                    //ddlParticularsPL.DataSource = dt;
                    //ddlParticularsPL.DataTextField = "AcGenLed";
                    //ddlParticularsPL.DataValueField = "AcGenLedId";
                    //ddlParticularsPL.DataBind();
                    //ListItem oli1 = new ListItem("<--Select-->", "-1");
                    //ddlParticularsPL.Items.Insert(0, oli1);
                }
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        private void PopLoanType()
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDate = gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate]));
            CLoanScheme oLS = new CLoanScheme();
            try
            {
                dt = oLS.GetActiveLnSchemePG();
                if (dt.Rows.Count > 0)
                {
                    ddlLnScheme.DataTextField = "LoanTypeName";
                    ddlLnScheme.DataValueField = "LoanTypeId";
                    ddlLnScheme.DataSource = dt;
                    ddlLnScheme.DataBind();
                    ListItem oItm = new ListItem();
                    oItm.Text = "<--- Select --->";
                    oItm.Value = "-1";
                    ddlLnScheme.Items.Insert(0, oItm);
                }
                else
                {
                    ddlLnScheme.DataSource = null;
                    ddlLnScheme.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oLS = null;
            }
        }
        private void GetComGLForBalShet()
        {
            CMember oMem = new CMember();
            DataSet ds = null;
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            try
            {
                ds = oMem.GetGenLedForBalSheet();
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    if (dt1.Rows.Count > 0)
                    {
                        gvLiability.DataSource = dt1;
                        gvLiability.DataBind();
                    }
                    else
                    {
                        gvLiability.DataSource = null;
                        gvLiability.DataBind();
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        gvAsset.DataSource = dt2;
                        gvAsset.DataBind();
                    }
                    else
                    {
                        gvAsset.DataSource = null;
                        gvAsset.DataBind();
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
                ds = null;
                oMem = null;
            }
        }
        private void GetComGLForPL()
        {
            CMember oMem = new CMember();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            try
            {
                ds = oMem.GetGenLedForPL();
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                }
                if (dt.Rows.Count > 0)
                {
                    gvIncome.DataSource = dt;
                    gvIncome.DataBind();
                }
                else
                {
                    gvIncome.DataSource = null;
                    gvIncome.DataBind();
                }
                if (dt1.Rows.Count > 0)
                {
                    gvExpense.DataSource = dt1;
                    gvExpense.DataBind();
                }
                else
                {
                    gvExpense.DataSource = null;
                    gvExpense.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ds = null;
                dt = null;
                dt1 = null;
                oMem = null;
            }
        }
        private Boolean ValidateFieldsForLnApp()
        {
            Boolean vResult = true;
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CApplication oCG = oCG = new CApplication();
            DateTime vAppDate = vLoginDt;

            if (txtAppDt.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Loan Application Date cannot be empty.");
                vResult = false;
            }
            else
            {
                vAppDate = gblFuction.setDate(txtAppDt.Text);
                if (vAppDate < vFinFrom || vAppDate > vFinTo)
                {
                    gblFuction.MsgPopup("Loan Application Date should be Financial Year.");
                    vResult = false;
                }
                if (vAppDate > vLoginDt)
                {
                    gblFuction.MsgPopup("Loan Application Date should not be greater than login date.");
                    vResult = false;
                }
            }
            if (Request[ddlLoanApplicantname.UniqueID] as string == "-1")
            {
                gblFuction.MsgPopup("Please select Applicant...");
                vResult = false;
            }

            return vResult;
        }
        private void ClearApplication()
        {
            txtAppNo.Text = "";
            txtAppDt.Text = "";
            ddlLoanApplicantname.SelectedIndex = -1;
            ddlLnPurpose.SelectedIndex = -1;
            ddlLnScheme.SelectedIndex = -1;
            ddlSourceName.SelectedIndex = -1;
            txtAppLnAmt.Text = "";
            txtTenure.Text = "";
            txtLnPurposeDetails.Text = "";
            txtAddTerms.Text = "";
        }
        private void ClearBankAccControl()
        {
            txtAppIdBankAc.Text = "";
            txtAcHolName.Text = "";
            txtBankAcSrNo.Text = "";
            txtBankName.Text = "";
            ddlACType.SelectedValue = "0";
            txtAccNo.Text = "";
            txtBankSinYr.Text = "";
            //gvBankDtl.DataSource = null;
            //gvBankDtl.DataBind();
            txtBankAcRemarks.Text = "";
            txtTransLimit.Text = "";
            txtCurrBalance.Text = "";
        }
        private void ClearCBControl()
        {
            //txtReportDate.Text = "";
            //txtReportId.Text = "";
            //ddlCBName.SelectedIndex = -1;
            //txtOrgNameCB.Text = "";
            //txtSRNo.Text = "";
            //txtLoanTypeCB.Text = "";
            //txtLnDtCB.Text = "";
            //txtLnAmtCB.Text = "";
            //txtEMIAmt.Text = "";
            //txtTenureCB.Text = "";
            //txtEMIPaidCB.Text = "";
            //txtPOSAmtCB.Text = "";

            //chbActiveCB.Checked = false;
            //txtBankValName.Text = "";
            //txtPMn1.Text = "";
            //txtPMn2.Text = "";
            //txtPMn3.Text = "";
            //txtPMn4.Text = "";
            //txtPMn5.Text = "";
            //txtPMn6.Text = "";
        }
        private void ClearBSControl()
        {
            // txtSRNoBS.Text = "";
            //ddlParticulars.SelectedIndex = -1;
            txtBSDate.Text = "";
            txtTotAsset.Text = "";
            txtTotLiability.Text = "";
            //txtBSAmt.Text = "";
            //txtChangeBS.Text = "";
            //txtRemarksBS.Text = "";
        }
        private void ClearPLControl()
        {
            // txtSlNoPL.Text = "";
            //ddlParticularsPL.SelectedIndex = -1;
            txtPLDate.Text = "";
            txtTotIncome.Text = "";
            txtTotExpense.Text = "";
            txtNetProfit.Text = "";
            //txtPLAmt.Text = "";
            //txtChangePL.Text = "";
            //txtRemarksPL.Text = "";
        }
        private void ClearRefControl()
        {
            txtRefName.Text = "";
            ddlRelWApp.SelectedIndex = -1;
            txtRefAddr.Text = "";
            txtRefMNo.Text = "";
            txtOffTelNoRef.Text = "";
            rblRefType.SelectedIndex = -1;
            txtOffTelNoRef.Text = "";
            txtCompNameRef.Text = "";
            txtStatus.Text = "";
        }
        protected void ddlACType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlACType.SelectedValue == "0")
                {
                    gblFuction.AjxMsgPopup("Please Select Account Type");
                    return;
                }
                else if (ddlACType.SelectedValue == "OD" || ddlACType.SelectedValue == "CC")
                {
                    txtTransLimit.Enabled = true;
                }
                else
                {
                    txtTransLimit.Text = "";
                    txtTransLimit.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void ddlLnScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLnScheme.SelectedValue != "-1")
            {
                int LnType = Convert.ToInt32(ddlLnScheme.SelectedValue);
                EnableMachinDtl(LnType);
            }
        }
        protected void ShowUploadedDocuments(string pLnAppId)
        {
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = OMem.GetUploadDocByLoanId(pLnAppId);
                if (dt.Rows.Count > 0)
                {
                    gvUpDetails.DataSource = dt;
                    gvUpDetails.DataBind();
                }
                else
                {
                    gvUpDetails.DataSource = null;
                    gvUpDetails.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
                dt = null;
            }
        }
        protected void ShowUploadedBSDoc(string pLnAppId)
        {
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = OMem.GetUploadBSDocByLoanId(pLnAppId, "BS");
                if (dt.Rows.Count > 0)
                {
                    gvCustBankDoc.DataSource = dt;
                    gvCustBankDoc.DataBind();
                }
                else
                {
                    gvCustBankDoc.DataSource = null;
                    gvCustBankDoc.DataBind();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
                dt = null;
            }
        }
        protected void ShowUploadedCBDoc(string pLnAppId)
        {
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = OMem.GetUploadCBDocByLoanId(pLnAppId, "CB");
                if (dt.Rows.Count > 0)
                {
                    gvCustCBDoc.DataSource = dt;
                    gvCustCBDoc.DataBind();
                }
                else
                {
                    gvCustCBDoc.DataSource = null;
                    gvCustCBDoc.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
                dt = null;
            }
        }
        protected void ShowUploadedFIDoc(string pLnAppId)
        {
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = OMem.GetFIDocByLnAppId(pLnAppId);
                if (dt.Rows.Count > 0)
                {
                    gvFIDocs.DataSource = dt;
                    gvFIDocs.DataBind();
                }
                else
                {
                    gvFIDocs.DataSource = null;
                    gvFIDocs.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
                dt = null;
            }
        }
        protected void btnComUpload_Click(object sender, EventArgs e)
        {

            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Company Documents..");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLnAppId = "", vFileName = "", vFilePath = "";
                vLnAppId = ViewState["LoanAppId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fileUpload1.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select Company CB File to upload");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLnAppId + "-" + "1.pdf";
                        vFilePath = "../../../CBDocuments/" + vFileName;
                        string path = Server.MapPath("../../../CBDocuments/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            vDel = OMem.DeleteCBDocuments(vLnAppId, vFileName, vFilePath, 0);
                        }

                        fileUpload1.SaveAs(Server.MapPath("../../../CBDocuments/" + vFileName));
                        vErr = OMem.SaveCBDocuments(vLnAppId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("Company CB File Uploaded Successfully");
                            ShowUploadedDocuments(vLnAppId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in Company CB File Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }
            }

        }
        protected void btnCustUpload_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Customer Documents..");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLnAppId = "", vFileName = "", vFilePath = "";
                vLnAppId = ViewState["LoanAppId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fileUpload2.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select Customer CB File to upload");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLnAppId + "-" + "2.pdf";
                        vFilePath = "../../../CBDocuments/" + vFileName;
                        string path = Server.MapPath("../../../CBDocuments/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            vDel = OMem.DeleteCBDocuments(vLnAppId, vFileName, vFilePath, 0);
                        }

                        fileUpload2.SaveAs(Server.MapPath("../../../CBDocuments/" + vFileName));
                        vErr = OMem.SaveCBDocuments(vLnAppId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("Customer CB File Uploaded Successfully");
                            ShowUploadedDocuments(vLnAppId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in Customer CB File Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }

            }
        }
        protected void btnCoAppUpload1_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Co Apllicant Document1..");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLnAppId = "", vFileName = "", vFilePath = "";
                vLnAppId = ViewState["LoanAppId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fileUpload3.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select  Co-Applicant CB File (i) File to upload");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLnAppId + "-" + "3.pdf";
                        vFilePath = "../../../CBDocuments/" + vFileName;
                        string path = Server.MapPath("../../../CBDocuments/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            vDel = OMem.DeleteCBDocuments(vLnAppId, vFileName, vFilePath, 0);
                        }

                        fileUpload3.SaveAs(Server.MapPath("../../../CBDocuments/" + vFileName));
                        vErr = OMem.SaveCBDocuments(vLnAppId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("Co-Applicant CB File (i) Uploaded Successfully");
                            ShowUploadedDocuments(vLnAppId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in  Co-Applicant CB File (i) Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }

            }
        }
        protected void btnCoAppUpload2_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Co Apllicant Document2..");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLnAppId = "", vFileName = "", vFilePath = "";
                vLnAppId = ViewState["LoanAppId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fileUpload4.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select  Co-Applicant CB File (ii) File to upload");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLnAppId + "-" + "4.pdf";
                        vFilePath = "../../../CBDocuments/" + vFileName;
                        string path = Server.MapPath("../../../CBDocuments/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            vDel = OMem.DeleteCBDocuments(vLnAppId, vFileName, vFilePath, 0);
                        }

                        fileUpload4.SaveAs(Server.MapPath("../../../CBDocuments/" + vFileName));
                        vErr = OMem.SaveCBDocuments(vLnAppId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("Co-Applicant CB File (ii) Uploaded Successfully");
                            ShowUploadedDocuments(vLnAppId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in  Co-Applicant CB File (ii) Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }

            }
        }
        protected void btnGuranUpload1_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Guarantor Document1..");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLnAppId = "", vFileName = "", vFilePath = "";
                vLnAppId = ViewState["LoanAppId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fileUpload5.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select  Guarantor CB File (i) File to upload");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLnAppId + "-" + "5.pdf";
                        vFilePath = "../../../CBDocuments/" + vFileName;
                        string path = Server.MapPath("../../../CBDocuments/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            vDel = OMem.DeleteCBDocuments(vLnAppId, vFileName, vFilePath, 0);
                        }

                        fileUpload5.SaveAs(Server.MapPath("../../../CBDocuments/" + vFileName));
                        vErr = OMem.SaveCBDocuments(vLnAppId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("Guarantor CB File (i) Uploaded Successfully");
                            ShowUploadedDocuments(vLnAppId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in  Guarantor CB File (i) Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }

            }
        }
        protected void btnGuranUpload2_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Guarantor Document2...");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLnAppId = "", vFileName = "", vFilePath = "";
                vLnAppId = ViewState["LoanAppId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fileUpload6.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select  Guarantor CB File (ii) File to upload");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLnAppId + "-" + "6.pdf";
                        vFilePath = "../../../CBDocuments/" + vFileName;
                        string path = Server.MapPath("../../../CBDocuments/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            vDel = OMem.DeleteCBDocuments(vLnAppId, vFileName, vFilePath, 0);
                        }

                        fileUpload6.SaveAs(Server.MapPath("../../../CBDocuments/" + vFileName));
                        vErr = OMem.SaveCBDocuments(vLnAppId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("Guarantor CB File (ii) Uploaded Successfully");
                            ShowUploadedDocuments(vLnAppId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in  Guarantor CB File (ii) Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }

            }
        }
        protected void btnGuranUpload3_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Guarantor Document3....");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLnAppId = "", vFileName = "", vFilePath = "";
                vLnAppId = ViewState["LoanAppId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fileUpload7.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select  Guarantor CB File (iii)  to upload");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLnAppId + "-" + "7.pdf";
                        vFilePath = "../../../CBDocuments/" + vFileName;
                        string path = Server.MapPath("../../../CBDocuments/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            vDel = OMem.DeleteCBDocuments(vLnAppId, vFileName, vFilePath, 0);
                        }

                        fileUpload7.SaveAs(Server.MapPath("../../../CBDocuments/" + vFileName));
                        vErr = OMem.SaveCBDocuments(vLnAppId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("Guarantor CB File (iii) Uploaded Successfully");
                            ShowUploadedDocuments(vLnAppId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in  Guarantor CB File (iii) Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }

            }
        }
        protected void btnGuranUpload4_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Guarantor Document4....");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLnAppId = "", vFileName = "", vFilePath = "";
                vLnAppId = ViewState["LoanAppId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fileUpload8.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select  Guarantor CB File (iv) to upload");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLnAppId + "-" + "8.pdf";
                        vFilePath = "../../../CBDocuments/" + vFileName;
                        string path = Server.MapPath("../../../CBDocuments/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            vDel = OMem.DeleteCBDocuments(vLnAppId, vFileName, vFilePath, 0);
                        }

                        fileUpload8.SaveAs(Server.MapPath("../../../CBDocuments/" + vFileName));
                        vErr = OMem.SaveCBDocuments(vLnAppId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("Guarantor CB File (iv) Uploaded Successfully");
                            ShowUploadedDocuments(vLnAppId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in  Guarantor CB File (iv) Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }

            }
        }
        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string filePath = gvUpDetails.DataKeys[gvrow.RowIndex].Value.ToString();
                Response.ContentType = "pdf";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filePath + "\"");
                Response.TransmitFile(Server.MapPath(filePath));
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        double LnAmt = 0;
        double POS = 0;
        double EMI = 0;
        protected void gvCB_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblLnAmt = (Label)e.Row.FindControl("lblLnAmt");
                string active = e.Row.Cells[4].Text.ToString();
                if (lblLnAmt.Text != "" && active == "Active")
                    LnAmt += Double.Parse(lblLnAmt.Text);
                Label lblPOSAmt = (Label)e.Row.FindControl("lblPOSAmt");
                if (lblPOSAmt.Text != "" && active == "Active")
                    POS += Double.Parse(lblPOSAmt.Text);
                Label lblEMI = (Label)e.Row.FindControl("lblEMI");
                if (lblEMI.Text != "" && active == "Active")
                    EMI += Double.Parse(lblEMI.Text);
                //Label lblPMonth7 = (Label)e.Row.FindControl("lblPMonth7");

                //if (lblPMonth7.Text == "")
                //{
                //  //  gvCB.Columns[18].Visible = false;
                //    //gvCB.HeaderRow.Cells[18].Visible = false;
                //    //e.Row.Cells[18].Visible = false;
                //}
                //else
                //{
                //  //  gvCB.Columns[18].Visible =true;
                //    //gvCB.HeaderRow.Cells[18].Visible = true;
                //    //e.Row.Cells[18].Visible = true;
                //}
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotLnAmt = (Label)e.Row.FindControl("lblTotLnAmt");
                lblTotLnAmt.Text = LnAmt.ToString();

                Label lblTotPOSAmt = (Label)e.Row.FindControl("lblTotPOSAmt");
                lblTotPOSAmt.Text = POS.ToString();

                Label lblTotEMI = (Label)e.Row.FindControl("lblTotEMI");
                lblTotEMI.Text = EMI.ToString();
            }
        }
        protected void gvBSPLDoc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vAttachAppDocUpID = 0;
            DataTable dt = null;
            CDocUpLoad oAD = null;
            try
            {
                vAttachAppDocUpID = Convert.ToInt32(e.CommandArgument);
                ViewState["DocUpID"] = vAttachAppDocUpID;
                if (e.CommandName == "cmdDownld")
                {
                    string vFileName = "";
                    string vFileTyp = "";
                    GridViewRow gvR = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    ImageButton imgDownload = (ImageButton)gvR.FindControl("imgDownload");
                    oAD = new CDocUpLoad();
                    dt = oAD.GetBSPLDocUpAttachById(vAttachAppDocUpID);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["AttachDoc"].ToString() != "")
                        {
                            byte[] fileData = (byte[])dt.Rows[0]["AttachDoc"];
                            vFileName = dt.Rows[0]["AttachDocName"].ToString();
                            vFileTyp = GetFileType(Convert.ToString(dt.Rows[0]["AttachType"]));
                            Response.Clear();
                            Response.Buffer = true;
                            Response.ContentType = vFileTyp;
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + vFileName + "" + Convert.ToString(dt.Rows[0]["AttachType"]));
                            BinaryWriter bw = new BinaryWriter(Response.OutputStream);
                            bw.Write(fileData);
                            bw.Close();
                            Response.End();
                        }
                        else
                        {
                            gblFuction.MsgPopup("No Attachment Found");
                            return;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("No Attachment Found");
                        return;
                    }
                }
            }
            finally
            {
                //dt.Dispose();
            }
        }
        private string GetFileType(string pFileTyp)
        {
            string vRst = "";
            switch (pFileTyp)
            {
                case ".txt":
                    vRst = "application/notepad";
                    break;
                case ".doc":
                    vRst = "application/ms-word";
                    break;
                case ".docx":
                    vRst = "application/vnd.ms-word.document.12";
                    break;
                case ".xls":
                    vRst = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    vRst = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".pdf":
                    vRst = "application/vnd.pdf";
                    break;
                case ".zip":
                    vRst = "application/zip";
                    break;
                case ".rar":
                    vRst = "application/WinRar";
                    break;
                case ".exe":
                    vRst = "application/executable";
                    break;
                default:
                    vRst = "";
                    break;
            }
            return vRst;
        }

        protected void ddlLoanApplicantname_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            string vCustId = ddlLoanApplicantname.SelectedValue.ToString();
            try
            {
                dt = OMem.GetCoAppByCustId(vCustId);
                if (dt.Rows.Count > 0)
                {
                    gvCoAppDtl.DataSource = dt;
                    gvCoAppDtl.DataBind();
                }
                else
                {
                    gvCoAppDtl.DataSource = null;
                    gvCoAppDtl.DataBind();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                OMem = null;
            }
        }
        protected void gvCBDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlCBname = (e.Row.FindControl("ddlCBname") as DropDownList);
                Label lblCBNameId = (e.Row.FindControl("lblCBNameId") as Label);
                Label lblIsActive = (e.Row.FindControl("lblIsActive") as Label);
                CheckBox chkCBIsActive = (e.Row.FindControl("chkCBIsActive") as CheckBox);
                Button btnAddCBRow = (e.Row.FindControl("btnAddCBRow") as Button);
                ImageButton ImDelCB = (e.Row.FindControl("ImDelCB") as ImageButton);
                try
                {
                    dt = oMem.GetCBName();
                    if (dt.Rows.Count > 0)
                    {
                        ddlCBname.DataSource = dt;
                        ddlCBname.DataTextField = "CBName";
                        ddlCBname.DataValueField = "CBNameId";
                        ddlCBname.DataBind();
                        ListItem oli1 = new ListItem("<--Select-->", "-1");
                        ddlCBname.Items.Insert(0, oli1);
                    }
                    string Comment = lblCBNameId.Text;
                    if (Comment != " ")
                        ddlCBname.SelectedValue = Comment;
                    if (lblIsActive.Text == "N" || lblIsActive.Text == "")
                    {
                        chkCBIsActive.Checked = false;
                    }
                    else
                    {
                        chkCBIsActive.Checked = true;
                    }

                    // change visibility of Add and Delete record with in gridview
                    if (hdfCBId.Value != "")
                    {
                        btnAddCBRow.Visible = false;
                        ImDelCB.Visible = false;
                    }
                    else
                    {
                        btnAddCBRow.Visible = true;
                        ImDelCB.Visible = true;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dt = null;
                    oMem = null;
                }
            }
        }
        protected void gvBankAcSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DataTable dt = (DataTable)Session["AccountsDt"];
                    if (dt.Rows.Count > 0)
                    {
                        int y = dt.Columns.Count;
                        int i = 11;
                        //Tot O/W Return No
                        e.Row.Cells[y - 1].Text = dt.AsEnumerable().Select(x => x.Field<int>(y - 1)).Sum().ToString();
                        //Tot EMI Return No
                        e.Row.Cells[y - 2].Text = dt.AsEnumerable().Select(x => x.Field<int>(y - 2)).Sum().ToString();
                        //Tot IW Return No
                        e.Row.Cells[y - 3].Text = dt.AsEnumerable().Select(x => x.Field<int>(y - 3)).Sum().ToString();
                        //Tot Sales Fig
                        e.Row.Cells[y - 4].Text = dt.AsEnumerable().Select(x => x.Field<double>(y - 4)).Sum().ToString();
                        //Tot Credit Sum
                        e.Row.Cells[y - 5].Text = dt.AsEnumerable().Select(x => x.Field<double>(y - 5)).Sum().ToString();
                        //Tot O/W Amt
                        e.Row.Cells[y - 6].Text = dt.AsEnumerable().Select(x => x.Field<double>(y - 6)).Sum().ToString();
                        //Tot EMI Amt
                        e.Row.Cells[y - 7].Text = dt.AsEnumerable().Select(x => x.Field<double>(y - 7)).Sum().ToString();
                        //Tot I/W Amt
                        e.Row.Cells[y - 8].Text = dt.AsEnumerable().Select(x => x.Field<double>(y - 8)).Sum().ToString();
                        //Tot Loan Availed
                        e.Row.Cells[y - 9].Text = dt.AsEnumerable().Select(x => x.Field<double>(y - 9)).Sum().ToString();
                        //Tot Transfer
                        e.Row.Cells[y - 10].Text = dt.AsEnumerable().Select(x => x.Field<double>(y - 10)).Sum().ToString();
                        for (i = 11; i < y; i++)
                        {
                            // Accountwise Tot Credit
                            e.Row.Cells[y - i].Text = dt.AsEnumerable().Select(x => x.Field<double>(y - i)).Sum().ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void gvITR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblITRFiledYN = (e.Row.FindControl("lblITRFiledYN") as Label);
                CheckBox chkITRFile = (e.Row.FindControl("chkITRFile") as CheckBox);
                Button btnAddITR = (e.Row.FindControl("btnAddITR") as Button);
                ImageButton ImDelITR = (e.Row.FindControl("ImDelITR") as ImageButton);
                try
                {
                    if (lblITRFiledYN.Text == "N" || lblITRFiledYN.Text == "")
                    {
                        chkITRFile.Checked = false;
                    }
                    else
                    {
                        chkITRFile.Checked = true;
                    }

                    // change visibility of Add and Delete record with in gridview
                    //if (hdfCBId.Value != "")
                    //{
                    //    btnAddITR.Visible = false;
                    //    ImDelITR.Visible = false;
                    //}
                    //else
                    //{
                    //    btnAddITR.Visible = true;
                    //    ImDelITR.Visible = true;
                    //}
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dt = null;
                    oMem = null;
                }
            }
        }
        protected void gvViewITR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblVITRFiledYN = (e.Row.FindControl("lblVITRFiledYN") as Label);
                CheckBox chkVITRFile = (e.Row.FindControl("chkVITRFile") as CheckBox);
                try
                {
                    if (lblVITRFiledYN.Text == "N" || lblVITRFiledYN.Text == "")
                    {
                        chkVITRFile.Checked = false;
                        chkVITRFile.Enabled = false;
                    }
                    else
                    {
                        chkVITRFile.Checked = true;
                        chkVITRFile.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dt = null;
                    oMem = null;
                }
            }
        }
        protected void gvPLList_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            decimal FYear = 0; decimal SYear = 0; decimal TYear = 0;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                TableHeaderCell NewCell = new TableHeaderCell();
                NewCell.Text = "Change(%)";
                e.Row.Cells.AddAt(5, NewCell);

                TableHeaderCell NewCell1 = new TableHeaderCell();
                NewCell1.Text = "Change(%)";
                e.Row.Cells.AddAt(7, NewCell1);
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int SlNo = int.Parse(e.Row.Cells[1].Text);
                if (SlNo == 5 || SlNo == 7 || SlNo == 11 || SlNo == 12 || SlNo == 17 || SlNo == 18 || SlNo == 20 || SlNo == 23 || SlNo == 25 || SlNo == 26)
                {
                    e.Row.BackColor = System.Drawing.Color.SkyBlue;
                    e.Row.Font.Bold = true;
                }

                if (e.Row.Cells[3].Text != "&nbsp;")
                    FYear = Convert.ToDecimal(e.Row.Cells[3].Text);
                if (e.Row.Cells[4].Text != "&nbsp;")
                    SYear = Convert.ToDecimal(e.Row.Cells[4].Text);
                if (e.Row.Cells[5].Text != "&nbsp;")
                    TYear = Convert.ToDecimal(e.Row.Cells[5].Text);

                TableCell NewCell = new TableCell();
                NewCell.ID = "NewCell";
                if (FYear != 0)
                    NewCell.Text = Math.Round((((SYear - FYear) / FYear) * 100), 2).ToString();
                else
                    NewCell.Text = "0";
                e.Row.Cells.AddAt(5, NewCell);

                TableCell NewCell1 = new TableCell();
                NewCell1.ID = "NewCell1";
                if (SYear != 0)
                    NewCell1.Text = Math.Round((((TYear - SYear) / SYear) * 100), 2).ToString();
                else
                    NewCell1.Text = "0";
                e.Row.Cells.AddAt(7, NewCell1);
            }
        }
        protected void gvBSList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal FYear = 0; decimal SYear = 0; decimal TYear = 0;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                TableHeaderCell NewCell = new TableHeaderCell();
                NewCell.Text = "Change(%)";
                e.Row.Cells.AddAt(5, NewCell);

                TableHeaderCell NewCell1 = new TableHeaderCell();
                NewCell1.Text = "Change(%)";
                e.Row.Cells.AddAt(7, NewCell1);

                //e.Row.Cells[0].Width = new Unit("1%");
                //e.Row.Cells[1].Width = new Unit("1%");
                //e.Row.Cells[2].Width = new Unit("5%");
                //e.Row.Cells[3].Width = new Unit("1%");
                //e.Row.Cells[4].Width = new Unit("1%");
                //e.Row.Cells[5].Width = new Unit("1%");
                //e.Row.Cells[6].Width = new Unit("1%");
                //e.Row.Cells[7].Width = new Unit("1%");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int SlNo = int.Parse(e.Row.Cells[1].Text);
                if (SlNo == 4 || SlNo == 8 || SlNo == 11 || SlNo == 14 || SlNo == 15 || SlNo == 19 || SlNo == 24 || SlNo == 25 || SlNo == 30 || SlNo == 34 || SlNo == 39 || SlNo == 44 || SlNo == 46)
                {
                    e.Row.BackColor = System.Drawing.Color.SkyBlue;
                    e.Row.Font.Bold = true;
                }


                if (e.Row.Cells[3].Text != "&nbsp;")
                    FYear = Convert.ToDecimal(e.Row.Cells[3].Text);
                if (e.Row.Cells[4].Text != "&nbsp;")
                    SYear = Convert.ToDecimal(e.Row.Cells[4].Text);
                if (e.Row.Cells[5].Text != "&nbsp;")
                    TYear = Convert.ToDecimal(e.Row.Cells[5].Text);

                TableCell NewCell = new TableCell();
                NewCell.ID = "NewCell";
                if (FYear != 0)
                    NewCell.Text = Math.Round((((SYear - FYear) / FYear) * 100), 2).ToString();
                else
                    NewCell.Text = "0";
                e.Row.Cells.AddAt(5, NewCell);

                TableCell NewCell1 = new TableCell();
                NewCell1.ID = "NewCell1";
                if (SYear != 0)
                    NewCell1.Text = Math.Round((((TYear - SYear) / SYear) * 100), 2).ToString();
                else
                    NewCell1.Text = "0";
                e.Row.Cells.AddAt(7, NewCell1);

                //e.Row.Cells[0].Width = new Unit("1%");
                //e.Row.Cells[1].Width = new Unit("1%");
                //e.Row.Cells[2].Width = new Unit("5%");
                //e.Row.Cells[3].Width = new Unit("1%");
                //e.Row.Cells[4].Width = new Unit("1%");
                //e.Row.Cells[5].Width = new Unit("1%");
                //e.Row.Cells[6].Width = new Unit("1%");
                //e.Row.Cells[7].Width = new Unit("1%");
            }
        }
        protected void gvRatio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.BackColor = System.Drawing.Color.SkyBlue;
                e.Row.Font.Bold = true;
            }
        }
        protected void EnableMachinDtl(int LnType)
        {
            try
            {
                // 1 for BL,2 for ML, 3 for STL
                if (LnType == 2)
                {
                    gvMLAsset.Enabled = true;
                }
                else
                {
                    gvMLAsset.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnUploadBankStat_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Bank Statement...");
                return;
            }
            string pLoanAppId = ViewState["LoanAppId"].ToString();
            string vAttchBankDocType = "", vDocDesc = "BS";
            byte[] vAttachOffDoc = null;
            string vAttchOffDocName = "";
            CMember oMem = new CMember();
            int pDocID = 0;
            int vError = 0;
            if (txtBSupDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Please Input Bank Statement Upload Date....");
                return;
            }
            if (fuBankStat.HasFile == false)
            {
                gblFuction.MsgPopup("Please Choose a file to attach....");
                return;
            }

            if (fuBankStat.PostedFile.InputStream.Length > 4194304)
            {
                gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
                return;
            }
            if (fuBankStat.HasFile)
            {
                vAttachOffDoc = new byte[fuBankStat.PostedFile.InputStream.Length + 1];
                fuBankStat.PostedFile.InputStream.Read(vAttachOffDoc, 0, vAttachOffDoc.Length);
                vAttchBankDocType = System.IO.Path.GetExtension(fuBankStat.FileName).ToLower();
                vAttchOffDocName = System.IO.Path.GetFileNameWithoutExtension(fuBankStat.FileName);
            }
            DateTime pUploadDate = gblFuction.setDate(txtBSupDate.Text.Trim());
            vError = oMem.SaveCustDoc(pDocID, pLoanAppId, vDocDesc, pUploadDate, vAttachOffDoc, vAttchBankDocType, vAttchOffDocName, Convert.ToInt32(hdUserID.Value), "Save", 0);
            if (vError == 0)
            {
                gblFuction.AjxMsgPopup("Bank Statement Uploaded Successfully......");
                ShowUploadedBSDoc(pLoanAppId);
                return;
            }
            else
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
            }
        }
        protected void gvCustBankDoc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDownld")
            {
                int vDocId = Convert.ToInt32(e.CommandArgument.ToString());
                DataTable dt = null;
                CDocUpLoad oAD = null;
                string vFileTyp = "";
                string vFileName = "";
                GridViewRow gvR = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton imgDownload = (ImageButton)gvR.FindControl("imgDownload");
                oAD = new CDocUpLoad();
                dt = oAD.GetCustBankDoc(Convert.ToInt32(vDocId));
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["AttachDoc"].ToString() != "")
                    {
                        byte[] fileData = (byte[])dt.Rows[0]["AttachDoc"];
                        vFileTyp = GetFileType(Convert.ToString(dt.Rows[0]["AttachType"]));
                        vFileName = dt.Rows[0]["AttachDocName"].ToString();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = vFileTyp;
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + vFileName + "" + Convert.ToString(dt.Rows[0]["AttachType"]));
                        BinaryWriter bw = new BinaryWriter(Response.OutputStream);
                        bw.Write(fileData);
                        bw.Close();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("No Attachment Found");
                        return;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("No Attachment Found");
                    return;
                }
            }

        }
        protected void btnUploadCBDoc_Click(object sender, EventArgs e)
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Credit Bureau...");
                return;
            }
            dt = oMem.GetLnAppPassYNByLoanid(ViewState["LoanAppId"].ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["LnAppPassYN"].ToString() == "N")
                {
                    gblFuction.MsgPopup("You Can Not Upload CB Documents unless it passed from Loan Application Section...");
                    return;
                }
            }
            else
            {
                gblFuction.MsgPopup("Please Tick Pass/Reject in Loan Application...");
            }
            string pLoanAppId = ViewState["LoanAppId"].ToString();
            string vAttchBankDocType = "", vDocDesc = "CB";
            byte[] vAttachOffDoc = null;
            string vAttchOffDocName = "";

            int pDocID = 0;
            int vError = 0;
            if (txtCBupDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Please Input Credit Bureau Upload Date....");
                return;
            }
            if (fuCustCBDoc.HasFile == false)
            {
                gblFuction.MsgPopup("Please Choose a file to attach....");
                return;
            }

            if (fuCustCBDoc.PostedFile.InputStream.Length > 4194304)
            {
                gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
                return;
            }
            if (fuCustCBDoc.HasFile)
            {
                vAttachOffDoc = new byte[fuCustCBDoc.PostedFile.InputStream.Length + 1];
                fuCustCBDoc.PostedFile.InputStream.Read(vAttachOffDoc, 0, vAttachOffDoc.Length);
                vAttchBankDocType = System.IO.Path.GetExtension(fuCustCBDoc.FileName).ToLower();
                vAttchOffDocName = System.IO.Path.GetFileNameWithoutExtension(fuCustCBDoc.FileName);
            }
            DateTime pUploadDate = gblFuction.setDate(txtCBupDate.Text.Trim());
            vError = oMem.SaveCustDoc(pDocID, pLoanAppId, vDocDesc, pUploadDate, vAttachOffDoc, vAttchBankDocType, vAttchOffDocName, Convert.ToInt32(hdUserID.Value), "Save", 0);
            if (vError == 0)
            {
                gblFuction.AjxMsgPopup("Credit Bureau Document Uploaded Successfully......");
                ShowUploadedCBDoc(pLoanAppId);
                return;
            }
            else
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
            }
        }
        protected void gvCustCBDoc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDownld")
            {
                int vDocId = Convert.ToInt32(e.CommandArgument.ToString());
                DataTable dt = null;
                CDocUpLoad oAD = null;
                string vFileTyp = "";
                string vFileName = "";
                GridViewRow gvR = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton imgDownload = (ImageButton)gvR.FindControl("imgDownload");
                oAD = new CDocUpLoad();
                dt = oAD.GetCustBankDoc(Convert.ToInt32(vDocId));
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["AttachDoc"].ToString() != "")
                    {
                        byte[] fileData = (byte[])dt.Rows[0]["AttachDoc"];
                        vFileTyp = GetFileType(Convert.ToString(dt.Rows[0]["AttachType"]));
                        vFileName = dt.Rows[0]["AttachDocName"].ToString();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = vFileTyp;
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + vFileName + "" + Convert.ToString(dt.Rows[0]["AttachType"]));
                        BinaryWriter bw = new BinaryWriter(Response.OutputStream);
                        bw.Write(fileData);
                        bw.Close();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("No Attachment Found");
                        return;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("No Attachment Found");
                    return;
                }
            }

        }
        protected void gvFIDocs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDownld")
            {
                int vDocId = Convert.ToInt32(e.CommandArgument.ToString());
                DataTable dt = null;
                CDocUpLoad oAD = null;
                string vFileTyp = "";
                string vFileName = "";
                GridViewRow gvR = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton imgDownload = (ImageButton)gvR.FindControl("imgDownload");
                oAD = new CDocUpLoad();
                dt = oAD.GetFIDoc(Convert.ToInt32(vDocId));
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["AttachDoc"].ToString() != "")
                    {
                        byte[] fileData = (byte[])dt.Rows[0]["AttachDoc"];
                        vFileTyp = GetFileType(Convert.ToString(dt.Rows[0]["AttachType"]));
                        vFileName = dt.Rows[0]["AttachDocName"].ToString();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = vFileTyp;
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + vFileName + "" + Convert.ToString(dt.Rows[0]["AttachType"]));
                        BinaryWriter bw = new BinaryWriter(Response.OutputStream);
                        bw.Write(fileData);
                        bw.Close();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("No Attachment Found");
                        return;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("No Attachment Found");
                    return;
                }
            }

        }
        protected void gvBSList_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //
            }
        }
        protected void btnFIOff_Click(object sender, EventArgs e)
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload FI Documents...");
                return;
            }
            dt = oMem.GetLnAppPassYNByLoanid(ViewState["LoanAppId"].ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["LnAppPassYN"].ToString() == "N")
                {
                    gblFuction.MsgPopup("You Can Not Upload FI Documents unless it passed from Loan Application Section...");
                    return;
                }
            }
            else
            {
                gblFuction.MsgPopup("Please Tick Pass/Reject in Loan Application...");
            }
            string pLoanAppId = ViewState["LoanAppId"].ToString();
            string vAttchOffDocType = "", vDocDesc = "Office Address";
            byte[] vAttachOffDoc = null;
            string vAttchOffDocName = "";

            int pDocID = 0;
            int vError = 0;

            if (fuFIOffice.HasFile == false)
            {
                gblFuction.MsgPopup("Please Choose a file to attach....");
                return;
            }

            if (fuFIOffice.PostedFile.InputStream.Length > 4194304)
            {
                gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
                return;
            }
            if (fuFIOffice.HasFile)
            {
                vAttachOffDoc = new byte[fuFIOffice.PostedFile.InputStream.Length + 1];
                fuFIOffice.PostedFile.InputStream.Read(vAttachOffDoc, 0, vAttachOffDoc.Length);
                vAttchOffDocType = System.IO.Path.GetExtension(fuFIOffice.FileName).ToLower();
                vAttchOffDocName = System.IO.Path.GetFileNameWithoutExtension(fuFIOffice.FileName);
            }
            vError = oMem.SaveFIDoc(pDocID, pLoanAppId, vDocDesc, vAttachOffDoc, vAttchOffDocType, vAttchOffDocName, Convert.ToInt32(hdUserID.Value), "Save", 0);
            if (vError == 0)
            {
                gblFuction.AjxMsgPopup("FI Document Uploaded Successfully......");
                ShowUploadedFIDoc(pLoanAppId);
                return;
            }
            else
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
            }
        }
        protected void btnFIRes_Click(object sender, EventArgs e)
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload FI Documents...");
                return;
            }
            dt = oMem.GetLnAppPassYNByLoanid(ViewState["LoanAppId"].ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["LnAppPassYN"].ToString() == "N")
                {
                    gblFuction.MsgPopup("You Can Not Upload FI Documents unless it passed from Loan Application Section...");
                    return;
                }
            }
            else
            {
                gblFuction.MsgPopup("Please Tick Pass/Reject in Loan Application...");
            }
            string pLoanAppId = ViewState["LoanAppId"].ToString();
            string vAttchResDocType = "", vDocDesc = "Residence Address";
            byte[] vAttachResDoc = null;
            string vAttchResDocName = "";

            int pDocID = 0;
            int vError = 0;

            if (fuFIResidence.HasFile == false)
            {
                gblFuction.MsgPopup("Please Choose a file to attach....");
                return;
            }

            if (fuFIResidence.PostedFile.InputStream.Length > 4194304)
            {
                gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
                return;
            }
            if (fuFIResidence.HasFile)
            {
                vAttachResDoc = new byte[fuFIResidence.PostedFile.InputStream.Length + 1];
                fuFIResidence.PostedFile.InputStream.Read(vAttachResDoc, 0, vAttachResDoc.Length);
                vAttchResDocType = System.IO.Path.GetExtension(fuFIResidence.FileName).ToLower();
                vAttchResDocName = System.IO.Path.GetFileNameWithoutExtension(fuFIResidence.FileName);
            }
            vError = oMem.SaveFIDoc(pDocID, pLoanAppId, vDocDesc, vAttachResDoc, vAttchResDocType, vAttchResDocName, Convert.ToInt32(hdUserID.Value), "Save", 0);
            if (vError == 0)
            {
                gblFuction.AjxMsgPopup("FI Document Uploaded Successfully......");
                ShowUploadedFIDoc(pLoanAppId);
                return;
            }
            else
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
            }
        }
        protected void btnFIGua_Click(object sender, EventArgs e)
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload FI Documents...");
                return;
            }
            dt = oMem.GetLnAppPassYNByLoanid(ViewState["LoanAppId"].ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["LnAppPassYN"].ToString() == "N")
                {
                    gblFuction.MsgPopup("You Can Not Upload FI Documents unless it passed from Loan Application Section...");
                    return;
                }
            }
            else
            {
                gblFuction.MsgPopup("Please Tick Pass/Reject in Loan Application...");
            }
            string pLoanAppId = ViewState["LoanAppId"].ToString();
            string vAttchGuaDocType = "", vDocDesc = "Guarantor";
            byte[] vAttachGuaDoc = null;
            string vAttchGuaDocName = "";

            int pDocID = 0;
            int vError = 0;

            if (fuFIGuarantor.HasFile == false)
            {
                gblFuction.MsgPopup("Please Choose a file to attach....");
                return;
            }

            if (fuFIGuarantor.PostedFile.InputStream.Length > 4194304)
            {
                gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
                return;
            }
            if (fuFIGuarantor.HasFile)
            {
                vAttachGuaDoc = new byte[fuFIGuarantor.PostedFile.InputStream.Length + 1];
                fuFIGuarantor.PostedFile.InputStream.Read(vAttachGuaDoc, 0, vAttachGuaDoc.Length);
                vAttchGuaDocType = System.IO.Path.GetExtension(fuFIGuarantor.FileName).ToLower();
                vAttchGuaDocName = System.IO.Path.GetFileNameWithoutExtension(fuFIGuarantor.FileName);
            }
            vError = oMem.SaveFIDoc(pDocID, pLoanAppId, vDocDesc, vAttachGuaDoc, vAttchGuaDocType, vAttchGuaDocName, Convert.ToInt32(hdUserID.Value), "Save", 0);
            if (vError == 0)
            {
                gblFuction.AjxMsgPopup("FI Document Uploaded Successfully......");
                ShowUploadedFIDoc(pLoanAppId);
                return;
            }
            else
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
            }
        }

    }

    public class RootObjectResponse
    {
        public bool Result { get; set; }
        public string Data { get; set; }
    }
}
