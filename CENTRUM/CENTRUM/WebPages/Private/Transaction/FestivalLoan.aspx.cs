using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FORCEBA;
using FORCECA;
using CENTRUM.Service_Equifax;
using CENTRUM.Service_Equifax_CCR;
using System.Configuration;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class FestivalLoan : CENTRUMBase
    {
        string CCRUserName = ConfigurationManager.AppSettings["CCRUserName"];
        string CCRPassword = ConfigurationManager.AppSettings["CCRPassword"];
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                popRO();
                lblBranch.Text = Convert.ToString(Session[gblValue.BrName]);
                txtDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Pre Approved Loan";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuPreApproveLoan);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnDone.Visible = false;
                    // btnClose.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnDone.Visible = true;
                    //  btnClose.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDone.Visible = true;
                    /// btnClose.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnDone.Visible = true;
                    //  btnClose.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Pre Approved Loan", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode;
            vBrCode = Convert.ToString(Session[gblValue.BrnchCode]);
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlLo.DataSource = dt;
                ddlLo.DataTextField = "EoName";
                ddlLo.DataValueField = "EoId";
                ddlLo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlLo.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        protected void ddlSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> Tenure = null;
            if (ddlSchedule.SelectedValue == "M")
            {
                Tenure = new List<string> { "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
            }
            else if (ddlSchedule.SelectedValue == "W")
            {
                Tenure = new List<string> {"26","27","28","29","30","31","32","33","34","35","36","37","38","39","40","41","42","43","44","45","46","47","48","49","50","51","52","53","54",
                "55","56","57","58","59","60","61","62","63","64","65","66","67","68","69","70","71","72","73","74","75","76","77","78","79","80","81","82","83","84","85","86"
                ,"87","88","89","90","91","92","93","94","95","96","97","98","99","100","101","102","103"};
            }
            ddlTenure.DataSource = Tenure;
            ddlTenure.DataBind();
        }

        protected void gvFestivLoan_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlLoanType = (DropDownList)e.Row.FindControl("ddlLoanType");
                DropDownList ddlPurpose = (DropDownList)e.Row.FindControl("ddlPurpose");
                DropDownList ddlSubPurpose = (DropDownList)e.Row.FindControl("ddlSubPurpose");
                DataTable dt = null;
                CApplication oApp = null;
                CSubPurpose oGb = null;
                try
                {
                    oApp = new CApplication();
                    dt = oApp.GetLoanTypeForApp("N", Convert.ToString(Session[gblValue.BrnchCode]), "Y");
                    ddlLoanType.DataSource = dt;
                    ddlLoanType.DataTextField = "LoanType";
                    ddlLoanType.DataValueField = "LoanTypeId";
                    ddlLoanType.DataBind();
                    ListItem oLi = new ListItem("<--Select-->", "-1");
                    ddlLoanType.Items.Insert(0, oLi);

                    oGb = new CSubPurpose();
                    dt = oGb.PopPurpose(gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate])));
                    ddlPurpose.DataSource = dt;
                    ddlPurpose.DataTextField = "Purpose";
                    ddlPurpose.DataValueField = "PurposeID";
                    ddlPurpose.DataBind();
                    ddlPurpose.Items.Insert(0, oLi);

                }
                finally
                {
                    dt = null;
                    oApp = null;
                }
            }
        }

        protected void ddlLoanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CApplication oAp = null;
            DataTable dt, dt1 = new DataTable();
            try
            {
                DropDownList dropdown = (DropDownList)sender;
                GridViewRow row = (GridViewRow)dropdown.NamingContainer;
                DropDownList ddlLoanType = (DropDownList)row.FindControl("ddlLoanType");
                DropDownList ddlTenure = (DropDownList)row.FindControl("ddlTenure");
                DropDownList ddlLnAmount = (DropDownList)row.FindControl("ddlLnAmount");

                oAp = new CApplication();
                dt = oAp.GetTenureByLoanTypeId(Convert.ToInt32(ddlLoanType.SelectedValue));
                ddlTenure.DataSource = dt;
                ddlTenure.DataTextField = "Tenure";
                ddlTenure.DataValueField = "Tenure";
                ddlTenure.DataBind();

                oAp = new CApplication();
                dt1 = oAp.GetLoanAmount(Convert.ToInt32(ddlLoanType.SelectedValue));
                ddlLnAmount.DataSource = dt1;
                ddlLnAmount.DataTextField = "Amount";
                ddlLnAmount.DataValueField = "Amount";
                ddlLnAmount.DataBind();
            }
            finally
            {
                dt = null;
                oAp = null;
            }
        }

        protected void ddlPurpose_SelectedIndexChanged(object sender, EventArgs e)
        {
            CSubPurpose oGb = new CSubPurpose();
            DataTable dt = new DataTable();
            try
            {
                DropDownList dropdown = (DropDownList)sender;
                GridViewRow row = (GridViewRow)dropdown.NamingContainer;
                DropDownList ddlSubPurpose = (DropDownList)row.FindControl("ddlSubPurpose");
                DropDownList ddlPurpose = (DropDownList)row.FindControl("ddlPurpose");

                dt = oGb.PopSubPurpose(gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate])), Convert.ToInt32(ddlPurpose.SelectedValue));
                ddlSubPurpose.DataSource = dt;
                ddlSubPurpose.DataTextField = "SubPurpose";
                ddlSubPurpose.DataValueField = "SubPurposeID";
                ddlSubPurpose.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlSubPurpose.Items.Insert(0, oli);
            }
            finally
            {
                dt = null;
                oGb = null;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }

        private void LoadGrid(Int32 pPgIndx)
        {
            CApplication oApp = null;
            DataTable dt = null;
            DateTime vDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oApp = new CApplication();
                dt = oApp.GetFestivalLoan(Convert.ToString(Session[gblValue.BrnchCode])
                    , Convert.ToInt32(ddlTenure.SelectedValue), vDate, ddlLo.Text);
                gvFestivLoan.DataSource = dt;
                gvFestivLoan.DataBind();
            }
            finally
            {
                oApp = null;
                dt = null;
            }
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            Int32 vRow = 0, vErr = 0, i = 0;
            string vMemberId = "", vMsg = "", vEndDate = "";
            string pEqXml = "", pStatusDesc = "";
            string pErrorMsg = "", vErrorMsg = "Pre Approved Loan failed for Member Id-";
            int pStatus = 0;
            DataTable dt, dt1 = null;
            CReLoanCB oRLC = null;
            CApplication oCAp = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string pBranchCode = Convert.ToString(Session[gblValue.BrnchCode]);
            int pCreatedBy = Convert.ToInt32(Session[gblValue.UserId]);
            WebServiceSoapClient eq = null;

            oRLC = new CReLoanCB();
            vEndDate = oRLC.GetLastDayEndByBrCode(Convert.ToString(Session[gblValue.BrnchCode]));
            if (gblFuction.setDate(vEndDate) != vLogDt)
            {
                gblFuction.AjxMsgPopup("Day end already done for this branch.");
                return;
            }

            if (gvFestivLoan.Rows.Count > 0)
            {
                for (vRow = 0; vRow < gvFestivLoan.Rows.Count; vRow++)
                {
                    CheckBox chkPre = (CheckBox)gvFestivLoan.Rows[vRow].FindControl("chkPre");
                    DropDownList ddlLoanType = (DropDownList)gvFestivLoan.Rows[vRow].FindControl("ddlLoanType");
                    DropDownList ddlTenure = (DropDownList)gvFestivLoan.Rows[vRow].FindControl("ddlTenure");
                    DropDownList ddlLnAmount = (DropDownList)gvFestivLoan.Rows[vRow].FindControl("ddlLnAmount");
                    DropDownList ddlPurpose = (DropDownList)gvFestivLoan.Rows[vRow].FindControl("ddlPurpose");
                    DropDownList ddlSubPurpose = (DropDownList)gvFestivLoan.Rows[vRow].FindControl("ddlSubPurpose");

                    if (chkPre.Checked)
                    {
                        vMemberId = gvFestivLoan.Rows[vRow].Cells[2].Text;
                        oRLC = new CReLoanCB();
                        dt = oRLC.GetReLoanMemberInfo(vMemberId, Convert.ToInt32(Session[gblValue.UserId].ToString()));
                        if (dt.Rows.Count > 0)
                        {
                            try
                            {                               
                                int pCBId = 0;
                                string pEnquiryId = "";
                                //-------------------------------------------------Save Initiali Approach------------------------------------------------------
                                oRLC = new CReLoanCB();
                                vErr = oRLC.SavePreApprIniApp(vMemberId, ddlLo.SelectedValue, pCreatedBy, vLogDt, pErrorMsg, pBranchCode, Convert.ToInt32(ddlLoanType.SelectedValue),
                                   Convert.ToInt32(ddlTenure.SelectedValue), Convert.ToDouble(ddlLnAmount.SelectedValue), Convert.ToInt32(ddlPurpose.SelectedValue),
                                   Convert.ToInt32(ddlSubPurpose.SelectedValue), Convert.ToInt32(Session[gblValue.FinYrNo]), ref pCBId, ref pEnquiryId);
                                if (vErr == 0)
                                {
                                    eq = new WebServiceSoapClient();
                                    oRLC = new CReLoanCB();
                                    dt1 = new DataTable();
                                    dt1 = oRLC.GetInitialApprOthMemDtlForCB(pEnquiryId, pCBId);
                                    if (dt1.Rows.Count > 0)
                                    {
                                        foreach (DataRow rs in dt1.Rows)
                                        {
                                            string pEqXmlOther = eq.Equifax(
                                              rs["FirstName"].ToString(), rs["MiddleName"].ToString(), rs["LastName"].ToString(), rs["DOB"].ToString()
                                              , rs["AddressType"].ToString(), rs["Address"].ToString(), rs["StateName"].ToString(), rs["PinCode"].ToString()
                                              , rs["MobileNo"].ToString(), rs["IDType1"].ToString(), rs["IDNo1"].ToString(), rs["IDType2"].ToString()
                                              , rs["IDNo2"].ToString(), rs["RelType"].ToString(), rs["RelName"].ToString()
                                              , "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");

                                            int pSlNo = Convert.ToInt32(rs["SlNo"].ToString());
                                            string pMemCategory = rs["MemCategory"].ToString();
                                            oRLC = new CReLoanCB();
                                            oRLC.UpdateEquifaxInformationOtherMem(pEnquiryId, pCBId, pEqXmlOther, pSlNo, pBranchCode, pCreatedBy, vLogDt, ddlLo.SelectedValue, pMemCategory);
                                        }
                                    }
                                    eq = new WebServiceSoapClient();
                                    pEqXml = eq.Equifax(
                                            dt.Rows[0]["MF_Name"].ToString(), dt.Rows[0]["MM_Name"].ToString(), dt.Rows[0]["ML_Name"].ToString()
                                            , dt.Rows[0]["M_DOB"].ToString(), "H", dt.Rows[0]["MemAdd"].ToString()
                                            , dt.Rows[0]["StateName"].ToString(), dt.Rows[0]["PIN"].ToString(), dt.Rows[0]["MobileNo"].ToString()
                                            , dt.Rows[0]["M_IdentyPRofId"].ToString(), dt.Rows[0]["M_IdentyProfNo"].ToString()
                                            , dt.Rows[0]["M_AddProfId"].ToString(), dt.Rows[0]["M_AddProfNo"].ToString()
                                            , dt.Rows[0]["FamilyPersonName"].ToString(), dt.Rows[0]["HF_YN"].ToString()
                                            , "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");
                                    oCAp = new CApplication();
                                    vErr = oCAp.UpdateEquifaxInformation(pEnquiryId, pCBId, pEqXml, pBranchCode, ddlLo.SelectedValue, pCreatedBy, vLogDt, "P", pErrorMsg, ref pStatus, ref pStatusDesc);
                                    
                                    i = i + 1;
                                }
                                if (vErr == 1)
                                {
                                    vErrorMsg = vErrorMsg + vMemberId + ",";
                                }

                            }
                            catch (Exception ex)
                            {
                                gblFuction.AjxMsgPopup(ex.ToString());
                                return;
                            }
                        }
                    }
                }
                if (i > 0)
                {
                    if (vErrorMsg.Length > 36)
                    {
                        vMsg = "Pre Approved Loan Save Successfully.But " + vErrorMsg;
                    }
                    else
                    {
                        vMsg = "Pre Approved Loan Successfully Saved.";
                    }

                }
                else
                {
                    vMsg = vErrorMsg;
                }
            }
            gblFuction.MsgPopup(vMsg.Substring(0, vMsg.Length - 1));
            LoadGrid(1);
        }

        private string GetStateSrtCode(string vStateName)
        {
            switch (vStateName)
            {
                case "ANDAMAN AND NICOBAR ISLANDS": return "AN";
                case "ANDHRA PRADESH": return "AP";
                case "ARUNACHAL PRADESH": return "AR";
                case "ASSAM": return "AS";
                case "BIHAR": return "BR";
                case "CHANDIGARH": return "CH";
                case "CHHATTISGARH": return "CT";
                case "DADRA AND NAGAR HAVELI": return "DN";
                case "DAMAN AND DIU": return "DD";
                case "DELHI": return "DL";
                case "GOA": return "GA";
                case "GUJARAT": return "GJ";
                case "HARYANA": return "HR";
                case "HIMACHAL PRADESH": return "HP";
                case "JAMMU AND KASHMIR": return "JK";
                case "JHARKHAND": return "JH";
                case "KARNATAKA": return "KA";
                case "KERALA": return "KL";
                case "LAKSHADWEEP": return "LD";
                case "MADHYA PRADESH": return "MP";
                case "MAHARASHTRA": return "MH";
                case "MANIPUR": return "MN";
                case "MEGHALAYA": return "ML";
                case "MIZORAM": return "MZ";
                case "NAGALAND": return "NL";
                case "ORISSA": return "OR";
                case "PONDICHERRY": return "PY";
                case "PUNJAB": return "PB";
                case "RAJASTHAN": return "RJ";
                case "SIKKIM": return "SK";
                case "TAMIL NADU": return "TN";
                case "TRIPURA": return "TR";
                case "UTTAR PRADESH": return "UP";
                case "UTTARAKHAND": return "UK";
                case "WEST BENGAL": return "WB";
                default: return "";
            }
        }

        private string ProcessOtherMemCB(string pMemberId, Int32 pCbId, string pBranchCode, Int32 pCreatedBy, string pDate
           , string CCRUserName, string CCRPassword, string pEoId)
        {
            WebServiceSoapClient eq = new WebServiceSoapClient();
            string pEqXml = string.Empty, equifaxResponse = string.Empty, pErrDesc = string.Empty;
            string pMemCategory = string.Empty, vMemTag = string.Empty;
            Int32 vErr2 = 0, vErr = 0, vCbIdOtherMem = 0;
            Int32 pSlNo = 0;
            DataTable dt = new DataTable();
            CApplication oApp = null;
            //--------------------------------------------------------------------------------
            oApp = new CApplication();
            dt = oApp.GetOthMemDtlForCB(pMemberId);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow rs in dt.Rows)
                {
                    pMemCategory = rs["MemCategory"].ToString();
                    pSlNo = Convert.ToInt32(rs["SlNo"].ToString());

                    if (pMemCategory == "CoAppMem") vMemTag = "Co-Applicant";
                    if (pMemCategory == "OtherMem") vMemTag = "Other Earning Member";

                    pEqXml = eq.Equifax(
                    rs["FirstName"].ToString(), rs["MiddleName"].ToString(), rs["LastName"].ToString(), rs["DOB"].ToString()
                    , rs["AddressType"].ToString(), rs["Address"].ToString(), rs["StateName"].ToString(), rs["PinCode"].ToString(), rs["MobileNo"].ToString()
                    , rs["IDType1"].ToString(), rs["IDNo1"].ToString(), rs["IDType2"].ToString(), rs["IDNo2"].ToString()
                    , rs["RelName"].ToString(), rs["RelType"].ToString()
                    , "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");

                    if (pEqXml.Equals(""))
                    {
                    }
                    else
                    {
                        oApp = new CApplication();
                        vErr2 = oApp.UpdateEquifaxInformationOtherMem(pMemberId, pCbId, pEqXml, pBranchCode, pEoId, pCreatedBy, pDate, "P", "", ref vErr, ref pErrDesc, "", pSlNo);
                    }
                }
                return "Successful";
            }
            else
            {
                return "Successful";
            }
            //return "Successful";
        }

    }
}