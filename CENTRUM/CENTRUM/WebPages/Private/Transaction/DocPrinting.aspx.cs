using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using FORCEBA;
using CrystalDecisions.Shared;
using FORCECA;
using System.IO;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class DocPrinting : CENTRUMBase
    {
        protected int cPgNo = 1;
        string path = ConfigurationManager.AppSettings["PathInitialApproach"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopLo();
                PopGroup();
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                string vFirst = vBrCode.Substring(0, 1);
                if (vFirst == "1" || vFirst == "4" || vFirst == "5" || vFirst == "7" || vFirst == "8" || vFirst == "9")
                    ddlLangu.SelectedIndex = 3;
                else if (vFirst == "2")
                    ddlLangu.SelectedIndex = 1;
                else if (vFirst == "3")
                    ddlLangu.SelectedIndex = 2;
                else if (vFirst == "6")
                    ddlLangu.SelectedIndex = 4;
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Document Print";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuNEFTCancel);
                if (this.UserID == 1) return;
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void PopLo()
        {
            DataTable dt = null;
            CEO oCEO = null;
            string vBrCode = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            vBrCode = (string)Session[gblValue.BrnchCode];
            oCEO = new CEO();
            dt = oCEO.PopRO(Session[gblValue.BrnchCode].ToString(), "", "", vLogDt, 0);
            if (dt.Rows.Count > 0)
            {
                ddlLo.DataSource = dt;
                ddlLo.DataTextField = "EoName";
                ddlLo.DataValueField = "EoId";
                ddlLo.DataBind();
            }
            ListItem Li = new ListItem("<-- Select -->", "-1");
            ddlLo.Items.Insert(0, Li);
        }

        protected void ddlLo_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopGroup();
        }
        private void PopGroup()
        {
            ddlGroup.Items.Clear();
            DataTable dt = null;
            CUser oCEO = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            oCEO = new CUser();
            dt = oCEO.popGroupByEoid(ddlLo.SelectedValue, vLogDt);
            if (dt.Rows.Count > 0)
            {
                ddlGroup.DataSource = dt;
                ddlGroup.DataTextField = "GroupName";
                ddlGroup.DataValueField = "Groupid";
                ddlGroup.DataBind();
            }
            ListItem Li = new ListItem("<-- Select -->", "-1");
            ddlGroup.Items.Insert(0, Li);
        }

        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }

        protected void ChangePage(object sender, CommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }

            LoadGrid(txtToDt.Text, txtFrmDt.Text);
            //tabLoanAppl.ActiveTabIndex = 0;
        }

        private void LoadGrid(string pToDt, string pFromDt)
        {
            DataTable dt = null;
            CApplication oLS = null;
            Int32 vRows = 0;
            try
            {
                DateTime vFromDt = gblFuction.setDate(pFromDt);
                DateTime vToDt = gblFuction.setDate(pToDt);
                oLS = new CApplication();
                dt = oLS.GetDocPrintDtl(vFromDt, vToDt, ddlGroup.SelectedValue);
                gvSanc.DataSource = dt;
                gvSanc.DataBind();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        ViewState["VsIsBC"] = dr["IsBC"].ToString();
                    } 
                }
                lblTotalPages.Text = CalTotPgs(vRows).ToString();
                lblCurrentPage.Text = cPgNo.ToString();
                if (cPgNo == 0)
                {
                    Btn_Previous.Enabled = false;
                    if (Int32.Parse(lblTotalPages.Text) > 1)
                        Btn_Next.Enabled = true;
                    else
                        Btn_Next.Enabled = false;
                }
                else
                {
                    Btn_Previous.Enabled = true;
                    if (cPgNo == Int32.Parse(lblTotalPages.Text))
                        Btn_Next.Enabled = false;
                    else
                        Btn_Next.Enabled = true;
                }
            }
            finally
            {
                dt = null;
                oLS = null;
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                LoadGrid(txtToDt.Text, txtFrmDt.Text);
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
        protected void btnClose_Click(object sender, EventArgs e)
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

        protected void btnLoanUnderTaking_Click(object sender, EventArgs e)
        {
            string vRptPath = "";
            DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            DataTable dt = null;
            CReports oRpt = new CReports();

            dt = oRpt.rptGroupLoanUndertaking(vFrmDt, vToDt, ddlGroup.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                if (ddlLangu.SelectedValue == "E")
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\GroupLoanUndertaking.rpt";
                else if (ddlLangu.SelectedValue == "O")
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\GroupLoanUndertakingOdia1.rpt";
                else if (ddlLangu.SelectedValue == "G")
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\GroupLoanUndertakingGujarati1.rpt";
                else if (ddlLangu.SelectedValue == "H")
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\GroupLoanUndertakingHindi.rpt";
                else if (ddlLangu.SelectedValue == "B")
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\GroupLoanUndertakingBengali.rpt";
                else
                {
                    gblFuction.AjxMsgPopup("This Language Format is not available...");
                    return;
                }
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pBranchName", Convert.ToString(dt.Rows[0]["BranchName"]));
                    rptDoc.SetParameterValue("pBranchCode", Convert.ToString(dt.Rows[0]["BranchCode"]));
                    rptDoc.SetParameterValue("pAppDate", Convert.ToString(dt.Rows[0]["AppDate"]));
                    rptDoc.SetParameterValue("pGroupName", Convert.ToString(dt.Rows[0]["GroupName"]));
                    rptDoc.SetParameterValue("pGroupId", Convert.ToString(dt.Rows[0]["Groupid"]));
                    rptDoc.SetParameterValue("pDay", Convert.ToString(dt.Rows[0]["day"]));
                    rptDoc.SetParameterValue("pMonth", Convert.ToString(dt.Rows[0]["month"]));
                    rptDoc.SetParameterValue("pYear", Convert.ToString(dt.Rows[0]["year"]));
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Group Loan Undertaking");
                    rptDoc.Dispose();
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }


        }

        protected void btnDemand_Click(object sender, EventArgs e)
        {
            string vRptPath = "", vIsBc = "";
            DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            DataTable dt = null;
            CReports oRpt = new CReports();

            vIsBc = Convert.ToString(ViewState["VsIsBC"]);

            dt = oRpt.rptDPN(vFrmDt, vToDt, ddlGroup.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
                {
                    foreach (DataRow dr in dt.Rows) // search whole table
                    {
                        dr["KYC"] = String.Format("{0}{1}", "********", Convert.ToString(dr["KYC"]).Substring(Convert.ToString(dr["KYC"]).Length - 4, 4));                   
                    }
                }

                if (ddlLangu.SelectedValue == "E")
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DPN.rpt";
                else if (ddlLangu.SelectedValue == "O")
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DPNOdia.rpt";
                else if (ddlLangu.SelectedValue == "G")
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DPNGujarati.rpt";
                else if (ddlLangu.SelectedValue == "H")
                    if (vIsBc == "Y")
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DPNHindiBC.rpt";
                    else
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DPNHindi.rpt";
                else if (ddlLangu.SelectedValue == "B")
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DPNBengali.rpt";
                else
                {
                    gblFuction.AjxMsgPopup("This Language Format is not available...");
                    return;
                }
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);

                    rptDoc.SetParameterValue("pGroupId", Convert.ToString(dt.Rows[0]["Groupid"]));
                    rptDoc.SetParameterValue("pDate", Convert.ToString(dt.Rows[0]["ExpDate"]));
                    rptDoc.SetParameterValue("pLocation", Convert.ToString(dt.Rows[0]["VillageName"]));
                    rptDoc.SetParameterValue("pInstRate", Convert.ToString(dt.Rows[0]["InstRate"]));

                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Demand Promissory Note");
                    rptDoc.Dispose();
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
        }


        protected void btnAgree_Click(object sender, EventArgs e)
        {
            string vRptPath = "", vIsBc = "";
            DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            DataTable dt = null;
            CReports oRpt = new CReports();

            vIsBc = Convert.ToString(ViewState["VsIsBC"]);

            dt = oRpt.rptLoanAgreement(vFrmDt, vToDt, ddlGroup.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                

                if (ddlLangu.SelectedValue == "E")
                {
                    gblFuction.AjxMsgPopup("This Language Format is not available...");
                    return;
                }
                else if (ddlLangu.SelectedValue == "O")
                {
                    gblFuction.AjxMsgPopup("This Language Format is not available...");
                    return;
                }
                else if (ddlLangu.SelectedValue == "G")
                {
                    gblFuction.AjxMsgPopup("This Language Format is not available...");
                    return;
                }
                else if (ddlLangu.SelectedValue == "H")
                    if (vIsBc == "Y")
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanAgreementHindiBC.rpt";
                    else
                    {
                        gblFuction.AjxMsgPopup("This Language Format is not available...");
                        return;
                    }
                else if (ddlLangu.SelectedValue == "B")
                {
                    gblFuction.AjxMsgPopup("This Language Format is not available...");
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup("This Language Format is not available...");
                    return;
                }
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);

                    //rptDoc.SetParameterValue("pGroupId", Convert.ToString(dt.Rows[0]["Groupid"]));
                    //rptDoc.SetParameterValue("pDate", Convert.ToString(dt.Rows[0]["ExpDate"]));
                    //rptDoc.SetParameterValue("pLocation", Convert.ToString(dt.Rows[0]["VillageName"]));
                    //rptDoc.SetParameterValue("pInstRate", Convert.ToString(dt.Rows[0]["InstRate"]));

                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Loan Agreement");
                    rptDoc.Dispose();
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
        }

        protected void gvSanc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            CReports oRpt = null;
            DataTable dt = null;
            DataSet ds = null;
            GridViewRow gvRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            string vRptPath = "", vRptName = "", vMode = "";
            string vImgPath = "", vIsBc = "";
            vIsBc = gvRow.Cells[15].Text;
            if (e.CommandName == "cmdAppFrm")
            {
                dt = new DataTable();
                oRpt = new CReports();
                string Id = gvRow.Cells[0].Text;
                dt = oRpt.GetApplicationForm(Id);
                foreach (DataRow dr in dt.Rows) 
                {
                    vImgPath = path.Replace(@"/", "\\");
                    dr["ImgPath"] = vImgPath + Convert.ToString(dr["ImgPath"]); 
                }
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
                    {
                        foreach (DataRow dr in dt.Rows) // search whole table
                        {                   
                            if (dr["KYC Name1"].ToString() == "AADHAAR")
                            {
                                dr["KYC No1"] = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["KYC No1"]).Substring(Convert.ToString(dt.Rows[0]["KYC No1"]).Length - 4, 4)); //change the name                           
                            }
                            if (dr["KYC Name2"].ToString() == "AADHAAR")
                            {
                                dr["KYC No2"] = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["KYC No2"]).Substring(Convert.ToString(dt.Rows[0]["KYC No2"]).Length - 4, 4));  //change the name                           
                            }
                            if (dr["COKYC Name1"].ToString() == "AADHAAR")
                            {
                                dr["COKYC No1"] = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["COKYC No1"]).Substring(Convert.ToString(dt.Rows[0]["COKYC No1"]).Length - 4, 4));  //change the name                           
                            }
                            if (dr["COKYC Name2"].ToString() == "AADHAAR")
                            {
                                dr["COKYC No2"] = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["COKYC No2"]).Substring(Convert.ToString(dt.Rows[0]["COKYC No2"]).Length - 4, 4));  //change the name                           
                            }
                        }
                    }

                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        if (ddlLangu.SelectedValue == "E")
                            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\ApplicationForm.rpt";
                        else if (ddlLangu.SelectedValue == "O")
                            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanApplOdia.rpt";
                        else if (ddlLangu.SelectedValue == "H")
                            if (vIsBc == "Y")
                                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\ApplicationFormHindiBC.rpt";
                            else
                                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\ApplicationFormHindi.rpt";
                        else if (ddlLangu.SelectedValue == "B")
                            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\ApplicationFormBengali.rpt";
                        else
                            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\ApplicationFormGujarati.rpt";

                        vRptName = "Application_Form";
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt);
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, Id + '_' + vRptName);
                        Response.ClearContent();
                        Response.ClearHeaders();
                    }
                }
            }
            if (e.CommandName == "cmdLoanInst")
            {
                ds = new DataSet();
                dt = new DataTable();
                DataTable dt1 = new DataTable();
                oRpt = new CReports();
                string Id = gvRow.Cells[0].Text;
                ds = oRpt.GetLoanInstallment(Id);
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                if (dt.Rows.Count > 0)
                {
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        if (ddlLangu.SelectedValue == "E")
                            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanInstallment.rpt";
                        else
                            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanInstallment.rpt";

                        vRptName = "Loan_Installment";
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt1);
                        rptDoc.SetParameterValue("pBranchName", Session[gblValue.BrName].ToString());
                        rptDoc.SetParameterValue("pCmpName", "Unity Small Finance Bank Ltd.");
                        rptDoc.SetParameterValue("pMemberName", Convert.ToString(dt.Rows[0]["MemberName"]));
                        rptDoc.SetParameterValue("pMemberNo", Convert.ToString(dt.Rows[0]["MemberNo"]));
                        rptDoc.SetParameterValue("pLoanAppNo", Convert.ToString(dt.Rows[0]["LoanAppNo"]));
                        rptDoc.SetParameterValue("pLoanCycle", Convert.ToInt32(dt.Rows[0]["LoanCycle"]));
                        rptDoc.SetParameterValue("pLoanType", Convert.ToString(dt.Rows[0]["LoanType"]));
                        rptDoc.SetParameterValue("pApprovedAmt", Convert.ToDouble(dt.Rows[0]["ApprovedAmt"]));
                        rptDoc.SetParameterValue("pInstRate", Convert.ToDouble(dt.Rows[0]["InstRate"]));
                        rptDoc.SetParameterValue("pInstallNo", Convert.ToInt32(dt.Rows[0]["InstallNo"]));
                        rptDoc.SetParameterValue("pPaySchedule", Convert.ToString(dt.Rows[0]["PaySchedule"]));
                        rptDoc.SetParameterValue("pDisbDt", Convert.ToString(dt.Rows[0]["DisbDt"]));
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, Id + '_' + vRptName);
                        Response.ClearContent();
                        Response.ClearHeaders();
                    }
                }
            }
            if (e.CommandName == "cmdKYC")
            {
                dt = new DataTable();
                oRpt = new CReports();
                string Id = gvRow.Cells[0].Text;               
                dt = oRpt.GetKYCDoc(Id);
                foreach (DataRow dr in dt.Rows)
                {
                    vImgPath = path.Replace(@"/", "\\");
                    dr["KYC1"] = vImgPath + Convert.ToString(dr["KYC1"]);
                    dr["KYC1Back"] = vImgPath + Convert.ToString(dr["KYC1Back"]);
                    dr["KYC2"] = vImgPath + Convert.ToString(dr["KYC2"]);
                    dr["KYC2Back"] = vImgPath + Convert.ToString(dr["KYC2Back"]);
                    dr["KYC3"] = vImgPath + Convert.ToString(dr["KYC3"]);
                    dr["KYC3Back"] = vImgPath + Convert.ToString(dr["KYC3Back"]);
                }

                if (dt.Rows.Count > 0)
                {
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\KYC.rpt";
                        vRptName = "KYC";
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt);
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, Id + '_' + vRptName);
                        Response.ClearContent();
                        Response.ClearHeaders();
                    }
                }
            }
            if (e.CommandName == "cmdTopUp")
            {
                dt = new DataTable();
                oRpt = new CReports();
                string Id = gvRow.Cells[0].Text;
                dt = oRpt.GetTopUpFrm(Id);
                if (dt.Rows.Count > 0)
                {
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\TopUpFrm.rpt";
                        vRptName = "Top-Up";
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt);
                        rptDoc.SetParameterValue("pDate", Session[gblValue.LoginDate].ToString());
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, Id + '_' + vRptName);
                        Response.ClearContent();
                        Response.ClearHeaders();
                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("This Customer is not a Top Up Customer..");
                }
            }

            if (e.CommandName == "cmdQRCode")
            {
                string Id = gvRow.Cells[0].Text;
                string pPayee = gvRow.Cells[3].Text;
                string pAmount = "0";
                DataTable dt1 = new DataTable();
                oRpt = new CReports();
                dt1 = oRpt.GetInstallMentAmt(Id);
                if (dt1.Rows.Count > 0)
                {
                    pAmount = dt1.Rows[0]["InstallmentAmount"].ToString();
                    CreateQRCode(pPayee, pAmount, Id);
                }
            }

            if (e.CommandName == "cmdLoanCard")
            {
                dt = new DataTable();
                DataTable dt1 = null, dt2 = null, dt3 = null;
                oRpt = new CReports();
                string Id = gvRow.Cells[0].Text;
                vMode = ddlLangu.SelectedValue;
                ds = oRpt.RptLoanCard_DocPrint(Id);
                dt1 = ds.Tables[0];
                dt1.TableName = "MemLoanInfo";
                dt2 = ds.Tables[1];
                if (dt1.Rows.Count > 0)
                {
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        if (vMode == "O")
                            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptLoanCardOdiya.rpt";
                        else if (vMode == "G")
                            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptLoanCardGujrati.rpt";
                        else if (vMode == "B")
                            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptLoanCardBengali.rpt";
                        else
                            if (vIsBc == "Y")
                                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptLoanCardHindiBC.rpt";
                            else
                                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptLoanCardEnglish.rpt";

                        vRptName = "LoanCard";
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt1);
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, Id + '_' + vRptName);
                        Response.ClearContent();
                        Response.ClearHeaders();
                    }
                }
                else
                {
                    gblFuction.MsgPopup("No Data Found...");
                    return;
                }
            }

            if (e.CommandName == "cmdDigitalDocForm")
            {
                DataSet ds1 = new DataSet(), dsDigiDoc= new DataSet();
                DataTable dt1 = new DataTable(), dt2 = new DataTable(), dt3 = new DataTable();
                DataTable dt4 = new DataTable(), dt5 = new DataTable(), dt6 = new DataTable();
                DataTable dt7 = new DataTable(), dtDigiDocDtls = new DataTable();

                oRpt = new CReports();
                vRptPath = string.Empty;
                if (ddlLangu.SelectedValue == "E") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocForm.rpt";
                if (ddlLangu.SelectedValue == "H") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocFormHindi.rpt";
                if (ddlLangu.SelectedValue == "G") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocFormGujarati.rpt";
                if (ddlLangu.SelectedValue == "O") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocFormOdia.rpt";
                if (ddlLangu.SelectedValue == "B") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintBengali.rpt";

                string Id = gvRow.Cells[0].Text;
                ds1 = oRpt.GetDigitalDocForm(Id, gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), ddlGroup.SelectedValue, 0, Convert.ToInt32(Session[gblValue.UserId].ToString()));
                dt1 = ds1.Tables[0];
                dt2 = ds1.Tables[1];
                dt3 = ds1.Tables[2];
                dt4 = ds1.Tables[3];
                dt5 = ds1.Tables[4];
                dt6 = ds1.Tables[5];
                dt7 = ds1.Tables[6];

                CDigiDoc oDD = new CDigiDoc();
                dsDigiDoc = oDD.getDigiDocDtlsByDocId(0, "", "Y");
                dtDigiDocDtls = dsDigiDoc.Tables[0];


                if (dt1.Rows.Count > 0)
                {
                    if (vRptPath != string.Empty)
                    {
                        foreach (DataRow dr in dt1.Rows)
                        {
                            vImgPath = path.Replace(@"/", "\\");
                            dr["ImgPath"] = vImgPath + Convert.ToString(dr["ImgPath"]);
                        }
                        dt1.AcceptChanges();

                        if (dt7.Rows.Count > 0) 
                        {
                            foreach (DataRow dr in dt7.Rows)
                            {
                                vImgPath = path.Replace(@"/", "\\");
                                dr["KYC1"] = vImgPath + Convert.ToString(dr["KYC1"]);
                                dr["KYC1Back"] = vImgPath + Convert.ToString(dr["KYC1Back"]);
                                dr["KYC2"] = vImgPath + Convert.ToString(dr["KYC2"]);
                                dr["KYC2Back"] = vImgPath + Convert.ToString(dr["KYC2Back"]);
                                dr["KYC3"] = vImgPath + Convert.ToString(dr["KYC3"]);
                                dr["KYC3Back"] = vImgPath + Convert.ToString(dr["KYC3Back"]);
                            }
                            dt7.AcceptChanges();
                        }

                        using (ReportDocument rptDoc = new ReportDocument())
                        {
                            vRptName = "Application_Form";
                            rptDoc.Load(vRptPath);
                            rptDoc.SetDataSource(dt1);

                            if (ddlLangu.SelectedValue == "E")
                            {
                                rptDoc.Subreports["DigitalDocFormP2.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFrmP4.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7.rpt"].SetDataSource(dt7);
                            }
                            if (ddlLangu.SelectedValue == "H")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Hindi.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Hindi.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Hindi.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Hindi.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Hindi.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Hindi.rpt"].SetDataSource(dt7);
                            }
                            if (ddlLangu.SelectedValue == "G")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Gujarati.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Gujarati.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Gujarati.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Gujarati.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Gujarati.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Gujarati.rpt"].SetDataSource(dt7);
                            }
                            if (ddlLangu.SelectedValue == "O")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Odia.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Odia.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Odia.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Odia.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Odia.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Odia.rpt"].SetDataSource(dt7);
                            }
                            if (ddlLangu.SelectedValue == "B")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Bengali.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Bengali.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Bengali.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Bengali.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Bengali.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Bengali.rpt"].SetDataSource(dt7);
                            }
                            rptDoc.Subreports["DigitalDoc_eSigned.rpt"].SetDataSource(dtDigiDocDtls);

                            rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, Id + '_' + vRptName);
                            rptDoc.Dispose();
                            Response.ClearContent();
                            Response.ClearHeaders();
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("Report Not Available");
                        return;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("No Data Found...");
                    return;
                }
            }
        }

        private void CreateQRCode(string ppayee, string pAmount, string pID)
        {
            // Hardcoded
            string merchantID = "CPL";
            string merchantName = "Centrum Microcredit Limited";
            // Hardcoded
            string payee = ppayee; //Name of the member
            string amount = pAmount;  // Installment Amount 
            string payeeAccountNo = pID;// LoanApplicationID

            CallQRCode(merchantID, merchantName, payee, amount, payeeAccountNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responsedata"></param>
        public void CallQRCode(string pmerchantID, string pmerchantName, string ppayee, string pamount, string ppayeeAccountNo)
        {
            string pQRMsg = "Ok";
            try
            {
                string json = "";

                RootObject rootobjectreq = new RootObject();
                rootobjectreq.merchantID = pmerchantID;
                rootobjectreq.merchantName = pmerchantName;
                rootobjectreq.payee = ppayee;
                rootobjectreq.amount = pamount;
                rootobjectreq.payeeAccountNo = ppayeeAccountNo;


                json = JsonConvert.SerializeObject(rootobjectreq);

                /*******Login URL********/
                string url = "https://eft.billcloud.in:9002/service/upiservice/getQR";


                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                /*****************************/
                httpWebRequest.Proxy = null;
                httpWebRequest.ContentType = "application/json; charset=utf-8";

                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "Basic dXBpcXI6dXAhcXJAMTIz");

                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

                //httpWebRequest.PreAuthenticate = true;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string responsedata = string.Empty;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    responsedata = result.ToString().Trim();
                }

                RootObjectResponseWITHQR rootobjectresponse = new RootObjectResponseWITHQR();
                rootobjectresponse = JsonConvert.DeserializeObject<RootObjectResponseWITHQR>(responsedata);

                pQRMsg = rootobjectresponse.QR;
                LoadImage(pQRMsg, ppayeeAccountNo);
                gblFuction.AjxMsgPopup("QR Code Saved Successfully.");
            }
            catch (Exception ex)
            {
                //Response.Redirect("~/Login.aspx");
                pQRMsg = ex.Message;
                gblFuction.AjxMsgPopup("Try Again!!!");

                //throw ex;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void LoadImage(string pQRImage, string ppayeeAccountNo)
        {
            string vMsg = "";
            try
            {
                string filePath = "G:\\WebApps\\CentrumMobile\\Files\\QRCode\\" + ppayeeAccountNo + ".png";  ///WebClient Config
                File.WriteAllBytes(filePath, Convert.FromBase64String(pQRImage));
            }
            catch (Exception ex)
            {
                //Response.Redirect("~/Login.aspx");
                vMsg = ex.Message;
                gblFuction.AjxFocus("Will be available Shortly!!!");
                //throw ex;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public class RootObject
        {
            public string merchantID { get; set; }
            public string merchantName { get; set; }
            public string payee { get; set; }
            public string amount { get; set; }
            public string payeeAccountNo { get; set; }

        }

        public class RootObjectResponseWITHQR
        {
            public string merchantID { get; set; }
            public string merchantName { get; set; }
            public string payee { get; set; }
            public string amount { get; set; }
            public string payeeAccountNo { get; set; }
            public string status { get; set; }
            public string reason { get; set; }
            public string QR { get; set; }
        }

    }
}