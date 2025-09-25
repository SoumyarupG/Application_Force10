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
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class DigitalDocPrinting : CENTRUMBase
    {
        MyWebBrowser browser;
        private readonly Encoding encoding = Encoding.UTF8;
        protected int cPgNo = 1;
        string path = ConfigurationManager.AppSettings["PathInitialApproach"];
        string InitialApproachURL = ConfigurationManager.AppSettings["InitialApproachURL"];
        string AadhaarURL = ConfigurationManager.AppSettings["AadhaarURL"];
        string DigiDocpath = ConfigurationManager.AppSettings["DigiDocpath"];
        string DigiDocpathArchive = ConfigurationManager.AppSettings["DigiDocpathArchive"];

        string IniPathHDrive = ConfigurationManager.AppSettings["IniPathHDrive"];
        string IniPathNetwork = ConfigurationManager.AppSettings["IniPathNetwork"];
        string IniPathNetwork2 = ConfigurationManager.AppSettings["IniPathNetwork2"];
        string IniPathNetwork3 = ConfigurationManager.AppSettings["IniPathNetwork3"];
        string IniPathGDrive = ConfigurationManager.AppSettings["IniPathGDrive"];
        string PathMinio = ConfigurationManager.AppSettings["PathMinio"];
        string PathDDHTML = ConfigurationManager.AppSettings["PathDDHTML"];

        //string DigiDocBucket = ConfigurationManager.AppSettings["DigiDocBucket"];
        //string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        //string MinioYN = ConfigurationManager.AppSettings["MinioYN"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                Session["DigiDocYN"] = null;
                PopLo();
                PopGroup();
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                string vStateId = Session[gblValue.StateID].ToString();
                string vFirst = vBrCode.Substring(0, 1);
                //if (vFirst == "1" || vFirst == "4" || vFirst == "5" || vFirst == "7" || vFirst == "8" || vFirst == "9")
                //    ddlLangu.SelectedIndex = 3;
                //else if (vFirst == "2")
                //    ddlLangu.SelectedIndex = 1;
                //else if (vFirst == "3")
                //    ddlLangu.SelectedIndex = 2;
                //else if (vFirst == "6")
                //    ddlLangu.SelectedIndex = 4;

                if (vStateId == "8")
                    ddlLangu.SelectedIndex = 1;
                else if (vStateId == "21")
                    ddlLangu.SelectedIndex = 2;
                else if (vStateId == "4" || vStateId == "16" || vStateId == "15" || vStateId == "9" || vStateId == "23" || vStateId == "29" || vStateId == "12" || vStateId == "5")
                    ddlLangu.SelectedIndex = 3;
                else if (vStateId == "30")
                    ddlLangu.SelectedIndex = 4;
                else if (vStateId == "13")
                    ddlLangu.SelectedIndex = 5;
                else if (vStateId == "25")
                    ddlLangu.SelectedIndex = 6;
                else if (vStateId == "14")
                    ddlLangu.SelectedIndex = 7;
                else if (vStateId == "1" || vStateId == "26")
                    ddlLangu.SelectedIndex = 8;
                else
                    ddlLangu.SelectedIndex = 0;

                DataTable dt = null;
                CDisburse oDbr = new CDisburse();
                dt = oDbr.ChkDigiDocbyBranch(vBrCode, "D");

                if (dt.Rows.Count > 0)
                {
                    Session["DigiDocYN"] = Convert.ToString(dt.Rows[0]["DigiDocYN"]);
                }
                ddlLangu.Enabled = false;
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
            System.Web.UI.WebControls.ListItem Li = new System.Web.UI.WebControls.ListItem("<-- Select -->", "-1");
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
            System.Web.UI.WebControls.ListItem Li = new System.Web.UI.WebControls.ListItem("<-- Select -->", "-1");
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
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    DataRow dr = dt.Rows[i];
                    //    ViewState["VsIsBC"] = dr["IsBC"].ToString();
                    //}
                    ViewState["VsIsBC"] = Convert.ToString(dt.Rows[0]["IsBC"]);
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

        protected void chkSendback_CheckChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;

            string IsDigitalDocYN = Convert.ToString(row.Cells[18].Text);
            string CoAppSignYN = Convert.ToString(row.Cells[24].Text);
            string IsDisbYN = Convert.ToString(row.Cells[28].Text);
            string SignType = Convert.ToString(row.Cells[25].Text);
            CheckBox chkSendback = (CheckBox)row.FindControl("chkSendback");

            CMember cMem = new CMember();
            dt = (DataTable)ViewState["Sanc"];
            if (checkbox.Checked == true)
            {
                if (IsDisbYN == "Y")
                {
                    gblFuction.AjxMsgPopup("Loan Already Disbursed Cannot Send Back.");
                    chkSendback.Checked = false;
                    return;
                }
                else if (IsDigitalDocYN == "Y" && SignType != "I")
                {
                    gblFuction.AjxMsgPopup("Applicant Assisted Sign is done,Cannot Send Back");
                    chkSendback.Checked = false;
                    return;
                }
                else if (IsDigitalDocYN == "Y" && CoAppSignYN == "Y" && SignType == "I")
                {
                    gblFuction.AjxMsgPopup("Both Applicant and Co Applicant IDFY Sign is done,Cannot Send Back");
                    chkSendback.Checked = false;
                    return;
                }
                else if ((IsDigitalDocYN == "N" || CoAppSignYN == "N") && IsDisbYN == "N" && SignType == "I")
                {
                    chkSendback.Checked = true;
                }
                else if (IsDigitalDocYN == "N" && IsDisbYN == "N" && SignType != "I")
                {
                    chkSendback.Checked = true;
                }
            }
        }

        protected void btnSendBack_Click(object sender, EventArgs e)
        {
            string vXml = "";
            int vErr = 0;
            DataTable dtXml = CreateTrData();
            CApplication oMem = new CApplication();
            DateTime vSendBackDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            using (StringWriter oSW = new StringWriter())
            {
                dtXml.WriteXml(oSW);
                vXml = oSW.ToString();
            }

            vErr = oMem.DigiDocSendback(vXml, Convert.ToInt32(Session[gblValue.UserId]), vSendBackDt);
            if (vErr > 0)
            {
                gblFuction.MsgPopup("Records Sent Back Successfully.");
                LoadGrid(txtToDt.Text, txtFrmDt.Text);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "CloseWindowScript", "window.close();", true);
            }
            else
            {
                gblFuction.MsgPopup(gblMarg.DBError);
            }
        }

        private DataTable CreateTrData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "DigiDocTbl";
            dt.Columns.Add("LoanAppId", typeof(string));
            dt.Columns.Add("IsSendBackYN", typeof(string));

            for (int i = 0; i < gvSanc.Rows.Count; i++)
            {
                CheckBox chkSendback = (CheckBox)gvSanc.Rows[i].FindControl("chkSendback");
                string LoanAppId = Convert.ToString(gvSanc.Rows[i].Cells[0].Text);

                dt.Rows.Add();
                dt.Rows[i]["LoanAppId"] = LoanAppId;
                dt.Rows[i]["IsSendBackYN"] = chkSendback.Checked == true ? "Y" : "N";

            }
            dt.AcceptChanges();
            return dt;
        }

        protected void gvSanc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            CReports oRpt = null;
            DataTable dt = null;
            DataSet ds = null;
            GridViewRow gvRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            string vRptPath = "", vRptName = "", vMode = "";
            string vImgPath = "", vIsBc = "", vDigitalDocPath = "", vDigitalServerileName = "", vDigitalServerFileName = "";
            vIsBc = gvRow.Cells[15].Text;
            #region ApplicationForm
            if (e.CommandName == "cmdAppFrm")
            {
                if (gvRow.Cells[18].Text.Trim() == "Y")
                {
                    try
                    {
                        vDigitalDocPath = DigiDocpath;
                        string Id = gvRow.Cells[0].Text;
                        vDigitalServerileName = vDigitalDocPath + Id + ".pdf";
                        vDigitalServerFileName = DigiDocpathArchive + Id + ".pdf";

                        if (File.Exists(vDigitalServerileName))
                        {
                            Response.Clear();
                            Response.AddHeader("content-disposition", "attachment;filename=" + vDigitalServerileName);

                            Response.WriteFile(vDigitalServerileName);
                            Response.End();
                        }
                        else if (File.Exists(vDigitalServerFileName))
                        {
                            Response.Clear();
                            Response.AddHeader("content-disposition", "attachment;filename=" + vDigitalServerFileName);
                            Response.WriteFile(vDigitalServerFileName);
                            Response.End();
                        }
                        else
                        {
                            //string pathNetwork = ConfigurationManager.AppSettings["pathKycNetwork"];
                            //string[] arrPathNetwork = pathNetwork.Split(',');
                            //int i;
                            string vPathDigiDoc = GetDigiDocPath(Id);
                            //for (i = 0; i <= arrPathNetwork.Length - 1; i++)
                            //{
                            //    if (ValidUrlChk(arrPathNetwork[i] + "DigitalDoc/" + Id + ".pdf"))
                            //    {
                            //        vPathDigiDoc = arrPathNetwork[i] + "DigitalDoc/" + Id + ".pdf";
                            //        break;
                            //    }
                            //    else if (ValidUrlChk(arrPathNetwork[i] + "jlgdigitaldocs/" + Id + ".pdf"))
                            //    {
                            //        vPathDigiDoc = arrPathNetwork[i] + "jlgdigitaldocs/" + Id + ".pdf";
                            //        break;
                            //    }
                            //    else if (ValidUrlChk(arrPathNetwork[i] + Id + ".pdf"))
                            //    {
                            //        vPathDigiDoc = arrPathNetwork[i] + Id + ".pdf";
                            //        break;
                            //    }
                            //}
                            if (vPathDigiDoc != "")
                            {
                                using (WebClient cln = new WebClient())
                                {
                                    byte[] vDoc = null;
                                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                                    vDoc = cln.DownloadData(vPathDigiDoc);
                                    Response.AddHeader("Content-Type", "Application/octet-stream");
                                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + Id + ".pdf");
                                    Response.BinaryWrite(vDoc);
                                    Response.Flush();
                                    Response.End();
                                }
                            }
                            else
                            {
                                gblFuction.MsgPopup("No Data Found..");
                            }

                            #region old
                            //WebClient cln = null;
                            //byte[] vDoc = null;
                            //if (ValidUrlChk("https://unitynetwork1.bijliftt.com/Centrum_Image/DigitalDoc/" + Id + ".pdf"))
                            //{
                            //    cln = new WebClient();
                            //    vDoc = cln.DownloadData("https://unitynetwork1.bijliftt.com/Centrum_Image/DigitalDoc/" + Id + ".pdf");
                            //    if (vDoc != null && vDoc.Length > 0)
                            //    {
                            //        Response.AddHeader("Content-Type", "Application/octet-stream");
                            //        Response.AddHeader("Content-Disposition", "attachment;   filename=" + Id + ".pdf");
                            //        Response.BinaryWrite(vDoc);
                            //        Response.Flush();
                            //        Response.End();
                            //    }
                            //}
                            //else if (ValidUrlChk("https://networkimage3.bijliftt.com/Centrum_Image/DigitalDoc/" + Id + ".pdf"))
                            //{
                            //    cln = new WebClient();
                            //    vDoc = cln.DownloadData("https://networkimage3.bijliftt.com/Centrum_Image/DigitalDoc/" + Id + ".pdf");
                            //    if (vDoc != null && vDoc.Length > 0)
                            //    {
                            //        Response.AddHeader("Content-Type", "Application/octet-stream");
                            //        Response.AddHeader("Content-Disposition", "attachment;   filename=" + Id + ".pdf");
                            //        Response.BinaryWrite(vDoc);
                            //        Response.Flush();
                            //        Response.End();
                            //    }
                            //}
                            //else if (ValidUrlChk("https://networkimage2.bijliftt.com/Centrum_Image/DigitalDoc/" + Id + ".pdf"))
                            //{
                            //    cln = new WebClient();
                            //    vDoc = cln.DownloadData("https://networkimage2.bijliftt.com/Centrum_Image/DigitalDoc/" + Id + ".pdf");
                            //    if (vDoc != null && vDoc.Length > 0)
                            //    {
                            //        Response.AddHeader("Content-Type", "Application/octet-stream");
                            //        Response.AddHeader("Content-Disposition", "attachment;   filename=" + Id + ".pdf");
                            //        Response.BinaryWrite(vDoc);
                            //        Response.Flush();
                            //        Response.End();
                            //    }
                            //}
                            //else
                            //{
                            //    gblFuction.MsgPopup("No Data Found..");
                            //}
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        gblFuction.MsgPopup("No Data Found.." + ex.Message);
                    }
                }
                else
                {
                    gblFuction.MsgPopup("No Data Found..");
                }
            }
            #endregion
            #region LoanInstallment
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
                        rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
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
            #endregion
            #region KYC
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
            #endregion
            #region Topup
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
            #endregion
            #region QR
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
            #endregion
            #region LoanCard
            if (e.CommandName == "cmdLoanCard")
            {
                dt = new DataTable();
                DataTable dt1 = null, dt2 = null, dt3 = null;
                oRpt = new CReports();
                string Id = gvRow.Cells[0].Text;
                string vEnquiryid = gvRow.Cells[21].Text;
                string vEnqDate = gvRow.Cells[22].Text.Replace("&nbsp;", "");
                vMode = ddlLangu.SelectedValue;
                if (gvRow.Cells[18].Text.Trim() == "Y" && gvRow.Cells[25].Text.Trim() == "I")
                {
                    string vPathDigiDoc = GetDigiDocPath(Id);
                    if (vPathDigiDoc != "")
                    {
                        using (WebClient cln = new WebClient())
                        {
                            byte[] vDoc = null;
                            byte[] vLoanCard = null;
                            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                            vDoc = cln.DownloadData(vPathDigiDoc);
                            vLoanCard = IdfyLoanCard(vDoc, ddlLangu.SelectedValue);
                            Response.AddHeader("Content-Type", "Application/octet-stream");
                            Response.AddHeader("Content-Disposition", "attachment;   filename=" + Id + "_LoanCard.pdf");
                            Response.BinaryWrite(vLoanCard);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
                else
                {
                    ds = oRpt.RptLoanCard_DocPrint(Id);
                    //ds = oRpt.RptLoanCard_DocPrint_KFS(Id);
                    dt1 = ds.Tables[0];
                    dt1.TableName = "MemLoanInfo";
                    dt2 = ds.Tables[1];
                    if (dt1.Rows.Count > 0)
                    {
                        //---------------------Image Drive Check---------------------                    
                        string ImgPath = dt1.Rows[0]["ImgPath"].ToString();
                        string NewFormatYN = dt1.Rows[0]["NewFormatYN"].ToString();
                        string vReportName = "";
                        //------------------------------------------------------                   
                        foreach (DataRow dr in dt1.Rows)
                        {
                            dr["ImgByte"] = GetByteImage("MemberPhoto.png", vEnqDate + vEnquiryid);
                        }
                        dt1.AcceptChanges();
                        //-----------------------------------------------------
                        using (ReportDocument rptDoc = new ReportDocument())
                        {
                            if (vMode == "O")
                            {
                                //vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptLoanCardOdiya.rpt";
                                vReportName = NewFormatYN == "Y" ? "Reports\\RptLoanCardOdiya_New.rpt" : "Reports\\RptLoanCardOdiya.rpt";
                                vRptPath = Request.PhysicalApplicationPath.ToString() + vReportName;
                            }
                            else if (vMode == "G")
                            {
                                //vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptLoanCardGujrati.rpt";
                                vReportName = NewFormatYN == "Y" ? "Reports\\RptLoanCardGujrati_New.rpt" : "Reports\\RptLoanCardGujrati.rpt";
                                vRptPath = Request.PhysicalApplicationPath.ToString() + vReportName;
                            }
                            else if (vMode == "B")
                            {
                                //vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptLoanCardBengali.rpt";
                                vReportName = NewFormatYN == "Y" ? "Reports\\RptLoanCardBengali_New.rpt" : "Reports\\RptLoanCardBengali.rpt";
                                vRptPath = Request.PhysicalApplicationPath.ToString() + vReportName;
                            }
                            else if (vMode == "K")
                            {
                                //vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptLoanCardKannad.rpt";
                                vReportName = NewFormatYN == "Y" ? "Reports\\RptLoanCardKannad_New.rpt" : "Reports\\RptLoanCardKannad.rpt";
                                vRptPath = Request.PhysicalApplicationPath.ToString() + vReportName;
                            }
                            else if (vMode == "T")
                            {
                                //vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptLoanCardTamil.rpt";
                                vReportName = NewFormatYN == "Y" ? "Reports\\RptLoanCardTamil_New.rpt" : "Reports\\RptLoanCardTamil.rpt";
                                vRptPath = Request.PhysicalApplicationPath.ToString() + vReportName;
                            }
                            else if (vMode == "M")
                            {
                                //vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptLoanCardMalayalam.rpt";
                                vReportName = NewFormatYN == "Y" ? "Reports\\RptLoanCardMalayalam_New.rpt" : "Reports\\RptLoanCardMalayalam.rpt";
                                vRptPath = Request.PhysicalApplicationPath.ToString() + vReportName;
                            }
                            else if (vMode == "L")
                            {
                                //vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptLoanCardTelugu.rpt";
                                vReportName = NewFormatYN == "Y" ? "Reports\\RptLoanCardTelugu_New.rpt" : "Reports\\RptLoanCardTelugu.rpt";
                                vRptPath = Request.PhysicalApplicationPath.ToString() + vReportName;
                            }
                            else
                            {
                                if (vIsBc == "Y")
                                {
                                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptLoanCardHindiBC.rpt";
                                }
                                else
                                {
                                    //vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptLoanCardEnglish.rpt";
                                    vReportName = NewFormatYN == "Y" ? "Reports\\RptLoanCardHindi_New.rpt" : "Reports\\RptLoanCardEnglish.rpt";
                                    vRptPath = Request.PhysicalApplicationPath.ToString() + vReportName;
                                }
                            }

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
            }
            #endregion
            #region DigitalDoc
            if (e.CommandName == "cmdDigitalDocForm")
            {
                if (gvRow.Cells[24].Text == "Y" && ddlDocType.SelectedValue != gvRow.Cells[25].Text)
                {
                    gblFuction.AjxMsgPopup("Failed:Please select correct digital signature provider.");
                    return;
                }
                if (ddlDocType.SelectedValue == "A" && gvRow.Cells[26].Text == "Y")
                {
                    gblFuction.AjxMsgPopup("Failed:IDFY Digital Signature is mandatory for this Customer.");
                    return;
                }
                if (ddlDocType.SelectedValue == "A")
                {
                    string vLoanAppId = gvRow.Cells[0].Text;
                    string vUrl = ConfigurationManager.AppSettings["vRequestUrl"];
                    string vToken = "";
                    Random ran = new Random();
                    String b = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    int length = 100;
                    String vRandomToken = "";
                    for (int i = 0; i < length; i++)
                    {
                        int a = ran.Next(b.Length); //string.Lenght gets the size of string
                        vRandomToken = vRandomToken + b.ElementAt(a);
                    }

                    vToken = vRandomToken;
                    CMember Omem = new CMember();
                    Int32 vErr = Omem.SaveInitiateDigitalDoc(vLoanAppId, GetMACAddress(), vUrl, vToken, "A", "A");

                    if (vErr == 0)
                    {
                        string url = vUrl + "?vLoanApp=" + vLoanAppId + "&vToken=" + vToken + "&vGroupId=" + ddlGroup.SelectedValue + "&vLanguage=" + ddlLangu.SelectedValue + "&vDateFrom=" + txtFrmDt.Text + "&vDateTo=" + txtToDt.Text;
                        string s = "window.open('" + url + "', 'popup_window', 'width=900,height=600,left=100,top=100,resizable=yes');";
                        ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Failed:Contact to system admin");
                    }
                }
                else if (ddlDocType.SelectedValue == "U")
                {
                    DataTable dtMob = null;
                    CMember oMem = null;

                    string vLoanAppId = gvRow.Cells[0].Text;
                    string vUrl = ConfigurationManager.AppSettings["vRequestUrl"];
                    string pSrtUrl = ConfigurationManager.AppSettings["vSrtUrl"];
                    string vToken = "";
                    Random ran = new Random();
                    String b = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    int length = 40;
                    String vRandomToken = "";
                    for (int i = 0; i < length; i++)
                    {
                        int a = ran.Next(b.Length); //string.Lenght gets the size of string
                        vRandomToken = vRandomToken + b.ElementAt(a);
                    }
                    vToken = vRandomToken;

                    oMem = new CMember();
                    Int32 vErr = oMem.SaveInitiateDigitalDoc(vLoanAppId, GetMACAddress(), vUrl, vToken, "U", "A");
                    if (vErr == 0)
                    {
                        dtMob = new DataTable();
                        oMem = new CMember();
                        dtMob = oMem.GetDisbSMSMemMob(vLoanAppId);
                        if (dtMob.Rows.Count > 0)
                        {
                            string vMemMobNo = Convert.ToString(dtMob.Rows[0]["MobNo"]).Trim();
                            oMem = new CMember();
                            Int64 vDigiDocId = oMem.GetDigitalDocId(vLoanAppId, vToken);

                            string url = pSrtUrl + "?p=" + vToken.Substring(0, 5) + Base64Encode(Convert.ToString(vDigiDocId)) + "&l=" + ddlLangu.SelectedValue;

                            // string vMessageBody = "Thank you for loan application no " + vLoanAppId + ". Please verify loan documents " + url + " Centrum Microcredit Ltd.";
                            string vMessageBody = "Thank you for loan application no " + vLoanAppId + " with Unity SFB. Please verify loan documents " + url;
                            string vRe = SendSMS(vMemMobNo, vMessageBody);
                            string[] arr = vRe.Split('|');
                            string vSuccessStat = arr[0];
                            if (vSuccessStat.ToString().Trim().ToLower() == "success")
                            {
                                gblFuction.AjxMsgPopup("Success:Successfully Sent.");
                            }
                            else
                            {
                                gblFuction.AjxMsgPopup("Failed:Unable to deliver.");
                            }
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Failed:Mobile no not Present.");
                        }
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Failed:Contact to system admin");
                    }
                }
                else if (ddlDocType.SelectedValue == "D")
                {
                    string vLoanAppId = gvRow.Cells[0].Text;
                    string vDigiSignRequestUrl = ConfigurationManager.AppSettings["vDigiSignRequestUrl"];
                    string vToken = "";
                    CMember Omem = new CMember();
                    Int32 vErr = Omem.SaveInitiateDigitalDoc(vLoanAppId, GetMACAddress(), vDigiSignRequestUrl, vToken, "D", "A");
                    if (vErr == 0)
                    {
                        string url = vDigiSignRequestUrl + "?vLoanApp=" + vLoanAppId + "&vToken=" + vToken + "&vGroupId=" + ddlGroup.SelectedValue + "&vLanguage=" + ddlLangu.SelectedValue + "&vDateFrom=" + txtFrmDt.Text + "&vDateTo=" + txtToDt.Text;
                        string s = "window.open('" + url + "', 'popup_window', 'width=900,height=600,left=100,top=100,resizable=yes');";
                        ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Failed:Contact to system admin");
                    }
                }
                else if (ddlDocType.SelectedValue == "I")
                {
                    string vLoanAppId = gvRow.Cells[0].Text;
                    string vDigiSignRequestUrl = ConfigurationManager.AppSettings["vIdfyDigiSignRequestUrl"];
                    string vToken = "";
                    CMember Omem = new CMember();
                    Int32 vErr = Omem.SaveInitiateDigitalDoc(vLoanAppId, GetMACAddress(), vDigiSignRequestUrl, vToken, "I", "A");
                    if (vErr == 0)
                    {
                        string url = vDigiSignRequestUrl + "?vLoanApp=" + vLoanAppId + "&vToken=" + vToken + "&vGroupId=" + ddlGroup.SelectedValue + "&vLanguage=" + ddlLangu.SelectedValue + "&vDateFrom=" + txtFrmDt.Text + "&vDateTo=" + txtToDt.Text;
                        string s = "window.open('" + url + "', 'popup_window', 'width=900,height=600,left=100,top=100,resizable=yes');";
                        ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Failed:Contact to system admin");
                    }
                }
                else
                {
                    DataSet ds1 = new DataSet(), dsDigiDoc = new DataSet();
                    DataTable dt1 = new DataTable(), dt2 = new DataTable(), dt3 = new DataTable();
                    DataTable dt4 = new DataTable(), dt5 = new DataTable(), dt6 = new DataTable();
                    DataTable dt7 = new DataTable(), dtDigiDocDtls = new DataTable(), dt8 = new DataTable();
                    oRpt = new CReports();
                    vRptPath = string.Empty;
                    if (ddlLangu.SelectedValue == "E") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocForm.rpt";
                    if (ddlLangu.SelectedValue == "H") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocFormHindi.rpt";
                    if (ddlLangu.SelectedValue == "G") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocFormGujarati.rpt";
                    if (ddlLangu.SelectedValue == "O") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocFormOdia.rpt";
                    if (ddlLangu.SelectedValue == "B") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintBengali.rpt";
                    if (ddlLangu.SelectedValue == "K") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintKannad.rpt";
                    if (ddlLangu.SelectedValue == "T") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintTamil.rpt";
                    if (ddlLangu.SelectedValue == "M") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintMalayalam.rpt";
                    if (ddlLangu.SelectedValue == "L") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintTelugu.rpt";
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

                    ds = new DataSet();
                    ds = oRpt.RptLoanCard_DocPrint(Id);
                    dt8 = ds.Tables[0];
                    dt8.TableName = "MemLoanInfo";

                    string vPath = "";
                    if (dt1.Rows.Count > 0)
                    {
                        if (vRptPath != string.Empty)
                        {
                            //-----------------------Download Images-----------------------
                            string vEnqId = dt7.Rows[0]["EnquiryId"].ToString();
                            string vEnqDate = dt7.Rows[0]["EnqDate"].ToString().Trim();
                            //string pRequestdata = "{\"pEnquiryId\":\"" + vEnqId + "\"}";
                            //CallAPI("GetImage", pRequestdata, "https://unityimage.bijliftt.com/ImageDownloadService.svc");
                            //-------------------------------------------------------------
                            string vMemberPhoto = "";
                            foreach (DataRow dr in dt1.Rows)
                            {
                                //string vMemPhoto = Convert.ToString(dr["ImgPath"]);
                                //vMemPhoto = vMemPhoto.Substring(1, vMemPhoto.Length - 1).Replace('\\', '_');
                                //vMemberPhoto = PathMinio + vMemPhoto;
                                //dr["ImgPath"] = vMemberPhoto;

                                dr["ImgByte"] = GetByteImage("MemberPhoto.png", vEnqDate + vEnqId);
                            }
                            dt1.AcceptChanges();
                            if (dt7.Rows.Count > 0)
                            {
                                vImgPath = PathMinio;
                                foreach (DataRow dr in dt7.Rows)
                                {
                                    //dr["KYC1"] = vImgPath + (Convert.ToString(dr["KYC1"]).Substring(1, Convert.ToString(dr["KYC1"]).Length - 1).Replace('\\', '_'));
                                    //dr["KYC1Back"] = vImgPath + (Convert.ToString(dr["KYC1Back"]).Substring(1, Convert.ToString(dr["KYC1Back"]).Length - 1).Replace('\\', '_'));
                                    //dr["KYC2"] = vImgPath + (Convert.ToString(dr["KYC2"]).Substring(1, Convert.ToString(dr["KYC2"]).Length - 1).Replace('\\', '_'));
                                    //dr["KYC2Back"] = vImgPath + (Convert.ToString(dr["KYC2Back"]).Substring(1, Convert.ToString(dr["KYC2Back"]).Length - 1).Replace('\\', '_'));
                                    //dr["KYC3"] = vImgPath + (Convert.ToString(dr["KYC3"]).Substring(1, Convert.ToString(dr["KYC3"]).Length - 1).Replace('\\', '_')); ;
                                    //dr["KYC3Back"] = vImgPath + (Convert.ToString(dr["KYC3Back"]).Substring(1, Convert.ToString(dr["KYC3Back"]).Length - 1).Replace('\\', '_'));
                                    if (dt7.Rows[0]["M_IdentyPRofId"].ToString() == "13")
                                    {
                                        dr["KYC1Byte"] = GetAadhaarByteImage("IDProofImage.png", vEnqDate + vEnqId);
                                        dr["KYC1BackByte"] = GetAadhaarByteImage("IDProofImageBack.png", vEnqDate + vEnqId);
                                    }
                                    else
                                    {
                                        dr["KYC1Byte"] = GetByteImage("IDProofImage.png", vEnqDate + vEnqId);
                                        dr["KYC1BackByte"] = GetByteImage("IDProofImageBack.png", vEnqDate + vEnqId);
                                    }
                                    if (dt7.Rows[0]["M_AddProfId"].ToString() == "13")
                                    {
                                        dr["KYC2Byte"] = GetAadhaarByteImage("AddressProofImage.png", vEnqDate + vEnqId);
                                        dr["KYC2BackByte"] = GetAadhaarByteImage("AddressProofImageBack.png", vEnqDate + vEnqId);
                                    }
                                    else
                                    {
                                        dr["KYC2Byte"] = GetByteImage("AddressProofImage.png", vEnqDate + vEnqId);
                                        dr["KYC2BackByte"] = GetByteImage("AddressProofImageBack.png", vEnqDate + vEnqId);
                                    }
                                    if (dt7.Rows[0]["B_IdentyProfId"].ToString() == "13")
                                    {
                                        dr["KYC3Byte"] = GetAadhaarByteImage("AddressProofImage2.png", vEnqDate + vEnqId);
                                        dr["KYC3BackByte"] = GetAadhaarByteImage("AddressProofImage2Back.png", vEnqDate + vEnqId);
                                    }
                                    else
                                    {
                                        dr["KYC3Byte"] = GetByteImage("AddressProofImage2.png", vEnqDate + vEnqId);
                                        dr["KYC3BackByte"] = GetByteImage("AddressProofImage2Back.png", vEnqDate + vEnqId);
                                    }

                                }
                            }
                            dt7.AcceptChanges();

                            if (dt3.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt3.Rows)
                                {
                                    // dr["ImgPath"] = vMemberPhoto;
                                    dr["ImgByte"] = GetByteImage("MemberPhoto.png", vEnqDate + vEnqId);
                                }
                                dt3.AcceptChanges();
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
                                else if (ddlLangu.SelectedValue == "H")
                                {
                                    rptDoc.Subreports["DigitalDocFormP2Hindi.rpt"].SetDataSource(dt2);
                                    rptDoc.Subreports["DigitalDocFormP3Hindi.rpt"].SetDataSource(dt3);
                                    rptDoc.Subreports["DigitalDocFormP4Hindi.rpt"].SetDataSource(dt4);
                                    rptDoc.Subreports["DigitalDocFormP5Hindi.rpt"].SetDataSource(dt5);
                                    rptDoc.Subreports["DigitalDocFormP6Hindi.rpt"].SetDataSource(dt6);
                                    rptDoc.Subreports["DigitalDocFormP7Hindi.rpt"].SetDataSource(dt7);
                                    rptDoc.Subreports["Kfs"].SetDataSource(dt8);
                                }
                                else if (ddlLangu.SelectedValue == "G")
                                {
                                    rptDoc.Subreports["DigitalDocFormP2Gujarati.rpt"].SetDataSource(dt2);
                                    rptDoc.Subreports["DigitalDocFormP3Gujarati.rpt"].SetDataSource(dt3);
                                    rptDoc.Subreports["DigitalDocFormP4Gujarati.rpt"].SetDataSource(dt4);
                                    rptDoc.Subreports["DigitalDocFormP5Gujarati.rpt"].SetDataSource(dt5);
                                    rptDoc.Subreports["DigitalDocFormP6Gujarati.rpt"].SetDataSource(dt6);
                                    rptDoc.Subreports["DigitalDocFormP7Gujarati.rpt"].SetDataSource(dt7);
                                }
                                else if (ddlLangu.SelectedValue == "O")
                                {
                                    rptDoc.Subreports["DigitalDocFormP2Odia.rpt"].SetDataSource(dt2);
                                    rptDoc.Subreports["DigitalDocFormP3Odia.rpt"].SetDataSource(dt3);
                                    rptDoc.Subreports["DigitalDocFormP4Odia.rpt"].SetDataSource(dt4);
                                    rptDoc.Subreports["DigitalDocFormP5Odia.rpt"].SetDataSource(dt5);
                                    rptDoc.Subreports["DigitalDocFormP6Odia.rpt"].SetDataSource(dt6);
                                    rptDoc.Subreports["DigitalDocFormP7Odia.rpt"].SetDataSource(dt7);
                                }
                                else if (ddlLangu.SelectedValue == "B")
                                {
                                    rptDoc.Subreports["DigitalDocFormP2Bengali.rpt"].SetDataSource(dt2);
                                    rptDoc.Subreports["DigitalDocFormP3Bengali.rpt"].SetDataSource(dt3);
                                    rptDoc.Subreports["DigitalDocFormP4Bengali.rpt"].SetDataSource(dt4);
                                    rptDoc.Subreports["DigitalDocFormP5Bengali.rpt"].SetDataSource(dt5);
                                    rptDoc.Subreports["DigitalDocFormP6Bengali.rpt"].SetDataSource(dt6);
                                    rptDoc.Subreports["DigitalDocFormP7Bengali.rpt"].SetDataSource(dt7);
                                }
                                else if (ddlLangu.SelectedValue == "K")
                                {
                                    rptDoc.Subreports["DigitalDocFormP2Kannad"].SetDataSource(dt2);
                                    rptDoc.Subreports["DigitalDocFormP3Kannad"].SetDataSource(dt3);
                                    rptDoc.Subreports["DigitalDocFormP4Kannad"].SetDataSource(dt4);
                                    rptDoc.Subreports["DigitalDocFormP5Kannad.rpt"].SetDataSource(dt5);
                                    rptDoc.Subreports["DigitalDocFormP6Kannad.rpt"].SetDataSource(dt6);
                                    rptDoc.Subreports["DigitalDocFormP7Kannada.rpt"].SetDataSource(dt7);
                                }
                                else if (ddlLangu.SelectedValue == "T")
                                {
                                    rptDoc.Subreports["DigitalDocFormP2Kannad"].SetDataSource(dt2);
                                    rptDoc.Subreports["DigitalDocFormP3Kannad"].SetDataSource(dt3);
                                    rptDoc.Subreports["DigitalDocFormP4Kannad"].SetDataSource(dt4);
                                    rptDoc.Subreports["DigitalDocFormP5Tamil.rpt"].SetDataSource(dt5);
                                    rptDoc.Subreports["DigitalDocFormP6Tamil.rpt"].SetDataSource(dt6);
                                    rptDoc.Subreports["DigitalDocFormP7Tamil.rpt"].SetDataSource(dt7);
                                }
                                else if (ddlLangu.SelectedValue == "M")
                                {
                                    rptDoc.Subreports["DigitalDocFormP2Kannad"].SetDataSource(dt2);
                                    rptDoc.Subreports["DigitalDocFormP3Kannad"].SetDataSource(dt3);
                                    rptDoc.Subreports["DigitalDocFormP4Kannad"].SetDataSource(dt4);
                                    rptDoc.Subreports["DigitalDocFormP5Malayalam.rpt"].SetDataSource(dt5);
                                    rptDoc.Subreports["DigitalDocFormP6Malayalam.rpt"].SetDataSource(dt6);
                                    rptDoc.Subreports["DigitalDocFormP7Malayalam.rpt"].SetDataSource(dt7);
                                }
                                else if (ddlLangu.SelectedValue == "L")
                                {
                                    rptDoc.Subreports["DigitalDocFormP2Bengali.rpt"].SetDataSource(dt2);
                                    rptDoc.Subreports["DigitalDocFormP3Bengali.rpt"].SetDataSource(dt3);
                                    rptDoc.Subreports["DigitalDocFormP4Bengali.rpt"].SetDataSource(dt4);
                                    rptDoc.Subreports["DigitalDocFormP5Bengali.rpt"].SetDataSource(dt5);
                                    rptDoc.Subreports["DigitalDocFormP6Bengali.rpt"].SetDataSource(dt6);
                                    rptDoc.Subreports["DigitalDocFormP7Bengali.rpt"].SetDataSource(dt7);
                                }

                                rptDoc.Subreports["DigitalDoc_eSigned.rpt"].SetDataSource(dtDigiDocDtls);

                                if (ddlDocType.SelectedValue == "M")
                                {
                                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, Id + '_' + vRptName);
                                    Response.ClearContent();
                                    Response.ClearHeaders();
                                }
                                else if (ddlDocType.SelectedValue == "D")
                                {
                                    //#region API Call for Digital Document
                                    ////pdf save
                                    //string vNSDLResponse = "";
                                    //string vFileName = "";
                                    //vFileName = PathDDHTML + Id + '_' + "JLG" + "_" + Convert.ToString(dt1.Rows[0]["MemberName"]);
                                    //rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, vFileName + ".pdf");

                                    ////string vNSDLResponse = EquifaxDIGITALDOCUMENT("demo@authbridge.com", Id, Convert.ToString(dt1.Rows[0]["MemberName"]), "", vRptName, "Mumbai");
                                    //vNSDLResponse = EquifaxDIGITALDOCUMENT("production.centrum@equifax.com", Id, Convert.ToString(dt1.Rows[0]["MemberName"]), "", "JLG", "Mumbai");
                                    ////----------------------------------
                                    //oRpt = new CReports();
                                    //oRpt.SavePNBDigitalDocResponse(Id, vNSDLResponse);
                                    ////----------------------------------
                                    //File.WriteAllText(vFileName + ".htm", vNSDLResponse);
                                    //vNSDLResponse = vNSDLResponse.Replace("\\n", "");

                                    //string url = vFileName + ".htm";
                                    ///*
                                    // * ///////////////////////////////////
                                    //AutoResetEvent resultEvent = new AutoResetEvent(false);
                                    //browser = new MyWebBrowser(true, url, resultEvent);


                                    /////////////////////////////////////
                                    // * */
                                    //downloadfile(url);
                                    //#endregion



                                }
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
            #endregion
            #region Insurance
            if (e.CommandName == "cmdInsurance")
            {
                string vLoanAppId = gvRow.Cells[0].Text;
                string vIcId = gvRow.Cells[20].Text;
                dt = new DataTable();
                oRpt = new CReports();
                dt = oRpt.GetInsuFormDtl(vLoanAppId);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(vIcId) == 6 || Convert.ToInt32(vIcId) == 7)
                    {
                        using (ReportDocument rptDoc = new ReportDocument())
                        {
                            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\Federal.rpt";
                            vRptName = vLoanAppId + "_Federal_Claim_Form";
                            rptDoc.Load(vRptPath);
                            rptDoc.SetDataSource(dt);
                            rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, vRptName);
                            Response.ClearHeaders();
                            Response.ClearContent();
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("Insuranc form not found.");
                        return;
                    }
                }
            }
            #endregion
            #region CoAppDigitalSign
            if (e.CommandName == "cmdCoDigiDocForm")
            {
                string vLoanAppId = gvRow.Cells[0].Text;
                string vTypeOfSign = ddlDocType.SelectedValue;

                string vUrl = ConfigurationManager.AppSettings["vCoAppDigiSignRequestUrl"];
                string vToken = "";
                Random ran = new Random();
                String b = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                int length = 40;
                String vRandomToken = "";
                for (int i = 0; i < length; i++)
                {
                    int a = ran.Next(b.Length); //string.Lenght gets the size of string
                    vRandomToken = vRandomToken + b.ElementAt(a);
                }

                vToken = vRandomToken;
                CMember Omem = new CMember();
                Int32 vErr = Omem.SaveInitiateDigitalDoc(vLoanAppId, GetMACAddress(), vUrl, vToken, vTypeOfSign, "C");

                if (vErr == 0)
                {
                    string url = vUrl + "?vLoanApp=" + vLoanAppId + "&vToken=" + vToken + "&vGroupId=" + ddlGroup.SelectedValue + "&vLanguage=" + ddlLangu.SelectedValue + "&vDateFrom=" + txtFrmDt.Text + "&vDateTo=" + txtToDt.Text + "&vTypeOfSign=" + vTypeOfSign;
                    string s = "window.open('" + url + "', 'popup_window', 'width=900,height=600,left=100,top=100,resizable=yes');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
            }
            #endregion
        }

        protected void gvSanc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton btnDigitalDocForm = (ImageButton)e.Row.FindControl("btnDigitalDocForm");
                    ImageButton btnCoDigiDocForm = (ImageButton)e.Row.FindControl("btnCoDigiDocForm");
                    string IsDisbYN = Convert.ToString(e.Row.Cells[28].Text);
                    string SignType = Convert.ToString(e.Row.Cells[25].Text);
                    string IsDigitalDocYN = Convert.ToString(e.Row.Cells[18].Text);
                    string CoAppSignYN = Convert.ToString(e.Row.Cells[24].Text);
                    CheckBox chkSendback = (CheckBox)e.Row.FindControl("chkSendback");

                    string vDocType = ddlDocType.SelectedValue;
                    if (e.Row.Cells[24].Text == "N" && Convert.ToString(Session["DigiDocYN"]) == "Y" && (vDocType == "I" || vDocType == "D"))
                    {
                        btnDigitalDocForm.Enabled = false;
                        btnCoDigiDocForm.Enabled = true;
                    }
                    else
                    {
                        btnDigitalDocForm.Enabled = true;
                        btnCoDigiDocForm.Enabled = false;
                    }

                    if (IsDisbYN == "Y")
                    {
                        chkSendback.Enabled = false;
                    }
                    else if (IsDigitalDocYN == "Y" && SignType != "I")
                    {
                        chkSendback.Enabled = false;
                    }
                    else if (IsDigitalDocYN == "Y" && CoAppSignYN == "Y" && SignType == "I")
                    {
                        chkSendback.Enabled = false;
                    }
                    else if (IsDigitalDocYN == "N" && IsDisbYN == "N" && SignType == "I")
                    {
                        chkSendback.Enabled = true;
                    }
                    else if (IsDigitalDocYN == "N" && IsDisbYN == "N" && SignType != "I")
                    {
                        chkSendback.Enabled = true;
                    }
                }
            }
            catch { }
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
        /// <param name="filename"></param>
        protected void downloadfile(string filename)
        {

            // check to see that the file exists 
            if (File.Exists(filename))
            {
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);

                Response.WriteFile(filename);
                //System.Diagnostics.Process.Start("C:\\WINDOWS\\explorer.exe", filename);
                //gblFuction.AjxMsgPopup(filename);

                Response.End();
                File.Delete(filename);
            }
            else
            {
                gblFuction.AjxMsgPopup("File could not be found");
            }
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

        #region Digital Document Related Class and Method

        public class FileParameter
        {
            public string transID { get; set; }             //Mandatory		Transaction ID provided by Client to make each transaction unique
            public string docType { get; set; }             //Mandatory		Document type is 42 for this.
            public byte[] file { get; set; }              //Mandatory		This indicates the type of document being uploaded for eSign. We currently support PDF, limit 3MB.
            public string pages { get; set; }               //Mandatory		1 - Last Page 2 - All Pages 3 - Pages (Indicates page no on which you want signature to be placed by the help of page_no parameter)
            public string firstName { get; set; }           //Mandatory		The first name which would be displayed in the digital signature. No name would be displayed in the signature if this is missing.
            public string lastName { get; set; }            //Optional		The last name which would be displayed in the digital signature. No name would be displayed in the signature if this is missing.
            public string reason { get; set; }              //String,Mandatory	Specify the reason for e-signing this document
            public string location { get; set; }            //String,Optional		Optionally specify the location where e-signing is being done
            public string page_no { get; set; }             //Mandatory		Indicates page numbers on which you want signatures to be placed. To be used in case of "3" in pages fields. The field should contain the page numbers based on the multiple signature requirement in a single document. For example if we need to to take 5 signs in a 3 page PDF, where 2 signs are on 1st & 2nd page and 1 sign is on last page the passed value should be 1,1,2,2,3
            public string x_cordinate { get; set; }         //Mandatory		Indicates x coordinates represent the horizontal postion of the signature on page, where you want to place the signature. To be used in case of "3" in pages fields. The number of x coordinates will should be equal to the number of times page numbers are passed in page_no filed. For eg, for above mentioned expample there should be 5 x coordinates eg. 40, 200, 40, 300, 50
            public string y_cordinate { get; set; }         //Mandatory		Indicates y coordinates represent the vertical postion of the signature on page, where you want to place the signature. To be used in case of "3" in pages fields. The number of y coordinates will should be equal to the number of times page numbers are passed in page_no filed. For eg, for above mentioned expample there should be 5 y coordinates eg. 60,60,60,60,60
            public string authmode { get; set; }            //Mandatory		Authmode value should by OTP or BIO
            //public byte[] File { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }

            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] pfile, string filename, string contenttype)
            {
                file = pfile;
                FileName = filename;
                ContentType = contenttype;
            }
        }

        public string MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

            return PostForm(postUrl, userAgent, contentType, formData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postParameters"></param>
        /// <param name="boundary"></param>
        /// <returns></returns>
        private byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {
                // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                    // Write the file data directly to the Stream, rather than serializing it to a string.
                    formDataStream.Write(fileToUpload.file, 0, fileToUpload.file.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postUrl"></param>
        /// <param name="userAgent"></param>
        /// <param name="contentType"></param>
        /// <param name="formData"></param>
        /// <param name="pUniqueID"></param>
        /// <param name="pFirstName"></param>
        /// <param name="pLastName"></param>
        /// <param name="pReason"></param>
        /// <param name="pLocation"></param>
        /// <returns></returns>
        //private HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
        private string PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }

                // Set up the request properties.
                request.Method = "POST";
                request.ContentType = contentType;
                request.CookieContainer = new CookieContainer();

                request.Headers.Add("username" + ":" + userAgent);

                request.KeepAlive = false;
                request.Timeout = System.Threading.Timeout.Infinite;
                request.ContentLength = formData.Length;

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(formData, 0, formData.Length);
                    requestStream.Close();
                }


                ///You must write ContentLength bytes to the request stream before calling [Begin]GetResponse.
                ///
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();

                //return request.GetResponse() as HttpWebResponse;

                return fullResponse;
            }
            catch (WebException ex)
            {
                string Response = "";
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Response = reader.ReadToEnd();
                }
                return Response;
            }
            finally
            {
                // streamWriter = null;
            }
        }

        public string EquifaxDIGITALDOCUMENT(string pUserName, string pUniqueID, string pFirstName, string pLastName, string pReason, string pLocation)
        {
            string vFileName = "";
            vFileName = "C:\\DDHTML\\" + pUniqueID + '_' + pReason + "_" + pFirstName + ".pdf";
            // Read file data
            FileStream fs = new FileStream(vFileName, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();
            pUniqueID = pUniqueID + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "");
            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("filename", vFileName);
            //postParameters.Add("fileformat", "pdf");
            postParameters.Add("transID", pUniqueID);
            postParameters.Add("docType", "373");//"42" for esign ------- 373 aadhaar esign
            postParameters.Add("file", new FileParameter(data, vFileName, "application/pdf"));
            postParameters.Add("pages", "2");
            postParameters.Add("firstName", pFirstName);
            postParameters.Add("lastName", pLastName);
            postParameters.Add("reason", pReason);
            postParameters.Add("location", pLocation);
            postParameters.Add("page_no", "1");
            postParameters.Add("x_cordinate", "60");//2000
            postParameters.Add("y_cordinate", "60");
            postParameters.Add("authmode", ddlauthmode.SelectedValue);

            // Create request and receive response
            string postURL = "https://www.truthscreen.com/api/v2.2/aadhaaresignapi"; // "test_centrum@equifax.com";

            //  string postURL = "https://uat.truthscreen.com/truthscreen-uat/api/v2.2/aadhaaresignapi";

            string fullResponse = MultipartFormDataPost(postURL, pUserName, postParameters);
            return fullResponse;
        }

        #endregion

        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            } return sMacAddress;
        }

        //-------------------------Send SMS------------------------------------
        public string SendSMS(string pMobileNo, string pMsgBody)
        {
            string result = "";
            WebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                //Predefined template can not be changed.if want to change then need to be verified by Gupshup (SMS provider)
                string vMsgBody = string.Empty;
                //vMsgBody = pMsgBody;
                vMsgBody = System.Web.HttpUtility.UrlEncode(pMsgBody, System.Text.Encoding.GetEncoding("ISO-8859-1"));
                //vMsgBody = System.Web.HttpUtility.UrlEncode(pMsgBody, System.Text.Encoding.GetEncoding("ISO-8859-15"));
                //********************************************************************
                String sendToPhoneNumber = pMobileNo;
                //String userid = "2000194447";
                //String passwd = "Centrum@2020";
                String userid = "2000203137";
                String passwd = "UnitySFB@1";
                //String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1007861727120133444&principalEntityId=1001301154610005078&mask=ARGUSS&v=1.1&format=text";
                String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1707163472077801532&principalEntityId=1701163041834094915&mask=UNTYBK&v=1.1&format=text";
                request = WebRequest.Create(url);
                // Send the 'HttpWebRequest' and wait for response.
                response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader reader = new System.IO.StreamReader(stream, ec);
                reader = new System.IO.StreamReader(stream);
                result = reader.ReadToEnd();
                reader.Close();
                stream.Close();
            }
            catch (Exception exp)
            {
                result = "Error sending SMS.." + exp.ToString();
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return result;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        #region URLExist
        public bool ValidUrlChk(string pUrl)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pUrl);
                using (WebResponse response = request.GetResponse())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        public string CallAPI(string pApiName, string pRequestdata, string pReportUrl)
        {
            try
            {
                string Requestdata = pRequestdata;
                string postURL = pReportUrl + "/" + pApiName;
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                responseReader.ReadToEnd();
                request.GetResponse().Close();
            }
            finally
            {
            }
            return "";
        }

        //#region Minio Image Upload
        //public string UploadFileMinio(byte[] image, string fileName, string enqId, string bucketName, string minioUrl)
        //{
        //    string fullResponse = "", isImageSaved = "N";
        //    Dictionary<string, object> postParameters = new Dictionary<string, object>();
        //    postParameters.Add("image", Convert.ToBase64String(image));
        //    postParameters.Add("KycId", enqId);
        //    postParameters.Add("BucketName", bucketName);
        //    postParameters.Add("ImageName", fileName);
        //    // Create request and receive response
        //    //  string postURL = "https://ocr.bijliftt.com/KYCFileUploadBase64";
        //    string postURL = minioUrl;
        //    string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
        //    string contentType = "multipart/form-data; boundary=" + formDataBoundary;
        //    byte[] formDataforRequest = GetMultipartFormData(postParameters, formDataBoundary);
        //    try
        //    {
        //        HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
        //        request.Method = "POST";
        //        request.ContentType = contentType;
        //        request.CookieContainer = new CookieContainer();
        //        request.KeepAlive = false;
        //        request.Timeout = System.Threading.Timeout.Infinite;
        //        request.ContentLength = formDataforRequest.Length;

        //        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
        //        using (Stream requestStream = request.GetRequestStream())
        //        {
        //            requestStream.Write(formDataforRequest, 0, formDataforRequest.Length);
        //            requestStream.Close();
        //        }
        //        StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
        //        fullResponse = responseReader.ReadToEnd();
        //        request.GetResponse().Close();
        //    }
        //    catch (WebException ex)
        //    {
        //        using (var stream = ex.Response.GetResponseStream())
        //        using (var reader = new StreamReader(stream))
        //        {
        //            fullResponse = reader.ReadToEnd();
        //        }
        //    }
        //    finally
        //    {
        //    }
        //    dynamic obj = JsonConvert.DeserializeObject(fullResponse);
        //    bool status = obj.status;
        //    if (status == true)
        //    {
        //        isImageSaved = "Y";
        //    }
        //    return isImageSaved;
        //}

        //public static byte[] strmToByte(Stream vStream)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        vStream.CopyTo(ms);
        //        return ms.ToArray();
        //    }
        //}
        //#endregion

        #region URLExist
        private bool URLExist(string pPath)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pPath);
                request.Timeout = 5000;
                using (WebResponse response = request.GetResponse())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region URLToByte
        public byte[] DownloadByteImage(string URL)
        {
            byte[] imgByte = null;
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var wc = new WebClient())
                {
                    imgByte = wc.DownloadData(URL);
                }
            }
            finally { }
            return imgByte;
        }
        #endregion

        public byte[] GetByteImage(string pImageName, string pEnquiryId)
        {
            string ActNetImage = "", ActNetImage1 = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = InitialApproachURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + pEnquiryId + "_" + pImageName;
                    ActNetImage1 = ActNetPath[j] + pEnquiryId + "/" + pImageName;
                    if (URLExist(ActNetImage))
                    {
                        imgByte = DownloadByteImage(ActNetImage);
                        break;
                    }
                    else if (URLExist(ActNetImage1))
                    {
                        imgByte = DownloadByteImage(ActNetImage1);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return imgByte;
        }

        public byte[] GetAadhaarByteImage(string pImageName, string pEnquiryId)
        {
            string ActNetImage = "", ActNetImage1 = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = AadhaarURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + pEnquiryId + "_" + pImageName;
                    ActNetImage1 = ActNetPath[j] + pEnquiryId + "/" + pImageName;
                    if (URLExist(ActNetImage))
                    {
                        imgByte = DownloadByteImage(ActNetImage);
                        break;
                    }
                    else if (URLExist(ActNetImage1))
                    {
                        imgByte = DownloadByteImage(ActNetImage1);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return imgByte;
        }

        public byte[] IdfyLoanCard(byte[] pdfBytes, string Language)
        {
            // int[] pagesToExtract = { 4, 5, 6, 7, 8, 9 };
            int[] pagesToExtract;
            if (Language == "G" || Language == "O" || Language == "H")
            {
                pagesToExtract = new int[] { 5, 6, 7, 8, 9, 10 };
            }
            else if (Language == "L" || Language == "T" || Language == "M")
            {
                pagesToExtract = new int[] { 6, 7, 8, 9, 10, 11 };
            }
            else if (Language == "B")
            {
                pagesToExtract = new int[] { 5, 6, 7, 8, 9, 10, 11 };
            }
            else
            {
                pagesToExtract = new int[] { 5, 6, 7, 8, 9, 10 };
            }
            byte[] loanCard = ExtractPagesFromByteArray(pdfBytes, pagesToExtract);
            return loanCard;
        }

        public byte[] ExtractPagesFromByteArray(byte[] pdfBytes, int[] pages)
        {
            var reader = new PdfReader(pdfBytes);
            using (var memoryStream = new MemoryStream())
            {
                var document = new Document();
                var copy = new PdfCopy(document, memoryStream);
                document.Open();
                foreach (var page in pages)
                {
                    copy.AddPage(copy.GetImportedPage(reader, page));
                }
                copy.Close();
                reader.Close();
                return memoryStream.ToArray();
            }
        }

        public string GetDigiDocPath(string Id)
        {
            string pathNetwork = ConfigurationManager.AppSettings["pathKycNetwork"];
            string[] arrPathNetwork = pathNetwork.Split(',');
            int i;
            string vPathDigiDoc = "";
            for (i = 0; i <= arrPathNetwork.Length - 1; i++)
            {
                if (ValidUrlChk(arrPathNetwork[i] + "DigitalDoc/" + Id + ".pdf"))
                {
                    vPathDigiDoc = arrPathNetwork[i] + "DigitalDoc/" + Id + ".pdf";
                    break;
                }
                else if (ValidUrlChk(arrPathNetwork[i] + "jlgdigitaldocs/" + Id + ".pdf"))
                {
                    vPathDigiDoc = arrPathNetwork[i] + "jlgdigitaldocs/" + Id + ".pdf";
                    break;
                }
                else if (ValidUrlChk(arrPathNetwork[i] + Id + ".pdf"))
                {
                    vPathDigiDoc = arrPathNetwork[i] + Id + ".pdf";
                    break;
                }
            }
            return vPathDigiDoc;
        }
    }
}