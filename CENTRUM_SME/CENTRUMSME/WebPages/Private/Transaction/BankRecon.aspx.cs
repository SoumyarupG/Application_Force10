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
using CENTRUMCA;
using CENTRUMBA;
using System.IO;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class BankRecon : CENTRUMBAse
    {
        protected int currentPageNumber = 1;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                txtFrmDt.Text = Session[gblValue.FinFromDt].ToString();
                txtToDt.Text = Session[gblValue.FinToDt].ToString();
                PopBank();
                LoadGrid(1);
                btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());

            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.PageHeading = "Bank Reconciliation";
                this.ShowBranchName = "Branch :: " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = "Financial Year :: " + Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuBankRecon);

                //if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnSave.Visible = false;
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                    btnSave.Visible = true;
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Bank Reconciliation", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopBank()
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            CVoucher oVou = new CVoucher();
            dt = oVou.GetBank(vBrCode);
            ddlBank.DataTextField = "Desc";
            ddlBank.DataValueField = "DescID";
            ddlBank.DataSource = dt;
            ddlBank.DataBind();
            ListItem oItem = new ListItem();
            oItem.Text = "<--- Select --->";
            oItem.Value = "-1";
            ddlBank.Items.Insert(0, oItem);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void rdbOpt_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (rdbRecon.SelectedValue == "N")
        //        lbl.Text = "Voucher Date From :";
        //    else if (rdbRecon.SelectedValue == "Y")
        //        lblCap.Text = "Reconciliated Date From :";
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlBank.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Please Select the Bank Name....");
                    return;
                }
                LoadGrid(1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateDate()
        {
            Boolean vResult = true;
            if (txtFrmDt.Text.Trim() == "")
            {
                gblFuction.MsgPopup("From Date Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtFromDt");
                vResult = false;
                return vResult;
            }
            if (txtFrmDt.Text.Trim() != "")
            {
                if (gblFuction.IsDate(txtFrmDt.Text) == false)
                {
                    gblFuction.MsgPopup("Please Enter Valid Date...");
                    gblFuction.focus("ctl00_cph_Main_txtFromDt");
                    vResult = false;
                    return vResult;
                }
            }
            if (txtToDt.Text.Trim() == "")
            {
                gblFuction.MsgPopup("To Date Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtToDt");
                vResult = false;
                return vResult;
            }
            if (txtToDt.Text.Trim() != "")
            {
                if (gblFuction.IsDate(txtToDt.Text) == false)
                {
                    gblFuction.MsgPopup("Please Enter Valid Date...");
                    gblFuction.focus("ctl00_cph_Main_txtToDt");
                    vResult = false;
                    return vResult;
                }
            }
            return vResult;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadGrid(Int32 pPgIndx)
        {
            if (ValidateDate() == true)
            {
                string vFlag = "";
                DataTable dt = null;
                Int32 totalRows = 0;
                string pDescID = ddlBank.SelectedValue;
                DateTime vFromDate = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDate = gblFuction.setDate(txtToDt.Text);
                if (rdbRecon.SelectedValue == "N")
                    vFlag = "N";
                else if (rdbRecon.SelectedValue == "R")
                    vFlag = "R";
                CBankRecon oRecon = new CBankRecon();
                dt = oRecon.GetBnkReconListPG(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(),
                     pDescID, vFromDate, vToDate, vFlag, Session[gblValue.BrnchCode].ToString(), pPgIndx, ref totalRows);

                dt.PrimaryKey = new DataColumn[] { dt.Columns["HeadID"] };        
                gvBankRecon.DataSource = dt;
                ViewState["ReconList"] = dt;
                gvBankRecon.DataBind();
                lblTotalPages.Text = CalculateTotalPages(totalRows).ToString();
                lblCurrentPage.Text = currentPageNumber.ToString();
                if (currentPageNumber == 1)
                {
                    Btn_Previous.Enabled = false;
                    if (Int32.Parse(lblTotalPages.Text) > 0)
                        Btn_Next.Enabled = true;
                    else
                        Btn_Next.Enabled = false;
                }
                else
                {
                    Btn_Previous.Enabled = true;
                    if (currentPageNumber == Int32.Parse(lblTotalPages.Text))
                        Btn_Next.Enabled = false;
                    else
                        Btn_Next.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalRows"></param>
        /// <returns></returns>
        private int CalculateTotalPages(double totalRows)
        {
            int totalPages = (int)Math.Ceiling(totalRows / gblValue.PgSize1);
            return totalPages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    currentPageNumber = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;

                case "Next":
                    currentPageNumber = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }
            LoadGrid(currentPageNumber);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCDt_TextChanged(object sender, EventArgs e)
        {
            Int32 vRow = 0;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtClearDt = (TextBox)gvRow.FindControl("txtCDt");
            DataTable dt = (DataTable)ViewState["ReconList"];
	    if (dt.Rows.Count > 0)
	    {
		    if (txtClearDt.Text != "")
		    {
			    if (gblFuction.setDate(gvRow.Cells[3].Text) > gblFuction.setDate(txtClearDt.Text))
			    {
				    gblFuction.MsgPopup("Cheeque clear date can not before voucher date...");
				    txtClearDt.Text = "";
				    return;
			    }
			    else
			    {
				    vRow = dt.Rows.IndexOf(dt.Rows[0]);  //dt.Rows.IndexOf(dt.Rows.Find(Convert.ToInt32(gvRow.Cells[9].Text)));
				    dt.Rows[vRow]["ClearDt"] = txtClearDt.Text;
				    dt.AcceptChanges();
			    }
		    }
	    }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string DataTableTOXml(DataTable dt)
        {
            string sXml = "";
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                sXml = oSW.ToString();
            }
            return sXml;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            string vXmlBRS = string.Empty;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Boolean vResult = false;
            DataTable dt = (DataTable)ViewState["ReconList"];
            CBankRecon oRecon = null;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0;
            string vAcHdTbl = Session[gblValue.ACVouMst].ToString();
            try
            {
                if (gvBankRecon.Rows.Count == 0)
                {
                    gblFuction.MsgPopup("No Records to Save.");
                    return true;
                }
                if (ValidateFields() == 1)
                {
                    gblFuction.MsgPopup("Please enter atleast one Clear Date.");
                    return false;
                }

                if (Mode == "Save" || Mode == "Edit")
                {
		     if (rdbRecon.SelectedValue.ToString() == "N")
		     {
			if (dt.Rows.Count > 0)
			{
				for (int r = 0; gvBankRecon.Rows.Count > r; r++)
				{
					TextBox txtCDt = (TextBox)gvBankRecon.Rows[r].FindControl("txtCDt");
					if (txtCDt.Text.Trim().Length > 0)
					{
						dt.Rows[r]["ClearDt"] = gblFuction.setDate(txtCDt.Text);
					}
					else
					{
						dt.Rows[r].Delete();
					}
				}
			}
		    }

		    if (rdbRecon.SelectedValue.ToString() == "R")
		    {
			if (dt.Rows.Count > 0)
			{
				for (int r = 0; gvBankRecon.Rows.Count > r; r++)
				{
					TextBox txtCDt = (TextBox)gvBankRecon.Rows[r].FindControl("txtCDt");
					if (txtCDt.Text.Trim().Length > 0)
					{
						dt.Rows[r]["ClearDt"] = gblFuction.setDate(txtCDt.Text);
					}
					else
					{
						dt.Rows[r]["ClearDt"] = DBNull.Value;
					}
				}
			}
		    }
		    
                    dt.AcceptChanges();
                    vXmlBRS = DataTableTOXml(dt);
                    oRecon = new CBankRecon();
                    vErr = oRecon.UpdateBankRecon(vXmlBRS,vBrCode, this.UserID,vAcHdTbl, "E");

                    if (vErr == 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Int32 ValidateFields()
        {
            Int32 vRow = 0, vFnd = 0, vTotRec = 0;
            vTotRec = gvBankRecon.Rows.Count;
            for (vRow = 0; vRow < gvBankRecon.Rows.Count; vRow++)
            {
                TextBox txtCDt = (TextBox)gvBankRecon.Rows[vRow].FindControl("txtCDt");
                if (txtCDt.Text == "")
                    vFnd += 1;
                else
                    vFnd = 0;
            }
            if (vTotRec == vFnd)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRecon_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["ReconList"];
                gvBankRecon.PageIndex = e.NewPageIndex;
                gvBankRecon.DataSource = dt;
                gvBankRecon.DataBind();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                if (vStateEdit == "" || vStateEdit == null)
                    vStateEdit = "Save";
                if (SaveRecords(vStateEdit) == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    LoadGrid(1);
                }
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
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\BanReCon.rpt";
            string vBranch = Session[gblValue.BrName].ToString();
            DataTable dt = null;
            CReports oRpt = null;
            CBankRecon oRecon = null;
            string vFlag = "";
            string pDescID = ddlBank.SelectedValue;
            DateTime vFromDate = gblFuction.setDate(txtFrmDt.Text);
            DateTime vToDate = gblFuction.setDate(txtToDt.Text);

            try
            {
                if (rdbRecon.SelectedValue == "N")
                    vFlag = "N";
                else if (rdbRecon.SelectedValue == "R")
                    vFlag = "R";
                oRecon = new CBankRecon();
                dt = oRecon.GetBnkReconList(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(),
                     pDescID, vFromDate, vToDate, vFlag, Session[gblValue.BrnchCode].ToString());
                if (dt == null)
                {
                    gblFuction.MsgPopup("No Records Found. Try Again.");
                    return;
                }
                else
                {
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        oRpt = new CReports();
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt);
                        rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                        rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()));
                        rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(Session[gblValue.BrnchCode].ToString()));
                        rptDoc.SetParameterValue("pBranch", vBranch);
                        rptDoc.SetParameterValue("pTitle", "Bank Reconciliation Statement");
                        rptDoc.SetParameterValue("DtFrom", txtFrmDt.Text);
                        rptDoc.SetParameterValue("DtTo", txtToDt.Text);
                        rptDoc.SetParameterValue("pBankNm", ddlBank.SelectedItem.Text);
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_BRS");
                        Response.ClearHeaders();
                        Response.ClearContent();
                    }
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
                oRecon = null;
            }
        }
    }
}