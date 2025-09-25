using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.IO;
using FORCECA;
using FORCEBA;
namespace CENTRUM.WebPages.Private.Report
{
    public partial class RptDeathDocStatus : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                //txtFDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtTDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                //PopBranch();
                PopBranch1(Session[gblValue.UserName].ToString());
            }

        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Death Document Status Report";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHORptDeathDoc);
                if (this.UserID == 1) return;
                //if (this.CanReport == "Y")
                //{
                //}
                //else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                //{
                //    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Repayment Schedule", false);
                //}
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }


        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
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
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            string vFileNm = "";
            Int32 vCollDay = 0;
            if (ddlBranch.SelectedValues == "")
            {
                gblFuction.AjxMsgPopup("Please Select atleast one branch...");
                return;
            }
            DataSet ds = null;
            //string vBrCode = ViewState["BrCode"].ToString();
            CReports oRpt = new CReports();
            //sourav
            //DateTime vFrDt = gblFuction.setDate(txtFDt.Text);
            //sourav
            DateTime vToDt = gblFuction.setDate(txtTDt.Text);
            string vBranch = null;
            vBranch = ddlBranch.SelectedValues.ToString();
            //sourav
            //if (ddlCollDay.SelectedIndex > 0) vCollDay = Convert.ToInt32(ddlCollDay.SelectedValue);
            //sourav

            //***************************************
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            ds = oRpt.RptDeathDocument(vToDt, ddlStatus.SelectedValue.ToString(), ddlBranch.SelectedValues.Replace("|", ","));
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();

            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            vFileNm = "attachment;filename=DeathDocumentStatus.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
            htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
            //htw.WriteLine("<tr><td align=center' colspan='6'><b><u><font size='5'>Death Document Status</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.CompName + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.Address1 + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.Address2 + "</font></u></b></td></tr>");
            //htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + vBranch + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>Death Document Status</font></u></b></td></tr>");
            htw.WriteLine("<tr><td></td></tr>");
            htw.WriteLine("<tr><td align='center' colspan='" + dt.Columns.Count + "'><b>As on Date : " + gblFuction.setDate(txtTDt.Text) +"</b></td></tr>");
            htw.WriteLine("<tr><td></td></tr>");
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</table>");

            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(sw.ToString());
            Response.End();

        }


        //private void PopBranch()
        //{
        //    Int32 vRow;
        //    string strin = "";
        //    ViewState["BrCode"] = null;
        //    DataTable dt = null;
        //    CUser oUsr = null;
        //    //string vBrCode = "";
        //    Int32 vBrId = 0;
        //    DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

        //    oUsr = new CUser();
        //    dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));

        //    chkBrDtl.DataSource = dt;
        //    chkBrDtl.DataTextField = "BranchName";
        //    chkBrDtl.DataValueField = "BranchCode";
        //    chkBrDtl.DataBind();

        //    if (rblAlSel.SelectedValue == "rbAll")
        //    {
        //        chkBrDtl.Enabled = false;
        //        for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
        //        {
        //            chkBrDtl.Items[vRow].Selected = true;
        //            if (strin == "")
        //            {
        //                strin = chkBrDtl.Items[vRow].Value;
        //            }
        //            else
        //            {
        //                strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
        //            }
        //        }
        //    }
        //    else if (rblAlSel.SelectedValue == "rbSel")
        //    {
        //        for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
        //        {
        //            chkBrDtl.Items[vRow].Selected = false;
        //        }
        //    }
        //    ViewState["BrCode"] = strin;
        //}


        //private void CheckBrAll()
        //{
        //    Int32 vRow;
        //    string strin = "";
        //    if (rblAlSel.SelectedValue == "rbAll")
        //    {
        //        chkBrDtl.Enabled = false;
        //        for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
        //        {
        //            chkBrDtl.Items[vRow].Selected = true;
        //            if (strin == "")
        //            {
        //                strin = chkBrDtl.Items[vRow].Value;
        //            }
        //            else
        //            {
        //                strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
        //            }
        //        }
        //        ViewState["BrCode"] = strin;
        //    }
        //    else if (rblAlSel.SelectedValue == "rbSel")
        //    {
        //        ViewState["BrCode"] = null;
        //        chkBrDtl.Enabled = true;
        //        for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
        //            chkBrDtl.Items[vRow].Selected = false;

        //    }
        //}

        //protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    CheckBrAll();
        //}


        //protected void chkBrDtl_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 vRow;
        //    string strin = "";
        //    for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
        //    {
        //        if (chkBrDtl.Items[vRow].Selected == true)
        //        {
        //            if (strin == "")
        //            {
        //                strin = chkBrDtl.Items[vRow].Value;
        //            }
        //            else
        //            {
        //                strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
        //            }
        //        }
        //    }
        //    ViewState["BrCode"] = strin;
        //}


        private void PopBranch1(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (Convert.ToString(row["BranchCode"]) != Convert.ToString(Session[gblValue.BrnchCode]))
                        {
                            row.Delete();
                        }
                    }
                    dt.AcceptChanges();
                }
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                }

            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }





    }
}