using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using FORCEBA;
using FORCECA;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;

namespace CENTRUM.WebPages.Private.BCOperation
{
    public partial class BC_URNID_Track : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
                    StatusButton("View");
                ViewState["URNAPP"] = null;
                ViewState["URNPrint"] = null;
                PopBranch(Session[gblValue.UserName].ToString());
                popRO();
                LoadLabel(0);
            }
        }
         /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);


                this.Menu = false;
                this.PageHeading = "BC Account Tracking";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBCAccountTracking);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "BC Account Tracking", false);
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
        /// <param name="pMode"></param>
        private void StatusButton(String pMode)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadLabel(Int32 pPgIndx)
        {
            DataTable dt = null;
            CBCCgt oBC = null;
            DateTime vDate = Convert.ToDateTime(gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
            try
            {
                oBC = new CBCCgt();
                dt = oBC.BC_Application_TrackSumm(vDate);

                if (dt.Rows.Count >0)
                {
                    lblCGTDone.Text = Convert.ToString(dt.Rows[0]["CGTDone"]).Trim();
                    lblCGTChkDone.Text = Convert.ToString(dt.Rows[0]["CGTCheckDone"]).Trim();
                    lblCBDone.Text = Convert.ToString(dt.Rows[0]["CBCheckDone"]).Trim();
                    lblGRTDone.Text = Convert.ToString(dt.Rows[0]["GRTDONE"]).Trim();
                    lblCustUp.Text = Convert.ToString(dt.Rows[0]["SFTPCUSTUPLOADDone"]).Trim();
                    lblCUstSuc.Text = Convert.ToString(dt.Rows[0]["CustSucAcceptDone"]).Trim();
                    lblJLGUp.Text = Convert.ToString(dt.Rows[0]["JLGUpLoadDone"]).Trim();
                    lblJLGSuc.Text = Convert.ToString(dt.Rows[0]["JLGSucAcceptDone"]).Trim();
                    lblBIJLIDisb.Text = Convert.ToString(dt.Rows[0]["DisbursedDone"]).Trim();
                    lblBIJLIDisbAmt.Text = Convert.ToString(dt.Rows[0]["BIJLIDisburseAmountDone"]).Trim();
                    lblJConfUp.Text = Convert.ToString(dt.Rows[0]["JConfUPLOADDone"]).Trim();
                    lblJCONFSuc.Text = Convert.ToString(dt.Rows[0]["JCONFSucAcceptDone"]).Trim();
                    lblIDBIDisbAmt.Text = Convert.ToString(dt.Rows[0]["DisburseAmountDone"]).Trim();
                    lblPrincOuts.Text = Convert.ToString(dt.Rows[0]["PrincipalOutstanding"]).Trim();
                }
                else
                {
                    lblCGTDone.Text = "0";
                    lblCGTChkDone.Text = "0";
                    lblCBDone.Text = "0";
                    lblGRTDone.Text = "0";
                    lblCustUp.Text = "0";
                    lblCUstSuc.Text = "0";
                    lblJLGUp.Text = "0";
                    lblJLGSuc.Text = "0";
                    lblBIJLIDisb.Text = "0";
                    lblJConfUp.Text = "0";
                    lblJCONFSuc.Text = "0";
                    lblIDBIDisbAmt.Text = "0";
                    lblPrincOuts.Text = "0";
                }

            }
            finally
            {
                dt = null;
                oBC = null;
            }
        }




        /// <summary>
        /// 
        /// </summary>
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]),"IDBI");
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                    ListItem liSel = new ListItem("All", "A");
                    ddlBranch.Items.Insert(0, liSel);
                    if (Session[gblValue.BrnchCode].ToString() != "0000")
                    {
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                        ddlBranch.Enabled = false;
                    }
                }
                else
                {

                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode;
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
            }
            else
            {
                vBrCode = ddlBranch.SelectedValue.ToString();
            }

            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlCo.DataSource = dt;
                ddlCo.DataTextField = "EoName";
                ddlCo.DataValueField = "EoId";
                ddlCo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "A");
                ddlCo.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowDetails(object sender,EventArgs e)
        {
            string vClickMode = "";
            DataTable dt = null, dt1 = null ;
            CBCCgt oBC = null;
            DateTime vDate = Convert.ToDateTime(gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
            LinkButton button = (LinkButton)sender;
            string buttonId = button.ID;
            tabPurps.ActiveTabIndex=1;
            switch (buttonId)
            {
                case "lblCGTDone": vClickMode = "CGTDate"; lblSelected.Text = "CGT Done(Field) Data"; break;
                case "lblCGTChkDone": vClickMode = "CgtChkYN"; lblSelected.Text = "CGT Checked Data"; break;
                case "lblCBDone": vClickMode = "CBPassYN"; lblSelected.Text = "CB Checked Data"; break;
                case "lblGRTDone": vClickMode = "GRTDONEYN"; lblSelected.Text = "GRt Done Data"; break;
                case "lblCustUp": vClickMode = "SFTPCustYN"; lblSelected.Text = "Customer File Uploaded Data"; break;
                case "lblCUstSuc": vClickMode = "CustSucAccept"; lblSelected.Text = "Customer File Successfully Accepted Data"; break;
                case "lblJLGUp": vClickMode = "JLGUpLoadedYN"; lblSelected.Text = "JLG Tag Uploaded Data"; break;
                case "lblJLGSuc": vClickMode = "JLGSucAccept"; lblSelected.Text = "JLG Tag Successfully Accepted Data"; break;
                case "lblBIJLIDisb": vClickMode = "Disbursed"; lblSelected.Text = "Amount Disbursed (in Branch) Data"; break;
                case "lblBIJLIDisbAmt": vClickMode = "Disbursed"; lblSelected.Text = "Amount Disbursed (in BIJLI) Data"; break;
                case "lblJConfUp": vClickMode = "JConfUPLOADED"; lblSelected.Text = "JCONF File Uploaded Data"; break;
                case "lblJCONFSuc": vClickMode = "JCONFSucAccept"; lblSelected.Text = "JCONF FIle Successfully Accepted Data"; break;
                case "lblIDBIDisbAmt": vClickMode = "DisbuseAmount"; lblSelected.Text = "Amount Disbursed ( in IDBI ) Data"; break;
            }
            try
            {
                oBC = new CBCCgt();
                dt = oBC.BC_Application_TrackDtl(vClickMode, vDate);
                if (dt.Rows.Count > 0)
                {
                    
                    ViewState["URNAPP"] = dt;
                    dt1 = dt.Copy();
                    foreach (DataRow dr in dt1.Select("Status='N'"))
                    {
                        dr.Delete();
                    }
                    dt1.AcceptChanges();
                    DataGrid1.DataSource = dt1;
                    DataGrid1.DataBind();
                    ViewState["URNPrint"] = dt1;
                   
                }
                else
                {

                    DataGrid1.DataSource = null;
                    DataGrid1.DataBind();
                    ViewState["URNAPP"] = null;
                    ViewState["URNPrint"] = null;
                }
            }
            finally
            {
                dt = null;
                oBC = null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            popRO();
            LoadList();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadList();
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadList()
        {
            DataTable dt = null, dt1 = null;
            Int32 vCnt = 1;
            dt = (DataTable)ViewState["URNAPP"];
            dt1 = dt.Clone();
            if (dt.Rows.Count > 0)
            {
                if (ddlBranch.SelectedItem.Text.Trim() != "All")
                {
                    if (ddlCo.SelectedItem.Text.Trim() != "All")
                    {
                        foreach (DataRow dr in dt.Select("BranchName ='" + ddlBranch.SelectedItem.Text.Trim() + "' And FOName = '" + ddlCo.SelectedItem.Text.Trim() + "' And Status = '" + ddlStatus.SelectedValue + "'"))
                        {
                            dt1.ImportRow(dr);
                            dt1.Rows[dt1.Rows.Count-1]["Slno"]=vCnt++;
                        }
                    }
                    if (ddlCo.SelectedItem.Text.Trim() == "All")
                    {
                        foreach (DataRow dr in dt.Select("BranchName ='" + ddlBranch.SelectedItem.Text.Trim() + "' And Status = '" + ddlStatus.SelectedValue + "'"))
                        {
                            dt1.ImportRow(dr);
                            dt1.Rows[dt1.Rows.Count - 1]["Slno"] = vCnt++;
                        }
                    }
                }
                else
                {
                    if (ddlCo.SelectedItem.Text.Trim() != "All")
                    {
                        foreach (DataRow dr in dt.Select("FOName = '" + ddlCo.SelectedItem.Text.Trim() + "' And Status = '" + ddlStatus.SelectedValue + "'"))
                        {
                            dt1.ImportRow(dr);
                            dt1.Rows[dt1.Rows.Count - 1]["Slno"] = vCnt++;
                        }
                    }
                    if (ddlCo.SelectedItem.Text.Trim() == "All")
                    {
                        foreach (DataRow dr in dt.Select("Status = '" + ddlStatus.SelectedValue + "'"))
                        {
                            dt1.ImportRow(dr);
                            dt1.Rows[dt1.Rows.Count - 1]["Slno"] = vCnt++;
                        }
                    }
                }

                dt1.AcceptChanges();
                DataGrid1.DataSource = dt1;
                DataGrid1.DataBind();
                ViewState["URNPrint"] = dt1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
            ViewState["URNAPP"] = null;
            ViewState["URNPrint"] = null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShowURN_Click(object sender, EventArgs e)
        {
            string vFileNm = "";
            DataTable dt = null;
            CBCCgt oBC = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();

            try
            {
                dt = new DataTable();
               
                System.Web.UI.WebControls.DataGrid DataGrid3 = new System.Web.UI.WebControls.DataGrid();
                oBC = new CBCCgt();
                dt = oBC.rptURN_STATUS(txtURNID.Text, vBrCode);
                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();
                tdx.Controls.Add(DataGrid1);
                tdx.Visible = false;
                vFileNm = "attachment;filename=URNID_Status_"+txtURNID.Text.Trim()+".xls";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                htw.WriteLine("<table border='1' cellpadding='5' widht='100%'>");
                htw.WriteLine("<tr><td align=center' colspan='23'><b><u><font size='5'>Client Details</font></u></b></td></tr>");
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
            finally
            {
                dt = null;
                oBC= null;
            }
        }
        
         /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string vFileNm = "";
            DataTable dt = null;
            dt = (DataTable)ViewState["URNPrint"];
            System.Web.UI.WebControls.DataGrid DataGrid2 = new System.Web.UI.WebControls.DataGrid();
            DataGrid2.DataSource = dt;
            DataGrid2.DataBind();

            tdx.Controls.Add(DataGrid2);
            tdx.Visible = false;
            vFileNm = "attachment;filename=BC_Application_Track_List.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='1' cellpadding='5' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan='10'><b><u><font size='5'>BC Application Track</font></u></b></td></tr>");
            DataGrid2.RenderControl(htw);
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

        

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            tabPurps.ActiveTabIndex = 0;
            
        }

    }
}

  