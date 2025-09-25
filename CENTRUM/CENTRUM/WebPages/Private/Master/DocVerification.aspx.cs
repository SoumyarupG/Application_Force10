using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FORCECA;
using FORCEBA;
using System.IO;


namespace CENTRUM.WebPages.Private.Master
{
    public partial class DocVerification :CENTRUMBase
    {
        protected int cPgNo = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();

                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                LoadGrid(txtFrmDt.Text, txtToDt.Text, vBrCode, 1);

            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Document Verification";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDocVerify);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnClose.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Document Verification", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {

            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                LoadGrid(txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAppMode"></param>
        /// <param name="pFromDt"></param>
        /// <param name="pToDt"></param>
        /// <param name="pBranch"></param>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(string pFromDt, string pToDt, string pBranch, Int32 pPgIndx)
        {
            DataTable dt = null;
            CApplication oLS = null;
            Int32 totalRows = 0;
            string vMode = rdbOpt.SelectedValue;
            try
            {
                string vBrCode = pBranch;
                DateTime vFromDt = gblFuction.setDate(pFromDt);
                DateTime vToDt = gblFuction.setDate(pToDt);
                oLS = new CApplication();
                dt = oLS.GetVerifyList(vFromDt, vToDt, vMode, vBrCode, 0, pPgIndx, ref totalRows);
                ViewState["Verify"] = dt;
                gvVerify.DataSource = dt;
                gvVerify.DataBind();
            }
            finally
            {
                dt = null;
                oLS = null;
            }
        }



        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        //protected void gvVerify_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    string vBCode = "";
        //    DataTable dt = null, dt1 = null;
        //    CBranch oBr = null;
        //    string vMemberId = "";
        //    try
        //    {
        //        vBCode = Convert.ToString(e.CommandArgument);

        //        if (e.CommandName == "cmdShow")
        //        {
        //            GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        //            LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
        //            foreach (GridViewRow gr in gvVerify.Rows)
        //            {
        //                LinkButton lb = (LinkButton)gr.FindControl("btnShow");
        //                lb.ForeColor = System.Drawing.Color.Black;
        //            }
        //            btnShow.ForeColor = System.Drawing.Color.Red;

        //            dt = (DataTable)ViewState["Verify"];
        //            if (dt.Rows.Count > 0)
        //            {
        //                vMemberId = Convert.ToString(dt.Rows[0]["MemberID"]);
        //            }
        //            Response.Redirect("~/WebPages/Private/Master/Member.aspx?id=" + vMemberId);
        //            //string strUrl = "Member.aspx?id=" + vMemberId;
        //            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);

        //        }
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oBr = null;
        //    }
        //}
    }
}