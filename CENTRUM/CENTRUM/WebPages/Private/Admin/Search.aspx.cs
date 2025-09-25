using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Admin
{
    public partial class Search : CENTRUMBase
    {
        protected int cPgNo = 1;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                PopList();
                CheckAll();
                popDetail();
            }
        }


        private void LoadGrid(string pSearch, string pBranch, string pMenu, Int32 pPgIndx)
        {
            DataTable dt = null;
            CSSOptionRight oSS = null;
            Int32 totalRows = 0;
            try
            {
                oSS = new CSSOptionRight();
                dt = oSS.GetSearchRecord(pSearch, pBranch, pMenu, pPgIndx);
                ViewState["Search"] = dt;
                gvSanc.DataSource = dt;
                gvSanc.DataBind();
                totalRows = dt.Rows.Count;
                lblTotalPages.Text = CalTotPgs(totalRows).ToString();
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
                oSS = null;
            }
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
            //LoadGrid(cPgNo);
            //LoadGrid(pSearch, pBranch, pMenu, cPgNo);
            string pBranch = Session[gblValue.BrnchCode].ToString();
            string pMenu = ViewState["Dtl"].ToString();
            LoadGrid(txtSearch.Text, pBranch, pMenu, cPgNo);
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            string pBranch = Session[gblValue.BrnchCode].ToString();
            string pMenu = ViewState["Dtl"].ToString();
            if (txtSearch.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Nothing to Search");
                return;

            }
            LoadGrid(txtSearch.Text, pBranch, pMenu, 1);
        }

        private void PopList()
        {
            DataTable dt = null;
            CSSOptionRight oSR = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            int vRoleID = Convert.ToInt32(Session[gblValue.RoleId]);
            oSR = new CSSOptionRight();
            dt = oSR.GetMenuByRollID(vRoleID, vBrCode);
            chkDtl.DataSource = dt;
            chkDtl.DataTextField = "MnCaption";
            chkDtl.DataValueField = "MenuName";
            chkDtl.DataBind();
        }


        private void CheckAll()
        {
            Int32 vRow;
            if (rblAlSel.SelectedValue == "rbAll")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = true;
                chkDtl.Enabled = false;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = false;
                chkDtl.Enabled = true;
            }
        }

        private void popDetail()
        {
            ViewState["Dtl"] = null;
            string str = "";
            for (int vRow = 0; vRow < chkDtl.Items.Count; vRow++)
            {
                if (chkDtl.Items[vRow].Selected == true)
                {
                    if (str == "")
                        str = chkDtl.Items[vRow].Value;
                    else if (str != "")
                        str = str + "," + chkDtl.Items[vRow].Value;
                }
            }
            if (str == "")
                ViewState["Dtl"] = 0;
            else
                ViewState["Dtl"] = str;
        }

        protected void chkDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            popDetail();
        }

        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
            popDetail();
        }

        //protected void gvSanc_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    string vSearchPg = "", vSearchId = "";
        //    try
        //    {
        //        if (e.CommandName == "cmdShow")
        //        {
        //            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        //            vSearchId = Convert.ToString(gvSanc.DataKeys[gvr.RowIndex].Value);
        //            vSearchPg = Convert.ToString(e.CommandArgument);
        //            //OpenRequest(vSearchPg, vSearchId);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void OpenRequest(string pPageNm, string pSearchId)
        {
            switch (pPageNm)
            {
                case "PVoucher":
                    Response.Redirect("~/WebPages/Private/Transaction/VoucherRP.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                    break;
                case "JVoucher":
                    Response.Redirect("~/WebPages/Private/Transaction/VoucherJ.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                    break;
                case "CVoucher":
                    Response.Redirect("~/WebPages/Private/Transaction/VoucherC.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                    break;
                case "Tehsil":
                    Response.Redirect("~/WebPages/Private/Master/TehsilMaster.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                    break;
                case "Block":
                    Response.Redirect("~/WebPages/Private/Master/BlockMaster.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                    break;
                case "Village":
                    Response.Redirect("~/WebPages/Private/Master/VillageMaster.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                    break;
                case "Mahalla":
                    Response.Redirect("~/WebPages/Private/Master/MohallaMaster.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                    break;
                case "Group":
                    Response.Redirect("~/WebPages/Private/Master/GrMst.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                    break;
                case "Member":
                    Response.Redirect("~/WebPages/Private/Master/MemAllocation.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                    break;
                case "LoanApplication":
                    Response.Redirect("~/WebPages/Private/Transaction/LoanAppl.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                    break;
                case "LoanDisbursementGroup":
                    Response.Redirect("~/WebPages/Private/Transaction/LoanDisbGrp.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                    break;
                case "LoanDisbursementInd":
                    Response.Redirect("~/WebPages/Private/Transaction/LoanDisbIndiv.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                    break;
                case "LoanProduct":
                    Response.Redirect("~/WebPages/Private/Master/LoanProductMaster.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                    break;
                //case "CM":
                //    Response.Redirect("~/Private/Webpages/Master/CMDtl.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                //    break;
                //case "FundSource":
                //    Response.Redirect("~/Private/Webpages/Master/FundSource.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                //    break;
                //case "GroupMst":
                //    Response.Redirect("~/Private/Webpages/Master/GroupDtl.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                //    break;
                //case "LoanPurpose":
                //    Response.Redirect("~/Private/Webpages/Master/LoanPurpose.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                //    break;
                //case "LoanScheme":
                //    Response.Redirect("~/Private/Webpages/Master/LoanScheme.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                //    break;
                //case "LoanProduct":
                //    Response.Redirect("~/Private/Webpages/Master/LoanProduct.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                //    break;
                //case "LoanApplication":
                //    Response.Redirect("~/Private/Webpages/Transaction/LoanApplication.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                //    break;
                //case "LoanCycle":
                //    Response.Redirect("~/Private/Webpages/Master/LoanCycle.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                //    break;
                //case "LoanDisbursement":
                //    Response.Redirect("~/Private/Webpages/Transaction/LoanDisbursement.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                //    break;
                //case "CenterMst":
                //    Response.Redirect("~/Private/Webpages/Master/CenterDtl.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                //    break;
                //case "PageOne":
                //    Response.Redirect("~/Private/Webpages/Master/NewMem1.aspx?sr=" + Server.HtmlEncode(pSearchId), false);
                //    break;
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
        }
    }
}
