using System;
using System.Web.UI;
using System.Data;
using CENTRUM.Service_Equifax;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using FORCECA;
using FORCEBA;
using System.Web.UI.WebControls;
using CENTRUM.Service_Equifax_CCR;
using System.Configuration;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class ReLoanCbCheck : CENTRUMBase
    {
        string CCRUserName = ConfigurationManager.AppSettings["CCRUserName"];
        string CCRPassword = ConfigurationManager.AppSettings["CCRPassword"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch();
                txtDt.Text = txtDtTo.Text=txtDtFrm.Text= Session[gblValue.LoginDate].ToString();                
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Re Loan CB Check ";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuReLoanCbCheck);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnDone.Visible = false;
                    //btnClose.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnDone.Visible = true;
                    //btnClose.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnDone.Visible = true;
                    //btnClose.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    //btnDone.Visible = true;
                    //btnClose.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Credit Bureau Operation", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void PopBranch()
        {
            CGblIdGenerator oGb = null;
            DataTable dt = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), Session[gblValue.BrnchCode].ToString());
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();

                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dt.Rows[i];
                        if (Convert.ToString(dr["BranchCode"]) != Session[gblValue.BrnchCode].ToString())
                        {
                            dr.Delete();
                            dt.AcceptChanges();
                        }
                    }
                }
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }

        private void LoadGrid(Int32 pPgIndx)
        {
            CReLoanCB oRLC = null;
            DataTable dt = null;
            DateTime vDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRLC = new CReLoanCB();
                dt = oRLC.GetReLoanCBData(ddlBranch.SelectedValues.Replace("|", ","), Convert.ToInt32(ddlTenure.SelectedValue), vDate,txtMemberName.Text);
                gvReLaonCB.DataSource = dt;
                gvReLaonCB.DataBind();
            }
            finally
            {
                oRLC = null;
                dt = null;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            Int32 vRow = 0, vErr=0,i=0;
            string vMemberId = "", vMsg = "";
            string pEqXml = "", pStatusDesc = "";
            string pErrorMsg = "", vErrorMsg = "Equifax upload failed for Member Id-";
            int pStatus = 0;      
            DataTable dt = null;
            CReLoanCB oRLC = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            if (gvReLaonCB.Rows.Count > 0)
            {
                for (vRow = 0; vRow < gvReLaonCB.Rows.Count; vRow++)
                {
                    CheckBox chkEqui = (CheckBox)gvReLaonCB.Rows[vRow].FindControl("chkEqui");

                    if (chkEqui.Checked)
                    {
                        vMemberId = gvReLaonCB.Rows[vRow].Cells[5].Text;
                        oRLC = new CReLoanCB();
                        dt = oRLC.GetReLoanMemberInfo(vMemberId, Convert.ToInt32(Session[gblValue.UserId]));
                        if (dt.Rows.Count > 0)
                        {
                            try
                            {
                                WebServiceSoapClient eq = new WebServiceSoapClient();
                                pEqXml = eq.Equifax(
                                        dt.Rows[0]["MF_Name"].ToString(), dt.Rows[0]["MM_Name"].ToString(), dt.Rows[0]["ML_Name"].ToString()
                                        , dt.Rows[0]["M_DOB"].ToString(), "H", dt.Rows[0]["MemAdd"].ToString()
                                        , dt.Rows[0]["StateName"].ToString(), dt.Rows[0]["PIN"].ToString(), dt.Rows[0]["MobileNo"].ToString()
                                        , dt.Rows[0]["M_IdentyPRofId"].ToString(), dt.Rows[0]["M_IdentyProfNo"].ToString()
                                        , dt.Rows[0]["M_AddProfId"].ToString(), dt.Rows[0]["M_AddProfNo"].ToString()
                                        , dt.Rows[0]["FamilyPersonName"].ToString(), dt.Rows[0]["HF_YN"].ToString()
                                        , "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");

                                oRLC = new CReLoanCB();
                                vErr = oRLC.SaveReLoanCbData_CCR(vMemberId, pEqXml, "", Convert.ToInt32(Session[gblValue.UserId]), vLogDt, pErrorMsg, ref pStatus, ref pStatusDesc);
                                if (vErr == 1)
                                {
                                    i = i + 1;
                                }
                                if (vErr == 5)
                                {
                                    vErrorMsg = vErrorMsg + vMemberId + ",";
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }                           
                        }                       
                    }
                }
                if (i > 0)
                {
                    if (vErrorMsg.Length > 36)
                    {
                        vMsg = "CB Checked Successfully.But " + vErrorMsg;
                    }
                    else
                    {
                        vMsg = "CB Checked Successfully";
                    }

                }
                else
                {
                    vMsg = vErrorMsg;
                }
            }

            gblFuction.MsgPopup(vMsg.Substring(0,vMsg.Length-1));
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

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            Button btnDownload = (Button)sender;
            GridViewRow gR = (GridViewRow)btnDownload.NamingContainer;
            string vEnqId = gR.Cells[7].Text.Trim();
            Int32 vCbId = Convert.ToInt32(gR.Cells[8].Text.Trim());
            SetParameterForRptData(vEnqId, vCbId, "PDF");
        }

        private void SetParameterForRptData(string pEnquiryId, int pCbAppId, string pType)
        {
            DataSet ds = null;
            DataTable dt1 = null, dt2 = null, dt3 = null;
            CReLoanCB oRpt = null;
            string vRptPath = "";
            string vBranch = Session[gblValue.BrName].ToString();
            try
            {
                //cvar = 1;
                oRpt = new CReLoanCB();
                string enqstatusmsg = "";
                ds = oRpt.Equifax_Report_ReLoan(pEnquiryId, pCbAppId, ref  enqstatusmsg);
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
                //---------------------------------------------------------------------------------------
                if (Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
                {
                    foreach (DataRow dr in dt2.Rows) // search whole table
                    {
                        if (Convert.ToInt32(dr["IdentyPRofId"].ToString()) == 1)
                        {
                            dr["VoterId"] = String.Format("{0}{1}", "********", Convert.ToString(dt2.Rows[0]["VoterId"]).Substring(Convert.ToString(dt2.Rows[0]["VoterId"]).Length - 4, 4)); ; //change the name                           
                        }
                        if (Convert.ToInt32(dr["AddProfId"].ToString()) == 1)
                        {
                            dr["UID"] = String.Format("{0}{1}", "********", Convert.ToString(dt2.Rows[0]["UID"]).Substring(Convert.ToString(dt2.Rows[0]["UID"]).Length - 4, 4)); ; //change the name                           
                        }
                    }
                }
                //----------------------------------------------------------------------------------------
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

        protected void btnShow_Click(object sender, EventArgs e)
        {
            CReLoanCB oRLC = null;
            DataTable dt = null;
            DateTime vFrmDate = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDate = gblFuction.setDate(txtDtTo.Text);
            string vBranchCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oRLC = new CReLoanCB();
                dt = oRLC.GetReLoanCbRecord(vFrmDate, vToDate, vBranchCode);
                gvhighmarAppr.DataSource = dt;
                gvhighmarAppr.DataBind();
            }
            finally
            {
                oRLC = null;
                dt = null;
            }
        }
    }
}