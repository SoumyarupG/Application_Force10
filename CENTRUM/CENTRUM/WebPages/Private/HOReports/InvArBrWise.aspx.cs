using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class InvArBrWise : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
                    rbList.SelectedValue = "rbB";
                PopList();
                PopBranch();
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Area/Branch Wise Stock Summary";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuArBrWiseStockRpt);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
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

        /// <summary>
        /// 
        /// </summary>
        private void PopList()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            CBranch oBr = null;
            CEO oRo = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            Int32 vBrId = Convert.ToInt32(vBrCode);
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            if (rbList.SelectedValue == "rbA")
            {
                ddlBr.Items.Clear();
                ddlBr.Enabled = false;
                ddlStaff.Items.Clear();
                ddlStaff.Enabled = false;
                ddlAr.Enabled = true;
                oBr = new CBranch();
                
                dt = oBr.GetBranchDetails(vBrCode);
                ddlAr.DataSource = dt;
                ddlAr.DataTextField = "AreaName";
                ddlAr.DataValueField = "AreaId";
                ddlAr.DataBind();
                ListItem oL1 = new ListItem("<-- Select -->", "-1");
                ddlAr.Items.Insert(0, oL1);
            }
            if (rbList.SelectedValue == "rbB")
            {
                CUser oUsr = null;
                oUsr = new CUser();

                ddlAr.Items.Clear();
                ddlAr.Enabled = false;
                ddlStaff.Items.Clear();
                ddlStaff.Enabled = false;
                ddlBr.Enabled = true;

                oCG = new CGblIdGenerator();
                if (vBrCode != "0000")
                {
                    dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]), "R");
                    ddlBr.DataSource = dt;
                    ddlBr.DataTextField = "BranchName";
                    ddlBr.DataValueField = "BranchCode";
                    ddlBr.DataBind();
                }

                else
                {
                    dt = oCG.PopComboMIS("N", "Y", "BranchCode", "BranchCode", "BranchName", "BranchMst", "", "", "AA", vLogDt, "0000");
                    ddlBr.DataSource = dt;
                    ddlBr.DataTextField = "Name";
                    ddlBr.DataValueField = "BranchCode";
                    ddlBr.DataBind();
                }
                ListItem oL1 = new ListItem("<-- Select -->", "-1");
                ddlBr.Items.Insert(0, oL1);
            }
            //if (rbList.SelectedValue == "rbS")
            //{
            //    ddlAr.Items.Clear();
            //    ddlAr.Enabled = false;
            //    ddlBr.Items.Clear();
            //    ddlBr.Enabled = false;
            //    ddlStaff.Enabled = true;

            //    oRo = new CEO();
            //    if (vBrCode != "0000")
            //        dt = oRo.PopBrWiseStaff(vBrCode, vLogDt);
            //    else
            //        dt = oRo.PopBrWiseStaff("", vLogDt);
            //    ddlStaff.DataSource = dt;
            //    ddlStaff.DataTextField = "EOName";
            //    ddlStaff.DataValueField = "EoID";
            //    ddlStaff.DataBind();
            //    ListItem oL1 = new ListItem("<-- Select -->", "-1");
            //    ddlStaff.Items.Insert(0, oL1);
            //}
            if (rbList.SelectedValue == "rbH")
            {
                ddlAr.Items.Clear();
                ddlAr.Enabled = false;
                ddlBr.Items.Clear();
                ddlBr.Enabled = false;
                ddlStaff.Items.Clear();
                ddlStaff.Enabled = false;
            }
        }

        protected void rbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000" && Convert.ToString(Session[gblValue.Designation]).ToUpper() == "AM")
            {
                if (rbList.SelectedValue == "rbH")
                {
                    gblFuction.AjxMsgPopup("Only HO Login can select this option");
                    rbList.SelectedValue = "rbB";
                    return;
                }
            }

            if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000" && Convert.ToString(Session[gblValue.Designation]).ToUpper() == "BM")
            {
                if (rbList.SelectedValue == "rbA")
                {
                    gblFuction.AjxMsgPopup("Only HO Login can select this option");
                    rbList.SelectedValue = "rbB";
                    return;
                }
                if (rbList.SelectedValue == "rbH")
                {
                    gblFuction.AjxMsgPopup("Only HO Login can select this option");
                    rbList.SelectedValue = "rbB";
                    return;
                }
            }
            PopList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text), vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = "", vItemList = ViewState["BrCode"].ToString(), vRptPath = "", vTitle = "", vMode = "", vEoID="";
            string vBranchCode = (string)Session[gblValue.BrnchCode];
            //ReportDocument rptDoc = new ReportDocument();
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();

            Int32 vArID = 0;

            if (rbList.SelectedValue == "rbA")
            {
                vTitle = "Item Stock Report for Area (" + ddlAr.SelectedItem.Text + ")";
                vMode = "AR";
                if (Convert.ToInt32(ddlAr.SelectedValue) > 0)
                {
                    vArID = Convert.ToInt32(ddlAr.SelectedValue);
                }
                else
                {
                    gblFuction.MsgPopup("Please Select at Area");
                    return;
                }
            }
            if (rbList.SelectedValue == "rbB")
            {
                vTitle = "Item Stock Report for Branch (" + ddlBr.SelectedItem.Text + ")";
                vMode = "BR";
                if (Convert.ToInt32(ddlBr.SelectedValue) > 0)
                {
                    vBrCode = ddlBr.SelectedValue;
                }
                else
                {
                    gblFuction.MsgPopup("Please Select at Branch");
                    return;
                }
            }
            //if (rbList.SelectedValue == "rbS")
            //{
            //    vTitle = "Item Stock Report for (Staff WIse)";
            //    vMode = "ST";
            //    if (ddlStaff.SelectedIndex > 0)
            //    {
            //        vEoID = ddlStaff.SelectedValue;
            //    }
            //    else
            //    {
            //        gblFuction.MsgPopup("Please Select at Staff Name");
            //        return;
            //    }
            //}
            if (rbList.SelectedValue == "rbH")
            {
                vTitle = "Item Stock Report for (Head Office)";
                vMode = "HO";
            }

            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptInvStock.rpt";
            dt = oRpt.rptINVBranchWise(vFromDt, vToDt, vMode, vBrCode, vArID, vEoID, vItemList);
            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBranchCode));
                rptDoc.SetParameterValue("pTitle", vTitle);
                rptDoc.SetParameterValue("dtFrom", txtDtFrm.Text);
                rptDoc.SetParameterValue("dtTo", txtToDt.Text);
                if (pMode == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Item_Stock" + txtToDt.Text.Replace("/", "_"));
                else if (pMode == "Excel")
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Item_Stock" + txtToDt.Text.Replace("/", "_") + ".xls");

                rptDoc.Dispose();
                Response.ClearHeaders();
                Response.ClearContent();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("PDF");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
        }


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
            dt = oCG.PopComboMIS("N", "Y", "ItemCode", "ItemID", "ItemName", "Inv_ItemMst", "", "", "AA", vLogDt, "0000");

            chkBrDtl.DataSource = dt;
            chkBrDtl.DataTextField = "Name";
            chkBrDtl.DataValueField = "ItemID";
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

        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBrAll();
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
    }
}