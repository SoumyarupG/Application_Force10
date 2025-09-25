using System;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Web.UI;
using System.Web;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HOPool : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtAsDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["ID"] = null;
                PopBranch();
                PopPool();
                CheckBrAll();
                PopState();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Pool";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHOPool);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Pool Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void PopPool()
        {
            DataTable dt = null;
            CGblIdGenerator oGbl = null;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oGbl = new CGblIdGenerator();
                dt = oGbl.PopComboMIS("N", "N", "AA", "PoolId", "PoolName", "PoolMst", "0", "AA", "AA", System.DateTime.Now, "0000");
                //ddlPool.DataSource = dt;
                //ddlPool.DataTextField = "PoolName";
                //ddlPool.DataValueField = "PoolId";
                //ddlPool.DataBind();
                chkPoolDtl.DataSource = dt;
                chkPoolDtl.DataTextField = "PoolName";
                chkPoolDtl.DataValueField = "PoolId";
                chkPoolDtl.DataBind();                
            }
            finally
            {
                dt = null;
                oGbl = null;
            }
        }
        
        private void Export()
        {
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = ViewState["BrCode"].ToString();
            //string vWithDef = "";
            DateTime vAsOn = gblFuction.setDate(txtAsDt.Text);
            //string vMode = "A", vRptPath = "", vTitle = "", vType = "D";
            DataTable dt = null;
            CReports oRpt = new CReports();
            ReportDocument rptDoc = new ReportDocument();
            if (chkAll.Checked == true)
                dt = oRpt.rptAllPool("P");
            else
                dt = oRpt.rptPool(vAsOn, ViewState["Bank"].ToString(), ViewState["BrCode"].ToString());
            //dt = oCI.ExportToInsuComp(vLoanId, gblFuction.setDate(txtRptDt.Text), vLnXml);
            if (Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
            {
                foreach (DataRow dr in dt.Rows) // search whole table
                {
                    if (dr["IDProof"].ToString() == "Adhar Card")
                    {
                        dr["M_IdentyProfNo"] = String.Format("{0}{1}", "********", Convert.ToString(dr["M_IdentyProfNo"]).Substring(Convert.ToString(dr["M_IdentyProfNo"]).Length - 5, 5));                           
                    }
                    if (dr["ResiProof"].ToString() == "Adhar Card")
                    {
                        dr["M_AddProfNo"] = String.Format("{0}{1}", "********", Convert.ToString(dr["M_AddProfNo"]).Substring(Convert.ToString(dr["M_AddProfNo"]).Length - 5, 5));                          
                    }
                }
            }
            dgLoanStatus.DataSource = dt;
            dgLoanStatus.DataBind();

            string vFileNm = "attachment;filename=sheet1.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
            htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
            //htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='5'>" + Session[gblValue.CompName].ToString() + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.CompName + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + CGblIdGenerator.GetBranchAddress1("0000") + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + CGblIdGenerator.GetBranchAddress2("0000") + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + vBranch + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>Pool Report</font></u></b></td></tr>");
            htw.WriteLine("<tr><td></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='" + ((dgLoanStatus.Columns.Count - 1) / 2) + "'><b>As On : " + gblFuction.setDate(txtAsDt.Text));
            htw.WriteLine("<tr><td align=center' colspan='" + ((dgLoanStatus.Columns.Count - 1) / 2) + "'><b>Pool : " + chkPoolDtl.SelectedItem);
            htw.WriteLine("<tr><td></td></tr>");
            dgLoanStatus.RenderControl(htw);
            htw.WriteLine("</table>");
            dgLoanStatus.DataSource = null;
            dgLoanStatus.DataBind();
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

        private void PopBranch()
        {
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            DataTable dt = null;
            CUser oUsr = null;
            //string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));

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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            if (rblAlSelPool.SelectedIndex == -1)
            {
                gblFuction.AjxMsgPopup("Please select Pool Information.");
                return;
            }
            else
            {
                Export();
            }
        }

        private void CheckBrAll()
        {
            Int32 vRow;
            string strin = "";
            string vbank = "";
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

        private void CheckPoolAll()
        {

            Int32 vRow;
           
            string vbank = "";

            if (rblAlSelPool.SelectedValue == "rbAllPool")
            {
                chkPoolDtl.Enabled = false;
                for (vRow = 0; vRow < chkPoolDtl.Items.Count; vRow++)
                {
                    chkPoolDtl.Items[vRow].Selected = true;
                    if (vbank == "")
                    {
                        vbank = chkPoolDtl.Items[vRow].Value;
                    }
                    else
                    {
                        vbank = vbank + "," + chkPoolDtl.Items[vRow].Value + "";
                    }
                }
            }
            else if (rblAlSelPool.SelectedValue == "rbSelPool")
            {
                chkPoolDtl.Enabled = true;
                for (vRow = 0; vRow < chkPoolDtl.Items.Count; vRow++)
                {
                    chkPoolDtl.Items[vRow].Selected = false;
                    vbank = vbank + "," + chkPoolDtl.Items[vRow].Value + "";
                }
            }
            ViewState["Bank"] = vbank;
        }        

        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBrAll();
        }

        protected void rblAlSelPool_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckPoolAll();
        }

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

        protected void chkPoolDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vRow;
            string strin = "";
            string vbank = ""; 
            for (vRow = 0; vRow < chkPoolDtl.Items.Count; vRow++)
            {
                if (chkPoolDtl.Items[vRow].Selected == true)
                {
                    if (strin == "")
                    {
                        strin = chkPoolDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkPoolDtl.Items[vRow].Value + "";
                    }
                }
            }
            ViewState["Bank"] = strin;
        }

        private void PopState()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            oCG = new CGblIdGenerator();
            dt = oCG.PopComboMIS("N", "N", "AA", "StateId", "StateName", "StateMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
            ddlState.DataSource = dt;
            ddlState.DataTextField = "StateName";
            ddlState.DataValueField = "StateId";
            ddlState.DataBind();
        }

        protected void btnStateWise_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CUser oUsr = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            string pUser = Session[gblValue.UserName].ToString();
            DateTime vLogDt = gblFuction.setDate(txtAsDt.Text.ToString());
            chkBrDtl.Items.Clear();
            ViewState["Id"] = null;
            try
            {
                oCG = new CGblIdGenerator();
                oUsr = new CUser();
                ViewState["Id"] = null;
                if (vBrCode == "0000")
                {
                    dt = oUsr.GetBranchByState(pUser, Convert.ToInt32(Session[gblValue.RoleId]), ddlState.SelectedValues.Replace("|", ","), ddlBrType.SelectedValue);
                    if (dt.Rows.Count > 0)
                    {
                        chkBrDtl.DataSource = dt;
                        chkBrDtl.DataTextField = "BranchName";
                        chkBrDtl.DataValueField = "BranchCode";
                        chkBrDtl.DataBind();
                        if (rblAlSel.SelectedValue == "rbAll")
                            CheckBrAll();
                    }
                }
                else
                {
                    dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, vBrCode);
                    chkBrDtl.DataSource = dt;
                    chkBrDtl.DataTextField = "Name";
                    chkBrDtl.DataValueField = "BranchCode";
                    chkBrDtl.DataBind();
                    if (rblAlSel.SelectedValue == "rbAll")
                        CheckBrAll();
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
                oCG = null;
            }

        }


    }
}
