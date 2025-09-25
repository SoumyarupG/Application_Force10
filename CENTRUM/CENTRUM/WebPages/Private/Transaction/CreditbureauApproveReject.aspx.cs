using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class CreditbureauApproveReject : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                txtAppDt.Text = Session[gblValue.LoginDate].ToString();
               // txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                //txtToDt.Text = Session[gblValue.LoginDate].ToString();
               // LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                PopBranch();
            }

        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Creditbureau Approval";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBrNEFT);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Application", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }


        protected void btnShow_Click(object sender, EventArgs e)
        {           
                try
                {
                    string vBrCode = ddlBranch.SelectedValues.Replace("|", ",");
                    if (rdbOpt.SelectedValue == "N")
                        LoadGrid("N", txtAppDt.Text,vBrCode);
                    else if (rdbOpt.SelectedValue == "A")
                        LoadGrid("A", txtAppDt.Text, vBrCode);
                    else if (rdbOpt.SelectedValue == "R")
                        LoadGrid("R", txtAppDt.Text, vBrCode);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            
        }


     

        protected void btnDone_Click(object sender, EventArgs e)
        {
            CcreditbureauApproveReject obA = null;
            DataTable dt = new DataTable();
            Int32 vErr = 0;
            string vXmlData = "";          
            DateTime vSanDt = gblFuction.setDate(txtAppDt.Text);
            string vBrCode = ddlBranch.SelectedValues.Replace("|", ",");
                try
                {
                    dt = (DataTable)ViewState["Sanc"];
                    if (dt == null) return;
                    int cnt = dt.Rows.Count;

                    for (int r = 0; r < cnt; r++)
                    {
                        CheckBox chkApprove = (CheckBox)gvCredit.Rows[r].FindControl("chkApprove");
                        CheckBox chkReject = (CheckBox)gvCredit.Rows[r].FindControl("chkReject");

                        TextBox txtAmountApplied = (TextBox)gvCredit.Rows[r].FindControl("txtAmountApplied");
                        TextBox txtAppReason = (TextBox)gvCredit.Rows[r].FindControl("txtAppReason");
                        TextBox txtRejectReason = (TextBox)gvCredit.Rows[r].FindControl("txtRejectReason");

                        if (chkApprove.Checked)
                        {
                            dt.Rows[r]["IsApproved"] = "Y";
                            dt.Rows[r]["AmountApplied"] = txtAmountApplied.Text;
                            dt.Rows[r]["ApprovalReason"] = txtAppReason.Text;
                            dt.Rows[r]["IsReject"] = "N";   
                        }
                        else if (chkReject.Checked)
                        {
                            dt.Rows[r]["IsReject"] = "Y";
                            dt.Rows[r]["RejectReason"] = txtRejectReason.Text;
                            dt.Rows[r]["IsApproved"] = "N";
                        }                                              
                        else
                        {
                            dt.Rows[r]["IsReject"] = null;
                        }
                    }

                    dt.AcceptChanges();
                    obA = new CcreditbureauApproveReject();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    //-----------XML Save----------
                    vErr = obA.SaveCreditbureauApprove(vXmlData, this.UserID);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
                        LoadGrid("N",txtAppDt.Text,vBrCode);
                        rdbOpt.SelectedValue = "N";
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                    }
                }
                finally
                {
                    obA = null;
                    dt = null;
                }
            
        }

        protected void txtAppDt_TextChanged(object sender, EventArgs e)
        {
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DateTime vAppDt = gblFuction.setDate(txtAppDt.Text);
            if (vAppDt > vLoginDt)
            {
                gblFuction.MsgPopup("Approved date should not grater than login date..");
                txtAppDt.Text = Session[gblValue.LoginDate].ToString();  //Convert.ToString(vLoginDt);
                return;
            }
        }
       

        private void LoadGrid(string pAppMode, string pAppDt, string pBranch)
        {
            DataTable dt = null;
            CcreditbureauApproveReject oCB = null;
            Int32 vRows = 0;          
            try
            {
                string vBrCode = pBranch;
                DateTime vAppDt = gblFuction.setDate(pAppDt);
                oCB = new CcreditbureauApproveReject();
                //dt = oCB.GetCreditbureauAppRej(vAppDt, pAppMode, pBranch);
                ViewState["Sanc"] = dt;
                gvCredit.DataSource = dt;
                gvCredit.DataBind();                
            }
            finally
            {
                dt = null;
                oCB = null;
            }
        }
      
       
        //private int CalTotPgs(double pRows)
        //{
        //    int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
        //    return totPg;
        //}


        //protected void ChangePage(object sender, CommandEventArgs e)
        //{
        //    string vBrCode = Session[gblValue.BrnchCode].ToString();
        //    switch (e.CommandName)
        //    {
        //        case "Previous":
        //            cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
        //            break;
        //        case "Next":
        //            cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
        //            break;
        //    }

        //    //LoadGrid(rdbOpt.SelectedValue, txtFrmDt.Text, txtToDt.Text, vBrCode, cPgNo);
        //    //tabLoanAppl.ActiveTabIndex = 0;
        //}
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }


        protected void gvCredit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkApprove = (CheckBox)e.Row.FindControl("chkApprove");
                    CheckBox chkReject = (CheckBox)e.Row.FindControl("chkReject");
                    TextBox txtAppReason = (TextBox)e.Row.FindControl("txtAppReason");
                    TextBox txtRejectReason = (TextBox)e.Row.FindControl("txtRejectReason");
                    if (e.Row.Cells[10].Text == "Y")
                    {
                        chkApprove.Checked = true;
                        chkReject.Enabled = false;
                        txtAppReason.Enabled = true;
                    }
                    else if (e.Row.Cells[10].Text == "N")
                    {
                        chkApprove.Checked = false;
                        chkReject.Enabled = true;
                    }

                    if (e.Row.Cells[13].Text == "Y")
                    {
                        chkReject.Checked = false;
                        chkApprove.Enabled = true;
                        txtRejectReason.Enabled = true;
                    }
                    else if (e.Row.Cells[13].Text == "N")
                    {
                        chkReject.Checked = false;
                        chkApprove.Enabled = true;                      
                    }

                }
            }
            finally
            {

            }
        }

        private void PopBranch()
        {
            CBranch ocb = new CBranch();
            DataTable dt = new DataTable();
            dt=ocb.GetBranchList();
            ddlBranch.DataSource = dt;
            ddlBranch.DataTextField = "BranchName";
            ddlBranch.DataValueField = "BranchCode";
            ddlBranch.DataBind();        
        }

    }
}