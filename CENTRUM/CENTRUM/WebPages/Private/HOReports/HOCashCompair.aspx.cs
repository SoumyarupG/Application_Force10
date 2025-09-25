using System;
using System.Web;
using System.Data;
using FORCECA;
using FORCEBA;


namespace CENTRUM.WebPages.Private.HOReports
{
	public partial class HOCashCompair : CENTRUMBase
    {
	    protected void Page_Load(object sender, EventArgs e)
	    {
		    InitBasePage();
		    if (!IsPostBack)
		    {
                ViewState["BrCode"] = null;
			    txtDtAsOn.Text = Session[gblValue.LoginDate].ToString();
                txtDtTo.Text = Session[gblValue.LoginDate].ToString();
		        PopBranch();
            }
            
	    }
	    /// <summary>
	    /// 
	    /// </summary>
	    private void InitBasePage()
	    {
		    try
		    {
			    this.Menu = false;
			    this.PageHeading = "Cash Bank Closing";
			    //this.ShowPageHeading = true;
			    this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
			    this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
			    //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHOCashBnkClsng);
			    if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
			    if (this.CanReport == "Y")
			    {
			    }
			    else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
			    {
				    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Cash/Bank Closing Report", false);
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
	    /// <param name="sender"></param>
	    /// <param name="e"></param>
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
        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBrAll();
        }
        /// <summary>
        /// 
        /// </summary>
        private void CheckBrAll()
        {
            Int32 vRow;
            string strin = "";
            if (rblAlSel.SelectedValue == "rbAll")
            {
                chkBrDtl.Enabled = false;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
                ViewState["BrCode"] = strin;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                ViewState["BrCode"] = null;
                chkBrDtl.Enabled = true;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    chkBrDtl.Items[vRow].Selected = false;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkBrDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
            {
                if (chkBrDtl.Items[vRow].Selected == true)
                {
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
            }
            
            ViewState["BrCode"] = strin;
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopBranch()
        {
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            //string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oCG = new CGblIdGenerator();
            dt = oCG.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", vBrId, "BranchCode", "AA", vLogDt, "0000");

            chkBrDtl.DataSource = dt;
            chkBrDtl.DataTextField = "BranchName";
            chkBrDtl.DataValueField = "BranchCode";
            chkBrDtl.DataBind();

            if (rblAlSel.SelectedValue == "rbAll")
            {
                chkBrDtl.Enabled = false;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = false;
                }
            }
            ViewState["BrCode"] = strin;
        }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="pFormat"></param>
	    private void GetData(string pFormat)
	    {
		    string vFileNm="";
		    string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = ViewState["BrCode"].ToString();
		    DateTime vFinYrFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
		    Int32 vFinYrNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());

            if (txtDtAsOn.Text.Trim().Length > 0 && txtDtTo.Text.Trim().Length > 0)
            {
                TimeSpan ts = gblFuction.setDate(txtDtTo.Text.ToString()) - gblFuction.setDate(txtDtAsOn.Text.ToString());
                TxtInDays.Value = ts.TotalDays.ToString();
            }
            else
            { 
                gblFuction.AjxMsgPopup("Please Select From and To Date...");
                return;
            }

            if (vFinYrFrom > gblFuction.setDate(txtDtAsOn.Text) || gblFuction.setDate(txtDtAsOn.Text) > vFinTo)
            {
                gblFuction.MsgPopup("From Date should be within this financial year.");
                return;
            }
            if (vFinYrFrom > gblFuction.setDate(txtDtTo.Text) || gblFuction.setDate(txtDtTo.Text) > vFinTo)
            {
                gblFuction.MsgPopup("To date should be within this financial year.");
                return;
            }

		    DataTable dt = null;
		    CReports oRpt = new CReports();

            dt = oRpt.rptCashComparision(ddlCashBank.SelectedValue, vBrCode, vFinYrFrom, gblFuction.setDate(txtDtTo.Text), "Branch", Convert.ToInt32(TxtInDays.Value), vFinYrNo, "D");
		    System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
		    DataGrid1.DataSource = dt;
		    DataGrid1.DataBind();

		    tdx.Controls.Add(DataGrid1);
		    tdx.Visible = false;
            if (ddlCashBank.SelectedValue == "C")
            {
                vFileNm = "attachment;filename=" + gblFuction.setDate(txtDtTo.Text).ToString("yyyyMMdd") + "_Cash_Comparison_Report.xls";
            }
            else
            {
                vFileNm = "attachment;filename=" + gblFuction.setDate(txtDtTo.Text).ToString("yyyyMMdd") + "_Bank_Comparison_Report.xls";
            }
		    System.IO.StringWriter sw = new System.IO.StringWriter();
		    System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
		    htw.WriteLine("<table border='0' cellpadding='3' widht='100%'>");


		    htw.WriteLine("<tr><td align=center' colspan='" + Convert.ToString(Convert.ToInt32(TxtInDays.Value)+4)+"'><b><u><font size='4'>"+gblValue.CompName+"</font></u></b></td></tr>");
            //htw.WriteLine("<tr><td align=center' colspan='" + Convert.ToString(Convert.ToInt32(TxtInDays.Text) + 3) + "'><b><u><font size='2'>"+CGblIdGenerator.GetBranchAddress1(vBrCode)+"</font></u></b></td></tr>");
            //htw.WriteLine("<tr><td align=center' colspan='" + Convert.ToString(Convert.ToInt32(TxtInDays.Text) + 3) + "'><b><u><font size='2'>"+CGblIdGenerator.GetBranchAddress2(vBrCode)+"</font></u></b></td></tr>");
            if (ddlCashBank.SelectedValue == "C")
            {
                htw.WriteLine("<tr><td align=center' colspan='" + Convert.ToString(Convert.ToInt32(TxtInDays.Value) + 4) + "'><b><u><font size='3'>Cash Comparison Report As On " + txtDtAsOn.Text.ToString() + "</font></u></b></td></tr>");

            } 
		    else
            {
                htw.WriteLine("<tr><td align=center' colspan='" + Convert.ToString(Convert.ToInt32(TxtInDays.Value) + 4) + "'><b><u><font size='3'>Bank Comparison Report As On " + txtDtAsOn.Text.ToString() + "</font></u></b></td></tr>");
            }
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</td></tr>");
		    htw.WriteLine("<tr><td colspan='4'><b><u><font size='2'></font></u></b></td></tr>");
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
	    /// <returns></returns>
	    private bool ValidateDate()
	    {
		    bool vRst = true;
		    return vRst;
	    }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="sender"></param>
	    /// <param name="e"></param>
	    protected void btnPdf_Click(object sender, EventArgs e)
	    {
		    if (ValidateDate() == false)
		    {
			    gblFuction.MsgPopup("Please Set The Valid Date Range.");
			    return;
		    }
		    else
		    {
			    GetData("PDF");
		    }
	    }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="sender"></param>
	    /// <param name="e"></param>
	    protected void btnExcl_Click(object sender, EventArgs e)
	    {
		    if (ValidateDate() == false)
		    {
			    gblFuction.MsgPopup("Please Set The Valid Date Range.");
			    return;
		    }
		    else
		    {
			    GetData("Excel");
		    }
	    }


        protected void txtDtAsOn_TextChanged(object sender, EventArgs e)
        {
            if (txtDtAsOn.Text.Trim().Length > 0 && txtDtTo.Text.Trim().Length > 0)
            {
                //TimeSpan ts = gblFuction.setDate(txtDtTo.Text.ToString()) - gblFuction.setDate(txtDtAsOn.Text.ToString());
                //TxtInDays.Value = ts.TotalDays.ToString(); 
            }
        }

        protected void txtDtTo_TextChanged(object sender, EventArgs e)
        {
            if (txtDtAsOn.Text.Trim().Length > 0 && txtDtTo.Text.Trim().Length > 0)
            {
                //TimeSpan ts =  gblFuction.setDate(txtDtTo.Text.ToString()) - gblFuction.setDate(txtDtAsOn.Text.ToString());
                //TxtInDays.Value = ts.TotalDays.ToString(); 
            }
        }

    }
}
